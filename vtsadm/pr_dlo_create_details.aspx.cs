using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_dlo_create_details : System.Web.UI.Page
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
                string purId = (txtPurID.Value ?? "").Trim().Replace("'", "''");
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), purId, "sp_list_pur_order_dlo_device_group");
                string deviceGroupVal = (CmbDeviceGroupID.SelectedItem != null) ? CmbDeviceGroupID.SelectedItem.Value.ToString() : "[Select]";
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), deviceGroupVal, "sp_list_pur_order_dlo_device_type '" + purId + "',");
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtQuantityUndone.Text = "";
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
                if ((txtDloID.Value ?? "").Trim() != "")
                {
                    if (CmbDeviceGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtQuantity.Value.Trim() != "" && (txtPrice.Value ?? "").Trim() != "")
                            {
                                int qtyIn = 0;
                                int qtyMax = 0;
                                if (!int.TryParse(txtQuantity.Value.Trim(), out qtyIn) || !int.TryParse((txtQuantityUndone.Text ?? "").Trim(), out qtyMax))
                                {
                                    lblMsg.Text = "<strong>Failed!</strong> Quantity invalid atau pilih Device Type dulu untuk isi Quantity Undone.";
                                    return;
                                }
                                if (qtyIn > qtyMax)
                                {
                                    lblMsg.Text = "<strong>Failed!</strong> Quantity tidak boleh melebihi Quantity Undone (dari PR).";
                                    return;
                                }
                                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                ExecCommand ec = new ExecCommand();
                                strSQL = "sp_insert_dlo_detail '" + txtDloID.Value.Trim().Replace("'", "''") + "','" + CmbDeviceGroupID.SelectedItem.Value.Trim().Replace("'", "''") + "'," +
                                         "'" + CmbDeviceTypeID.SelectedItem.Value.Trim().Replace("'", "''") + "','" + txtQuantity.Value.Trim().Replace("'", "''") + "'," + (txtPrice.Value.Trim().Replace("'", "''") ?? "0") + "," +
                                         "'" + txtRemark.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    // Stored procedure can return 0 / -1 affected rows (e.g. SET NOCOUNT ON),
                                    // so success should follow Execute() result + empty error text.
                                    if (string.IsNullOrWhiteSpace(sErr))
                                    {
                                        clear();
                                        lblMsg.Text = "<strong>Success!</strong> Create delivery order detail has been saved successfully!";
                                    }
                                    else
                                    {
                                        lblMsg.Text = "<strong>Failed!</strong> " + sErr;
                                    }
                                }
                                else
                                {
                                    lblMsg.Text = "<strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Save failed." : sErr);
                                }
                            }
                            else
                            {
                                lblMsg.Text = "<strong>Failed!</strong> Isi Quantity dan pastikan Device Type sudah dipilih (Price dari PR).";
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
                    lblMsg.Text = "<strong>Failed!</strong> Please create delivery order header first";
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
                string purId = (txtPurID.Value ?? "").Trim().Replace("'", "''");
                string deviceGroupVal = (CmbDeviceGroupID.SelectedItem != null) ? CmbDeviceGroupID.SelectedItem.Value.ToString() : "[Select]";
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), deviceGroupVal, "sp_list_pur_order_dlo_device_type '" + purId + "',");
                txtQuantityUndone.Text = "";
                txtPrice.Value = "0";
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbDeviceTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string purId = (txtPurID.Value ?? "").Trim().Replace("'", "''");
                string deviceTypeId = (CmbDeviceTypeID.SelectedItem != null && CmbDeviceTypeID.SelectedItem.Value != "[Select]") ? CmbDeviceTypeID.SelectedItem.Value.Trim().Replace("'", "''") : "";
                txtQuantityUndone.Text = "";
                txtPrice.Value = "0";
                lblMsg.Text = "";
                if (string.IsNullOrEmpty(purId) || string.IsNullOrEmpty(deviceTypeId)) return;
                // Logic sama seperti job_create_details: sp_get_pur_qty_dlo_device_type = sp_get_po_qty_job_order_device_type untuk PUR
                // Return: kolom 0 = QtyRemaining, kolom 1 = Price (dari PR)
                Recordset rec = new Recordset();
                rec.Open("sp_get_pur_qty_dlo_device_type '" + purId + "','" + deviceTypeId + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                if (rec.RecordCount() > 0)
                {
                    rec.MoveFirst();
                    txtQuantityUndone.Text = (rec.Fields(0) ?? "").ToString().Trim();
                    try { txtPrice.Value = (rec.Fields(1) ?? "0").ToString().Trim(); } catch { txtPrice.Value = "0"; }
                }
                rec = null;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
