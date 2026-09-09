using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_mirror_realtime_archive : Page
    {
        private const string MenuId = "MNUVEMIRRORARC";
        private const string ActiveTable = "dbo.vehicle_mirror_realtime";
        private const string ArchiveTable = "dbo.vehicle_mirror_realtime_archive";
        private const string SessionGridKey = "RecListVehicleMirrorRealtimeArchive";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (Session["ClsTypeAccessMenu"] == null ||
                    !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains(MenuId))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            ClearForm();
                            SetGridColumnVisibility(false);
                        }
                        else
                        {
                            Response.Redirect("login.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch
            {
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = string.Empty;
                BindGrid();
            }
            catch (Exception ex)
            {
                ShowMessage("Load failed: " + ex.Message, false);
            }
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> vehicleIds;
                int? companyId;
                string resolveError;
                if (!TryResolveCompanyVehicles(out vehicleIds, out companyId, out resolveError))
                {
                    ShowMessage(resolveError, false);
                    return;
                }

                string userId = GetCurrentUserId();
                int moved = ArchiveVehicles(vehicleIds, companyId, userId);
                rblViewMode.SelectedValue = "archived";
                BindGrid();
                ShowMessage("Archived " + moved + " row(s) for whole company.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Archive failed: " + ex.Message, false);
            }
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                int? companyId;
                string resolveError;
                if (!TryResolveCompanyId(out companyId, out resolveError))
                {
                    ShowMessage(resolveError, false);
                    return;
                }

                int moved = RestoreVehicles(null, companyId, byCompanyOnly: true);
                rblViewMode.SelectedValue = "active";
                BindGrid();
                ShowMessage("Restored " + moved + " row(s) for whole company.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Restore failed: " + ex.Message, false);
            }
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string vehicleId = Convert.ToString(e.CommandArgument).Trim();
                if (string.IsNullOrEmpty(vehicleId))
                {
                    return;
                }

                int? companyId = null;
                int parsedCompany;
                if (int.TryParse((txtCompID.Value ?? string.Empty).Trim(), out parsedCompany) && parsedCompany > 0)
                {
                    companyId = parsedCompany;
                }

                var vehicleIds = new List<string> { vehicleId };
                string userId = GetCurrentUserId();

                if (string.Equals(e.CommandName, "ARCHIVE_VEHICLE", StringComparison.OrdinalIgnoreCase))
                {
                    int moved = ArchiveVehicles(vehicleIds, companyId, userId);
                    ShowMessage("Archived " + moved + " row(s) for vehicle " + vehicleId + ".", true);
                    BindGrid();
                    return;
                }

                if (string.Equals(e.CommandName, "RESTORE_VEHICLE", StringComparison.OrdinalIgnoreCase))
                {
                    int moved = RestoreVehicles(vehicleIds, companyId, byCompanyOnly: false);
                    ShowMessage("Restored " + moved + " row(s) for vehicle " + vehicleId + ".", true);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Action failed: " + ex.Message, false);
            }
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            bool archived = string.Equals(rblViewMode.SelectedValue, "archived", StringComparison.OrdinalIgnoreCase);
            LinkButton btnArchive = e.Row.FindControl("btnRowArchive") as LinkButton;
            LinkButton btnRestore = e.Row.FindControl("btnRowRestore") as LinkButton;
            if (btnArchive != null)
            {
                btnArchive.Visible = !archived;
            }

            if (btnRestore != null)
            {
                btnRestore.Visible = archived;
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            DataSet cached = Session[SessionGridKey] as DataSet;
            if (cached != null && cached.Tables.Count > 0)
            {
                gvData.DataSource = cached.Tables[0];
                gvData.DataBind();
            }
            else
            {
                BindGrid();
            }
        }

        private string GetCurrentUserId()
        {
            return Session["ClsTypeUserID"] != null
                ? Convert.ToString(Session["ClsTypeUserID"]).Trim()
                : string.Empty;
        }

        private void BindGrid()
        {
            bool archived = string.Equals(rblViewMode.SelectedValue, "archived", StringComparison.OrdinalIgnoreCase);
            litGridTitle.Text = archived ? "Archived Mirror Rows" : "Active Mirror Rows";
            SetGridColumnVisibility(archived);

            List<string> vehicleIds;
            int? companyId;
            string resolveError;
            if (!TryResolveCompanyVehicles(out vehicleIds, out companyId, out resolveError, forArchiveTable: archived))
            {
                gvData.DataSource = null;
                gvData.DataBind();
                Session[SessionGridKey] = null;
                lblRowCount.Text = string.Empty;
                ShowMessage(resolveError, false);
                return;
            }

            DataTable table = archived
                ? LoadArchivedRows(companyId.Value)
                : LoadActiveRows(vehicleIds);

            gvData.PageIndex = 0;
            gvData.DataSource = table;
            gvData.DataBind();

            var ds = new DataSet();
            ds.Tables.Add(table.Copy());
            Session[SessionGridKey] = ds;

            lblRowCount.Text = table.Rows.Count + " row(s)";
        }

        private bool TryResolveCompanyId(out int? companyId, out string error)
        {
            companyId = null;
            error = null;

            string companyRaw = (txtCompID.Value ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(companyRaw))
            {
                error = "Pilih company terlebih dahulu.";
                return false;
            }

            int parsedCompany;
            if (!int.TryParse(companyRaw, out parsedCompany) || parsedCompany <= 0)
            {
                error = "Company ID tidak valid.";
                return false;
            }

            companyId = parsedCompany;
            return true;
        }

        private bool TryResolveCompanyVehicles(
            out List<string> vehicleIds,
            out int? companyId,
            out string error,
            bool forArchiveTable = false)
        {
            vehicleIds = new List<string>();
            if (!TryResolveCompanyId(out companyId, out error))
            {
                return false;
            }

            if (forArchiveTable)
            {
                // Archived list filters by company_id snapshot; vehicle list optional.
                vehicleIds = LoadVehicleIdsByCompany(companyId.Value);
                return true;
            }

            vehicleIds = LoadVehicleIdsByCompany(companyId.Value);
            if (vehicleIds.Count == 0)
            {
                error = "Tidak ada vehicle untuk company_id " + companyId.Value + ".";
                return false;
            }

            return true;
        }

        private List<string> LoadVehicleIdsByCompany(int companyId)
        {
            var result = new List<string>();
            string connectionString = Session["ClsTypeDBConnStringSQL"] != null
                ? Session["ClsTypeDBConnStringSQL"].ToString()
                : null;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Session DB connection tidak ditemukan.");
            }

            string sErr = string.Empty;
            Recordset rec = new Recordset();
            rec.Open("sp_get_list_vehicle_by_company " + companyId, connectionString, ref sErr);
            if (!string.IsNullOrEmpty(sErr))
            {
                throw new InvalidOperationException("Gagal load vehicle by company: " + sErr);
            }

            if (rec.RecData == null || rec.RecData.Tables.Count == 0)
            {
                return result;
            }

            DataTable table = rec.RecData.Tables[0];
            string vehicleCol = ResolveColumnName(table, "vehicle_id", "VehicleID", "VehicleId");
            foreach (DataRow row in table.Rows)
            {
                string id = vehicleCol != null
                    ? Convert.ToString(row[vehicleCol]).Trim()
                    : (row.ItemArray.Length > 0 ? Convert.ToString(row[0]).Trim() : string.Empty);

                if (!string.IsNullOrEmpty(id) &&
                    !result.Exists(x => x.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(id);
                }
            }

            return result;
        }

        private static string ResolveColumnName(DataTable table, params string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                if (table.Columns.Contains(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private DataTable LoadActiveRows(List<string> vehicleIds)
        {
            EnsureTempVehicleIds(vehicleIds);

            string sql = @"
SELECT a.autoid,
       a.vehicle_id,
       a.mirror_server,
       a.id_vendor,
       a.imei_vendor,
       a.is_enabled,
       a.usrupd,
       a.dtmupd,
       a.id_ruas,
       a.id_type_kendaraan,
       a.notes,
       CAST(NULL AS INT) AS company_id,
       CAST(NULL AS DATETIME) AS archived_at,
       CAST(NULL AS VARCHAR(100)) AS archived_by
FROM " + ActiveTable + @" a WITH (NOLOCK)
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = a.vehicle_id
ORDER BY a.vehicle_id, a.mirror_server";

            return ExecuteGpsbDataTableWithTempIds(sql, vehicleIds);
        }

        private DataTable LoadArchivedRows(int companyId)
        {
            string sqlByCompany = @"
SELECT a.autoid,
       a.vehicle_id,
       a.mirror_server,
       a.id_vendor,
       a.imei_vendor,
       a.is_enabled,
       a.usrupd,
       a.dtmupd,
       a.id_ruas,
       a.id_type_kendaraan,
       a.notes,
       a.company_id,
       a.archived_at,
       a.archived_by
FROM " + ArchiveTable + @" a WITH (NOLOCK)
WHERE a.company_id = @company_id
ORDER BY a.archived_at DESC, a.vehicle_id, a.mirror_server";

            return ExecuteGpsbDataTable(
                sqlByCompany,
                new SqlParameter("@company_id", SqlDbType.Int) { Value = companyId });
        }

        private int ArchiveVehicles(List<string> vehicleIds, int? companyId, string archivedBy)
        {
            EnsureArchiveTableExists();

            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    CreateTempVehicleIds(conn, tran, vehicleIds);

                    const string conflictSql = @"
SELECT COUNT(1)
FROM " + ArchiveTable + @" a WITH (UPDLOCK, HOLDLOCK)
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = a.vehicle_id";

                    using (SqlCommand conflictCmd = new SqlCommand(conflictSql, conn, tran))
                    {
                        int existing = Convert.ToInt32(conflictCmd.ExecuteScalar());
                        if (existing > 0)
                        {
                            throw new InvalidOperationException(
                                existing + " archived row(s) already exist for selected vehicle(s). Restore or clean archive first.");
                        }
                    }

                    string insertSql = @"
INSERT INTO " + ArchiveTable + @"
(
    autoid, vehicle_id, mirror_server, id_vendor, imei_vendor, is_enabled,
    usrupd, dtmupd, id_ruas, id_type_kendaraan, notes,
    archived_at, archived_by, company_id
)
SELECT a.autoid, a.vehicle_id, a.mirror_server, a.id_vendor, a.imei_vendor, a.is_enabled,
       a.usrupd, a.dtmupd, a.id_ruas, a.id_type_kendaraan, a.notes,
       GETDATE(), @archived_by, @company_id
FROM " + ActiveTable + @" a
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = a.vehicle_id";

                    int inserted;
                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn, tran))
                    {
                        insertCmd.CommandTimeout = 120;
                        insertCmd.Parameters.Add(new SqlParameter("@archived_by", SqlDbType.VarChar, 100)
                        {
                            Value = string.IsNullOrWhiteSpace(archivedBy) ? (object)DBNull.Value : archivedBy
                        });
                        insertCmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int)
                        {
                            Value = (object)companyId ?? DBNull.Value
                        });
                        inserted = insertCmd.ExecuteNonQuery();
                    }

                    if (inserted == 0)
                    {
                        throw new InvalidOperationException("Tidak ada active row yang cocok untuk di-archive.");
                    }

                    string deleteSql = @"
