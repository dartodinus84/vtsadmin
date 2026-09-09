using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class po_add_details_pojo : System.Web.UI.Page
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
                            clear(false);
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
                else
                {
                    SyncPoIDDisplay();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SyncPoIDDisplay()
        {
            try
            {
                ClsType ClType = new ClsType();
                string sPoID = ClType.CheckNbsp(txtPoID.Value.Trim());
                lblPoIDDisplay.InnerText = string.IsNullOrEmpty(sPoID) ? "-" : sPoID;
            }
            catch (Exception ex)
            {
            }
        }

        private void clear(bool keepPoID)
        {
            try
            {
                string sPoID = txtPoID.Value.Trim();
                ClsType ClType = new ClsType();

                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_purchase_order_device_group");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_purchase_order_device_type");
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtQuantity.Value = "";

                if (keepPoID)
                {
                    txtPoID.Value = sPoID;
                }
                else
                {
                    txtPoID.Value = "";
                }

                SyncPoIDDisplay();
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string cssClass = isSuccess ? "alert alert-success alert-dismissible" : "alert alert-danger alert-dismissible";
            lblMsg.Attributes["class"] = "alert-box " + cssClass;
            lblMsg.InnerHtml = "<button type='button' class='close' data-dismiss='alert'><span>&times;</span></button>" + message;
        }

        protected void CmdClearDetail_Click(object sender, EventArgs e)
        {
            try
            {
                clear(true);
                lblMsg.InnerHtml = "";
                lblMsg.Attributes["class"] = "alert-box";
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
                lblMsg.Attributes["class"] = "alert-box";

                if (txtPoID.Value.Trim() == "")
                {
                    ShowMessage("<strong>Failed!</strong> Please select purchase order first.", false);
                    return;
                }

                if (CmbDeviceGroupID.SelectedItem == null || CmbDeviceGroupID.SelectedItem.Value.Trim() == "[Select]")
                {
                    ShowMessage("<strong>Failed!</strong> Please select device group.", false);
                    return;
                }

                if (CmbDeviceTypeID.SelectedItem == null || CmbDeviceTypeID.SelectedItem.Value.Trim() == "[Select]")
                {
                    ShowMessage("<strong>Failed!</strong> Please select device type.", false);
                    return;
                }

                int iQuantity = 0;
                if (!int.TryParse(txtQuantity.Value.Trim(), out iQuantity) || iQuantity <= 0)
                {
                    ShowMessage("<strong>Failed!</strong> Quantity must be greater than 0.", false);
                    return;
                }

                int intAff = 0;
                string strSQL = "";
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                strSQL = "sp_insert_qty_device_purchase_order_detail '" + txtPoID.Value.Trim() + "','" + CmbDeviceTypeID.SelectedItem.Value.Trim() + "','" + iQuantity + "','" + Session["ClsTypeUserID"].ToString() + "'";

                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    clear(true);
                    ShowMessage("<strong>Success!</strong> Purchase order detail has been saved successfully.", true);
                }
                else
                {
                    ShowMessage("<strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Create purchase order detail has been failed." : sErr), false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("<strong>Failed!</strong> " + ex.Message, false);
            }
        }

        protected void CmbDeviceGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_purchase_order_device_type");
                CmbDeviceTypeID.SelectedValue = "[Select]";
                lblMsg.InnerHtml = "";
                lblMsg.Attributes["class"] = "alert-box";
                SyncPoIDDisplay();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
