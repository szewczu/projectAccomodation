using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class ManageAccount : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ManageAccount(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender, ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusMessagePassword { get; set; }
        [TempData]
        public string StatusMessageEmail { get; set; }

        [BindProperty]
        public InputModel InputInformation { get; set; }

        [BindProperty]
        public InputModel InputEmail { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Nowy email")]
            public string NewEmail { get; set; }

            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można pobrać użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można pobrać użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (InputEmail.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, InputEmail.NewEmail);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = InputEmail.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    InputEmail.NewEmail,
                    "Potwierdź swój adres email",
                    $"Proszę potwierdź swoje konto <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj.</a>.");

                StatusMessage = "Link z potwierdzeniem zmiany emaila został wysłany. Proszę sprawdź swoją pocztę.";
                return RedirectToPage();
            }

            StatusMessage = "Twój email nie został zmieniony";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można pobrać użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Potwierdź swój email",
                $"Proszę potwierdź swoje konto <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");

            StatusMessage = "Mail weryfikujący został wysłany. Proszę sprwadź swoją pocztę";
            return RedirectToPage();
        }



        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            InputInformation = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            InputEmail = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnPostAsync()
        {


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można pobrać użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (InputInformation.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, InputInformation.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Wystąpił niespodziewany błąd podczas ustawiania numeru telefonu dla użytkownika o ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został zaktulaizowany";
            return RedirectToPage();
        }

 
    }
}
