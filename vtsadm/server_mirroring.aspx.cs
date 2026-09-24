using System;
using System.Collections.Generic;
using System.Data;
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
                var kpi = LoadKpis(companies.Count, servers.Count, rows);

                return new
                {
                    success = true,
                    servers,
                    companies = companies.Select(c => new { id = c.Id, name = c.Name }).ToList(),
                    rows = rows.Select(ToClientRow).ToList(),
                    kpis = new
                    {
                        companyMirrored = kpi.CompanyMirrored,
                        companyActive = kpi.CompanyActive,
                        vehicleMirrored = kpi.VehicleMirrored,
                        vehicleActive = kpi.VehicleActive,
                        serverActive = kpi.ServerActive,
                        serverTotal = kpi.ServerTotal
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

                string editVehicleId = ParseVehicleIdFromRowId(form.id);
                if (!form.allVehicles
                    && (form.vehicleIds == null || form.vehicleIds.Length == 0)
                    && string.IsNullOrWhiteSpace(editVehicleId))
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
                    var wanted = new HashSet<string>(
                        (form.vehicleIds ?? new string[0]).Where(v => !string.IsNullOrWhiteSpace(v)),
                        StringComparer.OrdinalIgnoreCase);
                    if (!string.IsNullOrWhiteSpace(editVehicleId))
                        wanted.Add(editVehicleId);
                    selected = vehicles.Where(v => wanted.Contains(v.VehicleId) || wanted.Contains(v.Plate)).ToList();
                    if (selected.Count == 0 && !string.IsNullOrWhiteSpace(editVehicleId))
                    {
                        selected.Add(new VehicleItem
                        {
                            VehicleId = editVehicleId,
                            Plate = editVehicleId,
                            TvaId = ""
                        });
                    }
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

                var ec = new ExecCommand();
                int saved = 0;
                for (int i = 0; i < selected.Count; i++)
                {
                    var vehicleId = (selected[i].VehicleId ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(vehicleId) || IsPlaceholder(vehicleId))
                        continue;

                    for (int s = 0; s < servers.Length; s++)
                    {
                        int aff = 0;
                        string err = "";
                        string sql = "sp_vehicle_mirror_realtime_upsert '"
                            + vehicleId.Replace("'", "''") + "','"
                            + servers[s].Replace("'", "''") + "'";
                        if (!ec.Execute(sql, GetDbConn(), ref aff, ref err))
                        {
                            return Fail(string.IsNullOrWhiteSpace(err)
                                ? "Gagal menyimpan mirroring untuk vehicle " + vehicleId + "."
                                : err);
                        }
                        saved++;
                    }
                }

                if (saved == 0)
                    return Fail("Tidak ada data mirroring yang disimpan.");

                return new
                {
                    success = true,
                    message = "Pengaturan mirroring berhasil disimpan (" + saved.ToString() + ")."
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

        private static List<string> LoadServers()
        {
            var list = new List<string>();
            try
            {
                var rec = new Recordset();
                rec.Open("sp_list_server_mirroring_server ''", GetDbConn());
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
            try
            {
                var rec = new Recordset();
                rec.Open("sp_list_server_mirroring_company ''", GetDbConn());
                if (rec.RecData != null && rec.RecData.Tables.Count > 0)
                {
                    var table = rec.RecData.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        var id = GetColumn(row, "CustID", "cust_id");
                        if (string.IsNullOrWhiteSpace(id)) continue;
                        if (string.Equals(id, "[Select]", StringComparison.OrdinalIgnoreCase)) continue;
                        var name = GetColumn(row, "FullName", "Fullname");
                        if (list.Exists(c => string.Equals(c.Id, id, StringComparison.OrdinalIgnoreCase))) continue;
                        list.Add(new CompanyItem
                        {
                            Id = id.Trim(),
                            Name = string.IsNullOrWhiteSpace(name) ? id.Trim() : name.Trim()
                        });
                    }
                }
            }
            catch
            {
            }

            return list
                .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<VehicleItem> LoadVehicles(string companyId)
        {
            var list = new List<VehicleItem>();
            if (string.IsNullOrWhiteSpace(companyId)) return list;

            var rec = new Recordset();
            rec.Open("sp_list_server_mirroring_vehicle '" + companyId.Replace("'", "''") + "',''", GetDbConn());
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
                var rec = new Recordset();
                rec.Open("sp_list_server_mirroring", GetDbConn());
                if (rec.RecData == null || rec.RecData.Tables.Count == 0)
                    return new List<MirrorRow>();

                foreach (DataRow row in rec.RecData.Tables[0].Rows)
                {
                    int isEnabled = 0;
                    if (row.Table.Columns.Contains("IsEnabled") && row["IsEnabled"] != DBNull.Value)
                        isEnabled = Convert.ToInt32(row["IsEnabled"]);

                    DateTime? updatedAt = null;
                    if (row.Table.Columns.Contains("DtmUpd") && row["DtmUpd"] != DBNull.Value)
                        updatedAt = Convert.ToDateTime(row["DtmUpd"]);

                    raw.Add(new MirrorRow
                    {
                        CompanyId = GetColumn(row, "CustID"),
                        CompanyName = GetColumn(row, "FullName"),
                        VehicleId = GetColumn(row, "VehicleID", "VehicleId"),
                        VehiclePlate = GetColumn(row, "PoliceNo"),
                        TvaId = GetColumn(row, "TvaID", "TvaId"),
                        MirrorServers = new[] { GetColumn(row, "MirrorServer") },
                        Status = isEnabled == 1 ? "active" : "paused",
                        UpdatedBy = GetColumn(row, "UsrUpd"),
                        UpdatedAt = updatedAt
                    });
                }
            }
            catch
            {
                return new List<MirrorRow>();
            }

            return raw
                .Where(x => IsRealVehicleRow(x))
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

        private static MirrorKpi LoadKpis(int companyActiveFallback, int serverTotalFallback, List<MirrorRow> rows)
        {
            var kpi = new MirrorKpi
            {
                CompanyActive = companyActiveFallback,
                ServerTotal = serverTotalFallback,
                CompanyMirrored = rows
                    .Select(r => r.CompanyId)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count(),
                VehicleMirrored = rows
                    .Select(r => r.VehicleId)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count(),
                ServerActive = rows
                    .Where(r => string.Equals(r.Status, "active", StringComparison.OrdinalIgnoreCase))
                    .SelectMany(r => r.MirrorServers ?? new string[0])
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count(),
                VehicleActive = CountActiveVehicles()
            };

            try
            {
                var rec = new Recordset();
                rec.Open("sp_get_server_mirroring_kpi", GetDbConn());
                if (rec.RecData == null || rec.RecData.Tables.Count == 0 || rec.RecData.Tables[0].Rows.Count == 0)
                    return kpi;

                var row = rec.RecData.Tables[0].Rows[0];
                kpi.CompanyMirrored = GetInt(row, "CompanyMirrored", kpi.CompanyMirrored);
                kpi.CompanyActive = GetInt(row, "CompanyActive", kpi.CompanyActive);
                kpi.VehicleMirrored = GetInt(row, "VehicleMirrored", kpi.VehicleMirrored);
                kpi.VehicleActive = GetInt(row, "VehicleActive", kpi.VehicleActive);
                kpi.ServerActive = GetInt(row, "ServerActive", kpi.ServerActive);
                kpi.ServerTotal = GetInt(row, "ServerTotal", kpi.ServerTotal);
            }
            catch
            {
            }

            return kpi;
        }

        private static int CountActiveVehicles()
        {
            try
            {
                var rec = new Recordset();
                rec.Open("sp_count_server_mirroring_vehicle", GetDbConn());
                if (rec.RecData != null && rec.RecData.Tables.Count > 0 && rec.RecData.Tables[0].Rows.Count > 0)
                {
                    var row = rec.RecData.Tables[0].Rows[0];
                    if (row[0] != DBNull.Value)
                        return Convert.ToInt32(row[0]);
                }
            }
            catch
            {
            }
            return 0;
        }

        private static int GetInt(DataRow row, string column, int fallback)
        {
            if (row == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return fallback;
            return Convert.ToInt32(row[column]);
        }

        private static object ToClientRow(MirrorRow row)
        {
            var updated = row.UpdatedAt.HasValue
                ? row.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
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
                updatedBy = row.UpdatedBy ?? ""
            };
        }

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

        private static string GetColumn(DataRow row, params string[] names)
        {
            foreach (var name in names)
            {
                if (row.Table.Columns.Contains(name) && row[name] != DBNull.Value)
                    return Convert.ToString(row[name]) ?? "";
            }
            return "";
        }

        private static string ParseVehicleIdFromRowId(string rowId)
        {
            if (string.IsNullOrWhiteSpace(rowId)) return "";
            var parts = rowId.Split('|');
            if (parts.Length < 2) return "";
            return (parts[parts.Length - 1] ?? "").Trim();
        }

        private static bool IsPlaceholder(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            var v = value.Trim();
            return v == "0"
                || string.Equals(v, "[Select]", StringComparison.OrdinalIgnoreCase)
                || string.Equals(v, "dummy", StringComparison.OrdinalIgnoreCase)
                || string.Equals(v, "test", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsRealVehicleRow(MirrorRow row)
        {
            return row != null
                && !IsPlaceholder(row.VehicleId)
                && !IsPlaceholder(row.CompanyId);
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

        private class MirrorKpi
        {
            public int CompanyMirrored { get; set; }
            public int CompanyActive { get; set; }
            public int VehicleMirrored { get; set; }
            public int VehicleActive { get; set; }
            public int ServerActive { get; set; }
            public int ServerTotal { get; set; }
        }
    }
}
