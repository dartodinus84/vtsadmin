using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using System.Net;
using vtsadm.App_Code;


namespace vtsadm
{
    public partial class gsm_maint_suspend : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public gsm_maint_suspend()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTGSM"))
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
                            Open_GridViewTelegram();
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
                txtJobID.Value = "";
                txtRegDate.Value = "";
                txtJobCustName.Value = "";
                txtPoID.Value = "";
                txtSchDate.Value = "";

                txtGsmID.Value = "";
                txtMSIDN.Value = "";
                txtProviderName.Value = "";
                txtTgtID.Value = "";

                txtVehicleID.Value = "";
                txtVehicleDesc.Value = "";
                txtPoliceNo.Value = "";
                txtAssetNo.Value = "";
                txtTvaID.Value = "";

                txtCustBranchName.Value = "";
                txtCustomerName.Value = "";

                txtTechnicianID.Value = "";
                txtEmployeeNo.Value = "";
                txtName.Value = "";
                txtTechBranchName.Value = "";

                txtDeviceID.Value = "";
                txtNoSN.Value = "";
                txtVendorName.Value = "";
                txtDeviceTypeDesc.Value = "";
                txtWarehouseName.Value = "";
                txtTdtID.Value = "";

                txtDate.Value = "";
                txtRemark.Value = "";

                txtTvdID.Value = "";

