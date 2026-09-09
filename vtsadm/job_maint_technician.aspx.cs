using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using vtsadm.App_Code;
using System.Net.Mail;

namespace vtsadm
{
    public partial class job_maint_technician : System.Web.UI.Page
    {
        public job_maint_technician()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBMAINTTECH"))
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
                txtMarketing.Text = "";
                txtJobID.Text = "";
                txtRegDate.Value = "";
                ClType.Open_Combos(CmbBillAble, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_billable_type");
                CmbBillAble.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMigration, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_migration_job_order_header");
                CmbMigration.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbArea, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_area_selectbox");
                CmbArea.SelectedValue = "[Select]";
                txtScheduleDate.Value = "";
                txtRemark.Text = "";
                txtNotes.Text = "";
                txtSearch.Value = "";
                txtJobIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Text = "Submit";
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
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecMaintJobOrder"], LblPagingDetail, ViewState["RecMaintJobOrderFieldSort"].ToString(), ViewState["RecMaintJobOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
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
                            strSQL = "sp_submit_job_order_maint '" + txtJobID.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {

                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job order has been successfully</div>";
                                    //sendEmail(JOID);
                                    Session["ClsJobIDMaint"] = "";
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
                            strSQL = "sp_update_job_order_maint_technician '" + txtJobID.Text.Trim() + "','" + txtNotes.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {

                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job order has been successfully</div>";
                                    //sendEmail(JOID);
                                    Session["ClsJobIDMaint"] = "";
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
                    for (int i = 8; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].ToolTip = "Edit";
                    for (int i = 8; i <= 14; i++)
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
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
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
                string sStatus = ""; string sIsMigration = ""; string sBillableDesc = ""; string sMarketing = ""; string sArea = "";

                sJobID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sRegDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sBillableDesc = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sIsMigration = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();

                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBillableID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sArea = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sMarketing = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtCustID.Value = sCustID;
                            txtCustFullName.Text = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;
                            txtMarketing.Text = sMarketing;

                            txtJobID.Text = ClType.CheckNbsp(sJobID);
                            txtRegDate.Value = sRegDate;
                            CmbBillAble.SelectedValue = ClType.CheckNbsp(sBillableID);
                            CmbMigration.SelectedValue = ClType.CheckNbsp(sIsMigration);
                            CmbArea.SelectedValue = ClType.CheckNbsp(sArea);
                            txtScheduleDate.Value = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Session["ClsJobIDMaint"] = txtJobID.Text.Trim();
                            Session["ClsCustIDMaint"] = txtCustID.Value.Trim();
                            Open_GridView();
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
        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendEmail(string joid)
        {

            try
            {
                var pageData = new job_maint_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_maint_reaktivasi '" + joid + "','DR'";
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
                        string sTvdID = Rec.Fields("Tvdid");

                        notifEmail(sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sMaintTypeDesc, sTvdID);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sMaintTypeDesc, string sTvdID)
        {
            string emailpass = "";
            string emailto = "";
            string emailto2 = "";
            string emailto3 = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailTo'";
                Recordset Rec1 = new Recordset();
                Rec1.Open(strSQLemail, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec1.RecordCount() > 0)
                {
                    string rec1 = Rec1.Fields("ParValue");
                    emailto += rec1;
                }

                string strSQLemail2 = "sp_list_par_global 'EmailFrom'";
                Recordset Rec2 = new Recordset();
                Rec2.Open(strSQLemail2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec2.RecordCount() > 0)
                {
                    string rec2 = Rec2.Fields("ParValue");
                    emailfrom = rec2;
                }

                string strSQLemail3 = "sp_list_par_global 'EmailPass'";
                Recordset Rec3 = new Recordset();
                Rec3.Open(strSQLemail3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec3.RecordCount() > 0)
                {
                    string rec3 = Rec3.Fields("ParValue");
                    emailpass = rec3;
                }

                string strSQLemail4 = "sp_list_par_global 'EmailSettings'";
                Recordset Rec4 = new Recordset();
                Rec4.Open(strSQLemail4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec4.RecordCount() > 0)
                {
                    string rec4 = Rec4.Fields("ParValue");
                    emailsetings = rec4;
                }

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(emailsetings);
                SmtpServer.Host = emailsetings;
                SmtpServer.UseDefaultCredentials = false;

                mail.IsBodyHtml = true;
                mail.From = new MailAddress(emailfrom);
                foreach (var address in emailto.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(new MailAddress(address));
                }
                mail.Subject = sJoID + " Approval - VTS ADMIN Job Maintenance ";
                mail.Body = BodyEmail(sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sMaintTypeDesc, sTvdID);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailfrom, emailpass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        private string BodyEmail(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sMaintTypeDesc, string sTvdID)
        {
            string Body = "", Header = "";
            string ContentMail = "";

            string url = "https://vtsadmin.easygo-gps.co.id/job_maintenance_approve.aspx?JobID=" + sJoID + "TvdID=" + sTvdID;
            string urlcancel = "https://vtsadmin.easygo-gps.co.id/job_maintenance_cancel.aspx?JobID=" + sJoID + "TvdID=" + sTvdID;

            Header += "<div style='display:block;width:600px;max-width:100%;height:auto;border-radius:5px;box-sizing:border-box;margin:30px auto;border:1px solid #ddd;padding:30px'>";
            Header += "<div style='width:100%;margin:15px auto'>";
            Header += "<img style='width:100%;display:block' src='https://easygo-gps.co.id/images/header_email_igo.jpg' alt='' class='CToWUd a6T' tabindex='0'><div class='a6S' dir='ltr' style='opacity: 0.01; left: 665px; top: 297.85px;'>";
            Header += "<div id=':z8' class='T-I J-J5-Ji aQv T-I-ax7 L3 a5q' role='button' tabindex='0' aria-label='Download lampiran ' data-tooltip-class='a1V' data-tooltip='Download'>";
            Header += "<div class='aSK J-J5-Ji aYr'></div>";
            Header += "</div>";
            Header += "</div>";
            Header += "</div>";


            Header += "<div style='margin:30px auto;background:#fff;padding:0px;border-radius:5px;width:600px;max-width:100%'>";
            Header += "<span class='im'>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear, Tim <b>" + sCustomerName + "</b> Telah melakukan pengajuan Job Maintenance <b>" + sMaintTypeDesc + "</b></p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Silahkan melakukan proses Approve jika customer tidak mempunyai <b>Account Receivable</b> atau <b>Hutang</b>.</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance ";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nama Customer</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sCustomerName + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job Order</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sJoID + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Type Job Order</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMaintTypeDesc + "</span></td>";
            Header += "</tr>";


            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Tgl. Job Order</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sSchDate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nomor Polisi</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoliceNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Serial Number (GPS)</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMSIDN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Serial Number (GSM)</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMSIDN + "</span></td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            Header += "<div style='padding-top:32px;text-align:center'><a href ='" + url + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#00ab6b;border-radius:5px;min-width:100%' target = '_blank'> Approve Job Order</a></div>";
            Header += "<div style='padding-top:32px;text-align:center'><a href ='" + urlcancel + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#ab1100;border-radius:5px;min-width:100%' target = '_blank'> Reject Job Order</a></div>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }
    }
}