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
    public partial class accessories_maint : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public accessories_maint()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTACC"))
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
                txtJobID.Value = "";
                txtRegDate.Value = "";
                txtJobCustName.Value = "";
                txtPoID.Value = "";
                txtSchDate.Value = "";

                txtTvdID.Value = "";

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
                txtSearchAvai.Text = "";
                txtSearchSel.Text = "";
                txtTelegram.Text = "";
                Session["ClsTypeAccessoriesMaintPicture"] = "";
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
                Open_GridViews(GridView2, "sp_list_accessories_maint_upline", txtTvaID.Value.ToString(), "RecListAccessoriesMaintUpline",LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_accessories_maint_master", txtTvaID.Value.ToString(), "RecListAccessoriesMaintMaster",LblPagingMaster);
                Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                Open_GridViewTelegram();
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
                string strSQL = sSQL + " '" + sTvdID + "','" + sTechnicianID + "','" + sSearch + "'";
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
                Session["AccessoriesMaintTechnicianID"] = Session["ClsTypeUserTechnicianID"].ToString();
                Open_GridViews(GridView2, "sp_list_accessories_maint_upline", txtTvaID.Value.ToString(), "RecListAccessoriesMaintUpline", LblPagingUpline);
                Open_GridViews(GridView3, "sp_list_accessories_maint_master", txtTvaID.Value.ToString(), "RecListAccessoriesMaintMaster",LblPagingMaster);
                Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable", LblPagingA);
                Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                //Session["AccessoriesMaintTechnicianID"] = txtTechnicianID.Text.Trim();
                Open_GridViewTelegram();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

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
                            strSQL = "sp_accessories_maint_accessories_selected '" + sTvdID + "','" + sTdtID + "','" + sDeviceID + "','" + txtJobID.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    //clear();
                                    Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                                    Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);

                                    //div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> New installation has been remove successfully!</div>";
                                }
                                else
                                {
                                    //txtError.Value = "Delete failed";
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert accessories maintenance accessories has been failed!!</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert accessories maintenance accessories has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Insert accessories maintenance accessories has been failed (" + ex.Message + ")</div>";
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
                            strSQL = "sp_accessories_maintenance_accessories_removed '" + sTvdID + "','" + sTdtID + "','" + sDeviceID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                                    Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Removing accessories maintenance has been failed!!</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Removing accessories maintenance has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    case "BROKEN":
                        {
                            strSQL = "sp_accessories_maintenance_accessories_broken '" + sTvdID + "','" + sTdtID + "','" + sDeviceID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                                    Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Update broken accessories maintenance has been failed!!</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Update broken accessories maintenance has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Update broken accessories maintenance has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView4_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListAccessoriesMaintAccessoriesSelected"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
            div_comment.InnerHtml = "";
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListAccessoriesMaintUpline"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
            div_comment.InnerHtml = "";
        }
        protected void GridView3_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListAccessoriesMaintMaster"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
            div_comment.InnerHtml = "";
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListAccessoriesMaintAccessoriesAvailable"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
            div_comment.InnerHtml = "";
        }

        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);

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
                Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesCancel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0;
                string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (txtTvdID.Value.Trim() != "")
                {
                    strSQL = "sp_cancel_maint_accessories '" + txtTvdID.Value.Trim() + "','" + txtJobID.Value.Trim() + "'," +
                             "'" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridViews(GridView2, "sp_list_accessories_maint_upline", txtTvaID.Value.ToString(), "RecListAccessoriesMaintUpline",LblPagingUpline);
                            Open_GridViews(GridView3, "sp_list_accessories_maint_master", txtTvaID.Value.ToString(), "RecListAccessoriesMaintMaster",LblPagingMaster);
                            Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                            Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                            div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Cancel accessories maintenance has been save successfully</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cancel accessories maintenance has been failed</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cancel accessories maintenance has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cancel accessories maintenance has been failed (" + ex.Message + ")</div>";
            }

        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                saveTelegram();
                div_comment.InnerHtml = "";
                int intAff = 0;
                string strSQL = ""; string sErr = "";
                string jono = Convert.ToString(txtJobID.Value);
                string jodate = Convert.ToDateTime(txtSchDate.Value).ToString("dd/MM/yyyy");
                string cus = Convert.ToString(txtCustomerName.Value);
                string plat = Convert.ToString(txtPoliceNo.Value);
                string gsm = Convert.ToString(txtMSIDN.Value);
                string sn = Convert.ToString(txtNoSN.Value);
                string tec = Convert.ToString(txtName.Value);
                string remark = Convert.ToString(txtRemark.Value);
                string oldacc = "";
                string server = ""; /* Convert.ToString(CmbCustServerID.SelectedValue);*/
                string cc = Convert.ToString(txtTelegram.Text);
                ExecCommand ec = new ExecCommand();
                if (txtTvdID.Value.Trim() != "")
                {
                    strSQL = "sp_insert_maint_accessories '" + txtTvdID.Value.Trim() + "','" + txtJobID.Value.Trim() + "','" + txtVehicleID.Value.Trim() + "','" + txtNoSN.Value.Trim() + "','" + Session["AccessoriesMaintTechnicianID"].ToString().Trim() + "'," +
                             "'" + Session["ClsTypeAccessoriesMaintPicture"].ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridViews(GridView2, "sp_list_accessories_maint_upline", txtTvaID.Value.ToString(), "RecListAccessoriesMaintUpline",LblPagingUpline);
                            Open_GridViews(GridView3, "sp_list_accessories_maint_master", txtTvaID.Value.ToString(), "RecListAccessoriesMaintMaster",LblPagingMaster);
                            Open_GridViewAcc(GridView1, "sp_list_accessories_maint_accessories_available", txtTvdID.Value.ToString(), Session["AccessoriesMaintTechnicianID"].ToString(), txtSearchAvai.Text.Trim(), "RecListAccessoriesMaintAccessoriesAvailable",LblPagingA);
                            Open_GridViewAcc(GridView4, "sp_list_accessories_maint_accessories_selected", txtTvdID.Value.ToString(), txtTechnicianID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListAccessoriesMaintAccessoriesSelected",LblPagingS);
                            Bot(jono, jodate, server, cus, plat, gsm, sn, oldacc,tec,remark, cc);
                            div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Submit accessories maintenance has been save successfully</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Submit accessories maintenance has been failed</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Submit accessories maintenance has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Submit accessories maintenance has been failed (" + ex.Message + ")</div>";
            }

        }
        protected void Bot(string jono, string jodate, string server, string cus, string plat, string gsm, string sn, string oldacc,string tec, string remark, string cc)
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
                string text = BodyTelegram(jono, jodate, server, cus, plat, gsm, sn, oldacc, tec, remark, cc);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string jono, string jodate, string server, string cus, string plat, string gsm, string sn, string oldacc,string tec,string remark, string cc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE ACCESSORIES SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Schedule</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Decive SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>Old Accecories</b>\r\n";
            msg += "<b>" + oldacc + "</b>\r\n";
            msg += "<b>Technician</b>\r\n";
            msg += "<b>" + tec + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
			msg += "<b>New Technician</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>" + cc + "</b>\r\n";
            return msg;
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
                        txtTelegram.Text += " " + sTelegramID.ToString();
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
    }
}