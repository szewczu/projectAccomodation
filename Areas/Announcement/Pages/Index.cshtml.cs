using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Data;
using Noclegi.Helpers;

namespace Noclegi.Areas.Announcement.Pages
{

    public class IndexModel : PageModel
    {
        [BindProperty]
        public TableInput Input { get; set; }

        public class TableInput
        {
            public string HtmlTable { get; set; }

        }

        public void OnGet()
        {
           var rawHtmlString =  GetData().ToString();
            Input = new TableInput
            {
                HtmlTable = rawHtmlString
            };
        }

        public StringBuilder GetData()
        {
            StringBuilder html = new StringBuilder();
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("select * from AspNetUsers"))
                {

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    //Building an HTML string.


                    //Table start.
                    html.Append("<table class=\"table\" border = '1'>");


                    html.Append("<table>");
                    int index = 1;
                    while (sdr.Read())
                    {
                        if (index == 1)
                        {
                            html.Append("<tr>");
                            for (int i = 0; i < sdr.FieldCount; i++)
                            {
                                html.Append("<td>");
                                html.Append(sdr.GetName(i));
                                html.Append("</td>");
                            }
                            html.Append("</tr>");
                        }
                        index++;

                        html.Append("<tr>");
                        for (int i = 0; i < sdr.FieldCount; i++)
                        {
                            html.Append("<td>");
                            html.Append(sdr.GetValue(i));
                            html.Append("</td>");
                        }
                        html.Append("</tr>");
                    }

                    html.Append("</table>");
                    //Table end.
                    //Append the HTML string to Placeholder.
                  //  PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
                    con.Close();
                }
            }
            return html;
        }
    }
}