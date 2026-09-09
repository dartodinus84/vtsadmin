using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class rpt_stock_opname_device : System.Web.UI.Page
    {
        string sSessionRecList = "RecStockOpnameReport";
        string sSessionRecDetail = "RecStockOpnameReportDetail";

        #region Helper Methods

        /// <summary>
        /// Parse date filter values safely for SQL DATE parameters.
        /// </summary>
        private object SqlDateOrNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;

            DateTime parsedDate;

            string[] formats =
            {
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "MM/dd/yyyy",
                "dd-MM-yyyy",
                "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(
                    value.Trim(),
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate))
            {
                return parsedDate.Date;
            }

            throw new Exception("Format tanggal tidak valid. Gunakan format yyyy-MM-dd atau dd/MM/yyyy.");
        }

        private static string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return connectionString;

            try
            {
                return new SqlConnectionStringBuilder(connectionString.Trim()).ConnectionString;
            }
            catch
            {
                List<string> pairs = new List<string>();
                foreach (string part in connectionString.Split(';'))
                {
                    string trimmedPart = part.Trim();
                    if (string.IsNullOrEmpty(trimmedPart)) continue;

                    int separatorIndex = trimmedPart.IndexOf('=');
                    string key = separatorIndex >= 0
                        ? trimmedPart.Substring(0, separatorIndex).Trim().ToUpperInvariant()
                        : trimmedPart.ToUpperInvariant();

                    if (key == "PROVIDER") continue;
                    pairs.Add(trimmedPart);
                }

                return string.Join(";", pairs);
            }
        }

        private string CurrentDateInputValue()
        {
            return DateTime.Today.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Clear div_comment to prevent unwanted modal popups
        /// </summary>
        private void ClearDivComment()
        {
            try
            {
                div_comment.InnerHtml = "";
                System.Diagnostics.Debug.WriteLine("div_comment cleared");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing div_comment: {ex.Message}");
            }
        }

        /// <summary>
        /// Set error message only when necessary
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="type">Alert type (danger, warning, success, info)</param>
        private void SetErrorMessage(string message, string type = "danger")
        {
            try
            {
                if (!string.IsNullOrEmpty(message) && !message.Trim().Equals(""))
                {
                    string alertType = type.ToLower();
                    string strongText = alertType == "danger" ? "Failed" :
                                      alertType == "warning" ? "Warning" :
                                      alertType == "success" ? "Success" : "Info";

                    div_comment.InnerHtml = $"<div class='alert alert-{alertType}' role='alert'>" +
                                          $"<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                                          $"<span aria-hidden='true'>&times;</span></button>" +
                                          $"<strong>{strongText}!</strong> {message}</div>";

                    System.Diagnostics.Debug.WriteLine($"Error message set: {message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting error message: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear modal state to prevent unwanted modal appearances
        /// </summary>
        private void ClearModalState()
        {
            try
            {
                // Clear hidden fields
                hdnModalCategory.Value = "";
                hdnModalVendorId.Value = "";
                hdnModalDeviceTypeId.Value = "";
                hdnModalVendorName.Value = "";
                hdnModalDeviceTypeName.Value = "";

                // Clear modal-related session variables
                Session.Remove(sSessionRecDetail);

                // Clear ViewState untuk modal
                ViewState.Remove("ModalCategory");
                ViewState.Remove("ModalVendorId");
                ViewState.Remove("ModalDeviceTypeId");

                System.Diagnostics.Debug.WriteLine("Modal state cleared");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing modal state: {ex.Message}");
            }
        }

        /// <summary>
        /// Check and clear URL parameters that might trigger modal auto-show
        /// </summary>
        private void ClearUrlParameters()
        {
            try
            {
                if (Request.QueryString.Count > 0)
                {
                    var queryParams = new List<string>();
                    bool hasModalParams = false;

                    foreach (string key in Request.QueryString.Keys)
                    {
                        if (key != null && (key.ToLower().Contains("modal") ||
                                           key.ToLower().Contains("category") ||
                                           key.ToLower().Contains("vendor") ||
                                           key.ToLower().Contains("device")))
                        {
                            hasModalParams = true;
                            System.Diagnostics.Debug.WriteLine($"Found modal-related URL parameter: {key}");
                        }
                        else if (key != null)
                        {
                            queryParams.Add($"{key}={Request.QueryString[key]}");
                        }
                    }

                    // If there were modal-related parameters, redirect to clean URL
                    if (hasModalParams)
                    {
                        string cleanUrl = Request.Url.GetLeftPart(UriPartial.Path);
                        if (queryParams.Count > 0)
                        {
                            cleanUrl += "?" + string.Join("&", queryParams);
                        }

                        System.Diagnostics.Debug.WriteLine($"Redirecting to clean URL: {cleanUrl}");
                        Response.Redirect(cleanUrl, false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing URL parameters: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear form controls
        /// </summary>
        private void clear()
        {
            try
            {
                txtSearch.Text = "";
                txtDateFrom.Text = CurrentDateInputValue();
                txtDateTo.Text = CurrentDateInputValue();
                hdnModalCategory.Value = "";
                hdnModalVendorId.Value = "";
                hdnModalDeviceTypeId.Value = "";
                hdnModalVendorName.Value = "";
                hdnModalDeviceTypeName.Value = "";

                // Clear div_comment saat clear
                ClearDivComment();

                System.Diagnostics.Debug.WriteLine("Form cleared");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message, "danger");
            }
        }

        /// <summary>
        /// Force close all modals
        /// </summary>
        public void ForceCloseAllModals()
        {
            try
            {
                ClearDivComment();
                ClearModalState();

                // Register script untuk close all modals di client side
                string script = @"
                    $(document).ready(function() {
                        $('.modal').modal('hide');
                        $('.modal-backdrop').remove();
                        $('body').removeClass('modal-open');
                        $('body').css('padding-right', '');
                        if (typeof forceModalCleanup === 'function') {
                            forceModalCleanup();
                        }
                    });
                ";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ForceCloseModals", script, true);
                System.Diagnostics.Debug.WriteLine("Force close all modals executed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ForceCloseAllModals: {ex.Message}");
            }
        }

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAMEREPORT"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                // Clear div_comment dan modal state pada setiap page load untuk mencegah modal muncul otomatis
                ClearDivComment();

                // Clear URL parameters yang bisa menyebabkan modal auto-show
                if (!IsPostBack)
                {
                    ClearUrlParameters();
                }

                // Register postback triggers for export buttons
                if (Page.Master is SiteMaster)
                {
                    ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                    ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDetails);
                    ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportXls);
                    ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDetailsXls);
                }

                // Handle postback for LoadDeviceDetail
                string eventTarget = Request.Form["__EVENTTARGET"];
                string eventArgument = Request.Form["__EVENTARGUMENT"];

                System.Diagnostics.Debug.WriteLine($"Page_Load - EventTarget: {eventTarget}, EventArgument: {eventArgument}, IsPostBack: {IsPostBack}");

                if (eventTarget == "LoadDeviceDetail")
                {
                    System.Diagnostics.Debug.WriteLine("LoadDeviceDetail postback detected");
                    LoadDeviceDetail();
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView();

                            // Pastikan div_comment kosong dan modal state clear setelah initial load
                            ClearDivComment();
                            ClearModalState();

                            System.Diagnostics.Debug.WriteLine("Initial page load completed successfully");
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
                    // Handle postback scenarios
                    System.Diagnostics.Debug.WriteLine("PostBack detected");

                    // Clear modal state untuk postback yang bukan LoadDeviceDetail
                    if (eventTarget != "LoadDeviceDetail" && eventTarget != null && !eventTarget.Contains("Export"))
                    {
                        ClearModalState();
                        System.Diagnostics.Debug.WriteLine("Modal state cleared for non-LoadDeviceDetail postback");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Page_Load: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                // Pastikan div_comment clear sebelum render
                if (string.IsNullOrEmpty(div_comment.InnerHtml) ||
                    div_comment.InnerHtml.Trim() == "" ||
                    div_comment.InnerHtml.Trim() == "&nbsp;")
                {
                    ClearDivComment();
                }

                // Clear modal state jika tidak ada postback yang memerlukan modal
                string eventTarget = Request.Form["__EVENTTARGET"];
                if (eventTarget != "LoadDeviceDetail")
                {
                    // Jangan clear modal state jika sedang load device detail
                    // Tapi clear jika adalah postback lain
                    if (IsPostBack && eventTarget != null && !eventTarget.Contains("Export"))
                    {
                        ClearModalState();
                    }
                }

                System.Diagnostics.Debug.WriteLine("OnPreRender completed");
                base.OnPreRender(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnPreRender: {ex.Message}");
                base.OnPreRender(e);
            }
        }

        protected void Session_End()
        {
            try
            {
                // Clear all modal-related sessions
                Session.Remove(sSessionRecList);
                Session.Remove(sSessionRecDetail);
                System.Diagnostics.Debug.WriteLine("Session ended - cleared modal sessions");
            }
            catch (Exception)
            {
                // Ignore errors during session cleanup
            }
        }

        #endregion

        #region Data Loading

        protected void Open_GridView()
        {
            try
            {
                Recordset Rec = new Recordset();

                string sqlConnectionString = GetSqlClientConnectionString(Session["ClsTypeDBConnStringSQL"].ToString());

                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_report_stock_opname_device", conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = SqlDateOrNull(txtDateFrom.Text);
                    cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = SqlDateOrNull(txtDateTo.Text);
                    cmd.Parameters.Add("@DebugMode", SqlDbType.Bit).Value = 0;
                    cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(txtSearch.Text)
                            ? (object)DBNull.Value
                            : txtSearch.Text.Trim();

                    System.Diagnostics.Debug.WriteLine("Executing stored procedure: dbo.sp_report_stock_opname_device");

                    Rec.RecData = new DataSet();
                    adapter.Fill(Rec.RecData, "Table1");
                    Rec.EOF = Rec.RecordCount() == 0;
                }

                if (Rec.RecordCount() > 0)
                {
                    // Store in session for export
                    Session[sSessionRecList] = Rec.RecData;

                    // Build custom table
                    BuildCustomTable(Rec);

                    // Update paging info
                    LblPaging.Text = string.Format("Displaying {0} records found", Rec.RecordCount());

                    System.Diagnostics.Debug.WriteLine($"GridView loaded with {Rec.RecordCount()} records");
                }
                else
                {
                    // Clear table if no data
                    reportTableBody.InnerHtml = "<tr><td colspan='18' style='text-align:center; padding:20px; color:#999;'><i class='fa fa-info-circle'></i> No data found for the selected criteria</td></tr>";
                    LblPaging.Text = "No records found";
                    Session.Remove(sSessionRecList);

                    System.Diagnostics.Debug.WriteLine("No data found");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Open_GridView: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
                reportTableBody.InnerHtml = "<tr><td colspan='18' style='text-align:center; padding:20px; color:#dc3545;'><i class='fa fa-exclamation-triangle'></i> Error loading data: " + ex.Message + "</td></tr>";
            }
        }

        protected void BuildCustomTable(Recordset Rec)
        {
            try
            {
                string tableHtml = "";
                int rowNo = 1;

                Rec.MoveFirst();
                while (!Rec.EOF)
                {
                    string vendorId = Rec.Fields("vendor_id") ?? "";
                    string deviceTypeId = Rec.Fields("device_type_id") ?? "";
                    string vendorName = Rec.Fields("gps_vendor") ?? "";
                    string deviceType = Rec.Fields("gps_device_type") ?? "";

                    // URL encode parameters untuk query string
                    string vendorNameEncoded = HttpUtility.UrlEncode(vendorName);
                    string deviceTypeEncoded = HttpUtility.UrlEncode(deviceType);
                    string dateFromEncoded = HttpUtility.UrlEncode(txtDateFrom.Text.Trim());
                    string dateToEncoded = HttpUtility.UrlEncode(txtDateTo.Text.Trim());

                    tableHtml += "<tr style='font-size: 11px;'>";

                    // NO - Fixed column
                    tableHtml += $"<td class='fixed-col col-no'>{rowNo}</td>";

                    // SUPP (Vendor) - Fixed column
                    tableHtml += $"<td class='fixed-col col-supp' title='{vendorName}'>{vendorName}</td>";

                    // GPS KOMPONEN (Device Type) - Fixed column
                    tableHtml += $"<td class='fixed-col col-komponen' title='{deviceType}'>{deviceType}</td>";

                    // STOCK AWAL - Clickable dengan redirect
                    int stockAwal = Convert.ToInt32(Rec.Fields("stock_awal") ?? "0");
                    string stockAwalClass = stockAwal == 0 ? "zero-value" : "clickable-cell";
                    string stockAwalUrl = $"rpt_stock_opname_device_detail.aspx?category=stock_awal&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    tableHtml += $"<td class='{stockAwalClass}' tabindex='0' onclick=\"window.location.href='{stockAwalUrl}'\" title='Click to view {stockAwal} devices' aria-label='{stockAwal} stock awal devices'>{stockAwal:N0}</td>";


                    // Aktual Jumlah di Gudang (ALL) - 3 columns dengan redirect
                    int baru = Convert.ToInt32(Rec.Fields("baru") ?? "0");
                    int second = Convert.ToInt32(Rec.Fields("second") ?? "0");
                    int disposal = Convert.ToInt32(Rec.Fields("disposal") ?? "0");

                    string baruClass = baru == 0 ? "zero-value" : "clickable-cell";
                    string secondClass = second == 0 ? "zero-value" : "clickable-cell";
                    string disposalClass = disposal == 0 ? "zero-value" : "clickable-cell";

                    string baruUrl = $"rpt_stock_opname_device_detail.aspx?category=baru&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string secondUrl = $"rpt_stock_opname_device_detail.aspx?category=second&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string disposalUrl = $"rpt_stock_opname_device_detail.aspx?category=disposal&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";

                    tableHtml += $"<td class='{baruClass}' tabindex='0' onclick=\"window.location.href='{baruUrl}'\" title='Click to view {baru} new devices' aria-label='{baru} new devices'>{baru:N0}</td>";
                    tableHtml += $"<td class='{secondClass}' tabindex='0' onclick=\"window.location.href='{secondUrl}'\" title='Click to view {second} second devices' aria-label='{second} second devices'>{second:N0}</td>";
                    tableHtml += $"<td class='{disposalClass}' tabindex='0' onclick=\"window.location.href='{disposalUrl}'\" title='Click to view {disposal} disposal devices' aria-label='{disposal} disposal devices'>{disposal:N0}</td>";

                    // Lanjutkan pattern yang sama untuk kolom lainnya...
                    // Jakarta
                    int jakartaBaru = Convert.ToInt32(Rec.Fields("jakarta_baru") ?? "0");
                    int jakartaSecond = Convert.ToInt32(Rec.Fields("jakarta_second") ?? "0");
                    int jakartaDisposal = Convert.ToInt32(Rec.Fields("jakarta_disposal") ?? "0");

                    string jakartaBaruClass = jakartaBaru == 0 ? "zero-value" : "clickable-cell";
                    string jakartaSecondClass = jakartaSecond == 0 ? "zero-value" : "clickable-cell";
                    string jakartaDisposalClass = jakartaDisposal == 0 ? "zero-value" : "clickable-cell";

                    string jakartaBaruUrl = $"rpt_stock_opname_device_detail.aspx?category=jakarta_baru&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string jakartaSecondUrl = $"rpt_stock_opname_device_detail.aspx?category=jakarta_second&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string jakartaDisposalUrl = $"rpt_stock_opname_device_detail.aspx?category=jakarta_disposal&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";

                    tableHtml += $"<td class='{jakartaBaruClass}' tabindex='0' onclick=\"window.location.href='{jakartaBaruUrl}'\" title='Click to view {jakartaBaru} Jakarta new devices' aria-label='{jakartaBaru} Jakarta new devices'>{jakartaBaru:N0}</td>";
                    tableHtml += $"<td class='{jakartaSecondClass}' tabindex='0' onclick=\"window.location.href='{jakartaSecondUrl}'\" title='Click to view {jakartaSecond} Jakarta second devices' aria-label='{jakartaSecond} Jakarta second devices'>{jakartaSecond:N0}</td>";
                    tableHtml += $"<td class='{jakartaDisposalClass}' tabindex='0' onclick=\"window.location.href='{jakartaDisposalUrl}'\" title='Click to view {jakartaDisposal} Jakarta disposal devices' aria-label='{jakartaDisposal} Jakarta disposal devices'>{jakartaDisposal:N0}</td>";

                    // Surabaya
                    int surabayaBaru = Convert.ToInt32(Rec.Fields("surabaya_baru") ?? "0");
                    int surabayaSecond = Convert.ToInt32(Rec.Fields("surabaya_second") ?? "0");
                    int surabayaDisposal = Convert.ToInt32(Rec.Fields("surabaya_disposal") ?? "0");

                    string surabayaBaruClass = surabayaBaru == 0 ? "zero-value" : "clickable-cell";
                    string surabayaSecondClass = surabayaSecond == 0 ? "zero-value" : "clickable-cell";
                    string surabayaDisposalClass = surabayaDisposal == 0 ? "zero-value" : "clickable-cell";

                    string surabayaBaruUrl = $"rpt_stock_opname_device_detail.aspx?category=surabaya_baru&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string surabayaSecondUrl = $"rpt_stock_opname_device_detail.aspx?category=surabaya_second&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string surabayaDisposalUrl = $"rpt_stock_opname_device_detail.aspx?category=surabaya_disposal&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";

                    tableHtml += $"<td class='{surabayaBaruClass}' tabindex='0' onclick=\"window.location.href='{surabayaBaruUrl}'\" title='Click to view {surabayaBaru} Surabaya new devices' aria-label='{surabayaBaru} Surabaya new devices'>{surabayaBaru:N0}</td>";
                    tableHtml += $"<td class='{surabayaSecondClass}' tabindex='0' onclick=\"window.location.href='{surabayaSecondUrl}'\" title='Click to view {surabayaSecond} Surabaya second devices' aria-label='{surabayaSecond} Surabaya second devices'>{surabayaSecond:N0}</td>";
                    tableHtml += $"<td class='{surabayaDisposalClass}' tabindex='0' onclick=\"window.location.href='{surabayaDisposalUrl}'\" title='Click to view {surabayaDisposal} Surabaya disposal devices' aria-label='{surabayaDisposal} Surabaya disposal devices'>{surabayaDisposal:N0}</td>";

                    // Teknisi
                    int teknisiWestEast = Convert.ToInt32(Rec.Fields("teknisi_west_east") ?? "0");
                    int teknisiWest = Convert.ToInt32(Rec.Fields("teknisi_west") ?? "0");
                    int teknisiEast = Convert.ToInt32(Rec.Fields("teknisi_east") ?? "0");

                    string teknisiWestEastClass = teknisiWestEast == 0 ? "zero-value" : "clickable-cell";
                    string teknisiWestClass = teknisiWest == 0 ? "zero-value" : "clickable-cell";
                    string teknisiEastClass = teknisiEast == 0 ? "zero-value" : "clickable-cell";

                    string teknisiWestEastUrl = $"rpt_stock_opname_device_detail.aspx?category=teknisi_west_east&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string teknisiWestUrl = $"rpt_stock_opname_device_detail.aspx?category=teknisi_west&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    string teknisiEastUrl = $"rpt_stock_opname_device_detail.aspx?category=teknisi_east&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";

                    tableHtml += $"<td class='{teknisiWestEastClass}' tabindex='0' onclick=\"window.location.href='{teknisiWestEastUrl}'\" title='Click to view {teknisiWestEast} technician devices (West & East)' aria-label='{teknisiWestEast} West East technician devices'>{teknisiWestEast:N0}</td>";
                    tableHtml += $"<td class='{teknisiWestClass}' tabindex='0' onclick=\"window.location.href='{teknisiWestUrl}'\" title='Click to view {teknisiWest} West area technician devices' aria-label='{teknisiWest} West technician devices'>{teknisiWest:N0}</td>";
                    tableHtml += $"<td class='{teknisiEastClass}' tabindex='0' onclick=\"window.location.href='{teknisiEastUrl}'\" title='Click to view {teknisiEast} East area technician devices' aria-label='{teknisiEast} East technician devices'>{teknisiEast:N0}</td>";

                    // STOCK CUST
                    int stockCust = Convert.ToInt32(Rec.Fields("stock_cust") ?? "0");
                    tableHtml += $"<td style='text-align:right; padding:4px; color:#999; font-style:italic;' title='Customer stock (placeholder)'>{stockCust:N0}</td>";

                    // Total
                    int jumlahAktual = Convert.ToInt32(Rec.Fields("jumlah_aktual") ?? "0");
                    string jumlahAktualClass = jumlahAktual == 0 ? "zero-value total-column" : "clickable-cell total-column";
                    string jumlahAktualUrl = $"rpt_stock_opname_device_detail.aspx?category=jumlah_aktual&vendorId={vendorId}&deviceTypeId={deviceTypeId}&vendorName={vendorNameEncoded}&deviceTypeName={deviceTypeEncoded}&dateFrom={dateFromEncoded}&dateTo={dateToEncoded}";
                    tableHtml += $"<td class='{jumlahAktualClass}' tabindex='0' onclick=\"window.location.href='{jumlahAktualUrl}'\" title='Click to view {jumlahAktual} total actual devices' aria-label='{jumlahAktual} total devices'>{jumlahAktual:N0}</td>";

                    tableHtml += "</tr>";

                    Rec.MoveNext();
                    rowNo++;
                }

                if (!string.IsNullOrEmpty(tableHtml))
                {
                    reportTableBody.InnerHtml = tableHtml;
                    System.Diagnostics.Debug.WriteLine($"Table built successfully with {rowNo - 1} rows");
                }
                else
                {
                    reportTableBody.InnerHtml = "<tr><td colspan='18' style='text-align:center; padding:20px; color:#999;'><i class='fa fa-info-circle'></i> No data available to display</td></tr>";
                    System.Diagnostics.Debug.WriteLine("No data to build table");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in BuildCustomTable: {ex.Message}");
                throw new Exception("Error building custom table: " + ex.Message);
            }
        }

        protected void LoadDeviceDetail()
        {
            try
            {
                // Clear div_comment sebelum load detail untuk mencegah konflik
                ClearDivComment();

                string category = hdnModalCategory.Value;
                string vendorId = hdnModalVendorId.Value;
                string deviceTypeId = hdnModalDeviceTypeId.Value;

                System.Diagnostics.Debug.WriteLine($"LoadDeviceDetail - Category: {category}, VendorId: {vendorId}, DeviceTypeId: {deviceTypeId}");

                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(vendorId) && !string.IsNullOrEmpty(deviceTypeId))
                {
                    ClsType ClType = new ClsType();
                    string strSQL = "sp_get_stock_opname_device_detail '" + category + "'";

                    // Add date parameters if available
                    if (!string.IsNullOrEmpty(txtDateFrom.Text.Trim()) && !string.IsNullOrEmpty(txtDateTo.Text.Trim()))
                    {
                        strSQL += ",'" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                    }
                    else
                    {
                        strSQL += ",NULL,NULL";
                    }

                    // Add vendor and device type filters
                    strSQL += ",'" + vendorId + "','" + deviceTypeId + "'";

                    System.Diagnostics.Debug.WriteLine($"SQL Query: {strSQL}");

                    // Clear previous data
                    GridViewDetail.DataSource = null;
                    GridViewDetail.DataBind();

                    // Load new data
                    DataSet dsResult = ClType.Open_GridView(GridViewDetail, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail);

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        Session[sSessionRecDetail] = dsResult;
                        LblPagingDetail.Text = $"Displaying 1 to {Math.Min(GridViewDetail.PageSize, dsResult.Tables[0].Rows.Count)} of {dsResult.Tables[0].Rows.Count} devices found";
                        System.Diagnostics.Debug.WriteLine($"Detail data loaded: {dsResult.Tables[0].Rows.Count} rows");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No detail data found");
                        GridViewDetail.DataSource = new DataTable();
                        GridViewDetail.DataBind();
                        LblPagingDetail.Text = "No devices found for this category";
                        Session.Remove(sSessionRecDetail);
                    }

                    // Force update of the modal UpdatePanel
                    UpdatePanelModal.Update();

                    System.Diagnostics.Debug.WriteLine("Modal detail loaded successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Missing parameters for LoadDeviceDetail");
                    // Jangan set error message untuk parameter kosong karena ini normal
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoadDeviceDetail: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        #endregion

        #region Button Events

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum search
                Open_GridView();
                System.Diagnostics.Debug.WriteLine("Search completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdSearch_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridView();
                System.Diagnostics.Debug.WriteLine("Clear completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdClear_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        #endregion

        #region GridView Events

        protected void GridViewDetail_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum paging
                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecDetail], LblPagingDetail);
                UpdatePanelModal.Update();
                System.Diagnostics.Debug.WriteLine($"Detail grid paging to index: {e.NewPageIndex}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GridViewDetail_PageIndexChanging: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        #endregion

        #region Export Methods

        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum export

                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = "StockOpnameDevice_" + dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);

                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Laporan Stok Opname Device", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        System.Diagnostics.Debug.WriteLine($"CSV export successful: {strFileName}");

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
                        SetErrorMessage("Failed to export CSV: " + sMsg, "danger");
                    }
                }
                else
                {
                    SetErrorMessage("No records found to export", "warning");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdExport_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        protected void CmdExportXls_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum export

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = "StockOpnameDevice_" + dt.ToString("yyyyMMddHHmmss") + ".xls";

                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    System.Diagnostics.Debug.WriteLine($"XLS export successful: {strFileName}");

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
                    SetErrorMessage("No records found to export", "warning");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdExportXls_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        protected void CmdExportDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum export

                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                string category = hdnModalCategory.Value.Replace("_", "").Replace(" ", "");
                string vendor = hdnModalVendorName.Value.Replace(" ", "").Replace("/", "_");
                string deviceType = hdnModalDeviceTypeName.Value.Replace(" ", "").Replace("/", "_");
                strFileName = $"DeviceDetail_{category}_{vendor}_{deviceType}_{dt.ToString("yyyyMMddHHmmss")}.csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);

                Rec.RecData = Session[sSessionRecDetail] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, $"Device Detail - {hdnModalCategory.Value}", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        System.Diagnostics.Debug.WriteLine($"Detail CSV export successful: {strFileName}");

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
                        SetErrorMessage("Failed to export detail CSV: " + sMsg, "danger");
                    }
                }
                else
                {
                    SetErrorMessage("No detail records found to export", "warning");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdExportDetails_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        protected void CmdExportDetailsXls_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDivComment(); // Clear sebelum export

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string category = hdnModalCategory.Value.Replace("_", "").Replace(" ", "");
                string vendor = hdnModalVendorName.Value.Replace(" ", "").Replace("/", "_");
                string deviceType = hdnModalDeviceTypeName.Value.Replace(" ", "").Replace("/", "_");
                string strFileName = $"DeviceDetail_{category}_{vendor}_{deviceType}_{dt.ToString("yyyyMMddHHmmss")}.xls";

                Rec.RecData = Session[sSessionRecDetail] as System.Data.DataSet;

                if (Rec.RecData != null && Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    System.Diagnostics.Debug.WriteLine($"Detail XLS export successful: {strFileName}");

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
                    SetErrorMessage("No detail records found to export", "warning");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CmdExportDetailsXls_Click: {ex.Message}");
                SetErrorMessage(ex.Message, "danger");
            }
        }

        #endregion
    }
}