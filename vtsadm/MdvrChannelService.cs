using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace vtsadm
{
    /// <summary>
    /// Backend service untuk fitur Setting Channel MDVR (Device QC / New Installation).
    /// Master channel diambil dari dbo.ref_mdvr_channel, setting disimpan ke
    /// dbo.trx_mdvr_channel_setting. Dipakai oleh mdvr_channel.ashx dan
    /// dipanggil juga oleh code-behind QC (validasi submit server-side).
    /// </summary>
    public static class MdvrChannelService
    {
        public static string GetAction(HttpContext context)
        {
            string action = Convert.ToString(context.Request.Form["action"]);
            if (string.IsNullOrWhiteSpace(action))
            {
                action = Convert.ToString(context.Request.QueryString["action"]);
            }
            return (action ?? "").Trim();
        }

        public static object HandleAction(HttpContext context, string action)
        {
            if (context.Session == null || context.Session["ClsTypeIsLogin"] == null ||
                !Convert.ToBoolean(context.Session["ClsTypeIsLogin"]))
            {
                return new { success = false, message = "Session expired. Silakan login kembali." };
            }

            switch ((action ?? "").Trim().ToLowerInvariant())
            {
                case "load":
                    return LoadSetting(context);
                case "save":
                    return SaveSetting(context);
                case "require":
                    return GetRequireByNoSN(context);
                default:
                    return new { success = false, message = "Action tidak dikenali: " + action };
            }
        }

        /// <summary>
        /// Cek apakah NoSN wajib setting channel (MDVR/JT808) dan ambil MaxChannel.
        /// Dipakai new_install.aspx setelah Device Information terisi.
        /// </summary>
        public static object GetRequireByNoSN(HttpContext context)
        {
            string nosn = (context.Request["nosn"] ?? "").Trim();
            if (string.IsNullOrEmpty(nosn))
            {
                return new { success = false, message = "NoSN wajib diisi." };
            }

            string connectionString = ResolveSqlConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            try
            {
                const string sql = @"
SELECT TOP (1)
    ISNULL(gt.max_ch, 0) AS MaxChannel,
    CASE WHEN gt.autoid IS NOT NULL THEN 1 ELSE 0 END AS IsRequireChannelSetting
FROM dbo.mst_device d WITH (NOLOCK)
INNER JOIN dbo.ref_device_type dt WITH (NOLOCK)
    ON dt.DeviceTypeID = d.DeviceTypeID
LEFT JOIN
(
    SELECT DISTINCT autoid, max_ch
    FROM GPSB.dbo.tbl_gps_type WITH (NOLOCK)
    WHERE ISNULL(is_mdvr, 0) = 1
       OR ISNULL(is_jt808, 0) = 1
) gt ON gt.autoid = dt.GpsTypeID
WHERE d.NoSN = @nosn;";

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@nosn", SqlDbType.VarChar, 100).Value = nosn;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return new
                            {
                                success = true,
                                nosn = nosn,
                                isRequire = false,
                                maxChannel = 0,
                                message = "Device tidak ditemukan."
                            };
                        }

                        int maxChannel = 0;
                        int.TryParse(Convert.ToString(reader["MaxChannel"]), out maxChannel);
                        int isRequireFlag = 0;
                        int.TryParse(Convert.ToString(reader["IsRequireChannelSetting"]), out isRequireFlag);

                        return new
                        {
                            success = true,
                            nosn = nosn,
                            isRequire = isRequireFlag == 1 && maxChannel > 0,
                            maxChannel = maxChannel,
                            message = "OK"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        public static void WriteJson(HttpContext context, object payload)
        {
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.Buffer = true;
            context.Response.BufferOutput = true;
            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Write(JsonConvert.SerializeObject(payload));
            context.Response.Flush();
        }

        // ===== LOAD =====
        public static object LoadSetting(HttpContext context)
        {
            string nosn = (context.Request["nosn"] ?? "").Trim();
            int maxChannel;
            int.TryParse((context.Request["maxchannel"] ?? "").Trim(), out maxChannel);

            if (string.IsNullOrEmpty(nosn))
            {
                return new { success = false, message = "NoSN wajib diisi." };
            }
            if (maxChannel <= 0)
            {
                return new { success = false, message = "MaxChannel harus lebih dari 0." };
            }

            string connectionString = ResolveSqlConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            try
            {
                var master = new List<object>();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_get_ref_mdvr_channel", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maxchannel", SqlDbType.Int).Value = maxChannel;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            master.Add(new
                            {
                                ChanelID = ReadInt(reader, "ChanelID"),
                                ChanelCode = ReadString(reader, "ChanelCode"),
                                ChanelName = ReadString(reader, "ChanelName")
                            });
                        }
                    }
                }

                var existing = new List<object>();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_get_mdvr_channel_setting", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@nosn", SqlDbType.VarChar, 100).Value = nosn;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existing.Add(new
                            {
                                ChanelID = ReadInt(reader, "ChanelID"),
                                ChanelType = ReadString(reader, "ChanelType")
                            });
                        }
                    }
                }

                // Master tipe channel (ADAS/DMS/...) diambil dari tabel referensi,
                // sehingga dropdown di UI tidak lagi hardcode.
                var types = GetChannelTypes(connectionString);

                return new
                {
                    success = true,
                    message = "OK",
                    nosn = nosn,
                    maxChannel = maxChannel,
                    master = master,
                    existing = existing,
                    types = types
                };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        // ===== SAVE =====
        public static object SaveSetting(HttpContext context)
        {
            string nosn = (context.Request["nosn"] ?? "").Trim();
            int maxChannel;
            int.TryParse((context.Request["maxchannel"] ?? "").Trim(), out maxChannel);
            string settingsRaw = (context.Request["settings"] ?? "").Trim();

            // Validasi dasar (server-side, tidak mengandalkan UI).
            if (string.IsNullOrEmpty(nosn))
            {
                return new { success = false, message = "NoSN wajib diisi." };
            }
            if (maxChannel <= 0)
            {
                return new { success = false, message = "MaxChannel harus lebih dari 0." };
            }

            JArray settingsJson;
            try
            {
                settingsJson = string.IsNullOrEmpty(settingsRaw) ? new JArray() : JArray.Parse(settingsRaw);
            }
            catch
            {
                return new { success = false, message = "Format data setting tidak valid." };
            }

            string connectionString = ResolveSqlConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            // Daftar tipe channel valid diambil dari tabel referensi (bukan hardcode).
            // Key = kode UPPER, Value = kode kanonik untuk disimpan.
            Dictionary<string, string> validTypeCodes;
            try
            {
                validTypeCodes = GetChannelTypeCodeMap(connectionString);
            }
            catch (Exception ex)
            {
                return new { success = false, message = "Gagal memuat tipe channel: " + ex.Message };
            }

            var toUpsert = new List<KeyValuePair<int, string>>();
            var toDelete = new List<int>();
            var seenChannelIds = new HashSet<int>();

            foreach (var token in settingsJson)
            {
                JObject item = token as JObject;
                if (item == null)
                {
                    return new { success = false, message = "Format channel tidak valid." };
                }

                JToken idToken = item["ChanelID"] ?? item["chanelId"];
                int chanelId;
                if (idToken == null || !int.TryParse(idToken.ToString(), out chanelId) || chanelId <= 0)
                {
                    return new { success = false, message = "ChanelID tidak valid pada salah satu channel." };
                }

                // ChanelID tidak boleh lebih besar dari MaxChannel.
                if (chanelId > maxChannel)
                {
                    return new { success = false, message = "ChanelID " + chanelId + " melebihi MaxChannel (" + maxChannel + ")." };
                }

                // ChanelID tidak boleh duplikat.
                if (!seenChannelIds.Add(chanelId))
                {
                    return new { success = false, message = "ChanelID " + chanelId + " duplikat." };
                }

                JToken typeToken = item["ChanelType"] ?? item["chanelType"];
                string chanelType = typeToken == null ? "" : typeToken.ToString().Trim();
                string normalizedType = NormalizeChannelType(chanelType, validTypeCodes);

                if (normalizedType != null)
                {
                    // Hanya simpan channel yang tipenya terdaftar di ref_mdvr_channel_type.
                    toUpsert.Add(new KeyValuePair<int, string>(chanelId, normalizedType));
                }
                else
                {
                    // "Tidak Digunakan"/tipe tak dikenal -> hapus setting lama untuk channel ini.
                    toDelete.Add(chanelId);
                }
            }

            string usr = context.Session["ClsTypeUserID"]?.ToString();
            if (string.IsNullOrWhiteSpace(usr))
            {
                usr = "system";
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Hapus channel yang dipilih "Tidak Digunakan".
                            foreach (int cid in toDelete)
                            {
                                using (var del = new SqlCommand("dbo.sp_delete_mdvr_channel_setting", conn, tran))
                                {
                                    del.CommandType = CommandType.StoredProcedure;
                                    del.Parameters.Add("@nosn", SqlDbType.VarChar, 100).Value = nosn;
                                    del.Parameters.Add("@chanelid", SqlDbType.Int).Value = cid;
                                    del.ExecuteNonQuery();
                                }
                            }

                            // 2. Upsert channel ADAS/DMS (update jika ada, insert jika belum) via SP.
                            foreach (var kvp in toUpsert)
                            {
                                using (var ups = new SqlCommand("dbo.sp_upsert_mdvr_channel_setting", conn, tran))
                                {
                                    ups.CommandType = CommandType.StoredProcedure;
                                    ups.Parameters.Add("@nosn", SqlDbType.VarChar, 100).Value = nosn;
                                    ups.Parameters.Add("@chanelid", SqlDbType.Int).Value = kvp.Key;
                                    ups.Parameters.Add("@chaneltype", SqlDbType.VarChar, 20).Value = kvp.Value;
                                    ups.Parameters.Add("@usr", SqlDbType.VarChar, 50).Value = usr;
                                    ups.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); } catch { }
                            throw;
                        }
                    }
                }

                bool isConfigured = IsChannelConfigured(connectionString, nosn);
                return new
                {
                    success = true,
                    message = "Setting channel berhasil disimpan.",
                    nosn = nosn,
                    isConfigured = isConfigured,
                    statusChannel = isConfigured ? "Sudah disetting" : "Belum disetting"
                };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        /// <summary>
        /// Cek apakah sebuah NoSN sudah punya minimal 1 channel ADAS/DMS tersimpan.
        /// </summary>
        public static bool IsChannelConfigured(string connectionString, string nosn)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("dbo.sp_count_mdvr_channel_setting", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nosn", SqlDbType.VarChar, 100).Value = nosn ?? "";
                conn.Open();
                object result = cmd.ExecuteScalar();
                int count;
                return int.TryParse(Convert.ToString(result), out count) && count > 0;
            }
        }

        /// <summary>
        /// Dari daftar NoSN yang wajib setting channel (IsRequireChannelSetting = 1),
        /// kembalikan yang BELUM memiliki setting channel ADAS/DMS di database.
        /// Dipakai untuk validasi submit QC di server-side.
        /// </summary>
        public static List<string> GetUnconfiguredNoSN(string connectionString, IEnumerable<string> requiredNoSN)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(connectionString) || requiredNoSN == null)
            {
                return result;
            }

            var distinct = requiredNoSN
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (string nosn in distinct)
            {
                try
                {
                    if (!IsChannelConfigured(connectionString, nosn))
                    {
                        result.Add(nosn);
                    }
                }
                catch
                {
                    // Jika query gagal, perlakukan sebagai belum tersetting (aman: blok QC).
                    result.Add(nosn);
                }
            }

            return result;
        }

        /// <summary>
        /// Resolusi connection string SqlClient: utamakan config VTSADMIN,
        /// fallback ke session legacy (OLEDB) dengan menghapus token Provider.
        /// </summary>
        public static string ResolveSqlConnectionString(HttpContext context)
        {
            try
            {
                var cfg = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (cfg != null && !string.IsNullOrWhiteSpace(cfg.ConnectionString))
                {
                    return cfg.ConnectionString;
                }
            }
            catch
            {
            }

            string connectionString = context?.Session?["ClsTypeDBConnStringSQL"]?.ToString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return "";
            }

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

        /// <summary>
        /// Bangun HTML sel kolom "Status Channel" (badge + tombol) berdasarkan
        /// field SP: IsRequireChannelSetting, StatusChannel, NoSN, DeviceType, MaxChannel.
        /// Memakai class label/btn AdminLTE existing (tidak membuat style baru).
        /// </summary>
        public static string BuildStatusCellHtml(object isRequireObj, object statusObj, object nosnObj, object deviceTypeObj, object maxChannelObj)
        {
            bool isRequired = ToBool(isRequireObj);
            string nosn = ToStr(nosnObj);
            string deviceType = ToStr(deviceTypeObj);
            int maxChannel = ToInt(maxChannelObj);

            string encNosn = HttpUtility.HtmlAttributeEncode(nosn);

            if (!isRequired)
            {
                return "<div class=\"channel-status-cell\" data-nosn=\"" + encNosn + "\">" +
                       "<span class=\"label label-default\">Tidak diperlukan</span>" +
                       "</div>";
            }

            string status = ToStr(statusObj);
            bool configured = status.IndexOf("sudah", StringComparison.OrdinalIgnoreCase) >= 0;

            string badgeClass = configured ? "label label-success" : "label label-warning";
            string badgeText = configured ? "Sudah disetting" : "Belum disetting";
            string btnText = configured ? "Edit Setting" : "Setting Channel";
            string btnClass = configured ? "btn btn-xs btn-primary" : "btn btn-xs btn-warning";

            string encDeviceType = HttpUtility.HtmlAttributeEncode(deviceType);

            return "<div class=\"channel-status-cell\" data-nosn=\"" + encNosn + "\" " +
                   "data-devicetype=\"" + encDeviceType + "\" data-maxchannel=\"" + maxChannel + "\">" +
                   "<span class=\"label-channel-status " + badgeClass + "\">" + badgeText + "</span><br />" +
                   "<button type=\"button\" class=\"btn-channel-setting " + btnClass + "\" style=\"margin-top:4px;border-radius:0;\" " +
                   "data-nosn=\"" + encNosn + "\" data-devicetype=\"" + encDeviceType + "\" data-maxchannel=\"" + maxChannel + "\">" +
                   "<i class=\"fa fa-sliders\"></i> " + btnText + "</button>" +
                   "</div>";
        }

        private static bool ToBool(object value)
        {
            if (value == null || value == DBNull.Value) return false;
            string s = value.ToString().Trim().ToLowerInvariant();
            return s == "1" || s == "true" || s == "yes" || s == "y";
        }

        private static string ToStr(object value)
        {
            return (value == null || value == DBNull.Value) ? "" : value.ToString().Trim();
        }

        private static int ToInt(object value)
        {
            int parsed;
            return int.TryParse(ToStr(value), out parsed) ? parsed : 0;
        }

        /// <summary>
        /// Ambil daftar tipe channel aktif dari dbo.ref_mdvr_channel_type
        /// (via sp_get_ref_mdvr_channel_type) untuk dikirim ke UI.
        /// </summary>
        private static List<Dictionary<string, string>> GetChannelTypes(string connectionString)
        {
            var list = new List<Dictionary<string, string>>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("dbo.sp_get_ref_mdvr_channel_type", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string code = ReadString(reader, "ChanelTypeCode");
                        if (string.IsNullOrWhiteSpace(code)) continue;
                        string name = ReadString(reader, "ChanelTypeName");
                        if (string.IsNullOrWhiteSpace(name)) name = code;
                        list.Add(new Dictionary<string, string>
                        {
                            { "ChanelTypeCode", code },
                            { "ChanelTypeName", name }
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Peta tipe channel valid: key = kode UPPER, value = kode kanonik (untuk disimpan).
        /// Dipakai untuk validasi server-side saat save.
        /// </summary>
        private static Dictionary<string, string> GetChannelTypeCodeMap(string connectionString)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var t in GetChannelTypes(connectionString))
            {
                string code = t["ChanelTypeCode"].Trim();
                if (code.Length == 0) continue;
                string key = code.ToUpperInvariant();
                if (!map.ContainsKey(key))
                {
                    map[key] = code;
                }
            }
            return map;
        }

        private static string NormalizeChannelType(string chanelType, IDictionary<string, string> validTypeCodes)
        {
            if (string.IsNullOrWhiteSpace(chanelType) || validTypeCodes == null)
            {
                return null;
            }

            string canonical;
            if (validTypeCodes.TryGetValue(chanelType.Trim().ToUpperInvariant(), out canonical))
            {
                return canonical;
            }

            // Nilai lain (mis. "Tidak Digunakan" / kosong / tak terdaftar) dianggap tidak digunakan.
            return null;
        }

        private static string ReadString(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return reader.IsDBNull(i) ? "" : (reader.GetValue(i) ?? "").ToString().Trim();
                }
            }

            return "";
        }

        private static int ReadInt(SqlDataReader reader, string columnName)
        {
            string value = ReadString(reader, columnName);
            int parsed;
            return int.TryParse(value, out parsed) ? parsed : 0;
        }
    }
}
