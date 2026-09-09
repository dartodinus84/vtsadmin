using System;
using System.Collections.Generic;
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
                DataTable table = ExecuteDataTable(SpSurveyPeriodView);
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

                int periodId;
                int? surveyPeriodId = int.TryParse(hfId.Value, out periodId) ? (int?)periodId : null;

                ExecuteNonQuery(
                    SpSurveyPeriodSave,
                    new SqlParameter("@survey_period_id", SqlDbType.Int) { Value = (object)surveyPeriodId ?? DBNull.Value },
                    new SqlParameter("@period_code", SqlDbType.VarChar, 20) { Value = periodCode },
                    new SqlParameter("@period_name", SqlDbType.VarChar, 50) { Value = periodName },
                    new SqlParameter("@start_date", SqlDbType.Date) { Value = (object)startDate ?? DBNull.Value },
                    new SqlParameter("@end_date", SqlDbType.Date) { Value = (object)endDate ?? DBNull.Value },
                    new SqlParameter("@is_active", SqlDbType.Bit) { Value = chkActive.Checked });

                ShowMessage(surveyPeriodId.HasValue ? "Update berhasil." : "Save berhasil.", true);
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

                ShowMessage("Delete berhasil.", true);
                ClearForm();
            }
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
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
            btnSave.Text = "Save";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
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
