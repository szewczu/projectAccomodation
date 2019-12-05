using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Noclegi.Areas.Announcement.Pages
{
    public class CreateAnnouncementModel : PageModel
    {

        
        [BindProperty]
        public InputModel Input { get; set; }



        public class InputModel
        {

            [Display(Name = "Tytul")]
            public string Title { get; set; }

            [Display(Name = "Opis")]
            public string Description { get; set; }

            [Display(Name = "Data Rozpoczecia")]
            public DateTime StartDate { get; set; }

            [Display(Name = "Data Zakonczenia")]
            public DateTime EndDate { get; set; }

            [Display(Name = "Cena")]
            public int Pirce { get; set; }

            [Display(Name = "Typ Ogloszenia")]
            public string TypeOfAdvertisement { get; set; }

            [Display(Name = "Property Type")]
            public string PropertyType { get; set; }

            [Display(Name = "Pietro")]
            public int Floor { get; set; }

            [Display(Name = "Pokoje")]
            public int Rooms { get; set; }

            [Display(Name = "Ulica")]
            public string Street { get; set; }

            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Display(Name = "Kod Pocztowy")]
            public string Postcode { get; set; }

            [Display(Name = "Prowincja")]
            public string Province { get; set; }

            [Display(Name = "Kraj")]
            public string Country { get; set; }
        }
    }
}

