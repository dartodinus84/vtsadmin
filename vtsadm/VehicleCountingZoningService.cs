using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace vtsadm
{
    public static class VehicleCountingZoningService
    {
        private const int MaxImageBytes = 5 * 1024 * 1024;

        public static string GetAction(HttpContext context)
        {
            string action = Convert.ToString(context.Request.Form["action"]);
            if (string.IsNullOrWhiteSpace(action))
            {
                action = Convert.ToString(context.Request.QueryString["action"]);
            }
            return (action ?? "").Trim();
        }

        public static bool IsAjaxAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                return false;
            }

            switch (action.Trim().ToLowerInvariant())
            {
                case "get_zoning":
                case "upload_zoning_image":
                case "save_zoning":
                    return true;
                default:
                    return false;
            }
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
                case "get_zoning":
                    return GetZoning(context);
                case "upload_zoning_image":
                    return UploadImage(context);
                case "save_zoning":
                    return SaveZoning(context);
                default:
                    return new { success = false, message = "Action AJAX tidak dikenali: " + action };
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

        public static object GetZoning(HttpContext context)
        {
            int id;
            if (!int.TryParse((context.Request.Form["id"] ?? context.Request.QueryString["id"] ?? "").Trim(), out id) || id <= 0)
            {
                return new { success = false, message = "ID Vehicle Counting wajib diisi." };
            }

            string connectionString = GetSqlClientConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            ZoningRecordData record = LoadZoningRecord(id, connectionString);
            if (record == null)
            {
                return new { success = false, message = "Data Vehicle Counting tidak ditemukan." };
            }

            return new
            {
                success = true,
                message = "Data zoning berhasil diambil.",
                data = record
            };
        }

        public static object SaveZoning(HttpContext context)
        {
            int id;
            if (!int.TryParse((context.Request.Form["id"] ?? "").Trim(), out id) || id <= 0)
            {
                return new { success = false, message = "ID Vehicle Counting wajib diisi." };
            }

            string zonePoints = (context.Request.Form["zone_points"] ?? "").Trim();
            return SaveZoningCore(context, id, zonePoints);
        }

        public static object SaveZoningCore(HttpContext context, int id, string zonePoints)
        {
            if (id <= 0)
            {
                return new { success = false, message = "ID Vehicle Counting wajib diisi." };
            }

            if (string.IsNullOrWhiteSpace(zonePoints))
            {
                return new { success = false, message = "Data zoning wajib diisi." };
            }

            zonePoints = zonePoints.Trim();

            JObject zoningJson;
            try
            {
                zoningJson = JObject.Parse(zonePoints);
            }
            catch
            {
                return new { success = false, message = "Format zoning harus JSON valid." };
            }

            string connectionString = GetSqlClientConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            string validationError = ValidateZoningJsonForSave(id, zoningJson, connectionString);
            if (!string.IsNullOrEmpty(validationError))
            {
                return new { success = false, message = validationError };
            }

            string usrUpd = context.Session["ClsTypeUserID"]?.ToString() ?? "system";
            if (string.IsNullOrWhiteSpace(usrUpd))
            {
                usrUpd = "system";
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_vehicle_counting_update_zoning", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@zone_points", SqlDbType.VarChar, -1).Value = zonePoints;
                    cmd.Parameters.Add("@usrupd", SqlDbType.VarChar, 50).Value = usrUpd;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return new
                {
                    success = true,
                    message = "Zoning berhasil disimpan.",
                    data = new { id, zone_points = zonePoints }
                };
            }
            catch (SqlException ex)
            {
                return new { success = false, message = ex.Message };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        public static object UploadImage(HttpContext context)
        {
            int id;
            if (!int.TryParse((context.Request["id"] ?? "").Trim(), out id) || id <= 0)
            {
                return new { success = false, message = "ID Vehicle Counting wajib diisi." };
            }

            string gpsSn = (context.Request["gps_sn"] ?? "").Trim();
            string channelNo = (context.Request["channel_no"] ?? "").Trim();
            if (string.IsNullOrEmpty(gpsSn))
            {
                return new { success = false, message = "GPS SN wajib diisi." };
            }

            if (string.IsNullOrEmpty(channelNo) || !channelNo.All(char.IsDigit))
            {
                return new { success = false, message = "Channel wajib diisi dan numeric." };
            }

            string connectionString = GetSqlClientConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return new { success = false, message = "Koneksi database tidak tersedia." };
            }

            ZoningRecordData record = LoadZoningRecord(id, connectionString);
            if (record == null)
            {
                return new { success = false, message = "Data Vehicle Counting tidak ditemukan." };
            }

            var activeChannels = ParseChannelList(record.next_channel);
            if (!activeChannels.Contains(channelNo))
            {
                return new { success = false, message = "Channel " + channelNo + " bukan channel aktif untuk data ini." };
            }

            if (!string.Equals(record.gps_sn, gpsSn, StringComparison.OrdinalIgnoreCase))
            {
                return new { success = false, message = "GPS SN tidak sesuai dengan data Vehicle Counting." };
            }

            HttpPostedFile file = GetPostedFile(context);
            if (file == null || file.ContentLength <= 0)
            {
                return new { success = false, message = "File gambar wajib diupload." };
            }

            if (file.ContentLength > MaxImageBytes)
            {
                return new { success = false, message = "Ukuran file maksimal 5MB." };
            }

            string ext = Path.GetExtension(file.FileName ?? "").ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
            {
                return new { success = false, message = "Format file hanya jpg, jpeg, atau png." };
            }

            string safeGpsSn = SanitizePathSegment(gpsSn);
            string safeChannel = SanitizePathSegment(channelNo);
            string saveExt = ext == ".jpeg" ? ".jpg" : ext;
            string fileName = "channel_" + safeChannel + saveExt;
            string relativePath = "Picture/zoning/" + safeGpsSn + "/" + fileName;

            string folder = Path.Combine(context.Server.MapPath("~/Picture/zoning"), safeGpsSn);
            Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            file.SaveAs(fullPath);

            string imageUrl = VirtualPathUtility.ToAbsolute("~/" + relativePath.Replace('\\', '/'));
            return new
            {
                success = true,
                message = "Image berhasil diupload.",
                image_path = relativePath,
                image_url = imageUrl
            };
        }

        public static ZoningRecordData LoadZoningRecord(int id, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("dbo.sp_vehicle_counting_get_zoning", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new ZoningRecordData
                    {
                        id = ReadInt(reader, "id"),
                        company_id = ReadInt(reader, "company_id"),
                        company_name = ReadString(reader, "company_name"),
                        vehicle_id = ReadString(reader, "vehicle_id"),
                        nopol = ReadString(reader, "nopol"),
                        gps_sn = ReadString(reader, "gps_sn"),
                        channel = ReadString(reader, "channel"),
                        next_channel = ReadString(reader, "next_channel"),
                        zone_points = ReadString(reader, "zone_points")
                    };
                }
            }
        }

        private static HttpPostedFile GetPostedFile(HttpContext context)
        {
            HttpPostedFile file = null;
            if (context.Request.Files.Count > 0)
            {
                file = context.Request.Files[0];
            }

            if (file == null || file.ContentLength <= 0)
            {
                foreach (string key in context.Request.Files.AllKeys)
                {
                    if (string.IsNullOrEmpty(key)) continue;
                    var candidate = context.Request.Files[key];
                    if (candidate != null && candidate.ContentLength > 0)
                    {
                        return candidate;
                    }
                }
            }

            return file;
        }

        private static string ValidateZoningJsonForSave(int id, JObject zoningJson, string connectionString)
        {
            ZoningRecordData record = LoadZoningRecord(id, connectionString);
            if (record == null)
            {
                return "Data Vehicle Counting tidak ditemukan.";
            }

            var activeChannels = ParseChannelList(record.next_channel);
            if (activeChannels.Count == 0)
            {
                return "Channel aktif tidak ditemukan.";
            }

            bool hasReadyChannel = false;
            foreach (var property in zoningJson.Properties())
            {
                string channelKey = property.Name.Trim();
                if (!activeChannels.Contains(channelKey))
                {
                    return "Key channel \"" + channelKey + "\" di luar channel aktif.";
                }

                var channelObj = property.Value as JObject;
                if (channelObj == null)
                {
                    return "Format channel " + channelKey + " tidak valid.";
                }

                string imagePath = (channelObj["image"] ?? "").ToString().Trim();
                if (string.IsNullOrEmpty(imagePath))
                {
                    return "Channel " + channelKey + " belum memiliki image path.";
                }

                if (imagePath.IndexOf(":\\", StringComparison.Ordinal) >= 0 ||
                    imagePath.StartsWith("\\\\", StringComparison.Ordinal))
                {
                    return "Image path channel " + channelKey + " tidak boleh physical path Windows.";
                }

                if (imagePath.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    return "Image path channel " + channelKey + " tidak boleh base64.";
                }

                var zoneToken = channelObj["zone"];
                if (zoneToken == null || zoneToken.Type != JTokenType.Array)
                {
                    return "Zone channel " + channelKey + " harus array.";
                }

                var zoneArray = (JArray)zoneToken;
                if (zoneArray.Count < 3)
                {
                    return "Channel " + channelKey + " polygon minimal 3 titik.";
                }

                foreach (var point in zoneArray)
                {
                    if (point.Type != JTokenType.Array || ((JArray)point).Count != 2)
                    {
                        return "Titik zone channel " + channelKey + " harus [x,y].";
                    }

                    double x;
                    double y;
                    if (!double.TryParse(((JArray)point)[0].ToString(), out x) ||
                        !double.TryParse(((JArray)point)[1].ToString(), out y))
                    {
                        return "Koordinat channel " + channelKey + " harus numeric.";
                    }
                }

                hasReadyChannel = true;
            }

            if (!hasReadyChannel)
            {
                return "Minimal satu channel harus memiliki image dan zone valid (>= 3 titik).";
            }

            return null;
        }

        private static List<string> ParseChannelList(string channelText)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(channelText))
            {
                return result;
            }

            foreach (string part in channelText.Split(','))
            {
                string ch = part.Trim();
                if (!string.IsNullOrEmpty(ch))
                {
                    result.Add(ch);
                }
            }

            return result;
        }

        public static string GetSqlClientConnectionString(HttpContext context)
        {
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

        private static string SanitizePathSegment(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "unknown";
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            string safe = value.Trim();
            foreach (char c in invalid)
            {
                safe = safe.Replace(c, '_');
            }

            safe = safe.Replace("..", "_").Replace("/", "_").Replace("\\", "_");
            return safe;
        }

        public class ZoningRecordData
        {
            public int id { get; set; }
            public int company_id { get; set; }
            public string company_name { get; set; }
            public string vehicle_id { get; set; }
            public string nopol { get; set; }
            public string gps_sn { get; set; }
            public string channel { get; set; }
            public string next_channel { get; set; }
            public string zone_points { get; set; }
        }
    }
}
