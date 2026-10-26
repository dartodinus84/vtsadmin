using System;
using System.Collections.Generic;
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
            if (context.Session == null || context.Session["ClsTypeIsLogin"] == null)
            {
                context.Response.StatusCode = 403;
                context.Response.Write("Session expired.");
                return;
            }

            string action = (context.Request["action"] ?? "").Trim().ToLowerInvariant();
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
