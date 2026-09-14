using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using vtsadm.App_Code;

namespace vtsadm
{
    /// <summary>
    /// Telegram notifications and bot commands for IT Support job assignments.
    /// </summary>
    public static class ItsAssignTelegramService
    {
        private const int OpenListMaxRows = 15;
        private static readonly CultureInfo IndonesianCulture = CultureInfo.GetCultureInfo("id-ID");
        private static bool _tlsConfigured;

        private static void EnsureTls12()
        {
            if (_tlsConfigured)
            {
                return;
            }

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _tlsConfigured = true;
        }

        public static void NotifyAfterAssign(
            string connString,
            string jobId,
            string custId,
            string technicianId,
            DateTime schDate)
        {
            if (string.IsNullOrWhiteSpace(connString)
                || string.IsNullOrWhiteSpace(jobId)
                || string.IsNullOrWhiteSpace(technicianId))
            {
                return;
            }

            string assignId;
            int seq;
            TryResolveLatestAssignKey(connString, jobId, custId, technicianId, schDate, out assignId, out seq);

            AssignDetail detail = !string.IsNullOrWhiteSpace(assignId)
                ? LoadAssignDetail(connString, assignId, seq)
                : null;
            if (detail == null)
            {
                detail = BuildFallbackAssignDetail(connString, jobId, custId, technicianId, schDate);
            }

            if (detail == null || string.IsNullOrWhiteSpace(detail.JobId))
            {
                return;
            }

            string text = BuildDetailMessage(detail);
            TelegramConfig config = LoadTelegramConfig(connString);
            if (!config.IsValid)
            {
                return;
            }

            SendTelegramMessage(config.ApiToken, config.ChatId, text);
        }

