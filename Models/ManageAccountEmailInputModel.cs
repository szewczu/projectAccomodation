using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel
    {
        public class ManageAccountEmailInputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Nowy email")]
            public string NewEmail { get; set; }
        }
    }
}