DELETE a
FROM " + ActiveTable + @" a
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = a.vehicle_id";

                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn, tran))
                    {
                        deleteCmd.CommandTimeout = 120;
                        deleteCmd.ExecuteNonQuery();
                    }

                    DropTempVehicleIds(conn, tran);
                    tran.Commit();
                    return inserted;
                }
            }
        }

        private int RestoreVehicles(List<string> vehicleIds, int? companyId, bool byCompanyOnly)
        {
            EnsureArchiveTableExists();

            if (byCompanyOnly && !companyId.HasValue)
            {
                throw new InvalidOperationException("Company ID wajib untuk restore whole company.");
            }

            if (!byCompanyOnly)
            {
                EnsureTempVehicleIds(vehicleIds);
            }

            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    if (!byCompanyOnly)
                    {
                        CreateTempVehicleIds(conn, tran, vehicleIds);
                    }

                    string conflictSql = byCompanyOnly
                        ? @"
SELECT TOP 20 COALESCE(a1.vehicle_id, a2.vehicle_id) AS vehicle_id,
              COALESCE(a1.mirror_server, a2.mirror_server) AS mirror_server,
              COALESCE(a1.autoid, a2.autoid) AS autoid
FROM " + ArchiveTable + @" ar
LEFT JOIN " + ActiveTable + @" a1
    ON a1.vehicle_id = ar.vehicle_id AND a1.mirror_server = ar.mirror_server
LEFT JOIN " + ActiveTable + @" a2
    ON a2.autoid = ar.autoid
WHERE ar.company_id = @company_id
  AND (a1.autoid IS NOT NULL OR a2.autoid IS NOT NULL)"
                        : @"
SELECT TOP 20 COALESCE(a1.vehicle_id, a2.vehicle_id) AS vehicle_id,
              COALESCE(a1.mirror_server, a2.mirror_server) AS mirror_server,
              COALESCE(a1.autoid, a2.autoid) AS autoid
FROM " + ArchiveTable + @" ar
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = ar.vehicle_id
LEFT JOIN " + ActiveTable + @" a1
    ON a1.vehicle_id = ar.vehicle_id AND a1.mirror_server = ar.mirror_server
LEFT JOIN " + ActiveTable + @" a2
    ON a2.autoid = ar.autoid
WHERE a1.autoid IS NOT NULL OR a2.autoid IS NOT NULL";

                    using (SqlCommand conflictCmd = new SqlCommand(conflictSql, conn, tran))
                    {
                        if (byCompanyOnly)
                        {
                            conflictCmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int)
                            {
                                Value = companyId.Value
                            });
                        }

                        var conflicts = new StringBuilder();
                        using (SqlDataReader reader = conflictCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (conflicts.Length > 0)
                                {
                                    conflicts.Append("; ");
                                }

                                conflicts.AppendFormat(
                                    "{0}/{1} (autoid {2})",
                                    Convert.ToString(reader["vehicle_id"]),
                                    Convert.ToString(reader["mirror_server"]),
                                    Convert.ToString(reader["autoid"]));
                            }
                        }

                        if (conflicts.Length > 0)
                        {
                            throw new InvalidOperationException(
                                "Restore blocked — active table already has conflicting row(s): " + conflicts);
                        }
                    }

                    string selectInsertSql = byCompanyOnly
                        ? @"
