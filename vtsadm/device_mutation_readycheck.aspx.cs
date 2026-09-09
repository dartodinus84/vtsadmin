using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_mutation_readycheck : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListDeviceMutationTechnicianFieldSort";
        string sViewStateDirSort = "RecListDeviceMutationTechnicianDirSort";
        string sSessionRecList = "RecListDeviceMutationTechnician";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_device_mutation_readycheck '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "'";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTDEVTECH"))
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
                txtDeviceID.Value = "";
                txtNoSN.Text = "";
                txtVendorName.Text = "";
                txtDeviceTypeDesc.Text = "";
                txtSourceName.Text = "";

                txtTdwID.Text = "";
                txtWarehouseID.Text = "";
                txtWarehouseName.Text = "";
                txtWarehouseAddress.Text = "";
                txtWarehouseBranchName.Text = "";
                txtTechnicianID.Value = "";
                txtEmployeeNo.Text = "";
                txtName.Text = "";
                txtTechnicianBranchName.Text = "";
                txtRemark.Text = "";

                txtNewTechnicianID.Value = "";
                txtNewEmployeeNo.Text = "";
                txtNewName.Text = "";
                txtNewTechnicianBranchName.Text = "";
                txtOldTdtID.Text = "";

                txtRemark.Text = "";

                CmdSubmit.Text = "Submit";
                Session["ClsTypeDeviceMutationTechnicianPicture"] = "";

                txtNewTechnicianID.Attributes.Remove("required");
                txtNewEmployeeNo.Attributes.Remove("required");
                txtNewName.Attributes.Remove("required");
                txtNewTechnicianBranchName.Attributes.Remove("required");
                txtOldTdtID.Attributes.Remove("required");

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
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtTdwID.Text.Trim() != "" && txtDeviceID.Value.Trim() != "" && txtWarehouseID.Text.Trim() != "" && txtTechnicianID.Value.Trim() != "")
                    {
                        strSQL = "sp_insert_device_mutation_technician_new '" + txtTdwID.Text.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeDeviceMutationTechnicianPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        //strSQL = "sp_insert_device_mutation_technician '" + txtTdwID.Text.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device mutation technician has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device mutation technician has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device mutation technician has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Tdw ID</div>";
                    }
                }
                else if (CmdSubmit.Text.ToUpper() == "MUTATED")
                {
                    if (txtTechnicianID.Value.Trim() != txtNewTechnicianID.Value.Trim())
                    {
                        strSQL = "sp_mutation_device_mutation_technician_new '" + txtOldTdtID.Text.Trim() + "','" + txtTdwID.Text.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtNewTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeDeviceMutationTechnicianPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        //strSQL = "sp_mutation_device_mutation_technician '" + txtOldTdtID.Text.Trim() + "','" + txtTdwID.Text.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtNewTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Mutation technician has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation technician has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation technician has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Mutation technician has been failed (New technician should be different with existing technician)</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving or mutation technician has been failed!! (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sTdwID = ""; string sDeviceID = ""; string sWarehouseID = ""; string sTdtID = "";
                string sTechnicianID = ""; string sNoSN = ""; string sVendorName = ""; string sDeviceTypeDesc = ""; string sWarehouseName = "";
                string sWarehouseAddress = ""; string sWarehouseBranch = ""; string sEmployeeNo = "";
                string sTechnicianName = ""; string sTechnicianBranch = ""; string sRemark = ""; string sTechnicianBranchID = ""; string sSourceName = "";
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sNoSN = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sVendorName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sDeviceTypeDesc = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sWarehouseName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sTechnicianName = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                sTdtID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sTdwID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sWarehouseID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sWarehouseAddress = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sWarehouseBranch = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sTechnicianID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sEmployeeNo = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sTechnicianBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sTechnicianBranch = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sSourceName = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "MUTATION":
                        if (sTdtID != "")
                        {
                            if (sTechnicianID != "")
                            {
                                txtDeviceID.Value = sDeviceID;
                                txtNoSN.Text = sNoSN;
                                txtVendorName.Text = sVendorName;
                                txtDeviceTypeDesc.Text = sDeviceTypeDesc;
                                txtSourceName.Text = sSourceName;

                                txtTdwID.Text = sTdwID;
                                txtWarehouseID.Text = sWarehouseID;
                                txtWarehouseName.Text = sWarehouseName;
                                txtWarehouseAddress.Text = sWarehouseAddress;
                                txtWarehouseBranchName.Text = sWarehouseBranch;
                                txtTechnicianID.Value = sTechnicianID;
                                txtEmployeeNo.Text = sEmployeeNo;
                                txtName.Text = sTechnicianName;
                                txtTechnicianBranchName.Text = sTechnicianBranch;

                                txtNewTechnicianID.Value = "";
                                txtNewEmployeeNo.Text = "";
                                txtNewName.Text = "";
                                txtNewTechnicianBranchName.Text = "";

                                txtRemark.Text = "";
                                txtOldTdtID.Text = sTdtID;
                                txtNewTechnicianID.Attributes.Add("required", "required");
                                txtNewEmployeeNo.Attributes.Add("required", "required");
                                txtNewName.Attributes.Add("required", "required");
                                txtNewTechnicianBranchName.Attributes.Add("required", "required");
                                txtOldTdtID.Attributes.Add("required", "required");

                                Session["DeviceMutationTechnician"] = sTechnicianBranchID;

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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation technician has been failed!! (" + ex.Message + ")</div>";
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
                    for (int i = 8; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    e.Row.Cells[7].ToolTip = "Mutated";
                    
                }
            }
            catch (Exception ex)
            {

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