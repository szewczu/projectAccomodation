using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class DeletePersonalDataModel
    {
        public class ManageAccountDeletePersonalDataInputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
