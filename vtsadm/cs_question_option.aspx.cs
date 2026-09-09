using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class cs_question_option : Page
    {
        private const string SpQuestionOptionView = "sp_cs_question_option_view";
        private const string SpQuestionOptionSave = "sp_cs_question_option_save";
        private const string SpQuestionOptionDelete = "sp_cs_question_option_delete";
        private const string SpSurveyQuestionDropdown = "sp_cs_survey_question_dropdown";

        private string connStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EnsureUserLogin())
            {
                return;
            }

            connStr = GetConnectionString();

            if (!IsPostBack)
            {
                BindSurveyQuestionDropdown();

                if (Request.QueryString["survey_question_id"] == null)
                {
                    ShowMessage("survey_question_id required", false);
                    btnSave.Enabled = false;
                    return;
                }

                hfSurveyQuestionId.Value = Request.QueryString["survey_question_id"];
                SetSurveyQuestionSelection(hfSurveyQuestionId.Value);
                ClearForm();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            int surveyQuestionId;
            if (!int.TryParse(hfSurveyQuestionId.Value, out surveyQuestionId))
            {
                gvData.DataSource = new DataTable();
                gvData.DataBind();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(SpQuestionOptionView, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@survey_question_id", surveyQuestionId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvData.DataSource = dt;
                gvData.DataBind();
            }
        }

        private void BindSurveyQuestionDropdown()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpSurveyQuestionDropdown);

                string valueField = table.Columns.Contains("value") ? "value" :
                    (table.Columns.Contains("survey_question_id") ? "survey_question_id" : string.Empty);
                string textField = table.Columns.Contains("label") ? "label" :
                    (table.Columns.Contains("question_text") ? "question_text" : string.Empty);

                ddlSurveyQuestion.Items.Clear();
                if (string.IsNullOrWhiteSpace(valueField) || string.IsNullOrWhiteSpace(textField))
                {
                    ddlSurveyQuestion.Items.Add(new ListItem("[No Survey Question]", ""));
                    ShowMessage("Dropdown survey question gagal dibaca: kolom value/label tidak ditemukan.", false);
                    return;
                }

                ddlSurveyQuestion.DataSource = table;
                ddlSurveyQuestion.DataValueField = valueField;
                ddlSurveyQuestion.DataTextField = textField;
                ddlSurveyQuestion.DataBind();
                ddlSurveyQuestion.Items.Insert(0, new ListItem("[Select Survey Question]", ""));
            }
            catch (Exception ex)
            {
                ddlSurveyQuestion.Items.Clear();
                ddlSurveyQuestion.Items.Add(new ListItem("[No Survey Question]", ""));
                ShowMessage("Load survey question gagal: " + ex.Message, false);
            }
        }

        private void SetSurveyQuestionSelection(string surveyQuestionId)
        {
            if (string.IsNullOrWhiteSpace(surveyQuestionId))
            {
                ddlSurveyQuestion.SelectedIndex = 0;
                return;
            }

            ListItem item = ddlSurveyQuestion.Items.FindByValue(surveyQuestionId);
            if (item == null)
            {
                ddlSurveyQuestion.Items.Insert(1, new ListItem("Survey Question ID " + surveyQuestionId, surveyQuestionId));
                item = ddlSurveyQuestion.Items.FindByValue(surveyQuestionId);
            }

            ddlSurveyQuestion.ClearSelection();
            if (item != null)
            {
                item.Selected = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hfSurveyQuestionId.Value))
                {
                    ShowMessage("survey_question_id required", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtValue.Text) ||
                    string.IsNullOrWhiteSpace(txtLabel.Text))
                {
                    ShowMessage("Value & Label wajib", false);
                    return;
                }

                int sort;
                if (!int.TryParse(txtSort.Text, out sort))
                {
                    ShowMessage("Sort harus angka", false);
                    return;
                }

                int? id = string.IsNullOrEmpty(hfId.Value) ? (int?)null : Convert.ToInt32(hfId.Value);
                int surveyQuestionId = Convert.ToInt32(hfSurveyQuestionId.Value);

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(SpQuestionOptionSave, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@question_option_id", (object)id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@survey_question_id", surveyQuestionId);
                    cmd.Parameters.AddWithValue("@option_value", txtValue.Text.Trim());
                    cmd.Parameters.AddWithValue("@option_label", txtLabel.Text.Trim());
                    cmd.Parameters.AddWithValue("@sort_order", sort);
                    cmd.Parameters.AddWithValue("@is_active", chkActive.Checked);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    ShowMessage("Save berhasil", true);

                    ClearForm();
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

        protected void ddlSurveyQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfSurveyQuestionId.Value = ddlSurveyQuestion.SelectedValue;
            ClearForm();

            if (string.IsNullOrWhiteSpace(hfSurveyQuestionId.Value))
            {
                gvData.DataSource = new DataTable();
                gvData.DataBind();
                btnSave.Enabled = false;
                ShowMessage("survey_question_id required", false);
                return;
            }

            btnSave.Enabled = true;
            BindGrid();
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
            int optionId;
            if (!int.TryParse(id, out optionId))
            {
                ShowMessage("question_option_id tidak valid", false);
                return;
            }

            int surveyQuestionId;
            if (!int.TryParse(hfSurveyQuestionId.Value, out surveyQuestionId))
            {
                ShowMessage("survey_question_id required", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(SpQuestionOptionView, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@survey_question_id", surveyQuestionId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow row = dt.Select("question_option_id=" + optionId).FirstOrDefault();

                if (row != null)
                {
                    hfId.Value = optionId.ToString();
                    txtValue.Text = row["option_value"].ToString();
                    txtLabel.Text = row["option_label"].ToString();
                    txtSort.Text = row["sort_order"].ToString();
                    btnSave.Text = "Update";
                }
            }
        }

        private void DeleteData(string id)
        {
            try
            {
                int optionId;
                if (!int.TryParse(id, out optionId))
                {
                    ShowMessage("question_option_id tidak valid", false);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(SpQuestionOptionDelete, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@question_option_id", optionId);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    ShowMessage("Delete berhasil", true);
                    ClearForm();
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

        private void ClearForm()
        {
            hfId.Value = "";
            txtValue.Text = "";
            txtLabel.Text = "";
            txtSort.Text = "";
            chkActive.Checked = true;
            btnSave.Text = "Save";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ShowMessage(string msg, bool success)
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = success ? Color.Green : Color.Red;
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

        private DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                DataTable table = new DataTable();
                da.Fill(table);
                return table;
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
    }
}
