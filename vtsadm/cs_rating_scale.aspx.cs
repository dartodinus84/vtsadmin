using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class cs_rating_scale : Page
    {
        private const string SpView = "sp_cs_rating_scale_view";
        private const string SpSave = "sp_cs_rating_scale_save";
        private const string SpDelete = "sp_cs_rating_scale_delete";

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

        private void BindGrid()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpView);
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
                if (string.IsNullOrWhiteSpace(txtScaleName.Text))
                {
                    ShowMessage("Scale name wajib diisi", false);
                    return;
                }

                int minValue;
                int maxValue;
                if (!int.TryParse(txtMin.Text, out minValue) || !int.TryParse(txtMax.Text, out maxValue))
                {
                    ShowMessage("Min dan Max harus angka", false);
                    return;
                }

                if (minValue > maxValue)
                {
                    ShowMessage("Min tidak boleh lebih besar dari Max", false);
                    return;
                }

                int ratingScaleId;
                int? id = int.TryParse(hfId.Value, out ratingScaleId) ? (int?)ratingScaleId : null;

                ExecuteNonQuery(
                    SpSave,
                    new SqlParameter("@rating_scale_id", SqlDbType.Int) { Value = (object)id ?? DBNull.Value },
                    new SqlParameter("@scale_name", SqlDbType.VarChar, 100) { Value = txtScaleName.Text.Trim() },
                    new SqlParameter("@min_value", SqlDbType.Int) { Value = minValue },
                    new SqlParameter("@max_value", SqlDbType.Int) { Value = maxValue },
                    new SqlParameter("@is_active", SqlDbType.Bit) { Value = chkActive.Checked });

                ShowMessage(id.HasValue ? "Update berhasil" : "Save berhasil", true);
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

            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id))
            {
                ShowMessage("ID tidak valid", false);
                return;
            }

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
            catch (SqlException ex)
            {
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, false);
            }
        }

        private void LoadData(int id)
        {
            DataTable table = ExecuteDataTable(SpView);
            DataRow targetRow = table.AsEnumerable()
                .FirstOrDefault(row => Convert.ToInt32(row["rating_scale_id"]) == id);

            if (targetRow == null)
            {
                ShowMessage("Data tidak ditemukan", false);
                return;
            }

            hfId.Value = Convert.ToString(targetRow["rating_scale_id"]);
            txtScaleName.Text = Convert.ToString(targetRow["scale_name"]);
            txtMin.Text = Convert.ToString(targetRow["min_value"]);
            txtMax.Text = Convert.ToString(targetRow["max_value"]);
            chkActive.Checked = Convert.ToBoolean(targetRow["is_active"]);
            btnSave.Text = "Update";
            ShowMessage("Mode edit aktif", true);
        }

        private void DeleteData(int id)
        {
            ExecuteNonQuery(
                SpDelete,
                new SqlParameter("@rating_scale_id", SqlDbType.Int) { Value = id });

            if (hfId.Value == id.ToString())
            {
                ClearForm();
            }

            ShowMessage("Delete berhasil", true);
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

        private string GetConnectionString()
        {
            if (Session["ClsTypeDBConnStringSQL"] != null &&
                !string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString()))
            {
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

            ConnectionStringSettings cfg = ConfigurationManager.ConnectionStrings["connStr"];
            if (cfg != null && !string.IsNullOrWhiteSpace(cfg.ConnectionString))
            {
                return cfg.ConnectionString;
            }

            throw new InvalidOperationException("Connection string tidak ditemukan.");
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

        private void ClearForm()
        {
            hfId.Value = string.Empty;
            txtScaleName.Text = string.Empty;
            txtMin.Text = string.Empty;
            txtMax.Text = string.Empty;
            chkActive.Checked = true;
            btnSave.Text = "Save";
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
    }
}