INSERT INTO " + ActiveTable + @"
(
    autoid, vehicle_id, mirror_server, id_vendor, imei_vendor, is_enabled,
    usrupd, dtmupd, id_ruas, id_type_kendaraan, notes
)
SELECT ar.autoid, ar.vehicle_id, ar.mirror_server, ar.id_vendor, ar.imei_vendor, ar.is_enabled,
       ar.usrupd, ar.dtmupd, ar.id_ruas, ar.id_type_kendaraan, ar.notes
FROM " + ArchiveTable + @" ar
WHERE ar.company_id = @company_id"
                        : @"
INSERT INTO " + ActiveTable + @"
(
    autoid, vehicle_id, mirror_server, id_vendor, imei_vendor, is_enabled,
    usrupd, dtmupd, id_ruas, id_type_kendaraan, notes
)
SELECT ar.autoid, ar.vehicle_id, ar.mirror_server, ar.id_vendor, ar.imei_vendor, ar.is_enabled,
       ar.usrupd, ar.dtmupd, ar.id_ruas, ar.id_type_kendaraan, ar.notes
FROM " + ArchiveTable + @" ar
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = ar.vehicle_id";

                    int inserted;
                    using (SqlCommand identityOn = new SqlCommand(
                        "SET IDENTITY_INSERT " + ActiveTable + " ON;", conn, tran))
                    {
                        identityOn.ExecuteNonQuery();
                    }

                    try
                    {
                        using (SqlCommand insertCmd = new SqlCommand(selectInsertSql, conn, tran))
                        {
                            insertCmd.CommandTimeout = 120;
                            if (byCompanyOnly)
                            {
                                insertCmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int)
                                {
                                    Value = companyId.Value
                                });
                            }

                            inserted = insertCmd.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        using (SqlCommand identityOff = new SqlCommand(
                            "SET IDENTITY_INSERT " + ActiveTable + " OFF;", conn, tran))
                        {
                            identityOff.ExecuteNonQuery();
                        }
                    }

                    if (inserted <= 0)
                    {
                        throw new InvalidOperationException("Tidak ada archived row yang cocok untuk di-restore.");
                    }

                    string deleteSql = byCompanyOnly
                        ? "DELETE FROM " + ArchiveTable + " WHERE company_id = @company_id"
                        : @"
