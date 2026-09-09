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
    public partial class dashboard_calendar : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_small_box_schedule '" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntDone.InnerHtml = Convert.ToDouble(Rec.Fields("cntDone")).ToString("#,##0");
                    LblCntLate.InnerHtml = Convert.ToDouble(Rec.Fields("cntLate")).ToString("#,##0");
                    lblCntOpen.InnerHtml = Convert.ToDouble(Rec.Fields("cntOpen")).ToString("#,##0");
                    LblCntAvail.InnerHtml = Convert.ToDouble(Rec.Fields("cntAvail")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void open_dashboard_data_legend_maint()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_maint'" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblJKT1.InnerHtml = Convert.ToDouble(Rec.Fields("cntJKT")).ToString("#,##0");
                    lblSBY1.InnerHtml = Convert.ToDouble(Rec.Fields("cntSBY")).ToString("#,##0");
                    lblSMG1.InnerHtml = Convert.ToDouble(Rec.Fields("cntSMG")).ToString("#,##0");
                    lblMDN1.InnerHtml = Convert.ToDouble(Rec.Fields("cntMDN")).ToString("#,##0");
                    lblPDG1.InnerHtml = Convert.ToDouble(Rec.Fields("cntPDG")).ToString("#,##0");
                    lblPLG1.InnerHtml = Convert.ToDouble(Rec.Fields("cntPLG")).ToString("#,##0");

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void open_dashboard_data_legend_new()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_new '" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblJKT.InnerHtml = Convert.ToDouble(Rec.Fields("cntJKT")).ToString("#,##0");
                    lblSBY.InnerHtml = Convert.ToDouble(Rec.Fields("cntSBY")).ToString("#,##0");
                    lblSMG.InnerHtml = Convert.ToDouble(Rec.Fields("cntSMG")).ToString("#,##0");
                    lblMDN.InnerHtml = Convert.ToDouble(Rec.Fields("cntMDN")).ToString("#,##0");
                    lblPDG.InnerHtml = Convert.ToDouble(Rec.Fields("cntPDG")).ToString("#,##0");
                    lblPLG.InnerHtml = Convert.ToDouble(Rec.Fields("cntPLG")).ToString("#,##0");


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
                LblCntDone.InnerText = "0";
                LblCntLate.InnerText = "0";
                lblCntOpen.InnerText = "0";
                LblCntAvail.InnerText = "0";

                lblJKT.InnerHtml = "0";
                lblSBY.InnerHtml = "0";
                lblSMG.InnerHtml = "0";
                lblMDN.InnerHtml = "0";
                lblPDG.InnerHtml = "0";
                lblPLG.InnerHtml = "0";

                lblJKT1.InnerHtml = "0";
                lblSBY1.InnerHtml = "0";
                lblSMG1.InnerHtml = "0";
                lblMDN1.InnerHtml = "0";
                lblPDG1.InnerHtml = "0";
                lblPLG1.InnerHtml = "0";
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
                            open_dashboard_data_legend_maint();
                            open_dashboard_data_legend_new();
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
        public static string technicianSchedule()
        {
            string sOut = "";
            List<DataTechnicianScheduler> dJson = new List<DataTechnicianScheduler>();
            try
            {
                var pageData = new dashboard_calendar();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_schedule_technician '" + pageData.TechIDLogin() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianScheduler
                        {
                            startDate = Rec.Fields("hari"),
                            endDate = Rec.Fields("hari"),
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
    public class DataTechnicianScheduler
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string summary { get; set; }
    }
}