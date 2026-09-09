using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_qc_report : System.Web.UI.Page
    {
        private string connectionString;
        private string userID;
        private string userRole;

        // ViewState & Session keys
        string sViewStateFieldSortSummary = "RecListDeviceQCReportSummaryFieldSort";
        string sViewStateDirSortSummary = "RecListDeviceQCReportSummaryDirSort";
        string sSessionRecListSummary = "RecListDeviceQCReportSummary";

        string sViewStateFieldSortUser = "RecListDeviceQCReportUserFieldSort";
        string sViewStateDirSortUser = "RecListDeviceQCReportUserDirSort";
        string sSessionRecListUser = "RecListDeviceQCReportUser";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check login
                if (Session["ClsTypeIsLogin"] == null || !Convert.ToBoolean(Session["ClsTypeIsLogin"]))
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                // Get connection string from session
                connectionString = Session["ClsTypeDBConnStringSQL"]?.ToString();

                // Get user info
                userID = Session["ClsTypeUserID"]?.ToString() ?? "";
                
                // Determine user role (Manager or User QC)
                // LM = Manager, Administrators = Admin, selain itu = User QC
                string groupID = Session["ClsTypeUserGroupID"]?.ToString() ?? "";
                userRole = IsManager(groupID) ? "Manager" : "User";

                // Initialize UI based on role
                if (!IsPostBack)
                {
                    InitializePage();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading page: " + ex.Message);
            }
        }

        private void InitializePage()
        {
            try
            {
                // Set default dates (current month)
                DateTime now = DateTime.Now;
                txtStartDate.Text = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
                txtEndDate.Text = now.ToString("yyyy-MM-dd");

                // Load dropdowns
                LoadDeviceTypes();
                if (userRole == "Manager")
                {
                    LoadUserQCList();
                    pnlManagerReport.Visible = true;
                    pnlUserQCReport.Visible = false;
                }
                else
                {
                    divUserQC.Visible = false;
                    pnlManagerReport.Visible = false;
                    pnlUserQCReport.Visible = true;
                }

                // Load initial reports
                if (userRole == "Manager")
                {
                    LoadSummary();
                    LoadUserReport();
                    LoadYearly();
                }
                else
                {
                    LoadUserQCReport();
                    LoadUserQCDetail();
                    LoadYearly();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error initializing page: " + ex.Message);
            }
        }

        private bool IsManager(string groupID)
        {
            // LM = Manager, Administrators = Admin, selain itu = User QC
            if (string.IsNullOrEmpty(groupID))
            {
                return false;
            }

            string groupIDUpper = groupID.ToUpper().Trim();
            
            // Check if GroupID is "LM" (Manager) or "ADMINISTRATORS" (Admin)
            return groupIDUpper == "LM" || groupIDUpper == "ADMINISTRATORS";
        }

        private void LoadDeviceTypes()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(ddlDeviceType, connectionString, "", "sp_list_device_devicetype");
                // Insert [All Device Types] at the top
                if (ddlDeviceType.Items.Count > 0 && ddlDeviceType.Items[0].Value != "")
                {
                    ddlDeviceType.Items.Insert(0, new ListItem("[All Device Types]", ""));
                }
            }
            catch (Exception ex)
            {
                // Log error but don't break the page
                System.Diagnostics.Debug.WriteLine("Error loading device types: " + ex.Message);
            }
        }

        private void LoadUserQCList()
        {
            try
            {
                ddlUserQC.Items.Clear();
                ddlUserQC.Items.Add(new ListItem("[All Users]", ""));

                // Get users who have performed QC using stored procedure
                Recordset Rec = new Recordset();
                string strSQL = "sp_get_user_wqc_active";
                
                string sErr = "";
                Rec.Open(strSQL, connectionString, ref sErr);
                
                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Database error: " + sErr);
                }
                
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        // Assuming SP returns UserID and UserName columns (adjust field names as needed)
                        string qcUserID = Rec.Fields(0)?.ToString() ?? "";
                        string qcUserName = Rec.Fields(1)?.ToString() ?? qcUserID;
                        
                        // Try by column name if index doesn't work
                        if (string.IsNullOrEmpty(qcUserID))
                        {
                            qcUserID = Rec.Fields("UserID")?.ToString() ?? "";
                        }
                        if (string.IsNullOrEmpty(qcUserName) || qcUserName == qcUserID)
                        {
                            qcUserName = Rec.Fields("UserName")?.ToString() ?? qcUserID;
                        }
                        
                        if (!string.IsNullOrEmpty(qcUserID) && qcUserID != "[Select]")
                        {
                            ddlUserQC.Items.Add(new ListItem(qcUserName, qcUserID));
                        }
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                // If SP fails, at least we have [All Users] option
                System.Diagnostics.Debug.WriteLine("Error loading user QC list: " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                pnlLoader.Visible = true;
                UpdatePanelLoader.Update();

                // Validate dates
                if (string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
                {
                    ShowError("Start Date and End Date are required!");
                    pnlLoader.Visible = false;
                    UpdatePanelLoader.Update();
                    return;
                }

                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);

                if (startDate > endDate)
                {
                    ShowError("Start Date cannot be greater than End Date!");
                    pnlLoader.Visible = false;
                    UpdatePanelLoader.Update();
                    return;
                }

                // Load reports based on role
                if (userRole == "Manager")
                {
                    LoadSummary();
                    LoadUserReport();
                    LoadYearly();
                }
                else
                {
                    LoadUserQCReport();
                    LoadUserQCDetail();
                    LoadYearly();
                }

                pnlLoader.Visible = false;
                UpdatePanelLoader.Update();
            }
            catch (Exception ex)
            {
                pnlLoader.Visible = false;
                UpdatePanelLoader.Update();
                ShowError("Error loading data: " + ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset dates to current month
                DateTime now = DateTime.Now;
                txtStartDate.Text = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
                txtEndDate.Text = now.ToString("yyyy-MM-dd");

                // Clear dropdowns
                ddlDeviceType.SelectedValue = "";
                if (userRole == "Manager")
                {
                    ddlUserQC.SelectedValue = "";
                }

                // Clear grids
                gvSummary.DataSource = null;
                gvSummary.DataBind();
                gvUser.DataSource = null;
                gvUser.DataBind();
                gvUserQC.DataSource = null;
                gvUserQC.DataBind();
                gvUserQCDetail.DataSource = null;
                gvUserQCDetail.DataBind();
                gvYearly.DataSource = null;
                gvYearly.DataBind();
                gvDetail.DataSource = null;
                gvDetail.DataBind();

                UpdatePanelSummary.Update();
                UpdatePanelUser.Update();
                UpdatePanelUserQC.Update();
                UpdatePanelUserQCDetail.Update();
                UpdatePanelYearly.Update();
                UpdatePanelDetail.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error clearing data: " + ex.Message);
            }
        }

        private void LoadSummary()
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                string deviceTypeID = ddlDeviceType.SelectedValue ?? "";

                // Build SQL string with parameters (format: SP name followed by space and parameters)
                string strSQL = "dbo.sp_report_device_qc";
                if (string.IsNullOrEmpty(deviceTypeID))
                {
                    strSQL += " 'SUMMARY','" + startDate.ToString("yyyy-MM-dd") + "','" + endDate.ToString("yyyy-MM-dd") + "',NULL,NULL";
                }
                else
                {
                    strSQL += " 'SUMMARY','" + startDate.ToString("yyyy-MM-dd") + "','" + endDate.ToString("yyyy-MM-dd") + "',NULL,'" + deviceTypeID.Replace("'", "''") + "'";
                }

                ViewState[sViewStateFieldSortSummary] = "DeviceTypeID";
                ViewState[sViewStateDirSortSummary] = "ASC";

                ClsType ClType = new ClsType();
                string sErr = "";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, connectionString, ref sErr);
                
                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Database error loading Summary Report: " + sErr + " | SQL: " + strSQL);
                }
                
                if (Rec.RecordCount() > 0)
                {
                    gvSummary.DataSource = Rec.RecData;
                    gvSummary.DataBind();
                    LblPagingSummary.Text = string.Format("Displaying {0} records found", Rec.RecordCount());
                }
                else
                {
                    gvSummary.DataSource = null;
                    gvSummary.DataBind();
                    LblPagingSummary.Text = "No records found";
                }
                
                Session[sSessionRecListSummary] = Rec.RecData;
                UpdatePanelSummary.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error loading Summary Report: " + ex.Message);
            }
        }

        private void LoadUserReport()
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                string deviceTypeID = ddlDeviceType.SelectedValue ?? "";
                string qcBy = ddlUserQC.SelectedValue ?? "";

                ViewState[sViewStateFieldSortUser] = "DeviceType";
                ViewState[sViewStateDirSortUser] = "ASC";

                // PANGGIL SP LANGSUNG DENGAN PARAMETER YANG BENAR (DBNull.Value untuk NULL)
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    using (OleDbCommand cmd = new OleDbCommand("dbo.sp_report_device_qc", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Urutan parameter HARUS sama dengan definisi SP:
                        // (@mode, @start_date, @end_date, @qc_by, @device_type_id)
                        cmd.Parameters.AddWithValue("@mode", "USER");
                        cmd.Parameters.AddWithValue("@start_date", startDate);
                        cmd.Parameters.AddWithValue("@end_date", endDate);
                        if (string.IsNullOrEmpty(qcBy))
                            cmd.Parameters.AddWithValue("@qc_by", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@qc_by", qcBy);

                        if (string.IsNullOrEmpty(deviceTypeID))
                            cmd.Parameters.AddWithValue("@device_type_id", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@device_type_id", deviceTypeID);

                        using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds, "Table1");

                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                gvUser.DataSource = ds.Tables[0];
                                gvUser.DataBind();
                                LblPagingUser.Text = string.Format("Displaying {0} records found", ds.Tables[0].Rows.Count);

                                // Simpan untuk paging
                                Session[sSessionRecListUser] = ds;
                            }
                            else
                            {
                                gvUser.DataSource = null;
                                gvUser.DataBind();
                                LblPagingUser.Text = "No records found";
                                Session[sSessionRecListUser] = null;
                            }
                        }
                    }
                }

                UpdatePanelUser.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error loading Per User Report: " + ex.Message);
            }
        }

        private void LoadDetail(string deviceTypeFilter, string qcBy)
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);

                // Save filter values to ViewState for pagination (simpan DESKRIPSI DeviceType)
                ViewState["DetailDeviceTypeID"] = deviceTypeFilter;
                ViewState["DetailQcBy"] = qcBy;

                // Panggil SP langsung via OleDbCommand supaya NULL benar‑benar jadi DB NULL
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    using (OleDbCommand cmd = new OleDbCommand("dbo.sp_report_device_qc", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Urutan parameter HARUS sama dengan definisi SP:
                        // (@mode, @start_date, @end_date, @qc_by, @device_type_id)
                        cmd.Parameters.AddWithValue("@mode", "DETAIL");
                        cmd.Parameters.AddWithValue("@start_date", startDate);
                        cmd.Parameters.AddWithValue("@end_date", endDate);

                        if (string.IsNullOrEmpty(qcBy))
                            cmd.Parameters.AddWithValue("@qc_by", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@qc_by", qcBy);

                        // Di sisi DB kita tidak filter DeviceTypeID, cukup filter by QcBy + tanggal,
                        // lalu di sisi aplikasi kita saring lagi berdasarkan DeviceType (deskripsi).
                        cmd.Parameters.AddWithValue("@device_type_id", DBNull.Value);

                        using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds, "Table1");

                            if (ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];

                                // Jika ada filter DeviceType (deskripsi), saring di sini
                                if (!string.IsNullOrEmpty(deviceTypeFilter) && dt.Columns.Contains("DeviceType"))
                                {
                                    DataView dv = dt.DefaultView;
                                    // Escape single quote dengan menggandakan
                                    string safeDeviceType = deviceTypeFilter.Replace("'", "''");
                                    dv.RowFilter = $"DeviceType = '{safeDeviceType}'";
                                    dt = dv.ToTable();
                                }

                                gvDetail.DataSource = dt;
                                gvDetail.DataBind();
                            }
                            else
                            {
                                gvDetail.DataSource = null;
                                gvDetail.DataBind();
                            }

                            UpdatePanelDetail.Update();
                        }
                    }
                }

                // Show modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalDetail').modal('show');", true);
            }
            catch (Exception ex)
            {
                ShowError("Error loading detail: " + ex.Message);
            }
        }

        private void LoadUserQCReport()
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                string deviceTypeID = ddlDeviceType.SelectedValue ?? "";

                // Build SQL string with parameters - User QC can only see their own data
                string strSQL = "dbo.sp_report_device_qc";
                string deviceTypeParam = string.IsNullOrEmpty(deviceTypeID) ? "NULL" : "'" + deviceTypeID.Replace("'", "''") + "'";
                
                strSQL += " 'SUMMARY','" + startDate.ToString("yyyy-MM-dd") + "','" + endDate.ToString("yyyy-MM-dd") + "','" + userID.Replace("'", "''") + "'," + deviceTypeParam;

                Recordset Rec = new Recordset();
                string sErr = "";
                Rec.Open(strSQL, connectionString, ref sErr);

                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Database error: " + sErr);
                }

                gvUserQC.DataSource = Rec.RecData;
                gvUserQC.DataBind();
                UpdatePanelUserQC.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error loading user QC report: " + ex.Message);
            }
        }

        private void LoadUserQCDetail()
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                string deviceTypeID = ddlDeviceType.SelectedValue ?? "";

                // Build SQL string with parameters - User QC can only see their own data
                string strSQL = "dbo.sp_report_device_qc";
                string deviceTypeParam = string.IsNullOrEmpty(deviceTypeID) ? "NULL" : "'" + deviceTypeID.Replace("'", "''") + "'";
                
                strSQL += " 'DETAIL','" + startDate.ToString("yyyy-MM-dd") + "','" + endDate.ToString("yyyy-MM-dd") + "','" + userID.Replace("'", "''") + "'," + deviceTypeParam;

                Recordset Rec = new Recordset();
                string sErr = "";
                Rec.Open(strSQL, connectionString, ref sErr);

                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Database error: " + sErr);
                }

                gvUserQCDetail.DataSource = Rec.RecData;
                gvUserQCDetail.DataBind();
                UpdatePanelUserQCDetail.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error loading user QC detail: " + ex.Message);
            }
        }

        private void LoadYearly()
        {
            try
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                string deviceTypeID = ddlDeviceType.SelectedValue ?? "";
                string qcBy = "";

                if (userRole == "Manager")
                {
                    qcBy = ddlUserQC.SelectedValue ?? "";
                }
                else
                {
                    qcBy = userID;
                }

                // Build SQL string with parameters
                string strSQL = "dbo.sp_report_device_qc";
                string qcByParam = string.IsNullOrEmpty(qcBy) ? "NULL" : "'" + qcBy.Replace("'", "''") + "'";
                string deviceTypeParam = string.IsNullOrEmpty(deviceTypeID) ? "NULL" : "'" + deviceTypeID.Replace("'", "''") + "'";
                
                strSQL += " 'YEARLY','" + startDate.ToString("yyyy-MM-dd") + "','" + endDate.ToString("yyyy-MM-dd") + "'," + qcByParam + "," + deviceTypeParam;

                Recordset Rec = new Recordset();
                string sErr = "";
                Rec.Open(strSQL, connectionString, ref sErr);

                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Database error: " + sErr);
                }

                gvYearly.DataSource = Rec.RecData;
                gvYearly.DataBind();
                UpdatePanelYearly.Update();
            }
            catch (Exception ex)
            {
                ShowError("Error loading yearly report: " + ex.Message);
            }
        }

        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    // Get DeviceType (deskripsi) dan QcBy dari CommandArgument
                    string[] args = e.CommandArgument.ToString().Split('|');
                    string deviceTypeDesc = args.Length > 0 ? args[0] : "";
                    string qcBy = args.Length > 1 ? args[1] : "";
                    
                    // Kirim DeviceType (deskripsi) + QcBy ke LoadDetail.
                    // Di DB kita filter by QcBy + tanggal, lalu di aplikasi disaring lagi berdasarkan DeviceType.
                    LoadDetail(deviceTypeDesc, qcBy);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error viewing detail: " + ex.Message);
            }
        }

        protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Use session data for pagination
                if (Session[sSessionRecListUser] != null)
                {
                    DataSet ds = Session[sSessionRecListUser] as DataSet;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        gvUser.PageIndex = e.NewPageIndex;
                        gvUser.DataSource = ds.Tables[0];
                        gvUser.DataBind();
                        
                        // Update paging label
                        long totalRecs = ds.Tables[0].Rows.Count;
                        int currentStart = gvUser.PageIndex * gvUser.PageSize + 1;
                        int currentEnd = gvUser.PageIndex * gvUser.PageSize + gvUser.Rows.Count;
                        if (totalRecs == 0) currentStart = 0;
                        LblPagingUser.Text = string.Format("Displaying {0} to {1} of {2} records found", currentStart, currentEnd, totalRecs);
                        
                        UpdatePanelUser.Update();
                        return;
                    }
                }
                // If no session data, reload
                LoadUserReport();
            }
            catch (Exception ex)
            {
                LoadUserReport();
            }
        }

        protected void gvUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Try to access the button in TemplateField
                    Button btnViewDetail = e.Row.FindControl("btnViewDetail") as Button;
                    if (btnViewDetail != null)
                    {
                        // Get values from data row - SP mode 'USER' returns: DeviceType, QcBy, TotalQty
                        DataRowView rowView = e.Row.DataItem as DataRowView;
                        if (rowView != null)
                        {
                            object deviceType = rowView["DeviceType"];  // SP returns DeviceType (description), not DeviceTypeID
                            object qcBy = rowView["QcBy"];
                            // Pass DeviceType and QcBy as CommandArgument for detail filter
                            btnViewDetail.CommandArgument = (deviceType?.ToString() ?? "") + "|" + (qcBy?.ToString() ?? "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently handle errors in RowDataBound to prevent breaking the grid
                System.Diagnostics.Debug.WriteLine("Error in gvUser_RowDataBound: " + ex.Message);
            }
        }

        protected void gvUserQC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserQC.PageIndex = e.NewPageIndex;
            LoadUserQCReport();
        }

        protected void gvUserQCDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserQCDetail.PageIndex = e.NewPageIndex;
            LoadUserQCDetail();
        }

        protected void gvYearly_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvYearly.PageIndex = e.NewPageIndex;
            LoadYearly();
        }

        protected void gvDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetail.PageIndex = e.NewPageIndex;
            // Get current filter values from ViewState
            string deviceTypeID = ViewState["DetailDeviceTypeID"]?.ToString() ?? "";
            string qcBy = ViewState["DetailQcBy"]?.ToString() ?? "";
            LoadDetail(deviceTypeID, qcBy);
        }

        private void ShowError(string message)
        {
            divMessage.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + message + "</div>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessage", "$('#modalMessageBox').modal('show');", true);
        }

        private void ShowSuccess(string message)
        {
            divMessage.InnerHtml = "<div class='alert alert-success' role='alert'><strong>Success!</strong> " + message + "</div>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMessage", "$('#modalMessageBox').modal('show');", true);
        }

        protected string GetCommandArgument(object deviceTypeID, object qcBy)
        {
            try
            {
                string dtID = deviceTypeID?.ToString() ?? "";
                string qc = qcBy?.ToString() ?? "";
                return dtID + "|" + qc;
            }
            catch
            {
                return "|";
            }
        }
    }
}
