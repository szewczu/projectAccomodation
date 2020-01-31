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

namespace Noclegi.Areas.AdvertisementsAdministration.Pages
{

    public class AdvertisementsAdministrationModel : PageModel
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
            
        public int DeleteAdvertisement(int Id)
        {
            SqlConnection con2 = DatabaseFunctions.CreateSqlConnection();
            using (SqlCommand cmd2 = new SqlCommand("Exec DELETE_ADVERTISEMENT_P " + Id, con2))
            {
                cmd2.Connection = con2;
                con2.Open();
                cmd2.BeginExecuteNonQuery();
                con2.Close();
            }
            return 0;
        }

        public StringBuilder GetSelectedId()
        {
            StringBuilder html = new StringBuilder();
            html.Append("document.getElementById('SelectedId').textContent");
            return html;
        }

        public StringBuilder GetData()
        {
            StringBuilder html = new StringBuilder();
            using (SqlConnection con = DatabaseFunctions.CreateSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand("select " +
                    "ad.Id as 'Numer id:', " +
                    "us.UserName as 'Użytkownik:', ad.Title as 'Tytuł:', " +
                    "ad.Description as 'Opis:', " + //"FORMAT(ad.StartDate, 'dd MMMM yyyy') as 'Data od:', " +
                    "FORMAT(ad.EndDate, 'dd MMMM yyyy') as 'Data do:', " +
                    "ad.Price as 'Cena: ', " +
                    ///*"\"Rodzaj: \" =  CASE " +
                    //"WHEN ad.Rent = 1 THEN 'Mfg item - not for resale' " +
                    //"WHEN ad.LookingFor = 1 THEN 'Under $50'" +
                    //" WHEN ad.Exchange = 1 and ListPrice < 250 THEN 'Under $250'" +
                    //"WHEN ad.ExchangeAdId = 1 and ListPrice < 1000 THEN 'Under $1000'" +
                    //"ELSE ' ' END, " +*/
                    //"ad.Rent as 'Wynajme: ', " +
                    //"ad.LookingFor as 'Poszukuje: ', ad.Exchange as 'Zamienie: ', ad.ExchangeAdId as 'Id zamiany na: ', " +
                    //"FORMAT(ad.CreateDate, 'dd MMMM yyyy') as 'Data utworzenia:', " +
                    "ad.PropertyType as 'Typ zabudowy:', " +
                    //"ad.Floor as 'Piętro', ad.Rooms as 'Ilość pokoi:', " +
                    "ads.City as 'Miasto:' " + //, " +
                    //"ads.Country as 'Państwo:', " +
                                                    //"ads.Postcode as 'Kod pocztowy:', ads.Province as 'Województwo:', ads.Street as 'Ulica:' " +
                    "from AspNetAdvertisement ad " +
                    "left join AspNetUsers us on ad.UserId = us.Id " +
                    "left join AspNetAdress ads on ads.AdvertisementId = ad.Id; "))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    html.Append("<div class=\"table - responsive\">");
                    html.Append("<table class=\"table table-striped table-bordered dt-responsive nowrap\" width=\"100%\" cellspacing=\"0\">");
                    int index = 1;
                    int adId = 0;
                    int adIdValue = 0;
                    while (sdr.Read())
                    {
                        if (index == 1)
                        {
                            html.Append("<tr>");
                            for (int i = 0; i < sdr.FieldCount; i++)
                            {
                                html.Append("<th>");
                                html.Append(sdr.GetName(i));
                                if (sdr.GetName(i)== "Numer id:")
                                {
                                    adId = i;
                                }
                                html.Append("</th>");
                            }
                            html.Append("<td>");
                            html.Append(" ");
                            html.Append("</td>");
                            html.Append("<td>");
                            html.Append(" ");
                            html.Append("</td>");
                            html.Append("</tr>");
                        }
                        index++;

                        html.Append("<trOnClick=\"window.location.reload();\">");
                        for (int i = 0; i < sdr.FieldCount; i++)
                        {
                            html.Append("<td>");
                            html.Append(sdr.GetValue(i));
                            if (i == adId)
                            {
                                adIdValue = (int)sdr.GetValue(i);
                            }
                            html.Append("</td>");
                        }
                        html.Append("<td class\"click\">");
                        //html.Append("<button type=\"button\" class=\"button\" data-toggle=\"modal\" data-target=\"#exampleModalLong\" data-whatever=\"@mdo\"> Szczegóły</button>");
                        html.Append("<a class=\"button\" onclick=EditAdvertisement(" + adIdValue + "); href='/AdminPanelAdvertisement/ShowEditAdvertisement'  />Edytuj</a>");//asp-area="AdvertisementsAdministration" asp-page="/AdvertisementsAdministration"
                        /*html.Append("<button type=\"button\" class=\"button\" value=\"3\" OnClick=\"Details(" + adIdValue +
                            ");\" href='/AdvertisementsAdministration/Pages/AdvertisementDetails' > Szczegóły</button>"); //class=\"btn btn - primary\"*/
                        html.Append("</td>");
                        html.Append("<td class\"click\">");
                        //html.Append("<button id=\"demo" + adIdValue + "\" class=\"button\" value=\"3\" " + "OnClick=" + "\"DeleteData(" + adIdValue + ")\">Usun</button>");
                        html.Append("<a id=\"demo" + adIdValue + "\" class=\"button\" value=\"3\" " + "OnClick=" + "\"DeleteData(" + adIdValue + ")\">Usun</button>");
                        html.Append("</td>");
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