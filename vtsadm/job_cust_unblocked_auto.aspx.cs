using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using System.Net;
using vtsadm.App_Code;
using System.Net.Mail;

namespace vtsadm
{
    public partial class job_cust_unblocked_auto : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public job_cust_unblocked_auto()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBCUSTUNBLOCKEDA"))
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
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtUnblockID.Text = "";
                txtReqDate.Text = "";
                txtScheduleDate.Text = "";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtUnblockIDDelete.Value = "";
                txtStatusDelete.Value = "";
                Session["ClsTypeNewPictureCustUnblocked"] = "";
                Button2.Attributes.Remove("disabled");
                CmdCreate.Visible = true;
                CmdSubmit.Visible = false;
                CmdSubmit.Text = "Submit";
                CmdCreate.InnerText = "Create";
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
                string strSQL = "sp_list_header_job_cust_unblock '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreateJobCustUnblockHeaderFieldSort"] = "UnblockID";
                ViewState["RecCreateJobCustUnblockHeaderDirSort"] = "DESC";
                Session["RecCreateJobCustUnblockHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreateJobCustUnblockHeaderFieldSort"].ToString(), ViewState["RecCreateJobCustUnblockHeaderDirSort"].ToString());
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
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();

                div_comment.InnerHtml = "";
                if (txtCustID.Value.Trim() != "")
                {
                    if (txtReqDate.Text.Trim() != "[Select]")
                    {
                        if (txtScheduleDate.Text.Trim() != "[Select]")
                        {
                            if (CmdCreate.InnerText.ToUpper() == "CREATE")
                            {
                                strSQL = "sp_submit_job_cust_unblock_auto '" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (!sErr.ToLower().Contains("duplicate"))
                                    {
                                        txtUnblockID.Text = sErr.Trim();
                                        Session["ClsJobAutoCust"] = txtUnblockID.Text.Trim();
                                        Session["ClsCustIDCust"] = txtCustID.Value.Trim();
                                        if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                                        {
                                            CmdCreate.Visible = false;
                                            CmdSubmit.Visible = true;
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Create job customer block has been successfully</div>";
                                            sendTelegram(txtUnblockID.Text.Trim());
                                            sendEmail(txtUnblockID.Text.Trim());
                                            clear();
                                            Open_GridViewHeader();
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create Job Order header has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create Job Order header has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                strSQL = "sp_update_job_cust_unblock '" + txtUnblockID.Text.Trim() + "','" + txtReqDate.Text.Trim() + "','" + txtCustID.Value.Trim() + "'," +
                                 "'" + txtScheduleDate.Text.Trim() + "'," +
                                 "'" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        Session["ClsJobAutoCust"] = txtUnblockID.Text.Trim();
                                        Session["ClsCustIDCust"] = txtCustID.Value.Trim();

                                        if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                                        {
                                            CmdCreate.Visible = false;
                                            CmdSubmit.Visible = true;
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job customer block has been successfully</div>";
                                            sendTelegram(txtUnblockID.Text.Trim());
                                            sendEmail(txtUnblockID.Text.Trim());
                                            clear();
                                            Open_GridViewHeader();
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job customer block has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job customer block has been failed (" + sErr + ")</div>";
                                }
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();

                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtUnblockID.Text.Trim() != "")
                    {
                        strSQL = "sp_submit_unblocked_customer '" + txtUnblockID.Text.Trim() + "','" + txtScheduleDate.Text.ToString() + "','" + txtRemark.Text.ToString() + "','" + Session["ClsTypeNewPictureCustUnblocked"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (sErr.ToUpper().Contains("UBL"))
                            {
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Customer unblocked has been save successfully!</div>";
                                sendTelegramOk(txtUnblockID.Text.Trim());
                                clear();
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving customer unblocked has been failed (" + sErr + ")</div>";
                            }
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in Unblock ID</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving customer unblocked has been failed (" + ex.Message + ")</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateJobCustUnblockHeader"], LblPagingHeader, ViewState["RecCreateJobCustUnblockHeaderFieldSort"].ToString(), ViewState["RecCreateJobCustUnblockHeaderDirSort"].ToString());
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
                    for (int i = 7; i <= 10; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[5].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'); return false;";
                    for (int i = 7; i <= 10; i++)
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
                if (txtUnblockIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_cust_unblock '" + txtUnblockIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job customer unblock has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job customer unblock has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job customer unblock has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job customer unblock can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job customer unblock has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sUnblockID = "";
                string sReqDate = ""; string sSchDate = ""; string sRemark = "";
                string sStatus = "";

                sUnblockID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sReqDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtCustID.Value = sCustID;
                            txtCustFullName.Text = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;

                            txtUnblockID.Text = ClType.CheckNbsp(sUnblockID);
                            txtReqDate.Text = sReqDate;
                            txtScheduleDate.Text = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);
                            Button2.Style.Add("disabled", "disabled");
                            CmdCreate.InnerText = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job customer unblock can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job customer unblock can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreateJobCustUnblockHeader"], ViewState["RecCreateJobCustUnblockHeaderFieldSort"].ToString(), ViewState["RecCreateJobCustUnblockHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateJobCustUnblockHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreateJobCustUnblockHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        private void sendTelegram(string joid)
        {

            try
            {
                var pageData = new job_cust_unblocked_auto();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_job_unblock_auto '" + joid + "','RG'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("UnblockID");
                        string sCustomerName = Rec.Fields("CustomerName");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sMarketingName = Rec.Fields("MarketingName");
                        notifTelegram(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void sendTelegramOk(string joid)
        {

            try
            {
                var pageData = new job_cust_unblocked_auto();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_job_unblock_auto '" + joid + "','CL'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("UnblockID");
                        string sCustomerName = Rec.Fields("CustomerName");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sMarketingName = Rec.Fields("MarketingName");
                        notifTelegram(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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
                string text = BodyTelegram(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);

            }

            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE UNBLOCKED CUSTOMER SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Marketing Name</b>\r\n";
            msg += "<b>" + sMarketingName + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendEmail(string sJoID)
        {

            try
            {
                var pageData = new job_cust_unblocked_auto();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_job_unblock_auto '" + sJoID + "','RG'";
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

                        notifEmail(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName);
                        RecMail.MoveNext();
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
        {
            string emailpass = "";
            string emailto = "";
            string emailto2 = "";
            string emailto3 = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {
                string strSQLemail = "sp_list_par_global 'EmailToJob'";
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
                mail.Subject = sJoID + " Approval - Job Order Customer Unblocked ";
                mail.Body = BodyEmail(sJoID, sCustomerName, sRemark, sSchDate, sMarketingName);

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
        private string BodyEmail(string sJoID, string sCustomerName, string sRemark, string sSchDate, string sMarketingName)
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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear Tim, <b>" + Session["ClsTypeUserID"].ToString().ToUpper() + "</b> telah melakukan pengajuan Job Maintenance <b>Unblocked</b> </p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Silahkan melakukan proses Approve jika Job Maintenance diterima atau Reject jika Job Maintenance ditolak.</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Detail Job Maintenance ";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Job ID</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sJoID + " </ td >";
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
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }
    }
}