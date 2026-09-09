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
    public partial class dashboard_customer : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_mst_customer  '" + Session["ClsTypeUserGroupID"].ToString() + "', '" + Session["ClsTypeUserID"].ToString() + "','1', ''";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblTotalCustomer.InnerHtml = Convert.ToDouble(Rec.Fields("total_customer")).ToString("#,##0");
                    lblTotalCorporate.InnerHtml = Convert.ToDouble(Rec.Fields("total_corporate")).ToString("#,##0");
                    lblTotalPersonal.InnerHtml = Convert.ToDouble(Rec.Fields("total_personal")).ToString("#,##0");
                    lblTotalTrial.InnerHtml = Convert.ToDouble(Rec.Fields("total_trial")).ToString("#,##0");
                    lblTotalEastTrial.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_trial")).ToString("#,##0");
                    lblTotalWestTrial.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_trial")).ToString("#,##0");

                    
                    lblTotalEast.InnerHtml = Convert.ToDouble(Rec.Fields("total_east")).ToString("#,##0");
                    lblTotalWest.InnerHtml = Convert.ToDouble(Rec.Fields("total_west")).ToString("#,##0");

                    lblTotalEastUnitActive.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_unit_active")).ToString("#,##0");
                    lblTotalEastUnitSp.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_unit_suspend")).ToString("#,##0");
                    lblTotalEastSuspend.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_suspend")).ToString("#,##0");
                    lblTotalEastSoftBlock.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_softblock")).ToString("#,##0");

                    lblTotalWestUnitActive.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_unit_active")).ToString("#,##0");
                    lblTotalWestUnitSP.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_unit_suspend")).ToString("#,##0");
                    lblTotalWestSuspend.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_suspend")).ToString("#,##0");
                    lblTotalWestSoftBlock.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_softblock")).ToString("#,##0");

                    lblTotalWestReguler.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_reguler")).ToString("#,##0");
                    lblTotalWestMedium.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_medium")).ToString("#,##0");
                    lblTotalWestPriority.InnerHtml = Convert.ToDouble(Rec.Fields("total_west_priority")).ToString("#,##0");

                    lblTotalEastReguler.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_reguler")).ToString("#,##0");
                    lblTotalEastMedium.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_medium")).ToString("#,##0");
                    lblTotalEastPriority.InnerHtml = Convert.ToDouble(Rec.Fields("total_east_priority")).ToString("#,##0");

                    lblTotalGps.InnerHtml = Convert.ToDouble(Rec.Fields("total_gps")).ToString("#,##0");
                    lblTotalEseal.InnerHtml = Convert.ToDouble(Rec.Fields("total_eseal")).ToString("#,##0");
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

        [WebMethod(EnableSession = true)]
        public static string DashCustomer(string sType)
        {
            string sOut = "";
            List<DataDashCustomer> dJson = new List<DataDashCustomer>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();

                string strSQL = "sp_dashboard_mst_customer  '" + HttpContext.Current.Session["ClsTypeUserGroupID"].ToString() + "', '" + HttpContext.Current.Session["ClsTypeUserID"].ToString() + "','1', ''";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    if (sType == "CHART01") {
                        dJson.Add(new DataDashCustomer
                        {
                            title = "Corporate",
                            total = Convert.ToInt32(Rec.Fields("total_corporate"))
                        });

                        dJson.Add(new DataDashCustomer
                        {
                            title = "Personal",
                            total = Convert.ToInt32(Rec.Fields("total_personal"))
                        });
                    }

                    if (sType == "CHART02")
                    {
                        dJson.Add(new DataDashCustomer
                        {
                            title = "East",
                            total = Convert.ToInt32(Rec.Fields("total_east"))
                        });

                        dJson.Add(new DataDashCustomer
                        {
                            title = "West",
                            total = Convert.ToInt32(Rec.Fields("total_west"))
                        });
                    }

                    if (sType == "CHART03")
                    {
                        dJson.Add(new DataDashCustomer
                        {
                            title = "Priority",
                            total = Convert.ToInt32(Rec.Fields("total_priority"))
                        });

                        dJson.Add(new DataDashCustomer
                        {
                            title = "Below 10 Unit",
                            total = Convert.ToInt32(Rec.Fields("total_reguler"))
                        });

                        dJson.Add(new DataDashCustomer
                        {
                            title = "11 - 50 Unit",
                            total = Convert.ToInt32(Rec.Fields("total_medium"))
                        });
                    }

                    if (sType == "CHART04")
                    {
                        dJson.Add(new DataDashCustomer
                        {
                            title = "East",
                            total = Convert.ToInt32(Rec.Fields("total_activity_east"))
                        });

                        dJson.Add(new DataDashCustomer
                        {
                            title = "West",
                            total = Convert.ToInt32(Rec.Fields("total_activity_west"))
                        });
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
  

    public class DataDashCustomer
    {
        public string title { get; set; }
        public int total { get; set; }
    }
}