using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class server_mirroring : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var clType = new ClsType();
                var access = (Session["ClsTypeAccessMenu"] ?? "").ToString().ToUpperInvariant();
                if (!access.Contains("MNUSERVERMIRRORING") && !access.Contains("MNUINSTALLMIRRORING"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch
            {
                Response.Redirect("login.aspx");
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetPageData()
        {
            try
            {
                if (!IsAuthorized()) return Fail("Akses ditolak.");

                var companies = LoadCompanies();
                var servers = LoadServers();
                var rows = LoadConfigurations();
                var companyActive = companies.Count;
                var vehicleActive = CountActiveVehicles();
                var mirroredCompanies = rows.Select(r => r.CompanyId).Distinct(StringComparer.OrdinalIgnoreCase).Count();
                var mirroredVehicles = rows.Select(r => r.VehicleId).Distinct(StringComparer.OrdinalIgnoreCase).Count();
                var activeServers = rows
                    .Where(r => string.Equals(r.Status, "active", StringComparison.OrdinalIgnoreCase))
                    .SelectMany(r => r.MirrorServers ?? new string[0])
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count();

                return new
                {
                    success = true,
                    servers,
                    companies = companies.Select(c => new { id = c.Id, name = c.Name }).ToList(),
                    rows = rows.Select(ToClientRow).ToList(),
                    kpis = new
                    {
                        companyMirrored = mirroredCompanies,
                        companyActive,
                        vehicleMirrored = mirroredVehicles,
                        vehicleActive,
                        serverActive = activeServers,
                        serverTotal = servers.Count
                    }
                };
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetVehicles(string companyId)
        {
            try
            {
                if (!IsAuthorized()) return Fail("Akses ditolak.");
                if (string.IsNullOrWhiteSpace(companyId)) return new { success = true, items = new object[0] };

                var items = LoadVehicles(companyId.Trim());
                return new
                {
                    success = true,
                    items = items.Select(v => new { id = v.VehicleId, plate = v.Plate, tvaId = v.TvaId }).ToList()
                };
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        public static object SaveConfiguration(string payload)
        {
            try
            {
                if (!IsAuthorized()) return Fail("Akses ditolak.");

                var form = JsonConvert.DeserializeObject<MirrorFormValue>(payload ?? "");
                if (form == null || string.IsNullOrWhiteSpace(form.companyId))
                    return Fail("Pilih company, vehicle, dan server mirror.");
                if (form.mirrorServers == null || form.mirrorServers.Length == 0)
                    return Fail("Pilih company, vehicle, dan server mirror.");
                if (!form.allVehicles && (form.vehicleIds == null || form.vehicleIds.Length == 0))
                    return Fail("Pilih company, vehicle, dan server mirror.");

                var companies = LoadCompanies();
                var company = companies.FirstOrDefault(c => string.Equals(c.Id, form.companyId, StringComparison.OrdinalIgnoreCase));
                if (company == null) return Fail("Company tidak ditemukan.");

                var vehicles = LoadVehicles(form.companyId);
                if (vehicles.Count == 0) return Fail("Company ini belum memiliki vehicle.");

                List<VehicleItem> selected;
                if (form.allVehicles)
                {
                    selected = vehicles;
                }
                else
                {
                    var wanted = new HashSet<string>(form.vehicleIds.Where(v => !string.IsNullOrWhiteSpace(v)), StringComparer.OrdinalIgnoreCase);
                    selected = vehicles.Where(v => wanted.Contains(v.VehicleId) || wanted.Contains(v.Plate)).ToList();
                }

                if (selected.Count == 0) return Fail("Vehicle tidak ditemukan pada company terpilih.");
                selected = selected
                    .GroupBy(v => v.VehicleId, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .ToList();

                var servers = form.mirrorServers
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                if (servers.Length == 0) return Fail("Pilih company, vehicle, dan server mirror.");

                // Save/update/delete ke GPSB.dbo.vehicle_mirror_realtime ditunda dulu.
                // var userId = (HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                // var userName = (HttpContext.Current.Session["ClsTypeUserFullName"] ?? userId).ToString();
                // var now = DateTime.Now;
                // var isEdit = !string.IsNullOrWhiteSpace(form.id);
                // var connSql = GetSqlClientConnectionString(GetDbConn());
                // using (var conn = new SqlConnection(connSql))
                // {
                //     conn.Open();
                //     using (var tx = conn.BeginTransaction())
                //     {
                //         foreach (var vehicle in selected)
                //         {
                //             UpsertRow(conn, tx, form.id, isEdit && selected.Count == 1, company, vehicle, servers, userName, now);
                //             SaveGpsbMirroring(conn, tx, vehicle.VehicleId, servers, userId);
                //         }
                //         tx.Commit();
                //     }
                //     foreach (var vehicle in selected)
                //     {
                //         SyncLegacyMirror(conn, null, vehicle.TvaId, servers, userId, now);
                //     }
                // }

                return new
                {
                    success = false,
                    message = "Simpan mirroring sementara dinonaktifkan. Data hanya dibaca dari GPSB."
                };
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateStatus(string id, string status)
        {
            try
            {
                if (!IsAuthorized()) return Fail("Akses ditolak.");
                if (string.IsNullOrWhiteSpace(id)) return Fail("Data tidak ditemukan.");

                var next = string.Equals(status, "paused", StringComparison.OrdinalIgnoreCase) ? "paused" : "active";

                // Update status (jeda/aktif) ditunda dulu.
                // var userName = (HttpContext.Current.Session["ClsTypeUserFullName"] ?? HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                // var userId = (HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                // var connSql = GetSqlClientConnectionString(GetDbConn());
                // using (var conn = new SqlConnection(connSql))
                // using (var cmd = new SqlCommand(@"
                // UPDATE GPSB.dbo.vehicle_mirror_realtime
                // SET is_enabled = @Enabled, usrupd = @UsrUpd, dtmupd = GETDATE()
                // WHERE vehicle_id = @VehicleId;", conn))
                // {
                //     cmd.Parameters.AddWithValue("@Enabled", next == "active" ? 1 : 0);
                //     cmd.Parameters.AddWithValue("@UsrUpd", userName);
                //     cmd.Parameters.AddWithValue("@VehicleId", id);
                //     conn.Open();
                //     cmd.ExecuteNonQuery();
                // }

                return new
                {
                    success = false,
                    message = next == "active"
                        ? "Aktifkan mirroring sementara dinonaktifkan."
                        : "Jeda mirroring sementara dinonaktifkan."
                };
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /*
        private static void UpsertRow(SqlConnection conn, SqlTransaction tx, string editId, bool useEditId, CompanyItem company, VehicleItem vehicle, string[] servers, string userName, DateTime now)
        {
            var id = useEditId && !string.IsNullOrWhiteSpace(editId) ? editId : Guid.NewGuid().ToString();
            const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.trx_server_mirroring WHERE CompanyId = @CompanyId AND VehicleId = @VehicleId)
BEGIN
    UPDATE dbo.trx_server_mirroring
    SET CompanyName = @CompanyName,
        VehiclePlate = @VehiclePlate,
        TvaID = @TvaID,
        MirrorServers = @MirrorServers,
        Status = 'active',
        UsrUpd = @UsrUpd,
        DtmUpd = @DtmUpd
    WHERE CompanyId = @CompanyId AND VehicleId = @VehicleId;
END
ELSE
BEGIN
    INSERT INTO dbo.trx_server_mirroring
        (Id, CompanyId, CompanyName, VehicleId, VehiclePlate, TvaID, MirrorServers, Status, UsrUpd, DtmUpd)
    VALUES
        (@Id, @CompanyId, @CompanyName, @VehicleId, @VehiclePlate, @TvaID, @MirrorServers, 'active', @UsrUpd, @DtmUpd);
END";
            using (var cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@CompanyId", company.Id);
                cmd.Parameters.AddWithValue("@CompanyName", (object)company.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VehicleId", vehicle.VehicleId);
                cmd.Parameters.AddWithValue("@VehiclePlate", (object)vehicle.Plate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TvaID", string.IsNullOrWhiteSpace(vehicle.TvaId) ? (object)DBNull.Value : vehicle.TvaId);
                cmd.Parameters.AddWithValue("@MirrorServers", string.Join(",", servers));
                cmd.Parameters.AddWithValue("@UsrUpd", userName ?? "");
                cmd.Parameters.AddWithValue("@DtmUpd", now);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SyncLegacyMirror(SqlConnection conn, SqlTransaction tx, string tvaId, string[] servers, string userId, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(tvaId) || servers == null || servers.Length == 0) return;
            try
            {
                using (var del = new SqlCommand("DELETE FROM dbo.trx_vehicle_mirror_realtime WHERE TvaID = @TvaID", conn, tx))
                {
                    del.Parameters.AddWithValue("@TvaID", tvaId);
                    del.ExecuteNonQuery();
                }
                foreach (var server in servers)
                {
                    using (var ins = new SqlCommand(@"
INSERT INTO dbo.trx_vehicle_mirror_realtime (TvaID, MirrorServer, Status, UsrUpd, DtmUpd)
VALUES (@TvaID, @MirrorServer, 'RG', @UsrUpd, @DtmUpd);", conn, tx))
                    {
                        ins.Parameters.AddWithValue("@TvaID", tvaId);
                        ins.Parameters.AddWithValue("@MirrorServer", server);
                        ins.Parameters.AddWithValue("@UsrUpd", userId ?? "");
                        ins.Parameters.AddWithValue("@DtmUpd", now);
                        ins.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
            }
        }

        private static void PauseLegacyMirror(SqlConnection conn, string tvaId)
        {
            if (string.IsNullOrWhiteSpace(tvaId)) return;
            try
            {
                using (var cmd = new SqlCommand("UPDATE dbo.trx_vehicle_mirror_realtime SET Status = 'DJ' WHERE TvaID = @TvaID", conn))
                {
                    cmd.Parameters.AddWithValue("@TvaID", tvaId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        private static void SaveGpsbMirroring(SqlConnection conn, SqlTransaction tx, string vehicleId, string[] servers, string userId)
        {
            using (var del = new SqlCommand("usp_delete_mirroring", conn, tx))
            {
                del.CommandType = CommandType.StoredProcedure;
                del.Parameters.AddWithValue("@vehicle_id", vehicleId);
                del.ExecuteNonQuery();
            }
            foreach (var server in servers)
            {
                if (string.Equals(server, "pantos", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(server, "jasamarga", StringComparison.OrdinalIgnoreCase))
                    continue;
                using (var ins = new SqlCommand("usp_insert_mirroring", conn, tx))
                {
                    ins.CommandType = CommandType.StoredProcedure;
                    ins.Parameters.AddWithValue("@mirror_server", server);
                    ins.Parameters.AddWithValue("@vehicle_id", vehicleId);
                    ins.Parameters.AddWithValue("@mirror_vendor_id", "");
                    ins.ExecuteNonQuery();
                }
            }
        }
        */

        private static List<string> LoadServers()
        {
            var list = new List<string>();
            try
            {
                var rec = new Recordset();
                rec.Open("sp_list_mirror_realtime_server ''", GetDbConn());
                if (rec.RecData != null && rec.RecData.Tables.Count > 0)
                {
                    var table = rec.RecData.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        var value = GetColumn(row, "Value", "Text", "id", "Id", "MirrorServer", "mirror_server");
                        if (string.IsNullOrWhiteSpace(value) && table.Columns.Count > 0 && row[0] != DBNull.Value)
                            value = Convert.ToString(row[0]);
                        if (string.IsNullOrWhiteSpace(value)) continue;
                        value = value.Trim();
                        if (string.Equals(value, "[Select]", StringComparison.OrdinalIgnoreCase)) continue;
                        if (!list.Exists(s => string.Equals(s, value, StringComparison.OrdinalIgnoreCase)))
                            list.Add(value);
                    }
                }
            }
            catch
            {
            }

            return list;
        }

        private static List<CompanyItem> LoadCompanies()
        {
            var list = new List<CompanyItem>();
            var connSql = GetSqlClientConnectionString(GetDbConn());
            using (var conn = new SqlConnection(connSql))
            using (var cmd = new SqlCommand(@"
SELECT CustID, FullName
FROM dbo.mst_customer
WHERE Status = 'RG'
ORDER BY FullName;", conn))
            {
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new CompanyItem
                        {
                            Id = Convert.ToString(r["CustID"]) ?? "",
                            Name = Convert.ToString(r["FullName"]) ?? ""
                        });
                    }
                }
            }
            return list;
        }

        private static List<VehicleItem> LoadVehicles(string companyId)
        {
            var list = new List<VehicleItem>();
            if (string.IsNullOrWhiteSpace(companyId)) return list;

            var rec = new Recordset();
            rec.Open("sp_list_vehicle_assign_customer_selected '" + companyId.Replace("'", "''") + "',''", GetDbConn());
            if (rec.RecData != null && rec.RecData.Tables.Count > 0)
            {
                var table = rec.RecData.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    var vehicleId = GetColumn(row, "VehicleID", "vehicle_id", "VehicleId");
                    if (string.IsNullOrWhiteSpace(vehicleId)) continue;
                    list.Add(new VehicleItem
                    {
                        TvaId = GetColumn(row, "TvaID", "TvaId", "tva_id"),
                        VehicleId = vehicleId,
                        Plate = GetColumn(row, "PoliceNo", "police_no", "car_plate", "VehiclePlate")
                    });
                }
            }
            return list
                .GroupBy(v => v.VehicleId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();
        }

        private static List<MirrorRow> LoadConfigurations()
        {
            var raw = new List<MirrorRow>();
            try
            {
                var connSql = GetSqlClientConnectionString(GetDbConn());
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand(@"
SELECT
    a.VehicleID,
    a.MirrorServer,
    a.IsEnabled,
    a.UsrUpd,
    a.DtmUpd,
    a.TvaID,
    a.CustID,
    a.PoliceNo,
    a.FullName
FROM (
    SELECT a.*, b.FullName
    FROM (
        SELECT a.*, b.PoliceNo
        FROM (
            SELECT a.*, b.TvaID, b.CustID
            FROM (
                SELECT
                    vehicle_id AS VehicleID,
                    mirror_server AS MirrorServer,
                    CAST(CASE WHEN is_enabled = 1 THEN 1 ELSE 0 END AS INT) AS IsEnabled,
                    usrupd AS UsrUpd,
                    dtmupd AS DtmUpd
                FROM GPSB.dbo.vehicle_mirror_realtime WITH (NOLOCK)
                WHERE vehicle_id IS NOT NULL AND vehicle_id <> '0' AND vehicle_id <> ''
            ) a
            LEFT JOIN (
                SELECT TvaID, VehicleID, CustID
                FROM trx_vehicle_assign_header WITH (NOLOCK)
                WHERE Status <> 'DE'
            ) b ON b.VehicleID = a.VehicleID
        ) a
        LEFT JOIN mst_vehicle b WITH (NOLOCK) ON a.VehicleID = b.VehicleID
    ) a
    LEFT JOIN mst_customer b WITH (NOLOCK) ON b.CustID = a.CustID
) a
WHERE a.CustID IS NOT NULL;", conn))
                {
                    conn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            raw.Add(new MirrorRow
                            {
                                CompanyId = Convert.ToString(r["CustID"]) ?? "",
                                CompanyName = r["FullName"] == DBNull.Value ? "" : Convert.ToString(r["FullName"]),
                                VehicleId = Convert.ToString(r["VehicleID"]) ?? "",
                                VehiclePlate = r["PoliceNo"] == DBNull.Value ? "" : Convert.ToString(r["PoliceNo"]),
                                TvaId = r["TvaID"] == DBNull.Value ? "" : Convert.ToString(r["TvaID"]),
                                MirrorServers = new[] { r["MirrorServer"] == DBNull.Value ? "" : Convert.ToString(r["MirrorServer"]) },
                                Status = (r["IsEnabled"] != DBNull.Value && Convert.ToInt32(r["IsEnabled"]) == 1) ? "active" : "paused",
                                UpdatedBy = r["UsrUpd"] == DBNull.Value ? "" : Convert.ToString(r["UsrUpd"]),
                                UpdatedAt = r["DtmUpd"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["DtmUpd"])
                            });
                        }
                    }
                }
            }
            catch
            {
                return new List<MirrorRow>();
            }

            return raw
                .Where(x => !string.IsNullOrWhiteSpace(x.VehicleId))
                .GroupBy(x => x.CompanyId + "|" + x.VehicleId, StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    var latest = g.OrderByDescending(x => x.UpdatedAt ?? DateTime.MinValue).First();
                    var servers = g
                        .SelectMany(x => x.MirrorServers ?? new string[0])
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                    return new MirrorRow
                    {
                        Id = latest.CompanyId + "|" + latest.VehicleId,
                        CompanyId = latest.CompanyId,
                        CompanyName = latest.CompanyName,
                        VehicleId = latest.VehicleId,
                        VehiclePlate = latest.VehiclePlate,
                        TvaId = latest.TvaId,
                        MirrorServers = servers,
                        Status = g.Any(x => x.Status == "active") ? "active" : "paused",
                        UpdatedBy = latest.UpdatedBy,
                        UpdatedAt = latest.UpdatedAt
                    };
                })
                .OrderByDescending(x => x.UpdatedAt ?? DateTime.MinValue)
                .ToList();
        }

        private static int CountActiveVehicles()
        {
            try
            {
                var connSql = GetSqlClientConnectionString(GetDbConn());
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.mst_vehicle WHERE Status = 'RG'", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                return 0;
            }
        }

        private static object ToClientRow(MirrorRow row)
        {
            var culture = CultureInfo.GetCultureInfo("id-ID");
            var updated = row.UpdatedAt.HasValue
                ? row.UpdatedAt.Value.ToString("dd MMM yyyy, HH:mm", culture)
                : "";
            return new
            {
                id = row.Id,
                companyId = row.CompanyId,
                companyName = row.CompanyName,
                vehicleId = row.VehicleId,
                vehiclePlate = row.VehiclePlate,
                tvaId = row.TvaId,
                mirrorServers = row.MirrorServers,
                status = row.Status,
                updatedAt = updated,
                updatedBy = string.IsNullOrWhiteSpace(row.UpdatedBy) ? "Admin" : row.UpdatedBy
            };
        }

        /*
        private static void EnsureTable()
        {
            var connSql = GetSqlClientConnectionString(GetDbConn());
            const string sql = @"
IF OBJECT_ID('dbo.trx_server_mirroring', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.trx_server_mirroring
    (
        Id              VARCHAR(36)  NOT NULL CONSTRAINT PK_trx_server_mirroring PRIMARY KEY,
        CompanyId       VARCHAR(20)  NOT NULL,
        CompanyName     VARCHAR(200) NULL,
        VehicleId       VARCHAR(50)  NOT NULL,
        VehiclePlate    VARCHAR(50)  NULL,
        TvaID           VARCHAR(20)  NULL,
        MirrorServers   VARCHAR(1000) NOT NULL,
        Status          VARCHAR(10)  NOT NULL CONSTRAINT DF_trx_server_mirroring_status DEFAULT ('active'),
        UsrUpd          VARCHAR(50)  NULL,
        DtmUpd          DATETIME     NULL
    );
    CREATE UNIQUE INDEX UX_trx_server_mirroring_company_vehicle
        ON dbo.trx_server_mirroring (CompanyId, VehicleId);
END";
            using (var conn = new SqlConnection(connSql))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        */

        private static bool IsAuthorized()
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.Session == null) return false;
            if (ctx.Session["ClsTypeIsLogin"] == null) return false;
            var access = (ctx.Session["ClsTypeAccessMenu"] ?? "").ToString().ToUpperInvariant();
            return access.Contains("MNUSERVERMIRRORING") || access.Contains("MNUINSTALLMIRRORING");
        }

        private static string GetDbConn()
        {
            return (HttpContext.Current.Session["ClsTypeDBConnStringSQL"] ?? "").ToString();
        }

        private static string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return connectionString;
            var pairs = new List<string>();
            foreach (var part in connectionString.Split(';'))
            {
                var p = part.Trim();
                if (string.IsNullOrEmpty(p)) continue;
                var idx = p.IndexOf('=');
                var key = (idx >= 0 ? p.Substring(0, idx).Trim() : p).ToUpperInvariant();
                if (key == "PROVIDER") continue;
                pairs.Add(p);
            }
            return string.Join(";", pairs);
        }

        private static string[] SplitServers(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return new string[0];
            return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToArray();
        }

        private static string GetColumn(DataRow row, params string[] names)
        {
            foreach (var name in names)
            {
                if (row.Table.Columns.Contains(name) && row[name] != DBNull.Value)
                    return Convert.ToString(row[name]) ?? "";
            }
            return "";
        }

        private static object Fail(string message)
        {
            return new { success = false, message = message ?? "Terjadi kesalahan." };
        }

        private class CompanyItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        private class VehicleItem
        {
            public string TvaId { get; set; }
            public string VehicleId { get; set; }
            public string Plate { get; set; }
        }

        private class MirrorRow
        {
            public string Id { get; set; }
            public string CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string VehicleId { get; set; }
            public string VehiclePlate { get; set; }
            public string TvaId { get; set; }
            public string[] MirrorServers { get; set; }
            public string Status { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        private class MirrorFormValue
        {
            public string id { get; set; }
            public string companyId { get; set; }
            public bool allVehicles { get; set; }
            public string[] vehicleIds { get; set; }
            public string[] mirrorServers { get; set; }
        }
    }
}
