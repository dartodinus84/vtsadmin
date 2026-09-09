using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_job_list_installation : System.Web.UI.Page
    {
        private const string SessionDataKey = "RecDashboardJobListInstallation";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHJO"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(btnExport);

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Response.Redirect("login.aspx");
                        return;
                    }

                    BindGrid();
                }
            }
            catch (Exception)
            {
                // Keep existing project pattern: swallow unexpected errors in page lifecycle.
            }
        }

        protected void gvInstallation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable cachedTable = Session[SessionDataKey] as DataTable;
            gvInstallation.PageIndex = e.NewPageIndex;

            if (cachedTable == null)
            {
                cachedTable = LoadData();
                Session[SessionDataKey] = cachedTable;
            }

            gvInstallation.DataSource = cachedTable;
            gvInstallation.DataBind();
            UpdatePagingLabel(cachedTable);
        }

        private void BindGrid()
        {
            DataTable dt = LoadData();
            Session[SessionDataKey] = dt;
            gvInstallation.DataSource = dt;
            gvInstallation.DataBind();
            UpdatePagingLabel(dt);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string statusQuery;
                string statusTitle;
                string regionQuery;
                string regionDisplay;
                string dateFromQuery;
                string dateToQuery;
                string filterType;

                GetCurrentFilters(out statusQuery, out statusTitle, out regionQuery, out regionDisplay, out dateFromQuery, out dateToQuery, out filterType);
                DataTable sourceTable = ExecuteDataTable(filterType, statusQuery, regionQuery, dateFromQuery, dateToQuery);

                string[] dataFields = new string[]
                {
                    "JobID", "sRegDate", "CustomerName", "BranchName", "RegionalName", "AreaName", "AreaGroupName", "PONumber", "PoTypeDesc",
                    "MarketingName", "Quantity", "QuantityDone", "QuantityGPS", "QuantityGPSDone", "QuantityACS", "QuantityACSDone", "sSchDate",
                    "CloseDate", "sla", "Remark", "BillAbleDesc", "IsMigrationDesc", "ValueStatus"
                };

                string[] headers = new string[]
                {
                    "Job ID", "Register Date", "Customer Name", "Branch Name", "Regional Name", "Area Name", "Area Group Name", "PO Number", "PO Type",
                    "Marketing", "Total Quantity", "Total Quantity Done", "GPS Quantity Target", "GPS Quantity Done", "ACS Quantity Target", "ACS Quantity Done",
                    "Schedule Date", "Close Date", "SLA", "Remark", "Billable", "Migration", "Status"
                };

                DataTable exportTable = BuildExportTable(sourceTable, dataFields, headers);
                string fileName = "JobInstallation_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string regionText = string.IsNullOrWhiteSpace(regionDisplay) ? "All" : regionDisplay;
                string dateRange = dateFromQuery + " - " + dateToQuery;

                ExportDataTableToXls(exportTable, fileName, "Job Installation - " + statusTitle, regionText, dateRange);
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.Write("Export failed: " + ex.Message);
                Response.End();
            }
        }

        private void UpdatePagingLabel(DataTable dt)
        {
            int totalRows = dt == null ? 0 : dt.Rows.Count;
            lblPaging.Text = totalRows <= 0 ? "Total data: 0" : "Total data: " + totalRows.ToString("N0");
        }

        private void SetHeaderContext(string status, string regionDisplay, string dateFrom, string dateTo)
        {
            string statusTitle = GetStatusTitle(status);
            string regionText = string.IsNullOrWhiteSpace(regionDisplay) ? "All" : regionDisplay;
            string dateText = dateFrom + " - " + dateTo;

            lblHeaderTitle.Text = "New Installation - " + statusTitle;
            lblContextStatus.Text = "New Installation - " + statusTitle;
            lblContextRegion.Text = regionText;
            lblContextDate.Text = dateText;
        }

        private string NormalizeStatusParam(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return "All";
            }

            string normalized = status.Trim().ToLowerInvariant();
            if (normalized == "open") return "open";
            if (normalized == "process") return "process";
            if (normalized == "close") return "close";
            return "All";
        }

        private string GetStatusTitle(string status)
        {
            string normalized = NormalizeStatusParam(status);
            if (normalized == "open") return "Open";
            if (normalized == "process") return "Process";
            if (normalized == "close") return "Close";
            return "All";
        }

        private string NormalizeDateOrToday(string value)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(value, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd");
            }

            return DateTime.Today.ToString("yyyy-MM-dd");
        }

        private string NormalizeRegion(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private string NormalizeRegionName(string value)
        {
            string regionName = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
            if (string.Equals(regionName, "[Select]", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(regionName, "All", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return regionName;
        }

        private string GetFilterType()
        {
            object sessionFilterType = Session["SessionFilterType"];
            if (sessionFilterType == null) return string.Empty;

            string filterType = Convert.ToString(sessionFilterType).Trim();
            return filterType == "[Select]" ? string.Empty : filterType;
        }

        private string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            List<string> pairs = new List<string>();
            string[] parts = connectionString.Split(';');

            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();
                if (string.IsNullOrEmpty(part)) continue;

                int separatorIndex = part.IndexOf('=');
                string key = separatorIndex >= 0 ? part.Substring(0, separatorIndex).Trim().ToUpperInvariant() : part.ToUpperInvariant();
                if (key == "PROVIDER") continue;

                pairs.Add(part);
            }

            return string.Join(";", pairs);
        }

        private DataTable ExecuteDataTable(string filterType, string status, string region, string dateFrom, string dateTo)
        {
            DataTable dt = new DataTable();

            string rawConn = Session["ClsTypeDBConnStringSQL"] == null ? string.Empty : Session["ClsTypeDBConnStringSQL"].ToString();
            string sqlConn = GetSqlClientConnectionString(rawConn);

            if (string.IsNullOrWhiteSpace(sqlConn))
            {
                return dt;
            }

            using (SqlConnection conn = new SqlConnection(sqlConn))
            using (SqlCommand cmd = new SqlCommand("sp_dashboard_job_list_installation", conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FilterType", filterType ?? string.Empty);
                cmd.Parameters.AddWithValue("@status", status ?? "All");
                cmd.Parameters.AddWithValue("@supareaid", region ?? string.Empty);
                cmd.Parameters.AddWithValue("@datefrom", dateFrom ?? string.Empty);
                cmd.Parameters.AddWithValue("@dateto", dateTo ?? string.Empty);

                da.Fill(dt);
            }

            return dt;
        }

        private void GetCurrentFilters(out string statusQuery, out string statusTitle, out string regionQuery, out string regionDisplay, out string dateFromQuery, out string dateToQuery, out string filterType)
        {
            statusQuery = NormalizeStatusParam(Request.QueryString["status"]);
            statusTitle = GetStatusTitle(statusQuery);
            regionQuery = NormalizeRegion(Request.QueryString["region"]);
            string regionNameQuery = NormalizeRegionName(Request.QueryString["regionName"]);
            dateFromQuery = NormalizeDateOrToday(Request.QueryString["from"]);
            dateToQuery = NormalizeDateOrToday(Request.QueryString["to"]);
            filterType = GetFilterType();
            regionDisplay = string.IsNullOrWhiteSpace(regionNameQuery) ? regionQuery : regionNameQuery;
        }

        private DataTable BuildExportTable(DataTable sourceTable, string[] dataFields, string[] headers)
        {
            DataTable exportTable = new DataTable();

            for (int i = 0; i < headers.Length; i++)
            {
                exportTable.Columns.Add(headers[i]);
            }

            if (sourceTable == null)
            {
                return exportTable;
            }

            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                DataRow newRow = exportTable.NewRow();
                for (int i = 0; i < dataFields.Length; i++)
                {
                    string fieldName = dataFields[i];
                    newRow[i] = sourceTable.Columns.Contains(fieldName) && sourceRow[fieldName] != DBNull.Value
                        ? sourceRow[fieldName].ToString()
                        : string.Empty;
                }

                exportTable.Rows.Add(newRow);
            }

            return exportTable;
        }

        private void ExportDataTableToXls(DataTable dt, string fileName, string reportTitle, string region, string dateRange)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            hw.Write("<style>table{border-collapse:collapse;} td,th{border:1px solid #d0d0d0;padding:4px;} th{background:#f5f5f5;}</style>");
            hw.Write("<table>");
            hw.Write("<tr><td colspan='" + dt.Columns.Count + "'><strong>" + Server.HtmlEncode(reportTitle) + "</strong></td></tr>");
            hw.Write("<tr><td colspan='" + dt.Columns.Count + "'>Region: " + Server.HtmlEncode(region) + "</td></tr>");
            hw.Write("<tr><td colspan='" + dt.Columns.Count + "'>Date: " + Server.HtmlEncode(dateRange) + "</td></tr>");
            hw.Write("<tr><td colspan='" + dt.Columns.Count + "'>&nbsp;</td></tr>");
            hw.Write("</table>");

            hw.Write("<table>");
            hw.Write("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                hw.Write("<th>" + Server.HtmlEncode(column.ColumnName) + "</th>");
            }
            hw.Write("</tr>");

            foreach (DataRow row in dt.Rows)
            {
                hw.Write("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    hw.Write("<td>" + Server.HtmlEncode(Convert.ToString(row[column])) + "</td>");
                }
                hw.Write("</tr>");
            }
            hw.Write("</table>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public DataTable LoadData()
        {
            string statusQuery;
            string statusTitle;
            string regionQuery;
            string regionDisplay;
            string dateFromQuery;
            string dateToQuery;
            string filterType;

            GetCurrentFilters(out statusQuery, out statusTitle, out regionQuery, out regionDisplay, out dateFromQuery, out dateToQuery, out filterType);

            SetHeaderContext(statusQuery, regionDisplay, dateFromQuery, dateToQuery);

            return ExecuteDataTable(filterType, statusQuery, regionQuery, dateFromQuery, dateToQuery);
        }
    }
}
