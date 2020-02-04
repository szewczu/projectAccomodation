using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Noclegi.Helpers;
using Noclegi.Models;
using Microsoft.Data.SqlClient;

namespace Noclegi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public bool CheckAdmin(String Id)
        {
            int RoleId = 0;
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT RoleId FROM AspNetUserRoles WHERE UserId='" + Id + "'"))
                {
                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            RoleId = sdr.GetInt32(0);
                            if (RoleId == 2)
                            {
                                return true;
                            }
                        }
                    }
                    con.Close();
                }
            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
