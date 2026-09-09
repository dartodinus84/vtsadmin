using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
namespace vtsadm
{
    public partial class job_maint_auto_details : System.Web.UI.Page
    {
        public string sJobID = "";
        public string sCustID = "";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_job_maint_vehicle '" + sCustID + "','" + txtSearch.Text.Trim() + "'";
                Session["RecListJobMaintDetails"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewCust(string sCustID, string sFullName)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer_maint_new_customer_search '" + sCustID + "','" + txtSearchCust.Text.Trim() + "'";
                Session["RecListJobMaintCustDetails"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingCust);
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewGsm(string sMSIDN)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_gsm_maint_new_gsm_search_auto '" + sMSIDN + "'";
            Session["RecListJobMaintGSMDetails"] = ClType.Open_GridView(GridView3, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingGsm);
        }
        private void Open_GridViewVehicle(string sCustID, string sPoliceNo)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_vehicle_maint_new_vehicle_search '" + sCustID + "','" + sPoliceNo + "'";
            Session["RecListJobMaintVehicleDetails"] = ClType.Open_GridView(GridView4, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingVehicle);
        }
        protected void Open_GridViewUser(GridView GrdVw, string sSQL, string sCustID, string sServerID, string sSessionName, Label LblPagingUserAccess)
        {
            try
            {
                ClsType ClType = new ClsType();

                string strSQL = sSQL + " '" + sCustID + "','" + sServerID + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        private void Open_GridViewDevice(string sNoSN)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_device_maint_new_device_search_auto '" + sNoSN + "'";
            Session["RecListJobMaintDeviceDetails"] = ClType.Open_GridView(GridView6, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDevice);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBMAINT"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                sJobID = Session["ClsJobIDMaint"].ToString();
                sCustID = Session["ClsCustIDMaint"].ToString();
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.InnerHtml = "";
                            clear();
                            Open_GridView();
                            Open_GridViewCust(sCustID, "");
                            Open_GridViewGsm("");
                            Open_GridViewDevice("");
                            Open_GridViewVehicle(sCustID, "");

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
        protected void Open_Combos(DropDownList cmbTemp, string stDBConn, string sSearch, string strSQL)
        {
            try
            {
                Recordset Rec = new Recordset(); string stSQL = ""; ListItem LstItem;
                stSQL = strSQL + " '" + sSearch + "'";
                Rec.Open(stSQL, stDBConn);
                cmbTemp.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                cmbTemp.Items.Add(LstItem);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();
                        LstItem.Value = Rec.Fields(0).Trim();
                        cmbTemp.Items.Add(LstItem);
                        Rec.MoveNext();
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

                txtTvdID.Value = "";
                txtPoliceNo.Text = "";
                Open_Combos(CmbMaintTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_job_order_maint_type_auto");
                CmbMaintTypeID.SelectedValue = "[Select]";
                txtRemark.Text = "";

                //gsm
                txtNewGsmID.Visible = false;
                txtNewGsmID.Text = "";
                CmdSearchGsm.Visible = false;
                LblGridView3.Visible = false;
                LblNewMSIDN.Visible = false;
                LblNewProviderName.Visible = false;
                LblNewSourceName.Visible = false;
                LblNewNewTgtID.Visible = false;
                LblStatusOldGsm.Visible = false;
                LblNewGSM.Visible = false;
                txtNewGSM.Text = "";
                txtNewMSIDN.Text = "";
                txtNewProviderName.Text = "";
                txtNewSourceName.Text = "";
                txtNewTgtID.Text = "";
                txtNewMSIDN.Text = "";
                ClType.Open_Combos(CmbStatusOldGsm, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_maint_gsm_status");
                CmbStatusOldGsm.SelectedValue = "[Select]";


                //customer
                txtSearchCust.Visible = false;
                txtSearchCust.Text = "";
                CmdSearchCust.Visible = false;
                LblGridView1.Visible = false;
                LblCustID.Visible = false;
                LblCustName.Visible = false;
                LblBranch.Visible = false;
                LblServer.Visible = false;
                txtNewCustID.Text = "";
                txtCustName.Text = "";
                txtBranch.Text = "";
                Open_Combos(CmbServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_maint_server");
                CmbServer.SelectedValue = "[Select]";
                CmdNewLoadData.Visible = false;

                //vehicle
                txtVehicleID.Visible = false;
                txtVehicleID.Text = "";
                CmdSearchVehicle.Visible = false;
                LblGridView4.Visible = false;
                LblVehicleID.Visible = false;
                LblVehicleDescription.Visible = false;
                LblPoliceNo.Visible = false;
                LblAssetNo.Visible = false;
                LblTvaID.Visible = false;
                LblStatusVehicle.Visible = false;
                txtNewVehicleID.Text = "";
                txtNewVehicleDesc.Text = "";
                txtNewPoliceNo.Text = "";
                txtNewAssetNo.Text = "";
                txtNewTvaID.Text = "";
                ClType.Open_Combos(CmbStatusOldVehicle, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_vehicle_maint_vehicle_status");
                CmbStatusOldVehicle.SelectedValue = "[Select]";

                //server
                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_server_maint_server");
                CmbCustServerID.SelectedValue = "[Select]";
                LblGridView5.Visible = false;
                LblCustServerID.Visible = false;

                //device
                LblDeviceServerID.Visible = false;
                txtSearchDevice.Visible = false;
                txtSearchDevice.Text = "";
                CmdSearchDevice.Visible = false;
                LblGridView6.Visible = false;
                LblNewDeviceID.Visible = false;
                LblNewNoSN.Visible = false;
                LblNewVendorName.Visible = false;
                LblNewDeviceTypeDesc.Visible = false;
                LblNewDeviceSourceName.Visible = false;
                LblNewWarehouseName.Visible = false;
                LblNewTdtID.Visible = false;
                LblNewWarehouseName.Visible = false;
                txtNewDeviceID.Text = "";
                txtNewNoSN.Text = "";
                txtNewVendorName.Text = "";
                txtNewDeviceTypeDesc.Text = "";
                txtNewDeviceSourceName.Text = "";
                txtNewWarehouseName.Text = "";
                txtNewTdtID.Text = "";
                LblGridView7.Visible = false;
                LblStatusOldDevice.Visible = false;
                ClType.Open_Combos(CmbDeviceServerID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_server_maint_server");
                CmbDeviceServerID.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbStatusOldDevice, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_maint_device_status");
                CmbStatusOldDevice.SelectedValue = "[Select]";

            }
            catch (Exception ex)
            {

            }
        }
        protected void CmbMaintTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {

                string sTvdID = txtTvdID.Value.Trim();
                txtTvdID.Attributes.Add("Value", sTvdID);
                string sPoliceNo = txtPoliceNo.Text.Trim();
                txtPoliceNo.Attributes.Add("Value", sPoliceNo);
                string sRemarks = txtRemark.Text.Trim();
                txtRemark.Attributes.Add("Value", sRemarks);

                if (CmbMaintTypeID.SelectedItem.Value == "MTY0000001")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    CmdSearchCust.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = true;
                    CmdSearchVehicle.Visible = true;
                    LblGridView4.Visible = true;
                    LblVehicleID.Visible = true;
                    LblVehicleDescription.Visible = true;
                    LblPoliceNo.Visible = true;
                    LblAssetNo.Visible = true;
                    LblTvaID.Visible = true;
                    LblStatusVehicle.Visible = true;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000002")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    CmdSearchCust.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;
                    CmdNewLoadData.Visible = true;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = true;
                    txtSearchDevice.Visible = true;
                    CmdSearchDevice.Visible = true;
                    LblGridView6.Visible = true;
                    LblNewDeviceID.Visible = true;
                    LblNewNoSN.Visible = true;
                    LblNewVendorName.Visible = true;
                    LblNewDeviceTypeDesc.Visible = true;
                    LblNewDeviceSourceName.Visible = true;
                    LblNewWarehouseName.Visible = true;
                    LblNewTdtID.Visible = true;
                    LblNewWarehouseName.Visible = true;
                    LblGridView7.Visible = true;
                    LblStatusOldDevice.Visible = true;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000003")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = true;
                    LblGridView3.Visible = true;
                    LblNewGSM.Visible = true;
                    LblNewMSIDN.Visible = true;
                    LblNewProviderName.Visible = true;
                    LblNewSourceName.Visible = true;
                    LblNewNewTgtID.Visible = true;
                    LblStatusOldGsm.Visible = true;
                    CmdSearchGsm.Visible = true;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000004")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000005")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000006")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;

                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000007")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;
                    CmdNewLoadData.Visible = true;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = true;
                    LblCustServerID.Visible = true;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000008")
                {
                    LblGridView1.Visible = true;
                    LblCustID.Visible = true;
                    LblCustName.Visible = true;
                    LblBranch.Visible = true;
                    LblServer.Visible = true;
                    CmdSearchCust.Visible = true;
                    txtSearchCust.Visible = true;
                    CmdNewLoadData.Visible = true;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000009")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000010")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000011")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    txtSearchCust.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                else if (CmbMaintTypeID.SelectedItem.Value == "MTY0000012")
                {
                    LblGridView1.Visible = false;
                    LblCustID.Visible = false;
                    LblCustName.Visible = false;
                    LblBranch.Visible = false;
                    LblServer.Visible = false;
                    CmdSearchCust.Visible = false;
                    txtSearchCust.Visible = false;

                    txtNewGsmID.Visible = false;
                    LblGridView3.Visible = false;
                    LblNewGSM.Visible = false;
                    LblNewMSIDN.Visible = false;
                    LblNewProviderName.Visible = false;
                    LblNewSourceName.Visible = false;
                    LblNewNewTgtID.Visible = false;
                    LblStatusOldGsm.Visible = false;
                    CmdSearchGsm.Visible = false;

                    txtVehicleID.Visible = false;
                    CmdSearchVehicle.Visible = false;
                    LblGridView4.Visible = false;
                    LblVehicleID.Visible = false;
                    LblVehicleDescription.Visible = false;
                    LblPoliceNo.Visible = false;
                    LblAssetNo.Visible = false;
                    LblTvaID.Visible = false;
                    LblStatusVehicle.Visible = false;

                    LblGridView5.Visible = false;
                    LblCustServerID.Visible = false;

                    LblDeviceServerID.Visible = false;
                    txtSearchDevice.Visible = false;
                    CmdSearchDevice.Visible = false;
                    LblGridView6.Visible = false;
                    LblNewDeviceID.Visible = false;
                    LblNewNoSN.Visible = false;
                    LblNewVendorName.Visible = false;
                    LblNewDeviceTypeDesc.Visible = false;
                    LblNewDeviceSourceName.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblNewTdtID.Visible = false;
                    LblNewWarehouseName.Visible = false;
                    LblGridView7.Visible = false;
                    LblStatusOldDevice.Visible = false;
                }
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }

        }
        protected void CmbMaintCustServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", sCustID, CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }

        }
        protected void CmbDeviceServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewUser(GridView7, "sp_get_interfacing_user_login", sCustID, CmbDeviceServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin2", LblPagingUser);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }

        }
        protected void CmbCustServerID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", txtNewCustID.Value.Trim(), CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }

        }
        protected void CmdNewLoadData_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbServer, Session["ClsTypeDBConnStringSQL"].ToString(), txtNewCustID.Text.Trim(), "sp_list_customer_maint_server");
                CmbServer.SelectedValue = "[Select]";

                ClType.Open_Combos(CmbCustServerID, Session["ClsTypeDBConnStringSQL"].ToString(), sCustID, "sp_list_server_maint_server");
                CmbCustServerID.SelectedValue = "[Select]";
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", sCustID, CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);

                ClType.Open_Combos(CmbDeviceServerID, Session["ClsTypeDBConnStringSQL"].ToString(), sCustID, "sp_list_server_maint_server");
                CmbDeviceServerID.SelectedValue = "[Select]";
                Open_GridViewUser(GridView7, "sp_get_interfacing_user_login", sCustID, CmbDeviceServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin2", LblPagingUser);

                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClearDetail_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridViewUser(GridView5, "sp_get_interfacing_user_login", sCustID, CmbCustServerID.SelectedItem.Value.Trim(), "RecListMaintUserLogin", LblPagingUserAccess);
                lblMsg.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                lblMsg.InnerHtml = "";
                if (sJobID.Trim() != "")
                {
                    if (txtTvdID.Value.Trim() != "")
                    {
                        if (CmbMaintTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtRemark.Text.Trim() != "")
                            {
                                if (CmbMaintTypeID.SelectedItem.Value.Trim() == "MTY0000001")
                                {
                                    if (txtNewVehicleID.Text.Trim() != "")
                                    {
                                        if (CmbStatusOldVehicle.SelectedItem.Value.Trim() != "[Select]")
                                        {
                                            strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                            {
                                                if (intAff > 0)
                                                {
                                                    checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), "", "", "", "", "", "", "", "", "", txtNewVehicleID.Text.Trim(), txtNewPoliceNo.Text.Trim(), CmbStatusOldVehicle.SelectedItem.Value.Trim());
                                                    clear();
                                                    Open_GridView();
                                                    lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                                }
                                            }
                                            else
                                            {
                                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Status Old Vehicle";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select New Vehicle ID";
                                    }

                                }
                                else if (CmbMaintTypeID.SelectedItem.Value.Trim() == "MTY0000002")
                                {
                                    if (txtNewDeviceID.Text.Trim() != "")
                                    {
                                        if (CmbStatusOldDevice.SelectedItem.Value.Trim() != "[Select]")
                                        {
                                            if (CmbDeviceServerID.SelectedItem.Value != "[Select]")
                                            {
                                                if (checkUserAccessDevice() == true)
                                                {
                                                    strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                                    {
                                                        if (intAff > 0)
                                                        {
                                                            checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), "", "", "", "", "", "", txtNewDeviceID.Text.Trim(), txtNewNoSN.Text.Trim(), CmbStatusOldDevice.SelectedItem.Value.Trim(), "", "", "");
                                                            clear();
                                                            Open_GridView();
                                                            lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                                    }
                                                }
                                                else
                                                {
                                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select User TMS";
                                                }
                                            }
                                            else
                                            {
                                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Server TMS";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Status Old Vehicle";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select New Vehicle ID";
                                    }

                                }
                                else if (CmbMaintTypeID.SelectedItem.Value.Trim() == "MTY0000003")
                                {
                                    if (txtNewGSM.Text.Trim() != "")
                                    {
                                        if (CmbStatusOldGsm.SelectedItem.Value.Trim() != "[Select]")
                                        {
                                            strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                            {
                                                if (intAff > 0)
                                                {
                                                    checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), "", "", "", txtNewGSM.Text.Trim(), txtNewMSIDN.Text.Trim(), CmbStatusOldGsm.SelectedItem.Value.Trim(), "", "", "", "", "", "");
                                                    clear();
                                                    Open_GridView();
                                                    lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                                }
                                            }
                                            else
                                            {
                                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Status Old GSM";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select New Gsm ID";
                                    }

                                }
                                else if (CmbMaintTypeID.SelectedItem.Value.Trim() == "MTY0000007")
                                {
                                    if (CmbCustServerID.SelectedItem.Value != "[Select]")
                                    {
                                        if (checkUserAccess() == true)
                                        {
                                            strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                            {
                                                if (intAff > 0)
                                                {
                                                    checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), "", "", CmbCustServerID.SelectedItem.Value.Trim(), "", "", "", "", "", "", "", "", "");
                                                    clear();
                                                    Open_GridView();
                                                    lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                                }
                                            }
                                            else
                                            {
                                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select User TMS";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Server";
                                    }

                                }
                                else if (CmbMaintTypeID.SelectedItem.Value.Trim() == "MTY0000008")
                                {
                                    if (txtNewCustID.Text.Trim() != "")
                                    {
                                        if (CmbServer.SelectedItem.Value != "[Select]")
                                        {
                                            strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                            {
                                                if (intAff > 0)
                                                {
                                                    checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), txtNewCustID.Text.Trim(), txtCustName.Text.Trim(), "", "", "", "", "", "", "", "", "", "");
                                                    clear();
                                                    Open_GridView();
                                                    lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                                }
                                            }
                                            else
                                            {
                                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Server";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance , please select Customer";
                                    }
                                }
                                else
                                {
                                    strSQL = "sp_insert_job_order_maint_detail_auto '" + sJobID.Trim() + "','" + txtTvdID.Value.Trim() + "','" + CmbMaintTypeID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            checkJO(sJobID.Trim(), txtTvdID.Value.Trim(), "", "", "", "", "", "", "", "", "", "", "", "");
                                            clear();
                                            Open_GridView();
                                            lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                    }
                                }
                            }
                            else
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Please fill remarks";
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please select vehicle first";
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select vehicle first";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please create job header first";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + ex.Message + ")";
            }
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintDetails"], LblPaging);
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
        protected void CmdSearchCust_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewCust(sCustID, txtSearchCust.Text.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearchGsm_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewGsm(txtNewGsmID.Text.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearchVehicle_ServerClic(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewVehicle(sCustID, txtVehicleID.Text.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearchDevice_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewDevice(txtSearchDevice.Text.Trim());
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
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "');return false;";
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
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintCustDetails"], LblPagingCust);
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelectCust");
                    CmdButton.OnClientClick = "postDetailsCust('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void GridView3_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintGSMDetails"], LblPagingGsm);

        }
        protected void GridView3_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i <= 0; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelectGsm");
                    CmdButton.OnClientClick = "postNewGsmChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "');return false;";
                    for (int i = 0; i <= 0; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView4_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintVehicleDetails"], LblPaging);
        }
        protected void GridView4_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelectVehicle");
                    CmdButton.OnClientClick = "postNewVehicleChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView5_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListMaintUserLogin"], LblPagingUserAccess);
        }
        protected void GridView6_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintDeviceDetails"], LblPaging);
        }
        protected void GridView6_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i <= 0; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelectDevice");
                    CmdButton.OnClientClick = "postNewDeviceChild('" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[5].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[0].Text.ToString() + "');return false;";
                    for (int i = 0; i <= 0; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView7_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListMaintUserLogin2"], LblPagingUser);
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
        protected void saveUserAccess(string sMsidn)
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
                        strSQL = "sp_insert_interfacing_user_access '" + sMsidn + "','" + sAutoID + "','" + CmbCustServerID.SelectedItem.Value.Trim() + "'";
                        Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected bool checkUserAccessDevice()
        {
            bool boolOK = false;
            try
            {
                div_comment.InnerHtml = "";
                for (int i = 0; i < GridView7.Rows.Count; i++)
                {
                    CheckBox ChkBox = (CheckBox)GridView7.Rows[i].Cells[3].FindControl("Chk2");
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
        protected void saveUserAccessDevice(string sMsidn)
        {
            try
            {
                div_comment.InnerHtml = "";
                string sAutoID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView7.Rows.Count; i++)
                {
                    sAutoID = GridView7.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView7.Rows[i].Cells[2].FindControl("Chk2");
                    if (ChkBox.Checked == true)
                    {
                        strSQL = "sp_insert_interfacing_user_access '" + sMsidn + "','" + sAutoID + "','" + CmbDeviceServerID.SelectedItem.Value.Trim() + "'";
                        Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void checkJO(string joid, string tvdid, string sCustID, string sNewCust, string sServerid, string sGsmID, string sNewGSM, string sStatusGsm, string sDeviceID, string sNewSN, string sStatusDecive, string sVehicleID, string sNewPoliceNo, string sStatusVehicle)
        {

            try
            {
                var pageData = new job_maint_auto_details();
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_maint_suspend_status '" + joid + "','RG','" + tvdid + "'";
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
                        string sTvdid = Rec.Fields("TvdID");
                        string sMaintTypeID = Rec.Fields("MaintTypeID");
                        string sMaintTypeDesc = Rec.Fields("MaintTypeDesc");
                        string sBranchDesc = Rec.Fields("BranchName");

                        if (sMaintTypeID == "MTY0000001")
                        {
                            vehicle(sTvdid, sJoID, sVehicleID, sNewPoliceNo, sStatusVehicle);
                            notifTelegramVehicle(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewPoliceNo, sBranchDesc);
                        }
                        else if (sMaintTypeID == "MTY0000002")
                        {
                            device(sTvdid, sJoID, sDeviceID, sNewSN, sStatusDecive);
                            notifTelegramDevice(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewSN, sBranchDesc);
                            saveUserAccessDevice(sNewSN);
                        }
                        else if (sMaintTypeID == "MTY0000003")
                        {
                            gsm(sTvdid, sJoID, sGsmID, sNewGSM, sStatusGsm);
                            notifTelegramGsm(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewGSM, sBranchDesc);
                        }
                        else if (sMaintTypeID == "MTY0000006")
                        {
                            uninstall(sTvdid, sJoID);
                            notifTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);

                        }
                        else if (sMaintTypeID == "MTY0000007")
                        {
                            server(sTvdid, sJoID, sServerid);
                            saveUserAccess(sMSIDN);
                            notifTelegramServer(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sServerid, sBranchDesc);
                        }
                        else if (sMaintTypeID == "MTY0000008")
                        {
                            customer(sTvdid, sJoID, sCustID);
                            notifTelegramCust(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewCust, sBranchDesc);
                        }
                        else if (sMaintTypeID == "MTY0000009")
                        {
                            if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dara" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                            {
                                softblock(sTvdid, sJoID);
                                notifTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);
                            }
                            else
                            {
                                //notifTelegramApproval(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM);
                                sendEmail(sTvdid, sJoID);
                            }
                        }
                        else if (sMaintTypeID == "MTY0000010")
                        {
                            if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dara" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                            {
                                unblock(sTvdid, sJoID);
                                notifTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);
                            }
                            else
                            {
                                //notifTelegramApproval(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM);
                                sendEmail(sTvdid, sJoID);
                            }
                        }
                        else if (sMaintTypeID == "MTY0000011")
                        {
                            if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dara" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                            {
                                suspend(sTvdid, sJoID);
                                notifTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);
                            }
                            else
                            {
                                //notifTelegramApproval(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM);
                                sendEmail(sTvdid, sJoID);
                            }
                        }
                        else if (sMaintTypeID == "MTY0000012")
                        {
                            if (Session["ClsTypeUserID"].ToString() == "dodi" || Session["ClsTypeUserID"].ToString() == "dara" || Session["ClsTypeUserID"].ToString() == "dana" || Session["ClsTypeUserID"].ToString() == "admin")
                            {
                                reaktivasi(sTvdid, sJoID);
                                notifTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);
                            }
                            else
                            {
                                //notifTelegramApproval(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM);
                                sendEmail(sTvdid, sJoID);
                            }
                        }
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void sendEmail(string sTvdid, string sJoID)
        {

            try
            {
                var pageData = new job_maint_auto_details();
                Recordset RecMail = new Recordset();
                string strSQL = "sp_get_maint_suspend_status '" + sJoID + "','RG','" + sTvdid + "'";
                RecMail.Open(strSQL, pageData.DBConnstringSQL());
                if (RecMail.RecordCount() > 0)
                {
                    RecMail.MoveFirst();
                    while (!RecMail.EOF)
                    {

                        string sCustomerName = RecMail.Fields("Fullname");
                        string sRemark = RecMail.Fields("Remark");
                        string sSchDate = RecMail.Fields("SchDate");
                        string sPoliceNo = RecMail.Fields("policeno");
                        string sMSIDN = RecMail.Fields("nosn");
                        string sGSM = RecMail.Fields("msidn");
                        string sMaintTypeID = RecMail.Fields("MaintTypeID");
                        string sMaintTypeDesc = RecMail.Fields("MaintTypeDesc");

                        notifEmail(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdid);
                        RecMail.MoveNext();
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdid)
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
                mail.Subject = sJobID + " Approval - Job Order Maintenance ";
                mail.Body = BodyEmail(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sTvdid);

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
        private string BodyEmail(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sTvdid)
        {
            string url = "https://vtsadmin.easygo-gps.co.id/job_maint_approve.aspx?JobID=" + sJoID + "TvdID=" + sTvdid;
            string urlcancel = "https://vtsadmin.easygo-gps.co.id/job_maint_.aspx?JobID=" + sJoID + "TvdID=" + sTvdid;

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
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear Tim, <b>" + Session["ClsTypeUserID"].ToString().ToUpper() + "</b> telah melakukan pengajuan Job Maintenance <b>" + sMaintTypeDesc.ToUpper() + "</b> </p>";
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
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Car Plate</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sPoliceNo + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>IMEI</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sMSIDN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Remarks</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sRemark + "</span></td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            //Header += "<div style='padding-top:32px;text-align:center'><a href ='" + url + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#00ab6b;border-radius:5px;min-width:100%' target = '_blank'> Approve Job Order</a></div>";
            //Header += "<div style='padding-top:32px;text-align:center'><a href ='" + urlcancel + "' style ='line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:14px;display:inline-block;padding:10px 10px 10px 10px;background-color:#ab1100;border-radius:5px;min-width:100%' target = '_blank'> Reject Job Order</a></div>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }
        private void vehicle(string sTvdid, string sJoID, string sVehicleID, string sNewPoliceNo, string sStatusVehicle)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_vehicle_maint_auto '" + sTvdid + "','" + sVehicleID + "','" + sJoID + "','" + sStatusVehicle + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Vehicle maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Vehicle maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_vehicle_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Vehicle maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void device(string sTvdid, string sJoID, string sDeviceID, string sNewSN, string sStatusOld)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_device_maint_new_auto '" + sTvdid + "','" + sDeviceID + "','" + sJoID + "','" + sStatusOld + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Device maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Device maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_device_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Device maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void gsm(string sTvdid, string sJoID, string sGsmID, string sNewGSM, string sStatusOld)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_gsm_maint_new_auto '" + sTvdid + "','" + sGsmID + "','" + sJoID + "','" + sStatusOld + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>GSM maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving GSM maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_gsm_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving GSM maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void uninstall(string sTvdid, string sJoID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_uninstall_maint_new_auto '" + sTvdid + "','" + sJoID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Uninstall maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Uninstall maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_uninstall_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Uninstall maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void server(string sTvdid, string sJoID, string sServerID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_server_maint_auto '" + sTvdid + "','" + sJoID + "','" + sServerID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Server maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Server maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_server_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Server maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void customer(string sTvdid, string sJoID, string sCustIDNew)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_customer_maint_auto '" + sTvdid + "','" + txtNewCustID.Text.Trim() + "','" + sJoID + "','" + CmbServer.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Customer maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Customer maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_sp_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Customer maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void softblock(string sTvdid, string sJoID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_sb_maint_auto '" + sTvdid + "','" + sJoID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Softblock maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Softblock maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_sb_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Softblock maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void unblock(string sTvdid, string sJoID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_ub_maint_auto '" + sTvdid + "','" + sJoID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Unblock maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Unblock maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_ub_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Unblock maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void suspend(string sTvdid, string sJoID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_sp_maint_auto '" + sTvdid + "','" + sJoID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Suspended maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving suspended maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_sp_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving suspended maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void reaktivasi(string sTvdid, string sJoID)
        {
            try
            {
                Int32 intAff1 = 0; String strSQL1 = ""; string sErr1 = "";
                ExecCommand ec = new ExecCommand();
                strSQL1 = "sp_insert_re_maint_auto '" + sTvdid + "','" + sJoID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1))
                {
                    if (intAff1 > 0)
                    {

                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Reactivated maintenance has been save successfully</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Reactivated maintenance has been failed</div>";
                    }
                }
                else
                {
                    strSQL1 = "sp_insert_re_maint_rollback '" + sTvdid + "'";
                    ec.Execute(strSQL1, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff1, ref sErr1);
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving Reactivated maintenance has been failed (" + sErr1 + ")</div>";
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM,string sBranchDesc)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";
                string chatid = "";


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
                string text = BodyTelegram(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sBranchDesc);
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
        private string BodyTelegram(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm,string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramApproval(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM)
        {
            try
            {
                string sChatID = "";
                string apitoken = "";
                string url = "";
                string chatid = "";

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
                string text = BodyTelegramApproval(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM);
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
        private string BodyTelegramApproval(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " CREATE</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramCust(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewCust,string sBranchDesc)
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

                string text = BodyTelegramCust(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewCust, sBranchDesc);
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
        private string BodyTelegramCust(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string custname,string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Customer</b>\r\n";
            msg += "<b>" + custname + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramApproveCust(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewCustID)
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

                string strSQLtelegram4 = "sp_list_par_customer'" + sNewCustID + "'";
                Recordset RecCustName = new Recordset();
                RecCustName.Open(strSQLtelegram4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecCustName.RecordCount() > 0)
                {
                    fullname = RecCustName.Fields("Fullname");
                }

                string urlString = url;
                string apiToken = apitoken;
                string chatId = chatid;
                string sNewCust = fullname;
                string text = BodyTelegramApproveCust(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewCust);
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
        private string BodyTelegramApproveCust(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string custname)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " CREATE</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Customer</b>\r\n";
            msg += "<b>" + custname + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramServer(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sServerID,string sBranchDesc)
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

                string strSQLtelegram4 = "sp_list_par_server '" + sServerID + "'";
                Recordset RecCustName = new Recordset();
                RecCustName.Open(strSQLtelegram4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecCustName.RecordCount() > 0)
                {
                    fullname = RecCustName.Fields("ServerName");
                }

                string urlString = url;
                string apiToken = apitoken;
                string chatId = chatid;
                string sServer = fullname;
                string text = BodyTelegramServer(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sServer, sBranchDesc);
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
        private string BodyTelegramServer(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string sServer,string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Server</b>\r\n";
            msg += "<b>" + sServer + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramApproveServer(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewCustID)
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

                string strSQLtelegram4 = "sp_list_par_server '" + sNewCustID + "'";
                Recordset RecCustName = new Recordset();
                RecCustName.Open(strSQLtelegram4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecCustName.RecordCount() > 0)
                {
                    fullname = RecCustName.Fields("Fullname");
                }

                string urlString = url;
                string apiToken = apitoken;
                string chatId = chatid;
                string sNewServer = fullname;
                string text = BodyTelegramApproveServer(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewServer);
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
        private string BodyTelegramApproveServer(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string sNewServer)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " CREATE</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Server</b>\r\n";
            msg += "<b>" + sNewServer + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";
            return msg;
        }
        protected void notifTelegramGsm(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewGsm,string sBranchDesc)
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
                string text = BodyTelegramGsm(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewGsm, sBranchDesc);
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
        private string BodyTelegramGsm(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string sNewGsm,string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New GSM</b>\r\n";
            msg += "<b>" + sNewGsm + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramVehicle(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewPoliceNo, string sBranchDesc)
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
                string text = BodyTelegramVehicle(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewPoliceNo, sBranchDesc);
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
        private string BodyTelegramVehicle(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string sNewPoliceNo, string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Police No</b>\r\n";
            msg += "<b>" + sNewPoliceNo + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
        protected void notifTelegramDevice(string sMaintTypeDesc, string sJoID, string sCustomerName, string sRemark, string sSchDate, string sPoliceNo, string sMSIDN, string sGSM, string sNewSN,string sBranchDesc)
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
                string text = BodyTelegramDevice(sMaintTypeDesc, sJoID, sCustomerName, sRemark, sSchDate, sPoliceNo, sMSIDN, sGSM, sNewSN, sBranchDesc);
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
        private string BodyTelegramDevice(string sMaintTypeDesc, string jono, string cus, string remark, string jodate, string plat, string sn, string gsm, string sNewSN,string sBranchDesc)
        {
            string msg = "";
            msg += "<b>JOB ORDER MAINTENANCE " + sMaintTypeDesc.ToUpper() + " SUCCESS</b>\r\n";
            msg += "<b>Job Order Number</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Job Order Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Customer Branch</b>\r\n";
            msg += "<b>" + sBranchDesc + "</b>\r\n";
            msg += "<b>Car Plate</b>\r\n";
            msg += "<b>" + plat + "</b>\r\n";
            msg += "<b>GSM Number</b>\r\n";
            msg += "<b>" + gsm + "</b>\r\n";
            msg += "<b>Device SN</b>\r\n";
            msg += "<b>" + sn + "</b>\r\n";
            msg += "<b>New Device SN</b>\r\n";
            msg += "<b>" + sNewSN + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + remark + "</b>\r\n";
            msg += "<b>User Create</b>\r\n";
            msg += "<b>" + Session["ClsTypeUserID"].ToString() + "</b>\r\n";
            msg += "<b>CC</b>\r\n";
            msg += "<b>@CS_EasyGoGPS @cyndiasan @Latikafauziah @dodiiiiiiiiiiiiiiiiii</b>\r\n";

            return msg;
        }
    }
}
