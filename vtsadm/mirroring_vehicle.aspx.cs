using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mirroring_vehicle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMIRRORING"))
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


        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListMirroringVehicle"], LblPagingA);
            div_comment.InnerHtml = "";
        }

        protected void Open_GridViews(GridView GrdVw, string sSQL, string sCustID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sCustID + "','" + sSearch + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }

        protected void clear()
        {
            txtCustID.Value = "";
            txtCustFullName.Text = "";
            txtCustCustTypeDesc.Text = "";
            txtCustBranchName.Text = "";
            txtVtsCompanyName.Text = "";
            txtIntpCompanyName.Text = "";
            txtVtsCompanyID.Value = "";
            txtIntpCompanyID.Value = "";
            LblSNSelected.InnerHtml = "";
            LblSNDelete.InnerHtml = "";

            txtVehicleIDSelected.Value = "";
            txtNoSNSelected.Value = "";
            txtServerIDSelected.Value = "";

            txtVehicleIDDelete.Value = "";
            txtNoSNDelete.Value = "";
            txtServerIDDelete.Value = "";

        }

        protected void CmdLoadVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);

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
                Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);
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
                /**
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sVehicleID = ""; string sNoSN = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        sVehicleID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                        sNoSN = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();

                        if (sVehicleID != "")
                        {
                            strSQL = "sp_mirroring_vts_tointp_selected '" + sVehicleID + "','" + sNoSN + "','" + txtIntpCompanyID.Value + "','" + Session["ClsTypeUserID"].ToString() + "' ";

                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                  
                                    Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);
                                    div_comment.InnerHtml = "";
                                 
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
                */

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";

                Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowMirroring(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }
        protected void GridView2_RowUnMirroring(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Visible = false;

                    if (Convert.ToInt32(e.Row.Cells[5].Text.ToString()) == 1)
                    {
                        e.Row.Cells[7].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    e.Row.Cells[6].Visible = false;

                    if (Convert.ToInt32(e.Row.Cells[5].Text.ToString()) == 1)
                    {
                        e.Row.Cells[7].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = false;
                    }


                    LinkButton CmdButtonSelect = (LinkButton)e.Row.Cells[7].FindControl("CmdSelected");
                    CmdButtonSelect.OnClientClick = "confirmSelected('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesSelected_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";

                if (txtVehicleIDSelected.Value.ToUpper().Trim() != "")
                {

                    strSQL = "sp_vehicle_mirroring_selected '" + txtVehicleIDSelected.Value + "','" + txtNoSNSelected.Value + "','" + txtVtsCompanyID.Value + "','" + txtIntpCompanyID.Value + "','" + txtServerIDSelected.Value + "','" + Session["ClsTypeUserID"].ToString() + "' ";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Mirroring has been successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mirroring has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mirroring has been failed (" + sErr + ")</div>";
                    }

                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> NoSN can not be Mirroring</div>";
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mirroring has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";

                if (txtVehicleIDDelete.Value.ToUpper().Trim() != "")
                {
                    strSQL = "sp_vehicle_mirroring_unselected '" + txtVehicleIDDelete.Value + "','" + txtNoSNDelete.Value + "','" + txtVtsCompanyID.Value + "','" + txtIntpCompanyID.Value + "','" + txtServerIDDelete.Value + "','" + Session["ClsTypeUserID"].ToString() + "' ";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridViews(GridView2, "sp_list_vehicle_mirroring", txtCustID.Value, txtSearch.Text.Trim(), "RecListMirroringVehicle", LblPagingA);
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> UnMirroring has been successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> UnMirroring has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> UnMirroring has been failed (" + sErr + ")</div>";
                    }

                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> NoSN can not be UnMirroring</div>";
                }



            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> UnMirroring has been failed (" + ex.Message + ")</div>";
            }
        }

    }
}