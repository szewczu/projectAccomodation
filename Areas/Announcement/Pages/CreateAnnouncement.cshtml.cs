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
            public string Tytul { get; set; }

            [Display(Name = "Opis")]
            public string Opis { get; set; }

            [Display(Name = "Data Rozpoczecia")]
            public DateTime DataRozpoczecia { get; set; }

            [Display(Name = "Data Zakonczenia")]
            public DateTime DataZakonczenia { get; set; }

            [Display(Name = "Cena")]
            public int Cena { get; set; }

            [Display(Name = "Typ Ogloszenia")]
            public string TypOgloszenia { get; set; }

            [Display(Name = "Property Type")]
            public string PropertyType { get; set; }

            [Display(Name = "Pietro")]
            public int Pietro { get; set; }

            [Display(Name = "Pokoje")]
            public int Pokoje { get; set; }

            [Display(Name = "Ulica")]
            public string Ulica { get; set; }

            [Display(Name = "Miasto")]
            public string Miasto { get; set; }

            [Display(Name = "Kod Pocztowy")]
            public string KodPocztowy { get; set; }

            [Display(Name = "Prowincja")]
            public string Prowincja { get; set; }

            [Display(Name = "Kraj")]
            public string Kraj { get; set; }
        }
    }
}

