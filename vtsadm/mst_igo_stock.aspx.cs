using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_stock : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_stock '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListDeviceFieldSort"] = "autoid";
                ViewState["RecListDeviceDirSort"] = "DESC";
                Session["RecListDevice"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGOSTOCKDEVICE"))
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
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicegroup");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicetype");
                ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_vendor");
                ClType.Open_Combos(CmbSourceDevice, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_source");
                ClType.Open_Combos(CmbServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_server");

                txtDeviceID.Text = "";
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtNoSN.Text = "";
                txtGMT.Text = "";
                txtDate.Text = "";
                CmbSourceDevice.SelectedValue = "[Select]";
                CmbVendorID.SelectedValue = "[Select]";
                CmbServer.SelectedValue = "[Select]";

                LblStockID.InnerHtml = "";
                txtStockIDDelete.Value = "";
                txtStatusDelete.Value = "";

                txtVendorAddress.Text = "";


                //gsm
                ClType.Open_Combos(CmbProviderID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_gsm_provider");
                ClType.Open_Combos(CmbSourceGSM, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_gsm_source");
                txtGSMID.Text = "";
                txtMSIDN.Text = "";
                txtDate.Text = "";
                CmbProviderID.SelectedValue = "[Select]";

                //LblGsmID.InnerHtml = "";
                //txtGsmIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; Int32 intAff1 = 0; String strSQL = ""; String strSQL1 = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtNoSN.Text.Trim() != "")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbVendorID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (CmbSourceDevice.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (CmbServer.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        if (txtMSIDN.Text.Trim() != "")
                                        {
                                            if (CmbProviderID.SelectedItem.Value.Trim() != "")
                                            {
                                                strSQL = "usp_retail_insert_stock '" + txtNoSN.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "','" + txtDate.Text.Trim() + "','" + CmbSourceDevice.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + txtMSIDN.Text.Trim() + "','" + CmbProviderID.SelectedItem.Value.ToString() + "','" + txtDateArival.Text.Trim() + "','" + CmbSourceGSM.SelectedItem.Value.Trim() + "','','" + Session["ClsTypeUserID"].ToString() + "'";

                                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                                {
                                                    if (intAff > 0)
                                                    {
                                                        clear();
                                                        Open_GridView();
                                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been save successfully!</div>";
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
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select provider</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill MSIDN</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select server</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select source</div>";
                                }
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
                else
                {
                    if (txtDeviceID.Text.Trim() != "")
                    {
                        if (CmbVendorID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbSourceDevice.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (CmbServer.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (txtGSMID.Text.Trim() != "")
                                    {
                                        strSQL = "sp_update_igo_device '" + txtDeviceID.Text.Trim() + "','" + txtNoSN.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "','" + txtDate.Text.Trim() + "','" + CmbSourceDevice.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        strSQL1 = "sp_update_igo_gsm '" + txtGSMID.Text.Trim() + "','" + txtMSIDN.Text.Trim() + "','" + CmbProviderID.SelectedItem.Value.ToString() + "','" + txtDate.Text.Trim() + "','" + CmbSourceGSM.SelectedItem.Value.Trim() + "','','" + Session["ClsTypeUserID"].ToString() + "'";

                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been update successfully!</div>";
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update device has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update device has been failed (" + sErr + ")</div>";
                                        }
                                        if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr))
                                        {
                                            if (intAff1 > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> GSM has been update successfully!</div>";
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update GSM has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update GSM has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select server</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select source</div>";
                            }
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
                string sAutoid = ""; string sNoSN = ""; string sGSM = ""; string sSN_Model = ""; string sGenDate = "";
                string sGMT = ""; string sVendorAddress = ""; string sStatus = ""; string sSourceID = ""; string sServerID = ""; string sDateArrival = "";
                sAutoid = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sSN_Model = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sNoSN = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sGSM = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sGenDate = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE" && sStatus.ToUpper().Trim() != "IS")
                        {
                            txtDeviceID.Text = sAutoid;
                            txtNoSN.Text = sNoSN;
                            txtGMT.Text = sGMT;
                            txtDate.Text = sDateArrival;
                            CmbSourceDevice.SelectedValue = sSourceID;
                            CmbServer.SelectedValue = sServerID;
                            txtVendorAddress.Text = sVendorAddress;
                            txtDeviceID.Attributes.Add("disabled", "disabled");
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited or deleted, (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDevice"], LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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

        protected void CmbDeviceTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceTypeID.SelectedItem.Value.ToString(), "sp_list_device_vendor");
                txtVendorAddress.Text = "";
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbDeviceGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_device_devicetype");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbVendorID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                setAddress(txtVendorAddress, "sp_get_device_address_vendor '" + CmbVendorID.SelectedItem.Value.ToString() + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void setAddress(TextBox txtBox, string strSQL, string sDBConn)
        {
            try
            {
                Recordset Rec = new Recordset();
                txtBox.Text = "";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    txtBox.Text = Rec.Fields(0);
                }
            }
            catch (Exception)
            {
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

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
                //Open_GridView();
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
                if (txtStockIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "1")
                    {
                        strSQL = "sp_delete_igo_stock'" + txtStockIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be deleted, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListDevice"], ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString(), e.SortExpression);
                ViewState["RecListDeviceFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListDeviceDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
    }
}
