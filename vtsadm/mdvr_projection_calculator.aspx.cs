using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mdvr_projection_calculator : Page
    {
        private static readonly CultureInfo IdCulture = new CultureInfo("id-ID");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!EnsureUserLogin())
                {
                    return;
                }

                BindDropdowns(IsPostBack);

                if (!IsPostBack)
                {
                    pnlResult.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void ddlDeviceMdvr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var deviceTypeId = ddlDeviceMdvr.SelectedValue;
                if (deviceTypeId == "[Select]")
                {
                    deviceTypeId = string.Empty;
                }

                LoadPackageDropdown(deviceTypeId);
                pnlResult.Visible = false;
                div_comment.InnerHtml = string.Empty;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPackage.SelectedValue == "[Select]" || string.IsNullOrWhiteSpace(ddlPackage.SelectedValue))
                {
                    throw new InvalidOperationException("Package wajib dipilih.");
                }

                int projectionQty;
                if (!int.TryParse(txtProjectionQty.Text.Trim(), out projectionQty) || projectionQty <= 0)
                {
                    throw new InvalidOperationException("Qty unit harus lebih besar dari 0.");
                }

                var table = ExecuteProjectionCalculate(ddlPackage.SelectedValue, projectionQty);
                if (table.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Data proyeksi tidak ditemukan.");
                }

                BindSummary(table);
                gvProjectionDetail.DataSource = table;
                gvProjectionDetail.DataBind();
                pnlResult.Visible = true;
                div_comment.InnerHtml = string.Empty;
            }
            catch (Exception ex)
            {
                pnlResult.Visible = false;
                ShowError(GetSqlErrorMessage(ex));
            }
        }

        protected void gvProjectionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            var stockStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StockStatus"));
            if (string.Equals(stockStatus, "TIDAK CUKUP", StringComparison.OrdinalIgnoreCase))
            {
                e.Row.CssClass = "row-stock-shortage";
            }

            var lblStockStatus = e.Row.FindControl("lblStockStatus") as Label;
            if (lblStockStatus != null)
            {
                var isOk = string.Equals(stockStatus, "CUKUP", StringComparison.OrdinalIgnoreCase);
                lblStockStatus.Text = isOk
                    ? "<span class='mdvr-pill mdvr-pill-ok'><i class='fa fa-check'></i> CUKUP</span>"
                    : "<span class='mdvr-pill mdvr-pill-bad'><i class='fa fa-times'></i> TIDAK CUKUP</span>";
            }
        }

        private DataTable ExecuteProjectionCalculate(string packageId, int projectionQty)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.sp_mdvr_projection_calculate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add(new SqlParameter("@PackageID", SqlDbType.VarChar, 20) { Value = packageId });
                cmd.Parameters.Add(new SqlParameter("@ProjectionQty", SqlDbType.Int) { Value = projectionQty });

                conn.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        private void BindSummary(DataTable table)
        {
            var firstRow = table.Rows[0];
            var projectionQty = Convert.ToInt32(firstRow["ProjectionQty"]);

            litDeviceMdvr.Text = HttpUtility.HtmlEncode(Convert.ToString(firstRow["HeaderDeviceTypeDesc"]));
            litPackageName.Text = HttpUtility.HtmlEncode(Convert.ToString(firstRow["PackageName"]));
            litProjectionQty.Text = HttpUtility.HtmlEncode(projectionQty.ToString(IdCulture));

            decimal totalHpp = 0m;
            decimal totalSelling = 0m;
            var stockReadyCount = 0;
            var stockTotalCount = table.Rows.Count;

            foreach (DataRow row in table.Rows)
            {
                totalHpp += Convert.ToDecimal(row["TotalHppPrice"]);
                totalSelling += Convert.ToDecimal(row["TotalSellingPrice"]);

                if (string.Equals(Convert.ToString(row["StockStatus"]), "CUKUP", StringComparison.OrdinalIgnoreCase))
                {
                    stockReadyCount++;
                }
            }

            var grossMargin = totalSelling - totalHpp;
            var grossMarginPct = totalSelling > 0m ? (grossMargin / totalSelling) * 100m : 0m;
            var allReady = stockReadyCount == stockTotalCount;

            litTotalHpp.Text = FormatCurrency(totalHpp);
            litTotalSelling.Text = FormatCurrency(totalSelling);
            litGrossMargin.Text = FormatCurrency(grossMargin);
            litGrossMarginPct.Text = grossMarginPct.ToString("N2", IdCulture) + "%";

            if (table.Columns.Contains("HeaderTotalHppPrice") && table.Columns.Contains("HeaderTotalSellingPrice"))
            {
                var headerTotalHpp = Convert.ToDecimal(firstRow["HeaderTotalHppPrice"]);
                var headerTotalSelling = Convert.ToDecimal(firstRow["HeaderTotalSellingPrice"]);
                litHeaderPriceRef.Text = HttpUtility.HtmlEncode(
                    "HPP " + FormatCurrency(headerTotalHpp) + " | Selling " + FormatCurrency(headerTotalSelling));
                pnlHeaderPriceRef.Visible = true;
            }
            else
            {
                pnlHeaderPriceRef.Visible = false;
            }

            litOverallStatus.Text = allReady
                ? "<span class='mdvr-badge-ready'><i class='fa fa-check-circle'></i> READY</span>"
                : "<span class='mdvr-badge-not-ready'><i class='fa fa-exclamation-circle'></i> NOT READY</span>";

            litStockSummary.Text = HttpUtility.HtmlEncode(
                stockReadyCount + " / " + stockTotalCount + " item stock CUKUP");
        }

        private void BindDropdowns(bool preserveSelection)
        {
            var selectedDevice = ddlDeviceMdvr.SelectedValue;
            var selectedPackage = ddlPackage.SelectedValue;

            LoadDeviceDropdown();

            if (preserveSelection)
            {
                SetDropdownValue(ddlDeviceMdvr, selectedDevice);
                var deviceTypeId = ddlDeviceMdvr.SelectedValue;
                if (deviceTypeId == "[Select]")
                {
                    deviceTypeId = string.Empty;
                }

                LoadPackageDropdown(deviceTypeId);
                SetDropdownValue(ddlPackage, selectedPackage);
            }
            else
            {
                LoadPackageDropdown(string.Empty);
            }
        }

        private static void SetDropdownValue(DropDownList dropdown, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var item = dropdown.Items.FindByValue(value);
            if (item != null)
            {
                dropdown.ClearSelection();
                item.Selected = true;
            }
        }

        private void LoadDeviceDropdown()
        {
            ddlDeviceMdvr.Items.Clear();
            ddlDeviceMdvr.Items.Add(new ListItem("[Select]", "[Select]"));

            var table = ExecuteDataTable("dbo.sp_mdvr_dropdown_device", null);
            foreach (DataRow row in table.Rows)
            {
                ddlDeviceMdvr.Items.Add(new ListItem(
                    Convert.ToString(row["DeviceTypeDesc"]),
                    Convert.ToString(row["DeviceTypeID"])));
            }
        }

        private void LoadPackageDropdown(string deviceTypeId)
        {
            ddlPackage.Items.Clear();
            ddlPackage.Items.Add(new ListItem("[Select]", "[Select]"));

            if (string.IsNullOrWhiteSpace(deviceTypeId))
            {
                return;
            }

            var parameters = new[]
            {
                new SqlParameter("@DeviceTypeID", SqlDbType.VarChar, 20) { Value = deviceTypeId }
            };

            var table = ExecuteDataTable("dbo.sp_mdvr_dropdown_package_by_device", parameters);
            foreach (DataRow row in table.Rows)
            {
                ddlPackage.Items.Add(new ListItem(
                    Convert.ToString(row["DisplayName"]),
                    Convert.ToString(row["PackageID"])));
            }
        }

        private DataTable ExecuteDataTable(string procedureName, SqlParameter[] parameters)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        private static string FormatCurrency(decimal value)
        {
            return value.ToString("N0", IdCulture);
        }

        private bool EnsureUserLogin()
        {
            if (Session["ClsTypeIsLogin"] == null)
            {
                Response.Redirect("login.aspx");
                return false;
            }

            var clType = new ClsType();
            if (!clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
            {
                Response.Redirect("login.aspx");
                return false;
            }

            return true;
        }

        private string GetConnectionString()
        {
            if (Session["ClsTypeDBConnStringSQL"] == null ||
                string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString()))
            {
                throw new InvalidOperationException("Session connection string is not available.");
            }

            var rawConnectionString = Session["ClsTypeDBConnStringSQL"].ToString().Trim();
            try
            {
                return new SqlConnectionStringBuilder(rawConnectionString).ConnectionString;
            }
            catch
            {
                return ConvertOleDbToSqlConnectionString(rawConnectionString);
            }
        }

        private static string ConvertOleDbToSqlConnectionString(string rawConnectionString)
        {
            var sourceBuilder = new DbConnectionStringBuilder { ConnectionString = rawConnectionString };
            var map = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string key in sourceBuilder.Keys)
            {
                map[key] = Convert.ToString(sourceBuilder[key]);
            }

            var sqlBuilder = new SqlConnectionStringBuilder();
            SetIfAny(map, sqlBuilder, "Data Source", "Data Source", "Server", "Address", "Addr", "Network Address");
            SetIfAny(map, sqlBuilder, "Initial Catalog", "Initial Catalog", "Database");
            SetIfAny(map, sqlBuilder, "User ID", "User ID", "UID");
            SetIfAny(map, sqlBuilder, "Password", "Password", "Pwd");
            SetIfAny(map, sqlBuilder, "Integrated Security", "Integrated Security", "Trusted_Connection");
            SetIfAny(map, sqlBuilder, "Connect Timeout", "Connect Timeout");
            SetIfAny(map, sqlBuilder, "Persist Security Info", "Persist Security Info");
            SetIfAny(map, sqlBuilder, "Encrypt", "Encrypt");
            SetIfAny(map, sqlBuilder, "TrustServerCertificate", "TrustServerCertificate");

            if (string.IsNullOrWhiteSpace(sqlBuilder.DataSource))
            {
                throw new InvalidOperationException("Connection string is invalid: Data Source/Server was not found.");
            }

            return sqlBuilder.ConnectionString;
        }

        private static void SetIfAny(
            System.Collections.Generic.IDictionary<string, string> map,
            SqlConnectionStringBuilder sqlBuilder,
            string targetKey,
            params string[] candidateKeys)
        {
            foreach (var candidateKey in candidateKeys)
            {
                string value;
                if (!map.TryGetValue(candidateKey, out value) || string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                sqlBuilder[targetKey] = value.Trim();
                return;
            }
        }

        private static string GetSqlErrorMessage(Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null && sqlEx.Number >= 50000)
            {
                return sqlEx.Message;
            }

            return ex.Message;
        }

        private void ShowError(string message)
        {
            div_comment.InnerHtml = "<div class='mdvr-alert-danger' role='alert'><i class='fa fa-exclamation-circle' style='margin-right:8px;'></i>" + HttpUtility.HtmlEncode(message) + "</div>";
        }
    }
}
