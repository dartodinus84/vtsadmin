using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_create_details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBCREATE"))
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
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), Session["JobCreatePoID"].ToString(), "sp_list_job_order_device_group");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_job_order_device_type '" + Session["JobCreatePoID"].ToString() + "'");
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtQuantityUndone.Text = "";
                txtQuantity.Text = "";
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
                if (Session["JobCreateJobID"].ToString().Trim() != "")
                {
                    if (CmbDeviceGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtQuantity.Text.Trim() != "")
                            {
                                if(Convert.ToInt32(txtQuantity.Text.Trim())<= Convert.ToInt32(txtQuantityUndone.Text.Trim()))
                                {
                                    Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                    ExecCommand ec = new ExecCommand();
                                    strSQL = "sp_insert_job_order_detail '" + Session["JobCreateJobID"].ToString().Trim() + "','" + CmbDeviceGroupID.SelectedItem.Value.Trim() + "','" + CmbDeviceTypeID.SelectedItem.Value.Trim() + "','" + txtQuantity.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            lblMsg.InnerHtml = "<strong>Success!</strong> Create job detail has been save successfully!";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> " + sErr;
                                    }
                                }
                                else
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Quantity can not be larger than quantity undone";
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
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please create job header first";
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
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_job_order_device_type '" + Session["JobCreatePoID"].ToString().Trim() + "',");
                lblMsg.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbDeviceTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.setAttributes(txtQuantityUndone, "sp_get_po_qty_job_order_device_type '" + Session["JobCreatePoID"].ToString().Trim() + "','" + CmbDeviceTypeID.SelectedItem.Value.Trim() + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                lblMsg.InnerHtml = "";
            }
            catch(Exception ex)
            {

            }
        }
    }
}