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
    public partial class maintenance_vehicle : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public maintenance_vehicle()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_vehicle_installed '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListVehicleFieldSort"] = "VehicleID";
                ViewState["RecListVehicleDirSort"] = "DESC";
                Session["RecListVehicle"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLEDITVEH"))
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
                ClType.Open_Combos(CmbVehicleType, Session["ClsTypeDBConnStringSQL"].ToString(), "JenisTruk", "sp_list_par_global");
                ClType.Open_Combos(CmbContainerSize, Session["ClsTypeDBConnStringSQL"].ToString(), "ContainerSize", "sp_list_par_global");
                ClType.Open_Combos(CmbBrandID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_brand");
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_type");
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_model");
                ClType.Open_Combos(CmbIconVehicle, Session["ClsTypeDBConnStringSQL"].ToString(), "VehicleIcon", "sp_list_par_global");

                txtVehID.Text = "";
                CmbBrandID.SelectedValue = "[None]";
                CmbModelID.SelectedValue = "[None]";
                CmbTypeID.SelectedValue = "[None]";
                txtVehDesc.Text = "";
                txtPoliceNo.Text = "";
                txtAssetNo.Text = "";
                CmbVehicleType.SelectedValue = "[None]";
                CmbContainerSize.SelectedValue = "[None]";
                CmbIconVehicle.SelectedValue = "[None]";
                txtEngineNumber.Text = "";
                txtVin.Text = "";

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
                div_comment.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtVehID.Text.Trim() != "")
                    {
                        if (txtPoliceNo.Text.Trim() != "")
                        {
                            if (CmbVehicleType.SelectedItem.Value.Trim() != "[Select]" && CmbContainerSize.SelectedItem.Value.Trim() != "[Select]" && CmbBrandID.SelectedItem.Value.Trim() != "[Select]" && CmbTypeID.SelectedItem.Value.Trim() != "[Select]" && CmbModelID.SelectedItem.Value.Trim() != "[Select]")
                            {

                                strSQL = "sp_update_master_vehicle '" + txtVehID.Text.Trim() + "','" + CmbBrandID.SelectedItem.Value.Trim() + "','" + CmbModelID.SelectedItem.Value.Trim() + "','" + CmbTypeID.SelectedItem.Value.Trim() + "','" + txtVehDesc.Text.Trim() + "','" + txtPoliceNoOld.Text.Trim() + "','" + txtPoliceNo.Text.Trim() + "','" + txtAssetNo.Text.Trim() + "','" + txtVin.Text.Trim() + "','" + txtEngineNumber.Text.Trim() + "','','" + Session["ClsTypeUserID"].ToString() + "','" + CmbVehicleType.SelectedItem.Value.Trim() + "','" + CmbContainerSize.SelectedItem.Value.Trim() + "','" + CmbIconVehicle.SelectedItem.Value.Trim() + "'";

                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        sendTelegram(txtVehID.Text.Trim());
                                        txtVehDesc.Text = "";
                                        txtPoliceNo.Text = "";
                                        txtAssetNo.Text = "";
                                        txtVin.Text = "";
                                        txtEngineNumber.Text = "";
                                        clear();
                                        Open_GridView();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle has been update successfully!</div>";
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update vehicle has been failed</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update vehicle has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Brand / Model / Type / Container Size </div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vehicle type and container size</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update vehicle has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sVehID = ""; string sBrand = ""; string sModel = ""; string sType = ""; string sVehDesc = "";
                string sPoliceNo = ""; string sAssetNo = ""; string sStatus = ""; string sVehicleTypeID = ""; string sContainerSizeID = "";
                string sBrandID = ""; string sModelID = ""; string sTypeID = ""; string sVehDescs = ""; string sVin = ""; string sEngineNumber = "";
                string Brand = ""; string Model = ""; string Type = "";
                string BrandID = ""; string ModelID = ""; string TypeID = "";


                sVehID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sVehDesc = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();

                sBrand = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sModel = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sType = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                
                sPoliceNo = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sAssetNo = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();

                sVin = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sEngineNumber = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();

                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();

                sVehicleTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sContainerSizeID = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sBrandID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sModelID = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();

                string strSQL1 = "sp_get_brand_vehicle '" + sBrandID + "'";
                Recordset Rec1 = new Recordset();
                Rec1.Open(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec1.RecordCount() > 0)
                {
                    Brand = Rec1.Fields("BrandVehicle");
                    BrandID = Rec1.Fields("VehicleBrandID");
                }

                string strSQL2 = "sp_get_type_vehicle '" + sTypeID + "'";
                Recordset Rec2 = new Recordset();
                Rec2.Open(strSQL2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec2.RecordCount() > 0)
                {
                    Type = Rec2.Fields("TypeVehicle");
                    TypeID = Rec2.Fields("VehicleTypeID");

                }

                string PoliceNo = sPoliceNo.Replace("&#160;", " ");
                string strSQL3 = "sp_get_model_vehicle '" + sModelID + "'";
                Recordset Rec3 = new Recordset();
                Rec3.Open(strSQL3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec3.RecordCount() > 0)
                {
                    Model = Rec3.Fields("ModelVehicle");
                    ModelID = Rec3.Fields("VehicleModelID");

                }
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        ClsType ClType = new ClsType();
                        txtVehID.Text = sVehID;
                        txtVehDesc.Text = sVehDesc.Replace("&nbsp;", "").Trim();
                        txtPoliceNo.Text = sPoliceNo.Replace("&nbsp;", "").Trim();
                        txtPoliceNoOld.Text = sPoliceNo.Replace("&nbsp;", "").Trim();
                        txtAssetNo.Text = sAssetNo.Replace("&nbsp;", "").Trim();

                        txtVin.Text = sVin.Replace("&nbsp;", "").Trim();
                        txtEngineNumber.Text = sEngineNumber.Replace("&nbsp;", "").Trim();

                        if (sVehicleTypeID == "" || sVehicleTypeID == "&nbsp;") { sVehicleTypeID = "[Select]"; }
                        CmbVehicleType.SelectedValue = sVehicleTypeID;
                        if (sContainerSizeID == "" || sContainerSizeID == "&nbsp;") { sContainerSizeID = "[Select]"; }
                        CmbContainerSize.SelectedValue = sContainerSizeID;
                        CmbBrandID.SelectedValue = sBrandID;
                        ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBrandID.SelectedItem.Value.ToString(), "sp_get_typevehicle_type");
                        CmbTypeID.SelectedValue = sTypeID;
                        ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value.ToString(), "sp_get_typevehicle_model");
                        CmbModelID.SelectedValue = ModelID;

                        txtVehID.Attributes.Add("disabled", "disabled");
                        div_comment.InnerHtml = "";
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicle"], LblPaging, ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString());
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
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 14; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }


                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 14; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    e.Row.Cells[11].ToolTip = "Edit";
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
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListVehicle"], ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString(), e.SortExpression);
                ViewState["RecListVehicleFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListVehicleDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmbBrandID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBrandID.SelectedItem.Value.ToString(), "sp_get_typevehicle_type");
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value.ToString(), "sp_get_typevehicle_model");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value, "sp_get_typevehicle_model");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        private void sendTelegram(string VehicleID)
        {
            try
            {
                string strSQL = "sp_get_edit_car_master '" + VehicleID + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    string sCustID = Rec.Fields("CustID");
                    string sCustName = Rec.Fields("FullName");
                    string sVehID = Rec.Fields("VehicleID");
                    string sOld = Rec.Fields("OldPoliceNo");
                    string sNew = Rec.Fields("PoliceNo");
                    string sUsrUpd = Rec.Fields("UsrUpd");
                    string sBranchName = Rec.Fields("BranchName");

                    //notifTelegram(sCustID, sCustName, sVehID, sOld, sNew, sUsrUpd,sBranchName);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sCustID, string sCustName, string sVehID, string sOld, string sNew, string sUsrUpd,string sBranchName)
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

                string text = BodyTelegram(sCustID, sCustName, sVehID, sOld, sNew, sUsrUpd, sBranchName);
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
        private string BodyTelegram(string sCustID, string sCustName, string sVehID, string sOld, string sNew, string sUsrUpd,string sBranchName)
        {
            string msg = "";
            msg += "<b>EDIT CAR MASTER SUCCESS</b>\r\n";
            msg += "<b>Cust ID</b>\r\n";
            msg += "<b>" + sCustID + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + sCustName + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchName + "</b>\r\n";
            msg += "<b>Vehicle ID</b>\r\n";
            msg += "<b>" + sVehID + "</b>\r\n";
            msg += "<b>Old Car Plate</b>\r\n";
            msg += "<b>" + sOld + "</b>\r\n";
            msg += "<b>New Car Plate</b>\r\n";
            msg += "<b>" + sNew + "</b>\r\n";
            msg += "<b>User Update</b>\r\n";
            msg += "<b>" + sUsrUpd + "</b>\r\n";
            return msg;
        }

    }
}