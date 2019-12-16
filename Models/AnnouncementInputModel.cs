using System;
using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Announcement.Pages
{

    public partial class CreateAnnouncementModel
    {
        public class AnnouncementInputModel
        {
            [Required]
            [Display(Name = "Tytul")]
            public string Title { get; set; }

            [Display(Name = "Opis")]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Data Rozpoczecia")]
            public DateTime StartDate { get; set; }

            [Required]
            [Display(Name = "Data Zakonczenia")]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [Display(Name = "Cena")]
            public int Price { get; set; }

            [Required]
            [Display(Name = "Typ Ogloszenia")]
            public string TypeOfAdvertisement { get; set; }

            [Required]
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
            public string PostCode { get; set; }

            [Display(Name = "Prowincja")]
            public string Province { get; set; }

            [Display(Name = "Kraj")]
            public string Country { get; set; }

            public string ExchangeAdId { get; set; }
        }


    }
}


