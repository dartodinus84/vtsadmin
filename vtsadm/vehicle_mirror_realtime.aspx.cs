using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_mirror_realtime : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_mirror_realtime '" + txtTvaID.Text.ToString() + "','" + txtSearch.Text.Trim() + "'";

                ViewState["RecListVehicleMirrorRealtimeFieldSort"] = "VehicleID";
                ViewState["RecListVehicleMirrorRealtimeDirSort"] = "DESC";
                Session["RecListVehicleMirrorRealtime"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListVehicleMirrorRealtimeFieldSort"].ToString(), ViewState["RecListVehicleMirrorRealtimeDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETVEHUPLINE"))
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

        protected void clear()
        {
            ClsType ClType = new ClsType();
            ClType.Open_Combos(CmbMirrorServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_mirror_realtime_server");

            txtCustID.Value = "";
            txtCustFullName.Text = "";
            txtCustCustTypeDesc.Text = "";
            txtCustBranchName.Text = "";
            txtVehicleID.Value = "";
            txtVehicleDesc.Text = "";
            txtPoliceNo.Text = "";
            txtAssetNo.Text = "";
            txtTvaID.Text = "";

            LblAutoID.InnerHtml = "";
            txtAutoIDDelete.Value = "";
            txtVehicleIDDelete.Value = "";

            CmbMirrorServer.SelectedValue = "[Select]";

            //Debug.WriteLine("OK");

        }

        protected void CmdLoadVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("TESTING " + txtTvaID.Text.ToString());
                Open_GridViews(GridView2, "sp_list_mirror_realtime", txtTvaID.Text.ToString(), "", "RecListVehicleMirrorRealtime", LblPaging);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleMirrorRealtime"], LblPaging, ViewState["RecListVehicleMirrorRealtimeFieldSort"].ToString(), ViewState["RecListVehicleMirrorRealtimeDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListVehicleMirrorRealtime"], ViewState["RecListVehicleMirrorRealtimeFieldSort"].ToString(), ViewState["RecListVehicleMirrorRealtimeDirSort"].ToString(), e.SortExpression);
                ViewState["RecListVehicleMirrorRealtimeFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListVehicleMirrorRealtimeDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    /**
                    for (int i = 14; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    */
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                    
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /**
                    for (int i = 14; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                     */

                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                   

                    e.Row.Cells[6].ToolTip = "Delete";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[3].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'); return false;";

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sDeviceID = ""; string sNoSN = ""; string sVendorID = ""; string sDeviceTypeID = ""; string sDeviceGroupID = "";
                string sGMT = ""; string sVendorAddress = ""; string sStatus = ""; string sSourceID = ""; string sServerID = ""; string sDateArrival = "";
                string sIsMobileID = "";
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sNoSN = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sGMT = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sDateArrival = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();

                sVendorID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sVendorAddress = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sDeviceGroupID = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sSourceID = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sServerID = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sIsMobileID = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE" && sStatus.ToUpper().Trim() != "IS")
                        {
                            /**
                            txtDeviceID.Text = sDeviceID;
                            CmbDeviceGroupID.SelectedValue = sDeviceGroupID;
                            ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_device_devicetype");
                            CmbDeviceTypeID.SelectedValue = sDeviceTypeID;
                            ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceTypeID.SelectedItem.Value.ToString(), "sp_list_device_vendor");
                            CmbVendorID.SelectedValue = sVendorID;
                            txtNoSN.Text = sNoSN;
                            txtGMT.Text = sGMT;
                            txtDate.Text = sDateArrival;
                            CmbSource.SelectedValue = sSourceID;
                            CmbServer.SelectedValue = sServerID;
                            txtVendorAddress.Text = sVendorAddress;
                            txtDeviceID.Attributes.Add("disabled", "disabled");
                            if (sIsMobileID == "" || sIsMobileID == "&nbsp;" || sIsMobileID == "0") { sIsMobileID = "[Select]"; }
                            CmbIsMobile.SelectedValue = sIsMobileID;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                            */
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited or deleted, (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtVehicleID.Value.Trim() != "")
                    {
                        if (txtTvaID.Text.Trim() != "")
                        {
                            if (CmbMirrorServer.SelectedItem.Value.Trim() != "[Select]")
                            {

                                /**
                                strSQL = "sp_insert_device '" + txtNoSN.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "','" + txtDate.Text.Trim() + "','" + CmbSource.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + CmbIsMobile.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {

                                        clear();
                                        Open_GridView();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been save successfully!</div>";
                                        //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                        //UpPnl.Update();
                                        //insert audit trails
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device has been failed</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save device has been failed (" + sErr + ")</div>";
                                }
                                */

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vendor</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select device type</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill No SN</div>";
                    }
                }
                
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtAutoIDDelete.Value.Trim() != "")
                {
                    if (txtVehicleIDDelete.Value.ToUpper().Trim() != "")
                    {
                        strSQL = "sp_delete_mirror_realtime '" + txtAutoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                //clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Mirror Realtime has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Mirror Realtime has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Mirror Realtime has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mirror Realtime can not be deleted, vehicle id " + txtVehicleIDDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Mirror Realtime has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }
        protected void GridView2_RowDeleting(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

       
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sTvaID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTvaID + "','" + sSearch + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }


        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();

               
                Open_GridViews(GridView2, "sp_list_mirror_realtime", txtTvaID.Text.ToString(),"", "RecListVehicleMirrorRealtime", LblPaging);
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

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
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