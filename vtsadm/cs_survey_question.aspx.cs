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

        private const string ViewStateAllowListLoad = "AllowListLoad";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EnsureUserLogin())
            {
                return;
            }

            if (!IsPostBack)
            {
                BindDropdowns();
                ClearForm();
                HideList();
            }
            else
            {
                // ListItem Attributes (data-min/max) are not restored from ViewState.
                string selectedScale = ddlRatingScale.SelectedValue;
                BindRatingScaleDropdown(ExecuteDataTable(SpRatingScaleView));
                SetDropDownValue(ddlRatingScale, selectedScale);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPeriod.SelectedValue))
            {
                ShowMessage("Pilih Period dulu, lalu Load.", false);
                HideList();
                return;
            }

            ViewState[ViewStateAllowListLoad] = true;
            ClearForm(keepPeriod: true);
            BindGrid();
        }

        private bool IsListLoadAllowed()
        {
            return ViewState[ViewStateAllowListLoad] is bool && (bool)ViewState[ViewStateAllowListLoad];
        }

        private void HideList()
        {
            ViewState[ViewStateAllowListLoad] = false;
            pnlList.Visible = false;
            pnlListEmpty.Visible = true;
            gvData.DataSource = null;
            gvData.DataBind();
            lblLoadedPeriod.Text = string.Empty;
        }

        private void BindGrid()
        {
            try
            {
                if (!IsListLoadAllowed() || string.IsNullOrEmpty(ddlPeriod.SelectedValue))
                {
                    HideList();
                    return;
                }

                string periodId = ddlPeriod.SelectedValue;
                DataTable table = ExecuteDataTable(SpSurveyQuestionView);
                EnsureDisplayColumns(table);
                MergePeriodFromGpsb(table);
                EnrichPeriodNames(table);

                DataView view = table.DefaultView;
                view.RowFilter = string.Format("survey_period_id = {0}", periodId);
                if (table.Columns.Contains("sort_order"))
                {
                    view.Sort = "sort_order ASC";
                }

                gvData.DataSource = view;
                gvData.DataBind();

                lblLoadedPeriod.Text = ddlPeriod.SelectedItem != null
                    ? ddlPeriod.SelectedItem.Text
                    : periodId;
                pnlList.Visible = true;
                pnlListEmpty.Visible = false;
            }
            catch (Exception ex)
            {
                ShowMessage("Load data failed: " + ex.Message, false);
                HideList();
            }
        }

        private void BindDropdowns()
        {
            BindDropdown(
                ddlPeriod,
                ExecuteDataTable(SpSurveyPeriodView),
                "survey_period_id",
                "period_name",
                "[Select Period]");

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

            BindRatingScaleDropdown(ExecuteDataTable(SpRatingScaleView));
        }

        private void BindRatingScaleDropdown(DataTable source)
        {
            ddlRatingScale.Items.Clear();
            ddlRatingScale.Items.Add(new ListItem("[No Rating Scale]", string.Empty));

            var metaJson = new System.Text.StringBuilder();
            metaJson.Append("{");
            bool firstMeta = true;

            if (source != null)
            {
                bool hasMin = source.Columns.Contains("min_value");
                bool hasMax = source.Columns.Contains("max_value");

                foreach (DataRow row in source.Rows)
                {
                    string id = Convert.ToString(row["rating_scale_id"]);
                    string name = Convert.ToString(row["scale_name"]);
                    int minVal = 1;
                    int maxVal = 5;
                    bool hasRange = hasMin && hasMax
                        && row["min_value"] != DBNull.Value
                        && row["max_value"] != DBNull.Value;

                    if (hasRange)
                    {
                        minVal = Convert.ToInt32(row["min_value"]);
                        maxVal = Convert.ToInt32(row["max_value"]);
                        if (minVal > maxVal)
                        {
                            int tmp = minVal;
                            minVal = maxVal;
                            maxVal = tmp;
                        }
                        name = string.Format("{0} ({1}-{2})", name, minVal, maxVal);
                    }

                    var item = new ListItem(name, id);
                    if (hasRange)
                    {
                        item.Attributes["data-min"] = minVal.ToString();
                        item.Attributes["data-max"] = maxVal.ToString();
                    }
                    ddlRatingScale.Items.Add(item);

                    if (hasRange)
                    {
                        if (!firstMeta) metaJson.Append(",");
                        metaJson.AppendFormat(
                            "\"{0}\":{{\"min\":{1},\"max\":{2}}}",
                            id.Replace("\\", "\\\\").Replace("\"", "\\\""),
                            minVal,
                            maxVal);
                        firstMeta = false;
                    }
                }
            }

            metaJson.Append("}");
            ClientScript.RegisterStartupScript(
                GetType(),
                "csRatingScaleMeta",
                "window.__csRatingScaleMeta = " + metaJson + ";",
                true);
        }

        private static string BuildDefaultNegativeForRating(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                int tmp = minValue;
                minValue = maxValue;
                maxValue = tmp;
            }

            // Small scales (e.g. 1-3): only the lowest by default; admin can add more.
            if ((maxValue - minValue) <= 2)
            {
                return minValue.ToString();
            }

            int mid = (minValue + maxValue) / 2;
            var parts = new List<string>();
            for (int v = minValue; v <= mid; v++)
            {
                parts.Add(v.ToString());
            }
            return string.Join(",", parts);
        }

        private void NormalizeNegativeOptionBeforeSave(string typeCode, int? ratingId)
        {
            if (string.Equals(typeCode, "TEXT", StringComparison.OrdinalIgnoreCase)
                || string.Equals(typeCode, "TEXT2", StringComparison.OrdinalIgnoreCase))
            {
                txtNegativeValue.Text = string.Empty;
                chkReason.Checked = false;
                return;
            }

            if (string.Equals(typeCode, "YES_NO", StringComparison.OrdinalIgnoreCase))
            {
                // Keep whatever the picker posted (NO or empty).
                string yn = (txtNegativeValue.Text ?? string.Empty).Trim();
                if (string.Equals(yn, "NO", StringComparison.OrdinalIgnoreCase))
                {
                    txtNegativeValue.Text = "NO";
                }
                else if (string.IsNullOrWhiteSpace(yn))
                {
                    txtNegativeValue.Text = string.Empty;
                }
                else
                {
                    txtNegativeValue.Text = "NO";
                }
            }
            else if (string.Equals(typeCode, "RATING", StringComparison.OrdinalIgnoreCase))
            {
                if (!ratingId.HasValue)
                {
                    txtNegativeValue.Text = string.Empty;
                }
                else
                {
                    int min;
                    int max;
                    if (!TryResolveRatingScaleRange(ratingId.Value, out min, out max))
                    {
                        txtNegativeValue.Text = string.Empty;
                    }
                    else
                    {
                        string raw = (txtNegativeValue.Text ?? string.Empty).Trim();
                        if (string.IsNullOrEmpty(raw))
                        {
                            txtNegativeValue.Text = BuildDefaultNegativeForRating(min, max);
                        }
                        else
                        {
                            txtNegativeValue.Text = ClampNegativeValuesToRange(raw, min, max);
                        }
                    }
                }
            }

            // One rule: having negative answers means reason is required.
            chkReason.Checked = !string.IsNullOrWhiteSpace(txtNegativeValue.Text);
        }

        private bool TryResolveRatingScaleRange(int ratingId, out int min, out int max)
        {
            min = 1;
            max = 5;

            var item = ddlRatingScale.Items.FindByValue(ratingId.ToString());
            if (item != null
                && int.TryParse(item.Attributes["data-min"], out min)
                && int.TryParse(item.Attributes["data-max"], out max))
            {
                if (min > max)
                {
                    int tmp = min;
                    min = max;
                    max = tmp;
                }
                return true;
            }

            DataTable scales = ExecuteDataTable(SpRatingScaleView);
            if (scales == null || !scales.Columns.Contains("min_value") || !scales.Columns.Contains("max_value"))
            {
                return false;
            }

            DataRow scaleRow = scales.Select("rating_scale_id = " + ratingId).FirstOrDefault();
            if (scaleRow == null || scaleRow["min_value"] == DBNull.Value || scaleRow["max_value"] == DBNull.Value)
            {
                return false;
            }

            min = Convert.ToInt32(scaleRow["min_value"]);
            max = Convert.ToInt32(scaleRow["max_value"]);
            if (min > max)
            {
                int tmp = min;
                min = max;
                max = tmp;
            }
            return true;
        }

        private static string ClampNegativeValuesToRange(string raw, int min, int max)
        {
            var parts = (raw ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x =>
                {
                    int n;
                    return int.TryParse(x, out n) && n >= min && n <= max;
                })
                .Distinct()
                .ToList();
            return string.Join(",", parts);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlPeriod.SelectedValue))
                {
                    ShowMessage("Period wajib", false);
                    return;
                }

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

                int? id = string.IsNullOrEmpty(hfId.Value) ? (int?)null : Convert.ToInt32(hfId.Value);
                int? ratingId = string.IsNullOrEmpty(ddlRatingScale.SelectedValue) ? (int?)null : Convert.ToInt32(ddlRatingScale.SelectedValue);
                int? periodId = string.IsNullOrEmpty(ddlPeriod.SelectedValue) ? (int?)null : Convert.ToInt32(ddlPeriod.SelectedValue);

                string typeCode = (ddlQuestionType.SelectedValue ?? string.Empty).Trim();
                bool isTextType = string.Equals(typeCode, "TEXT", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(typeCode, "TEXT2", StringComparison.OrdinalIgnoreCase);
                bool isRatingType = string.Equals(typeCode, "RATING", StringComparison.OrdinalIgnoreCase);

                if (isTextType)
                {
                    ratingId = null;
                    if (ddlRatingScale.Items.Count > 0)
                    {
                        ddlRatingScale.SelectedIndex = 0;
                    }
                }
                else if (!isRatingType)
                {
                    ratingId = null;
                    if (ddlRatingScale.Items.Count > 0)
                    {
                        ddlRatingScale.SelectedIndex = 0;
                    }
                }

                NormalizeNegativeOptionBeforeSave(typeCode, ratingId);
                SaveSurveyQuestion(id, periodId, ratingId, sort);

                ShowMessage("Save berhasil", true);
                ViewState[ViewStateAllowListLoad] = true;
                ClearForm(keepPeriod: true);
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

                    string periodId = string.Empty;
                    if (row.Table.Columns.Contains("survey_period_id") && row["survey_period_id"] != DBNull.Value)
                    {
                        periodId = row["survey_period_id"].ToString();
                    }
                    else
                    {
                        periodId = LoadQuestionPeriodId(id);
                    }

                    SetDropDownValue(ddlPeriod, periodId);
                    SetDropDownValue(ddlCategory, row["survey_category_id"].ToString());
                    SetDropDownValue(ddlQuestionType, row["question_type_code"].ToString());

                    SetDropDownValue(
                        ddlRatingScale,
                        row["rating_scale_id"] == DBNull.Value ? string.Empty : row["rating_scale_id"].ToString());

                    txtQuestion.Text = row["question_text"].ToString();
                    txtSort.Text = row["sort_order"].ToString();

                    chkReason.Checked = Convert.ToBoolean(row["is_reason_required_if_negative"]);
                    txtNegativeValue.Text = row["negative_option_value"].ToString();
                    // Old rows could have negatives with reason off — UI is one control now.
                    if (!chkReason.Checked)
                    {
                        txtNegativeValue.Text = string.Empty;
                    }

                    chkActive.Checked = Convert.ToBoolean(row["is_active"]);
                    btnSave.Text = "Update";
                    ViewState[ViewStateAllowListLoad] = true;
                    BindGrid();
                    ClientScript.RegisterStartupScript(GetType(), "syncQuestionTypeUi", "if (typeof syncQuestionTypeUi === 'function') { syncQuestionTypeUi(true); }", true);
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

                ShowMessage("Delete berhasil", true);
                BindGrid();
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
        }

        private void ClearForm(bool keepPeriod = false)
        {
            string keepPeriodId = keepPeriod ? ddlPeriod.SelectedValue : string.Empty;

            hfId.Value = "";
            txtQuestion.Text = "";
            txtSort.Text = "";
            txtNegativeValue.Text = "";

            chkReason.Checked = false;
            chkActive.Checked = true;

            if (!keepPeriod)
            {
                if (ddlPeriod.Items.Count > 0) ddlPeriod.SelectedIndex = 0;
            }
            else
            {
                SetDropDownValue(ddlPeriod, keepPeriodId);
            }

            if (ddlCategory.Items.Count > 0) ddlCategory.SelectedIndex = 0;
            if (ddlQuestionType.Items.Count > 0) ddlQuestionType.SelectedIndex = 0;
            if (ddlRatingScale.Items.Count > 0) ddlRatingScale.SelectedIndex = 0;
            btnSave.Text = "Save";
        }

        private void SaveSurveyQuestion(int? id, int? periodId, int? ratingId, int sort)
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

        private string LoadQuestionPeriodId(string surveyQuestionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
                using (SqlCommand cmd = new SqlCommand(@"
SELECT survey_period_id
FROM cs_mst_survey_question WITH (NOLOCK)
WHERE survey_question_id = @survey_question_id", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@survey_question_id", SqlDbType.Int) { Value = Convert.ToInt32(surveyQuestionId) });
                    conn.Open();
                    object raw = cmd.ExecuteScalar();
                    if (raw == null || raw == DBNull.Value)
                    {
                        return string.Empty;
                    }

                    return Convert.ToString(raw);
                }
            }
            catch
            {
                return string.Empty;
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

            if (!table.Columns.Contains("survey_period_id"))
            {
                table.Columns.Add("survey_period_id", typeof(int));
            }
        }

        private void MergePeriodFromGpsb(DataTable table)
        {
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

                        if (gpsbRow["survey_period_id"] != DBNull.Value)
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

        private void EnrichPeriodNames(DataTable table)
        {
            if (table == null || !table.Columns.Contains("period_name") || !table.Columns.Contains("survey_period_id"))
            {
                return;
            }

            Dictionary<string, string> periodNames = LoadPeriodNamesMap();
            foreach (DataRow row in table.Rows)
            {
                if (row["survey_period_id"] == DBNull.Value)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(row["period_name"])))
                    {
                        row["period_name"] = "";
                    }

                    continue;
                }

                string periodId = Convert.ToString(row["survey_period_id"]);
                string name;
                if (periodNames.TryGetValue(periodId, out name))
                {
                    row["period_name"] = name;
                }
                else if (string.IsNullOrWhiteSpace(Convert.ToString(row["period_name"])))
                {
                    row["period_name"] = "";
                }
            }
        }

        private Dictionary<string, string> LoadPeriodNamesMap()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                DataTable periods = ExecuteDataTable(SpSurveyPeriodView);
                if (periods == null)
                {
                    return map;
                }

                foreach (DataRow row in periods.Rows)
                {
                    if (!row.Table.Columns.Contains("survey_period_id") || !row.Table.Columns.Contains("period_name"))
                    {
                        continue;
                    }

                    string id = Convert.ToString(row["survey_period_id"]);
                    if (!string.IsNullOrWhiteSpace(id) && !map.ContainsKey(id))
                    {
                        map[id] = Convert.ToString(row["period_name"]);
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
            HideList();
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
