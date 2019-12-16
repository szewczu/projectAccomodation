using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Helpers;

namespace Noclegi.Areas.Announcement.Pages
{

    public partial class CreateAnnouncementModel : PageModel
    {

        public CreateAnnouncementModel()
        {  }

        [BindProperty]
        public AnnouncementInputModel Input { get; set; }

        [BindProperty]
        public AnnouncementInputModel ExchangeInput { get; set; }

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
            if(Input.StartDate > Input.EndDate)
            {
                //error message: error: start date should be before end date
                return Page();
            }

            if(Input.StartDate < DateTime.Now)
            {
                //error message: error: start date should be before end date
                return Page();
            }
            string typeOfAdvertisement = Input.TypeOfAdvertisement;
            if (typeOfAdvertisement != "LookingFor" && IsAddressNotEmpty())
            {
                string userId = GetCurrentUserIdByUserName();
                int announcementID = CreateNewAnnouncement(userId, Input);
                CreateNewAnnoucementAddress(announcementID, Input);
                if(Input.TypeOfAdvertisement == "Exchange")
                {
                    int announcementExchangeID = CreateNewAnnouncement(userId, ExchangeInput, announcementID);
                    CreateNewAnnoucementAddress(announcementExchangeID, ExchangeInput);
                }
                return RedirectToPage("Index");
            }
            else if (typeOfAdvertisement == "LookingFor")
            {
                string userId = GetCurrentUserIdByUserName();
                CreateNewAnnouncement(userId, Input);
                return RedirectToPage("Index");
            }
            //should show error: address is required when selected Rent or Exchange

             string message = "error: address is required when selected Rent or Exchange";
            return Page();
        }



        private void CreateNewAnnoucementAddress(object announcementID, AnnouncementInputModel Input)
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

        private int CreateNewAnnouncement(object userId, AnnouncementInputModel Input, int exchangeId = 0) // test.test@test.test id "476a5306-6060-4ab2-ae1b-890de13527e0"
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
            object exchangeAdId = CheckIfAnnouncementIdWasGiven(exchangeId);
            string sqlSetQuery = $"INSERT INTO AspNetAdvertisement " +
              $"(UserId, Title, Description, StartDate,EndDate,Price, {typeOfAdvertisement}, PropertyType,Floor,Rooms, ExchangeAdId) " +
              $"OUTPUT INSERTED.ID " +
              $"VALUES ('{userId}','{title}','{description}','{startDate}', '{endDate}', '{price}','true','{propertyType}','{floor}','{rooms}','{exchangeAdId.GetType()}');"; //  SELECT SCOPE_IDENTITY(),               $"output inserted.Id" +
            SqlCommand command = new SqlCommand(sqlSetQuery, connection);
            int insertedID = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();
            connection.Close();
            return insertedID;
        }

        private static object CheckIfAnnouncementIdWasGiven(int exchangeId)
        {
            return exchangeId != 0 ? exchangeId : (object)null;
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


