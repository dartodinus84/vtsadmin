using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class warehouse_receive_return : System.Web.UI.Page
    {
        // ViewState & Session keys for Pending GridView
        string sViewStateFieldSortPending = "RecListWarehouseReceiveReturnPendingFieldSort";
        string sViewStateDirSortPending = "RecListWarehouseReceiveReturnPendingDirSort";
        string sSessionRecListPending = "RecListWarehouseReceiveReturnPending";
        
        // ViewState & Session keys for History GridView
        string sViewStateFieldSortHistory = "RecListWarehouseReceiveReturnHistoryFieldSort";
        string sViewStateDirSortHistory = "RecListWarehouseReceiveReturnHistoryDirSort";
        string sSessionRecListHistory = "RecListWarehouseReceiveReturnHistory";

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
                
                // Show all pending devices with search filter
                string strSQL = "sp_list_warehouse_receive_return ''";
                
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
                
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNURETWHRCV"))
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
                        
                        Open_GridView_Pending();
                        
                        // Update UpdatePanels
                        UpdatePanelPending.Update();
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

        protected void GridViewPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Hide kolom 7-9 (hidden fields: TdwID, WarehouseID, SourceName)
                    for (int i = 7; i <= 9; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Hide kolom 7-9 (hidden fields: TdwID, WarehouseID, SourceName)
                    for (int i = 7; i <= 9; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                    
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


        protected void btnSearchMultiple_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                // Get scanned serial numbers from hidden field
                string scannedSerials = hdnScannedDevices.Value.Trim();
                
                if (string.IsNullOrEmpty(scannedSerials))
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No serial numbers scanned. Please scan at least one serial number.</div>";
                    return;
                }
                
                // Parse serial numbers (NoSN)
                string[] serialNumbers = scannedSerials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (serialNumbers.Length == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No valid serial numbers to search.</div>";
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
            }
        }

        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                // 1. Get selected devices from Pending GridView
                List<string> selectedDevices = new List<string>();
                
                if (GridViewPending.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridViewPending.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect != null && chkSelect.Checked)
                        {
                            string deviceID = row.Cells[1].Text.Trim(); // DeviceID column (index 1, after checkbox)
                            selectedDevices.Add(deviceID);
                        }
                    }
                }
                
                // 2. Validation
                if (selectedDevices.Count == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select at least one device!</div>";
                    return;
                }
                
                // 3. Process each selected device
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
                        
                        // Escape inputs to prevent SQL injection
                        string escapedDeviceID = deviceID.Replace("'", "''");
                        string escapedUser = Session["ClsTypeUserID"].ToString().Replace("'", "''");
                        
                        // Call SP: sp_update_warehouse_receive_return (@deviceid, @UsrUpd)
                        string strSQL = "sp_update_warehouse_receive_return '" + escapedDeviceID + "', '" + escapedUser + "'";
                        
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
                
                // 4. Refresh GridView
                Open_GridView_Pending();
                UpdatePanelPending.Update();
                
                // 5. Show result
                if (failCount == 0)
                {
                    // All success
                    string devices = successDevices.ToString().TrimEnd(',', ' ');
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-success' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-check'></i> Success!</strong><br/>" +
                        "<strong>{0}</strong> device(s) have been received successfully!<br/>" +
                        "<small>Devices: {1}</small>" +
                        "</div>", successCount, devices);
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
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Error!</strong> " + ex.Message + "</div>";
            }
        }

    }
}

