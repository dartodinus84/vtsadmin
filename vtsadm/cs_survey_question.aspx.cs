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
    public partial class cs_survey_question : Page
    {
        private const string SpSurveyQuestionView = "sp_cs_survey_question_view";
        private const string SpSurveyQuestionSave = "sp_cs_survey_question_save";
        private const string SpSurveyQuestionDelete = "sp_cs_survey_question_delete";
        private const string SpSurveyCategoryView = "sp_cs_survey_category_view";
        private const string SpQuestionTypeView = "sp_cs_question_type_view";
        private const string SpRatingScaleView = "sp_cs_rating_scale_view";
        private const string SpSurveyPeriodView = "sp_cs_survey_period_view";
        private const string SpListCompany = "sp_list_customer_companyid";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EnsureUserLogin())
            {
                return;
            }

            if (!IsPostBack)
            {
                BindDropdowns();
                ToggleCompanyVisibility();
                ClearForm();
                BindGrid();
            }
        }

        protected void rblTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleCompanyVisibility();
            RegisterCompanyUiRefresh();
        }

        private void BindGrid()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpSurveyQuestionView);
                EnsureDisplayColumns(table);
                MergeCompanyIdsFromGpsb(table);
                EnrichTargetLabels(table);
                gvData.DataSource = table;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Load data failed: " + ex.Message, false);
            }
        }

        private void BindDropdowns()
        {
            BindDropdown(
                ddlPeriod,
                ExecuteDataTable(SpSurveyPeriodView),
                "survey_period_id",
                "period_name",
                "[No Period]");

            BindDropdown(
                ddlCategory,
                ExecuteDataTable(SpSurveyCategoryView),
                "survey_category_id",
                "category_name",
                "[Select Category]");

            BindDropdown(
                ddlQuestionType,
                ExecuteDataTable(SpQuestionTypeView),
                "question_type_code",
                "question_type_name",
                "[Select Question Type]");

            BindDropdown(
                ddlRatingScale,
                ExecuteDataTable(SpRatingScaleView),
                "rating_scale_id",
                "scale_name",
                "[No Rating Scale]");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlCategory.SelectedValue))
                {
                    ShowMessage("Category wajib", false);
                    return;
                }

                if (string.IsNullOrEmpty(ddlQuestionType.SelectedValue))
                {
                    ShowMessage("Question type wajib", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtQuestion.Text))
                {
                    ShowMessage("Question wajib", false);
                    return;
                }

                int sort;
                if (!int.TryParse(txtSort.Text, out sort))
                {
                    ShowMessage("Sort harus angka", false);
                    return;
                }

                bool isGlobal = string.Equals(rblTarget.SelectedValue, "GLOBAL", StringComparison.OrdinalIgnoreCase);
                List<int> companyIds = GetSelectedCompanyIds();
                if (!isGlobal && companyIds.Count == 0)
                {
                    ShowMessage("Pilih minimal 1 company.", false);
                    return;
                }

                int? id = string.IsNullOrEmpty(hfId.Value) ? (int?)null : Convert.ToInt32(hfId.Value);
                int? ratingId = string.IsNullOrEmpty(ddlRatingScale.SelectedValue) ? (int?)null : Convert.ToInt32(ddlRatingScale.SelectedValue);
                int? periodId = string.IsNullOrEmpty(ddlPeriod.SelectedValue) ? (int?)null : Convert.ToInt32(ddlPeriod.SelectedValue);

                SaveSurveyQuestion(id, periodId, isGlobal, companyIds, ratingId, sort);

                ShowMessage("Save berhasil", true);
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
            if (e.CommandName == "EDIT_DATA")
            {
                LoadData(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "DELETE_DATA")
            {
                DeleteData(e.CommandArgument.ToString());
            }
        }

        private void LoadData(string id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand(SpSurveyQuestionView, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow row = dt.Select("survey_question_id = " + id).FirstOrDefault();

                if (row != null)
                {
                    hfId.Value = id;

                    SetDropDownValue(
                        ddlPeriod,
                        row.Table.Columns.Contains("survey_period_id") && row["survey_period_id"] != DBNull.Value
                            ? row["survey_period_id"].ToString()
                            : string.Empty);

                    ApplyTargetFromLinkTable(id);
                    ToggleCompanyVisibility();

                    SetDropDownValue(ddlCategory, row["survey_category_id"].ToString());
                    SetDropDownValue(ddlQuestionType, row["question_type_code"].ToString());

                    SetDropDownValue(
                        ddlRatingScale,
                        row["rating_scale_id"] == DBNull.Value ? string.Empty : row["rating_scale_id"].ToString());

                    txtQuestion.Text = row["question_text"].ToString();
                    txtSort.Text = row["sort_order"].ToString();

                    chkReason.Checked = Convert.ToBoolean(row["is_reason_required_if_negative"]);
                    txtNegativeValue.Text = row["negative_option_value"].ToString();

                    chkActive.Checked = Convert.ToBoolean(row["is_active"]);
                    btnSave.Text = "Update";
                }
            }
        }

        private void DeleteData(string id)
        {
            try
            {
                ExecuteNonQuery(
                    SpSurveyQuestionDelete,
                    new SqlParameter("@survey_question_id", id));

                DeleteQuestionCompanyLinks(Convert.ToInt32(id));

                ShowMessage("Delete berhasil", true);
                BindGrid();
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
        }

        private void ClearForm()
        {
            hfId.Value = "";
            txtQuestion.Text = "";
            txtSort.Text = "";
            txtNegativeValue.Text = "";

            chkReason.Checked = false;
            chkActive.Checked = true;

            if (ddlPeriod.Items.Count > 0) ddlPeriod.SelectedIndex = 0;
            rblTarget.SelectedValue = "GLOBAL";
            hfSelectedCompanies.Value = "";
            ToggleCompanyVisibility();
            RegisterCompanyUiRefresh();
            if (ddlCategory.Items.Count > 0) ddlCategory.SelectedIndex = 0;
            if (ddlQuestionType.Items.Count > 0) ddlQuestionType.SelectedIndex = 0;
            if (ddlRatingScale.Items.Count > 0) ddlRatingScale.SelectedIndex = 0;
            btnSave.Text = "Save";
        }

        private void ToggleCompanyVisibility()
        {
            bool isCompany = string.Equals(rblTarget.SelectedValue, "COMPANY", StringComparison.OrdinalIgnoreCase);
            divCompany.Visible = isCompany;
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

        private void ApplyTargetFromLinkTable(string surveyQuestionId)
        {
            List<int?> links = LoadQuestionCompanyLinks(surveyQuestionId);
            bool hasGlobal = links.Any(x => !x.HasValue);
            var companyIds = links.Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();

            if (companyIds.Count > 0 && !hasGlobal)
            {
                rblTarget.SelectedValue = "COMPANY";
                Dictionary<int, string> names = LoadCompanyNames(companyIds);
                hfSelectedCompanies.Value = string.Join("|", companyIds.Select(id =>
                {
                    string name;
                    return id + ":" + (names.TryGetValue(id, out name) ? name : ("Company " + id));
                }));
            }
            else
            {
                rblTarget.SelectedValue = "GLOBAL";
                hfSelectedCompanies.Value = "";
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
            string script = "if (typeof renderSelectedCompanies === 'function') { renderSelectedCompanies(); }";
            ClientScript.RegisterStartupScript(GetType(), "refreshSurveyCompanies", script, true);
        }

        private void SaveSurveyQuestion(int? id, int? periodId, bool isGlobal, List<int> companyIds, int? ratingId, int sort)
        {
            var attempts = new List<SqlParameter[]>();
            attempts.Add(CreateQuestionSaveParameters(id, periodId, ratingId, sort, includePeriod: true));
            attempts.Add(CreateQuestionSaveParameters(id, periodId, ratingId, sort, includePeriod: false));

            Exception lastError = null;
            foreach (SqlParameter[] parameters in attempts)
            {
                try
                {
                    ExecuteNonQuery(SpSurveyQuestionSave, parameters);
                    int questionId = id ?? ResolveLatestQuestionId();
                    if (questionId > 0)
                    {
                        UpdateQuestionPeriod(questionId, periodId);
                        SaveQuestionCompanyLinks(questionId, isGlobal, companyIds);
                    }

                    return;
                }
                catch (SqlException ex)
                {
                    lastError = ex;
                }
            }

            if (lastError != null)
            {
                throw lastError;
            }
        }

        private SqlParameter[] CreateQuestionSaveParameters(int? id, int? periodId, int? ratingId, int sort, bool includePeriod)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = (object)id ?? DBNull.Value },
                new SqlParameter("@survey_category_id", ddlCategory.SelectedValue),
                new SqlParameter("@question_type_code", ddlQuestionType.SelectedValue),
                new SqlParameter("@rating_scale_id", (object)ratingId ?? DBNull.Value),
                new SqlParameter("@question_text", txtQuestion.Text.Trim()),
                new SqlParameter("@sort_order", sort),
                new SqlParameter("@is_reason_required_if_negative", chkReason.Checked),
                new SqlParameter("@negative_option_value", txtNegativeValue.Text.Trim()),
                new SqlParameter("@is_active", chkActive.Checked),
                new SqlParameter("@user", "SYSTEM")
            };

            if (includePeriod)
            {
                parameters.Insert(1, new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = (object)periodId ?? DBNull.Value });
            }

            return parameters.ToArray();
        }

        private int ResolveLatestQuestionId()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1 survey_question_id