DELETE ar
FROM " + ArchiveTable + @" ar
INNER JOIN #vmr_vehicle_ids v ON v.vehicle_id = ar.vehicle_id";

                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn, tran))
                    {
                        deleteCmd.CommandTimeout = 120;
                        if (byCompanyOnly)
                        {
                            deleteCmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int)
                            {
                                Value = companyId.Value
                            });
                        }

                        deleteCmd.ExecuteNonQuery();
                    }

                    if (!byCompanyOnly)
                    {
                        DropTempVehicleIds(conn, tran);
                    }

                    tran.Commit();
                    return inserted;
                }
            }
        }

        private void EnsureArchiveTableExists()
        {
            const string sql = @"
IF OBJECT_ID('dbo.vehicle_mirror_realtime_archive', 'U') IS NULL
BEGIN
    RAISERROR('Table dbo.vehicle_mirror_realtime_archive belum ada. Jalankan sql/vehicle_mirror_realtime_archive.sql di GPSB.', 16, 1);
END";

            ExecuteGpsbNonQuery(sql);
        }

        private static void EnsureTempVehicleIds(List<string> vehicleIds)
        {
            if (vehicleIds == null || vehicleIds.Count == 0)
            {
                throw new InvalidOperationException("Vehicle ID list kosong.");
            }
        }

        private static void CreateTempVehicleIds(SqlConnection conn, SqlTransaction tran, List<string> vehicleIds)
        {
            EnsureTempVehicleIds(vehicleIds);

            using (SqlCommand drop = new SqlCommand(
                "IF OBJECT_ID('tempdb..#vmr_vehicle_ids') IS NOT NULL DROP TABLE #vmr_vehicle_ids;", conn, tran))
            {
                drop.ExecuteNonQuery();
            }

            using (SqlCommand create = new SqlCommand(
                "CREATE TABLE #vmr_vehicle_ids (vehicle_id VARCHAR(20) NOT NULL PRIMARY KEY);", conn, tran))
            {
                create.ExecuteNonQuery();
            }

            const int batchSize = 200;
            for (int offset = 0; offset < vehicleIds.Count; offset += batchSize)
            {
                var batch = vehicleIds.Skip(offset).Take(batchSize).ToList();
                var sql = new StringBuilder();
                sql.Append("INSERT INTO #vmr_vehicle_ids (vehicle_id) VALUES ");
                using (SqlCommand insert = new SqlCommand())
                {
                    insert.Connection = conn;
                    insert.Transaction = tran;
                    for (int i = 0; i < batch.Count; i++)
                    {
                        if (i > 0)
                        {
                            sql.Append(',');
                        }

                        string paramName = "@v" + i;
                        sql.Append('(').Append(paramName).Append(')');
                        string value = batch[i];
                        if (value.Length > 20)
                        {
                            value = value.Substring(0, 20);
                        }

                        insert.Parameters.Add(new SqlParameter(paramName, SqlDbType.VarChar, 20) { Value = value });
                    }

                    insert.CommandText = sql.ToString();
                    insert.ExecuteNonQuery();
                }
            }
        }

        private static void DropTempVehicleIds(SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand drop = new SqlCommand(
                "IF OBJECT_ID('tempdb..#vmr_vehicle_ids') IS NOT NULL DROP TABLE #vmr_vehicle_ids;", conn, tran))
            {
                drop.ExecuteNonQuery();
            }
        }

        private DataTable ExecuteGpsbDataTableWithTempIds(string sql, List<string> vehicleIds)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    CreateTempVehicleIds(conn, tran, vehicleIds);

                    using (SqlCommand cmd = new SqlCommand(sql, conn, tran))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 120;
                        var table = new DataTable();
                        da.Fill(table);
                        DropTempVehicleIds(conn, tran);
                        tran.Commit();
                        return table;
                    }
                }
            }
        }

        private DataTable ExecuteGpsbDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                var table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        private int ExecuteGpsbNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(GetGpsbConnectionString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private string GetGpsbConnectionString()
        {
            try
            {
                ConnectionStringSettings gpsb = ConfigurationManager.ConnectionStrings["GPSBDBClient"];
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
                ConnectionStringSettings gpsbOle = ConfigurationManager.ConnectionStrings["GPSBDB"];
                if (gpsbOle != null && !string.IsNullOrWhiteSpace(gpsbOle.ConnectionString))
                {
                    return ConvertOleDbToSqlConnectionString(gpsbOle.ConnectionString.Trim());
                }
            }
            catch
            {
            }

            throw new InvalidOperationException("GPSB connection string (GPSBDBClient/GPSBDB) tidak ditemukan.");
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

        private void SetGridColumnVisibility(bool archived)
        {
            // Columns: 0 autoid, 1 vehicle_id, 2 mirror_server, 3 id_vendor, 4 imei_vendor,
            // 5 is_enabled, 6 dtmupd, 7 notes, 8 company_id, 9 archived_at, 10 archived_by, 11 Action
            if (gvData.Columns.Count >= 11)
            {
                gvData.Columns[8].Visible = archived;
                gvData.Columns[9].Visible = archived;
                gvData.Columns[10].Visible = archived;
            }
        }

        private void ClearForm()
        {
            txtCompID.Value = string.Empty;
            txtCompanyName.Text = string.Empty;
            rblViewMode.SelectedValue = "active";
            litGridTitle.Text = "Active Mirror Rows";
            lblRowCount.Text = string.Empty;
            gvData.DataSource = null;
            gvData.DataBind();
            Session[SessionGridKey] = null;
            SetGridColumnVisibility(false);
            div_comment.InnerHtml = string.Empty;
        }

        private void ShowMessage(string msg, bool success)
        {
            string alertClass = success ? "alert-success" : "alert-danger";
            string title = success ? "Success!" : "Failed!";
            div_comment.InnerHtml =
                "<div class='alert " + alertClass + "' role='alert'>" +
                "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
                "<span aria-hidden='true'>&times;</span></button>" +
                "<strong>" + title + "</strong> " + HttpUtility.HtmlEncode(msg) +
                "</div>";
        }
    }
}
