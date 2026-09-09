using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mdvr_package_master : Page
    {
        private const string ViewStateDetailTable = "MdvrPackageDetailTable";
        private const string SessionGridKey = "RecMdvrPackageList";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterAllGridAsyncControls();
        }

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
                    BindDropdowns();
                    ClearForm();
                    BindPackageGrid();
                }

                RegisterAsyncPostBackControls();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateHeaderInput();

                var detailTable = GetDetailTable();
                ValidateDetailNotEmpty(detailTable);
                ValidateDetailNoDuplicate(detailTable);

                var packageId = hfPackageID.Value.Trim();
                var detailXml = BuildDetailXml(detailTable);
                var userId = GetUserId();

                if (string.IsNullOrEmpty(packageId))
                {
                    CreatePackage(detailXml, userId);
                    ShowSuccess("Paket MDVR berhasil disimpan.");
                }
                else
                {
                    UpdatePackage(packageId, detailXml, userId);
                    ShowSuccess("Paket MDVR berhasil diperbarui.");
                }

                ClearForm();
                BindPackageGrid();
            }
            catch (Exception ex)
            {
                ShowError(GetSqlErrorMessage(ex));
            }
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                var deviceTypeId = ddlDetailDeviceType.SelectedValue;
                if (string.IsNullOrWhiteSpace(deviceTypeId) || deviceTypeId == "[Select]")
                {
                    throw new InvalidOperationException("Accessory wajib dipilih.");
                }

                decimal qty;
                if (!TryParsePositiveDecimal(txtDetailQty.Text, out qty))
                {
                    throw new InvalidOperationException("Detail Qty harus lebih besar dari 0.");
                }

                decimal hppPrice;
                if (!TryParseNonNegativeDecimal(txtDetailHppPrice.Text, out hppPrice))
                {
                    throw new InvalidOperationException("Detail HPP tidak boleh minus.");
                }

                decimal sellingPrice;
                if (!TryParseNonNegativeDecimal(txtDetailSellingPrice.Text, out sellingPrice))
                {
                    throw new InvalidOperationException("Detail Selling Price tidak boleh minus.");
                }

                var detailTable = GetDetailTable().Copy();
                var deviceDesc = ddlDetailDeviceType.SelectedItem != null
                    ? ddlDetailDeviceType.SelectedItem.Text
                    : deviceTypeId;

                if (!IsDeviceTypeInDetailDropdown(deviceTypeId))
                {
                    throw new InvalidOperationException("Accessory harus dipilih dari data SP.");
                }

                if (IsDuplicateDetailDevice(detailTable, deviceTypeId))
                {
                    throw new InvalidOperationException(
                        "Accessory \"" + deviceDesc + "\" sudah ada dalam paket ini. Tidak boleh duplikat.");
                }

                var nextSeq = detailTable.Rows.Count + 1;

                detailTable.Rows.Add(nextSeq, deviceTypeId, deviceDesc, qty, hppPrice, sellingPrice);
                ViewState[ViewStateDetailTable] = detailTable;
                BindDetailGrid();

                txtDetailQty.Text = "1";
                txtDetailHppPrice.Text = "0";
                txtDetailSellingPrice.Text = "0";
                ddlDetailDeviceType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RegisterGridLinkButton(e.Row, "btnRemoveDetail");
        }

        protected void gvPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RegisterGridLinkButton(e.Row, "btnEdit");
            RegisterGridLinkButton(e.Row, "btnDelete");
        }

        protected void gvDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "RemoveDetail", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                var seq = ResolveDetailSeq(e);
                var detailTable = GetDetailTable().Copy();
                for (var i = detailTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(detailTable.Rows[i]["Seq"]) == seq)
                    {
                        detailTable.Rows.RemoveAt(i);
                    }
                }

                ReindexDetailSeq(detailTable);
                ViewState[ViewStateDetailTable] = detailTable;
                BindDetailGrid();

                var packageId = hfPackageID.Value.Trim();
                if (!string.IsNullOrEmpty(packageId))
                {
                    ValidateDetailNotEmpty(detailTable);
                    ValidateDetailNoDuplicate(detailTable);
                    ValidateHeaderInput();
                    UpdatePackage(packageId, BuildDetailXml(detailTable), GetUserId());
                }

                ShowSuccess("Item berhasil dihapus dari detail paket.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        protected void gvPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandName))
            {
                return;
            }

            try
            {
                var packageId = ResolvePackageId(e);
                if (string.IsNullOrWhiteSpace(packageId))
                {
                    throw new InvalidOperationException("PackageID wajib diisi.");
                }

                if (string.Equals(e.CommandName, "EditData", StringComparison.OrdinalIgnoreCase))
                {
                    LoadPackageForEdit(packageId);
                    return;
                }

                if (string.Equals(e.CommandName, "DeleteData", StringComparison.OrdinalIgnoreCase))
                {
                    DeletePackage(packageId);
                    ClearForm();
                    BindPackageGrid();
                    ShowSuccess("Paket MDVR berhasil dihapus.");
                }
            }
            catch (Exception ex)
            {
                ShowError(GetSqlErrorMessage(ex));
            }
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            BindPackageGrid();
        }

        private void CreatePackage(string detailXml, string userId)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.sp_mdvr_package_create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;

                cmd.Parameters.Add(new SqlParameter("@DeviceTypeID", SqlDbType.VarChar, 20) { Value = ddlDeviceMdvr.SelectedValue });
                cmd.Parameters.Add(new SqlParameter("@PackageName", SqlDbType.VarChar, 100) { Value = txtPackageName.Text.Trim() });
                cmd.Parameters.Add(new SqlParameter("@HppPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ParseDecimal(txtHppPrice.Text) });
                cmd.Parameters.Add(new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ParseDecimal(txtSellingPrice.Text) });
                cmd.Parameters.Add(new SqlParameter("@Remark", SqlDbType.VarChar, 255) { Value = GetNullableString(txtRemark.Text) });
                cmd.Parameters.Add(new SqlParameter("@DetailXml", SqlDbType.Xml) { Value = detailXml });
                cmd.Parameters.Add(new SqlParameter("@UsrCrt", SqlDbType.VarChar, 120) { Value = GetNullableString(userId) });

                var outPackageId = new SqlParameter("@PackageID", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outPackageId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdatePackage(string packageId, string detailXml, string userId)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.sp_mdvr_package_update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;

                cmd.Parameters.Add(new SqlParameter("@PackageID", SqlDbType.VarChar, 20) { Value = packageId });
                cmd.Parameters.Add(new SqlParameter("@DeviceTypeID", SqlDbType.VarChar, 20) { Value = ddlDeviceMdvr.SelectedValue });
                cmd.Parameters.Add(new SqlParameter("@PackageName", SqlDbType.VarChar, 100) { Value = txtPackageName.Text.Trim() });
                cmd.Parameters.Add(new SqlParameter("@HppPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ParseDecimal(txtHppPrice.Text) });
                cmd.Parameters.Add(new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = ParseDecimal(txtSellingPrice.Text) });
                cmd.Parameters.Add(new SqlParameter("@Remark", SqlDbType.VarChar, 255) { Value = GetNullableString(txtRemark.Text) });
                cmd.Parameters.Add(new SqlParameter("@DetailXml", SqlDbType.Xml) { Value = detailXml });
                cmd.Parameters.Add(new SqlParameter("@UsrUpd", SqlDbType.VarChar, 120) { Value = GetNullableString(userId) });

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeletePackage(string packageId)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.sp_mdvr_package_delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add(new SqlParameter("@PackageID", SqlDbType.VarChar, 20) { Value = packageId });
                cmd.Parameters.Add(new SqlParameter("@UsrUpd", SqlDbType.VarChar, 120) { Value = GetNullableString(GetUserId()) });

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadPackageForEdit(string packageId)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("dbo.sp_mdvr_package_view_by_id", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add(new SqlParameter("@PackageID", SqlDbType.VarChar, 20) { Value = packageId });

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Data paket tidak ditemukan.");
                    }

                    hfPackageID.Value = Convert.ToString(reader["PackageID"]);
                    SetDropdownValue(ddlDeviceMdvr, Convert.ToString(reader["DeviceTypeID"]));
                    txtPackageName.Text = Convert.ToString(reader["PackageName"]);
                    txtHppPrice.Text = Convert.ToString(reader["HppPrice"], CultureInfo.InvariantCulture);
                    txtSellingPrice.Text = Convert.ToString(reader["SellingPrice"], CultureInfo.InvariantCulture);
                    txtRemark.Text = Convert.ToString(reader["Remark"]);
                    btnSave.Text = "Update Paket";

                    if (!reader.NextResult())
                    {
                        InitDetailTable();
                        BindDetailGrid();
                        return;
                    }

                    InitDetailTable();
                    var detailTable = GetDetailTable();
                    while (reader.Read())
                    {
                        detailTable.Rows.Add(
                            Convert.ToInt32(reader["Seq"]),
                            Convert.ToString(reader["DeviceTypeID"]),
                            Convert.ToString(reader["DeviceTypeDesc"]),
                            Convert.ToDecimal(reader["Qty"]),
                            Convert.ToDecimal(reader["HppPrice"]),
                            Convert.ToDecimal(reader["SellingPrice"]));
                    }

                    ViewState[ViewStateDetailTable] = detailTable.Copy();
                }
            }

            BindDetailGrid();
            div_comment.InnerHtml = string.Empty;
        }

        private void BindPackageGrid()
        {
            var table = ExecuteDataTable("dbo.sp_mdvr_package_view_all", null);
            table = ApplySearchFilter(table, txtSearch.Text.Trim());
            Session[SessionGridKey] = table;
            gvPackage.DataSource = table;
            gvPackage.DataBind();
        }

        private void BindDropdowns()
        {
            LoadDeviceDropdown(ddlDeviceMdvr);
            LoadDetailDeviceDropdown();
        }

        private void LoadDeviceDropdown(DropDownList dropdown)
        {
            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem("[Select]", "[Select]"));

            var table = ExecuteDataTable("dbo.sp_mdvr_dropdown_device", null);
            foreach (DataRow row in table.Rows)
            {
                dropdown.Items.Add(new ListItem(
                    Convert.ToString(row["DeviceTypeDesc"]),
                    Convert.ToString(row["DeviceTypeID"])));
            }
        }

        private void LoadDetailDeviceDropdown()
        {
            ddlDetailDeviceType.Items.Clear();
            ddlDetailDeviceType.Items.Add(new ListItem("[Select]", "[Select]"));

            var table = ExecuteDataTable("dbo.sp_mdvr_dropdown_accessories", null);
            foreach (DataRow row in table.Rows)
            {
                ddlDetailDeviceType.Items.Add(new ListItem(
                    Convert.ToString(row["DeviceTypeDesc"]),
                    Convert.ToString(row["DeviceTypeID"])));
            }
        }

        private bool IsDeviceTypeInDetailDropdown(string deviceTypeId)
        {
            foreach (ListItem item in ddlDetailDeviceType.Items)
            {
                if (string.Equals(item.Value, deviceTypeId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private DataTable ExecuteDataTable(string procedureName, SqlParameter[] parameters)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        private void ValidateHeaderInput()
        {
            if (ddlDeviceMdvr.SelectedValue == "[Select]" || string.IsNullOrWhiteSpace(ddlDeviceMdvr.SelectedValue))
            {
                throw new InvalidOperationException("Device MDVR header wajib dipilih.");
            }

            if (string.IsNullOrWhiteSpace(txtPackageName.Text))
            {
                throw new InvalidOperationException("PackageName wajib diisi.");
            }

            decimal dummyDecimal;
            if (!TryParseNonNegativeDecimal(txtHppPrice.Text, out dummyDecimal))
            {
                throw new InvalidOperationException("HppPrice tidak boleh minus.");
            }

            if (!TryParseNonNegativeDecimal(txtSellingPrice.Text, out dummyDecimal))
            {
                throw new InvalidOperationException("SellingPrice tidak boleh minus.");
            }
        }

        private static bool IsDuplicateDetailDevice(DataTable detailTable, string deviceTypeId)
        {
            if (detailTable == null || string.IsNullOrWhiteSpace(deviceTypeId))
            {
                return false;
            }

            foreach (DataRow row in detailTable.Rows)
            {
                if (string.Equals(Convert.ToString(row["DeviceTypeID"]).Trim(), deviceTypeId.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ValidateDetailNotEmpty(DataTable detailTable)
        {
            if (detailTable == null || detailTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("Detail kebutuhan item wajib diisi.");
            }
        }

        private static void ValidateDetailNoDuplicate(DataTable detailTable)
        {
            var seen = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in detailTable.Rows)
            {
                var deviceTypeId = Convert.ToString(row["DeviceTypeID"]).Trim();
                if (string.IsNullOrEmpty(deviceTypeId))
                {
                    continue;
                }

                if (!seen.Add(deviceTypeId))
                {
                    var deviceDesc = Convert.ToString(row["DeviceTypeDesc"]);
                    if (string.IsNullOrWhiteSpace(deviceDesc))
                    {
                        deviceDesc = deviceTypeId;
                    }

                    throw new InvalidOperationException(
                        "Accessory \"" + deviceDesc + "\" tidak boleh duplikat dalam satu paket.");
                }
            }
        }

        private static string BuildDetailXml(DataTable detailTable)
        {
            var sb = new StringBuilder();
            sb.Append("<Details>");
            foreach (DataRow row in detailTable.Rows)
            {
                sb.Append("<Detail Seq=\"")
                    .Append(Convert.ToInt32(row["Seq"]))
                    .Append("\" DeviceTypeID=\"")
                    .Append(XmlAttributeEncode(Convert.ToString(row["DeviceTypeID"])))
                    .Append("\" Qty=\"")
                    .Append(Convert.ToDecimal(row["Qty"]).ToString(CultureInfo.InvariantCulture))
                    .Append("\" HppPrice=\"")
                    .Append(Convert.ToDecimal(row["HppPrice"]).ToString(CultureInfo.InvariantCulture))
                    .Append("\" SellingPrice=\"")
                    .Append(Convert.ToDecimal(row["SellingPrice"]).ToString(CultureInfo.InvariantCulture))
                    .Append("\" />");
            }

            sb.Append("</Details>");
            return sb.ToString();
        }

        private void ClearForm()
        {
            hfPackageID.Value = string.Empty;
            ddlDeviceMdvr.SelectedIndex = 0;
            txtPackageName.Text = string.Empty;
            txtHppPrice.Text = "0";
            txtSellingPrice.Text = "0";
            txtRemark.Text = string.Empty;
            txtDetailQty.Text = "1";
            txtDetailHppPrice.Text = "0";
            txtDetailSellingPrice.Text = "0";
            ddlDetailDeviceType.SelectedIndex = 0;
            btnSave.Text = "Simpan Paket";
            div_comment.InnerHtml = string.Empty;
            InitDetailTable();
            BindDetailGrid();
        }

        private DataTable GetDetailTable()
        {
            if (ViewState[ViewStateDetailTable] == null)
            {
                InitDetailTable();
            }

            return (DataTable)ViewState[ViewStateDetailTable];
        }

        private void InitDetailTable()
        {
            var table = new DataTable();
            table.Columns.Add("Seq", typeof(int));
            table.Columns.Add("DeviceTypeID", typeof(string));
            table.Columns.Add("DeviceTypeDesc", typeof(string));
            table.Columns.Add("Qty", typeof(decimal));
            table.Columns.Add("HppPrice", typeof(decimal));
            table.Columns.Add("SellingPrice", typeof(decimal));
            ViewState[ViewStateDetailTable] = table;
        }

        private void BindDetailGrid()
        {
            gvDetail.DataSource = GetDetailTable();
            gvDetail.DataBind();
        }

        private static void ReindexDetailSeq(DataTable table)
        {
            for (var i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i]["Seq"] = i + 1;
            }
        }

        private static DataTable ApplySearchFilter(DataTable sourceTable, string searchText)
        {
            if (sourceTable == null || string.IsNullOrWhiteSpace(searchText))
            {
                return sourceTable ?? new DataTable();
            }

            var escaped = searchText.Trim().Replace("'", "''").Replace("[", "[[]");
            var filterParts = new System.Collections.Generic.List<string>();

            foreach (var column in new[] { "PackageID", "PackageName", "DeviceTypeDesc" })
            {
                if (sourceTable.Columns.Contains(column))
                {
                    filterParts.Add("Convert(" + column + ", 'System.String') LIKE '%" + escaped + "%'");
                }
            }

            if (filterParts.Count == 0)
            {
                return sourceTable;
            }

            var dv = sourceTable.DefaultView;
            dv.RowFilter = string.Join(" OR ", filterParts.ToArray());
            return dv.ToTable();
        }

        private static void SetDropdownValue(DropDownList dropdown, string value)
        {
            var item = dropdown.Items.FindByValue(value);
            if (item != null)
            {
                dropdown.ClearSelection();
                item.Selected = true;
            }
        }

        private static bool TryParsePositiveDecimal(string text, out decimal value)
        {
            value = 0m;
            if (!decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                && !decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out value))
            {
                return false;
            }

            return value > 0;
        }

        private static bool TryParseNonNegativeDecimal(string text, out decimal value)
        {
            value = 0m;
            if (!decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                && !decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out value))
            {
                return false;
            }

            return value >= 0;
        }

        private static decimal ParseDecimal(string text)
        {
            decimal value;
            if (decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            return decimal.Parse(text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture);
        }

        private static object GetNullableString(string value)
        {
            var trimmed = (value ?? string.Empty).Trim();
            return string.IsNullOrEmpty(trimmed) ? (object)DBNull.Value : trimmed;
        }

        private string GetUserId()
        {
            return Session["ClsTypeUserID"] == null ? string.Empty : Session["ClsTypeUserID"].ToString().Trim();
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

        private static string XmlAttributeEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        private void RegisterAllGridAsyncControls()
        {
            foreach (GridViewRow row in gvDetail.Rows)
            {
                RegisterGridLinkButton(row, "btnRemoveDetail");
            }

            foreach (GridViewRow row in gvPackage.Rows)
            {
                RegisterGridLinkButton(row, "btnEdit");
                RegisterGridLinkButton(row, "btnDelete");
            }
        }

        private void RegisterAsyncPostBackControls()
        {
            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager == null)
            {
                return;
            }

            scriptManager.RegisterAsyncPostBackControl(btnAddDetail);
            scriptManager.RegisterAsyncPostBackControl(btnClear);
            scriptManager.RegisterAsyncPostBackControl(btnSave);
            scriptManager.RegisterAsyncPostBackControl(btnSearch);
        }

        private void RegisterGridLinkButton(GridViewRow row, string controlId)
        {
            if (row == null || row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            var button = row.FindControl(controlId) as LinkButton;
            var scriptManager = ScriptManager.GetCurrent(Page);
            if (button != null && scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(button);
            }
        }

        private int ResolveDetailSeq(GridViewCommandEventArgs e)
        {
            var arg = Convert.ToString(e.CommandArgument);
            if (!string.IsNullOrWhiteSpace(arg))
            {
                return Convert.ToInt32(arg);
            }

            var source = e.CommandSource as Control;
            if (source != null)
            {
                var row = source.NamingContainer as GridViewRow;
                if (row != null && row.RowIndex >= 0 && gvDetail.DataKeys.Count > row.RowIndex)
                {
                    return Convert.ToInt32(gvDetail.DataKeys[row.RowIndex].Value);
                }
            }

            throw new InvalidOperationException("Seq item detail tidak valid.");
        }

        private string ResolvePackageId(GridViewCommandEventArgs e)
        {
            var source = e.CommandSource as Control;
            if (source != null)
            {
                var row = source.NamingContainer as GridViewRow;
                if (row != null && row.RowIndex >= 0 && gvPackage.DataKeys.Count > row.RowIndex)
                {
                    return Convert.ToString(gvPackage.DataKeys[row.RowIndex].Value).Trim();
                }
            }

            return Convert.ToString(e.CommandArgument).Trim();
        }

        private static string GetSqlErrorMessage(Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                if (sqlEx.Number >= 50000)
                {
                    return sqlEx.Message;
                }

                if (sqlEx.Number == 547)
                {
                    return "Paket MDVR tidak dapat dihapus karena sudah digunakan.";
                }
            }

            return ex.Message;
        }

        private void ShowSuccess(string message)
        {
            div_comment.InnerHtml = "<div class='mdvr-alert mdvr-alert-success' role='alert'><i class='fa fa-check-circle' style='margin-right:8px;'></i>" + HttpUtility.HtmlEncode(message) + "</div>";
        }

        private void ShowError(string message)
        {
            div_comment.InnerHtml = "<div class='mdvr-alert mdvr-alert-danger' role='alert'><i class='fa fa-exclamation-circle' style='margin-right:8px;'></i>" + HttpUtility.HtmlEncode(message) + "</div>";
        }
    }
}