FROM cs_mst_survey_question WITH (NOLOCK)
WHERE survey_category_id = @survey_category_id
  AND question_text = @question_text
ORDER BY survey_question_id DESC", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_category_id", SqlDbType.Int) { Value = Convert.ToInt32(ddlCategory.SelectedValue) });
                    cmd.Parameters.Add(new SqlParameter("@question_text", SqlDbType.NVarChar, 500) { Value = txtQuestion.Text.Trim() });
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

        private void UpdateQuestionPeriod(int surveyQuestionId, int? periodId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE cs_mst_survey_question
SET survey_period_id = @survey_period_id,
    updated_at = GETDATE()
WHERE survey_question_id = @survey_question_id", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = (object)periodId ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = surveyQuestionId });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        private List<int?> LoadQuestionCompanyLinks(string surveyQuestionId)
        {
            var result = new List<int?>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT company_id
FROM cs_mst_survey_question_company WITH (NOLOCK)
WHERE survey_question_id = @survey_question_id
  AND ISNULL(is_active, 1) = 1", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = Convert.ToInt32(surveyQuestionId) });
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

        private void SaveQuestionCompanyLinks(int surveyQuestionId, bool isGlobal, List<int> companyIds)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            {
                conn.Open();
                using (SqlTransaction trx = conn.BeginTransaction())
                {
                    using (SqlCommand del = new SqlCommand(
                        "DELETE FROM cs_mst_survey_question_company WHERE survey_question_id = @survey_question_id",
                        conn,
                        trx))
                    {
                        del.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = surveyQuestionId });
                        del.ExecuteNonQuery();
                    }

                    if (isGlobal)
                    {
                        using (SqlCommand ins = new SqlCommand(@"
INSERT INTO cs_mst_survey_question_company (survey_question_id, company_id, is_active, created_at)
VALUES (@survey_question_id, NULL, 1, GETDATE())", conn, trx))
                        {
                            ins.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = surveyQuestionId });
                            ins.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        foreach (int companyId in companyIds.Distinct())
                        {
                            using (SqlCommand ins = new SqlCommand(@"
INSERT INTO cs_mst_survey_question_company (survey_question_id, company_id, is_active, created_at)
VALUES (@survey_question_id, @company_id, 1, GETDATE())", conn, trx))
                            {
                                ins.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = surveyQuestionId });
                                ins.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId });
                                ins.ExecuteNonQuery();
                            }
                        }
                    }

                    trx.Commit();
                }
            }
        }

        private void DeleteQuestionCompanyLinks(int surveyQuestionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM cs_mst_survey_question_company WHERE survey_question_id = @survey_question_id",
                    conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = surveyQuestionId });
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

            if (!table.Columns.Contains("period_name"))
            {
                table.Columns.Add("period_name", typeof(string));
            }

            if (!table.Columns.Contains("target_label"))
            {
                table.Columns.Add("target_label", typeof(string));
            }
        }

        private void MergeCompanyIdsFromGpsb(DataTable table)
        {
            // Replaced by EnrichTargetLabelsFromLinkTable — keep period merge only.
            if (table == null || table.Rows.Count == 0 || !table.Columns.Contains("survey_question_id"))
            {
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT survey_question_id, survey_period_id
FROM cs_mst_survey_question WITH (NOLOCK)", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var gpsb = new DataTable();
                    da.Fill(gpsb);
                    var map = gpsb.AsEnumerable().ToDictionary(
                        r => Convert.ToString(r["survey_question_id"]),
                        r => r,
                        StringComparer.OrdinalIgnoreCase);

                    foreach (DataRow row in table.Rows)
                    {
                        string id = Convert.ToString(row["survey_question_id"]);
                        DataRow gpsbRow;
                        if (!map.TryGetValue(id, out gpsbRow))
                        {
                            continue;
                        }

                        if (table.Columns.Contains("survey_period_id") && gpsbRow["survey_period_id"] != DBNull.Value)
                        {
                            row["survey_period_id"] = gpsbRow["survey_period_id"];
                        }
                    }
                }
            }
            catch
            {
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
                string id = Convert.ToString(row["survey_question_id"]);
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
SELECT survey_question_id, company_id
FROM cs_mst_survey_question_company WITH (NOLOCK)
WHERE ISNULL(is_active, 1) = 1", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    foreach (var group in dt.AsEnumerable().GroupBy(r => Convert.ToString(r["survey_question_id"])))
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private static void SetDropDownValue(DropDownList ddl, string value)
        {
            if (ddl == null)
            {
                return;
            }

            ListItem item = ddl.Items.FindByValue(value ?? string.Empty);
            if (item == null)
            {
                ddl.SelectedIndex = 0;
                return;
            }

            ddl.ClearSelection();
            item.Selected = true;
        }

        private static void BindDropdown(DropDownList ddl, DataTable source, string valueField, string textField, string defaultText)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(defaultText, string.Empty));

            ddl.DataSource = source;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();
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