                txtNewGsmID.Value = "";
                txtNewMSIDN.Value = "";
                txtNewProviderName.Value = "";
                txtNewTgtID.Value = "";
                ClType.Open_Combos(CmbStatusOldGsm, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_maint_gsm_status");
                CmbStatusOldGsm.SelectedValue = "[Select]";
                txtNewRemark.Text = "";
                txtTelegram.Value = "";
                Session["ClsTypeGsmMaintPicture"] = "";
                CmdSubmit.Text = "Submit";
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
                Open_GridViews(GridView2, "sp_list_gsm_maint_upline", txtTvaID.Value.ToString(), "RecListGsmMaintUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_gsm_maint_master", txtTvaID.Value.ToString(), "RecListGsmMaintMaster", LblPagingMaster);
                Open_GridViewAcc(GridView4, "sp_list_gsm_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListGsmMaintAccessoriesSelected", LblPagingS);
                Open_GridViewTelegram();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewAcc(GridView GrdVw, string sSQL, string sTvdID, string sTechnicianID, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTvdID + "','" + sTechnicianID + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sTvaID, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTvaID + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }
        protected void CmdLoadData_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViews(GridView2, "sp_list_gsm_maint_upline", txtTvaID.Value.ToString(), "RecListGsmMaintUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_gsm_maint_master", txtTvaID.Value.ToString(), "RecListGsmMaintMaster", LblPagingMaster);
                Open_GridViewAcc(GridView4, "sp_list_gsm_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListGsmMaintAccessoriesSelected", LblPagingS);
                //Session["GsmMaintTechnicianID"] = txtTechnicianID.Text.Trim();
                Session["GsmMaintTechnicianID"] = Session["ClsTypeUserTechnicianID"].ToString();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        private string getTvdID(string sTvaID, string sTdtID, string sTgtID, string sTechnicianID)
        {
            string sOut = "";
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_new_installation_get_tvdid '" + sTvaID + "','" + sTdtID + "','" + sTgtID + "','" + sTechnicianID + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim());
                if (Rec.RecordCount() > 0)
                {
                    sOut = Rec.Fields(0).ToString();
                }
            }
            catch (Exception)
            {
                sOut = "";
            }
            return sOut;
        }
        private string EscapeSql(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                Int32 intAff = 0; String strSQL = ""; string sErr = "";

                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtJobID.Value.Trim() != "" && txtVehicleID.Value.Trim() != "" && txtDeviceID.Value.Trim() != "" && txtNewGsmID.Value.Trim() != "")
                    {
                        if (CmbStatusOldGsm.SelectedItem.Value.Trim() != "[Select]")
                        {
                            strSQL = "sp_insert_gsm_maint_suspend '" + EscapeSql(txtTvdID.Value.Trim()) + "','" + EscapeSql(txtTvaID.Value.Trim()) + "','" + EscapeSql(txtTdtID.Value.Trim()) + "'," +
                                  "'" + EscapeSql(txtTgtID.Value.Trim()) + "','" + EscapeSql(txtNewTgtID.Value.Trim()) + "','" + EscapeSql(txtGsmID.Value.Trim()) + "','" + EscapeSql(txtNewGsmID.Value.Trim()) + "'," +
                                  "'" + EscapeSql(txtTechnicianID.Value.Trim()) + "','" + EscapeSql(Session["ClsTypeUserTechnicianID"].ToString()) + "','" + EscapeSql(txtJobID.Value.Trim()) + "','" + EscapeSql(txtDate.Value.Trim()) + "'," +
                                  "'" + EscapeSql(CmbStatusOldGsm.SelectedItem.Value.Trim()) + "','" + EscapeSql(txtNewRemark.Text.ToString()) + "'," +
                                  "'" + EscapeSql(Session["ClsTypeGsmMaintPicture"].ToString()) + "','" + EscapeSql(Session["ClsTypeUserID"].ToString()) + "'";

                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    //saveTelegram();
                                    //string sTVDID = getTvdID(txtTvaID.Value.ToString(), txtTdtID.Value.ToString(), txtNewTgtID.Value.ToString(), Session["ClsTypeUserTechnicianID"].ToString());
                                    //checkJO(txtJobID.Value.Trim(), sTVDID, txtTelegram.Value.Trim(), txtGsmID.Value.Trim(), txtMSIDN.Value.Trim(), CmbStatusOldGsm.SelectedItem.Value.Trim());
                                    clear();
                                    Open_GridViews(GridView2, "sp_list_gsm_maint_upline", txtTvaID.Value.ToString(), "RecListGsmMaintUpline", LblPagingUpline);
                                    Open_GridViews(GridView3, "sp_list_gsm_maint_master", txtTvaID.Value.ToString(), "RecListGsmMaintMaster", LblPagingMaster);
                                    Open_GridViewAcc(GridView4, "sp_list_gsm_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListGsmMaintAccessoriesSelected", LblPagingS);
                                    div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Gsm maintenance has been save successfully</div>";
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving gsm maintenance has been failed</div>";
                                }
                            }
                            else
                            {
                                string errOriginal = sErr;
                                strSQL = "sp_insert_gsm_maint_rollback '" + EscapeSql(txtTvdID.Value.Trim()) + "'";
                                ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr);
                                string errorMessage = !string.IsNullOrEmpty(errOriginal) ? errOriginal : sErr;
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving gsm maintenance has been failed (" + errorMessage + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select old status</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in Job ID, Vehicle ID, Device ID and Gsm ID</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving gsm maintenance has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView4_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListGsmMaintAccessoriesSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListGsmMaintUpline"], LblPagingUpline);
            div_comment.InnerHtml = "";
        }
        protected void GridView3_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListGsmMaintMaster"], LblPagingMaster);
            div_comment.InnerHtml = "";
        }
        protected void GridView11_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[4].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sIsAttach = e.Row.Cells[4].Text.ToString();
                    if (sIsAttach == "1")
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = true;
                    }
                    else
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = false;
                    }
                    e.Row.Cells[4].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewTelegram()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_get_telegram '" + txtTechnicianID.Value.Trim() + "'";
                Session["RecListCustomerServer"] = ClType.Open_GridView(GridView11, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingTelegram);
            }
            catch (Exception ex)
            {

            }
        }
        protected void saveTelegram()
        {
            try
            {
                div_comment.InnerHtml = "";
                string sTelegramID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView11.Rows.Count; i++)
                {
                    sTelegramID = GridView11.Rows[i].Cells[1].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView11.Rows[i].Cells[3].FindControl("Chk1");
                    if (ChkBox.Checked == true)
                    {
                        txtTelegram.Value += " " + sTelegramID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void checkJO(string sJobID, string sTvdid, string sTelegram, string sGsmID, string sOldGSM, string sStatusGSM)
        {
            try
            {
                var pageData = new gsm_maint_suspend();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_maint_gsm_status '" + sJobID + "','CL','" + sTvdid + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("JobID");
                        string sCustomerName = Rec.Fields("Fullname");
                        string sRemark = Rec.Fields("Remark");
                        string sSchDate = Rec.Fields("SchDate");
                        string sPoliceNo = Rec.Fields("policeno");
                        string sMSIDN = Rec.Fields("nosn");
                        string sGSM = Rec.Fields("msidn");
                        string sMaintTypeID = Rec.Fields("MaintTypeID");
                        string sMaintTypeDesc = Rec.Fields("MaintTypeDesc");
                        string sBranch = Rec.Fields("BranchName");

                        notifTelegramGsm(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sOldGSM, sTelegram, sBranch);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegramGsm(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sOldGSM, string sTelegram, string sBranch)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";
                string chatid = "";
                string fullname = "";

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
                    url = RecUrl.Fields("ParValue");
                }
                string urlString = url;
                string apiToken = apitoken;
                string chatId = chatid;
                string text = BodyTelegramGSM(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sOldGSM, sTelegram, sBranch);
                urlString = String.Format(urlString, apiToken, chatId, text);

                try
                {
                    string connection = Session["ClsTypeDBConnStringSQL"].ToString();
                    var strSQL = "sp_save_log_telegram '" + urlString + "'";
                    ExecCommand ec2 = new ExecCommand();
                    int intAff = 0;
                    string strSQLLOG = ""; string sErr = "";
                    if (ec2.Execute(strSQL, connection.ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {

                        }
                    }
                    WebClient webclient = new WebClient();
                    webclient.DownloadString(urlString);
                }
                catch (Exception ex)
                {

                }

            }

            catch (Exception ex)
            {

            }
        }
        private string BodyTelegramGSM(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sOldGSM, string sTelegram, string sBranch)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJoID + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + sSchDate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + sCustomerName + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranch + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + sPoliceNo + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + sOldGSM + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sMSIDN + "</b>\r\n";
            msg += "<b>New GSM Number</b>\r\n";
            msg += "<b>" + sGSM + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>Technician</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>" + sTelegram + "</b>\r\n";

            return msg;
        }

    }
}