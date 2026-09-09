using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_master_job_assign : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_job_assign '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListDeviceFieldSort"] = "job_id";
                ViewState["RecListDeviceDirSort"] = "ASC";
                Session["RecListDevice"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGOJOBASS"))
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
                ClType.Open_Combos(CmbSourceTech, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_source_igo");
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbSourceTech.SelectedItem.Value,"sp_list_source_mitra_technician");
                ClType.Open_Combos(CmbComision, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_commision_igo");

                txtOrderID.Value = "";
                txtSalesOrderDesc.Value = "";
                txtSellerDesc.Value = "";
                txtMarketingNameDesc.Value = "";
                txtSalesOrderDesc.Value = "";
                txtMarketingSourceDesc.Value = "";


                txtInvoice.Value = "";
                txtCustName.Value = "";
                txtCustAdd.Value = "";
                txtCustEmail.Value = "";
                txtCustAdd.Value = "";

                txtJobID.Value = "";
                txtSN.Value = "";
                txtGSM.Value = "";

                CmbSourceTech.SelectedValue = "[Select]";
                CmbMarketing.SelectedValue = "[Select]";
                CmbComision.SelectedValue = "[Select]";

                txtDate.Text = "";

                LblStockID.InnerHtml = "";
                txtStockIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
                //CmbPriceID.SelectedValue = "[Select]";
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtInvoice.Value.Trim() != "")
                    {
                        if (txtMarketingNameDesc.Value.Trim() != "")
                        {
                            if (txtCustName.Value.Trim() != "")
                            {
                                if (txtCustEmail.Value.Trim() != "")
                                {
                                    if (txtCustAdd.Value.Trim() != "")
                                    {
                                        if (txtDate.Text.Trim() != "")
                                        {
                                            if (CmbComision.SelectedItem.Value.Trim() != "[Select]")
                                            {
                                                if (CmbSourceTech.SelectedItem.Value.Trim() != "[Select]")
                                                {
                                                    strSQL = "usp_retail_insert_job_assign '" + txtOrderID.Value.Trim() + "','" + txtJobID.Value.Trim() + "','" + CmbMarketing.SelectedItem.Value.ToString() + "','" + txtDate.Text.Trim() + "','" + CmbComision.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                                    {
                                                        if (intAff > 0)
                                                        {
                                                            sendEmail(txtJobID.Value.Trim());
                                                            clear();
                                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Selling Voucher has been save successfully!</div>";
                                                            Open_GridView();
                                                        }
                                                        else
                                                        {
                                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + sErr + ")</div>";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + sErr + ")</div>";
                                                    }
                                                }
                                                else
                                                {
                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select source technician</div>";
                                                }
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Commision</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Date Job</div>";
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
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Name</div>";
                            }
                            //}
                            //else
                            //{
                            //    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Market</div>";
                            //}
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Voucher</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Invoice Market</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + ex.Message + ")</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDevice"], LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                    //LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    //CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

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
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListDevice"], ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString(), e.SortExpression);
                ViewState["RecListDeviceFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListDeviceDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void CmbSourceTech_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbSourceTech.SelectedItem.Value.ToString(), "sp_list_source_mitra_technician");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
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
                var pageData = new mst_igo_master_job_assign();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_job_assign_igo '" + orderid + "'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sAgentName = RecMail.Fields("AgentName");
                        string sJobDate = RecMail.Fields("sGenDate");
                        string sCommision = RecMail.Fields("commission");
                        string sAgentEmail = RecMail.Fields("AgentEmail");

                        string sCustName = RecMail.Fields("seller_cust_name");
                        string sCustAdd = RecMail.Fields("seller_cust_address");
                        string sCustPhone = RecMail.Fields("seller_cust_phone");

                        string sGPS = RecMail.Fields("gps_sn");
                        string sGSM = RecMail.Fields("gsm_no");

                        notifEmail(sAgentName, sJobDate, sCommision, sCustName, sCustAdd, sCustPhone, sAgentEmail, sGPS, sGSM);
                        RecMail.MoveNext();
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sAgentName, string sJobDate, string sCommision, string sCustName, string sCustAdd, string sCustPhone, string sAgentEmail, string sGPS, string sGSM)
        {
            string emailpass = "";
            string emailto = "";
            string emailto2 = "";
            string emailto3 = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {

                emailto = sAgentEmail;

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
                mail.Subject = " Job Agent - IGO TRACK ";
                mail.Body = BodyEmail(sAgentName, sJobDate, sCommision, sCustName, sCustAdd, sCustPhone, sAgentEmail, sGPS, sGSM);

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
        private string BodyEmail(string sAgentName, string sJobDate, string sCommision, string sCustName, string sCustAdd, string sCustPhone, string sAgentEmail, string sGPS, string sGSM)
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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear, <b>" + sAgentName + "</b></p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Anda baru saja mendapatkan Job Assignment dari <b> " + sCustName + "</b>.Silahkan melakukan pengambilan pekerjaan <b> " + sCustName + "</b> pada mobile apps iGO Track.</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Pekerjaan iGO TRACK";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nama Pelanggan</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sCustName + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Alamat Pelanggan</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sCustAdd + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Nomor Handphone</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sCustPhone + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Tanggal Pekerjaan</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sJobDate + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GPS SN</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGPS + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";


            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Komisi</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sCommision + "</span></td>";
            Header += "</tr>";


            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset' colspan='3'>Disclaimer : Aktivasi voucher di lakukan setelah GPS berhasil terinstall pada kendaraan. Pastikan SN & GSM yang di input sesuai dengan data pada email yang telah diterima oleh pelanggan.</td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }

    }
}
