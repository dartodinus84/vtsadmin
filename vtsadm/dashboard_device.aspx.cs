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
    public partial class dashboard_device : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_device";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblDeviceRG.InnerHtml = Convert.ToDouble(Rec.Fields("sum_RG")).ToString("#,##0");
                    lblDeviceMW.InnerHtml = Convert.ToDouble(Rec.Fields("sum_MW")).ToString("#,##0");
                    lblDeviceMT.InnerHtml = Convert.ToDouble(Rec.Fields("sum_MT")).ToString("#,##0");
                    lblDeviceIS.InnerHtml = Convert.ToDouble(Rec.Fields("sum_IS")).ToString("#,##0");
                    lblDeviceBR.InnerHtml = Convert.ToDouble(Rec.Fields("sum_BR")).ToString("#,##0");
                    lblDeviceDE.InnerHtml = Convert.ToDouble(Rec.Fields("sum_DE")).ToString("#,##0");
                    lblWarehouseJKT .InnerHtml = Convert.ToDouble(Rec.Fields("sum_JKT")).ToString("#,##0");
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHDEVICE"))
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
        public static string DeviceWarehouse()
        {
            string sOut = "";
            List<DataWarehouseDevice> dJson = new List<DataWarehouseDevice>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_device_warehouse";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataWarehouseDevice
                        {
                            warehouse = Rec.Fields("WarehouseName"),
                            mw = Convert.ToInt32(Rec.Fields("warehouse")),
                            mt = Convert.ToInt32(Rec.Fields("technician")),
                            inst = Convert.ToInt32(Rec.Fields("installed")),
                            br = Convert.ToInt32(Rec.Fields("brooken"))
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
        public static string DeviceTechnician(string sBranchID)
        {
            string sOut = "";
            List<DataTechnicianDevice> dJson = new List<DataTechnicianDevice>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_device_technician '" + sBranchID + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianDevice
                        {
                            technician = Rec.Fields("TechnicianName"),
                            device = Convert.ToInt32(Rec.Fields("sum_device"))
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
    public class DataWarehouseDevice
    {
        public string warehouse { get; set; }
        public int mw { get; set; }
        public int mt { get; set; }
        public int inst { get; set; }
        public int br { get; set; }
    }

    public class DataTechnicianDevice
    {
        public string technician { get; set; }
        public int device { get; set; }
    }
}