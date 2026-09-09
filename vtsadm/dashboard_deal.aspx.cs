using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using OfficeOpenXml;
using vtsadm.App_Code;
using System.EnterpriseServices;

namespace vtsadm
{
    public partial class dashboard_deal : System.Web.UI.Page
    {
        protected void LoadDashboardMetrics()
        {
            try
            {
                Recordset Rec = new Recordset();
                string dateFrom = txtDateFrom.Text.Trim();
                string dateTo = txtDateTo.Text.Trim();

                // Tentukan MarketingID yang digunakan untuk filter:
                // - Jika user punya MarketingID di session (user marketing tertentu), pakai itu
                // - Jika tidak, pakai nilai dari dropdown (bisa [All Marketing] atau marketing tertentu)
                string marketingId = ddlMarketing.SelectedValue;
                if (Session["ClsTypeUserMarketingID"] != null &&
                    !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
                {
                    marketingId = Session["ClsTypeUserMarketingID"].ToString();
                }

                string strSQL = "sp_dashboard_deal_metrics '" + dateFrom + "','" + dateTo + "','" + marketingId + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    // Total Pipeline Value
                    decimal totalValue = Convert.ToDecimal(Rec.Fields("totalPipelineValue"));
                    LblCntDealNew.Text = totalValue.ToString("#,##0");

                    // Value Change Percentage
                    decimal valueChangePercent = Convert.ToDecimal(Rec.Fields("pctValueChange"));
                    LblPctDealNew.Text = Math.Abs(valueChangePercent).ToString("#,##0.00");
                    if (valueChangePercent >= 0)
                    {
                        LblPctDealNewIndicator.Text = "<span class='positive'>+</span>";
                    }
                    else
                    {
                        LblPctDealNewIndicator.Text = "<span class='negative'>-</span>";
                    }

                    // Active Deals
                    int activeDeals = Convert.ToInt32(Rec.Fields("activeDeals"));
                    LblCntDealActive.Text = activeDeals.ToString();

                    // Active Deals Change
                    int activeDealsChange = Convert.ToInt32(Rec.Fields("activeDealsChange"));
                    LblPctDealActive.Text = Math.Abs(activeDealsChange).ToString();
                    if (activeDealsChange >= 0)
                    {
                        LblPctDealActiveIndicator.Text = "<span class='positive'>+</span>";
                    }
                    else
                    {
                        LblPctDealActiveIndicator.Text = "<span class='negative'>-</span>";
                    }

                    // Average Deal Size
                    decimal avgDealSize = Convert.ToDecimal(Rec.Fields("avgDealSize"));
                    LblAvgDealSize.Text = avgDealSize.ToString("#,##0.00");

                    // Average Deal Size Change Percentage
                    decimal avgSizeChangePercent = Convert.ToDecimal(Rec.Fields("pctAvgSizeChange"));
                    LblPctAvgDealSize.Text = Math.Abs(avgSizeChangePercent).ToString("#,##0.00");
                    if (avgSizeChangePercent >= 0)
                    {
                        LblPctAvgDealSizeIndicator.Text = "<span class='positive'>+</span>";
                    }
                    else
                    {
                        LblPctAvgDealSizeIndicator.Text = "<span class='negative'>-</span>";
                    }

                    // Win Rate
                    decimal winRate = Convert.ToDecimal(Rec.Fields("winRate"));
                    LblWinRate.Text = winRate.ToString("#,##0.00");

                    // Win Rate Change Percentage
                    decimal winRateChangePercent = Convert.ToDecimal(Rec.Fields("pctWinRateChange"));
                    LblPctWinRate.Text = Math.Abs(winRateChangePercent).ToString("#,##0.00");
                    if (winRateChangePercent >= 0)
                    {
                        LblPctWinRateIndicator.Text = "<span class='positive'>+</span>";
                    }
                    else
                    {
                        LblPctWinRateIndicator.Text = "<span class='negative'>-</span>";
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error loading dashboard metrics: " + ex.Message);
            }
        }

        protected void LoadDealCounts()
        {
            try
            {
                Recordset Rec = new Recordset();
                string dateFrom = txtDateFrom.Text.Trim();
                string dateTo = txtDateTo.Text.Trim();
                string marketingId = ddlMarketing.SelectedValue;

                // Get deal counts from the stored procedure
                string strSQL = "sp_dashboard_deal_count '" + dateFrom + "','" + dateTo + "','" + marketingId + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

                // Inisialisasi nilai default ke 0
                string prospectCount = "0";
                string leadsCount = "0";
                string visitCount = "0";
                string presentasiCount = "0";
                string trialCount = "0";
                string closeWonCount = "0";
                string closeLostCount = "0";

                // Ambil nilai dari recordset jika ada data
                if (Rec.RecordCount() > 0)
                {
                    prospectCount = Rec.Fields("ProspectCount").ToString();
                    leadsCount = Rec.Fields("LeadsCount").ToString();
                    visitCount = Rec.Fields("VisitCount").ToString();
                    presentasiCount = Rec.Fields("PresentasiCount").ToString();
                    trialCount = Rec.Fields("TrialCount").ToString();
                    closeWonCount = Rec.Fields("CloseWonCount").ToString();
                    closeLostCount = Rec.Fields("CloseLostCount").ToString();
                }

                // Set nilai ke label-label, pastikan tidak ada nilai null
                LblCntDealProspect.Text = string.IsNullOrEmpty(prospectCount) ? "0" : prospectCount;
                LblCntDealLeads.Text = string.IsNullOrEmpty(leadsCount) ? "0" : leadsCount;
                LblCntDealVisit.Text = string.IsNullOrEmpty(visitCount) ? "0" : visitCount;
                LblCntDealPresentasi.Text = string.IsNullOrEmpty(presentasiCount) ? "0" : presentasiCount;
                LblCntDealTrial.Text = string.IsNullOrEmpty(trialCount) ? "0" : trialCount;
                LblCntDealWon.Text = string.IsNullOrEmpty(closeWonCount) ? "0" : closeWonCount;
                LblCntDealLost.Text = string.IsNullOrEmpty(closeLostCount) ? "0" : closeLostCount;
            }
            catch (Exception ex)
            {
                LogError("Error loading deal counts: " + ex.Message);

                // Set nilai default ke 0 jika terjadi error
                LblCntDealProspect.Text = "0";
                LblCntDealLeads.Text = "0";
                LblCntDealVisit.Text = "0";
                LblCntDealPresentasi.Text = "0";
                LblCntDealTrial.Text = "0";
                LblCntDealWon.Text = "0";
                LblCntDealLost.Text = "0";
            }
        }

        protected void LoadDealCards()
        {
            try
            {
                // Load cards for each deal status
                LoadDealCardsByStatus("CDS0000001", prospectCards);
                LoadDealCardsByStatus("CDS0000002", leadsCards);
                LoadDealCardsByStatus("CDS0000003", visitCards);
                LoadDealCardsByStatus("CDS0000004", presentasiCards);
                LoadDealCardsByStatus("CDS0000005", trialCards);
                LoadDealCardsByStatus("CDS0000006", wonCards);
                LoadDealCardsByStatus("CDS0000007", lostCards);
            }
            catch (Exception ex)
            {
                LogError("Error loading deal cards: " + ex.Message);
            }
        }
        private void LoadDealCardsByStatus(string statusId, Literal cardContainer)
        {
            try
            {
                Recordset Rec = new Recordset();
                string dateFrom = txtDateFrom.Text.Trim();
                string dateTo = txtDateTo.Text.Trim();
                string marketingId = ddlMarketing.SelectedValue;
                // Pastikan stored procedure juga mengembalikan JobActivityID
                string strSQL = "sp_dashboard_deal_cards_by_status '" + statusId + "','" + dateFrom + "','" + dateTo + "','" + marketingId + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                StringBuilder html = new StringBuilder();
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string product = Rec.Fields("Product").ToString();
                        string customer = Rec.Fields("Customer").ToString();
                        string marketing = Rec.Fields("Marketing").ToString();
                        decimal amount = Convert.ToDecimal(Rec.Fields("Amount"));
                        string updateDate = Convert.ToDateTime(Rec.Fields("UpdateDate")).ToString("dd MMM yyyy");
                        string duration = Rec.Fields("StatusDuration").ToString();
                        string jobActivityId = Rec.Fields("JobActivityID").ToString(); // Pastikan sp mengembalikan kolom ini

                        // Cek apakah status adalah Presentasi dan duration > 7 hari
                        // ATAU status adalah Trial dan duration > 14 hari
                        int durationDays;
                        bool isExpired = false;
                        string expiredText = "";

                        if (int.TryParse(duration, out durationDays))
                        {
                            if (statusId == "CDS0000004" && durationDays > 7) // Presentasi > 7 hari
                            {
                                isExpired = true;
                                expiredText = "🔴 Expired >7 hari";
                            }
                            else if (statusId == "CDS0000005" && durationDays > 14) // Trial > 14 hari
                            {
                                isExpired = true;
                                expiredText = "🔴 Expired >14 hari";
                            }
                        }

                        // Create the card HTML
                        html.Append("<div class='deal-card' data-job-activity-id='" + jobActivityId + "' onclick='showDealDetail(\"" + jobActivityId + "\")'>");
                        html.Append("<div class='deal-title'>" + HttpUtility.HtmlEncode(product) + "</div>");
                        html.Append("<div class='deal-company'>" + HttpUtility.HtmlEncode(customer) + "</div>");

                        // Format durasi dengan ikon jam
                        html.Append("<div class='deal-duration'><i class='fa fa-clock-o'></i> " + HttpUtility.HtmlEncode(duration) + " days in status</div>");

                        // Tambahkan badge expired jika kondisi terpenuhi
                        if (isExpired)
                        {
                            html.Append("<div class='deal-expired'>" + expiredText + "</div>");
                        }

                        html.Append("<div class='deal-marketing'><i class='fa fa-user'></i> " + HttpUtility.HtmlEncode(marketing) + "</div>");

                        html.Append("<div class='row'>");
                        html.Append("<div class='col-xs-6'><div class='deal-amount'>Rp " + amount.ToString("#,##0") + "</div></div>");
                        html.Append("<div class='col-xs-6'><div class='deal-date'>" + updateDate + "</div></div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        Rec.MoveNext();
                    }
                }
                else
                {
                    // No deals found for this status
                    html.Append("<div class='text-center text-muted' style='padding: 20px;'>No deals found</div>");
                }
                cardContainer.Text = html.ToString();
            }
            catch (Exception ex)
            {
                LogError("Error loading cards for status " + statusId + ": " + ex.Message);
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDashboardMetrics();
                LoadDealCounts();
                LoadDealCards();
            }
            catch (Exception ex)
            {
                LogError("Error processing request: " + ex.Message);
            }
        }

        protected void CmdExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string dateFrom = txtDateFrom.Text.Trim();
                string dateTo = txtDateTo.Text.Trim();
                string marketingId = GetEffectiveMarketingId();
                string marketingName = GetEffectiveMarketingName();

                DashboardSummaryExport summary = GetDashboardSummaryExport(dateFrom, dateTo, marketingId);
                List<DealExportRow> detailRows = GetDealExportRows(dateFrom, dateTo, marketingId);
                StringBuilder html = new StringBuilder();
                html.Append("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
                html.Append("<style>");
                html.Append("body{font-family:Calibri,Arial,sans-serif;font-size:11pt;color:#1f2937;} ");
                html.Append(".title{font-size:16pt;font-weight:bold;color:#1f3a8a;} ");
                html.Append(".subtitle{color:#4b5563;font-size:10pt;} ");
                html.Append(".card{border:1px solid #d1d5db;background:#f8fafc;padding:8px;} ");
                html.Append(".cardLabel{font-size:9pt;color:#6b7280;} ");
                html.Append(".cardValue{font-size:14pt;font-weight:bold;color:#111827;} ");
                html.Append(".th{background:#1f3a8a;color:#ffffff;font-weight:bold;text-align:center;} ");
                html.Append(".thStage{background:#334155;color:#ffffff;font-weight:bold;text-align:center;} ");
                html.Append(".num{text-align:right;} ");
                html.Append(".center{text-align:center;} ");
                html.Append(".expired{background:#fee2e2;color:#991b1b;font-weight:bold;} ");
                html.Append(".stageProspect{background:#ecfeff;} .stageLeads{background:#eff6ff;} .stageVisit{background:#f5f3ff;} .stagePresentasi{background:#fff7ed;} .stageTrial{background:#fefce8;} .stageWon{background:#ecfdf5;} .stageLost{background:#fef2f2;}");
                html.Append("</style></head><body>");

                html.Append("<table cellpadding='4' cellspacing='0' border='0'>");
                html.Append("<tr><td class='title' colspan='8'>Dashboard Deal Export</td></tr>");
                html.Append("<tr><td class='subtitle' colspan='8'>Date From: " + HttpUtility.HtmlEncode(dateFrom) + " | Date To: " + HttpUtility.HtmlEncode(dateTo) + " | Marketing: " + HttpUtility.HtmlEncode(marketingName) + "</td></tr>");
                html.Append("</table><br/>");

                html.Append("<table cellpadding='6' cellspacing='0' border='1'>");
                html.Append("<tr>");
                html.Append("<td class='card'><div class='cardLabel'>Total Pipeline Value</div><div class='cardValue'>Rp " + summary.TotalPipelineValue.ToString("#,##0") + "</div></td>");
                html.Append("<td class='card'><div class='cardLabel'>Active Deals</div><div class='cardValue'>" + summary.ActiveDeals + "</div></td>");
                html.Append("<td class='card'><div class='cardLabel'>Avg Deal Size</div><div class='cardValue'>Rp " + summary.AvgDealSize.ToString("#,##0.00") + "</div></td>");
                html.Append("<td class='card'><div class='cardLabel'>Win Rate</div><div class='cardValue'>" + summary.WinRate.ToString("#,##0.00") + "%</div></td>");
                html.Append("</tr>");
                html.Append("</table><br/>");

                html.Append("<table cellpadding='5' cellspacing='0' border='1'>");
                html.Append("<tr><th class='th' colspan='7'>Pipeline Stage Summary</th></tr>");
                html.Append("<tr><th>Prospect</th><th>Leads</th><th>Visit</th><th>Presentasi</th><th>Trial</th><th>Close Won</th><th>Close Lost</th></tr>");
                html.Append("<tr class='center'>");
                html.Append("<td>" + summary.ProspectCount + "</td>");
                html.Append("<td>" + summary.LeadsCount + "</td>");
                html.Append("<td>" + summary.VisitCount + "</td>");
                html.Append("<td>" + summary.PresentasiCount + "</td>");
                html.Append("<td>" + summary.TrialCount + "</td>");
                html.Append("<td>" + summary.CloseWonCount + "</td>");
                html.Append("<td>" + summary.CloseLostCount + "</td>");
                html.Append("</tr>");
                html.Append("</table><br/>");

                html.Append("<table cellpadding='5' cellspacing='0' border='1'>");
                html.Append("<tr><th class='th' colspan='10'>Deal Detail</th></tr>");
                html.Append("<tr class='thStage'>");
                html.Append("<th>Stage</th><th>JobActivityID</th><th>Product</th><th>Customer</th><th>Marketing</th><th>Amount</th><th>Last Update Date</th><th>Days In Status</th><th>Expired Flag</th><th>Expired Rule</th>");
                html.Append("</tr>");

                foreach (DealExportRow row in detailRows)
                {
                    string stageClass = "";
                    string stageValue = (row.Stage ?? "").ToLowerInvariant();
                    if (stageValue.Contains("prospect")) stageClass = "stageProspect";
                    else if (stageValue.Contains("lead")) stageClass = "stageLeads";
                    else if (stageValue.Contains("visit")) stageClass = "stageVisit";
                    else if (stageValue.Contains("presentasi")) stageClass = "stagePresentasi";
                    else if (stageValue.Contains("trial")) stageClass = "stageTrial";
                    else if (stageValue.Contains("won")) stageClass = "stageWon";
                    else if (stageValue.Contains("lost")) stageClass = "stageLost";

                    bool isExpired = string.Equals(row.ExpiredFlag, "Yes", StringComparison.OrdinalIgnoreCase);

                    html.Append("<tr class='" + stageClass + "'>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(row.Stage) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(row.JobActivityID) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(row.Product) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(row.Customer) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(row.Marketing) + "</td>");
                    html.Append("<td class='num'>" + row.Amount.ToString("#,##0") + "</td>");
                    html.Append("<td class='center'>" + (row.LastUpdateDate.HasValue ? row.LastUpdateDate.Value.ToString("dd MMM yyyy") : "") + "</td>");
                    html.Append("<td class='center'>" + row.DaysInStatus + "</td>");
                    html.Append("<td class='" + (isExpired ? "expired center" : "center") + "'>" + HttpUtility.HtmlEncode(row.ExpiredFlag) + "</td>");
                    html.Append("<td class='" + (isExpired ? "expired" : "") + "'>" + HttpUtility.HtmlEncode(row.ExpiredRule) + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                html.Append("</body></html>");

                string filename = "dashboard_deal_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xls";
                byte[] bytes = Encoding.UTF8.GetBytes(html.ToString());

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                LogError("Error exporting dashboard deal: " + ex.Message);
            }
        }

        private string GetEffectiveMarketingId()
        {
            string marketingId = ddlMarketing.SelectedValue;
            if (Session["ClsTypeUserMarketingID"] != null &&
                !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
            {
                marketingId = Session["ClsTypeUserMarketingID"].ToString();
            }
            return marketingId;
        }

        private string GetEffectiveMarketingName()
        {
            if (ddlMarketing.SelectedItem != null && !string.IsNullOrEmpty(ddlMarketing.SelectedItem.Text))
            {
                return ddlMarketing.SelectedItem.Text;
            }
            return "[All Marketing]";
        }

        private static string EscapeSql(string value)
        {
            return (value ?? "").Trim().Replace("'", "''");
        }

        private static int SafeToInt(object value)
        {
            int result;
            return int.TryParse(Convert.ToString(value), out result) ? result : 0;
        }

        private static decimal SafeToDecimal(object value)
        {
            decimal result;
            return decimal.TryParse(Convert.ToString(value), out result) ? result : 0m;
        }

        private static DateTime? SafeToDateTime(object value)
        {
            DateTime result;
            return DateTime.TryParse(Convert.ToString(value), out result) ? result : (DateTime?)null;
        }

        private DashboardSummaryExport GetDashboardSummaryExport(string dateFrom, string dateTo, string marketingId)
        {
            DashboardSummaryExport summary = new DashboardSummaryExport();
            string connString = Session["ClsTypeDBConnStringSQL"].ToString();

            Recordset recMetrics = new Recordset();
            string sqlMetrics = "sp_dashboard_deal_metrics '" + EscapeSql(dateFrom) + "','" + EscapeSql(dateTo) + "','" + EscapeSql(marketingId) + "'";
            recMetrics.Open(sqlMetrics, connString);
            if (recMetrics.RecordCount() > 0)
            {
                summary.TotalPipelineValue = SafeToDecimal(recMetrics.Fields("totalPipelineValue"));
                summary.ActiveDeals = SafeToInt(recMetrics.Fields("activeDeals"));
                summary.AvgDealSize = SafeToDecimal(recMetrics.Fields("avgDealSize"));
                summary.WinRate = SafeToDecimal(recMetrics.Fields("winRate"));
            }

            Recordset recCounts = new Recordset();
            string sqlCounts = "sp_dashboard_deal_count '" + EscapeSql(dateFrom) + "','" + EscapeSql(dateTo) + "','" + EscapeSql(marketingId) + "'";
            recCounts.Open(sqlCounts, connString);
            if (recCounts.RecordCount() > 0)
            {
                summary.ProspectCount = SafeToInt(recCounts.Fields("ProspectCount"));
                summary.LeadsCount = SafeToInt(recCounts.Fields("LeadsCount"));
                summary.VisitCount = SafeToInt(recCounts.Fields("VisitCount"));
                summary.PresentasiCount = SafeToInt(recCounts.Fields("PresentasiCount"));
                summary.TrialCount = SafeToInt(recCounts.Fields("TrialCount"));
                summary.CloseWonCount = SafeToInt(recCounts.Fields("CloseWonCount"));
                summary.CloseLostCount = SafeToInt(recCounts.Fields("CloseLostCount"));
            }

            return summary;
        }

        private List<DealExportRow> GetDealExportRows(string dateFrom, string dateTo, string marketingId)
        {
            List<DealExportRow> rows = new List<DealExportRow>();
            string connString = Session["ClsTypeDBConnStringSQL"].ToString();

            KeyValuePair<string, string>[] stages = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("CDS0000001", "Prospect"),
                new KeyValuePair<string, string>("CDS0000002", "Leads"),
                new KeyValuePair<string, string>("CDS0000003", "Visit"),
                new KeyValuePair<string, string>("CDS0000004", "Presentasi"),
                new KeyValuePair<string, string>("CDS0000005", "Trial"),
                new KeyValuePair<string, string>("CDS0000006", "Close Won"),
                new KeyValuePair<string, string>("CDS0000007", "Close Lost")
            };

            foreach (KeyValuePair<string, string> stage in stages)
            {
                Recordset rec = new Recordset();
                string sql = "sp_dashboard_deal_cards_by_status '" + stage.Key + "','" + EscapeSql(dateFrom) + "','" + EscapeSql(dateTo) + "','" + EscapeSql(marketingId) + "'";
                rec.Open(sql, connString);

                if (rec.RecordCount() <= 0)
                    continue;

                rec.MoveFirst();
                while (!rec.EOF)
                {
                    DealExportRow row = new DealExportRow();
                    row.Stage = stage.Value;
                    row.JobActivityID = Convert.ToString(rec.Fields("JobActivityID"));
                    row.Product = Convert.ToString(rec.Fields("Product"));
                    row.Customer = Convert.ToString(rec.Fields("Customer"));
                    row.Marketing = Convert.ToString(rec.Fields("Marketing"));
                    row.Amount = SafeToDecimal(rec.Fields("Amount"));
                    row.LastUpdateDate = SafeToDateTime(rec.Fields("UpdateDate"));

                    int daysInStatus = SafeToInt(rec.Fields("StatusDuration"));
                    row.DaysInStatus = daysInStatus;
                    row.ExpiredFlag = "No";
                    row.ExpiredRule = "";

                    if (stage.Key == "CDS0000004" && daysInStatus > 7)
                    {
                        row.ExpiredFlag = "Yes";
                        row.ExpiredRule = ">7 hari Presentasi";
                    }
                    else if (stage.Key == "CDS0000005" && daysInStatus > 14)
                    {
                        row.ExpiredFlag = "Yes";
                        row.ExpiredRule = ">14 hari Trial";
                    }

                    rows.Add(row);
                    rec.MoveNext();
                }
            }

            return rows;
        }

        private static void BuildSummaryWorksheet(ExcelPackage package, DashboardSummaryExport summary)
        {
            var ws = package.Workbook.Worksheets.Add("Summary");
            ws.Cells[1, 1].Value = "Metric";
            ws.Cells[1, 2].Value = "Value";

            ws.Cells[2, 1].Value = "Total Pipeline Value";
            ws.Cells[2, 2].Value = summary.TotalPipelineValue;
            ws.Cells[3, 1].Value = "Active Deals";
            ws.Cells[3, 2].Value = summary.ActiveDeals;
            ws.Cells[4, 1].Value = "Avg Deal Size";
            ws.Cells[4, 2].Value = summary.AvgDealSize;
            ws.Cells[5, 1].Value = "Win Rate (%)";
            ws.Cells[5, 2].Value = summary.WinRate;
            ws.Cells[6, 1].Value = "Prospect Count";
            ws.Cells[6, 2].Value = summary.ProspectCount;
            ws.Cells[7, 1].Value = "Leads Count";
            ws.Cells[7, 2].Value = summary.LeadsCount;
            ws.Cells[8, 1].Value = "Visit Count";
            ws.Cells[8, 2].Value = summary.VisitCount;
            ws.Cells[9, 1].Value = "Presentasi Count";
            ws.Cells[9, 2].Value = summary.PresentasiCount;
            ws.Cells[10, 1].Value = "Trial Count";
            ws.Cells[10, 2].Value = summary.TrialCount;
            ws.Cells[11, 1].Value = "Close Won Count";
            ws.Cells[11, 2].Value = summary.CloseWonCount;
            ws.Cells[12, 1].Value = "Close Lost Count";
            ws.Cells[12, 2].Value = summary.CloseLostCount;

            ws.Cells[1, 1, 1, 2].Style.Font.Bold = true;
            ws.Cells[2, 2].Style.Numberformat.Format = "#,##0";
            ws.Cells[4, 2].Style.Numberformat.Format = "#,##0.00";
            ws.Cells[5, 2].Style.Numberformat.Format = "0.00";
            ws.Cells.AutoFitColumns();
        }

        private static void BuildDetailWorksheet(ExcelPackage package, List<DealExportRow> rows)
        {
            var ws = package.Workbook.Worksheets.Add("Deal Detail");
            string[] headers = new string[]
            {
                "Stage",
                "JobActivityID",
                "Product",
                "Customer",
                "Marketing",
                "Amount",
                "Last Update Date",
                "Days In Status",
                "Expired Flag",
                "Expired Rule"
            };

            for (int col = 0; col < headers.Length; col++)
            {
                ws.Cells[1, col + 1].Value = headers[col];
            }
            ws.Cells[1, 1, 1, headers.Length].Style.Font.Bold = true;

            int rowIndex = 2;
            foreach (DealExportRow row in rows)
            {
                ws.Cells[rowIndex, 1].Value = row.Stage;
                ws.Cells[rowIndex, 2].Value = row.JobActivityID;
                ws.Cells[rowIndex, 3].Value = row.Product;
                ws.Cells[rowIndex, 4].Value = row.Customer;
                ws.Cells[rowIndex, 5].Value = row.Marketing;
                ws.Cells[rowIndex, 6].Value = row.Amount;
                ws.Cells[rowIndex, 7].Value = row.LastUpdateDate;
                ws.Cells[rowIndex, 8].Value = row.DaysInStatus;
                ws.Cells[rowIndex, 9].Value = row.ExpiredFlag;
                ws.Cells[rowIndex, 10].Value = row.ExpiredRule;
                rowIndex++;
            }

            if (rowIndex > 2)
            {
                ws.Cells[2, 6, rowIndex - 1, 6].Style.Numberformat.Format = "#,##0";
                ws.Cells[2, 7, rowIndex - 1, 7].Style.Numberformat.Format = "dd MMM yyyy";
            }

            ws.Cells[1, 1, Math.Max(rowIndex - 1, 1), headers.Length].AutoFilter = true;
            ws.View.FreezePanes(2, 1);
            ws.Cells.AutoFitColumns();
        }

        private class DashboardSummaryExport
        {
            public decimal TotalPipelineValue { get; set; }
            public int ActiveDeals { get; set; }
            public decimal AvgDealSize { get; set; }
            public decimal WinRate { get; set; }
            public int ProspectCount { get; set; }
            public int LeadsCount { get; set; }
            public int VisitCount { get; set; }
            public int PresentasiCount { get; set; }
            public int TrialCount { get; set; }
            public int CloseWonCount { get; set; }
            public int CloseLostCount { get; set; }
        }

        private class DealExportRow
        {
            public string Stage { get; set; }
            public string JobActivityID { get; set; }
            public string Product { get; set; }
            public string Customer { get; set; }
            public string Marketing { get; set; }
            public decimal Amount { get; set; }
            public DateTime? LastUpdateDate { get; set; }
            public int DaysInStatus { get; set; }
            public string ExpiredFlag { get; set; }
            public string ExpiredRule { get; set; }
        }

        // Safe method to display errors
        private void LogError(string errorMessage)
        {
            try
            {
                if (div_comment != null)
                {
                    div_comment.Controls.Add(new LiteralControl("<div class='alert alert-danger'>" + errorMessage + "</div>"));
                }
                // Could also log to a file or database here
            }
            catch
            {
                // Last resort fallback - don't throw another exception
            }
        }

        protected void Clear()
        {
            try
            {
                txtDateFrom.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Mengisi dropdown marketing menggunakan stored procedure
                Recordset Rec = new Recordset();
                string userId = Session["ClsTypeUserID"] != null
                    ? Session["ClsTypeUserID"].ToString()
                    : "";
                string strSQL = "sp_get_marketing_for_dropdown '" + userId + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

                ddlMarketing.Items.Clear();
                ddlMarketing.Items.Add(new ListItem("[All Marketing]", ""));

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        string marketingId = Rec.Fields("MarketingID").ToString();
                        string marketingName = Rec.Fields("MarketingName").ToString();
                        ddlMarketing.Items.Add(new ListItem(marketingName, marketingId));
                        Rec.MoveNext();
                    }
                }

                // Jika user adalah marketing tertentu (punya MarketingID di session),
                // maka dropdown dikunci ke marketing tersebut dan di-disable.
                if (Session["ClsTypeUserMarketingID"] != null &&
                    !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
                {
                    string userMarketingId = Session["ClsTypeUserMarketingID"].ToString();
                    ListItem item = ddlMarketing.Items.FindByValue(userMarketingId);
                    if (item != null)
                    {
                        ddlMarketing.SelectedValue = userMarketingId;
                    }
                    ddlMarketing.Enabled = false;
                }
                else
                {
                    // User non-marketing: boleh pilih semua / per marketing
                    ddlMarketing.SelectedIndex = 0;
                    ddlMarketing.Enabled = true;
                }

                // Reset metrics
                LblCntDealNew.Text = "0";
                LblPctDealNew.Text = "0";
                LblPctDealNewIndicator.Text = "<span class='positive'>+</span>";

                LblCntDealActive.Text = "0";
                LblPctDealActive.Text = "0";
                LblPctDealActiveIndicator.Text = "<span class='positive'>+</span>";

                LblAvgDealSize.Text = "0";
                LblPctAvgDealSize.Text = "0";
                LblPctAvgDealSizeIndicator.Text = "<span class='positive'>+</span>";

                LblWinRate.Text = "0";
                LblPctWinRate.Text = "0";
                LblPctWinRateIndicator.Text = "<span class='positive'>+</span>";

                // Reset stage counters
                LblCntDealProspect.Text = "0";
                LblCntDealLeads.Text = "0";
                LblCntDealVisit.Text = "0";
                LblCntDealPresentasi.Text = "0";
                LblCntDealTrial.Text = "0";
                LblCntDealWon.Text = "0";
                LblCntDealLost.Text = "0";

                // Clear deal cards
                prospectCards.Text = "";
                leadsCards.Text = "";
                visitCards.Text = "";
                presentasiCards.Text = "";
                trialCards.Text = "";
                wonCards.Text = "";
                lostCards.Text = "";

                // Clear any error messages
                if (div_comment != null)
                {
                    div_comment.Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                LogError("Error clearing form: " + ex.Message);
            }
        }

        private static int? ParseIntFromRecord(Recordset rec, string fieldName)
        {
            try
            {
                string s = rec.Fields(fieldName).ToString().Trim();
                if (string.IsNullOrEmpty(s)) return null;
                return Convert.ToInt32(s);
            }
            catch { return null; }
        }

        [WebMethod]
        public static DealDetail GetDealDetail(string JobActivityId) // Ubah parameter dari activityId menjadi JobActivityId
        {
            try
            {
                DealDetail detail = new DealDetail();

                // Create an instance of the page to get the DB connection string
                dashboard_deal page = new dashboard_deal();
                string connString = page.DBConnstringSQL();

                Recordset Rec = new Recordset();
                string strSQL = "sp_get_deal_detail '" + JobActivityId + "'"; // Gunakan JobActivityId
                Rec.Open(strSQL, connString);

                bool isFirstRow = true;

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        // Data informasi dasar deal hanya diambil dari baris pertama
                        if (isFirstRow)
                        {
                            detail.JobActivityID = Rec.Fields("JobActivityID").ToString();
                            detail.ActivityCode = Rec.Fields("ActivityCode").ToString();
                            detail.FullName = Rec.Fields("FullName").ToString();
                            detail.MarketingName = Rec.Fields("MarketingName").ToString();
                            detail.ProductName = Rec.Fields("ProductName").ToString();
                            detail.Price = Convert.ToDecimal(Rec.Fields("Price"));
                            detail.RegDate = Convert.ToDateTime(Rec.Fields("RegDate"));
                            detail.PoID = Rec.Fields("PoID") != null ? Rec.Fields("PoID").ToString().Trim() : "";
                            detail.TotalUnit = ParseIntFromRecord(Rec, "TotalUnit");
                            detail.TotalInstalledUnits = ParseIntFromRecord(Rec, "TotalUnitInstall");
                            detail.TotalUninstalledUnits = ParseIntFromRecord(Rec, "TotalUnitNotInstall");

                            isFirstRow = false;
                        }

                        // Setiap baris menambahkan satu item history
                        DealHistoryItem historyItem = new DealHistoryItem
                        {
                            ActivityId = Rec.Fields("ActivityId").ToString(),
                            DealStatusID = Rec.Fields("DealStatusID").ToString(),
                            DealStatusName = Rec.Fields("DealStatusName").ToString(),
                            ActivityDate = Convert.ToDateTime(Rec.Fields("ActivityDate")),
                            AddressLocation = Rec.Fields("addressLocation") != null ? Rec.Fields("addressLocation").ToString() : "",
                            StatusChangeDate = Convert.ToDateTime(Rec.Fields("StatusChangeDate")),
                            Remark = Rec.Fields("Remark") != null ? Rec.Fields("Remark").ToString() : ""
                        };

                        detail.Timeline.Add(historyItem);
                        Rec.MoveNext();
                    }
                }

                return detail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting deal details: " + ex.Message);
            }
        }

        // Class untuk menyimpan detail deal
        public class DealDetail
        {
            // Informasi dasar deal (akan diisi dari baris pertama data)
            public string JobActivityID { get; set; }
            public string ActivityCode { get; set; }
            public string FullName { get; set; }
            public string MarketingName { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public DateTime RegDate { get; set; }
            public string PoID { get; set; }
            public int? TotalUnit { get; set; }
            public int? TotalInstalledUnits { get; set; }
            public int? TotalUninstalledUnits { get; set; }

            // History timeline
            public List<DealHistoryItem> Timeline { get; set; } = new List<DealHistoryItem>();
        }

        public class DealHistoryItem
        {
            public string ActivityId { get; set; }
            public string DealStatusID { get; set; }
            public string DealStatusName { get; set; }
            public DateTime ActivityDate { get; set; }
            public string AddressLocation { get; set; }
            public DateTime StatusChangeDate { get; set; }
            public string Remark { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHDEAL"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportExcel);

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            LoadDashboardMetrics();
                            LoadDealCounts();
                            LoadDealCards();
                        }
                        else
                        {
                            Response.Redirect("login.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                // Use safer error logging
                try
                {
                    if (div_comment != null)
                    {
                        div_comment.Controls.Add(new LiteralControl("<div class='alert alert-danger'>Error loading page: " + ex.Message + "</div>"));
                    }
                }
                catch
                {
                    // Suppress any further exceptions
                }
            }
        }

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
    }
}