using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    public class VehicleCountingZoningImage : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session == null || context.Session["ClsTypeIsLogin"] == null)
                {
                    context.Response.StatusCode = 403;
                    context.Response.Write("Session expired.");
                    return;
                }

                string relativePath = (context.Request["path"] ?? "").Trim().Replace('\\', '/');
                if (string.IsNullOrEmpty(relativePath))
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Path wajib diisi.");
                    return;
                }

                relativePath = relativePath.TrimStart('/');
                if (relativePath.StartsWith("Picture/zoning/", StringComparison.OrdinalIgnoreCase) == false)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Path tidak valid.");
                    return;
                }

                if (relativePath.IndexOf("..", StringComparison.Ordinal) >= 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Path tidak valid.");
                    return;
                }

                string physicalPath = context.Server.MapPath("~/" + relativePath);
                if (!File.Exists(physicalPath))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("Gambar tidak ditemukan.");
                    return;
                }

                string ext = Path.GetExtension(physicalPath).ToLowerInvariant();
                string contentType = "image/jpeg";
                if (ext == ".png") contentType = "image/png";
                else if (ext == ".gif") contentType = "image/gif";
                else if (ext == ".webp") contentType = "image/webp";

                context.Response.ContentType = contentType;
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.TransmitFile(physicalPath);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
