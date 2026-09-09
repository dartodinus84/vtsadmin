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
    public partial class unblocked_cust : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public unblocked_cust()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBUNBLOCK"))
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
                            //Open_GridViewFunc();
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
                txtUnblockID.Value = "";
                txtReqDate.Value = "";
                txtCustName.Value = "";
                txtSchDate.Value = "";

                txtCustBranchName.Value = "";
                txtCustomerName.Value = "";
                txtCustID.Value = "";

                txtUnblockedDate.Text = "";
                txtRemark.Value = "";

                Session["ClsTypeNewPictureCustUnblocked"] = "";
                CmdSubmit.Text = "Submit";
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
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        //protected void saveFunction(string sTrainID)
        //{
        //    try
        //    {
        //        div_comment.InnerHtml = "";
        //        string sFunctionID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
        //        for (int i = 0; i < GridView1.Rows.Count; i++)
        //        {
        //            sFunctionID = GridView1.Rows[i].Cells[0].Text.ToString();
        //            RadioButton RdoYes = (RadioButton)GridView1.Rows[i].Cells[2].FindControl("RdoYes");
        //            RadioButton RdoNo = (RadioButton)GridView1.Rows[i].Cells[3].FindControl("RdoNo");
        //            TextBox txRemark = (TextBox)GridView1.Rows[i].Cells[4].FindControl("txtFunctionRemark");

        //            strSQL = "sp_insert_training_customer_function '" + sTrainID + "','" + sFunctionID + "','" + RdoYes.Checked.ToString() + "','" + RdoNo.Checked.ToString() + "','" + txRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
        //            Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //protected void Open_GridViewFunc()
        //{
        //    try
        //    {
        //        ClsType ClType = new ClsType();
        //        string strSQL = "sp_list_training_function";
        //        Session["RecListTrainingFunction"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingFunc);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //protected bool checkFunction()
        //{
        //    bool boolOK = false;
        //    try
        //    {
        //        div_comment.InnerHtml = "";
        //        if (GridView1.Rows.Count != 0)
        //        {
        //            for (int i = 0; i < GridView1.Rows.Count; i++)
        //            {
        //                RadioButton RdoYes = (RadioButton)GridView1.Rows[i].Cells[2].FindControl("RdoYes");
        //                RadioButton RdoNo = (RadioButton)GridView1.Rows[i].Cells[3].FindControl("RdoNo");
        //                if (RdoYes.Checked == true || RdoNo.Checked == true)
        //                {
        //                    boolOK = true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            boolOK = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        boolOK = false;
        //    }
        //    return boolOK;
        //}
        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
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
                div_comment.InnerHtml = "";
                int intAff = 0;

                string jono = Convert.ToString(txtUnblockID.Value);
                string jodate = Convert.ToDateTime(txtSchDate.Value).ToString("dd/MM/yyyy");
                string unblockeddate = Convert.ToDateTime(txtUnblockedDate.Text).ToString("dd/MM/yyyy");
                string cus = Convert.ToString(txtCustName.Value);
                string tanda = Convert.ToString(txtRemark.Value);
                string user = Convert.ToString(Session["ClsTypeUserID"].ToString());

                string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtUnblockID.Value.Trim() != "")
                    {
                        strSQL = "sp_submit_unblocked_customer '" + txtUnblockID.Value.Trim() + "','" + txtUnblockedDate.Text.ToString() + "'," +
                                 "'" + txtRemark.Value.ToString() + "'," +
                                 "'" + Session["ClsTypeNewPictureCustUnblocked"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (sErr.ToUpper().Contains("UBL"))
                            {
                                clear();
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Customer unblocked has been save successfully!</div>";
                                Bot(jono, jodate, unblockeddate, cus, tanda, user);
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
        protected void Bot(string jono, string jodate, string unblockeddate, string cus, string tanda, string user)
        {
            string apitoken = "";
            string url = "";
            string chatid = "";
            try
            {
                string strSQLtelegram = "sp_list_par_global 'TelegramChatID2'";
                Recordset RecChatID = new Recordset();
                RecChatID.Open(strSQLtelegram, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecChatID.RecordCount() > 0)
                {
                    chatid += RecChatID.Fields("ParValue");
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

                string text = BodyTelegram(jono, jodate, unblockeddate, cus, tanda, user);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string jono, string jodate, string unblockeddate, string cus, string tanda, string user)
        {
            string msg = "";
            msg += "<b>JOB ORDER CUSTOMER UNBLOCK SUCCESS</b>\r\n";
            msg += "<b>Unblock Job Order</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Unblock Date</b>\r\n";
            msg += "<b>" + unblockeddate + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + tanda + "</b>\r\n";
            msg += "<b>User</b>\r\n";
            msg += "<b>" + user + "</b>\r\n";
            return msg;
        }
    }
}