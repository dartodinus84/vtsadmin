using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class cs_question_type : Page
    {
        private const string SpQuestionTypeView = "sp_cs_question_type_view";
        private const string SpQuestionTypeSave = "sp_cs_question_type_save";
        private const string SpQuestionTypeDelete = "sp_cs_question_type_delete";

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
                var table = ExecuteDataTable(SpQuestionTypeView);
                gvData.DataSource = table;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Load data failed: " + ex.Message, false);
            }
        }

        protected void ClearForm()
        {
            txtQuestionTypeCode.Text = string.Empty;
            txtQuestionTypeName.Text = string.Empty;
            txtQuestionTypeCode.ReadOnly = false;
            txtQuestionTypeCode.Enabled = true;
            btnSave.Text = "Save";
            ClearMessage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ClearMessage();
            try
            {
                var questionTypeCode = txtQuestionTypeCode.Text.Trim();
                var questionTypeName = txtQuestionTypeName.Text.Trim();

                if (string.IsNullOrWhiteSpace(questionTypeCode) || string.IsNullOrWhiteSpace(questionTypeName))
                {
                    ShowMessage("Question Type Code dan Question Type Name wajib diisi.", false);
                    return;
                }

                ExecuteNonQuery(
                    SpQuestionTypeSave,
                    new SqlParameter("@question_type_code", SqlDbType.VarChar, 30) { Value = questionTypeCode },
                    new SqlParameter("@question_type_name", SqlDbType.VarChar, 100) { Value = questionTypeName });

                ShowMessage("Save berhasil.", true);
                ClearForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                ShowMessage("Save gagal: " + ex.Message, false);
            }
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandArgument == null ? null : e.CommandArgument.ToString()))
            {
                return;
            }

            ClearMessage();
            var questionTypeCode = e.CommandArgument.ToString().Trim();
            try
            {
                if (string.Equals(e.CommandName, "EditData", StringComparison.OrdinalIgnoreCase))
                {
                    LoadData(questionTypeCode);
                    return;
                }

                if (string.Equals(e.CommandName, "DeleteData", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteData(questionTypeCode);
                    BindGrid();
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "successDeleteQuestionType",
                        "showSuccess('Data berhasil dihapus');",
                        true);
                }
            }
            catch (Exception ex)
            {
                var safeMessage = ex.Message.Replace("'", string.Empty);
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "errorDeleteQuestionType",
                    "showError('" + safeMessage + "');",
                    true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void LoadData(string questionTypeCode)
        {
            var table = ExecuteDataTable(SpQuestionTypeView);
            var targetRow = table.AsEnumerable()
                .FirstOrDefault(row => string.Equals(
                    Convert.ToString(row["question_type_code"]),
                    questionTypeCode,
                    StringComparison.OrdinalIgnoreCase));

            if (targetRow == null)
            {
                ShowMessage("Data question type tidak ditemukan.", false);
                return;
            }

            txtQuestionTypeCode.Text = Convert.ToString(targetRow["question_type_code"]).Trim();
            txtQuestionTypeName.Text = Convert.ToString(targetRow["question_type_name"]).Trim();
            txtQuestionTypeCode.Enabled = false;
            btnSave.Text = "Update";
        }

        private void DeleteData(string questionTypeCode)
        {
            ExecuteNonQuery(
                SpQuestionTypeDelete,
                new SqlParameter("@question_type_code", SqlDbType.VarChar, 30) { Value = questionTypeCode });

            ClearForm();
        }

        private DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(storedProcedureName, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
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

        private int ExecuteNonQuery(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(storedProcedureName, conn))
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
                throw new InvalidOperationException("Session connection string tidak tersedia.");
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

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = HttpUtility.HtmlEncode(message);
            lblMessage.ForeColor = isSuccess ? Color.Green : Color.Red;
        }

        private void ClearMessage()
        {
            lblMessage.Text = string.Empty;
            lblMessage.ForeColor = Color.Empty;
        }
    }
}

