using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_job_cust_blocked_approve : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewJoBlApproveFieldSort";
        string sViewStateDirSort = "RecViewJoBlApproveDirSort";
        string sSessionRecList = "RecViewJoBlApprove";
        static ITelegramBotClient botClient;
        public view_job_cust_blocked_approve()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_view_job_cust_block_approve'" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "BlockID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOCUSBLAPP"))
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
                txtDecline.Value = "";
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
                div_comment.InnerHtml = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_comment.InnerHtml = "No records found";
                }
                //ClsType clType = new ClsType();
                //Recordset Rec = new Recordset();
                //string strFullPath = ""; string sMsg = ""; string strFileName = "";
                //DateTime dt = DateTime.Now;
                //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                //strFullPath = Server.MapPath("~/Export//" + strFileName);
                //Rec.RecData = Session["RecViewJoBlApprove"] as System.Data.DataSet;
                //if (Rec.RecordCount() > 0)
                //{
                //    if (clType.ExportToCsvTab(Rec, "Job Maint Customer Blocked - Approve", strFullPath.Trim(), 50000, ref sMsg))
                //    {
                //        Response.Clear();
                //        Response.ContentType = "text/plain";
                //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                //        Response.TransmitFile(strFullPath);
                //        Response.Flush();
                //        File.Delete(strFullPath);
                //        Response.End();
                //    }

                //}
                //else
                //{
                //    div_comment.InnerHtml = "No records found";
                //}
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
                    //for (int i = 10; i <= 11; i++)
                    //{
                    //    e.Row.Cells[i].Visible = false;
                    //}
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdDelete = (LinkButton)e.Row.FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    CmdApprove.OnClientClick = "confirmApprove('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    //for (int i = 10; i <= 11; i++)
                    //{
                    //    e.Row.Cells[i].Visible = false;
                    //}
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

                    strSQL1 = "sp_submit_blocked_customer_auto '" + txtJoIDApprove.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                    {
                        if (intAff1 > 0)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4> Reject Customer Blocked maintenance has been save successfully</div>";
                            //sendEmailApprove(txtJoIDApprove.Value.Trim());
                            sendTelegramOk(txtJoIDApprove.Value.Trim());
                            Open_GridView();
                            clear();
                            //div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Reactivated Customer Blocked  has been save successfully</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Customer Blocked has been failed</div>";
                        }
                    }
                    else
                    {
                    }

                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Job Customer Blocked has been failed (" + sErr1 + ")</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Customer Blocked has been failed (" + ex.Message + ")</div>";
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
                    if (txtDecline.Value.Trim() != "")
                    {
                        strSQL1 = "sp_delete_job_cust_block '" + txtJoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                        {
                            if (intAff1 > 0)
                            {
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4> Reject Customer Blocked has been save successfully</div>";
                                sendTelegramReject(txtJoIDApprove.Value.Trim(),txtDecline.Value.Trim());
                                clear();
                                Open_GridView();

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4> Reject Customer Blocked has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4> Reject Customer Blocked has been failed (" + sErr1 + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Reject Customer Unblocked has been failed (" + sErr1 + ")</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Reject Customer Blocked has been failed (" + sErr1 + ")</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Customer Blocked has been failed (" + ex.Message + ")</div>";
            }
        }
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendEmailApprove(string sJobID)
        {
            try
            {
                var pageData = new view_job_cust_blocked_approve();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_job_block_auto '" + sJobID + "','CL'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sCustomerName = RecMail.Fields("CustomerName");
                        string sRemark = RecMail.Fields("Remark");
                        string sSchDate = RecMail.Fields("SchDate");
                        string sMarketingName = RecMail.Fields("MarketingName");

                        notifEmailApprove(sJobID, sCustomerName, sRemark, sSchDate, sMarketingName);
                        RecMail.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailApprove(string sJobID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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
                mail.Subject = sJobID + " " + " Approve - Job Customer Blocked " + sCustomerName;
                mail.Body = BodyEmailApprove(sJobID, sCustomerName, sRemark, sSchDate, sMarketingName);

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
        private string BodyEmailApprove(string sJobID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance Softblock";
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
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Marketing Name</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMarketingName + "</span></td>";
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
        private void sendEmailReject(string sJobID)
        {
            try
            {
                var pageData = new view_job_cust_blocked_approve();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_job_block_auto '" + sJobID + "','DE'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sCustomerName = RecMail.Fields("CustomerName");
                        string sRemark = RecMail.Fields("Remark");
                        string sSchDate = RecMail.Fields("SchDate");
                        string sMarketingName = RecMail.Fields("MarketingName");

                        notifEmailReject(sJobID, sCustomerName, sRemark, sSchDate, sMarketingName);
                        RecMail.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailReject(string sJobID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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

                mail.Subject = sJobID + " " + " Approve - Job Customer Blocked " + sCustomerName;
                mail.Body = BodyEmailReject(sJobID, sCustomerName, sRemark, sSchDate, sMarketingName);


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
        private string BodyEmailReject(string sJobID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance Softblock";
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
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Marketing Name</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMarketingName + "</span></td>";
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
        private void sendTelegramOk(string joid)
        {

            try
            {
                var pageData = new view_job_cust_blocked_approve();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_job_block_auto '" + joid + "','CL'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("BlockID");
                        string sCustomerName = Rec.Fields("CustomerName");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sMarketingName = Rec.Fields("MarketingName");
                        string sUsrUpd = Rec.Fields("UsrUpd");

                        notifTelegram(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName, sUsrUpd);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName,string sUsrUpd)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";

                string strSQLtelegram = "sp_list_par_global 'TelegramChatID2'";
                Recordset RecChatID = new Recordset();
                RecChatID.Open(strSQLtelegram, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecChatID.RecordCount() > 0)
                {
                    sChatID = RecChatID.Fields("ParValue");
                }

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
                string text = BodyTelegram(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName, sUsrUpd);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);

            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName,string sUsrUpd)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE BLOCKED CUSTOMER SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Marekting Name</b>\r\n";
            msg += "<b>" + sMarketingName + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + sUsrUpd + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            return msg;
        }
        private void sendTelegramReject(string joid, string sReason)
        {

            try
            {
                var pageData = new view_job_cust_blocked_approve();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_job_block_auto '" + joid + "','DE'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("BlockID");
                        string sCustomerName = Rec.Fields("CustomerName");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sMarketingName = Rec.Fields("MarketingName");
                        string sUsrUpd = Rec.Fields("UsrUpd");

                        notifTelegramReject(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName, sUsrUpd, sReason);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegramReject(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName, string sUsrUpd, string sReason)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";

                string strSQLtelegram = "sp_list_par_global 'TelegramChatID2'";
                Recordset RecChatID = new Recordset();
                RecChatID.Open(strSQLtelegram, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecChatID.RecordCount() > 0)
                {
                    sChatID = RecChatID.Fields("ParValue");
                }

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
                string text = BodyTelegramReject(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName, sUsrUpd, sReason);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);

            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegramReject(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName, string sUsrUpd, string sReason)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE BLOCKED CUSTOMER Reject</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Marekting Name</b>\r\n";
            msg += "<b>" + sMarketingName + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + sUsrUpd + "</b>\r\n";
            msg += "<b>User Close</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>Reason</b>\r\n";
            msg += "<b>" + sReason + "</b>\r\n";
            return msg;
        }

    }
}