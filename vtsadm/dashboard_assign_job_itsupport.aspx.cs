using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace vtsadm
{
  public partial class dashboard_assign_job_itsupport : dashboard_assign_job
  {
    private const string SessionPeriodeKey = "SessionAssignJobPeriode";

    private sealed class JobTrxAssignStats
    {
      public int AssignGps { get; set; }
      public int AssignAcs { get; set; }
      public string TechnicianId { get; set; }
      public string TechnicianName { get; set; }
    }

    protected override string FixedActiveTab
    {
      get { return "itsupport"; }
    }

    protected override bool UseJobTrainingDataSource
    {
      get { return true; }
    }

    protected override JobOrderInformationResponse ResolveJobOrderInformationForRequest(
        string activeTab,
        string searchKeyword,
        int pageIndex,
        int pageSize,
        string branchFilter)
    {
      return BuildJobTrainingOrderInformationResponse(
          activeTab,
          searchKeyword,
          pageIndex,
          pageSize,
          branchFilter);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static JobOrderInformationResponse LoadJobOrderInformation(
        string activeTab,
        string searchKeyword,
        int pageIndex,
        int pageSize,
        string branchFilter = "")
    {
      return BuildJobTrainingOrderInformationResponse(
          activeTab,
          searchKeyword,
          pageIndex,
          pageSize,
          branchFilter);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static AreaOptionResponse LoadAreaOptions(string supAreaId)
    {
      return dashboard_assign_job.LoadAreaOptions(supAreaId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static SaveAssignResponse SaveAssignJob(
        string assignId,
        string jobId,
        string custId,
        string technicianId,
        string schDate,
        int qtyAssign,
        string deviceGroupId,
        string areaId,
        string targetStatus,
        string insDeviceTypeId,
        string assignRemark = "")
    {
      return dashboard_assign_job.SaveAssignJob(
          assignId,
          jobId,
          custId,
          technicianId,
          schDate,
          qtyAssign,
          deviceGroupId,
          areaId,
          targetStatus,
          insDeviceTypeId,
          assignRemark);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static SaveAssignResponse UpdateTechnicianStatus(string technicianId, string schDate, string targetStatus)
    {
      return dashboard_assign_job.UpdateTechnicianStatus(technicianId, schDate, targetStatus);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static TechnicianStockResponse LoadTechnicianStock(string technicianId)
    {
      return dashboard_assign_job.LoadTechnicianStock(technicianId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static TechnicianGpsDetailResponse LoadTechnicianGpsDetail(
        string technicianId,
        string deviceTypeId,
        string technicianName,
        string deviceTypeDesc)
    {
      return dashboard_assign_job.LoadTechnicianGpsDetail(technicianId, deviceTypeId, technicianName, deviceTypeDesc);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static ScheduleReportResponse LoadScheduleReport(string technicianId, string schDate, string technicianName)
    {
      return BuildItsScheduleDayReport(technicianId, schDate, technicianName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static TrainingLookupResponse LoadTrainingLookups()
    {
      return BuildTrainingLookupsResponse();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static SaveAssignResponse SaveJobTrainingAssign(
        string custId,
        string reqDate,
        string billableId,
        string schDate,
        string remark,
        string categoryId,
        string itUserId,
        string itUserName)
    {
      return ExecuteSaveJobTrainingAssign(custId, reqDate, billableId, schDate, remark, categoryId, itUserId, itUserName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static DeleteScheduleAssignResponse DeleteScheduleAssign(string assignId, int seq, string actionRemark = "")
    {
      return dashboard_assign_job.DeleteScheduleAssign(assignId, seq, actionRemark);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static RefreshAvailabilityResponse RefreshAvailability(string technicianId, string schDate)
    {
      return dashboard_assign_job.RefreshAvailability(technicianId, schDate);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static RefreshScheduleDayStateResponse RefreshScheduleDayState(string schDate, string[] technicianIds)
    {
      return dashboard_assign_job.RefreshScheduleDayState(schDate, technicianIds);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static DayTotalJoModalResponse GetDayTotalJoList(string scheduleDate)
    {
      return dashboard_assign_job.GetDayTotalJoList(scheduleDate);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static ClosedJobModalResponse GetClosedJobList(
        string technicianId,
        string technicianName,
        string closeType,
        string periode)
    {
      return dashboard_assign_job.GetClosedJobList(technicianId, technicianName, closeType, periode);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static PerfTechnicianListResponse LoadPerfTechnicianList(string periode)
    {
      return dashboard_assign_job.LoadPerfTechnicianList(periode);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public new static PerfChartDataResponse LoadPerfChartData(string periode, string technicianId)
    {
      return dashboard_assign_job.LoadPerfChartData(periode, technicianId);
    }

    private static bool IsInstallationTab(string activeTab)
    {
      string tab = (activeTab ?? string.Empty).Trim().ToLowerInvariant();
      return tab == "installation" || tab == "new_install" || tab == "new installation";
    }

    private static DateTime ResolveJobOrderPeriodeMonthStart()
    {
      HttpContext context = HttpContext.Current;
      string periode = string.Empty;
      if (context != null && context.Session != null && context.Session[SessionPeriodeKey] != null)
      {
        periode = Convert.ToString(context.Session[SessionPeriodeKey]);
      }

      if (string.IsNullOrWhiteSpace(periode))
      {
        periode = DateTime.Today.ToString("yyyy-MM");
      }

      string normalized = periode.Trim();
      DateTime parsed;
      if (DateTime.TryParseExact(normalized, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
      {
        return new DateTime(parsed.Year, parsed.Month, 1);
      }

      if (DateTime.TryParse(normalized, out parsed))
      {
        return new DateTime(parsed.Year, parsed.Month, 1);
      }

      DateTime today = DateTime.Today;
      return new DateTime(today.Year, today.Month, 1);
    }

    private static bool IsActiveAssignDetailRow(DataRow row)
    {
      if (row == null)
      {
        return false;
      }

      string status = FirstNonEmptyStatic(
          GetValue(row, "Status"),
          GetValue(row, "StatusCode")).Trim().ToUpperInvariant();
      return status != "DE";
    }

    private static Dictionary<string, string> BuildCustomerLastClosedAssignDateMap()
    {
      Dictionary<string, string> map =
          new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      DataTable table = ExecuteJobTrainingQuery(
          "SELECT LTRIM(RTRIM(ISNULL(h.CustID, ''))) AS CustID, "
          + "MAX(CONVERT(varchar(10), d.SchDate, 120)) AS LastClosedAssignDate "
          + "FROM trx_job_assign_header h WITH (NOLOCK) "
          + "INNER JOIN trx_job_assign_detail d WITH (NOLOCK) "
          + "ON d.AssignID = h.AssignID AND ISNULL(d.Status, '') NOT IN ('DE') "
          + "INNER JOIN trx_training_order t WITH (NOLOCK) ON t.TrainingID = d.JobID "
          + "WHERE ISNULL(h.Status, '') NOT IN ('DE') "
          + "AND LTRIM(RTRIM(ISNULL(h.CustID, ''))) <> '' "
          + "AND UPPER(LTRIM(RTRIM(ISNULL(t.Status, '')))) IN ('CL', 'CLOSE', 'CLOSED') "
          + "GROUP BY h.CustID");
      if (table == null || table.Rows.Count == 0)
      {
        return map;
      }

      foreach (DataRow row in table.Rows)
      {
        string custId = GetValue(row, "CustID").Trim();
        string closedDate = GetValue(row, "LastClosedAssignDate").Trim();
        if (string.IsNullOrWhiteSpace(custId) || string.IsNullOrWhiteSpace(closedDate))
        {
          continue;
        }

        map[custId] = closedDate;
      }

      return map;
    }

    private static int CalculateJobOrderSlaDays(string createDateRaw)
    {
      DateTime createDate;
      if (!DateTime.TryParseExact(
              (createDateRaw ?? string.Empty).Trim(),
              "yyyy-MM-dd",
              CultureInfo.InvariantCulture,
              DateTimeStyles.None,
              out createDate)
          && !DateTime.TryParse((createDateRaw ?? string.Empty).Trim(), out createDate))
      {
        return 0;
      }

      return Math.Max(0, (DateTime.Today - createDate.Date).Days);
    }

    private static Dictionary<string, JobTrxAssignStats> BuildJobTrainingGlobalAssignStatsMap()
    {
      Dictionary<string, JobTrxAssignStats> map =
          new Dictionary<string, JobTrxAssignStats>(StringComparer.OrdinalIgnoreCase);
      Dictionary<string, string> itSupportNames = ItsSupportAssignData.LoadItSupportNameMap();
      DataTable details = ExecuteJobTrainingQuery(
          "SELECT TechnicianID, SchDate, JobID, AssignID, Seq, Status, DeviceGroupID, QtyGPS, QtyACS "
          + "FROM trx_job_assign_detail WITH (NOLOCK) "
          + "WHERE ISNULL(Status, '') NOT IN ('DE') "
          + "ORDER BY SchDate DESC");
      if (details == null || details.Rows.Count == 0)
      {
        return map;
      }

      foreach (DataRow row in details.Rows)
      {
        if (!IsActiveAssignDetailRow(row))
        {
          continue;
        }

        string jobId = FirstNonEmptyStatic(GetValue(row, "JobID"), GetValue(row, "TrainingID")).Trim();
        if (string.IsNullOrWhiteSpace(jobId))
        {
          continue;
        }

        JobTrxAssignStats stats = map.ContainsKey(jobId)
            ? map[jobId]
            : new JobTrxAssignStats
            {
              TechnicianId = string.Empty,
              TechnicianName = string.Empty
            };

        if (string.IsNullOrWhiteSpace(stats.TechnicianId))
        {
          string technicianId = FirstNonEmptyStatic(GetValue(row, "TechnicianID")).Trim();
          stats.TechnicianId = technicianId;
          stats.TechnicianName = ItsSupportAssignData.ResolveItSupportName(technicianId, itSupportNames);
        }

        string deviceGroup = NormalizeDeviceGroupId(
            FirstNonEmptyStatic(GetValue(row, "DeviceGroupID"), GetValue(row, "DeviceGroup")));
        int qtyGps = ParseIntFromColumns(row, "QtyGPS", "QtyGps");
        int qtyAcs = ParseIntFromColumns(row, "QtyACS", "QtyAcs");
        if (deviceGroup == "ACS")
        {
          stats.AssignAcs += qtyAcs > 0 ? qtyAcs : 1;
        }
        else
        {
          stats.AssignGps += qtyGps > 0 ? qtyGps : 1;
        }

        map[jobId] = stats;
      }

      return map;
    }

    private static DataTable FilterJobOrderInformationByBranch(DataTable source, string branchFilter)
    {
      if (source == null || source.Rows.Count == 0)
      {
        return new DataTable();
      }

      string branch = (branchFilter ?? string.Empty).Trim();
      if (string.IsNullOrWhiteSpace(branch)
          || branch.Equals("SEMUA", StringComparison.OrdinalIgnoreCase)
          || branch.Equals("ALL", StringComparison.OrdinalIgnoreCase))
      {
        return source.Copy();
      }

      DataTable filtered = source.Clone();
      string branchLower = branch.ToLowerInvariant();
      foreach (DataRow row in source.Rows)
      {
        string branchName = GetValue(row, "BranchName");
        if (branchName.Equals(branch, StringComparison.OrdinalIgnoreCase)
            || branchName.ToLowerInvariant().Contains(branchLower))
        {
          filtered.ImportRow(row);
        }
      }

      return filtered;
    }

    private static List<string> BuildJobOrderBranchOptions(DataTable source)
    {
      List<string> branches = new List<string>();
      HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      if (source == null)
      {
        return branches;
      }

      foreach (DataRow row in source.Rows)
      {
        string branchName = GetValue(row, "BranchName").Trim();
        if (string.IsNullOrWhiteSpace(branchName) || seen.Contains(branchName))
        {
          continue;
        }

        seen.Add(branchName);
        branches.Add(branchName);
      }

      branches.Sort(StringComparer.OrdinalIgnoreCase);
      return branches;
    }

    private static DataTable FilterJobOrderInformationForIts(DataTable source, string keyword)
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
            + GetValue(row, "Address") + " "
            + GetValue(row, "PicName") + " "
            + GetValue(row, "CustomerNumber") + " "
            + GetValue(row, "Remark") + " "
            + GetValue(row, "MarketingName") + " "
            + GetValue(row, "DeviceTypeDesc")).ToLowerInvariant();
        if (merged.Contains(search.ToLowerInvariant()))
        {
          filtered.ImportRow(row);
        }
      }

      return filtered;
    }

    private static bool IsExactJobIdSearch(DataTable source, string keyword)
    {
      string search = (keyword ?? string.Empty).Trim();
      if (source == null || source.Rows.Count == 0 || string.IsNullOrWhiteSpace(search))
      {
        return false;
      }

      foreach (DataRow row in source.Rows)
      {
        if (GetValue(row, "JobID").Equals(search, StringComparison.OrdinalIgnoreCase))
        {
          return true;
        }
      }

      return false;
    }

    private static DataTable FilterJobOrderInformationHideAssignedForIts(DataTable source, string keyword)
    {
      if (source == null || source.Rows.Count == 0)
      {
        return new DataTable();
      }

      string search = (keyword ?? string.Empty).Trim();
      bool allowAssignedTransfer = IsExactJobIdSearch(source, search);
      DataTable filtered = source.Clone();
      foreach (DataRow row in source.Rows)
      {
        int remaining = ParseIntFromColumns(row, "RemainingUnit");
        int assignedTotal = ParseIntFromColumns(row, "TotalAssign");
        string jobId = GetValue(row, "JobID");
        bool isExactMatch = !string.IsNullOrWhiteSpace(search)
            && jobId.Equals(search, StringComparison.OrdinalIgnoreCase);

        if (remaining > 0 || (allowAssignedTransfer && isExactMatch && assignedTotal > 0))
        {
          filtered.ImportRow(row);
        }
      }

      return filtered;
    }

    private static bool ParseBoolFromDataRow(DataRow row, string columnName)
    {
      if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
      {
        return false;
      }

      object value = row[columnName];
      if (value == null || value == DBNull.Value)
      {
        return false;
      }

      if (value is bool)
      {
        return (bool)value;
      }

      string text = Convert.ToString(value).Trim();
      return text.Equals("1", StringComparison.OrdinalIgnoreCase)
          || text.Equals("true", StringComparison.OrdinalIgnoreCase)
          || text.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    protected static JobOrderInformationResponse BuildJobTrainingOrderInformationResponse(
        string activeTab,
        string searchKeyword,
        int pageIndex,
        int pageSize,
        string branchFilter)
    {
      JobOrderInformationResponse response = new JobOrderInformationResponse
      {
        Rows = new List<JobOrderInformationItem>(),
        BranchOptions = new List<string>(),
        TotalRecords = 0,
        PageIndex = pageIndex < 1 ? 1 : pageIndex,
        PageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100),
        TotalPages = 1,
        ErrorMessage = string.Empty
      };

      try
      {
        bool showAllJoTypes = string.Equals((activeTab ?? string.Empty).Trim(), "all", StringComparison.OrdinalIgnoreCase);
        bool isVisitTab = !showAllJoTypes && !IsInstallationTab(activeTab);
        DataTable raw = LoadJobTrainingHeaderTable(string.Empty);
        DateTime monthStart = ResolveJobOrderPeriodeMonthStart();
        string viewDateFrom = monthStart.AddYears(-1).ToString("yyyy-MM-dd");
        string viewDateTo = monthStart.AddYears(1).ToString("yyyy-MM-dd");
        DataTable viewRows = ExecuteJobTrainingQuery(
            "sp_view_job_training '','','"
            + viewDateFrom.Replace("'", "''") + "','"
            + viewDateTo.Replace("'", "''") + "'");
        if (raw != null && raw.Rows.Count > 0 && viewRows != null && viewRows.Rows.Count > 0)
        {
          MergeJobTrainingTrainerColumns(raw, viewRows);
        }

        EnrichJobTrainingWithCustomerContext(raw);
        Dictionary<string, CustomerScheduleContext> customerMap = LoadCustomerScheduleContextMap();
        Dictionary<string, string> marketingByCustId = ItsSupportAssignData.LoadCustomerMarketingMap();
        Dictionary<string, string> marketingByTrainingId = ItsSupportAssignData.LoadTrainingMarketingMap();
        Dictionary<string, JobTrxAssignStats> trxStats = BuildJobTrainingGlobalAssignStatsMap();
        Dictionary<string, string> customerLastClosedAssignMap = BuildCustomerLastClosedAssignDateMap();
        Dictionary<string, ItsSupportAssignData.CustomerDeviceCounts> customerDeviceCounts =
            ItsSupportAssignData.LoadCustomerGpsCountMap();

        DataTable mapped = new DataTable();
        mapped.Columns.Add("JobID");
        mapped.Columns.Add("CustID");
        mapped.Columns.Add("CustomerName");
        mapped.Columns.Add("BranchName");
        mapped.Columns.Add("Address");
        mapped.Columns.Add("MarketingName");
        mapped.Columns.Add("DefaultAreaId");
        mapped.Columns.Add("DeviceTypeID");
        mapped.Columns.Add("DeviceTypeDesc");
        mapped.Columns.Add("TotalAssign", typeof(int));
        mapped.Columns.Add("TotalAssignGps", typeof(int));
        mapped.Columns.Add("TotalAssignAcs", typeof(int));
        mapped.Columns.Add("LastAssignDate");
        mapped.Columns.Add("AssignDate");
        mapped.Columns.Add("SlaDays", typeof(int));
        mapped.Columns.Add("Remark");
        mapped.Columns.Add("PicName");
        mapped.Columns.Add("CustomerNumber");
        mapped.Columns.Add("RemainingUnit", typeof(int));
        mapped.Columns.Add("TotalUnit", typeof(int));
        mapped.Columns.Add("RemainingUnitGps", typeof(int));
        mapped.Columns.Add("RemainingUnitAcs", typeof(int));
        mapped.Columns.Add("TotalUnitGps", typeof(int));
        mapped.Columns.Add("TotalUnitAcs", typeof(int));
        mapped.Columns.Add("TotalUnitGpsDone", typeof(int));
        mapped.Columns.Add("TotalUnitAcsDone", typeof(int));
        mapped.Columns.Add("CustomerGpsCount", typeof(int));
        mapped.Columns.Add("CustomerAcsCount", typeof(int));
        mapped.Columns.Add("IsTransfer", typeof(bool));
        mapped.Columns.Add("AssignedTechnicianId");
        mapped.Columns.Add("AssignedTechnicianName");

        if (raw != null)
        {
          foreach (DataRow row in raw.Rows)
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

            if (!IsTrainingOrVisitCategory(categoryName, categoryId))
            {
              continue;
            }

            bool isVisit = IsVisitCategory(categoryName, categoryId);
            if (!showAllJoTypes && isVisitTab != isVisit)
            {
              continue;
            }

            string rawStatus = FirstNonEmptyStatic(
                GetValue(row, "Status"),
                GetValue(row, "ValueStatus"),
                GetValue(row, "StatusCode"));
            if (NormalizeJobTrainingStatusCode(rawStatus) == "close")
            {
              continue;
            }

            string jobId = FirstNonEmptyStatic(GetValue(row, "TrainingID"), GetValue(row, "JobID"));
            string custId = GetValue(row, "CustID");
            CustomerScheduleContext customerContext = null;
            if (!string.IsNullOrWhiteSpace(custId) && customerMap.ContainsKey(custId))
            {
              customerContext = customerMap[custId];
            }

            JobTrxAssignStats trxStat = trxStats.ContainsKey(jobId) ? trxStats[jobId] : null;
            int assignGps = trxStat != null ? trxStat.AssignGps : 0;
            int assignAcs = trxStat != null ? trxStat.AssignAcs : 0;
            int assignedTotal = assignGps + assignAcs;
            string lastAssignDate = string.Empty;
            if (!string.IsNullOrWhiteSpace(custId) && customerLastClosedAssignMap.ContainsKey(custId))
            {
              lastAssignDate = customerLastClosedAssignMap[custId];
            }
            string assignDate = FirstNonEmptyStatic(
                GetValue(row, "sReqDate"),
                GetValue(row, "ReqDate"));
            int slaDays = CalculateJobOrderSlaDays(assignDate);
            // JO header remark from trx_training_order (same as sp_list_header_job_training).
            string remark = FirstNonEmptyStatic(
                GetValue(row, "Remark"),
                GetValue(row, "Remarks"));

            // IT Support Training/Visit: assign slot is per trx_job_assign_detail, not installation GPS units.
            // Training JO close status (CL) must not block IT Support scheduling.
            int totalUnit = 1;
            int remaining = Math.Max(0, totalUnit - Math.Max(assignedTotal, 0));

            string address = FirstNonEmptyStatic(
                GetValue(row, "Address"),
                customerContext != null ? customerContext.Address : string.Empty,
                GetValue(row, "CustAddress"));
            string picName = FirstNonEmptyStatic(
                GetValue(row, "PICName1"),
                GetValue(row, "PicName"),
                customerContext != null ? customerContext.PicName : string.Empty);
            string customerNumber = FirstNonEmptyStatic(
                GetValue(row, "OfficePhone1"),
                GetValue(row, "MobilePhone1"),
                GetValue(row, "CustomerNumber"),
                customerContext != null ? customerContext.CustomerNumber : string.Empty);
            string marketingName = FirstNonEmptyStatic(
                GetValue(row, "MarketingName"),
                GetValue(row, "Marketing"),
                GetValue(row, "MarketingUserName"),
                customerContext != null ? customerContext.MarketingName : string.Empty,
                ItsSupportAssignData.ResolveMarketingName(jobId, marketingByTrainingId),
                ItsSupportAssignData.ResolveMarketingName(custId, marketingByCustId));
            string defaultAreaId = FirstNonEmptyStatic(
                GetValue(row, "SupAreaID"),
                customerContext != null ? customerContext.SupAreaID : string.Empty);
            int customerGpsCount = 0;
            if (!string.IsNullOrWhiteSpace(custId) && customerDeviceCounts.ContainsKey(custId))
            {
              customerGpsCount = customerDeviceCounts[custId].TotalGps;
            }
            string assignedTechnicianId = trxStat != null
                ? FirstNonEmptyStatic(trxStat.TechnicianId)
                : string.Empty;
            string assignedTechnicianName = trxStat != null
                ? FirstNonEmptyStatic(trxStat.TechnicianName, trxStat.TechnicianId)
                : string.Empty;
            bool isTransfer = assignedTotal > 0;

            DataRow target = mapped.NewRow();
            target["JobID"] = jobId;
            target["CustID"] = custId;
            target["CustomerName"] = FirstNonEmptyStatic(
                GetValue(row, "CustomerName"),
                GetValue(row, "CustName"),
                GetValue(row, "FullName"));
            target["BranchName"] = GetValue(row, "BranchName");
            target["Address"] = address;
            target["MarketingName"] = marketingName;
            target["DefaultAreaId"] = defaultAreaId;
            target["DeviceTypeID"] = categoryId;
            target["DeviceTypeDesc"] = FirstNonEmptyStatic(
                categoryName,
                GetValue(row, "TrainCategoryDesc"),
                isVisit ? "Visit" : "Training");
            target["TotalAssign"] = assignedTotal;
            target["TotalAssignGps"] = assignGps;
            target["TotalAssignAcs"] = assignAcs;
            target["LastAssignDate"] = lastAssignDate;
            target["AssignDate"] = assignDate;
            target["SlaDays"] = slaDays;
            target["Remark"] = remark;
            target["PicName"] = picName;
            target["CustomerNumber"] = customerNumber;
            target["RemainingUnit"] = remaining;
            target["TotalUnit"] = totalUnit;
            target["RemainingUnitGps"] = remaining;
            target["RemainingUnitAcs"] = 0;
            target["TotalUnitGps"] = totalUnit;
            target["TotalUnitAcs"] = 0;
            target["TotalUnitGpsDone"] = assignGps;
            target["TotalUnitAcsDone"] = assignAcs;
            target["CustomerGpsCount"] = customerGpsCount;
            target["CustomerAcsCount"] = 0;
            target["IsTransfer"] = isTransfer;
            target["AssignedTechnicianId"] = assignedTechnicianId;
            target["AssignedTechnicianName"] = assignedTechnicianName;
            mapped.Rows.Add(target);
          }
        }

        response.BranchOptions = BuildJobOrderBranchOptions(mapped);
        DataTable branchFiltered = FilterJobOrderInformationByBranch(mapped, branchFilter);
        DataTable filtered = FilterJobOrderInformationForIts(branchFiltered, searchKeyword);
        filtered = FilterJobOrderInformationHideAssignedForIts(filtered, searchKeyword);
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
          response.Rows.Add(new JobOrderInformationItem
          {
            JobID = GetValue(row, "JobID"),
            Customer = customerId,
            CustomerName = ResolveCustomerName(row, customerId),
            BranchName = GetValue(row, "BranchName"),
            Address = GetValue(row, "Address"),
            MarketingName = GetValue(row, "MarketingName"),
            DefaultAreaId = GetValue(row, "DefaultAreaId"),
            DeviceTypeID = GetValue(row, "DeviceTypeID"),
            DeviceTypeDesc = GetValue(row, "DeviceTypeDesc"),
            TotalAssign = ParseIntFromColumns(row, "TotalAssign"),
            TotalAssignGps = ParseIntFromColumns(row, "TotalAssignGps"),
            TotalAssignAcs = ParseIntFromColumns(row, "TotalAssignAcs"),
            LastAssignDate = FormatDateForDisplay(GetValue(row, "LastAssignDate")),
            AssignDate = FormatDateForDisplay(GetValue(row, "AssignDate")),
            SlaDays = ParseIntFromColumns(row, "SlaDays"),
            Remark = GetValue(row, "Remark"),
            PicName = GetValue(row, "PicName"),
            CustomerNumber = GetValue(row, "CustomerNumber"),
            RemainingUnit = ParseIntFromColumns(row, "RemainingUnit"),
            TotalUnit = ParseIntFromColumns(row, "TotalUnit"),
            RemainingUnitGps = ParseIntFromColumns(row, "RemainingUnitGps"),
            RemainingUnitAcs = ParseIntFromColumns(row, "RemainingUnitAcs"),
            TotalUnitGps = ParseIntFromColumns(row, "TotalUnitGps"),
            TotalUnitAcs = ParseIntFromColumns(row, "TotalUnitAcs"),
            TotalUnitGpsDone = ParseIntFromColumns(row, "TotalUnitGpsDone"),
            TotalUnitAcsDone = ParseIntFromColumns(row, "TotalUnitAcsDone"),
            CustomerGpsCount = ParseIntFromColumns(row, "CustomerGpsCount"),
            CustomerAcsCount = 0,
            IsTransfer = ParseBoolFromDataRow(row, "IsTransfer"),
            AssignedTechnicianId = GetValue(row, "AssignedTechnicianId"),
            AssignedTechnicianName = GetValue(row, "AssignedTechnicianName")
          });
        }
      }
      catch (Exception ex)
      {
        response.ErrorMessage = "Terjadi kesalahan saat memuat Job Order Training/Visit: " + (ex.Message ?? string.Empty);
      }

      return response;
    }
  }
}
