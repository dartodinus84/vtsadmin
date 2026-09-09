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
    public partial class vehicle_position : System.Web.UI.Page
    {
        public string strPosition = "";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_dashboard_vehicle_position '" + txtCustID.Value.Trim() + "','" + txtSearch.Text.Trim() + "'";
                Session["RecDashboardPosition"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
                //Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                //GridView1.DataSource = Rec.DataRecord();
                //GridView1.DataBind();
                // = Rec.RecData;
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

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            strPosition = "";
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "";
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

        private void clear()
        {
            try
            {
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                txtPosition.Value = "";
                txtSearch.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            txtPosition.Value = "";
            ClType.Gv_PageIndexChanging(GridView1, e.NewPageIndex, Session["RecDashboardPosition"], LblPaging);            
            div_comment.InnerHtml = "";
        }

        protected void CmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string sPos = "";
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 3; i <= 5; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 3; i <= 5; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    sPos = txtPosition.Value.Trim();
                    sPos = sPos + "|" + e.Row.Cells[3].Text.ToString() + ";" + e.Row.Cells[4].Text.ToString() + ";" + e.Row.Cells[5].Text.ToString();
                    txtPosition.Value = sPos;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtPosition.Value = "";
                div_comment.InnerHtml = "";
                Open_GridView();
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
        public static string realtime(string sCustID, string sPoliceNo)
        {
            string sOut = "";
            var dataJson = new DataPosition() { };
            try
            {
                var pageData = new vehicle_position();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_realtime '" + sCustID.Trim() + "','" + sPoliceNo + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    dataJson = new DataPosition
                    {
                        RowNo = Rec.Fields("RowNo"),
                        CustID = Rec.Fields("CustID"),
                        VehicleID = Rec.Fields("VehicleID"),
                        PoliceNo = Rec.Fields("PoliceNo"),
                        Long = Rec.Fields("Long"),
                        Lat = Rec.Fields("Lat"),
                        GPS_Time = Rec.Fields("gps_time")
                    };
                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }
            return JsonConvert.SerializeObject(new { data = dataJson });
        }
    }

    public class DataPosition
    {
        public string RowNo { get; set; }
        public string CustID { get; set; }
        public string VehicleID { get; set; }
        public string PoliceNo { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public string GPS_Time { get; set; }
    }
}