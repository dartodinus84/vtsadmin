using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class cs_survey_category : Page
    {
        private const string SessionGridKey = "RecListCategory";
        private const string DefaultSortField = "survey_category_id";
        private const string DefaultSortDirection = "ASC";
        private const string SpSurveyCategoryView = "sp_cs_survey_category_view";
        private const string SpSurveyCategorySave = "sp_cs_survey_category_save";
        private const string SpSurveyCategoryDelete = "sp_cs_survey_category_delete";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
            catch
            {
                ShowError("Page initialization failed.");
            }
        }

        private void BindGrid()
        {
            try
            {
                if (!HasConnectionString())
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                var clType = new ClsType();
                var data = clType.Open_GridView(
                    gvData,
                    SpSurveyCategoryView,
                    Session["ClsTypeDBConnStringSQL"].ToString(),
                    LblPaging,
                    DefaultSortField,
                    DefaultSortDirection);

                var dataTable = ConvertToDataTable(data);
                dataTable = ApplyTenantFilter(dataTable);
                dataTable = ApplySearchFilter(dataTable, txtSearch.Text.Trim());

                Session[SessionGridKey] = dataTable;
                gvData.DataSource = dataTable;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                RegisterClientError(ex);
            }
        }

        private void SaveData(int? surveyCategoryId)
        {
            if (!HasConnectionString())
            {
                throw new InvalidOperationException("Session connection string is not available.");
            }

            var categoryCode = txtCategoryCode.Text.Trim();
            var categoryName = txtCategoryName.Text.Trim();
            var description = txtDescription.Text.Trim();
            var isActive = chkIsActive.Checked ? 1 : 0;

            if (string.IsNullOrWhiteSpace(categoryCode) || string.IsNullOrWhiteSpace(categoryName))
            {
                throw new InvalidOperationException("Category Code dan Category Name wajib diisi.");
            }

            if (IsDuplicateCategory(surveyCategoryId, categoryCode, categoryName))
            {
                throw new InvalidOperationException("Duplicate category code/name found. Please use another value.");
            }

            int rowsAffected;
            string errMessage;
            if (!ExecuteSurveyCategorySave(surveyCategoryId, categoryCode, categoryName, description, isActive, out rowsAffected, out errMessage))
            {
                throw new Exception("Save failed (" + errMessage + ").");
            }
        }

        private void DeleteData(int id)
        {
            int rowsAffected;
            string errMessage;
            if (!ExecuteSurveyCategoryDelete(id, out rowsAffected, out errMessage))
            {
                throw new Exception("Delete failed (" + errMessage + ").");
            }
        }

        protected void ClearForm()
        {
            hfId.Value = string.Empty;
            txtCategoryCode.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            div_comment.InnerHtml = string.Empty;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int? id = string.IsNullOrEmpty(hfId.Value) ? (int?)null : Convert.ToInt32(hfId.Value);
                SaveData(id);
                BindGrid();
                ClearForm();
                RegisterClientSuccess("closeModalFix(); showSuccess('Data berhasil disimpan');");
            }
            catch (Exception ex)
            {
                RegisterClientError(ex);
            }
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandName))
            {
                return;
            }

            try
            {
                if (string.Equals(e.CommandName, "EditData", StringComparison.OrdinalIgnoreCase))
                {
                    LoadData(Convert.ToString(e.CommandArgument));
                    return;
                }

                if (string.Equals(e.CommandName, "DeleteData", StringComparison.OrdinalIgnoreCase))
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    DeleteData(id);
                    BindGrid();
                    ClearForm();
                    RegisterClientSuccess("closeModalFix(); showSuccess('Data berhasil dihapus');");
                }
            }
            catch (Exception ex)
            {
                RegisterClientError(ex);
            }
        }

        private void LoadData(string idText)
        {
            int id;
            if (!int.TryParse(idText, out id))
            {
                throw new InvalidOperationException("Invalid category id.");
            }

            var selectedRow = GetCategoryRowById(id);
            if (selectedRow == null)
            {
                throw new InvalidOperationException("Selected data was not found.");
            }

            hfId.Value = SafeDataRowValue(selectedRow, "survey_category_id");
            txtCategoryCode.Text = SafeDataRowValue(selectedRow, "category_code");
            txtCategoryName.Text = SafeDataRowValue(selectedRow, "category_name");
            txtDescription.Text = SafeDataRowValue(selectedRow, "description");
            chkIsActive.Checked = IsTrueValue(SafeDataRowValue(selectedRow, "is_active"));
            btnSave.Text = "Update";
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            BindGrid();
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

        private bool HasConnectionString()
        {
            return Session["ClsTypeDBConnStringSQL"] != null &&
                   !string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString());
        }

        private static int? ParseNullableInt(string value)
        {
            int id;
            return int.TryParse(value, out id) ? (int?)id : null;
        }

        private static bool IsTrueValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var normalized = value.Trim().ToUpperInvariant();
            return normalized == "TRUE" || normalized == "1" || normalized == "YES" || normalized == "Y";
        }

        private DataRow GetCategoryRowById(int id)
        {
            var table = Session[SessionGridKey] as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                table = GetAllCategoryData();
            }

            if (table == null || table.Rows.Count == 0 || !table.Columns.Contains("survey_category_id"))
            {
                return null;
            }

            foreach (DataRow row in table.Rows)
            {
                int rowId;
                if (!int.TryParse(Convert.ToString(row["survey_category_id"]), out rowId))
                {
                    continue;
                }

                if (rowId == id)
                {
                    return row;
                }
            }

            return null;
        }

        private static string SafeDataRowValue(DataRow row, string columnName)
        {
            if (row == null || string.IsNullOrWhiteSpace(columnName) || row.Table == null || !row.Table.Columns.Contains(columnName))
            {
                return string.Empty;
            }

            return Convert.ToString(row[columnName]).Trim();
        }

        private DataTable ConvertToDataTable(object data)
        {
            if (data == null)
            {
                return new DataTable();
            }

            if (data is DataTable)
            {
                return ((DataTable)data).Copy();
            }

            if (data is DataView)
            {
                return ((DataView)data).ToTable();
            }

            if (data is DataSet)
            {
                var ds = (DataSet)data;
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Copy();
                }
            }

            if (data is IEnumerable)
            {
                var table = new DataTable();
                return table;
            }

            return new DataTable();
        }

        private DataTable ApplySearchFilter(DataTable sourceTable, string searchText)
        {
            if (sourceTable == null)
            {
                return new DataTable();
            }

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return sourceTable;
            }

            if (!sourceTable.Columns.Contains("category_code") && !sourceTable.Columns.Contains("category_name"))
            {
                return sourceTable;
            }

            var escaped = searchText.Trim().Replace("'", "''").Replace("[", "[[]");
            var filterParts = new System.Collections.Generic.List<string>();
            if (sourceTable.Columns.Contains("category_code"))
            {
                filterParts.Add("Convert(category_code, 'System.String') LIKE '%" + escaped + "%'");
            }

            if (sourceTable.Columns.Contains("category_name"))
            {
                filterParts.Add("Convert(category_name, 'System.String') LIKE '%" + escaped + "%'");
            }

            if (filterParts.Count == 0)
            {
                return sourceTable;
            }

            var dv = sourceTable.DefaultView;
            dv.RowFilter = string.Join(" OR ", filterParts.ToArray());
            return dv.ToTable();
        }

        private DataTable ApplyTenantFilter(DataTable sourceTable)
        {
            if (sourceTable == null)
            {
                return new DataTable();
            }

            var customerId = GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId) || !sourceTable.Columns.Contains("customer_id"))
            {
                return sourceTable;
            }

            var dv = sourceTable.DefaultView;
            dv.RowFilter = "Convert(customer_id, 'System.String') = '" + customerId.Replace("'", "''") + "'";
            return dv.ToTable();
        }

        private string GetCustomerId()
        {
            var keys = new[]
            {
                "ClsTypeCustomerID",
                "ClsCustIDMaint",
                "CustomerMaintCustID",
                "VehicleMaintCustID",
                "ClsJobCustIDAssign",
                "ClsCustIDCust"
            };

            foreach (var key in keys)
            {
                if (Session[key] != null)
                {
                    var value = Session[key].ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }
            }

            return string.Empty;
        }

        private bool IsDuplicateCategory(int? currentId, string categoryCode, string categoryName)
        {
            var table = GetAllCategoryData();
            if (table.Rows.Count == 0)
            {
                return false;
            }

            foreach (DataRow row in table.Rows)
            {
                var rowId = ParseNullableInt(Convert.ToString(row["survey_category_id"]));
                if (currentId.HasValue && rowId.HasValue && currentId.Value == rowId.Value)
                {
                    continue;
                }

                var rowCode = Convert.ToString(row["category_code"]).Trim();
                var rowName = Convert.ToString(row["category_name"]).Trim();
                if (string.Equals(rowCode, categoryCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(rowName, categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private DataTable GetAllCategoryData()
        {
            try
            {
                if (!HasConnectionString())
                {
                    return new DataTable();
                }

                var rec = new Recordset();
                rec.Open(SpSurveyCategoryView, Session["ClsTypeDBConnStringSQL"].ToString());

                var table = new DataTable();
                table.Columns.Add("survey_category_id", typeof(string));
                table.Columns.Add("category_code", typeof(string));
                table.Columns.Add("category_name", typeof(string));
                table.Columns.Add("customer_id", typeof(string));

                if (rec.RecordCount() > 0)
                {
                    rec.MoveFirst();
                    while (!rec.EOF)
                    {
                        var row = table.NewRow();
                        row["survey_category_id"] = SafeField(rec, "survey_category_id");
                        row["category_code"] = SafeField(rec, "category_code");
                        row["category_name"] = SafeField(rec, "category_name");
                        row["customer_id"] = SafeField(rec, "customer_id");
                        table.Rows.Add(row);
                        rec.MoveNext();
                    }
                }

                return ApplyTenantFilter(table);
            }
            catch
            {
                return new DataTable();
            }
        }

        private static string SafeField(Recordset rec, string fieldName)
        {
            try
            {
                if (rec == null || string.IsNullOrWhiteSpace(fieldName))
                {
                    return string.Empty;
                }

                var value = rec.Fields(fieldName);
                return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool ExecuteSurveyCategorySave(int? id, string categoryCode, string categoryName, string description, int isActive, out int rowsAffected, out string errorMessage)
        {
            var customerId = GetCustomerId();
            var userId = Session["ClsTypeUserID"] == null ? string.Empty : Session["ClsTypeUserID"].ToString().Trim();

            var attempts = new System.Collections.Generic.List<SqlParameter[]>();
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                attempts.Add(CreateSaveParameters(id, categoryCode, categoryName, description, isActive, customerId, userId));
                attempts.Add(CreateSaveParameters(id, categoryCode, categoryName, description, isActive, customerId, null));
            }

            attempts.Add(CreateSaveParameters(id, categoryCode, categoryName, description, isActive, null, userId));
            attempts.Add(CreateSaveParameters(id, categoryCode, categoryName, description, isActive, null, null));

            return ExecuteStoredProcedureWithFallback(SpSurveyCategorySave, attempts, out rowsAffected, out errorMessage);
        }

        private bool ExecuteSurveyCategoryDelete(int id, out int rowsAffected, out string errorMessage)
        {
            var customerId = GetCustomerId();
            var userId = Session["ClsTypeUserID"] == null ? string.Empty : Session["ClsTypeUserID"].ToString().Trim();
            var attempts = new System.Collections.Generic.List<SqlParameter[]>();

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                attempts.Add(CreateDeleteParameters(id, customerId, userId));
                attempts.Add(CreateDeleteParameters(id, customerId, null));
            }

            attempts.Add(CreateDeleteParameters(id, null, userId));
            attempts.Add(CreateDeleteParameters(id, null, null));

            return ExecuteStoredProcedureWithFallback(SpSurveyCategoryDelete, attempts, out rowsAffected, out errorMessage);
        }

        private bool ExecuteStoredProcedureWithFallback(string procedureName, System.Collections.Generic.IEnumerable<SqlParameter[]> parameterSets, out int rowsAffected, out string errorMessage)
        {
            rowsAffected = 0;
            errorMessage = "Unknown database error.";

            foreach (var parameters in parameterSets)
            {
                string executeError;
                if (TryExecuteStoredProcedure(procedureName, parameters, out rowsAffected, out executeError))
                {
                    return true;
                }

                errorMessage = executeError;
            }

            return false;
        }

        private bool TryExecuteStoredProcedure(string procedureName, SqlParameter[] parameters, out int rowsAffected, out string errorMessage)
        {
            rowsAffected = 0;
            errorMessage = string.Empty;

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(procedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        rowsAffected = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static SqlParameter[] CreateSaveParameters(int? id, string categoryCode, string categoryName, string description, int isActive, string customerId, string userId)
        {
            var parameters = new System.Collections.Generic.List<SqlParameter>
            {
                new SqlParameter("@survey_category_id", SqlDbType.Int) { Value = (object)id ?? DBNull.Value },
                new SqlParameter("@category_code", SqlDbType.VarChar, 30) { Value = categoryCode },
                new SqlParameter("@category_name", SqlDbType.VarChar, 100) { Value = categoryName },
                new SqlParameter("@description", SqlDbType.VarChar, 500) { Value = string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description },
                new SqlParameter("@is_active", SqlDbType.Bit) { Value = isActive == 1 }
            };

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add(new SqlParameter("@customer_id", SqlDbType.VarChar, 50) { Value = customerId.Trim() });
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 50) { Value = userId.Trim() });
            }

            return parameters.ToArray();
        }

        private static SqlParameter[] CreateDeleteParameters(int id, string customerId, string userId)
        {
            var parameters = new System.Collections.Generic.List<SqlParameter>
            {
                new SqlParameter("@survey_category_id", SqlDbType.Int) { Value = id }
            };

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add(new SqlParameter("@customer_id", SqlDbType.VarChar, 50) { Value = customerId.Trim() });
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 50) { Value = userId.Trim() });
            }

            return parameters.ToArray();
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

        private void RegisterClientSuccess(string script)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "csSurveyCategorySuccess_" + Guid.NewGuid().ToString("N"), script, true);
        }

        private void RegisterClientError(Exception ex)
        {
            var message = ex == null ? "Unknown error." : ex.Message;
            var safeMessage = message.Replace("'", string.Empty).Replace("\r", " ").Replace("\n", " ");
            ScriptManager.RegisterStartupScript(this, GetType(), "csSurveyCategoryError_" + Guid.NewGuid().ToString("N"), "showError('" + safeMessage + "');", true);
        }

        private void ShowSuccess(string message)
        {
            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> " + HttpUtility.HtmlEncode(message) + "</div>";
        }

        private void ShowError(string message)
        {
            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + HttpUtility.HtmlEncode(message) + "</div>";
        }
    }
}
