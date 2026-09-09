using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class stock_opname_details : System.Web.UI.Page
    {
        public int sOpnameId = 0;
        public string sLocationType = "";
        public string sLocationId = "";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_device_for_opname " + sOpnameId + ",'" +
                                sLocationType + "','" + sLocationId + "','" + txtSearch.Text.Trim() + "'";
                Session["RecListDeviceForOpname"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAME"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (Session["ClsOpnameID"] != null && !string.IsNullOrEmpty(Session["ClsOpnameID"].ToString()))
                {
                    sOpnameId = Convert.ToInt32(Session["ClsOpnameID"]);
                }

                if (Session["ClsLocationType"] != null && !string.IsNullOrEmpty(Session["ClsLocationType"].ToString()))
                {
                    sLocationType = Session["ClsLocationType"].ToString();
                }

                if (Session["ClsLocationID"] != null && !string.IsNullOrEmpty(Session["ClsLocationID"].ToString()))
                {
                    sLocationId = Session["ClsLocationID"].ToString();
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.InnerHtml = "";
                            clear();
                            Open_GridView();
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        private void clear()
        {
            try
            {
                txtDeviceID.Value = "";
                txtNoSN.Text = "";
                txtCurrentStatus.Text = "";
                CmbPhysicalFound.SelectedValue = "[Select]";
                CmbPhysicalStatus.SelectedValue = "[Select]";
                txtRemarks.Text = "";
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                if (sOpnameId > 0)
                {
                    if (txtDeviceID.Value.Trim() != "")
                    {
                        if (CmbPhysicalFound.SelectedItem.Value.Trim() != "[Select]")
                        {
                            bool physicalFound = CmbPhysicalFound.SelectedItem.Value == "1";

                            // Validasi physical status jika device ditemukan
                            if (physicalFound && CmbPhysicalStatus.SelectedItem.Value.Trim() == "[Select]")
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Please select physical status";
                                return;
                            }

                            // Dapatkan nomor seri - coba dari txtNoSN atau dari hidden field
                            string serialNumber = txtNoSN.Text.Trim();

                            // Jika masih kosong, coba dari hidden field
                            if (string.IsNullOrEmpty(serialNumber) && Request.Form["hiddenNoSN"] != null)
                            {
                                serialNumber = Request.Form["hiddenNoSN"].Trim();
                            }

                            if (string.IsNullOrEmpty(serialNumber))
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Could not determine device serial number";
                                return;
                            }

                            // Ambil current status dari hidden field atau database
                            string currentStatus = "";

                            // Metode 1: Coba dari hidden field (paling reliable)
                            if (Request.Form["hiddenCurrentStatus"] != null)
                            {
                                currentStatus = Request.Form["hiddenCurrentStatus"].Trim();
                            }

                            if (string.IsNullOrEmpty(currentStatus))
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Could not determine current device status";
                                return;
                            }

                            // Siapkan parameter untuk stored procedure
                            string physicalStatus = physicalFound ? CmbPhysicalStatus.SelectedItem.Value.Trim() : null;
                            string remarks = txtRemarks.Text.Trim();

                            // Ambil user ID dari session
                            string userId = Session["ClsTypeUserID"]?.ToString() ?? "SYSTEM";

                            // Log nilai-nilai untuk debugging
                            System.Diagnostics.Debug.WriteLine("Saving opname detail - Device ID: " + txtDeviceID.Value.Trim() + ", SN: " + serialNumber + ", Status: " + currentStatus);

                            // Panggil method SaveOpnameDetail dengan parameter userId
                            ClsStockOpname stockOpname = new ClsStockOpname();
                            string errorMessage;
                            bool result = stockOpname.SaveOpnameDetail(
                                sOpnameId,
                                txtDeviceID.Value.Trim(),
                                serialNumber,
                                currentStatus,
                                physicalFound,
                                physicalStatus,
                                remarks,
                                Session["ClsTypeDBConnStringSQL"].ToString(),
                                out errorMessage
                            );

                            if (result)
                            {
                                // Clear form dan reload grid
                                clear();
                                Open_GridView();

                                // Registrasi script untuk menutup modal parent dan refresh data
                                string script = @"
                                    setTimeout(function() {
                                        if (window.parent) {
                                            // Refresh data parent
                                            if (window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad')) {
                                                window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad').click();
                                            }
                                            
                                            // Tutup modal parent dengan benar
                                            if (window.parent.$) {
                                                window.parent.$('#modal-details').modal('hide');
                                                
                                                // Hapus backdrop modal dan reset body
                                                setTimeout(function() {
                                                    window.parent.$('.modal-backdrop').remove();
                                                    window.parent.$('body').removeClass('modal-open');
                                                    window.parent.$('body').css('padding-right', '');
                                                }, 300);
                                            }
                                        }
                                    }, 1500);
                                ";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModalScript", script, true);

                                // Tampilkan pesan sukses
                                lblMsg.InnerHtml = "<strong>Success!</strong> Device check saved successfully";
                            }
                            else
                            {
                                // Tampilkan pesan error dari stored procedure
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    if (errorMessage.Contains("already registered"))
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> " + errorMessage;
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> " + errorMessage;
                                    }
                                }
                                else
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Error saving device check";
                                }
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please select whether device is found or not";
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select device first";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Invalid opname ID";
                }
            }
            catch (Exception ex)
            {
                // Cek apakah error dari SQL Server
                if (ex.Message.Contains("already registered"))
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> This device is already registered in this opname";
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Error: " + ex.Message;
                    System.Diagnostics.Debug.WriteLine("Error in CmdSaveDetail_Click: " + ex.Message);
                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDeviceForOpname"], LblPaging);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string deviceId = e.Row.Cells[0].Text;
                    string sn = e.Row.Cells[1].Text;
                    string status = e.Row.Cells[2].Text;
                    string statusDesc = e.Row.Cells[3].Text;

                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "postDetails('" + deviceId + "','" +
                                              sn + "','" +
                                              status + "','" +
                                              statusDesc.Replace("'", "\\'") + "');return false;";

                    // Tambahkan tooltip
                    CmdButton.ToolTip = "Select Device: " + sn;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
                System.Diagnostics.Debug.WriteLine("Error in GridView2_RowDataBound: " + ex.Message);
            }
        }
    }
}