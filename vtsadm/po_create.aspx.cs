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
using System.Net.Mail;
using System.Data.SqlClient;

using vtsadm.App_Code;

namespace vtsadm
{
    public partial class po_create : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public po_create()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUPOCREATE"))
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
                txtPoID.Text = "";
                txtPoDate.Text = "";
                txtTrialExpireDate.Text = "";
                txtPoNumber.Text = "";
                ClType.Open_Combos(CmbPoType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_po_type");
                CmbPoType.SelectedValue = "[Select]";
                txtContractTime.Value = "";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtPoIDDelete.Value = "";
                txtStatusDelete.Value = "";
                txtSeqDelete.Value = "";
                txtRemarkClose.Value = "";
                txtSeqClose.Value = "";
                ChkJo.Checked = false;
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
                string strSQL = "sp_list_header_purchase_create '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreatePurchaseOrderHeaderFieldSort"] = "PoID";
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
                string strSQL = "sp_list_purchase_order_detail '" + txtPoID.Text.Trim() + "'";
                ViewState["RecCreatePurchaseOrderFieldSort"] = "PoID";
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
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtPoDate.Text.Trim() != "")
                    {
                        if (txtPoNumber.Text.Trim() != "")
                        {
                            if (CmbPoType.SelectedItem.Value.Trim() != "[Select]")
                            {
                                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                ExecCommand ec = new ExecCommand();
                                strSQL = "sp_insert_purchase_order_header '" + txtCustID.Value.Trim() + "','" + txtPoDate.Text.Trim() + "','" + txtPoNumber.Text.Trim() + "','" + CmbPoType.SelectedItem.Value.Trim() + "','" + txtContractTime.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtTrialExpireDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (!sErr.ToLower().Contains("contract"))
                                    {
                                        txtPoID.Text = sErr.Trim();
                                        Session["ClsPoID"] = txtPoID.Text.Trim();
                                        CmdCreate.Visible = false;
                                        CmdAddDetail.Visible = true;
                                        CmdSubmit.Visible = true;
                                        CmdLoad.Visible = true;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create purchase order header has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select PO Type</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in PO Number</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select po date</div>";
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

        private bool checkJO()
        {
            bool BoolOK = false;
            try
            {
                if (ChkJo.Checked)
                {
                    if (txtRemark.Text.Trim() != "")
                    {
                        BoolOK = true;
                    }
                }
                else
                {
                    BoolOK = true;
                }
            }
            catch(Exception ex)
            {
                BoolOK = false;
            }
            return BoolOK;
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtPoID.Text.Trim() != "")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            if (checkJO())
                            {
							    //strSQL = "sp_submit_purchase_order_new '" + txtPoID.Text.Trim() + "','" + ChkJo.Checked.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                strSQL = "sp_submit_purchase_order_validate '" + txtPoID.Text.Trim() + "','" + ChkJo.Checked.ToString() + "','" + txtCustID.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
								if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
									                   
                                        sendEmail(txtPoID.Text.Trim());
                                        sendTelegram(txtPoID.Text.Trim());
                                        clear();
                                        Open_GridView();
                                        Open_GridViewHeader();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit purchase order has been successfully</div>";
                                        Session["ClsPoID"] = "";
                                        CmdCreate.Visible = true;
                                        CmdAddDetail.Visible = false;
                                        CmdSubmit.Visible = false;
                                        CmdLoad.Visible = false;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit purchase order has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit purchase order has been failed (Remark should be filled when auto job order is true)</div>";
                            }
                        }
                        else
                        {
                            if (CmbPoType.SelectedItem.Value.Trim() != "[Select]")
                            {
                                strSQL = "sp_update_purchase_order_new '" + txtPoID.Text.Trim() + "','" + txtPoDate.Text.Trim() + "','" + txtCustID.Value.Trim() + "'," +
                                         "'" + txtPoNumber.Text.Trim() + "','" + CmbPoType.SelectedItem.Value.Trim() + "','" + txtContractTime.Value.Trim() + "'," +
                                         "'" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        sendTelegram(txtPoID.Text.Trim());
                                        clear();
                                        Open_GridView();
                                        Open_GridViewHeader();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update purchase order has been successfully</div>";
                                        Session["ClsPoID"] = "";
                                        CmdCreate.Visible = true;
                                        CmdAddDetail.Visible = false;
                                        CmdSubmit.Visible = false;
                                        CmdLoad.Visible = false;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update purchase order has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select PO Type</div>";
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
                string strSQL = "sp_get_po_status '" + poid + "','RG'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string sPoID = Rec.Fields("PoID");
                    string sPoDate = Rec.Fields("podate");
                    string sPoNo = Rec.Fields("ponumber");
                    string sCustName = Rec.Fields("customerName");
                    string sMarketingName = Rec.Fields("MarketingName");
                    notifTelegram(sPoNo, sPoDate, sCustName, "", 0, sMarketingName);
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
                string strSQL = "sp_get_po_status '" + poid + "','NA'";
                Recordset RecMail = new Recordset();
                RecMail.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecMail.RecordCount() > 0)
                {
                    string sPoID = RecMail.Fields("PoID");
                    string sPoDate = RecMail.Fields("podate");
                    string sPoNo = RecMail.Fields("ponumber");
                    string sCustName = RecMail.Fields("customerName");
                    string sMarketingName = RecMail.Fields("MarketingName");
                    notifEmail(sPoID, sPoNo, sPoDate, sCustName, "", 0, sMarketingName);
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
                string chatId = chatid;
                string text = BodyTelegram(pono, podate, cus, tolls, qty, marketing);
                urlString = String.Format(urlString, apiToken, chatId, text);

                System.Net.WebClient webclient = new System.Net.WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string pono, string podate, string cus, string tolls, int qty, string marketing)
        {
            string msg = "";
            msg += "<b>NEW SALES ORDER</b>\r\n";
            msg += "<b>Sales Order Number</b>\r\n";
            msg += "<b>" + pono + "</b>\r\n";
            msg += "<b>Sales Order Date</b>\r\n";
            msg += "<b>" + podate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Marketing</b>\r\n";
            msg += "<b>" + marketing + "</b>\r\n";
            return msg;

        }

        private void notifEmail(string poid, string pono, string podate, string cus, string tolls, int qty, string marketing)
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
                
                mail.Subject = poid + " " + " Confirmation - Sales Order " + cus;
                mail.Body = BodyEmail(poid, pono, podate, cus, tolls, qty, marketing);

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
        private string BodyEmail(string poid, string pono, string podate, string cus, string tolls, int qty, string marketing)
        {
            string Body = "", Header = "";
            string ContentMail = "";

            string url = "https://vtsadmin.easygo-gps.co.id/po_create_approve.aspx?PoID=" + poid;
            string urlcancel = "https://vtsadmin.easygo-gps.co.id/po_create_cancel.aspx?PoID=" + poid;

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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Hi <span>Tim Finance</span>,</p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Mohon memverifikasi daftar keuangan <b> " + cus + "</b>. Silahkan melakukan Approval E-mail jika <b> " + cus + "</b> tidak memiliki Account Receivable atau Tunggakan</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Pembelian / Penyewaan GPS";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nomor Sales Order</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + pono + " </ td >";
            Header += "</tr>";

            Header += "<tr style='vertical-align:top'>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nama Custommer</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><b>" + cus + "</b></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Tanggal Sales Order</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + podate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top'>";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Marketing</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + marketing + "</ span ></ td>";

            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                string sPoID = GridView2.Rows[i].Cells[0].Text.ToString();
                if (poid == sPoID)
                {
                    string sSeq = GridView2.Rows[i].Cells[1].Text.ToString();
                    string sDeviceGroupDesc = GridView2.Rows[i].Cells[2].Text.ToString();
                    string sDeviceTypeDesc = GridView2.Rows[i].Cells[3].Text.ToString();
                    string sQuantity = GridView2.Rows[i].Cells[4].Text.ToString();

                    Header += "<tr style = 'vertical-align:top'>";
                    Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Group</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + sDeviceGroupDesc + "</ span ></ td>";

                    Header += "<tr style = 'vertical-align:top'>";
                    Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Alat</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + sDeviceTypeDesc + "</ span ></ td>";

                    Header += "<tr style = 'vertical-align:top'>";
                    Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Qty</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
                    Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span >" + sQuantity + "</ span ></ td>";
                }
            }
            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            Header += "<div style='padding-top:32px;text-align:center'><a href ='" + url + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#00ab6b;border-radius:5px;min-width:100%' target = '_blank'> Approve Sales Order</a></div>";
            Header += "<div style='padding-top:32px;text-align:center'><a href ='" + urlcancel + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#ab1100;border-radius:5px;min-width:100%' target = '_blank'> Reject Sales Order</a></div>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
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
                if (txtPoIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_purchase_create '" + txtPoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase order has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtPoID.Text.Trim() != "" && txtSeqDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_purchase_order_detail '" + txtPoID.Text.Trim() + "'," + txtSeqDelete.Value.Trim() + "";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase order detail has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order detail has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete purchase order detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete purchase order detail has been failed (" + ex.Message + ")</div>";
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
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sPoID = "";
                string sPoDate = ""; string sPONo = ""; string sRemark = "";
                string sStatus = ""; string sPoTypeID = "";string sContractTime = "";
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();

                sPoID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sPoDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sPONo = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sPoTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sContractTime = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtCustID.Value = sCustID;
                            txtCustFullName.Text = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;

                            txtPoID.Text = ClType.CheckNbsp(sPoID);
                            txtPoDate.Text = sPoDate;
                            txtPoNumber.Text = ClType.CheckNbsp(sPONo);
                            CmbPoType.SelectedValue = ClType.CheckNbsp(sPoTypeID);
                            txtContractTime.Value = ClType.CheckNbsp(sContractTime);
                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Session["ClsPoID"] = txtPoID.Text.Trim();
                            Open_GridView();
                            CmdCreate.Visible = false;
                            CmdAddDetail.Visible = true;
                            CmdSubmit.Visible = true;
                            CmdLoad.Visible = true;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesClose_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtPoID.Text.Trim() != "" && txtSeqClose.Value.Trim() != "")
                {
                    strSQL = "sp_close_purchase_order_detail '" + txtPoID.Text.Trim() + "'," + txtSeqClose.Value.Trim() + ",'" + txtRemarkClose.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase order detail has been close successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing purchase order detail has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing purchase order detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Closing purchase order detail has been failed (" + ex.Message + ")</div>";
            }
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
    }
}