using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_maint_new_vehicle_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTVEH"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (Session["VehicleMaintCustID"] != null)
                {
                    txtCustID.Value = Session["VehicleMaintCustID"].ToString();
                }
                Open_GridViewVehicle(txtCustID.Value.Trim(), txtSearchVehicle.Value.Trim());

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            //Open_GridViewVehicle(txtCustID.Value.Trim(), txtSearchVehicle.Value.Trim());
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

        protected void CmdSearchVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewVehicle(txtCustID.Value.Trim(), txtSearchVehicle.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewVehicle(string sCustID, string sPoliceNo)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_vehicle_maint_new_vehicle_search '" + sCustID + "','" + sPoliceNo + "'";
            Session["RecListVehicleMaintNewVehicleSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleMaintNewVehicleSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 7; i <= 8; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    string PoliceNo = e.Row.Cells[2].Text.ToString().Replace("#160;", " ");

                    PoliceNo= System.Text.RegularExpressions.Regex.Replace(PoliceNo, @"(\s+|\.|\,|\:|\*|&|\?|\/)", " ");

                    CmdButton.OnClientClick = "parent.postNewVehicleChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + PoliceNo  + "'," +
                                              "'" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[5].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[7].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "');return false;";
                    for (int i = 7; i <= 8; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}