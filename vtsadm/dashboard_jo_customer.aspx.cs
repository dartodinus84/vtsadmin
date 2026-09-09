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
    public partial class dashboard_jo_customer : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();

                string filterType = Session["SessionFilterType"].ToString();
                string SupportAreaName = Session["SessionSupportAreaName"].ToString();
                string dateFrom = Session["SessionDateFrom"].ToString();
                string dateTo = Session["SessionDateTo"].ToString();

                string strSQL = "sp_dashboard_jo_customer '" + filterType + "','" + SupportAreaName + "','" + dateFrom + "','" + dateTo + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntJONew.InnerHtml = Convert.ToDouble(Rec.Fields("cntJONew")).ToString("#,##0");
                    LblCntJOMaint.InnerHtml = Convert.ToDouble(Rec.Fields("cntJOMaint")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }



        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SessionDateFrom"] = txtDateFrom.Text.Trim();
                Session["SessionDateTo"] = txtDateTo.Text.Trim();
                Session["SessionFilterType"] = CmbFilterType.SelectedItem.Value.Trim();
                Session["SessionSupportAreaName"] = CmbSupportAreaName.SelectedItem.Value.Trim();

                open_dashboard_data();
                jo_dashboard_new();
                jo_dashboard_maint();
                jo_dashboard_unit();
            }
            catch (Exception ex)
            {

            }
        }

        protected void jo_dashboard_new()
        {
            try
            {
                Recordset Rec = new Recordset();

                string filterType = Session["SessionFilterType"].ToString();
                string SupportAreaName = Session["SessionSupportAreaName"].ToString();
                string dateFrom = Session["SessionDateFrom"].ToString();
                string dateTo = Session["SessionDateTo"].ToString();


                string strSQL = "sp_dashboard_jo_customer_status 'NEW','" + filterType + "','" + SupportAreaName + "','" + dateFrom + "','" + dateTo + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntJoNewOpen.InnerHtml = Convert.ToDouble(Rec.Fields("JobOpen")).ToString("#,##0");
                    LblCntJoNewClose.InnerHtml = Convert.ToDouble(Rec.Fields("JobClose")).ToString("#,##0");
                    LblCntJoNewSla.InnerHtml = Convert.ToDouble(Rec.Fields("sla")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void jo_dashboard_maint()
        {
            try
            {
                Recordset Rec = new Recordset();

                string filterType = Session["SessionFilterType"].ToString();
                string SupportAreaName = Session["SessionSupportAreaName"].ToString();
                string dateFrom = Session["SessionDateFrom"].ToString();
                string dateTo = Session["SessionDateTo"].ToString();

                string strSQL = "sp_dashboard_jo_customer_status 'MAINTENANCE','" + filterType + "','" + SupportAreaName + "','" + dateFrom + "','" + dateTo + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntJoMaintOpen.InnerHtml = Convert.ToDouble(Rec.Fields("JobOpen")).ToString("#,##0");
                    LblCntJoMaintClose.InnerHtml = Convert.ToDouble(Rec.Fields("JobClose")).ToString("#,##0");
                    LblCntJoMaintSla.InnerHtml = Convert.ToDouble(Rec.Fields("sla")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void jo_dashboard_unit()
        {
            try
            {
                Recordset Rec = new Recordset();

                string filterType = Session["SessionFilterType"].ToString();
                string SupportAreaName = Session["SessionSupportAreaName"].ToString();
                string dateFrom = Session["SessionDateFrom"].ToString();
                string dateTo = Session["SessionDateTo"].ToString();


                string strSQL = "sp_dashboard_jo_status 'UNIT','" + filterType + "','" + SupportAreaName + "','" + dateFrom + "','" + dateTo + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntJoUnitOpen.InnerHtml = Convert.ToDouble(Rec.Fields("JobOpen")).ToString("#,##0");
                    LblCntJoUnitClose.InnerHtml = Convert.ToDouble(Rec.Fields("JobClose")).ToString("#,##0");
                    LblCntJoUnitSla.InnerHtml = Convert.ToDouble(Rec.Fields("sla")).ToString("#,##0");
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
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbFilterType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_dashboard_filter");
                ClType.Open_Combos(CmbSupportAreaName, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_list_support_area");

                txtDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

                CmbFilterType.SelectedValue = "[Select]";
                CmbSupportAreaName.SelectedValue = "[Select]";
                LblCntJONew.InnerText = "0";
                LblCntJOMaint.InnerText = "0";

                LblCntJoNewOpen.InnerHtml = "0";
                LblCntJoNewClose.InnerHtml = "0";
                LblCntJoNewSla.InnerHtml = "0";

                LblCntJoMaintOpen.InnerHtml = "0";
                LblCntJoMaintClose.InnerHtml = "0";
                LblCntJoMaintSla.InnerHtml = "0";

                LblCntJoUnitOpen.InnerHtml = "0";
                LblCntJoUnitClose.InnerHtml = "0";
                LblCntJoUnitSla.InnerHtml = "0";


                Session["SessionDateFrom"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionDateTo"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionFilterType"] = "";

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHJO"))
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
                            jo_dashboard_new();
                            jo_dashboard_maint();
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
            List<DataWarehouseDevice2> dJson = new List<DataWarehouseDevice2>();
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
                        dJson.Add(new DataWarehouseDevice2
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
            List<DataTechnicianDevice2> dJson = new List<DataTechnicianDevice2>();
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
                        dJson.Add(new DataTechnicianDevice2
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
    public class DataWarehouseDevice3
    {
        public string warehouse { get; set; }
        public int mw { get; set; }
        public int mt { get; set; }
        public int inst { get; set; }
        public int br { get; set; }
    }

    public class DataTechnicianDevice3
    {
        public string technician { get; set; }
        public int device { get; set; }
    }
}