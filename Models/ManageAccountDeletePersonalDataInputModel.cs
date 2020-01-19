using System.ComponentModel.DataAnnotations;

namespace Noclegi.Models
{
    public class ManageAccountDeletePersonalDataInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
