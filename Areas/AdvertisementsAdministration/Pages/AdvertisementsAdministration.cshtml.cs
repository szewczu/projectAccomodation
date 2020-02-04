using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
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
            var rawHtmlString = GetData().ToString();
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
                    "ad.Description as 'Opis:', " +
                    "FORMAT(ad.EndDate, 'dd-MM-yyyy') as 'Data do:', " +
                    "ad.Price as 'Cena: ', " +
                    "ad.PropertyType as 'Typ zabudowy:', " +
                    "ads.City as 'Miasto:' " +
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
                                if (sdr.GetName(i) == "Numer id:")
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
                        html.Append("<a class=\"button\" onclick=EditAdvertisement(" + adIdValue + "); href='/AdminPanelAdvertisement/ShowEditAdvertisement'  />Edytuj</a>");//asp-area="AdvertisementsAdministration" asp-page="/AdvertisementsAdministration"
                        html.Append("</td>");
                        html.Append("<td class\"click\">");
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