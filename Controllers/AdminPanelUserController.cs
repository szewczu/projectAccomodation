using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Noclegi.Models;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Noclegi.Data;
using Noclegi.Helpers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860  

namespace Noclegi.Controllers
{
    public class AdminPanelUserController : Controller
    {
        static string GlobalId= " pusty global id";
        static string GlobalUserName = " pusty global username";
        static string GlobalEmail = " pusty global email";
        static string GlobalPhoneNumber = " pusty global phone";
        static string GlobalName = " pusty global name";
        static string GlobalSurname = " pusty global surname";
        static string GlobalGender = " pusty global gender";
        static string GlobalDateOfBirth = " pusty global dateofbirth";
        private ApplicationDbContext _context;

        public AdminPanelUserController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/  
        public IActionResult ShowGrid()
        {

            return View();

        }
        public IActionResult ShowEdit()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            string Id = GlobalId;
            string UserName = "", Email = "", PhoneNumber = "", Name = "", Surname = "", Gender = "", DateOfBrith = "";
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                
                using (SqlCommand cmd = new SqlCommand("SELECT UserName,Email,ISNULL(PhoneNumber,' '),ISNULL(Name,' '),ISNULL(Surname,' '),Gender,ISNULL(FORMAT (DateOfBirth, 'yyyy-MM-dd'),' ') FROM AspNetUsers WHERE Id='" + Id + "'"))
                {

                    cmd.Connection = con;

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            UserName = sdr.GetString(0);
                            Email = sdr.GetString(1);
                            PhoneNumber = sdr.GetString(2);
                            Name = sdr.GetString(3);
                            Surname = sdr.GetString(4);
                            Gender = sdr.GetString(5);
                            DateOfBrith = sdr.GetString(6);


                        }
                    }
                    con.Close();
                }

            }
            
            GlobalUserName = UserName;
            GlobalEmail = Email;
            GlobalPhoneNumber = PhoneNumber;
            GlobalName = Name;
            GlobalSurname = Surname;
            GlobalGender = Gender;
            GlobalDateOfBirth = DateOfBrith;

            TempData["UserName"] = GlobalUserName;
            TempData["Email"] = GlobalEmail;
            TempData["PhoneNumber"] = GlobalPhoneNumber;
            TempData["Name"] = GlobalName;
            TempData["Surname"] = GlobalSurname;
            TempData["Gender"] = GlobalGender;
            TempData["DateOfBirth"] = GlobalDateOfBirth;







            return PartialView();

        }

        public void EditUser(string Id)
        {
            GlobalId = Id;

        }

        public void EditUserButton(string Id, string inputUserName, string inputEmail, string inputPhoneNumber, string inputName, string inputSurname, string inputGender, string inputDateOfBirth)
        {
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE AspNetUsers SET UserName='" + inputUserName + "',Email='" + inputEmail + "' ,PhoneNumber='" + inputPhoneNumber + "',Name='" + inputName + "',Surname='" + inputSurname + "',Gender='" + inputGender + "',DateOfBirth='" + inputDateOfBirth + "' WHERE Id='" + Id + "'"))
                {

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    con.Close();
                }

            }
        }

        public void DeleteUser(string Id)
        {
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                //using (SqlCommand cmd = new SqlCommand("Delete FROM AspNetUsers WHERE Id='"+Id+"'")) 
                using (SqlCommand cmd = new SqlCommand("EXEC DELETE_USER_P @USER_ID='"+Id+"'"))

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

                // Getting all Customer data  
                var customerData = (from tempcustomer in _context.UsersTB
                                    select tempcustomer);
                



                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.UserName == searchValue);
                }

                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
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