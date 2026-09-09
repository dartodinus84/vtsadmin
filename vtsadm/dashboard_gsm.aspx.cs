using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_gsm : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_gsm";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblGsmMW.InnerHtml = Convert.ToDouble(Rec.Fields("sum_MW")).ToString("#,##0");
                    lblGsmMT.InnerHtml = Convert.ToDouble(Rec.Fields("sum_MT")).ToString("#,##0");
                    lblGsmIS.InnerHtml = Convert.ToDouble(Rec.Fields("sum_IS")).ToString("#,##0");
                    lblGsmRS.InnerHtml = Convert.ToDouble(Rec.Fields("sum_RS")).ToString("#,##0");
                    lblGsmSP.InnerHtml = Convert.ToDouble(Rec.Fields("sum_SP")).ToString("#,##0");
                    lblGsmSB.InnerHtml = Convert.ToDouble(Rec.Fields("sum_SB")).ToString("#,##0");
                    lblWarehouseJKT.InnerHtml = Convert.ToDouble(Rec.Fields("sum_JKT")).ToString("#,##0");
                    lblWarehouseSBY.InnerHtml = Convert.ToDouble(Rec.Fields("sum_SBY")).ToString("#,##0");
                    lblWarehouseSBYQC.InnerHtml = Convert.ToDouble(Rec.Fields("sum_SBY_QC")).ToString("#,##0");
                    lblWarehousePDG.InnerHtml = Convert.ToDouble(Rec.Fields("sum_PDG")).ToString("#,##0");
                    lblWarehouseSMG.InnerHtml = Convert.ToDouble(Rec.Fields("sum_SMG")).ToString("#,##0");
                    lblWarehousePLB.InnerHtml = Convert.ToDouble(Rec.Fields("sum_PLB")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHGSM"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            open_dashboard_data();
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

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        [WebMethod]
        public static string GsmWarehouse()
        {
            string sOut = "";
            List<DataWarehouseGsm> dJson = new List<DataWarehouseGsm>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_gsm_warehouse";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataWarehouseGsm
                        {
                            warehouse = Rec.Fields("WarehouseName"),
                            mw = Convert.ToInt32(Rec.Fields("warehouse")),
                            mt = Convert.ToInt32(Rec.Fields("technician")),
                            inst = Convert.ToInt32(Rec.Fields("installed")),
                            rs = Convert.ToInt32(Rec.Fields("readysuspend")),
                            sp = Convert.ToInt32(Rec.Fields("suspended")),
                            sb = Convert.ToInt32(Rec.Fields("softblocked"))
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

        [WebMethod]
        public static string GsmTechnician(string sBranchID)
        {
            string sOut = "";
            List<DataTechnicianGsm> dJson = new List<DataTechnicianGsm>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_gsm_technician '" + sBranchID + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianGsm
                        {
                            technician = Rec.Fields("TechnicianName"),
                            gsm = Convert.ToInt32(Rec.Fields("sum_gsm"))
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
    public class DataWarehouseGsm
    {
        public string warehouse { get; set; }
        public int mw { get; set; }
        public int mt { get; set; }
        public int inst { get; set; }
        public int rs { get; set; }
        public int sp { get; set; }
        public int sb { get; set; }
    }

    public class DataTechnicianGsm
    {
        public string technician { get; set; }
        public int gsm { get; set; }
    }
}