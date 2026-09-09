using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_operational : System.Web.UI.Page
    {
        /// <summary>Builds a connection string compatible with SqlConnection by removing OLE DB keywords (e.g. Provider).</summary>
        private static string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return connectionString;
            var pairs = new List<string>();
            foreach (var part in connectionString.Split(';'))
            {
                var p = part.Trim();
                if (string.IsNullOrEmpty(p)) continue;
                var idx = p.IndexOf('=');
                var key = (idx >= 0 ? p.Substring(0, idx).Trim() : p).ToUpperInvariant();
                if (key == "PROVIDER") continue;
                pairs.Add(p);
            }
            return string.Join(";", pairs);
        }

        private static string SafeReaderString(SqlDataReader r, string columnName)
        {
            try
            {
                object o = r[columnName];
                return o == DBNull.Value ? "" : Convert.ToString(o);
            }
            catch
            {
                return "";
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetManagementList(string search, string areaFilter = "")
        {
            try
            {
                var list = new List<object>();
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"] != null ? ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString : null;
                if (string.IsNullOrEmpty(connSql)) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("sp_list_trx_training_target_setting", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Search", search ?? "");
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    conn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var settingIdVal = r["SettingId"];
                            list.Add(new
                            {
                                UserID = r["UserID"] != DBNull.Value ? r["UserID"].ToString() : "",
                                NamaStaff = r["NamaStaff"] != DBNull.Value ? r["NamaStaff"].ToString() : "",
                                Role = r["Role"] != DBNull.Value ? r["Role"].ToString() : "",
                                TotalCust = r["TotalCust"] != DBNull.Value ? Convert.ToInt32(r["TotalCust"]) : 0,
                                DailyTarget = r["DailyTarget"] != DBNull.Value ? Convert.ToInt32(r["DailyTarget"]) : 0,
                                Cycle = r["Cycle"] != DBNull.Value ? Convert.ToInt32(r["Cycle"]) : 0,
                                SettingId = settingIdVal != null && settingIdVal != DBNull.Value && !string.IsNullOrEmpty(settingIdVal.ToString()) ? settingIdVal.ToString() : ""
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object ManagementInsert(string userId, int totalCust, int dailyTarget)
        {
            try
            {
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"] != null ? ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString : null;
                if (string.IsNullOrEmpty(connSql)) return new { success = false, message = "Connection not found" };
                if (string.IsNullOrWhiteSpace(userId)) return new { success = false, message = "UserID tidak valid" };
                var usrUpd = (HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("sp_insert_trx_training_target_setting", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@TotalCust", totalCust);
                    cmd.Parameters.AddWithValue("@DailyTarget", dailyTarget);
                    cmd.Parameters.AddWithValue("@UsrUpd", usrUpd);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return new { success = true, message = "Data berhasil disimpan" };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object ManagementUpdate(int settingId, int totalCust, int dailyTarget)
        {
            try
            {
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"] != null ? ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString : null;
                if (string.IsNullOrEmpty(connSql)) return new { success = false, message = "Connection not found" };
                var usrUpd = (HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("sp_update_trx_training_target_setting", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", settingId);
                    cmd.Parameters.AddWithValue("@TotalCust", totalCust);
                    cmd.Parameters.AddWithValue("@DailyTarget", dailyTarget);
                    cmd.Parameters.AddWithValue("@UsrUpd", usrUpd);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0) return new { success = false, message = "Tidak ada baris yang diupdate" };
                }
                return new { success = true, message = "Data berhasil diupdate" };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        /// <summary>Parse dateFrom/dateTo strings. Default to yesterday if invalid.</summary>
        private static void ParseDateRange(string dateFromStr, string dateToStr, out DateTime dateFrom, out DateTime dateTo)
        {
            var today = DateTime.Now.Date;
            var yesterday = today.AddDays(-1);
            DateTime df, dt;
            dateFrom = DateTime.TryParse(dateFromStr, out df) ? df.Date : yesterday;
            dateTo = DateTime.TryParse(dateToStr, out dt) ? dt.Date : yesterday;
            if (dateFrom > dateTo) { var t = dateFrom; dateFrom = dateTo; dateTo = t; }
        }

        private static HashSet<string> _holidayCache;
        private static DateTime _holidayCacheTime = DateTime.MinValue;

        private static HashSet<string> GetIndonesiaHolidays()
        {
            if (_holidayCache != null && (DateTime.Now - _holidayCacheTime).TotalHours < 24)
                return _holidayCache;
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers["User-Agent"] = "VTSAdmin/1.0";
                    string json = wc.DownloadString("https://raw.githubusercontent.com/guangrei/APIHariLibur_V2/main/holidays.json");
                    var obj = JObject.Parse(json);
                    var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var p in obj.Properties())
                        if (p.Name != "info" && p.Name.Length == 10 && p.Name[4] == '-' && p.Name[7] == '-')
                            set.Add(p.Name);
                    _holidayCache = set;
                    _holidayCacheTime = DateTime.Now;
                    return set;
                }
            }
            catch { _holidayCache = new HashSet<string>(); _holidayCacheTime = DateTime.Now; return _holidayCache; }
        }

        private static int CountWorkdays(DateTime from, DateTime to, bool excludeWeekend, bool excludeHoliday)
        {
            var holidays = excludeHoliday ? GetIndonesiaHolidays() : null;
            int count = 0;
            for (var d = from.Date; d <= to.Date; d = d.AddDays(1))
            {
                if (excludeWeekend && (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)) continue;
                if (holidays != null && holidays.Contains(d.ToString("yyyy-MM-dd"))) continue;
                count++;
            }
            return count;
        }

        /// <summary>Number of days in the selected date range. Target = daily * periodDays.</summary>
        private static int GetPeriodDays(DateTime dateFrom, DateTime dateTo, bool excludeWeekend = false, bool excludeHoliday = false)
        {
            int days = (excludeWeekend || excludeHoliday) ? CountWorkdays(dateFrom, dateTo, excludeWeekend, excludeHoliday) : (dateTo - dateFrom).Days + 1;
            return days < 1 ? 1 : days;
        }

        [WebMethod(EnableSession = true)]
        public static object GetVisitDetail(string dateFrom, string dateTo)
        {
            try
            {
                var list = new List<object>();
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (connSql == null) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                string conn = connSql.ConnectionString;
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection string kosong", Data = list };

                DateTime df, dt;
                ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);

                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_visit_detail", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new
                            {
                                Tanggal = r["Tanggal"] != DBNull.Value ? r["Tanggal"].ToString() : "",
                                Waktu = r["Waktu"] != DBNull.Value ? r["Waktu"].ToString() : "",
                                ITStaff = r["ITStaff"] != DBNull.Value ? r["ITStaff"].ToString() : "",
                                Customer = r["Customer"] != DBNull.Value ? r["Customer"].ToString() : "",
                                StatusCustomer = r["StatusCustomer"] != DBNull.Value ? r["StatusCustomer"].ToString() : "",
                                StatusVisit = r["StatusVisit"] != DBNull.Value ? r["StatusVisit"].ToString() : "",
                                Hasil = r["Hasil"] != DBNull.Value ? r["Hasil"].ToString() : ""
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetKunjunganDetail(string dateFrom, string dateTo, string staffName)
        {
            try
            {
                var list = new List<object>();
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (connSql == null) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                string conn = connSql.ConnectionString;
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection string kosong", Data = list };

                DateTime df, dt;
                ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);
                string staffFilter = staffName ?? "";

                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_kunjungan_detail", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                    cmd.Parameters.AddWithValue("@StaffName", staffFilter);
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        int idxMarketing = -1;
                        try { idxMarketing = r.GetOrdinal("MarketingName"); } catch { }
                        while (r.Read())
                        {
                            string marketingName = "";
                            if (idxMarketing >= 0 && r["MarketingName"] != DBNull.Value) marketingName = r["MarketingName"].ToString();
                            list.Add(new
                            {
                                Tanggal = r["Tanggal"] != DBNull.Value ? r["Tanggal"].ToString() : "",
                                Waktu = r["Waktu"] != DBNull.Value ? r["Waktu"].ToString() : "",
                                ITStaff = r["ITStaff"] != DBNull.Value ? r["ITStaff"].ToString() : "",
                                Customer = r["Customer"] != DBNull.Value ? r["Customer"].ToString() : "",
                                MarketingName = marketingName,
                                StatusCustomer = r["StatusCustomer"] != DBNull.Value ? r["StatusCustomer"].ToString() : "",
                                StatusVisit = r["StatusVisit"] != DBNull.Value ? r["StatusVisit"].ToString() : "",
                                Hasil = r["Hasil"] != DBNull.Value ? r["Hasil"].ToString() : "",
                                Attachment = r["Attachment"] != DBNull.Value ? r["Attachment"].ToString() : ""
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetGapDetail(string dateFrom, string dateTo, bool excludeWeekend = false, bool excludeHoliday = false, string areaFilter = "")
        {
            try
            {
                var list = new List<object>();
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (connSql == null) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                string conn = connSql.ConnectionString;
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection string kosong", Data = list };

                DateTime df, dt;
                DateTime d1, d2;
                df = DateTime.TryParse(dateFrom, out d1) ? d1.Date : DateTime.Today.AddDays(-1);
                dt = DateTime.TryParse(dateTo, out d2) ? d2.Date : DateTime.Today.AddDays(-1);
                if (df > dt) { var t = df; df = dt; dt = t; }
                int periodDays = (excludeWeekend || excludeHoliday) ? CountWorkdays(df, dt, excludeWeekend, excludeHoliday) : (dt - df).Days + 1;
                if (periodDays < 1) periodDays = 1;

                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_team_performa", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", df);
                    cmd.Parameters.AddWithValue("@DateTo", dt);
                    cmd.Parameters.AddWithValue("@PageIndex", 1);
                    cmd.Parameters.AddWithValue("@PageSize", 9999);
                    cmd.Parameters.AddWithValue("@Search", "");
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    var pTotal = cmd.Parameters.Add("@TotalCount", SqlDbType.Int);
                    pTotal.Direction = ParameterDirection.Output;
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int dailyTarget = r["Target"] != DBNull.Value ? Convert.ToInt32(r["Target"]) : 0;
                            int periodTarget = dailyTarget * periodDays;
                            int realisasi = r["Realisasi"] != DBNull.Value ? Convert.ToInt32(r["Realisasi"]) : 0;
                            int totalCust = r["TotalCust"] != DBNull.Value ? Convert.ToInt32(r["TotalCust"]) : 0;
                            int gap = Math.Max(0, periodTarget - realisasi);
                            if (gap <= 0) continue;

                            int cycle = (periodTarget > 0 && totalCust > 0) ? (int)Math.Ceiling(totalCust / (double)periodTarget) : 0;
                            string cycleEnds = cycle > 0 ? cycle.ToString() : "-";

                            list.Add(new
                            {
                                ITStaff = r["Nama"] != DBNull.Value ? r["Nama"].ToString() : "",
                                SisaTarget = gap + " Visit",
                                CycleEnds = cycleEnds
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetBelumVisitCustomers(string dateFrom = "", string dateTo = "", string areaFilter = "")
        {
            try
            {
                var list = new List<object>();
                string conn = null;
                if (ConfigurationManager.ConnectionStrings["VTSAdminDB"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ConnectionString;
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSADMIN"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                if (HttpContext.Current?.Session?["ClsTypeDBConnStringSQL"] != null)
                {
                    var s = HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString();
                    if (!string.IsNullOrEmpty(s)) conn = conn ?? s;
                }
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                conn = GetSqlClientConnectionString(conn);

                int cnt = 0;
                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_customer_not_visit", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DateTime df, dt;
                    ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);
                    cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read() && r["Cnt"] != DBNull.Value)
                            cnt = Convert.ToInt32(r["Cnt"]);
                        if (r.NextResult())
                        {
                            while (r.Read())
                            {
                                list.Add(new
                                {
                                    TrainingID = r["TrainingID"] != DBNull.Value ? r["TrainingID"].ToString() : "",
                                    NamaCustomer = r["NamaCustomer"] != DBNull.Value ? r["NamaCustomer"].ToString() : "",
                                    Assignee = r["Assignee"] != DBNull.Value ? r["Assignee"].ToString() : "",
                                    BusinessFields = r["BusinessFields"] != DBNull.Value ? r["BusinessFields"].ToString() : "",
                                    PICName = r["PIC Name"] != DBNull.Value ? r["PIC Name"].ToString() : "",
                                    ContactNumber = r["Contact Number"] != DBNull.Value ? r["Contact Number"].ToString() : ""
                                });
                            }
                        }
                    }
                }
                return new { Success = true, Count = cnt, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetCustomerList(string search, string areaFilter = "")
        {
            try
            {
                var list = new List<object>();
                string conn = null;
                if (ConfigurationManager.ConnectionStrings["VTSAdminDB"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ConnectionString;
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSADMIN"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                if (HttpContext.Current?.Session?["ClsTypeDBConnStringSQL"] != null)
                {
                    var s = HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString();
                    if (!string.IsNullOrEmpty(s)) conn = conn ?? s;
                }
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection tidak ditemukan", Data = list };
                conn = GetSqlClientConnectionString(conn);

                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_customer_list", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Search", search ?? "");
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new
                            {
                                NamaCustomer = r["NamaCustomer"] != DBNull.Value ? r["NamaCustomer"].ToString() : "",
                                Tipe = r["Tipe"] != DBNull.Value ? r["Tipe"].ToString() : "",
                                Assignee = r["Assignee"] != DBNull.Value ? r["Assignee"].ToString() : "",
                                Status = r["Status"] != DBNull.Value ? r["Status"].ToString() : "",
                                BusinessFields = r["BusinessFields"] != DBNull.Value ? r["BusinessFields"].ToString() : "",
                                PICName = r["PIC Name"] != DBNull.Value ? r["PIC Name"].ToString() : "",
                                ContactNumber = r["Contact Number"] != DBNull.Value ? r["Contact Number"].ToString() : ""
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetCustomerTrainingChart(string search, string areaFilter, string dateFrom, string dateTo)
        {
            try
            {
                string conn = null;
                if (ConfigurationManager.ConnectionStrings["VTSAdminDB"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ConnectionString;
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSADMIN"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["ClsTypeDBConnStringSQL"] != null)
                {
                    var sessionConn = HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString();
                    if (!string.IsNullOrEmpty(sessionConn)) conn = conn ?? sessionConn;
                }
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection tidak ditemukan", Data = new List<object>() };
                conn = GetSqlClientConnectionString(conn);

                DateTime df, dt;
                ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);

                var list = new List<object>();
                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_customer_training_chart", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Search", search ?? "");
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new
                            {
                                CustomerName = r["CustomerName"] != DBNull.Value ? r["CustomerName"].ToString() : "",
                                MarketingName = r["MarketingName"] != DBNull.Value ? r["MarketingName"].ToString() : "",
                                BusinessFields = r["BusinessFields"] != DBNull.Value ? r["BusinessFields"].ToString() : "",
                                PICName = r["PIC Name"] != DBNull.Value ? r["PIC Name"].ToString() : "",
                                ContactNumber = r["Contact Number"] != DBNull.Value ? r["Contact Number"].ToString() : "",
                                TrainingCount = r["TrainingCount"] != DBNull.Value ? Convert.ToInt32(r["TrainingCount"]) : 0,
                                StatusCustomer = r["StatusCustomer"] != DBNull.Value ? r["StatusCustomer"].ToString() : "New",
                                CustomerDate = r["Customer Date"] != DBNull.Value ? r["Customer Date"].ToString() : "-",
                                CustomerDateDays = r["CustomerDateDays"] != DBNull.Value ? Convert.ToInt32(r["CustomerDateDays"]) : 999999,
                                ITOutboundID = SafeReaderString(r, "ITOutboundID"),
                                JobOrderID = SafeReaderString(r, "JobOrderID")
                            });
                        }
                    }
                }
                return new { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>() };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetTeamPerforma(string dateFrom, string dateTo, int pageIndex, int pageSize, string search, bool excludeWeekend = false, bool excludeHoliday = false, string areaFilter = "")
        {
            try
            {
                var list = new List<object>();
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (connSql == null) return new { Success = false, Error = "Connection tidak ditemukan", Data = list, TotalCount = 0 };
                string conn = connSql.ConnectionString;
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection string kosong", Data = list, TotalCount = 0 };

                DateTime df, dt;
                ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);
                if (pageIndex < 1) pageIndex = 1;
                if (pageSize < 1) pageSize = 10;
                string searchVal = search ?? "";

                int totalCount = 0;
                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_team_performa", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Search", searchVal);
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    connObj.Open();
                    int periodDays = GetPeriodDays(df, dt, excludeWeekend, excludeHoliday);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (r["TotalCount"] != DBNull.Value) totalCount = Convert.ToInt32(r["TotalCount"]);
                            int dailyTarget = r["Target"] != DBNull.Value ? Convert.ToInt32(r["Target"]) : 0;
                            int periodTarget = dailyTarget * periodDays;
                            int realisasi = r["Realisasi"] != DBNull.Value ? Convert.ToInt32(r["Realisasi"]) : 0;
                            int totalCust = r["TotalCust"] != DBNull.Value ? Convert.ToInt32(r["TotalCust"]) : 0;
                            int progress = (periodTarget > 0) ? Math.Min(100, (int)Math.Round(100.0 * realisasi / periodTarget)) : 0;
                            int cycle = (periodTarget > 0 && totalCust > 0) ? (int)Math.Ceiling(totalCust / (double)periodTarget) : 0;
                            list.Add(new
                            {
                                UserID = r["UserID"] != DBNull.Value ? r["UserID"].ToString() : "",
                                Nama = r["Nama"] != DBNull.Value ? r["Nama"].ToString() : "",
                                Target = periodTarget,
                                Realisasi = realisasi,
                                Progress = progress,
                                Cycle = cycle
                            });
                        }
                    }
                }
                return new { Success = true, Data = list, TotalCount = totalCount };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message, Data = new List<object>(), TotalCount = 0 };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object ManagementDelete(int settingId)
        {
            try
            {
                var connSql = ConfigurationManager.ConnectionStrings["VTSADMIN"] != null ? ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString : null;
                if (string.IsNullOrEmpty(connSql)) return new { success = false, message = "Connection not found" };
                var usrUpd = (HttpContext.Current.Session["ClsTypeUserID"] ?? "").ToString();
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("sp_delete_trx_training_target_setting", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", settingId);
                    cmd.Parameters.AddWithValue("@UsrUpd", usrUpd);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0) return new { success = false, message = "Tidak ada baris yang dihapus" };
                }
                return new { success = true, message = "Data berhasil dihapus" };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        /// <summary>Returns operational dashboard stats for AJAX refresh (no postback).</summary>
        [WebMethod(EnableSession = true)]
        public static object GetOperationalDashboardData(string dateFrom, string dateTo, string areaFilter, bool excludeWeekend, bool excludeHoliday)
        {
            try
            {
                string conn = null;
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["ClsTypeDBConnStringSQL"] != null)
                    conn = HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString().Trim();
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSAdminDB"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ConnectionString;
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSADMIN"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                if (string.IsNullOrEmpty(conn)) return new { Success = false, Error = "Connection tidak ditemukan" };
                conn = GetSqlClientConnectionString(conn);

                DateTime df, dt;
                ParseDateRange(dateFrom ?? "", dateTo ?? "", out df, out dt);
                string area = string.IsNullOrEmpty(areaFilter) ? "" : (areaFilter ?? "").Trim();
                int periodDays = GetPeriodDays(df, dt, excludeWeekend, excludeHoliday);

                int total = 0, priority = 0, regular = 0;
                int totalVisit = 0, targetVisit = 0, visitPct = 0;
                int pctExisting = 0, pctNew = 0, pctTrial = 0;
                int pendingVisits = 0;
                string chartJson = "{}";

                using (var cnn = new SqlConnection(conn))
                {
                    cnn.Open();

                    using (var cmd = new SqlCommand("sp_dashboard_operational_total_customer", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AreaFilter", area);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                total = r["total"] != DBNull.Value ? Convert.ToInt32(r["total"]) : 0;
                                priority = r["priority"] != DBNull.Value ? Convert.ToInt32(r["priority"]) : 0;
                                regular = r["regular"] != DBNull.Value ? Convert.ToInt32(r["regular"]) : 0;
                            }
                        }
                    }

                    using (var cmd = new SqlCommand("sp_dashboard_operational_visit_stats", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                        cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                totalVisit = r["TotalVisit"] != DBNull.Value ? Convert.ToInt32(r["TotalVisit"]) : 0;
                                int dailyTarget = r["TargetVisit"] != DBNull.Value ? Convert.ToInt32(r["TargetVisit"]) : 0;
                                targetVisit = dailyTarget * periodDays;
                                visitPct = targetVisit > 0 ? Math.Min(100, (int)Math.Round(100.0 * totalVisit / targetVisit)) : 0;
                            }
                        }
                    }

                    using (var cmd = new SqlCommand("sp_dashboard_operational_customer_status", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AreaFilter", area);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                pctExisting = r["PctExisting"] != DBNull.Value ? Convert.ToInt32(r["PctExisting"]) : 0;
                                pctNew = r["PctNew"] != DBNull.Value ? Convert.ToInt32(r["PctNew"]) : 0;
                                pctTrial = r["PctTrial"] != DBNull.Value ? Convert.ToInt32(r["PctTrial"]) : 0;
                            }
                        }
                    }

                    using (var cmd = new SqlCommand("sp_dashboard_operational_customer_not_visit", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                        cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                        cmd.Parameters.AddWithValue("@AreaFilter", area);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read() && r["Cnt"] != DBNull.Value)
                                pendingVisits = Convert.ToInt32(r["Cnt"]);
                        }
                    }

                    var labels = new List<string>();
                    var dataTarget = new List<int>();
                    var dataRealisasi = new List<int>();
                    using (var cmd = new SqlCommand("sp_dashboard_operational_chart", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DateFrom", df.Date);
                        cmd.Parameters.AddWithValue("@DateTo", dt.Date);
                        cmd.Parameters.AddWithValue("@AreaFilter", area);
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                labels.Add((r["FullName"] ?? r["UserID"] ?? "").ToString().Trim());
                                int t = r["Target"] != DBNull.Value ? Convert.ToInt32(r["Target"]) : 0;
                                int re = r["Realisasi"] != DBNull.Value ? Convert.ToInt32(r["Realisasi"]) : 0;
                                dataTarget.Add(t * periodDays);
                                dataRealisasi.Add(re);
                            }
                        }
                    }
                    chartJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { labels = labels, dataTarget = dataTarget, dataRealisasi = dataRealisasi });
                }

                string display = df.ToString("dd/MM/yyyy") + " - " + dt.ToString("dd/MM/yyyy");
                return new
                {
                    Success = true,
                    Total = total,
                    Priority = priority,
                    Regular = regular,
                    TotalVisit = totalVisit,
                    TargetVisit = targetVisit,
                    VisitPct = visitPct,
                    VisitPeriod = "(" + display + ")",
                    VisitPeriodDisplay = display,
                    PctExisting = pctExisting,
                    PctNew = pctNew,
                    PctTrial = pctTrial,
                    PendingVisits = pendingVisits,
                    ChartJson = chartJson
                };
            }
            catch (Exception ex)
            {
                return new { Success = false, Error = ex.Message };
            }
        }

        /// <summary>Reads date range from hidDateFrom/hidDateTo. Defaults to yesterday if invalid.</summary>
        private void GetDateRangeFromPage(out DateTime dateFrom, out DateTime dateTo)
        {
            string df = hidDateFrom != null ? (hidDateFrom.Value ?? "").Trim() : "";
            string dt = hidDateTo != null ? (hidDateTo.Value ?? "").Trim() : "";
            ParseDateRange(df, dt, out dateFrom, out dateTo);
        }

        /// <summary>Builds area filter string from area checkboxes: "" = all, "WEST", "EAST", or "WEST,EAST".</summary>
        private string GetAreaFilterFromCheckboxes()
        {
            var parts = new List<string>();
            if (chkAreaWest != null && chkAreaWest.Checked) parts.Add("WEST");
            if (chkAreaEast != null && chkAreaEast.Checked) parts.Add("EAST");
            return string.Join(",", parts);
        }

        /// <summary>Gets connection string for Recordset/OleDb (LoadCustomerListForModal, LoadOperationalCount). Prefers session; falls back to VTSAdminDB (OLE DB).</summary>
        private string GetConnForOperationalRecordset()
        {
            string conn = (Session["ClsTypeDBConnStringSQL"] ?? "").ToString().Trim();
            if (!string.IsNullOrEmpty(conn)) return conn;
            var vtsAdminDb = ConfigurationManager.ConnectionStrings["VTSAdminDB"];
            if (vtsAdminDb != null && !string.IsNullOrEmpty(vtsAdminDb.ConnectionString))
                return vtsAdminDb.ConnectionString;
            var vtsAdmin = ConfigurationManager.ConnectionStrings["VTSADMIN"];
            if (vtsAdmin != null && !string.IsNullOrEmpty(vtsAdmin.ConnectionString))
                return vtsAdmin.ConnectionString;
            return "";
        }

        /// <summary>Gets connection string for SqlConnection (SetManagementButtonVisibility). Prefers session; falls back to VTSADMIN.</summary>
        private string GetConnForSetManagement()
        {
            string conn = (Session["ClsTypeDBConnStringSQL"] ?? "").ToString().Trim();
            if (!string.IsNullOrEmpty(conn)) return conn;
            var vtsAdmin = ConfigurationManager.ConnectionStrings["VTSADMIN"];
            if (vtsAdmin != null && !string.IsNullOrEmpty(vtsAdmin.ConnectionString))
                return vtsAdmin.ConnectionString;
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ClsTypeAccessMenu"] == null || Session["ClsTypeIsLogin"] == null || Session["ClsTypeUserID"] == null)
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHOPERATIONAL"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }
                ClsType ClType = new ClsType();
                if (hidDateFrom != null && string.IsNullOrEmpty(hidDateFrom.Value))
                {
                    var y = DateTime.Today.AddDays(-1);
                    hidDateFrom.Value = y.ToString("yyyy-MM-dd");
                    if (hidDateTo != null) hidDateTo.Value = y.ToString("yyyy-MM-dd");
                }
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            string conn = GetConnForOperationalRecordset();
                            LoadOperationalCount(conn);
                            SetManagementButtonVisibility(GetConnForSetManagement());
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
                SyncDateRangeLabel();
            }
            catch (Exception)
            {
                EnsureEmptyDisplay();
            }
        }

        private void SetManagementButtonVisibility(string conn)
        {
            if (PanelManagementButton == null) return;
            try
            {
                string groupId = (Session["ClsTypeUserGroupID"] ?? "").ToString().Trim();
                if (!string.IsNullOrEmpty(groupId))
                {
                    string g = groupId.ToUpperInvariant();
                    if (g == "ADMINISTRATORS" || g == "ADMINISTRATOR")
                    {
                        PanelManagementButton.Visible = true;
                        return;
                    }
                }

                if (string.IsNullOrEmpty(conn)) { PanelManagementButton.Visible = false; return; }
                string userId = (Session["ClsTypeUserID"] ?? "").ToString().Trim();
                if (string.IsNullOrEmpty(userId)) { PanelManagementButton.Visible = false; return; }
                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand(
                    "SELECT 1 FROM conf_auth_user WHERE UserID = @UserID AND ISNULL(Status, '') <> 'DE' AND LOWER(LTRIM(RTRIM(ISNULL(GroupID, '')))) IN ('administrators', 'administrator')",
                    connObj))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    connObj.Open();
                    var val = cmd.ExecuteScalar();
                    PanelManagementButton.Visible = (val != null && val != DBNull.Value);
                }
            }
            catch
            {
                PanelManagementButton.Visible = false;
            }
        }

        private void SyncDateRangeLabel()
        {
            string df = hidDateFrom != null ? (hidDateFrom.Value ?? "").Trim() : "";
            string dt = hidDateTo != null ? (hidDateTo.Value ?? "").Trim() : "";
            if (string.IsNullOrEmpty(df) || string.IsNullOrEmpty(dt))
            {
                var y = DateTime.Today.AddDays(-1);
                df = y.ToString("yyyy-MM-dd");
                dt = df;
            }
            DateTime d1, d2;
            string display = (DateTime.TryParse(df, out d1) && DateTime.TryParse(dt, out d2))
                ? d1.ToString("dd/MM/yyyy") + " - " + d2.ToString("dd/MM/yyyy")
                : "(Periode)";
            if (lblVisitPeriod != null) lblVisitPeriod.InnerText = "(" + display + ")";
            if (spanTeamPeriod != null) spanTeamPeriod.InnerText = display;
        }

        protected void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SyncDateRangeLabel();
                string conn = GetConnForOperationalRecordset();
                if (!string.IsNullOrEmpty(conn))
                {
                    LoadOperationalCount(conn);
                }
            }
            catch { }
        }

        private void LoadOperationalCount(string conn)
        {
            try
            {
                DateTime df, dt;
                GetDateRangeFromPage(out df, out dt);
                string area = GetAreaFilterFromCheckboxes();

                var Rec = new Recordset();
                Rec.Open("sp_dashboard_operational_total_customer '" + (area ?? "").Replace("'", "''") + "'", conn);
                if (Rec.RecordCount() > 0)
                {
                    int total = Convert.ToInt32(Rec.Fields("total"));
                    int regular = Convert.ToInt32(Rec.Fields("regular"));
                    int priority = Convert.ToInt32(Rec.Fields("priority"));

                    if (lblTotalCustomers != null) lblTotalCustomers.InnerText = total.ToString("#,##0");
                    if (lblPriorityCount != null) lblPriorityCount.InnerText = priority.ToString("#,##0");
                    if (lblRegularCount != null) lblRegularCount.InnerText = regular.ToString("#,##0");
                }
                else
                {
                    if (lblTotalCustomers != null) lblTotalCustomers.InnerText = "0";
                    if (lblPriorityCount != null) lblPriorityCount.InnerText = "0";
                    if (lblRegularCount != null) lblRegularCount.InnerText = "0";
                }

                bool excludeWeekend = chkExcludeWeekend != null && chkExcludeWeekend.Checked;
                bool excludeHoliday = chkExcludeHoliday != null && chkExcludeHoliday.Checked;
                LoadVisitStats(conn, df, dt, excludeWeekend, excludeHoliday);
                LoadCustomerStatus(conn, area);
                LoadBelumVisitCount(df, dt, area);
                LoadChartData(conn, df, dt, excludeWeekend, excludeHoliday, area);
            }
            catch
            {
                EnsureEmptyDisplay();
            }
        }

        private void LoadVisitStats(string conn, DateTime dateFrom, DateTime dateTo, bool excludeWeekend = false, bool excludeHoliday = false)
        {
            try
            {
                var Rec = new Recordset();
                Rec.Open("sp_dashboard_operational_visit_stats '" + dateFrom.ToString("yyyy-MM-dd") + "', '" + dateTo.ToString("yyyy-MM-dd") + "'", conn);
                if (Rec.RecordCount() > 0)
                {
                    int totalVisit = Convert.ToInt32(Rec.Fields("TotalVisit"));
                    int dailyTargetVisit = Convert.ToInt32(Rec.Fields("TargetVisit"));
                    int periodDays = GetPeriodDays(dateFrom, dateTo, excludeWeekend, excludeHoliday);
                    int targetVisit = dailyTargetVisit * periodDays;
                    if (lblTotalVisits != null) lblTotalVisits.InnerText = totalVisit.ToString("#,##0");
                    if (lblTargetVisits != null) lblTargetVisits.InnerText = targetVisit.ToString("#,##0");

                    int pct = (targetVisit > 0) ? Math.Min(100, (int)Math.Round(100.0 * totalVisit / targetVisit)) : 0;
                    if (divVisitProgress != null)
                    {
                        divVisitProgress.Style["width"] = pct + "%";
                        divVisitProgress.InnerText = pct + "%";
                    }
                }
            }
            catch
            {
                if (lblTotalVisits != null) lblTotalVisits.InnerText = "0";
                if (lblTargetVisits != null) lblTargetVisits.InnerText = "0";
                if (divVisitProgress != null) { divVisitProgress.Style["width"] = "0%"; divVisitProgress.InnerText = "0%"; }
            }
        }

        /// <summary>Loads Status Customer percentages. Existing = has (any) training; New = not trained at all; Trial = company name starts with "Trial". See sp_dashboard_operational_customer_status.sql.</summary>
        private void LoadCustomerStatus(string conn, string areaFilter = "")
        {
            try
            {
                string area = (areaFilter ?? "").Replace("'", "''");
                var Rec = new Recordset();
                Rec.Open("sp_dashboard_operational_customer_status '" + area + "'", conn);
                if (Rec.RecordCount() > 0)
                {
                    if (lblStatusExisting != null) lblStatusExisting.InnerText = Rec.Fields("PctExisting").ToString();
                    if (lblStatusNew != null) lblStatusNew.InnerText = Rec.Fields("PctNew").ToString();
                    if (lblStatusTrial != null) lblStatusTrial.InnerText = Rec.Fields("PctTrial").ToString();
                }
            }
            catch
            {
                if (lblStatusExisting != null) lblStatusExisting.InnerText = "0";
                if (lblStatusNew != null) lblStatusNew.InnerText = "0";
                if (lblStatusTrial != null) lblStatusTrial.InnerText = "0";
            }
        }

        /// <summary>Loads Total Belum Visit count (customers not visited in selected period) from sp_dashboard_operational_customer_not_visit.</summary>
        private void LoadBelumVisitCount(DateTime dateFrom, DateTime dateTo, string areaFilter)
        {
            try
            {
                string conn = null;
                if (ConfigurationManager.ConnectionStrings["VTSAdminDB"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ConnectionString;
                if (string.IsNullOrEmpty(conn) && ConfigurationManager.ConnectionStrings["VTSADMIN"] != null)
                    conn = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                if (HttpContext.Current?.Session?["ClsTypeDBConnStringSQL"] != null)
                {
                    var s = HttpContext.Current.Session["ClsTypeDBConnStringSQL"].ToString();
                    if (!string.IsNullOrEmpty(s)) conn = conn ?? s;
                }
                if (string.IsNullOrEmpty(conn)) { if (lblPendingVisits != null) lblPendingVisits.InnerText = "0"; return; }
                conn = GetSqlClientConnectionString(conn);
                using (var connObj = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_dashboard_operational_customer_not_visit", connObj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom.Date);
                    cmd.Parameters.AddWithValue("@DateTo", dateTo.Date);
                    cmd.Parameters.AddWithValue("@AreaFilter", string.IsNullOrEmpty(areaFilter) ? "" : areaFilter.Trim());
                    connObj.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read() && r["Cnt"] != DBNull.Value && lblPendingVisits != null)
                            lblPendingVisits.InnerText = Convert.ToInt32(r["Cnt"]).ToString("#,##0");
                        else if (lblPendingVisits != null)
                            lblPendingVisits.InnerText = "0";
                    }
                }
            }
            catch
            {
                if (lblPendingVisits != null) lblPendingVisits.InnerText = "0";
            }
        }

        private void LoadChartData(string conn, DateTime dateFrom, DateTime dateTo, bool excludeWeekend = false, bool excludeHoliday = false, string areaFilter = "")
        {
            try
            {
                string area = (areaFilter ?? "").Replace("'", "''");
                var Rec = new Recordset();
                Rec.Open("sp_dashboard_operational_chart '" + dateFrom.ToString("yyyy-MM-dd") + "', '" + dateTo.ToString("yyyy-MM-dd") + "', '" + area + "'", conn);
                var labels = new List<string>();
                var dataTarget = new List<int>();
                var dataRealisasi = new List<int>();
                int periodDays = GetPeriodDays(dateFrom, dateTo, excludeWeekend, excludeHoliday);
                if (Rec.RecordCount() > 0)
                {
                    do
                    {
                        labels.Add(Rec.Fields("FullName")?.ToString()?.Trim() ?? Rec.Fields("UserID")?.ToString() ?? "");
                        int t, r;
                        int dailyTarget = int.TryParse(Rec.Fields("Target")?.ToString() ?? "0", out t) ? t : 0;
                        dataTarget.Add(dailyTarget * periodDays);
                        dataRealisasi.Add(int.TryParse(Rec.Fields("Realisasi")?.ToString() ?? "0", out r) ? r : 0);
                        Rec.MoveNext();
                    } while (!Rec.EOF);
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { labels = labels, dataTarget = dataTarget, dataRealisasi = dataRealisasi });
                if (chartOperationalData != null) chartOperationalData.Value = json;
            }
            catch { }
        }



        private void EnsureEmptyDisplay()
        {
            if (lblTotalCustomers != null) lblTotalCustomers.InnerText = "0";
            if (lblPriorityCount != null) lblPriorityCount.InnerText = "0";
            if (lblRegularCount != null) lblRegularCount.InnerText = "0";
            if (lblTotalVisits != null) lblTotalVisits.InnerText = "0";
            if (lblTargetVisits != null) lblTargetVisits.InnerText = "0";
            if (lblVisitPeriod != null) lblVisitPeriod.InnerText = "(Periode)";
            if (divVisitProgress != null) { divVisitProgress.Style["width"] = "0%"; divVisitProgress.InnerText = "0%"; }
            if (lblPendingVisits != null) lblPendingVisits.InnerText = "0";
            if (lblStatusExisting != null) lblStatusExisting.InnerText = "0";
            if (lblStatusNew != null) lblStatusNew.InnerText = "0";
            if (lblStatusTrial != null) lblStatusTrial.InnerText = "0";
        }
    }
}
