using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Noclegi.Data;
using Noclegi.Helpers;

namespace Noclegi.Areas.Accommodation.Pages
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
            var rawHtmlString = GetData().ToString();
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
                using (SqlCommand cmd = new SqlCommand("select " +
                    "ad.Id as 'Numer id:', " +
                    "ad.Title as 'Tytuł:', " +
                    "us.UserName as 'Użytkownik:'," +
                    "FORMAT(ad.StartDate, 'dd MMMM yyyy') as 'Data od:', " +
                    "FORMAT(ad.EndDate, 'dd MMMM yyyy') as 'Data do:', " +
                    "ad.Price as 'Cena: ', " +
                    "ad.PropertyType as 'Typ zabudowy:', " +
                    "ads.City," +
                    "ads.Country," +
                    "ads.Postcode," +
                    "ads.Street, " +
                    "pic.Picture1 " +
                    "from AspNetAdvertisement ad " +
                    "left join AspNetUsers us on ad.UserId = us.Id " +
                    "left join AspNetPicture pic on pic.AdvertisementId = ad.Id " +
                    "left join AspNetAdress ads on ads.AdvertisementId = ad.Id; "))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    html.Append("<div class=\"table\">");
                    html.Append("<table id=\"pagin\"class=\"table table-striped table-bordered nowrap\"  cellspacing=\"10\">");
                    int index = 1;
                    int adId = 0;
                    int adIdValue = 0;
                    while (sdr.Read())
                    {
                        if (index == 1)
                        {
                            for (int i = 0; i < sdr.FieldCount; i++)
                            {
                                if (sdr.GetName(i) == "Numer id:")
                                {
                                    adId = i;
                                }
                            }
                        }
                        index++;
                        for (int i = 0; i < sdr.FieldCount; i++)
                        {
                            if (i == adId)
                            {
                                adIdValue = (int)sdr.GetValue(i);
                            }
                        }
                        html.Append("<tr>");
                        html.Append("<td style=\"width:auto; height:auto;\">");
                        html.Append("<div id=\"Img\" style=\"float: left; width:35%;height:25%;\">");
                        //html.Append("<img src=\"https://tinyurl.com/um5qhoz\" style = \"border: 5;  width:40%; float:left;\"> ");
                        html.Append("<img src=\"data: image; base64," + sdr.GetValue(11) + "\" onError=\"this.onerror = null; this.src='https://tinyurl.com/y782eus8';\" style = \"border: 5;  width:40%;height:25%; float:left;\"> ");
                        html.Append("</div>");
                        html.Append("<div id=\"tytul\" style=\"float: left; width:20%;height:auto; margin-top:5%; margin-left:-15%; text-align:center;\">");
                        html.Append("<h3>");
                        html.Append(sdr.GetValue(1));
                        html.Append("</h3>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("<div id=\"click\" style=\" text-align:center; width:10%; height:auto;margin-top:5%;float:right;\">"); ;
                        html.Append("<a class=\"button\" onclick=GetID(" + adIdValue + "); href='/Accommodation/ShowAccommodation'  />Szczegóły</a>");

                        html.Append("</div>");
                        html.Append("<div id=\"informacje\" style=\"float: right; width:25%; height:auto; text-align:left;\">");
                        html.Append("<b>Dodane przez: </b>");
                        html.Append(sdr.GetValue(2));
                        html.Append("<br>");
                        html.Append("<b>Od: </b>");
                        html.Append(sdr.GetValue(3));
                        html.Append("<br>");
                        html.Append("<b>Do: </b>");
                        html.Append(sdr.GetValue(4));
                        html.Append("<br>");
                        html.Append("<b>Cena: </b>");
                        html.Append(sdr.GetValue(5));
                        html.Append("<br>");
                        html.Append("<b>Typ zabudowy: </b>");
                        html.Append(sdr.GetValue(6));
                        html.Append("<br>");

                        html.Append("</tr>");
                    }

                    html.Append("</table>");
                    html.Append("</div>");
                    con.Close();
                }
            }
            return html;
        }
    }
}







