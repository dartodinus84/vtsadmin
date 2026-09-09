using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net;
using System.Text;
using vtsadm.App_Code;
using Newtonsoft.Json;
using System.IO;

namespace vtsadm
{
    public partial class new_install : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        string UplineID { get; set; }
        public new_install()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLNEW"))
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
                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_new_installation_server");
                CmbCustServerID.SelectedValue = "[Select]";

                txtJobID.Value = "";
                txtRegDate.Value = "";
                txtJobCustName.Value = "";
                txtPoID.Value = "";
                txtSchDate.Value = "";

                txtVehicleID.Value = "";
                txtVehicleDesc.Value = "";
                txtPoliceNo.Value = "";
                txtAssetNo.Value = "";
                txtTvaID.Value = "";
                txtCustBranchName.Value = "";
                txtCustomerName.Value = "";
                txtCustID.Value = "";
                txtDeviceID.Value = "";
                txtNoSN.Value = "";
                txtVendorName.Value = "";
                txtDeviceBrand.Value = "";
                txtDeviceModel.Value = "";
                txtDeviceTypeDesc.Value = "";
                txtDeviceSourceName.Value = "";
                txtWarehouseName.Value = "";
                txtTdtID.Value = "";
                txtDeviceServerName.Value = "";
                txtContract.Value = "";
                txtDeviceServerID.Value = "";
                txtGsmID.Value = "";
                txtMSIDN.Value = "";
                txtProviderName.Value = "";
                txtGsmSourceName.Value = "";
                txtTgtID.Value = "";
                txtTechnicianID.Value = "";
                txtEmployeeNo.Value = "";
                txtName.Value = "";
                txtTechBranchName.Value = "";
                txtDate.Text = "";
                txtRemark.Value = "";
                txtTvdID.Value = "";
                txtTelegram.Value = "";
                txtRelay.Value = "";

