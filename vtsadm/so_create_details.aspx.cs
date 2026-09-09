using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class so_create_details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.InnerHtml = "";
                            clear();
                            //Open_GridView();
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
                //txtSeq.Text = "";
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_purchase_order_device_group");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_purchase_order_device_type");
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtQuantity.Value = "";
                txtRemark.Text = "";
                txtPrice.Value = "0";
                txtInstallFee.Value = "0";
                txtMonthlyFee.Value = "0";
                ClType.Open_Combos(CmbPricePPN, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_yesno");
                CmbPricePPN.SelectedValue = "0";
                ClType.Open_Combos(CmbInstallFeePPN, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_yesno");
                CmbInstallFeePPN.SelectedValue = "0";
                ClType.Open_Combos(CmbMonthlyFeePPN, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_yesno");
                CmbMonthlyFeePPN.SelectedValue = "0";

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
                lblMsg.InnerHtml = "";
                if (txtPoID.Value.Trim() != "")
                {
                    if (CmbDeviceGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtQuantity.Value.Trim() != "")
                            {
                                if (txtPrice.Value.Trim() != "" && txtInstallFee.Value.Trim() != "" && txtMonthlyFee.Value.Trim() != "")
                                {
                                    if (CmbPricePPN.SelectedItem.Value.Trim() != "[Select]" && CmbInstallFeePPN.SelectedItem.Value.Trim() != "[Select]" && CmbMonthlyFeePPN.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                        ExecCommand ec = new ExecCommand();
                                        strSQL = "sp_insert_purchase_order_detail '" + txtPoID.Value.Trim() + "','" + CmbDeviceGroupID.SelectedItem.Value.Trim() + "'," +
                                                 "'" + CmbDeviceTypeID.SelectedItem.Value.Trim() + "','" + txtQuantity.Value.Trim() + "'," + txtPrice.Value.Trim() + "," +
                                                 "" + txtInstallFee.Value.Trim() + "," + txtMonthlyFee.Value.Trim() + ",'" + CmbPricePPN.SelectedItem.Value.Trim() + "'," +
                                                 "'" + CmbInstallFeePPN.SelectedItem.Value.Trim() + "','" + CmbMonthlyFeePPN.SelectedItem.Value.Trim() + "'," +
                                                 "'" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                lblMsg.InnerHtml = "<strong>Success!</strong> Create purchase order detail has been save successfully!";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> " + sErr;
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> All PPN should be filled";
                                    }
                                }
                                else
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> All price and fees should be filled";
                                }
                            }
                            else
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Please fill in quantity";
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please select device type";
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select device group";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please create purchase order header first";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> " + ex.Message;
            }
        }

        protected void CmbDeviceGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_purchase_order_device_type");
                lblMsg.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
    }
}