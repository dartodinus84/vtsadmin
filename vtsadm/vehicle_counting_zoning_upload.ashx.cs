using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    public class VehicleCountingZoningUpload : IHttpHandler, IRequiresSessionState
    {
        private const int MaxImageBytes = 5 * 1024 * 1024;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                if (context.Session == null || context.Session["ClsTypeIsLogin"] == null)
                {
                    WriteJson(context, false, "Session expired. Silakan login kembali.");
                    return;
                }

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
                            file = candidate;
                            break;
                        }
                    }
                }

                if (file == null || file.ContentLength <= 0)
                {
                    WriteJson(context, false, "File gambar wajib diupload.");
                    return;
                }

                string gpsSn = (context.Request["gps_sn"] ?? "").Trim();
                string channelNo = (context.Request["channel_no"] ?? "").Trim();

                if (string.IsNullOrEmpty(gpsSn))
                {
                    WriteJson(context, false, "GPS SN wajib diisi.");
                    return;
                }

                if (string.IsNullOrEmpty(channelNo))
                {
                    WriteJson(context, false, "Channel wajib diisi.");
                    return;
                }

                if (file.ContentLength > MaxImageBytes)
                {
                    WriteJson(context, false, "Ukuran file maksimal 5MB.");
                    return;
                }

                string ext = Path.GetExtension(file.FileName ?? "").ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    WriteJson(context, false, "Format file hanya jpg, jpeg, atau png.");
                    return;
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

                WriteJson(context, true, "Upload gambar berhasil.", new
                {
                    image = relativePath,
                    image_url = imageUrl,
                    channel_no = channelNo
                });
            }
            catch (Exception ex)
            {
                WriteJson(context, false, ex.Message);
            }
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

        private static void WriteJson(HttpContext context, bool success, string message, object data = null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success,
                message,
                data
            }));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