        public static void ProcessWebhook(HttpContext context)
        {
            if (context == null)
            {
                return;
            }

            EnsureTls12();
            context.Response.ContentType = "text/plain; charset=utf-8";

            if (TryHandleSetupRequest(context))
            {
                return;
            }

            string body = string.Empty;
            try
            {
                if (context.Request.InputStream != null && context.Request.InputStream.CanRead)
                {
                    using (StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                    {
                        body = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                body = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                context.Response.StatusCode = 200;
                context.Response.Write(
                    "IT Support Telegram webhook is ready.\r\n"
                    + "Register webhook: add ?setup=itssetup to this URL in your browser.");
                return;
            }

            string connString = ResolveConnString(context);
            TelegramConfig config = LoadTelegramConfig(connString);
            if (!config.HasApiToken)
            {
                context.Response.StatusCode = 200;
                context.Response.Write("Telegram bot token not configured.");
                return;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> update = serializer.Deserialize<Dictionary<string, object>>(body);
            if (update == null)
            {
                context.Response.StatusCode = 200;
                context.Response.Write("OK");
                return;
            }

            if (update.ContainsKey("callback_query"))
            {
                HandleCallbackQuery(connString, config, update["callback_query"], serializer);
            }
            else if (update.ContainsKey("message"))
            {
                HandleMessage(connString, config, update["message"], serializer);
            }
            else if (update.ContainsKey("edited_message"))
            {
                HandleMessage(connString, config, update["edited_message"], serializer);
            }

            context.Response.StatusCode = 200;
            context.Response.Write("OK");
        }

        private static void HandleMessage(
            string connString,
            TelegramConfig config,
            object messageObj,
            JavaScriptSerializer serializer)
        {
            Dictionary<string, object> message = messageObj as Dictionary<string, object>;
            if (message == null)
            {
                return;
            }

            string text = GetDictString(message, "text");
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            text = text.Trim();
            string chatId = GetChatIdFromMessage(message);
            if (string.IsNullOrWhiteSpace(chatId))
            {
                return;
            }

            if (IsBotCommand(text, "/help")
                || IsBotCommand(text, "/start"))
            {
                SendHtmlMessage(
                    config.ApiToken,
                    chatId,
                    "<b>IT Support Assign Bot</b>\r\n"
                    + "/open — daftar penugasan belum selesai\r\n"
                    + "/detail &lt;AssignID&gt; [Seq] — detail penugasan\r\n"
                    + "/chatid — tampilkan Chat ID chat ini");
                return;
            }

            if (IsBotCommand(text, "/chatid"))
            {
                SendChatIdInfo(config.ApiToken, message);
                return;
            }

            if (text.StartsWith("/open", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("/list", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("/belumselesai", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(connString))
                {
                    SendHtmlMessage(config.ApiToken, chatId, "Database connection belum dikonfigurasi di server.");
                    return;
                }

                SendOpenAssignList(connString, config.ApiToken, chatId);
                return;
            }

            if (text.StartsWith("/detail", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(connString))
                {
                    SendHtmlMessage(config.ApiToken, chatId, "Database connection belum dikonfigurasi di server.");
                    return;
                }

                string[] parts = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                {
                    SendHtmlMessage(config.ApiToken, chatId, "Format: /detail &lt;AssignID&gt; [Seq]");
                    return;
                }

                string assignId = parts[1].Trim();
                int seq = 1;
                if (parts.Length >= 3)
                {
                    int parsedSeq;
                    if (int.TryParse(parts[2], out parsedSeq) && parsedSeq > 0)
                    {
                        seq = parsedSeq;
                    }
                }

                AssignDetail detail = LoadAssignDetail(connString, assignId, seq);
                if (detail == null)
                {
                    SendHtmlMessage(config.ApiToken, chatId, "Data penugasan tidak ditemukan.");
                    return;
                }

                SendHtmlMessage(config.ApiToken, chatId, BuildDetailMessage(detail));
            }
        }

        private static void HandleCallbackQuery(
            string connString,
            TelegramConfig config,
            object callbackObj,
            JavaScriptSerializer serializer)
        {
            Dictionary<string, object> callback = callbackObj as Dictionary<string, object>;
            if (callback == null)
            {
                return;
            }

            string callbackId = GetDictString(callback, "id");
            string data = GetDictString(callback, "data");
            string chatId = string.Empty;

            Dictionary<string, object> message = callback["message"] as Dictionary<string, object>;
            if (message != null)
            {
                chatId = GetChatIdFromMessage(message);
            }

            if (!string.IsNullOrWhiteSpace(callbackId))
            {
                AnswerCallbackQuery(config.ApiToken, callbackId);
            }

            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(chatId))
            {
                return;
            }

            if (!data.StartsWith("itsd:", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string[] parts = data.Split(':');
            if (parts.Length < 3)
            {
                return;
            }

            string assignId = parts[1].Trim();
            int seq = 1;
            int parsedSeq;
            if (int.TryParse(parts[2], out parsedSeq) && parsedSeq > 0)
            {
                seq = parsedSeq;
            }

            AssignDetail detail = LoadAssignDetail(connString, assignId, seq);
            if (detail == null)
            {
                SendHtmlMessage(config.ApiToken, chatId, "Data penugasan tidak ditemukan.");
                return;
            }

            SendHtmlMessage(config.ApiToken, chatId, BuildDetailMessage(detail));
        }

        private static void SendOpenAssignList(string connString, string apiToken, string chatId)
        {
            List<OpenAssignSummary> rows = LoadOpenAssignSummaries(connString);
            if (rows.Count == 0)
            {
                SendHtmlMessage(apiToken, chatId, "Tidak ada penugasan IT Support yang belum selesai.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<b>📋 Penugasan IT Support belum selesai</b>");
            sb.AppendLine("<i>Klik tombol di bawah untuk detail</i>");
            sb.AppendLine();

            int index = 1;
            foreach (OpenAssignSummary row in rows)
            {
                sb.AppendLine(index.ToString() + ". <b>" + EscapeHtml(row.JobId) + "</b> | "
                    + EscapeHtml(FormatDateShort(row.SchDate)) + " | "
                    + EscapeHtml(row.CustomerName) + " | "
                    + EscapeHtml(row.TechnicianName));
                index++;
            }

            List<List<Dictionary<string, string>>> keyboard = new List<List<Dictionary<string, string>>>();
            foreach (OpenAssignSummary row in rows)
            {
                string label = TrimButtonLabel(row.JobId + " - " + row.CustomerName, 40);
                Dictionary<string, string> button = new Dictionary<string, string>();
                button["text"] = label;
                button["callback_data"] = "itsd:" + row.AssignId + ":" + row.Seq.ToString();
                keyboard.Add(new List<Dictionary<string, string>> { button });
            }

            Dictionary<string, object> replyMarkup = new Dictionary<string, object>();
            replyMarkup["inline_keyboard"] = keyboard;

            SendHtmlMessage(apiToken, chatId, sb.ToString().TrimEnd(), replyMarkup);
        }

        private static List<OpenAssignSummary> LoadOpenAssignSummaries(string connString)
        {
            List<OpenAssignSummary> list = new List<OpenAssignSummary>();
            DateTime monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            string fromText = monthStart.ToString("yyyy-MM-dd");

            string[] queries = new string[]
            {
                "SELECT TOP " + OpenListMaxRows.ToString()
                    + " d.AssignID, d.Seq, d.JobID, d.SchDate, "
                    + "ISNULL(d.CustomerName, d.Customer) AS CustomerName, "
                    + "ISNULL(d.TechnicianName, d.TechnicianID) AS TechnicianName, d.Status "
                    + "FROM trx_job_assign_detail d WITH (NOLOCK) "
                    + "WHERE d.SchDate >= '" + fromText.Replace("'", "''") + "' "
                    + "AND UPPER(LTRIM(RTRIM(ISNULL(d.TechnicianID,'')))) LIKE 'IT%' "
                    + "AND UPPER(LTRIM(RTRIM(ISNULL(d.Status,'')))) NOT IN ('CL','CLOSE','CLOSED','SELESAI') "
                    + "ORDER BY d.SchDate DESC, d.JobID ASC",
                "SELECT TOP " + OpenListMaxRows.ToString()
                    + " d.AssignID, d.Seq, d.JobID, d.SchDate, "
                    + "ISNULL(d.Customer, '') AS CustomerName, "
                    + "d.TechnicianID AS TechnicianName, d.Status "
                    + "FROM trx_job_assign_detail d WITH (NOLOCK) "
                    + "WHERE d.SchDate >= '" + fromText.Replace("'", "''") + "' "
                    + "AND UPPER(LTRIM(RTRIM(ISNULL(d.TechnicianID,'')))) LIKE 'IT%' "
                    + "AND UPPER(LTRIM(RTRIM(ISNULL(d.Status,'')))) NOT IN ('CL','CLOSE','CLOSED','SELESAI') "
                    + "ORDER BY d.SchDate DESC, d.JobID ASC"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(connString, sql);
                if (table != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        OpenAssignSummary item = new OpenAssignSummary();
                        item.AssignId = GetRowString(row, "AssignID");
                        item.Seq = ParseInt(GetRowString(row, "Seq"), 1);
                        item.JobId = GetRowString(row, "JobID");
                        item.SchDate = ParseDate(GetRowString(row, "SchDate"));
                        item.CustomerName = FirstNonEmpty(
                            GetRowString(row, "CustomerName"),
                            GetRowString(row, "Customer"),
                            "-");
                        item.TechnicianName = FirstNonEmpty(
                            GetRowString(row, "TechnicianName"),
                            GetRowString(row, "TechnicianID"),
                            "-");
                        list.Add(item);
                    }
                    break;
                }
            }

            return list;
        }

        private static AssignDetail LoadAssignDetail(string connString, string assignId, int seq)
        {
            if (string.IsNullOrWhiteSpace(assignId))
            {
                return null;
            }

            AssignDetail detail = TryLoadFromSp(connString, assignId);
            if (detail == null)
            {
                detail = new AssignDetail();
                detail.AssignId = assignId.Trim();
                detail.Seq = seq;
            }

            EnrichFromTrxRow(connString, detail, assignId, seq);
            EnrichFromAssignHeader(connString, detail);
            EnrichFromCustomer(connString, detail);
            EnrichFromTrainingOrder(connString, detail);
            EnrichFromItsSupport(connString, detail);

            if (string.IsNullOrWhiteSpace(detail.JobId))
            {
                return null;
            }

            return detail;
        }

        private static AssignDetail TryLoadFromSp(string connString, string assignId)
        {
            try
            {
                Recordset rec = new Recordset();
                string sql = "sp_get_assign_status '" + EscapeSqlLiteral(assignId.Trim()) + "','RG'";
                rec.Open(sql, connString);
                if (rec.RecordCount() <= 0)
                {
                    return null;
                }

                AssignDetail detail = new AssignDetail();
                detail.AssignId = FirstNonEmpty(rec.Fields("AssignID"), assignId).Trim();
                detail.Seq = ParseInt(rec.Fields("Seq"), 1);
                detail.JobId = FirstNonEmpty(rec.Fields("JobID"), rec.Fields("JoID"));
                detail.CustomerName = FirstNonEmpty(rec.Fields("Fullname"), rec.Fields("CustomerName"), rec.Fields("Customer"));
                detail.TechnicianId = FirstNonEmpty(rec.Fields("TechnicianID"), rec.Fields("ITID"));
                detail.TechnicianName = FirstNonEmpty(rec.Fields("Name"), rec.Fields("TechnicianName"), detail.TechnicianId);
                detail.SchDate = ParseDate(FirstNonEmpty(rec.Fields("SchDate"), rec.Fields("ScheduleDate")));
                detail.Remark = FirstNonEmpty(rec.Fields("Remark"), rec.Fields("Remarks"));
                detail.MarketingName = FirstNonEmpty(rec.Fields("MarketingName"), rec.Fields("Marketing"));
                detail.PoId = FirstNonEmpty(rec.Fields("POID"), rec.Fields("PoID"));
                detail.CustId = FirstNonEmpty(rec.Fields("CustID"), rec.Fields("CustomerID"));
                detail.PoliceNo = FirstNonEmpty(rec.Fields("PoliceNo"), rec.Fields("NoPol"));
                detail.NoSn = FirstNonEmpty(rec.Fields("NoSN"), rec.Fields("MSIDN"), rec.Fields("SerialNo"));
                detail.GsmNo = FirstNonEmpty(rec.Fields("GSMNo"), rec.Fields("GSM"), rec.Fields("MSISDN"));
                detail.JobType = FirstNonEmpty(rec.Fields("JobType"), rec.Fields("DeviceTypeDesc"));
                detail.InstallDate = ParseDate(FirstNonEmpty(rec.Fields("InstallDate"), rec.Fields("JODate")));
                return detail;
            }
            catch
            {
                return null;
            }
        }

        private static void EnrichFromTrxRow(string connString, AssignDetail detail, string assignId, int seq)
        {
            string sql = "SELECT TOP 1 * FROM trx_job_assign_detail WITH (NOLOCK) "
                + "WHERE AssignID = '" + EscapeSqlLiteral(assignId.Trim()) + "' "
                + "AND Seq = " + seq.ToString()
                + " ORDER BY Seq DESC";

            DataTable table = ExecuteQuery(connString, sql);
            if (table == null || table.Rows.Count == 0)
            {
                sql = "SELECT TOP 1 * FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE AssignID = '" + EscapeSqlLiteral(assignId.Trim()) + "' "
                    + "ORDER BY Seq DESC";
                table = ExecuteQuery(connString, sql);
            }

            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            DataRow row = table.Rows[0];
            detail.AssignId = FirstNonEmpty(GetRowString(row, "AssignID"), detail.AssignId);
            detail.Seq = ParseInt(GetRowString(row, "Seq"), detail.Seq);
            detail.JobId = FirstNonEmpty(GetRowString(row, "JobID"), detail.JobId);
            detail.CustId = FirstNonEmpty(GetRowString(row, "CustID"), detail.CustId);
            detail.CustomerName = FirstNonEmpty(
                GetRowString(row, "CustomerName"),
                GetRowString(row, "Customer"),
                detail.CustomerName);
            detail.TechnicianId = FirstNonEmpty(
                GetRowString(row, "TechnicianID"),
                detail.TechnicianId);
            detail.TechnicianName = FirstNonEmpty(
                GetRowString(row, "TechnicianName"),
                GetRowString(row, "TechnicianID"),
                detail.TechnicianName);
            detail.SchDate = ParseDate(FirstNonEmpty(GetRowString(row, "SchDate"), FormatDateValue(detail.SchDate)));
            detail.Remark = FirstNonEmpty(GetRowString(row, "Remark"), detail.Remark);
            detail.PoliceNo = FirstNonEmpty(GetRowString(row, "PoliceNo"), detail.PoliceNo);
            detail.NoSn = FirstNonEmpty(GetRowString(row, "NoSN"), detail.NoSn);
            detail.GsmNo = FirstNonEmpty(GetRowString(row, "GSMNo"), detail.GsmNo);
            detail.JobType = FirstNonEmpty(GetRowString(row, "JobType"), GetRowString(row, "DeviceTypeDesc"), detail.JobType);
            detail.InstallDate = ParseDate(FirstNonEmpty(GetRowString(row, "InstallDate"), GetRowString(row, "MIS_Date"), FormatDateValue(detail.InstallDate)));
            detail.AreaName = FirstNonEmpty(GetRowString(row, "AreaName"), detail.AreaName);
            detail.Status = FirstNonEmpty(GetRowString(row, "Status"), detail.Status);
        }

        private static void EnrichFromCustomer(string connString, AssignDetail detail)
        {
            if (string.IsNullOrWhiteSpace(detail.CustId))
            {
                return;
            }

            string custId = EscapeSqlLiteral(detail.CustId.Trim());
            string sql = "SELECT TOP 1 c.CustID, c.FullName, c.BranchName, c.MarketingID, "
                + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                + "FROM mst_customer c WITH (NOLOCK) "
                + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) "
                + "WHERE c.CustID = '" + custId + "'";

            DataTable table = ExecuteQuery(connString, sql);
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            DataRow row = table.Rows[0];
            string fullName = GetRowString(row, "FullName");
            string custCode = GetRowString(row, "CustID");
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                detail.CustomerName = fullName;
                detail.CompanyLine = fullName + "(" + custCode + ")";
            }

            detail.BranchName = FirstNonEmpty(GetRowString(row, "BranchName"), detail.BranchName);
            detail.MarketingName = FirstNonEmpty(GetRowString(row, "MarketingName"), detail.MarketingName);
        }

        private static AssignDetail BuildFallbackAssignDetail(
            string connString,
            string jobId,
            string custId,
            string technicianId,
            DateTime schDate)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return null;
            }

            AssignDetail detail = new AssignDetail
            {
                JobId = jobId.Trim(),
                CustId = (custId ?? string.Empty).Trim(),
                SchDate = schDate,
                TechnicianId = (technicianId ?? string.Empty).Trim(),
                TechnicianName = (technicianId ?? string.Empty).Trim()
            };

            EnrichFromTrainingOrder(connString, detail);
            EnrichFromCustomer(connString, detail);
            EnrichFromItsSupport(connString, detail);
            return detail;
        }

        private static void EnrichFromAssignHeader(string connString, AssignDetail detail)
        {
            if (string.IsNullOrWhiteSpace(detail.AssignId))
            {
                return;
            }

            string assignId = EscapeSqlLiteral(detail.AssignId.Trim());
            string sql = "SELECT TOP 1 CustID, AreaID, ScheduleDate "
                + "FROM trx_job_assign_header WITH (NOLOCK) "
                + "WHERE AssignID = '" + assignId + "' "
                + "AND ISNULL(Status, '') NOT IN ('DE')";

            DataTable table = ExecuteQuery(connString, sql);
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            DataRow row = table.Rows[0];
            detail.CustId = FirstNonEmpty(GetRowString(row, "CustID"), detail.CustId);
            string areaId = GetRowString(row, "AreaID");
            if (!string.IsNullOrWhiteSpace(areaId))
            {
                detail.AreaName = FirstNonEmpty(ResolveSupportAreaLabel(connString, areaId), areaId, detail.AreaName);
            }

            DateTime? scheduleDate = ParseDate(GetRowString(row, "ScheduleDate"));
            if (scheduleDate.HasValue)
            {
                detail.SchDate = scheduleDate;
            }
        }

        private static void EnrichFromItsSupport(string connString, AssignDetail detail)
        {
            string technicianKey = FirstNonEmpty(detail.TechnicianId, detail.TechnicianName, string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(technicianKey))
            {
                return;
            }

            string safeKey = EscapeSqlLiteral(technicianKey);
            string[] queries =
            {
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(Name, ''))) AS Name "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(ITID, ''))) = '" + safeKey + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL')",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(Name, ''))) AS Name "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + safeKey + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL')"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(connString, sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                string name = GetRowString(table.Rows[0], "Name");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    detail.TechnicianName = name;
                    return;
                }
            }
        }

