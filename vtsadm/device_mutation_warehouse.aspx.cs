using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_mutation_warehouse : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListDeviceMutationWarehouseFieldSort";
        string sViewStateDirSort = "RecListDeviceMutationWarehouseDirSort";
        string sSessionRecList = "RecListDeviceMutationWarehouse";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_device_mutation_warehouse '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "DeviceID";
                ViewState[sViewStateDirSort] = "DESC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTDEVWAR"))
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
                txtDeviceID.Value = "";
                txtNoSN.Text = "";
                txtVendorName.Text = "";
                txtDeviceTypeDesc.Text = "";
                txtSourceName.Text = "";
                txtWarehouseID.Value = "";
                txtWarehouseName.Text = "";
                txtWarehouseAddress.Text = "";
                txtWarehouseBranchName.Text = "";
                txtRemark.Text = "";

                txtNewWarehouseID.Value = "";
                txtNewWarehouseName.Text = "";
                txtNewWarehouseAddress.Text = "";
                txtNewWarehouseBranchName.Text = "";
                txtOldTdwID.Text = "";

                txtRemark.Text = "";

                LblTdwID.InnerHtml = "";
                txtTdwIDDelete.Value = "";
                txtDeviceIDDelete.Value = "";
                txtWareIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";

                txtNewWarehouseID.Attributes.Remove("required");
                txtNewWarehouseName.Attributes.Remove("required");
                txtNewWarehouseAddress.Attributes.Remove("required");
                txtNewWarehouseBranchName.Attributes.Remove("required");
                txtOldTdwID.Attributes.Remove("required");

                div_mutation.Attributes.Add("hidden", "hidden");
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
                div_comment.InnerHtml = "";
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtDeviceID.Value.Trim() != "" && txtWarehouseID.Value.Trim() != "")
                    {
                        strSQL = "sp_insert_device_mutation_warehouse '" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device mutation warehouse has been save successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device mutation warehouse has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device mutation warehouse has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Device ID and Warehouse ID</div>";
                    }
                }
                else if (CmdSubmit.Text.ToUpper() == "MUTATED")
                {
                    if (txtWarehouseID.Value.Trim()!=txtNewWarehouseID.Value.Trim())
                    {
                        strSQL = "sp_mutation_device_mutation_warehouse '" + txtOldTdwID.Text.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Value.Trim() + "','" + txtNewWarehouseID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Mutation warehouse has been save successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation warehouse has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation warehouse has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation warehouse has been failed (New warehouse should be different with existing warehouse)</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving or mutation warehouse has been failed!! (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sTdwID = "";string sDeviceID = "";string sNoSN = "";string sVendorName = "";string sDeviceTypeDesc = "";
                string sWarehouseID = "";string sWarehouseName = "";string sWarehouseAddress = "";string sWarehouseBranch = "";
                string sRemark = "";string sSourceName = "";
                
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sNoSN = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sVendorName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sDeviceTypeDesc = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sWarehouseName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                sTdwID = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sWarehouseID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sWarehouseAddress = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sWarehouseBranch = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sSourceName = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "MUTATION":
                        if (sTdwID != "")
                        {
                            if (sWarehouseID != "")
                            {
                                txtDeviceID.Value = sDeviceID;
                                txtNoSN.Text = sNoSN;
                                txtVendorName.Text = sVendorName;
                                txtDeviceTypeDesc.Text = sDeviceTypeDesc;
                                txtSourceName.Text = sSourceName;

                                txtWarehouseID.Value = sWarehouseID;
                                txtWarehouseName.Text = sWarehouseName;
                                txtWarehouseAddress.Text = sWarehouseAddress;
                                txtWarehouseBranchName.Text = sWarehouseBranch;

                                txtNewWarehouseID.Value = "";
                                txtNewWarehouseName.Text = "";
                                txtNewWarehouseAddress.Text = "";
                                txtNewWarehouseBranchName.Text = "";

                                txtRemark.Text = "";
                                txtOldTdwID.Text = sTdwID;
                                txtNewWarehouseID.Attributes.Add("required", "required");
                                txtNewWarehouseName.Attributes.Add("required", "required");
                                txtNewWarehouseAddress.Attributes.Add("required", "required");
                                txtNewWarehouseBranchName.Attributes.Add("required", "required");
                                txtOldTdwID.Attributes.Add("required", "required");

                                //Session["DeviceMutationTechnician"] = sTechnicianBranchID;

                                div_mutation.Attributes.Remove("hidden");
                                CmdSubmit.Text = "Mutated";
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation warehouse has been failed!! (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 8; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[6].ToolTip = "Mutated";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

                }
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
                if (txtTdwIDDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_device_mutation_warehouse '" + txtTdwIDDelete.Value.Trim() + "','" + txtDeviceIDDelete.Value.Trim() + "','" + txtWareIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device mutation warehouse has been remove successfully!</div>";
                        }
                        else
                        {
                            //txtError.Value = "Delete failed";
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation warehouse has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation warehouse has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch(Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation warehouse has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}