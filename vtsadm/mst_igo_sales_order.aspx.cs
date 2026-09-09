using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_sales_order : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        string txtEmailCust = "";
        public mst_igo_sales_order()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_order '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListDeviceFieldSort"] = "order_id";
                ViewState["RecListDeviceDirSort"] = "DESC";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGOSALEORDER"))
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

                ClType.Open_Combos(CmbMarketID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_igo_master_seller");
                ClType.Open_Combos(CmbSourceMarekting, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_source_mitra");
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_mitra");
                ClType.Open_Combos(CmbTypeSalesID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_type_order");
                ClType.Open_Combos(CmbDeliveryID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_igo_master_xpedition");
                ClType.Open_Combos(CmdPriceItem, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_igo_master_orice_bundle");

                CmbMarketID.SelectedValue = "[Select]";
                CmbSourceMarekting.SelectedValue = "[Select]";
                CmdPriceItem.SelectedValue = "[Select]";
                CmbTypeSalesID.SelectedValue = "[Select]";
                CmbMarketing.SelectedValue = "[Select]";
                CmbDeliveryID.SelectedValue = "[Select]";

                CmdPriceItem.Attributes.Add("style", "display:none");
                admDivCheck.Attributes.Add("style", "display:none");
                txtCustEmail.Attributes["type"] = "email";

                txtNoInv.Text = "";
                txtCustName.Text = "";
                txtCustAdd.Text = "";
                txtCustEmail.Text = "";
                txtCustAdd.Text = "";
                txtCount.Text = "0";
                txtResi.Text = "";
                txtCustPhone.Text = "";
                txtQty12.Text = "0";
                txtQty24.Text = "0";

                Session["ClsTypeSalesOrderPicture"] = "";
                Session["ClsTypeSalesOrderPictureResi"] = "";
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
                Int32 intAff = 0; Int32 intAff1 = 0; String strSQL = ""; String strSQL1 = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (CmbTypeSalesID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (txtNoInv.Text.Trim() != "")
                        {
                            if (CmbMarketID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (txtCustName.Text.Trim() != "")
                                {
                                    if (txtCustEmail.Text.Trim() != "" && txtCustEmail.Text.Contains("@"))
                                    {
                                        txtEmailCust = txtCustEmail.Text.Replace(" ", "");
                                        if (txtCustAdd.Text.Trim() != "")
                                        {
                                            if (CmbSourceMarekting.SelectedItem.Value.Trim() != "[Select]")
                                            {
                                                if (txtCount.Text.Trim() != "")
                                                {
                                                    if (txtQty12.Text.Trim() != "")
                                                    {
                                                        if (txtQty24.Text.Trim() != "")
                                                        {
                                                            strSQL = "usp_retail_sales_order_new '" + CmbTypeSalesID.SelectedItem.Value.ToString() + "','" + CmbMarketID.SelectedItem.Value.ToString() + "','" + CmbSourceMarekting.SelectedItem.Value.ToString() + "','" + CmbMarketing.SelectedItem.Value.ToString() + "','" + txtNoInv.Text.Trim() + "','" + txtCustName.Text.Trim() + "','" + txtCustAdd.Text.Trim() + "','" + txtEmailCust + "','" + txtCustPhone.Text.Trim() + "','" + CmbDeliveryID.SelectedItem.Value.ToString() + "','" + txtResi.Text.Trim() + "','" + CmdPriceItem.SelectedItem.Value.Trim() + "','" + txtCount.Text.Trim() + "','" + Session["ClsTypeSalesOrderPicture"].ToString() + "','" + Session["ClsTypeSalesOrderPictureResi"].ToString() + "','" + txtQty12.Text.Trim() + "','" + txtQty24.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                                            {
                                                                if (intAff > 0)
                                                                {
                                                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Sales Order has been save successfully!</div>";
                                                                    sendTelegram(txtNoInv.Text.Trim());
                                                                    clear();
                                                                    Open_GridView();
                                                                }
                                                                else
                                                                {
                                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sales Order has been failed</div>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sales Order has been failed (" + sErr + ")</div>";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Input Quantity Relay 24 V</div>";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Input Quantity  Relay 12 V</div>";
                                                    }
                                                }
                                                else
                                                {
                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Input Quantity</div>";
                                                }
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Select Source Marketing</div>";
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
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Market</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Invoice Market</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Type Sales Order</div>";
                    }
                }
                else
                {
                    if (txtOrderID.Text.Trim() != "")
                    {
                        if (CmbTypeSalesID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtNoInv.Text.Trim() != "")
                            {
                                if (CmbMarketID.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (txtCustName.Text.Trim() != "")
                                    {
                                        if (txtCustEmail.Text.Trim() != "" && txtCustEmail.Text.Contains("@"))
                                        {
                                            txtEmailCust = txtCustEmail.Text.Replace(" ", "");
                                            if (txtCustAdd.Text.Trim() != "")
                                            {
                                                if (CmbSourceMarekting.SelectedItem.Value.Trim() != "[Select]")
                                                {
                                                    if (txtCount.Text.Trim() != "")
                                                    {
                                                        if (txtQty12.Text.Trim() != "")
                                                        {
                                                            if (txtQty24.Text.Trim() != "")
                                                            {
                                                                strSQL = "usp_update_retail_sales_order_new '" + txtOrderID.Text.Trim() + "','" + CmbTypeSalesID.SelectedItem.Value.ToString() + "','" + CmbMarketID.SelectedItem.Value.ToString() + "','" + CmbSourceMarekting.SelectedItem.Value.ToString() + "','" + CmbMarketing.SelectedItem.Value.ToString() + "','" + txtNoInv.Text.Trim() + "','" + txtCustName.Text.Trim() + "','" + txtCustAdd.Text.Trim() + "','" + txtEmailCust + "','" + txtCustPhone.Text.Trim() + "','" + CmbDeliveryID.SelectedItem.Value.ToString() + "','" + txtResi.Text.Trim() + "','" + CmdPriceItem.SelectedItem.Value.Trim() + "','" + txtCount.Text.Trim() + "','" + Session["ClsTypeSalesOrderPicture"].ToString() + "','" + Session["ClsTypeSalesOrderPictureResi"].ToString() + "','" + txtQty12.Text.Trim() + "','" + txtQty24.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                                                {
                                                                    if (intAff > 0)
                                                                    {
                                                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Sales Order has been update successfully!</div>";
                                                                        sendTelegram(txtNoInv.Text.Trim());
                                                                        clear();
                                                                        Open_GridView();
                                                                    }
                                                                    else
                                                                    {
                                                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sales Order has been failed</div>";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sales Order has been failed (" + sErr + ")</div>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Relay 24 V</div>";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Relay 24 V</div>";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Quantity</div>";
                                                    }
                                                }
                                                else
                                                {
                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Source Marketing / Agent</div>";
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
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Seller (Marketplace)</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Invoice</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Sales Order Type</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Sales Order</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sales Order has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sOrderID = ""; string sOrderTypeID = ""; string sSellerID = ""; string sInv = ""; string sRecipt = "";
                string sMitraTypeID = ""; string sMitraID = ""; string sStatus = ""; string sExpeditionID = ""; string sQuantity = ""; string sDateArrival = "";
                string sCustName = ""; string sCustAdd = ""; string sCustEmail = ""; string sCustPhone = ""; string sRelay12 = ""; string sRelay24 = ""; string sPrice = "";

                sOrderID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sQuantity = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sInv = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                sCustName = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sCustAdd = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sCustEmail = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sCustPhone = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();

                sSellerID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sOrderTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sMitraTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sMitraID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();

                sExpeditionID = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sRecipt = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();

                sRelay12 = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sRelay24 = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();
                sPrice = (e.CommandSource as GridView).Rows[iRow].Cells[21].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "REGISTERED")
                        {
                            txtOrderID.Text = sOrderID;
                            CmbTypeSalesID.SelectedValue = sOrderTypeID;
                            CmbMarketID.SelectedValue = sSellerID;
                            CmbSourceMarekting.SelectedValue = sMitraTypeID;
                            ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbSourceMarekting.SelectedItem.Value.ToString(), "sp_list_mitra");
                            CmbMarketing.SelectedValue = sMitraID;
                            CmbDeliveryID.SelectedValue = sExpeditionID;
                            txtResi.Text = sRecipt;
                            txtNoInv.Text = sInv;
                            txtCount.Text = sQuantity;
                            txtCustName.Text = sCustName;
                            txtCustAdd.Text = sCustAdd;
                            txtCustEmail.Text = sCustEmail;
                            txtCustPhone.Text = sCustPhone;
                            txtQty12.Text = sRelay12;
                            if (sOrderTypeID == "1" || sOrderTypeID == "11")
                            {
                                CmdPriceItem.Attributes.Add("style", "display:block");
                                admDivCheck.Attributes.Add("style", "display:block");
                                CmdPriceItem.SelectedValue = sPrice;
                            }
                            else
                            {
                                CmdPriceItem.Attributes.Add("style", "display:none");
                                admDivCheck.Attributes.Add("style", "display:none");
                                CmdPriceItem.SelectedValue = "[Select]";
                            }
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited or deleted, (" + ex.Message + ")</div>";
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
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 12; i <= 21; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 12; i <= 21; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[22].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[23].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "'); return false;";

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
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtSalesOrderIDDelete.Value.Trim() != "")
                {
                    strSQL = "usp_delete_retail_sales_order_new '" + txtSalesOrderIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Sales Order has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing sales order has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete sales order has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete sales order has been failed (" + ex.Message + ")</div>";
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
        protected void CmbSourceMarekting_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbSourceMarekting.SelectedItem.Value.ToString(), "sp_list_mitra");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbTypeSalesID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("1") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("11") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("13") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("24") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("26") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("27") || Convert.ToString(CmbTypeSalesID.SelectedItem.Value).Equals("22"))
                {
                    CmdPriceItem.Attributes.Add("style", "display:block");
                    admDivCheck.Attributes.Add("style", "display:block");
                }
                else
                {
                    //CmdPriceItem.Attributes.Add("style", "display:none");
                    //admDivCheck.Attributes.Add("style", "display:none");

                    CmdPriceItem.Attributes.Add("style", "display:block");
                    admDivCheck.Attributes.Add("style", "display:block");
                }

                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        private void sendTelegram(string Inv)
        {
            try
            {
                string strSQL = "sp_get_sales_order '" + Inv + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string sOrderID = Rec.Fields("order_id");
                        string sCustName = Rec.Fields("seller_cust_name");
                        string sCustEmail = Rec.Fields("seller_cust_email");
                        string sInvoice = Rec.Fields("seller_invoice");
                        string sCustPhone = Rec.Fields("seller_cust_phone");
                        string sQuantity = Rec.Fields("quantity");
                        string sSalesOrderType = Rec.Fields("order_type_desc");
                        string sAgentName = Rec.Fields("mitra_name");
                        string sSeller = Rec.Fields("Seller");
                        notifTelegram(sOrderID, sCustName, sCustEmail, sInvoice, sCustPhone, sQuantity, sSalesOrderType, sAgentName, sSeller);
                        Rec.MoveNext();
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void notifTelegram(string sOrderID, string sCustName, string sCustEmail, string sInvoice, string sCustPhone, string sQuantity, string sSalesOrderType, string sAgentName, string sSeller)
        {
            string apitoken = "";
            string url = "";
            string chatid = "";
            try
            {
                string strSQLtelegram = "sp_list_par_global 'TelegramChatID5'";
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
                string apiToken = apitoken;
                string chatId = chatid;
                

                foreach (var sChatID in chatId.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string urlString = "";
                    string text = BodyTelegram(sOrderID, sCustName, sCustEmail, sInvoice, sCustPhone, sQuantity, sSalesOrderType, sAgentName, sSeller);
                    urlString = String.Format(url, apiToken, sChatID, text);
                    WebClient webclient = new WebClient();
                    webclient.DownloadString(urlString);

                }
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string sOrderID, string sCustName, string sCustEmail, string sInvoice, string sCustPhone, string sQuantity, string sSalesOrderType, string sAgentName, string sSeller)
        {
            string msg = "";
            msg += "<b>NEW SALES ORDER IGO TRACKER</b>\r\n";
            msg += "<b>Sales Oder ID</b>\r\n";
            msg += "<b>" + sOrderID + "</b>\r\n";
            msg += "<b>Source Sales Oder</b>\r\n";
            msg += "<b>" + sSeller + "</b>\r\n";
            msg += "<b>Sales Order Name</b>\r\n";
            msg += "<b>" + sSalesOrderType + "</b>\r\n";
            msg += "<b>Customer Name</b>\r\n";
            msg += "<b>" + sCustName + "</b>\r\n";
            msg += "<b>Customer Email</b>\r\n";
            msg += "<b>" + sCustEmail + "</b>\r\n";
            msg += "<b>Customer Phone</b>\r\n";
            msg += "<b>" + sCustPhone + "</b>\r\n";
            msg += "<b>Invoice</b>\r\n";
            msg += "<b>" + sInvoice + "</b>\r\n";
            msg += "<b>Quantity</b>\r\n";
            msg += "<b>" + sQuantity + "</b>\r\n";
            msg += "<b>Agent / Marketing</b>\r\n";
            msg += "<b>" + sAgentName + "</b>\r\n";
            return msg;
        }

    }
}