        private static string ResolveSupportAreaLabel(string connString, string areaId)
        {
            if (string.IsNullOrWhiteSpace(areaId))
            {
                return string.Empty;
            }

            string safeAreaId = EscapeSqlLiteral(areaId.Trim());
            try
            {
                Recordset rec = new Recordset();
                rec.Open("sp_list_support_area '" + safeAreaId + "'", connString);
                if (rec.RecordCount() > 0)
                {
                    return FirstNonEmpty(
                        rec.Fields("SupAreaName"),
                        rec.Fields("AreaName"),
                        rec.Fields("SupportAreaName"),
                        rec.Fields("Description"),
                        rec.Fields("SupAreaID"));
                }
            }
            catch
            {
            }

            return areaId.Trim();
        }

        private static void EnrichFromTrainingOrder(string connString, AssignDetail detail)
        {
            if (string.IsNullOrWhiteSpace(detail.JobId))
            {
                return;
            }

            string jobId = EscapeSqlLiteral(detail.JobId.Trim());
            string[] queries = new string[]
            {
                "SELECT TOP 1 TrainingID, CustID, sReqDate, ScheduleDate, Remark, RemarkTraining, BranchName, "
                    + "TrainingCategoryName, TrainCategoryName, CategoryName "
                    + "FROM trx_training_order WITH (NOLOCK) WHERE TrainingID = '" + jobId + "'",
                "SELECT TOP 1 TrainingID, CustID, ReqDate, ScheduleDate, Remark, RemarkTraining, BranchName "
                    + "FROM trx_training_order WITH (NOLOCK) WHERE TrainingID = '" + jobId + "'"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(connString, sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                DataRow row = table.Rows[0];
                detail.CustId = FirstNonEmpty(GetRowString(row, "CustID"), detail.CustId);
                detail.ReqDate = ParseDate(FirstNonEmpty(
                    GetRowString(row, "sReqDate"),
                    GetRowString(row, "ReqDate"),
                    FormatDateValue(detail.ReqDate)));
                detail.TrainingScheduleDate = ParseDate(GetRowString(row, "ScheduleDate"));
                string remarkTraining = FirstNonEmpty(GetRowString(row, "RemarkTraining"), GetRowString(row, "Remark"));
                if (!string.IsNullOrWhiteSpace(remarkTraining))
                {
                    detail.RemarkTraining = remarkTraining;
                }

                string category = FirstNonEmpty(
                    GetRowString(row, "TrainingCategoryName"),
                    GetRowString(row, "TrainCategoryName"),
                    GetRowString(row, "CategoryName"));
                if (!string.IsNullOrWhiteSpace(category))
                {
                    detail.CategoryLabel = category;
                }

                detail.BranchName = FirstNonEmpty(GetRowString(row, "BranchName"), detail.BranchName);
                break;
            }
        }

        private static string BuildDetailMessage(AssignDetail detail)
        {
            string category = FirstNonEmpty(detail.CategoryLabel, detail.JobType, "Training/Visit");
            if (category.IndexOf("visit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                category = "Visit";
            }
            else if (category.IndexOf("train", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                category = "Training";
            }

            StringBuilder sb = new StringBuilder();
            string customerName = FirstNonEmpty(detail.CustomerName, detail.CompanyLine, detail.CustId, "-");
            string itSupportName = FirstNonEmpty(detail.TechnicianName, "-");
            string remark = FirstNonEmpty(detail.RemarkTraining, detail.Remark, "-");
            string lokasi = FirstNonEmpty(detail.AreaName, detail.BranchName, "-");

            sb.AppendLine("🛠 <b>NOTIFIKASI PENUGASAN PEKERJAAN</b>");
            sb.AppendLine("<b>" + EscapeHtml(category) + "━━━━━━━━━━━━━━</b>");
            sb.AppendLine();
            sb.AppendLine("Nomor JO : <b>" + EscapeHtml(detail.JobId) + "</b>");
            sb.AppendLine("Tanggal JO : <b>" + EscapeHtml(FormatDateId(detail.ReqDate ?? detail.InstallDate)) + "</b>");
            sb.AppendLine("Tanggal Jadwal : <b>" + EscapeHtml(FormatDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            sb.AppendLine();
            sb.AppendLine("Pelanggan : <b>" + EscapeHtml(customerName) + "</b>");
            sb.AppendLine("IT Support : <b>" + EscapeHtml(itSupportName) + "</b>");
            if (!string.IsNullOrWhiteSpace(detail.MarketingName))
            {
                sb.AppendLine("Marketing : <b>" + EscapeHtml(detail.MarketingName) + "</b>");
            }
            sb.AppendLine();
            sb.AppendLine("<b>Catatan :</b>");
            sb.AppendLine(EscapeHtml(remark));
            sb.AppendLine();
            sb.AppendLine("Hari/tanggal : <b>" + EscapeHtml(FormatDayDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            sb.AppendLine("Lokasi : <b>" + EscapeHtml(lokasi) + "</b>");
            sb.AppendLine();
            sb.AppendLine("<b>Detail Unit :</b>");
            sb.AppendLine("Job Type  : <b>" + EscapeHtml(FirstNonEmpty(detail.JobType, category, "Training/Visit")) + "</b>");
            sb.AppendLine("No Polisi    : <b>" + EscapeHtml(FirstNonEmpty(detail.PoliceNo, "-")) + "</b>");
            sb.AppendLine("GPS SN      : <b>" + EscapeHtml(FirstNonEmpty(detail.NoSn, "-")) + "</b>");
            sb.AppendLine("GSM No      : <b>" + EscapeHtml(FirstNonEmpty(detail.GsmNo, "-")) + "</b>");

            return sb.ToString().TrimEnd();
        }

        private static bool TryResolveLatestAssignKey(
            string connString,
            string jobId,
            string custId,
            string technicianId,
            DateTime schDate,
            out string assignId,
            out int seq)
        {
            assignId = string.Empty;
            seq = 1;

            string safeJobId = EscapeSqlLiteral(jobId.Trim());
            string safeTechnicianId = EscapeSqlLiteral(technicianId.Trim());
            string safeSchDate = schDate.ToString("yyyy-MM-dd");
            string activeFilter = "AND ISNULL(Status, '') NOT IN ('DE') ";
            string techMatch = "LTRIM(RTRIM(ISNULL(TechnicianID, ''))) = '" + safeTechnicianId + "'";

            string[] queries =
            {
                "SELECT TOP 1 AssignID, Seq FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(JobID, ''))) = '" + safeJobId + "' "
                    + "AND " + techMatch + " "
                    + "AND CONVERT(date, SchDate) = '" + safeSchDate + "' "
                    + activeFilter
                    + "ORDER BY Seq DESC",
                "SELECT TOP 1 AssignID, Seq FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(JobID, ''))) = '" + safeJobId + "' "
                    + "AND " + techMatch + " "
                    + activeFilter
                    + "ORDER BY Seq DESC",
                "SELECT TOP 1 AssignID, Seq FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(JobID, ''))) = '" + safeJobId + "' "
                    + activeFilter
                    + "ORDER BY Seq DESC"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(connString, sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                assignId = GetRowString(table.Rows[0], "AssignID");
                seq = ParseInt(GetRowString(table.Rows[0], "Seq"), 1);
                if (!string.IsNullOrWhiteSpace(assignId))
                {
                    return true;
                }
            }

            return false;
        }

        private static TelegramConfig LoadTelegramConfig(string connString)
        {
            TelegramConfig config = new TelegramConfig();

            // IT Support Telegram: Web.config only (not par_global / DB).
            config.ApiToken = FirstNonEmpty(
                GetAppSetting("ItsSupportTelegramApiToken"),
                GetAppSetting("TelegramApi"));
            config.ChatId = FirstNonEmpty(
                GetAppSetting("ItsSupportTelegramChatId"),
                GetAppSetting("TelegramChatIDItsSupport"));
            config.UrlTemplate = FirstNonEmpty(
                GetAppSetting("ItsSupportTelegramUrl"),
                GetAppSetting("TelegramUrl"));

            config.HasApiToken = !string.IsNullOrWhiteSpace(config.ApiToken);
            config.IsValid = config.HasApiToken && !string.IsNullOrWhiteSpace(config.ChatId);
            return config;
        }

        private static bool IsBotCommand(string text, string command)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(command))
            {
                return false;
            }

            string normalized = text.Trim();
            if (normalized.Equals(command, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (normalized.StartsWith(command + "@", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return normalized.StartsWith(command + " ", StringComparison.OrdinalIgnoreCase);
        }

        private static void SendChatIdInfo(string apiToken, Dictionary<string, object> message)
        {
            if (message == null)
            {
                return;
            }

            string chatId = GetChatIdFromMessage(message);
            if (string.IsNullOrWhiteSpace(chatId))
            {
                return;
            }

            Dictionary<string, object> chat = message["chat"] as Dictionary<string, object>;
            string chatType = chat == null ? string.Empty : GetDictString(chat, "type");
            string chatName = chat == null
                ? string.Empty
                : FirstNonEmpty(
                    GetDictString(chat, "title"),
                    GetDictString(chat, "first_name"),
                    GetDictString(chat, "username"));
            string username = chat == null ? string.Empty : GetDictString(chat, "username");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<b>Chat ID</b>");
            sb.AppendLine("<code>" + EscapeHtml(chatId) + "</code>");
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(chatType))
            {
                sb.AppendLine("Type: <b>" + EscapeHtml(chatType) + "</b>");
            }

            if (!string.IsNullOrWhiteSpace(chatName))
            {
                sb.AppendLine("Name: <b>" + EscapeHtml(chatName) + "</b>");
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                sb.AppendLine("Username: @" + EscapeHtml(username));
            }

            sb.AppendLine();
            sb.AppendLine("<i>Untuk notifikasi assign, salin Chat ID ke Web.config:</i>");
            sb.AppendLine("<code>ItsSupportTelegramChatId</code>");

            SendHtmlMessage(apiToken, chatId, sb.ToString().TrimEnd());
        }

        private static string GetAppSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetParValue(string connString, string parCode)
        {
            try
            {
                Recordset rec = new Recordset();
                rec.Open("sp_list_par_global '" + EscapeSqlLiteral(parCode) + "'", connString);
                if (rec.RecordCount() > 0)
                {
                    return (rec.Fields("ParValue") ?? string.Empty).Trim();
                }
            }
            catch
            {
            }

            return string.Empty;
        }

        private static bool SendTelegramMessage(
            string apiToken,
            string chatId,
            string text,
            Dictionary<string, object> replyMarkup = null)
        {
            if (SendHtmlMessage(apiToken, chatId, text, replyMarkup))
            {
                return true;
            }

            return SendPlainMessage(apiToken, chatId, StripHtmlTags(text), replyMarkup);
        }

        private static bool SendHtmlMessage(
            string apiToken,
            string chatId,
            string text,
            Dictionary<string, object> replyMarkup = null)
        {
            if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(chatId) || string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            try
            {
                EnsureTls12();
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload["chat_id"] = BuildChatIdPayloadValue(chatId);
                payload["text"] = text;
                payload["parse_mode"] = "HTML";
                if (replyMarkup != null)
                {
                    payload["reply_markup"] = replyMarkup;
                }

                string response = CallTelegramApi(apiToken, "sendMessage", payload);
                return !string.IsNullOrWhiteSpace(response) && response.IndexOf("\"ok\":true", StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
                return false;
            }
        }

        private static bool SendPlainMessage(
            string apiToken,
            string chatId,
            string text,
            Dictionary<string, object> replyMarkup = null)
        {
            if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(chatId) || string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            try
            {
                EnsureTls12();
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload["chat_id"] = BuildChatIdPayloadValue(chatId);
                payload["text"] = text;
                if (replyMarkup != null)
                {
                    payload["reply_markup"] = replyMarkup;
                }

                string response = CallTelegramApi(apiToken, "sendMessage", payload);
                return !string.IsNullOrWhiteSpace(response) && response.IndexOf("\"ok\":true", StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
                return false;
            }
        }

        private static string StripHtmlTags(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            return System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", string.Empty);
        }

        private static object BuildChatIdPayloadValue(string chatId)
        {
            long parsed;
            if (long.TryParse((chatId ?? string.Empty).Trim(), out parsed))
            {
                return parsed;
            }

            return chatId;
        }

        private static string CallTelegramApi(string apiToken, string method, Dictionary<string, object> payload)
        {
            if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(method))
            {
                return string.Empty;
            }

            EnsureTls12();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = payload == null ? "{}" : serializer.Serialize(payload);
            string url = "https://api.telegram.org/bot" + apiToken.Trim() + "/" + method.Trim();

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            return client.UploadString(url, json);
        }

        private static string CallTelegramApiGet(string apiToken, string methodWithQuery)
        {
            if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(methodWithQuery))
            {
                return string.Empty;
            }

            EnsureTls12();
            string url = "https://api.telegram.org/bot" + apiToken.Trim() + "/" + methodWithQuery.Trim();
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            return client.DownloadString(url);
        }

        private static bool TryHandleSetupRequest(HttpContext context)
        {
            string setupKey = (context.Request["setup"] ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(setupKey))
            {
                return false;
            }

            string expectedKey = FirstNonEmpty(GetAppSetting("ItsSupportTelegramSetupKey"), "itssetup");
            if (!setupKey.Equals(expectedKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = 403;
                context.Response.Write("Invalid setup key.");
                return true;
            }

            string connString = ResolveConnString(context);
            TelegramConfig config = LoadTelegramConfig(connString);
            if (!config.HasApiToken)
            {
                context.Response.StatusCode = 200;
                context.Response.Write("Telegram bot token not configured in Web.config (ItsSupportTelegramApiToken).");
                return true;
            }

            string webhookUrl = FirstNonEmpty(
                GetAppSetting("ItsSupportTelegramWebhookUrl"),
                BuildWebhookUrl(context));

            StringBuilder report = new StringBuilder();
            report.AppendLine("IT Support Telegram setup");
            report.AppendLine("Webhook URL: " + webhookUrl);
            report.AppendLine();

            try
            {
                string setResult = CallTelegramApiGet(
                    config.ApiToken,
                    "setWebhook?url=" + Uri.EscapeDataString(webhookUrl));
                report.AppendLine("setWebhook:");
                report.AppendLine(setResult);
                report.AppendLine();

                string infoResult = CallTelegramApiGet(config.ApiToken, "getWebhookInfo");
                report.AppendLine("getWebhookInfo:");
                report.AppendLine(infoResult);
                report.AppendLine();

                string meResult = CallTelegramApiGet(config.ApiToken, "getMe");
                report.AppendLine("getMe:");
                report.AppendLine(meResult);
                report.AppendLine();
                report.AppendLine("Now open the bot in Telegram and send /help or /chatid.");
            }
            catch (Exception ex)
            {
                report.AppendLine("Setup failed: " + ex.Message);
            }

            context.Response.StatusCode = 200;
            context.Response.Write(report.ToString().TrimEnd());
            return true;
        }

        private static string BuildWebhookUrl(HttpContext context)
        {
            if (context == null || context.Request == null || context.Request.Url == null)
            {
                return string.Empty;
            }

            string appPath = (context.Request.ApplicationPath ?? "/").TrimEnd('/');
            if (appPath.Length == 0)
            {
                appPath = string.Empty;
            }

            return context.Request.Url.GetLeftPart(UriPartial.Authority) + appPath + "/telegram_itsupport_bot.ashx";
        }

        private static void AnswerCallbackQuery(string apiToken, string callbackQueryId)
        {
            if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(callbackQueryId))
            {
                return;
            }

            try
            {
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload["callback_query_id"] = callbackQueryId;
                CallTelegramApi(apiToken, "answerCallbackQuery", payload);
            }
            catch
            {
            }
        }

        private static DataTable ExecuteQuery(string connString, string sql)
        {
            if (string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }

            try
            {
                string trimmed = sql.Trim();
                if (trimmed.StartsWith("sp_", StringComparison.OrdinalIgnoreCase)
                    || trimmed.StartsWith("exec", StringComparison.OrdinalIgnoreCase))
                {
                    Recordset rec = new Recordset();
                    rec.Open(trimmed, connString);
                    DataTable spTable = rec.DataRecord();
                    return spTable != null && spTable.Rows.Count > 0 ? spTable : null;
                }

                string sqlConn = ResolveSqlClientConnectionString(connString);
                if (string.IsNullOrWhiteSpace(sqlConn))
                {
                    return null;
                }

                DataTable table = new DataTable();
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(trimmed, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 120;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }
                    }
                }

                return table.Rows.Count > 0 ? table : null;
            }
            catch
            {
                return null;
            }
        }

        private static string ResolveSqlClientConnectionString(string rawConnectionString)
        {
            if (string.IsNullOrWhiteSpace(rawConnectionString))
            {
                return string.Empty;
            }

            ConnectionStringSettings sqlSettings = ConfigurationManager.ConnectionStrings["VTSADMIN"];
            if (sqlSettings != null && !string.IsNullOrWhiteSpace(sqlSettings.ConnectionString))
            {
                return sqlSettings.ConnectionString.Trim();
            }

            string trimmed = rawConnectionString.Trim();
            if (trimmed.IndexOf("provider=", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    string[] parts = trimmed.Split(';');
                    StringBuilder builder = new StringBuilder();
                    foreach (string part in parts)
                    {
                        string item = (part ?? string.Empty).Trim();
                        if (item.Length == 0)
                        {
                            continue;
                        }

                        if (item.StartsWith("Provider=", StringComparison.OrdinalIgnoreCase)
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
                catch
                {
                    return string.Empty;
                }
            }

            return trimmed;
        }

        private static string ResolveConnString(HttpContext context)
        {
            if (context != null && context.Session != null)
            {
                string sessionConn = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
                if (!string.IsNullOrWhiteSpace(sessionConn))
                {
                    return sessionConn.Trim();
                }
            }

            ConnectionStringSettings sqlSettings = ConfigurationManager.ConnectionStrings["VTSADMIN"];
            if (sqlSettings != null && !string.IsNullOrWhiteSpace(sqlSettings.ConnectionString))
            {
                return sqlSettings.ConnectionString.Trim();
            }

            ConnectionStringSettings oleDbSettings = ConfigurationManager.ConnectionStrings["VTSAdminDB"];
            if (oleDbSettings != null && !string.IsNullOrWhiteSpace(oleDbSettings.ConnectionString))
            {
                return oleDbSettings.ConnectionString.Trim();
            }

            return string.Empty;
        }

        private static string EscapeSqlLiteral(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }

        private static string EscapeHtml(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        private static string FormatDateId(DateTime? value)
        {
            if (!value.HasValue)
            {
                return "-";
            }

            return value.Value.ToString("d MMMM yyyy", IndonesianCulture);
        }

        private static string FormatDayDateId(DateTime? value)
        {
            if (!value.HasValue)
            {
                return "-";
            }

            return value.Value.ToString("dddd dd/MM/yyyy", IndonesianCulture);
        }

        private static string FormatDateShort(DateTime? value)
        {
            if (!value.HasValue)
            {
                return "-";
            }

            return value.Value.ToString("dd MMM yyyy", IndonesianCulture);
        }

        private static string FormatDateValue(DateTime? value)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }

            return value.Value.ToString("yyyy-MM-dd");
        }

        private static DateTime? ParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            DateTime parsed;
            if (DateTime.TryParse(value, out parsed))
            {
                return parsed;
            }

            return null;
        }

        private static int ParseInt(string value, int defaultValue)
        {
            int parsed;
            if (int.TryParse((value ?? string.Empty).Trim(), out parsed))
            {
                return parsed;
            }

            return defaultValue;
        }

        private static string FirstNonEmpty(params string[] values)
        {
            if (values == null)
            {
                return string.Empty;
            }

            foreach (string value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return string.Empty;
        }

        private static string GetRowString(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
            {
                return string.Empty;
            }

            object value = row[columnName];
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
        }

        private static string GetDictString(Dictionary<string, object> dict, string key)
        {
            if (dict == null || !dict.ContainsKey(key) || dict[key] == null)
            {
                return string.Empty;
            }

            return Convert.ToString(dict[key]).Trim();
        }

        private static string GetChatIdFromMessage(Dictionary<string, object> message)
        {
            if (message == null || !message.ContainsKey("chat"))
            {
                return string.Empty;
            }

            Dictionary<string, object> chat = message["chat"] as Dictionary<string, object>;
            if (chat == null || !chat.ContainsKey("id") || chat["id"] == null)
            {
                return string.Empty;
            }

            object idValue = chat["id"];
            if (idValue is int || idValue is long || idValue is short || idValue is decimal || idValue is double)
            {
                return Convert.ToInt64(idValue).ToString(CultureInfo.InvariantCulture);
            }

            return Convert.ToString(idValue).Trim();
        }

        private static string TrimButtonLabel(string value, int maxLength)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength - 1) + "…";
        }

        private sealed class TelegramConfig
        {
            public string ApiToken { get; set; }
            public string ChatId { get; set; }
            public string UrlTemplate { get; set; }
            public bool HasApiToken { get; set; }
            public bool IsValid { get; set; }
        }

        private sealed class OpenAssignSummary
        {
            public string AssignId { get; set; }
            public int Seq { get; set; }
            public string JobId { get; set; }
            public DateTime? SchDate { get; set; }
            public string CustomerName { get; set; }
            public string TechnicianName { get; set; }
        }

        private sealed class AssignDetail
        {
            public string AssignId { get; set; }
            public int Seq { get; set; }
            public string JobId { get; set; }
            public string CustId { get; set; }
            public string CustomerName { get; set; }
            public string TechnicianId { get; set; }
            public string TechnicianName { get; set; }
            public string MarketingName { get; set; }
            public string Remark { get; set; }
            public string RemarkTraining { get; set; }
            public string PoId { get; set; }
            public string PoliceNo { get; set; }
            public string NoSn { get; set; }
            public string GsmNo { get; set; }
            public string JobType { get; set; }
            public string CategoryLabel { get; set; }
            public string CompanyLine { get; set; }
            public string BranchName { get; set; }
            public string AreaName { get; set; }
            public string Status { get; set; }
            public DateTime? SchDate { get; set; }
            public DateTime? ReqDate { get; set; }
            public DateTime? InstallDate { get; set; }
            public DateTime? TrainingScheduleDate { get; set; }
        }
    }
}
