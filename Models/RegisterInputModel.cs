using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Identity.Pages.Account
{
    public partial class RegisterModel
    {
        public class RegisterInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Hasło musi zaweirać od {2} do maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła nie są takie same")]
            public string ConfirmPassword { get; set; }
        }
    }
}
