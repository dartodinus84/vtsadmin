using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class cash_out : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_cash_out '" + txtSearch.Text.Trim() + "'";
                ViewState["RecCashOutHeaderFieldSort"] = "CashOutID";
                ViewState["RecCashOutHeaderDirSort"] = "DESC";
                Session["RecCashOutHeader"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecCashOutHeaderFieldSort"].ToString(), ViewState["RecCashOutHeaderDirSort"].ToString());

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCASHOUT"))
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

                CmbType.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_cash_out_type");

                CmbPayment.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbPayment, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_cash_out_payment");

                CmbBranch.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbBranch, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_cash_out_branch");

                CmbEmploye.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbEmploye, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_cash_out_employe");

                txtAmount.Text = "0";
                txtScheduleDate.Text = "";
                txtRemark.Text = "";

                LblCashOutID.InnerHtml = "";
                txtCashOutIDDelete.Value = "";
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
        protected void CmdSumbit_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (CmbType.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbPayment.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbEmploye.SelectedItem.Value.Trim() != "[Select]" && CmbBranch.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (txtAmount.Text.Trim() != "0" || txtAmount.Text.Trim() != "")
                                {
                                    if (txtScheduleDate.Text.Trim() != "")
                                    {
                                        strSQL = "sp_insert_cash_out '" + CmbType.SelectedItem.Value.Trim() + "','" + CmbPayment.SelectedItem.Value.Trim() + "','" + CmbBranch.SelectedItem.Value.Trim() + "','" + CmbEmploye.SelectedItem.Value.Trim() + "','" + txtAmount.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (!sErr.ToLower().Contains("duplicate"))
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Request Cash Out has been save successfully!</div>";

                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Request Cash Out has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Cash Out has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Schedule Date</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Amount </div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Employe / Branch </div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Payment</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Type</div>";
                    }
                }
                else
                {
                    if (CmbType.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbPayment.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbEmploye.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (txtAmount.Text.Trim() != "0" || txtAmount.Text.Trim() != "")
                                {
                                    if (txtScheduleDate.Text.Trim() != "")
                                    {
                                        strSQL = "sp_update_cash_out '" + txtCashOutID.Text.Trim() + "','" + CmbType.SelectedItem.Value.Trim() + "','" + CmbPayment.SelectedItem.Value.Trim() + "','" + CmbEmploye.SelectedItem.Value.Trim() + "','" + txtAmount.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Cash Out has been update successfully!</div>";
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Request Cash Out has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Cash Out has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Schedule Date</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Amount </div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Employe</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Payment</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Type</div>";
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
                string sCashOutID = ""; string sCashOutType = ""; string sCashOutPayment = ""; string sEmployerName = ""; string sAmout = "";
                string sStatus = ""; string sRemarks = ""; string sSchDate = "";
                string sCashOutTypeID = ""; string sCashOutPaymentID = ""; string sEmployerNameID = "";
                string sBranchName = ""; string sBranchID = "";

                sCashOutID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sCashOutType = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sCashOutPayment = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sEmployerName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                sAmout = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();

                sRemarks = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sCashOutTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sCashOutPaymentID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sEmployerNameID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "CL" && sStatus.ToUpper().Trim() != "DE")
                        {
                            txtCashOutID.Text = sCashOutID;
                            txtCashOutID.Attributes.Add("disabled", "disabled");
                            if (sCashOutTypeID == "" || sCashOutTypeID == "&nbsp;" || sCashOutTypeID == "0") { sCashOutTypeID = "[Select]"; }
                            CmbType.SelectedValue = sCashOutTypeID;
                            if (sCashOutPaymentID == "" || sCashOutPaymentID == "&nbsp;" || sCashOutPaymentID == "0") { sCashOutPaymentID = "[Select]"; }
                            CmbPayment.SelectedValue = sCashOutPaymentID;
                            if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                            CmbBranch.SelectedValue = sBranchID;

                            ClType.Open_Combos(CmbEmploye, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranch.SelectedItem.Value.ToString(), "sp_list_cash_out_employe");
                            CmbEmploye.SelectedValue = sEmployerNameID;

                            txtAmount.Text = sAmout;
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> CashOut can not be edited or deleted, (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListCashOutHeader"], LblPaging, ViewState["RecListCashOutHeaderFieldSort"].ToString(), ViewState["RecListCashOutHeaderDirSort"].ToString());
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
                    for (int i = 8; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[8].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[9].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";

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
                if (txtCashOutIDDelete.Value.Trim() != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_cash_out '" + txtCashOutIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Cash Out has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Cash Out has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Cash Out has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Cash Out can not be deleted, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Cash Out has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListCashOutHeader"], ViewState["RecListCashOutHeaderFieldSort"].ToString(), ViewState["RecListCashOutHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecListCashOutHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListCashOutHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void CmbBranch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbEmploye, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranch.SelectedItem.Value.ToString(), "sp_list_cash_out_employe");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
    }
}
