using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    public class DeviceMaintNewDeviceHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                object result = Handle(context);
                WriteJson(context, result);
            }
            catch (Exception ex)
            {
                WriteJson(context, new
                {
                    ok = false,
                    message = ex.Message,
                    rows = new object[0]
                });
            }
        }

        private static void WriteJson(HttpContext context, object payload)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Write(JsonConvert.SerializeObject(payload));
        }

        private static object Handle(HttpContext context)
        {
            if (context.Session == null || context.Session["ClsTypeIsLogin"] == null)
                return new { ok = false, message = "Session expired. Silakan login kembali.", rows = new object[0] };

            string techId = "";
            if (context.Session["DeviceMaintTechnicianID"] != null && context.Session["DeviceMaintTechnicianID"].ToString().Trim() != "")
                techId = context.Session["DeviceMaintTechnicianID"].ToString().Trim();
            else if (context.Session["ClsTypeUserTechnicianID"] != null)
                techId = context.Session["ClsTypeUserTechnicianID"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(techId))
                return new { ok = false, message = "Technician ID kosong. Pastikan sudah login / Load data.", rows = new object[0] };

            string conn = MdvrChannelService.ResolveSqlConnectionString(context);
            if (string.IsNullOrWhiteSpace(conn))
                return new { ok = false, message = "Koneksi database tidak tersedia.", rows = new object[0] };

            string search = context.Request["search"] ?? "";
            var rows = new List<object>();

            using (var sql = new SqlConnection(conn))
            using (var cmd = new SqlCommand("dbo.sp_list_device_maint_new_device_search", sql))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;
                cmd.Parameters.Add("@technicianid", SqlDbType.VarChar, 10).Value = techId;
                cmd.Parameters.Add("@NoSN", SqlDbType.VarChar, 50).Value = search;
                sql.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        rows.Add(new
                        {
                            TdtID = GetStr(rd, "TdtID"),
                            DeviceID = GetStr(rd, "DeviceID"),
                            NoSN = GetStr(rd, "NoSN"),
                            vendorname = GetStr(rd, "VendorName", "vendorname"),
                            devicetypedesc = GetStr(rd, "DeviceTypeDesc", "devicetypedesc"),
                            sourcename = GetStr(rd, "sourcename", "SourceName"),
                            warehousename = GetStr(rd, "WarehouseName", "warehousename")
                        });
                    }
                }
            }

            return new
            {
                ok = true,
                message = rows.Count == 0 ? "No items to display" : "",
                rows = rows
            };
        }

        private static string GetStr(SqlDataReader rd, params string[] names)
        {
            for (int n = 0; n < names.Length; n++)
            {
                try
                {
                    int i = rd.GetOrdinal(names[n]);
                    if (rd.IsDBNull(i)) return "";
                    return Convert.ToString(rd.GetValue(i)) ?? "";
                }
                catch
                {
                }
            }
            return "";
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
