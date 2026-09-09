using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_eseal_rent : System.Web.UI.Page
    {
        public class EsealRentFilter
        {
            public string search { get; set; }
            public string rental_type { get; set; }
            public string evidence_status { get; set; }
            public string period { get; set; }
            public string date_from { get; set; }
            public string date_to { get; set; }
        }

        public class EsealRentSummaryData
        {
            public int SoDaily { get; set; }
            public int SoWeekly { get; set; }
            public int SoMonthly { get; set; }
            public int TotalPackage { get; set; }
        }

        public class EsealRentChartPoint
        {
            public string label { get; set; }
            public int total { get; set; }
        }

        public class EsealRentActivityPoint
        {
            public string label { get; set; }
            public int daily { get; set; }
            public int weekly { get; set; }
            public int monthly { get; set; }
        }

        public class EsealRentRowItem
        {
            public string PoID { get; set; }
            public string PONumber { get; set; }
            public string Customer { get; set; }
            public string Pic { get; set; }
            public string PhoneNo { get; set; }
            public string RentalType { get; set; }
            public int? TotalUnit { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? ExpiredDate { get; set; }
            public decimal? Price { get; set; }
            public string EvidenceStatus { get; set; }
            public string EvidenceStatusCode { get; set; }

            public string TotalUnitDisplay { get; set; }
            public string OrderDateDisplay { get; set; }
            public string ExpiredDateDisplay { get; set; }
            public string PriceDisplay { get; set; }
            public string EvidenceStatusHtml { get; set; }
            public string ActionHtml { get; set; }
        }

        public class EsealRentDashboardData
        {
            public EsealRentSummaryData Summary { get; set; }
            public List<EsealRentActivityPoint> Activity { get; set; }
            public List<EsealRentRowItem> Details { get; set; }

            public EsealRentDashboardData()
            {
                Summary = new EsealRentSummaryData();
                Activity = new List<EsealRentActivityPoint>();
                Details = new List<EsealRentRowItem>();
            }
        }

        public class EsealRentDetailResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
            public EsealRentDetailHeader header { get; set; }
            public List<EsealRentDetailItem> items { get; set; }
            public List<EsealRentDetailJob> jobs { get; set; }

            public EsealRentDetailResponse()
            {
                message = string.Empty;
                items = new List<EsealRentDetailItem>();
                jobs = new List<EsealRentDetailJob>();
            }
        }

        public class EsealRentDetailHeader
        {
            public string PoID { get; set; }
            public string PONumber { get; set; }
            public string OrderDate { get; set; }
            public string CustID { get; set; }
            public string CustomerName { get; set; }
            public string PicName { get; set; }
            public string PicMobilePhone { get; set; }
            public string OfficePhone { get; set; }
            public string PoTypeID { get; set; }
            public string RentalType { get; set; }
            public string ExpiredDate { get; set; }
            public string PoStatus { get; set; }
            public int TotalItem { get; set; }
            public int TotalUnit { get; set; }
            public int TotalQuantityDone { get; set; }
            public decimal TotalPrice { get; set; }
            public decimal TotalInstallFee { get; set; }
            public decimal TotalMonthlyFee { get; set; }
            public decimal GrandTotal { get; set; }
            public int TotalJob { get; set; }
            public int TotalClosedJob { get; set; }
            public int TotalPendingJob { get; set; }
            public string EvidenceStatusCode { get; set; }
            public string EvidenceStatus { get; set; }
        }

        public class EsealRentDetailItem
        {
            public int Quantity { get; set; }
            public int QuantityDone { get; set; }
            public decimal Price { get; set; }
            public decimal InstallFee { get; set; }
            public decimal MonthlyFee { get; set; }
            public decimal SubTotalPrice { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
        }

        public class EsealRentDetailJob
        {
            public string JobID { get; set; }
            public string Status { get; set; }
            public string JobStatusDescription { get; set; }
            public int IsEvidenceComplete { get; set; }
        }

        protected TextBox txtSearch;
        protected DropDownList ddlRentalType;
        protected DropDownList ddlEvidenceStatus;
        protected TextBox txtPeriode;
        protected Button btnCari;
        protected Button btnReset;
        protected Button btnExportCsv;
        protected HiddenField hfChartTypeJson;
        protected HiddenField hfChartDailyJson;
        protected HiddenField hfPeriodValue;
        protected HtmlGenericControl lblSoDaily;
        protected HtmlGenericControl lblSoWeekly;
        protected HtmlGenericControl lblSoMonthly;
        protected HtmlGenericControl lblTotalPackage;
        protected HtmlGenericControl lblChartPeriode;
        protected HtmlGenericControl lblGridCount;
        protected HtmlGenericControl lblGridEmpty;
        protected Repeater rptDetail;

        private const string SessionFilterKey = "SessionEsealRentFilter";
        private const string PoTypeDaily = "PTY0000011";
        private const string PoTypeWeekly = "PTY0000012";
        private const string PoTypeMonthly = "PTY0000013";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                // TODO: aktifkan setelah menu MNUDASHESEALRENT tersedia di akses user.
                // if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHESEALRENT"))
                // {
                //     Response.Redirect("dashboard.aspx");
                //     return;
                // }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    ResetFilter();
                    LoadDashboard();
                }
            }
            catch
            {
                ResetSummary();
                BindEmptyCharts();
                BindEmptyGrid();
            }
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetFilter();
            LoadDashboard();
        }

        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            EsealRentFilter filter = GetFilter();
            EsealRentDashboardData data = ExecuteEsealRentDashboard(filter);
            ExportRowsToCsv(data.Details, filter.period);
        }

        private void LoadDashboard()
        {
            EsealRentFilter filter = GetFilter();
            Session[SessionFilterKey] = filter;

            EsealRentDashboardData data = ExecuteEsealRentDashboard(filter);
            BindSummary(data.Summary, filter);
            BindCharts(data);
            BindGrid(data.Details);
        }

        private void BindSummary(EsealRentSummaryData summary, EsealRentFilter filter)
        {
            if (summary == null)
            {
                summary = new EsealRentSummaryData();
            }

            lblSoDaily.InnerText = summary.SoDaily.ToString("#,##0");
            lblSoWeekly.InnerText = summary.SoWeekly.ToString("#,##0");
            lblSoMonthly.InnerText = summary.SoMonthly.ToString("#,##0");
            lblTotalPackage.InnerText = summary.TotalPackage.ToString("#,##0");
            lblChartPeriode.InnerText = FormatPeriodeDisplay(filter.period);
        }

        private void BindCharts(EsealRentDashboardData data)
        {
            EsealRentSummaryData summary = data != null && data.Summary != null
                ? data.Summary
                : new EsealRentSummaryData();

            var typePayload = new
            {
                totalPackage = summary.TotalPackage,
                items = new List<EsealRentChartPoint>
                {
                    new EsealRentChartPoint { label = "Daily", total = summary.SoDaily },
                    new EsealRentChartPoint { label = "Weekly", total = summary.SoWeekly },
                    new EsealRentChartPoint { label = "Monthly", total = summary.SoMonthly }
                }
            };

            List<EsealRentActivityPoint> activity = data != null && data.Activity != null
                ? data.Activity
                : new List<EsealRentActivityPoint>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            hfChartTypeJson.Value = serializer.Serialize(typePayload);
            hfChartDailyJson.Value = serializer.Serialize(activity);

            string script = "if (typeof renderEsealCharts === 'function') { renderEsealCharts(); }";
            ScriptManager.RegisterStartupScript(this, GetType(), "eseal-render-charts", script, true);
        }

        private void BindGrid(List<EsealRentRowItem> rows)
        {
            List<EsealRentRowItem> source = rows ?? new List<EsealRentRowItem>();
            List<EsealRentRowItem> displayRows = source
                .Select(row => BuildDisplayRow(row))
                .ToList();

            rptDetail.DataSource = displayRows;
            rptDetail.DataBind();

            lblGridCount.InnerText = displayRows.Count.ToString("#,##0");
            lblGridEmpty.Visible = displayRows.Count == 0;
        }

        private EsealRentFilter GetFilter()
        {
            string period = NormalizePeriode(!string.IsNullOrWhiteSpace(hfPeriodValue.Value)
                ? hfPeriodValue.Value
                : txtPeriode.Text);

            hfPeriodValue.Value = period;
            txtPeriode.Text = FormatPeriodeDisplay(period);

            DateTime dateFrom;
            DateTime dateTo;
            ResolvePeriodDateRange(period, out dateFrom, out dateTo);

            return new EsealRentFilter
            {
                search = (txtSearch.Text ?? string.Empty).Trim(),
                rental_type = NormalizeRentalType(ddlRentalType.SelectedValue),
                evidence_status = NormalizeEvidenceStatus(ddlEvidenceStatus.SelectedValue),
                period = period,
                date_from = dateFrom.ToString("yyyy-MM-dd"),
                date_to = dateTo.ToString("yyyy-MM-dd")
            };
        }

        private void ResetFilter()
        {
            string defaultPeriod = DateTime.Now.ToString("yyyy-MM");

            txtSearch.Text = string.Empty;
            ddlRentalType.SelectedValue = string.Empty;
            ddlEvidenceStatus.SelectedValue = string.Empty;
            hfPeriodValue.Value = defaultPeriod;
            txtPeriode.Text = FormatPeriodeDisplay(defaultPeriod);
        }

        private EsealRentDashboardData ExecuteEsealRentDashboard(EsealRentFilter filter)
        {
            var result = new EsealRentDashboardData();

            try
            {
                string rawConn = Session["ClsTypeDBConnStringSQL"] == null
                    ? string.Empty
                    : Session["ClsTypeDBConnStringSQL"].ToString();
                string sqlConn = GetSqlClientConnectionString(rawConn);

                if (string.IsNullOrWhiteSpace(sqlConn))
                {
                    return result;
                }

                DateTime dateFrom;
                DateTime dateTo;
                ResolvePeriodDateRange(filter.period, out dateFrom, out dateTo);
                string periodParam = dateFrom.ToString("yyyy-MM-01", CultureInfo.InvariantCulture);
                string poTypeId = MapPoTypeId(filter.rental_type);
                string evidenceStatus = MapEvidenceStatus(filter.evidence_status);

                using (SqlConnection conn = new SqlConnection(sqlConn))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_dashboard_eseal_rent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;

                    cmd.Parameters.Add("@Period", SqlDbType.VarChar, 10).Value = periodParam;
                    cmd.Parameters.Add("@Search", SqlDbType.VarChar, 200).Value = string.IsNullOrWhiteSpace(filter.search)
                        ? (object)DBNull.Value
                        : filter.search.Trim();
                    cmd.Parameters.Add("@PoTypeID", SqlDbType.VarChar, 20).Value = string.IsNullOrWhiteSpace(poTypeId)
                        ? (object)DBNull.Value
                        : poTypeId;
                    cmd.Parameters.Add("@EvidenceStatus", SqlDbType.VarChar, 20).Value = string.IsNullOrWhiteSpace(evidenceStatus)
                        ? (object)DBNull.Value
                        : evidenceStatus;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // RESULT SET 1 — CARD RINGKASAN
                        if (reader.Read())
                        {
                            result.Summary.SoDaily = ReadInt(reader, "TotalDaily");
                            result.Summary.SoWeekly = ReadInt(reader, "TotalWeekly");
                            result.Summary.SoMonthly = ReadInt(reader, "TotalMonthly");
                            result.Summary.TotalPackage = ReadInt(reader, "TotalPackage");
                        }

                        // RESULT SET 2 — GRAFIK AKTIVITAS RENTAL
                        if (reader.NextResult())
                        {
                            var activityBuffer = new List<Tuple<DateTime?, EsealRentActivityPoint>>();
                            CultureInfo culture = new CultureInfo("id-ID");

                            while (reader.Read())
                            {
                                DateTime? activityDate = ReadNullableDate(reader, "ActivityDate");
                                var point = new EsealRentActivityPoint
                                {
                                    label = activityDate.HasValue
                                        ? activityDate.Value.ToString("dd MMM", culture)
                                        : DisplayOrDash(ReadString(reader, "ActivityDate")),
                                    daily = ReadInt(reader, "TotalDaily"),
                                    weekly = ReadInt(reader, "TotalWeekly"),
                                    monthly = ReadInt(reader, "TotalMonthly")
                                };
                                activityBuffer.Add(Tuple.Create(activityDate, point));
                            }

                            result.Activity = activityBuffer
                                .OrderBy(x => x.Item1.HasValue ? x.Item1.Value : DateTime.MaxValue)
                                .ThenBy(x => x.Item2.label, StringComparer.OrdinalIgnoreCase)
                                .Select(x => x.Item2)
                                .ToList();
                        }

                        // RESULT SET 3 — DETAIL INFORMASI SEWA
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result.Details.Add(new EsealRentRowItem
                                {
                                    PoID = ReadString(reader, "PoID"),
                                    PONumber = ReadString(reader, "PONumber"),
                                    Customer = ReadString(reader, "CustomerName"),
                                    Pic = ReadString(reader, "PicName"),
                                    PhoneNo = ReadString(reader, "PicMobilePhone"),
                                    RentalType = ReadString(reader, "RentalType"),
                                    TotalUnit = ReadNullableInt(reader, "TotalUnit"),
                                    OrderDate = ReadNullableDate(reader, "OrderDate"),
                                    ExpiredDate = ReadNullableDate(reader, "ExpiredDate"),
                                    Price = ReadNullableDecimal(reader, "Price"),
                                    EvidenceStatus = ReadString(reader, "EvidenceStatus"),
                                    EvidenceStatusCode = ReadString(reader, "EvidenceStatusCode")
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                return new EsealRentDashboardData();
            }

            return result;
        }

        private static string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            List<string> pairs = new List<string>();
            string[] parts = connectionString.Split(';');

            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();
                if (string.IsNullOrEmpty(part))
                {
                    continue;
                }

                int separatorIndex = part.IndexOf('=');
                string key = separatorIndex >= 0
                    ? part.Substring(0, separatorIndex).Trim().ToUpperInvariant()
                    : part.ToUpperInvariant();

                if (key == "PROVIDER")
                {
                    continue;
                }

                pairs.Add(part);
            }

            return string.Join(";", pairs);
        }

        private EsealRentRowItem BuildDisplayRow(EsealRentRowItem row)
        {
            CultureInfo culture = new CultureInfo("id-ID");

            row.Customer = DisplayOrDash(row.Customer);
            row.Pic = DisplayOrDash(row.Pic);
            row.PhoneNo = DisplayOrDash(row.PhoneNo);
            row.RentalType = DisplayOrDash(row.RentalType);
            row.TotalUnitDisplay = row.TotalUnit.HasValue
                ? row.TotalUnit.Value.ToString("#,##0", culture)
                : "-";
            row.OrderDateDisplay = row.OrderDate.HasValue
                ? row.OrderDate.Value.ToString("dd MMM yyyy", culture)
                : "-";
            row.ExpiredDateDisplay = row.ExpiredDate.HasValue
                ? row.ExpiredDate.Value.ToString("dd MMM yyyy", culture)
                : "-";
            row.PriceDisplay = row.Price.HasValue
                ? row.Price.Value.ToString("#,##0", culture)
                : "-";
            row.EvidenceStatusHtml = BuildEvidenceStatusHtml(row.EvidenceStatusCode, row.EvidenceStatus);
            row.ActionHtml = BuildActionHtml(row.PoID, row.PONumber);
            return row;
        }

        private string BuildEvidenceStatusHtml(string evidenceStatusCode, string evidenceStatus)
        {
            string code = (evidenceStatusCode ?? string.Empty).Trim().ToUpperInvariant();
            if (code == "COMPLETE")
            {
                return "<span class=\"eseal-status-badge eseal-status-lengkap\">Evidence Lengkap</span>";
            }

            if (code == "INCOMPLETE")
            {
                return "<span class=\"eseal-status-badge eseal-status-belum\">Belum Lengkap</span>";
            }

            string label = DisplayOrDash(evidenceStatus);
            if (label == "-")
            {
                return "<span class=\"eseal-status-badge eseal-status-belum\">Belum Lengkap</span>";
            }

            return "<span class=\"eseal-status-badge eseal-status-belum\">" + HttpUtility.HtmlEncode(label) + "</span>";
        }

        private string BuildActionHtml(string poId, string poNumber)
        {
            string safePoId = HttpUtility.HtmlAttributeEncode(poId ?? string.Empty);
            return "<button type=\"button\" class=\"btn eseal-btn-action eseal-btn-detail\" data-po-id=\""
                + safePoId
                + "\">Detail</button>";
        }

        [WebMethod(EnableSession = true)]
        public static EsealRentDetailResponse GetEsealRentDetail(string poId)
        {
            var response = new EsealRentDetailResponse
            {
                success = false,
                message = "Data sewa tidak ditemukan.",
                header = null,
                items = new List<EsealRentDetailItem>(),
                jobs = new List<EsealRentDetailJob>()
            };

            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                {
                    response.message = "Sesi tidak valid. Silakan login ulang.";
                    return response;
                }

                if (HttpContext.Current.Session["ClsTypeIsLogin"] == null)
                {
                    response.message = "Sesi tidak valid. Silakan login ulang.";
                    return response;
                }

                string normalizedPoId = (poId ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(normalizedPoId))
                {
                    response.message = "PoID tidak valid.";
                    return response;
                }

                string rawConn = HttpContext.Current.Session["ClsTypeDBConnStringSQL"] == null
                    ? string.Empty
                    : HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString();
                string sqlConn = GetSqlClientConnectionString(rawConn);
                if (string.IsNullOrWhiteSpace(sqlConn))
                {
                    response.message = "Koneksi database tidak tersedia.";
                    return response;
                }

                using (SqlConnection conn = new SqlConnection(sqlConn))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_dashboard_eseal_rent_detail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.Add("@PoID", SqlDbType.VarChar, 50).Value = normalizedPoId;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // RESULT SET 1 — HEADER
                        if (!reader.Read())
                        {
                            return response;
                        }

                        response.header = MapDetailHeader(reader);
                        response.success = true;
                        response.message = string.Empty;

                        // RESULT SET 2 — ITEMS
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                response.items.Add(new EsealRentDetailItem
                                {
                                    Quantity = ReadInt(reader, "Quantity"),
                                    QuantityDone = ReadInt(reader, "QuantityDone"),
                                    Price = ReadDecimal(reader, "Price"),
                                    InstallFee = ReadDecimal(reader, "InstallFee"),
                                    MonthlyFee = ReadDecimal(reader, "MonthlyFee"),
                                    SubTotalPrice = ReadDecimal(reader, "SubTotalPrice"),
                                    TotalAmount = ReadDecimal(reader, "TotalAmount"),
                                    Status = ReadString(reader, "Status")
                                });
                            }
                        }

                        // RESULT SET 3 — JOBS
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                response.jobs.Add(new EsealRentDetailJob
                                {
                                    JobID = ReadString(reader, "JobID"),
                                    Status = ReadString(reader, "Status"),
                                    JobStatusDescription = ReadString(reader, "JobStatusDescription"),
                                    IsEvidenceComplete = ReadInt(reader, "IsEvidenceComplete")
                                });
                            }
                        }
                    }
                }

                return response;
            }
            catch
            {
                return new EsealRentDetailResponse
                {
                    success = false,
                    message = "Gagal memuat detail sewa. Silakan coba lagi.",
                    header = null,
                    items = new List<EsealRentDetailItem>(),
                    jobs = new List<EsealRentDetailJob>()
                };
            }
        }

        private static EsealRentDetailHeader MapDetailHeader(IDataRecord reader)
        {
            return new EsealRentDetailHeader
            {
                PoID = ReadString(reader, "PoID"),
                PONumber = ReadString(reader, "PONumber"),
                OrderDate = FormatDateDisplay(ReadNullableDate(reader, "OrderDate")),
                CustID = ReadString(reader, "CustID"),
                CustomerName = ReadString(reader, "CustomerName"),
                PicName = ReadString(reader, "PicName"),
                PicMobilePhone = ReadString(reader, "PicMobilePhone"),
                OfficePhone = ReadString(reader, "OfficePhone"),
                PoTypeID = ReadString(reader, "PoTypeID"),
                RentalType = ReadString(reader, "RentalType"),
                ExpiredDate = FormatDateDisplay(ReadNullableDate(reader, "ExpiredDate")),
                PoStatus = ReadString(reader, "PoStatus"),
                TotalItem = ReadInt(reader, "TotalItem"),
                TotalUnit = ReadInt(reader, "TotalUnit"),
                TotalQuantityDone = ReadInt(reader, "TotalQuantityDone"),
                TotalPrice = ReadDecimal(reader, "TotalPrice"),
                TotalInstallFee = ReadDecimal(reader, "TotalInstallFee"),
                TotalMonthlyFee = ReadDecimal(reader, "TotalMonthlyFee"),
                GrandTotal = ReadDecimal(reader, "GrandTotal"),
                TotalJob = ReadInt(reader, "TotalJob"),
                TotalClosedJob = ReadInt(reader, "TotalClosedJob"),
                TotalPendingJob = ReadInt(reader, "TotalPendingJob"),
                EvidenceStatusCode = ReadString(reader, "EvidenceStatusCode"),
                EvidenceStatus = ReadString(reader, "EvidenceStatus")
            };
        }

        private static string FormatDateDisplay(DateTime? value)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }

            return value.Value.ToString("dd MMM yyyy", new CultureInfo("id-ID"));
        }

        private static decimal ReadDecimal(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return 0m;
            }

            return Convert.ToDecimal(reader[columnName]);
        }

        private void ExportRowsToCsv(List<EsealRentRowItem> rows, string period)
        {
            var exportRows = (rows ?? new List<EsealRentRowItem>())
                .Select(row => BuildDisplayRow(row))
                .ToList();
            var sb = new StringBuilder();
            sb.AppendLine("Nama Customer,Nama PIC,No. HP PIC,Tipe Sewa,Total Unit,Tanggal Order,Tanggal Expired,Harga,Status Evidence");

            foreach (EsealRentRowItem row in exportRows)
            {
                string evidenceLabel = ResolveEvidenceLabel(row.EvidenceStatusCode, row.EvidenceStatus);
                sb.AppendLine(string.Join(",",
                    CsvCell(row.Customer),
                    CsvCell(row.Pic),
                    CsvCell(row.PhoneNo),
                    CsvCell(row.RentalType),
                    CsvCell(row.TotalUnitDisplay),
                    CsvCell(row.OrderDateDisplay),
                    CsvCell(row.ExpiredDateDisplay),
                    CsvCell(row.PriceDisplay),
                    CsvCell(evidenceLabel)));
            }

            string fileName = "dashboard_eseal_rent_" + (period ?? DateTime.Now.ToString("yyyy-MM")) + ".csv";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = string.Empty;
            Response.ContentType = "text/csv";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private static string ResolveEvidenceLabel(string evidenceStatusCode, string evidenceStatus)
        {
            string code = (evidenceStatusCode ?? string.Empty).Trim().ToUpperInvariant();
            if (code == "COMPLETE")
            {
                return "Evidence Lengkap";
            }

            if (code == "INCOMPLETE")
            {
                return "Belum Lengkap";
            }

            return DisplayOrDash(evidenceStatus);
        }

        private static string CsvCell(string value)
        {
            string safe = (value ?? string.Empty).Replace("\"", "\"\"");
            return "\"" + safe + "\"";
        }

        private void ResetSummary()
        {
            lblSoDaily.InnerText = "0";
            lblSoWeekly.InnerText = "0";
            lblSoMonthly.InnerText = "0";
            lblTotalPackage.InnerText = "0";
        }

        private void BindEmptyCharts()
        {
            BindCharts(new EsealRentDashboardData());
        }

        private void BindEmptyGrid()
        {
            rptDetail.DataSource = new List<EsealRentRowItem>();
            rptDetail.DataBind();
            lblGridCount.InnerText = "0";
            lblGridEmpty.Visible = true;
        }

        private void ResolvePeriodDateRange(string period, out DateTime dateFrom, out DateTime dateTo)
        {
            string normalized = NormalizePeriode(period);
            int year = int.Parse(normalized.Substring(0, 4));
            int month = int.Parse(normalized.Substring(5, 2));
            dateFrom = new DateTime(year, month, 1);
            dateTo = dateFrom.AddMonths(1).AddDays(-1);
        }

        private string NormalizePeriode(string value)
        {
            string source = (value ?? string.Empty).Trim();
            DateTime parsedDate;
            CultureInfo[] cultures = new[]
            {
                new CultureInfo("id-ID"),
                new CultureInfo("en-US"),
                CultureInfo.InvariantCulture
            };
            string[] formats = new[] { "yyyy-MM", "MM-yyyy", "MMMM yyyy", "MMM yyyy", "yyyy-MM-dd", "yyyy-MM-01" };

            foreach (CultureInfo culture in cultures)
            {
                if (DateTime.TryParseExact(source, formats, culture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM");
                }
            }

            if (DateTime.TryParse(source + "-01", out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM");
            }

            if (DateTime.TryParse(source, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM");
            }

            return DateTime.Now.ToString("yyyy-MM");
        }

        private string FormatPeriodeDisplay(string periodeValue)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact((periodeValue ?? string.Empty).Trim(), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate.ToString("MMMM yyyy", new CultureInfo("id-ID"));
            }

            return DateTime.Now.ToString("MMMM yyyy", new CultureInfo("id-ID"));
        }

        private string NormalizeRentalType(string value)
        {
            string rentalType = (value ?? string.Empty).Trim();
            if (rentalType.Equals("Daily", StringComparison.OrdinalIgnoreCase)
                || rentalType.Equals("Weekly", StringComparison.OrdinalIgnoreCase)
                || rentalType.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
            {
                return rentalType.Substring(0, 1).ToUpper() + rentalType.Substring(1).ToLower();
            }

            return string.Empty;
        }

        private string NormalizeEvidenceStatus(string value)
        {
            string status = (value ?? string.Empty).Trim().ToLowerInvariant();
            if (status == "belum_lengkap" || status == "lengkap"
                || status == "incomplete" || status == "complete")
            {
                return status;
            }

            return string.Empty;
        }

        private string MapPoTypeId(string rentalType)
        {
            switch ((rentalType ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "daily":
                    return PoTypeDaily;
                case "weekly":
                    return PoTypeWeekly;
                case "monthly":
                    return PoTypeMonthly;
                default:
                    return null;
            }
        }

        private string MapEvidenceStatus(string value)
        {
            switch ((value ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "lengkap":
                case "complete":
                    return "COMPLETE";
                case "belum_lengkap":
                case "incomplete":
                    return "INCOMPLETE";
                default:
                    return null;
            }
        }

        private static string DisplayOrDash(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
        }

        private static bool HasColumn(IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ReadString(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(reader[columnName]);
        }

        private static int ReadInt(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        private static int? ReadNullableInt(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        private static decimal? ReadNullableDecimal(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(reader[columnName]);
        }

        private static DateTime? ReadNullableDate(IDataRecord reader, string columnName)
        {
            if (!HasColumn(reader, columnName) || reader[columnName] == DBNull.Value)
            {
                return null;
            }

            if (reader[columnName] is DateTime)
            {
                return (DateTime)reader[columnName];
            }

            DateTime parsed;
            if (DateTime.TryParse(Convert.ToString(reader[columnName]), out parsed))
            {
                return parsed;
            }

            return null;
        }
    }
}
