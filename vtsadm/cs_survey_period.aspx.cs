using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class cs_survey_period : Page
    {
        private const string SpSurveyPeriodView = "sp_cs_survey_period_view";
        private const string SpSurveyPeriodSave = "sp_cs_survey_period_save";
        private const string SpSurveyPeriodDelete = "sp_cs_survey_period_delete";
        private const string SpListCompany = "sp_list_customer_companyid";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EnsureUserLogin())
            {
                return;
            }

            if (!IsPostBack)
            {
                ClearForm();
                BindGrid();
            }
        }

        protected void BindGrid()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpSurveyPeriodView) ?? new DataTable();
                EnsureDisplayColumns(table);
                try
                {
                    EnrichTargetLabels(table);
                }
                catch
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (table.Columns.Contains("target_label") &&
                            (row["target_label"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(row["target_label"]))))
                        {
                            row["target_label"] = "Global";
                        }
                    }
                }

                try
                {
                    EnrichDeployLabels(table);
                }
                catch
                {
                    // Keep draft defaults from EnsureDisplayColumns; do not block the list.
                }

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
                string periodCode = txtPeriodCode.Text.Trim();
                string periodName = txtPeriodName.Text.Trim();

                if (string.IsNullOrWhiteSpace(periodCode))
                {
                    ShowMessage("Period code wajib diisi.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(periodName))
                {
                    ShowMessage("Period name wajib diisi.", false);
                    return;
                }

                DateTime startDateValue;
                DateTime endDateValue;
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    if (!DateTime.TryParse(txtStartDate.Text, out startDateValue))
                    {
                        ShowMessage("Start date tidak valid.", false);
                        return;
                    }

                    startDate = startDateValue.Date;
                }

                if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    if (!DateTime.TryParse(txtEndDate.Text, out endDateValue))
                    {
                        ShowMessage("End date tidak valid.", false);
                        return;
                    }

                    endDate = endDateValue.Date;
                }

                if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
                {
                    ShowMessage("Start date tidak boleh lebih besar dari end date.", false);
                    return;
                }

                string audience = (rblAudience.SelectedValue ?? "GLOBAL").Trim().ToUpperInvariant();
                bool isGlobal = audience != "COMPANY";
                List<int> companyIds = GetSelectedCompanyIds();
                if (!isGlobal && companyIds.Count == 0)
                {
                    ShowMessage("Pilih minimal 1 company untuk Specific companies only.", false);
                    return;
                }

                if (isGlobal)
                {
                    companyIds = new List<int>();
                }

                int periodIdParsed;
                int? surveyPeriodId = int.TryParse(hfId.Value, out periodIdParsed) ? (int?)periodIdParsed : null;

                ExecuteNonQuery(
                    SpSurveyPeriodSave,
                    new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = (object)surveyPeriodId ?? DBNull.Value },
                    new SqlParameter("@period_code", SqlDbType.VarChar, 20) { Value = periodCode },
                    new SqlParameter("@period_name", SqlDbType.VarChar, 50) { Value = periodName },
                    new SqlParameter("@start_date", SqlDbType.Date) { Value = (object)startDate ?? DBNull.Value },
                    new SqlParameter("@end_date", SqlDbType.Date) { Value = (object)endDate ?? DBNull.Value },
                    new SqlParameter("@is_active", SqlDbType.Bit) { Value = chkActive.Checked });

                int periodId = surveyPeriodId ?? ResolvePeriodId(periodCode);
                if (periodId > 0)
                {
                    SavePeriodCompanyLinks(periodId, isGlobal, companyIds);
                }

                ShowMessage(surveyPeriodId.HasValue
                    ? "Update berhasil. Gunakan Deploy di list agar periode tampil di TMS."
                    : "Save berhasil. Gunakan Deploy di list agar periode tampil di TMS.", true);
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

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(e.CommandArgument)))
            {
                return;
            }

            string id = e.CommandArgument.ToString().Trim();
            try
            {
                if (string.Equals(e.CommandName, "EDIT_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    LoadData(id);
                    return;
                }

                if (string.Equals(e.CommandName, "DELETE_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteData(id);
                    BindGrid();
                    return;
                }

                if (string.Equals(e.CommandName, "DEPLOY_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    SetDeployed(id, true);
                    return;
                }

                if (string.Equals(e.CommandName, "UNDEPLOY_DATA", StringComparison.OrdinalIgnoreCase))
                {
                    SetDeployed(id, false);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Process failed: " + ex.Message, false);
            }
        }

        private void LoadData(string id)
        {
            int surveyPeriodId;
            if (!int.TryParse(id, out surveyPeriodId))
            {
                ShowMessage("survey_period_id tidak valid.", false);
                return;
            }

            DataTable table = ExecuteDataTable(SpSurveyPeriodView);
            DataRow row = table.AsEnumerable()
                .FirstOrDefault(x => Convert.ToInt32(x["survey_period_id"]) == surveyPeriodId);

            if (row == null)
            {
                ShowMessage("Data survey period tidak ditemukan.", false);
                return;
            }

            hfId.Value = row["survey_period_id"].ToString();
            txtPeriodCode.Text = Convert.ToString(row["period_code"]);
            txtPeriodName.Text = Convert.ToString(row["period_name"]);
            txtStartDate.Text = row["start_date"] == DBNull.Value
                ? string.Empty
                : Convert.ToDateTime(row["start_date"]).ToString("yyyy-MM-dd");
            txtEndDate.Text = row["end_date"] == DBNull.Value
                ? string.Empty
                : Convert.ToDateTime(row["end_date"]).ToString("yyyy-MM-dd");
            chkActive.Checked = row["is_active"] != DBNull.Value && Convert.ToBoolean(row["is_active"]);

            ApplyTargetFromLinkTable(id);
            RegisterCompanyUiRefresh();

            btnSave.Text = "Update";
        }

        private void DeleteData(string id)
        {
            int surveyPeriodId;
            if (!int.TryParse(id, out surveyPeriodId))
            {
                ShowMessage("survey_period_id tidak valid.", false);
                return;
            }

            try
            {
                ExecuteNonQuery(
                    SpSurveyPeriodDelete,
                    new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });

                DeletePeriodCompanyLinks(surveyPeriodId);

                ShowMessage("Delete berhasil.", true);
                ClearForm();
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
        }

        private void SetDeployed(string id, bool deployed)
        {
            int surveyPeriodId;
            if (!int.TryParse(id, out surveyPeriodId))
            {
                ShowMessage("survey_period_id tidak valid.", false);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE cs_mst_survey_period
SET is_deployed = @is_deployed
WHERE survey_period_id = @survey_period_id", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@is_deployed", SqlDbType.Bit) { Value = deployed });
                    cmd.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });
                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();
                    if (affected < 1)
                    {
                        ShowMessage("Period tidak ditemukan.", false);
                        return;
                    }
                }

                ShowMessage(deployed
                    ? "Deploy berhasil. Periode ini aktif di TMS."
                    : "Undeploy berhasil. Periode ini disembunyikan dari TMS.", true);
                BindGrid();
            }
            catch (SqlException ex)
            {
                ShowMessage("Deploy gagal. Pastikan kolom is_deployed ada di cs_mst_survey_period. " + ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowMessage("Deploy gagal: " + ex.Message, false);
            }
        }

        private void ClearForm()
        {
            hfId.Value = string.Empty;
            txtPeriodCode.Text = string.Empty;
            txtPeriodName.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            chkActive.Checked = true;
            rblAudience.ClearSelection();
            var globalItem = rblAudience.Items.FindByValue("GLOBAL");
            if (globalItem != null)
            {
                globalItem.Selected = true;
            }
            hfSelectedCompanies.Value = string.Empty;
            RegisterCompanyUiRefresh();
            btnSave.Text = "Save";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private List<int> GetSelectedCompanyIds()
        {
            var ids = new List<int>();
            string raw = hfSelectedCompanies.Value ?? string.Empty;
            foreach (string part in raw.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int sep = part.IndexOf(':');
                string idText = sep >= 0 ? part.Substring(0, sep) : part;
                int companyId;
                if (int.TryParse(idText, out companyId))
                {
                    ids.Add(companyId);
                }
            }

            return ids;
        }

        private void ApplyTargetFromLinkTable(string surveyPeriodId)
        {
            List<int?> links = LoadPeriodCompanyLinks(surveyPeriodId);
            bool hasGlobal = links.Any(x => !x.HasValue);
            var companyIds = links.Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();

            rblAudience.ClearSelection();
            string audienceValue = (!hasGlobal && companyIds.Count > 0) ? "COMPANY" : "GLOBAL";
            var audienceItem = rblAudience.Items.FindByValue(audienceValue);
            if (audienceItem != null)
            {
                audienceItem.Selected = true;
            }

            if (companyIds.Count > 0)
            {
                Dictionary<int, string> names = LoadCompanyNames(companyIds);
                hfSelectedCompanies.Value = string.Join("|", companyIds.Select(id =>
                {
                    string name;
                    return id + ":" + (names.TryGetValue(id, out name) ? name : ("Company " + id));
                }));
            }
            else
            {
                hfSelectedCompanies.Value = string.Empty;
            }

            RegisterCompanyUiRefresh();
        }

        private Dictionary<int, string> LoadCompanyNames(List<int> companyIds)
        {
            var map = new Dictionary<int, string>();
            if (companyIds == null || companyIds.Count == 0)
            {
                return map;
            }

            try
            {
                var temp = new DropDownList();
                ClsType clType = new ClsType();
                clType.Open_Combos(temp, Session["ClsTypeDBConnStringSQL"].ToString(), "", SpListCompany);
                foreach (ListItem item in temp.Items)
                {
                    int id;
                    if (int.TryParse(item.Value, out id) && companyIds.Contains(id) && !map.ContainsKey(id))
                    {
                        map[id] = item.Text;
                    }
                }
            }
            catch
            {
            }

            return map;
        }

        private void RegisterCompanyUiRefresh()
        {
            string script = "if (typeof updateAudienceUi === 'function') { updateAudienceUi(); } else if (typeof renderSelectedCompanies === 'function') { renderSelectedCompanies(); }";
            ClientScript.RegisterStartupScript(GetType(), "refreshSurveyCompanies", script, true);
        }

        private int ResolvePeriodId(string periodCode)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1 survey_period_id
FROM cs_mst_survey_period WITH (NOLOCK)
WHERE period_code = @period_code
ORDER BY survey_period_id DESC", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@period_code", SqlDbType.VarChar, 20) { Value = periodCode });
                    conn.Open();
                    object raw = cmd.ExecuteScalar();
                    if (raw == null || raw == DBNull.Value)
                    {
                        return 0;
                    }

                    return Convert.ToInt32(raw);
                }
            }
            catch
            {
                return 0;
            }
        }

        private List<int?> LoadPeriodCompanyLinks(string surveyPeriodId)
        {
            var result = new List<int?>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT company_id
FROM cs_mst_survey_period_company WITH (NOLOCK)
WHERE survey_period_id = @survey_period_id
  AND ISNULL(is_active, 1) = 1", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = Convert.ToInt32(surveyPeriodId) });
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                            {
                                result.Add(null);
                            }
                            else
                            {
                                result.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private void SavePeriodCompanyLinks(int surveyPeriodId, bool isGlobal, List<int> companyIds)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            {
                conn.Open();
                using (SqlTransaction trx = conn.BeginTransaction())
                {
                    using (SqlCommand del = new SqlCommand(
                        "DELETE FROM cs_mst_survey_period_company WHERE survey_period_id = @survey_period_id",
                        conn,
                        trx))
                    {
                        del.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });
                        del.ExecuteNonQuery();
                    }

                    if (isGlobal)
                    {
                        using (SqlCommand ins = new SqlCommand(@"
INSERT INTO cs_mst_survey_period_company (survey_period_id, company_id, is_active, created_at)
VALUES (@survey_period_id, NULL, 1, GETDATE())", conn, trx))
                        {
                            ins.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });
                            ins.ExecuteNonQuery();
                        }
                    }

                    if (companyIds != null)
                    {
                        foreach (int companyId in companyIds.Distinct())
                        {
                            using (SqlCommand ins = new SqlCommand(@"
INSERT INTO cs_mst_survey_period_company (survey_period_id, company_id, is_active, created_at)
VALUES (@survey_period_id, @company_id, 1, GETDATE())", conn, trx))
                            {
                                ins.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });
                                ins.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId });
                                ins.ExecuteNonQuery();
                            }
                        }
                    }

                    trx.Commit();
                }
            }
        }

        private void DeletePeriodCompanyLinks(int surveyPeriodId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM cs_mst_survey_period_company WHERE survey_period_id = @survey_period_id",
                    conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = surveyPeriodId });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        private static void EnsureDisplayColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            if (!table.Columns.Contains("target_label"))
            {
                table.Columns.Add("target_label", typeof(string));
            }

            if (!table.Columns.Contains("deploy_label"))
            {
                table.Columns.Add("deploy_label", typeof(string));
            }

            if (!table.Columns.Contains("is_deployed_flag"))
            {
                table.Columns.Add("is_deployed_flag", typeof(bool));
            }

            foreach (DataRow row in table.Rows)
            {
                if (row["target_label"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(row["target_label"])))
                {
                    row["target_label"] = "Global";
                }

                if (row["is_deployed_flag"] == DBNull.Value)
                {
                    row["is_deployed_flag"] = false;
                }

                if (row["deploy_label"] == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(row["deploy_label"])))
                {
                    row["deploy_label"] = "Draft";
                }
            }
        }

        private void EnrichTargetLabels(DataTable table)
        {
            if (table == null || !table.Columns.Contains("target_label"))
            {
                return;
            }

            Dictionary<string, string> labels = LoadTargetLabelsMap();
            foreach (DataRow row in table.Rows)
            {
                string id = Convert.ToString(row["survey_period_id"]);
                string label;
                row["target_label"] = labels.TryGetValue(id, out label) ? label : "Global";
            }
        }

        private Dictionary<string, string> LoadTargetLabelsMap()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT survey_period_id, company_id
