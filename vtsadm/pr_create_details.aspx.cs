using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_create_details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                var access = Session["ClsTypeAccessMenu"].ToString().ToUpper();
                if (!access.Contains("MNUPRCREATE") && !access.Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.Text = "";
                            clear();
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
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_pur_order_device_group");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_pur_order_device_type");
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtQuantity.Value = "";
                txtPrice.Value = "0";
                txtRemark.Text = "";
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
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if (txtPurID.Value.Trim() != "")
                {
                    if (CmbDeviceGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtQuantity.Value.Trim() != "" && txtPrice.Value.Trim() != "")
                            {
                                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                ExecCommand ec = new ExecCommand();
                                strSQL = "sp_insert_pur_order_detail '" + txtPurID.Value.Trim() + "','" + CmbDeviceGroupID.SelectedItem.Value.Trim() + "'," +
                                         "'" + CmbDeviceTypeID.SelectedItem.Value.Trim() + "','" + txtQuantity.Value.Trim() + "'," + txtPrice.Value.Trim() + "," +
                                         "'" + txtRemark.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        clear();
                                        lblMsg.Text = "<strong>Success!</strong> Create purchase request detail has been saved successfully!";
                                    }
                                }
                                else
                                {
                                    lblMsg.Text = "<strong>Failed!</strong> " + sErr;
                                }
                            }
                            else
                            {
                                lblMsg.Text = "<strong>Failed!</strong> Please fill in quantity and price";
                            }
                        }
                        else
                        {
                            lblMsg.Text = "<strong>Failed!</strong> Please select device type";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "<strong>Failed!</strong> Please select device group";
                    }
                }
                else
                {
                    lblMsg.Text = "<strong>Failed!</strong> Please create purchase request header first";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "<strong>Failed!</strong> " + ex.Message;
            }
        }

        protected void CmbDeviceGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_pur_order_device_type");
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
    }
}
