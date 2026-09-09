using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class gsm_mutation_technician : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListGsmMutationTechnicianFieldSort";
        string sViewStateDirSort = "RecListGsmMutationTechnicianDirSort";
        string sSessionRecList = "RecListGsmMutationTechnician";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_gsm_mutation_technician '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "GsmID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTGSMTECH"))
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
                txtGsmID.Value = "";
                txtMSIDN.Text = "";
                txtProviderName.Text = "";
                txtSourceName.Text = "";

                txtTgwID.Text = "";
                txtWarehouseID.Text = "";
                txtWarehouseName.Text = "";
                txtWarehouseAddress.Text = "";
                txtWarehouseBranchName.Text = "";
                txtTechnicianID.Value = "";
                txtEmployeeNo.Text = "";
                txtName.Text = "";
                txtTechnicianBranchName.Text = "";

                txtNewTechnicianID.Value = "";
                txtNewEmployeeNo.Text = "";
                txtNewName.Text = "";
                txtNewTechnicianBranchName.Text = "";
                txtOldTgtID.Text = "";

                txtRemark.Text = "";

                LblTgtID.InnerHtml = "";
                txtTgtIDDelete.Value = "";
                txtTgwIDDelete.Value = "";
                txtGsmIDDelete.Value = "";
                txtWareIDDelete.Value = "";
                txtTechIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
                Session["ClsTypeGsmMutationTechnicianPicture"] = "";

                txtNewTechnicianID.Attributes.Remove("required");
                txtNewEmployeeNo.Attributes.Remove("required");
                txtNewName.Attributes.Remove("required");
                txtNewTechnicianBranchName.Attributes.Remove("required");
                txtOldTgtID.Attributes.Remove("required");

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
                    if (txtTgwID.Text.Trim() != "" && txtGsmID.Value.Trim() != "" && txtWarehouseID.Text.Trim() != "" && txtTechnicianID.Value.Trim() != "")
                    {
					//dandy
                        strSQL = "sp_insert_gsm_mutation_technician_new '" + txtTgwID.Text.Trim() + "','" + txtGsmID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeGsmMutationTechnicianPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    //end
					    //strSQL = "sp_insert_gsm_mutation_technician '" + txtTgwID.Text.Trim() + "','" + txtGsmID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Gsm mutation technician has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving gsm mutation technician has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving gsm mutation technician has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Tgw ID</div>";
                    }
                }
                else if (CmdSubmit.Text.ToUpper() == "MUTATED")
                {
                    if (txtTechnicianID.Value.Trim() != txtNewTechnicianID.Value.Trim())
                    {
					//dandy
                        strSQL = "sp_mutation_gsm_mutation_technician_new '" + txtOldTgtID.Text.Trim() + "','" + txtTgwID.Text.Trim() + "','" + txtGsmID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtNewTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeGsmMutationTechnicianPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    //end
					    //strSQL = "sp_mutation_gsm_mutation_technician '" + txtOldTgtID.Text.Trim() + "','" + txtTgwID.Text.Trim() + "','" + txtGsmID.Value.Trim() + "','" + txtWarehouseID.Text.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + txtNewTechnicianID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
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
                string sTgwID = ""; string sGsmID = ""; string sWarehouseID = ""; string sTgtID = "";
                string sTechnicianID = ""; string sMSIDN = ""; string sProviderName = ""; string sWarehouseName = "";
                string sWarehouseAddress = ""; string sWarehouseBranch = ""; string sEmployeeNo = "";
                string sTechnicianName = ""; string sTechnicianBranch = ""; string sRemark = ""; string sTechnicianBranchID = "";
                string sSourceName = "";
                sGsmID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sMSIDN = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sProviderName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sWarehouseName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sTechnicianName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                sTgtID = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sTgwID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sWarehouseID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sWarehouseAddress = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sWarehouseBranch = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sTechnicianID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sEmployeeNo = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sTechnicianBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sTechnicianBranch = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sSourceName = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "MUTATION":
                        if (sTgtID != "")
                        {
                            if (sTechnicianID != "")
                            {
                                txtGsmID.Value = sGsmID;
                                txtMSIDN.Text = sMSIDN;
                                txtProviderName.Text = sProviderName;
                                txtSourceName.Text = sSourceName;

                                txtTgwID.Text = sTgwID;
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
                                txtOldTgtID.Text = sTgtID;
                                txtNewTechnicianID.Attributes.Add("required", "required");
                                txtNewEmployeeNo.Attributes.Add("required", "required");
                                txtNewName.Attributes.Add("required", "required");
                                txtNewTechnicianBranchName.Attributes.Add("required", "required");
                                txtOldTgtID.Attributes.Add("required", "required");

                                Session["GsmMutationTechnician"] = sTechnicianBranchID;

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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing gsm mutation technician has been failed!! (" + ex.Message + ")</div>";
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
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[6].ToolTip = "Mutated";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "','" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "','" + e.Row.Cells[13].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";
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
                if (txtTgtIDDelete.Value.Trim() != "" && txtTgwIDDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_gsm_mutation_technician '" + txtTgtIDDelete.Value.Trim() + "','" + txtTgwIDDelete.Value.Trim() + "','" + txtGsmIDDelete.Value.Trim() + "','" + txtWareIDDelete.Value.Trim() + "','" + txtTechIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Gsm mutation technician has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing gsm mutation technician has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing gsm mutation technician has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing gsm mutation technician has been failed (" + ex.Message + ")</div>";
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