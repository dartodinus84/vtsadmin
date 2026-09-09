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
    public partial class vehicle_assign_customer : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public vehicle_assign_customer()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETVEHCUST"))
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
        protected void clear()
        {
            txtCustID.Value = "";
            txtCustFullName.Text = "";
            txtCustCustTypeDesc.Text = "";
            txtCustBranchName.Text = "";
            txtSearchAvai.Text = "";
            txtSearchSel.Text = "";
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleAssignCustomerAvailable"], LblPagingA);
            div_comment.InnerHtml = "";
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicleAssignCustomerSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sCustID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sCustID + "','" + sSearch + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }

        protected void CmdLoadVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
                Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
                Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sVehicleID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        sVehicleID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sVehicleID != "")
                        {
                            strSQL = "sp_vehicle_assign_customer_selected '" + txtCustID.Value.ToString() + "','" + sVehicleID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    string strSQLcount = "sp_get_training_status '" + txtCustID.Value.ToString() + "'";
                                    Recordset RecMail = new Recordset();
                                    RecMail.Open(strSQLcount, Session["ClsTypeDBConnStringSQL"].ToString());
                                    if (RecMail.RecordCount() > 0)
                                    {
                                        string sCustName = RecMail.Fields("customerName");
                                        sendTelegram(txtCustID.Value.ToString());
                                    }
                                    Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sTvaID = ""; string sVehicleID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "REMOVE":
                        sTvaID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                        sVehicleID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sVehicleID != "")
                        {
                            strSQL = "sp_vehicle_assign_customer_removed '" + sTvaID + "','" + txtCustID.Value.ToString() + "','" + sVehicleID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle assignment has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle assignment has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchSel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
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
                    LinkButton CmdMutation = (LinkButton)e.Row.Cells[5].FindControl("CmdMutation");
                    CmdMutation.OnClientClick = "customerMutation('" + txtCustID.Value.Trim() + "','" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesMutated_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; int intAff = 0; string sErr = ""; ExecCommand ec = new ExecCommand();
                if (txtTvaID.Value.Trim() != "" && txtNewCustID.Value.Trim() != "" && txtVehicleID.Value.Trim() != "")
                {
                    strSQL = "sp_vehicle_assign_customer_mutation '" + txtTvaID.Value.Trim() + "','" + txtNewCustID.Value.ToString() + "','" + txtVehicleID.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridViews(GridView2, "sp_list_vehicle_assign_customer_available", txtCustID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVehicleAssignCustomerAvailable", LblPagingA);
                            Open_GridViews(GridView1, "sp_list_vehicle_assign_customer_selected", txtCustID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVehicleAssignCustomerSelected", LblPagingS);
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle customer mutation has been success</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Vehicle customer mutation has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Vehicle customer mutation has been failed (" + ex.Message + ")</div>";
            }
        }

        private void sendTelegram(string custid)
        {
            try
            {
                string strSQL = "sp_get_training '" + custid + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string TrainingID = Rec.Fields("TrainingID");
                    string customerName = Rec.Fields("customerName");
                    string MarketingName = Rec.Fields("MarketingName");
                    string ReqDate = Convert.ToDateTime(Rec.Fields("ReqDate")).ToString("dd/MM/yyyy"); ;
                    string ScheduleDate = Convert.ToDateTime(Rec.Fields("ScheduleDate")).ToString("dd/MM/yyyy");
                    notifTelegram(TrainingID, customerName, MarketingName, ReqDate, ScheduleDate);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string TrainingID, string customerName, string MarketingName, string ReqDate, string ScheduleDate)
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
                string text = BodyTelegram(TrainingID, customerName, MarketingName, ReqDate, ScheduleDate);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string TrainingID, string customerName, string MarketingName, string ReqDate, string ScheduleDate)
        {
            string msg = "";
            msg += "<b>CREATE JOB ORDER TRAINING</b>\r\n";
            msg += "<b>Training ID</b>\r\n";
            msg += "<b>" + TrainingID + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + customerName + "</b>\r\n";
            msg += "<b>Marketing Name</b>\r\n";
            msg += "<b>" + MarketingName + "</b>\r\n";
            msg += "<b>Create Date</b>\r\n";
            msg += "<b>" + ReqDate + "</b>\r\n";
            return msg;

        }
    }
}