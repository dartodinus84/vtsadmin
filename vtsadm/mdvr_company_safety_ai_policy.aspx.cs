using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mdvr_company_safety_ai_policy : Page
    {
        private const string TableName = "tbl_mdvr_company_safety_ai_policy";
        private const string MenuId = "MNUMDVRSAIPOL";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EnsureUserLogin())
            {
                return;
            }

            if (Session["ClsTypeAccessMenu"] == null ||
                !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains(MenuId))
            {
                Response.Redirect("dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ClearForm();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                DataTable table = LoadPolicies();
                EnrichCompanyNames(table);
                gvData.DataSource = table;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Load data failed: " + ex.Message, false);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int companyId;
                if (!int.TryParse((txtCompID.Value ?? string.Empty).Trim(), out companyId) || companyId <= 0)
                {
                    ShowMessage("Company wajib dipilih.", false);
                    return;
                }

                int intervalSeconds;
                string intervalRaw = (txtIntervalSeconds.Value ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(intervalRaw))
                {
                    intervalSeconds = 30;
                }
                else if (!int.TryParse(intervalRaw, out intervalSeconds) || intervalSeconds <= 0)
                {
                    ShowMessage("Report interval (seconds) harus angka > 0.", false);
                    return;
                }

                string updatedBy = Session["ClsTypeUserID"] != null
                    ? Convert.ToString(Session["ClsTypeUserID"]).Trim()
                    : string.Empty;

                bool isEdit = string.Equals(hfIsEdit.Value, "1", StringComparison.OrdinalIgnoreCase);
                if (isEdit)
                {
                    UpdatePolicy(companyId, intervalSeconds, chkActive.Checked, updatedBy);
                    ShowMessage("Update berhasil.", true);
                }
                else
                {
                    if (PolicyExists(companyId))
                    {
                        ShowMessage("Policy untuk company_id " + companyId + " sudah ada. Gunakan Edit.", false);
                        return;
                    }

                    InsertPolicy(companyId, intervalSeconds, chkActive.Checked, updatedBy);
                    ShowMessage("Save berhasil.", true);
                }

                ClearForm();
                BindGrid();
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(e.CommandArgument)))
            {
                return;
            }

            int companyId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out companyId))
            {
                ShowMessage("Company ID tidak valid.", false);
                return;
            }

            try
            {
                if (string.Equals(e.CommandName, "EDIT_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    LoadData(companyId);
                    return;
                }

                if (string.Equals(e.CommandName, "DELETE_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    DeletePolicy(companyId);
                    if (string.Equals((txtCompID.Value ?? string.Empty).Trim(), companyId.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        ClearForm();
                    }

                    ShowMessage("Delete berhasil.", true);
                    BindGrid();
                }
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        private void LoadData(int companyId)
        {
            DataTable table = LoadPolicies(companyId);
            if (table.Rows.Count == 0)
            {
                ShowMessage("Data tidak ditemukan.", false);
                return;
            }

            DataRow row = table.Rows[0];
            txtCompID.Value = Convert.ToString(row["company_id"]);
            txtCompanyName.Text = ResolveCompanyName(companyId);
            txtIntervalSeconds.Value = row.Table.Columns.Contains("safety_ai_report_interval_seconds")
                ? Convert.ToString(row["safety_ai_report_interval_seconds"])
                : Convert.ToString(row["interval_minutes"]);
            chkActive.Checked = row["is_active"] != DBNull.Value && Convert.ToBoolean(row["is_active"]);
            hfIsEdit.Value = "1";
            btnSave.Text = "Update";
            BtnSearchCustomer.Disabled = true;
            ShowMessage("Mode edit aktif.", true);
        }

        private DataTable LoadPolicies(int? companyId = null)
        {
            string sql = @"
SELECT company_id,
       is_active,
       created_at,
       updated_at,
       updated_by,
       safety_ai_report_interval_seconds
FROM " + TableName + @" WITH (NOLOCK)
WHERE (@company_id IS NULL OR company_id = @company_id)
ORDER BY company_id";

            DataTable table = ExecuteGpsbDataTable(
                sql,
                new SqlParameter("@company_id", SqlDbType.Int) { Value = (object)companyId ?? DBNull.Value });

            EnsureIntervalColumns(table);
            return table;
        }

        private static void EnsureIntervalColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            bool hasSeconds = table.Columns.Contains("safety_ai_report_interval_seconds");
            bool hasMinutes = table.Columns.Contains("interval_minutes");

            if (hasSeconds && !hasMinutes)
            {
                table.Columns.Add("interval_minutes", typeof(int));
                foreach (DataRow row in table.Rows)
                {
                    row["interval_minutes"] = row["safety_ai_report_interval_seconds"] == DBNull.Value
                        ? DBNull.Value
                        : (object)Convert.ToInt32(row["safety_ai_report_interval_seconds"]);
                }
            }
            else if (hasMinutes && !hasSeconds)
            {
                table.Columns.Add("safety_ai_report_interval_seconds", typeof(int));
                foreach (DataRow row in table.Rows)
                {
                    row["safety_ai_report_interval_seconds"] = row["interval_minutes"] == DBNull.Value
                        ? DBNull.Value
                        : (object)Convert.ToInt32(row["interval_minutes"]);
                }
            }
        }

        private bool PolicyExists(int companyId)
        {
            const string sql = @"
SELECT COUNT(1)
FROM tbl_mdvr_company_safety_ai_policy WITH (NOLOCK)
WHERE company_id = @company_id";

            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId });
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void InsertPolicy(int companyId, int intervalSeconds, bool isActive, string updatedBy)
        {
            const string sql = @"
INSERT INTO tbl_mdvr_company_safety_ai_policy
(
    company_id,
    is_active,
    created_at,
    updated_at,
    updated_by,
    safety_ai_report_interval_seconds
)
VALUES
(
    @company_id,
    @is_active,
    GETDATE(),
    GETDATE(),
    @updated_by,
    @safety_ai_report_interval_seconds
)";

            ExecuteGpsbNonQuery(
                sql,
                new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId },
                new SqlParameter("@is_active", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@updated_by", SqlDbType.VarChar, 100) { Value = (object)updatedBy ?? DBNull.Value },
                new SqlParameter("@safety_ai_report_interval_seconds", SqlDbType.Int) { Value = intervalSeconds });
        }

        private void UpdatePolicy(int companyId, int intervalSeconds, bool isActive, string updatedBy)
        {
            const string sql = @"
UPDATE tbl_mdvr_company_safety_ai_policy
SET is_active = @is_active,
    updated_at = GETDATE(),
    updated_by = @updated_by,
    safety_ai_report_interval_seconds = @safety_ai_report_interval_seconds
WHERE company_id = @company_id";

            int affected = ExecuteGpsbNonQuery(
                sql,
                new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId },
                new SqlParameter("@is_active", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@updated_by", SqlDbType.VarChar, 100) { Value = (object)updatedBy ?? DBNull.Value },
                new SqlParameter("@safety_ai_report_interval_seconds", SqlDbType.Int) { Value = intervalSeconds });

            if (affected == 0)
            {
                throw new InvalidOperationException("Data tidak ditemukan untuk di-update.");
            }
        }

        private void DeletePolicy(int companyId)
        {
            const string sql = @"
DELETE FROM tbl_mdvr_company_safety_ai_policy
WHERE company_id = @company_id";

            ExecuteGpsbNonQuery(
                sql,
                new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId });
        }

        private void EnrichCompanyNames(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            if (!table.Columns.Contains("company_nm"))
            {
                table.Columns.Add("company_nm", typeof(string));
            }

            Dictionary<int, string> map = LoadCompanyNameMap();
            foreach (DataRow row in table.Rows)
            {
                int companyId;
                if (!int.TryParse(Convert.ToString(row["company_id"]), out companyId))
                {
                    row["company_nm"] = string.Empty;
                    continue;
                }

                string name;
                row["company_nm"] = map.TryGetValue(companyId, out name) && !string.IsNullOrWhiteSpace(name)
                    ? name
                    : Convert.ToString(companyId);
            }
        }

        private string ResolveCompanyName(int companyId)
        {
            Dictionary<int, string> map = LoadCompanyNameMap();
            string name;
            return map.TryGetValue(companyId, out name) && !string.IsNullOrWhiteSpace(name)
                ? name
                : string.Empty;
        }

        private Dictionary<int, string> LoadCompanyNameMap()
        {
            var map = new Dictionary<int, string>();
            try
            {
                var temp = new DropDownList();
                ClsType clType = new ClsType();
                clType.Open_Combos(temp, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_companyid");
                foreach (ListItem item in temp.Items)
                {
                    int id;
                    if (!int.TryParse(item.Value, out id) || map.ContainsKey(id))
                    {
                        continue;
                    }

                    map[id] = item.Text ?? string.Empty;
                }
            }
            catch
            {
                // Company name is display-only; keep page usable if lookup fails.
            }

            return map;
        }

        private DataTable ExecuteGpsbDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                var table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        private int ExecuteGpsbNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private string GetGpsbConnectionString()
        {
            try
            {
                ConnectionStringSettings gpsb = ConfigurationManager.ConnectionStrings["GPSBDBClient"];
                if (gpsb != null && !string.IsNullOrWhiteSpace(gpsb.ConnectionString))
                {
                    return new SqlConnectionStringBuilder(gpsb.ConnectionString).ConnectionString;
                }
            }
            catch
            {
            }

            try
            {
                ConnectionStringSettings gpsbOle = ConfigurationManager.ConnectionStrings["GPSBDB"];
                if (gpsbOle != null && !string.IsNullOrWhiteSpace(gpsbOle.ConnectionString))
                {
                    return ConvertOleDbToSqlConnectionString(gpsbOle.ConnectionString.Trim());
                }
            }
            catch
            {
            }

            throw new InvalidOperationException("GPSB connection string (GPSBDBClient/GPSBDB) tidak ditemukan.");
        }

        private static string ConvertOleDbToSqlConnectionString(string rawConnectionString)
        {
            var sourceBuilder = new DbConnectionStringBuilder { ConnectionString = rawConnectionString };
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
                throw new InvalidOperationException("Connection string tidak valid: Data Source/Server tidak ditemukan.");
            }

            return sqlBuilder.ConnectionString;
        }

        private static void SetIfAny(
            IDictionary<string, string> map,
            SqlConnectionStringBuilder sqlBuilder,
            string targetKey,
            params string[] candidateKeys)
        {
            foreach (string candidateKey in candidateKeys)
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

        private void ClearForm()
        {
            hfIsEdit.Value = "0";
            txtCompID.Value = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtIntervalSeconds.Value = string.Empty;
            chkActive.Checked = true;
            btnSave.Text = "Save";
            BtnSearchCustomer.Disabled = false;
            div_comment.InnerHtml = string.Empty;
        }

        private void ShowMessage(string msg, bool success)
        {
            string alertClass = success ? "alert-success" : "alert-danger";
            string title = success ? "Success!" : "Failed!";
            div_comment.InnerHtml =
                "<div class='alert " + alertClass + "' role='alert'>" +
                "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                "<span aria-hidden='true'>&times;</span></button>" +
                "<strong>" + title + "</strong> " + HttpUtility.HtmlEncode(msg) +
                "</div>";
        }

        private bool EnsureUserLogin()
        {
            if (Session["ClsTypeIsLogin"] == null)
            {
                Response.Redirect("login.aspx");
                return false;
            }

            ClsType clType = new ClsType();
            if (!clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
            {
                Response.Redirect("login.aspx");
                return false;
            }

            return true;
        }
    }
}
