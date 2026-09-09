using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_assign_master : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETVEHMASTER"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
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

        private string getServerID(string sTvaID)
        {
            string sOut = "";
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_serverid '" + sTvaID + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim());
                if (Rec.RecordCount() > 0)
                {
                    sOut = Rec.Fields(0).ToString();
                }
            }
            catch (Exception)
            {
                sOut = "";
            }
            return sOut;
        }

        protected void clear()
        {
            ClsType ClType = new ClsType();
            ClType.Open_Combos(CmbCustServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_server_default");

            txtCustID.Value = "";
            txtCustFullName.Text = "";
            txtCustCustTypeDesc.Text = "";
            txtCustBranchName.Text = "";
            txtVehicleID.Value = "";
            txtVehicleDesc.Text = "";
            txtPoliceNo.Text = "";
            txtAssetNo.Text = "";
            txtTvaID.Text = "";
            txtUplineID.Value = "";
            txtUplineFullName.Text = "";
            txtUplineCustTypeDesc.Text = "";
            txtUplineBranchName.Text = "";
            txtSearchAvai.Text = "";
            txtSearchSel.Text = "";

            CmbCustServer.SelectedValue = "[Select]";
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleAssignMasterAvailable"], LblPagingA);
            div_comment.InnerHtml = "";
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleAssignMasterSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sTvaID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTvaID + "','" + txtUplineID.Value.ToString() + "','" + txtCustID.Value.ToString() + "','" + sSearch + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }

        protected void CmdLoadVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViews(GridView2, "sp_list_vehicle_assign_master_available", txtTvaID.Text.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignMasterAvailable",LblPagingA);
                Open_GridViews(GridView1, "sp_list_vehicle_assign_master_selected", txtTvaID.Text.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignMasterSelected",LblPagingS);
                CmbCustServer.SelectedValue = getServerID(txtTvaID.Text.ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridViews(GridView2, "sp_list_vehicle_assign_master_available", txtTvaID.Text.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignMasterAvailable",LblPagingA);
                Open_GridViews(GridView1, "sp_list_vehicle_assign_master_selected", txtTvaID.Text.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignMasterSelected",LblPagingS);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sUplineCustID = ""; string sMasterCustID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        sUplineCustID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        sMasterCustID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                        if (sMasterCustID != "")
                        {
                            strSQL = "sp_vehicle_assign_master_selected_server '" + txtTvaID.Text.ToString() + "','" + sUplineCustID + "','" + sMasterCustID + "','" + CmbCustServer.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_vehicle_assign_master_available", txtTvaID.Text.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignMasterAvailable",LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vehicle_assign_master_selected", txtTvaID.Text.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignMasterSelected",LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment master has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment master has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sTvaID = ""; string sUplineCustID = ""; string sMasterCustID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "REMOVE":
                        sTvaID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                        sUplineCustID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        sMasterCustID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                        if (sMasterCustID != "")
                        {
                            strSQL = "sp_vehicle_assign_master_removed_server '" + sTvaID + "','" + sUplineCustID + "','" + sMasterCustID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_vehicle_assign_master_available", txtTvaID.Text.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignMasterAvailable",LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vehicle_assign_master_selected", txtTvaID.Text.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignMasterSelected",LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle assignment master has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment master has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView2, "sp_list_vehicle_assign_master_available", txtTvaID.Text.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignMasterAvailable",LblPagingA);
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchSel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView1, "sp_list_vehicle_assign_master_selected", txtTvaID.Text.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignMasterSelected",LblPagingS);
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbCustServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
    }
}