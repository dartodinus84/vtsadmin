using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Telegram.Bot;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_maint_auto : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public job_maint_auto()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBMAINT"))
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
                            Open_GridViewHeader();
                            Open_GridViewTelegram();
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
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtJobID.Text = "";
                txtRegDate.Value = "";
                ClType.Open_Combos(CmbBillAble, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_billable_type");
                CmbBillAble.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMigration, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_migration_job_order_header");
                CmbMigration.SelectedValue = "[Select]";
                txtScheduleDate.Value = "";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtJobIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Text = "Submit";
                Button2.Attributes.Remove("disabled");
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdUpload.Visible = false;
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
                string strSQL = "sp_list_header_job_maint '" + txtSearch.Value.Trim() + "'";
                ViewState["RecMaintJobOrderHeaderFieldSort"] = "JobID";
                ViewState["RecMaintJobOrderHeaderDirSort"] = "DESC";
                Session["RecMaintJobOrderHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecMaintJobOrderHeaderFieldSort"].ToString(), ViewState["RecMaintJobOrderHeaderDirSort"].ToString());
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
                string strSQL = "sp_list_job_order_maint_detail '" + txtJobID.Text.Trim() + "'";
                ViewState["RecMaintJobOrderFieldSort"] = "JobID";
                ViewState["RecMaintJobOrderDirSort"] = "DESC";
                Session["RecMaintJobOrder"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecMaintJobOrderFieldSort"].ToString(), ViewState["RecMaintJobOrderDirSort"].ToString());
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecMaintJobOrder"], LblPagingDetail, ViewState["RecMaintJobOrderFieldSort"].ToString(), ViewState["RecMaintJobOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtRegDate.Value.Trim() != "[Select]")
                    {
                        if (txtScheduleDate.Value.Trim() != "[Select]")
                        {
                            if (CmbBillAble.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (CmbMigration.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                    ExecCommand ec = new ExecCommand();
                                    strSQL = "sp_insert_job_order_maint_header_auto '" + txtCustID.Value.Trim() + "','" + txtRegDate.Value.Trim() + "','','" + CmbBillAble.SelectedItem.Value.Trim() + "','" + CmbMigration.SelectedItem.Value.Trim() + "','" + txtScheduleDate.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (!sErr.ToLower().Contains("duplicate"))
                                        {
                                            txtJobID.Text = sErr.Trim();
                                            Session["ClsJobIDMaint"] = txtJobID.Text.Trim();
                                            Session["ClsCustIDMaint"] = txtCustID.Value.Trim();
                                            CmdCreate.Visible = false;
                                            CmdAddDetail.Visible = true;
                                            CmdUpload.Visible = true;
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
                Int32 intAff = 0; String strSQL = ""; string sErr = "";

                string JOID = txtJobID.Text.Trim();
                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtJobID.Text.Trim() != "")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            strSQL = "sp_job_order_maint_update_status_close '" + txtJobID.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job order has been successfully</div>";
                                    sendTelegram(JOID);
                                    Session["ClsJobIDMaint"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdUpload.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job order has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job order has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            strSQL = "sp_update_job_order_maint '" + txtJobID.Text.Trim() + "','" + txtRegDate.Value.Trim() + "','" + txtCustID.Value.Trim() + "'," +
                                     "'','" + txtScheduleDate.Value.Trim() + "','" + CmbBillAble.SelectedItem.Value.Trim() + "'," +
                                     "'" + CmbMigration.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job order has been successfully</div>";
                                    sendTelegram(JOID);
                                    Session["ClsJobIDMaint"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdUpload.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job order has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job order has been failed (" + sErr + ")</div>";
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
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecMaintJobOrderHeader"], LblPagingHeader, ViewState["RecMaintJobOrderHeaderFieldSort"].ToString(), ViewState["RecMaintJobOrderHeaderDirSort"].ToString());
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
                    for (int i = 9; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                    for (int i = 9; i <= 13; i++)
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
                if (txtJobIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_maint '" + txtJobIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job order has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job order has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job irder has been failed (" + sErr + ")</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job irder has been failed (" + ex.Message + ")</div>";
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
                    strSQL = "sp_delete_job_order_maint_detail '" + txtJobID.Text.Trim() + "'," + txtSeqDelete.Value.Trim() + "";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job order detail can not be deleted, (" + ex.Message + ")</div>";
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDeleteDetail");
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + e.Row.Cells[1].Text.ToString() + "'); return false;";
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
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
                string sRegDate = ""; string sBillableID = ""; string sSchDate = ""; string sRemark = "";
                string sStatus = ""; string sIsMigration = ""; string sBillableDesc = "";

                sJobID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sRegDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sBillableDesc = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sIsMigration = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();

                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sBillableID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
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
                            txtRegDate.Value = sRegDate;
                            CmbBillAble.SelectedValue = ClType.CheckNbsp(sBillableID);
                            CmbMigration.SelectedValue = ClType.CheckNbsp(sIsMigration);
                            txtScheduleDate.Value = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Session["ClsJobIDMaint"] = txtJobID.Text.Trim();
                            Session["ClsCustIDMaint"] = txtCustID.Value.Trim();
                            Open_GridView();
                            Button2.Style.Add("disabled", "disabled");
                            CmdCreate.Visible = false;
                            CmdAddDetail.Visible = true;
                            CmdUpload.Visible = true;
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
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecMaintJobOrder"], ViewState["RecMaintJobOrderFieldSort"].ToString(), ViewState["RecMaintJobOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecMaintJobOrderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecMaintJobOrderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecMaintJobOrderHeader"], ViewState["RecMaintJobOrderHeaderFieldSort"].ToString(), ViewState["RecMaintJobOrderHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecMaintJobOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecMaintJobOrderHeaderDirSort"] = sNewDirSort;
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
        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendTelegram(string joid)
        {

            try
            {
                var pageData = new job_maint();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_maint_status '" + joid + "','RG'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("JobID");
                        string sCustomerName = Rec.Fields("Fullname");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sPoliceNo = Rec.Fields("policeno");
                        string sMSIDN = Rec.Fields("nosn");
                        string sGSM = Rec.Fields("msidn");
                        string sMaintTypeDesc = Rec.Fields("MaintTypeDesc");

                        notifTelegram(sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sMaintTypeDesc);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sMaintTypeDesc)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";
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
                            apitoken = RecApi.Fields("ParValue");
                        }

                        string strSQLtelegram3 = "sp_list_par_global 'TelegramUrl'";
                        Recordset RecUrl = new Recordset();
                        RecUrl.Open(strSQLtelegram3, Session["ClsTypeDBConnStringSQL"].ToString());
                        if (RecUrl.RecordCount() > 0)
                        {
                            url = RecUrl.Fields("ParValue");
                        }

                        string urlString = url;
                        string apiToken = apitoken;
                        string chatId = sChatID.ToString();
                        string text = BodyTelegram(sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sMaintTypeDesc);
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
        private string BodyTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM,string sMaintTypeDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " CREATE</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Police Number</b>\r\n";
            msg += "<b>" + sPoliceNo + "</b>\r\n";
            msg += "<b>Device Number</b>\r\n";
            msg += "<b>" + sMSIDN + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + sGSM + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
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