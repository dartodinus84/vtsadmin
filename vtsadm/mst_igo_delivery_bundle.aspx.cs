using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_delivery_bundle : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_delivery '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListBundleFieldSort"] = "order_id";
                ViewState["RecListBundleDirSort"] = "DESC";
                Session["RecListBundle"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListBundleFieldSort"].ToString(), ViewState["RecListBundleDirSort"].ToString());

            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewDetail()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_mst_igo_sales_order_detail '" + txtOrderID.Value.Trim() + "'";
                ViewState["RecListBundleDetailFieldSort"] = "order_id";
                ViewState["RecListBundleDetailDirSort"] = "DESC";
                Session["RecListBundleDetail"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecListBundleDetailFieldSort"].ToString(), ViewState["RecListBundleDetailDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGODELIVERDEVICE"))
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
                            Open_GridViewDetail();
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
                ClType.Open_Combos(CmbDeliveryID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_igo_master_xpedition");
                CmbDeliveryID.SelectedValue = "[Select]";

                txtOrderID.Value = "";
                txtSalesOrderDesc.Value = "";
                txtSellerDesc.Value = "";
                txtMarketingSourceDesc.Value = "";
                txtMarketingNameDesc.Value = "";
                txtInvoice.Value = "";

                txtCustName.Value = "";
                txtCustAdd.Value = "";
                txtCustEmail.Value = "";
                txtCustPhone.Value = "";
                txtCount.Value = "";
                txtInvImg.Text = "";
                txtRecipt.Text = "";

                txtResi.Text = "";
                Button1.Attributes.Remove("disabled");

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
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtOrderID.Value.Trim() != "")
                {

                    Session["ClsJobIDMaint"] = txtOrderID.Value.Trim();
                    Session["ClsJobIDMaintInv"] = txtInvoice.Value.Trim();
                    Session["ClsJobIDMaintName"] = txtCustName.Value.Trim();
                    Session["ClsJobIDMaintAdd"] = txtCustAdd.Value.Trim();
                    Session["ClsJobIDMaintEmail"] = txtCustEmail.Value.Trim();
                    Session["ClsJobIDMaintPhone"] = txtCustPhone.Value.Trim();

                    CmdCreate.Visible = false;
                    CmdAddDetail.Visible = true;
                    CmdSubmit.Visible = true;
                    CmdLoad.Visible = true;

                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select sales order</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; Int32 intAff1 = 0; String strSQL = ""; String strSQL1 = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtCustName.Value.Trim() != "")
                    {
                        if (txtCustEmail.Value.Trim() != "")
                        {
                            if (txtCustAdd.Value.Trim() != "")
                            {
                                strSQL = "usp_retail_delivery_bundle '" + txtOrderID.Value.Trim() + "','" + CmbDeliveryID.SelectedItem.Value.ToString() + "','" + txtResi.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        sendEmail(txtOrderID.Value.Trim());

                                        clear();
                                        Open_GridView();
                                        Open_GridViewDetail();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Delivery Bundle has been save successfully!</div>";

                                        Session["ClsJobIDMaint"] = "";
                                        Session["ClsJobIDMaintInv"] = "";
                                        Session["ClsJobIDMaintName"] = "";
                                        Session["ClsJobIDMaintAdd"] = "";
                                        Session["ClsJobIDMaintEmail"] = "";
                                        Session["ClsJobIDMaintPhone"] = "";

                                        CmdCreate.Visible = true;
                                        CmdAddDetail.Visible = false;
                                        CmdSubmit.Visible = false;
                                        CmdLoad.Visible = false;
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delivery Bundle has been failed</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delivery Bundle has been failed (" + sErr + ")</div>";
                                }

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Address</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Email</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill No Recipt</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewDetail();
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListBundle"], LblPaging, ViewState["RecListBundleFieldSort"].ToString(), ViewState["RecListBundleDirSort"].ToString());
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

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
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecListBundle"], ViewState["RecListBundleFieldSort"].ToString(), ViewState["RecListBundleDirSort"].ToString(), e.SortExpression);
                ViewState["RecListBundleFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListBundleDirSort"] = sNewDirSort;

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListBundleDetail"], LblPagingDetail, ViewState["RecListBundleDetailFieldSort"].ToString(), ViewState["RecListBundleDetailDirSort"].ToString());
            div_comment.InnerHtml = "";
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
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDeleteDetail");
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + e.Row.Cells[1].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecListBundleDetail"], ViewState["RecListBundleDetailFieldSort"].ToString(), ViewState["RecListBundleDetailDirSort"].ToString(), e.SortExpression);
                ViewState["RecListBundleDetailFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListBundleDetailDirSort"] = sNewDirSort;
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
        private void sendEmail(string orderid)
        {

            try
            {
                var pageData = new mst_igo_delivery_bundle();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_device_delivery '" + orderid + "'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {
                        string sCustName = RecMail.Fields("seller_cust_name");
                        string sCustEmail = RecMail.Fields("seller_cust_email");
                        string sInvoice = RecMail.Fields("seller_invoice");
                        string sSN = RecMail.Fields("gps_sn");
                        string sGSM = RecMail.Fields("gsm_no");
                        string sVoucher = RecMail.Fields("voucher_code");
                        notifEmail(sCustName, sCustEmail, sInvoice, sSN, sGSM, sVoucher);
                        RecMail.MoveNext();
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sCustName, string sCustEmail, string sInvoice, string sSN, string sGSM, string sVoucher)
        {
            string emailpass = "";
            string emailto = "";
            string emailto2 = "";
            string emailto3 = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {

                emailto = sCustEmail;

                string strSQLemail2 = "sp_list_par_global 'EmailFromIGO'";
                Recordset Rec2 = new Recordset();
                Rec2.Open(strSQLemail2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec2.RecordCount() > 0)
                {
                    string rec2 = Rec2.Fields("ParValue");
                    emailfrom = rec2;
                }

                string strSQLemail3 = "sp_list_par_global 'EmailPassIGO'";
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
                mail.Subject = " Activation - IGO TRACKER ";
                mail.Body = BodyEmail(sCustName, sCustEmail, sInvoice, sSN, sGSM, sVoucher);

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
        private string BodyEmail(string sCustName, string sCustEmail, string sInvoice, string sSN, string sGSM, string sVoucher)
        {
            string Body = "", Header = "";
            string ContentMail = "";

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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear, <b>" + sCustName + "</b></p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Terimakasih telah melakukan pembelian Device iGO TRACKER silakan melakukan aktivasi kendaraan.</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Pembelian Device iGO TRACK";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>No Invoice</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sInvoice + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Serial Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sSN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Voucher Code</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sVoucher + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset' colspan='3'>Disclaimer : Aktivasi voucher di lakukan setelah GPS berhasil terinstall pada kendaraan. Pastikan SN & GSM yang di input sesuai dengan data pada email di atas.</td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }
        protected void CmdDownloadInv_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (txtOrderID.Value.Trim() != "")
                {
                    if (txtInvImg.Text.Trim() != "" && !txtInvImg.Text.Contains("&nbsp;"))
                    {
                        string strFullPath = Server.MapPath("~/Export//" + txtInvImg.Text.Trim());
                        
                        Response.Clear();
                        Response.ContentType = "image/jpeg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + txtInvImg.Text.Trim() + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Picture Invoice is empty</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select sales order</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdDownloadRecipt_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (txtOrderID.Value.Trim() != "")
                {
                    if (txtRecipt.Text.Trim() != "" && !txtRecipt.Text.Contains("&nbsp;"))
                    {
                        Response.ContentType = "image/jpeg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + txtInvImg.Text.Trim() + "\"");
                        Response.TransmitFile(Server.MapPath(txtInvImg.Text.Trim()));
                        Response.End();

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Picture Recipt is empty</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select sales order</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
