using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_device_stock_monitoring : System.Web.UI.Page
    {
        protected int TotalDeviceTypes { get; private set; }
        protected int TotalAvailableDevices { get; private set; }
        protected int TotalDeviceTypesBelowMinimum { get; private set; }
        protected string StockTableRowsHtml { get; private set; }
        protected string LowStockRowsHtml { get; private set; }
        protected string ErrorMessage { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            StockTableRowsHtml = string.Empty;
            LowStockRowsHtml = string.Empty;
            ErrorMessage = string.Empty;

            try
            {
                DataTable stockData = GetStockMonitoringData();
                CalculateSummary(stockData);
                StockTableRowsHtml = BuildStockTableRows(stockData);
                LowStockRowsHtml = BuildLowStockRows(stockData);
            }
            catch (Exception ex)
            {
                TotalDeviceTypes = 0;
                TotalAvailableDevices = 0;
                TotalDeviceTypesBelowMinimum = 0;
                StockTableRowsHtml = "<tr><td colspan='4' class='text-center text-muted'>No data available.</td></tr>";
                LowStockRowsHtml = "<tr><td colspan='3' class='text-center text-muted'>No alert data.</td></tr>";
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
            }
        }

        private DataTable GetStockMonitoringData()
        {
            if (Session["ClsTypeDBConnStringSQL"] == null || string.IsNullOrWhiteSpace(Convert.ToString(Session["ClsTypeDBConnStringSQL"])))
                throw new InvalidOperationException("Session connection string (ClsTypeDBConnStringSQL) tidak ditemukan.");

            Recordset rec = new Recordset();
            rec.Open("sp_dashboard_device_stock_monitoring", Session["ClsTypeDBConnStringSQL"].ToString());

            return rec.DataRecord();
        }

        private void CalculateSummary(DataTable stockData)
        {
            if (stockData == null || stockData.Rows.Count == 0)
            {
                TotalDeviceTypes = 0;
                TotalAvailableDevices = 0;
                TotalDeviceTypesBelowMinimum = 0;
                return;
            }

            TotalDeviceTypes = stockData.Rows.Count;
            TotalAvailableDevices = stockData.AsEnumerable().Sum(row => ToInt(row, "AvailableStock"));
            TotalDeviceTypesBelowMinimum = stockData.AsEnumerable()
                .Count(row => !string.Equals(ToSafeString(row, "StockStatus"), "SAFE", StringComparison.OrdinalIgnoreCase));
        }

        private string BuildStockTableRows(DataTable stockData)
        {
            if (stockData == null || stockData.Rows.Count == 0)
            {
                return "<tr><td colspan='4' class='text-center text-muted'>No data available.</td></tr>";
            }

            StringBuilder html = new StringBuilder();
            foreach (DataRow row in stockData.Rows)
            {
                string deviceType = HttpUtility.HtmlEncode(ToSafeString(row, "DeviceTypeDesc"));
                int availableStock = ToInt(row, "AvailableStock");
                int minimumStock = ToInt(row, "MinimumStock");
                string stockStatus = ToSafeString(row, "StockStatus").ToUpperInvariant();
                string statusCssClass = GetStatusCssClass(stockStatus);

                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", deviceType);
                html.AppendFormat("<td class='text-right'>{0:N0}</td>", availableStock);
                html.AppendFormat("<td class='text-right'>{0:N0}</td>", minimumStock);
                html.AppendFormat("<td><span class='status-pill {0}'>{1}</span></td>", statusCssClass, HttpUtility.HtmlEncode(stockStatus));
                html.Append("</tr>");
            }

            return html.ToString();
        }

        private string BuildLowStockRows(DataTable stockData)
        {
            if (stockData == null || stockData.Rows.Count == 0)
            {
                return "<tr><td colspan='3' class='text-center text-muted'>No alert data.</td></tr>";
            }

            var lowStockRows = stockData.AsEnumerable()
                .Where(row =>
                    string.Equals(ToSafeString(row, "StockStatus"), "LOW", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(ToSafeString(row, "StockStatus"), "CRITICAL", StringComparison.OrdinalIgnoreCase))
                .OrderBy(row => ToInt(row, "AvailableStock"))
                .ToList();

            if (lowStockRows.Count == 0)
            {
                return "<tr><td colspan='3' class='text-center text-success'>All device types are SAFE.</td></tr>";
            }

            StringBuilder html = new StringBuilder();
            foreach (DataRow row in lowStockRows)
            {
                string deviceType = HttpUtility.HtmlEncode(ToSafeString(row, "DeviceTypeDesc"));
                int availableStock = ToInt(row, "AvailableStock");
                string stockStatus = ToSafeString(row, "StockStatus").ToUpperInvariant();
                string statusCssClass = GetStatusCssClass(stockStatus);

                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", deviceType);
                html.AppendFormat("<td class='text-right'>{0:N0}</td>", availableStock);
                html.AppendFormat("<td><span class='status-pill {0}'>{1}</span></td>", statusCssClass, HttpUtility.HtmlEncode(stockStatus));
                html.Append("</tr>");
            }

            return html.ToString();
        }

        private static int ToInt(DataRow row, string columnName)
        {
            if (row == null || row[columnName] == DBNull.Value)
            {
                return 0;
            }

            int parsed;
            if (int.TryParse(Convert.ToString(row[columnName]), out parsed))
            {
                return parsed;
            }

            return 0;
        }

        private static string ToSafeString(DataRow row, string columnName)
        {
            if (row == null || row[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(row[columnName]).Trim();
        }

        private static string GetStatusCssClass(string stockStatus)
        {
            switch (stockStatus)
            {
                case "SAFE":
                    return "status-safe";
                case "LOW":
                    return "status-low";
                case "CRITICAL":
                    return "status-critical";
                default:
                    return "status-low";
            }
        }
    }
}
