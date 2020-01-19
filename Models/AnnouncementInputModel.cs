using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Noclegi.Areas.Announcement.Pages
{

    public partial class CreateAnnouncementModel
    {
        public class AnnouncementInputModel
        {
            [Required(ErrorMessage = "Wprowadź tytuł ogłoszenia")]
            [Display(Name = "Tytul")]
            public string Title { get; set; }

            [Display(Name = "Opis")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Wprowadź datę rozpoczęcia")]
            [DataType(DataType.Date)]
            [Display(Name = "Data Rozpoczecia")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "Wprowadź datę zakończenia")]
            [Display(Name = "Data Zakonczenia")]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Wprowadź cenę")]
            [Display(Name = "Cena")]
            public int Price { get; set; }

            [Required(ErrorMessage = "Wprowadź typ ogłoszenia")]
            [Display(Name = "Typ Ogloszenia")]
            public string TypeOfAdvertisement { get; set; }

            [Required(ErrorMessage = "Wprowadź rodzaj ogłoszenia")]
            [Display(Name = "Property Type")]
            public string PropertyType { get; set; }

            [Required(ErrorMessage = "Wprowadź numer piętra")]
            [Display(Name = "Pietro")]
            public int Floor { get; set; }

            [Required(ErrorMessage = "Wprowadź liczbę pokoi")]
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

            
            public IFormFile Picture1Bin { get; set; }
            public IFormFile Picture2Bin { get; set; }
            public IFormFile Picture3Bin { get; set; }
            public IFormFile Picture4Bin { get; set; }
            public IFormFile Picture5Bin { get; set; }
            public IFormFile Picture6Bin { get; set; }
        }


    }
}


