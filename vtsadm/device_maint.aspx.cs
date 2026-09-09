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
    public partial class device_maint : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        string UplineID { get; set; }
        public device_maint()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTDEV"))
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
                            if (Session["ClsTypeUserTechnicianID"] != null)
                            {
                                Session["DeviceMaintTechnicianID"] = Session["ClsTypeUserTechnicianID"].ToString();
                            }
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

                txtDeviceID.Value = "";
                txtNoSN.Value = "";
                txtVendorName.Value = "";
                txtDeviceTypeDesc.Value = "";
                txtDeviceSourceName.Value = "";
                txtWarehouseName.Value = "";
                txtTdtID.Value = "";

                txtVehicleID.Value = "";
                txtVehicleDesc.Value = "";
                txtPoliceNo.Value = "";
                txtAssetNo.Value = "";
                txtTvaID.Value = "";

                txtCustBranchName.Value = "";
                txtCustID.Value = "";
                txtCustomerName.Value = "";

                txtTechnicianID.Value = "";
                txtEmployeeNo.Value = "";
                txtName.Value = "";
                txtTechBranchName.Value = "";

                txtGsmID.Value = "";
                txtMSIDN.Value = "";
                txtProviderName.Value = "";
                txtGsmSourceName.Value = "";
                txtTgtID.Value = "";

                txtDate.Value = "";
                txtRemark.Value = "";
                txtWaranty.Value = "";
                txtTvdID.Value = "";
                textCheckNotif.Checked = false;
                txtEmailNotif.Value = "";

                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_maint_server");
                CmbCustServerID.SelectedValue = "[Select]";

                txtNewDeviceID.Value = "";
                txtNewNoSN.Value = "";
                txtNewVendorName.Value = "";
                txtNewDeviceTypeDesc.Value = "";
                txtNewDeviceSourceName.Value = "";
                txtNewWarehouseName.Value = "";
                txtNewTdtID.Value = "";
                ClType.Open_Combos(CmbStatusOldDevice, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_maint_device_status");
                CmbStatusOldDevice.SelectedValue = "[Select]";
                txtNewRemark.Text = "";
                Session["ClsTypeDeviceMaintPicture"] = "";
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
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                Open_GridViews(GridView2, "sp_list_device_maint_upline", txtTvaID.Value.ToString(), "RecListDeviceMaintUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_device_maint_master", txtTvaID.Value.ToString(), "RecListDeviceMaintMaster", LblPagingMaster);
                Open_GridViewAcc(GridView4, "sp_list_device_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListDeviceMaintAccessoriesSelected", LblPagingS);
                Open_GridViewTelegram();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }


        protected void Open_GridViewUser(GridView GrdVw, string sSQL, string sCustID, string sServerID, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();

                string strSQL = sSQL + " '" + sCustID + "','" + sServerID + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
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
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_device_maint_server");
                CmbCustServerID.SelectedValue = "[Select]";

                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                Open_GridViews(GridView2, "sp_list_device_maint_upline", txtTvaID.Value.ToString(), "RecListDeviceMaintUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_device_maint_master", txtTvaID.Value.ToString(), "RecListDeviceMaintMaster", LblPagingMaster);
                Open_GridViewAcc(GridView4, "sp_list_device_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListDeviceMaintAccessoriesSelected", LblPagingS);
                //Session["DeviceMaintTechnicianID"] = txtTechnicianID.Text.Trim();  //in order to new technician can be execute this job order
                Session["DeviceMaintTechnicianID"] = Session["ClsTypeUserTechnicianID"].ToString();
                Open_GridViewTelegram();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected bool checkUserAccess()
        {
            bool boolOK = false;
            try
            {
                div_comment.InnerHtml = "";
                for (int i = 0; i < GridView5.Rows.Count; i++)
                {
                    CheckBox ChkBox = (CheckBox)GridView5.Rows[i].Cells[3].FindControl("Chk1");
                    if (ChkBox.Checked == true)
                    {
                        boolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
            }
            return boolOK;
        }
        protected void saveUserAccess()
        {
            try
            {
                div_comment.InnerHtml = "";
                string sAutoID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView5.Rows.Count; i++)
                {
                    sAutoID = GridView5.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView5.Rows[i].Cells[2].FindControl("Chk1");
                    if (ChkBox.Checked == true)
                    {
                        strSQL = "sp_insert_interfacing_user_access '" + txtNewNoSN.Value.Trim() + "','" + sAutoID + "','" + CmbCustServerID.SelectedItem.Value.Trim() + "'";
                        Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                    }
                }
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0;
                string strSQL = ""; string sErr = "";

                /**
                if (textCheckNotif.Checked == true)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4> "+ txtEmailNotif.Value.Trim() + "</div>";
                }
                else {
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Not OK " + txtEmailNotif.Value.Trim() + "</div>";
                }
                */

                
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtJobID.Value.Trim() != "" && txtVehicleID.Value.Trim() != "" && txtNewDeviceID.Value.Trim() != "" && txtGsmID.Value.Trim() != "")
                    {
                        if (CmbStatusOldDevice.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbCustServerID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (checkUserAccess() == true)
                                {
                                    strSQL = "sp_insert_device_maint_new '" + txtTvdID.Value.Trim() + "','" + txtTvaID.Value.Trim() + "','" + txtTdtID.Value.Trim() + "'," +
                                            "'" + txtNewTdtID.Value.Trim() + "','" + txtDeviceID.Value.ToString() + "','" + txtNewDeviceID.Value.ToString() + "'," +
                                            "'" + txtTgtID.Value.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtJobID.Value.Trim() + "'," +
                                            "'" + txtDate.Value.Trim() + "'," +
                                            "'" + txtWaranty.Value.Trim() + "','" + CmbStatusOldDevice.SelectedItem.Value.Trim() + "','" + txtNewRemark.Text.ToString() + "'," +
                                            "'" + Session["ClsTypeDeviceMaintPicture"].ToString() + "','" + CmbCustServerID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            saveTelegram();
                                            string sTVDID = getTvdID(txtTvaID.Value.ToString(), txtNewTdtID.Value.ToString(), txtTgtID.Value.ToString(), Session["ClsTypeUserTechnicianID"].ToString());
                                            checkJO(txtJobID.Value.Trim(), sTVDID, txtTelegram.Value.Trim(), txtDeviceID.Value.Trim(), txtNoSN.Value.Trim(), CmbStatusOldDevice.SelectedItem.Value.Trim());
                                            saveUserAccess();
                                            clear();
                                            Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                                            Open_GridViews(GridView2, "sp_list_device_maint_upline", txtTvaID.Value.ToString(), "RecListDeviceMaintUpline", LblPagingUpline);
                                            Open_GridViews(GridView3, "sp_list_device_maint_master", txtTvaID.Value.ToString(), "RecListDeviceMaintMaster", LblPagingMaster);
                                            Open_GridViewAcc(GridView4, "sp_list_device_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), "RecListDeviceMaintAccessoriesSelected", LblPagingS);
                                            div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Device maintenance has been save successfully</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving device maintenance has been failed</div>";
                                        }
                                    }
                                    else
                                    {
                                        string sErrRollback = "";
                                        strSQL = "sp_insert_device_maint_rollback '" + txtTvdID.Value.Trim() + "'";
                                        ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErrRollback);
                                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving device maintenance has been failed (" + sErr + ")</div>";
                                    }
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select server</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving device maintenance has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView4_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDeviceMaintAccessoriesSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDeviceMaintUpline"], LblPagingUpline);
            div_comment.InnerHtml = "";
        }
        protected void GridView3_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDeviceMaintMaster"], LblPagingMaster);
            div_comment.InnerHtml = "";
        }
        protected void CmbCustServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }

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
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void checkJO(string sJobID, string sTvdid, string sTelegram, string sDeviceID, string sOldSN, string sStatusDecive)
        {
            try
            {
                var pageData = new device_maint();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_maint_device_status '" + sJobID + "','CL','" + sTvdid + "'";
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

                        notifTelegramDevice(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sOldSN, sTelegram, sBranch);

                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegramDevice(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sOldSN, string sTelegram, string sBranch)
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
                string text = BodyTelegramDevice(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sOldSN, sTelegram, sBranch);
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
        private string BodyTelegramDevice(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sOldSN, string sTelegram, string sBranch)
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
            msg += "<b>" + sGSM + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sOldSN + "</b>\r\n";
            msg += "<b>New Device SN</b>\r\n";
            msg += "<b>" + sMSIDN + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>Technician</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>" + sTelegram + "</b>\r\n";

            return msg;
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
    }
}
