using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;
using Noclegi.Data;
using Noclegi.Helpers;
using System.Threading;

namespace Noclegi.Controllers
{
    public class AdminPanelAdvertisement : Controller
    {
        static string GlobalId= " pusty global id";
        static string GlobalUserName = " pusty global username";
        static string GlobalTitle = " pusty global title";
        static string GlobalDescription = " pusty global Description";
        static string GlobalStartDate = " pusty global StartDate";
        static string GlobalEndDate = " pusty global EndDate";
        static string GlobalPrice = " pusty global Price";
        static string GlobalRent = " pusty global Rent";
        static string GlobalLookingFor = " pusty global LookingFor";
        static string GlobalExchange = " pusty global Exchange";
        static string GlobalExchangeAdId = " pusty global ExchangeAdId";
        static string GlobalCreateDate = " pusty global CreateDate";
        static string GlobalPropertyType = " pusty global PropertyType";
        static string GlobalFloor = " pusty global Floor";
        static string GlobalRooms = " pusty global Rooms";
        static string GlobalCity = " pusty global City";
        static string GlobalCountry = " pusty global Country";
        static string GlobalPostcode = " pusty global Postcode";
        static string GlobalProvince = " pusty global Province";
        static string GlobalStreet = " pusty global Street";
        private readonly  ApplicationDbContext _context;

        public AdminPanelAdvertisement(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ShowEditAdvertisement()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            string Id = GlobalId;
            string UserName = "", Title = "", Description = "", StartDate = "", EndDate = "", Price = "", Rent = "", LookingFor = "";
            string CreateDate = "", PropertyType = "", Floor = "", Rooms = "", City = "", Country = "", Exchange = "", ExchangeAdId = "", Postcode = "", Province = "", Street = "";
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("select us.UserName " +
                    ", ad.Title, ISNULL(ad.Description,' '),  ISNULL(FORMAT(ad.StartDate, 'yyyy-MM-dd'),' '), " +
                    "ISNULL(FORMAT(ad.EndDate, 'yyyy-MM-dd'), ' '), CAST(ISNULL(ad.Price, 0) as nvarchar(30)), CAST(ad.Rent as nvarchar(30)), CAST(ad.LookingFor as nvarchar(30)), CAST(ad.Exchange as nvarchar(30)), CAST(ISNULL(ad.ExchangeAdId, ' ') as nvarchar(30)), " +
                    "ISNULL(FORMAT(ad.CreateDate, 'yyyy-MM-dd'), ' '), ad.PropertyType, CAST(ISNULL(ad.Floor, 0) as nvarchar(30)), CAST(ad.Rooms as nvarchar(30)), ISNULL(ads.City, ' '), ISNULL(ads.Country, ' '), " +
                    "ISNULL(ads.Postcode, ' '), ISNULL(ads.Province, ' '), ISNULL(ads.Street, ' ') " +
                    "from AspNetAdvertisement ad " +
                    "left join AspNetUsers us on ad.UserId = us.Id " +
                    "left join AspNetAdress ads on ads.AdvertisementId = ad.Id " +
                    "WHERE ad.Id='" + Id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            UserName = sdr.GetString(0);
                            Title = sdr.GetString(1);
                            Description = sdr.GetString(2);
                            StartDate = sdr.GetString(3);
                            EndDate = sdr.GetString(4); 
                            Price = sdr.GetString(5);
                            Rent = sdr.GetString(6);
                            LookingFor = sdr.GetString(7);
                            Exchange = sdr.GetString(8);
                            ExchangeAdId = sdr.GetString(9);
                            CreateDate = sdr.GetString(10);
                            PropertyType = sdr.GetString(11);
                            Floor = sdr.GetString(12);
                            Rooms = sdr.GetString(13);
                            City = sdr.GetString(14);
                            Country = sdr.GetString(15);
                            Postcode = sdr.GetString(16);
                            Province = sdr.GetString(17);
                            Street = sdr.GetString(18);
                        }
                    }
                    con.Close();
                }
            }
            GlobalId = Id;
            GlobalUserName = UserName;
            GlobalTitle = Title;
            GlobalDescription = Description;
            GlobalStartDate = StartDate;
            GlobalEndDate = EndDate;
            GlobalPrice = Price;
            GlobalRent = Rent;
            GlobalLookingFor = LookingFor;
            GlobalExchange = Exchange;
            GlobalExchangeAdId = ExchangeAdId;
            GlobalCreateDate = CreateDate;
            GlobalPropertyType = PropertyType;
            GlobalFloor = Floor;
            GlobalRooms = Rooms;
            GlobalCity = City;
            GlobalCountry = Country;
            GlobalPostcode = Postcode;
            GlobalProvince = Province;
            GlobalStreet = Street;

            TempData["Id"] = GlobalId;
            TempData["UserName"] = GlobalUserName;
            TempData["Title"] = GlobalTitle;
            TempData["Description"] = GlobalDescription;
            TempData["StartDate"] = GlobalStartDate;
            TempData["EndDate"] = GlobalEndDate;
            TempData["Price"] = GlobalPrice;
            TempData["Rent"] = GlobalRent;
            TempData["LookingFor"] = GlobalLookingFor;
            TempData["Exchange"] = GlobalExchange;
            TempData["ExchangeAdId"] = GlobalExchangeAdId;
            TempData["CreateDate"] = GlobalCreateDate;
            TempData["PropertyType"] = GlobalPropertyType;
            TempData["Floor"] = GlobalFloor;
            TempData["Rooms"] = GlobalRooms;
            TempData["City"] = GlobalCity;
            TempData["Country"] = GlobalCountry;
            TempData["Postcode"] = GlobalPostcode;
            TempData["Province"] = GlobalProvince;
            TempData["Street"] = GlobalStreet; 
            return PartialView();
        }

        public void EditAdvertisement(string Id)
        {
            GlobalId = Id;
        }

        public void EditAdvertisementButton(string Id, string inputUserName, string inputTitle, string inputDescription, string inputStartDate, string inputEndDate, string inputPrice, string inputRent, string inputLookingFor, string inputExchange, string inputExchangeAdId, string inputCreateDate, string inputPropertyType, string inputFloor, string inputRooms, string inputCity, string inputCountry, string inputPostcode, string inputProvince, string inputStreet)
        {   
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE AspNetAdvertisement SET Title='" + inputTitle + "', Description ='" + inputDescription + 
                    "', StartDate='" + inputStartDate + "', EndDate='" + inputEndDate + "', Price='" + inputPrice + "', CreateDate='" + inputCreateDate + 
                    "', PropertyType='" + inputPropertyType + "', Floor='" + inputFloor + "', Rooms='" + inputRooms + "'  WHERE Id='" + Id + 
                    "'; UPDATE AspNetAdress SET City='" + inputCity + "', Country='" + inputCountry + "', Postcode='" + inputPostcode + "', Province='" + 
                    inputProvince + "', Street='" + inputStreet + "' WHERE AdvertisementId='" + Id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    con.Close();
                }
            }
        }
        public void DeleteAdvertisement(string Id)
        {
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("EXEC DELETE_ADVERTISEMENT_P @ADVERTISEMENT_ID='" + Id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    con.Close();
                }
            }
        }
    }
}