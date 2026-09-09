using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_maint_vehicle_search : System.Web.UI.Page
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

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewVehicle("");
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
                Open_GridViewVehicle(txtSearchVehicle.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewVehicle(string sSearch)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_vehicle_maint_vehicle_search '" + txtCustID.Value.Trim() + "','" + txtJobID.Value.Trim() + "','" + sSearch + "'";
            Session["RecListVehicleMaintVehicleSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging); ;
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleMaintVehicleSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 7; i <= 27; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string PoliceNo = e.Row.Cells[2].Text.ToString().Replace("&#160;", " ");

                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postVehicleChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + PoliceNo + "'," +
                                              "'" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[5].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[7].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[9].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[11].Text.ToString() + "','" + e.Row.Cells[12].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[13].Text.ToString() + "','" + e.Row.Cells[14].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[15].Text.ToString() + "','" + e.Row.Cells[16].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[17].Text.ToString() + "','" + e.Row.Cells[18].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[19].Text.ToString() + "','" + e.Row.Cells[20].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[21].Text.ToString() + "','" + e.Row.Cells[22].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[23].Text.ToString() + "','" + e.Row.Cells[24].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[25].Text.ToString() + "','" + e.Row.Cells[26].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[27].Text.ToString() + "');return false;";
                    for (int i = 7; i <= 27; i++)
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