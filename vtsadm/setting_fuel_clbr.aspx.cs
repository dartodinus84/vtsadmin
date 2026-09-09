using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class setting_fuel_clbr : System.Web.UI.Page
    {
        private const string SpView = "sp_setting_fuel_clbr_view";
        private const string SpInsert = "sp_setting_fuel_clbr_insert";
        private const string SpUpdate = "sp_setting_fuel_clbr_update";
        private const string SpDelete = "sp_setting_fuel_clbr_delete";
        private const string SpDropdownVendor = "sp_setting_fuel_clbr_dropdown_vendor";
        private const string SpDropdownBrand = "sp_setting_fuel_clbr_dropdown_brand";
        private const string SpDropdownTypeByBrand = "sp_setting_fuel_clbr_dropdown_type_by_brand";

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
                    BindVendor();
                    BindBrand();
                    BindType();
                    BindGrid();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void BindVendor()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpDropdownVendor);
                ddlVendor.DataSource = table;
                ddlVendor.DataValueField = "VendorID";
                ddlVendor.DataTextField = "VendorName";
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, new ListItem("[Select]", "[Select]"));
            }
            catch (Exception ex)
            {
                ShowError("Load vendor failed (" + ex.Message + ")");
            }
        }

        protected void BindBrand()
        {
            try
            {
                DataTable table = ExecuteDataTable(SpDropdownBrand);
                ddlBrand.DataSource = table;
                ddlBrand.DataValueField = "VehicleBrandID";
                ddlBrand.DataTextField = "BrandVehicle";
                ddlBrand.DataBind();
                ddlBrand.Items.Insert(0, new ListItem("[Select]", "[Select]"));
            }
            catch (Exception ex)
            {
                ShowError("Load brand failed (" + ex.Message + ")");
            }
        }

        protected void BindType()
        {
            ddlType.Items.Clear();
            ddlType.Items.Insert(0, new ListItem("[Select]", "[Select]"));

            if (ddlBrand.SelectedValue == "[Select]")
            {
                return;
            }

            try
            {
                DataTable table = ExecuteDataTable(
                    SpDropdownTypeByBrand,
                    new SqlParameter("@VehicleBrandID", SqlDbType.VarChar, 10) { Value = ddlBrand.SelectedValue.Trim() });

                ddlType.DataSource = table;
                ddlType.DataValueField = "VehicleTypeID";
                ddlType.DataTextField = "TypeVehicle";
                ddlType.DataBind();
                ddlType.Items.Insert(0, new ListItem("[Select]", "[Select]"));
            }
            catch (Exception ex)
            {
                ShowError("Load type failed (" + ex.Message + ")");
            }
        }

        protected void BindGrid()
        {
            try
            {
                string search = txtSearch.Text == null ? string.Empty : txtSearch.Text.Trim();
                DataTable table = ExecuteDataTable(
                    SpView,
                    new SqlParameter("@Search", SqlDbType.VarChar, 100)
                    {
                        Value = string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search
                    });

                gvData.DataSource = table;
                gvData.DataBind();
                ShowPaging(table.Rows.Count, gvData.PageIndex, gvData.PageSize);
            }
            catch (Exception ex)
            {
                ShowError("Load data failed (" + ex.Message + ")");
            }
        }

        protected void SaveData()
        {
            double voltage;
            double fuelValue;
            if (!ValidateInput(out voltage, out fuelValue))
            {
                return;
            }

            try
            {
                ExecuteNonQuery(
                    SpInsert,
                    new SqlParameter("@VendorID", SqlDbType.VarChar, 10) { Value = ddlVendor.SelectedValue.Trim() },
                    new SqlParameter("@SensorType", SqlDbType.VarChar, 20) { Value = ddlSensorType.SelectedValue.Trim() },
                    new SqlParameter("@VehicleBrandID", SqlDbType.VarChar, 10) { Value = ddlBrand.SelectedValue.Trim() },
                    new SqlParameter("@VehicleTypeID", SqlDbType.VarChar, 10) { Value = ddlType.SelectedValue.Trim() },
                    new SqlParameter("@Acc", SqlDbType.TinyInt) { Value = Convert.ToByte(ddlACC.SelectedValue.Trim()) },
                    new SqlParameter("@Voltage", SqlDbType.Float) { Value = voltage },
                    new SqlParameter("@FuelValue", SqlDbType.Float) { Value = fuelValue },
                    new SqlParameter("@UsrUpd", SqlDbType.VarChar, 50) { Value = GetCurrentUserId() });

                ShowSuccess("Save data berhasil.");
                BindGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void UpdateData()
        {
            double voltage;
            double fuelValue;
            if (!ValidateInput(out voltage, out fuelValue))
            {
                return;
            }

            int id;
            if (!int.TryParse(hidID.Value.Trim(), out id))
            {
                ShowError("ID data tidak valid.");
                return;
            }

            try
            {
                ExecuteNonQuery(
                    SpUpdate,
                    new SqlParameter("@ID", SqlDbType.Int) { Value = id },
                    new SqlParameter("@VendorID", SqlDbType.VarChar, 10) { Value = ddlVendor.SelectedValue.Trim() },
                    new SqlParameter("@SensorType", SqlDbType.VarChar, 20) { Value = ddlSensorType.SelectedValue.Trim() },
                    new SqlParameter("@VehicleBrandID", SqlDbType.VarChar, 10) { Value = ddlBrand.SelectedValue.Trim() },
                    new SqlParameter("@VehicleTypeID", SqlDbType.VarChar, 10) { Value = ddlType.SelectedValue.Trim() },
                    new SqlParameter("@Acc", SqlDbType.TinyInt) { Value = Convert.ToByte(ddlACC.SelectedValue.Trim()) },
                    new SqlParameter("@Voltage", SqlDbType.Float) { Value = voltage },
                    new SqlParameter("@FuelValue", SqlDbType.Float) { Value = fuelValue },
                    new SqlParameter("@UsrUpd", SqlDbType.VarChar, 50) { Value = GetCurrentUserId() });

                ShowSuccess("Update data berhasil.");
                BindGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void DeleteData(int id)
        {
            try
            {
                ExecuteNonQuery(
                    SpDelete,
                    new SqlParameter("@ID", SqlDbType.Int) { Value = id },
                    new SqlParameter("@UsrUpd", SqlDbType.VarChar, 50) { Value = GetCurrentUserId() });

                ShowSuccess("Delete data berhasil.");
                BindGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void ClearForm()
        {
            hidID.Value = string.Empty;
            ddlVendor.SelectedValue = "[Select]";
            ddlBrand.SelectedValue = "[Select]";
            BindType();
            ddlSensorType.SelectedValue = "[Select]";
            ddlACC.SelectedValue = "[Select]";
            txtVoltage.Text = string.Empty;
            txtFuelValue.Text = string.Empty;
            btnSave.Text = "Save";
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindType();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hidID.Value))
            {
                SaveData();
                return;
            }

            UpdateData();
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rowIndex;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out rowIndex))
                {
                    return;
                }

                if (rowIndex < 0 || rowIndex >= gvData.Rows.Count)
                {
                    return;
                }

                GridViewRow row = gvData.Rows[rowIndex];
                int id = ParseInt(GetCellText(row, 14));

                if (string.Equals(e.CommandName, "EditData", StringComparison.OrdinalIgnoreCase))
                {
                    string vendorId = GetCellText(row, 10);
                    string brandId = GetCellText(row, 11);
                    string typeId = GetCellText(row, 12);
                    string accValue = GetCellText(row, 13);

                    hidID.Value = id.ToString();
                    SelectDropDownValue(ddlVendor, vendorId, "[Select]");
                    SelectDropDownValue(ddlBrand, brandId, "[Select]");
                    BindType();
                    SelectDropDownValue(ddlType, typeId, "[Select]");
                    SelectDropDownValue(ddlSensorType, GetCellText(row, 4), "[Select]");
                    SelectDropDownValue(ddlACC, accValue, "[Select]");
                    txtVoltage.Text = GetCellText(row, 6);
                    txtFuelValue.Text = GetCellText(row, 7);
                    btnSave.Text = "Update";
                    return;
                }

                if (string.Equals(e.CommandName, "DeleteData", StringComparison.OrdinalIgnoreCase))
                {
                    DeleteData(id);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 10; i <= 14; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
                return;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNo = (Label)e.Row.FindControl("lblNo");
                if (lblNo != null)
                {
                    lblNo.Text = ((gvData.PageIndex * gvData.PageSize) + e.Row.RowIndex + 1).ToString();
                }

                string statusCode = GetCellText(e.Row, 8).ToUpperInvariant();
                if (statusCode == "RG")
                {
                    e.Row.Cells[8].Text = "<span class='label label-success'>RG</span>";
                }
                else if (statusCode == "DE")
                {
                    e.Row.Cells[8].Text = "<span class='label label-danger'>DE</span>";
                }
                else
                {
                    e.Row.Cells[8].Text = "<span class='label label-default'>" + HttpUtility.HtmlEncode(statusCode) + "</span>";
                }

                for (int i = 10; i <= 14; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvData.PageIndex = 0;
            BindGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private bool ValidateInput(out double voltage, out double fuelValue)
        {
            voltage = 0;
            fuelValue = 0;

            if (ddlVendor.SelectedValue == "[Select]")
            {
                ShowError("Vendor wajib dipilih.");
                return false;
            }

            if (ddlBrand.SelectedValue == "[Select]")
            {
                ShowError("Brand wajib dipilih.");
                return false;
            }

            if (ddlType.SelectedValue == "[Select]")
            {
                ShowError("Type wajib dipilih.");
                return false;
            }

            if (ddlSensorType.SelectedValue == "[Select]")
            {
                ShowError("Sensor type wajib dipilih.");
                return false;
            }

            if (ddlACC.SelectedValue == "[Select]")
            {
                ShowError("ACC wajib dipilih.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtVoltage.Text))
            {
                ShowError("Voltage wajib diisi.");
                return false;
            }

            if (!TryParseDouble(txtVoltage.Text.Trim(), out voltage))
            {
                ShowError("Voltage harus decimal.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFuelValue.Text))
            {
                ShowError("Fuel value wajib diisi.");
                return false;
            }

            if (!TryParseDouble(txtFuelValue.Text.Trim(), out fuelValue))
            {
                ShowError("Fuel value harus decimal.");
                return false;
            }

            return true;
        }

        private static bool TryParseDouble(string rawValue, out double value)
        {
            string normalized = (rawValue ?? string.Empty).Trim();
            return double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out value)
                   || double.TryParse(normalized, NumberStyles.Any, CultureInfo.CurrentCulture, out value);
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

        private string GetCurrentUserId()
        {
            if (Session["ClsTypeUserID"] == null)
            {
                return string.Empty;
            }

            return Session["ClsTypeUserID"].ToString();
        }

        private static void SelectDropDownValue(DropDownList ddl, string targetValue, string defaultValue)
        {
            if (ddl == null)
            {
                return;
            }

            ListItem item = ddl.Items.FindByValue(targetValue);
            if (item != null)
            {
                ddl.SelectedValue = targetValue;
                return;
            }

            ddl.SelectedValue = defaultValue;
        }

        private static int ParseInt(string text)
        {
            int result;
            if (!int.TryParse(text, out result))
            {
                return 0;
            }

            return result;
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

        private static string GetCellText(GridViewRow row, int cellIndex)
        {
            if (row == null || cellIndex < 0 || cellIndex >= row.Cells.Count)
            {
                return string.Empty;
            }

            string raw = HttpUtility.HtmlDecode(row.Cells[cellIndex].Text ?? string.Empty);
            return string.Equals(raw, "&nbsp;", StringComparison.OrdinalIgnoreCase) ? string.Empty : raw.Trim();
        }

        private void ShowSuccess(string message)
        {
            SetMessages(
                "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> " + HttpUtility.HtmlEncode(message) + "</div>");
        }

        private void ShowError(string message)
        {
            SetMessages(
                "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + HttpUtility.HtmlEncode(message) + "</div>");
        }

        private void SetMessages(string htmlMessage)
        {
            div_comment.InnerHtml = htmlMessage;
            div_message.InnerHtml = htmlMessage;
        }

        private void ShowPaging(int totalRows, int pageIndex, int pageSize)
        {
            int currentStart = 0;
            int currentEnd = 0;
            if (totalRows > 0)
            {
                currentStart = (pageIndex * pageSize) + 1;
                currentEnd = Math.Min((pageIndex * pageSize) + gvData.Rows.Count, totalRows);
            }

            lblPaging.Text = string.Format(
                "Displaying {0} to {1} of {2} records found",
                currentStart,
                currentEnd,
                totalRows);
        }
    }
}
