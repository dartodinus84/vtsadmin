using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_stok_teknisi : System.Web.UI.Page
    {
        private const string MenuAccessId = "MNUDASHSTOKTEK";
        private const string SessionDeviceDetailExport = "RecStokTeknisiDeviceDetail";
        private const string SessionAksesorisDetailExport = "RecStokTeknisiAksesorisDetail";

        protected DropDownList ddlBranchAlat;
        protected DropDownList ddlTeknisi;
        protected DropDownList ddlTypeAlat;
        protected DropDownList ddlStatusBucket;
        protected DropDownList ddlStatusDevice;
        protected DropDownList ddlBranchAksesoris;
        protected DropDownList ddlTeknisiAksesoris;
        protected Button btnTampilkanAlat;
        protected Button btnTampilAksesoris;
        protected Button btnTampilSisaAksesoris;
        protected Button btnExportAlatXls;
        protected Button btnExportAksesorisXls;

        protected string DeviceSummaryRowsHtml { get; private set; }
        protected string AksesorisSummaryRowsHtml { get; private set; }
        protected string DeviceDetailRowsHtml { get; private set; }
        protected string AksesorisDetailRowsHtml { get; private set; }
        protected string ErrorMessage { get; private set; }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for GridView.RenderControl during Excel export.
        }

        protected override void OnPreInit(EventArgs e)
        {
            // Harus sebelum Site.Master Page_Load agar AJAX tidak kena redirect HTML / full render.
            if (HandleUnitDetailAjaxRequest())
            {
                return;
            }

            base.OnPreInit(e);
        }

        private bool HandleUnitDetailAjaxRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("unit_detail", StringComparison.OrdinalIgnoreCase)
                && !action.Equals("unit_export", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                Dictionary<string, object> payload = ReadJsonPayloadDictionary();

                if (action.Equals("unit_detail", StringComparison.OrdinalIgnoreCase))
                {
                    UnitDetailPageResponse result = GetUnitDetailPage(
                        GetPayloadString(payload, "panel"),
                        GetPayloadString(payload, "technicianId"),
                        GetPayloadString(payload, "deviceTypeId"),
                        GetPayloadString(payload, "statusBucket"),
                        GetPayloadString(payload, "metric"),
                        GetPayloadString(payload, "deviceStatus"),
                        GetPayloadString(payload, "branchId"),
                        GetPayloadInt(payload, "pageNumber", 1),
                        GetPayloadInt(payload, "pageSize", 10));
                    WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object> { { "d", result } }));
                    return true;
                }

                UnitDetailExportResponse exportResult = ExportUnitDetailXls(
                    GetPayloadString(payload, "panel"),
                    GetPayloadString(payload, "technicianId"),
                    GetPayloadString(payload, "deviceTypeId"),
                    GetPayloadString(payload, "statusBucket"),
                    GetPayloadString(payload, "metric"),
                    GetPayloadString(payload, "deviceStatus"),
                    GetPayloadString(payload, "branchId"));
                WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object> { { "d", exportResult } }));
                return true;
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                object error;
                if (action.Equals("unit_export", StringComparison.OrdinalIgnoreCase))
                {
                    error = new UnitDetailExportResponse
                    {
                        success = false,
                        message = ex.Message ?? "Gagal export detail unit.",
                        fileName = string.Empty,
                        html = string.Empty,
                        truncated = false,
                        exportedCount = 0,
                        totalCount = 0
                    };
                }
                else
                {
                    error = new UnitDetailPageResponse
                    {
                        success = false,
                        message = ex.Message ?? "Gagal memproses detail unit.",
                        totalCount = 0,
                        pageNumber = 1,
                        pageSize = 10,
                        rows = new List<UnitDetailItem>()
                    };
                }
                WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object> { { "d", error } }));
                return true;
            }
        }

        private static void WriteRawJsonAndEnd(string json)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearHeaders();
            response.BufferOutput = true;
            response.TrySkipIisCustomErrors = true;
            response.StatusCode = 200;
            response.ContentType = "application/json; charset=utf-8";
            response.Charset = "utf-8";
            response.Write(json ?? "{}");
            response.Flush();
            response.SuppressContent = true;
            response.End();
        }

        private static string SerializeToJson(object payload)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue
            };
            return serializer.Serialize(payload ?? new { });
        }

        private static Dictionary<string, object> ReadJsonPayloadDictionary()
        {
            try
            {
                HttpRequest request = HttpContext.Current != null ? HttpContext.Current.Request : null;
                if (request == null || request.InputStream == null)
                {
                    return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                }

                if (request.InputStream.CanSeek)
                {
                    request.InputStream.Position = 0;
                }

                string body;
                using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                {
                    body = reader.ReadToEnd();
                }

                body = (body ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(body))
                {
                    return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = int.MaxValue
                };
                Dictionary<string, object> parsed = serializer.Deserialize<Dictionary<string, object>>(body);
                if (parsed == null)
                {
                    return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                }

                return new Dictionary<string, object>(parsed, StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private static string GetPayloadString(Dictionary<string, object> payload, string key)
        {
            if (payload == null || string.IsNullOrWhiteSpace(key))
                return string.Empty;

            object value;
            if (!payload.TryGetValue(key, out value) || value == null)
                return string.Empty;

            return Convert.ToString(value) ?? string.Empty;
        }

        private static int GetPayloadInt(Dictionary<string, object> payload, string key, int fallback)
        {
            string raw = GetPayloadString(payload, key);
            int parsed;
            if (int.TryParse(raw, out parsed))
                return parsed;
            return fallback;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceSummaryRowsHtml = EmptyRow(2);
            AksesorisSummaryRowsHtml = EmptyRow(2);
            DeviceDetailRowsHtml = EmptyRow(5, "Belum ada data. Silakan atur filter lalu klik Tampilkan.");
            AksesorisDetailRowsHtml = EmptyRow(7, "Pilih teknisi request (atau SEMUA TEKNISI) lalu klik Tampilkan.");
            ErrorMessage = string.Empty;

            try
            {
                // Export XLS butuh full postback (bukan async UpdatePanel), kalau tidak file tidak ter-download
                SiteMaster siteMaster = this.Master as SiteMaster;
                if (siteMaster != null)
                {
                    if (btnExportAlatXls != null)
                        siteMaster.RegisterPostBackTrigger(btnExportAlatXls);
                    if (btnExportAksesorisXls != null)
                        siteMaster.RegisterPostBackTrigger(btnExportAksesorisXls);
                }

                ClsType clType = new ClsType();

                if (Session["ClsTypeAccessMenu"] == null ||
                    !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains(MenuAccessId))
                {
                    Response.Redirect("dashboard.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (Session["ClsTypeIsLogin"] == null ||
                    !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    Response.Redirect("login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (!IsPostBack)
                {
                    BindFilters();
                }

                LoadSummaries();
                LoadDeviceDetail();
                RestoreAksesorisDetailIfNeeded();
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
            }
        }

        protected void btnTampilkanAlat_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = string.Empty;
                LoadDeviceDetail();
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
                DeviceDetailRowsHtml = EmptyRow(5);
            }
        }

        protected void btnTampilAksesoris_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = string.Empty;
                LoadAksesorisFromFilter(onlySisa: false);
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
                AksesorisDetailRowsHtml = EmptyRow(7);
            }
        }

        protected void btnTampilSisaAksesoris_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = string.Empty;
                LoadAksesorisFromFilter(onlySisa: true);
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
                AksesorisDetailRowsHtml = EmptyRow(7);
            }
        }

        private void LoadAksesorisFromFilter(bool onlySisa)
        {
            string technicianId = GetSelectedValue(ddlTeknisiAksesoris);
            string branchId = NormalizeFilterId(GetSelectedValue(ddlBranchAksesoris), "ALL");
            if (string.IsNullOrWhiteSpace(technicianId))
            {
                ViewState["AksesorisMode"] = null;
                ViewState["AksesorisTech"] = null;
                ViewState["AksesorisBranch"] = null;
                ViewState["AksesorisOnlySisa"] = null;
                Session[SessionAksesorisDetailExport] = null;
                AksesorisDetailRowsHtml = EmptyRow(7, "Pilih teknisi request (atau SEMUA TEKNISI).");
                return;
            }

            bool showAll = string.Equals(technicianId, "ALL", StringComparison.OrdinalIgnoreCase);
            ViewState["AksesorisMode"] = showAll ? "ALL" : "ONE";
            ViewState["AksesorisTech"] = showAll ? "ALL" : technicianId;
            ViewState["AksesorisBranch"] = branchId;
            ViewState["AksesorisOnlySisa"] = onlySisa ? "1" : "0";
            LoadAksesorisDetail(showAll ? "ALL" : technicianId, showAll: showAll, onlySisa: onlySisa, branchId: branchId);
        }

        protected void btnExportAlatXls_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = string.Empty;
                DataTable dt = Session[SessionDeviceDetailExport] as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    // Refresh from current filters if session empty
                    LoadDeviceDetail();
                    dt = Session[SessionDeviceDetailExport] as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    ErrorMessage = "No records found";
                    return;
                }

                ExportDataTableToXls(dt, "stok_alat_teknisi_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Response.End()
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
            }
        }

        protected void btnExportAksesorisXls_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorMessage = string.Empty;
                DataTable dt = Session[SessionAksesorisDetailExport] as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    ErrorMessage = "No records found. Tampilkan data aksesoris terlebih dahulu.";
                    return;
                }

                ExportDataTableToXls(dt, "stok_aksesoris_teknisi_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Response.End()
            }
            catch (Exception ex)
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message);
            }
        }

        private void RestoreAksesorisDetailIfNeeded()
        {
            string mode = Convert.ToString(ViewState["AksesorisMode"] ?? string.Empty);
            bool onlySisa = string.Equals(Convert.ToString(ViewState["AksesorisOnlySisa"] ?? "0"), "1", StringComparison.OrdinalIgnoreCase);
            string branchId = Convert.ToString(ViewState["AksesorisBranch"] ?? "ALL");
            if (string.Equals(mode, "ALL", StringComparison.OrdinalIgnoreCase))
            {
                LoadAksesorisDetail("ALL", showAll: true, onlySisa: onlySisa, branchId: branchId);
            }
            else if (string.Equals(mode, "ONE", StringComparison.OrdinalIgnoreCase))
            {
                string tech = Convert.ToString(ViewState["AksesorisTech"] ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(tech))
                    LoadAksesorisDetail(tech, showAll: false, onlySisa: onlySisa, branchId: branchId);
            }
        }

        private void BindFilters()
        {
            BindBranchDropdown(ddlBranchAlat);
            BindBranchDropdown(ddlBranchAksesoris);
            BindTechnicianDropdown(ddlTeknisi, GetSelectedValue(ddlBranchAlat), includeAll: true, allText: "SEMUA TEKNISI");
            BindTechnicianDropdown(ddlTeknisiAksesoris, GetSelectedValue(ddlBranchAksesoris), includeAll: true, allText: "SEMUA TEKNISI");
            BindDeviceTypeDropdown();
            BindStatusBucketDropdown();
            BindStatusDeviceDropdown();
        }

        protected void ddlBranchAlat_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTechnicianDropdown(ddlTeknisi, GetSelectedValue(ddlBranchAlat), includeAll: true, allText: "SEMUA TEKNISI");
        }

        protected void ddlBranchAksesoris_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTechnicianDropdown(ddlTeknisiAksesoris, GetSelectedValue(ddlBranchAksesoris), includeAll: true, allText: "SEMUA TEKNISI");
        }

        private void BindBranchDropdown(DropDownList ddl)
        {
            if (ddl == null)
                return;

            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("SEMUA BRANCH", "ALL"));

            DataTable dt = ExecSp("sp_rpt_stok_teknisi_filter_branch");
            if (dt == null)
                return;

            foreach (DataRow row in dt.Rows)
            {
                string id = ToSafeString(row, "BranchID");
                string name = ToSafeString(row, "BranchName");
                if (string.IsNullOrWhiteSpace(id))
                    continue;
                ddl.Items.Add(new ListItem(name, id));
            }
        }

        private void BindTechnicianDropdown(DropDownList ddl, string branchId, bool includeAll, string allText)
        {
            if (ddl == null)
                return;

            ddl.Items.Clear();
            if (includeAll)
                ddl.Items.Add(new ListItem(allText, "ALL"));
            else
                ddl.Items.Add(new ListItem(allText, string.Empty));

            string branch = NormalizeFilterId(branchId, "ALL");
            string sql = string.Format(
                "sp_rpt_stok_teknisi_filter_technician '{0}'",
                EscapeSqlLiteral(branch));
            DataTable dt = ExecSp(sql);
            if (dt == null)
                return;

            foreach (DataRow row in dt.Rows)
            {
                string id = ToSafeString(row, "TechnicianID");
                string name = ToSafeString(row, "TechnicianName");
                if (string.IsNullOrWhiteSpace(id))
                    continue;

                if (row.Table.Columns.Contains("Status") &&
                    string.Equals(ToSafeString(row, "Status"), "DE", StringComparison.OrdinalIgnoreCase))
                    continue;

                ddl.Items.Add(new ListItem(name, id));
            }
        }

        private void BindDeviceTypeDropdown()
        {
            ddlTypeAlat.Items.Clear();
            ddlTypeAlat.Items.Add(new ListItem("SEMUA TYPE ALAT", "ALL"));

            // sp_rpt_stok_teknisi_filter_device_type: WHERE ISNULL(Status,'') <> 'DE'
            DataTable dt = ExecSp("sp_rpt_stok_teknisi_filter_device_type 'GPS'");
            if (dt == null)
                return;

            foreach (DataRow row in dt.Rows)
            {
                string id = ToSafeString(row, "DeviceTypeID");
                string desc = ToSafeString(row, "DeviceTypeDesc");
                if (string.IsNullOrWhiteSpace(id))
                    continue;

                if (row.Table.Columns.Contains("Status") &&
                    string.Equals(ToSafeString(row, "Status"), "DE", StringComparison.OrdinalIgnoreCase))
                    continue;

                ddlTypeAlat.Items.Add(new ListItem(desc, id));
            }
        }

        private void BindStatusBucketDropdown()
        {
            ddlStatusBucket.Items.Clear();
            string[] buckets =
            {
                "STOK DITEKNISI",
                "STOK CUSTOMER",
                "TERPASANG",
                "STOK DIKANTOR",
                "HILANG",
                "BROKEN DITEKNISI",
                "BROKEN DIKANTOR"
            };

            foreach (string bucket in buckets)
                ddlStatusBucket.Items.Add(new ListItem(bucket, bucket));

            ddlStatusBucket.SelectedValue = "STOK DITEKNISI";
        }

        private void BindStatusDeviceDropdown()
        {
            if (ddlStatusDevice == null)
                return;

            // Posisi di teknisi: mst_device.Status bisa MT / MQ / DS / BR
            ddlStatusDevice.Items.Clear();
            ddlStatusDevice.Items.Add(new ListItem("SEMUA (MT/MQ/DS/BR)", "ALL"));
            ddlStatusDevice.Items.Add(new ListItem("MT - Stok di Teknisi", "MT"));
            ddlStatusDevice.Items.Add(new ListItem("MQ - Mutasi QC", "MQ"));
            ddlStatusDevice.Items.Add(new ListItem("DS - Hilang", "DS"));
            ddlStatusDevice.Items.Add(new ListItem("BR - Broken", "BR"));
            ddlStatusDevice.SelectedValue = "ALL";
        }

        private void LoadSummaries()
        {
            DataTable deviceSummary = ExecSp("sp_rpt_stok_teknisi_device_summary 'GPS'");
            DeviceSummaryRowsHtml = BuildDeviceSummaryRows(deviceSummary);

            DataTable aksesorisSummary = ExecSp("sp_rpt_stok_teknisi_aksesoris_summary");
            AksesorisSummaryRowsHtml = BuildAksesorisSummaryRows(aksesorisSummary);
        }

        private void LoadDeviceDetail()
        {
            string branchId = NormalizeFilterId(GetSelectedValue(ddlBranchAlat), "ALL");
            string technicianId = NormalizeFilterId(GetSelectedValue(ddlTeknisi), "ALL");
            string deviceTypeId = NormalizeFilterId(GetSelectedValue(ddlTypeAlat), "ALL");
            string statusBucket = string.IsNullOrWhiteSpace(GetSelectedValue(ddlStatusBucket))
                ? "STOK DITEKNISI"
                : GetSelectedValue(ddlStatusBucket);
            string deviceStatus = NormalizeFilterId(GetSelectedValue(ddlStatusDevice), "ALL");

            string sql = string.Format(
                "sp_rpt_stok_teknisi_device_detail '{0}','{1}','{2}','GPS','{3}','{4}'",
                EscapeSqlLiteral(technicianId),
                EscapeSqlLiteral(deviceTypeId),
                EscapeSqlLiteral(statusBucket),
                EscapeSqlLiteral(deviceStatus),
                EscapeSqlLiteral(branchId));

            DataTable dt = ExecSp(sql);
            DataTable exportDt = BuildDeviceDetailExportTable(dt);
            Session[SessionDeviceDetailExport] = exportDt;
            DeviceDetailRowsHtml = BuildDeviceDetailRows(dt, technicianId, deviceTypeId, statusBucket, deviceStatus, branchId);
        }

        private void LoadAksesorisDetail(string technicianId, bool showAll, bool onlySisa = false, string branchId = "ALL")
        {
            string sql = string.Format(
                "sp_rpt_stok_teknisi_aksesoris_detail '{0}', {1}, '{2}'",
                EscapeSqlLiteral(NormalizeFilterId(technicianId, "ALL")),
                showAll ? "1" : "0",
                EscapeSqlLiteral(NormalizeFilterId(branchId, "ALL")));

            DataTable dt = ExecSp(sql);
            if (onlySisa && dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("Sisa"))
            {
                DataTable filtered = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    if (ToInt(row, "Sisa") > 0)
                        filtered.ImportRow(row);
                }
                dt = filtered;
            }

            DataTable exportDt = BuildAksesorisDetailExportTable(dt);
            Session[SessionAksesorisDetailExport] = exportDt;
            AksesorisDetailRowsHtml = BuildAksesorisDetailRows(dt);
        }

        private void ExportDataTableToXls(DataTable dt, string fileName)
        {
            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.AllowPaging = false;
                gv.DataBind();
                gv.RenderControl(hw);

                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + fileName + "\"");
                Response.Charset = "utf-8";
                Response.ContentEncoding = Encoding.UTF8;
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write('\uFEFF');
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        private static DataTable BuildDeviceDetailExportTable(DataTable source)
        {
            DataTable export = new DataTable();
            export.Columns.Add("Teknisi", typeof(string));
            export.Columns.Add("Type Alat", typeof(string));
            export.Columns.Add("Status", typeof(string));
            export.Columns.Add("Status Device", typeof(string));
            export.Columns.Add("Qty", typeof(int));

            if (source == null || source.Rows.Count == 0)
                return export;

            int totalQty = 0;
            foreach (DataRow row in source.Rows)
            {
                string teknisi = ToSafeString(row, "Teknisi");
                string typeAlat = ToSafeString(row, "TypeAlat");
                string status = ToSafeString(row, "Status");
                string deviceStatus = ToSafeString(row, "DeviceStatus");
                if (string.IsNullOrWhiteSpace(teknisi) &&
                    string.IsNullOrWhiteSpace(typeAlat) &&
                    string.IsNullOrWhiteSpace(status))
                    continue;

                int qty = ToInt(row, "Qty");
                totalQty += qty;
                export.Rows.Add(teknisi, typeAlat, status, deviceStatus, qty);
            }

            if (export.Rows.Count > 0)
                export.Rows.Add("TOTAL", string.Empty, string.Empty, string.Empty, totalQty);

            return export;
        }

        private static DataTable BuildAksesorisDetailExportTable(DataTable source)
        {
            DataTable export = new DataTable();
            export.Columns.Add("Teknisi Request", typeof(string));
            export.Columns.Add("Aksesoris", typeof(string));
            export.Columns.Add("Qty Cut Off", typeof(int));
            export.Columns.Add("Masuk", typeof(int));
            export.Columns.Add("Terpakai", typeof(int));
            export.Columns.Add("Kembali", typeof(int));
            export.Columns.Add("Sisa", typeof(int));

            if (source == null || source.Rows.Count == 0)
                return export;

            foreach (DataRow row in source.Rows)
            {
                string teknisi = ToSafeString(row, "TeknisiRequest");
                string aksesoris = ToSafeString(row, "Aksesoris");
                if (string.IsNullOrWhiteSpace(teknisi) && string.IsNullOrWhiteSpace(aksesoris))
                    continue;

                export.Rows.Add(
                    teknisi,
                    aksesoris,
                    ToInt(row, "QtyCutOff"),
                    ToInt(row, "Masuk"),
                    ToInt(row, "Terpakai"),
                    ToInt(row, "Kembali"),
                    ToInt(row, "Sisa"));
            }

            return export;
        }

        private string BuildDeviceSummaryRows(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return EmptyRow(2);

            var ordered = dt.AsEnumerable()
                .OrderBy(r => ToInt(r, "SortOrder"))
                .ToList();

            StringBuilder html = new StringBuilder();
            foreach (DataRow row in ordered)
            {
                string status = ToSafeString(row, "Status");
                int qty = ToInt(row, "Qty");
                bool highlight = string.Equals(status, "TOTAL", StringComparison.OrdinalIgnoreCase);
                html.Append(highlight ? "<tr class='row-highlight-orange'>" : "<tr>");
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(status));
                html.Append("<td class='text-right'>");
                if (!highlight && qty > 0)
                {
                    html.Append(BuildQtyLink(
                        qty,
                        "device",
                        "ALL",
                        "ALL",
                        status,
                        string.Empty,
                        "Detail Device - " + status,
                        "Semua teknisi / semua type alat"));
                }
                else
                {
                    html.AppendFormat("{0:N0}", qty);
                }
                html.Append("</td></tr>");
            }

            return html.ToString();
        }

        private string BuildAksesorisSummaryRows(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return EmptyRow(2);

            var ordered = dt.AsEnumerable()
                .OrderBy(r => ToInt(r, "SortOrder"))
                .ToList();

            StringBuilder html = new StringBuilder();
            foreach (DataRow row in ordered)
            {
                string indikator = ToSafeString(row, "Indikator");
                int qty = ToInt(row, "Qty");
                bool highlight = string.Equals(indikator, "Total Sisa Stok Teknisi", StringComparison.OrdinalIgnoreCase);
                string metric = MapAksesorisSummaryMetric(indikator);

                html.Append(highlight ? "<tr class='row-highlight-orange'>" : "<tr>");
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(indikator));
                html.Append("<td class='text-right'>");
                if (qty > 0 && !string.IsNullOrEmpty(metric))
                {
                    html.Append(BuildQtyLink(
                        qty,
                        "aksesoris",
                        "ALL",
                        "ALL",
                        string.Empty,
                        metric,
                        "Detail Aksesoris - " + indikator,
                        "Semua teknisi"));
                }
                else
                {
                    html.AppendFormat("{0:N0}", qty);
                }
                html.Append("</td></tr>");
            }

            return html.ToString();
        }

        private string BuildDeviceDetailRows(
            DataTable dt,
            string filterTechId,
            string filterTypeId,
            string filterBucket,
            string filterDeviceStatus,
            string filterBranchId)
        {
            if (dt == null || dt.Rows.Count == 0)
                return EmptyRow(5);

            bool hasRealData = dt.AsEnumerable().Any(r =>
                !string.IsNullOrWhiteSpace(ToSafeString(r, "Teknisi")) ||
                !string.IsNullOrWhiteSpace(ToSafeString(r, "TypeAlat")) ||
                ToInt(r, "Qty") > 0);

            if (!hasRealData)
                return EmptyRow(5);

            StringBuilder html = new StringBuilder();
            int totalQty = 0;
            foreach (DataRow row in dt.Rows)
            {
                string teknisi = ToSafeString(row, "Teknisi");
                string typeAlat = ToSafeString(row, "TypeAlat");
                string status = ToSafeString(row, "Status");
                string deviceStatus = ToSafeString(row, "DeviceStatus");
                string technicianId = ToSafeString(row, "TechnicianID");
                string deviceTypeId = ToSafeString(row, "DeviceTypeID");
                int qty = ToInt(row, "Qty");
                if (string.IsNullOrWhiteSpace(teknisi) &&
                    string.IsNullOrWhiteSpace(typeAlat) &&
                    string.IsNullOrWhiteSpace(status))
                    continue;

                totalQty += qty;
                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(teknisi));
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(typeAlat));
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(status));
                html.AppendFormat("<td><span class='label label-default'>{0}</span></td>",
                    HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(deviceStatus) ? "-" : deviceStatus));
                html.Append("<td class='text-right'>");
                if (qty > 0)
                {
                    html.Append(BuildQtyLink(
                        qty,
                        "device",
                        string.IsNullOrWhiteSpace(technicianId) ? "ALL" : technicianId,
                        string.IsNullOrWhiteSpace(deviceTypeId) ? "ALL" : deviceTypeId,
                        status,
                        string.Empty,
                        string.IsNullOrWhiteSpace(deviceStatus) ? "ALL" : deviceStatus,
                        string.IsNullOrWhiteSpace(filterBranchId) ? "ALL" : filterBranchId,
                        "Detail Device - " + status,
                        teknisi + " / " + typeAlat + " / " + deviceStatus));
                }
                else
                {
                    html.AppendFormat("{0:N0}", qty);
                }
                html.Append("</td></tr>");
            }

            if (html.Length == 0)
                return EmptyRow(5);

            html.Append("<tr class='stok-total-row'>");
            html.Append("<td colspan='4'><strong>TOTAL</strong></td>");
            html.Append("<td class='text-right'><strong>");
            if (totalQty > 0)
            {
                html.Append(BuildQtyLink(
                    totalQty,
                    "device",
                    string.IsNullOrWhiteSpace(filterTechId) ? "ALL" : filterTechId,
                    string.IsNullOrWhiteSpace(filterTypeId) ? "ALL" : filterTypeId,
                    string.IsNullOrWhiteSpace(filterBucket) ? "STOK DITEKNISI" : filterBucket,
                    string.Empty,
                    string.IsNullOrWhiteSpace(filterDeviceStatus) ? "ALL" : filterDeviceStatus,
                    string.IsNullOrWhiteSpace(filterBranchId) ? "ALL" : filterBranchId,
                    "Detail Device - TOTAL",
                    "Filter aktif / seluruh baris di tabel"));
            }
            else
            {
                html.AppendFormat("{0:N0}", totalQty);
            }
            html.Append("</strong></td></tr>");

            return html.ToString();
        }

        private string BuildAksesorisDetailRows(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return EmptyRow(7);

            bool hasRealData = dt.AsEnumerable().Any(r =>
                !string.IsNullOrWhiteSpace(ToSafeString(r, "TeknisiRequest")) ||
                !string.IsNullOrWhiteSpace(ToSafeString(r, "Aksesoris")));

            if (!hasRealData)
                return EmptyRow(7);

            StringBuilder html = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                string teknisi = ToSafeString(row, "TeknisiRequest");
                string aksesoris = ToSafeString(row, "Aksesoris");
                string technicianId = ToSafeString(row, "TechnicianID");
                string deviceTypeId = ToSafeString(row, "DeviceTypeID");
                int qtyCutOff = ToInt(row, "QtyCutOff");
                int masuk = ToInt(row, "Masuk");
                int terpakai = ToInt(row, "Terpakai");
                int kembali = ToInt(row, "Kembali");
                int sisa = ToInt(row, "Sisa");
                if (string.IsNullOrWhiteSpace(teknisi) && string.IsNullOrWhiteSpace(aksesoris))
                    continue;

                string techArg = string.IsNullOrWhiteSpace(technicianId) ? "ALL" : technicianId;
                string typeArg = string.IsNullOrWhiteSpace(deviceTypeId) ? "ALL" : deviceTypeId;
                string branchArg = NormalizeFilterId(GetSelectedValue(ddlBranchAksesoris), "ALL");
                string subtitle = teknisi + " / " + aksesoris;

                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(teknisi));
                html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(aksesoris));
                html.AppendFormat("<td class='text-right'>{0:N0}</td>", qtyCutOff);
                html.Append("<td class='text-right'>");
                html.Append(masuk > 0
                    ? BuildQtyLink(masuk, "aksesoris", techArg, typeArg, string.Empty, "MASUK", "ALL", branchArg, "Detail Aksesoris - Masuk", subtitle)
                    : string.Format("{0:N0}", masuk));
                html.Append("</td><td class='text-right'>");
                html.Append(terpakai > 0
                    ? BuildQtyLink(terpakai, "aksesoris", techArg, typeArg, string.Empty, "TERPAKAI", "ALL", branchArg, "Detail Aksesoris - Terpakai", subtitle)
                    : string.Format("{0:N0}", terpakai));
                html.Append("</td>");
                html.AppendFormat("<td class='text-right'>{0:N0}</td>", kembali);
                html.Append("<td class='text-right'>");
                html.Append(sisa > 0
                    ? BuildQtyLink(sisa, "aksesoris", techArg, typeArg, string.Empty, "SISA", "ALL", branchArg, "Detail Aksesoris - Sisa", subtitle)
                    : string.Format("{0:N0}", sisa));
                html.Append("</td></tr>");
            }

            if (html.Length == 0)
                return EmptyRow(7);

            return html.ToString();
        }

        private static string MapAksesorisSummaryMetric(string indikator)
        {
            if (string.Equals(indikator, "Total Stok Tersedia", StringComparison.OrdinalIgnoreCase))
                return "MASUK";
            if (string.Equals(indikator, "Total Terpakai", StringComparison.OrdinalIgnoreCase))
                return "TERPAKAI";
            if (string.Equals(indikator, "Total Sisa Stok Teknisi", StringComparison.OrdinalIgnoreCase))
                return "SISA";
            return string.Empty;
        }

        private static string BuildQtyLink(
            int qty,
            string panel,
            string technicianId,
            string deviceTypeId,
            string statusBucket,
            string metric,
            string title,
            string subtitle)
        {
            return BuildQtyLink(qty, panel, technicianId, deviceTypeId, statusBucket, metric, "ALL", "ALL", title, subtitle);
        }

        private static string BuildQtyLink(
            int qty,
            string panel,
            string technicianId,
            string deviceTypeId,
            string statusBucket,
            string metric,
            string deviceStatus,
            string branchId,
            string title,
            string subtitle)
        {
            return string.Format(
                "<a href=\"#\" class=\"qty-link\" data-panel=\"{0}\" data-tech=\"{1}\" data-type=\"{2}\" data-bucket=\"{3}\" data-metric=\"{4}\" data-devstatus=\"{5}\" data-branch=\"{6}\" data-title=\"{7}\" data-subtitle=\"{8}\">{9:N0}</a>",
                HttpUtility.HtmlAttributeEncode(panel ?? string.Empty),
                HttpUtility.HtmlAttributeEncode(technicianId ?? "ALL"),
                HttpUtility.HtmlAttributeEncode(deviceTypeId ?? "ALL"),
                HttpUtility.HtmlAttributeEncode(statusBucket ?? string.Empty),
                HttpUtility.HtmlAttributeEncode(metric ?? string.Empty),
                HttpUtility.HtmlAttributeEncode(string.IsNullOrWhiteSpace(deviceStatus) ? "ALL" : deviceStatus),
                HttpUtility.HtmlAttributeEncode(string.IsNullOrWhiteSpace(branchId) ? "ALL" : branchId),
                HttpUtility.HtmlAttributeEncode(title ?? "Detail Unit"),
                HttpUtility.HtmlAttributeEncode(subtitle ?? string.Empty),
                qty);
        }

        public class UnitDetailPageResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
            public int totalCount { get; set; }
            public int pageNumber { get; set; }
            public int pageSize { get; set; }
            public List<UnitDetailItem> rows { get; set; }
        }

        public class UnitDetailExportResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
            public string fileName { get; set; }
            public string html { get; set; }
            public bool truncated { get; set; }
            public int exportedCount { get; set; }
            public int totalCount { get; set; }
        }

        public class UnitDetailItem
        {
            public string DeviceID { get; set; }
            public string NoSN { get; set; }
            public string Teknisi { get; set; }
            public string TypeAlat { get; set; }
            public string Status { get; set; }
            public string DeviceStatus { get; set; }
        }

        private const int UnitExportMaxRows = 10000;
        private const int UnitExportPageSize = 200;

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static UnitDetailPageResponse GetUnitDetailPage(
            string panel,
            string technicianId,
            string deviceTypeId,
            string statusBucket,
            string metric,
            string deviceStatus,
            string branchId,
            int pageNumber,
            int pageSize)
        {
            var response = new UnitDetailPageResponse
            {
                success = false,
                message = string.Empty,
                totalCount = 0,
                pageNumber = pageNumber < 1 ? 1 : pageNumber,
                pageSize = pageSize < 1 ? 10 : (pageSize > 200 ? 200 : pageSize),
                rows = new List<UnitDetailItem>()
            };

            try
            {
                string conn;
                string error;
                if (!TryAuthorizeUnitDetail(out conn, out error))
                {
                    response.message = error;
                    return response;
                }

                string spCall;
                if (!TryBuildUnitDetailSpCall(
                    panel, technicianId, deviceTypeId, statusBucket, metric, deviceStatus, branchId,
                    response.pageNumber, response.pageSize, out spCall, out error))
                {
                    response.message = error;
                    return response;
                }

                DataTable dt = ExecSpStatic(spCall, conn, out error);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    response.message = error;
                    return response;
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (response.totalCount == 0)
                        response.totalCount = ToInt(row, "TotalCount");

                    response.rows.Add(MapUnitDetailItem(row));
                }

                response.success = true;
                response.message = "OK";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static UnitDetailExportResponse ExportUnitDetailXls(
            string panel,
            string technicianId,
            string deviceTypeId,
            string statusBucket,
            string metric,
            string deviceStatus,
            string branchId)
        {
            var response = new UnitDetailExportResponse
            {
                success = false,
                message = string.Empty,
                fileName = "detail_unit_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls",
                html = string.Empty,
                truncated = false,
                exportedCount = 0,
                totalCount = 0
            };

            try
            {
                string conn;
                string error;
                if (!TryAuthorizeUnitDetail(out conn, out error))
                {
                    response.message = error;
                    return response;
                }

                var rows = new List<UnitDetailItem>();
                int page = 1;
                int totalCount = 0;

                while (rows.Count < UnitExportMaxRows)
                {
                    string spCall;
                    if (!TryBuildUnitDetailSpCall(
                        panel, technicianId, deviceTypeId, statusBucket, metric, deviceStatus, branchId,
                        page, UnitExportPageSize, out spCall, out error))
                    {
                        response.message = error;
                        return response;
                    }

                    DataTable dt = ExecSpStatic(spCall, conn, out error);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        response.message = error;
                        return response;
                    }

                    if (dt.Rows.Count == 0)
                        break;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (totalCount == 0)
                            totalCount = ToInt(row, "TotalCount");

                        rows.Add(MapUnitDetailItem(row));
                        if (rows.Count >= UnitExportMaxRows)
                            break;
                    }

                    if (rows.Count >= totalCount || dt.Rows.Count < UnitExportPageSize)
                        break;

                    page++;
                }

                if (rows.Count == 0)
                {
                    response.message = "No records found";
                    return response;
                }

                response.totalCount = totalCount;
                response.exportedCount = rows.Count;
                response.truncated = totalCount > rows.Count;
                response.html = BuildUnitDetailExcelHtml(rows);
                response.success = true;
                response.message = response.truncated
                    ? string.Format("Export berhasil. Ditampilkan {0:N0} dari {1:N0} unit (batas export).", rows.Count, totalCount)
                    : "OK";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        private static bool TryAuthorizeUnitDetail(out string conn, out string error)
        {
            conn = string.Empty;
            error = string.Empty;

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null ||
                context.Session["ClsTypeIsLogin"] == null ||
                context.Session["ClsTypeDBConnStringSQL"] == null)
            {
                error = "Sesi tidak valid. Silakan login ulang.";
                return false;
            }

            if (context.Session["ClsTypeAccessMenu"] == null ||
                !context.Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains(MenuAccessId))
            {
                error = "Akses menu tidak diizinkan.";
                return false;
            }

            conn = context.Session["ClsTypeDBConnStringSQL"].ToString().Trim();
            return true;
        }

        private static bool TryBuildUnitDetailSpCall(
            string panel,
            string technicianId,
            string deviceTypeId,
            string statusBucket,
            string metric,
            string deviceStatus,
            string branchId,
            int pageNumber,
            int pageSize,
            out string spCall,
            out string error)
        {
            spCall = string.Empty;
            error = string.Empty;

            string tech = string.IsNullOrWhiteSpace(technicianId) ? "ALL" : technicianId.Trim();
            string type = string.IsNullOrWhiteSpace(deviceTypeId) ? "ALL" : deviceTypeId.Trim();
            string panelKey = (panel ?? string.Empty).Trim().ToLowerInvariant();
            string devStatus = string.IsNullOrWhiteSpace(deviceStatus) ? "ALL" : deviceStatus.Trim();
            string branch = string.IsNullOrWhiteSpace(branchId) ? "ALL" : branchId.Trim();
            int page = pageNumber < 1 ? 1 : pageNumber;
            int size = pageSize < 1 ? 10 : (pageSize > 200 ? 200 : pageSize);

            if (panelKey == "device")
            {
                string bucket = string.IsNullOrWhiteSpace(statusBucket) ? "STOK DITEKNISI" : statusBucket.Trim();
                if (string.Equals(bucket, "TOTAL", StringComparison.OrdinalIgnoreCase))
                {
                    error = "Detail untuk TOTAL tidak tersedia. Klik qty per status.";
                    return false;
                }

                spCall = string.Format(
                    "sp_rpt_stok_teknisi_device_units '{0}','{1}','{2}','GPS',{3},{4},'{5}','{6}'",
                    EscapeSqlLiteral(tech),
                    EscapeSqlLiteral(type),
                    EscapeSqlLiteral(bucket),
                    page,
                    size,
                    EscapeSqlLiteral(devStatus),
                    EscapeSqlLiteral(branch));
                return true;
            }

            if (panelKey == "aksesoris")
            {
                string mode = string.IsNullOrWhiteSpace(metric) ? "SISA" : metric.Trim().ToUpperInvariant();
                if (mode != "SISA" && mode != "TERPAKAI" && mode != "MASUK")
                    mode = "SISA";

                spCall = string.Format(
                    "sp_rpt_stok_teknisi_aksesoris_units '{0}','{1}','{2}',{3},{4},'{5}'",
                    EscapeSqlLiteral(tech),
                    EscapeSqlLiteral(type),
                    EscapeSqlLiteral(mode),
                    page,
                    size,
                    EscapeSqlLiteral(branch));
                return true;
            }

            error = "Panel detail tidak dikenali.";
            return false;
        }

        private static DataTable ExecSpStatic(string spCall, string conn, out string error)
        {
            error = string.Empty;
            string stErr = string.Empty;
            try
            {
                Recordset rec = new Recordset();
                rec.Open(spCall, conn, ref stErr);
                if (!string.IsNullOrWhiteSpace(stErr))
                {
                    error = stErr;
                    return new DataTable();
                }

                return rec.DataRecord() ?? new DataTable();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return new DataTable();
            }
        }

        private static UnitDetailItem MapUnitDetailItem(DataRow row)
        {
            return new UnitDetailItem
            {
                DeviceID = ToSafeString(row, "DeviceID"),
                NoSN = ToSafeString(row, "NoSN"),
                Teknisi = ToSafeString(row, "Teknisi"),
                TypeAlat = ToSafeString(row, "TypeAlat"),
                Status = ToSafeString(row, "Status"),
                DeviceStatus = ToSafeString(row, "DeviceStatus")
            };
        }

        private static string BuildUnitDetailExcelHtml(List<UnitDetailItem> rows)
        {
            var sb = new StringBuilder();
            sb.Append("<table border='1'>");
            sb.Append("<tr>");
            sb.Append("<th>No</th><th>DeviceID</th><th>NoSN</th><th>Teknisi</th><th>Type</th><th>Status</th><th>Status Device</th>");
            sb.Append("</tr>");

            for (int i = 0; i < rows.Count; i++)
            {
                UnitDetailItem row = rows[i];
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", i + 1);
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.DeviceID ?? string.Empty));
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.NoSN ?? string.Empty));
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.Teknisi ?? string.Empty));
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.TypeAlat ?? string.Empty));
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.Status ?? string.Empty));
                sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row.DeviceStatus ?? string.Empty));
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }

        private DataTable ExecSp(string spCall)
        {
            string conn = GetConn();
            string stErr = string.Empty;
            Recordset rec = new Recordset();
            rec.Open(spCall, conn, ref stErr);

            if (!string.IsNullOrWhiteSpace(stErr))
                throw new InvalidOperationException(stErr);

            DataTable dt = rec.DataRecord();
            return dt ?? new DataTable();
        }

        private string GetConn()
        {
            if (Session["ClsTypeDBConnStringSQL"] == null ||
                string.IsNullOrWhiteSpace(Convert.ToString(Session["ClsTypeDBConnStringSQL"])))
            {
                throw new InvalidOperationException("Session connection string (ClsTypeDBConnStringSQL) tidak ditemukan.");
            }

            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        private static string GetSelectedValue(DropDownList ddl)
        {
            if (ddl == null || ddl.SelectedItem == null)
                return string.Empty;

            return (ddl.SelectedValue ?? string.Empty).Trim();
        }

        private static string NormalizeFilterId(string value, string fallbackAll)
        {
            if (string.IsNullOrWhiteSpace(value))
                return fallbackAll;
            return value.Trim();
        }

        private static string EscapeSqlLiteral(string value)
        {
            if (value == null)
                return string.Empty;
            return value.Replace("'", "''");
        }

        private static string EmptyRow(int colspan, string message = "Tidak ada data.")
        {
            return string.Format(
                "<tr><td colspan='{0}' class='text-center text-muted' style='padding:18px;'>{1}</td></tr>",
                colspan,
                HttpUtility.HtmlEncode(message));
        }

        private static int ToInt(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return 0;

            int parsed;
            if (int.TryParse(Convert.ToString(row[columnName]), out parsed))
                return parsed;

            return 0;
        }

        private static string ToSafeString(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(row[columnName]).Trim();
        }
    }
}
