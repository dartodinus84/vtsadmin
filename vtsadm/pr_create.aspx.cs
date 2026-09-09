using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Data.SqlClient;

using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_create : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public pr_create()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                var access = Session["ClsTypeAccessMenu"].ToString().ToUpper();
                if (!access.Contains("MNUPRCREATE") && !access.Contains("MNUPOCREATE"))
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
                else
                {
                    if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Open_GridViewHeader();
                        if (txtPurID.Text.Trim() != "")
                        {
                            PanelListDetail.Visible = true;
                            Open_GridView();
                        }
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
                txtVendorID.Value = "";
                txtVendorName.Text = "";
                txtVendorAddress.Text = "";
                txtVendorPhone.Text = "";
                txtPurID.Text = "";
                txtPurDate.Text = "";
                txtPurNumber.Text = "";
                txtContractTime.Value = "";
                txtTrialExpireDate.Value = "";
                ClType.Open_Combos(CmbPurType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_pur_type");
                CmbPurType.SelectedValue = "[Select]";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtPurIDDelete.Value = "";
                txtStatusDelete.Value = "";
                txtSeqDelete.Value = "";
                txtRemarkClose.Value = "";
                txtSeqClose.Value = "";
                CmdSubmit.Text = "Submit";
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdLoad.Visible = false;
                CmdSubmit.Visible = false;
                PanelListDetail.Visible = false;
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
                string strSQL = "sp_list_header_pur_create '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreatePurchaseOrderHeaderFieldSort"] = "PurID";
                ViewState["RecCreatePurchaseOrderHeaderDirSort"] = "DESC";
                Session["RecCreatePurchaseOrderHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString());
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
                string strSQL = "sp_list_pur_order_detail '" + txtPurID.Text.Trim() + "'";
                ViewState["RecCreatePurchaseOrderFieldSort"] = "PurID";
                ViewState["RecCreatePurchaseOrderDirSort"] = "DESC";
                Session["RecCreatePurchaseOrder"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString());
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

        protected void CmdAddDetail_Click(object sender, EventArgs e)
        {

        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreatePurchaseOrder"], LblPagingDetail, ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtVendorID.Value.Trim() != "")
                {
                    if (txtPurDate.Text.Trim() != "")
                    {
                        if (txtPurNumber.Text.Trim() != "")
                        {
                            if (CmbPurType.SelectedItem.Value.Trim() != "[Select]")
                            {
                                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                ExecCommand ec = new ExecCommand();
                                strSQL = "sp_insert_pur_order_header '" + txtVendorID.Value.Trim() + "','" + txtPurDate.Text.Trim() + "','" + txtPurNumber.Text.Trim().Replace("'", "''") + "','" + CmbPurType.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (sErr.Trim().StartsWith("PUR", StringComparison.OrdinalIgnoreCase))
                                    {
                                        txtPurID.Text = sErr.Trim();
                                        Session["ClsPurID"] = txtPurID.Text.Trim();
                                        Open_GridView();
                                        PanelListDetail.Visible = true;
                                        CmdCreate.Visible = false;
                                        CmdAddDetail.Visible = true;
                                        CmdSubmit.Visible = true;
                                        CmdLoad.Visible = true;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create purchase request header has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select PR Type</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in PR Number</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select PR date</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vendor</div>";
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
                ExecCommand ec = new ExecCommand();
                if (txtVendorID.Value.Trim() != "")
                {
                if (txtPurID.Text.Trim() != "")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            strSQL = "sp_submit_pur_order_validate '" + txtPurID.Text.Trim() + "','" + txtVendorID.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    //sendEmail(txtPurID.Text.Trim());
                                    //sendTelegram(txtPurID.Text.Trim());
                                    clear();
                                    Open_GridView();
                                    Open_GridViewHeader();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit purchase request has been successfully</div>";
                                    Session["ClsPurID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit purchase request has been failed (" + sErr + ")</div>";
                                }
                            }
                        }
                        else
                        {
                            if (CmbPurType.SelectedItem.Value.Trim() != "[Select]")
                            {
                                strSQL = "sp_update_pur_order_new '" + txtPurID.Text.Trim() + "','" + txtPurDate.Text.Trim() + "','" + txtVendorID.Value.Trim() + "'," +
                                         "'" + txtPurNumber.Text.Trim().Replace("'", "''") + "','" + CmbPurType.SelectedItem.Value.Trim() + "'," +
                                         "'" + txtRemark.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        //sendTelegram(txtPurID.Text.Trim());
                                        clear();
                                        Open_GridView();
                                        Open_GridViewHeader();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update purchase request has been successfully</div>";
                                        Session["ClsPurID"] = "";
                                        CmdCreate.Visible = true;
                                        CmdAddDetail.Visible = false;
                                        CmdSubmit.Visible = false;
                                        CmdLoad.Visible = false;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update purchase request has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select PR Type</div>";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void sendTelegram(string poid)
        {
            try
            {
                string strSQL = "sp_get_pur_status '" + poid + "','RG'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string sPurID = Rec.Fields("PurID");
                    string sPurDate = Rec.Fields("purdate");
                    string sPurNo = Rec.Fields("PurNumber");
                    string sVendorName = Rec.Fields("VendorName");
                    notifTelegram(sPurNo, sPurDate, sVendorName, "", 0, "");
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void sendEmail(string poid)
        {
            try
            {
                string strSQL = "sp_get_pur_status '" + poid + "','NA'";
                Recordset RecMail = new Recordset();
                RecMail.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecMail.RecordCount() > 0)
                {
                    string sPurID = RecMail.Fields("PurID");
                    string sPurDate = RecMail.Fields("purdate");
                    string sPurNo = RecMail.Fields("PurNumber");
                    string sVendorName = RecMail.Fields("VendorName");
                    notifEmail(sPurID, sPurNo, sPurDate, sVendorName, "", 0, "");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void notifTelegram(string pono, string podate, string cus, string tolls, int qty, string marketing)
        {
            string chatid = "";
            string apitoken = "";
            string url = "";

            try
            {
                string strSQLtelegram = "sp_list_par_global 'TelegramChatId'";
                Recordset Recchatid = new Recordset();
                Recchatid.Open(strSQLtelegram, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Recchatid.RecordCount() > 0)
                {
                    chatid += Recchatid.Fields("ParValue");
                }

                string strSQLtelegram2 = "sp_list_par_global 'TelegramAPIToken'";
                Recordset Recchatid2 = new Recordset();
                Recchatid2.Open(strSQLtelegram2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Recchatid2.RecordCount() > 0)
                {
                    apitoken += Recchatid2.Fields("ParValue");
                }
                url = Session["ClsTypeBaseUrl"].ToString() + "pr_create_approve.aspx?PurID=" + pono.ToString() + "&VendorName=" + cus + "&purdate=" + podate + "&tolls=" + tolls + "&qty=" + qty + "&marketing=" + marketing;
                botClient = new TelegramBotClient(apitoken);
                var me = botClient.GetMeAsync().Result;
                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
                var message = botClient.SendTextMessageAsync(chatid, "Dear Bapak/Ibu," + "\n" + "\n" + "Terdapat pengajuan Purchase Request untuk customer " + cus + " pada tanggal " + podate + "!" + "\n" + "\n" + "Klik link dibawah ini untuk melakukan approval" + "\n" + url + "\n" + "\n" + "\n" + "Best Regards," + "\n" + "VTS Admin" + "\n" + "" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss")).Result;
            }
            catch (Exception ex)
            {
            }
        }
        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            botClient.StopReceiving();
        }

        protected void notifEmail(string id, string pono, string podate, string customer, string tolls, int qty, string marketing)
        {
            string emailpass = "";
            string emailto = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailFeedback'";
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
                mail.Subject = id + " " + " New Purchase Request " + customer;
                mail.Body = BodyEmail(id, pono, podate, customer, tolls, qty, marketing);

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
        private string BodyEmail(string id, string pono, string podate, string customer, string tolls, int qty, string marketing)
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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Purchase Request <b> " + id + "</b>. Atas nama Vendor <b> " + customer + "</b> telah dibuat</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Pembelian / Penyewaan GPS";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nomor Purchase Request</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + pono + " </ td >";
            Header += "</tr>";

            Header += "<tr style='vertical-align:top'>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nama Vendor</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><b>" + customer + "</b></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Tanggal Purchase Request</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + podate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top'>";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Marketing</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + marketing + "</ span ></ td>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";

            string Mail = Header + Body + ContentMail;
            return Mail;
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewHeader();
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreatePurchaseOrderHeader"], LblPagingHeader, ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString());
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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 8; i <= 10; i++)
                        e.Row.Cells[i].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 3; i <= 5; i++)
                    {
                        if (string.IsNullOrWhiteSpace(e.Row.Cells[i].Text))
                            e.Row.Cells[i].Text = "-";
                    }
                    e.Row.Cells[6].ToolTip = "Edit PR";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    if (CmdButton != null)
                    {
                        string sPurId = HttpUtility.HtmlDecode((e.Row.Cells[0].Text ?? "").Trim()).Replace("\u00a0", "").Replace("'", "\\'");
                        string sStatus = HttpUtility.HtmlDecode((e.Row.Cells[5].Text ?? "").Trim()).Replace("\u00a0", "").Replace("'", "\\'");
                        CmdButton.OnClientClick = "confirmDelete('" + sPurId + "','" + sStatus + "'); return false;";
                    }
                    for (int i = 8; i <= 10; i++)
                        e.Row.Cells[i].Visible = false;
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
                if (e.CommandName.ToUpper() != "CHANGES")
                    return;

                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sPurID = "";
                string sPurDate = ""; string sPurNo = ""; string sRemark = "";
                string sStatus = ""; string sPurTypeID = ""; string sContractTime = "";
                sCustID = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim()));
                sFullName = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim()));
                sCustType = "";
                sBranchName = "";

                sPurID = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim()));
                sPurDate = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim()));
                sPurNo = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim()));
                sPurTypeID = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim()));
                sContractTime = "";
                sRemark = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim()));
                sStatus = ClType.CheckNbsp(HttpUtility.HtmlDecode((e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim()));
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtVendorID.Value = ClType.CheckNbsp(sCustID);
                            txtVendorName.Text = ClType.CheckNbsp(sFullName);
                            txtVendorAddress.Text = "";
                            txtVendorPhone.Text = "";

                            txtPurID.Text = ClType.CheckNbsp(sPurID);
                            txtPurDate.Text = sPurDate;
                            txtPurNumber.Text = ClType.CheckNbsp(sPurNo);
                            string sPurType = ClType.CheckNbsp(sPurTypeID);
                            if (CmbPurType.Items.FindByValue(sPurType) != null)
                                CmbPurType.SelectedValue = sPurType;
                            else
                                CmbPurType.SelectedValue = "[Select]";
                            txtContractTime.Value = ClType.CheckNbsp(sContractTime);
                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Session["ClsPurID"] = txtPurID.Text.Trim();
                            Open_GridView();
                            PanelListDetail.Visible = true;
                            CmdCreate.Visible = false;
                            CmdAddDetail.Visible = true;
                            CmdSubmit.Visible = true;
                            CmdLoad.Visible = true;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase request can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreatePurchaseOrderHeader"], ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreatePurchaseOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreatePurchaseOrderHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView2_RowDeleting(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            e.Cancel = true;
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecCreatePurchaseOrder"], ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreatePurchaseOrderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreatePurchaseOrderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView2_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sSeq = HttpUtility.HtmlDecode((e.Row.Cells[1].Text ?? "").Trim()).Replace("\u00a0", "");
                    LinkButton CmdCloseDetail = (LinkButton)e.Row.FindControl("CmdCloseDetail");
                    if (CmdCloseDetail != null)
                        CmdCloseDetail.OnClientClick = "confirmCloseDetail('" + sSeq.Replace("'", "\\'") + "');return false;";

                    LinkButton CmdDeleteDetail = (LinkButton)e.Row.FindControl("CmdDeleteDetail");
                    if (CmdDeleteDetail != null)
                        CmdDeleteDetail.OnClientClick = "confirmDeleteDetail('" + sSeq.Replace("'", "\\'") + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesClose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                strSQL = "sp_close_pur_order_detail '" + txtPurID.Text.Trim() + "'," + txtSeqClose.Value.Trim() + ",'" + txtRemarkClose.Value.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        Open_GridView();
                        Open_GridViewHeader();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Close purchase request detail has been successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Close purchase request detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                strSQL = "sp_delete_pur_order_detail '" + txtPurID.Text.Trim() + "','" + txtSeqDelete.Value.Trim() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        Open_GridView();
                        Open_GridViewHeader();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Delete purchase request detail has been successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete purchase request detail has been failed (" + sErr + ")</div>";
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
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (txtPurIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR" || txtStatusDelete.Value.ToUpper().Trim() == "OP")
                    {
                        strSQL = "sp_delete_pur_order_header '" + txtPurIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase request has been removed successfully</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase request has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase request has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase request can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase request has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}
