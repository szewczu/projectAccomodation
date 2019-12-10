using System;
using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel
    {
        public class ManageAccountIndexInputModel
        {
            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Nazwa użytkownika")]
            public string Username { get; set; }

            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Display(Name = "Płeć")]
            public string Gender { get; set; }

            [Display(Name = "Data urodzenia")]
            public DateTime DateOfBirth { get; set; }

        }

    }
}
