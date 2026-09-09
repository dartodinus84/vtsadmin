using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_igo : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_small_box_igo '" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblStock.InnerHtml = Convert.ToDouble(Rec.Fields("cntStock")).ToString("#,##0");
                    lblSale.InnerHtml = Convert.ToDouble(Rec.Fields("cntSO")).ToString("#,##0");
                    lblCust.InnerHtml = Convert.ToDouble(Rec.Fields("cntCust")).ToString("#,##0");
                    lblDeactive.InnerHtml = Convert.ToDouble(Rec.Fields("cntExp")).ToString("#,##0");

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Clear()
        {
            try
            {
               
            }
            catch (Exception ex)
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strHtmlMenu = "";
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHCAL"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            open_dashboard_data();
                            strHtmlMenu = ClType.BuildDashMenu(Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString());
                        }
                        else
                        {
                            Response.Redirect("login.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {

        }
        public string TechIDLogin()
        {
            return Session["ClsTypeUserTechnicianID"].ToString().Trim();
        }
        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        [WebMethod]
        public static string technicianIGOSchedule()
        {
            string sOut = "";
            List<DataTechnicianIGOScheduler> dJson = new List<DataTechnicianIGOScheduler>();
            try
            {
                var pageData = new dashboard_igo();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_schedule_igo '" + pageData.TechIDLogin() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianIGOScheduler
                        {
                            startDate = Rec.Fields("job_date"),
                            endDate = Rec.Fields("job_date"),
                            summary = Rec.Fields("title")

                        });
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }
            return JsonConvert.SerializeObject(new { data = dJson });
        }

    }
    public class DataTechnicianIGOScheduler
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string summary { get; set; }
    }
}