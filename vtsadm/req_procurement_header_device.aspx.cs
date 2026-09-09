using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_procurement_header_device : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_procurement_device'" + txtSearch.Text.Trim() + "'";
                ViewState["RecProcurementHeaderFieldSort"] = "ProcurementID";
                ViewState["RecProcurementHeaderDirSort"] = "DESC";
                Session["RecProcurementHeader"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecProcurementHeaderFieldSort"].ToString(), ViewState["RecProcurementHeaderDirSort"].ToString());

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUREQDEVICE"))
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
                ClsType ClType = new ClsType();

                CmbProcurement.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbProcurement, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_procurement_type");
                CmbBranchID.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbBranchID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_technician_branch");
                CmbWarehouse.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_procurement");
                CmbTechnician.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_technician_procurement");
                CmbTypeDevice.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbTypeDevice, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicetype");
                txtQtyDevice.Text = "0";
                txtQty12.Text = "0";
                txtQty24.Text = "0";
                txtScheduleDate.Text = "";

                LblProcurementID.InnerHtml = "";
                txtProcurementIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";

                CmbWarehouse.Attributes.Add("style", "display:none");
                LblCmbWarehouse.Attributes.Add("style", "display:none");

                CmbTechnician.Attributes.Add("style", "display:none");
                LblCmbTechnician.Attributes.Add("style", "display:none");

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
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSumbit_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (CmbProcurement.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (txtQtyDevice.Text.Trim() != "")
                        {
                            if (txtQty12.Text.Trim() != "")
                            {
                                if (txtQty24.Text.Trim() != "")
                                {
                                    if (CmbBranchID.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        strSQL = "sp_insert_procurement '" + CmbProcurement.SelectedItem.Value.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbWarehouse.SelectedItem.Value.Trim() + "','" + CmbTechnician.SelectedItem.Value.Trim() + "','" + CmbTypeDevice.SelectedItem.Value.ToString() + "','" + txtQtyDevice.Text.Trim() + "','[Select]','0','" + txtQty12.Text.Trim() + "','" + txtQty24.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (!sErr.ToLower().Contains("duplicate"))
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Request Procurement has been save successfully!</div>";

                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Request Procurement has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Procurement has been failed (" + sErr + ")</div>";
                                        }


                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Branch</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity relay 24 Volt </div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity relay 12 Volt</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity Device</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Procurement Type</div>";
                    }
                }
                else
                {
                    if (CmbProcurement.SelectedItem.Value.Trim() != "[Select]")
                    {

                        if (txtQtyDevice.Text.Trim() != "")
                        {
                            if (txtQty12.Text.Trim() != "")
                            {
                                if (txtQty24.Text.Trim() != "")
                                {
                                    if (CmbWarehouse.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        strSQL = "sp_update_procurement '" + txtProcurementID.Text.Trim() + "','" + CmbProcurement.SelectedItem.Value.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbWarehouse.SelectedItem.Value.Trim() + "','" + CmbTechnician.SelectedItem.Value.Trim() + "','" + CmbTypeDevice.SelectedItem.Value.ToString() + "','" + txtQtyDevice.Text.Trim() + "','[Select]','0','" + txtQty12.Text.Trim() + "','" + txtQty24.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Procurement has been update successfully!</div>";
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Request Procurement has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Procurement has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select warehouse</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity relay 24 Volt </div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity relay 12 Volt</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill quantity Device</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Procurement Type</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sProcurementID = ""; string sProcurementType = ""; string sWarehouseName = ""; string sWarehouseID = ""; string sDeviceTypeID = "";
                string sQty_Device = ""; string sProvider = ""; string sStatus = ""; string sQty_Gsm = ""; string sRemarks = ""; string sSchDate = "";
                string sDeviceTypeDesc = ""; string sProviderID = ""; string s24V = ""; string s12V = ""; string sTechID = ""; string sBranchID = "";
                string sTechName = "";

                sProcurementID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sProcurementType = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sWarehouseName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sTechName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();

                sDeviceTypeDesc = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sQty_Device = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sProvider = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sQty_Gsm = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();

                sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sProviderID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sWarehouseID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sRemarks = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                s12V = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                s24V = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sProcurementType = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sTechID = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();


                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "CL" && sStatus.ToUpper().Trim() != "DE")
                        {
                            txtProcurementID.Text = sProcurementID;
                            txtProcurementID.Attributes.Add("disabled", "disabled");
                            CmbProcurement.SelectedValue = sProcurementType;
                            if (sProcurementType.Equals("REQIN00001"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");
                                CmbTechnician.Attributes.Add("style", "display:block");
                                LblCmbTechnician.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;
                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;
                                if (sTechID == "" || sTechID == "&nbsp;" || sTechID == "0") { sTechID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechID;
                            }
                            else if (sProcurementType.Equals("REQIN00002"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");
                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;
                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;
                                if (sTechID == "" || sTechID == "&nbsp;" || sTechID == "0") { sTechID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechID;
                            }
                            else if (sProcurementType.Equals("REQIN00003"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");
                                CmbTechnician.Attributes.Add("style", "display:block");
                                LblCmbTechnician.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;
                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;
                                if (sTechID == "" || sTechID == "&nbsp;" || sTechID == "0") { sTechID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechID;
                            }
                            CmbTypeDevice.SelectedValue = sDeviceTypeID;
                            txtQtyDevice.Text = sQty_Device;
                            //CmbTypeGSM.SelectedValue = sProviderID;
                            //txtQtyGSM.Text = sQty_Gsm;
                            txtQty24.Text = s24V;
                            txtQty12.Text = s12V;
                            txtRemark.Text = sRemarks;
                            txtScheduleDate.Text = sSchDate;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Procurement can not be edited or deleted, (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListProcurementHeader"], LblPaging, ViewState["RecListProcurementHeaderFieldSort"].ToString(), ViewState["RecListProcurementHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 11; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[9].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 11; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[9].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[10].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "'); return false;";

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
                div_comment.InnerHtml = "";
                if (txtSearch.Text.Trim() == "")
                {
                    clear();
                }
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtProcurementIDDelete.Value.Trim() != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_procurement '" + txtProcurementIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Procurement has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Procurement has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Procurement has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Procurement can not be deleted, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Procurement has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListProcurementHeader"], ViewState["RecListProcurementHeaderFieldSort"].ToString(), ViewState["RecListProcurementHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecListProcurementHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListProcurementHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void CmbProcurement_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00001"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:block");
                    LblCmbTechnician.Attributes.Add("style", "display:block");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00002"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00003"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:block");
                    LblCmbTechnician.Attributes.Add("style", "display:block");

                }
                else
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");
                }

                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbBranchID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbWarehouseID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
    }
}
