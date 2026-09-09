using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class rpt_stock_opname_device_detail : System.Web.UI.Page
    {
        string sSessionRecDetail = "RecStockOpnameReportDetail";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Clear div_comment on page load to prevent empty modal
                div_comment.InnerHtml = "";

                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAMEREPORT"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            // Get parameters from query string
                            string category = Request.QueryString["category"];
                            string vendorId = Request.QueryString["vendorId"];
                            string deviceTypeId = Request.QueryString["deviceTypeId"];
                            string vendorName = Request.QueryString["vendorName"];
                            string deviceTypeName = Request.QueryString["deviceTypeName"];
                            string dateFrom = Request.QueryString["dateFrom"];
                            string dateTo = Request.QueryString["dateTo"];

                            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(vendorId) && !string.IsNullOrEmpty(deviceTypeId))
                            {
                                // Set labels
                                lblDetailCategory.Text = category.Replace("_", " ").ToUpper();
                                lblCategory.Text = category.Replace("_", " ").ToUpper();
                                lblVendorName.Text = vendorName;
                                lblDeviceTypeName.Text = deviceTypeName;

                                // Store in ViewState
                                ViewState["Category"] = category;
                                ViewState["VendorId"] = vendorId;
                                ViewState["DeviceTypeId"] = deviceTypeId;
                                ViewState["VendorName"] = vendorName;
                                ViewState["DeviceTypeName"] = deviceTypeName;
                                ViewState["DateFrom"] = dateFrom;
                                ViewState["DateTo"] = dateTo;

                                // Load data
                                LoadDeviceDetail();
                            }
                            else
                            {
                                // Redirect back if parameters invalid
                                Response.Redirect("rpt_stock_opname_device.aspx");
                            }
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
                // Log error but don't show modal
                System.Diagnostics.Debug.WriteLine($"Error in Page_Load: {ex.Message}");
                lblStatus.Text = "Error loading page";
                lblStatus.CssClass = "text-danger pull-right";
            }
        }

        protected void LoadDeviceDetail()
        {
            try
            {
                string category = ViewState["Category"]?.ToString() ?? "";
                string vendorId = ViewState["VendorId"]?.ToString() ?? "";
                string deviceTypeId = ViewState["DeviceTypeId"]?.ToString() ?? "";
                string dateFrom = ViewState["DateFrom"]?.ToString() ?? "";
                string dateTo = ViewState["DateTo"]?.ToString() ?? "";

                ClsType ClType = new ClsType();
                string strSQL = "sp_get_stock_opname_device_detail '" + category + "'";

                // Add date parameters if available
                if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strSQL += ",'" + dateFrom + "','" + dateTo + "'";
                }
                else
                {
                    strSQL += ",NULL,NULL";
                }

                // Add vendor and device type filters
                strSQL += ",'" + vendorId + "','" + deviceTypeId + "'";

                // Load data
                DataSet dsResult = ClType.Open_GridView(GridViewDetail, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    Session[sSessionRecDetail] = dsResult;
                    lblStatus.Text = $"Total: {dsResult.Tables[0].Rows.Count} devices";
                    lblStatus.CssClass = "text-muted pull-right";
                }
                else
                {
                    lblStatus.Text = "No devices found";
                    lblStatus.CssClass = "text-warning pull-right";
                    Session.Remove(sSessionRecDetail);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoadDeviceDetail: {ex.Message}");
                lblStatus.Text = "Error loading data";
                lblStatus.CssClass = "text-danger pull-right";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                // Redirect back to main report page
                Response.Redirect("rpt_stock_opname_device.aspx");
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridViewDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecDetail], LblPagingDetail);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdExportDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = "";
                string sMsg = "";
                string strFileName = "";
                DateTime dt = DateTime.Now;

                string category = ViewState["Category"]?.ToString().Replace("_", "").Replace(" ", "") ?? "";
                string vendor = ViewState["VendorName"]?.ToString().Replace(" ", "").Replace("/", "_") ?? "";
                string deviceType = ViewState["DeviceTypeName"]?.ToString().Replace(" ", "").Replace("/", "_") ?? "";

                strFileName = $"DeviceDetail_{category}_{vendor}_{deviceType}_{dt.ToString("yyyyMMddHHmmss")}.csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);

                Rec.RecData = Session[sSessionRecDetail] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, $"Device Detail - {category}", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();

                        if (File.Exists(strFullPath))
                        {
                            File.Delete(strFullPath);
                        }

                        Response.End();
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Export failed: " + sMsg + "</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> No records found to export</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdExportDetailsXls_Click(object sender, EventArgs e)
        {
            try
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;

                string category = ViewState["Category"]?.ToString().Replace("_", "").Replace(" ", "") ?? "";
                string vendor = ViewState["VendorName"]?.ToString().Replace(" ", "").Replace("/", "_") ?? "";
                string deviceType = ViewState["DeviceTypeName"]?.ToString().Replace(" ", "").Replace("/", "_") ?? "";

                string strFileName = $"DeviceDetail_{category}_{vendor}_{deviceType}_{dt.ToString("yyyyMMddHHmmss")}.xls";

                Rec.RecData = Session[sSessionRecDetail] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> No records found to export</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }
    }
}