FROM cs_mst_survey_period_company WITH (NOLOCK)
WHERE ISNULL(is_active, 1) = 1", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    foreach (var group in dt.AsEnumerable().GroupBy(r => Convert.ToString(r["survey_period_id"])))
                    {
                        bool hasGlobal = group.Any(r => r["company_id"] == DBNull.Value);
                        var companies = group
                            .Where(r => r["company_id"] != DBNull.Value)
                            .Select(r => Convert.ToString(r["company_id"]))
                            .Distinct()
                            .ToList();

                        if (hasGlobal && companies.Count == 0)
                        {
                            map[group.Key] = "Global";
                        }
                        else if (!hasGlobal && companies.Count > 0)
                        {
                            map[group.Key] = "Company " + string.Join(", ", companies);
                        }
                        else if (hasGlobal && companies.Count > 0)
                        {
                            map[group.Key] = "Global + " + string.Join(", ", companies);
                        }
                    }
                }
            }
            catch
            {
            }

            return map;
        }

        private void EnrichDeployLabels(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            if (!table.Columns.Contains("is_deployed_flag"))
            {
                table.Columns.Add("is_deployed_flag", typeof(bool));
            }

            if (!table.Columns.Contains("deploy_label"))
            {
                table.Columns.Add("deploy_label", typeof(string));
            }

            // GPSB is_deployed is the source of truth (Deploy/Undeploy write there).
            Dictionary<string, bool> map = LoadDeployedMap();
            foreach (DataRow row in table.Rows)
            {
                string id = Convert.ToString(row["survey_period_id"]);
                bool deployed = false;
                if (!string.IsNullOrEmpty(id) && map.ContainsKey(id))
                {
                    deployed = map[id];
                }
                else if (table.Columns.Contains("is_deployed") && row["is_deployed"] != DBNull.Value)
                {
                    deployed = Convert.ToBoolean(row["is_deployed"]);
                }

                row["is_deployed_flag"] = deployed;
                row["deploy_label"] = deployed ? "Deployed" : "Draft";
            }
        }

        private Dictionary<string, bool> LoadDeployedMap()
        {
            var map = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT survey_period_id, ISNULL(is_deployed, 0) AS is_deployed
FROM cs_mst_survey_period WITH (NOLOCK)", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        map[Convert.ToString(row["survey_period_id"])] = Convert.ToBoolean(row["is_deployed"]);
                    }
                }
            }
            catch
            {
            }

            return map;
        }

        private DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                DataTable table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        private int ExecuteNonQuery(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
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

        private string GetGpsbConnectionString()
        {
            try
            {
                var gpsb = ConfigurationManager.ConnectionStrings["GPSBDBClient"];
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
                var gpsbOle = ConfigurationManager.ConnectionStrings["GPSBDB"];
                if (gpsbOle != null && !string.IsNullOrWhiteSpace(gpsbOle.ConnectionString))
                {
                    return ConvertOleDbToSqlConnectionString(gpsbOle.ConnectionString.Trim());
                }
            }
            catch
            {
            }

            return GetConnectionString();
        }

        private string GetConnectionString()
        {
            if (Session["ClsTypeDBConnStringSQL"] == null ||
                string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString()))
            {
                throw new InvalidOperationException("Session connection string tidak tersedia.");
            }

            string rawConnectionString = Session["ClsTypeDBConnStringSQL"].ToString().Trim();

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
            DbConnectionStringBuilder sourceBuilder = new DbConnectionStringBuilder { ConnectionString = rawConnectionString };
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string key in sourceBuilder.Keys)
            {
                map[key] = Convert.ToString(sourceBuilder[key]);
            }

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
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

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = HttpUtility.HtmlEncode(message);
            lblMessage.ForeColor = isSuccess ? Color.Green : Color.Red;
        }
    }
}
