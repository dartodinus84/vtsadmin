using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_qc_returned : System.Web.UI.Page
    {
        // ViewState & Session keys for Pending GridView
        string sViewStateFieldSortPending = "RecListDeviceQCReturnedPendingFieldSort";
        string sViewStateDirSortPending = "RecListDeviceQCReturnedPendingDirSort";
        string sSessionRecListPending = "RecListDeviceQCReturnedPending";
        
        // ViewState & Session keys for History GridView
        string sViewStateFieldSortHistory = "RecListDeviceQCReturnedHistoryFieldSort";
        string sViewStateDirSortHistory = "RecListDeviceQCReturnedHistoryDirSort";
        string sSessionRecListHistory = "RecListDeviceQCReturnedHistory";

        // Helper method to ensure GridView is in safe state
        private void EnsureGridViewSafeState()
        {
            try
            {
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                LblTotalPending.Text = "0";
            }
            catch
            {
                // Ignore errors in safe state method
            }
        }

        // Helper method to handle errors gracefully
        private void HandleError(string errorMessage, bool showToUser = true)
        {
            try
            {
                if (showToUser)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + errorMessage + "</div>";
                }
                
                // Ensure GridViews are in safe state
                EnsureGridViewSafeState();
                
                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even error handling fails, just ensure safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to safely bind GridView with data
        private void SafeBindGridView(GridView gridView, object dataSource, Label totalLabel)
        {
            try
            {
                // First clear any existing data source
                gridView.DataSource = null;
                gridView.DataBind();
                
                // Then set new data source if provided
                if (dataSource != null)
                {
                    gridView.DataSource = dataSource;
                    gridView.DataBind();
                    
                    // Update total count
                    if (totalLabel != null)
                    {
                        totalLabel.Text = gridView.Rows.Count.ToString();
                    }
                }
                else
                {
                    // No data available
                    if (totalLabel != null)
                    {
                        totalLabel.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                // If binding fails, ensure safe state
                gridView.DataSource = null;
                gridView.DataBind();
                if (totalLabel != null)
                {
                    totalLabel.Text = "0";
                }
                throw new Exception("Failed to bind GridView: " + ex.Message);
            }
        }

        // Helper method to handle IListSource errors specifically
        private void HandleIListSourceError()
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                
                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;
                
                // Reset labels
                LblTotalPending.Text = "0";
                
                // Show user-friendly message
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> No data available. Please try refreshing the page.</div>";
                
                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle all types of errors with specific IListSource handling
        private void HandleAllErrors(Exception ex)
        {
            try
            {
                // Check if it's an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                    return;
                }
                
                // For other errors, use standard error handling
                HandleError(ex.Message);
            }
            catch
            {
                // If even error handling fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        protected void Open_GridView_Pending()
        {
            try
            {
                // Initialize GridView with empty data first to prevent binding errors
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                LblTotalPending.Text = "0";
                
                ClsType ClType = new ClsType();
                
                // Check if connection string exists
                if (Session["ClsTypeDBConnStringSQL"] == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> Database connection not available. Please login again.</div>";
                    UpdatePanelPending.Update();
                    return;
                }
                
                // Jika card dashboard dipilih, gunakan SP filter by DeviceTypeID.
                // Jika tidak ada filter, fallback ke list default (load awal).
                string selectedDeviceTypeId = (Request.QueryString["DeviceTypeID"] ?? string.Empty).Trim();
                string strSQL = "sp_list_device_qc_returned ''";
                if (!string.IsNullOrEmpty(selectedDeviceTypeId))
                {
                    string escapedDeviceTypeId = selectedDeviceTypeId.Replace("'", "''");
                    strSQL = "sp_list_device_qc_returned_type '" + escapedDeviceTypeId + "'";
                }
                
                ViewState[sViewStateFieldSortPending] = "DeviceID";
                ViewState[sViewStateDirSortPending] = "DESC";
                
                // Use try-catch for Open_GridView to handle potential errors
                try
                {
                    Session[sSessionRecListPending] = ClType.Open_GridView(GridViewPending, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingPending, ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString());
                    
                    // Use SafeBindGridView to ensure proper binding
                    SafeBindGridView(GridViewPending, Session[sSessionRecListPending], LblTotalPending);
                }
                catch (Exception gridEx)
                {
                    // Check if it's an IListSource error
                    if (gridEx.Message.Contains("IListSource") || gridEx.Message.Contains("data sources"))
                    {
                        HandleIListSourceError();
                        return;
                    }
                    
                    // If Open_GridView fails, ensure GridView is in safe state
                    SafeBindGridView(GridViewPending, null, LblTotalPending);
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> Unable to load data. " + gridEx.Message + "</div>";
                }
                
                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                HandleAllErrors(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                
                // Check if session variables exist before accessing them
                if (Session["ClsTypeAccessMenu"] == null || Session["ClsTypeIsLogin"] == null || Session["ClsTypeUserID"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }
                
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDEVQC"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        // Initialize GridViews with empty data first to prevent binding errors
                        EnsureGridViewSafeState();
                        
                        ClearForm();
                        Open_GridView_Pending();
                        
                        // Update UpdatePanels
                        UpdatePanelPending.Update();
                        UpdatePanelQCForm.Update();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
                else
                {
                    // On PostBack, clear ScannedDeviceIDs ViewState if not coming from btnSearchMultiple
                    // This prevents checkboxes from being auto-checked on subsequent postbacks
                    string eventTarget = Request.Form["__EVENTTARGET"];
                    if (eventTarget != null && !eventTarget.Contains("btnSearchMultiple"))
                    {
                        ViewState["ScannedDeviceIDs"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Check if it's an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                }
                else
                {
                    HandleAllErrors(ex);
                }
            }
        }

        private void ClearForm()
        {
            try
            {
                CmbQCResult.SelectedIndex = 0;
                txtQCNotes.Text = "";
                
                // Uncheck all checkboxes in Pending GridView
                if (GridViewPending.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridViewPending.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                        if (chk != null) chk.Checked = false;
                    }
                }
                
                div_comment.InnerHtml = "";
                
                // Update UpdatePanel
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                
                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        protected void CmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                Open_GridView_Pending();
                
                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                
                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                // 1. Get selected devices from Pending GridView
                List<string> selectedDevices = new List<string>();
                // Map DeviceID -> NoSN (untuk validasi setting channel MDVR)
                Dictionary<string, string> selectedDeviceNoSN = new Dictionary<string, string>();
                
                if (GridViewPending.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridViewPending.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect != null && chkSelect.Checked)
                        {
                            string deviceID = row.Cells[1].Text.Trim(); // DeviceID column (index 1, after checkbox)
                            selectedDevices.Add(deviceID);

                            string noSn = row.Cells[2].Text.Trim(); // NoSN column (index 2)
                            if (!string.IsNullOrEmpty(deviceID) && !selectedDeviceNoSN.ContainsKey(deviceID))
                            {
                                selectedDeviceNoSN[deviceID] = noSn;
                            }
                        }
                    }
                }
                
                // 2. Validation
                // Validate Warehouse ID (MANDATORY)
                if (txtWarehouseID == null || string.IsNullOrEmpty(txtWarehouseID.Value.Trim()))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong><i class='fa fa-exclamation-triangle'></i> Error!</strong> Warehouse ID is required! Please select warehouse first.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }
                
                if (selectedDevices.Count == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select at least one device!</div>";
                    
                    // Update UpdatePanel untuk menampilkan warning
                    UpdatePanelQCForm.Update();
                    return;
                }
                
                if (CmbQCResult.SelectedIndex == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select QC Result!</div>";
                    
                    // Update UpdatePanel untuk menampilkan warning
                    UpdatePanelQCForm.Update();
                    return;
                }

                // 2b. Validasi setting channel MDVR (server-side, tidak mengandalkan status UI).
                // SN yang IsRequireChannelSetting = 1 namun belum punya setting channel (ADAS/DMS) tidak boleh di-QC.
                string channelBlockWarning = "";
                List<string> blockedChannelNoSN = GetBlockedChannelNoSN(selectedDeviceNoSN.Values);
                if (blockedChannelNoSN.Count > 0)
                {
                    HashSet<string> blockedSet = new HashSet<string>(blockedChannelNoSN, StringComparer.OrdinalIgnoreCase);
                    selectedDevices = selectedDevices
                        .Where(id => !(selectedDeviceNoSN.ContainsKey(id) && blockedSet.Contains(selectedDeviceNoSN[id])))
                        .ToList();

                    channelBlockWarning =
                        "<div class='alert alert-warning' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-exclamation-triangle'></i> QC tidak dapat diproses.</strong><br/>" +
                        "Setting channel belum lengkap untuk NoSN:<br/>" +
                        Server.HtmlEncode(string.Join(", ", blockedChannelNoSN)) +
                        "</div>";

                    if (selectedDevices.Count == 0)
                    {
                        div_comment.InnerHtml = channelBlockWarning;
                        UpdatePanelQCForm.Update();
                        return;
                    }
                }
                
                // 3. Prepare QC notes
                string fullNotes = txtQCNotes.Text.Trim();
                
                // 4. Process each selected device
                int successCount = 0;
                int failCount = 0;
                StringBuilder errorMsg = new StringBuilder();
                StringBuilder successDevices = new StringBuilder();
                
                ExecCommand ec = new ExecCommand();
                
                foreach (string deviceID in selectedDevices)
                {
                    try
                    {
                        Int32 intAff = 0;
                        string sErr = "";
                        
                        // Escape all parameters to prevent SQL injection
                        string escapedDeviceID = deviceID.Replace("'", "''");
                        string escapedNotes = fullNotes.Replace("'", "''");
                        string escapedQCBy = Session["ClsTypeUserID"].ToString().Replace("'", "''");
                        string escapedWarehouseID = (txtWarehouseID != null && !string.IsNullOrEmpty(txtWarehouseID.Value)) 
                            ? txtWarehouseID.Value.Trim().Replace("'", "''") 
                            : "";
                        
                        // Call SP: sp_update_device_qc_returned (@deviceid, @is_qc, @remark, @QcBy, @WarehouseID)
                        string strSQL = "sp_update_device_qc_returned '" + 
                                       escapedDeviceID + "'," + 
                                       CmbQCResult.SelectedValue + ",'" + 
                                       escapedNotes + "','" + 
                                       escapedQCBy + "','" + 
                                       escapedWarehouseID + "'";
                        
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                successCount++;
                                successDevices.Append(deviceID + ", ");
                            }
                            else
                            {
                                failCount++;
                                errorMsg.Append("<li>" + deviceID + ": Update failed (no rows affected)</li>");
                            }
                        }
                        else
                        {
                            failCount++;
                            errorMsg.Append("<li>" + deviceID + ": " + sErr + "</li>");
                        }
                    }
                    catch (Exception exDevice)
                    {
                        failCount++;
                        errorMsg.Append("<li>" + deviceID + ": " + exDevice.Message + "</li>");
                    }
                }
                
                // 6. Refresh both GridViews
                ClearForm();
                Open_GridView_Pending();  // Refresh pending list
                
                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
                
                // 7. Show result
                string resultText = CmbQCResult.SelectedItem.Text;
                
                if (failCount == 0)
                {
                    // All success
                    string devices = successDevices.ToString().TrimEnd(',', ' ');
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-success' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-check'></i> Success!</strong><br/>" +
                        "<strong>{0}</strong> device(s) have been QC checked as <strong>{1}</strong><br/>" +
                        "<small>Devices: {2}</small>" +
                        "</div>", successCount, resultText, devices);
                }
                else if (successCount > 0)
                {
                    // Partial success
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-warning' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-exclamation-triangle'></i> Partial Success!</strong><br/>" +
                        "<strong>{0}</strong> device(s) succeeded, <strong>{1}</strong> device(s) failed.<br/>" +
                        "<strong>Failed devices:</strong>" +
                        "<ul>{2}</ul>" +
                        "</div>", successCount, failCount, errorMsg.ToString());
                }
                else
                {
                    // All failed
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-danger' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-times'></i> Failed!</strong><br/>" +
                        "All <strong>{0}</strong> device(s) failed to process.<br/>" +
                        "<strong>Errors:</strong>" +
                        "<ul>{1}</ul>" +
                        "</div>", failCount, errorMsg.ToString());
                }

                // Sisipkan peringatan SN yang di-skip karena setting channel belum lengkap.
                if (!string.IsNullOrEmpty(channelBlockWarning))
                {
                    div_comment.InnerHtml = channelBlockWarning + div_comment.InnerHtml;
                }
                
                // Update UpdatePanel untuk menampilkan hasil
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Error!</strong> " + ex.Message + "</div>";
                
                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        // Validasi setting channel: dari daftar NoSN terpilih, kembalikan yang wajib setting channel
        // (IsRequireChannelSetting = 1) namun belum memiliki setting ADAS/DMS di database.
        private List<string> GetBlockedChannelNoSN(IEnumerable<string> selectedNoSN)
        {
            List<string> required = new List<string>();
            try
            {
                HashSet<string> selected = new HashSet<string>(
                    (selectedNoSN ?? Enumerable.Empty<string>())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase);

                if (selected.Count == 0)
                {
                    return required;
                }

                DataSet ds = Session[sSessionRecListPending] as DataSet;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Columns.Contains("IsRequireChannelSetting") && dt.Columns.Contains("NoSN"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string sn = Convert.ToString(dr["NoSN"]).Trim();
                            if (sn == "" || !selected.Contains(sn))
                            {
                                continue;
                            }

                            string reqRaw = Convert.ToString(dr["IsRequireChannelSetting"]).Trim().ToLowerInvariant();
                            bool isReq = reqRaw == "1" || reqRaw == "true" || reqRaw == "yes" || reqRaw == "y";
                            if (isReq && !required.Contains(sn))
                            {
                                required.Add(sn);
                            }
                        }
                    }
                }
            }
            catch
            {
                return new List<string>();
            }

            if (required.Count == 0)
            {
                return required;
            }

            string connStr = MdvrChannelService.ResolveSqlConnectionString(HttpContext.Current);
            return MdvrChannelService.GetUnconfiguredNoSN(connStr, required);
        }

        // ===== PENDING GRIDVIEW HANDLERS =====
        protected void GridViewPending_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                if (Session[sSessionRecListPending] == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No data available. Please refresh the page.</div>";
                    UpdatePanelPending.Update();
                    return;
                }
                
                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecListPending], LblPagingPending, ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString());
                div_comment.InnerHtml = "";
                
                try
                {
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();
                }
                catch
                {
                    LblTotalPending.Text = "0";
                }
                
                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    LblTotalPending.Text = "0";
                    
                    // Ensure GridView is in safe state
                    GridViewPending.DataSource = null;
                    GridViewPending.DataBind();
                    
                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }

        // Render sel kolom "Status Channel" dari data SP (IsRequireChannelSetting, StatusChannel, MaxChannel, NoSN).
        private void RenderChannelStatusCell(GridViewRow row)
        {
            try
            {
                Literal lit = (Literal)row.FindControl("litChannelStatus");
                if (lit == null)
                {
                    return;
                }

                DataRowView rowView = row.DataItem as DataRowView;
                object isReq = GetChannelRowValue(rowView, "IsRequireChannelSetting");
                object status = GetChannelRowValue(rowView, "StatusChannel");
                object nosn = GetChannelRowValue(rowView, "NoSN");
                object deviceType = GetChannelRowValue(rowView, "DeviceTypeDesc");
                object maxCh = GetChannelRowValue(rowView, "MaxChannel");

                lit.Text = MdvrChannelService.BuildStatusCellHtml(isReq, status, nosn, deviceType, maxCh);
            }
            catch
            {
                // Best effort: jangan ganggu binding grid jika kolom channel tidak tersedia.
            }
        }

        private object GetChannelRowValue(System.Data.DataRowView rowView, string columnName)
        {
            try
            {
                if (rowView != null && rowView.Row.Table.Columns.Contains(columnName))
                {
                    return rowView[columnName];
                }
            }
            catch
            {
            }
            return null;
        }

        protected void GridViewPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Hide kolom 8-10 (hidden fields: TdwID, WarehouseID, SourceName)
                    // Note: Index bertambah 1 karena ada kolom Status Channel setelah Type
                    for (int i = 8; i <= 10; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Hide kolom 8-10 (hidden fields: TdwID, WarehouseID, SourceName)
                    // Kolom: Select(0), DeviceID(1), NoSN(2), Vendor(3), Type(4), Status Channel(5), Warehouse(6), Technician(7), TdwID(8), WarehouseID(9), Source(10)
                    for (int i = 8; i <= 10; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }

                    // Render kolom Status Channel (badge + tombol setting) berdasarkan data SP.
                    RenderChannelStatusCell(e.Row);
                    
                    // Auto-check checkbox if Serial Number (NoSN) is in scanned list
                    if (ViewState["ScannedDeviceIDs"] != null && e.Row.Cells.Count > 2)
                    {
                        string scannedSerials = ViewState["ScannedDeviceIDs"].ToString();
                        if (!string.IsNullOrEmpty(scannedSerials))
                        {
                            string[] serialNumbers = scannedSerials.Split(',');
                            string currentSerialNo = e.Row.Cells[2].Text.Trim().ToUpper(); // NoSN is column index 2
                            
                            if (serialNumbers.Contains(currentSerialNo))
                            {
                                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                                if (chkSelect != null)
                                {
                                    chkSelect.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    
                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }

        protected void GridViewPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session[sSessionRecListPending] == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No data available. Please refresh the page.</div>";
                    UpdatePanelPending.Update();
                    return;
                }
                
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridViewPending, Session[sSessionRecListPending], ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSortPending] = e.SortExpression.ToString();
                ViewState[sViewStateDirSortPending] = sNewDirSort;
                
                try
                {
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();
                }
                catch
                {
                    LblTotalPending.Text = "0";
                }
                
                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    LblTotalPending.Text = "0";
                    
                    // Ensure GridView is in safe state
                    GridViewPending.DataSource = null;
                    GridViewPending.DataBind();
                    
                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }


        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClearForm();
                Open_GridView_Pending();
                
                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    
                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelQCForm.Update();
                }
            }
        }

        protected void btnSearchMultiple_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                // Validate warehouse selected (MANDATORY)
                if (txtWarehouseID == null || string.IsNullOrEmpty(txtWarehouseID.Value.Trim()))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong><i class='fa fa-exclamation-triangle'></i> Error!</strong> Warehouse ID is required! Please select a warehouse first.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }
                
                // Get scanned serial numbers from hidden field
                string scannedSerials = hdnScannedDevices.Value.Trim();
                
                if (string.IsNullOrEmpty(scannedSerials))
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No serial numbers scanned. Please scan at least one serial number.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }
                
                // Parse serial numbers (NoSN)
                string[] serialNumbers = scannedSerials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (serialNumbers.Length == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No valid serial numbers to search.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }
                
                // Store serial numbers in ViewState for auto-check in RowDataBound event
                // Note: ViewState key is still "ScannedDeviceIDs" for backward compatibility, but contains Serial Numbers
                ViewState["ScannedDeviceIDs"] = string.Join(",", serialNumbers.Select(sn => sn.Trim().ToUpper()));
                
                // Load ALL pending devices first (same as Page_Load)
                // RowDataBound event akan auto-check checkbox berdasarkan ViewState
                Open_GridView_Pending();
                
                // Update UpdatePanels first to render GridView with checked checkboxes
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
                
                // Call JavaScript to highlight rows (client-side) after rendering
                string serialNumbersJson = "['" + string.Join("','", serialNumbers.Select(sn => sn.Trim().Replace("'", "\\'"))) + "']";
                ScriptManager.RegisterStartupScript(this, GetType(), "autoCheckDevices", 
                    $"autoCheckMultipleDevices({serialNumbersJson});", true);
                
                // Clear ViewState after rendering complete (will be cleared on next postback anyway)
                // Keep it for now so checkboxes remain checked during this request
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                UpdatePanelQCForm.Update();
            }
        }

    }
}
