using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_job_maint_uns_approve : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewJOUNApproveFieldSort";
        string sViewStateDirSort = "RecViewJOUNApproveDirSort";
        string sSessionRecList = "RecViewJOUNApprove";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_view_job_maint_uns_approve'" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "jobid";
                ViewState[sViewStateDirSort] = "DESC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOUNAPPROVE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
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
                txtSearch.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }
        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }
        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewJOSPApprove"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Job Maint Sofblock - Approve", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }

                }
                else
                {
                    div_comment.InnerHtml = "No records found";
                }
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
                    for (int i = 10; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdDelete = (LinkButton)e.Row.FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "'); return false;";

                    LinkButton CmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    CmdApprove.OnClientClick = "confirmApprove('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "'); return false;";

                    for (int i = 10; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving or mutation warehouse has been failed!! (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();

                if (txtJoIDApprove.Value.Trim() != "")
                {
                    if (txtTvdIDApprove.Value.Trim() != "")
                    {
                        strSQL1 = "sp_insert_uninstall_maint_new_auto_approve '" + txtTvdIDApprove.Value.Trim() + "','" + txtJoIDApprove.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                        {
                            if (intAff1 > 0)
                            {
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4> Approve Uninstall maintenance has been save successfully</div>";
                                sendEmailApprove(txtJoIDApprove.Value.Trim(), txtTvdIDApprove.Value.Trim(), txtSeqIDApprove.Value.Trim());
                                clear();
                                Open_GridView();

                                clear();
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Uninstall maintenance has been save successfully</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Uninstall maintenance has been failed</div>";
                            }
                        }
                        else
                        {
                            strSQL1 = "sp_insert_uninstall_maint_rollback '" + txtTvdIDApprove.Value.Trim() + "'";
                            ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Uninstall maintenance has been failed (" + sErr1 + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Job Maintenance has been failed (" + sErr1 + ")</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Job Maintenance has been failed (" + sErr1 + ")</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Job Maintenance has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                if (txtJoIDDelete.Value.Trim() != "")
                {
                    if (txtTvdIDDelete.Value.Trim() != "")
                    {
                        strSQL1 = "sp_job_order_maint_detail_uns_reject '" + txtJoIDDelete.Value.Trim() + "','" + txtTvdIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                        {
                            if (intAff1 > 0)
                            {
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4> Reject Uninstall maintenance has been save successfully</div>";
                                sendEmailReject(txtJoIDApprove.Value.Trim(), txtTvdIDApprove.Value.Trim(), txtSeqIDApprove.Value.Trim());
                                clear();
                                Open_GridView();

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4> Reject Uninstall maintenance has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4> Reject Uninstall maintenance has been failed (" + sErr1 + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Reject Job Maintenance has been failed (" + sErr1 + ")</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Reject Job Maintenance has been failed (" + sErr1 + ")</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Job Maintenance has been failed (" + ex.Message + ")</div>";
            }
        }
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendEmailApprove(string sJobID, string sTvdID, string sSeq)
        {
            try
            {
                var pageData = new view_job_maint_uns_approve();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_maint_suspend_status_approve '" + sJobID + "','CL','" + sSeq + "'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sCustomerName = RecMail.Fields("Fullname");
                        string sRemark = RecMail.Fields("Remark");
                        string sSchDate = RecMail.Fields("SchDate");
                        string sPoliceNo = RecMail.Fields("policeno");
                        string sMSIDN = RecMail.Fields("nosn");
                        string sGSM = RecMail.Fields("msidn");
                        string sMaintTypeID = RecMail.Fields("MaintTypeID");
                        string sMaintTypeDesc = RecMail.Fields("MaintTypeDesc");

                        notifEmailApprove(sMaintTypeDesc, sJobID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdID);
                        RecMail.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailApprove(string sMaintTypeDesc, string sJobID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdID)
        {
            string emailpass = "";
            string emailto = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailFeedbackJob'";
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
                    emailfrom += rec2;
                }

                string strSQLemail3 = "sp_list_par_global 'EmailPass'";
                Recordset Rec3 = new Recordset();
                Rec3.Open(strSQLemail3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec3.RecordCount() > 0)
                {
                    string rec3 = Rec3.Fields("ParValue");
                    emailpass += rec3;
                }

                string strSQLemail4 = "sp_list_par_global 'EmailSettings'";
                Recordset Rec4 = new Recordset();
                Rec4.Open(strSQLemail4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec4.RecordCount() > 0)
                {
                    string rec4 = Rec4.Fields("ParValue");
                    emailsetings += rec4;
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
                mail.Subject = sJobID + " " + " Approve - Job Maintenance " + sCustomerName;
                mail.Body = BodyEmailApprove(sMaintTypeDesc, sJobID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdID);

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
        private string BodyEmailApprove(string sMaintTypeDesc, string sJobID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdID)
        {
            string Body = "", Header = "";
            string ContentMail = "";

            Header += "<div style='display:block;width:600px;max-width:100%;height:auto;border-radius:5px;box-sizing:border-box;margin:30px auto;border:1px solid #ddd;padding:30px'>";
            Header += "<div style='width:320px;margin:15px auto'>";
            Header += "<img style='width:100%;display:block' src='https://ci3.googleusercontent.com/proxy/uK6RGpEa8zUR7su2fFuBejJrSGudB2sAgkzNlWjfHqKh_YCDIV4dLvHl-Xnr6o9ypVsj2IAgYwD0zqxl1npu4NNXO-bHGa8=s0-d-e1-ft#https://app.bibit.id/assets/images/imageEmail-10.png' alt='' class='CToWUd a6T' tabindex='0'><div class='a6S' dir='ltr' style='opacity: 0.01; left: 665px; top: 297.85px;'>";
            Header += "<div id=':z8' class='T-I J-J5-Ji aQv T-I-ax7 L3 a5q' role='button' tabindex='0' aria-label='Download lampiran ' data-tooltip-class='a1V' data-tooltip='Download'>";
            Header += "<div class='aSK J-J5-Ji aYr'></div>";
            Header += "</div>";
            Header += "</div>";
            Header += "</div>";

            Header += "<div style='margin:30px auto;background:#fff;padding:0px;border-radius:5px;width:600px;max-width:100%'>";
            Header += "<span class='im'>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Hi <span> Team</span>,</p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Job Maintenance Order <b> " + sJobID + "</b>. Atas nama Customer <b> " + sCustomerName + "</b> telah dilakukan proses <b>approve</b></p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance " + sMaintTypeDesc;
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job ID</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sJobID + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Customer Name</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sCustomerName + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job Schedule</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sSchDate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Car Plate</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoliceNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>IMEI</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMSIDN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Remarks</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sRemark + "</span></td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";

            string Mail = Header + Body + ContentMail;
            return Mail;
        }
        private void sendEmailReject(string sJobID, string sTvdID, string sSeq)
        {
            try
            {
                var pageData = new view_job_maint_uns_approve();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_maint_suspend_status_approve '" + sJobID + "','DE','" + sSeq + "'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sCustomerName = RecMail.Fields("Fullname");
                        string sRemark = RecMail.Fields("Remark");
                        string sSchDate = RecMail.Fields("SchDate");
                        string sPoliceNo = RecMail.Fields("policeno");
                        string sMSIDN = RecMail.Fields("nosn");
                        string sGSM = RecMail.Fields("msidn");
                        string sMaintTypeID = RecMail.Fields("MaintTypeID");
                        string sMaintTypeDesc = RecMail.Fields("MaintTypeDesc");

                        notifEmailReject(sMaintTypeDesc, sJobID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdID);
                        RecMail.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailReject(string sMaintTypeDesc, string sJobID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdID)
        {
            string emailpass = "";
            string emailto = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailFeedbackJob'";
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
                    emailfrom += rec2;
                }

                string strSQLemail3 = "sp_list_par_global 'EmailPass'";
                Recordset Rec3 = new Recordset();
                Rec3.Open(strSQLemail3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec3.RecordCount() > 0)
                {
                    string rec3 = Rec3.Fields("ParValue");
                    emailpass += rec3;
                }

                string strSQLemail4 = "sp_list_par_global 'EmailSettings'";
                Recordset Rec4 = new Recordset();
                Rec4.Open(strSQLemail4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec4.RecordCount() > 0)
                {
                    string rec4 = Rec4.Fields("ParValue");
                    emailsetings += rec4;
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
                //mail.To.Add(new MailAddress(emailto));

                mail.Subject = sJobID + " " + " Approve - Job Maintenance " + sCustomerName;
                mail.Body = BodyEmailReject(sMaintTypeDesc, sJobID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdID);


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
        private string BodyEmailReject(string sMaintTypeDesc, string sJobID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdID)
        {
            string Body = "", Header = "";
            string ContentMail = "";

            Header += "<div style='display:block;width:600px;max-width:100%;height:auto;border-radius:5px;box-sizing:border-box;margin:30px auto;border:1px solid #ddd;padding:30px'>";
            Header += "<div style='width:320px;margin:15px auto'>";
            Header += "<img style='width:100%;display:block' src='https://ci3.googleusercontent.com/proxy/uK6RGpEa8zUR7su2fFuBejJrSGudB2sAgkzNlWjfHqKh_YCDIV4dLvHl-Xnr6o9ypVsj2IAgYwD0zqxl1npu4NNXO-bHGa8=s0-d-e1-ft#https://app.bibit.id/assets/images/imageEmail-10.png' alt='' class='CToWUd a6T' tabindex='0'><div class='a6S' dir='ltr' style='opacity: 0.01; left: 665px; top: 297.85px;'>";
            Header += "<div id=':z8' class='T-I J-J5-Ji aQv T-I-ax7 L3 a5q' role='button' tabindex='0' aria-label='Download lampiran ' data-tooltip-class='a1V' data-tooltip='Download'>";
            Header += "<div class='aSK J-J5-Ji aYr'></div>";
            Header += "</div>";
            Header += "</div>";
            Header += "</div>";

            Header += "<div style='margin:30px auto;background:#fff;padding:0px;border-radius:5px;width:600px;max-width:100%'>";
            Header += "<span class='im'>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Hi <span> Team</span>,</p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Job Maintenance Order <b> " + sJobID + "</b>. Atas nama Customer <b> " + sCustomerName + "</b> telah dilakukan proses <b>reject</b> </p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance " + sMaintTypeDesc;
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job ID</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sJobID + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Customer Name</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sCustomerName + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job Schedule</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sSchDate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Car Plate</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoliceNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>IMEI</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMSIDN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Remarks</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sRemark + "</span></td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";

            string Mail = Header + Body + ContentMail;
            return Mail;
        }

    }
}