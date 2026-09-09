using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class installation_job_new : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ClsTypeAccessMenu"] == null ||
                    !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLNEW"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] == null ||
                        !new ClsType().SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Response.Redirect("login.aspx");
                        return;
                    }
                    div_comment.InnerHtml = "";
                    div_validation.InnerHtml = "";
                }
            }
            catch (Exception)
            {
            }
        }

        protected void CmdCancel_ServerClick(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            div_validation.InnerHtml = "";
        }

        #region Helper

        private static string Esc(string value)
        {
            return (value ?? "").Replace("'", "''");
        }

        private static string GetSessionConn()
        {
            var ctx = HttpContext.Current;
            if (ctx?.Session?["ClsTypeDBConnStringSQL"] == null) return "";
            return ctx.Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        private static string GetSessionUserTechnicianId()
        {
            var ctx = HttpContext.Current;
            if (ctx?.Session?["ClsTypeUserTechnicianID"] == null) return "";
            return ctx.Session["ClsTypeUserTechnicianID"].ToString().Trim();
        }

        private static Dictionary<string, string> ReadRow(Recordset rec)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (rec.RecData?.Tables.Count == 0 || rec.RecData.Tables[0] == null) return dict;
            for (int i = 0; i < rec.FieldCount(); i++)
            {
                string name = rec.RecData.Tables[0].Columns[i].ColumnName;
                dict[name] = rec.Fields(name);
            }
            return dict;
        }

        private static string F(Dictionary<string, string> d, params string[] keys)
        {
            foreach (var k in keys)
            {
                if (d.ContainsKey(k) && !string.IsNullOrWhiteSpace(d[k])) return d[k].Trim();
            }
            return "";
        }

        private static List<Dictionary<string, string>> QuerySpRows(string spCall, out string error)
        {
            error = "";
            var list = new List<Dictionary<string, string>>();
            var rec = new Recordset();
            rec.Open(spCall, GetSessionConn(), ref error);
            if (!string.IsNullOrEmpty(error)) return list;
            while (!rec.EOF)
            {
                list.Add(ReadRow(rec));
                rec.MoveNext();
            }
            return list;
        }

        private static ApiResult RowsResult(List<Dictionary<string, string>> rows, string error)
        {
            if (!string.IsNullOrEmpty(error))
                return new ApiResult { Success = false, Message = error };
            return new ApiResult { Success = true, Data = rows };
        }

        private static int ParseIntSafe(string value)
        {
            int n;
            return int.TryParse((value ?? "").Trim(), out n) ? n : 0;
        }

        private static string NormalizeBase64(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            var trimmed = value.Trim();
            var comma = trimmed.IndexOf(',');
            if (comma >= 0)
            {
                var prefix = trimmed.Substring(0, comma);
                if (prefix.IndexOf("base64", StringComparison.OrdinalIgnoreCase) >= 0)
                    trimmed = trimmed.Substring(comma + 1);
            }
            var sb = new StringBuilder(trimmed.Length);
            for (int i = 0; i < trimmed.Length; i++)
            {
                if (!char.IsWhiteSpace(trimmed[i]))
                    sb.Append(trimmed[i]);
            }
            return sb.ToString();
        }

        private static byte[] DecodeBase64Content(string rawBase64)
        {
            var normalized = NormalizeBase64(rawBase64);
            if (string.IsNullOrEmpty(normalized))
                throw new FormatException("Base64 kosong.");
            return Convert.FromBase64String(normalized);
        }

        private static bool TryDetectAllowedImageExt(byte[] bytes, out string ext)
        {
            ext = "";
            if (bytes == null || bytes.Length < 2) return false;
            if (bytes[0] == 0xFF && bytes[1] == 0xD8)
            {
                ext = ".jpg";
                return true;
            }
            if (bytes.Length >= 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            {
                ext = ".png";
                return true;
            }
            return false;
        }

        private static float SafeDpi(float dpi)
        {
            // Some images contain invalid 0/NaN DPI metadata that makes SetResolution throw
            // "Parameter is not valid." in GDI+.
            if (float.IsNaN(dpi) || float.IsInfinity(dpi) || dpi <= 0f || dpi > 1200f)
                return 96f;
            return dpi;
        }

        private static byte[] ResizeImageIfNeeded(byte[] originalBytes, string ext)
        {
            const int maxWidth = 1600;
            const int maxHeight = 1600;
            const byte jpegQuality = 82;

            if (originalBytes == null || originalBytes.Length == 0) return originalBytes;

            try
            {
                using (var inputMs = new MemoryStream(originalBytes))
                using (var originalImage = Image.FromStream(inputMs, true, true))
                {
                    int srcW = originalImage.Width;
                    int srcH = originalImage.Height;
                    if (srcW <= maxWidth && srcH <= maxHeight)
                        return originalBytes;

                    double ratioW = (double)maxWidth / srcW;
                    double ratioH = (double)maxHeight / srcH;
                    double ratio = Math.Min(ratioW, ratioH);
                    int dstW = Math.Max(1, (int)Math.Round(srcW * ratio));
                    int dstH = Math.Max(1, (int)Math.Round(srcH * ratio));

                    using (var bitmap = new Bitmap(dstW, dstH))
                    {
                        var dpiX = SafeDpi(originalImage.HorizontalResolution);
                        var dpiY = SafeDpi(originalImage.VerticalResolution);
                        bitmap.SetResolution(dpiX, dpiY);
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.DrawImage(originalImage, 0, 0, dstW, dstH);
                        }

                        using (var outMs = new MemoryStream())
                        {
                            if (string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase))
                            {
                                bitmap.Save(outMs, ImageFormat.Png);
                            }
                            else
                            {
                                var codec = GetImageCodec("image/jpeg");
                                if (codec == null)
                                {
                                    bitmap.Save(outMs, ImageFormat.Jpeg);
                                }
                                else
                                {
                                    using (var encParams = new EncoderParameters(1))
                                    {
                                        encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
                                        bitmap.Save(outMs, codec, encParams);
                                    }
                                }
                            }
                            return outMs.ToArray();
                        }
                    }
                }
            }
            catch
            {
                // Keep save process resilient: if GDI+ fails for specific metadata/profile,
                // use original bytes instead of failing the whole submit.
                return originalBytes;
            }
        }

        private static ImageCodecInfo GetImageCodec(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
            {
                if (string.Equals(codecs[i].MimeType, mimeType, StringComparison.OrdinalIgnoreCase))
                    return codecs[i];
            }
            return null;
        }

        private sealed class PendingImageFile
        {
            public string FileName { get; set; }
            public byte[] Content { get; set; }
            public string FullPath { get; set; }
        }

        private static bool SaveAccessoriesByCommand(string tvdId, List<AccessoryPickInput> selectedAccessories, string userId, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(tvdId) || selectedAccessories == null || selectedAccessories.Count == 0)
                return true;

            try
            {
                var uniq = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var ec = new ExecCommand();
                int aff = 0;

                for (int i = 0; i < selectedAccessories.Count; i++)
                {
                    var item = selectedAccessories[i] ?? new AccessoryPickInput();
                    var accTdtId = (item.TdtID ?? "").Trim();
                    var accDeviceId = (item.DeviceID ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(accTdtId) || string.IsNullOrWhiteSpace(accDeviceId))
                        continue;

                    var key = accTdtId + "|" + accDeviceId;
                    if (!uniq.Add(key)) continue;

                    string sql = "sp_new_installation_accessories_selected '" + Esc(tvdId) + "','" + Esc(accTdtId) + "','" + Esc(accDeviceId) + "','" + Esc(userId) + "'";
                    if (!ec.Execute(sql, GetSessionConn(), ref aff, ref error))
                    {
                        if (string.IsNullOrWhiteSpace(error))
                            error = "Gagal menyimpan accessories terpilih.";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static string GetJwtAuth(string sUserID, string sPassword, string sAppID, string sUrl)
        {
            string strJwt = "";
            try
            {
                string sCred = Convert.ToBase64String(Encoding.ASCII.GetBytes((sUserID ?? "").Trim() + ":" + (sPassword ?? "").Trim()));
                WebClient wc = new WebClient();
                wc.QueryString.Add("app_id", sAppID ?? "");
                wc.Headers.Add("Accept", "application/json");
                wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + sCred);
                var response = wc.DownloadString(sUrl);
                var resJwt = new ResultJwt();
                JsonConvert.PopulateObject(response, resJwt);
                strJwt = (resJwt.jwt ?? "").Trim();
            }
            catch
            {
                strJwt = "";
            }
            return strJwt;
        }

        private static bool EsealAdd(string vehicleid, string noImei, string merk, string model, string tipe, ref string statusMessage, ref string outMessage)
        {
            var isValid = true;
            try
            {
                var ctx = HttpContext.Current;
                string connection = (ctx?.Session?["ClsTypeDBConnStringSQL"] ?? "").ToString();
                string url_api = "";
                string url_jwt = "";
                string app_id = "";
                string vendorid = "";
                string token = "";
                string sUserID = "";
                string sPassword = "";

                string strSQLAPI = "sp_get_api_config 'BCESEAL-ADD'";
                Recordset RecAPI = new Recordset();
                RecAPI.Open(strSQLAPI, connection);

                if (RecAPI.RecordCount() > 0)
                {
                    url_api = RecAPI.Fields("url_api");
                    url_jwt = RecAPI.Fields("url_req_jwt");
                    app_id = RecAPI.Fields("app_id");
                    vendorid = RecAPI.Fields("vendorid");
                    token = RecAPI.Fields("token");
                    sUserID = RecAPI.Fields("userid");
                    sPassword = RecAPI.Fields("password");

                    Uri uri = new Uri(url_api);
                    var param = new ParamEsealAdd
                    {
                        idVendor = vendorid,
                        merk = merk,
                        model = model,
                        noImei = noImei,
                        tipe = tipe,
                        token = token
                    };
                    string sJwt = GetJwtAuth(sUserID, sPassword, app_id, url_jwt);

                    var rawData = JsonConvert.SerializeObject(param);
                    WebClient webclient = new WebClient();
                    webclient.UseDefaultCredentials = true;
                    webclient.Headers.Add("Accept", "application/json");
                    webclient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + sJwt);
                    webclient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = webclient.UploadString(uri, "POST", rawData);
                    var myObject = new ResponseEsealAdd();
                    JsonConvert.PopulateObject(response, myObject);
                    outMessage = myObject.status;
                    statusMessage = myObject.message;
                    if ((myObject.status ?? "").ToLower() == "success" && myObject.item != null)
                    {
                        var strSQL = "sp_save_log_api_eseal_add '" + Esc(myObject.item.idEseal) + "','" + Esc(myObject.item.merk) + "','" + Esc(myObject.item.model) + "','" + Esc(myObject.item.tipe) + "','" + Esc(myObject.item.idVendor) + "','" + Esc(myObject.item.noImei) + "','" + Esc(myObject.item.status) + "','" + Esc(myObject.item.wkRekam) + "','" + Esc(myObject.item.wkUpdate) + "','" + Esc(outMessage) + "','" + Esc(statusMessage) + "'";
                        ExecCommand ec = new ExecCommand();
                        int intAff = 0;
                        string sErr = "";
                        ec.Execute(strSQL, connection, ref intAff, ref sErr);
                    }
                }
                else
                {
                    statusMessage = "Config API Eseal tidak ditemukan.";
                    isValid = false;
                }
            }
            catch (WebException ex)
            {
                isValid = false;
                if (ex.Response != null)
                {
                    var response = ex.Response;
                    var dataStream = response.GetResponseStream();
                    var reader = new StreamReader(dataStream);
                    var details = reader.ReadToEnd();
                    outMessage = (outMessage ?? "") + "," + details;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                outMessage = ex.Message;
            }
            return isValid;
        }

        private static string ResolveTvdIdByKeys(string tvaId, string tdtId, string tgtId, string technicianId)
        {
            if (string.IsNullOrWhiteSpace(tvaId) || string.IsNullOrWhiteSpace(tdtId) ||
                string.IsNullOrWhiteSpace(tgtId) || string.IsNullOrWhiteSpace(technicianId))
                return "";

            try
            {
                string err = "";
                var rec = new Recordset();
                rec.Open(
                    "sp_new_installation_get_tvdid '" + Esc(tvaId.Trim()) + "','" + Esc(tdtId.Trim()) + "','" + Esc(tgtId.Trim()) + "','" + Esc(technicianId.Trim()) + "'",
                    GetSessionConn(),
                    ref err);
                if (!string.IsNullOrEmpty(err) || rec.EOF || rec.FieldCount() == 0) return "";
                return (rec.Fields(0) ?? "").Trim();
            }
            catch
            {
                return "";
            }
        }

        private static string ResolveTvdIdByKeysInScope(OleDbConnection connection, OleDbTransaction transaction, string tvaId, string tdtId, string tgtId, string technicianId)
        {
            if (connection == null || connection.State != ConnectionState.Open || transaction == null)
                return "";
            if (string.IsNullOrWhiteSpace(tvaId) || string.IsNullOrWhiteSpace(tdtId) ||
                string.IsNullOrWhiteSpace(tgtId) || string.IsNullOrWhiteSpace(technicianId))
                return "";

            using (var cmd = new OleDbCommand("sp_new_installation_get_tvdid", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;
                cmd.Parameters.AddWithValue("?", tvaId.Trim());
                cmd.Parameters.AddWithValue("?", tdtId.Trim());
                cmd.Parameters.AddWithValue("?", tgtId.Trim());
                cmd.Parameters.AddWithValue("?", technicianId.Trim());

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader != null && reader.Read() && !reader.IsDBNull(0))
                        return (reader.GetValue(0) ?? "").ToString().Trim();
                }
            }

            return "";
        }

        private static string BuildExceptionMessage(Exception ex)
        {
            if (ex == null) return "Unknown error.";
            var sb = new StringBuilder();
            int depth = 0;
            var cur = ex;
            while (cur != null && depth < 5)
            {
                var part = (cur.Message ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(part))
                {
                    if (sb.Length > 0) sb.Append(" | ");
                    sb.Append(part);
                }

                var ole = cur as OleDbException;
                if (ole != null && ole.Errors != null && ole.Errors.Count > 0)
                {
                    for (int i = 0; i < ole.Errors.Count; i++)
                    {
                        var m = (ole.Errors[i].Message ?? "").Trim();
                        if (!string.IsNullOrWhiteSpace(m))
                        {
                            if (sb.Length > 0) sb.Append(" | ");
                            sb.Append(m);
                        }
                    }
                }

                cur = cur.InnerException;
                depth++;
            }
            return sb.Length > 0 ? sb.ToString() : "Terjadi kesalahan saat memproses data.";
        }

        #endregion

        #region WebMethods

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallJobList(string Search)
        {
            try
            {
                string err = "";
                var rows = QuerySpRows("sp_list_new_installation_job_order_search '" + Esc(Search ?? "") + "'", out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallTechnicianList(string Search)
        {
            try
            {
                string err = "";
                var rows = QuerySpRows(
                    "sp_list_new_installation_technician_search '" + Esc(GetSessionUserTechnicianId()) + "','" + Esc(Search ?? "") + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallVehicleList(string CustID, string PoliceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };
                string err = "";
                var rows = QuerySpRows(
                    "sp_list_new_installation_vehicle_search '" + Esc(CustID.Trim()) + "','" + Esc(PoliceNo ?? "") + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallDeviceList(string JobID, string TechnicianID, string NoSN)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(JobID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };
                if (string.IsNullOrWhiteSpace(TechnicianID))
                    return new ApiResult { Success = false, Message = "Pilih Technician ID terlebih dahulu." };
                string err = "";
                var rows = QuerySpRows(
                    "sp_list_new_installation_device_search_new '" + Esc(JobID.Trim()) + "','" + Esc(TechnicianID.Trim()) + "','" + Esc(NoSN ?? "") + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallGsmList(string TechnicianID, string MSIDN)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TechnicianID))
                    return new ApiResult { Success = false, Message = "Pilih Technician ID terlebih dahulu." };
                string err = "";
                var rows = QuerySpRows(
                    "sp_list_new_installation_gsm_search '" + Esc(TechnicianID.Trim()) + "','" + Esc(MSIDN ?? "") + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult ResolveTvdId(string TvaID, string TdtID, string TgtID, string TechnicianID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TvaID) || string.IsNullOrWhiteSpace(TdtID) ||
                    string.IsNullOrWhiteSpace(TgtID) || string.IsNullOrWhiteSpace(TechnicianID))
                    return new ApiResult { Success = true, Data = new { TvdID = "" } };

                string err = "";
                var rec = new Recordset();
                rec.Open(
                    "sp_new_installation_get_tvdid '" + Esc(TvaID) + "','" + Esc(TdtID) + "','" + Esc(TgtID) + "','" + Esc(TechnicianID) + "'",
                    GetSessionConn(),
                    ref err);
                if (!string.IsNullOrEmpty(err))
                    return new ApiResult { Success = false, Message = err };

                string tvdId = "";
                if (!rec.EOF && rec.FieldCount() > 0)
                    tvdId = rec.Fields(0);
                return new ApiResult { Success = true, Data = new { TvdID = tvdId ?? "" } };
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallMasterInfo(string TvaID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TvaID))
                    return new ApiResult { Success = true, Data = new { MarketingName = "" } };

                string err = "";
                var rows = QuerySpRows("sp_list_new_installation_master '" + Esc(TvaID.Trim()) + "'", out err);
                if (!string.IsNullOrEmpty(err))
                    return new ApiResult { Success = false, Message = err };

                string marketing = "";
                if (rows.Count > 0)
                    marketing = F(rows[0], "MasterFullName", "MarketingName", "MasterName");

                return new ApiResult { Success = true, Data = new { MarketingName = marketing } };
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallAccessoriesList(string JobID, string TechnicianID, string Search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(JobID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };
                if (string.IsNullOrWhiteSpace(TechnicianID))
                    return new ApiResult { Success = false, Message = "Pilih Technician ID terlebih dahulu." };

                string err = "";
                var rows = QuerySpRows(
                    "sp_installation_job_list_accessories_available '" + Esc(JobID.Trim()) + "','" + Esc(TechnicianID.Trim()) + "','" + Esc(Search ?? "") + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallJobCreateDetails(string JobID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(JobID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };

                string err = "";
                var rows = QuerySpRows(
                    "sp_view_job_create_details '" + Esc(JobID.Trim()) + "',''",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetNewInstallServerList(string CustID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };

                string err = "";
                var rows = QuerySpRows(
                    "sp_list_new_installation_server '" + Esc(CustID.Trim()) + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetInterfacingUserLogin(string CustID, string ServerID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CustID))
                    return new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." };
                if (string.IsNullOrWhiteSpace(ServerID))
                    return new ApiResult { Success = false, Message = "Pilih Server Name terlebih dahulu." };

                string err = "";
                var rows = QuerySpRows(
                    "sp_installation_job_get_interfacing_user_login '" + Esc(CustID.Trim()) + "','" + Esc(ServerID.Trim()) + "'",
                    out err);
                return RowsResult(rows, err);
            }
            catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult SaveNewInstallation(
            string TvaID,
            string PoID,
            string VehicleID,
            string TdtID,
            string DeviceID,
            string TgtID,
            string GsmID,
            string NoGSM,
            string NoSN,
            string DeviceTypeDesc,
            string DeviceBrand,
            string DeviceModel,
            string TechnicianID,
            string JobID,
            string ServerIDInstall,
            string ServerIDDevice,
            string UserAccessAutoID,
            string InstallDate,
            string Remarks,
            string Contrac,
            string IsRelay,
            List<UploadImageInput> PictureFiles,
            string SelectedAccessoriesJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(JobID) || string.IsNullOrWhiteSpace(PoID) ||
                    string.IsNullOrWhiteSpace(TvaID) || string.IsNullOrWhiteSpace(VehicleID) ||
                    string.IsNullOrWhiteSpace(TdtID) || string.IsNullOrWhiteSpace(DeviceID) ||
                    string.IsNullOrWhiteSpace(TgtID) || string.IsNullOrWhiteSpace(GsmID) ||
                    string.IsNullOrWhiteSpace(TechnicianID))
                {
                    return new ApiResult { Success = false, Message = "Data wajib belum lengkap. Pastikan Job, Technician, Vehicle, Device, dan GSM sudah dipilih." };
                }

                var ctx = HttpContext.Current;
                if (ctx?.Session?["ClsTypeUserID"] == null)
                    return new ApiResult { Success = false, Message = "Session user tidak ditemukan. Silakan login ulang." };

                string installDate = string.IsNullOrWhiteSpace(InstallDate)
                    ? DateTime.Now.ToString("yyyy-MM-dd")
                    : InstallDate.Trim();
                int contrac = ParseIntSafe(Contrac);
                int isRelay = ParseIntSafe(IsRelay);
                string userId = ctx.Session["ClsTypeUserID"].ToString();
                string serverInstall = (ServerIDInstall ?? "").Trim();
                string serverDevice = (ServerIDDevice ?? "").Trim();
                if (string.IsNullOrEmpty(serverInstall)) serverInstall = serverDevice;
                if (string.IsNullOrEmpty(serverDevice)) serverDevice = serverInstall;
                string interfacingAutoId = (UserAccessAutoID ?? "").Trim();
                string msisdn = (NoGSM ?? "").Trim();
                string noSn = (NoSN ?? "").Trim();
                if (string.IsNullOrWhiteSpace(interfacingAutoId))
                    return new ApiResult { Success = false, Message = "User Akses wajib dipilih." };
                if (string.IsNullOrWhiteSpace(msisdn))
                    return new ApiResult { Success = false, Message = "No GSM belum tersedia." };
                var files = PictureFiles ?? new List<UploadImageInput>();
                if (files.Count > 10)
                    return new ApiResult { Success = false, Message = "Maksimal 10 file gambar." };
                var selectedAccessories = new List<AccessoryPickInput>();
                if (!string.IsNullOrWhiteSpace(SelectedAccessoriesJson))
                {
                    try
                    {
                        var js = new JavaScriptSerializer();
                        selectedAccessories = js.Deserialize<List<AccessoryPickInput>>(SelectedAccessoriesJson) ?? new List<AccessoryPickInput>();
                    }
                    catch
                    {
                        return new ApiResult { Success = false, Message = "Format accessories terpilih tidak valid." };
                    }
                }

                var pendingFiles = new List<PendingImageFile>();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    if (file == null)
                        return new ApiResult { Success = false, Message = "Data file tidak valid." };

                    var originalName = (file.FileName ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(file.ContentBase64))
                        return new ApiResult { Success = false, Message = "Isi file gambar tidak valid." };

                    byte[] content;
                    try
                    {
                        content = DecodeBase64Content(file.ContentBase64);
                    }
                    catch
                    {
                        return new ApiResult { Success = false, Message = "Konten base64 gambar tidak valid." };
                    }

                    if (content.Length == 0 || content.Length > (5 * 1024 * 1024))
                        return new ApiResult { Success = false, Message = "Ukuran tiap file maksimal 5 MB." };

                    string ext;
                    if (!TryDetectAllowedImageExt(content, out ext))
                        return new ApiResult { Success = false, Message = "Signature file gambar tidak valid. Gunakan file JPG/JPEG atau PNG asli." };
                    content = ResizeImageIfNeeded(content, ext);
                    if (content.Length > (5 * 1024 * 1024))
                        return new ApiResult { Success = false, Message = "Ukuran file gambar setelah resize masih terlalu besar." };

                    pendingFiles.Add(new PendingImageFile
                    {
                        FileName = "IJN_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ext,
                        Content = content
                    });
                }

                var pictureDir = ctx.Server.MapPath("~/Picture/");
                if (!Directory.Exists(pictureDir))
                    Directory.CreateDirectory(pictureDir);
                var createdPaths = new List<string>();
                string insertedTvdId = "";

                try
                {
                    for (int i = 0; i < pendingFiles.Count; i++)
                    {
                        var fullPath = Path.Combine(pictureDir, pendingFiles[i].FileName);
                        File.WriteAllBytes(fullPath, pendingFiles[i].Content);
                        pendingFiles[i].FullPath = fullPath;
                        createdPaths.Add(fullPath);
                    }

                    var executor = new StoredProcedureTransactionExecutor();
                    using (var connection = new OleDbConnection(GetSessionConn()))
                    {
                        connection.Open();
                        using (var tx = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                var parameters = new object[]
                                {
                                    TvaID.Trim(),
                                    PoID.Trim(),
                                    VehicleID.Trim(),
                                    TdtID.Trim(),
                                    DeviceID.Trim(),
                                    TgtID.Trim(),
                                    GsmID.Trim(),
                                    TechnicianID.Trim(),
                                    JobID.Trim(),
                                    serverInstall,
                                    serverDevice,
                                    installDate,
                                    (Remarks ?? "").Trim(),
                                    contrac,
                                    isRelay,
                                    pendingFiles.Count > 0 ? pendingFiles[0].FileName : "",
                                    userId
                                };

                                // Do not rely on ExecuteNonQuery affected rows for SP success,
                                // because many procedures use SET NOCOUNT ON and may return 0/-1.
                                executor.ExecuteInScope(connection, tx, "sp_insert_new_installation_new", parameters);
                                insertedTvdId = ResolveTvdIdByKeysInScope(connection, tx, TvaID, TdtID, TgtID, TechnicianID);
                                if (string.IsNullOrWhiteSpace(insertedTvdId))
                                    throw new InvalidOperationException("sp_insert_new_installation_new tidak menghasilkan data TVDID.");

                                if (pendingFiles.Count > 0)
                                {
                                    var pictureNames = new List<string>();
                                    for (int i = 0; i < pendingFiles.Count; i++) pictureNames.Add(pendingFiles[i].FileName);
                                    while (pictureNames.Count < 10) pictureNames.Add("");
                                    var picParams = new object[]
                                    {
                                        TdtID.Trim(),
                                        JobID.Trim(),
                                        pictureNames[0],
                                        pictureNames[1],
                                        pictureNames[2],
                                        pictureNames[3],
                                        pictureNames[4],
                                        pictureNames[5],
                                        pictureNames[6],
                                        pictureNames[7],
                                        pictureNames[8],
                                        pictureNames[9],
                                        userId
                                    };
                                    executor.ExecuteInScope(connection, tx, "sp_insert_trx_vehicle_device_picture", picParams);
                                }

                                try
                                {
                                    // Keep compatibility with legacy flow (new_install.aspx.cs): 3 parameters only.
                                    executor.ExecuteInScope(connection, tx, "sp_insert_interfacing_user_access", new object[]
                                    {
                                        noSn,
                                        interfacingAutoId,
                                        serverInstall
                                    });
                                }
                                catch
                                {
                                    // Fallback for installations that already use 4-parameter signature.
                                    // Do not use session user id here by request.
                                    executor.ExecuteInScope(connection, tx, "sp_insert_interfacing_user_access", new object[]
                                    {
                                        noSn,
                                        interfacingAutoId,
                                        serverInstall,
                                        ""
                                    });
                                }

                                tx.Commit();
                            }
                            catch
                            {
                                try { tx.Rollback(); } catch { }
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < createdPaths.Count; i++)
                    {
                        try
                        {
                            if (File.Exists(createdPaths[i])) File.Delete(createdPaths[i]);
                        }
                        catch { }
                    }
                    return new ApiResult { Success = false, Message = BuildExceptionMessage(ex) };
                }

                if (selectedAccessories.Count > 0)
                {
                    var tvdId = insertedTvdId;
                    if (string.IsNullOrWhiteSpace(tvdId))
                        tvdId = ResolveTvdIdByKeys(TvaID, TdtID, TgtID, TechnicianID);
                    if (string.IsNullOrWhiteSpace(tvdId))
                        return new ApiResult { Success = false, Message = "Data install berhasil disimpan, tetapi TVDID untuk accessories tidak ditemukan." };

                    string accErr;
                    if (!SaveAccessoriesByCommand(tvdId, selectedAccessories, userId, out accErr))
                        return new ApiResult { Success = false, Message = string.IsNullOrWhiteSpace(accErr) ? "Data install berhasil disimpan, tetapi simpan accessories gagal." : accErr };
                }

                var esealType = (DeviceTypeDesc ?? "").ToUpper().Trim();
                if (esealType == "ESEAL38" || esealType == "AT-10" || esealType == "JT-701" || esealType == "AT16")
                {
                    string esealStatus = "", esealMsg = "";
                    EsealAdd((VehicleID ?? "").Trim(), (NoSN ?? "").Trim(), (DeviceBrand ?? "").Trim(), (DeviceModel ?? "").Trim(), (DeviceTypeDesc ?? "").Trim(), ref esealStatus, ref esealMsg);
                    // Keep submit successful regardless of Eseal API response (same behavior as legacy flow).
                }

                return new ApiResult { Success = true, Message = "New installation berhasil disimpan." };
            }
            catch (Exception ex)
            {
                return new ApiResult { Success = false, Message = BuildExceptionMessage(ex) };
            }
        }

        #endregion

        public class ApiResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        public class UploadImageInput
        {
            public string FileName { get; set; }
            public string ContentBase64 { get; set; }
        }

        public class AccessoryPickInput
        {
            public string TdtID { get; set; }
            public string DeviceID { get; set; }
            public string NoSN { get; set; }
            public string DeviceTypeDesc { get; set; }
            public string Status { get; set; }
        }

        public class ResultJwt
        {
            public string jwt { get; set; }
        }

        public class ParamEsealAdd
        {
            public string idVendor { get; set; }
            public string merk { get; set; }
            public string model { get; set; }
            public string noImei { get; set; }
            public string tipe { get; set; }
            public string token { get; set; }
        }

        public class ResponseEsealAdd
        {
            public string status { get; set; }
            public string message { get; set; }
            public ResponseItemEsealAdd item { get; set; }
        }

        public class ResponseItemEsealAdd
        {
            public string idEseal { get; set; }
            public string merk { get; set; }
            public string model { get; set; }
            public string tipe { get; set; }
            public string idVendor { get; set; }
            public string noImei { get; set; }
            public string status { get; set; }
            public string wkRekam { get; set; }
            public string wkUpdate { get; set; }
        }
    }
}
