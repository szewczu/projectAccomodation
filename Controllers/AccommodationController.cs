using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Noclegi.Data;
using Noclegi.Helpers;
using System.Threading;

namespace Noclegi.Controllers
{
    public class AccommodationController : Controller
    {
        static string GlobalId = " pusty global id";
        static string GlobalUserName = " pusty global username";
        static string GlobalTitle = " pusty global Title";
        static string GlobalDescription = " pusty global Description ";
        static string GlobalCreateDate = " pusty global CreateDate";
        static string GlobalStartDate = " pusty global CreateDate";
        static string GlobalEndDate = " pusty global EndDate";
        static decimal GlobalPrice = 0;
        static string GlobalPropertyType = " pusty global PropertyType";
        static Int32 GlobalFloor = 0;
        static Int32 GlobalRooms = 0;
        static string GlobalCity = " pusty global City";
        static string GlobalCountry = " pusty global Country";
        static string GlobalPostCode = " pusty global PostCode";
        static string GlobalStreet = " pusty global Street";
        static string GlobalPhoto = " pusty global Photo";
        private readonly ApplicationDbContext _context;
        public AccommodationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ShowDirect()
        {
            return View();
        }
        public IActionResult ShowAccommodation()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            string Id = GlobalId;
            string UserName = "", Title = "", Description = "", CreateDate = "", StartDate = "", EndDate = "", PropertyType = "", City = "", Country = "", PostCode = "", Street = "", Photo = "";
            decimal Price = 0;
            Int32 Floor = 0, Rooms = 0;
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {

                using (SqlCommand cmd = new SqlCommand("SELECT us.UserName," +
                    "ISNULL(ad.Title, ' ')," +
                    "ISNULL(ad.Description, ' ')," +
                    "ISNULL(FORMAT(ad.CreateDate, 'dd mm yyyy'), ' ')," +
                    "ISNULL(FORMAT(ad.StartDate, 'dd mm yyyy'), ' ')," +
                    "ISNULL(FORMAT(ad.EndDate, 'dd mm yyyy'), ' ')," +
                    "ISNULL(ad.Price, ' ')," +
                    "ISNULL(ad.PropertyType, ' ')," +
                    "ISNULL(ad.Floor, ' ')," +
                    "ISNULL(ad.Rooms, ' ')," +
                    "ISNULL(ads.City, ' ')," +
                    "ISNULL(ads.Country, ' ')," +
                    "ISNULL(ads.Postcode, ' ')," +
                    "ISNULL(ads.Street, ' ')," +
                    "ISNULL(pic.Picture1, ' ') " +
                    "from AspNetAdvertisement ad " +
                    "left join AspNetUsers us on ad.UserId = us.Id " +
                    "left join AspNetPicture pic on pic.AdvertisementId = ad.Id " +
                    "left join AspNetAdress ads on ads.AdvertisementId = ad.Id WHERE ad.Id=" + Id + "; "))
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
                            CreateDate = sdr.GetString(3);
                            StartDate = sdr.GetString(4);
                            EndDate = sdr.GetString(5);
                            Price = sdr.GetDecimal(6);
                            PropertyType = sdr.GetString(7);
                            Floor = sdr.GetInt32(8);
                            Rooms = sdr.GetInt32(9);
                            City = sdr.GetString(10);
                            Country = sdr.GetString(11);
                            PostCode = sdr.GetString(12);
                            Street = sdr.GetString(13);

                            Photo = sdr.GetString(14);
                        }
                    }
                    con.Close();
                }
            }

            GlobalUserName = UserName;
            GlobalTitle = Title;
            GlobalDescription = Description;
            GlobalCreateDate = CreateDate;
            GlobalStartDate = StartDate;
            GlobalEndDate = EndDate;
            GlobalPrice = Price;
            GlobalPropertyType = PropertyType;
            GlobalFloor = Floor;
            GlobalRooms = Rooms;
            GlobalCity = City;
            GlobalCountry = Country;
            GlobalPostCode = PostCode;
            GlobalStreet = Street;
            GlobalPhoto = Photo;

            TempData["UserName"] = GlobalUserName;
            TempData["Title"] = GlobalTitle;
            TempData["Description"] = GlobalDescription;
            TempData["CreateDate"] = GlobalCreateDate;
            TempData["StartDate"] = GlobalStartDate;
            TempData["EndDate"] = GlobalEndDate;
            TempData["Price"] = GlobalPrice;
            TempData["PropertyType"] = GlobalPropertyType;
            TempData["Floor"] = GlobalFloor;
            TempData["Rooms"] = GlobalRooms;
            TempData["City"] = GlobalCity;
            TempData["Country"] = GlobalCountry;
            TempData["PostCode"] = GlobalPostCode;
            TempData["Street"] = GlobalStreet;
            TempData["Photo"] = GlobalPhoto;

            return PartialView();
        }

        public void GetID(int Id)
        {
            GlobalId = Id.ToString();
        }

        public void DeleteUser(string Id)
        {
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("EXEC DELETE_USER_P @USER_ID='" + Id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    con.Close();
                }
            }
        }

        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var AccommodationData = (from ep in _context.AspNetAdvertisement
                                         join t in _context.UsersTB on ep.UserId equals t.Id
                                         join e in _context.AspNetAdress on ep.Id equals e.AdvertisementId
                                         select new
                                         {
                                             Id = ep.Id,
                                             Title = ep.Title,
                                             UserName = t.UserName,
                                             StartDate = ep.StartDate,
                                             EndDate = ep.EndDate,
                                             Price = ep.Price,
                                             PropertyType = ep.PropertyType,
                                             City = e.City
                                         });

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    AccommodationData = AccommodationData.Where(m => m.Title == searchValue);
                }

                //total number of rows count   
                recordsTotal = AccommodationData.Count();
                //Paging   
                var data = AccommodationData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}