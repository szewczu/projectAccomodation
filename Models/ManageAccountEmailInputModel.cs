using System.ComponentModel.DataAnnotations;

namespace Noclegi.Models
{
    public class ManageAccountEmailInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Nowy email")]
        public string NewEmail { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

    }
}
