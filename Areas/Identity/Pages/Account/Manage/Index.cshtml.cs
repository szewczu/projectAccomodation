using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Noclegi.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Nazwa użytkownika")]
            public string Username { get; set; }

            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Display(Name = "Płeć")]
            public string Gender { get; set; }

            [Display(Name = "Data urodzenia")]
            public DateTime DateOfBirth { get; set; }

        }
        
        public SqlConnection CreateSqlConnection()
        {
            string connectionString = @"Data Source=noclegiDB.mssql.somee.com; user id = aspr1me_SQLLogin_1; pwd = uzputoxk9x";
            SqlConnection cnn = new SqlConnection(connectionString);
            return cnn;
        }

        public Object GetSingleValueFromSqlCommand(string query)
        {
            SqlConnection cnn = CreateSqlConnection();
            cnn.Open();
            SqlCommand command = new SqlCommand(query, cnn);
            Object result = null;
            if(command.ExecuteScalar() == DBNull.Value)
            {
                return result;
            }
            else
            {
              return  command.ExecuteScalar();
            }
        }

        public IActionResult OnPost(IdentityUser user)
        {
            var cnn = CreateSqlConnection();
            cnn.Open();
            var name= Input.Name;
            var surname = Input.Surname;
            var dateOfBirth = Input.DateOfBirth;
            var phoneNumber = Input.PhoneNumber;
            var userName = HttpContext.User.Identity.Name;
            var gender = "Other";
            if (string.IsNullOrEmpty(Input.Gender) || Input.Gender != "Male" && Input.Gender != "Female")
            {
                 gender = "Other";
            }
            else
            {
                 gender = Input.Gender;
            }

           string dateOfBirthString =  dateOfBirth.ToString("yyyy-MM-dd");

            string  sql = $"Update AspNetUsers " +
                $"set  PhoneNumber = \'{phoneNumber}\' ," +
                $"Name = \'{name}\'," +
                $"DateOfBirth= \'{dateOfBirthString}\'," +
                $"Gender= \'{gender}\'," +
                $"Surname= \'{surname}\'" +
                $"where UserName = \'{userName}\';";
            SqlCommand command = new SqlCommand(sql, cnn);
            command.ExecuteNonQuery();
            cnn.Close();
            return Page();
        }

        public IActionResult OnGet(IdentityUser user)
        {
            var connection = CreateSqlConnection();
            connection.Open();
            var userName = HttpContext.User.Identity.Name;
            // Maybe it should be single query to database and place data to object to retrive data later
                var name = GetSingleValueFromSqlCommand($"Select Name from AspNetUsers where UserName = \'{userName}\'");
                var surname = GetSingleValueFromSqlCommand($"Select Surname from AspNetUsers where UserName = \'{userName}\'");
             var phoneNumber = GetSingleValueFromSqlCommand($"Select PhoneNumber from AspNetUsers where UserName = \'{userName}\'");
            var id = GetSingleValueFromSqlCommand($"Select Id from AspNetUsers where UserName = \'{userName}\'");
            var gender = GetSingleValueFromSqlCommand($"Select Gender from AspNetUsers where UserName = \'{userName}\'");
            var dateOfBirth= GetSingleValueFromSqlCommand($"Select DateOfBirth from AspNetUsers where UserName = \'{userName}\'");
            if(dateOfBirth == null)
            {
                dateOfBirth = DateTime.MinValue;
            }
            Input = new InputModel
            {
                PhoneNumber = (string)phoneNumber,
                Username = (string)userName,
                Surname = (string)surname,
                Name = (string)name,
                Gender = (string)gender,
               DateOfBirth = (DateTime)dateOfBirth
 
            };
            return Page();
        }

    }
}
