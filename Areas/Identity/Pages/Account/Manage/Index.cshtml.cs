using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Helpers;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ManageAccountIndexInputModel Input { get; set; }

        public IActionResult OnPost(IdentityUser user)
        {
            var cnn = DatabaseFunctions.CreateSqlConnection();
            cnn.Open();
            var name = Input.Name;
            var surname = Input.Surname;
            var dateOfBirth = Input.DateOfBirth;
            var phoneNumber = Input.PhoneNumber;
            var userName = HttpContext.User.Identity.Name;
            var gender = "Other";
            if (string.IsNullOrEmpty(Input.Gender) || Input.Gender != "Male" && Input.Gender != "Female")
            {
                gender = "Other";
            }
            else
            {
                gender = Input.Gender;
            }

            string dateOfBirthString = dateOfBirth.ToString("yyyy-MM-dd");

            string sql = $"Update AspNetUsers " +
                $"set  PhoneNumber = \'{phoneNumber}\' ," +
                $"Name = \'{name}\'," +
                $"DateOfBirth= \'{dateOfBirthString}\'," +
                $"Gender= \'{gender}\'," +
                $"Surname= \'{surname}\'" +
                $"where UserName = \'{userName}\';";
            SqlCommand command = new SqlCommand(sql, cnn);
            command.ExecuteNonQuery();
            cnn.Close();
            return Page();
        }

        public IActionResult OnGet(IdentityUser user)
        {
            var connection = DatabaseFunctions.CreateSqlConnection();
            connection.Open();
            var userName = HttpContext.User.Identity.Name;
            // Maybe it should be single query to database and place data to object to retrive data later
            var name = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select Name from AspNetUsers where UserName = \'{userName}\'", connection);
            var surname = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select Surname from AspNetUsers where UserName = \'{userName}\'", connection);
            var phoneNumber = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select PhoneNumber from AspNetUsers where UserName = \'{userName}\'", connection);
            var id = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select Id from AspNetUsers where UserName = \'{userName}\'", connection);
            var gender = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select Gender from AspNetUsers where UserName = \'{userName}\'", connection);
            var dateOfBirth = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand($"Select DateOfBirth from AspNetUsers where UserName = \'{userName}\'", connection);
            if (dateOfBirth == null)
            {
                dateOfBirth = DateTime.MinValue;
            }
            Input = new ManageAccountIndexInputModel
            {
                PhoneNumber = (string)phoneNumber,
                Username = userName,
                Surname = (string)surname,
                Name = (string)name,
                Gender = (string)gender,
                DateOfBirth = (DateTime)dateOfBirth

            };
            return Page();
        }

    }
}
