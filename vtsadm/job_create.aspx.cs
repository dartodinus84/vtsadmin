using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using Telegram.Bot;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_create : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public job_create()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                //Session["ClsTypeMenuActive"] = "MNUDO";
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView();
                            Open_GridViewHeader();
                            Open_GridViewTelegram();
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
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtJobID.Text = "";
                txtRegDate.Text = "";
                txtPoID.Value = "";
                txtPONumber.Text = "";
                ClType.Open_Combos(CmbBillAble, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_billable_type");
                CmbBillAble.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMigration, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_migration_job_order_header");
                CmbMigration.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbArea, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_area_selectbox");
                CmbArea.SelectedValue = "[Select]";
                txtScheduleDate.Text = "";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtJobIDDelete.Value = "";
                txtPoIDDelete.Value = "";
                txtStatusDelete.Value = "";
                txtSeqDelete.Value = "";
                txtRemarkClose.Value = "";
                txtSeqClose.Value = "";
                Button2.Attributes.Remove("disabled");
                CmdSubmit.Text = "Submit";
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdLoad.Visible = false;
                CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_job_create '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreateJobOrderHeaderFieldSort"] = "JobID";
                ViewState["RecCreateJobOrderHeaderDirSort"] = "DESC";
                Session["RecCreateJobOrderHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreateJobOrderHeaderFieldSort"].ToString(), ViewState["RecCreateJobOrderHeaderDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_job_order_detail '" + txtJobID.Text.Trim() + "'";
                ViewState["RecCreateJobOrderFieldSort"] = "JobID";
                ViewState["RecCreateJobOrderDirSort"] = "ASC";
                Session["RecCreateJobOrder"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecCreateJobOrderFieldSort"].ToString(), ViewState["RecCreateJobOrderDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
                Open_GridView();
                Open_GridViewHeader();
                Open_GridViewTelegram();
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdAddDetail_Click(object sender, EventArgs e)
        {

        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateJobOrder"], LblPagingDetail, ViewState["RecCreateJobOrderFieldSort"].ToString(), ViewState["RecCreateJobOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtRegDate.Text.Trim() != "")
                    {
                        if (txtPoID.Value.Trim() != "")
                        {
                            if (txtScheduleDate.Text.Trim() != "")
                            {
                                if (CmbBillAble.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (CmbMigration.SelectedItem.Value.Trim() != "[Select]")
                                    {

                                        Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                        ExecCommand ec = new ExecCommand();
                                        strSQL = "sp_insert_job_order_header '" + txtCustID.Value.Trim() + "','" + txtRegDate.Text.Trim() + "','" + txtPoID.Value.Trim() + "','" + CmbBillAble.SelectedItem.Value.Trim() + "','" + CmbMigration.SelectedItem.Value.Trim() + "','" + CmbArea.SelectedItem.Value.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (!sErr.ToLower().Contains("duplicate"))
                                            {
                                                txtJobID.Text = sErr.Trim();
                                                Session["JobCreateJobID"] = txtJobID.Text.Trim();
                                                Session["JobCreatePoID"] = txtPoID.Value.Trim();
                                                CmdCreate.Visible = false;
                                                CmdAddDetail.Visible = true;
                                                CmdSubmit.Visible = true;
                                                CmdLoad.Visible = true;
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create job order header has been failed (" + sErr + ")</div>";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select migration type</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select maintenance type</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select schedule date</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in Po ID</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select register date</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select customer</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdLoad_Click(object sender, EventArgs e)
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                string jono = Convert.ToString(txtJobID.Text);
                string jodate = Convert.ToDateTime(txtScheduleDate.Text).ToString("dd/MM/yyyy");
                string cus = Convert.ToString(txtCustFullName.Text);
                string remarks = Convert.ToString(txtRemark.Text);
                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtJobID.Text.Trim() != "")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            strSQL = "sp_submit_job_order '" + txtJobID.Text.Trim() + "','" + txtPoID.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job order has been successfully</div>";
                                    Bot(jono, jodate, cus, remarks);
                                    Session["JobCreateJobID"] = "";
                                    Session["JobCreatePoID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job order has been failed (" + sErr + ")</div>";
                                }
                            }
                        }
                        else
                        {
                            strSQL = "sp_update_job_order '" + txtJobID.Text.Trim() + "','" + txtRegDate.Text.Trim() + "','" + txtCustID.Value.Trim() + "'," +
                                     "'" + txtPoID.Value.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + CmbBillAble.SelectedItem.Value.Trim() + "'," +
                                     "'" + CmbMigration.SelectedItem.Value.Trim() + "','" + CmbArea.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job order has been successfully</div>";
                                    Bot(jono, jodate, cus, remarks);
                                    Session["JobCreateJobID"] = "";
                                    Session["JobCreatePoID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job order has been failed (" + sErr + ")</div>";
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateJobOrderHeader"], LblPagingHeader, ViewState["RecCreateJobOrderHeaderFieldSort"].ToString(), ViewState["RecCreateJobOrderHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
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
                    for (int i = 10; i <= 16; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[8].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[9].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";
                    for (int i = 10; i <= 16; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtJobIDDelete.Value != "" && txtPoIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_create '" + txtJobIDDelete.Value.Trim() + "','" + txtPoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job order has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job order has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job order has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job order can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job order has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtJobID.Text.Trim() != "" && txtSeqDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_job_order_detail '" + txtJobID.Text.Trim() + "'," + txtSeqDelete.Value.Trim() + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job order detail has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job order detail has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete job order detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete job order detail has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdClose = (LinkButton)e.Row.Cells[7].FindControl("CmdCloseDetail");
                    CmdClose.OnClientClick = "confirmCloseDetail('" + e.Row.Cells[1].Text.ToString() + "'); return false;";

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDeleteDetail");
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + e.Row.Cells[1].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sJobID = "";
                string sRegDate = ""; string sPoID = ""; string sPoNo = ""; string sBillableID = ""; string sSchDate = ""; string sRemark = "";
                string sStatus = ""; string sIsMigration = ""; string sBillableDesc = ""; string sArea = "";

                sJobID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sRegDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sPoNo = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sBillableDesc = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sIsMigration = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sBillableID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sArea = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sPoID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtCustID.Value = sCustID;
                            txtCustFullName.Text = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;

                            txtJobID.Text = ClType.CheckNbsp(sJobID);
                            txtRegDate.Text = sRegDate;
                            txtPoID.Value = ClType.CheckNbsp(sPoID);
                            txtPONumber.Text = ClType.CheckNbsp(sPoNo);
                            CmbBillAble.SelectedValue = sBillableID;
                            CmbMigration.SelectedValue = ClType.CheckNbsp(sIsMigration);
                            CmbArea.SelectedValue = sArea;
                            txtScheduleDate.Text = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);
                            Session["JobCreateJobID"] = txtJobID.Text.Trim();
                            Session["JobCreatePoID"] = txtPoID.Value.Trim();
                            Open_GridView();
                            Button2.Style.Add("disabled", "disabled");
                            CmdCreate.Visible = false;
                            CmdAddDetail.Visible = true;
                            CmdSubmit.Visible = true;
                            CmdLoad.Visible = true;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesClose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtJobID.Text.Trim() != "" && txtSeqClose.Value.Trim() != "")
                {
                    strSQL = "sp_close_job_order_detail '" + txtJobID.Text.Trim() + "'," + txtSeqClose.Value.Trim() + ",'" + txtRemarkClose.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job order detail has been close successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing job order detail has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing job order detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing job order detail has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreateJobOrderHeader"], ViewState["RecCreateJobOrderHeaderFieldSort"].ToString(), ViewState["RecCreateJobOrderHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateJobOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreateJobOrderHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecCreateJobOrder"], ViewState["RecCreateJobOrderFieldSort"].ToString(), ViewState["RecCreateJobOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateJobOrderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreateJobOrderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView11_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sIsAttach = e.Row.Cells[4].Text.ToString();
                    if (sIsAttach == "1")
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = true;
                    }
                    else
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = false;
                    }
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Bot(string jono, string jodate, string cus, string remarks)
        {
            string apitoken = "";
            string url = "";
            try
            {
                string sChatID = "";
                for (int i = 0; i < GridView11.Rows.Count; i++)
                {
                    sChatID = GridView11.Rows[i].Cells[4].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView11.Rows[i].Cells[3].FindControl("Chk1");
                    if (ChkBox.Checked == true)
                    {
                        string strSQLtelegram2 = "sp_list_par_global 'TelegramApi'";
                        Recordset RecApi = new Recordset();
                        RecApi.Open(strSQLtelegram2, Session["ClsTypeDBConnStringSQL"].ToString());
                        if (RecApi.RecordCount() > 0)
                        {
                            apitoken += RecApi.Fields("ParValue");
                        }

                        string strSQLtelegram3 = "sp_list_par_global 'TelegramUrl'";
                        Recordset RecUrl = new Recordset();
                        RecUrl.Open(strSQLtelegram3, Session["ClsTypeDBConnStringSQL"].ToString());
                        if (RecUrl.RecordCount() > 0)
                        {
                            url += RecUrl.Fields("ParValue");
                        }

                        string urlString = url;
                        string apiToken = apitoken;
                        string chatId = sChatID.ToString();
                        string text = BodyTelegram(jono, jodate, cus, remarks);
                        urlString = String.Format(urlString, apiToken, chatId, text);

                        WebClient webclient = new WebClient();
                        webclient.DownloadString(urlString);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string jono, string jodate, string cus, string remarks)
        {
            string msg = "";
            msg += "<b>JOB ORDER NEW INSTALL</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Schedule</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                string sJoID = GridView2.Rows[i].Cells[0].Text.ToString();
                if (jono == sJoID)
                {
                    string sDeviceGroupDesc = GridView2.Rows[i].Cells[2].Text.ToString();
                    string sDeviceTypeDesc = GridView2.Rows[i].Cells[3].Text.ToString();
                    string sQuantity = GridView2.Rows[i].Cells[4].Text.ToString();
                    msg += "\r\n";
                    msg += "<b>Group Device</b>\r\n";
                    msg += "<b>" + sDeviceGroupDesc + "</b>\r\n";
                    msg += "<b>Type Device</b>\r\n";
                    msg += "<b>" + sDeviceTypeDesc + "</b>\r\n";
                    msg += "<b>Quantity Request</b>\r\n";
                    msg += "<b>" + sQuantity + "</b>\r\n";
                    msg += "\r\n";
                }
            }
            msg += "<b>Remarks</b>\r\n";
            msg += "<b>" + remarks + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            return msg;
        }
        protected void Open_GridViewTelegram()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_get_telegram '" + txtCustID.Value.Trim() + "'";
                Session["RecListCustomerServer"] = ClType.Open_GridView(GridView11, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingTelegram);
            }
            catch (Exception ex)
            {

            }
        }
    }
}