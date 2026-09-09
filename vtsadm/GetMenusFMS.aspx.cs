using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace vtsadm
{
    public partial class GetMenusFMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            string json = string.Empty;

            try
            {
                string companyId = Request.QueryString["company_id"];
                string levelUserOri = Request.QueryString["levelUserOri"];
                string secretKey = Request.QueryString["key"];

                // Baca secret key dari web.config
                string expectedKey = System.Configuration.ConfigurationManager.AppSettings["FMSSecretKey"];

                if (string.IsNullOrEmpty(secretKey))
                {
                    json = "{\"status\":400, \"message\":\"Missing secret key.\"}";
                    Response.Write(json);
                    return;
                }

                if (secretKey != expectedKey)
                {
                    json = "{\"status\":403, \"message\":\"Invalid secret key.\"}";
                    Response.Write(json);
                    return;
                }

                if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(levelUserOri))
                {
                    json = "{\"status\":400, \"message\":\"Missing required parameters.\"}";
                    Response.Write(json);
                    return;
                }

                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["VTSADMIN"].ToString();
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("sp_get_fms_menus_tmsv2", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", companyId);
                    cmd.Parameters.AddWithValue("@serverid", "SVR0000002");
                    cmd.Parameters.AddWithValue("@level_user", levelUserOri);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string strHTML = "";
                        string urlHost = "https://indocement-beta.easygo-gps.co.id";
                        string sBadge = "";

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string menuId = reader["MenuID"].ToString();
                                string menuName = reader["MenuName"].ToString();
                                string menuIcon = reader["MenuIcon"].ToString();
                                string menuName1 = reader["MenuName1"].ToString();
                                string menuName2 = reader["MenuName2"].ToString();

                                strHTML += $"\r\n<li class='kt-menu__item' aria-haspopup='true'>" +
                                           $"<a href='{urlHost}/Sitemap?MenuID={menuId}&&MenuName={menuName}' " +
                                           $"data-link-href='{urlHost}/Sitemap?MenuID={menuId}' class='kt-menu__link' " +
                                           $"data-trigger='{menuName}' data-trigger1='{menuName1}' data-trigger2='{menuName2}' " +
                                           $"onclick='setPageMenu(this);'>" +
                                           $"<i class='{menuIcon}'><span></span></i><span class='kt-menu__link-text'>{menuName}</span>{sBadge}</a></li>";
                            }
                            strHTML += "\r\n</ul></div></li>";
                        }
                        else
                        {
                            strHTML = "<li class='kt-menu__item'><span>No data found</span></li>";
                        }

                        var result = new { status = 200, html = strHTML };
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        json = js.Serialize(result);
                    }
                }
            }
            catch (Exception ex)
            {
                json = "{\"status\":500, \"message\":\"" + ex.Message.Replace("\"", "'") + "\"}";
            }

            Response.Write(json);
            Response.End();
        }
    }
}
