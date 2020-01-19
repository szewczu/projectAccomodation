using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Identity.Pages.Account
{
    public partial class LoginModel
    {
        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Zapamiętaj mnie?")]
            public bool RememberMe { get; set; }
        }
    }
}
