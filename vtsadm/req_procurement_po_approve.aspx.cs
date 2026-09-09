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
    public partial class req_procurement_po_approve : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecPOToolsApproveFieldSort";
        string sViewStateDirSort = "RecPOToolsApproveDirSort";
        string sSessionRecList = "RecPOToolsApprove";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_view_header_po_tools'" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "PurID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUAPPPURTLS"))
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
                //ClsType clType = new ClsType();
                //Recordset Rec = new Recordset();
                //string strFullPath = ""; string sMsg = ""; string strFileName = "";
                //DateTime dt = DateTime.Now;
                //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                //strFullPath = Server.MapPath("~/Export//" + strFileName);
                //Rec.RecData = Session["RecPOToolsApprove"] as System.Data.DataSet;
                //if (Rec.RecordCount() > 0)
                //{
                //    if (clType.ExportToCsvTab(Rec, "Purchase Order Tools - Approve", strFullPath.Trim(), 50000, ref sMsg))
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
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdDelete = (LinkButton)e.Row.FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    CmdApprove.OnClientClick = "confirmApprove('" + e.Row.Cells[0].Text.ToString() + "'); return false;";
                    
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
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                string POID = txtPOIDApprove.Value.Trim();

                if (POID != "")
                {
                    strSQL = "sp_submit_po_tools_approval'" + POID + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase Order has been approve successfully!</div>";
                            sendEmailApprove(POID);
                            clear();
                            Open_GridView();

                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Purchase Order has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Purchase Order has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Purchase Order has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                string POID = txtPOIDDelete.Value.Trim();

                if (POID != "")
                {
                    strSQL = "sp_submit_po_tools_cancel'" + POID + "";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase Order has been remove successfully!</div>";
                            sendEmailReject(POID);
                            clear();
                            Open_GridView();
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Purchase Order has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Purchase Order has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order has been failed (" + ex.Message + ")</div>";
            }
        }
        private void sendEmailReject(string poid)
        {
            try
            {
                string strSQL = "sp_get_po_tools_status '" + poid + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string sPoID = Rec.Fields("PurID");
                    string sPoDate = Rec.Fields("PurDate");
                    string sPoNo = Rec.Fields("PurNumber");
                    string sRemark = Rec.Fields("Remark");
                    string sName = Rec.Fields("UsrUpd");
                    notifEmailReject(sPoID, sPoNo, sPoDate, sRemark, sName);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailReject(string sPoID, string sPoDate, string sPoNo, string sRemark, string sName)
        {
            string emailpass = "";
            string emailto = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailFeedbackPO'";
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
                mail.Subject = sPoID + " " + " Approve - Purchase Order Tools " + sName;
                mail.Body = BodyEmailReject(sPoID, sPoNo, sPoDate, sRemark, sName);

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
        private string BodyEmailReject(string sPoID, string sPoDate, string sPoNo, string sRemark, string sName)
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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Purchase Order Tools <b> " + sPoID + "</b>. Atas nama <b> " + sName + "</b> telah dilakukan proses approve</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Purchase Order Tools";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order ID</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sPoID + " </ td >";
            Header += "</tr>";

            Header += "<tr style='vertical-align:top'>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order Date</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><b>" + sPoDate + "</b></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top'>";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Request By</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + sName + "</ span ></ td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";

            string Mail = Header + Body + ContentMail;
            return Mail;
        }
        private void sendEmailApprove(string poid)
        {
            try
            {
                string strSQL = "sp_get_po_tools_status '" + poid + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string sPoID = Rec.Fields("PurID");
                    string sPoDate = Rec.Fields("PurDate");
                    string sPoNo = Rec.Fields("PurNumber");
                    string sRemark = Rec.Fields("Remark");
                    string sName = Rec.Fields("UsrUpd");
                    notifEmailApprove(sPoID, sPoNo, sPoDate, sRemark, sName);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifEmailApprove(string sPoID, string sPoDate, string sPoNo, string sRemark, string sName)
        {
            string emailpass = "";
            string emailto = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailFeedbackPO'";
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
                mail.Subject = sPoID + " " + " Approve - Purchase Order Tools " + sName;
                mail.Body = BodyEmailApprove(sPoID, sPoNo, sPoDate, sRemark, sName);

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
        private string BodyEmailApprove(string sPoID, string sPoDate, string sPoNo, string sRemark, string sName)
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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Purchase Order Tools <b> " + sPoID + "</b>. Atas nama <b> " + sName + "</b> telah dilakukan proses approve</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Purchase Order Tools";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order ID</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sPoID + " </ td >";
            Header += "</tr>";

            Header += "<tr style='vertical-align:top'>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order Date</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><b>" + sPoDate + "</b></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Purchase Order Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top'>";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Request By</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + sName + "</ span ></ td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";

            string Mail = Header + Body + ContentMail;
            return Mail;
        }
    }
}