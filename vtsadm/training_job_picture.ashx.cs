using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    public class TrainingJobPicture : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string action = (context.Request["action"] ?? "").Trim().ToLowerInvariant();
            if (action == "save")
            {
                SaveFromAfterSales(context);
                return;
            }

            if (context.Session == null || context.Session["ClsTypeIsLogin"] == null)
            {
                context.Response.StatusCode = 403;
                context.Response.Write("Session expired.");
                return;
            }

            if (action == "list")
            {
                WriteList(context);
                return;
            }

            if (action == "file")
            {
                WriteFile(context);
                return;
            }

            context.Response.StatusCode = 400;
            context.Response.Write("Action tidak valid.");
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private static void WriteList(HttpContext context)
        {
            string trainId = SafeTrainId(context.Request["trainId"]);
            string clickedName = SafeFileName(context.Request["name"]);
            List<string> names = new List<string>();
            if (trainId.Length > 0)
            {
                string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
                DataTable table = Query(
                    connString,
                    "SELECT PictureFileName FROM ("
                    + " SELECT LTRIM(RTRIM(PictureFileName)) AS PictureFileName, 0 AS Ord, 0 AS Seq"
                    + " FROM trx_training_header WITH (NOLOCK)"
                    + " WHERE TrainID = ? AND PictureFileName IS NOT NULL AND LTRIM(RTRIM(PictureFileName)) <> ''"
                    + " UNION ALL"
                    + " SELECT LTRIM(RTRIM(PictureFileName)) AS PictureFileName, 1 AS Ord, ISNULL(Seq, 0) AS Seq"
                    + " FROM trx_job_assign_picture WITH (NOLOCK)"
                    + " WHERE TrainID = ? AND PictureFileName IS NOT NULL AND LTRIM(RTRIM(PictureFileName)) <> ''"
                    + ") s ORDER BY Ord, Seq",
                    trainId,
                    trainId);
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string name = SafeFileName(Convert.ToString(row["PictureFileName"]));
                        if (name.Length > 0 && !ContainsName(names, name))
                        {
                            names.Add(name);
                        }
                    }
                }
            }

            if (clickedName.Length > 0 && !ContainsName(names, clickedName))
            {
                names.Insert(0, clickedName);
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(ToJson(names));
        }

        private static void WriteFile(HttpContext context)
        {
            string name = SafeFileName(context.Request["name"]);
            if (name.Length == 0)
            {
                context.Response.StatusCode = 404;
                return;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            DataTable known = Query(
                connString,
                "SELECT TOP 1 PictureFileName FROM ("
                + " SELECT PictureFileName FROM trx_training_header WITH (NOLOCK) WHERE PictureFileName = ?"
                + " UNION ALL"
                + " SELECT PictureFileName FROM trx_job_assign_picture WITH (NOLOCK) WHERE PictureFileName = ?"
                + ") s",
                name,
                name);
            if (known == null || known.Rows.Count == 0)
            {
                context.Response.StatusCode = 404;
                return;
            }

            string path = FindFile(context, name);
            if (string.IsNullOrEmpty(path))
            {
                context.Response.StatusCode = 404;
                return;
            }

            string ext = Path.GetExtension(path).ToLowerInvariant();
            string contentType = "image/jpeg";
            if (ext == ".png") contentType = "image/png";
            else if (ext == ".gif") contentType = "image/gif";
            else if (ext == ".webp") contentType = "image/webp";

            context.Response.ContentType = contentType;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.TransmitFile(path);
        }

        private static void SaveFromAfterSales(HttpContext context)
        {
            string configured = ConfigurationManager.AppSettings["TmcClosePictureKey"] ?? "";
            string sent = context.Request.Headers["X-Tmc-Picture-Key"] ?? "";
            if (configured.Length == 0 || !SameKey(configured, sent))
            {
                context.Response.StatusCode = 403;
                context.Response.Write("Forbidden.");
                return;
            }

            string name = SafeFileName(context.Request["name"]);
            if (name.Length == 0 || !Regex.IsMatch(name, @"^TRH\d{1,10}_(\d{2}|sign)\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Nama file tidak valid.");
                return;
            }

            int length = context.Request.ContentLength;
            if (length <= 0 || length > 4 * 1024 * 1024)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Ukuran gambar tidak valid.");
                return;
            }

            byte[] bytes = new byte[length];
            int read = 0;
            while (read < length)
            {
                int n = context.Request.InputStream.Read(bytes, read, length - read);
                if (n <= 0)
                {
                    break;
                }

                read += n;
            }

            if (read != length || !IsImage(bytes))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("File bukan gambar.");
                return;
            }

            string folder = context.Server.MapPath("~/Picture/");
            Directory.CreateDirectory(folder);
            File.WriteAllBytes(Path.Combine(folder, name), bytes);

            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");
        }

        private static bool SameKey(string configured, string sent)
        {
            if (configured.Length != sent.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < configured.Length; i++)
            {
                diff |= configured[i] ^ sent[i];
            }

            return diff == 0;
        }

        private static bool IsImage(byte[] bytes)
        {
            if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return true;
            }

            return bytes.Length >= 8
                && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
                && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A;
        }

        private static string FindFile(HttpContext context, string name)
        {
            string[] folders = new[]
            {
                context.Server.MapPath("~/Picture/"),
                context.Server.MapPath("~/trx_job_assign_picture/")
            };
            foreach (string folder in folders)
            {
                string path = Path.Combine(folder, name);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        private static DataTable Query(string connString, string sql, params string[] args)
        {
            if (string.IsNullOrWhiteSpace(connString))
            {
                return null;
            }

            DataTable ole = QueryOle(connString, sql, args);
            if (ole != null)
            {
                return ole;
            }

            return QuerySql(ToSqlConnectionString(connString), sql, args);
        }

        private static DataTable QueryOle(string connString, string sql, string[] args)
        {
            try
            {
                DataTable table = new DataTable();
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        if (args != null)
                        {
                            for (int i = 0; i < args.Length; i++)
                            {
                                cmd.Parameters.Add("p" + i.ToString(), OleDbType.VarWChar).Value = args[i] ?? "";
                            }
                        }

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }
                    }
                }

                return table;
            }
            catch
            {
                return null;
            }
        }

        private static DataTable QuerySql(string connString, string sql, string[] args)
        {
            if (string.IsNullOrWhiteSpace(connString))
            {
                return null;
            }

            try
            {
                DataTable table = new DataTable();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(ToNamedSql(sql, args == null ? 0 : args.Length), conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        if (args != null)
                        {
                            for (int i = 0; i < args.Length; i++)
                            {
                                cmd.Parameters.AddWithValue("@p" + i.ToString(), args[i] ?? "");
                            }
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }
                    }
                }

                return table;
            }
            catch
            {
                return null;
            }
        }

        private static string ToNamedSql(string sql, int count)
        {
            StringBuilder named = new StringBuilder();
            int index = 0;
            for (int i = 0; i < sql.Length; i++)
            {
                if (sql[i] == '?' && index < count)
                {
                    named.Append("@p").Append(index.ToString());
                    index++;
                }
                else
                {
                    named.Append(sql[i]);
                }
            }

            return named.ToString();
        }

        private static string ToSqlConnectionString(string raw)
        {
            string trimmed = (raw ?? "").Trim();
            if (trimmed.IndexOf("provider=", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return trimmed;
            }

            string[] parts = trimmed.Split(';');
            StringBuilder builder = new StringBuilder();
            foreach (string part in parts)
            {
                string item = (part ?? "").Trim();
                if (item.Length == 0
                    || item.StartsWith("Provider=", StringComparison.OrdinalIgnoreCase)
                    || item.StartsWith("Persist Security Info", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.Append(';');
                }

                builder.Append(item);
            }

            return builder.ToString();
        }

        private static string SafeTrainId(string value)
        {
            string text = (value ?? "").Trim();
            if (!Regex.IsMatch(text, @"^TRH\d{1,10}$", RegexOptions.IgnoreCase))
            {
                return "";
            }

            return text.ToUpperInvariant();
        }

        private static string SafeFileName(string value)
        {
            string text = Path.GetFileName((value ?? "").Trim());
            if (text.Length == 0 || text == "&nbsp;" || text.IndexOf("..", StringComparison.Ordinal) >= 0)
            {
                return "";
            }

            string ext = Path.GetExtension(text).ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
            {
                return "";
            }

            return text;
        }

        private static bool ContainsName(List<string> names, string name)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (string.Equals(names[i], name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ToJson(List<string> names)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            for (int i = 0; i < names.Count; i++)
            {
                if (i > 0)
                {
                    json.Append(",");
                }

                json.Append("\"");
                json.Append(names[i].Replace("\\", "\\\\").Replace("\"", "\\\""));
                json.Append("\"");
            }

            json.Append("]");
            return json.ToString();
        }
    }
}