                Session["ClsTypeNewPicture"] = "";
                Session["ClsTypeNewPicture2"] = "";
                Session["ClsTypeNewPicture3"] = "";
                Session["ClsTypeNewPicture4"] = "";
                Session["ClsTypeNewPicture5"] = "";
                Session["ClsTypeNewPicture6"] = "";
                Session["ClsTypeNewPicture7"] = "";
                Session["ClsTypeNewPicture8"] = "";
                Session["ClsTypeNewPicture9"] = "";
                Session["ClsTypeNewPicture10"] = "";
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
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                Open_GridViews(GridView2, "sp_list_new_installation_upline", txtTvaID.Value.ToString(), "RecListNewInstallationUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_new_installation_master", txtTvaID.Value.ToString(), "RecListNewInstallationMaster", LblPagingMaster);
                Open_GridViewAcc(GridView1, "sp_list_new_installation_accessories_available", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListNewInstallationAccessoriesAvailable", LblPagingA);
                Open_GridViewAcc(GridView4, "sp_list_new_installation_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListNewInstallationAccessoriesSelected", LblPagingS);
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
        protected void Open_GridViewAcc(GridView GrdVw, string sSQL, string sTvdID, string sTechnicianID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTvdID + "','" + txtJobID.Value.Trim() + "','" + sTechnicianID + "','" + sSearch + "'";
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
        private void addAcc()
        {
            try
            {
                Open_GridViewAcc(GridView1, "sp_list_new_installation_accessories_available", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListNewInstallationAccessoriesAvailable", LblPagingA);
                Open_GridViewAcc(GridView4, "sp_list_new_installation_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListNewInstallationAccessoriesSelected", LblPagingS);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdAddAccessories_ServerClick(object sender, EventArgs e)
        {
            addAcc();
        }
        protected void CmdLoadCustomer_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_new_installation_server");
                CmbCustServerID.SelectedValue = "[Select]";

                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                Open_GridViews(GridView2, "sp_list_new_installation_upline", txtTvaID.Value.ToString(), "RecListNewInstallationUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_new_installation_master", txtTvaID.Value.ToString(), "RecListNewInstallationMaster", LblPagingMaster);
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
        protected bool checkUserAccess()
        {
            bool boolOK = false;
            try
            {
                div_comment.InnerHtml = "";
                if (GridView5.Rows.Count != 0)
                {
                    for (int i = 0; i < GridView5.Rows.Count; i++)
                    {
                        CheckBox ChkBox = (CheckBox)GridView5.Rows[i].Cells[3].FindControl("Chk1");
                        if (ChkBox.Checked == true)
                        {
                            boolOK = true;
                        }
                    }
                }
                else
                {
                    boolOK = true;
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
                        strSQL = "sp_insert_interfacing_user_access '" + txtNoSN.Value.Trim() + "','" + sAutoID + "','" + CmbCustServerID.SelectedItem.Value.Trim() + "'";
                        Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void savepicture(string sTdtID, string sJobID, string sPicture1, string sPicture2, string sPicture3, string sPicture4, string sPicture5, string sPicture6, string sPicture7, string sPicture8, string sPicture9, string sPicture10)
        {
            try
            {
                if (sPicture1 != "")
                {
                    Int32 intAff = 0; String strSQL = ""; string sErr = "";
                    ExecCommand ec = new ExecCommand();

                    strSQL = "sp_insert_trx_vehicle_device_picture '" + sTdtID + "','" + sJobID + "','" + sPicture1 + "','" + sPicture2 + "','" + sPicture3 + "','" + sPicture4 + "','" + sPicture5 + "','" + sPicture6 + "','" + sPicture7 + "','" + sPicture8 + "','" + sPicture9 + "','" + sPicture10 + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr);
                }


            }
            catch (Exception ex)
            {

            }
        }


        private string getJwtAuth(string sUserID, string sPassword, string sAppID, string sUrl)
        {
            string strJwt = "";
            try
            {
                string sCred = Convert.ToBase64String(Encoding.ASCII.GetBytes(sUserID.Trim() + ":" + sPassword.Trim()));
                WebClient wc = new WebClient();
                wc.QueryString.Add("app_id", sAppID);
                wc.Headers.Add("Accept", "application/json");
                wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + sCred);
                var response = wc.DownloadString(sUrl);
                var resJwt = new ResultJwt();
                JsonConvert.PopulateObject(response, resJwt);
                strJwt = resJwt.jwt.ToString().Trim();
            }
            catch (Exception ex)
            {
                strJwt = "";
            }

            return strJwt.Trim();
        }
        private bool EsealAdd(string vehicleid, string noImei, string merk, string model, string tipe, ref string statusMessage, ref string outMessage)
        {
            var isValid = true;
            try
            {
                string connection = Session["ClsTypeDBConnStringSQL"].ToString();
                string url_api = "";
                string url_jwt = "";
                string app_id = "";
                string vendorid = "";
                string token = "";
                string sUserID = "";
                string sPassword = "";
                string gps_sn = noImei;
                string message = "";

                string strSQLAPI = "sp_get_api_config 'BCESEAL-ADD'";
                Recordset RecAPI = new Recordset();
                RecAPI.Open(strSQLAPI, connection);

                if (RecAPI.RecordCount() > 0)
                {
                    url_api = RecAPI.Fields("url_api");
                    url_jwt = RecAPI.Fields("url_req_jwt");
                    app_id = RecAPI.Fields("app_id");
                    vendorid = RecAPI.Fields("vendorid");
                    token = RecAPI.Fields("token");
                    sUserID = RecAPI.Fields("userid");
                    sPassword = RecAPI.Fields("password");

                    Uri uri = new Uri(url_api);
                    var param = new ParamEsealAdd();
                    param.idVendor = vendorid;
                    param.merk = merk;
                    param.model = model;
                    param.noImei = noImei;
                    param.tipe = tipe;
                    param.token = token;
                    string sJwt = getJwtAuth(sUserID, sPassword, app_id, url_jwt);

                    var rawData = JsonConvert.SerializeObject(param);
                    WebClient webclient = new WebClient();
                    webclient.UseDefaultCredentials = true;
                    webclient.Headers.Add("Accept", "application/json");
                    webclient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + sJwt);
                    webclient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var response = webclient.UploadString(uri, "POST", rawData);
                    var myObject = new ResponseEsealAdd();
                    JsonConvert.PopulateObject(response, myObject);
                    var status = myObject.status.ToString().ToLower();
                    outMessage = myObject.status;
                    statusMessage = myObject.message;
                    var idEseal = myObject.item.idEseal;
                    var merkEseal = myObject.item.merk;
                    var tipeEseal = myObject.item.tipe;
                    var modelEseal = myObject.item.model;
                    var idVendor = myObject.item.idVendor;
                    var noImeiEseal = myObject.item.noImei;
                    var statusEseal = myObject.item.status;
                    var wkRekam = myObject.item.wkRekam;
                    var wkUpdate = myObject.item.wkUpdate;
                    if (myObject.status.ToString().ToLower() == "success")
                    {
                        var strSQL = "sp_save_log_api_eseal_add '" + idEseal + "','" + merkEseal + "','" + modelEseal + "','" + tipeEseal + "','" + idVendor + "','" + noImeiEseal + "','" + statusEseal + "','" + wkRekam + "','" + wkUpdate + "','" + outMessage + "','" + statusMessage + "'";
                        ExecCommand ec = new ExecCommand();
                        int intAff = 0;
                        string strSQLLOG = ""; string sErr = "";
                        if (ec.Execute(strSQL, connection.ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                outMessage = outMessage.TrimStart(',') + "," + sErr;
                            }
                        }
                    }
                }
                else
                {
                    message = "config Api tidak di temukan";
                    isValid = false;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = ex.Response;
                    var dataStream = response.GetResponseStream();
                    var reader = new StreamReader(dataStream);
                    var details = reader.ReadToEnd();
                    outMessage = outMessage + "," + details;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isValid;
        }
        protected void CmdSubmitYesDev_ServerClick(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            try
            {
                string statusMessage = "", outMessage = "";
                if (EsealAdd("VEH1234567", "TEST_ESEAL0011211", "TEST_BRAND", "TEST_MODEL", "TEST_TYPE", ref statusMessage, ref outMessage))
                {
                    div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>" + outMessage + "</div>";
                }
            }
            catch (Exception ex)
            {

                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>" + ex.Message + "</div>";
            }
        }

        protected void CmdYesSubmit_ServerClickTest(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            int intAff = 0;
            string strSQL = ""; string sErr = "";
            ExecCommand ec = new ExecCommand();

            try
            {
                savepicture(txtTdtID.Value.Trim(), txtJobID.Value.Trim(), Session["ClsTypeNewPicture"].ToString(), Session["ClsTypeNewPicture2"].ToString(), Session["ClsTypeNewPicture3"].ToString(), Session["ClsTypeNewPicture4"].ToString(), Session["ClsTypeNewPicture5"].ToString(), Session["ClsTypeNewPicture6"].ToString(), Session["ClsTypeNewPicture7"].ToString(), Session["ClsTypeNewPicture8"].ToString(), Session["ClsTypeNewPicture9"].ToString(), Session["ClsTypeNewPicture10"].ToString());
                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>New installation has been save successfully!</div>";
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving new installation has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0;
                string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    
                    if (txtJobID.Value.Trim() != "" && txtVehicleID.Value.Trim() != "" && txtDeviceID.Value.Trim() != "" && txtGsmID.Value.Trim() != "")
                    {
                        if (CmbCustServerID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (checkUserAccess() == true)
                            {
                                strSQL = "sp_insert_new_installation_new '" + txtTvaID.Value.Trim() + "','" + txtPoID.Value.ToString() + "','" + txtVehicleID.Value.ToString() + "','" + txtTdtID.Value.Trim() + "'," +
                                            "'" + txtDeviceID.Value.ToString() + "','" + txtTgtID.Value.Trim() + "','" + txtGsmID.Value.ToString() + "'," +
                                            "'" + txtTechnicianID.Value.Trim() + "','" + txtJobID.Value.Trim() + "','" + CmbCustServerID.SelectedItem.Value.Trim() + "','" + txtDeviceServerID.Value.Trim() + "','" + txtDate.Text.Trim() + "'," +
                                            "'" + txtRemark.Value.ToString() + "'," + "'" + txtContract.Value.ToString() + "'," + "'" + txtRelay.Value.ToString() + "','" + Session["ClsTypeNewPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        saveTelegram();
                                        txtTvdID.Value = getTvdID(txtTvaID.Value.ToString(), txtTdtID.Value.ToString(), txtTgtID.Value.ToString(), txtTechnicianID.Value.Trim());
                                        sendTelegram(txtJobID.Value.Trim(), txtTvdID.Value.Trim(), txtTelegram.Value.Trim());
                                        //Bot(jono, jodate.ToString(), server.ToString(), cus, plat, gsm, sn, source, tec, cc);
                                        saveUserAccess();
                                        addAcc();
                                        savepicture(txtTdtID.Value.Trim(), txtJobID.Value.Trim(), Session["ClsTypeNewPicture"].ToString(), Session["ClsTypeNewPicture2"].ToString(), Session["ClsTypeNewPicture3"].ToString(), Session["ClsTypeNewPicture4"].ToString(), Session["ClsTypeNewPicture5"].ToString(), Session["ClsTypeNewPicture6"].ToString(), Session["ClsTypeNewPicture7"].ToString(), Session["ClsTypeNewPicture8"].ToString(), Session["ClsTypeNewPicture9"].ToString(), Session["ClsTypeNewPicture10"].ToString());

                                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>New installation has been save successfully!</div>";
                                        if (txtDeviceTypeDesc.Value.ToUpper().Trim() == "ESEAL38" || txtDeviceTypeDesc.Value.ToUpper().Trim() == "AT-10" || txtDeviceTypeDesc.Value.ToUpper().Trim() == "JT-701" || txtDeviceTypeDesc.Value.ToUpper().Trim() == "AT16")
                                        {
                                            EsealAdd(txtVehicleID.Value.Trim(), txtNoSN.Value.Trim(), txtDeviceBrand.Value.Trim(), txtDeviceModel.Value.Trim(), txtDeviceTypeDesc.Value.Trim(), ref sErr, ref sErr);
                                        }
                                        //clear();
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving new installation has been failed</div>";
                                    }
                                }
                                else
                                {
                                    string sErrRollback = "";
                                    string sTvdID = getTvdID(txtTvaID.Value.ToString(), txtTdtID.Value.ToString(), txtTgtID.Value.ToString(), txtTechnicianID.Value.Trim());
                                    strSQL = "sp_insert_new_installation_rollback '" + sTvdID.Trim() + "'";
                                    ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErrRollback);
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving new installation has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select user id for access device</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select server</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving new installation has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                String strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sTvdID = ""; string sTdtID = ""; string sDeviceID = ""; string sErr = "";
                sTvdID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sTdtID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        if (sTdtID != "")
                        {
                            strSQL = "sp_new_installation_accessories_selected '" + sTvdID + "','" + sTdtID + "','" + sDeviceID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    //clear();
                                    Open_GridViewAcc(GridView1, "sp_list_new_installation_accessories_available", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListNewInstallationAccessoriesAvailable", LblPagingA);
                                    Open_GridViewAcc(GridView4, "sp_list_new_installation_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListNewInstallationAccessoriesSelected", LblPagingS);

                                    //div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> New installation has been remove successfully!</div>";
                                }
                                else
                                {
                                    //txtError.Value = "Delete failed";
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert new installation accessoriess has been failed!!</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert new installation accessoriess has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert new installation accessoriess has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView4_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sTvdID = ""; string sTdtID = ""; string sDeviceID = ""; string sErr = "";
                sTvdID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sTdtID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "REMOVE":
                        if (sTdtID != "")
                        {
                            strSQL = "sp_new_installation_accessories_removed '" + sTvdID + "','" + sTdtID + "','" + sDeviceID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViewAcc(GridView1, "sp_list_new_installation_accessories_available", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListNewInstallationAccessoriesAvailable", LblPagingA);
                                    Open_GridViewAcc(GridView4, "sp_list_new_installation_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListNewInstallationAccessoriesSelected", LblPagingS);
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Removing new installation has been failed!!</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Removing new installation has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Removing new installation has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView4_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListNewInstallationAccessoriesSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListNewInstallationAccessoriesAvailable"], LblPagingA);
            div_comment.InnerHtml = "";
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListNewInstallationUpline"], LblPagingUpline);
            div_comment.InnerHtml = "";
        }
        protected void GridView3_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListNewInstallationMaster"], LblPagingMaster);
            div_comment.InnerHtml = "";
        }
        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewAcc(GridView1, "sp_list_new_installation_accessories_available", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListNewInstallationAccessoriesAvailable", LblPagingA);
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
                Open_GridViewAcc(GridView4, "sp_list_new_installation_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListNewInstallationAccessoriesSelected", LblPagingS);
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmbCustServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        private string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendTelegram(string sJobID, string sTvdID, string sCC)
        {

            try
            {
                var pageData = new new_install();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_install_device_status '" + sJobID + "','RG','" + sTvdID + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sJoID = Rec.Fields("JobID");
                        string sCust = Rec.Fields("CustomerName");
                        string sRemark = Rec.Fields("Remark");
                        string sJobDate = Rec.Fields("SchDate");
                        string sPoliceNo = Rec.Fields("policeno");
                        string sMSIDN = Rec.Fields("nosn");
                        string sGSM = Rec.Fields("msidn");
                        string sBranch = Rec.Fields("BranchName");
                        string sSourceDevice = Rec.Fields("SourceDevice");
                        string sSourceGSM = Rec.Fields("SourceGSM");
                        string sTech = Rec.Fields("TechnicianName");
                        string sServer = Rec.Fields("ServerName");

                        //notifTelegram(sJoID, sJobDate, sServer, sCust, sPoliceNo, sGSM, sMSIDN, sSourceDevice, sSourceGSM, sTech, sCC, sBranch, sRemark);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sJobID, string sJobDate, string sServer, string sCust, string sCar, string sGSM, string sSN, string sSourceDevice, string sSourceGSM, string sTech, string sCC, string sBranch, string sRemark)
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
                    chatid = RecChatID.Fields("ParValue");
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
                string chatId = chatid;

                string text = BodyTelegram(sJobID, sJobDate, sServer, sCust, sCar, sGSM, sSN, sSourceDevice, sSourceGSM, sTech, sCC, sBranch, sRemark);
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
        private string BodyTelegram(string sJobID, string sJobDate, string sServer, string sCust, string sCar, string sGSM, string sSN, string sSourceDevice, string sSourceGSM, string sTech, string sCC, string sBranch, string sRemark)
        {
            string msg = "";
            msg += "<b>JOB ORDER NEW INSTALLMENT SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + sJobID + "</b>\r\n";
            msg += "<b>Schedule</b>\r\n";
            msg += "<b>" + sJobDate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + sCust + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranch + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + sCar + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + sGSM + "</b>\r\n";
            msg += "<b>GSM Source</b>\r\n";
            msg += "<b>" + sSourceGSM + "</b>\r\n";
            msg += "<b>Decive SN</b>\r\n";
            msg += "<b>" + sSN + "</b>\r\n";
            msg += "<b>Device Source</b>\r\n";
            msg += "<b>" + sSourceDevice + "</b>\r\n";
            msg += "<b>Server Install</b>\r\n";
            msg += "<b>" + sServer + "</b>\r\n";
            msg += "<b>Technician</b>\r\n";
            msg += "<b>" + sTech + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + sRemark + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>" + sCC + "</b>\r\n";
            return msg;
        }
        protected void Open_GridViewTelegram()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_get_telegram '" + txtCustID.Value.Trim() + "'";
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

        protected void CmdLoadAll_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_new_installation_server");
                CmbCustServerID.SelectedValue = txtDeviceServerName.Value.Trim();

                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListUserLogin", LblPagingUserAccess);
                Open_GridViews(GridView2, "sp_list_new_installation_upline", txtTvaID.Value.ToString(), "RecListNewInstallationUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_new_installation_master", txtTvaID.Value.ToString(), "RecListNewInstallationMaster", LblPagingMaster);
                addAcc();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
    }
    public partial class ResultJwt
    {
        public string jwt { get; set; }
    }
    public partial class ParamEsealAdd
    {
        public string idVendor { get; set; }
        public string merk { get; set; }
        public string model { get; set; }
        public string noImei { get; set; }
        public string tipe { get; set; }
        public string token { get; set; }
    }
    public partial class ResponseEsealAdd
    {
        public string status { get; set; }
        public string message { get; set; }
        public ResponseItemEsealAdd item { get; set; }
    }
    public partial class ResponseItemEsealAdd
    {
        public string idEseal { get; set; }
        public string merk { get; set; }
        public string model { get; set; }
        public string tipe { get; set; }
        public string idVendor { get; set; }
        public string noImei { get; set; }
        public string status { get; set; }
        public string wkRekam { get; set; }
        public string wkUpdate { get; set; }
    }
}