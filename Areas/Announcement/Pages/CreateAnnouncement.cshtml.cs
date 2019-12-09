using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Helpers;

namespace Noclegi.Areas.Announcement.Pages
{

    public class CreateAnnouncementModel : PageModel
    {

        public CreateAnnouncementModel()
        {  }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Tytul")]
            public string Title { get; set; }

            [Display(Name = "Opis")]
            public string Description { get; set; }

            [Required]
            [Display(Name = "Data Rozpoczecia")]
            public DateTime StartDate { get; set; }

            [Required]
            [Display(Name = "Data Zakonczenia")]
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
        }

        private bool IsAddressNotEmpty()
        {
            string[] address = { Input.Street, Input.City, Input.PostCode, Input.Province, Input.Country };
            foreach (var item in address)
            {
                var check = string.IsNullOrEmpty(item);
                if (check)
                    return false;
            }
            return true;

        }
        public IActionResult OnPost()
        {
            string typeOfAdvertisement = Input.TypeOfAdvertisement;
            if (typeOfAdvertisement != "LookingFor" && IsAddressNotEmpty())
            {
                string userId = GetCurrentUserIdByUserName();
                int announcementID = CreateNewAnnouncement(userId);
                CreateNewAnnoucementAddress(announcementID);
                return RedirectToPage("Index");
            }
            else if (typeOfAdvertisement == "LookingFor")
            {
                string userId = GetCurrentUserIdByUserName();
                CreateNewAnnouncement(userId);
                return RedirectToPage("Index");
            }
            //should show error: address is required when selected Rent or Exchange
            return Page();
        }

        private void CreateNewAnnoucementAddress(object announcementID)
        {
            SqlConnection connection = DatabaseFunctions.CreateSqlConnection();
            connection.Open();

            var street = Input.Street;
            var city = Input.City;
            var postCode = Input.PostCode;
            var province = Input.Province;
            var country = Input.Country;

            string sqlSetQuery = $"INSERT INTO AspNetAdress " +
              $"(AdvertisementID, Street, City, Postcode,Province,Country) " +
              $"VALUES ('{announcementID}','{street}','{city}','{postCode}', '{province}', '{country}');";

            SqlCommand command = new SqlCommand(sqlSetQuery, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private int CreateNewAnnouncement(object userId) // test.test@test.test id "476a5306-6060-4ab2-ae1b-890de13527e0"
        {
            SqlConnection connection = DatabaseFunctions.CreateSqlConnection();
            connection.Open();

            var title = Input.Title;
            var description = Input.Description;
            var startDate = Input.StartDate.ToString("yyyy-MM-dd");
            var endDate = Input.EndDate.ToString("yyyy-MM-dd");
            var price = Input.Price;
            var typeOfAdvertisement = Input.TypeOfAdvertisement;
            var propertyType = Input.PropertyType;
            var floor = Input.Floor;
            var rooms = Input.Rooms;

            string sqlSetQuery = $"INSERT INTO AspNetAdvertisement " +
              $"(UserId, Title, Description, StartDate,EndDate,Price, {typeOfAdvertisement}, PropertyType,Floor,Rooms) " +
              $"OUTPUT INSERTED.ID " +
              $"VALUES ('{userId}','{title}','{description}','{startDate}', '{endDate}', '{price}','true','{propertyType}','{floor}','{rooms}');"; //  SELECT SCOPE_IDENTITY(),               $"output inserted.Id" +
            SqlCommand command = new SqlCommand(sqlSetQuery, connection);
            int insertedID = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();
            connection.Close();
            return insertedID;


        }

        private string GetCurrentUserIdByUserName()
        {
            SqlConnection connection = DatabaseFunctions.CreateSqlConnection();
            connection.Open();
            var userName = HttpContext.User.Identity.Name;
            string sql = $"Select Id " +
                $"from AspNetUsers " +
                $"where UserName = \'{userName}\'";
            string userId = DatabaseFunctions.GetFirstValueOfTheFirstColumnFromSqlCommand(sql, connection).ToString(); // to mo¿e byæ stringiem 
            connection.Close();

            return userId;
        }

    }
}


