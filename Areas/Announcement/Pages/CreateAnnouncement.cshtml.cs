using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Helpers;
using Noclegi.Models;

namespace Noclegi.Areas.Announcement.Pages
{

    public partial class CreateAnnouncementModel : PageModel
    {
        [BindProperty]
      public  AnnouncementInputModel Input { get; set; }

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
            if (Input.StartDate > Input.EndDate)
            {
                //error message: error: start date should be before end date
                return Page();
            }

            if (Input.StartDate < DateTime.Now)
            {
                //error message: error: start date should be before end date
                return Page();
            }
            string typeOfAdvertisement = Input.TypeOfAdvertisement;
            string userId = GetCurrentUserIdByUserName();

            if (typeOfAdvertisement != "LookingFor" && IsAddressNotEmpty())
            {
                int announcementID = CreateNewAnnouncement(userId, Input);
                CreateNewAnnoucementAddress(announcementID, Input);
                UploadImages(announcementID, Input);

                if (Input.TypeOfAdvertisement == "Exchange")
                {
                    int announcementExchangeID = CreateNewAnnouncement(userId, ExchangeInput, announcementID);
                    CreateNewAnnoucementAddress(announcementExchangeID, ExchangeInput);
                }

                return RedirectToPage("Index");
            }
            else if (typeOfAdvertisement == "LookingFor")
            {
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

            string sqlSetQuery = null;
            if (exchangeId != 0)
            {
                sqlSetQuery = $"INSERT INTO AspNetAdvertisement " +
                 $"(UserId, Title, Description, StartDate,EndDate,Price, {typeOfAdvertisement}, PropertyType,Floor,Rooms, ExchangeAdId) " +
                 $"OUTPUT INSERTED.ID " +
                 $"VALUES ('{userId}','{title}','{description}','{startDate}', '{endDate}', '{price}','true','{propertyType}','{floor}','{rooms}','{exchangeId}');";
            }
            else
            {
                sqlSetQuery = $"INSERT INTO AspNetAdvertisement " +
               $"(UserId, Title, Description, StartDate,EndDate,Price, {typeOfAdvertisement}, PropertyType,Floor,Rooms) " +
               $"OUTPUT INSERTED.ID " +
                $"VALUES ('{userId}','{title}','{description}','{startDate}', '{endDate}', '{price}','true','{propertyType}','{floor}','{rooms}');";

            }
            SqlCommand command = new SqlCommand(sqlSetQuery, connection);
            int insertedID = Convert.ToInt32(command.ExecuteScalar());
            // command.ExecuteNonQuery();
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


        private void UploadImages(int announcementId, AnnouncementInputModel Input)
        {
            var adverteismentId = announcementId;
            var image1 = Input.Picture1Bin;
            var image2 = Input.Picture2Bin;
            var image3 = Input.Picture3Bin;
            var image4 = Input.Picture4Bin;
            var image5 = Input.Picture5Bin;
            var image6 = Input.Picture6Bin;

            SqlConnection connection = DatabaseFunctions.CreateSqlConnection();
            connection.Open();
            string sqlSetQuery = $"INSERT INTO AspNetPicture " +
                 $"(AdvertisementId, Picture1Base64,Picture2Base64,Picture3Base64,Picture4Base64,Picture5Base64,Picture6Base64) " +
                 $"VALUES (@adverteismentId,@image1,@image2,@image3,@image4,@image5,@image6);";

            SqlCommand command = new SqlCommand(sqlSetQuery, connection);
            command.Parameters.AddWithValue("@adverteismentId", adverteismentId);
            //add parameter
            AddParameter("image1", image1, command);
            AddParameter("image2", image2, command);
            AddParameter("image3", image3, command);
            AddParameter("image4", image4, command);
            AddParameter("image5", image5, command);
            AddParameter("image6", image6, command);

            command.ExecuteNonQuery();

            connection.Close();
        }

        private static void AddParameter(string parameterName, IFormFile image1, SqlCommand command)
        {
            command.Parameters.Add($"@{parameterName}", SqlDbType.VarChar, -1);

            if (ConvertImageToByteArray(image1) != null)
                command.Parameters[$"@{parameterName}"].Value = ConvertImageToByteArray(image1);
            else
                command.Parameters[$"@{parameterName}"].Value = DBNull.Value;
        }

        private static string ConvertImageToByteArray(IFormFile image)
        {
            string imageBase64;
            if (image == null)
            {
                return null;
            }
            using (BinaryReader br = new BinaryReader(image.OpenReadStream()))
            {
                byte[] imageByteArray = br.ReadBytes((int)image.OpenReadStream().Length);
                imageBase64 = Convert.ToBase64String(imageByteArray);
                // Convert the image in to bytes
            }

            return imageBase64;
        }
    }
}
