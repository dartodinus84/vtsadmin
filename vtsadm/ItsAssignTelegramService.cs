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
        private const int WebhookEnsureIntervalSeconds = 60;
        private static readonly CultureInfo IndonesianCulture = CultureInfo.GetCultureInfo("id-ID");
        private static readonly object WebhookEnsureLock = new object();
        private static bool _tlsConfigured;
        private static DateTime lastWebhookEnsureUtc = DateTime.MinValue;

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
            DateTime schDate,
            string jobRemark = "")
        {
            AssignDetail detail = ResolveAssignNotificationDetail(
                connString,
                jobId,
                custId,
                technicianId,
                schDate);
            if (detail == null)
            {
                return;
            }

            ApplyJobOrderRemarkOverride(detail, jobRemark);
            SendNotificationMessage(connString, BuildDetailMessage(detail));
        }

        public static void NotifyAfterTransfer(
            string connString,
            string jobId,
            string custId,
            string fromTechnicianId,
            string toTechnicianId,
            DateTime schDate,
            string actionNote = "")
        {
            if (string.IsNullOrWhiteSpace(connString)
                || string.IsNullOrWhiteSpace(jobId)
                || string.IsNullOrWhiteSpace(fromTechnicianId)
                || string.IsNullOrWhiteSpace(toTechnicianId))
            {
                return;
            }

            AssignDetail detail = ResolveAssignNotificationDetail(
                connString,
                jobId,
                custId,
                toTechnicianId,
                schDate);
            if (detail == null)
            {
                return;
            }

            detail.PreviousTechnicianId = fromTechnicianId.Trim();
            detail.PreviousTechnicianName = ResolveItsSupportName(connString, fromTechnicianId);
            detail.ActionNote = (actionNote ?? string.Empty).Trim();
            SendNotificationMessage(connString, BuildTransferMessage(detail));
        }

        public static AssignDetail PrepareDeleteNotification(
            string connString,
            string assignId,
            int seq,
            string actionNote)
        {
            if (string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(assignId))
            {
                return null;
            }

            AssignDetail detail = LoadAssignDetail(connString, assignId, seq);
            if (detail == null)
            {
                return null;
            }

            detail.ActionNote = (actionNote ?? string.Empty).Trim();
            return detail;
        }

        public static void NotifyAfterDelete(string connString, AssignDetail detail)
        {
            if (detail == null || string.IsNullOrWhiteSpace(detail.JobId))
            {
                return;
            }

            SendNotificationMessage(connString, BuildDeleteMessage(detail));
        }

        private static AssignDetail ResolveAssignNotificationDetail(
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
                return null;
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
                return null;
            }

            return detail;
        }

        private static void SendNotificationMessage(string connString, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            EnsurePublicWebhook();
            TelegramConfig config = LoadTelegramConfig(connString);
            if (!config.IsValid)
            {
                return;
            }

            SendTelegramMessage(config.ApiToken, config.ChatId, text);
        }

        public static void EnsurePublicWebhook()
        {
            string webhookUrl = GetAppSetting("ItsSupportTelegramWebhookUrl");
            if (IsLocalOrPrivateWebhookUrl(webhookUrl))
            {
                return;
            }

            string apiToken = FirstNonEmpty(
                GetAppSetting("ItsSupportTelegramApiToken"),
                GetAppSetting("TelegramApi"));
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                return;
            }

            lock (WebhookEnsureLock)
            {
                if ((DateTime.UtcNow - lastWebhookEnsureUtc).TotalSeconds < WebhookEnsureIntervalSeconds)
                {
                    return;
                }

                lastWebhookEnsureUtc = DateTime.UtcNow;
            }

            try
            {
                CallTelegramApiGet(
                    apiToken,
                    "setWebhook?url=" + Uri.EscapeDataString(webhookUrl)
                    + "&allowed_updates=" + Uri.EscapeDataString(
                        "[\"message\",\"edited_message\",\"callback_query\"]"));
            }
            catch
            {
            }
        }

        private static string ResolveItsSupportName(string connString, string technicianId)
        {
            if (string.IsNullOrWhiteSpace(technicianId))
            {
                return string.Empty;
            }

            AssignDetail temp = new AssignDetail
            {
                TechnicianId = technicianId.Trim(),
                TechnicianName = technicianId.Trim()
            };
            EnrichFromItsSupport(connString, temp);
            return FirstNonEmpty(temp.TechnicianName, technicianId.Trim());
        }

        public static void ProcessWebhook(HttpContext context)
        {
            if (context == null)
            {
                return;
            }

            EnsureTls12();
            context.Response.ContentType = "text/plain; charset=utf-8";

            if (TryHandleSimulateRequest(context))
            {
                return;
            }

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
                    + "/open — penugasan IT Support yang sudah di-assign &amp; belum selesai\r\n"
                    + "/detail &lt;JobID&gt; [Seq] — detail penugasan\r\n"
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
                || text.StartsWith("/belumselesai", StringComparison.OrdinalIgnoreCase)
                || IsBotCommand(text, "/open")
                || IsBotCommand(text, "/list")
                || IsBotCommand(text, "/belumselesai"))
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
                    SendHtmlMessage(
                        config.ApiToken,
                        chatId,
                        "Format: /detail &lt;JobID&gt; [Seq]\r\n"
                        + "Contoh: /detail TRO0006127\r\n"
                        + "Atau: /detail TRO0006127 1");
                    return;
                }

                int seq = 1;
                if (parts.Length >= 3)
                {
                    int parsedSeq;
                    if (int.TryParse(parts[2].Trim(), out parsedSeq) && parsedSeq > 0)
                    {
                        seq = parsedSeq;
                    }
                }

                AssignDetail detail = LoadAssignDetailByJobId(connString, parts[1], seq);
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

            if (data.StartsWith("itsj:", StringComparison.OrdinalIgnoreCase)
                || data.StartsWith("itsd:", StringComparison.OrdinalIgnoreCase))
            {
                string[] parts = data.Split(':');
                if (parts.Length < 2)
                {
                    return;
                }

                if (data.StartsWith("itsj:", StringComparison.OrdinalIgnoreCase))
                {
                    string jobId = parts[1].Trim();
                    if (string.IsNullOrWhiteSpace(jobId))
                    {
                        return;
                    }

                    int seq = 1;
                    int parsedSeq;
                    if (parts.Length >= 3 && int.TryParse(parts[2], out parsedSeq) && parsedSeq > 0)
                    {
                        seq = parsedSeq;
                    }

                    AssignDetail detail = LoadAssignDetailByJobId(connString, jobId, seq);
                    if (detail == null)
                    {
                        SendHtmlMessage(config.ApiToken, chatId, "Data penugasan tidak ditemukan.");
                        return;
                    }

                    SendHtmlMessage(config.ApiToken, chatId, BuildDetailMessage(detail));
                }
                else
                {
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
            }
        }

        private static void SendOpenAssignList(string connString, string apiToken, string chatId)
        {
            List<OpenAssignSummary> rows = LoadOpenAssignSummaries(connString);
            if (rows.Count == 0)
            {
                SendHtmlMessage(apiToken, chatId, "Tidak ada penugasan IT Support yang sudah di-assign dan belum selesai.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<b>📋 Penugasan IT Support (belum selesai)</b>");
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
                button["callback_data"] = "itsj:" + row.JobId + ":" + row.Seq.ToString();
                keyboard.Add(new List<Dictionary<string, string>> { button });
            }

            Dictionary<string, object> replyMarkup = new Dictionary<string, object>();
            replyMarkup["inline_keyboard"] = keyboard;

            SendHtmlMessage(apiToken, chatId, sb.ToString().TrimEnd(), replyMarkup);
        }

        private static List<OpenAssignSummary> LoadOpenAssignSummaries(string connString)
        {
            List<OpenAssignSummary> list = new List<OpenAssignSummary>();
            DataTable table = ItsSupportAssignData.LoadOpenItSupportAssignRows(OpenListMaxRows, connString);
            if (table == null || table.Rows.Count == 0)
            {
                string[] queries =
                {
                    ItsSupportAssignData.BuildOpenItSupportAssignListSql(OpenListMaxRows),
                    ItsSupportAssignData.BuildOpenItSupportAssignBareSql(OpenListMaxRows),
                    ItsSupportAssignData.BuildOpenItSupportAssignListFallbackSql(OpenListMaxRows)
                };

                foreach (string sql in queries)
                {
                    table = ExecuteBotQuery(connString, sql);
                    if (table != null && table.Rows.Count > 0)
                    {
                        break;
                    }
                }
            }

            if (table == null || table.Rows.Count == 0)
            {
                return list;
            }

            Dictionary<string, string> itSupportNames = ItsSupportAssignData.LoadItSupportNameMap();
            foreach (DataRow row in table.Rows)
            {
                if (!ItsSupportAssignData.IsOpenItSupportAssignStatus(GetRowString(row, "Status")))
                {
                    continue;
                }

                string jobId = GetRowString(row, "JobID");
                string technicianId = GetRowString(row, "TechnicianID");
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    continue;
                }

                if (!ItsSupportAssignData.IsAssignedItSupportTechnician(technicianId, itSupportNames))
                {
                    continue;
                }

                OpenAssignSummary item = new OpenAssignSummary();
                item.AssignId = GetRowString(row, "AssignID");
                item.Seq = ParseInt(GetRowString(row, "Seq"), 1);
                item.JobId = jobId;
                item.SchDate = ParseDate(GetRowString(row, "SchDate"));
                item.CustomerName = ResolveOpenListCustomerDisplayName(
                    connString,
                    jobId,
                    FirstNonEmpty(
                        GetRowString(row, "CustomerName"),
                        "-"));
                item.TechnicianName = FirstNonEmpty(
                    GetRowString(row, "TechnicianName"),
                    ItsSupportAssignData.ResolveItSupportName(technicianId, itSupportNames),
                    ResolveItsSupportName(connString, technicianId),
                    technicianId,
                    "-");
                list.Add(item);
            }

            return list;
        }

        private static AssignDetail LoadAssignDetailByJobId(string connString, string jobId, int seq)
        {
            if (string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(jobId))
            {
                return null;
            }

            int requestedSeq = Math.Max(1, seq);
            DataTable keyTable = ItsSupportAssignData.FindAssignDetailKeyByJobId(
                jobId.Trim(),
                requestedSeq,
                connString);
            if (keyTable == null || keyTable.Rows.Count == 0)
            {
                keyTable = ItsSupportAssignData.FindAssignDetailKeyByJobId(jobId.Trim(), 0, connString);
            }

            if (keyTable == null || keyTable.Rows.Count == 0)
            {
                return null;
            }

            string assignId = GetRowString(keyTable.Rows[0], "AssignID");
            int resolvedSeq = ParseInt(GetRowString(keyTable.Rows[0], "Seq"), requestedSeq);
            string resolvedJobId = FirstNonEmpty(
                GetRowString(keyTable.Rows[0], "JobID"),
                jobId.Trim());
            if (string.IsNullOrWhiteSpace(assignId))
            {
                return null;
            }

            return LoadAssignDetail(connString, assignId, resolvedSeq, resolvedJobId);
        }

        private static AssignDetail LoadAssignDetail(string connString, string assignId, int seq)
        {
            return LoadAssignDetail(connString, assignId, seq, null);
        }

        private static AssignDetail LoadAssignDetail(
            string connString,
            string assignId,
            int seq,
            string knownJobId)
        {
            if (string.IsNullOrWhiteSpace(assignId))
            {
                return null;
            }

            int resolvedSeq = Math.Max(1, seq);
            AssignDetail detail = new AssignDetail
            {
                AssignId = assignId.Trim(),
                Seq = resolvedSeq,
                JobId = (knownJobId ?? string.Empty).Trim()
            };

            EnrichFromTrxRow(connString, detail, assignId, resolvedSeq);
            if (string.IsNullOrWhiteSpace(detail.JobId))
            {
                AssignDetail fromSp = TryLoadFromSp(connString, assignId);
                if (fromSp != null)
                {
                    detail = fromSp;
                    detail.AssignId = assignId.Trim();
                    detail.Seq = resolvedSeq;
                }
            }

            if (string.IsNullOrWhiteSpace(detail.JobId) && !string.IsNullOrWhiteSpace(knownJobId))
            {
                detail.JobId = knownJobId.Trim();
            }

            EnrichFromAssignHeader(connString, detail);
            EnrichFromTrainingOrder(connString, detail);
            EnrichFromCustomer(connString, detail);
            EnrichFromItsSupport(connString, detail);

            if (!detail.AssignDate.HasValue && !string.IsNullOrWhiteSpace(detail.AssignId))
            {
                string assignDateText = ItsSupportAssignData.ResolveItSupportAssignDate(
                    connString,
                    detail.AssignId,
                    detail.Seq,
                    detail.JobId);
                detail.AssignDate = ParseDate(assignDateText);
            }

            if (string.IsNullOrWhiteSpace(detail.JobId) && !string.IsNullOrWhiteSpace(knownJobId))
            {
                detail.JobId = knownJobId.Trim();
            }

            if (string.IsNullOrWhiteSpace(detail.JobId))
            {
                return null;
            }

            string resolvedCustomerName = ResolveBotCustomerDisplayName(detail);
            if (!string.IsNullOrWhiteSpace(resolvedCustomerName) && !resolvedCustomerName.Equals("-", StringComparison.Ordinal))
            {
                detail.CustomerName = resolvedCustomerName;
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
            DataTable table = ItsSupportAssignData.LoadAssignDetailRow(assignId, seq, connString);
            if (table == null || table.Rows.Count == 0)
            {
                table = ExecuteBotQuery(
                    connString,
                    "SELECT TOP 1 "
                    + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                    + "ISNULL(Seq, 0) AS Seq, "
                    + "LTRIM(RTRIM(ISNULL(JobID, ''))) AS JobID, "
                    + "LTRIM(RTRIM(ISNULL(TechnicianID, ''))) AS TechnicianID, "
                    + "SchDate, "
                    + "LTRIM(RTRIM(ISNULL(Remark, ''))) AS Remark, "
                    + "LTRIM(RTRIM(ISNULL(Status, ''))) AS Status, "
                    + "LTRIM(RTRIM(ISNULL(DeviceTypeID, ''))) AS DeviceTypeID, "
                    + "DtmUpd "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE AssignID = '" + EscapeSqlLiteral(assignId.Trim()) + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE') "
                    + "ORDER BY Seq DESC");
            }
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            DataRow row = table.Rows[0];
            detail.AssignId = FirstNonEmpty(GetRowString(row, "AssignID"), detail.AssignId);
            detail.Seq = ParseInt(GetRowString(row, "Seq"), detail.Seq);
            detail.JobId = FirstNonEmpty(GetRowString(row, "JobID"), detail.JobId);
            detail.TechnicianId = FirstNonEmpty(
                GetRowString(row, "TechnicianID"),
                detail.TechnicianId);
            detail.TechnicianName = FirstNonEmpty(
                detail.TechnicianName,
                detail.TechnicianId);
            detail.SchDate = ParseDate(FirstNonEmpty(GetRowString(row, "SchDate"), FormatDateValue(detail.SchDate)));
            detail.AssignDate = ParseDate(FirstNonEmpty(GetRowString(row, "DtmUpd"), FormatDateValue(detail.AssignDate)));
            detail.Remark = FirstNonEmpty(GetRowString(row, "Remark"), detail.Remark);
            detail.PoliceNo = FirstNonEmpty(GetRowString(row, "PoliceNo"), detail.PoliceNo);
            detail.NoSn = FirstNonEmpty(GetRowString(row, "NoSN"), detail.NoSn);
            detail.GsmNo = FirstNonEmpty(GetRowString(row, "GSMNo"), detail.GsmNo);
            detail.JobType = FirstNonEmpty(GetRowString(row, "JobType"), GetRowString(row, "DeviceTypeDesc"), detail.JobType);
            detail.InstallDate = ParseDate(FirstNonEmpty(GetRowString(row, "InstallDate"), GetRowString(row, "MIS_Date"), FormatDateValue(detail.InstallDate)));
            detail.AreaName = FirstNonEmpty(GetRowString(row, "AreaName"), detail.AreaName);
            detail.Status = FirstNonEmpty(GetRowString(row, "Status"), detail.Status);
            string assignCategoryId = FirstNonEmpty(
                GetRowString(row, "DeviceTypeID"),
                GetRowString(row, "TrainCategoryID"),
                GetRowString(row, "InsDeviceTypeID"));
            if (!string.IsNullOrWhiteSpace(assignCategoryId)
                && !assignCategoryId.Equals("[Select]", StringComparison.OrdinalIgnoreCase)
                && !assignCategoryId.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                detail.TrainCategoryId = FirstNonEmpty(detail.TrainCategoryId, assignCategoryId);
            }
        }

        private static void EnrichFromCustomer(string connString, AssignDetail detail)
        {
            if (detail == null)
            {
                return;
            }

            ItsSupportAssignData.CustomerContact contact = ItsSupportAssignData.LoadCustomerContact(
                detail.CustId,
                detail.JobId,
                connString);
            if (contact == null)
            {
                if (string.IsNullOrWhiteSpace(detail.MarketingName) && !string.IsNullOrWhiteSpace(detail.CustId))
                {
                    detail.MarketingName = ItsSupportAssignData.LookupMarketingNameByCustId(detail.CustId);
                }
                return;
            }

            detail.CustId = FirstNonEmpty(detail.CustId, contact.CustId);
            if (!string.IsNullOrWhiteSpace(contact.FullName))
            {
                detail.CustomerName = contact.FullName;
                detail.CompanyLine = contact.FullName + "(" + FirstNonEmpty(contact.CustId, detail.CustId) + ")";
            }

            detail.BranchName = FirstNonEmpty(contact.BranchName, detail.BranchName);
            detail.MarketingName = FirstNonEmpty(
                contact.MarketingName,
                detail.MarketingName,
                ItsSupportAssignData.LookupMarketingNameByCustId(detail.CustId));
            detail.Address = FirstNonEmpty(contact.Address, detail.Address);
            detail.PicName = FirstNonEmpty(contact.PicName, detail.PicName);
            detail.CustomerNumber = FirstNonEmpty(contact.OfficePhone1);
            detail.Latitude = FirstNonEmpty(contact.Lat);
            detail.Longitude = FirstNonEmpty(contact.Long);
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

            DataTable table = ExecuteBotQuery(connString, sql);
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
                "SELECT TOP 1 * "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(ITID, ''))) = '" + safeKey + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL')",
                "SELECT TOP 1 * "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + safeKey + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL')"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteBotQuery(connString, sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                DataRow row = table.Rows[0];
                string name = GetRowString(row, "Name");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    detail.TechnicianName = name;
                }

                string telegram = FirstNonEmpty(
                    GetRowString(row, "Telegram"),
                    GetRowString(row, "TelegramID"),
                    GetRowString(row, "TelegramName"),
                    GetRowString(row, "TelegramUser"),
                    GetRowString(row, "TelegramUsername"),
                    GetRowString(row, "TelegramAccount"));
                if (!string.IsNullOrWhiteSpace(telegram))
                {
                    detail.TechnicianTelegram = telegram;
                }

                if (!string.IsNullOrWhiteSpace(detail.TechnicianName)
                    || !string.IsNullOrWhiteSpace(detail.TechnicianTelegram))
                {
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
                "SELECT TOP 1 * FROM trx_training_order WITH (NOLOCK) WHERE TrainingID = '" + jobId + "'",
                "SELECT TOP 1 t.TrainingID, t.CustID, t.ScheduleDate, t.Remark, t.RemarkTraining, t.BranchName, "
                    + "t.TrainCategoryID, cat.TrainCategoryDesc "
                    + "FROM trx_training_order t WITH (NOLOCK) "
                    + "LEFT JOIN ref_train_category cat WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(cat.TrainCategoryID, ''))) = LTRIM(RTRIM(ISNULL(t.TrainCategoryID, ''))) "
                    + "WHERE t.TrainingID = '" + jobId + "'",
                "SELECT TOP 1 TrainingID, CustID, ScheduleDate, Remark, RemarkTraining, BranchName, "
                    + "TrainCategoryID, TrainingCategoryName, TrainCategoryName, CategoryName "
                    + "FROM trx_training_order WITH (NOLOCK) WHERE TrainingID = '" + jobId + "'",
                "SELECT TOP 1 TrainingID, CustID, ScheduleDate, Remark, BranchName, TrainCategoryID "
                    + "FROM trx_training_order WITH (NOLOCK) WHERE TrainingID = '" + jobId + "'"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteBotQuery(connString, sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                DataRow row = table.Rows[0];
                detail.CustId = FirstNonEmpty(GetRowString(row, "CustID"), detail.CustId);
                detail.TrainingScheduleDate = ParseDate(GetRowString(row, "ScheduleDate"));
                detail.RemarkOrder = FirstNonEmpty(
                    GetAnyTrainingRemark(row),
                    detail.RemarkOrder);
                detail.RemarkTraining = FirstNonEmpty(
                    GetRowString(row, "RemarkTraining"),
                    GetAnyTrainingRemark(row),
                    detail.RemarkTraining);
                detail.Remark = FirstNonEmpty(GetAnyTrainingRemark(row), detail.Remark);
                detail.TrainCategoryId = FirstNonEmpty(
                    GetRowString(row, "TrainCategoryID"),
                    GetRowString(row, "TrainingCategoryID"),
                    detail.TrainCategoryId);

                string category = FirstNonEmpty(
                    GetRowString(row, "TrainCategoryDesc"),
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

        private static string ResolveBotCustomerDisplayName(AssignDetail detail)
        {
            if (detail == null)
            {
                return "-";
            }

            string custId = FirstNonEmpty(detail.CustId, string.Empty).Trim();
            string name = FirstNonEmpty(detail.CustomerName, string.Empty).Trim();

            if (!string.IsNullOrWhiteSpace(name) && name.IndexOf(" - ", StringComparison.Ordinal) >= 0)
            {
                string[] parts = name.Split(new[] { " - " }, 2, StringSplitOptions.None);
                if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[1]))
                {
                    name = parts[1].Trim();
                }
            }

            if (!string.IsNullOrWhiteSpace(name) && name.Contains("("))
            {
                int openIndex = name.LastIndexOf('(');
                if (openIndex > 0 && name.EndsWith(")", StringComparison.Ordinal))
                {
                    name = name.Substring(0, openIndex).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(name)
                || name == "-"
                || (!string.IsNullOrWhiteSpace(custId)
                    && name.Equals(custId, StringComparison.OrdinalIgnoreCase)))
            {
                return "-";
            }

            return name;
        }

        private static string ResolveOpenListCustomerDisplayName(string connString, string jobId, string rawName)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return "-";
            }

            string safeJobId = EscapeSqlLiteral(jobId.Trim());
            DataTable table = ExecuteBotQuery(
                connString,
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(c.FullName, ''))) AS FullName "
                + "FROM trx_training_order t WITH (NOLOCK) "
                + "LEFT JOIN mst_customer c WITH (NOLOCK) ON c.CustID = t.CustID "
                + "WHERE t.TrainingID = '" + safeJobId + "' "
                + "AND ISNULL(t.Status, '') NOT IN ('DE')");
            if (table != null && table.Rows.Count > 0)
            {
                string fullName = GetRowString(table.Rows[0], "FullName").Trim();
                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    return fullName;
                }
            }

            AssignDetail temp = new AssignDetail
            {
                CustId = string.Empty,
                CustomerName = rawName
            };
            string resolved = ResolveBotCustomerDisplayName(temp);
            return string.IsNullOrWhiteSpace(resolved) || resolved == "-" ? "-" : resolved;
        }

        private static void AppendBotCustomerContactSection(StringBuilder sb, AssignDetail detail)
        {
            string alamat = FirstNonEmpty(detail != null ? detail.Address : string.Empty, "-");
            string picCustomer = FirstNonEmpty(detail != null ? detail.PicName : string.Empty, "-");
            string officePhone = FirstNonEmpty(detail != null ? detail.CustomerNumber : string.Empty, "-");
            sb.AppendLine("Alamat : <b>" + EscapeHtml(alamat) + "</b>");
            sb.AppendLine("PIC Customer : <b>" + EscapeHtml(picCustomer) + "</b>");
            sb.AppendLine("No. Telp : <b>" + EscapeHtml(officePhone) + "</b>");
            AppendBotGoogleMapsLine(sb, detail);
        }

        private static void AppendBotGoogleMapsLine(StringBuilder sb, AssignDetail detail)
        {
            if (detail == null)
            {
                return;
            }

            string mapsUrl = ItsSupportAssignData.BuildGoogleMapsUrl(detail.Latitude, detail.Longitude);
            if (string.IsNullOrWhiteSpace(mapsUrl))
            {
                return;
            }

            sb.AppendLine("Lokasi : <a href=\"" + EscapeHtml(mapsUrl) + "\">Google Maps</a>");
        }

        private static void AppendBotMarketingLine(StringBuilder sb, AssignDetail detail)
        {
            string marketingName = FirstNonEmpty(detail != null ? detail.MarketingName : string.Empty, "-");
            sb.AppendLine("Marketing : <b>" + EscapeHtml(marketingName) + "</b>");
        }

        private static void AppendTelegramUserTag(StringBuilder sb, AssignDetail detail)
        {
            string tag = FormatTelegramMention(detail == null ? string.Empty : detail.TechnicianTelegram);
            if (string.IsNullOrWhiteSpace(tag))
            {
                return;
            }

            sb.AppendLine();
            sb.AppendLine(EscapeHtml(tag));
        }

        private static string FormatTelegramMention(string raw)
        {
            string value = (raw ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            int spaceIndex = value.IndexOf(' ');
            if (spaceIndex > 0)
            {
                value = value.Substring(0, spaceIndex).Trim();
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (value[0] != '@')
            {
                value = "@" + value.TrimStart('@');
            }

            return value;
        }

        private static DateTime? ResolveBotAssignDate(AssignDetail detail)
        {
            if (detail == null)
            {
                return null;
            }

            if (detail.AssignDate.HasValue)
            {
                return detail.AssignDate;
            }

            return detail.ReqDate;
        }

        private static string BuildDetailMessage(AssignDetail detail)
        {
            string category = ResolveCategoryLabel(detail);
            StringBuilder sb = new StringBuilder();
            string customerName = ResolveBotCustomerDisplayName(detail);
            string itSupportName = FirstNonEmpty(detail.TechnicianName, "-");
            string remark = ResolveJobOrderCatatan(detail);

            sb.AppendLine("🛠 <b>NOTIFIKASI PENUGASAN PEKERJAAN</b>");
            sb.AppendLine("<b>" + EscapeHtml(category) + "━━━━━━━━━━━━━━</b>");
            sb.AppendLine();
            sb.AppendLine("Nomor JO : <b>" + EscapeHtml(detail.JobId) + "</b>");
            sb.AppendLine("Tanggal Assign : <b>" + EscapeHtml(FormatDateId(ResolveBotAssignDate(detail))) + "</b>");
            sb.AppendLine("Tanggal Jadwal : <b>" + EscapeHtml(FormatDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            sb.AppendLine();
            sb.AppendLine("Pelanggan : <b>" + EscapeHtml(customerName) + "</b>");
            sb.AppendLine("IT Support : <b>" + EscapeHtml(itSupportName) + "</b>");
            AppendBotMarketingLine(sb, detail);
            sb.AppendLine();
            sb.AppendLine("<b>Catatan :</b>");
            sb.AppendLine(EscapeHtml(remark));
            sb.AppendLine();
            sb.AppendLine("Hari/tanggal : <b>" + EscapeHtml(FormatDayDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            AppendBotCustomerContactSection(sb, detail);
            AppendTelegramUserTag(sb, detail);

            return sb.ToString().TrimEnd();
        }

        private static string BuildTransferMessage(AssignDetail detail)
        {
            string category = ResolveCategoryLabel(detail);
            StringBuilder sb = new StringBuilder();
            string customerName = ResolveBotCustomerDisplayName(detail);
            string fromItSupport = FirstNonEmpty(detail.PreviousTechnicianName, detail.PreviousTechnicianId, "-");
            string toItSupport = FirstNonEmpty(detail.TechnicianName, detail.TechnicianId, "-");
            string remark = ResolveJobOrderCatatan(detail);

            sb.AppendLine("🔁 <b>NOTIFIKASI PINDAH " + EscapeHtml(category.ToUpperInvariant()) + "</b>");
            sb.AppendLine("<b>" + EscapeHtml(category) + "━━━━━━━━━━━━━━</b>");
            sb.AppendLine();
            sb.AppendLine("Jenis : <b>" + EscapeHtml(category) + "</b>");
            sb.AppendLine("Nomor JO : <b>" + EscapeHtml(detail.JobId) + "</b>");
            sb.AppendLine("Tanggal Assign : <b>" + EscapeHtml(FormatDateId(ResolveBotAssignDate(detail))) + "</b>");
            sb.AppendLine("Tanggal Jadwal : <b>" + EscapeHtml(FormatDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            sb.AppendLine();
            sb.AppendLine("Pelanggan : <b>" + EscapeHtml(customerName) + "</b>");
            sb.AppendLine("Dari IT Support : <b>" + EscapeHtml(fromItSupport) + "</b>");
            sb.AppendLine("Ke IT Support : <b>" + EscapeHtml(toItSupport) + "</b>");
            AppendBotMarketingLine(sb, detail);
            sb.AppendLine();
            sb.AppendLine("<b>Catatan :</b>");
            sb.AppendLine(EscapeHtml(remark));
            sb.AppendLine();
            sb.AppendLine("Hari/tanggal : <b>" + EscapeHtml(FormatDayDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            AppendBotCustomerContactSection(sb, detail);
            sb.AppendLine("Reason Move : <b>" + EscapeHtml(ResolveActionNoteForMessage(detail)) + "</b>");
            AppendTelegramUserTag(sb, detail);

            return sb.ToString().TrimEnd();
        }

        private static string BuildDeleteMessage(AssignDetail detail)
        {
            string category = ResolveCategoryLabel(detail);
            StringBuilder sb = new StringBuilder();
            string customerName = ResolveBotCustomerDisplayName(detail);
            string itSupportName = FirstNonEmpty(detail.TechnicianName, detail.TechnicianId, "-");
            string remark = ResolveJobOrderCatatan(detail);

            sb.AppendLine("🗑 <b>NOTIFIKASI HAPUS " + EscapeHtml(category.ToUpperInvariant()) + "</b>");
            sb.AppendLine("<b>" + EscapeHtml(category) + "━━━━━━━━━━━━━━</b>");
            sb.AppendLine();
            sb.AppendLine("Jenis : <b>" + EscapeHtml(category) + "</b>");
            sb.AppendLine("Nomor JO : <b>" + EscapeHtml(detail.JobId) + "</b>");
            sb.AppendLine("Tanggal Assign : <b>" + EscapeHtml(FormatDateId(ResolveBotAssignDate(detail))) + "</b>");
            sb.AppendLine("Tanggal Jadwal : <b>" + EscapeHtml(FormatDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            sb.AppendLine();
            sb.AppendLine("Pelanggan : <b>" + EscapeHtml(customerName) + "</b>");
            sb.AppendLine("IT Support : <b>" + EscapeHtml(itSupportName) + "</b>");
            AppendBotMarketingLine(sb, detail);
            sb.AppendLine();
            sb.AppendLine("<b>Catatan :</b>");
            sb.AppendLine(EscapeHtml(remark));
            sb.AppendLine();
            sb.AppendLine("Hari/tanggal : <b>" + EscapeHtml(FormatDayDateId(detail.SchDate ?? detail.TrainingScheduleDate)) + "</b>");
            AppendBotCustomerContactSection(sb, detail);
            sb.AppendLine("Reason Delete : <b>" + EscapeHtml(ResolveActionNoteForMessage(detail)) + "</b>");
            AppendTelegramUserTag(sb, detail);

            return sb.ToString().TrimEnd();
        }

        private static string ResolveCategoryLabel(AssignDetail detail)
        {
            string fromId = LookupTrainCategoryDesc(detail);
            if (!string.IsNullOrWhiteSpace(fromId))
            {
                return fromId;
            }

            string classified = ClassifyTrainVisitLabel(detail == null ? string.Empty : detail.CategoryLabel);
            if (!string.IsNullOrWhiteSpace(classified))
            {
                return classified;
            }

            return FirstNonEmpty(detail == null ? string.Empty : detail.CategoryLabel, "Training/Visit");
        }

        private static string LookupTrainCategoryDesc(AssignDetail detail)
        {
            if (detail == null)
            {
                return string.Empty;
            }

            string categoryId = FirstNonEmpty(detail.TrainCategoryId);
            if (categoryId.Equals("[Select]", StringComparison.OrdinalIgnoreCase)
                || categoryId.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                categoryId = string.Empty;
            }

            if (categoryId.Equals("TRC0000001", StringComparison.OrdinalIgnoreCase))
            {
                return "Training";
            }

            if (categoryId.Equals("TRC0000002", StringComparison.OrdinalIgnoreCase))
            {
                return "Visit";
            }

            return ClassifyTrainVisitLabel(FirstNonEmpty(detail.CategoryLabel, detail.JobType));
        }

        private static string ClassifyTrainVisitLabel(string raw)
        {
            string text = (raw ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            if (text.Equals("Training", StringComparison.OrdinalIgnoreCase))
            {
                return "Training";
            }

            if (text.Equals("Visit", StringComparison.OrdinalIgnoreCase))
            {
                return "Visit";
            }

            string lower = text.ToLowerInvariant();
            bool hasTrain = lower.IndexOf("train", StringComparison.Ordinal) >= 0;
            bool hasVisit = lower.IndexOf("visit", StringComparison.Ordinal) >= 0;
            if (hasTrain && hasVisit)
            {
                return string.Empty;
            }

            if (hasTrain)
            {
                return "Training";
            }

            if (hasVisit)
            {
                return "Visit";
            }

            return string.Empty;
        }

        private static string ResolveJobOrderCatatan(AssignDetail detail)
        {
            if (detail == null)
            {
                return "-";
            }

            return FirstNonEmpty(
                StripAssigneeTagsFromRemark(detail.RemarkOrder),
                StripAssigneeTagsFromRemark(detail.RemarkTraining),
                StripAssigneeTagsFromRemark(detail.Remark),
                "-");
        }

        private static void ApplyJobOrderRemarkOverride(AssignDetail detail, string jobRemark)
        {
            string cleaned = StripAssigneeTagsFromRemark(jobRemark);
            if (detail == null || string.IsNullOrWhiteSpace(cleaned))
            {
                return;
            }

            detail.RemarkOrder = cleaned;
            detail.Remark = FirstNonEmpty(cleaned, detail.Remark);
        }

        private static string GetAnyTrainingRemark(DataRow row)
        {
            if (row == null || row.Table == null)
            {
                return string.Empty;
            }

            string direct = FirstNonEmpty(
                GetRowString(row, "Remark"),
                GetRowString(row, "Remarks"),
                GetRowString(row, "RemarkTraining"),
                GetRowString(row, "RemarkOrder"),
                GetRowString(row, "Catatan"));
            if (!string.IsNullOrWhiteSpace(direct))
            {
                return direct;
            }

            foreach (DataColumn column in row.Table.Columns)
            {
                string name = column.ColumnName ?? string.Empty;
                if (name.IndexOf("remark", StringComparison.OrdinalIgnoreCase) < 0
                    && name.IndexOf("catatan", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                string value = GetRowString(row, name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return string.Empty;
        }

        private static string StripAssigneeTagsFromRemark(string remark)
        {
            string raw = (remark ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return string.Empty;
            }

            string[] parts = raw.Split('|');
            List<string> kept = new List<string>();
            foreach (string part in parts)
            {
                string text = (part ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                if (text.StartsWith("ITID:", StringComparison.OrdinalIgnoreCase)
                    || text.StartsWith("ITNAME:", StringComparison.OrdinalIgnoreCase)
                    || text.StartsWith("IT:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                kept.Add(text);
            }

            return kept.Count == 0 ? string.Empty : string.Join(" | ", kept.ToArray());
        }

        private static string ResolveActionNoteForMessage(AssignDetail detail)
        {
            return FirstNonEmpty(detail.ActionNote, "-");
        }

        private static bool TryResolveAssignDetailKey(
            string connString,
            string input,
            string optionalSeqText,
            out string assignId,
            out int seq)
        {
            assignId = string.Empty;
            seq = 1;

            string key = (input ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(optionalSeqText))
            {
                int parsedSeq;
                if (int.TryParse(optionalSeqText.Trim(), out parsedSeq) && parsedSeq > 0)
                {
                    seq = parsedSeq;
                }
            }

            DataTable byJob = ItsSupportAssignData.FindAssignDetailKeyByJobId(key, seq, connString);
            if (byJob != null && byJob.Rows.Count > 0)
            {
                assignId = GetRowString(byJob.Rows[0], "AssignID");
                seq = ParseInt(GetRowString(byJob.Rows[0], "Seq"), seq);
                return !string.IsNullOrWhiteSpace(assignId);
            }

            if (seq > 0)
            {
                byJob = ItsSupportAssignData.FindAssignDetailKeyByJobId(key, 0, connString);
                if (byJob != null && byJob.Rows.Count > 0)
                {
                    assignId = GetRowString(byJob.Rows[0], "AssignID");
                    seq = ParseInt(GetRowString(byJob.Rows[0], "Seq"), seq);
                    return !string.IsNullOrWhiteSpace(assignId);
                }
            }

            if (AssignDetailRowExists(connString, key, seq))
            {
                assignId = key;
                return true;
            }

            DataTable byAssign = ItsSupportAssignData.FindAssignDetailKeyByAssignId(key, connString);
            if (byAssign != null && byAssign.Rows.Count > 0)
            {
                assignId = GetRowString(byAssign.Rows[0], "AssignID");
                seq = ParseInt(GetRowString(byAssign.Rows[0], "Seq"), seq);
                return !string.IsNullOrWhiteSpace(assignId);
            }

            return false;
        }

        private static bool AssignDetailRowExists(string connString, string assignId, int seq)
        {
            DataTable table = ItsSupportAssignData.LoadAssignDetailRow(assignId, seq, connString);
            return table != null && table.Rows.Count > 0;
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
                DataTable table = ItsSupportAssignData.QueryDataTable(sql);
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
            return !string.IsNullOrWhiteSpace(response)
                && response.Replace(" ", string.Empty).IndexOf("\"ok\":true", StringComparison.OrdinalIgnoreCase) >= 0;
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
            return !string.IsNullOrWhiteSpace(response)
                && response.Replace(" ", string.Empty).IndexOf("\"ok\":true", StringComparison.OrdinalIgnoreCase) >= 0;
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

        private static bool TryHandleSimulateRequest(HttpContext context)
        {
            string simulate = (context.Request["simulate"] ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(simulate))
            {
                return false;
            }

            string setupKey = (context.Request["setup"] ?? string.Empty).Trim();
            string expectedKey = FirstNonEmpty(GetAppSetting("ItsSupportTelegramSetupKey"), "itssetup");
            if (!setupKey.Equals(expectedKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = 403;
                context.Response.Write("Invalid setup key.");
                return true;
            }

            string connString = ResolveConnString(context);
            StringBuilder report = new StringBuilder();
            report.AppendLine("IT Support Telegram simulate (local test, no Telegram send)");
            report.AppendLine("DB connection: "
                + (string.IsNullOrWhiteSpace(connString) ? "NOT CONFIGURED" : "OK"));
            report.AppendLine();

            if (string.IsNullOrWhiteSpace(connString))
            {
                context.Response.StatusCode = 200;
                context.Response.Write(report.ToString().TrimEnd());
                return true;
            }

            if (simulate.Equals("open", StringComparison.OrdinalIgnoreCase))
            {
                List<OpenAssignSummary> rows = LoadOpenAssignSummaries(connString);
                report.AppendLine("/open bot summaries: " + rows.Count.ToString());
                if (rows.Count == 0)
                {
                    report.AppendLine("Bot would reply: Tidak ada penugasan IT Support yang sudah di-assign dan belum selesai.");
                }
                else
                {
                    int index = 1;
                    foreach (OpenAssignSummary row in rows)
                    {
                        report.AppendLine(index.ToString() + ". "
                            + row.JobId + " | "
                            + FormatDateShort(row.SchDate) + " | "
                            + row.CustomerName + " | "
                            + row.TechnicianName);
                        index++;
                    }
                }
            }
            else if (simulate.Equals("detail", StringComparison.OrdinalIgnoreCase))
            {
                string jobId = FirstNonEmpty(context.Request["job"], "TRO0006127").Trim();
                AssignDetail detail = LoadAssignDetailByJobId(connString, jobId, 1);
                report.AppendLine("/detail " + jobId + " bot load: "
                    + (detail == null ? "NOT FOUND" : "OK"));
                if (detail != null)
                {
                    report.AppendLine();
                    report.AppendLine(BuildDetailMessage(detail));
                }
            }
            else
            {
                report.AppendLine("Unknown simulate command.");
                report.AppendLine("Usage:");
                report.AppendLine("  ?setup=itssetup&simulate=open");
                report.AppendLine("  ?setup=itssetup&simulate=detail&job=TRO0006127");
            }

            context.Response.StatusCode = 200;
            context.Response.Write(report.ToString().TrimEnd());
            return true;
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

            string webhookUrl = ResolvePublicWebhookUrl(context);
            bool isLocalWebhook = IsLocalOrPrivateWebhookUrl(webhookUrl);

            StringBuilder report = new StringBuilder();
            report.AppendLine("IT Support Telegram setup");
            report.AppendLine("Webhook URL: " + webhookUrl);
            report.AppendLine("DB connection: "
                + (string.IsNullOrWhiteSpace(connString) ? "NOT CONFIGURED" : "OK"));
            if (!string.IsNullOrWhiteSpace(connString))
            {
                DataTable openRows = ItsSupportAssignData.LoadOpenItSupportAssignRows(5, connString);
                int openCount = openRows != null ? openRows.Rows.Count : 0;
                report.AppendLine("/open sample rows: " + openCount.ToString());

                List<OpenAssignSummary> botOpenRows = LoadOpenAssignSummaries(connString);
                report.AppendLine("/open bot summaries: " + botOpenRows.Count.ToString());

                DataTable detailKey = ItsSupportAssignData.FindAssignDetailKeyByJobId("TRO0006127", 0, connString);
                int detailCount = detailKey != null ? detailKey.Rows.Count : 0;
                report.AppendLine("/detail lookup TRO0006127: " + detailCount.ToString());

                AssignDetail detail = LoadAssignDetailByJobId(connString, "TRO0006127", 1);
                report.AppendLine("/detail bot load TRO0006127: "
                    + (detail == null ? "NOT FOUND" : "OK"));
            }
            report.AppendLine();
            report.AppendLine("Local bot test (no Telegram):");
            report.AppendLine("  " + BuildWebhookUrl(context) + "?setup=itssetup&simulate=open");
            report.AppendLine("  " + BuildWebhookUrl(context) + "?setup=itssetup&simulate=detail&job=TRO0006127");
            report.AppendLine();
            report.AppendLine("NOTE: Telegram app sends /help /open to the registered public webhook, not localhost.");
            report.AppendLine();

            string skipRegister = (context.Request["register"] ?? string.Empty).Trim();
            bool skipByDefault = isLocalWebhook && string.IsNullOrWhiteSpace(skipRegister);
            if (skipByDefault
                || skipRegister == "0"
                || skipRegister.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                report.AppendLine(isLocalWebhook
                    ? "Skipped setWebhook on localhost so Telegram keeps the public webhook."
                    : "Skipped setWebhook (register=0).");
                context.Response.StatusCode = 200;
                context.Response.Write(report.ToString().TrimEnd());
                return true;
            }

            if (isLocalWebhook)
            {
                report.AppendLine("Refused setWebhook: Telegram cannot reach localhost/http URLs.");
                context.Response.StatusCode = 200;
                context.Response.Write(report.ToString().TrimEnd());
                return true;
            }

            try
            {
                string setResult = CallTelegramApiGet(
                    config.ApiToken,
                    "setWebhook?url=" + Uri.EscapeDataString(webhookUrl)
                    + "&allowed_updates=" + Uri.EscapeDataString(
                        "[\"message\",\"edited_message\",\"callback_query\"]"));
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

        private static string ResolvePublicWebhookUrl(HttpContext context)
        {
            string configured = GetAppSetting("ItsSupportTelegramWebhookUrl");
            if (!IsLocalOrPrivateWebhookUrl(configured))
            {
                return configured;
            }

            string built = BuildWebhookUrl(context);
            if (!IsLocalOrPrivateWebhookUrl(built))
            {
                return built;
            }

            return FirstNonEmpty(configured, built);
        }

        private static bool IsLocalOrPrivateWebhookUrl(string webhookUrl)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                return true;
            }

            Uri uri;
            if (!Uri.TryCreate(webhookUrl.Trim(), UriKind.Absolute, out uri))
            {
                return true;
            }

            if (!string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string host = (uri.Host ?? string.Empty).Trim().ToLowerInvariant();
            return host == "localhost"
                || host == "127.0.0.1"
                || host == "::1"
                || host.EndsWith(".local", StringComparison.Ordinal)
                || host.StartsWith("192.168.", StringComparison.Ordinal)
                || host.StartsWith("10.", StringComparison.Ordinal)
                || host.StartsWith("172.16.", StringComparison.Ordinal);
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

        private static DataTable ExecuteBotQuery(string connString, string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(connString))
            {
                DataTable dashboardTable = dashboard_assign_job.ExecuteJobTrainingQuery(sql, connString);
                if (dashboardTable != null && dashboardTable.Rows.Count > 0)
                {
                    return dashboardTable;
                }

                DataTable legacyTable = ExecuteQuery(connString, sql);
                if (legacyTable != null && legacyTable.Rows.Count > 0)
                {
                    return legacyTable;
                }
            }

            DataTable table = ItsSupportAssignData.QueryDataTable(sql, connString);
            if (table != null && table.Rows.Count > 0)
            {
                return table;
            }

            return null;
        }

        private static DataTable ExecuteQuery(string connString, string sql)
        {
            if (string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }

            string trimmed = (sql ?? string.Empty).Trim();
            if (trimmed.Length == 0)
            {
                return null;
            }

            try
            {
                if (trimmed.StartsWith("sp_", StringComparison.OrdinalIgnoreCase)
                    || trimmed.StartsWith("exec", StringComparison.OrdinalIgnoreCase))
                {
                    Recordset rec = new Recordset();
                    rec.Open(trimmed, connString);
                    DataTable spTable = rec.DataRecord();
                    return spTable != null && spTable.Rows.Count > 0 ? spTable : null;
                }

                string sqlConn = ResolveSqlClientConnectionString(connString);
                if (!string.IsNullOrWhiteSpace(sqlConn))
                {
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

                    if (table.Rows.Count > 0)
                    {
                        return table;
                    }
                }
            }
            catch
            {
            }

            try
            {
                if ((connString ?? string.Empty).IndexOf("provider=", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return null;
                }

                Recordset rec = new Recordset();
                string openError = string.Empty;
                rec.Open(trimmed, connString.Trim(), ref openError);
                if (string.IsNullOrWhiteSpace(openError))
                {
                    DataTable recordsetTable = rec.DataRecord() ?? new DataTable();
                    if (recordsetTable.Rows.Count > 0)
                    {
                        return recordsetTable;
                    }
                }
            }
            catch
            {
            }

            return null;
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
            string recordsetConn = ItsSupportAssignData.ResolveRecordsetConnectionString();
            if (!string.IsNullOrWhiteSpace(recordsetConn))
            {
                return recordsetConn;
            }

            string sqlConn = ItsSupportAssignData.ResolveSqlClientConnectionString();
            if (!string.IsNullOrWhiteSpace(sqlConn))
            {
                return sqlConn;
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
            if (row == null || row.Table == null || string.IsNullOrWhiteSpace(columnName))
            {
                return string.Empty;
            }

            if (row.Table.Columns.Contains(columnName))
            {
                object value = row[columnName];
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
            }

            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    object value = row[column];
                    return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
                }
            }

            return string.Empty;
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

        public sealed class AssignDetail
        {
            public string AssignId { get; set; }
            public int Seq { get; set; }
            public string JobId { get; set; }
            public string CustId { get; set; }
            public string CustomerName { get; set; }
            public string TechnicianId { get; set; }
            public string TechnicianName { get; set; }
            public string TechnicianTelegram { get; set; }
            public string PreviousTechnicianId { get; set; }
            public string PreviousTechnicianName { get; set; }
            public string MarketingName { get; set; }
            public string Remark { get; set; }
            public string RemarkOrder { get; set; }
            public string RemarkTraining { get; set; }
            public string ActionNote { get; set; }
            public string PoId { get; set; }
            public string PoliceNo { get; set; }
            public string NoSn { get; set; }
            public string GsmNo { get; set; }
            public string JobType { get; set; }
            public string CategoryLabel { get; set; }
            public string TrainCategoryId { get; set; }
            public string CompanyLine { get; set; }
            public string BranchName { get; set; }
            public string AreaName { get; set; }
            public string Address { get; set; }
            public string PicName { get; set; }
            public string CustomerNumber { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Status { get; set; }
            public DateTime? SchDate { get; set; }
            public DateTime? AssignDate { get; set; }
            public DateTime? ReqDate { get; set; }
            public DateTime? InstallDate { get; set; }
            public DateTime? TrainingScheduleDate { get; set; }
        }
    }
}
