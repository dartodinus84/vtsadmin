using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_assign_technician : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public job_assign_technician()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBASSIGN"))
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
                txtJobID.Value = "";
                txtRegDate2.Value = "";
                txtJobCustName.Value = "";
                txtJobCustID.Value = "";
                txtScheduleDate2.Value = "";
                txtPoID.Value = "";

                txtAssignID.Text = "";
                txtRegDate.Value = "";
                txtRemark.Text = "";

                txtAreaID.Value = "";

                txtSearch.Value = "";
                txtJobIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Text = "Submit";
                Button2.Attributes.Remove("disabled");
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdAddTraining.Visible = false;
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
                string strSQL = "sp_list_header_job_assign '" + txtSearch.Value.Trim() + "'";
                ViewState["RecAssignJobOrderHeaderFieldSort"] = "AssignId";
                ViewState["RecAssignJobOrderHeaderDirSort"] = "DESC";
                Session["RecAssignJobOrderHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecAssignJobOrderHeaderFieldSort"].ToString(), ViewState["RecAssignJobOrderHeaderDirSort"].ToString());
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
                string strSQL = "sp_list_job_order_assign_detail '" + txtAssignID.Text.Trim() + "'";
                ViewState["RecAssignJobOrderFieldSort"] = "JobID";
                ViewState["RecAssignJobOrderDirSort"] = "DESC";
                Session["RecAssignJobOrder"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecAssignJobOrderFieldSort"].ToString(), ViewState["RecAssignJobOrderDirSort"].ToString());
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecAssignJobOrder"], LblPagingDetail, ViewState["RecAssignJobOrderFieldSort"].ToString(), ViewState["RecAssignJobOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";

                if (txtJobID.Value.Trim() != "")
                {
                    if (txtRegDate.Value.Trim() != "")
                    {
                        Int32 intAff = 0; String strSQL = ""; string sErr = "";
                        ClsType ClType = new ClsType();
                        ExecCommand ec = new ExecCommand();
                        strSQL = "sp_insert_job_order_assign_header '" + txtJobID.Value.Trim() + "','" + txtAreaID.Value.Trim() + "','" + txtJobCustID.Value.Trim() + "','" + txtPoID.Value.Trim() + "','" + txtRegDate.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        string a = sErr;
                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            try
                            {
                                if (!sErr.ToLower().Contains("duplicate"))
                                {
                                    txtAssignID.Text = sErr.Trim();
                                    Session["ClsAssignIDAssign"] = txtAssignID.Text.Trim();
                                    Session["ClsJobIDAssign"] = txtJobID.Value.Trim();
                                    Session["ClsJobCustIDAssign"] = txtJobCustID.Value.Trim();
                                    Session["ClsAreaID"] = txtAreaID.Value.Trim();

                                    CmdCreate.Visible = false;
                                    if (txtJobID.Value.Trim().Contains("TRO"))
                                    {
                                        CmdAddDetail.Visible = false;
                                        CmdAddTraining.Visible = true;
                                    }
                                    else if (txtJobID.Value.Trim().Contains("JOB"))
                                    {
                                        CmdAddDetail.Visible = true;
                                        CmdAddTraining.Visible = false;
                                    }
                                    CmdSubmit.Visible = true;
                                    CmdLoad.Visible = true;

                                    //CmdCreate.Visible = false;
                                    //CmdAddDetail.Visible = true;
                                    //CmdSubmit.Visible = true;
                                    //CmdLoad.Visible = true;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create Job Order Assign Header has been failed (" + sErr + ")</div>";
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create Job Order Assign Header has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Register Date</div>";
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
                ExecCommand ec = new ExecCommand();
                if (txtJobID.Value.Trim() != "")
                {
                    if (txtAssignID.Text.Trim() != "")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            strSQL = "sp_submit_job_order_assign '" + txtAssignID.Text.Trim() + "','" + txtJobID.Value.Trim() + "','" + txtAreaID.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {

                                if (intAff > 0)
                                {
                                    string assignid = txtAssignID.Text.Trim();
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job order has been successfully</div>";
                                    sendTelegram(assignid);
                                    Session["ClsAssignIDAssign"] = "";
                                    Session["ClsAreaID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdAddTraining.Visible = false;
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
                            strSQL = "sp_update_job_order_assign '" + txtAssignID.Text.Trim() + "','" + txtJobID.Value.Trim() + "','" + txtRegDate.Value.Trim() + "','" + txtJobCustID.Value.Trim() + "','" + txtPoID.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            string assignid = txtAssignID.Text.Trim();
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job order has been successfully</div>";
                                    sendTelegram(assignid);
                                    Session["ClsAssignIDAssign"] = "";
                                    Session["ClsAreaID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdAddTraining.Visible = false;
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecAssignJobOrderHeader"], LblPagingHeader, ViewState["RecAssignJobOrderHeaderFieldSort"].ToString(), ViewState["RecAssignJobOrderHeaderDirSort"].ToString());
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
                    for (int i = 8; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[6].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";
                    for (int i = 8; i <= 13; i++)
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
                if (txtStatusDelete.Value != "" && txtAssignIDDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_assign '" + txtAssignIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
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
                if (txtAssignID.Text.Trim() != "" && txtSeqDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_job_order_assign_detail '" + txtAssignID.Text.Trim() + "'," + txtSeqDelete.Value.Trim() + "";
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
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDeleteDetail");
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + e.Row.Cells[2].Text.ToString() + "'); return false;";
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
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sJobID = ""; string sAssignID = "";
                string sRegDate = ""; string sBillableID = ""; string sSchDate = ""; string sRemark = "";
                string sStatus = ""; string sIsMigration = ""; string sBillableDesc = ""; string sPoID = ""; string sAreaID = ""; string sAreaName = "";

                sAssignID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sJobID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sRegDate = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sAreaName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                string rawPoID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sPoID = (rawPoID == "&nbsp;" ? "" : rawPoID);
                sAreaID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtAssignID.Text = ClType.CheckNbsp(sAssignID);
                            txtRegDate.Value = sRegDate;

                            txtJobID.Value = sJobID;
                            txtJobCustID.Value = sCustID;
                            txtRegDate.Value = sRegDate;
                            txtJobCustName.Value = sFullName;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);
                            txtPoID.Value = sPoID;

                            txtAreaID.Value = ClType.CheckNbsp(sAreaID);
                            txtAreaName.Value = ClType.CheckNbsp(sAreaName);

                            Session["ClsAssignIDAssign"] = txtAssignID.Text.Trim();
                            Session["ClsJobIDAssign"] = txtJobID.Value.Trim();
                            Session["ClsJobCustIDAssign"] = txtJobCustID.Value.Trim();
                            Session["ClsAreaID"] = txtAreaID.Value.Trim();

                            Open_GridView();
                            Button2.Style.Add("disabled", "disabled");
                            CmdCreate.Visible = false;
                            if (txtJobID.Value.Trim().Contains("TRO"))
                            {
                                CmdAddDetail.Visible = false;
                                CmdAddTraining.Visible = true;
                            }
                            else if (txtJobID.Value.Trim().Contains("JOB"))
                            {
                                CmdAddDetail.Visible = true;
                                CmdAddTraining.Visible = false;
                            }
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecAssignJobOrder"], ViewState["RecAssignJobOrderHeaderFieldSort"].ToString(), ViewState["RecAssignJobOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecAssignJobOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecAssignJobOrderDirSort"] = sNewDirSort;
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecAssignJobOrderHeader"], ViewState["RecAssignJobOrderHeaderFieldSort"].ToString(), ViewState["RecAssignJobOrderHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecAssignJobOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecAssignJobOrderHeaderDirSort"] = sNewDirSort;
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
        private void sendTelegram(string assignid)
        {

            try
            {
                var pageData = new job_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_assign_status '" + assignid + "','RG'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sAssignID = Rec.Fields("AssignID");
                        string sJoID = Rec.Fields("JobID");
                        string sCustomerName = Rec.Fields("Fullname");
                        string sPoID = Rec.Fields("POID");
                        string sRemark = Rec.Fields("Remark");
                        string sTechName = Rec.Fields("Name");
                        string sSchDate = Rec.Fields("SchDate");
                        string sMarketingName = Rec.Fields("MarketingName");

                        notifTelegram(sAssignID, sJoID, sCustomerName, sPoID, sRemark, sTechName, sSchDate, sMarketingName);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sAssignID, string sJoID, string sCustomerName, string sPoID, string sRemark, string sTechName, string sSchDate, string sMarketingName)
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
                        string text = BodyTelegram(sAssignID, sJoID, sCustomerName, sPoID, sRemark, sTechName, sSchDate, sMarketingName);
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
        private string BodyTelegram(string sAssignID, string sJoID, string sCustomerName, string sPoID, string sRemark, string sTechName, string sSchDate, string sMarketingName)
        {
            string msg = "";
            //msg += "<b>JOB ORDER ASSIGN</b>\r\n";
            //msg += "<b>Assign Number</b>\r\n";
            //msg += "<b>" + sAssignID + "</b>\r\n";
            //msg += "<b>Job Order Number</b>\r\n";
            //msg += "<b>" + sJoID + "</b>\r\n";
            //msg += "<b>Purcahse Order Number</b>\r\n";
            //msg += "<b>" + sPoID + "</b>\r\n";
            //msg += "<b>Customer Name</b>\r\n";
            //msg += "<b>" + sCustomerName + "</b>\r\n";
            //msg += "<b>Schedule Date</b>\r\n";
            //msg += "<b>" + sSchDate + "</b>\r\n";
            //msg += "<b>Technician Name</b>\r\n";
            //msg += "<b>" + sTechName + "</b>\r\n";
            //msg += "<b>Remarks</b>\r\n";
            //msg += "<b>" + sRemark + "</b>\r\n";


            msg += "<b>JOB ORDER ASSIGN</b>\r\n";
            msg += "<b>Assign Number</b>\r\n";
            msg += "<b>" + sAssignID + "</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Technician Name</b>\r\n";
            msg += "<b>" + sTechName + "</b>\r\n";
            msg += "<b>Marketing Name</b>\r\n";
            msg += "<b>" + sMarketingName + "</b>\r\n";
            msg += "<b>Remarks </b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            return msg;
        }
        protected void Open_GridViewTelegram()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_get_telegram '" + txtJobID.Value.Trim() + "'";
                Session["RecListCustomerServer"] = ClType.Open_GridView(GridView11, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingTelegram);
            }
            catch (Exception ex)
            {

            }
        }
    }
}