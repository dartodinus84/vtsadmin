using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_assign_job : System.Web.UI.Page
    {
        public class JobOrderInformationItem
        {
            public string JobID { get; set; }
            public string Customer { get; set; }
            public string CustomerName { get; set; }
            public string BranchName { get; set; }
            public string DeviceTypeID { get; set; }
            public string DeviceTypeDesc { get; set; }
            public int TotalAssign { get; set; }
            public int TotalAssignGps { get; set; }
            public int TotalAssignAcs { get; set; }
            public string LastAssignDate { get; set; }
            public int RemainingUnit { get; set; }
            public int TotalUnit { get; set; }
            public int RemainingUnitGps { get; set; }
            public int RemainingUnitAcs { get; set; }
            public int TotalUnitGps { get; set; }
            public int TotalUnitAcs { get; set; }
            public int TotalUnitGpsDone { get; set; }
            public int TotalUnitAcsDone { get; set; }
            public string Address { get; set; }
            public string MarketingName { get; set; }
            public string DefaultAreaId { get; set; }
            public int CustomerGpsCount { get; set; }
            public int CustomerAcsCount { get; set; }
            public bool IsTransfer { get; set; }
            public string AssignedTechnicianId { get; set; }
            public string AssignedTechnicianName { get; set; }
        }

        public class JobOrderInformationResponse
        {
            public List<JobOrderInformationItem> Rows { get; set; }
            public List<string> BranchOptions { get; set; }
            public int TotalRecords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class SaveAssignResponse
        {
            public string Result { get; set; }
            public string AssignID { get; set; }
            public string Message { get; set; }
        }

        public class TrainingLookupItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public class TrainingLookupResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public List<TrainingLookupItem> Categories { get; set; }
            public List<TrainingLookupItem> Billables { get; set; }
        }

        public class AreaOptionItem
        {
            public string AreaID { get; set; }
            public string AreaName { get; set; }
        }

        public class AreaOptionResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public List<AreaOptionItem> Rows { get; set; }
        }

        public class RefreshAvailabilityResponse
        {
            public string Result { get; set; }
            public string DisplayValue { get; set; }
            public bool CanAssign { get; set; }
            public int RemainingJo { get; set; }
            public bool HasRemainingJo { get; set; }
            public string Message { get; set; }
        }

        public class TechnicianStockAccessoryItem
        {
            public string DeviceTypeDesc { get; set; }
            public int Total { get; set; }
        }

        public class TechnicianStockGpsItem
        {
            public string DeviceTypeID { get; set; }
            public string DeviceTypeDesc { get; set; }
            public int Total { get; set; }
        }

        public class TechnicianStockGsmItem
        {
            public string ProviderID { get; set; }
            public int Total { get; set; }
        }

        public class TechnicianStockResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public int GPSCount { get; set; }
            public int GSMCount { get; set; }
            public int TotalUnit { get; set; }
            public List<TechnicianStockGpsItem> GpsDevices { get; set; }
            public List<TechnicianStockGsmItem> GsmItems { get; set; }
            public List<TechnicianStockAccessoryItem> Accessories { get; set; }
        }

        public class TechnicianGpsDetailItem
        {
            public string DeviceID { get; set; }
            public string NoSN { get; set; }
            public string DeviceTypeDesc { get; set; }
            public string Status { get; set; }
            public string StatusText { get; set; }
        }

        public class TechnicianGpsDetailResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public string TechnicianID { get; set; }
            public string TechnicianName { get; set; }
            public string DeviceTypeID { get; set; }
            public string DeviceTypeDesc { get; set; }
            public List<TechnicianGpsDetailItem> Rows { get; set; }
        }

        public class ClosedJobUnitItem
        {
            public string AssignID { get; set; }
            public int Seq { get; set; }
            public string JobID { get; set; }
            public string TvdID { get; set; }
            public string JobType { get; set; }
            public string MaintTypeID { get; set; }
            public string TechnicianID { get; set; }
            public string DeviceGroupID { get; set; }
            public string DeviceTypeID { get; set; }
            public string DeviceTypeDesc { get; set; }
            public string SchDate { get; set; }
            public string InstallDate { get; set; }
            public string MISDate { get; set; }
            public string CustID { get; set; }
            public string FullName { get; set; }
            public string PoliceNo { get; set; }
            public string NoSN { get; set; }
            public string GSMNo { get; set; }
            public string Status { get; set; }
            public string TechnicianName { get; set; }
            public string Area { get; set; }
            public string Marketing { get; set; }
            public int TotalUnit { get; set; }
        }

        public class ClosedJobCardItem
        {
            public string JobID { get; set; }
            public int TotalUnit { get; set; }
            public List<ClosedJobUnitItem> Units { get; set; }
        }

        public class ClosedJobModalResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public string TechnicianName { get; set; }
            public int TotalClosedJO { get; set; }
            public int TotalClosedUnit { get; set; }
            public bool HasData { get; set; }
            public string HtmlContent { get; set; }
        }

        public class DayTotalJoModalResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public string ScheduleDate { get; set; }
            public int TotalJO { get; set; }
            public int TotalUnit { get; set; }
            public bool HasData { get; set; }
            public string HtmlContent { get; set; }
        }

        public class ScheduleReportRowItem
        {
            public string AssignID { get; set; }
            public int Seq { get; set; }
            public string JobID { get; set; }
            public string JobType { get; set; }
            public string DeviceGroupID { get; set; }
            public string DeviceTypeDesc { get; set; }
            public string CustID { get; set; }
            public string Customer { get; set; }
            public string AreaID { get; set; }
            public string AreaName { get; set; }
            public string PoliceNo { get; set; }
            public string NoSN { get; set; }
            public string GSMNo { get; set; }
            public string InstallDate { get; set; }
            public string MISDate { get; set; }
            public string SchDate { get; set; }
            public string TechnicianName { get; set; }
            public string StatusCode { get; set; }
            public string StatusText { get; set; }
            public string Remark { get; set; }
            public bool CanDelete { get; set; }
        }

        public class ScheduleReportResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public string TechnicianID { get; set; }
            public string TechnicianName { get; set; }
            public string SchDate { get; set; }
            public int TotalUnitSelesai { get; set; }
            public int TotalUnitBelumSelesai { get; set; }
            public List<ScheduleReportRowItem> Rows { get; set; }
        }

        public class DeleteScheduleAssignResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
        }

        public class PerfTechnicianItem
        {
            public string TechnicianID { get; set; }
            public string TechnicianName { get; set; }
        }

        public class PerfTechnicianListResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public List<PerfTechnicianItem> Rows { get; set; }
        }

        public class PerfDailyRowItem
        {
            public string SchDate { get; set; }
            public int DayNo { get; set; }
            public int JoAssignNew { get; set; }
            public int UnitCloseNew { get; set; }
            public int JoCloseNew { get; set; }
            public int JoAssignMaint { get; set; }
            public int UnitCloseMaint { get; set; }
            public int JoCloseMaint { get; set; }
            public int JoAssignTotal { get; set; }
            public int UnitCloseTotal { get; set; }
            public int JoCloseTotal { get; set; }
        }

        public class PerfMonthlyRowItem
        {
            public string PeriodeMonth { get; set; }
            public int JoAssignNew { get; set; }
            public int UnitCloseNew { get; set; }
            public int JoCloseNew { get; set; }
            public int JoAssignMaint { get; set; }
            public int UnitCloseMaint { get; set; }
            public int JoCloseMaint { get; set; }
            public int JoAssignTotal { get; set; }
            public int UnitCloseTotal { get; set; }
            public int JoCloseTotal { get; set; }
        }

        public class PerfChartDataResponse
        {
            public string Result { get; set; }
            public string Message { get; set; }
            public string TechnicianID { get; set; }
            public string TechnicianName { get; set; }
            public List<PerfDailyRowItem> DailyRows { get; set; }
            public List<PerfMonthlyRowItem> MonthlyRows { get; set; }
        }

        // Declared in code-behind to keep implementation in two files.
        protected TextBox txtPeriode;
        protected Button btnApply;
        protected Button btnReset;
        protected HiddenField hfActiveTab;
        protected HiddenField hfDetailStatus;
        protected HiddenField hfIsTechnician;
        protected HiddenField hfDetailJobType;
        protected HiddenField hfDetailMetric;
        protected LinkButton btnTabRefresh;
        protected LinkButton btnOpenDetail;
        protected HtmlGenericControl lblTotalJONew;
        protected HtmlGenericControl lblTotalJOMaint;
        protected HtmlGenericControl lblTotalJOOpenNew;
        protected HtmlGenericControl lblTotalJOOpenMaint;
        protected HtmlGenericControl lblTotalJOScheduledNew;
        protected HtmlGenericControl lblTotalJOScheduledMaint;
        protected HtmlGenericControl lblTotalJOCloseNew;
        protected HtmlGenericControl lblTotalJOCloseMaint;
        protected HtmlGenericControl lblTotalUnitNew;
        protected HtmlGenericControl lblTotalUnitMaint;
        protected HtmlGenericControl lblTotalUnitOpenNew;
        protected HtmlGenericControl lblTotalUnitOpenMaint;
        protected HtmlGenericControl lblTotalUnitScheduledNew;
        protected HtmlGenericControl lblTotalUnitScheduledMaint;
        protected HtmlGenericControl lblTotalUnitCloseNew;
        protected HtmlGenericControl lblTotalUnitCloseMaint;
        protected HtmlGenericControl lblActiveTabTitle;
        protected HtmlGenericControl lblScheduleTabTitle;
        protected HtmlGenericControl lblModalTitle;
        protected HtmlGenericControl lblModalPeriode;
        protected HtmlGenericControl lblPerformanceEmpty;
        protected HtmlGenericControl lblScheduleEmpty;
        protected HtmlGenericControl lblRegionalEmpty;
        protected HtmlGenericControl lblDetailEmpty;
        protected HtmlGenericControl lblDetailTotalGps;
        protected HtmlGenericControl lblDetailTotalAcs;
        protected HtmlGenericControl lblMemberCount;
        protected Literal litPerformanceRows;
        protected Literal litScheduleHeader;
        protected Literal litScheduleRows;
        protected Literal litCapacityRows;
        protected Repeater rptAreaGroupTabs;
        protected Repeater rptRegionalTabs;
        protected Repeater rptDetailJO;

        private const string SessionPeriode = "SessionAssignJobPeriode";
        private const string SessionActiveTab = "SessionAssignJobTab";
        private const string SessionRegionalGroupTab = "SessionAssignJobRegionalGroupTab";
        private const string SessionRegionalTab = "SessionAssignJobRegionalTab";
        private const string SessionItsRegionalGroupTab = "SessionAssignJobItsRegionalGroupTab";
        private const string SessionItsRegionalTab = "SessionAssignJobItsRegionalTab";
        private const string SessionUserSupAreaID = "SessionAssignJobUserSupAreaID";
        private const string SessionUserAreaGroupID = "SessionAssignJobUserAreaGroupID";
        private const string SessionUserIsTechnician = "SessionAssignJobUserIsTechnician";
        private const string SessionUserIsRegion = "SessionAssignJobUserIsRegion";
        private const string ViewStateRegionalTabs = "ViewStateAssignJobRegionalTabs";
        private const string RegionalAllValue = "ALL";
        private const string AreaGroupEastValue = "ARG0000002";
        private const string AreaGroupWestValue = "ARG0000001";
        private static readonly string[] KnownWestSupAreaIds = { "SUP0000001" };
        private static readonly string[] KnownEastSupAreaIds = { "SUP0000010", "SUP0000011", "SUP0000013" };
        private const string DefaultTab = "teknisi";

        protected virtual string FixedActiveTab
        {
            get { return DefaultTab; }
        }

        /// <summary>
        /// When true (IT Support page), schedule/summary/closed lists use trx_job_assign_detail keyed by mst_itsupport.ITID via ItsSupportAssignData.
        /// </summary>
        protected virtual bool UseJobTrainingDataSource
        {
            get { return false; }
        }

        private static bool IsJobTrainingAssignRequestContext()
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Request == null)
            {
                return false;
            }

            // Use execution file path only (never querystring/raw URL) so Teknisi
            // requests cannot be mistaken for IT Support.
            string path = (context.Request.AppRelativeCurrentExecutionFilePath
                ?? context.Request.CurrentExecutionFilePath
                ?? context.Request.Path
                ?? string.Empty).Trim();
            int queryIndex = path.IndexOf('?');
            if (queryIndex >= 0)
            {
                path = path.Substring(0, queryIndex);
            }

            path = path.Replace('\\', '/');
            return path.EndsWith("/dashboard_assign_job_itsupport.aspx", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith("~/dashboard_assign_job_itsupport.aspx", StringComparison.OrdinalIgnoreCase)
                || path.Equals("dashboard_assign_job_itsupport.aspx", StringComparison.OrdinalIgnoreCase)
                || path.Equals("~/dashboard_assign_job_itsupport.aspx", StringComparison.OrdinalIgnoreCase);
        }

        private static string AssignRoleDisplayName()
        {
            return IsJobTrainingAssignRequestContext() ? "IT Support" : "Teknisi";
        }

        private string RegionalTabSessionKey
        {
            get { return UseJobTrainingDataSource ? SessionItsRegionalTab : SessionRegionalTab; }
        }

        private string RegionalGroupTabSessionKey
        {
            get { return UseJobTrainingDataSource ? SessionItsRegionalGroupTab : SessionRegionalGroupTab; }
        }

        private static string GetRegionalTabSessionKeyStatic()
        {
            return IsJobTrainingAssignRequestContext() ? SessionItsRegionalTab : SessionRegionalTab;
        }

        private static string GetRegionalGroupTabSessionKeyStatic()
        {
            return IsJobTrainingAssignRequestContext() ? SessionItsRegionalGroupTab : SessionRegionalGroupTab;
        }

        private sealed class SummarySplitMetrics
        {
            public int TotalJoNew { get; set; }
            public int TotalJoMaint { get; set; }
            public int OpenJoNew { get; set; }
            public int OpenJoMaint { get; set; }
            public int ScheduledJoNew { get; set; }
            public int ScheduledJoMaint { get; set; }
            public int CloseJoNew { get; set; }
            public int CloseJoMaint { get; set; }

            public int TotalUnitNew { get; set; }
            public int TotalUnitMaint { get; set; }
            public int OpenUnitNew { get; set; }
            public int OpenUnitMaint { get; set; }
            public int ScheduledUnitNew { get; set; }
            public int ScheduledUnitMaint { get; set; }
            public int CloseUnitNew { get; set; }
            public int CloseUnitMaint { get; set; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            // Must run before Site.Master Page_Load, which redirects to login.aspx HTML on errors.
            if (HandleJobOrderInformationRequest()
                || HandleDayTotalJoListRequest()
                || HandleAreaOptionsRequest()
                || HandleWebMethodBridgeRequest())
            {
                return;
            }

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHASSIGNJOB"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                EnsureUserProfileLoaded();
                EnforceFixedActiveTab();
                hfIsTechnician.Value = IsCurrentUserTechnician() ? "1" : "0";
                if (HandleDetailExportRequest())
                {
                    return;
                }

                if (!IsPostBack)
                {
                    EnsureTeknisiFiltersNotZeroingClosedJo();
                    InitializeFilter();
                }

                BindAllSection();
            }
            catch
            {
                SetDefaultSummary();
                SetDefaultUnitSummary();
            }
        }

        private bool HandleJobOrderInformationRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("load_job_order", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            JobOrderInformationResponse payload = new JobOrderInformationResponse
            {
                Rows = new List<JobOrderInformationItem>(),
                TotalRecords = 0,
                PageIndex = 1,
                PageSize = 5,
                TotalPages = 1,
                ErrorMessage = string.Empty
            };

            try
            {
                ClsType clType = new ClsType();
                string accessMenu = Session["ClsTypeAccessMenu"] == null
                    ? string.Empty
                    : Convert.ToString(Session["ClsTypeAccessMenu"]);
                if (string.IsNullOrWhiteSpace(accessMenu) || !accessMenu.ToUpperInvariant().Contains("MNUDASHASSIGNJOB"))
                {
                    payload.ErrorMessage = "Akses menu tidak valid.";
                    WriteRawJsonAndEnd(SerializeJobOrderPayload(payload));
                    return true;
                }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    payload.ErrorMessage = "Session login tidak valid. Silakan login ulang.";
                    WriteRawJsonAndEnd(SerializeJobOrderPayload(payload));
                    return true;
                }

                string activeTab = Request.QueryString["activeTab"] ?? string.Empty;
                string searchKeyword = Request.QueryString["searchKeyword"] ?? string.Empty;
                int pageIndex;
                int pageSize;
                if (!int.TryParse(Request.QueryString["pageIndex"], out pageIndex))
                {
                    pageIndex = 1;
                }
                if (!int.TryParse(Request.QueryString["pageSize"], out pageSize))
                {
                    pageSize = 5;
                }

                string branchFilter = Request.QueryString["branchFilter"] ?? string.Empty;
                payload = ResolveJobOrderInformationForRequest(activeTab, searchKeyword, pageIndex, pageSize, branchFilter);
                WriteRawJsonAndEnd(SerializeJobOrderPayload(payload));
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Expected from Response.End().
                throw;
            }
            catch (Exception ex)
            {
                payload.ErrorMessage = "Terjadi kesalahan saat memuat Job Order: " + (ex.Message ?? string.Empty);
                WriteRawJsonAndEnd(SerializeJobOrderPayload(payload));
            }

            return true;
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

        private static string SerializeJobOrderPayload(JobOrderInformationResponse response)
        {
            return SerializeToJson(response ?? new JobOrderInformationResponse
            {
                Rows = new List<JobOrderInformationItem>(),
                TotalRecords = 0,
                PageIndex = 1,
                PageSize = 5,
                TotalPages = 1,
                ErrorMessage = string.Empty
            });
        }

        private bool HandleDayTotalJoListRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("load_day_total_jo", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                ClsType clType = new ClsType();
                string accessMenu = Session["ClsTypeAccessMenu"] == null
                    ? string.Empty
                    : Convert.ToString(Session["ClsTypeAccessMenu"]);
                if (string.IsNullOrWhiteSpace(accessMenu) || !accessMenu.ToUpperInvariant().Contains("MNUDASHASSIGNJOB"))
                {
                    WriteRawJsonAndEnd(SerializeToJson(ShowDayTotalJoModal(
                        "-",
                        0,
                        0,
                        BuildClosedJobEmptyStateHtml("Akses menu tidak valid."),
                        false,
                        "Akses menu tidak valid.")));
                    return true;
                }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    WriteRawJsonAndEnd(SerializeToJson(ShowDayTotalJoModal(
                        "-",
                        0,
                        0,
                        BuildClosedJobEmptyStateHtml("Session login tidak valid. Silakan login ulang."),
                        false,
                        "Session login tidak valid. Silakan login ulang.")));
                    return true;
                }

                string scheduleDate = Request.QueryString["scheduleDate"] ?? string.Empty;
                WriteRawJsonAndEnd(SerializeToJson(BuildDayTotalJoListResponse(scheduleDate)));
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                WriteRawJsonAndEnd(SerializeToJson(ShowDayTotalJoModal(
                    "-",
                    0,
                    0,
                    BuildClosedJobEmptyStateHtml("Gagal memuat daftar JO per tanggal."),
                    false,
                    "Gagal memuat daftar JO per tanggal: " + (ex.Message ?? string.Empty))));
            }

            return true;
        }

        private bool HandleAreaOptionsRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("load_area_options", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                ClsType clType = new ClsType();
                string accessMenu = Session["ClsTypeAccessMenu"] == null
                    ? string.Empty
                    : Convert.ToString(Session["ClsTypeAccessMenu"]);
                if (string.IsNullOrWhiteSpace(accessMenu) || !accessMenu.ToUpperInvariant().Contains("MNUDASHASSIGNJOB"))
                {
                    WriteRawJsonAndEnd(SerializeToJson(new AreaOptionResponse
                    {
                        Result = "ERROR",
                        Message = "Akses menu tidak valid.",
                        Rows = new List<AreaOptionItem>()
                    }));
                    return true;
                }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    WriteRawJsonAndEnd(SerializeToJson(new AreaOptionResponse
                    {
                        Result = "ERROR",
                        Message = "Session login tidak valid. Silakan login ulang.",
                        Rows = new List<AreaOptionItem>()
                    }));
                    return true;
                }

                string supAreaId = Request.QueryString["supAreaId"] ?? string.Empty;
                WriteRawJsonAndEnd(SerializeToJson(BuildAreaOptionsResponse(supAreaId)));
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                WriteRawJsonAndEnd(SerializeToJson(new AreaOptionResponse
                {
                    Result = "ERROR",
                    Message = "Gagal memuat area: " + (ex.Message ?? string.Empty),
                    Rows = new List<AreaOptionItem>()
                }));
            }

            return true;
        }

        private bool HandleWebMethodBridgeRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("wm", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string methodName = (Request.QueryString["method"] ?? string.Empty).Trim();
            Dictionary<string, object> payload = ReadJsonPayloadDictionary();

            try
            {
                ClsType clType = new ClsType();
                string accessMenu = Session["ClsTypeAccessMenu"] == null
                    ? string.Empty
                    : Convert.ToString(Session["ClsTypeAccessMenu"]);
                if (string.IsNullOrWhiteSpace(accessMenu) || !accessMenu.ToUpperInvariant().Contains("MNUDASHASSIGNJOB"))
                {
                    WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object>
                    {
                        { "Result", "ERROR" },
                        { "Message", "Akses menu tidak valid." }
                    }));
                    return true;
                }

                if (Session["ClsTypeIsLogin"] == null || !clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object>
                    {
                        { "Result", "ERROR" },
                        { "Message", "Session login tidak valid. Silakan login ulang." }
                    }));
                    return true;
                }

                object result = DispatchWebMethodBridge(methodName, payload);
                WriteRawJsonAndEnd(SerializeToJson(result));
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                WriteRawJsonAndEnd(SerializeToJson(new Dictionary<string, object>
                {
                    { "Result", "ERROR" },
                    { "Message", "Gagal memproses permintaan: " + (ex.Message ?? string.Empty) }
                }));
            }

            return true;
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

        private static object DispatchWebMethodBridge(string methodName, Dictionary<string, object> payload)
        {
            string method = (methodName ?? string.Empty).Trim();
            Dictionary<string, object> args = payload ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (method.Equals("LoadScheduleReport", StringComparison.OrdinalIgnoreCase))
            {
                if (IsJobTrainingAssignRequestContext())
                {
                    return BuildItsScheduleDayReport(
                        GetPayloadString(args, "technicianId"),
                        GetPayloadString(args, "schDate"),
                        GetPayloadString(args, "technicianName"));
                }

                return LoadScheduleReport(
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "schDate"),
                    GetPayloadString(args, "technicianName"));
            }

            if (method.Equals("SaveAssignJob", StringComparison.OrdinalIgnoreCase))
            {
                return SaveAssignJob(
                    GetPayloadString(args, "assignId"),
                    GetPayloadString(args, "jobId"),
                    GetPayloadString(args, "custId"),
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "schDate"),
                    GetPayloadInt(args, "qtyAssign", 0),
                    GetPayloadString(args, "deviceGroupId"),
                    GetPayloadString(args, "areaId"),
                    GetPayloadString(args, "targetStatus"),
                    GetPayloadString(args, "insDeviceTypeId"));
            }

            if (method.Equals("DeleteScheduleAssign", StringComparison.OrdinalIgnoreCase))
            {
                return DeleteScheduleAssign(
                    GetPayloadString(args, "assignId"),
                    GetPayloadInt(args, "seq", -1));
            }

            if (method.Equals("GetClosedJobList", StringComparison.OrdinalIgnoreCase))
            {
                return GetClosedJobList(
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "technicianName"),
                    GetPayloadString(args, "closeType"),
                    GetPayloadString(args, "periode"));
            }

            if (method.Equals("LoadTechnicianGpsDetail", StringComparison.OrdinalIgnoreCase))
            {
                return LoadTechnicianGpsDetail(
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "deviceTypeId"),
                    GetPayloadString(args, "technicianName"),
                    GetPayloadString(args, "deviceTypeDesc"));
            }

            if (method.Equals("LoadTechnicianStock", StringComparison.OrdinalIgnoreCase))
            {
                return LoadTechnicianStock(GetPayloadString(args, "technicianId"));
            }

            if (method.Equals("RefreshAvailability", StringComparison.OrdinalIgnoreCase))
            {
                return RefreshAvailability(
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "schDate"));
            }

            if (method.Equals("UpdateTechnicianStatus", StringComparison.OrdinalIgnoreCase))
            {
                return UpdateTechnicianStatus(
                    GetPayloadString(args, "technicianId"),
                    GetPayloadString(args, "schDate"),
                    GetPayloadString(args, "targetStatus"));
            }

            if (method.Equals("LoadPerfChartData", StringComparison.OrdinalIgnoreCase))
            {
                return LoadPerfChartData(
                    GetPayloadString(args, "periode"),
                    GetPayloadString(args, "technicianId"));
            }

            if (method.Equals("LoadPerfTechnicianList", StringComparison.OrdinalIgnoreCase))
            {
                return LoadPerfTechnicianList(GetPayloadString(args, "periode"));
            }

            if (method.Equals("LoadAreaOptions", StringComparison.OrdinalIgnoreCase))
            {
                return LoadAreaOptions(GetPayloadString(args, "supAreaId"));
            }

            if (method.Equals("GetDayTotalJoList", StringComparison.OrdinalIgnoreCase))
            {
                return GetDayTotalJoList(GetPayloadString(args, "scheduleDate"));
            }

            if (method.Equals("LoadTrainingLookups", StringComparison.OrdinalIgnoreCase))
            {
                return BuildTrainingLookupsResponse();
            }

            if (method.Equals("SaveJobTrainingAssign", StringComparison.OrdinalIgnoreCase))
            {
                return ExecuteSaveJobTrainingAssign(
                    GetPayloadString(args, "custId"),
                    GetPayloadString(args, "reqDate"),
                    GetPayloadString(args, "billableId"),
                    GetPayloadString(args, "schDate"),
                    GetPayloadString(args, "remark"),
                    GetPayloadString(args, "categoryId"),
                    GetPayloadString(args, "itUserId"),
                    GetPayloadString(args, "itUserName"));
            }

            if (method.Equals("LoadJobOrderInformation", StringComparison.OrdinalIgnoreCase))
            {
                return LoadJobOrderInformation(
                    GetPayloadString(args, "activeTab"),
                    GetPayloadString(args, "searchKeyword"),
                    GetPayloadInt(args, "pageIndex", 1),
                    GetPayloadInt(args, "pageSize", 5));
            }

            return new Dictionary<string, object>
            {
                { "Result", "ERROR" },
                { "Message", "Method tidak dikenali: " + method }
            };
        }

        private static string GetPayloadString(Dictionary<string, object> payload, string key)
        {
            if (payload == null || string.IsNullOrWhiteSpace(key) || !payload.ContainsKey(key) || payload[key] == null)
            {
                return string.Empty;
            }

            return Convert.ToString(payload[key], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static int GetPayloadInt(Dictionary<string, object> payload, string key, int defaultValue)
        {
            if (payload == null || string.IsNullOrWhiteSpace(key) || !payload.ContainsKey(key) || payload[key] == null)
            {
                return defaultValue;
            }

            object raw = payload[key];
            try
            {
                if (raw is int)
                {
                    return (int)raw;
                }
                if (raw is long)
                {
                    return Convert.ToInt32((long)raw);
                }
                if (raw is decimal)
                {
                    return Convert.ToInt32((decimal)raw);
                }
                if (raw is double)
                {
                    return Convert.ToInt32((double)raw);
                }

                int parsed;
                if (int.TryParse(Convert.ToString(raw, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed))
                {
                    return parsed;
                }
            }
            catch
            {
                // fall through
            }

            return defaultValue;
        }

        private static JobOrderInformationResponse BuildJobOrderInformationResponse(string activeTab, string searchKeyword, int pageIndex, int pageSize)
        {
            JobOrderInformationResponse response = new JobOrderInformationResponse
            {
                Rows = new List<JobOrderInformationItem>(),
                BranchOptions = new List<string>(),
                TotalRecords = 0,
                PageIndex = pageIndex < 1 ? 1 : pageIndex,
                PageSize = pageSize < 1 ? 5 : Math.Min(pageSize, 100),
                TotalPages = 1,
                ErrorMessage = string.Empty
            };

            try
            {
                DataTable source = IsInstallationTab(activeTab)
                    ? BindJobOrderInstallation()
                    : BindJobOrderMaintenance();

                DataTable filtered = FilterJobOrderInformation(source, searchKeyword);
                response.TotalRecords = filtered.Rows.Count;
                response.TotalPages = Math.Max(1, (int)Math.Ceiling((double)response.TotalRecords / response.PageSize));

                if (response.PageIndex > response.TotalPages)
                {
                    response.PageIndex = response.TotalPages;
                }

                int start = (response.PageIndex - 1) * response.PageSize;
                int end = Math.Min(start + response.PageSize, filtered.Rows.Count);
                for (int i = start; i < end; i++)
                {
                    DataRow row = filtered.Rows[i];
                    string customerId = GetValue(row, "CustID");
                    int totalAssignGps = ParseIntFromColumns(row, "TotalAssignGps");
                    int totalAssignAcs = ParseIntFromColumns(row, "TotalAssignAcs");
                    int totalUnitGps = ParseIntFromColumns(row, "TotalUnitGps");
                    int totalUnitAcs = ParseIntFromColumns(row, "TotalUnitAcs");
                    int totalUnitGpsDone = ParseIntFromColumns(row, "TotalUnitGpsDone");
                    int totalUnitAcsDone = ParseIntFromColumns(row, "TotalUnitAcsDone");
                    int remainingUnitGps = ParseIntFromColumns(row, "RemainingUnitGps");
                    int remainingUnitAcs = ParseIntFromColumns(row, "RemainingUnitAcs");
                    int totalAssignLegacy = ParseIntFromColumns(row, "TotalAssign");
                    int totalUnitLegacy = ParseIntFromColumns(row, "TotalUnit");
                    int remainingUnitLegacy = ParseIntFromColumns(row, "RemainingUnit");

                    response.Rows.Add(new JobOrderInformationItem
                    {
                        JobID = GetValue(row, "JobID"),
                        Customer = customerId,
                        CustomerName = ResolveCustomerName(row, customerId),
                        BranchName = GetValue(row, "BranchName"),
                        DeviceTypeID = GetValue(row, "DeviceTypeID"),
                        DeviceTypeDesc = GetValue(row, "DeviceTypeDesc"),
                        TotalAssign = totalAssignLegacy > 0 ? totalAssignLegacy : (totalAssignGps + totalAssignAcs),
                        TotalAssignGps = totalAssignGps,
                        TotalAssignAcs = totalAssignAcs,
                        LastAssignDate = FormatDateForDisplay(GetValue(row, "LastAssignDate")),
                        RemainingUnit = remainingUnitLegacy > 0 ? remainingUnitLegacy : (remainingUnitGps + remainingUnitAcs),
                        TotalUnit = totalUnitLegacy > 0 ? totalUnitLegacy : (totalUnitGps + totalUnitAcs),
                        RemainingUnitGps = remainingUnitGps,
                        RemainingUnitAcs = remainingUnitAcs,
                        TotalUnitGps = totalUnitGps,
                        TotalUnitAcs = totalUnitAcs,
                        TotalUnitGpsDone = totalUnitGpsDone,
                        TotalUnitAcsDone = totalUnitAcsDone
                    });
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Terjadi kesalahan saat memuat Job Order: " + (ex.Message ?? string.Empty);
            }

            return response;
        }

        private void EnforceFixedActiveTab()
        {
            string previousTab = NormalizeTab(Convert.ToString(Session[SessionActiveTab]));
            string activeTab = NormalizeTab(FixedActiveTab);
            hfActiveTab.Value = activeTab;
            Session[SessionActiveTab] = activeTab;

            // Switching ITS -> Teknisi: clear shared filter pollution that can zero/skew
            // Total Closed JO New and installation summary on dashboard_assign_job.aspx.
            if (!UseJobTrainingDataSource
                && previousTab == "itsupport"
                && !IsCurrentUserRegionalScoped())
            {
                Session[SessionRegionalGroupTab] = RegionalAllValue;
            }
        }

        private void EnsureTeknisiFiltersNotZeroingClosedJo()
        {
            if (UseJobTrainingDataSource || IsCurrentUserRegionalScoped())
            {
                return;
            }

            // Area / area-group bleed from ITS (WEST/EAST or a regional tab) can make
            // sp_dashboard_assign_job_summary / availability return Total Closed JO New = 0.
            // On fresh Teknisi load, start from SEMUA for non-regional-scoped users.
            Session[SessionRegionalGroupTab] = RegionalAllValue;
            Session[SessionRegionalTab] = RegionalAllValue;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            Session[SessionPeriode] = NormalizePeriode(txtPeriode.Text);
            Session[SessionActiveTab] = NormalizeTab(hfActiveTab.Value);
            BindAllSection();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string defaultPeriode = DateTime.Now.ToString("yyyy-MM");
            bool isRegionalScoped = IsCurrentUserRegionalScoped();
            Session[SessionPeriode] = defaultPeriode;
            Session[SessionActiveTab] = NormalizeTab(FixedActiveTab);
            Session[RegionalGroupTabSessionKey] = isRegionalScoped
                ? GetCurrentUserAreaGroupId()
                : RegionalAllValue;
            Session[RegionalTabSessionKey] = UseJobTrainingDataSource
                ? RegionalAllValue
                : (isRegionalScoped
                    ? GetCurrentUserSupAreaId()
                    : RegionalAllValue);
            txtPeriode.Text = FormatPeriodeDisplay(defaultPeriode);
            hfActiveTab.Value = NormalizeTab(FixedActiveTab);
            BindAllSection();
        }

        protected void btnTabRefresh_Click(object sender, EventArgs e)
        {
            Session[SessionActiveTab] = NormalizeTab(hfActiveTab.Value);
            Session[SessionPeriode] = NormalizePeriode(txtPeriode.Text);
            BindAllSection();
        }

        protected void btnOpenDetail_Click(object sender, EventArgs e)
        {
            string status = NormalizeStatus(hfDetailStatus.Value);
            string detailJobType = NormalizeDetailJobType(hfDetailJobType.Value);
            string detailMetric = NormalizeDetailMetric(hfDetailMetric.Value);
            Session[SessionPeriode] = NormalizePeriode(txtPeriode.Text);
            Session[SessionActiveTab] = NormalizeTab(hfActiveTab.Value);
            BindDetailModal(status, detailJobType, detailMetric);
            ScriptManager.RegisterStartupScript(this, GetType(), "show-detail-jo", "$('#modal-detail-jo').modal('show');", true);
        }

        private void InitializeFilter()
        {
            string periode = DateTime.Now.ToString("yyyy-MM");
            string activeTab = NormalizeTab(Convert.ToString(Session[SessionActiveTab]));
            bool isRegionalScoped = IsCurrentUserRegionalScoped();
            string userScopedGroup = GetCurrentUserAreaGroupId();
            string userScopedSupArea = GetCurrentUserSupAreaId();

            txtPeriode.Text = FormatPeriodeDisplay(periode);
            hfActiveTab.Value = activeTab;
            hfIsTechnician.Value = IsCurrentUserTechnician() ? "1" : "0";
            Session[SessionPeriode] = periode;
            Session[SessionActiveTab] = activeTab;
            if (Session[RegionalGroupTabSessionKey] == null)
            {
                Session[RegionalGroupTabSessionKey] = isRegionalScoped && !string.IsNullOrWhiteSpace(userScopedGroup)
                    ? userScopedGroup
                    : RegionalAllValue;
            }
            if (UseJobTrainingDataSource)
            {
                // IT Support filters only by SEMUA / WEST / EAST.
                Session[RegionalTabSessionKey] = RegionalAllValue;
            }
            else if (Session[RegionalTabSessionKey] == null)
            {
                Session[RegionalTabSessionKey] = isRegionalScoped && !string.IsNullOrWhiteSpace(userScopedSupArea)
                    ? userScopedSupArea
                    : RegionalAllValue;
            }
        }

        private void EnsureUserProfileLoaded()
        {
            if (Session[SessionUserSupAreaID] != null
                && Session[SessionUserAreaGroupID] != null
                && Session[SessionUserIsTechnician] != null
                && Session[SessionUserIsRegion] != null)
            {
                return;
            }

            Session[SessionUserSupAreaID] = string.Empty;
            Session[SessionUserAreaGroupID] = string.Empty;
            Session[SessionUserIsTechnician] = "0";
            Session[SessionUserIsRegion] = "0";

            string userId = Convert.ToString(Session["ClsTypeUserID"]).Trim();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            try
            {
                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_user_profile '" + userId.Replace("'", "''") + "'",
                    DBConnstringSQL());
                DataTable dt = rec.DataRecord();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string supAreaId = GetString(row, "SupAreaID");
                    string areaGroupId = GetString(row, "AreaGroupID");
                    string isTechnicianRaw = GetString(row, "IsTechnician");
                    string isRegionRaw = GetString(row, "IsRegion");

                    Session[SessionUserSupAreaID] = supAreaId;
                    Session[SessionUserAreaGroupID] = areaGroupId;
                    Session[SessionUserIsTechnician] = ParseBooleanFlag(isTechnicianRaw) ? "1" : "0";
                    Session[SessionUserIsRegion] = ParseBooleanFlag(isRegionRaw) ? "1" : "0";
                }
            }
            catch
            {
            }
        }

        private string GetCurrentUserSupAreaId()
        {
            return Convert.ToString(Session[SessionUserSupAreaID]).Trim();
        }

        private bool IsCurrentUserTechnician()
        {
            return ParseBooleanFlag(Convert.ToString(Session[SessionUserIsTechnician]));
        }

        private string GetCurrentUserAreaGroupId()
        {
            return NormalizeAreaGroupTabValue(Convert.ToString(Session[SessionUserAreaGroupID]).Trim());
        }

        private bool IsCurrentUserRegionalScoped()
        {
            return ParseBooleanFlag(Convert.ToString(Session[SessionUserIsRegion]));
        }

        private bool ParseBooleanFlag(string value)
        {
            string normalized = (value ?? string.Empty).Trim();
            int numericFlag;
            if (int.TryParse(normalized, out numericFlag))
            {
                return numericFlag > 0;
            }

            return normalized.Equals("true", StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("y", StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private void BindAllSection()
        {
            string periode = NormalizePeriode(txtPeriode.Text);
            string activeTab = NormalizeTab(FixedActiveTab);

            txtPeriode.Text = FormatPeriodeDisplay(periode);
            hfActiveTab.Value = activeTab;
            Session[SessionPeriode] = periode;
            Session[SessionActiveTab] = activeTab;

            lblActiveTabTitle.InnerText = activeTab == "itsupport" ? "IT Support" : "Teknisi";
            lblScheduleTabTitle.InnerText = lblActiveTabTitle.InnerText;

            BindRegionalTabs();
            string selectedRegional = GetSelectedRegionalTab();
            string selectedAreaGroup = UseJobTrainingDataSource
                ? ResolveItsScheduleAreaGroupFilter(GetSelectedRegionalGroupTab())
                : NormalizeAreaGroupId(GetSelectedRegionalGroupTab());
            string supAreaForQuery = ResolveSupAreaParameter(selectedRegional);
            bool summaryBound = BindSummary(periode, supAreaForQuery, selectedAreaGroup);
            bool unitSummaryBound = BindUnitSummary(periode, supAreaForQuery, selectedAreaGroup);

            // Fallback list query is intentionally lazy to reduce initial page load latency.
            if (!summaryBound || !unitSummaryBound)
            {
                DataTable dtSource = GetAssignJobList(periode, "all", supAreaForQuery, selectedAreaGroup);
                DataTable dtByRole = FilterByTab(dtSource, activeTab);
                DataTable dtFilteredRegional = FilterByRegional(dtByRole, selectedRegional);
                SummarySplitMetrics summary = CalculateSummarySplitMetrics(dtFilteredRegional);
                if (!summaryBound)
                {
                    ApplySummarySplitMetrics(summary);
                }
                if (!unitSummaryBound)
                {
                    ApplyUnitSummarySplitMetrics(summary);
                }
            }

            BindPerformance(activeTab);
            BindAvailabilitySchedule(periode, selectedRegional, selectedAreaGroup);
        }

        private bool BindSummary(string periode, string supAreaId, string areaGroupId)
        {
            if (UseJobTrainingDataSource)
            {
                return BindJobTrainingSummary(periode, supAreaId, areaGroupId);
            }

            try
            {
                Recordset rec = new Recordset();
                string safePeriode = (periode ?? string.Empty).Replace("'", "''");
                string safeSupAreaId = (supAreaId ?? string.Empty).Replace("'", "''");
                if (safeSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                {
                    safeSupAreaId = string.Empty;
                }
                string safeAreaGroupId = NormalizeAreaGroupId(areaGroupId).Replace("'", "''");
                rec.Open("sp_dashboard_assign_job_summary '" + safePeriode + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'", DBConnstringSQL());
                if (rec.RecordCount() > 0)
                {
                    int totalNew = ParseInt(rec.Fields("TotalJONew"));
                    int openNew = ParseInt(rec.Fields("TotalJONewOpen"));
                    int closeNew = ParseSummaryCloseCount(rec, true);
                    int scheduledNew = ParseInt(rec.Fields("TotalJOSchNew"));

                    int totalMaint = ParseInt(rec.Fields("TotalJOMaint"));
                    int openMaint = ParseInt(rec.Fields("TotalJOMaintOpen"));
                    int closeMaint = ParseSummaryCloseCount(rec, false);
                    int scheduledMaint = ParseInt(rec.Fields("TotalJOSchMaint"));

                    lblTotalJONew.InnerText = FormatCount(totalNew.ToString());
                    lblTotalJOMaint.InnerText = FormatCount(totalMaint.ToString());
                    lblTotalJOOpenNew.InnerText = FormatCount(openNew.ToString());
                    lblTotalJOOpenMaint.InnerText = FormatCount(openMaint.ToString());
                    lblTotalJOScheduledNew.InnerText = FormatCount(scheduledNew.ToString());
                    lblTotalJOScheduledMaint.InnerText = FormatCount(scheduledMaint.ToString());
                    lblTotalJOCloseNew.InnerText = FormatCount(closeNew.ToString());
                    lblTotalJOCloseMaint.InnerText = FormatCount(closeMaint.ToString());
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private bool BindJobTrainingSummary(string periode, string supAreaId, string areaGroupId)
        {
            try
            {
                DataTable source = LoadJobTrainingRowsForSchedule(periode);
                DataTable mapped = MapJobTrainingRowsToAssignList(
                    source,
                    periode,
                    "all",
                    ResolveSupAreaParameter(supAreaId),
                    NormalizeAreaGroupId(areaGroupId));
                SummarySplitMetrics summary = CalculateSummarySplitMetrics(mapped);
                ApplySummarySplitMetrics(summary);
                return mapped.Rows.Count > 0;
            }
            catch
            {
            }

            return false;
        }

        private int ParseSummaryCloseCount(Recordset rec, bool isNewInstall)
        {
            if (rec == null)
            {
                return 0;
            }

            string[] candidates = isNewInstall
                ? new[] { "TotalJONewClose", "TotalJobCloseNew", "TotalClosedJONew", "TotalJOCloseNew", "TotalJobClose" }
                : new[] { "TotalJOMaintClose", "TotalJobCloseMaint", "TotalClosedJOMaint", "TotalJOCloseMaint" };

            foreach (string fieldName in candidates)
            {
                try
                {
                    string raw = Convert.ToString(rec.Fields(fieldName));
                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        return ParseInt(raw);
                    }
                }
                catch
                {
                }
            }

            return 0;
        }

        private bool BindUnitSummary(string periode, string supAreaId, string areaGroupId)
        {
            if (UseJobTrainingDataSource)
            {
                try
                {
                    DataTable source = LoadJobTrainingRowsForSchedule(periode);
                    DataTable mapped = MapJobTrainingRowsToAssignList(
                        source,
                        periode,
                        "all",
                        ResolveSupAreaParameter(supAreaId),
                        NormalizeAreaGroupId(areaGroupId));
                    SummarySplitMetrics summary = CalculateSummarySplitMetrics(mapped);
                    ApplyUnitSummarySplitMetrics(summary);
                    return mapped.Rows.Count > 0;
                }
                catch
                {
                }

                return false;
            }

            try
            {
                Recordset rec = new Recordset();
                string safePeriode = (periode ?? string.Empty).Replace("'", "''");
                string safeSupAreaId = (supAreaId ?? string.Empty).Replace("'", "''");
                string safeAreaGroupId = NormalizeAreaGroupId(areaGroupId).Replace("'", "''");
                rec.Open("sp_dashboard_assign_job_summary_unit '" + safePeriode + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'", DBConnstringSQL());
                if (rec.RecordCount() > 0)
                {
                    int totalUnitNew = ParseInt(rec.Fields("TotalUnitJONew"));
                    int openUnitNew = ParseInt(rec.Fields("TotalUnitJONewOpen"));
                    int closeUnitNew = ParseInt(rec.Fields("TotalUnitJONewClose"));
                    int scheduledUnitNew = ParseInt(rec.Fields("TotalUnitJOSchNew"));

                    int totalUnitMaint = ParseInt(rec.Fields("TotalUnitJOMaint"));
                    int openUnitMaint = ParseInt(rec.Fields("TotalUnitJOMaintOpen"));
                    int closeUnitMaint = ParseInt(rec.Fields("TotalUnitJOMaintClose"));
                    int scheduledUnitMaint = ParseInt(rec.Fields("TotalUnitJOSchMaint"));

                    lblTotalUnitNew.InnerText = FormatCount(totalUnitNew.ToString());
                    lblTotalUnitMaint.InnerText = FormatCount(totalUnitMaint.ToString());
                    lblTotalUnitOpenNew.InnerText = FormatCount(openUnitNew.ToString());
                    lblTotalUnitOpenMaint.InnerText = FormatCount(openUnitMaint.ToString());
                    lblTotalUnitScheduledNew.InnerText = FormatCount(scheduledUnitNew.ToString());
                    lblTotalUnitScheduledMaint.InnerText = FormatCount(scheduledUnitMaint.ToString());
                    lblTotalUnitCloseNew.InnerText = FormatCount(closeUnitNew.ToString());
                    lblTotalUnitCloseMaint.InnerText = FormatCount(closeUnitMaint.ToString());
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private void ApplySummarySplitMetrics(SummarySplitMetrics summary)
        {
            summary = summary ?? new SummarySplitMetrics();
            lblTotalJONew.InnerText = FormatCount(summary.TotalJoNew.ToString());
            lblTotalJOMaint.InnerText = FormatCount(summary.TotalJoMaint.ToString());
            lblTotalJOOpenNew.InnerText = FormatCount(summary.OpenJoNew.ToString());
            lblTotalJOOpenMaint.InnerText = FormatCount(summary.OpenJoMaint.ToString());
            lblTotalJOScheduledNew.InnerText = FormatCount(summary.ScheduledJoNew.ToString());
            lblTotalJOScheduledMaint.InnerText = FormatCount(summary.ScheduledJoMaint.ToString());
            lblTotalJOCloseNew.InnerText = FormatCount(summary.CloseJoNew.ToString());
            lblTotalJOCloseMaint.InnerText = FormatCount(summary.CloseJoMaint.ToString());
        }

        private void ApplyUnitSummarySplitMetrics(SummarySplitMetrics summary)
        {
            summary = summary ?? new SummarySplitMetrics();
            lblTotalUnitNew.InnerText = FormatCount(summary.TotalUnitNew.ToString());
            lblTotalUnitMaint.InnerText = FormatCount(summary.TotalUnitMaint.ToString());
            lblTotalUnitOpenNew.InnerText = FormatCount(summary.OpenUnitNew.ToString());
            lblTotalUnitOpenMaint.InnerText = FormatCount(summary.OpenUnitMaint.ToString());
            lblTotalUnitScheduledNew.InnerText = FormatCount(summary.ScheduledUnitNew.ToString());
            lblTotalUnitScheduledMaint.InnerText = FormatCount(summary.ScheduledUnitMaint.ToString());
            lblTotalUnitCloseNew.InnerText = FormatCount(summary.CloseUnitNew.ToString());
            lblTotalUnitCloseMaint.InnerText = FormatCount(summary.CloseUnitMaint.ToString());
        }

        private SummarySplitMetrics CalculateSummarySplitMetrics(DataTable source)
        {
            SummarySplitMetrics summary = new SummarySplitMetrics();
            if (source == null || source.Rows.Count == 0)
            {
                return summary;
            }

            HashSet<string> totalNewJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> totalMaintJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> openNewJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> openMaintJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> scheduledNewJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> scheduledMaintJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> closeNewJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> closeMaintJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < source.Rows.Count; i++)
            {
                DataRow row = source.Rows[i];
                bool isMaintenance = IsMaintenanceCategory(GetJobCategory(row));
                string normalizedStatus = NormalizeSummaryStatus(GetString(row, "Status"));
                string jobKey = ResolveSummaryJobKey(row, i);

                if (isMaintenance)
                {
                    summary.TotalUnitMaint++;
                    totalMaintJobs.Add(jobKey);
                }
                else
                {
                    summary.TotalUnitNew++;
                    totalNewJobs.Add(jobKey);
                }

                if (normalizedStatus == "open")
                {
                    if (isMaintenance)
                    {
                        summary.OpenUnitMaint++;
                        openMaintJobs.Add(jobKey);
                    }
                    else
                    {
                        summary.OpenUnitNew++;
                        openNewJobs.Add(jobKey);
                    }
                }
                else if (normalizedStatus == "close")
                {
                    if (isMaintenance)
                    {
                        summary.CloseUnitMaint++;
                        closeMaintJobs.Add(jobKey);
                    }
                    else
                    {
                        summary.CloseUnitNew++;
                        closeNewJobs.Add(jobKey);
                    }
                }
                else
                {
                    if (isMaintenance)
                    {
                        summary.ScheduledUnitMaint++;
                        scheduledMaintJobs.Add(jobKey);
                    }
                    else
                    {
                        summary.ScheduledUnitNew++;
                        scheduledNewJobs.Add(jobKey);
                    }
                }
            }

            summary.TotalJoNew = totalNewJobs.Count;
            summary.TotalJoMaint = totalMaintJobs.Count;
            summary.OpenJoNew = openNewJobs.Count;
            summary.OpenJoMaint = openMaintJobs.Count;
            summary.ScheduledJoNew = scheduledNewJobs.Count;
            summary.ScheduledJoMaint = scheduledMaintJobs.Count;
            summary.CloseJoNew = closeNewJobs.Count;
            summary.CloseJoMaint = closeMaintJobs.Count;
            return summary;
        }

        private string GetJobCategory(DataRow row)
        {
            if (row == null)
            {
                return string.Empty;
            }

            return FirstNonEmpty(
                GetString(row, "JobCategory"),
                GetString(row, "Job_Category"),
                GetString(row, "Category"),
                GetString(row, "JobCategoryName"));
        }

        private bool IsMaintenanceCategory(string jobCategory)
        {
            string normalized = (jobCategory ?? string.Empty).Trim().ToUpperInvariant();
            if (UseJobTrainingDataSource)
            {
                return normalized.Contains("VISIT");
            }

            return normalized.Contains("MAINT");
        }

        private string NormalizeSummaryStatus(string status)
        {
            if (UseJobTrainingDataSource)
            {
                return NormalizeJobTrainingStatusCode(status);
            }

            string normalized = (status ?? string.Empty).Replace("&nbsp;", " ").Trim().ToLowerInvariant();
            if (normalized == "open" || normalized == "op" || normalized == "rg")
            {
                return "open";
            }

            if (normalized == "close" || normalized == "cl" || normalized == "closed")
            {
                return "close";
            }

            if (normalized == "scheduled" || normalized == "sc" || normalized == "sch")
            {
                return "scheduled";
            }

            return "scheduled";
        }

        private string ResolveSummaryJobKey(DataRow row, int index)
        {
            string jobId = GetString(row, "JobID");
            if (!string.IsNullOrWhiteSpace(jobId))
            {
                return jobId;
            }

            return "__ROW__" + index.ToString();
        }

        private void BindRegionalTabs()
        {
            bool isRegionalScoped = IsCurrentUserRegionalScoped();
            string userScopedGroup = GetCurrentUserAreaGroupId();
            string userScopedSupArea = GetCurrentUserSupAreaId();
            bool itsGroupOnlyFilters = UseJobTrainingDataSource;

            DataTable groupTabs = BuildAreaGroupTabs();
            string selectedGroup = isRegionalScoped && !string.IsNullOrWhiteSpace(userScopedGroup)
                ? userScopedGroup
                : NormalizeAreaGroupTabValue(GetSelectedRegionalGroupTab());
            if (!groupTabs.AsEnumerable().Any(r => string.Equals(GetString(r, "AreaGroupID"), selectedGroup, StringComparison.OrdinalIgnoreCase)))
            {
                selectedGroup = RegionalAllValue;
            }
            Session[RegionalGroupTabSessionKey] = selectedGroup;

            // IT Support page: only SEMUA / WEST / EAST area-group filters; never scope by regional/suparea.
            if (itsGroupOnlyFilters)
            {
                Session[RegionalTabSessionKey] = RegionalAllValue;
                DataTable emptyRegionalTabs = new DataTable();
                emptyRegionalTabs.Columns.Add("SupAreaID");
                emptyRegionalTabs.Columns.Add("SupAreaName");
                ViewState[ViewStateRegionalTabs] = emptyRegionalTabs;
                if (rptAreaGroupTabs != null)
                {
                    rptAreaGroupTabs.DataSource = groupTabs;
                    rptAreaGroupTabs.DataBind();
                }
                if (rptRegionalTabs != null)
                {
                    rptRegionalTabs.DataSource = new DataTable();
                    rptRegionalTabs.DataBind();
                    rptRegionalTabs.Visible = false;
                }
                return;
            }

            DataTable sourceTabs = GetRegionalSourceTabs(selectedGroup);
            if (isRegionalScoped && !string.IsNullOrWhiteSpace(userScopedSupArea))
            {
                DataTable scopedTabs = sourceTabs.Clone();
                foreach (DataRow row in sourceTabs.Rows)
                {
                    if (GetString(row, "SupAreaID").Equals(userScopedSupArea, StringComparison.OrdinalIgnoreCase))
                    {
                        scopedTabs.ImportRow(row);
                    }
                }
                sourceTabs = scopedTabs;
            }

            DataTable dtTabs = BuildRegionalTabsByGroup(sourceTabs, selectedGroup, !isRegionalScoped);
            string selected = isRegionalScoped && !string.IsNullOrWhiteSpace(userScopedSupArea)
                ? userScopedSupArea
                : GetSelectedRegionalTab();
            bool hasSelectedTab = dtTabs.AsEnumerable().Any(r => string.Equals(GetString(r, "SupAreaID"), selected, StringComparison.OrdinalIgnoreCase));
            if (!hasSelectedTab)
            {
                selected = dtTabs.Rows.Count > 0
                    ? GetString(dtTabs.Rows[0], "SupAreaID")
                    : RegionalAllValue;
                Session[RegionalTabSessionKey] = selected;
            }
            else if (isRegionalScoped)
            {
                Session[RegionalTabSessionKey] = selected;
            }

            ViewState[ViewStateRegionalTabs] = dtTabs;
            if (rptAreaGroupTabs != null)
            {
                rptAreaGroupTabs.DataSource = groupTabs;
                rptAreaGroupTabs.DataBind();
            }
            if (rptRegionalTabs != null)
            {
                rptRegionalTabs.Visible = true;
                rptRegionalTabs.DataSource = dtTabs;
                rptRegionalTabs.DataBind();
            }
        }

        private DataTable GetRegionalSourceTabs(string selectedGroup)
        {
            DataTable sourceTabs = new DataTable();
            sourceTabs.Columns.Add("SupAreaID");
            sourceTabs.Columns.Add("SupAreaName");
            sourceTabs.Columns.Add("AreaGroupID");
            sourceTabs.Columns.Add("AreaGroupName");

            try
            {
                Recordset rec = new Recordset();
                string safeDefaultSupAreaId = GetCurrentUserSupAreaId().Replace("'", "''");
                string safeAreaGroupId = NormalizeAreaGroupId(selectedGroup).Replace("'", "''");

                rec.Open(
                    "sp_dashboard_assign_job_get_list_regional '" + safeDefaultSupAreaId + "','" + safeAreaGroupId + "'",
                    DBConnstringSQL());
                DataTable dt = rec.DataRecord();

                if (dt != null && dt.Rows.Count > 0)
                {
                    HashSet<string> dedupe = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (DataRow row in dt.Rows)
                    {
                        string supAreaId = GetString(row, "SupAreaID");
                        if (string.IsNullOrWhiteSpace(supAreaId) || !dedupe.Add(supAreaId))
                        {
                            continue;
                        }

                        DataRow dr = sourceTabs.NewRow();
                        dr["SupAreaID"] = supAreaId;
                        dr["SupAreaName"] = GetString(row, "SupAreaName");
                        dr["AreaGroupID"] = GetString(row, "AreaGroupID");
                        dr["AreaGroupName"] = GetString(row, "AreaGroupName");
                        sourceTabs.Rows.Add(dr);
                    }
                }
            }
            catch
            {
            }

            return sourceTabs;
        }

        private DataTable BuildAreaGroupTabs()
        {
            DataTable groupTabs = new DataTable();
            groupTabs.Columns.Add("AreaGroupID");
            groupTabs.Columns.Add("AreaGroupName");

            bool isRegionalScoped = IsCurrentUserRegionalScoped();
            string userScopedGroup = GetCurrentUserAreaGroupId();

            if (!isRegionalScoped)
            {
                DataRow allRow = groupTabs.NewRow();
                allRow["AreaGroupID"] = RegionalAllValue;
                allRow["AreaGroupName"] = "SEMUA";
                groupTabs.Rows.Add(allRow);
            }

            DataRow westRow = groupTabs.NewRow();
            westRow["AreaGroupID"] = AreaGroupWestValue;
            westRow["AreaGroupName"] = "WEST AREA";
            if (!isRegionalScoped || userScopedGroup.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase))
            {
                groupTabs.Rows.Add(westRow);
            }

            DataRow eastRow = groupTabs.NewRow();
            eastRow["AreaGroupID"] = AreaGroupEastValue;
            eastRow["AreaGroupName"] = "EAST AREA";
            if (!isRegionalScoped || userScopedGroup.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase))
            {
                groupTabs.Rows.Add(eastRow);
            }

            return groupTabs;
        }

        private DataTable BuildRegionalTabsByGroup(DataTable sourceTabs, string selectedGroup, bool includeAllOption)
        {
            DataTable dtTabs = new DataTable();
            dtTabs.Columns.Add("SupAreaID");
            dtTabs.Columns.Add("SupAreaName");

            bool isAllGroup = string.IsNullOrWhiteSpace(selectedGroup)
                || selectedGroup.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase);

            if (includeAllOption)
            {
                DataRow allRow = dtTabs.NewRow();
                allRow["SupAreaID"] = RegionalAllValue;
                allRow["SupAreaName"] = "SEMUA";
                dtTabs.Rows.Add(allRow);
            }

            if (sourceTabs == null || sourceTabs.Rows.Count == 0)
            {
                return dtTabs;
            }

            foreach (DataRow row in sourceTabs.Rows)
            {
                if (!isAllGroup)
                {
                    string rowAreaGroupId = GetString(row, "AreaGroupID");
                    if (!rowAreaGroupId.Equals(selectedGroup, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }

                DataRow dr = dtTabs.NewRow();
                dr["SupAreaID"] = GetString(row, "SupAreaID");
                dr["SupAreaName"] = GetString(row, "SupAreaName");
                dtTabs.Rows.Add(dr);
            }

            return dtTabs;
        }

        private DataTable GetAssignJobList(string periode, string status, string supAreaId, string areaGroupId)
        {
            if (UseJobTrainingDataSource)
            {
                return GetJobTrainingAssignList(periode, status, ResolveSupAreaParameter(supAreaId), NormalizeAreaGroupId(areaGroupId));
            }

            try
            {
                string safePeriode = (periode ?? string.Empty).Replace("'", "''");
                string safeSupAreaId = (supAreaId ?? string.Empty).Replace("'", "''");
                string safeAreaGroupId = NormalizeAreaGroupId(areaGroupId).Replace("'", "''");
                Recordset rec = new Recordset();
                rec.Open("sp_dashboard_assign_job_list '" + safePeriode + "','" + NormalizeStatus(status) + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'", DBConnstringSQL());
                DataTable dt = rec.DataRecord();
                return dt ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }

        private DataTable GetJobTrainingAssignList(string periode, string status, string supAreaId, string areaGroupId)
        {
            try
            {
                DataTable raw = LoadJobTrainingRowsForSchedule(periode);
                return MapJobTrainingRowsToAssignList(
                    raw,
                    periode,
                    status,
                    ResolveSupAreaParameter(supAreaId),
                    NormalizeAreaGroupId(areaGroupId));
            }
            catch
            {
                return new DataTable();
            }
        }

        protected static DataTable LoadJobTrainingHeaderTable(string searchKeyword)
        {
            return LoadJobTrainingHeaderTableFromStoredProcedure(
                "sp_list_header_job_training",
                searchKeyword);
        }

        protected static DataTable LoadJobTrainingHeaderTableForItSupport(string searchKeyword)
        {
            return LoadJobTrainingHeaderTableFromStoredProcedure(
                "sp_list_header_job_training_itsupport",
                searchKeyword);
        }

        private static DataTable LoadJobTrainingHeaderTableFromStoredProcedure(
            string storedProcedureName,
            string searchKeyword)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null || context.Session["ClsTypeDBConnStringSQL"] == null)
            {
                return new DataTable();
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                return new DataTable();
            }

            string safeKeyword = (searchKeyword ?? string.Empty).Replace("'", "''");
            string spName = (storedProcedureName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(spName))
            {
                return new DataTable();
            }

            string openError = string.Empty;
            Recordset rec = new Recordset();
            rec.Open(spName + " '" + safeKeyword + "'", connString.Trim(), ref openError);
            if (!string.IsNullOrWhiteSpace(openError))
            {
                throw new InvalidOperationException(spName + ": " + openError);
            }

            return rec.DataRecord() ?? new DataTable();
        }

        private DataTable MapTrxJobAssignDetailRowsToAssignList(
            DataTable source,
            string periode,
            string status,
            string filterSupAreaId,
            string filterAreaGroupId)
        {
            DataTable mapped = CreateJobTrainingAssignListSchema();
            if (source == null || source.Rows.Count == 0)
            {
                return mapped;
            }

            string normalizedPeriode = NormalizePeriode(periode);
            string normalizedStatus = NormalizeStatus(status);
            Dictionary<string, JobTrainingTrainerSchedule> itsUsers = BuildItsUserScheduleMap(31);
            HashSet<string> validItIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (JobTrainingTrainerSchedule user in itsUsers.Values)
            {
                if (IsValidItId(user.ItId))
                {
                    validItIds.Add(NormalizeItId(user.ItId));
                }
            }

            foreach (DataRow row in source.Rows)
            {
                string technicianId = FirstNonEmpty(
                    GetString(row, "TechnicianID"),
                    GetString(row, "TechnicianId"),
                    GetString(row, "ITID"));
                if (!IsValidItId(technicianId) || (validItIds.Count > 0 && !validItIds.Contains(NormalizeItId(technicianId))))
                {
                    continue;
                }

                string schDate = FirstNonEmpty(GetString(row, "SchDate"), GetString(row, "ScheduleDate"), GetString(row, "sSchDate"));
                if (!IsJobTrainingSchDateInPeriode(normalizedPeriode, schDate))
                {
                    continue;
                }

                string rawStatus = FirstNonEmpty(GetString(row, "Status"), GetString(row, "StatusCode"), GetString(row, "ValueStatus"));
                string mappedStatus = NormalizeSummaryStatus(rawStatus);
                if (normalizedStatus != "all" && !string.Equals(mappedStatus, normalizedStatus, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                bool isVisit = IsAssignDetailVisitOrMaint(row);
                string areaId = FirstNonEmpty(GetString(row, "AreaID"), GetString(row, "SupAreaID"));
                if (!string.IsNullOrWhiteSpace(filterSupAreaId)
                    && !filterSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(areaId)
                    && !areaId.Equals(filterSupAreaId, StringComparison.OrdinalIgnoreCase)
                    && (row.Table.Columns.Contains("AreaID") || row.Table.Columns.Contains("SupAreaID")))
                {
                    continue;
                }

                int qtyGps = ParseIntValue(FirstNonEmpty(GetString(row, "QtyGPS"), "1"));
                int qtyAcs = ParseIntValue(FirstNonEmpty(GetString(row, "QtyACS"), "0"));
                if (qtyGps <= 0 && qtyAcs <= 0)
                {
                    qtyGps = 1;
                }

                DataRow target = mapped.NewRow();
                target["JobID"] = FirstNonEmpty(GetString(row, "JobID"), "-");
                target["CustID"] = GetString(row, "CustID");
                target["CustomerName"] = FirstNonEmpty(GetString(row, "CustomerName"), GetString(row, "Customer"), GetString(row, "CustID"));
                target["BranchName"] = FirstNonEmpty(GetString(row, "BranchName"), GetString(row, "AreaName"), "-");
                target["Status"] = mappedStatus;
                target["JobCategory"] = isVisit ? "Visit" : "Training";
                target["JobType"] = isVisit ? "Visit" : "Training";
                target["ScheduleDate"] = schDate;
                target["DtmUpd"] = schDate;
                target["AreaName"] = FirstNonEmpty(GetString(row, "AreaName"), GetString(row, "BranchName"), "-");
                target["AreaID"] = areaId;
                target["SupAreaID"] = areaId;
                target["AreaGroupID"] = GetString(row, "AreaGroupID");
                target["QtyGPS"] = qtyGps;
                target["QtyACS"] = qtyAcs;
                target["TotalGPS"] = qtyGps;
                target["TotalACS"] = qtyAcs;
                target["OverSLA"] = 0;
                mapped.Rows.Add(target);
            }

            return mapped;
        }

        private DataTable MapJobTrainingRowsToAssignList(DataTable source, string periode, string status, string filterSupAreaId, string filterAreaGroupId)
        {
            DataTable mapped = CreateJobTrainingAssignListSchema();
            if (source == null || source.Rows.Count == 0)
            {
                return mapped;
            }

            string normalizedPeriode = NormalizePeriode(periode);
            string normalizedStatus = NormalizeStatus(status);

            foreach (DataRow row in source.Rows)
            {
                string categoryName = FirstNonEmpty(
                    GetString(row, "TrainingCategoryName"),
                    GetString(row, "TrainCategoryName"),
                    GetString(row, "CategoryName"),
                    GetString(row, "Category"));
                string categoryId = FirstNonEmpty(
                    GetString(row, "TrainCategoryID"),
                    GetString(row, "TrainingCategoryID"),
                    GetString(row, "CategoryID"));

                if (!IsJobTrainingScheduleCategory(row, categoryName, categoryId))
                {
                    continue;
                }

                if (!PassesJobTrainingDashboardFilter(row, filterSupAreaId, filterAreaGroupId))
                {
                    continue;
                }

                string reqDate = FirstNonEmpty(GetString(row, "sReqDate"), GetString(row, "ReqDate"));
                string schDate = FirstNonEmpty(GetString(row, "sSchDate"), GetString(row, "SchDate"), GetString(row, "ScheduleDate"));
                if (!IsJobTrainingSchDateInPeriode(normalizedPeriode, schDate))
                {
                    continue;
                }

                string rawStatus = FirstNonEmpty(GetString(row, "Status"), GetString(row, "ValueStatus"), GetString(row, "StatusCode"));
                string mappedStatus = NormalizeSummaryStatus(rawStatus);
                if (normalizedStatus != "all" && !string.Equals(mappedStatus, normalizedStatus, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                bool isVisit = IsVisitCategory(categoryName, categoryId);
                string jobId = FirstNonEmpty(GetString(row, "TrainingID"), GetString(row, "JobID"));
                string custId = GetString(row, "CustID");
                string customerName = FirstNonEmpty(GetString(row, "CustomerName"), GetString(row, "CustName"), GetString(row, "FullName"));
                string branchName = GetString(row, "BranchName");
                string customerSupArea = FirstNonEmpty(
                    GetString(row, "SupAreaID"),
                    GetString(row, "SupportAreaID"),
                    GetString(row, "AreaID"));

                DataRow target = mapped.NewRow();
                target["JobID"] = jobId;
                target["CustID"] = custId;
                target["CustomerName"] = customerName;
                target["FullName"] = customerName;
                target["BranchName"] = branchName;
                target["Status"] = mappedStatus;
                target["JobCategory"] = isVisit ? "Visit" : "Training";
                target["JobType"] = isVisit ? "Visit" : "Training";
                target["ScheduleDate"] = schDate;
                target["DtmUpd"] = reqDate;
                target["AreaName"] = branchName;
                target["AreaID"] = customerSupArea;
                target["SupAreaID"] = customerSupArea;
                target["AreaGroupID"] = GetString(row, "AreaGroupID");
                target["QtyGPS"] = 1;
                target["QtyACS"] = 0;
                target["TotalGPS"] = 1;
                target["TotalACS"] = 0;
                target["OverSLA"] = 0;
                target["Remark"] = FirstNonEmpty(
                    GetString(row, "Remark"),
                    GetString(row, "Remarks"),
                    GetString(row, "BillAbleDesc"),
                    "-");
                mapped.Rows.Add(target);
            }

            return mapped;
        }

        private static DataTable CreateJobTrainingAssignListSchema()
        {
            DataTable mapped = new DataTable();
            mapped.Columns.Add("JobID");
            mapped.Columns.Add("CustID");
            mapped.Columns.Add("CustomerName");
            mapped.Columns.Add("FullName");
            mapped.Columns.Add("BranchName");
            mapped.Columns.Add("Status");
            mapped.Columns.Add("JobCategory");
            mapped.Columns.Add("JobType");
            mapped.Columns.Add("ScheduleDate");
            mapped.Columns.Add("DtmUpd");
            mapped.Columns.Add("AreaName");
            mapped.Columns.Add("AreaID");
            mapped.Columns.Add("SupAreaID");
            mapped.Columns.Add("AreaGroupID");
            mapped.Columns.Add("QtyGPS", typeof(int));
            mapped.Columns.Add("QtyACS", typeof(int));
            mapped.Columns.Add("TotalGPS", typeof(int));
            mapped.Columns.Add("TotalACS", typeof(int));
            mapped.Columns.Add("OverSLA", typeof(int));
            mapped.Columns.Add("Remark");
            return mapped;
        }

        private const string TrainCategoryTrainingId = "TRC0000001";
        private const string TrainCategoryVisitId = "TRC0000002";
        private const string TrainCategoryMapCacheKey = "DashboardAssignJobTrainCategoryMap";

        protected static bool IsTrainingOrVisitCategory(string categoryName, string categoryId)
        {
            return !string.IsNullOrWhiteSpace(ResolveTrainCategoryId(categoryName, categoryId));
        }

        private static bool IsJobTrainingScheduleCategory(DataRow row, string categoryName, string categoryId)
        {
            // Only ref_train_category Training / Visit (TRC0000001 / TRC0000002).
            return IsTrainingOrVisitCategory(categoryName, categoryId);
        }

        protected static bool IsVisitCategory(string categoryName, string categoryId)
        {
            string resolvedId = ResolveTrainCategoryId(categoryName, categoryId);
            if (string.IsNullOrWhiteSpace(resolvedId))
            {
                return false;
            }

            if (resolvedId.Equals(TrainCategoryVisitId, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (resolvedId.Equals(TrainCategoryTrainingId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string description;
            if (GetTrainCategoryMap().TryGetValue(resolvedId, out description))
            {
                return description.IndexOf("VISIT", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        private static string ResolveTrainCategoryId(string categoryName, string categoryId)
        {
            Dictionary<string, string> map = GetTrainCategoryMap();
            string id = (categoryId ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    if (map.ContainsKey(id))
                    {
                        return id;
                    }

                    if (id.Equals(TrainCategoryTrainingId, StringComparison.OrdinalIgnoreCase)
                        || id.Equals(TrainCategoryVisitId, StringComparison.OrdinalIgnoreCase))
                    {
                        return id;
                    }
                }

            string name = (categoryName ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(name))
            {
                foreach (KeyValuePair<string, string> item in map)
                {
                    if (item.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return item.Key;
                    }
                }

                if (name.Equals("Training", StringComparison.OrdinalIgnoreCase)
                    || name.Equals("TRAINING", StringComparison.OrdinalIgnoreCase))
                {
                    return TrainCategoryTrainingId;
                }

                if (name.Equals("Visit", StringComparison.OrdinalIgnoreCase)
                    || name.Equals("VISIT", StringComparison.OrdinalIgnoreCase))
                {
                    return TrainCategoryVisitId;
                }
            }

            return string.Empty;
        }

        private static Dictionary<string, string> GetTrainCategoryMap()
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Items[TrainCategoryMapCacheKey] is Dictionary<string, string>)
            {
                return (Dictionary<string, string>)context.Items[TrainCategoryMapCacheKey];
            }

            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            DataTable source = ExecuteJobTrainingQuery(
                "SELECT TrainCategoryID, TrainCategoryDesc, Status FROM ref_train_category WITH (NOLOCK)");
            if (source != null && source.Rows.Count > 0)
            {
                foreach (DataRow row in source.Rows)
                {
                    string id = FirstNonEmptyStatic(GetValue(row, "TrainCategoryID"), GetValue(row, "CategoryID")).Trim();
                    string desc = FirstNonEmptyStatic(
                        GetValue(row, "TrainCategoryDesc"),
                        GetValue(row, "TrainingCategoryName"),
                        GetValue(row, "CategoryName")).Trim();
                    string status = GetValue(row, "Status").Trim().ToUpperInvariant();
                    if (string.IsNullOrWhiteSpace(id) || map.ContainsKey(id))
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(status) && status != "RG" && status != "OP" && status != "AC")
                    {
                        continue;
                    }

                    map[id] = string.IsNullOrWhiteSpace(desc) ? id : desc;
                }
            }

            // Hard defaults from ref_train_category seed.
            if (!map.ContainsKey(TrainCategoryTrainingId))
            {
                map[TrainCategoryTrainingId] = "Training";
            }

            if (!map.ContainsKey(TrainCategoryVisitId))
            {
                map[TrainCategoryVisitId] = "Visit";
            }

            if (context != null)
            {
                context.Items[TrainCategoryMapCacheKey] = map;
            }

            return map;
        }

        private static bool TryParseTrainingDate(string raw, out DateTime date)
        {
            date = DateTime.MinValue;
            string source = (raw ?? string.Empty).Replace("&nbsp;", " ").Trim();
            if (string.IsNullOrWhiteSpace(source)
                || source.Equals("-", StringComparison.OrdinalIgnoreCase)
                || source.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string[] formats =
            {
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "d/M/yyyy",
                "dd-MM-yyyy",
                "d-M-yyyy",
                "dd MMM yyyy",
                "d MMM yyyy",
                "dd MMMM yyyy",
                "d MMMM yyyy",
                "MM/dd/yyyy",
                "yyyy/MM/dd"
            };

            CultureInfo[] cultures =
            {
                new CultureInfo("id-ID"),
                CultureInfo.InvariantCulture,
                new CultureInfo("en-US")
            };

            foreach (CultureInfo culture in cultures)
            {
                if (DateTime.TryParseExact(source, formats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out date))
                {
                    return true;
                }
            }

            foreach (CultureInfo culture in cultures)
            {
                if (DateTime.TryParse(source, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out date))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsJobTrainingSchDateInPeriode(string periode, string schDate)
        {
            string normalizedPeriode = (periode ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedPeriode))
            {
                return true;
            }

            DateTime parsed;
            if (!TryParseTrainingDate(schDate, out parsed))
            {
                return false;
            }

            return parsed.ToString("yyyy-MM", CultureInfo.InvariantCulture) == normalizedPeriode;
        }

        private static bool IsJobTrainingInPeriode(string periode, string reqDate, string schDate)
        {
            // Jadwal / dashboard period filter follows Sch Date only.
            return IsJobTrainingSchDateInPeriode(periode, schDate);
        }

        private DataTable GetAvailabilityData(string periode, string supAreaId, string areaGroupId)
        {
            if (UseJobTrainingDataSource)
            {
                return BuildJobTrainingAvailabilitySchedule(periode, supAreaId, areaGroupId);
            }

            try
            {
                string safePeriode = (periode ?? string.Empty).Replace("'", "''");
                string safeSupAreaId = (supAreaId ?? string.Empty).Replace("'", "''");
                string safeAreaGroupId = NormalizeAreaGroupId(areaGroupId).Replace("'", "''");
                if (safeSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                {
                    safeSupAreaId = string.Empty;
                }

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_availability '" + safePeriode + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'",
                    DBConnstringSQL());

                DataTable dt = rec.DataRecord();
                return dt ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }

        private DataTable BuildJobTrainingAvailabilitySchedule(string periode, string selectedRegional, string selectedAreaGroup)
        {
            DataTable result = CreateJobTrainingAvailabilitySchema();
            DateTime periodDate;
            if (!DateTime.TryParseExact((periode ?? string.Empty).Trim() + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodDate)
                && !DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            int totalDays = DateTime.DaysInMonth(periodDate.Year, periodDate.Month);
            string filterSupArea = ResolveSupAreaParameter(selectedRegional);
            string filterAreaGroup = ResolveItsScheduleAreaGroupFilter(selectedAreaGroup);
            Dictionary<string, JobTrainingTrainerSchedule> trainers = BuildItsUserScheduleMap(totalDays, filterAreaGroup, periode);
            if (trainers.Count == 0 && string.IsNullOrWhiteSpace(ResolveItsAreaFilterToken(filterAreaGroup)))
            {
                EnsureUnassignedScheduleUser(trainers, totalDays);
            }

            // Col-day numbers = assignments to IT Support (trx_job_assign_detail).
            ApplyTrxJobAssignDetailDayCounts(trainers, periodDate, filterSupArea, filterAreaGroup);
            // Total Closed JO Training/Visit columns = job_training.aspx data.
            ApplyJobTrainingClosedCountsOnly(trainers, periodDate, filterSupArea, filterAreaGroup);

            IEnumerable<JobTrainingTrainerSchedule> orderedTrainers = trainers.Values
                .GroupBy(t => t.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .OrderBy(t => string.Equals(t.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(t => t.TrainerName, StringComparer.OrdinalIgnoreCase);

            foreach (JobTrainingTrainerSchedule trainer in orderedTrainers)
            {
                for (int day = 1; day <= totalDays; day++)
                {
                    DateTime currentDate = new DateTime(periodDate.Year, periodDate.Month, day);
                    bool isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
                    int totalJob = trainer.DayJobCount[day];
                    int remainingJo = trainer.DayOpenCount[day];

                    DataRow target = result.NewRow();
                    target["TechnicianID"] = trainer.TrainerId;
                    target["Name"] = trainer.TrainerName;
                    target["DayNo"] = day;
                    target["DayName"] = currentDate.ToString("ddd");
                    target["IsWeekend"] = isWeekend ? "1" : "0";
                    target["DisplayValue"] = totalJob > 0 ? totalJob.ToString() : "AV";
                    target["TotalJob"] = totalJob;
                    target["RemainingJo"] = remainingJo;
                    target["TotalJobCloseNew"] = day == 1 ? trainer.ClosedTraining : 0;
                    target["TotalJobCloseMaint"] = day == 1 ? trainer.ClosedVisit : 0;
                    target["TotalJobCloseUnit"] = totalJob > 0 && remainingJo == 0 ? totalJob : 0;
                    target["IsAvailable"] = "1";
                    target["SupAreaID"] = filterSupArea;
                    target["ITID"] = trainer.ItId ?? string.Empty;
                    result.Rows.Add(target);
                }
            }

            return result;
        }

        private static void ApplyJobTrainingClosedCountsOnly(
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            DateTime periodDate,
            string filterSupAreaId,
            string filterAreaGroupId)
        {
            if (trainers == null || trainers.Count == 0)
            {
                return;
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values
                .GroupBy(t => t.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()))
            {
                trainer.ClosedTraining = 0;
                trainer.ClosedVisit = 0;
            }

            DataTable jobs = LoadJobTrainingRowsForSchedule(periodDate.ToString("yyyy-MM"));
            if (jobs == null || jobs.Rows.Count == 0)
            {
                return;
            }

            Dictionary<string, string> technicianByJobId = BuildJobTechnicianIdByJobMap(periodDate);

            foreach (DataRow row in jobs.Rows)
            {
                string categoryName = FirstNonEmptyStatic(
                    GetValue(row, "TrainingCategoryName"),
                    GetValue(row, "TrainCategoryName"),
                    GetValue(row, "CategoryName"),
                    GetValue(row, "Category"));
                string categoryId = FirstNonEmptyStatic(
                    GetValue(row, "TrainCategoryID"),
                    GetValue(row, "TrainingCategoryID"),
                    GetValue(row, "CategoryID"));
                if (!IsJobTrainingScheduleCategory(row, categoryName, categoryId))
                {
                    continue;
                }

                if (!PassesJobTrainingDashboardFilter(row, filterSupAreaId, filterAreaGroupId))
                {
                    continue;
                }

                string schDateRaw = FirstNonEmptyStatic(
                    GetValue(row, "sSchDate"),
                    GetValue(row, "SchDate"),
                    GetValue(row, "ScheduleDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                List<JobTrainingTrainerSchedule> matchedUsers =
                    ResolveJobTrainingAssigneesByTechnicianId(row, trainers, technicianByJobId);
                if (matchedUsers.Count == 0)
                {
                    JobTrainingTrainerSchedule unassigned;
                    if (trainers.TryGetValue("UNASSIGNED", out unassigned) && unassigned != null)
                    {
                        matchedUsers.Add(unassigned);
                    }
                }

                bool isVisit = IsVisitCategory(categoryName, categoryId);
                string statusRaw = FirstNonEmptyStatic(
                    GetValue(row, "Status"),
                    GetValue(row, "ValueStatus"),
                    GetValue(row, "StatusCode"));
                if (!IsClosedJobTrainingStatus(statusRaw))
                {
                    continue;
                }

                foreach (JobTrainingTrainerSchedule trainer in matchedUsers)
                {
                    if (trainer == null)
                    {
                        continue;
                    }

                    if (isVisit)
                    {
                        trainer.ClosedVisit++;
                    }
                    else
                    {
                        trainer.ClosedTraining++;
                    }
                }
            }
        }

        private static void ApplyJobTrainingScheduleDayCounts(
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            DateTime periodDate,
            string filterSupAreaId,
            string filterAreaGroupId)
        {
            if (trainers == null || trainers.Count == 0)
            {
                return;
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values
                .GroupBy(t => t.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()))
            {
                trainer.ClosedTraining = 0;
                trainer.ClosedVisit = 0;
                if (trainer.DayJobCount != null)
                {
                    Array.Clear(trainer.DayJobCount, 0, trainer.DayJobCount.Length);
                }
                if (trainer.DayOpenCount != null)
                {
                    Array.Clear(trainer.DayOpenCount, 0, trainer.DayOpenCount.Length);
                }
            }

            DataTable jobs = LoadJobTrainingRowsForSchedule(periodDate.ToString("yyyy-MM"));
            if (jobs == null || jobs.Rows.Count == 0)
            {
                return;
            }

            Dictionary<string, string> technicianByJobId = BuildJobTechnicianIdByJobMap(periodDate);

            foreach (DataRow row in jobs.Rows)
            {
                string categoryName = FirstNonEmptyStatic(
                    GetValue(row, "TrainingCategoryName"),
                    GetValue(row, "TrainCategoryName"),
                    GetValue(row, "CategoryName"),
                    GetValue(row, "Category"));
                string categoryId = FirstNonEmptyStatic(
                    GetValue(row, "TrainCategoryID"),
                    GetValue(row, "TrainingCategoryID"),
                    GetValue(row, "CategoryID"));
                if (!IsJobTrainingScheduleCategory(row, categoryName, categoryId))
                {
                    continue;
                }

                if (!PassesJobTrainingDashboardFilter(row, filterSupAreaId, filterAreaGroupId))
                {
                    continue;
                }

                string schDateRaw = FirstNonEmptyStatic(
                    GetValue(row, "sSchDate"),
                    GetValue(row, "SchDate"),
                    GetValue(row, "ScheduleDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                int dayNo = schDate.Day;
                if (dayNo <= 0)
                {
                    continue;
                }

                List<JobTrainingTrainerSchedule> matchedUsers =
                    ResolveJobTrainingAssigneesByTechnicianId(row, trainers, technicianByJobId);
                if (matchedUsers.Count == 0)
                {
                    JobTrainingTrainerSchedule unassigned;
                    if (trainers.TryGetValue("UNASSIGNED", out unassigned) && unassigned != null)
                    {
                        matchedUsers.Add(unassigned);
                    }
                }

                bool isVisit = IsVisitCategory(categoryName, categoryId);
                string statusRaw = FirstNonEmptyStatic(
                    GetValue(row, "Status"),
                    GetValue(row, "ValueStatus"),
                    GetValue(row, "StatusCode"));
                bool isClosed = IsClosedJobTrainingStatus(statusRaw);

                foreach (JobTrainingTrainerSchedule trainer in matchedUsers)
                {
                    if (trainer == null || trainer.DayJobCount == null || dayNo >= trainer.DayJobCount.Length)
                    {
                        continue;
                    }

                    trainer.DayJobCount[dayNo]++;
                    if (!isClosed && trainer.DayOpenCount != null && dayNo < trainer.DayOpenCount.Length)
                    {
                        trainer.DayOpenCount[dayNo]++;
                    }

                    if (isClosed)
                    {
                        if (isVisit)
                        {
                            trainer.ClosedVisit++;
                        }
                        else
                        {
                            trainer.ClosedTraining++;
                        }
                    }
                }
            }
        }

        private static void ApplyTrxJobAssignDetailDayCounts(
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            DateTime periodDate,
            string filterSupAreaId = "",
            string filterAreaGroupId = "")
        {
            if (trainers == null || trainers.Count == 0)
            {
                return;
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values
                .GroupBy(t => t.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()))
            {
                if (trainer.DayJobCount != null)
                {
                    Array.Clear(trainer.DayJobCount, 0, trainer.DayJobCount.Length);
                }
                if (trainer.DayOpenCount != null)
                {
                    Array.Clear(trainer.DayOpenCount, 0, trainer.DayOpenCount.Length);
                }
            }

            DateTime monthStart = new DateTime(periodDate.Year, periodDate.Month, 1);
            DateTime monthEndExclusive = monthStart.AddMonths(1);
            DataTable details = LoadTrxJobAssignDetailRows(monthStart, monthEndExclusive);
            if (details == null || details.Rows.Count == 0)
            {
                return;
            }

            Dictionary<string, string> areaIdAreaGroupMap = LoadAreaIdAreaGroupMap();

            foreach (DataRow row in details.Rows)
            {
                if (!PassesTrxAssignDashboardFilter(row, filterSupAreaId, filterAreaGroupId, areaIdAreaGroupMap))
                {
                    continue;
                }

                string technicianId = FirstNonEmptyStatic(
                    GetValue(row, "TechnicianID"),
                    GetValue(row, "TechnicianId"),
                    GetValue(row, "ITID")).Trim();
                if (string.IsNullOrWhiteSpace(technicianId))
                {
                    continue;
                }

                JobTrainingTrainerSchedule trainer = FindItsTrainerByTechnicianKey(trainers, technicianId);
                if (trainer == null)
                {
                    continue;
                }

                string schDateRaw = FirstNonEmptyStatic(
                    GetValue(row, "SchDate"),
                    GetValue(row, "ScheduleDate"),
                    GetValue(row, "sSchDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                int dayNo = schDate.Day;
                if (dayNo <= 0 || trainer.DayJobCount == null || dayNo >= trainer.DayJobCount.Length)
                {
                    continue;
                }

                trainer.DayJobCount[dayNo]++;
                string statusRaw = FirstNonEmptyStatic(
                    GetValue(row, "Status"),
                    GetValue(row, "StatusCode"),
                    GetValue(row, "ValueStatus"));
                bool isClosed = IsClosedAssignDetailStatus(statusRaw);
                if (!isClosed && trainer.DayOpenCount != null && dayNo < trainer.DayOpenCount.Length)
                {
                    trainer.DayOpenCount[dayNo]++;
                }
            }
        }

        private static bool IsAssignDetailVisitOrMaint(DataRow row)
        {
            if (row == null)
            {
                return false;
            }

            string raw = FirstNonEmptyStatic(
                GetValue(row, "JobType"),
                GetValue(row, "JOType"),
                GetValue(row, "JobCategory"),
                GetValue(row, "Category"),
                GetValue(row, "InsDeviceTypeID"),
                GetValue(row, "InsDeviceTypeId"),
                GetValue(row, "DeviceTypeDesc")).Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            return raw.Contains("VISIT")
                || raw.Contains("MAINT")
                || raw.Contains("MAINTENANCE")
                || raw == "MNT"
                || raw == "MT";
        }

        protected static DataTable LoadTrxJobAssignDetailRows(DateTime dateFrom, DateTime dateToExclusive)
        {
            string fromText = dateFrom.ToString("yyyy-MM-dd");
            string toText = dateToExclusive.ToString("yyyy-MM-dd");
            string dateFilter = "WHERE SchDate >= '" + fromText.Replace("'", "''") + "' "
                + "AND SchDate < '" + toText.Replace("'", "''") + "'";

            string[] queries =
            {
                "SELECT TechnicianID, SchDate, JobID, AssignID, Seq, Status, CustID, DeviceGroupID, "
                    + "PoliceNo, NoSN, GSMNo, AreaID, AreaName, JobType, Customer, CustomerName, "
                    + "DeviceTypeDesc, InstallDate, MIS_Date, MISDate, Remark, QtyGPS, QtyACS "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) " + dateFilter,
                "SELECT TechnicianID, SchDate, JobID, AssignID, Seq, Status, CustID, DeviceGroupID, "
                    + "PoliceNo, NoSN, GSMNo, JobType, Remark "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) " + dateFilter,
                "SELECT TechnicianID, SchDate, JobID, AssignID, Seq, Status "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) " + dateFilter,
                "SELECT TechnicianID, SchDate, JobID, AssignID "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) " + dateFilter
            };

            foreach (string sql in queries)
            {
                DataTable details = ExecuteJobTrainingQuery(sql);
                if (details != null && details.Columns.Count > 0)
                {
                    return details;
                }
            }

            return new DataTable();
        }

        private static Dictionary<string, string> BuildJobTechnicianIdByJobMap(DateTime periodDate)
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            DateTime monthStart = new DateTime(periodDate.Year, periodDate.Month, 1);
            DataTable details = LoadTrxJobAssignDetailRows(monthStart, monthStart.AddMonths(1));
            if (details == null || details.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in details.Rows)
            {
                string jobId = FirstNonEmptyStatic(GetValue(row, "JobID"), GetValue(row, "AssignID")).Trim();
                string technicianId = FirstNonEmptyStatic(
                    GetValue(row, "TechnicianID"),
                    GetValue(row, "TechnicianId"),
                    GetValue(row, "ITID")).Trim();
                if (string.IsNullOrWhiteSpace(jobId) || string.IsNullOrWhiteSpace(technicianId))
                {
                    continue;
                }

                map[jobId] = technicianId;
            }

            return map;
        }

        private static string GetJobTrainingTechnicianId(DataRow row, Dictionary<string, string> technicianByJobId)
        {
            if (row == null)
            {
                return string.Empty;
            }

            string fromRow = FirstNonEmptyStatic(
                GetValue(row, "TechnicianID"),
                GetValue(row, "TechnicianId"),
                GetValue(row, "ITID")).Trim();
            if (!string.IsNullOrWhiteSpace(fromRow))
            {
                return fromRow;
            }

            string jobId = FirstNonEmptyStatic(GetValue(row, "TrainingID"), GetValue(row, "JobID")).Trim();
            if (string.IsNullOrWhiteSpace(jobId) || technicianByJobId == null)
            {
                return string.Empty;
            }

            string fromAssignment;
            if (technicianByJobId.TryGetValue(jobId, out fromAssignment) && !string.IsNullOrWhiteSpace(fromAssignment))
            {
                return fromAssignment.Trim();
            }

            return string.Empty;
        }

        private static JobTrainingTrainerSchedule FindItsTrainerByTechnicianKey(
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            string technicianKey)
        {
            if (trainers == null || string.IsNullOrWhiteSpace(technicianKey))
            {
                return null;
            }

            string key = technicianKey.Trim();
            JobTrainingTrainerSchedule match;
            if (trainers.TryGetValue(key, out match) && match != null)
            {
                return match;
            }

            string trimmedKey = TrimToLength(key, 20);
            if (!trimmedKey.Equals(key, StringComparison.OrdinalIgnoreCase)
                && trainers.TryGetValue(trimmedKey, out match)
                && match != null)
            {
                return match;
            }

            string normalizedItId = NormalizeItId(key);
            if (IsValidItId(normalizedItId)
                && trainers.TryGetValue(normalizedItId, out match)
                && match != null)
            {
                return match;
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values)
            {
                if (trainer == null
                    || string.Equals(trainer.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (key.Equals(trainer.TrainerId, StringComparison.OrdinalIgnoreCase)
                    || key.Equals(trainer.ItId, StringComparison.OrdinalIgnoreCase)
                    || trimmedKey.Equals(trainer.TrainerId, StringComparison.OrdinalIgnoreCase)
                    || (IsValidItId(normalizedItId)
                        && IsValidItId(trainer.ItId)
                        && normalizedItId.Equals(NormalizeItId(trainer.ItId), StringComparison.OrdinalIgnoreCase)))
                {
                    return trainer;
                }
            }

            return null;
        }

        private static List<JobTrainingTrainerSchedule> ResolveJobTrainingAssigneesByTechnicianId(
            DataRow row,
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            Dictionary<string, string> technicianByJobId)
        {
            List<JobTrainingTrainerSchedule> matched = new List<JobTrainingTrainerSchedule>();
            if (row == null || trainers == null || trainers.Count == 0)
            {
                return matched;
            }

            string technicianId = GetJobTrainingTechnicianId(row, technicianByJobId);
            if (string.IsNullOrWhiteSpace(technicianId))
            {
                return matched;
            }

            JobTrainingTrainerSchedule trainer = FindItsTrainerByTechnicianKey(trainers, technicianId);
            if (trainer != null
                && !string.Equals(trainer.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase))
            {
                matched.Add(trainer);
            }

            return matched;
        }

        private static bool JobMatchesTechnicianId(
            DataRow row,
            string wantedTechnicianKey,
            string wantedTechnicianName,
            Dictionary<string, JobTrainingTrainerSchedule> itsUsers,
            Dictionary<string, string> technicianByJobId)
        {
            if (row == null)
            {
                return false;
            }

            string wantKey = (wantedTechnicianKey ?? string.Empty).Trim();
            string wantName = (wantedTechnicianName ?? string.Empty).Trim();
            bool isUnassignedTarget = string.Equals(wantKey, "UNASSIGNED", StringComparison.OrdinalIgnoreCase)
                || string.Equals(wantName, "Unassigned", StringComparison.OrdinalIgnoreCase);
            string rowTechnicianId = GetJobTrainingTechnicianId(row, technicianByJobId);

            if (isUnassignedTarget)
            {
                return string.IsNullOrWhiteSpace(rowTechnicianId);
            }

            if (string.IsNullOrWhiteSpace(rowTechnicianId))
            {
                return false;
            }

            JobTrainingTrainerSchedule rowTrainer = FindItsTrainerByTechnicianKey(itsUsers, rowTechnicianId);
            JobTrainingTrainerSchedule wantTrainer = FindItsTrainerByTechnicianKey(itsUsers, wantKey);
            if (rowTrainer != null && wantTrainer != null)
            {
                return rowTrainer.TrainerId.Equals(wantTrainer.TrainerId, StringComparison.OrdinalIgnoreCase);
            }

            if (!string.IsNullOrWhiteSpace(wantKey))
            {
                if (rowTechnicianId.Equals(wantKey, StringComparison.OrdinalIgnoreCase)
                    || TrimToLength(rowTechnicianId, 20).Equals(TrimToLength(wantKey, 20), StringComparison.OrdinalIgnoreCase)
                    || (IsValidItId(rowTechnicianId)
                        && IsValidItId(wantKey)
                        && NormalizeItId(rowTechnicianId).Equals(NormalizeItId(wantKey), StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            if (!string.IsNullOrWhiteSpace(wantName) && rowTrainer != null)
            {
                return rowTrainer.TrainerName.Equals(wantName, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private static bool IsClosedAssignDetailStatus(string status)
        {
            string normalized = (status ?? string.Empty).Trim().ToUpperInvariant();
            return normalized == "CL"
                || normalized == "CLOSE"
                || normalized == "CLOSED"
                || normalized == "SELESAI";
        }

        private static string NormalizeItId(string value)
        {
            return (value ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static bool IsValidItId(string value)
        {
            string normalized = NormalizeItId(value);
            if (normalized.Length < 3 || !normalized.StartsWith("IT", StringComparison.Ordinal))
            {
                return false;
            }

            for (int i = 2; i < normalized.Length; i++)
            {
                if (!char.IsDigit(normalized[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TryResolveItsAssignTechnicianId(string userIdOrItId, out string itId, out string message)
        {
            itId = string.Empty;
            message = string.Empty;
            string raw = (userIdOrItId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(raw)
                || raw.Equals("UNASSIGNED", StringComparison.OrdinalIgnoreCase))
            {
                message = "ITID not exist";
                return false;
            }

            if (IsValidItId(raw))
            {
                itId = NormalizeItId(raw);
                return true;
            }

            string resolved = LookupItIdByUserId(raw);
            if (!IsValidItId(resolved))
            {
                message = "ITID not exist";
                return false;
            }

            itId = NormalizeItId(resolved);
            return true;
        }

        private static string LookupItIdByUserId(string userId)
        {
            string safeUserId = EscapeSqlLiteral((userId ?? string.Empty).Trim());
            if (string.IsNullOrWhiteSpace(safeUserId))
            {
                return string.Empty;
            }

            if (IsJobTrainingAssignRequestContext())
            {
                string fromMst = ItsSupportAssignData.LookupItId(safeUserId);
                if (!string.IsNullOrWhiteSpace(fromMst))
                {
                    return fromMst;
                }
            }

            DataTable rows = ExecuteJobTrainingQuery(
                "SELECT TOP 1 ITID FROM conf_mst_user WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + safeUserId + "'");
            if (rows != null && rows.Rows.Count > 0)
            {
                return FirstNonEmptyStatic(GetValue(rows.Rows[0], "ITID"));
            }

            rows = ExecuteJobTrainingQuery(
                "SELECT TOP 1 ITID FROM conf_mst_user WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(UsrID, ''))) = '" + safeUserId + "'");
            if (rows != null && rows.Rows.Count > 0)
            {
                return FirstNonEmptyStatic(GetValue(rows.Rows[0], "ITID"));
            }

            return string.Empty;
        }

        private static Dictionary<string, JobTrainingTrainerSchedule> BuildItsUserScheduleMap(
            int totalDays,
            string filterAreaGroupId = "",
            string periode = "")
        {
            Dictionary<string, JobTrainingTrainerSchedule> trainers =
                new Dictionary<string, JobTrainingTrainerSchedule>(StringComparer.OrdinalIgnoreCase);

            DataTable users = LoadItsAuthUsers(filterAreaGroupId, periode);
            if (users == null || users.Rows.Count == 0)
            {
                return trainers;
            }

            Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();

            foreach (DataRow row in users.Rows)
            {
                string itId = NormalizeItId(FirstNonEmptyStatic(GetRowValueInsensitive(row, "ITID")));
                string userId = FirstNonEmptyStatic(
                    GetRowValueInsensitive(row, "UserID", "UsrID"));
                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = itId;
                }

                string fullName = FirstNonEmptyStatic(
                    GetRowValueInsensitive(row, "FullName", "UserName", "Name"),
                    userId);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    continue;
                }

                if (!IsValidItId(itId))
                {
                    itId = NormalizeItId(LookupItIdByUserId(userId));
                }

                string trainerKey = IsValidItId(itId) ? itId : TrimToLength(userId, 20);

                string supAreaId = FirstNonEmptyStatic(
                    GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID")).Trim();
                if (supAreaId.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                    || supAreaId.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    supAreaId = string.Empty;
                }

                string areaGroupId = NormalizeAreaGroupId(GetRowValueInsensitive(row, "AreaGroupID"));
                if (string.IsNullOrWhiteSpace(areaGroupId) && !string.IsNullOrWhiteSpace(supAreaId))
                {
                    string derivedGroup;
                    if (supAreaToGroup.TryGetValue(supAreaId, out derivedGroup))
                    {
                        areaGroupId = NormalizeAreaGroupId(derivedGroup);
                    }
                }

                if (!trainers.ContainsKey(trainerKey))
                {
                    JobTrainingTrainerSchedule entry = new JobTrainingTrainerSchedule
                    {
                        TrainerId = trainerKey,
                        TrainerName = string.IsNullOrWhiteSpace(fullName) ? userId : fullName,
                        ItId = IsValidItId(itId) ? itId : string.Empty,
                        SupAreaID = supAreaId,
                        AreaGroupID = areaGroupId,
                        DayJobCount = new int[Math.Max(totalDays, 1) + 1],
                        DayOpenCount = new int[Math.Max(totalDays, 1) + 1]
                    };
                    trainers[trainerKey] = entry;
                    if (!string.IsNullOrWhiteSpace(userId)
                        && !userId.Equals(trainerKey, StringComparison.OrdinalIgnoreCase)
                        && !trainers.ContainsKey(userId))
                    {
                        trainers[userId] = entry;
                    }
                    if (!string.IsNullOrWhiteSpace(entry.TrainerId)
                        && !trainers.ContainsKey(entry.TrainerId))
                    {
                        trainers[entry.TrainerId] = entry;
                    }
                    if (IsValidItId(entry.ItId)
                        && !entry.ItId.Equals(trainerKey, StringComparison.OrdinalIgnoreCase)
                        && !trainers.ContainsKey(entry.ItId))
                    {
                        trainers[entry.ItId] = entry;
                    }
                }
            }

            return trainers;
        }

        private static void EnsureUnassignedScheduleUser(
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            int totalDays)
        {
            if (trainers == null)
            {
                return;
            }

            if (trainers.ContainsKey("UNASSIGNED"))
            {
                return;
            }

            trainers["UNASSIGNED"] = new JobTrainingTrainerSchedule
            {
                TrainerId = "UNASSIGNED",
                TrainerName = "Unassigned",
                DayJobCount = new int[Math.Max(totalDays, 1) + 1],
                DayOpenCount = new int[Math.Max(totalDays, 1) + 1]
            };
        }

        private static List<string> CollectJobTrainingAssigneeTokens(DataRow row)
        {
            List<string> tokens = new List<string>();
            if (row == null)
            {
                return tokens;
            }

            // Primary assignee for job_training Jadwal: mst_customer.ITOutbound (UserID).
            AppendAssigneeTokens(tokens, NormalizeItOutboundValue(GetValue(row, "ITOutbound")));

            AppendAssigneeTokens(
                tokens,
                FirstNonEmptyStatic(
                    GetValue(row, "Trainers"),
                    GetValue(row, "Trainer"),
                    GetValue(row, "ITStaff"),
                    GetValue(row, "ITName")));
            // Remark can contain ITID:/ITNAME:/IT: tags written on create from Jadwal Harian.
            AppendAssigneeTokens(tokens, GetValue(row, "Remark"));
            return tokens;
        }

        private static List<JobTrainingTrainerSchedule> ResolveJobTrainingAssignees(
            DataRow row,
            Dictionary<string, JobTrainingTrainerSchedule> trainers,
            bool includeItOutbound = true)
        {
            List<JobTrainingTrainerSchedule> matched = new List<JobTrainingTrainerSchedule>();
            if (row == null || trainers == null || trainers.Count == 0)
            {
                return matched;
            }

            if (includeItOutbound)
            {
                string itOutbound = NormalizeItOutboundValue(GetValue(row, "ITOutbound"));
                if (!string.IsNullOrWhiteSpace(itOutbound))
                {
                    matched = ResolveItsUsersForTokens(new List<string> { itOutbound }, trainers);
                    if (matched.Count > 0)
                    {
                        return matched;
                    }
                }
            }

            List<string> fallbackTokens = new List<string>();
            AppendAssigneeTokens(
                fallbackTokens,
                FirstNonEmptyStatic(
                    GetValue(row, "Trainers"),
                    GetValue(row, "Trainer"),
                    GetValue(row, "ITStaff"),
                    GetValue(row, "ITName")));
            AppendAssigneeTokens(fallbackTokens, GetValue(row, "Remark"));
            return ResolveItsUsersForTokens(fallbackTokens, trainers);
        }

        private static void AppendAssigneeTokens(List<string> tokens, string raw)
        {
            if (tokens == null || string.IsNullOrWhiteSpace(raw))
            {
                return;
            }

            HashSet<string> seen = new HashSet<string>(tokens, StringComparer.OrdinalIgnoreCase);
            foreach (string part in SplitTrainerNames(raw))
            {
                string normalized = NormalizeItsAssigneeToken(part);
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    continue;
                }

                if (seen.Add(normalized))
                {
                    tokens.Add(normalized);
                }

                if (seen.Add(part) && !part.Equals(normalized, StringComparison.OrdinalIgnoreCase))
                {
                    tokens.Add(part);
                }
            }

            AppendTaggedAssigneeTokens(tokens, seen, raw);
        }

        private static void AppendTaggedAssigneeTokens(List<string> tokens, HashSet<string> seen, string raw)
        {
            if (tokens == null || seen == null || string.IsNullOrWhiteSpace(raw))
            {
                return;
            }

            string[] prefixes = { "ITID:", "ITNAME:", "IT:" };
            string upper = raw.ToUpperInvariant();
            foreach (string prefix in prefixes)
            {
                int searchFrom = 0;
                while (searchFrom < raw.Length)
                {
                    int idx = upper.IndexOf(prefix, searchFrom, StringComparison.Ordinal);
                    if (idx < 0)
                    {
                        break;
                    }

                    int valueStart = idx + prefix.Length;
                    int valueEnd = valueStart;
                    while (valueEnd < raw.Length)
                    {
                        char ch = raw[valueEnd];
                        if (ch == '|' || ch == ',' || ch == ';' || ch == '/' || ch == '\n' || ch == '\r')
                        {
                            break;
                        }

                        valueEnd++;
                    }

                    string value = NormalizeItsAssigneeToken(raw.Substring(idx, valueEnd - idx));
                    if (!string.IsNullOrWhiteSpace(value) && seen.Add(value))
                    {
                        tokens.Add(value);
                    }

                    searchFrom = valueEnd;
                }
            }
        }

        private static string NormalizeItsAssigneeToken(string token)
        {
            string value = (token ?? string.Empty).Trim();
            if (value.StartsWith("ITID:", StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(5).Trim();
            }

            if (value.StartsWith("ITNAME:", StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(7).Trim();
            }

            if (value.StartsWith("IT:", StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(3).Trim();
            }

            return value;
        }

        private static List<JobTrainingTrainerSchedule> ResolveItsUsersForTokens(
            List<string> tokens,
            Dictionary<string, JobTrainingTrainerSchedule> trainers)
        {
            List<JobTrainingTrainerSchedule> matched = new List<JobTrainingTrainerSchedule>();
            if (trainers == null || trainers.Count == 0)
            {
                return matched;
            }

            if (tokens == null || tokens.Count == 0)
            {
                return matched;
            }

            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string token in tokens)
            {
                JobTrainingTrainerSchedule user = FindItsUserByToken(token, trainers);
                if (user != null
                    && !string.Equals(user.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase)
                    && seen.Add(user.TrainerId))
                {
                    matched.Add(user);
                }
            }

            return matched;
        }

        private static JobTrainingTrainerSchedule FindItsUserByToken(
            string token,
            Dictionary<string, JobTrainingTrainerSchedule> trainers)
        {
            string value = NormalizeItsAssigneeToken(token);
            if (string.IsNullOrWhiteSpace(value) || trainers == null || trainers.Count == 0)
            {
                return null;
            }

            JobTrainingTrainerSchedule exact;
            if (trainers.TryGetValue(value, out exact)
                && !string.Equals(exact.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase))
            {
                return exact;
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values)
            {
                if (string.Equals(trainer.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (trainer.TrainerName.Equals(value, StringComparison.OrdinalIgnoreCase)
                    || trainer.TrainerId.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return trainer;
                }
            }

            foreach (JobTrainingTrainerSchedule trainer in trainers.Values)
            {
                if (string.Equals(trainer.TrainerId, "UNASSIGNED", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if ((!string.IsNullOrWhiteSpace(trainer.TrainerName) && trainer.TrainerName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(value) && value.IndexOf(trainer.TrainerName, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    return trainer;
                }
            }

            return null;
        }

        private static string ResolveItsAreaFilterToken(string filterAreaGroupId)
        {
            string normalized = NormalizeAreaGroupId(filterAreaGroupId);
            if (normalized.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase))
            {
                return "WEST";
            }

            if (normalized.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase))
            {
                return "EAST";
            }

            return string.Empty;
        }

        private static string ResolveItsScheduleAreaGroupFilter(string selectedAreaGroup)
        {
            string normalized = NormalizeAreaGroupId(selectedAreaGroup);
            if (!string.IsNullOrWhiteSpace(normalized))
            {
                return normalized;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return string.Empty;
            }

            object sessionValue = context.Session[GetRegionalGroupTabSessionKeyStatic()];
            return NormalizeAreaGroupId(Convert.ToString(sessionValue).Trim());
        }

        private static DataTable LoadItsUserIdsWithCustomersInAreaGroup(string areaGroupId)
        {
            DataTable result = new DataTable();
            result.Columns.Add("UserID");

            string normalizedGroup = NormalizeAreaGroupId(areaGroupId);
            if (string.IsNullOrWhiteSpace(normalizedGroup))
            {
                return result;
            }

            string safeGroup = normalizedGroup.Replace("'", "''");
            string sql =
                "SELECT DISTINCT UserID FROM ( "
                + "SELECT LTRIM(RTRIM(ISNULL(c.ITS, ''))) AS UserID "
                + "FROM mst_customer c WITH (NOLOCK) "
                + "LEFT JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = c.SupAreaID "
                + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID AND ISNULL(ag.Status, '') = 'RG' "
                + "WHERE ISNULL(c.Status, '') NOT IN ('DE', 'BL') "
                + "AND LTRIM(RTRIM(ISNULL(c.ITS, ''))) NOT IN ('', '[SELECT]') "
                + "AND ag.AreaGroupID = '" + safeGroup + "' "
                + "UNION "
                + "SELECT LTRIM(RTRIM(ISNULL(c.ITOutbound, ''))) AS UserID "
                + "FROM mst_customer c WITH (NOLOCK) "
                + "LEFT JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = c.SupAreaID "
                + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID AND ISNULL(ag.Status, '') = 'RG' "
                + "WHERE ISNULL(c.Status, '') NOT IN ('DE', 'BL') "
                + "AND LTRIM(RTRIM(ISNULL(c.ITOutbound, ''))) NOT IN ('', '[SELECT]') "
                + "AND ag.AreaGroupID = '" + safeGroup + "' "
                + ") regional WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) <> ''";

            DataTable rows = ExecuteJobTrainingQuery(sql);
            return rows ?? result;
        }

        private static HashSet<string> BuildItsUserIdSetFromTable(DataTable source)
        {
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source == null || source.Rows.Count == 0)
            {
                return ids;
            }

            foreach (DataRow row in source.Rows)
            {
                string userId = FirstNonEmptyStatic(GetValue(row, "UserID"), GetValue(row, "UsrID")).Trim();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    ids.Add(userId);
                    ids.Add(TrimToLength(userId, 20));
                }
            }

            return ids;
        }

        private static void EnsureItsAuthUserAreaColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            if (!table.Columns.Contains("SupAreaID"))
            {
                table.Columns.Add("SupAreaID", typeof(string));
            }
            if (!table.Columns.Contains("SupportAreaID"))
            {
                table.Columns.Add("SupportAreaID", typeof(string));
            }
            if (!table.Columns.Contains("AreaGroupID"))
            {
                table.Columns.Add("AreaGroupID", typeof(string));
            }
        }

        private static bool ItsUserMatchesAreaGroup(
            DataRow userRow,
            string userId,
            string requiredGroup,
            HashSet<string> customerRegionalUserIds,
            Dictionary<string, string> supAreaToGroup)
        {
            if (string.IsNullOrWhiteSpace(requiredGroup) || string.IsNullOrWhiteSpace(userId))
            {
                return true;
            }

            if (customerRegionalUserIds != null
                && (customerRegionalUserIds.Contains(userId)
                    || customerRegionalUserIds.Contains(TrimToLength(userId, 20))))
            {
                return true;
            }

            string rowGroup = NormalizeAreaGroupId(GetValue(userRow, "AreaGroupID"));
            if (rowGroup.Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string supAreaId = FirstNonEmptyStatic(
                GetValue(userRow, "SupAreaID"),
                GetValue(userRow, "SupportAreaID")).Trim();
            if (!string.IsNullOrWhiteSpace(supAreaId) && supAreaToGroup != null)
            {
                string mappedGroup;
                if (supAreaToGroup.TryGetValue(supAreaId, out mappedGroup)
                    && NormalizeAreaGroupId(mappedGroup).Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static HashSet<string> LoadItsUserIdsFromTrxInAreaGroup(string areaGroupId, string periode)
        {
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string requiredGroup = NormalizeAreaGroupId(areaGroupId);
            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return ids;
            }

            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            DateTime monthStart = new DateTime(periodDate.Year, periodDate.Month, 1);
            DataTable details = LoadTrxJobAssignDetailRows(monthStart, monthStart.AddMonths(1));
            if (details == null || details.Rows.Count == 0)
            {
                return ids;
            }

            Dictionary<string, string> areaIdAreaGroupMap = LoadAreaIdAreaGroupMap();
            foreach (DataRow row in details.Rows)
            {
                if (!PassesTrxAssignDashboardFilter(row, string.Empty, requiredGroup, areaIdAreaGroupMap))
                {
                    continue;
                }

                string technicianId = FirstNonEmptyStatic(
                    GetValue(row, "TechnicianID"),
                    GetValue(row, "TechnicianId"),
                    GetValue(row, "ITID")).Trim();
                if (string.IsNullOrWhiteSpace(technicianId))
                {
                    continue;
                }

                ids.Add(technicianId);
                ids.Add(TrimToLength(technicianId, 20));
                if (IsValidItId(technicianId))
                {
                    ids.Add(NormalizeItId(technicianId));
                }
            }

            return ids;
        }

        private static DataTable FilterItsAuthUsersByAreaGroup(
            DataTable allUsers,
            string filterAreaGroupId,
            string periode = "")
        {
            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                return allUsers ?? new DataTable();
            }

            string requiredGroup = NormalizeAreaGroupId(filterAreaGroupId);
            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return allUsers;
            }

            return FilterItsSupportAuthUsersByMstAreaGroup(allUsers, requiredGroup);
        }

        private static DataTable FilterItsSupportAuthUsersByMstAreaGroup(DataTable allUsers, string requiredGroup)
        {
            string requiredCanon = NormalizeAreaGroupId(requiredGroup);
            HashSet<string> allowedSupAreaIds = LoadSupAreaIdsForAreaGroup(requiredCanon);
            DataTable filtered = allUsers.Clone();
            EnsureItsAuthUserAreaColumns(filtered);

            foreach (DataRow row in allUsers.Rows)
            {
                if (ItSupportRowMatchesAreaGroupFilter(row, requiredCanon, allowedSupAreaIds))
                {
                    filtered.ImportRow(row);
                }
            }

            return filtered;
        }

        private static DataTable LoadItsAuthUsersFromCustomerItList()
        {
            DataTable combo = ExecuteJobTrainingQuery("sp_list_customer_it ''");
            if (combo == null || combo.Rows.Count == 0)
            {
                return new DataTable();
            }

            DataTable result = new DataTable();
            result.Columns.Add("UserID");
            result.Columns.Add("FullName");
            result.Columns.Add("GroupID");
            result.Columns.Add("Status");
            result.Columns.Add("ITID");
            result.Columns.Add("SupAreaID");
            result.Columns.Add("SupportAreaID");
            result.Columns.Add("AreaGroupID");
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in combo.Rows)
            {
                string userId = FirstNonEmptyStatic(
                    GetValue(row, "UserID"),
                    GetValue(row, "UsrID"),
                    GetValue(row, "Value"),
                    GetValue(row, "ID")).Trim();
                if (string.IsNullOrWhiteSpace(userId) || !seen.Add(userId))
                {
                    continue;
                }

                DataRow imported = result.NewRow();
                imported["UserID"] = userId;
                imported["FullName"] = FirstNonEmptyStatic(
                    GetValue(row, "FullName"),
                    GetValue(row, "Name"),
                    GetValue(row, "Text"),
                    userId);
                imported["GroupID"] = "ITS";
                imported["Status"] = "RG";
                imported["ITID"] = LookupItIdByUserId(userId);
                imported["SupAreaID"] = string.Empty;
                imported["SupportAreaID"] = string.Empty;
                imported["AreaGroupID"] = string.Empty;
                result.Rows.Add(imported);
            }

            return result;
        }

        private static string GetItSupportSupAreaSqlExpressionBasic()
        {
            return "LTRIM(RTRIM(ISNULL(it.SupAreaID, '')))";
        }

        private static string GetItSupportSupAreaSqlExpression()
        {
            return "LTRIM(RTRIM(COALESCE("
                + "NULLIF(LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))), ''), "
                + "NULLIF(LTRIM(RTRIM(ISNULL(it.SupportAreaID, ''))), ''))))";
        }

        private static string BuildItSupportUsersByAreaGroupSql(string areaGroupFilter = "", bool useBasicSupAreaExpr = false)
        {
            string supAreaExpr = useBasicSupAreaExpr
                ? GetItSupportSupAreaSqlExpressionBasic()
                : GetItSupportSupAreaSqlExpression();
            string requiredGroup = NormalizeAreaGroupId(areaGroupFilter);
            string selectCore = "SELECT "
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                + "'ITS' AS GroupID, "
                + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                + supAreaExpr + " AS SupAreaID, "
                + supAreaExpr + " AS SupportAreaID, "
                + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName ";

            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return selectCore
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "LEFT JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = it.SupAreaID "
                    + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY ISNULL(it.Name, it.UserID)";
            }

            string safeGroup = requiredGroup.Replace("'", "''");
            string westEastToken = requiredGroup.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase)
                ? "WEST"
                : (requiredGroup.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase) ? "EAST" : string.Empty);
            string namePredicate = string.IsNullOrWhiteSpace(westEastToken)
                ? string.Empty
                : "OR UPPER(LTRIM(RTRIM(ISNULL(ag.AreaGroupName, '')))) LIKE '%" + westEastToken + "%' ";

            return selectCore
                + "FROM mst_itsupport it WITH (NOLOCK) "
                + "INNER JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = it.SupAreaID "
                + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID "
                + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + "AND (LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + safeGroup + "' " + namePredicate + ") "
                + "ORDER BY ISNULL(it.Name, it.UserID)";
        }

        /// <summary>
        /// User-verified shape: mst_itsupport.SupAreaID -> ref_support_area -> ref_area_group.
        /// </summary>
        private static DataTable QueryItSupportUsersFullJoin()
        {
            string[] queries =
            {
                "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + "LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) AS SupportAreaID, "
                    + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                    + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "LEFT JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = it.SupAreaID "
                    + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY ISNULL(it.Name, it.UserID)",
                "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + "LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) AS SupportAreaID, "
                    + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                    + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "LEFT JOIN ref_support_area sa WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) = LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) "
                    + "LEFT JOIN ref_area_group ag WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) = LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY ISNULL(it.Name, it.UserID)"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteJobTrainingQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    EnrichItSupportUsersFromSupAreaMap(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static DataTable FilterItSupportUsersByJoinedAreaGroup(DataTable allUsers, string requiredGroup)
        {
            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                return allUsers ?? new DataTable();
            }

            string requiredCanon = NormalizeAreaGroupId(requiredGroup);
            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return allUsers;
            }

            HashSet<string> allowedSupAreaIds = LoadSupAreaIdsForAreaGroup(requiredCanon);
            DataTable filtered = allUsers.Clone();
            EnsureItsAuthUserAreaColumns(filtered);
            if (!filtered.Columns.Contains("AreaGroupName"))
            {
                filtered.Columns.Add("AreaGroupName", typeof(string));
            }

            foreach (DataRow row in allUsers.Rows)
            {
                string rowAreaGroupId = NormalizeAreaGroupId(GetRowValueInsensitive(row, "AreaGroupID"));
                if (!string.IsNullOrWhiteSpace(rowAreaGroupId)
                    && requiredCanon.Equals(rowAreaGroupId, StringComparison.OrdinalIgnoreCase))
                {
                    CopyItSupportUserRow(row, filtered);
                    continue;
                }

                if (ItSupportRowMatchesAreaGroupFilter(row, requiredCanon, allowedSupAreaIds))
                {
                    CopyItSupportUserRow(row, filtered);
                }
            }

            return filtered;
        }

        private static void CopyItSupportUserRow(DataRow source, DataTable target)
        {
            if (source == null || target == null)
            {
                return;
            }

            DataRow targetRow = target.NewRow();
            foreach (DataColumn column in source.Table.Columns)
            {
                if (!target.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                targetRow[column.ColumnName] = source[column.ColumnName];
            }

            target.Rows.Add(targetRow);
        }

        private static void MergeKnownSupAreaIdsForAreaGroup(string requiredCanon, HashSet<string> ids)
        {
            if (ids == null || string.IsNullOrWhiteSpace(requiredCanon))
            {
                return;
            }

            string[] knownIds = null;
            if (requiredCanon.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase))
            {
                knownIds = KnownWestSupAreaIds;
            }
            else if (requiredCanon.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase))
            {
                knownIds = KnownEastSupAreaIds;
            }

            if (knownIds == null)
            {
                return;
            }

            foreach (string supAreaId in knownIds)
            {
                if (!string.IsNullOrWhiteSpace(supAreaId))
                {
                    ids.Add(supAreaId.Trim());
                }
            }
        }

        private static DataTable QueryItSupportUsersBySupAreaIds(HashSet<string> supAreaIds)
        {
            if (supAreaIds == null || supAreaIds.Count == 0)
            {
                return new DataTable();
            }

            List<string> safeIds = new List<string>();
            foreach (string supAreaId in supAreaIds)
            {
                string trimmed = (supAreaId ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    safeIds.Add("'" + trimmed.Replace("'", "''") + "'");
                }
            }

            if (safeIds.Count == 0)
            {
                return new DataTable();
            }

            string inList = string.Join(",", safeIds);
            string supAreaExprBasic = GetItSupportSupAreaSqlExpressionBasic();
            string[] queries =
            {
                "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + supAreaExprBasic + " AS SupAreaID, "
                    + supAreaExprBasic + " AS SupportAreaID, "
                    + "'' AS AreaGroupID, "
                    + "'' AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "AND LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))) IN (" + inList + ") "
                    + "ORDER BY ISNULL(it.Name, it.UserID)",
                "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + supAreaExprBasic + " AS SupAreaID, "
                    + supAreaExprBasic + " AS SupportAreaID, "
                    + "'' AS AreaGroupID, "
                    + "'' AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "AND it.SupAreaID IN (" + inList + ") "
                    + "ORDER BY ISNULL(it.Name, it.UserID)"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteJobTrainingQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    EnrichItSupportUsersFromSupAreaMap(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static DataTable QueryItSupportUsersByAreaGroup(string areaGroupFilter = "")
        {
            string requiredGroup = NormalizeAreaGroupId(areaGroupFilter);
            string supAreaExpr = GetItSupportSupAreaSqlExpression();
            string supAreaExprBasic = GetItSupportSupAreaSqlExpressionBasic();
            List<string> queries = new List<string>
            {
                BuildItSupportUsersByAreaGroupSql(areaGroupFilter, true),
                BuildItSupportUsersByAreaGroupSql(areaGroupFilter, false)
            };

            if (!string.IsNullOrWhiteSpace(requiredGroup))
            {
                string safeGroup = requiredGroup.Replace("'", "''");
                string westEastToken = requiredGroup.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase)
                    ? "WEST"
                    : (requiredGroup.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase) ? "EAST" : string.Empty);
                string namePredicate = string.IsNullOrWhiteSpace(westEastToken)
                    ? string.Empty
                    : "OR UPPER(LTRIM(RTRIM(ISNULL(ag.AreaGroupName, '')))) LIKE '%" + westEastToken + "%' ";
                queries.Add(
                    "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + supAreaExpr + " AS SupAreaID, "
                    + supAreaExpr + " AS SupportAreaID, "
                    + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                    + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "INNER JOIN ref_support_area sa WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) = " + supAreaExpr + " "
                    + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "AND (LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + safeGroup + "' " + namePredicate + ") "
                    + "ORDER BY ISNULL(it.Name, it.UserID)");
                queries.Add(BuildItSupportMasterUsersSql(areaGroupFilter));
                queries.Add(
                    "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + supAreaExprBasic + " AS SupAreaID, "
                    + supAreaExprBasic + " AS SupportAreaID, "
                    + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                    + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "INNER JOIN ref_support_area sa WITH (NOLOCK) ON sa.SupAreaID = it.SupAreaID "
                    + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON ag.AreaGroupID = sa.AreaGroupID "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "AND LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + safeGroup + "' "
                    + "ORDER BY ISNULL(it.Name, it.UserID)");
            }
            else
            {
                queries.Add(
                    "SELECT "
                    + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                    + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                    + "'ITS' AS GroupID, "
                    + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                    + supAreaExprBasic + " AS SupAreaID, "
                    + supAreaExprBasic + " AS SupportAreaID, "
                    + "'' AS AreaGroupID, "
                    + "'' AS AreaGroupName "
                    + "FROM mst_itsupport it WITH (NOLOCK) "
                    + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY ISNULL(it.Name, it.UserID)");
            }

            foreach (string sql in queries)
            {
                DataTable table = ExecuteJobTrainingQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    EnrichItSupportUsersFromSupAreaMap(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static void EnrichItsAuthUsersFromMstItSupport(DataTable users)
        {
            if (users == null || users.Rows.Count == 0)
            {
                return;
            }

            DataTable mstUsers = QueryItSupportUsersByAreaGroup(string.Empty);
            if (mstUsers == null || mstUsers.Rows.Count == 0)
            {
                return;
            }

            EnsureItsAuthUserAreaColumns(users);
            if (!users.Columns.Contains("AreaGroupName"))
            {
                users.Columns.Add("AreaGroupName", typeof(string));
            }

            Dictionary<string, DataRow> byUserId = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, DataRow> byItId = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow mstRow in mstUsers.Rows)
            {
                string userId = GetRowValueInsensitive(mstRow, "UserID", "UsrID");
                string itId = GetRowValueInsensitive(mstRow, "ITID");
                if (!string.IsNullOrWhiteSpace(userId) && !byUserId.ContainsKey(userId))
                {
                    byUserId[userId] = mstRow;
                }

                if (!string.IsNullOrWhiteSpace(itId) && !byItId.ContainsKey(itId))
                {
                    byItId[itId] = mstRow;
                }
            }

            foreach (DataRow row in users.Rows)
            {
                string userId = GetRowValueInsensitive(row, "UserID", "UsrID");
                string itId = GetRowValueInsensitive(row, "ITID");
                DataRow mstRow;
                if ((!string.IsNullOrWhiteSpace(userId) && byUserId.TryGetValue(userId, out mstRow))
                    || (!string.IsNullOrWhiteSpace(itId) && byItId.TryGetValue(itId, out mstRow)))
                {
                    string supAreaId = GetRowValueInsensitive(mstRow, "SupAreaID", "SupportAreaID");
                    if (!string.IsNullOrWhiteSpace(supAreaId))
                    {
                        row["SupAreaID"] = supAreaId;
                        row["SupportAreaID"] = supAreaId;
                    }

                    string areaGroupId = GetRowValueInsensitive(mstRow, "AreaGroupID");
                    if (!string.IsNullOrWhiteSpace(areaGroupId))
                    {
                        row["AreaGroupID"] = areaGroupId;
                    }

                    string areaGroupName = GetRowValueInsensitive(mstRow, "AreaGroupName");
                    if (!string.IsNullOrWhiteSpace(areaGroupName))
                    {
                        row["AreaGroupName"] = areaGroupName;
                    }

                    string mstItId = GetRowValueInsensitive(mstRow, "ITID");
                    if (!string.IsNullOrWhiteSpace(mstItId))
                    {
                        row["ITID"] = mstItId;
                    }

                    string mstName = GetRowValueInsensitive(mstRow, "FullName", "Name");
                    if (!string.IsNullOrWhiteSpace(mstName))
                    {
                        row["FullName"] = mstName;
                    }
                }
            }
        }

        private static string BuildItSupportSupAreaExistsFilter(string requiredAreaGroupId)
        {
            string requiredGroup = NormalizeAreaGroupId(requiredAreaGroupId);
            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return string.Empty;
            }

            string safeGroup = requiredGroup.Replace("'", "''");
            string supAreaExpr = GetItSupportSupAreaSqlExpression();
            return "AND EXISTS (SELECT 1 FROM ref_support_area saf WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(saf.SupAreaID, ''))) = " + supAreaExpr + " "
                + "AND LTRIM(RTRIM(ISNULL(saf.AreaGroupID, ''))) = '" + safeGroup + "') ";
        }

        private static string BuildItSupportMasterUsersSql(string filterAreaGroupId = "")
        {
            string areaFilter = BuildItSupportSupAreaExistsFilter(filterAreaGroupId);
            string supAreaExpr = GetItSupportSupAreaSqlExpression();
            return "SELECT "
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                + "'ITS' AS GroupID, "
                + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                + supAreaExpr + " AS SupAreaID, "
                + supAreaExpr + " AS SupportAreaID, "
                + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                + "FROM mst_itsupport it WITH (NOLOCK) "
                + "LEFT JOIN ref_support_area sa WITH (NOLOCK) ON LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) = " + supAreaExpr + " "
                + "LEFT JOIN ref_area_group ag WITH (NOLOCK) ON LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) = LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) "
                + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + areaFilter
                + "ORDER BY ISNULL(it.Name, it.UserID)";
        }

        private static string BuildItSupportMasterUsersSqlMstOnly(string filterAreaGroupId = "")
        {
            string areaFilter = BuildItSupportSupAreaExistsFilter(filterAreaGroupId);
            string supAreaExpr = GetItSupportSupAreaSqlExpression();
            return "SELECT "
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                + "'ITS' AS GroupID, "
                + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                + supAreaExpr + " AS SupAreaID, "
                + supAreaExpr + " AS SupportAreaID, "
                + "'' AS AreaGroupID, "
                + "'' AS AreaGroupName "
                + "FROM mst_itsupport it WITH (NOLOCK) "
                + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + areaFilter
                + "ORDER BY ISNULL(it.Name, it.UserID)";
        }

        private static HashSet<string> LoadSupAreaIdsForAreaGroup(string requiredAreaGroupId)
        {
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string requiredGroup = NormalizeAreaGroupId(requiredAreaGroupId);
            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return ids;
            }

            string safeGroup = requiredGroup.Replace("'", "''");
            string[] queries =
            {
                "SELECT LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) AS SupAreaID "
                    + "FROM ref_support_area sa WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + safeGroup + "' "
                    + "AND LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) <> ''",
                "SELECT LTRIM(RTRIM(ISNULL(SupAreaID, ''))) AS SupAreaID "
                    + "FROM ref_support_area WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(AreaGroupID, ''))) = '" + safeGroup + "' "
                    + "AND LTRIM(RTRIM(ISNULL(SupAreaID, ''))) <> ''"
            };

            foreach (string sql in queries)
            {
                DataTable rows = ExecuteJobTrainingQuery(sql);
                if (rows == null || rows.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in rows.Rows)
                {
                    string supAreaId = GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID").Trim();
                    if (!string.IsNullOrWhiteSpace(supAreaId))
                    {
                        ids.Add(supAreaId);
                    }
                }

                if (ids.Count > 0)
                {
                    break;
                }
            }

            if (ids.Count == 0)
            {
                DataTable spAreas = ExecuteJobTrainingQuery("sp_list_support_area ''");
                if (spAreas != null && spAreas.Rows.Count > 0)
                {
                    foreach (DataRow row in spAreas.Rows)
                    {
                        string rowGroup = FirstNonEmptyStatic(
                            GetRowValueInsensitive(row, "AreaGroupID", "GroupSupportAreaID", "GroupID"));
                        if (!NormalizeAreaGroupId(rowGroup).Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        string supAreaId = GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID").Trim();
                        if (!string.IsNullOrWhiteSpace(supAreaId))
                        {
                            ids.Add(supAreaId);
                        }
                    }
                }
            }

            MergeKnownSupAreaIdsForAreaGroup(requiredGroup, ids);

            if (ids.Count == 0)
            {
                Dictionary<string, string> supAreaToGroup = ItsSupportAssignData.LoadSupAreaToAreaGroupMap();
                Dictionary<string, string> areaGroupNames = ItsSupportAssignData.LoadAreaGroupNameMap();
                foreach (KeyValuePair<string, string> pair in supAreaToGroup)
                {
                    string mappedName;
                    areaGroupNames.TryGetValue(pair.Value, out mappedName);
                    if (ItsSupportAssignData.ResolveCanonicalAreaGroupId(pair.Value, mappedName)
                        .Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
                    {
                        ids.Add(pair.Key.Trim());
                    }
                }
            }

            return ids;
        }

        private static string GetRowValueInsensitive(DataRow row, params string[] columnNames)
        {
            if (row == null || row.Table == null || columnNames == null || columnNames.Length == 0)
            {
                return string.Empty;
            }

            foreach (string columnName in columnNames)
            {
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    continue;
                }

                foreach (DataColumn column in row.Table.Columns)
                {
                    if (!column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    object value = row[column];
                    return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
                }
            }

            return string.Empty;
        }

        private static void EnrichItSupportUsersFromSupAreaMap(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            EnsureItsAuthUserAreaColumns(table);
            if (!table.Columns.Contains("AreaGroupName"))
            {
                table.Columns.Add("AreaGroupName", typeof(string));
            }

            Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();
            Dictionary<string, string> areaGroupNames = LoadAreaGroupNameMap();
            foreach (DataRow row in table.Rows)
            {
                string areaGroupId = GetRowValueInsensitive(row, "AreaGroupID");
                if (!string.IsNullOrWhiteSpace(areaGroupId))
                {
                    continue;
                }

                string supAreaId = FirstNonEmptyStatic(
                    GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID")).Trim();
                if (string.IsNullOrWhiteSpace(supAreaId)
                    || supAreaId.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                    || supAreaId.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string mappedGroup;
                if (!supAreaToGroup.TryGetValue(supAreaId, out mappedGroup)
                    || string.IsNullOrWhiteSpace(mappedGroup))
                {
                    continue;
                }

                row["AreaGroupID"] = mappedGroup.Trim();
                string mappedName;
                if (areaGroupNames.TryGetValue(mappedGroup.Trim(), out mappedName)
                    && !string.IsNullOrWhiteSpace(mappedName))
                {
                    row["AreaGroupName"] = mappedName.Trim();
                }
            }
        }

        private static string ResolveItSupportRowAreaGroup(DataRow row)
        {
            if (row == null)
            {
                return string.Empty;
            }

            string areaGroupId = NormalizeAreaGroupId(GetRowValueInsensitive(row, "AreaGroupID"));
            if (!string.IsNullOrWhiteSpace(areaGroupId))
            {
                return areaGroupId;
            }

            string areaGroupName = GetRowValueInsensitive(row, "AreaGroupName");
            if (areaGroupName.IndexOf("WEST", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return AreaGroupWestValue;
            }

            if (areaGroupName.IndexOf("EAST", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return AreaGroupEastValue;
            }

            string supAreaId = FirstNonEmptyStatic(
                GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID")).Trim();
            if (string.IsNullOrWhiteSpace(supAreaId))
            {
                return string.Empty;
            }

            Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();
            string mappedGroup;
            if (supAreaToGroup.TryGetValue(supAreaId, out mappedGroup))
            {
                return NormalizeAreaGroupId(mappedGroup);
            }

            return string.Empty;
        }

        private static bool ItSupportRowMatchesAreaGroupFilter(
            DataRow row,
            string requiredGroup,
            HashSet<string> allowedSupAreaIds = null)
        {
            string requiredCanon = NormalizeAreaGroupId(requiredGroup);
            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return true;
            }

            if (row == null)
            {
                return false;
            }

            string rowGroup = ResolveItSupportRowAreaGroup(row);
            if (requiredCanon.Equals(rowGroup, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (allowedSupAreaIds == null)
            {
                allowedSupAreaIds = LoadSupAreaIdsForAreaGroup(requiredCanon);
            }

            string supAreaId = FirstNonEmptyStatic(
                GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID")).Trim();
            if (supAreaId.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                || supAreaId.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                supAreaId = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(supAreaId)
                && allowedSupAreaIds != null
                && allowedSupAreaIds.Count > 0
                && allowedSupAreaIds.Contains(supAreaId))
            {
                return true;
            }

            return false;
        }

        private static DataTable QueryItSupportMasterUsersUnfiltered()
        {
            string[] queries =
            {
                BuildItSupportUsersByAreaGroupSql(string.Empty),
                BuildItSupportMasterUsersSql(string.Empty),
                BuildItSupportMasterUsersSqlMstOnly(string.Empty)
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteJobTrainingQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    EnrichItSupportUsersFromSupAreaMap(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static DataTable FilterItSupportMasterUsersBySupArea(
            DataTable allUsers,
            string requiredGroup)
        {
            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                return allUsers ?? new DataTable();
            }

            string requiredCanon = NormalizeAreaGroupId(requiredGroup);
            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return allUsers;
            }

            HashSet<string> allowedSupAreaIds = LoadSupAreaIdsForAreaGroup(requiredCanon);
            DataTable filtered = allUsers.Clone();
            EnsureItsAuthUserAreaColumns(filtered);
            if (!filtered.Columns.Contains("AreaGroupName"))
            {
                filtered.Columns.Add("AreaGroupName", typeof(string));
            }

            foreach (DataRow row in allUsers.Rows)
            {
                if (ItSupportRowMatchesAreaGroupFilter(row, requiredCanon, allowedSupAreaIds))
                {
                    filtered.ImportRow(row);
                }
            }

            return filtered;
        }

        private static DataTable LoadItsAuthUsersFromMaster(string filterAreaGroupId = "")
        {
            string requiredGroup = NormalizeAreaGroupId(filterAreaGroupId);
            DataTable direct = QueryItSupportUsersByAreaGroup(requiredGroup);
            if (direct != null && direct.Rows.Count > 0)
            {
                return direct;
            }

            DataTable allUsers = QueryItSupportUsersByAreaGroup(string.Empty);
            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                allUsers = QueryItSupportMasterUsersUnfiltered();
            }

            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                return new DataTable();
            }

            return FilterItSupportMasterUsersBySupArea(allUsers, filterAreaGroupId);
        }

        private static DataTable LoadItsAuthUsersAll()
        {
            DataTable fromMaster = LoadItsAuthUsersFromMaster(string.Empty);
            if (fromMaster != null && fromMaster.Rows.Count > 0)
            {
                return fromMaster;
            }

            DataTable direct = ExecuteJobTrainingQuery(
                "SELECT a.UserID, ISNULL(m.FullName, a.UserID) AS FullName, a.GroupID, a.Status, m.ITID "
                + "FROM conf_auth_user a WITH (NOLOCK) "
                + "LEFT JOIN conf_mst_user m WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(a.UserID, ''))) = LTRIM(RTRIM(ISNULL(m.UserID, ''))) "
                + "WHERE UPPER(LTRIM(RTRIM(ISNULL(a.GroupID, '')))) = 'ITS' "
                + "AND ISNULL(a.Status, '') <> 'DE' "
                + "ORDER BY ISNULL(m.FullName, a.UserID)");
            if (direct != null && direct.Rows.Count > 0)
            {
                EnsureItsAuthUserAreaColumns(direct);
                EnrichItsAuthUsersFromMstItSupport(direct);
                return direct;
            }

            DataTable customerItUsers = LoadItsAuthUsersFromCustomerItList();
            if (customerItUsers != null && customerItUsers.Rows.Count > 0)
            {
                EnrichItsAuthUsersFromMstItSupport(customerItUsers);
                return customerItUsers;
            }

            DataTable authList = ExecuteJobTrainingQuery("sp_list_user_authentication ''");
            if (authList == null || authList.Rows.Count == 0)
            {
                return new DataTable();
            }

            DataTable filtered = authList.Clone();
            EnsureItsAuthUserAreaColumns(filtered);
            if (!filtered.Columns.Contains("ITID"))
            {
                filtered.Columns.Add("ITID", typeof(string));
            }
            if (!filtered.Columns.Contains("FullName"))
            {
                filtered.Columns.Add("FullName", typeof(string));
            }
            foreach (DataRow row in authList.Rows)
            {
                string groupId = FirstNonEmptyStatic(GetValue(row, "GroupID"), GetValue(row, "GroupId")).Trim().ToUpperInvariant();
                string status = FirstNonEmptyStatic(GetValue(row, "Status")).Trim().ToUpperInvariant();
                if (groupId != "ITS" || status == "DE")
                {
                    continue;
                }

                DataRow imported = filtered.NewRow();
                foreach (DataColumn column in authList.Columns)
                {
                    if (filtered.Columns.Contains(column.ColumnName))
                    {
                        imported[column.ColumnName] = row[column.ColumnName];
                    }
                }
                string userId = FirstNonEmptyStatic(GetValue(imported, "UserID"), GetValue(imported, "UsrID"));
                imported["ITID"] = LookupItIdByUserId(userId);

                DataTable mst = ExecuteJobTrainingQuery(
                    "SELECT TOP 1 FullName, ITID, "
                    + "LTRIM(RTRIM(ISNULL(SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(SupportAreaID, ''))) AS SupportAreaID, "
                    + "LTRIM(RTRIM(ISNULL(AreaGroupID, ''))) AS AreaGroupID "
                    + "FROM conf_mst_user WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + EscapeSqlLiteral(userId) + "'");
                if (mst != null && mst.Rows.Count > 0)
                {
                    imported["FullName"] = FirstNonEmptyStatic(GetValue(mst.Rows[0], "FullName"), userId);
                    string itId = FirstNonEmptyStatic(GetValue(mst.Rows[0], "ITID"));
                    if (!string.IsNullOrWhiteSpace(itId))
                    {
                        imported["ITID"] = itId;
                    }

                    imported["SupAreaID"] = GetValue(mst.Rows[0], "SupAreaID");
                    imported["SupportAreaID"] = GetValue(mst.Rows[0], "SupportAreaID");
                    imported["AreaGroupID"] = GetValue(mst.Rows[0], "AreaGroupID");
                }
                else if (string.IsNullOrWhiteSpace(GetValue(imported, "FullName")))
                {
                    imported["FullName"] = userId;
                }

                filtered.Rows.Add(imported);
            }

            EnrichItsAuthUsersFromMstItSupport(filtered);
            return filtered;
        }

        private static DataTable TryLoadItSupportUsersForAreaGroup(string requiredGroup)
        {
            if (string.IsNullOrWhiteSpace(requiredGroup))
            {
                return new DataTable();
            }

            DataTable joinedUsers = QueryItSupportUsersFullJoin();
            if (joinedUsers != null && joinedUsers.Rows.Count > 0)
            {
                DataTable joinedFiltered = FilterItSupportUsersByJoinedAreaGroup(joinedUsers, requiredGroup);
                if (joinedFiltered != null && joinedFiltered.Rows.Count > 0)
                {
                    return joinedFiltered;
                }
            }

            HashSet<string> supAreaIds = LoadSupAreaIdsForAreaGroup(requiredGroup);
            DataTable bySupArea = QueryItSupportUsersBySupAreaIds(supAreaIds);
            if (bySupArea != null && bySupArea.Rows.Count > 0)
            {
                return bySupArea;
            }

            return new DataTable();
        }

        private static DataTable LoadItsAuthUsers(string filterAreaGroupId = "", string periode = "")
        {
            string requiredGroup = NormalizeAreaGroupId(filterAreaGroupId);

            DataTable joinedUsers = QueryItSupportUsersFullJoin();
            if (joinedUsers != null && joinedUsers.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(requiredGroup))
                {
                    return joinedUsers;
                }

                DataTable areaUsers = TryLoadItSupportUsersForAreaGroup(requiredGroup);
                if (areaUsers != null && areaUsers.Rows.Count > 0)
                {
                    return areaUsers;
                }
            }
            else if (!string.IsNullOrWhiteSpace(requiredGroup))
            {
                DataTable areaUsers = TryLoadItSupportUsersForAreaGroup(requiredGroup);
                if (areaUsers != null && areaUsers.Rows.Count > 0)
                {
                    return areaUsers;
                }
            }

            DataTable fromMaster = QueryItSupportUsersByAreaGroup(requiredGroup);
            if (fromMaster != null && fromMaster.Rows.Count > 0)
            {
                return fromMaster;
            }

            if (!string.IsNullOrWhiteSpace(requiredGroup))
            {
                DataTable allMst = joinedUsers;
                if (allMst == null || allMst.Rows.Count == 0)
                {
                    allMst = QueryItSupportUsersByAreaGroup(string.Empty);
                }

                if (allMst != null && allMst.Rows.Count > 0)
                {
                    DataTable filtered = FilterItSupportUsersByJoinedAreaGroup(allMst, requiredGroup);
                    if (filtered == null || filtered.Rows.Count == 0)
                    {
                        filtered = FilterItSupportMasterUsersBySupArea(allMst, requiredGroup);
                    }

                    if (filtered != null && filtered.Rows.Count > 0)
                    {
                        return filtered;
                    }
                }

                DataTable authUsers = LoadItsAuthUsersAll();
                EnrichItsAuthUsersFromMstItSupport(authUsers);
                return FilterItsSupportAuthUsersByMstAreaGroup(authUsers, requiredGroup);
            }

            DataTable semua = joinedUsers;
            if (semua == null || semua.Rows.Count == 0)
            {
                semua = LoadItsAuthUsersAll();
            }

            return semua ?? new DataTable();
        }

        private sealed class JobTrainingTrainerSchedule
        {
            public string TrainerId { get; set; }
            public string TrainerName { get; set; }
            public string ItId { get; set; }
            public string SupAreaID { get; set; }
            public string AreaGroupID { get; set; }
            public int[] DayJobCount { get; set; }
            public int[] DayOpenCount { get; set; }
            public int ClosedTraining { get; set; }
            public int ClosedVisit { get; set; }
        }

        private static DataTable CreateJobTrainingAvailabilitySchema()
        {
            DataTable table = new DataTable();
            table.Columns.Add("TechnicianID");
            table.Columns.Add("Name");
            table.Columns.Add("DayNo", typeof(int));
            table.Columns.Add("DayName");
            table.Columns.Add("IsWeekend");
            table.Columns.Add("DisplayValue");
            table.Columns.Add("TotalJob", typeof(int));
            table.Columns.Add("RemainingJo", typeof(int));
            table.Columns.Add("TotalJobCloseNew", typeof(int));
            table.Columns.Add("TotalJobCloseMaint", typeof(int));
            table.Columns.Add("TotalJobCloseUnit", typeof(int));
            table.Columns.Add("IsAvailable");
            table.Columns.Add("SupAreaID");
            table.Columns.Add("ITID");
            return table;
        }

        private static DataTable LoadJobTrainingRowsForSchedule(string periode)
        {
            DateTime periodDate;
            if (!DateTime.TryParseExact((periode ?? string.Empty).Trim() + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodDate)
                && !DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            string dateFrom = new DateTime(periodDate.Year, periodDate.Month, 1).ToString("yyyy-MM-dd");
            string dateTo = new DateTime(periodDate.Year, periodDate.Month, DateTime.DaysInMonth(periodDate.Year, periodDate.Month)).ToString("yyyy-MM-dd");

            DataTable headerRows = new DataTable();
            try
            {
                headerRows = LoadJobTrainingHeaderTable(string.Empty) ?? new DataTable();
            }
            catch
            {
                headerRows = new DataTable();
            }

            DataTable viewRows = ExecuteJobTrainingQuery(
                "sp_view_job_training '','','" + dateFrom.Replace("'", "''") + "','" + dateTo.Replace("'", "''") + "'");

            // Prefer the same list as job_training.aspx, then enrich Trainers from view SP.
            if (headerRows.Rows.Count > 0)
            {
                MergeJobTrainingTrainerColumns(headerRows, viewRows);
                EnrichJobTrainingWithCustomerContext(headerRows);
                return headerRows;
            }

            if (viewRows != null && viewRows.Rows.Count > 0)
            {
                EnrichJobTrainingWithCustomerContext(viewRows);
                return viewRows;
            }

            EnrichJobTrainingWithCustomerContext(headerRows);
            return headerRows;
        }

        protected static void EnrichJobTrainingWithCustomerContext(DataTable jobs)
        {
            if (jobs == null || jobs.Rows.Count == 0)
            {
                return;
            }

            if (!jobs.Columns.Contains("SupAreaID"))
            {
                jobs.Columns.Add("SupAreaID", typeof(string));
            }

            if (!jobs.Columns.Contains("AreaGroupID"))
            {
                jobs.Columns.Add("AreaGroupID", typeof(string));
            }

            if (!jobs.Columns.Contains("MarketingName"))
            {
                jobs.Columns.Add("MarketingName", typeof(string));
            }

            Dictionary<string, CustomerScheduleContext> customers = LoadCustomerScheduleContextMap();
            Dictionary<string, string> areaGroupBySupArea = LoadSupAreaAreaGroupMap();
            if (customers.Count == 0 && areaGroupBySupArea.Count == 0)
            {
                return;
            }

            foreach (DataRow row in jobs.Rows)
            {
                string custId = GetValue(row, "CustID").Trim();
                CustomerScheduleContext context = null;
                if (!string.IsNullOrWhiteSpace(custId))
                {
                    customers.TryGetValue(custId, out context);
                }

                if (context != null)
                {
                    if (string.IsNullOrWhiteSpace(GetValue(row, "SupAreaID"))
                        && !string.IsNullOrWhiteSpace(context.SupAreaID))
                    {
                        row["SupAreaID"] = context.SupAreaID;
                    }

                    if (string.IsNullOrWhiteSpace(GetValue(row, "MarketingName"))
                        && !string.IsNullOrWhiteSpace(context.MarketingName))
                    {
                        row["MarketingName"] = context.MarketingName;
                    }
                }

                string supAreaId = FirstNonEmptyStatic(
                    GetValue(row, "SupAreaID"),
                    context == null ? string.Empty : context.SupAreaID).Trim();
                if (string.IsNullOrWhiteSpace(GetValue(row, "AreaGroupID")) && !string.IsNullOrWhiteSpace(supAreaId))
                {
                    string areaGroupId;
                    if (areaGroupBySupArea.TryGetValue(supAreaId, out areaGroupId)
                        && !string.IsNullOrWhiteSpace(areaGroupId))
                    {
                        row["AreaGroupID"] = areaGroupId;
                    }
                }
            }
        }

        protected sealed class CustomerScheduleContext
        {
            public string SupAreaID { get; set; }
            public string Address { get; set; }
            public string BranchAddress { get; set; }
            public string MarketingName { get; set; }
        }

        protected static Dictionary<string, CustomerScheduleContext> LoadCustomerScheduleContextMap()
        {
            Dictionary<string, CustomerScheduleContext> map =
                new Dictionary<string, CustomerScheduleContext>(StringComparer.OrdinalIgnoreCase);

            DataTable customers = ExecuteJobTrainingQuery(
                "SELECT c.CustID, "
                + "LTRIM(RTRIM(ISNULL(c.SupAreaID, ''))) AS SupAreaID, "
                + "LTRIM(RTRIM(ISNULL(c.Address, ''))) AS Address, "
                + "LTRIM(RTRIM(ISNULL(c.BranchAddress, ''))) AS BranchAddress, "
                + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                + "FROM mst_customer c WITH (NOLOCK) "
                + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, '')))");
            if (customers == null || customers.Rows.Count == 0)
            {
                customers = ExecuteJobTrainingQuery(
                    "SELECT c.CustID, "
                    + "LTRIM(RTRIM(ISNULL(c.SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(c.Address, ''))) AS Address, "
                    + "LTRIM(RTRIM(ISNULL(c.BranchAddress, ''))) AS BranchAddress, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) ON m.MarketingID = c.MarketingID");
            }
            if (customers == null || customers.Rows.Count == 0)
            {
                customers = ExecuteJobTrainingQuery(
                    "SELECT CustID, "
                    + "LTRIM(RTRIM(ISNULL(SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(Address, ''))) AS Address, "
                    + "LTRIM(RTRIM(ISNULL(BranchAddress, ''))) AS BranchAddress "
                    + "FROM mst_customer WITH (NOLOCK)");
            }
            if (customers == null || customers.Rows.Count == 0)
            {
                customers = ExecuteJobTrainingQuery(
                    "SELECT CustID, "
                    + "LTRIM(RTRIM(ISNULL(SupportAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(Address, ''))) AS Address, "
                    + "LTRIM(RTRIM(ISNULL(BranchAddress, ''))) AS BranchAddress "
                    + "FROM mst_customer WITH (NOLOCK)");
            }

            if (customers == null || customers.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in customers.Rows)
            {
                string custId = GetValue(row, "CustID").Trim();
                if (string.IsNullOrWhiteSpace(custId) || map.ContainsKey(custId))
                {
                    continue;
                }

                string supAreaId = FirstNonEmptyStatic(GetValue(row, "SupAreaID"), GetValue(row, "SupportAreaID")).Trim();
                if (supAreaId.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                    || supAreaId.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    supAreaId = string.Empty;
                }

                map[custId] = new CustomerScheduleContext
                {
                    SupAreaID = supAreaId,
                    Address = FirstNonEmptyStatic(GetValue(row, "Address"), GetValue(row, "CustAddress")).Trim(),
                    BranchAddress = GetValue(row, "BranchAddress").Trim(),
                    MarketingName = GetValue(row, "MarketingName").Trim()
                };
            }

            return map;
        }

        private static Dictionary<string, string> LoadSupAreaAreaGroupMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] queries =
            {
                "SELECT "
                    + "LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) AS SupAreaID, "
                    + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID "
                    + "FROM ref_support_area sa WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) <> '' "
                    + "AND LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) <> ''",
                "sp_list_support_area ''"
            };

            foreach (string sql in queries)
            {
                DataTable areas = ExecuteJobTrainingQuery(sql);
                if (areas == null || areas.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in areas.Rows)
                {
                    string supAreaId = FirstNonEmptyStatic(
                        GetRowValueInsensitive(row, "SupAreaID", "SupportAreaID")).Trim();
                    string areaGroupId = FirstNonEmptyStatic(
                        GetRowValueInsensitive(row, "AreaGroupID", "GroupSupportAreaID", "GroupID")).Trim();
                    if (string.IsNullOrWhiteSpace(supAreaId)
                        || string.IsNullOrWhiteSpace(areaGroupId)
                        || map.ContainsKey(supAreaId))
                    {
                        continue;
                    }

                    map[supAreaId] = areaGroupId;
                }

                if (map.Count > 0)
                {
                    break;
                }
            }

            if (map.Count == 0)
            {
                Dictionary<string, string> fallback = ItsSupportAssignData.LoadSupAreaToAreaGroupMap();
                foreach (KeyValuePair<string, string> pair in fallback)
                {
                    if (!map.ContainsKey(pair.Key))
                    {
                        map[pair.Key] = pair.Value;
                    }
                }
            }

            return map;
        }

        private static Dictionary<string, string> LoadAreaGroupNameMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            DataTable groups = ExecuteJobTrainingQuery(
                "SELECT "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) AS AreaGroupID, "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                + "FROM ref_area_group ag WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) <> ''");
            if (groups == null || groups.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in groups.Rows)
            {
                string areaGroupId = GetRowValueInsensitive(row, "AreaGroupID");
                string areaGroupName = GetRowValueInsensitive(row, "AreaGroupName");
                if (string.IsNullOrWhiteSpace(areaGroupId) || map.ContainsKey(areaGroupId))
                {
                    continue;
                }

                map[areaGroupId] = areaGroupName;
            }

            return map;
        }

        private static bool PassesJobTrainingDashboardFilter(DataRow row, string filterSupAreaId, string filterAreaGroupId)
        {
            if (row == null)
            {
                return false;
            }

            string requiredSupArea = (filterSupAreaId ?? string.Empty).Trim();
            if (requiredSupArea.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                requiredSupArea = string.Empty;
            }

            string requiredGroup = NormalizeAreaGroupId(filterAreaGroupId);

            string rowSupArea = FirstNonEmptyStatic(
                GetValue(row, "SupAreaID"),
                GetValue(row, "AreaID"),
                GetValue(row, "SupportAreaID")).Trim();
            string rowAreaGroup = NormalizeAreaGroupId(GetValue(row, "AreaGroupID"));
            if (string.IsNullOrWhiteSpace(rowAreaGroup) && !string.IsNullOrWhiteSpace(rowSupArea))
            {
                Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();
                string derivedGroup;
                if (supAreaToGroup.TryGetValue(rowSupArea, out derivedGroup))
                {
                    rowAreaGroup = NormalizeAreaGroupId(derivedGroup);
                }
            }

            if (!string.IsNullOrWhiteSpace(requiredGroup)
                && !rowAreaGroup.Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(requiredSupArea)
                && !rowSupArea.Equals(requiredSupArea, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static Dictionary<string, string> LoadAreaIdAreaGroupMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();

            string[] queries =
            {
                "sp_list_ref_area_by_group_area ''",
                "sp_list_area_selectbox ''"
            };

            foreach (string query in queries)
            {
                DataTable areas = ExecuteJobTrainingQuery(query);
                if (areas == null || areas.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in areas.Rows)
                {
                    string areaId = FirstNonEmptyStatic(GetValue(row, "AreaID"), GetValue(row, "AreaId")).Trim();
                    if (string.IsNullOrWhiteSpace(areaId) || map.ContainsKey(areaId))
                    {
                        continue;
                    }

                    string areaGroupId = NormalizeAreaGroupId(FirstNonEmptyStatic(
                        GetValue(row, "AreaGroupID"),
                        GetValue(row, "GroupID"),
                        GetValue(row, "GroupAreaID")));
                    if (string.IsNullOrWhiteSpace(areaGroupId))
                    {
                        string supAreaId = FirstNonEmptyStatic(
                            GetValue(row, "SupAreaID"),
                            GetValue(row, "SupportAreaID")).Trim();
                        if (!string.IsNullOrWhiteSpace(supAreaId)
                            && supAreaToGroup.TryGetValue(supAreaId, out areaGroupId))
                        {
                            areaGroupId = NormalizeAreaGroupId(areaGroupId);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(areaGroupId))
                    {
                        map[areaId] = areaGroupId;
                    }
                }

                if (map.Count > 0)
                {
                    break;
                }
            }

            return map;
        }

        private static bool PassesTrxAssignDashboardFilter(
            DataRow row,
            string filterSupAreaId,
            string filterAreaGroupId,
            Dictionary<string, string> areaIdAreaGroupMap)
        {
            if (row == null)
            {
                return false;
            }

            string requiredSupArea = (filterSupAreaId ?? string.Empty).Trim();
            if (requiredSupArea.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                requiredSupArea = string.Empty;
            }

            string requiredGroup = NormalizeAreaGroupId(filterAreaGroupId);
            if (string.IsNullOrWhiteSpace(requiredGroup) && string.IsNullOrWhiteSpace(requiredSupArea))
            {
                return true;
            }

            string rowAreaId = FirstNonEmptyStatic(
                GetValue(row, "AreaID"),
                GetValue(row, "AreaId"),
                GetValue(row, "SupAreaID"),
                GetValue(row, "SupportAreaID")).Trim();
            string rowSupArea = FirstNonEmptyStatic(
                GetValue(row, "SupAreaID"),
                GetValue(row, "SupportAreaID"),
                rowAreaId).Trim();
            string rowAreaGroup = NormalizeAreaGroupId(GetValue(row, "AreaGroupID"));
            if (string.IsNullOrWhiteSpace(rowAreaGroup)
                && areaIdAreaGroupMap != null
                && !string.IsNullOrWhiteSpace(rowAreaId)
                && areaIdAreaGroupMap.TryGetValue(rowAreaId, out rowAreaGroup))
            {
                rowAreaGroup = NormalizeAreaGroupId(rowAreaGroup);
            }

            if (string.IsNullOrWhiteSpace(rowAreaGroup) && !string.IsNullOrWhiteSpace(rowSupArea))
            {
                Dictionary<string, string> supAreaToGroup = LoadSupAreaAreaGroupMap();
                string derivedGroup;
                if (supAreaToGroup.TryGetValue(rowSupArea, out derivedGroup))
                {
                    rowAreaGroup = NormalizeAreaGroupId(derivedGroup);
                }
            }

            if (!string.IsNullOrWhiteSpace(requiredGroup)
                && !rowAreaGroup.Equals(requiredGroup, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(requiredSupArea)
                && !rowSupArea.Equals(requiredSupArea, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static string NormalizeItOutboundValue(string raw)
        {
            string value = (raw ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(value)
                || value.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                || value.Equals("-", StringComparison.OrdinalIgnoreCase)
                || value.Equals("null", StringComparison.OrdinalIgnoreCase)
                || value.Equals("&nbsp;", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return value;
        }

        protected static void MergeJobTrainingTrainerColumns(DataTable headerRows, DataTable viewRows)
        {
            if (headerRows == null || viewRows == null || viewRows.Rows.Count == 0)
            {
                return;
            }

            string[] enrichColumns = { "Trainers", "Trainer", "ITStaff", "ITName", "MarketingName", "Marketing" };
            foreach (string columnName in enrichColumns)
            {
                if (!headerRows.Columns.Contains(columnName) && viewRows.Columns.Contains(columnName))
                {
                    headerRows.Columns.Add(columnName, typeof(string));
                }
            }

            if (!headerRows.Columns.Contains("TrainingID") && !headerRows.Columns.Contains("JobID"))
            {
                return;
            }

            Dictionary<string, DataRow> viewById = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow viewRow in viewRows.Rows)
            {
                string id = FirstNonEmptyStatic(GetValue(viewRow, "TrainingID"), GetValue(viewRow, "JobID"));
                if (!string.IsNullOrWhiteSpace(id) && !viewById.ContainsKey(id))
                {
                    viewById[id] = viewRow;
                }
            }

            foreach (DataRow headerRow in headerRows.Rows)
            {
                string id = FirstNonEmptyStatic(GetValue(headerRow, "TrainingID"), GetValue(headerRow, "JobID"));
                DataRow viewRow;
                if (string.IsNullOrWhiteSpace(id) || !viewById.TryGetValue(id, out viewRow))
                {
                    continue;
                }

                foreach (string columnName in enrichColumns)
                {
                    if (!headerRows.Columns.Contains(columnName) || !viewRows.Columns.Contains(columnName))
                    {
                        continue;
                    }

                    string existing = GetValue(headerRow, columnName);
                    if (!string.IsNullOrWhiteSpace(existing))
                    {
                        continue;
                    }

                    string fromView = GetValue(viewRow, columnName);
                    if (!string.IsNullOrWhiteSpace(fromView))
                    {
                        headerRow[columnName] = fromView;
                    }
                }
            }
        }

        private static bool IsStoredProcedureCall(string sql)
        {
            string trimmed = (sql ?? string.Empty).Trim();
            if (trimmed.Length == 0)
            {
                return false;
            }

            string firstToken = trimmed.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
            return firstToken.StartsWith("sp_", StringComparison.OrdinalIgnoreCase)
                || firstToken.StartsWith("exec", StringComparison.OrdinalIgnoreCase);
        }

        private static string ResolveJobTrainingSqlConnectionString(string rawConnectionString)
        {
            if (string.IsNullOrWhiteSpace(rawConnectionString))
            {
                return string.Empty;
            }

            try
            {
                string[] parts = rawConnectionString.Split(';');
                StringBuilder sanitized = new StringBuilder();
                foreach (string item in parts)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    if (item.TrimStart().StartsWith("Provider=", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    sanitized.Append(item).Append(';');
                }

                return new SqlConnectionStringBuilder(sanitized.ToString()).ConnectionString;
            }
            catch
            {
                return rawConnectionString.Trim();
            }
        }

        private static DataTable ExecuteAdHocSqlQuery(string sql, string connString)
        {
            DataTable table = new DataTable();
            string sqlConn = ResolveJobTrainingSqlConnectionString(connString);
            if (string.IsNullOrWhiteSpace(sqlConn))
            {
                return table;
            }

            using (SqlConnection conn = new SqlConnection(sqlConn))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }

            return table;
        }

        public static DataTable ExecuteJobTrainingQuery(string sql)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null || context.Session["ClsTypeDBConnStringSQL"] == null)
            {
                return new DataTable();
            }

            return ExecuteJobTrainingQuery(sql, Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]));
        }

        public static DataTable ExecuteJobTrainingQuery(string sql, string connString)
        {
            if (string.IsNullOrWhiteSpace(connString) || string.IsNullOrWhiteSpace(sql))
            {
                return new DataTable();
            }

            try
            {
                if (!IsStoredProcedureCall(sql))
                {
                    return ExecuteAdHocSqlQuery(sql, connString);
                }

                string openError = string.Empty;
                Recordset rec = new Recordset();
                rec.Open(sql, connString.Trim(), ref openError);
                if (!string.IsNullOrWhiteSpace(openError))
                {
                    return new DataTable();
                }

                return rec.DataRecord() ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }

        private static List<string> SplitTrainerNames(string trainers)
        {
            List<string> names = new List<string>();
            if (string.IsNullOrWhiteSpace(trainers))
            {
                return names;
            }

            string normalized = trainers.Replace("&nbsp;", " ").Trim();
            string[] parts = normalized.Split(new[] { ',', '/', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string part in parts)
            {
                string name = (part ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name)
                    || name.Equals("null", StringComparison.OrdinalIgnoreCase)
                    || name.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (seen.Add(name))
                {
                    names.Add(name);
                }
            }

            return names;
        }

        private static string BuildTrainerScheduleId(string trainerName)
        {
            string source = (trainerName ?? string.Empty).Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(source))
            {
                return "UNASSIGNED";
            }

            char[] buffer = new char[Math.Min(10, source.Length)];
            int index = 0;
            foreach (char ch in source)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    buffer[index++] = ch;
                    if (index >= buffer.Length)
                    {
                        break;
                    }
                }
            }

            if (index == 0)
            {
                return "TRAINER";
            }

            return new string(buffer, 0, index);
        }

        private DataTable GetDailyJoListData(string periode, string supAreaId, string areaGroupId)
        {
            try
            {
                string safePeriode = (periode ?? string.Empty).Replace("'", "''");
                string safeSupAreaId = ResolveSupAreaParameter(supAreaId).Replace("'", "''");
                string safeAreaGroupId = NormalizeAreaGroupId(areaGroupId).Replace("'", "''");

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_daily_jo_list '"
                    + safePeriode + "','"
                    + safeSupAreaId + "','"
                    + safeAreaGroupId + "'",
                    DBConnstringSQL());

                DataTable dt = rec.DataRecord();
                return dt ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }

        private void PopulateDailyJoTotalsFromList(int[] capacityTotalJO, string periode, string selectedRegional, string selectedAreaGroup)
        {
            if (capacityTotalJO == null || capacityTotalJO.Length <= 1)
            {
                return;
            }

            if (UseJobTrainingDataSource)
            {
                PopulateDailyJoTotalsFromTrxAssignments(capacityTotalJO, periode, selectedRegional, selectedAreaGroup);
                return;
            }

            DataTable dtDailyJo = GetDailyJoListData(periode, selectedRegional, selectedAreaGroup);
            if (dtDailyJo == null || dtDailyJo.Rows.Count == 0)
            {
                return;
            }

            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            string safePeriode = NormalizeClosedJobPeriode(periode);
            if (string.IsNullOrWhiteSpace(safePeriode))
            {
                safePeriode = periodDate.ToString("yyyy-MM");
            }

            string safeSupAreaId = ResolveSupAreaParameter(selectedRegional);
            string safeAreaGroupId = NormalizeAreaGroupId(selectedAreaGroup);

            foreach (DataRow row in dtDailyJo.Rows)
            {
                int dayNo = ParseFirstAvailableInt(row, "DayNo", "DAYNO");
                if (dayNo <= 0 || dayNo >= capacityTotalJO.Length)
                {
                    continue;
                }

                int listValue = ParseFirstAvailableInt(row, "TotalJO", "TotalJo", "TOTALJO", "TotalUnit", "TOTALUNIT");
                if (listValue <= 0)
                {
                    capacityTotalJO[dayNo] = 0;
                    continue;
                }

                // List SP value is unit-based; day button must show Total JO (distinct JobID),
                // same as assign-day-total-btn modal.
                string scheduleDate = periodDate.ToString("yyyy-MM-") + dayNo.ToString("00");
                DataTable detail = GetDailyJoDetailListData(scheduleDate, safePeriode, safeSupAreaId, safeAreaGroupId);
                int totalJo = CountDistinctJobIds(detail);
                capacityTotalJO[dayNo] = Math.Max(0, totalJo > 0 ? totalJo : listValue);
            }
        }

        private void PopulateDailyJoTotalsFromTrxAssignments(
            int[] capacityTotalJO,
            string periode,
            string selectedRegional,
            string selectedAreaGroup)
        {
            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            DateTime monthStart = new DateTime(periodDate.Year, periodDate.Month, 1);
            DataTable details = LoadTrxJobAssignDetailRows(monthStart, monthStart.AddMonths(1));
            if (details == null || details.Rows.Count == 0)
            {
                return;
            }

            string filterSupAreaId = ResolveSupAreaParameter(selectedRegional);
            string filterAreaGroupId = NormalizeAreaGroupId(selectedAreaGroup);
            Dictionary<string, string> areaIdAreaGroupMap = LoadAreaIdAreaGroupMap();
            Dictionary<string, JobTrainingTrainerSchedule> itsUsers = BuildItsUserScheduleMap(31, filterAreaGroupId);

            Dictionary<int, HashSet<string>> jobsByDay = new Dictionary<int, HashSet<string>>();
            foreach (DataRow row in details.Rows)
            {
                if (!PassesTrxAssignDashboardFilter(row, filterSupAreaId, filterAreaGroupId, areaIdAreaGroupMap))
                {
                    continue;
                }

                string technicianId = FirstNonEmpty(
                    GetString(row, "TechnicianID"),
                    GetString(row, "TechnicianId"),
                    GetString(row, "ITID"));
                if (string.IsNullOrWhiteSpace(technicianId)
                    || FindItsTrainerByTechnicianKey(itsUsers, technicianId) == null)
                {
                    continue;
                }

                string schDateRaw = FirstNonEmpty(GetString(row, "SchDate"), GetString(row, "ScheduleDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                int dayNo = schDate.Day;
                if (dayNo <= 0 || dayNo >= capacityTotalJO.Length)
                {
                    continue;
                }

                HashSet<string> dayJobs;
                if (!jobsByDay.TryGetValue(dayNo, out dayJobs))
                {
                    dayJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    jobsByDay[dayNo] = dayJobs;
                }

                string jobId = FirstNonEmpty(GetString(row, "JobID"), GetString(row, "AssignID"), "__ROW__" + dayJobs.Count);
                dayJobs.Add(jobId);
            }

            foreach (KeyValuePair<int, HashSet<string>> pair in jobsByDay)
            {
                capacityTotalJO[pair.Key] = pair.Value.Count;
            }
        }

        private void PopulateDailyJoTotalsFromTraining(int[] capacityTotalJO, string periode, string selectedRegional, string selectedAreaGroup)
        {
            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            DataTable details = LoadJobTrainingRowsForSchedule(periode);
            if (details == null || details.Rows.Count == 0)
            {
                return;
            }

            string filterSupAreaId = ResolveSupAreaParameter(selectedRegional);
            string filterAreaGroupId = NormalizeAreaGroupId(selectedAreaGroup);
            Dictionary<int, HashSet<string>> jobsByDay = new Dictionary<int, HashSet<string>>();
            foreach (DataRow row in details.Rows)
            {
                string categoryName = FirstNonEmpty(
                    GetString(row, "TrainingCategoryName"),
                    GetString(row, "TrainCategoryName"),
                    GetString(row, "CategoryName"),
                    GetString(row, "Category"));
                string categoryId = FirstNonEmpty(
                    GetString(row, "TrainCategoryID"),
                    GetString(row, "TrainingCategoryID"),
                    GetString(row, "CategoryID"));
                if (!IsJobTrainingScheduleCategory(row, categoryName, categoryId))
                {
                    continue;
                }

                if (!PassesJobTrainingDashboardFilter(row, filterSupAreaId, filterAreaGroupId))
                {
                    continue;
                }

                string schDateRaw = FirstNonEmpty(
                    GetString(row, "sSchDate"),
                    GetString(row, "SchDate"),
                    GetString(row, "ScheduleDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                int dayNo = schDate.Day;
                if (dayNo <= 0 || dayNo >= capacityTotalJO.Length)
                {
                    continue;
                }

                HashSet<string> dayJobs;
                if (!jobsByDay.TryGetValue(dayNo, out dayJobs))
                {
                    dayJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    jobsByDay[dayNo] = dayJobs;
                }

                string jobId = FirstNonEmpty(GetString(row, "TrainingID"), GetString(row, "JobID"), "__ROW__" + dayJobs.Count);
                dayJobs.Add(jobId);
            }

            foreach (KeyValuePair<int, HashSet<string>> pair in jobsByDay)
            {
                capacityTotalJO[pair.Key] = pair.Value.Count;
            }
        }

        private DataTable GetDailyJoDetailListData(string scheduleDate, string periode, string supAreaId, string areaGroupId)
        {
            try
            {
                string connString = DBConnstringSQL();
                if (string.IsNullOrWhiteSpace(connString))
                {
                    return new DataTable();
                }

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_daily_jo_detail_list '"
                    + EscapeSqlLiteral((scheduleDate ?? string.Empty).Trim()) + "','"
                    + EscapeSqlLiteral((periode ?? string.Empty).Trim()) + "','"
                    + EscapeSqlLiteral((supAreaId ?? string.Empty).Trim()) + "','"
                    + EscapeSqlLiteral(NormalizeAreaGroupId(areaGroupId)) + "'",
                    connString.Trim());

                return rec.DataRecord() ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }

        private DataTable FilterByTab(DataTable source, string activeTab)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return new DataTable();
            }

            if (UseJobTrainingDataSource)
            {
                return source.Copy();
            }

            DataTable clone = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                string jobCategory = GetJobCategory(row);
                if (IsRoleMatched(jobCategory, activeTab))
                {
                    clone.ImportRow(row);
                }
            }
            return clone;
        }

        private void BindPerformance(string activeTab)
        {
            litPerformanceRows.Text = "";
            lblPerformanceEmpty.Visible = false;

            DataTable dtPerf = UseJobTrainingDataSource
                ? BuildItsPerformanceData(NormalizePeriode(txtPeriode.Text))
                : BuildDummyPerformanceData(activeTab);
            if (dtPerf == null || dtPerf.Rows.Count == 0)
            {
                lblPerformanceEmpty.Visible = true;
                return;
            }

            StringBuilder html = new StringBuilder();
            int rank = 1;
            foreach (DataRow row in dtPerf.Rows)
            {
                html.Append("<div class=\"perf-item\">");
                html.Append("<div class=\"perf-rank\">" + rank.ToString() + "</div>");
                html.Append("<div class=\"perf-info\">");
                html.Append("<div class=\"perf-name\">" + HttpUtility.HtmlEncode(Convert.ToString(row["FullName"])) + "</div>");
                html.Append("<div class=\"perf-area\">Open: " + FormatCount(Convert.ToString(row["OpenJob"])) + " | Close: " + FormatCount(Convert.ToString(row["CloseJob"])) + "</div>");
                html.Append("</div>");
                html.Append("<div class=\"perf-score\">" + FormatCount(Convert.ToString(row["TotalJob"])) + "<small>Total</small></div>");
                html.Append("</div>");
                rank++;
            }

            litPerformanceRows.Text = html.ToString();
        }

        private DataTable BuildItsPerformanceData(string periode)
        {
            DataTable dtPerf = new DataTable();
            dtPerf.Columns.Add("FullName");
            dtPerf.Columns.Add("TotalJob", typeof(int));
            dtPerf.Columns.Add("OpenJob", typeof(int));
            dtPerf.Columns.Add("CloseJob", typeof(int));

            Dictionary<string, JobTrainingTrainerSchedule> users = BuildItsUserScheduleMap(31);
            if (users.Count == 0)
            {
                return dtPerf;
            }

            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            ApplyTrxJobAssignDetailDayCounts(users, periodDate);
            ApplyJobTrainingClosedCountsOnly(users, periodDate, string.Empty, string.Empty);

            foreach (JobTrainingTrainerSchedule user in users.Values
                .GroupBy(u => u.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .OrderByDescending(u =>
                {
                    int dayTotal = 0;
                    if (u.DayJobCount != null)
                    {
                        for (int i = 1; i < u.DayJobCount.Length; i++)
                        {
                            dayTotal += u.DayJobCount[i];
                        }
                    }
                    return u.ClosedTraining + u.ClosedVisit + dayTotal;
                })
                .ThenBy(u => u.TrainerName, StringComparer.OrdinalIgnoreCase))
            {
                int closeJob = user.ClosedTraining + user.ClosedVisit;
                int openJob = 0;
                if (user.DayOpenCount != null)
                {
                    for (int i = 1; i < user.DayOpenCount.Length; i++)
                    {
                        openJob += user.DayOpenCount[i];
                    }
                }
                int totalJob = 0;
                if (user.DayJobCount != null)
                {
                    for (int i = 1; i < user.DayJobCount.Length; i++)
                    {
                        totalJob += user.DayJobCount[i];
                    }
                }
                AddDummyPerfRow(dtPerf, user.TrainerName, totalJob, openJob, closeJob);
            }

            return dtPerf;
        }

        private DataTable BuildDummyPerformanceData(string activeTab)
        {
            DataTable dtPerf = new DataTable();
            dtPerf.Columns.Add("FullName");
            dtPerf.Columns.Add("TotalJob", typeof(int));
            dtPerf.Columns.Add("OpenJob", typeof(int));
            dtPerf.Columns.Add("CloseJob", typeof(int));

            string tab = NormalizeTab(activeTab);
            if (tab == "itsupport")
            {
                return BuildItsPerformanceData(NormalizePeriode(Convert.ToString(Session[SessionPeriode])));
            }

            AddDummyPerfRow(dtPerf, "Joko Susanto", 18, 6, 12);
            AddDummyPerfRow(dtPerf, "Ariyanto", 12, 3, 9);
            AddDummyPerfRow(dtPerf, "Febri SH", 10, 2, 8);
            AddDummyPerfRow(dtPerf, "Fanny Hermawan", 10, 3, 7);
            AddDummyPerfRow(dtPerf, "Dwi Haryanto", 9, 1, 8);
            AddDummyPerfRow(dtPerf, "Rama", 9, 2, 7);
            AddDummyPerfRow(dtPerf, "Rahmat Hidayat", 8, 2, 6);
            AddDummyPerfRow(dtPerf, "Hagi Muhammad", 8, 2, 6);
            AddDummyPerfRow(dtPerf, "Angga Ferdyansah", 8, 1, 7);
            AddDummyPerfRow(dtPerf, "Joko Suswilo", 7, 2, 5);
            AddDummyPerfRow(dtPerf, "Trias S", 6, 1, 5);
            AddDummyPerfRow(dtPerf, "Siman", 6, 2, 4);
            return dtPerf;
        }

        private void AddDummyPerfRow(DataTable table, string fullName, int total, int open, int close)
        {
            DataRow dr = table.NewRow();
            dr["FullName"] = fullName;
            dr["TotalJob"] = total;
            dr["OpenJob"] = open;
            dr["CloseJob"] = close;
            table.Rows.Add(dr);
        }

        private void BindAvailabilitySchedule(string periode, string selectedRegional, string selectedAreaGroup)
        {
            litScheduleHeader.Text = "";
            litScheduleRows.Text = "";
            litCapacityRows.Text = "";
            lblScheduleEmpty.Visible = false;

            DateTime periodDate;
            if (!DateTime.TryParse(periode + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            int totalDays = DateTime.DaysInMonth(periodDate.Year, periodDate.Month);
            DataTable dtAvailability = GetAvailabilityData(periode, selectedRegional, selectedAreaGroup);

            Dictionary<int, string> dayNameMap = new Dictionary<int, string>();
            Dictionary<int, bool> weekendMap = new Dictionary<int, bool>();
            if (dtAvailability != null && dtAvailability.Rows.Count > 0)
            {
                foreach (var dayGroup in dtAvailability.AsEnumerable()
                    .GroupBy(r => ParseInt(GetString(r, "DayNo")))
                    .Where(g => g.Key > 0 && g.Key <= totalDays))
                {
                    DataRow sampleDay = dayGroup.First();
                    dayNameMap[dayGroup.Key] = GetShortDayName(GetString(sampleDay, "DayName"), new DateTime(periodDate.Year, periodDate.Month, dayGroup.Key));
                    weekendMap[dayGroup.Key] = GetString(sampleDay, "IsWeekend") == "1";
                }
            }

            int[] capacityTotalJO = new int[totalDays + 1];
            PopulateDailyJoTotalsFromList(capacityTotalJO, periode, selectedRegional, selectedAreaGroup);

            if (dtAvailability == null || dtAvailability.Rows.Count == 0)
            {
                litScheduleHeader.Text = BuildScheduleDayHeader(totalDays, periodDate, dayNameMap, weekendMap, capacityTotalJO);
                lblScheduleEmpty.Visible = true;
                lblMemberCount.InnerText = UseJobTrainingDataSource ? "0 IT Support" : "0 Person";
                litScheduleRows.Text = "<tr>"
                    + "<td class=\"col-no sticky-no cell-no\">-</td>"
                    + "<td class=\"col-name sticky-name cell-name\">-</td>"
                    + "<td class=\"col-total cell-total\">0</td>"
                    + "<td class=\"col-total-maint cell-total\">0</td>"
                    + (UseJobTrainingDataSource ? string.Empty : "<td class=\"col-stock cell-stock\">-</td>")
                    + "<td colspan=\"" + totalDays.ToString() + "\">Tidak ada data</td>"
                    + "</tr>";
                return;
            }

            var techGroups = dtAvailability.AsEnumerable()
                .GroupBy(r => new
                {
                    TechnicianID = GetAvailabilityTechnicianId(r),
                    Name = GetAvailabilityTechnicianName(r)
                })
                .Select(g => new
                {
                    Group = g,
                    TechnicianName = string.IsNullOrWhiteSpace(g.Key.Name) ? "-" : g.Key.Name,
                    TotalClosedJobNew = AggregateAvailabilityClosedCount(
                        g,
                        "TotalJobCloseNew",
                        "TotalJOCloseNew",
                        "TotalClosedJONew",
                        "TotalCloseNew",
                        "CloseJONew",
                        "JOCloseNew",
                        "TotalJobClose"),
                    TotalClosedJobMaint = AggregateAvailabilityClosedCount(
                        g,
                        "TotalJobCloseMaint",
                        "TotalJOCloseMaint",
                        "TotalClosedJOMaint",
                        "TotalCloseMaint",
                        "CloseJOMaint",
                        "JOCloseMaint")
                })
                .OrderByDescending(x => x.TotalClosedJobNew + x.TotalClosedJobMaint)
                .ThenBy(x => x.TechnicianName)
                .ThenBy(x => x.Group.Key.TechnicianID)
                .ToList();

            lblMemberCount.InnerText = techGroups.Count.ToString() + (UseJobTrainingDataSource ? " IT Support" : " Person");

            StringBuilder rows = new StringBuilder();
            int[] capacityAvailable = new int[totalDays + 1];
            int[] capacityCloseUnit = new int[totalDays + 1];
            int[] capacityJO1 = new int[totalDays + 1];
            int[] capacityJO2 = new int[totalDays + 1];
            int[] capacityJO3 = new int[totalDays + 1];
            int[] capacityOff = new int[totalDays + 1];
            int[] capacityCuti = new int[totalDays + 1];
            int[] capacityIzin = new int[totalDays + 1];

            for (int rowIndex = 0; rowIndex < techGroups.Count; rowIndex++)
            {
                var techEntry = techGroups[rowIndex];
                var techGroup = techEntry.Group;
                Dictionary<int, DataRow> dayMap = techGroup
                    .GroupBy(r => ParseInt(GetString(r, "DayNo")))
                    .Where(g => g.Key > 0 && g.Key <= totalDays)
                    .ToDictionary(g => g.Key, g => g.First());

                rows.Append("<tr>");
                rows.Append("<td class=\"col-no sticky-no cell-no\">" + (rowIndex + 1).ToString("00") + "</td>");
                string technicianName = string.IsNullOrWhiteSpace(techGroup.Key.Name) ? "-" : techGroup.Key.Name;
                string technicianNameHtml = HttpUtility.HtmlEncode(technicianName);
                string technicianNameAttr = HttpUtility.HtmlAttributeEncode(technicianName);
                rows.Append("<td class=\"col-name sticky-name cell-name\">" + technicianNameHtml + "</td>");

                int totalClosedJobNew = techEntry.TotalClosedJobNew;
                int totalClosedJobMaint = techEntry.TotalClosedJobMaint;

                rows.Append("<td class=\"col-total cell-total\">");
                rows.Append("<button type=\"button\" class=\"assign-totaljob-link tech-totaljob-trigger\" data-tech-id=\"" + HttpUtility.HtmlAttributeEncode(techGroup.Key.TechnicianID) + "\" data-tech-name=\"" + technicianNameAttr + "\" data-total-job=\"" + totalClosedJobNew.ToString() + "\" data-closed-training=\"" + totalClosedJobNew.ToString() + "\" data-closed-visit=\"" + totalClosedJobMaint.ToString() + "\" data-close-type=\"new_install\" data-periode=\"" + HttpUtility.HtmlAttributeEncode(periode) + "\" title=\"" + (UseJobTrainingDataSource ? "Lihat detail Total Closed JO Training" : "Lihat detail Total Closed JO New") + "\">");
                rows.Append(HttpUtility.HtmlEncode(totalClosedJobNew.ToString()));
                rows.Append("</button></td>");

                rows.Append("<td class=\"col-total-maint cell-total\">");
                rows.Append("<button type=\"button\" class=\"assign-totaljob-link tech-totaljob-trigger\" data-tech-id=\"" + HttpUtility.HtmlAttributeEncode(techGroup.Key.TechnicianID) + "\" data-tech-name=\"" + technicianNameAttr + "\" data-total-job=\"" + totalClosedJobMaint.ToString() + "\" data-closed-training=\"" + totalClosedJobNew.ToString() + "\" data-closed-visit=\"" + totalClosedJobMaint.ToString() + "\" data-close-type=\"maintenance\" data-periode=\"" + HttpUtility.HtmlAttributeEncode(periode) + "\" title=\"" + (UseJobTrainingDataSource ? "Lihat detail Total Closed JO Visit" : "Lihat detail Total Closed JO Maint") + "\">");
                rows.Append(HttpUtility.HtmlEncode(totalClosedJobMaint.ToString()));
                rows.Append("</button>");
                rows.Append("</td>");

                if (!UseJobTrainingDataSource)
                {
                    rows.Append("<td class=\"col-stock cell-stock\">");
                    rows.Append("<button type=\"button\" class=\"assign-stock-btn tech-stock-trigger\" data-tech-id=\"" + HttpUtility.HtmlAttributeEncode(techGroup.Key.TechnicianID) + "\" data-tech-name=\"" + technicianNameAttr + "\" title=\"Lihat stok alat teknisi\">");
                    rows.Append("<span class=\"assign-stock-icon\">📦</span> Stok");
                    rows.Append("</button>");
                    rows.Append("</td>");
                }

                for (int day = 1; day <= totalDays; day++)
                {
                    DataRow dayRow;
                    string displayValue = "AV";
                    int totalJob = 0;
                    int remainingJo = -1;
                    bool hasRemainingJo = false;
                    int totalJobCloseUnit = 0;
                    DateTime currentDate = new DateTime(periodDate.Year, periodDate.Month, day);
                    bool isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
                    if (dayMap.TryGetValue(day, out dayRow))
                    {
                        displayValue = GetString(dayRow, "DisplayValue");
                        isWeekend = GetString(dayRow, "IsWeekend") == "1";
                        if (dayRow.Table.Columns.Contains("TotalJob"))
                        {
                            totalJob = ParseInt(GetString(dayRow, "TotalJob"));
                        }
                        hasRemainingJo = dayRow.Table.Columns.Contains("RemainingJo")
                            || dayRow.Table.Columns.Contains("RemainingJO")
                            || dayRow.Table.Columns.Contains("Remaining_JO")
                            || dayRow.Table.Columns.Contains("RemainingJOCount");
                        if (hasRemainingJo)
                        {
                            remainingJo = ParseFirstAvailableInt(dayRow, "RemainingJo", "RemainingJO", "Remaining_JO", "RemainingJOCount");
                        }
                        totalJobCloseUnit = ParseFirstAvailableInt(dayRow, "TotalJobCloseUnit", "TotalJOCloseUnit", "TotalClosedUnit", "TotalCloseUnit");
                    }
                    capacityCloseUnit[day] += Math.Max(0, totalJobCloseUnit);

                    // Prioritas display:
                    // 1. Jika TotalJob > 0 maka tampilkan angka job.
                    // 2. Jika TotalJob = 0, gunakan DisplayValue dari SP.
                    if (totalJob > 0)
                    {
                        displayValue = totalJob.ToString();
                    }

                    string normalizedValue = (displayValue ?? string.Empty).Trim().ToUpperInvariant();
                    if (normalizedValue == "OFF")
                    {
                        normalizedValue = "OF";
                    }
                    int jobCount = 0;
                    bool isJobCount = int.TryParse(normalizedValue, out jobCount);
                    string statusCss = "st-av";
                    if (normalizedValue == "OF")
                    {
                        statusCss = "st-off";
                        capacityOff[day]++;
                    }
                    else if (normalizedValue == "C" || normalizedValue == "CT")
                    {
                        statusCss = "st-cuti";
                        capacityCuti[day]++;
                    }
                    else if (normalizedValue == "S" || normalizedValue == "SK")
                    {
                        statusCss = "st-sakit";
                    }
                    else if (normalizedValue == "I" || normalizedValue == "IZ")
                    {
                        statusCss = "st-izin";
                        capacityIzin[day]++;
                    }
                    else if (normalizedValue == "AV" || string.IsNullOrEmpty(normalizedValue))
                    {
                        normalizedValue = "AV";
                        statusCss = "st-av";
                        capacityAvailable[day]++;
                    }
                    else if (isJobCount)
                    {
                        bool isRemainingJoDone = hasRemainingJo && remainingJo == 0;
                        statusCss = isRemainingJoDone ? "st-unit-done" : "st-unit-open";
                        normalizedValue = jobCount.ToString();
                        if (jobCount <= 1)
                        {
                            capacityJO1[day]++;
                        }
                        else if (jobCount == 2)
                        {
                            capacityJO2[day]++;
                        }
                        else
                        {
                            capacityJO3[day]++;
                        }
                    }
                    else
                    {
                        normalizedValue = displayValue;
                    }

                    bool isUnavailable = dayRow != null
                        && dayRow.Table.Columns.Contains("IsAvailable")
                        && GetString(dayRow, "IsAvailable") == "0";
                    // IsAvailable hanya untuk AV/AD/status non-angka; jangan timpa warna RemainingJo pada sel job.
                    if (isUnavailable && !isJobCount)
                    {
                        statusCss = "st-unavailable";
                    }

                    DateTime cellDate = new DateTime(periodDate.Year, periodDate.Month, day);
                    bool isTodayCell = cellDate.Date == DateTime.Today;
                    bool isTodayAvCell = isTodayCell && normalizedValue == "AV";
                    bool isFutureOrToday = cellDate.Date >= DateTime.Today;
                    bool isAssignableStatus = normalizedValue == "AV"
                        || normalizedValue == "AD"
                        || normalizedValue == "OF";
                    bool availabilityBlocksAssign = isUnavailable
                        && normalizedValue != "OF";
                    bool canAssign = isAssignableStatus
                        && isFutureOrToday
                        && !availabilityBlocksAssign
                        && !IsCurrentUserTechnician();
                    string itIdForCell = string.Empty;
                    if (dayRow != null && dayRow.Table != null && dayRow.Table.Columns.Contains("ITID"))
                    {
                        itIdForCell = GetString(dayRow, "ITID");
                    }
                    string supAreaForCell = string.Empty;
                    if (dayRow != null && dayRow.Table != null)
                    {
                        if (dayRow.Table.Columns.Contains("SupAreaID"))
                        {
                            supAreaForCell = GetString(dayRow, "SupAreaID");
                        }
                        else if (dayRow.Table.Columns.Contains("AreaID"))
                        {
                            supAreaForCell = GetString(dayRow, "AreaID");
                        }
                    }
                    if (string.IsNullOrWhiteSpace(supAreaForCell)
                        || supAreaForCell.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                    {
                        supAreaForCell = GetCurrentUserSupAreaId();
                    }

                    string cellDisplayText = normalizedValue;
                    if (isJobCount && cellDate.Date < DateTime.Today && hasRemainingJo && remainingJo != 0)
                    {
                        cellDisplayText = normalizedValue + "*";
                    }

                    bool isReportClickable = isJobCount;
                    bool isInteractiveCell = canAssign || isReportClickable;
                    string interactCss = canAssign
                        ? " assign-cell--clickable"
                        : (isReportClickable ? " assign-cell--report-clickable" : " assign-cell--disabled");
                    string cellClass = "status-cell assign-cell-trigger " + statusCss + interactCss + (isTodayAvCell ? " day-today-status" : "");
                    rows.Append("<td class=\"col-day" + (isWeekend ? " day-weekend-cell" : "") + (isTodayAvCell ? " day-today-cell" : "") + "\">");
                    rows.Append("<div class=\"" + cellClass + "\"");
                    rows.Append(" data-tech-id=\"" + HttpUtility.HtmlAttributeEncode(techGroup.Key.TechnicianID) + "\"");
                    rows.Append(" data-tech-name=\"" + technicianNameAttr + "\"");
                    rows.Append(" data-it-id=\"" + HttpUtility.HtmlAttributeEncode(itIdForCell ?? string.Empty) + "\"");
                    rows.Append(" data-sup-area=\"" + HttpUtility.HtmlAttributeEncode(supAreaForCell) + "\"");
                    rows.Append(" data-date=\"" + cellDate.ToString("yyyy-MM-dd") + "\"");
                    rows.Append(" data-status=\"" + HttpUtility.HtmlAttributeEncode(normalizedValue) + "\"");
                    rows.Append(" data-can-assign=\"" + (canAssign ? "1" : "0") + "\"");
                    rows.Append(" role=\"button\" tabindex=\"" + (isInteractiveCell ? "0" : "-1") + "\"");
                    rows.Append(" aria-disabled=\"" + (isInteractiveCell ? "false" : "true") + "\">");
                    rows.Append(HttpUtility.HtmlEncode(cellDisplayText));
                    rows.Append("</div></td>");
                }

                rows.Append("</tr>");
            }

            litScheduleHeader.Text = BuildScheduleDayHeader(totalDays, periodDate, dayNameMap, weekendMap, capacityTotalJO);
            litScheduleRows.Text = rows.ToString();
            litCapacityRows.Text = BuildCapacityRows(totalDays, capacityCloseUnit, capacityAvailable, capacityJO1, capacityJO2, capacityJO3, capacityOff, capacityCuti, capacityIzin);
        }

        private string BuildScheduleDayHeader(
            int totalDays,
            DateTime periodDate,
            Dictionary<int, string> dayNameMap,
            Dictionary<int, bool> weekendMap,
            int[] totalJoPerDay)
        {
            StringBuilder header = new StringBuilder();
            for (int day = 1; day <= totalDays; day++)
            {
                DateTime currentDate = new DateTime(periodDate.Year, periodDate.Month, day);
                string shortDayName = dayNameMap.ContainsKey(day)
                    ? dayNameMap[day]
                    : GetShortDayName(string.Empty, currentDate);
                bool isWeekend = weekendMap.ContainsKey(day)
                    ? weekendMap[day]
                    : (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday);
                bool isToday = currentDate.Date == DateTime.Today;
                int totalJo = totalJoPerDay != null && day < totalJoPerDay.Length ? totalJoPerDay[day] : 0;
                string isoDate = currentDate.ToString("yyyy-MM-dd");

                header.Append("<th class=\"col-day" + (isWeekend ? " day-weekend-th" : "") + (isToday ? " day-today-th" : "") + "\">");
                header.Append("<button type=\"button\" class=\"assign-day-total-btn assign-day-total-trigger");
                header.Append(totalJo > 0 ? "" : " is-zero");
                header.Append("\" data-date=\"" + HttpUtility.HtmlAttributeEncode(isoDate) + "\"");
                header.Append(" data-total-jo=\"" + totalJo.ToString() + "\"");
                header.Append(" title=\"Total JO: " + totalJo.ToString() + "\">");
                header.Append(totalJo.ToString());
                header.Append("</button>");
                header.Append("<span class=\"assign-day-no\">" + day.ToString() + "</span>");
                header.Append("<span class=\"assign-day-name\">" + HttpUtility.HtmlEncode(shortDayName) + "</span>");
                header.Append("</th>");
            }

            return header.ToString();
        }

        private string GetShortDayName(string dayNameFromData, DateTime dateValue)
        {
            string source = (dayNameFromData ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(source))
            {
                source = dateValue.ToString("ddd");
            }

            if (source.StartsWith("mon", StringComparison.OrdinalIgnoreCase)) return "Sen";
            if (source.StartsWith("tue", StringComparison.OrdinalIgnoreCase)) return "Sel";
            if (source.StartsWith("wed", StringComparison.OrdinalIgnoreCase)) return "Rab";
            if (source.StartsWith("thu", StringComparison.OrdinalIgnoreCase)) return "Kam";
            if (source.StartsWith("fri", StringComparison.OrdinalIgnoreCase)) return "Jum";
            if (source.StartsWith("sat", StringComparison.OrdinalIgnoreCase)) return "Sab";
            if (source.StartsWith("sun", StringComparison.OrdinalIgnoreCase)) return "Min";

            if (source.Length >= 3)
            {
                return source.Substring(0, 3);
            }

            return source;
        }

        private string BuildCapacityRows(
            int totalDays,
            int[] closeUnit,
            int[] available,
            int[] jo1,
            int[] jo2,
            int[] jo3,
            int[] off,
            int[] cuti,
            int[] izin)
        {
            StringBuilder cap = new StringBuilder();
            cap.Append("<tr>");
            cap.Append("<td class=\"col-no sticky-no cap-header-cell\"></td>");
            cap.Append("<td class=\"col-name sticky-name cap-header-cell\">Kapasitas per Hari</td>");
            cap.Append("<td class=\"col-total cap-header-cell\"></td>");
            cap.Append("<td class=\"col-total-maint cap-header-cell\"></td>");
            if (!UseJobTrainingDataSource)
            {
                cap.Append("<td class=\"col-stock cap-header-cell\"></td>");
            }
            for (int day = 1; day <= totalDays; day++)
            {
                int closeUnitValue = closeUnit[day];
                cap.Append("<td class=\"cap-header-cell\" style=\"font-size:11px; font-weight:700;\">" + closeUnitValue.ToString() + "</td>");
            }
            cap.Append("</tr>");

            AppendCapacityRow(cap, "Available", "cap-avail", totalDays, available);
            AppendCapacityRow(cap, "JO 1 Unit", "cap-jo1", totalDays, jo1);
            AppendCapacityRow(cap, "JO 2 Unit", "cap-jo2", totalDays, jo2);
            AppendCapacityRow(cap, "JO 3+ Unit", "cap-jo3", totalDays, jo3);
            AppendCapacityRow(cap, "Off", "cap-off", totalDays, off);
            AppendCapacityRow(cap, "Cuti", "cap-cuti", totalDays, cuti);
            AppendCapacityRow(cap, "Izin", "cap-izin", totalDays, izin);

            return cap.ToString();
        }

        private void AppendCapacityRow(StringBuilder builder, string label, string cssClass, int totalDays, int[] values)
        {
            builder.Append("<tr>");
            builder.Append("<td class=\"col-no sticky-no cap-row-label " + cssClass + "\"></td>");
            builder.Append("<td class=\"col-name sticky-name cap-row-label " + cssClass + "\">" + label + "</td>");
            builder.Append("<td class=\"col-total cap-scroll-gap " + cssClass + "\"></td>");
            builder.Append("<td class=\"col-total-maint cap-scroll-gap " + cssClass + "\"></td>");
            if (!UseJobTrainingDataSource)
            {
                builder.Append("<td class=\"col-stock cap-scroll-gap " + cssClass + "\"></td>");
            }
            for (int day = 1; day <= totalDays; day++)
            {
                int val = values[day];
                builder.Append("<td class=\"" + cssClass + "\" style=\"font-size:11px; font-weight:600;\">" + (val > 0 ? val.ToString() : "") + "</td>");
            }
            builder.Append("</tr>");
        }

        private static string GetAvailabilityTechnicianId(DataRow row)
        {
            return FirstNonEmpty(
                GetValue(row, "TechnicianID"),
                GetValue(row, "TechnicianId"),
                GetValue(row, "UserID"),
                GetValue(row, "ITID"));
        }

        private static string GetAvailabilityTechnicianName(DataRow row)
        {
            return FirstNonEmpty(
                GetValue(row, "Name"),
                GetValue(row, "FullName"),
                GetValue(row, "TechnicianName"));
        }

        private int AggregateAvailabilityClosedCount(IEnumerable<DataRow> rows, params string[] columnCandidates)
        {
            if (rows == null)
            {
                return 0;
            }

            List<int> values = rows
                .Select(r => ParseFirstAvailableInt(r, columnCandidates))
                .Where(v => v > 0)
                .ToList();
            if (values.Count == 0)
            {
                return 0;
            }

            // Month total duplicated on every day row (same non-zero on each day).
            if (values.Count > 1 && values.Distinct().Count() == 1)
            {
                return values[0];
            }

            // Per-day closed counts: sum across the month.
            return values.Sum();
        }

        private int ParseFirstAvailableInt(DataRow row, params string[] columnCandidates)
        {
            if (row == null || row.Table == null || columnCandidates == null || columnCandidates.Length == 0)
            {
                return 0;
            }

            foreach (string columnName in columnCandidates)
            {
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    continue;
                }

                if (row.Table.Columns.Contains(columnName))
                {
                    int exact = ParseInt(GetString(row, columnName));
                    if (exact != 0 || row[columnName] != DBNull.Value)
                    {
                        return exact;
                    }
                }

                // Case-insensitive fallback when SP column casing differs from expected name.
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (column == null || !column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    object raw = row[column];
                    if (raw == null || raw == DBNull.Value)
                    {
                        return 0;
                    }

                    return ParseInt(Convert.ToString(raw).Trim());
                }
            }

            return 0;
        }

        private void BindDetailModal(string status, string detailJobType, string detailMetric)
        {
            string periode = NormalizePeriode(txtPeriode.Text);
            string activeTab = NormalizeTab(hfActiveTab.Value);
            string selectedRegional = GetSelectedRegionalTab();
            string selectedAreaGroup = NormalizeAreaGroupId(GetSelectedRegionalGroupTab());
            string normalizedStatus;
            string normalizedDetailJobType;
            string normalizedDetailMetric;
            int totalQtyGps;
            int totalQtyAcs;
            DataTable dtByRole = GetDetailModalData(
                periode,
                activeTab,
                selectedRegional,
                selectedAreaGroup,
                status,
                detailJobType,
                detailMetric,
                out normalizedStatus,
                out normalizedDetailJobType,
                out normalizedDetailMetric,
                out totalQtyGps,
                out totalQtyAcs);

            if (lblDetailTotalGps != null)
            {
                lblDetailTotalGps.InnerText = totalQtyGps.ToString();
            }
            if (lblDetailTotalAcs != null)
            {
                lblDetailTotalAcs.InnerText = totalQtyAcs.ToString();
            }

            lblModalTitle.InnerText = BuildDetailModalTitle(normalizedStatus, normalizedDetailJobType, normalizedDetailMetric);
            lblModalPeriode.InnerText = periode;

            if (dtByRole.Rows.Count > 0)
            {
                rptDetailJO.DataSource = dtByRole;
                rptDetailJO.DataBind();
                lblDetailEmpty.Visible = false;
            }
            else
            {
                rptDetailJO.DataSource = null;
                rptDetailJO.DataBind();
                lblDetailEmpty.Visible = true;
            }
        }

        private DataTable GetDetailModalData(
            string periode,
            string activeTab,
            string selectedRegional,
            string selectedAreaGroup,
            string status,
            string detailJobType,
            string detailMetric,
            out string normalizedStatus,
            out string normalizedDetailJobType,
            out string normalizedDetailMetric,
            out int totalQtyGps,
            out int totalQtyAcs)
        {
            string supAreaForQuery = ResolveSupAreaParameter(selectedRegional);
            normalizedStatus = NormalizeStatus(status);
            normalizedDetailJobType = NormalizeDetailJobType(detailJobType);
            normalizedDetailMetric = NormalizeDetailMetric(detailMetric);
            string spStatus = MapStatusForDetailStoredProcedure(normalizedStatus);
            DataTable dtSource = GetAssignJobList(periode, spStatus, supAreaForQuery, selectedAreaGroup);
            DataTable dtByRole = FilterByTab(dtSource, activeTab);
            dtByRole = FilterByDetailJobType(dtByRole, normalizedDetailJobType);
            if (normalizedDetailMetric == "jo")
            {
                dtByRole = KeepSingleRowPerJob(dtByRole);
            }

            dtByRole = SortDetailRowsByDtmUpdAsc(dtByRole);
            PrepareDetailDisplayColumns(dtByRole);
            totalQtyGps = dtByRole.AsEnumerable()
                .Sum(r => ParseFirstAvailableInt(r, "QtyGPS", "QTYGPS", "QtyGps", "TotalGPS", "TotalGps"));
            totalQtyAcs = dtByRole.AsEnumerable()
                .Sum(r => ParseFirstAvailableInt(r, "QtyACS", "QTYACS", "QtyAcs", "TotalACS", "TotalAcs"));
            return dtByRole;
        }

        private void PrepareDetailDisplayColumns(DataTable source)
        {
            if (source == null)
            {
                return;
            }

            if (!source.Columns.Contains("ScheduleDateDisplay"))
            {
                source.Columns.Add("ScheduleDateDisplay");
            }
            if (!source.Columns.Contains("AreaDisplay"))
            {
                source.Columns.Add("AreaDisplay");
            }
            if (!source.Columns.Contains("QtyGpsDisplay"))
            {
                source.Columns.Add("QtyGpsDisplay");
            }
            if (!source.Columns.Contains("QtyAcsDisplay"))
            {
                source.Columns.Add("QtyAcsDisplay");
            }
            if (!source.Columns.Contains("JobTypeDisplay"))
            {
                source.Columns.Add("JobTypeDisplay");
            }
            if (!source.Columns.Contains("JobDateDisplay"))
            {
                source.Columns.Add("JobDateDisplay");
            }
            if (!source.Columns.Contains("OverSlaDisplay"))
            {
                source.Columns.Add("OverSlaDisplay");
            }
            if (!source.Columns.Contains("OverSlaCssClass"))
            {
                source.Columns.Add("OverSlaCssClass");
            }
            if (!source.Columns.Contains("FullName"))
            {
                source.Columns.Add("FullName");
            }
            if (!source.Columns.Contains("Remark"))
            {
                source.Columns.Add("Remark");
            }

            foreach (DataRow row in source.Rows)
            {
                if (string.IsNullOrWhiteSpace(GetString(row, "FullName")))
                {
                    row["FullName"] = FirstNonEmpty(
                        GetString(row, "CustomerName"),
                        GetString(row, "CustName"),
                        GetString(row, "CustID"),
                        "-");
                }

                if (string.IsNullOrWhiteSpace(GetString(row, "Remark")))
                {
                    row["Remark"] = "-";
                }

                DateTime dateValue;
                if (DateTime.TryParse(GetString(row, "ScheduleDate"), out dateValue))
                {
                    row["ScheduleDateDisplay"] = dateValue.ToString("yyyy-MM-dd");
                }
                else
                {
                    row["ScheduleDateDisplay"] = GetString(row, "ScheduleDate");
                }

                row["AreaDisplay"] = FirstNonEmpty(
                    GetString(row, "AreaName"),
                    GetString(row, "AreaID"),
                    "-");

                row["QtyGpsDisplay"] = ParseFirstAvailableInt(row, "QtyGPS", "QTYGPS", "QtyGps", "TotalGPS", "TotalGps").ToString();
                row["QtyAcsDisplay"] = ParseFirstAvailableInt(row, "QtyACS", "QTYACS", "QtyAcs", "TotalACS", "TotalAcs").ToString();
                row["JobTypeDisplay"] = FirstNonEmpty(
                    GetString(row, "JobType"),
                    GetJobCategory(row),
                    "-");

                string jobDateRaw = FirstNonEmpty(
                    GetString(row, "DtmUpd"),
                    GetString(row, "DtmUpdate"),
                    GetString(row, "JobDate"));
                DateTime jobDateValue;
                if (DateTime.TryParse(jobDateRaw, out jobDateValue))
                {
                    row["JobDateDisplay"] = jobDateValue.ToString("yyyy-MM-dd");
                }
                else
                {
                    row["JobDateDisplay"] = jobDateRaw;
                }

                int overSla = ParseFirstAvailableInt(row, "OverSLA", "OverSla", "OVERSLA");
                row["OverSlaDisplay"] = overSla.ToString();
                row["OverSlaCssClass"] = overSla > 7 ? "detail-jo-over-sla" : string.Empty;
            }
        }

        private DataTable SortDetailRowsByDtmUpdAsc(DataTable source)
        {
            if (source == null || source.Rows.Count <= 1)
            {
                return source;
            }

            string dtmUpdColumn = ResolveFirstAvailableColumn(source, "DtmUpd", "DtmUpdate", "JobDate");
            if (string.IsNullOrWhiteSpace(dtmUpdColumn))
            {
                return source;
            }

            DataView view = source.DefaultView;
            view.Sort = dtmUpdColumn + " ASC";
            return view.ToTable();
        }

        private string ResolveFirstAvailableColumn(DataTable source, params string[] columnCandidates)
        {
            if (source == null || columnCandidates == null)
            {
                return string.Empty;
            }

            foreach (string candidate in columnCandidates)
            {
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    continue;
                }

                if (source.Columns.Contains(candidate))
                {
                    return candidate;
                }

                foreach (DataColumn column in source.Columns)
                {
                    if (column.ColumnName.Equals(candidate, StringComparison.OrdinalIgnoreCase))
                    {
                        return column.ColumnName;
                    }
                }
            }

            return string.Empty;
        }

        private bool HandleDetailExportRequest()
        {
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            if (!action.Equals("export_detail", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string status = Request.QueryString["status"];
            string detailJobType = Request.QueryString["detailJobType"];
            string detailMetric = Request.QueryString["detailMetric"];
            try
            {
                ExportDetailModalToExcel(status, detailJobType, detailMetric);
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/plain";
                Response.Write("Export gagal: " + ex.Message);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            return true;
        }

        private void ExportDetailModalToExcel(string status, string detailJobType, string detailMetric)
        {
            string periode = NormalizePeriode(Convert.ToString(Session[SessionPeriode]));
            string activeTab = NormalizeTab(Convert.ToString(Session[SessionActiveTab]));
            string selectedRegional = GetSelectedRegionalTab();
            string selectedAreaGroup = NormalizeAreaGroupId(GetSelectedRegionalGroupTab());

            string normalizedStatus;
            string normalizedDetailJobType;
            string normalizedDetailMetric;
            int totalQtyGps;
            int totalQtyAcs;
            DataTable dt = GetDetailModalData(
                periode,
                activeTab,
                selectedRegional,
                selectedAreaGroup,
                status,
                detailJobType,
                detailMetric,
                out normalizedStatus,
                out normalizedDetailJobType,
                out normalizedDetailMetric,
                out totalQtyGps,
                out totalQtyAcs);

            string title = BuildDetailModalTitle(normalizedStatus, normalizedDetailJobType, normalizedDetailMetric);
            string fileName = "detail_job_order_" + periode + "_" + normalizedStatus + "_" + normalizedDetailJobType + ".xls";
            string html = BuildDetailExportHtml(dt, title, periode, totalQtyGps, totalQtyAcs);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.Write(html);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private string BuildDetailExportHtml(DataTable source, string title, string periode, int totalQtyGps, int totalQtyAcs)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<html><head><meta charset='utf-8' /></head><body>");
            html.Append("<h3>" + HttpUtility.HtmlEncode(title) + "</h3>");
            html.Append("<div>Periode: " + HttpUtility.HtmlEncode(periode) + "</div>");
            html.Append("<br/>");
            html.Append("<table border='1' cellspacing='0' cellpadding='4'>");
            html.Append("<thead><tr>");
            html.Append("<th>No</th>");
            html.Append("<th>JobID</th>");
            html.Append("<th>FullName</th>");
            html.Append("<th>JobType</th>");
            html.Append("<th>Job Date</th>");
            html.Append("<th>ScheduleDate</th>");
            html.Append("<th>Area</th>");
            html.Append("<th>Total GPS</th>");
            html.Append("<th>Total ACS</th>");
            html.Append("<th>Status</th>");
            html.Append("<th>OverSLA</th>");
            html.Append("<th>Remark</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            if (source != null)
            {
                for (int i = 0; i < source.Rows.Count; i++)
                {
                    DataRow row = source.Rows[i];
                    int overSla = ParseFirstAvailableInt(row, "OverSLA", "OverSla", "OVERSLA");
                    string overSlaCellStyle = overSla > 7
                        ? " style='background-color:#fee2e2;color:#991b1b;'"
                        : string.Empty;
                    string overSlaValueStyle = overSla > 7
                        ? " style='background-color:#fee2e2;color:#991b1b;font-weight:bold;'"
                        : string.Empty;
                    html.Append("<tr>");
                    html.Append("<td" + overSlaCellStyle + ">" + (i + 1).ToString() + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "JobID")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "FullName")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "JobTypeDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "JobDateDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "ScheduleDateDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "AreaDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "QtyGpsDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "QtyAcsDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "Status")) + "</td>");
                    html.Append("<td" + overSlaValueStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "OverSlaDisplay")) + "</td>");
                    html.Append("<td" + overSlaCellStyle + ">" + HttpUtility.HtmlEncode(GetString(row, "Remark")) + "</td>");
                    html.Append("</tr>");
                }
            }

            html.Append("</tbody>");
            html.Append("<tfoot><tr>");
            html.Append("<td colspan='7' style='text-align:right;font-weight:bold;'>Total</td>");
            html.Append("<td style='font-weight:bold;'>" + totalQtyGps.ToString() + "</td>");
            html.Append("<td style='font-weight:bold;'>" + totalQtyAcs.ToString() + "</td>");
            html.Append("<td colspan='3'></td>");
            html.Append("</tr></tfoot>");
            html.Append("</table>");
            html.Append("</body></html>");
            return html.ToString();
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
            string[] formats = new[] { "yyyy-MM", "MM-yyyy", "MMMM yyyy", "MMM yyyy" };

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

        private string NormalizeTab(string value)
        {
            string tab = (value ?? string.Empty).Trim().ToLower();
            return tab == "itsupport" ? "itsupport" : "teknisi";
        }

        private string NormalizeStatus(string value)
        {
            string status = (value ?? string.Empty).Trim().ToLower();
            if (status == "open" || status == "close" || status == "scheduled")
            {
                return status;
            }
            return "all";
        }

        private string NormalizeDetailJobType(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToLowerInvariant();
            if (normalized == "new_installation" || normalized == "new")
            {
                return "new_installation";
            }

            if (normalized == "maintenance" || normalized == "maint")
            {
                return "maintenance";
            }

            return "all";
        }

        private string NormalizeDetailMetric(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToLowerInvariant();
            return normalized == "unit" ? "unit" : "jo";
        }

        private string MapStatusForDetailStoredProcedure(string normalizedStatus)
        {
            if (normalizedStatus == "open")
            {
                return "open";
            }

            if (normalizedStatus == "close")
            {
                return "close";
            }

            if (normalizedStatus == "scheduled")
            {
                return "scheduled";
            }

            return "all";
        }

        private bool IsRoleMatched(string jobCategory, string activeTab)
        {
            string type = (jobCategory ?? string.Empty).Trim().ToUpperInvariant();
            bool isITSupport = type.Contains("IT")
                || type.Contains("SUPPORT")
                || type.Contains("HELPDESK")
                || type.Contains("NOC");

            if (NormalizeTab(activeTab) == "itsupport")
            {
                return isITSupport;
            }

            return !isITSupport;
        }

        private DataTable FilterByDetailJobType(DataTable source, string normalizedDetailJobType)
        {
            if (source == null || source.Rows.Count == 0 || normalizedDetailJobType == "all")
            {
                return source ?? new DataTable();
            }

            DataTable clone = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                bool isMaintenance = IsMaintenanceCategory(GetJobCategory(row));
                bool include = normalizedDetailJobType == "maintenance"
                    ? isMaintenance
                    : !isMaintenance;
                if (include)
                {
                    clone.ImportRow(row);
                }
            }

            return clone;
        }

        private DataTable KeepSingleRowPerJob(DataTable source)
        {
            if (source == null || source.Rows.Count == 0 || !source.Columns.Contains("JobID"))
            {
                return source ?? new DataTable();
            }

            DataTable clone = source.Clone();
            HashSet<string> seenJobIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in source.Rows)
            {
                string jobId = GetString(row, "JobID");
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    clone.ImportRow(row);
                    continue;
                }

                if (!seenJobIds.Contains(jobId))
                {
                    seenJobIds.Add(jobId);
                    clone.ImportRow(row);
                }
            }

            return clone;
        }

        private string BuildDetailModalTitle(string normalizedStatus, string normalizedDetailJobType, string normalizedDetailMetric)
        {
            string statusLabel;
            if (normalizedStatus == "open")
            {
                statusLabel = "JO OPEN";
            }
            else if (normalizedStatus == "close")
            {
                statusLabel = "JO CLOSE";
            }
            else if (normalizedStatus == "scheduled")
            {
                statusLabel = "JO SCHEDULED";
            }
            else
            {
                statusLabel = "TOTAL JOB ORDER";
            }

            string typeLabel;
            if (UseJobTrainingDataSource)
            {
                typeLabel = normalizedDetailJobType == "maintenance"
                    ? "VISIT"
                    : (normalizedDetailJobType == "new_installation" ? "TRAINING" : "SEMUA TYPE");
            }
            else
            {
                typeLabel = normalizedDetailJobType == "maintenance"
                    ? "MAINTENANCE"
                    : (normalizedDetailJobType == "new_installation" ? "NEW INSTALLATION" : "SEMUA TYPE");
            }
            string metricLabel = normalizedDetailMetric == "unit" ? "TOTAL UNIT" : "TOTAL JO";
            return statusLabel + " - " + typeLabel + " (" + metricLabel + ")";
        }

        private string GetString(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
            {
                return string.Empty;
            }
            return Convert.ToString(row[columnName]).Trim();
        }

        private void SetDefaultSummary()
        {
            lblTotalJONew.InnerText = "0";
            lblTotalJOMaint.InnerText = "0";
            lblTotalJOOpenNew.InnerText = "0";
            lblTotalJOOpenMaint.InnerText = "0";
            lblTotalJOScheduledNew.InnerText = "0";
            lblTotalJOScheduledMaint.InnerText = "0";
            lblTotalJOCloseNew.InnerText = "0";
            lblTotalJOCloseMaint.InnerText = "0";
        }

        private void SetDefaultUnitSummary()
        {
            lblTotalUnitNew.InnerText = "0";
            lblTotalUnitMaint.InnerText = "0";
            lblTotalUnitOpenNew.InnerText = "0";
            lblTotalUnitOpenMaint.InnerText = "0";
            lblTotalUnitScheduledNew.InnerText = "0";
            lblTotalUnitScheduledMaint.InnerText = "0";
            lblTotalUnitCloseNew.InnerText = "0";
            lblTotalUnitCloseMaint.InnerText = "0";
        }

        private int ParseInt(string value)
        {
            int parsed;
            if (int.TryParse(value, out parsed))
            {
                return parsed;
            }

            double parsedDouble;
            if (double.TryParse(value, out parsedDouble))
            {
                return Convert.ToInt32(parsedDouble);
            }

            return 0;
        }

        private DataTable FilterScheduledOnly(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return new DataTable();
            }

            DataTable clone = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                string normalizedStatus = NormalizeSummaryStatus(GetString(row, "Status"));
                if (normalizedStatus == "scheduled")
                {
                    clone.ImportRow(row);
                }
            }
            return clone;
        }

        private DataTable FilterByRegional(DataTable source, string selectedRegional)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return new DataTable();
            }

            if (string.IsNullOrWhiteSpace(selectedRegional)
                || selectedRegional.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                return source.Copy();
            }

            DataTable clone = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                string rowArea = FirstNonEmpty(
                    GetString(row, "AreaID"),
                    GetString(row, "SupAreaID"),
                    GetString(row, "SupportAreaID"));
                if (rowArea.Equals(selectedRegional, StringComparison.OrdinalIgnoreCase))
                {
                    clone.ImportRow(row);
                }
            }

            return clone;
        }

        private string GetSelectedRegionalTab()
        {
            object sessionValue = Session[RegionalTabSessionKey];
            string value = sessionValue == null ? RegionalAllValue : Convert.ToString(sessionValue).Trim();
            return string.IsNullOrEmpty(value) ? RegionalAllValue : value;
        }

        private string GetSelectedRegionalGroupTab()
        {
            object sessionValue = Session[RegionalGroupTabSessionKey];
            string value = sessionValue == null ? RegionalAllValue : Convert.ToString(sessionValue).Trim();
            return string.IsNullOrEmpty(value) ? RegionalAllValue : value;
        }

        private static string NormalizeAreaGroupTabValue(string value)
        {
            string normalized = (value ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalized) || normalized.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                return RegionalAllValue;
            }

            if (normalized.StartsWith("ARG", StringComparison.OrdinalIgnoreCase))
            {
                return normalized;
            }

            string upper = normalized.ToUpperInvariant();
            if (upper.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase)
                || upper.Equals("EAST", StringComparison.OrdinalIgnoreCase)
                || upper.Equals("EAST AREA", StringComparison.OrdinalIgnoreCase))
            {
                return AreaGroupEastValue;
            }

            if (upper.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase)
                || upper.Equals("WEST", StringComparison.OrdinalIgnoreCase)
                || upper.Equals("WEST AREA", StringComparison.OrdinalIgnoreCase))
            {
                return AreaGroupWestValue;
            }

            return normalized;
        }

        private static string NormalizeAreaGroupId(string value)
        {
            string normalizedTabValue = NormalizeAreaGroupTabValue(value);
            if (normalizedTabValue.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return normalizedTabValue;
        }

        private string ResolveSupAreaParameter(string selectedRegional)
        {
            string supAreaId = (selectedRegional ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(supAreaId)
                || supAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return supAreaId;
        }

        protected string GetRegionalTabCss(object supAreaId)
        {
            string tabValue = supAreaId == null ? "" : Convert.ToString(supAreaId).Trim();
            bool isActive = string.Equals(tabValue, GetSelectedRegionalTab(), StringComparison.OrdinalIgnoreCase);
            return isActive ? "area-filter-btn active" : "area-filter-btn";
        }

        protected string GetAreaGroupTabCss(object areaGroupId)
        {
            string tabValue = NormalizeAreaGroupTabValue(areaGroupId == null ? "" : Convert.ToString(areaGroupId).Trim());
            bool isActive = string.Equals(tabValue, NormalizeAreaGroupTabValue(GetSelectedRegionalGroupTab()), StringComparison.OrdinalIgnoreCase);
            return isActive ? "area-filter-btn active" : "area-filter-btn";
        }

        protected void rptAreaGroupTabs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "SelectAreaGroupTab", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (IsCurrentUserRegionalScoped())
            {
                BindAllSection();
                return;
            }

            string selectedGroup = NormalizeAreaGroupTabValue(e.CommandArgument == null ? RegionalAllValue : Convert.ToString(e.CommandArgument).Trim());

            Session[RegionalGroupTabSessionKey] = selectedGroup;
            Session[RegionalTabSessionKey] = RegionalAllValue;
            BindAllSection();
        }

        protected void rptRegionalTabs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "SelectRegionalTab", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (IsCurrentUserRegionalScoped())
            {
                BindAllSection();
                return;
            }

            string selected = e.CommandArgument == null ? RegionalAllValue : Convert.ToString(e.CommandArgument).Trim();
            if (string.IsNullOrEmpty(selected))
            {
                selected = RegionalAllValue;
            }

            Session[RegionalTabSessionKey] = selected;
            BindAllSection();
        }

        private DataTable GetRegionalTabsFromViewState()
        {
            object stateValue = ViewState[ViewStateRegionalTabs];
            DataTable dt = stateValue as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }

            DataTable fallback = new DataTable();
            fallback.Columns.Add("SupAreaID");
            fallback.Columns.Add("SupAreaName");

            DataRow row = fallback.NewRow();
            row["SupAreaID"] = RegionalAllValue;
            row["SupAreaName"] = "SEMUA";
            fallback.Rows.Add(row);
            return fallback;
        }

        private string[] GetRegionalIdSequence(DataTable regionalTabs)
        {
            if (regionalTabs == null || regionalTabs.Rows.Count == 0)
            {
                return new string[0];
            }

            List<string> ids = new List<string>();
            foreach (DataRow row in regionalTabs.Rows)
            {
                string id = GetString(row, "SupAreaID");
                if (!string.IsNullOrWhiteSpace(id) && !id.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                {
                    ids.Add(id);
                }
            }
            return ids.ToArray();
        }

        private string GetRegionalLabel(string supAreaId, DataTable regionalTabs)
        {
            if (string.IsNullOrWhiteSpace(supAreaId))
            {
                return "-";
            }

            if (regionalTabs != null)
            {
                foreach (DataRow row in regionalTabs.Rows)
                {
                    if (GetString(row, "SupAreaID").Equals(supAreaId, StringComparison.OrdinalIgnoreCase))
                    {
                        string supAreaName = GetString(row, "SupAreaName");
                        if (!string.IsNullOrWhiteSpace(supAreaName))
                        {
                            return supAreaName.Length <= 8 ? supAreaName : supAreaName.Substring(0, 8);
                        }
                    }
                }
            }

            return supAreaId;
        }

        private string FormatCount(string value)
        {
            double numericValue;
            if (double.TryParse(value, out numericValue))
            {
                return numericValue.ToString("#,##0");
            }
            return "0";
        }

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static JobOrderInformationResponse LoadJobOrderInformation(string activeTab, string searchKeyword, int pageIndex, int pageSize)
        {
            return BuildJobOrderInformationResponse(activeTab, searchKeyword, pageIndex, pageSize);
        }

        protected virtual JobOrderInformationResponse ResolveJobOrderInformationForRequest(
            string activeTab,
            string searchKeyword,
            int pageIndex,
            int pageSize,
            string branchFilter)
        {
            return BuildJobOrderInformationResponse(activeTab, searchKeyword, pageIndex, pageSize);
        }

        private static DataTable BindJobOrderInstallation()
        {
            return ExecuteJobOrderInformationStoredProcedure("sp_dashboard_assign_job_new_installation");
        }

        private static DataTable BindJobOrderMaintenance()
        {
            return ExecuteJobOrderInformationStoredProcedure("sp_dashboard_assign_job_maintenance");
        }

        private static DataTable ExecuteJobOrderInformationStoredProcedure(string storedProcedureName)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null || context.Session["ClsTypeDBConnStringSQL"] == null)
            {
                throw new InvalidOperationException("Koneksi database tidak tersedia di session.");
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new InvalidOperationException("Koneksi database kosong.");
            }

            string openError = string.Empty;
            Recordset rec = new Recordset();
            rec.Open(storedProcedureName, connString.Trim(), ref openError);
            if (!string.IsNullOrWhiteSpace(openError))
            {
                throw new InvalidOperationException(storedProcedureName + ": " + openError);
            }

            DataTable dt = rec.DataRecord();
            return dt ?? new DataTable();
        }

        protected static DataTable FilterJobOrderInformation(DataTable source, string keyword)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return new DataTable();
            }

            string search = (keyword ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(search))
            {
                return source.Copy();
            }

            DataTable filtered = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                string merged = (
                    GetValue(row, "JobID") + " "
                    + GetValue(row, "CustID") + " "
                    + GetValue(row, "CustomerName") + " "
                    + GetValue(row, "BranchName") + " "
                    + GetValue(row, "DeviceTypeDesc")).ToLowerInvariant();
                if (merged.Contains(search.ToLowerInvariant()))
                {
                    filtered.ImportRow(row);
                }
            }

            return filtered;
        }

        private static bool IsInstallationTab(string activeTab)
        {
            string tab = (activeTab ?? string.Empty).Trim().ToLowerInvariant();
            return tab == "installation" || tab == "new_install" || tab == "new installation";
        }

        protected static string GetValue(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(row[columnName]).Trim();
        }

        protected static string NormalizeJobTrainingStatusCode(string status)
        {
            string normalized = (status ?? string.Empty).Replace("&nbsp;", " ").Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(normalized)
                || normalized == "-"
                || normalized == "NULL")
            {
                return "scheduled";
            }

            // job_training editable / open statuses.
            if (normalized == "RG" || normalized == "DR" || normalized == "OP" || normalized == "OPEN"
                || normalized == "DRAFT")
            {
                return "open";
            }

            // Only real closed statuses count toward Total Closed JO.
            if (normalized == "CL" || normalized == "CLOSE" || normalized == "CLOSED")
            {
                return "close";
            }

            if (normalized == "SC" || normalized == "SCH" || normalized == "SCHEDULED")
            {
                return "scheduled";
            }

            return "scheduled";
        }

        private static bool IsClosedJobTrainingStatus(string status)
        {
            return NormalizeJobTrainingStatusCode(status) == "close";
        }

        private static bool JobMatchesItsUser(
            DataRow row,
            string technicianId,
            string technicianName,
            Dictionary<string, JobTrainingTrainerSchedule> itsUsers,
            bool includeItOutbound = true)
        {
            if (row == null)
            {
                return false;
            }

            string wantId = TrimToLength((technicianId ?? string.Empty).Trim(), 20);
            string wantIdRaw = (technicianId ?? string.Empty).Trim();
            string wantName = (technicianName ?? string.Empty).Trim();
            bool isUnassignedTarget = string.Equals(wantIdRaw, "UNASSIGNED", StringComparison.OrdinalIgnoreCase)
                || string.Equals(wantName, "Unassigned", StringComparison.OrdinalIgnoreCase);

            List<JobTrainingTrainerSchedule> matchedUsers = ResolveJobTrainingAssignees(row, itsUsers, includeItOutbound);
            if (isUnassignedTarget)
            {
                return matchedUsers.Count == 0;
            }

            foreach (JobTrainingTrainerSchedule user in matchedUsers)
            {
                if ((!string.IsNullOrWhiteSpace(wantId)
                        && (user.TrainerId.Equals(wantId, StringComparison.OrdinalIgnoreCase)
                            || user.TrainerId.Equals(wantIdRaw, StringComparison.OrdinalIgnoreCase)))
                    || (!string.IsNullOrWhiteSpace(wantIdRaw)
                        && user.TrainerId.Equals(TrimToLength(wantIdRaw, 20), StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrWhiteSpace(wantName)
                        && user.TrainerName.Equals(wantName, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            if (includeItOutbound)
            {
                string itOutbound = NormalizeItOutboundValue(GetValue(row, "ITOutbound"));
                if (!string.IsNullOrWhiteSpace(itOutbound))
                {
                    if ((!string.IsNullOrWhiteSpace(wantIdRaw) && itOutbound.Equals(wantIdRaw, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrWhiteSpace(wantId) && itOutbound.Equals(wantId, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrWhiteSpace(wantId) && TrimToLength(itOutbound, 20).Equals(wantId, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrWhiteSpace(wantName) && itOutbound.Equals(wantName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }

                    JobTrainingTrainerSchedule byOutbound = FindItsUserByToken(itOutbound, itsUsers);
                    if (byOutbound != null
                        && ((!string.IsNullOrWhiteSpace(wantId) && byOutbound.TrainerId.Equals(wantId, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrWhiteSpace(wantName) && byOutbound.TrainerName.Equals(wantName, StringComparison.OrdinalIgnoreCase))))
                    {
                        return true;
                    }
                }
            }

            string remark = GetValue(row, "Remark");
            if (!string.IsNullOrWhiteSpace(remark))
            {
                if ((!string.IsNullOrWhiteSpace(wantName) && remark.IndexOf("IT:" + wantName, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(wantIdRaw) && remark.IndexOf("ITID:" + wantIdRaw, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(wantId) && remark.IndexOf("ITID:" + wantId, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(wantName) && remark.IndexOf("ITNAME:" + wantName, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    return true;
                }
            }

            return false;
        }

        protected static string FirstNonEmptyStatic(params string[] values)
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

        private static int ParseIntValue(string value)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                return intValue;
            }

            decimal decimalValue;
            if (decimal.TryParse(value, out decimalValue))
            {
                return Convert.ToInt32(decimalValue);
            }

            return 0;
        }

        protected static int ParseIntFromColumns(DataRow row, params string[] columns)
        {
            if (row == null || columns == null || columns.Length == 0)
            {
                return 0;
            }

            foreach (string columnName in columns)
            {
                if (row.Table != null && row.Table.Columns.Contains(columnName))
                {
                    return ParseIntValue(GetValue(row, columnName));
                }
            }

            return 0;
        }

        protected static string FormatDateForDisplay(string source)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(source, out parsedDate))
            {
                return parsedDate.ToString("dd MMM yyyy");
            }

            return source;
        }

        private static string NormalizeAssignStatusCode(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized == "IZSK")
            {
                return "IZ";
            }
            return normalized;
        }

        protected static string NormalizeDeviceGroupId(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized == "ACS")
            {
                return "ACS";
            }

            if (normalized == "GPS")
            {
                return "GPS";
            }

            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static AreaOptionResponse LoadAreaOptions(string supAreaId)
        {
            return BuildAreaOptionsResponse(supAreaId);
        }

        private static AreaOptionResponse BuildAreaOptionsResponse(string supAreaId)
        {
            AreaOptionResponse response = new AreaOptionResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                Rows = new List<AreaOptionItem>()
            };

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            string selectedRegional = Convert.ToString(context.Session[GetRegionalTabSessionKeyStatic()]).Trim();
            string effectiveSupAreaId = (supAreaId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(effectiveSupAreaId)
                || effectiveSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(selectedRegional)
                    && !selectedRegional.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                {
                    effectiveSupAreaId = selectedRegional;
                }
                else
                {
                    effectiveSupAreaId = Convert.ToString(context.Session[SessionUserSupAreaID]).Trim();
                }
            }

            bool loadAllAreas = string.IsNullOrWhiteSpace(effectiveSupAreaId)
                || effectiveSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase);

            string selectedAreaGroup = NormalizeAreaGroupId(Convert.ToString(context.Session[GetRegionalGroupTabSessionKeyStatic()]).Trim());

            // Teknisi keeps original behavior: require a concrete regional/suparea.
            // IT Support may load all / by area-group when regional is ALL.
            if (loadAllAreas && !IsJobTrainingAssignRequestContext())
            {
                response.Message = "Regional/SupArea belum dipilih.";
                return response;
            }

            try
            {
                DataTable dt;
                if (loadAllAreas)
                {
                    if (!string.IsNullOrWhiteSpace(selectedAreaGroup))
                    {
                        dt = LoadAreaOptionsTableByAreaGroup(connString.Trim(), selectedAreaGroup);
                    }
                    else
                    {
                        dt = LoadAllAreaOptionsTable(connString.Trim());
                    }
                }
                else
                {
                    dt = LoadAreaOptionsTableBySupArea(connString.Trim(), effectiveSupAreaId);
                }

                HashSet<string> dedupe = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in dt.Rows)
                {
                    string areaId = FirstNonEmpty(
                        GetValue(row, "AreaID"),
                        GetValue(row, "Value"),
                        row.Table.Columns.Count > 0 ? Convert.ToString(row[0]) : string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(areaId)
                        || areaId.Equals("[Select]", StringComparison.OrdinalIgnoreCase)
                        || !dedupe.Add(areaId))
                    {
                        continue;
                    }

                    string areaName = FirstNonEmpty(
                        GetValue(row, "AreaName"),
                        GetValue(row, "Text"),
                        row.Table.Columns.Count > 1 ? Convert.ToString(row[1]) : string.Empty,
                        areaId).Trim();
                    response.Rows.Add(new AreaOptionItem
                    {
                        AreaID = areaId,
                        AreaName = areaName
                    });
                }

                response.Result = "SUCCESS";
                if (response.Rows.Count > 0)
                {
                    response.Message = loadAllAreas
                        ? "Data semua area berhasil dimuat."
                        : "Data area berhasil dimuat.";
                }
                else
                {
                    response.Message = loadAllAreas
                        ? "Area tidak ditemukan."
                        : "Area tidak ditemukan untuk regional " + effectiveSupAreaId + ".";
                }
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat area: " + ex.Message;
            }

            return response;
        }

        private static DataTable LoadAreaOptionsTableBySupArea(string connString, string supAreaId)
        {
            Recordset rec = new Recordset();
            rec.Open(
                "sp_dashboard_assign_job_get_area_by_suparea '" + EscapeSqlLiteral(TrimToLength(supAreaId, 10)) + "'",
                connString);
            return rec.DataRecord() ?? new DataTable();
        }

        private static DataTable LoadAreaOptionsTableByAreaGroup(string connString, string areaGroupId)
        {
            DataTable merged = new DataTable();
            merged.Columns.Add("AreaID");
            merged.Columns.Add("AreaName");
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string normalizedGroupId = NormalizeAreaGroupId(areaGroupId);
            string safeGroupId = EscapeSqlLiteral(normalizedGroupId);
            try
            {
                Recordset regionalRec = new Recordset();
                regionalRec.Open(
                    "sp_dashboard_assign_job_get_list_regional '','" + safeGroupId + "'",
                    connString);
                DataTable regionals = regionalRec.DataRecord() ?? new DataTable();
                foreach (DataRow regionalRow in regionals.Rows)
                {
                    string regionalId = FirstNonEmpty(
                        GetValue(regionalRow, "SupAreaID"),
                        GetValue(regionalRow, "SupportAreaID")).Trim();
                    if (string.IsNullOrWhiteSpace(regionalId)
                        || regionalId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string rowGroup = NormalizeAreaGroupId(FirstNonEmpty(
                        GetValue(regionalRow, "AreaGroupID"),
                        GetValue(regionalRow, "GroupID")));
                    if (!string.IsNullOrWhiteSpace(normalizedGroupId)
                        && !string.IsNullOrWhiteSpace(rowGroup)
                        && !rowGroup.Equals(normalizedGroupId, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    DataTable areas = LoadAreaOptionsTableBySupArea(connString, regionalId);
                    foreach (DataRow areaRow in areas.Rows)
                    {
                        string areaId = FirstNonEmpty(GetValue(areaRow, "AreaID"), "").Trim();
                        if (string.IsNullOrWhiteSpace(areaId) || !seen.Add(areaId))
                        {
                            continue;
                        }

                        DataRow imported = merged.NewRow();
                        imported["AreaID"] = areaId;
                        imported["AreaName"] = FirstNonEmpty(GetValue(areaRow, "AreaName"), areaId);
                        merged.Rows.Add(imported);
                    }
                }
            }
            catch
            {
            }

            if (merged.Rows.Count == 0)
            {
                return LoadAllAreaOptionsTable(connString);
            }

            return merged;
        }

        private static DataTable LoadAllAreaOptionsTable(string connString)
        {
            try
            {
                Recordset rec = new Recordset();
                rec.Open("sp_list_area_selectbox ''", connString);
                DataTable direct = rec.DataRecord();
                if (direct != null && direct.Rows.Count > 0)
                {
                    return direct;
                }
            }
            catch
            {
            }

            DataTable merged = new DataTable();
            merged.Columns.Add("AreaID");
            merged.Columns.Add("AreaName");
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                Recordset regionalRec = new Recordset();
                regionalRec.Open("sp_dashboard_assign_job_get_list_regional '','" + EscapeSqlLiteral(string.Empty) + "'", connString);
                DataTable regionals = regionalRec.DataRecord() ?? new DataTable();
                foreach (DataRow regionalRow in regionals.Rows)
                {
                    string regionalId = FirstNonEmpty(
                        GetValue(regionalRow, "SupAreaID"),
                        GetValue(regionalRow, "SupportAreaID")).Trim();
                    if (string.IsNullOrWhiteSpace(regionalId)
                        || regionalId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    DataTable areas = LoadAreaOptionsTableBySupArea(connString, regionalId);
                    foreach (DataRow areaRow in areas.Rows)
                    {
                        string areaId = FirstNonEmpty(GetValue(areaRow, "AreaID"), "").Trim();
                        if (string.IsNullOrWhiteSpace(areaId) || !seen.Add(areaId))
                        {
                            continue;
                        }

                        DataRow imported = merged.NewRow();
                        imported["AreaID"] = areaId;
                        imported["AreaName"] = FirstNonEmpty(GetValue(areaRow, "AreaName"), areaId);
                        merged.Rows.Add(imported);
                    }
                }
            }
            catch
            {
            }

            return merged;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static SaveAssignResponse SaveAssignJob(string assignId, string jobId, string custId, string technicianId, string schDate, int qtyAssign, string deviceGroupId, string areaId, string targetStatus, string insDeviceTypeId)
        {
            SaveAssignResponse response = new SaveAssignResponse
            {
                Result = "ERROR",
                AssignID = string.Empty,
                Message = string.Empty
            };

            string normalizedTargetStatus = NormalizeAssignStatusCode(targetStatus);
            if (normalizedTargetStatus != "AV"
                && normalizedTargetStatus != "OF"
                && normalizedTargetStatus != "CT"
                && normalizedTargetStatus != "IZ"
                && normalizedTargetStatus != "AD"
                && normalizedTargetStatus != "UD")
            {
                response.Message = "Status assignment tidak valid.";
                return response;
            }

            bool requiresJobAssignment = normalizedTargetStatus == "AV";
            if (requiresJobAssignment)
            {
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    response.Message = "Job Order wajib dipilih.";
                    return response;
                }
                if (jobId.Trim().Length > 10)
                {
                    response.Message = "JobID melebihi batas 10 karakter.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(custId))
                {
                    response.Message = "Customer wajib dipilih.";
                    return response;
                }
                if (custId.Trim().Length > 10)
                {
                    response.Message = "CustID melebihi batas 10 karakter.";
                    return response;
                }
            }
            else
            {
                jobId = string.Empty;
                custId = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(technicianId))
            {
                response.Message = AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            if (IsJobTrainingAssignRequestContext())
            {
                string resolvedItId;
                string itIdMessage;
                if (!TryResolveItsAssignTechnicianId(technicianId, out resolvedItId, out itIdMessage))
                {
                    response.Message = string.IsNullOrWhiteSpace(itIdMessage) ? "ITID not exist" : itIdMessage;
                    return response;
                }
                technicianId = resolvedItId;
            }

            if (technicianId.Trim().Length > 10)
            {
                response.Message = AssignRoleDisplayName() + " melebihi batas 10 karakter.";
                return response;
            }

            DateTime schDateValue;
            if (!DateTime.TryParse(schDate, out schDateValue))
            {
                response.Message = "Tanggal schedule tidak valid.";
                return response;
            }

            if (requiresJobAssignment && qtyAssign <= 0)
            {
                response.Message = "Qty assign minimal 1.";
                return response;
            }

            if (!requiresJobAssignment)
            {
                qtyAssign = 0;
                areaId = string.Empty;
            }

            string normalizedAreaId = (areaId ?? string.Empty).Trim();
            if (normalizedAreaId.Length > 10)
            {
                response.Message = "AreaID melebihi batas 10 karakter.";
                return response;
            }
            if (requiresJobAssignment && string.IsNullOrWhiteSpace(normalizedAreaId))
            {
                response.Message = "Area wajib dipilih.";
                return response;
            }

            string normalizedDeviceGroupId = NormalizeDeviceGroupId(deviceGroupId);
            if (string.IsNullOrWhiteSpace(normalizedDeviceGroupId))
            {
                // IT Support Training/Visit: Device Group is optional (UI may still default GPS).
                if (IsJobTrainingAssignRequestContext())
                {
                    normalizedDeviceGroupId = string.Empty;
                }
                else
                {
                    response.Message = "Device group tidak valid.";
                    return response;
                }
            }

            string normalizedInsDeviceTypeId = TrimToLength((insDeviceTypeId ?? string.Empty).Trim(), 10);
            if (!requiresJobAssignment)
            {
                normalizedInsDeviceTypeId = string.Empty;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            string usrUpd = Convert.ToString(context.Session["ClsTypeUserID"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(usrUpd))
            {
                response.Message = "User login tidak ditemukan.";
                return response;
            }

            try
            {
                SaveAssignResponse saveOnce = ExecuteSaveAssignOnce(
                    connString,
                    jobId,
                    custId,
                    technicianId,
                    schDateValue,
                    qtyAssign,
                    normalizedDeviceGroupId,
                    normalizedAreaId,
                    normalizedTargetStatus,
                    usrUpd,
                    normalizedInsDeviceTypeId);
                if (!"SUCCESS".Equals(saveOnce.Result, StringComparison.OrdinalIgnoreCase))
                {
                    return saveOnce;
                }

                response = saveOnce;
                response.Result = "SUCCESS";
                if (string.IsNullOrWhiteSpace(response.Message))
                {
                    response.Message = "Assignment berhasil disimpan.";
                }
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal menyimpan assignment: " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static SaveAssignResponse UpdateTechnicianStatus(string technicianId, string schDate, string targetStatus)
        {
            SaveAssignResponse response = new SaveAssignResponse
            {
                Result = "ERROR",
                AssignID = string.Empty,
                Message = string.Empty
            };

            string normalizedTargetStatus = NormalizeAssignStatusCode(targetStatus);
            if (normalizedTargetStatus != "AV"
                && normalizedTargetStatus != "OF"
                && normalizedTargetStatus != "CT"
                && normalizedTargetStatus != "IZ")
            {
                response.Message = "Status " + AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(technicianId))
            {
                response.Message = AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            if (IsJobTrainingAssignRequestContext())
            {
                string resolvedItId;
                string itIdMessage;
                if (!TryResolveItsAssignTechnicianId(technicianId, out resolvedItId, out itIdMessage))
                {
                    response.Message = string.IsNullOrWhiteSpace(itIdMessage) ? "ITID not exist" : itIdMessage;
                    return response;
                }
                technicianId = resolvedItId;
            }

            if (technicianId.Trim().Length > 10)
            {
                response.Message = AssignRoleDisplayName() + " melebihi batas 10 karakter.";
                return response;
            }

            DateTime schDateValue;
            if (!DateTime.TryParse(schDate, out schDateValue))
            {
                response.Message = "Tanggal schedule tidak valid.";
                return response;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            string usrUpd = Convert.ToString(context.Session["ClsTypeUserID"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }
            if (string.IsNullOrWhiteSpace(usrUpd))
            {
                response.Message = "User login tidak ditemukan.";
                return response;
            }

            try
            {
                SaveAssignResponse updateOnce = ExecuteUpdateStatusOnlyOnce(
                    connString,
                    technicianId,
                    schDateValue,
                    normalizedTargetStatus,
                    usrUpd);
                if (!"SUCCESS".Equals(updateOnce.Result, StringComparison.OrdinalIgnoreCase))
                {
                    return updateOnce;
                }

                response = updateOnce;
                response.Result = "SUCCESS";
                if (string.IsNullOrWhiteSpace(response.Message))
                {
                    response.Message = "Status " + AssignRoleDisplayName() + " berhasil diperbarui.";
                }
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal mengubah status " + AssignRoleDisplayName() + ": " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TechnicianStockResponse LoadTechnicianStock(string technicianId)
        {
            TechnicianStockResponse response = new TechnicianStockResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                GPSCount = 0,
                GSMCount = 0,
                TotalUnit = 0,
                GpsDevices = new List<TechnicianStockGpsItem>(),
                GsmItems = new List<TechnicianStockGsmItem>(),
                Accessories = new List<TechnicianStockAccessoryItem>()
            };

            string safeTechnicianId = TrimToLength((technicianId ?? string.Empty).Trim(), 20);
            if (string.IsNullOrWhiteSpace(safeTechnicianId))
            {
                response.Message = AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                string escapedTechnicianId = EscapeSqlLiteral(safeTechnicianId);
                int totalGpsDevice = 0;
                int totalGsmDevice = 0;

                Recordset gpsRecordset = new Recordset();
                gpsRecordset.Open(
                    "sp_dashboard_assign_job_batch_stock_teknisi_gps '" + escapedTechnicianId + "'",
                    connString.Trim());
                DataTable gpsTable = gpsRecordset.DataRecord() ?? new DataTable();
                foreach (DataRow row in gpsTable.Rows)
                {
                    TechnicianStockGpsItem gpsItem = new TechnicianStockGpsItem
                    {
                        DeviceTypeID = FirstNonEmpty(GetValue(row, "DeviceTypeID"), GetValue(row, "DeviceTypeId"), "-"),
                        DeviceTypeDesc = FirstNonEmpty(GetValue(row, "DeviceTypeDesc"), GetValue(row, "DeviceType"), "-"),
                        Total = ParseIntValue(FirstNonEmpty(GetValue(row, "Total"), GetValue(row, "Qty"), "0"))
                    };
                    response.GpsDevices.Add(gpsItem);
                    totalGpsDevice += Math.Max(0, gpsItem.Total);
                }

                Recordset gsmRecordset = new Recordset();
                gsmRecordset.Open(
                    "sp_dashboard_assign_job_batch_stock_teknisi_gsm '" + escapedTechnicianId + "'",
                    connString.Trim());
                DataTable gsmTable = gsmRecordset.DataRecord() ?? new DataTable();
                foreach (DataRow row in gsmTable.Rows)
                {
                    TechnicianStockGsmItem gsmItem = new TechnicianStockGsmItem
                    {
                        ProviderID = FirstNonEmpty(GetValue(row, "ProviderID"), GetValue(row, "Provider"), "-"),
                        Total = ParseIntValue(FirstNonEmpty(GetValue(row, "Total"), GetValue(row, "Qty"), "0"))
                    };
                    response.GsmItems.Add(gsmItem);
                    totalGsmDevice += Math.Max(0, gsmItem.Total);
                }

                Recordset accessoriesRecordset = new Recordset();
                accessoriesRecordset.Open(
                    "sp_dashboard_assign_job_batch_stock_teknisi_accesories '" + escapedTechnicianId + "'",
                    connString.Trim());
                DataTable accessoriesTable = accessoriesRecordset.DataRecord() ?? new DataTable();

                foreach (DataRow row in accessoriesTable.Rows)
                {
                    response.Accessories.Add(new TechnicianStockAccessoryItem
                    {
                        DeviceTypeDesc = GetValue(row, "DeviceTypeDesc"),
                        Total = ParseIntValue(GetValue(row, "Total"))
                    });
                }

                response.GPSCount = totalGpsDevice;
                response.GSMCount = totalGsmDevice;
                response.TotalUnit = totalGpsDevice + totalGsmDevice;
                response.Result = "SUCCESS";
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat stock " + AssignRoleDisplayName() + ": " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TechnicianGpsDetailResponse LoadTechnicianGpsDetail(string technicianId, string deviceTypeId, string technicianName, string deviceTypeDesc)
        {
            TechnicianGpsDetailResponse response = new TechnicianGpsDetailResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                TechnicianID = TrimToLength((technicianId ?? string.Empty).Trim(), 20),
                TechnicianName = (technicianName ?? string.Empty).Trim(),
                DeviceTypeID = TrimToLength((deviceTypeId ?? string.Empty).Trim(), 30),
                DeviceTypeDesc = (deviceTypeDesc ?? string.Empty).Trim(),
                Rows = new List<TechnicianGpsDetailItem>()
            };

            if (string.IsNullOrWhiteSpace(response.TechnicianID))
            {
                response.Message = AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(response.DeviceTypeID))
            {
                response.Message = "DeviceTypeID tidak valid.";
                return response;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                string escapedTechnicianId = EscapeSqlLiteral(response.TechnicianID);
                string escapedDeviceTypeId = EscapeSqlLiteral(response.DeviceTypeID);

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_batch_stock_teknisi_gps_detail '" + escapedTechnicianId + "','" + escapedDeviceTypeId + "'",
                    connString.Trim());
                DataTable dt = rec.DataRecord() ?? new DataTable();

                foreach (DataRow row in dt.Rows)
                {
                    string status = FirstNonEmpty(GetValue(row, "Status"), "-");
                    string resolvedDeviceTypeDesc = FirstNonEmpty(
                        GetValue(row, "DeviceTypeDesc"),
                        response.DeviceTypeDesc,
                        "-");
                    response.Rows.Add(new TechnicianGpsDetailItem
                    {
                        DeviceID = FirstNonEmpty(GetValue(row, "DeviceID"), GetValue(row, "TvdID"), "-"),
                        NoSN = FirstNonEmpty(GetValue(row, "NoSN"), "-"),
                        DeviceTypeDesc = resolvedDeviceTypeDesc,
                        Status = status,
                        StatusText = NormalizeDeviceStatusText(status)
                    });
                }

                if (response.Rows.Count > 0)
                {
                    response.DeviceTypeDesc = FirstNonEmpty(response.Rows[0].DeviceTypeDesc, response.DeviceTypeDesc, "-");
                }

                response.Result = "SUCCESS";
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat detail device " + AssignRoleDisplayName() + ": " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ScheduleReportResponse LoadScheduleReport(string technicianId, string schDate, string technicianName)
        {
            ScheduleReportResponse response = new ScheduleReportResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                TechnicianID = TrimToLength((technicianId ?? string.Empty).Trim(), 20),
                TechnicianName = (technicianName ?? string.Empty).Trim(),
                SchDate = string.Empty,
                TotalUnitSelesai = 0,
                TotalUnitBelumSelesai = 0,
                Rows = new List<ScheduleReportRowItem>()
            };

            DateTime schDateValue;
            if (string.IsNullOrWhiteSpace(response.TechnicianID) || !DateTime.TryParse(schDate, out schDateValue))
            {
                response.Message = "Parameter laporan schedule tidak valid.";
                return response;
            }

            response.SchDate = schDateValue.ToString("yyyy-MM-dd");

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                string safeTechnicianId = EscapeSqlLiteral(response.TechnicianID);
                string safeSchDate = schDateValue.ToString("yyyy-MM-dd");

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_schedule '" + safeTechnicianId + "','" + safeSchDate + "'",
                    connString.Trim());
                DataTable dt = rec.DataRecord() ?? new DataTable();

                List<ScheduleReportRowItem> rows = new List<ScheduleReportRowItem>();
                string resolvedTechnicianName = response.TechnicianName;
                foreach (DataRow row in dt.Rows)
                {
                    string statusCode = FirstNonEmpty(
                        GetValue(row, "Status"),
                        GetValue(row, "StatusCode"));
                    bool isCompleted = statusCode.Equals("CL", StringComparison.OrdinalIgnoreCase);
                    int seq = ParseIntFromColumns(row, "Seq", "SEQ");
                    string assignId = FirstNonEmpty(
                        GetValue(row, "AssignID"),
                        GetValue(row, "AssignId"),
                        GetValue(row, "ScheduleID"));

                    DateTime installDateValue;
                    string installDateText = GetValue(row, "InstallDate");
                    if (DateTime.TryParse(installDateText, out installDateValue))
                    {
                        installDateText = installDateValue.ToString("yyyy-MM-dd");
                    }

                    DateTime scheduleDateValue;
                    string scheduleDateText = FirstNonEmpty(
                        GetValue(row, "SchDate"),
                        response.SchDate);
                    if (DateTime.TryParse(scheduleDateText, out scheduleDateValue))
                    {
                        scheduleDateText = scheduleDateValue.ToString("yyyy-MM-dd");
                    }

                    string technicianNameFromRow = FirstNonEmpty(
                        GetValue(row, "TechnicianName"),
                        GetValue(row, "Name"),
                        GetValue(row, "TechName"));
                    if (!string.IsNullOrWhiteSpace(technicianNameFromRow))
                    {
                        resolvedTechnicianName = technicianNameFromRow;
                    }

                    rows.Add(new ScheduleReportRowItem
                    {
                        AssignID = assignId,
                        Seq = seq,
                        JobID = GetValue(row, "JobID"),
                        JobType = FirstNonEmpty(
                            GetValue(row, "JobType"),
                            GetValue(row, "Job_Type"),
                            GetValue(row, "MaintTypeID"),
                            "-"),
                        DeviceGroupID = FirstNonEmpty(GetValue(row, "DeviceGroupID"), "-"),
                        DeviceTypeDesc = FirstNonEmpty(GetValue(row, "DeviceTypeDesc"), "-"),
                        CustID = GetValue(row, "CustID"),
                        Customer = FirstNonEmpty(
                            GetValue(row, "FullName"),
                            GetValue(row, "CustomerName"),
                            GetValue(row, "CustID"),
                            "-"),
                        AreaID = FirstNonEmpty(GetValue(row, "AreaID"), GetValue(row, "SupAreaID")),
                        AreaName = FirstNonEmpty(
                            GetValue(row, "AreaName"),
                            GetValue(row, "Area"),
                            "-"),
                        PoliceNo = FirstNonEmpty(GetValue(row, "PoliceNo"), "-"),
                        NoSN = FirstNonEmpty(GetValue(row, "NoSN"), "-"),
                        GSMNo = FirstNonEmpty(GetValue(row, "GSMNo"), "-"),
                        InstallDate = installDateText,
                        MISDate = FirstNonEmpty(GetValue(row, "MIS_Date"), GetValue(row, "MISDate")),
                        SchDate = scheduleDateText,
                        TechnicianName = FirstNonEmpty(technicianNameFromRow, response.TechnicianName, "-"),
                        StatusCode = statusCode,
                        StatusText = isCompleted ? "Selesai" : "Belum Selesai",
                        Remark = FirstNonEmpty(
                            GetValue(row, "Remark"),
                            GetValue(row, "REMARK"),
                            GetValue(row, "Remarks"),
                            string.Empty),
                        CanDelete = !isCompleted && !string.IsNullOrWhiteSpace(assignId)
                    });
                }

                response.Rows = rows;
                response.TechnicianName = FirstNonEmpty(resolvedTechnicianName, response.TechnicianName, "-");
                response.TotalUnitSelesai = rows.Count(r =>
                    string.Equals((r.StatusCode ?? string.Empty).Trim(), "CL", StringComparison.OrdinalIgnoreCase)
                    || string.Equals((r.StatusText ?? string.Empty).Trim(), "Selesai", StringComparison.OrdinalIgnoreCase));
                response.TotalUnitBelumSelesai = Math.Max(0, rows.Count - response.TotalUnitSelesai);
                response.Result = "SUCCESS";
                response.Message = rows.Count > 0 ? "OK" : "Belum ada data schedule.";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat laporan schedule: " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DeleteScheduleAssignResponse DeleteScheduleAssign(string assignId, int seq)
        {
            DeleteScheduleAssignResponse response = new DeleteScheduleAssignResponse
            {
                Result = "ERROR",
                Message = string.Empty
            };

            string safeAssignId = TrimToLength((assignId ?? string.Empty).Trim(), 50);
            if (string.IsNullOrWhiteSpace(safeAssignId))
            {
                response.Message = "AssignID tidak valid.";
                return response;
            }

            if (seq < 0)
            {
                response.Message = "Seq tidak valid.";
                return response;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                int affectRows = 0;
                string executeMessage = string.Empty;
                string sql = "sp_dashboard_assign_job_delete '"
                    + EscapeSqlLiteral(safeAssignId) + "',"
                    + seq.ToString();

                ExecCommand ec = new ExecCommand();
                bool executeOk = ec.Execute(sql, connString.Trim(), ref affectRows, ref executeMessage);
                if (executeOk)
                {
                    response.Result = "SUCCESS";
                    response.Message = string.IsNullOrWhiteSpace(executeMessage)
                        ? "Assign job berhasil dihapus."
                        : executeMessage;
                    return response;
                }

                response.Result = "ERROR";
                response.Message = string.IsNullOrWhiteSpace(executeMessage)
                    ? "Gagal menghapus assign job."
                    : executeMessage;
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal menghapus assign job: " + ex.Message;
            }

            return response;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static RefreshAvailabilityResponse RefreshAvailability(string technicianId, string schDate)
        {
            RefreshAvailabilityResponse response = new RefreshAvailabilityResponse
            {
                Result = "ERROR",
                DisplayValue = "AV",
                CanAssign = true,
                Message = string.Empty
            };
            HttpContext context = HttpContext.Current;

            DateTime schDateValue;
            if (string.IsNullOrWhiteSpace(technicianId) || !DateTime.TryParse(schDate, out schDateValue))
            {
                response.Message = "Parameter refresh tidak valid.";
                return response;
            }

            try
            {
                DataTable dt = GetAvailabilityDataForRefresh(schDateValue.ToString("yyyy-MM"), string.Empty, string.Empty);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.AsEnumerable()
                        .FirstOrDefault(r =>
                            GetValue(r, "TechnicianID").Equals(technicianId, StringComparison.OrdinalIgnoreCase)
                            && ParseIntValue(GetValue(r, "DayNo")) == schDateValue.Day);

                    if (row != null)
                    {
                        string displayValue = GetValue(row, "DisplayValue");
                        int totalJob = ParseIntValue(GetValue(row, "TotalJob"));
                        if (totalJob > 0)
                        {
                            displayValue = totalJob.ToString();
                        }

                        if (string.IsNullOrWhiteSpace(displayValue))
                        {
                            displayValue = "AV";
                        }

                        bool hasRemainingJo = row.Table.Columns.Contains("RemainingJo")
                            || row.Table.Columns.Contains("RemainingJO")
                            || row.Table.Columns.Contains("Remaining_JO")
                            || row.Table.Columns.Contains("RemainingJOCount");
                        int remainingJo = -1;
                        if (hasRemainingJo)
                        {
                            remainingJo = ParseIntFromColumns(row, "RemainingJo", "RemainingJO", "Remaining_JO", "RemainingJOCount");
                        }

                        bool isUnavailable = GetValue(row, "IsAvailable") == "0";
                        bool canAssign = (displayValue.Equals("AV", StringComparison.OrdinalIgnoreCase)
                            || displayValue.Equals("AD", StringComparison.OrdinalIgnoreCase)
                            || displayValue.Equals("OF", StringComparison.OrdinalIgnoreCase))
                            && schDateValue.Date >= DateTime.Today
                            && (!isUnavailable || displayValue.Equals("OF", StringComparison.OrdinalIgnoreCase));
                        bool isTechnicianUser = context != null
                            && context.Session != null
                            && Convert.ToString(context.Session[SessionUserIsTechnician]).Trim() == "1";
                        if (isTechnicianUser)
                        {
                            canAssign = false;
                        }

                        response.Result = "SUCCESS";
                        response.DisplayValue = displayValue;
                        response.CanAssign = canAssign;
                        response.HasRemainingJo = hasRemainingJo;
                        response.RemainingJo = remainingJo;
                        response.Message = "OK";
                        return response;
                    }
                }

                response.Result = "SUCCESS";
                response.DisplayValue = "AV";
                bool canAssignDefault = schDateValue.Date >= DateTime.Today;
                bool isTechnicianUserDefault = context != null
                    && context.Session != null
                    && Convert.ToString(context.Session[SessionUserIsTechnician]).Trim() == "1";
                response.CanAssign = canAssignDefault && !isTechnicianUserDefault;
                response.Message = "Data tidak ditemukan, gunakan nilai default.";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal refresh availability: " + ex.Message;
            }

            return response;
        }

        private static SaveAssignResponse ExecuteSaveAssignOnce(
            string connString,
            string jobId,
            string custId,
            string technicianId,
            DateTime schDate,
            int assignInputUnit,
            string deviceGroupId,
            string areaId,
            string status,
            string usrUpd,
            string insDeviceTypeId)
        {
            SaveAssignResponse response = new SaveAssignResponse
            {
                Result = "ERROR",
                AssignID = string.Empty,
                Message = string.Empty
            };

            int affectRows = 0;
            string executeMessage = string.Empty;
            string saveProcedure = ResolveSaveAssignStoredProcedureName(jobId);
            string sql = saveProcedure + " '"
                + EscapeSqlLiteral(TrimToLength(jobId, 10)) + "','"
                + EscapeSqlLiteral(TrimToLength(custId, 10)) + "','"
                + EscapeSqlLiteral(TrimToLength(technicianId, 10)) + "','"
                + schDate.ToString("yyyy-MM-dd") + "',"
                + assignInputUnit.ToString() + ",'"
                + EscapeSqlLiteral(TrimToLength(deviceGroupId, 10)) + "','"
                + EscapeSqlLiteral(TrimToLength(areaId, 10)) + "','"
                + EscapeSqlLiteral(TrimToLength(status, 10)) + "','"
                + EscapeSqlLiteral((usrUpd ?? string.Empty).Trim()) + "','"
                + EscapeSqlLiteral(TrimToLength(insDeviceTypeId ?? string.Empty, 10)) + "'";

            ExecCommand ec = new ExecCommand();
            bool executeOk = ec.Execute(sql, (connString ?? string.Empty).Trim(), ref affectRows, ref executeMessage);

            if (executeOk)
            {
                response.Result = "SUCCESS";
                response.Message = string.IsNullOrWhiteSpace(executeMessage)
                    ? "Assignment berhasil disimpan."
                    : executeMessage;
                return response;
            }

            response.Result = "ERROR";
            response.Message = string.IsNullOrWhiteSpace(executeMessage)
                ? "Proses save assignment gagal."
                : executeMessage;

            return response;
        }

        protected static string ResolveSaveAssignStoredProcedureName(string jobId)
        {
            if (IsJobTrainingAssignRequestContext()
                && !string.IsNullOrWhiteSpace((jobId ?? string.Empty).Trim()))
            {
                return "sp_dashboard_assign_job_itsupport_save";
            }

            return "sp_dashboard_assign_job_save";
        }

        private static SaveAssignResponse ExecuteUpdateStatusOnlyOnce(
            string connString,
            string technicianId,
            DateTime schDate,
            string status,
            string usrUpd)
        {
            SaveAssignResponse response = new SaveAssignResponse
            {
                Result = "ERROR",
                AssignID = string.Empty,
                Message = string.Empty
            };

            int affectRows = 0;
            string executeMessage = string.Empty;
            string sql = "sp_dashboard_assign_job_update_status '"
                + EscapeSqlLiteral(TrimToLength(technicianId, 10)) + "','"
                + schDate.ToString("yyyy-MM-dd") + "','"
                + EscapeSqlLiteral(TrimToLength(status, 10)) + "','"
                + EscapeSqlLiteral(TrimToLength((usrUpd ?? string.Empty).Trim(), 50)) + "'";

            ExecCommand ec = new ExecCommand();
            bool executeOk = ec.Execute(sql, (connString ?? string.Empty).Trim(), ref affectRows, ref executeMessage);

            if (executeOk)
            {
                response.Result = "SUCCESS";
                response.Message = string.IsNullOrWhiteSpace(executeMessage)
                    ? ("Status " + AssignRoleDisplayName() + " berhasil diperbarui.")
                    : executeMessage;
                return response;
            }

            response.Result = "ERROR";
            response.Message = string.IsNullOrWhiteSpace(executeMessage)
                ? ("Proses ubah status " + AssignRoleDisplayName() + " gagal.")
                : executeMessage;

            return response;
        }

        private static string EscapeSqlLiteral(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }

        private static string TrimToLength(string value, int maxLength)
        {
            string source = (value ?? string.Empty).Trim();
            if (source.Length <= maxLength)
            {
                return source;
            }
            return source.Substring(0, maxLength);
        }

        private static DataTable GetAvailabilityDataForRefresh(string periode, string supAreaId, string areaGroupId)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null || context.Session["ClsTypeDBConnStringSQL"] == null)
            {
                return new DataTable();
            }

            string safePeriode = (periode ?? string.Empty).Replace("'", "''");
            string resolvedSupAreaId;
            string resolvedAreaGroupId;
            ResolveRegionalFiltersFromSession(context, supAreaId, areaGroupId, out resolvedSupAreaId, out resolvedAreaGroupId);
            string safeSupAreaId = resolvedSupAreaId.Replace("'", "''");
            string safeAreaGroupId = resolvedAreaGroupId.Replace("'", "''");

            Recordset rec = new Recordset();
            rec.Open("sp_dashboard_assign_job_availability '" + safePeriode + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'",
                Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]).Trim());

            DataTable dt = rec.DataRecord();
            return dt ?? new DataTable();
        }

        protected static string ResolveCustomerName(DataRow row, string customerId)
        {
            string[] candidates = new[]
            {
                "CustomerName",
                "CustName",
                "NamaCustomer",
                "FullName"
            };

            foreach (string column in candidates)
            {
                string value = GetValue(row, column);
                if (!string.IsNullOrWhiteSpace(value)
                    && !value.Equals(customerId ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DayTotalJoModalResponse GetDayTotalJoList(string scheduleDate)
        {
            return BuildDayTotalJoListResponse(scheduleDate);
        }

        private static DayTotalJoModalResponse BuildDayTotalJoListResponse(string scheduleDate)
        {
            DayTotalJoModalResponse invalidParamResponse = ShowDayTotalJoModal(
                scheduleDate,
                0,
                0,
                BuildClosedJobEmptyStateHtml("Tanggal schedule tidak valid."),
                false,
                "Tanggal schedule tidak valid.");

            DateTime scheduleDateValue;
            if (!DateTime.TryParse(scheduleDate, out scheduleDateValue))
            {
                return invalidParamResponse;
            }

            string safeScheduleDate = scheduleDateValue.ToString("yyyy-MM-dd");
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return ShowDayTotalJoModal(
                    safeScheduleDate,
                    0,
                    0,
                    BuildClosedJobEmptyStateHtml("Session tidak ditemukan."),
                    false,
                    "Session tidak ditemukan.");
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                return ShowDayTotalJoModal(
                    safeScheduleDate,
                    0,
                    0,
                    BuildClosedJobEmptyStateHtml("Koneksi database tidak tersedia."),
                    false,
                    "Koneksi database tidak tersedia.");
            }

            try
            {
                if (IsJobTrainingAssignRequestContext())
                {
                    DataTable trainingDayRows = BuildJobTrainingDayTotalSource(scheduleDateValue);
                    return BindDayTotalJoModal(safeScheduleDate, trainingDayRows);
                }

                string safePeriode = NormalizeClosedJobPeriode(Convert.ToString(context.Session[SessionPeriode]));
                if (string.IsNullOrWhiteSpace(safePeriode))
                {
                    safePeriode = scheduleDateValue.ToString("yyyy-MM");
                }

                string safeSupAreaId;
                string safeAreaGroupId;
                ResolveRegionalFiltersFromSession(context, string.Empty, string.Empty, out safeSupAreaId, out safeAreaGroupId);

                Recordset rec = new Recordset();
                rec.Open(
                    "sp_dashboard_assign_job_daily_jo_detail_list '"
                    + EscapeSqlLiteral(safeScheduleDate) + "','"
                    + EscapeSqlLiteral(safePeriode) + "','"
                    + EscapeSqlLiteral(safeSupAreaId) + "','"
                    + EscapeSqlLiteral(safeAreaGroupId) + "'",
                    connString.Trim());

                DataTable source = rec.DataRecord() ?? new DataTable();
                return BindDayTotalJoModal(safeScheduleDate, source);
            }
            catch (Exception ex)
            {
                return ShowDayTotalJoModal(
                    safeScheduleDate,
                    0,
                    0,
                    BuildClosedJobEmptyStateHtml("Gagal memuat daftar JO per tanggal."),
                    false,
                    "Gagal memuat daftar JO per tanggal: " + ex.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ClosedJobModalResponse GetClosedJobList(string technicianId, string technicianName, string closeType, string periode)
        {
            string invalidIdMessage = IsJobTrainingAssignRequestContext()
                ? "IT Support tidak valid."
                : "TechnicianID tidak valid.";
            ClosedJobModalResponse invalidParamResponse = ShowClosedJobModal(
                technicianName,
                0,
                BuildClosedJobEmptyStateHtml(invalidIdMessage),
                false,
                invalidIdMessage);

            string safeTechnicianId = TrimToLength((technicianId ?? string.Empty).Trim(), 20);
            if (string.IsNullOrWhiteSpace(safeTechnicianId))
            {
                return invalidParamResponse;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return ShowClosedJobModal(
                    technicianName,
                    0,
                    BuildClosedJobEmptyStateHtml("Session tidak ditemukan."),
                    false,
                    "Session tidak ditemukan.");
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                return ShowClosedJobModal(
                    technicianName,
                    0,
                    BuildClosedJobEmptyStateHtml("Koneksi database tidak tersedia."),
                    false,
                    "Koneksi database tidak tersedia.");
            }

            try
            {
                string normalizedCloseType = NormalizeClosedJobType(closeType);
                string normalizedPeriode = NormalizeClosedJobPeriode(periode);

                if (IsJobTrainingAssignRequestContext())
                {
                    DataTable allTrainingClosed = BuildJobTrainingClosedJobSource(
                        technicianId,
                        technicianName,
                        "all",
                        normalizedPeriode);
                    return BindJobTrainingClosedJobModal(technicianName, allTrainingClosed, normalizedCloseType);
                }

                string storedProcedure = normalizedCloseType == "maintenance"
                    ? "sp_dashboard_assign_job_maint_close_list"
                    : "sp_dashboard_assign_job_new_close_list";
                Recordset rec = new Recordset();
                rec.Open(
                    storedProcedure + " '" + EscapeSqlLiteral(normalizedPeriode) + "','" + EscapeSqlLiteral(safeTechnicianId) + "'",
                    connString.Trim());

                DataTable source = rec.DataRecord() ?? new DataTable();
                return BindClosedJobModal(technicianName, source);
            }
            catch (Exception ex)
            {
                return ShowClosedJobModal(
                    technicianName,
                    0,
                    BuildClosedJobEmptyStateHtml("Gagal memuat daftar pekerjaan selesai."),
                    false,
                    "Gagal memuat daftar pekerjaan selesai: " + ex.Message);
            }
        }

        private static void ResolveRegionalFiltersFromSession(
            HttpContext context,
            string supAreaId,
            string areaGroupId,
            out string resolvedSupAreaId,
            out string resolvedAreaGroupId)
        {
            resolvedSupAreaId = (supAreaId ?? string.Empty).Trim();
            resolvedAreaGroupId = NormalizeAreaGroupId(areaGroupId).Trim();
            if (context == null || context.Session == null)
            {
                return;
            }

            string selectedAreaGroupTab = Convert.ToString(context.Session[GetRegionalGroupTabSessionKeyStatic()]).Trim();
            if (resolvedSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrWhiteSpace(resolvedSupAreaId))
            {
                string selectedRegionalTab = Convert.ToString(context.Session[GetRegionalTabSessionKeyStatic()]).Trim();
                if (!string.IsNullOrWhiteSpace(selectedRegionalTab)
                    && !selectedRegionalTab.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
                {
                    resolvedSupAreaId = selectedRegionalTab;
                }
            }

            if (resolvedSupAreaId.Equals(RegionalAllValue, StringComparison.OrdinalIgnoreCase))
            {
                resolvedSupAreaId = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(resolvedAreaGroupId))
            {
                resolvedAreaGroupId = NormalizeAreaGroupId(selectedAreaGroupTab);
            }
        }

        private static DayTotalJoModalResponse BindDayTotalJoModal(string scheduleDate, DataTable source)
        {
            // Align with day-column Total JO: count distinct JobID (fallback row count).
            // Total Unit = sum QtyGPS (fallback row count when QtyGPS is empty).
            int totalJo = CountDistinctJobIds(source);
            if (totalJo <= 0)
            {
                totalJo = source != null ? source.Rows.Count : 0;
            }

            int totalUnit = SumQtyGpsFromTable(source);
            if (totalUnit <= 0)
            {
                totalUnit = source != null ? source.Rows.Count : 0;
            }

            bool hasData = totalJo > 0 || totalUnit > 0;
            string html = hasData
                ? BuildDayTotalJoTableHtml(source)
                : BuildClosedJobEmptyStateHtml("Belum ada JO pada tanggal ini.");

            return ShowDayTotalJoModal(
                scheduleDate,
                totalJo,
                totalUnit,
                html,
                hasData,
                hasData ? "OK" : "Belum ada JO pada tanggal ini.");
        }

        private static DataTable BuildJobTrainingDayTotalSource(DateTime scheduleDate)
        {
            DataTable mapped = new DataTable();
            mapped.Columns.Add("JobID");
            mapped.Columns.Add("CustID");
            mapped.Columns.Add("CustomerName");
            mapped.Columns.Add("FullName");
            mapped.Columns.Add("JobType");
            mapped.Columns.Add("Status");
            mapped.Columns.Add("Remark");
            mapped.Columns.Add("QtyGPS", typeof(int));
            mapped.Columns.Add("ScheduleDate");
            mapped.Columns.Add("AreaName");

            DateTime dayStart = scheduleDate.Date;
            DataTable source = LoadTrxJobAssignDetailRows(dayStart, dayStart.AddDays(1));
            if (source == null || source.Rows.Count == 0)
            {
                return mapped;
            }

            Dictionary<string, JobTrainingTrainerSchedule> itsUsers = BuildItsUserScheduleMap(31);
            HashSet<string> validItIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (JobTrainingTrainerSchedule user in itsUsers.Values)
            {
                if (IsValidItId(user.ItId))
                {
                    validItIds.Add(NormalizeItId(user.ItId));
                }
            }

            string targetDate = scheduleDate.ToString("yyyy-MM-dd");
            foreach (DataRow row in source.Rows)
            {
                string technicianId = FirstNonEmptyStatic(
                    GetValue(row, "TechnicianID"),
                    GetValue(row, "TechnicianId"),
                    GetValue(row, "ITID"));
                if (!IsValidItId(technicianId) || (validItIds.Count > 0 && !validItIds.Contains(NormalizeItId(technicianId))))
                {
                    continue;
                }

                string schDateRaw = FirstNonEmptyStatic(
                    GetValue(row, "SchDate"),
                    GetValue(row, "ScheduleDate"),
                    GetValue(row, "sSchDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate) || schDate.ToString("yyyy-MM-dd") != targetDate)
                {
                    continue;
                }

                bool isVisit = IsAssignDetailVisitOrMaint(row);
                string customerName = FirstNonEmptyStatic(
                    GetValue(row, "CustomerName"),
                    GetValue(row, "Customer"),
                    GetValue(row, "FullName"),
                    GetValue(row, "CustID"));
                int qtyGps = ParseIntValue(FirstNonEmptyStatic(GetValue(row, "QtyGPS"), "1"));
                if (qtyGps <= 0)
                {
                    qtyGps = 1;
                }

                DataRow target = mapped.NewRow();
                target["JobID"] = FirstNonEmptyStatic(GetValue(row, "JobID"), "-");
                target["CustID"] = GetValue(row, "CustID");
                target["CustomerName"] = customerName;
                target["FullName"] = customerName;
                target["JobType"] = isVisit ? "Visit" : "Training";
                target["Status"] = FirstNonEmptyStatic(GetValue(row, "Status"), GetValue(row, "StatusCode"));
                target["Remark"] = FirstNonEmptyStatic(GetValue(row, "Remark"), GetValue(row, "TechnicianID"));
                target["QtyGPS"] = qtyGps;
                target["ScheduleDate"] = schDate.ToString("yyyy-MM-dd");
                target["AreaName"] = FirstNonEmptyStatic(GetValue(row, "AreaName"), GetValue(row, "BranchName"));
                mapped.Rows.Add(target);
            }

            return mapped;
        }

        private static int CountDistinctJobIds(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return 0;
            }

            return source.AsEnumerable()
                .Select(row => GetValue(row, "JobID"))
                .Where(jobId => !string.IsNullOrWhiteSpace(jobId))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();
        }

        private static int SumQtyGpsFromTable(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return 0;
            }

            return source.AsEnumerable()
                .Sum(row => ParseIntValue(GetValue(row, "QtyGPS")));
        }

        private static DayTotalJoModalResponse ShowDayTotalJoModal(
            string scheduleDate,
            int totalJo,
            int totalUnit,
            string htmlContent,
            bool hasData,
            string message)
        {
            return new DayTotalJoModalResponse
            {
                Result = "SUCCESS",
                Message = message ?? string.Empty,
                ScheduleDate = string.IsNullOrWhiteSpace(scheduleDate) ? "-" : scheduleDate.Trim(),
                TotalJO = Math.Max(0, totalJo),
                TotalUnit = Math.Max(0, totalUnit),
                HasData = hasData,
                HtmlContent = htmlContent ?? string.Empty
            };
        }

        private static string BuildDayTotalJoTableHtml(DataTable source)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div class=\"assign-closed-table-wrap\">");
            html.Append("<table class=\"assign-closed-table\">");
            html.Append("<thead><tr>");
            html.Append("<th>No</th>");
            html.Append("<th>JobID</th>");
            html.Append("<th>FullName</th>");
            html.Append("<th>JobType</th>");
            html.Append("<th>Job Date</th>");
            html.Append("<th>ScheduleDate</th>");
            html.Append("<th>Area</th>");
            html.Append("<th>Total GPS</th>");
            html.Append("<th>Status</th>");
            html.Append("<th>Remark</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            if (source != null)
            {
                for (int i = 0; i < source.Rows.Count; i++)
                {
                    DataRow row = source.Rows[i];
                    html.Append("<tr>");
                    html.Append("<td>" + (i + 1).ToString() + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "JobID"), "-")) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(
                        GetValue(row, "CustomerName"),
                        GetValue(row, "FullName"),
                        GetValue(row, "CustID"),
                        "-")) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "JobType"), "-")) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForTable(FirstNonEmpty(
                        GetValue(row, "RegDate"),
                        GetValue(row, "DtmUpd"),
                        GetValue(row, "JobDate")))) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForTable(GetValue(row, "ScheduleDate"))) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(
                        GetValue(row, "AreaName"),
                        GetValue(row, "AreaID"),
                        "-")) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(ParseIntValue(GetValue(row, "QtyGPS")).ToString()) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "Status"), "-")) + "</td>");
                    html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "Remark"), "-")) + "</td>");
                    html.Append("</tr>");
                }
            }

            html.Append("</tbody></table></div>");
            return html.ToString();
        }

        private static string FormatDateForTable(string source)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(source, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd");
            }

            return string.IsNullOrWhiteSpace(source) ? "-" : source.Trim();
        }

        private static DataTable BuildJobTrainingClosedJobSource(
            string technicianId,
            string technicianName,
            string closeType,
            string periode)
        {
            DataTable mapped = new DataTable();
            mapped.Columns.Add("JobID");
            mapped.Columns.Add("AssignID");
            mapped.Columns.Add("JobType");
            mapped.Columns.Add("TechnicianID");
            mapped.Columns.Add("TechnicianName");
            mapped.Columns.Add("DeviceGroupID");
            mapped.Columns.Add("DeviceTypeDesc");
            mapped.Columns.Add("SchDate");
            mapped.Columns.Add("CustID");
            mapped.Columns.Add("CustomerName");
            mapped.Columns.Add("FullName");
            mapped.Columns.Add("AreaName");
            mapped.Columns.Add("Status");
            mapped.Columns.Add("TotalUnit", typeof(int));

            DateTime periodDate;
            if (!DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodDate))
            {
                periodDate = DateTime.Now;
            }

            string normalizedPeriode = periodDate.ToString("yyyy-MM");
            DataTable source = LoadJobTrainingRowsForSchedule(normalizedPeriode);
            if (source == null || source.Rows.Count == 0)
            {
                return mapped;
            }

            string closeTypeNorm = (closeType ?? string.Empty).Trim().ToLowerInvariant();
            bool includeAllTypes = string.IsNullOrWhiteSpace(closeTypeNorm)
                || closeTypeNorm == "all"
                || closeTypeNorm == "both";
            bool wantVisit = closeTypeNorm == "maintenance" || closeTypeNorm == "visit";

            Dictionary<string, JobTrainingTrainerSchedule> itsUsers = BuildItsUserScheduleMap(31);
            string wantedUserId = TrimToLength((technicianId ?? string.Empty).Trim(), 20);
            string wantedName = (technicianName ?? string.Empty).Trim();
            Dictionary<string, string> technicianByJobId = BuildJobTechnicianIdByJobMap(periodDate);

            foreach (DataRow row in source.Rows)
            {
                if (!JobMatchesTechnicianId(row, wantedUserId, wantedName, itsUsers, technicianByJobId))
                {
                    continue;
                }

                string categoryName = FirstNonEmptyStatic(
                    GetValue(row, "TrainingCategoryName"),
                    GetValue(row, "TrainCategoryName"),
                    GetValue(row, "CategoryName"),
                    GetValue(row, "Category"));
                string categoryId = FirstNonEmptyStatic(
                    GetValue(row, "TrainCategoryID"),
                    GetValue(row, "TrainingCategoryID"),
                    GetValue(row, "CategoryID"));
                if (!IsJobTrainingScheduleCategory(row, categoryName, categoryId))
                {
                    continue;
                }

                bool isVisit = IsVisitCategory(categoryName, categoryId);
                if (!includeAllTypes && wantVisit != isVisit)
                {
                    continue;
                }

                string schDateRaw = FirstNonEmptyStatic(
                    GetValue(row, "sSchDate"),
                    GetValue(row, "SchDate"),
                    GetValue(row, "ScheduleDate"));
                DateTime schDate;
                if (!TryParseTrainingDate(schDateRaw, out schDate)
                    || schDate.Year != periodDate.Year
                    || schDate.Month != periodDate.Month)
                {
                    continue;
                }

                string customerName = FirstNonEmptyStatic(
                    GetValue(row, "CustomerName"),
                    GetValue(row, "CustName"),
                    GetValue(row, "FullName"),
                    GetValue(row, "CustID"));
                string jobId = FirstNonEmptyStatic(GetValue(row, "TrainingID"), GetValue(row, "JobID"));
                string rawStatus = FirstNonEmptyStatic(
                    GetValue(row, "Status"),
                    GetValue(row, "ValueStatus"),
                    GetValue(row, "StatusCode"));

                DataRow target = mapped.NewRow();
                target["JobID"] = jobId;
                target["AssignID"] = jobId;
                target["JobType"] = isVisit ? "Visit" : "Training";
                target["TechnicianID"] = FirstNonEmptyStatic(wantedUserId, "-");
                target["TechnicianName"] = FirstNonEmptyStatic(wantedName, "-");
                target["DeviceGroupID"] = "-";
                target["DeviceTypeDesc"] = isVisit ? "Visit" : "Training";
                target["SchDate"] = schDate.ToString("yyyy-MM-dd");
                target["CustID"] = GetValue(row, "CustID");
                target["CustomerName"] = customerName;
                target["FullName"] = customerName;
                target["AreaName"] = FirstNonEmptyStatic(GetValue(row, "BranchName"), GetValue(row, "AreaName"), "-");
                target["Status"] = rawStatus;
                target["TotalUnit"] = 1;
                mapped.Rows.Add(target);
            }

            return mapped;
        }

        private static ClosedJobModalResponse BindJobTrainingClosedJobModal(
            string technicianName,
            DataTable allJobsSource,
            string closeType)
        {
            int closedTraining = CountDistinctClosedJobsByType(allJobsSource, false);
            int closedVisit = CountDistinctClosedJobsByType(allJobsSource, true);

            bool wantVisit = NormalizeClosedJobType(closeType) == "maintenance";
            DataTable listSource = FilterJobTrainingClosedJobsByType(allJobsSource, wantVisit);
            bool hasData = listSource != null && listSource.Rows.Count > 0;
            string html = hasData
                ? BuildJobTrainingJobsTableHtml(listSource)
                : BuildClosedJobEmptyStateHtml(wantVisit
                    ? "Belum ada Job Visit untuk IT Support ini pada periode terpilih."
                    : "Belum ada Job Training untuk IT Support ini pada periode terpilih.");

            ClosedJobModalResponse response = ShowClosedJobModal(
                technicianName,
                closedTraining,
                html,
                hasData,
                hasData ? "OK" : "Belum ada data",
                closedVisit);
            response.TotalClosedJO = closedTraining;
            response.TotalClosedUnit = closedVisit;
            return response;
        }

        private static string BuildJobTrainingJobsTableHtml(DataTable source)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div class=\"assign-closed-table-wrap\">");
            html.Append("<table class=\"assign-closed-table\">");
            html.Append("<thead><tr>");
            html.Append("<th>Training ID</th>");
            html.Append("<th>Customer</th>");
            html.Append("<th>Category</th>");
            html.Append("<th>Sch Date</th>");
            html.Append("<th>Branch</th>");
            html.Append("<th>Status</th>");
            html.Append("</tr></thead><tbody>");

            foreach (DataRow row in source.Rows)
            {
                string statusCode = FirstNonEmpty(GetValue(row, "Status"), "-");
                string statusNorm = NormalizeJobTrainingStatusCode(statusCode);
                string statusLabel = FormatJobTrainingStatusLabel(statusCode, statusNorm);
                string statusBadgeClass = statusNorm == "close"
                    ? "assign-report-status done"
                    : (statusNorm == "open" ? "assign-report-status pending" : "assign-report-status");

                html.Append("<tr>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "JobID"), "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "FullName"), GetValue(row, "CustomerName"), GetValue(row, "CustID"), "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "JobType"), GetValue(row, "DeviceTypeDesc"), "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForDisplay(GetValue(row, "SchDate"))) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(GetValue(row, "AreaName"), "-")) + "</td>");
                html.Append("<td><span class=\"" + statusBadgeClass + "\">" + HttpUtility.HtmlEncode(statusLabel) + "</span></td>");
                html.Append("</tr>");
            }

            html.Append("</tbody></table></div>");
            return html.ToString();
        }

        private static string FormatJobTrainingStatusLabel(string statusCode, string statusNorm)
        {
            string code = FirstNonEmpty(statusCode, "-").Trim().ToUpperInvariant();
            if (statusNorm == "close")
            {
                return code + " - Closed";
            }

            if (statusNorm == "open")
            {
                return code + " - Open";
            }

            if (statusNorm == "scheduled")
            {
                return code + " - Scheduled";
            }

            return code;
        }

        private static int CountDistinctClosedJobsByType(DataTable source, bool visitOnly)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return 0;
            }

            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in source.Rows)
            {
                if (!IsClosedJobTrainingStatus(GetValue(row, "Status")))
                {
                    continue;
                }

                string jobType = GetValue(row, "JobType");
                bool isVisit = jobType.IndexOf("Visit", StringComparison.OrdinalIgnoreCase) >= 0;
                if (visitOnly != isVisit)
                {
                    continue;
                }

                string jobId = GetValue(row, "JobID");
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    jobId = "__ROW__" + ids.Count.ToString();
                }

                ids.Add(jobId);
            }

            return ids.Count;
        }

        private static DataTable FilterJobTrainingClosedJobsByType(DataTable source, bool visitOnly)
        {
            if (source == null)
            {
                return new DataTable();
            }

            DataTable filtered = source.Clone();
            foreach (DataRow row in source.Rows)
            {
                string jobType = GetValue(row, "JobType");
                bool isVisit = jobType.IndexOf("Visit", StringComparison.OrdinalIgnoreCase) >= 0;
                if (visitOnly == isVisit)
                {
                    filtered.ImportRow(row);
                }
            }

            return filtered;
        }

        private static ClosedJobModalResponse BindClosedJobModal(string technicianName, DataTable source)
        {
            List<ClosedJobUnitItem> rows = new List<ClosedJobUnitItem>();
            if (source != null && source.Rows.Count > 0)
            {
                rows = source.AsEnumerable()
                    .Select(row => new ClosedJobUnitItem
                    {
                        AssignID = FirstNonEmpty(GetValue(row, "AssignID"), GetValue(row, "AssignId")),
                        Seq = ParseIntValue(GetValue(row, "Seq")),
                        JobID = GetValue(row, "JobID"),
                        TvdID = GetValue(row, "TvdID"),
                        JobType = FirstNonEmpty(
                            GetValue(row, "JobType"),
                            GetValue(row, "Job_Type"),
                            GetValue(row, "MaintTypeID"),
                            "-"),
                        MaintTypeID = GetValue(row, "MaintTypeID"),
                        TechnicianID = GetValue(row, "TechnicianID"),
                        DeviceGroupID = FirstNonEmpty(GetValue(row, "DeviceGroupID"), "-"),
                        DeviceTypeID = GetValue(row, "DeviceTypeID"),
                        DeviceTypeDesc = FirstNonEmpty(GetValue(row, "DeviceTypeDesc"), "-"),
                        SchDate = GetValue(row, "SchDate"),
                        InstallDate = GetValue(row, "InstallDate"),
                        MISDate = FirstNonEmpty(GetValue(row, "MIS_Date"), GetValue(row, "MISDate")),
                        CustID = GetValue(row, "CustID"),
                        FullName = FirstNonEmpty(GetValue(row, "FullName"), GetValue(row, "CustomerName"), GetValue(row, "CustID"), "-"),
                        Area = FirstNonEmpty(GetValue(row, "AreaName"), GetValue(row, "Area"), "-"),
                        PoliceNo = FirstNonEmpty(GetValue(row, "PoliceNo"), "-"),
                        NoSN = FirstNonEmpty(GetValue(row, "NoSN"), "-"),
                        GSMNo = FirstNonEmpty(GetValue(row, "GSMNo"), "-"),
                        Status = GetValue(row, "Status"),
                        TechnicianName = FirstNonEmpty(
                            GetValue(row, "TechnicianName"),
                            GetValue(row, "Name"),
                            technicianName,
                            GetValue(row, "TechnicianID"),
                            "-"),
                        TotalUnit = ParseIntValue(GetValue(row, "TotalUnit"))
                    })
                    .ToList();
            }

            int totalClosedJo = rows
                .Select(r => r.JobID ?? string.Empty)
                .Where(jobId => !string.IsNullOrWhiteSpace(jobId))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();
            if (totalClosedJo <= 0)
            {
                totalClosedJo = rows.Count;
            }

            bool hasData = rows.Count > 0;
            int totalClosedUnit = rows
                .GroupBy(r => FirstNonEmpty(r.JobID, r.AssignID, "-"))
                .Sum(g =>
                {
                    int maxUnit = g.Max(x => x.TotalUnit);
                    return maxUnit > 0 ? maxUnit : g.Count();
                });
            string html = hasData
                ? BuildClosedJobCardsHtml(rows)
                : BuildClosedJobEmptyStateHtml("Belum ada pekerjaan selesai");

            return ShowClosedJobModal(technicianName, totalClosedJo, html, hasData, hasData ? "OK" : "Belum ada pekerjaan selesai", totalClosedUnit);
        }

        private static ClosedJobModalResponse ShowClosedJobModal(string technicianName, int totalClosedJo, string htmlContent, bool hasData, string message, int totalClosedUnit = 0)
        {
            return new ClosedJobModalResponse
            {
                Result = "SUCCESS",
                Message = message ?? string.Empty,
                TechnicianName = string.IsNullOrWhiteSpace(technicianName) ? "-" : technicianName.Trim(),
                TotalClosedJO = Math.Max(0, totalClosedJo),
                TotalClosedUnit = Math.Max(0, totalClosedUnit),
                HasData = hasData,
                HtmlContent = htmlContent ?? string.Empty
            };
        }

        private static string BuildClosedJobCardsHtml(List<ClosedJobUnitItem> units)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<div class=\"assign-closed-table-wrap\">");
            html.Append("<table class=\"assign-closed-table\">");
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th>Customer</th>");
            html.Append("<th>Job Order</th>");
            html.Append("<th>Job Type</th>");
            html.Append("<th>DeviceGroup</th>");
            html.Append("<th>Device Type</th>");
            html.Append("<th>Area</th>");
            html.Append("<th>Nomor Polisi</th>");
            html.Append("<th>No SN</th>");
            html.Append("<th>No GSM</th>");
            html.Append("<th>Schedule Date</th>");
            html.Append("<th>Tanggal Instalasi</th>");
            html.Append("<th>Tanggal MIS</th>");
            html.Append("<th>Dikerjakan Oleh</th>");
            html.Append("<th>Status</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");

            foreach (ClosedJobUnitItem unit in units)
            {
                string statusCode = FirstNonEmpty(unit.Status, "-");
                string statusText = statusCode.Equals("CL", StringComparison.OrdinalIgnoreCase) ? "Selesai" : statusCode;
                string statusBadgeClass = statusCode.Equals("CL", StringComparison.OrdinalIgnoreCase)
                    ? "assign-report-status done"
                    : "assign-report-status pending";

                html.Append("<tr>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.FullName, unit.CustID, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.JobID, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.JobType, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.DeviceGroupID, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.DeviceTypeDesc, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.Area, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.PoliceNo, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.NoSN, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.GSMNo, "-")) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForDisplay(unit.SchDate)) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForDisplay(unit.InstallDate)) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FormatDateForDisplay(unit.MISDate)) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(FirstNonEmpty(unit.TechnicianName, unit.TechnicianID, "-")) + "</td>");
                html.Append("<td><span class=\"" + statusBadgeClass + "\">" + HttpUtility.HtmlEncode(statusText) + "</span></td>");
                html.Append("</tr>");
            }

            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</div>");
            return html.ToString();
        }

        private static string NormalizeClosedJobType(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToLowerInvariant();
            if (normalized == "maint" || normalized == "maintenance")
            {
                return "maintenance";
            }

            return "new_install";
        }

        private static string NormalizeClosedJobPeriode(string value)
        {
            string source = (value ?? string.Empty).Trim();
            DateTime parsedDate;
            if (DateTime.TryParseExact(source, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM");
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

        private static string BuildClosedJobEmptyStateHtml(string message)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div class=\"assign-closed-empty\">");
            html.Append("<div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#128229;</div>");
            html.Append("<p>" + HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(message) ? "Belum ada pekerjaan selesai" : message) + "</p>");
            html.Append("</div>");
            return html.ToString();
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

        private static string NormalizeDeviceStatusText(string statusCode)
        {
            string normalized = (statusCode ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized == "MT")
            {
                return "Ready";
            }

            if (string.IsNullOrWhiteSpace(statusCode))
            {
                return "-";
            }

            return statusCode.Trim();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PerfTechnicianListResponse LoadPerfTechnicianList(string periode)
        {
            PerfTechnicianListResponse response = new PerfTechnicianListResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                Rows = new List<PerfTechnicianItem>()
            };

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                if (IsJobTrainingAssignRequestContext())
                {
                    response.Rows = BuildItsPerfTechnicianListRows();
                    response.Result = "SUCCESS";
                    response.Message = response.Rows.Count > 0
                        ? "OK"
                        : ("Belum ada " + AssignRoleDisplayName() + " pada filter ini.");
                    return response;
                }

                DataTable source = ExecutePerfScopeStoredProcedure(
                    "sp_dashboard_assign_job_perf_technician_list",
                    NormalizeClosedJobPeriode(periode),
                    context,
                    connString.Trim());

                if (source != null && source.Rows.Count > 0)
                {
                    response.Rows = source.AsEnumerable()
                        .Select(row => new PerfTechnicianItem
                        {
                            TechnicianID = GetValue(row, "TechnicianID"),
                            TechnicianName = FirstNonEmpty(GetValue(row, "Name"), GetValue(row, "TechnicianName"), GetValue(row, "TechnicianID"), "-")
                        })
                        .Where(item => !string.IsNullOrWhiteSpace(item.TechnicianID))
                        .ToList();
                }

                response.Result = "SUCCESS";
                response.Message = response.Rows.Count > 0 ? "OK" : ("Belum ada " + AssignRoleDisplayName() + " pada filter ini.");
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat daftar " + AssignRoleDisplayName() + ": " + ex.Message;
            }

            return response;
        }

        private static List<PerfTechnicianItem> BuildItsPerfTechnicianListRows()
        {
            Dictionary<string, JobTrainingTrainerSchedule> users = BuildItsUserScheduleMap(31);
            if (users == null || users.Count == 0)
            {
                return new List<PerfTechnicianItem>();
            }

            return users.Values
                .GroupBy(u => u.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .Where(u => u != null
                    && !string.IsNullOrWhiteSpace(u.TrainerId)
                    && !u.TrainerId.Equals("UNASSIGNED", StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.TrainerName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(u => u.TrainerId, StringComparer.OrdinalIgnoreCase)
                .Select(u => new PerfTechnicianItem
                {
                    TechnicianID = u.TrainerId,
                    TechnicianName = FirstNonEmptyStatic(u.TrainerName, u.TrainerId, "-")
                })
                .ToList();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PerfChartDataResponse LoadPerfChartData(string periode, string technicianId)
        {
            PerfChartDataResponse response = new PerfChartDataResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                TechnicianID = TrimToLength((technicianId ?? string.Empty).Trim(), 20),
                TechnicianName = string.Empty,
                DailyRows = new List<PerfDailyRowItem>(),
                MonthlyRows = new List<PerfMonthlyRowItem>()
            };

            if (string.IsNullOrWhiteSpace(response.TechnicianID))
            {
                response.Message = AssignRoleDisplayName() + " tidak valid.";
                return response;
            }

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            try
            {
                if (IsJobTrainingAssignRequestContext())
                {
                    return BuildItsPerfChartDataResponse(NormalizeClosedJobPeriode(periode), response.TechnicianID);
                }

                string safePeriode = NormalizeClosedJobPeriode(periode);
                DataTable dailySource = ExecutePerfScopeStoredProcedure(
                    "sp_dashboard_assign_job_perf_daily",
                    safePeriode,
                    context,
                    connString.Trim(),
                    response.TechnicianID);
                DataTable monthlySource = ExecutePerfScopeStoredProcedure(
                    "sp_dashboard_assign_job_perf_monthly",
                    safePeriode,
                    context,
                    connString.Trim(),
                    response.TechnicianID);

                string technicianName = string.Empty;
                response.DailyRows = MapPerfDailyRows(dailySource, response.TechnicianID, out technicianName);
                response.MonthlyRows = MapPerfMonthlyRows(monthlySource, response.TechnicianID, ref technicianName);
                response.TechnicianName = technicianName;
                response.Result = "SUCCESS";
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat data grafik kinerja: " + ex.Message;
            }

            return response;
        }

        private static PerfChartDataResponse BuildItsPerfChartDataResponse(string periode, string userIdOrItId)
        {
            PerfChartDataResponse response = new PerfChartDataResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                TechnicianID = TrimToLength((userIdOrItId ?? string.Empty).Trim(), 20),
                TechnicianName = string.Empty,
                DailyRows = new List<PerfDailyRowItem>(),
                MonthlyRows = new List<PerfMonthlyRowItem>()
            };

            string selectedUserId = response.TechnicianID;
            string technicianName = selectedUserId;
            string itId = string.Empty;

            Dictionary<string, JobTrainingTrainerSchedule> users = BuildItsUserScheduleMap(31);
            JobTrainingTrainerSchedule matched = null;
            if (users != null)
            {
                if (users.TryGetValue(selectedUserId, out matched) && matched != null)
                {
                    selectedUserId = matched.TrainerId;
                    technicianName = FirstNonEmptyStatic(matched.TrainerName, selectedUserId);
                    itId = matched.ItId;
                }
                else
                {
                    matched = users.Values
                        .FirstOrDefault(u => u != null
                            && (string.Equals(u.TrainerId, selectedUserId, StringComparison.OrdinalIgnoreCase)
                                || string.Equals(u.ItId, selectedUserId, StringComparison.OrdinalIgnoreCase)));
                    if (matched != null)
                    {
                        selectedUserId = matched.TrainerId;
                        technicianName = FirstNonEmptyStatic(matched.TrainerName, selectedUserId);
                        itId = matched.ItId;
                    }
                }
            }

            if (!IsValidItId(itId))
            {
                itId = NormalizeItId(LookupItIdByUserId(selectedUserId));
            }
            if (!IsValidItId(itId) && IsValidItId(selectedUserId))
            {
                itId = NormalizeItId(selectedUserId);
            }

            response.TechnicianID = selectedUserId;
            response.TechnicianName = technicianName;

            if (!IsValidItId(itId))
            {
                response.Result = "SUCCESS";
                response.Message = "ITID not exist";
                response.DailyRows = BuildEmptyItsPerfDailyRows(periode);
                response.MonthlyRows = BuildEmptyItsPerfMonthlyRows(periode);
                return response;
            }

            DateTime periodMonth;
            if (!DateTime.TryParseExact((periode ?? string.Empty).Trim() + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodMonth)
                && !DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodMonth))
            {
                periodMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            periodMonth = new DateTime(periodMonth.Year, periodMonth.Month, 1);

            DateTime monthlyStart = periodMonth.AddMonths(-5);
            DateTime monthlyEndExclusive = periodMonth.AddMonths(1);
            DataTable details = LoadTrxJobAssignDetailRows(monthlyStart, monthlyEndExclusive);

            Dictionary<string, PerfDailyRowItem> dailyMap = new Dictionary<string, PerfDailyRowItem>(StringComparer.OrdinalIgnoreCase);
            int daysInMonth = DateTime.DaysInMonth(periodMonth.Year, periodMonth.Month);
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dayDate = new DateTime(periodMonth.Year, periodMonth.Month, day);
                string key = dayDate.ToString("yyyy-MM-dd");
                dailyMap[key] = new PerfDailyRowItem
                {
                    SchDate = key,
                    DayNo = day
                };
            }

            Dictionary<string, PerfMonthlyRowItem> monthlyMap = new Dictionary<string, PerfMonthlyRowItem>(StringComparer.OrdinalIgnoreCase);
            for (int offset = 0; offset < 6; offset++)
            {
                DateTime monthCursor = monthlyStart.AddMonths(offset);
                string key = monthCursor.ToString("yyyy-MM");
                monthlyMap[key] = new PerfMonthlyRowItem
                {
                    PeriodeMonth = key
                };
            }

            string normalizedItId = NormalizeItId(itId);
            if (details != null)
            {
                foreach (DataRow row in details.Rows)
                {
                    string rowTechId = NormalizeItId(FirstNonEmptyStatic(
                        GetValue(row, "TechnicianID"),
                        GetValue(row, "TechnicianId"),
                        GetValue(row, "ITID")));
                    if (!rowTechId.Equals(normalizedItId, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string schDateRaw = FirstNonEmptyStatic(
                        GetValue(row, "SchDate"),
                        GetValue(row, "ScheduleDate"),
                        GetValue(row, "sSchDate"));
                    DateTime schDate;
                    if (!TryParseTrainingDate(schDateRaw, out schDate))
                    {
                        continue;
                    }

                    bool isVisit = IsAssignDetailVisitOrMaint(row);
                    bool isClosed = IsClosedAssignDetailStatus(FirstNonEmptyStatic(
                        GetValue(row, "Status"),
                        GetValue(row, "StatusCode"),
                        GetValue(row, "ValueStatus")));

                    if (schDate.Year == periodMonth.Year && schDate.Month == periodMonth.Month)
                    {
                        string dayKey = schDate.ToString("yyyy-MM-dd");
                        PerfDailyRowItem daily;
                        if (!dailyMap.TryGetValue(dayKey, out daily) || daily == null)
                        {
                            daily = new PerfDailyRowItem
                            {
                                SchDate = dayKey,
                                DayNo = schDate.Day
                            };
                            dailyMap[dayKey] = daily;
                        }

                        ApplyItsPerfCounters(daily, isVisit, isClosed);
                    }

                    string monthKey = schDate.ToString("yyyy-MM");
                    PerfMonthlyRowItem monthly;
                    if (monthlyMap.TryGetValue(monthKey, out monthly) && monthly != null)
                    {
                        ApplyItsPerfCounters(monthly, isVisit, isClosed);
                    }
                }
            }

            response.DailyRows = dailyMap.Values
                .OrderBy(r => r.DayNo)
                .ToList();
            response.MonthlyRows = monthlyMap.Values
                .OrderBy(r => r.PeriodeMonth, StringComparer.OrdinalIgnoreCase)
                .ToList();
            response.Result = "SUCCESS";
            response.Message = "OK";
            return response;
        }

        private static void ApplyItsPerfCounters(PerfDailyRowItem row, bool isVisit, bool isClosed)
        {
            if (row == null)
            {
                return;
            }

            if (isVisit)
            {
                row.JoAssignMaint++;
                row.JoAssignTotal++;
                if (isClosed)
                {
                    row.JoCloseMaint++;
                    row.UnitCloseMaint++;
                    row.JoCloseTotal++;
                    row.UnitCloseTotal++;
                }
            }
            else
            {
                row.JoAssignNew++;
                row.JoAssignTotal++;
                if (isClosed)
                {
                    row.JoCloseNew++;
                    row.UnitCloseNew++;
                    row.JoCloseTotal++;
                    row.UnitCloseTotal++;
                }
            }
        }

        private static void ApplyItsPerfCounters(PerfMonthlyRowItem row, bool isVisit, bool isClosed)
        {
            if (row == null)
            {
                return;
            }

            if (isVisit)
            {
                row.JoAssignMaint++;
                row.JoAssignTotal++;
                if (isClosed)
                {
                    row.JoCloseMaint++;
                    row.UnitCloseMaint++;
                    row.JoCloseTotal++;
                    row.UnitCloseTotal++;
                }
            }
            else
            {
                row.JoAssignNew++;
                row.JoAssignTotal++;
                if (isClosed)
                {
                    row.JoCloseNew++;
                    row.UnitCloseNew++;
                    row.JoCloseTotal++;
                    row.UnitCloseTotal++;
                }
            }
        }

        private static List<PerfDailyRowItem> BuildEmptyItsPerfDailyRows(string periode)
        {
            DateTime periodMonth;
            if (!DateTime.TryParseExact((periode ?? string.Empty).Trim() + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodMonth)
                && !DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodMonth))
            {
                periodMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            List<PerfDailyRowItem> rows = new List<PerfDailyRowItem>();
            int daysInMonth = DateTime.DaysInMonth(periodMonth.Year, periodMonth.Month);
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dayDate = new DateTime(periodMonth.Year, periodMonth.Month, day);
                rows.Add(new PerfDailyRowItem
                {
                    SchDate = dayDate.ToString("yyyy-MM-dd"),
                    DayNo = day
                });
            }
            return rows;
        }

        private static List<PerfMonthlyRowItem> BuildEmptyItsPerfMonthlyRows(string periode)
        {
            DateTime periodMonth;
            if (!DateTime.TryParseExact((periode ?? string.Empty).Trim() + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out periodMonth)
                && !DateTime.TryParse((periode ?? string.Empty).Trim() + "-01", out periodMonth))
            {
                periodMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            periodMonth = new DateTime(periodMonth.Year, periodMonth.Month, 1);
            DateTime monthlyStart = periodMonth.AddMonths(-5);

            List<PerfMonthlyRowItem> rows = new List<PerfMonthlyRowItem>();
            for (int offset = 0; offset < 6; offset++)
            {
                DateTime monthCursor = monthlyStart.AddMonths(offset);
                rows.Add(new PerfMonthlyRowItem
                {
                    PeriodeMonth = monthCursor.ToString("yyyy-MM")
                });
            }
            return rows;
        }

        private static DataTable ExecutePerfScopeStoredProcedure(
            string storedProcedureName,
            string periode,
            HttpContext context,
            string connString,
            string technicianId = null)
        {
            string safePeriode = EscapeSqlLiteral(NormalizeClosedJobPeriode(periode));
            string safeSupAreaId;
            string safeAreaGroupId;
            ResolveRegionalFiltersFromSession(context, string.Empty, string.Empty, out safeSupAreaId, out safeAreaGroupId);
            safeSupAreaId = EscapeSqlLiteral(safeSupAreaId);
            safeAreaGroupId = EscapeSqlLiteral(NormalizeAreaGroupId(safeAreaGroupId));

            string callParams = "'" + safePeriode + "','" + safeSupAreaId + "','" + safeAreaGroupId + "'";
            if (technicianId != null)
            {
                callParams += ",'" + EscapeSqlLiteral(TrimToLength((technicianId ?? string.Empty).Trim(), 20)) + "'";
            }

            Recordset rec = new Recordset();
            rec.Open(
                storedProcedureName + " " + callParams,
                connString);

            return rec.DataRecord() ?? new DataTable();
        }

        private static List<PerfDailyRowItem> MapPerfDailyRows(DataTable source, string technicianId, out string technicianName)
        {
            technicianName = string.Empty;
            List<PerfDailyRowItem> rows = new List<PerfDailyRowItem>();
            if (source == null || source.Rows.Count == 0 || string.IsNullOrWhiteSpace(technicianId))
            {
                return rows;
            }

            foreach (DataRow row in source.Rows)
            {
                if (!GetValue(row, "TechnicianID").Equals(technicianId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(technicianName))
                {
                    technicianName = FirstNonEmpty(GetValue(row, "TechnicianName"), GetValue(row, "Name"), technicianId);
                }

                DateTime schDateValue;
                string schDateRaw = GetValue(row, "SchDate");
                string schDateText = schDateRaw;
                if (DateTime.TryParse(schDateRaw, out schDateValue))
                {
                    schDateText = schDateValue.ToString("yyyy-MM-dd");
                }

                rows.Add(new PerfDailyRowItem
                {
                    SchDate = schDateText,
                    DayNo = ParseIntFromColumns(row, "DayNo", "DAYNO"),
                    JoAssignNew = ParseIntFromColumns(row, "JoAssignNew"),
                    UnitCloseNew = ParseIntFromColumns(row, "UnitCloseNew"),
                    JoCloseNew = ParseIntFromColumns(row, "JoCloseNew"),
                    JoAssignMaint = ParseIntFromColumns(row, "JoAssignMaint"),
                    UnitCloseMaint = ParseIntFromColumns(row, "UnitCloseMaint"),
                    JoCloseMaint = ParseIntFromColumns(row, "JoCloseMaint"),
                    JoAssignTotal = ParseIntFromColumns(row, "JoAssignTotal"),
                    UnitCloseTotal = ParseIntFromColumns(row, "UnitCloseTotal"),
                    JoCloseTotal = ParseIntFromColumns(row, "JoCloseTotal")
                });
            }

            return rows;
        }

        private static List<PerfMonthlyRowItem> MapPerfMonthlyRows(DataTable source, string technicianId, ref string technicianName)
        {
            List<PerfMonthlyRowItem> rows = new List<PerfMonthlyRowItem>();
            if (source == null || source.Rows.Count == 0 || string.IsNullOrWhiteSpace(technicianId))
            {
                return rows;
            }

            foreach (DataRow row in source.Rows)
            {
                if (!GetValue(row, "TechnicianID").Equals(technicianId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(technicianName))
                {
                    technicianName = FirstNonEmpty(GetValue(row, "TechnicianName"), GetValue(row, "Name"), technicianId);
                }

                rows.Add(new PerfMonthlyRowItem
                {
                    PeriodeMonth = GetValue(row, "PeriodeMonth"),
                    JoAssignNew = ParseIntFromColumns(row, "JoAssignNew"),
                    UnitCloseNew = ParseIntFromColumns(row, "UnitCloseNew"),
                    JoCloseNew = ParseIntFromColumns(row, "JoCloseNew"),
                    JoAssignMaint = ParseIntFromColumns(row, "JoAssignMaint"),
                    UnitCloseMaint = ParseIntFromColumns(row, "UnitCloseMaint"),
                    JoCloseMaint = ParseIntFromColumns(row, "JoCloseMaint"),
                    JoAssignTotal = ParseIntFromColumns(row, "JoAssignTotal"),
                    UnitCloseTotal = ParseIntFromColumns(row, "UnitCloseTotal"),
                    JoCloseTotal = ParseIntFromColumns(row, "JoCloseTotal")
                });
            }

            return rows;
        }

        protected static TrainingLookupResponse BuildTrainingLookupsResponse()
        {
            TrainingLookupResponse response = new TrainingLookupResponse
            {
                Result = "ERROR",
                Message = string.Empty,
                Categories = new List<TrainingLookupItem>(),
                Billables = new List<TrainingLookupItem>()
            };

            try
            {
                response.Categories = LoadLookupItemsFromSp("sp_list_training_category ''", true);
                response.Billables = LoadLookupItemsFromSp("sp_list_billable_type ''", false);
                response.Result = "SUCCESS";
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Result = "ERROR";
                response.Message = "Gagal memuat lookup Training/Visit: " + (ex.Message ?? string.Empty);
            }

            return response;
        }

        private static List<TrainingLookupItem> LoadLookupItemsFromSp(string storedProcedureName, bool trainingOrVisitOnly)
        {
            List<TrainingLookupItem> items = new List<TrainingLookupItem>();
            DataTable source = ExecuteJobTrainingQuery(storedProcedureName);
            if (source == null || source.Rows.Count == 0)
            {
                return items;
            }

            string valueCol = ResolveFirstAvailableLookupColumn(source, "Value", "ID", "Code", "TrainCategoryID", "BillAbleID", "BillableID");
            string textCol = ResolveFirstAvailableLookupColumn(source, "Text", "Name", "Desc", "TrainingCategoryName", "TrainCategoryName", "BillAbleDesc", "BillableDesc");
            if (string.IsNullOrWhiteSpace(valueCol))
            {
                // Common combo SP shape: first column = id, second = description.
                if (source.Columns.Count >= 1)
                {
                    valueCol = source.Columns[0].ColumnName;
                }
                if (source.Columns.Count >= 2)
                {
                    textCol = source.Columns[1].ColumnName;
                }
            }

            foreach (DataRow row in source.Rows)
            {
                string value = GetValue(row, valueCol);
                string text = string.IsNullOrWhiteSpace(textCol) ? value : GetValue(row, textCol);
                if (string.IsNullOrWhiteSpace(value) || value.Equals("[Select]", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (trainingOrVisitOnly && !IsTrainingOrVisitCategory(text, value))
                {
                    continue;
                }

                items.Add(new TrainingLookupItem
                {
                    Value = value,
                    Text = string.IsNullOrWhiteSpace(text) ? value : text
                });
            }

            return items;
        }

        private static string ResolveFirstAvailableLookupColumn(DataTable source, params string[] candidates)
        {
            if (source == null || candidates == null)
            {
                return string.Empty;
            }

            foreach (string candidate in candidates)
            {
                if (source.Columns.Contains(candidate))
                {
                    return candidate;
                }
            }

            return string.Empty;
        }

        protected static SaveAssignResponse ExecuteSaveJobTrainingAssign(
            string custId,
            string reqDate,
            string billableId,
            string schDate,
            string remark,
            string categoryId,
            string itUserId,
            string itUserName)
        {
            SaveAssignResponse response = new SaveAssignResponse
            {
                Result = "ERROR",
                AssignID = string.Empty,
                Message = string.Empty
            };

            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                response.Message = "Session tidak ditemukan.";
                return response;
            }

            string connString = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
            string userId = Convert.ToString(context.Session["ClsTypeUserID"]);
            if (string.IsNullOrWhiteSpace(connString))
            {
                response.Message = "Koneksi database tidak tersedia.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(custId))
            {
                response.Message = "Customer wajib dipilih.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(categoryId) || categoryId.Equals("[Select]", StringComparison.OrdinalIgnoreCase))
            {
                response.Message = "Category Training/Visit wajib dipilih.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(billableId) || billableId.Equals("[Select]", StringComparison.OrdinalIgnoreCase))
            {
                response.Message = "Billable wajib dipilih.";
                return response;
            }

            DateTime scheduleDateValue;
            if (!DateTime.TryParse(schDate, out scheduleDateValue))
            {
                response.Message = "Schedule Date tidak valid.";
                return response;
            }

            if (scheduleDateValue.Date < DateTime.Today)
            {
                response.Message = "Training/Visit hanya bisa ditambahkan untuk tanggal hari ini atau setelahnya.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(itUserId) && string.IsNullOrWhiteSpace(itUserName))
            {
                response.Message = "IT Support wajib dipilih.";
                return response;
            }

            DateTime requestDateValue;
            if (!DateTime.TryParse(reqDate, out requestDateValue))
            {
                requestDateValue = DateTime.Today;
            }

            string safeRemark = (remark ?? string.Empty).Trim();
            string safeItName = (itUserName ?? string.Empty).Trim();
            string safeItId = (itUserId ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(safeItName) || !string.IsNullOrWhiteSpace(safeItId))
            {
                List<string> tags = new List<string>();
                if (!string.IsNullOrWhiteSpace(safeItId))
                {
                    tags.Add("ITID:" + safeItId);
                }

                if (!string.IsNullOrWhiteSpace(safeItName))
                {
                    tags.Add("ITNAME:" + safeItName);
                    tags.Add("IT:" + safeItName);
                }
                else if (!string.IsNullOrWhiteSpace(safeItId))
                {
                    tags.Add("IT:" + safeItId);
                }

                foreach (string tag in tags)
                {
                    if (safeRemark.IndexOf(tag, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        safeRemark = string.IsNullOrWhiteSpace(safeRemark) ? tag : (safeRemark + " | " + tag);
                    }
                }
            }

            try
            {
                string sql = "sp_submit_job_training '"
                    + EscapeSqlLiteral(custId.Trim()) + "','"
                    + EscapeSqlLiteral(requestDateValue.ToString("yyyy-MM-dd")) + "','"
                    + EscapeSqlLiteral(billableId.Trim()) + "','"
                    + EscapeSqlLiteral(scheduleDateValue.ToString("yyyy-MM-dd")) + "','"
                    + EscapeSqlLiteral(safeRemark) + "','"
                    + EscapeSqlLiteral(categoryId.Trim()) + "','"
                    + EscapeSqlLiteral((userId ?? string.Empty).Trim()) + "'";

                ExecCommand ec = new ExecCommand();
                int affected = 0;
                string error = string.Empty;
                if (!ec.Execute(sql, connString.Trim(), ref affected, ref error))
                {
                    response.Message = string.IsNullOrWhiteSpace(error) ? "Submit job training gagal." : error;
                    return response;
                }

                if (affected <= 0)
                {
                    response.Message = string.IsNullOrWhiteSpace(error)
                        ? "Submit job training gagal (no rows affected)."
                        : error;
                    return response;
                }

                response.Result = "SUCCESS";
                response.AssignID = string.Empty;
                response.Message = "Job Training/Visit berhasil dibuat.";
            }
            catch (Exception ex)
            {
                response.Message = "Gagal submit job training: " + (ex.Message ?? string.Empty);
            }

            return response;
        }

        protected static ScheduleReportResponse BuildItsScheduleDayReport(string technicianId, string schDate, string technicianName)
        {
            string displayName = FirstNonEmptyStatic(technicianName, technicianId, "-");
            string resolvedItId;
            string itIdMessage;
            if (!TryResolveItsAssignTechnicianId(technicianId, out resolvedItId, out itIdMessage))
            {
                return new ScheduleReportResponse
                {
                    Result = "ERROR",
                    Message = string.IsNullOrWhiteSpace(itIdMessage) ? "ITID not exist" : itIdMessage,
                    TechnicianID = TrimToLength((technicianId ?? string.Empty).Trim(), 20),
                    TechnicianName = displayName,
                    SchDate = (schDate ?? string.Empty).Trim(),
                    TotalUnitSelesai = 0,
                    TotalUnitBelumSelesai = 0,
                    Rows = new List<ScheduleReportRowItem>()
                };
            }

            // Numbered col-day detail = assignments saved for this IT Support user (trx_job_assign_detail).
            ScheduleReportResponse response = LoadScheduleReport(resolvedItId, schDate, displayName);
            if (response != null)
            {
                response.TechnicianID = TrimToLength((technicianId ?? string.Empty).Trim(), 20);
                if (string.IsNullOrWhiteSpace(response.TechnicianName))
                {
                    response.TechnicianName = displayName;
                }
            }

            return response;
        }
    }
}
