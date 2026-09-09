using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class installation_job_maint : Page
    {
        private const string VsProcessKey = "IjmProcessKey";
        private const string VsCategoryKey = "IjmCategoryKey";

        #region Load Data

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPageMethodRequest()) return;

                if (!JobInstallationProcess.HasHubAccess(GetAccessMenu()))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] == null || !new ClsType().SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Response.Redirect("login.aspx");
                        return;
                    }

                    ClearForm();
                    BindJobTypeDropdown();
                    ApplyJobTypeUi();
                    div_comment.InnerHtml = "";
                    div_validation.InnerHtml = "";
                }
                else
                {
                    SyncProcessFromDropdowns();
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsPageMethodRequest()
        {
            try
            {
                return Request != null
                    && !string.IsNullOrEmpty(Request.PathInfo)
                    && Request.PathInfo.StartsWith("/", StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        protected void CmbJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearJobSelection();
            SyncProcessFromDropdowns();
            ApplyJobTypeUi();
            div_validation.InnerHtml = "";
        }

        private void ClearForm()
        {
            hfTvdID.Value = "";
            hfTvaID.Value = "";
            hfTdtID.Value = "";
            hfTgtID.Value = "";
            hfDeviceID.Value = "";
            hfGsmID.Value = "";
            hfVehicleID.Value = "";
            hfCustID.Value = "";
            hfNewTdtID.Value = "";
            hfNewTgtID.Value = "";
            hfNewTvaID.Value = "";
            hfNewCustID.Value = "";
            hfNewDeviceID.Value = "";
            hfNewGsmID.Value = "";
            hfNewVehicleID.Value = "";
            hfJobCustID.Value = "";
            hfJobID.Value = "";
            hfServerID.Value = "";
            txtJobID.Value = "";
            txtRemark.Value = "";
            txtMaintTypeDesc.Value = "";
            hfMaintTypeID.Value = "";
            hfProcessKey.Value = "";
            ClearReadonlyPanels();
            ClearMaintPanels();
        }

        private void ClearJobSelection()
        {
            txtJobID.Value = "";
            hfJobID.Value = "";
            ClearReadonlyPanels();
            ClearMaintPanels();
        }

        private void ClearReadonlyPanels()
        {
            txtRegDate.Value = "";
            txtSchDate.Value = "";
            txtCustomerName.Value = "";
            txtBranchName.Value = "";
            txtMarketingName.Value = "";
            txtPoliceNo.Value = "";
            txtVehicleDesc.Value = "";
            txtAssetNo.Value = "";
            txtNoSNInfo.Value = "";
            txtDeviceTypeDesc.Value = "";
            txtWarehouseName.Value = "";
            txtNoGSMInfo.Value = "";
            txtProviderName.Value = "";
            txtGsmWarehouseName.Value = "";
            txtServerNameInfo.Value = "";
            txtTechnicianID.Value = "";
            txtTechnicianName.Value = "";
            txtTechBranchName.Value = "";
            hfTechnicianID.Value = "";
        }

        private void ClearMaintPanels()
        {
            ResetSelect(CmbNewNoSN);
            ResetSelect(CmbNewNoGSM);
            ResetSelect(CmbNewCustomer);
            ResetSelect(CmbNewServerName);
            ResetSelect(CmbNewPoliceNo);
            ClearNewInfoPanels();
        }

        private void ClearNewInfoPanels()
        {
            txtNewDeviceTypeDesc.Value = txtNewWarehouseName.Value = "";
            txtNewProviderName.Value = txtNewGsmWarehouse.Value = "";
            txtNewBranchName.Value = txtNewMarketingName.Value = "";
            txtNewVehicleDesc.Value = txtNewAssetNo.Value = "";
        }

        private void ApplyJobTypeUi()
        {
            var processKey = ViewState[VsProcessKey] as string ?? hfProcessKey.Value ?? "";
            PnlMaintDevice.Visible = processKey == "device_maint";
            PnlMaintGsm.Visible = processKey == "gsm_maint" || processKey == "gsm_maint_suspend";
            PnlMaintCustomer.Visible = processKey == "customer_maint";
            PnlMaintServer.Visible = processKey == "server_maint";
            PnlMaintVehicle.Visible = processKey == "vehicle_maint";

            if (IsPostBack)
                LoadMaintCombos();
        }

        private void LoadMaintCombos()
        {
            var process = GetCurrentProcess();
            if (process == null) return;
            var conn = GetConn();
            var clType = new ClsType();

            if (PnlMaintServer.Visible && CmbNewServerName != null)
            {
                clType.Open_Combos(CmbNewServerName, conn, hfCustID.Value.Trim(), "sp_list_server_maint_server");
                EnsureSelectItem(CmbNewServerName);
            }
        }

        protected void CmdCancel_ServerClick(object sender, EventArgs e)
        {
            ClearForm();
            BindJobTypeDropdown();
            ApplyJobTypeUi();
            div_comment.InnerHtml = "";
            div_validation.InnerHtml = "";
        }

        #endregion

        #region Bind Dropdown

        private void BindJobTypeDropdown()
        {
            CmbJobType.Items.Clear();
            CmbJobType.Items.Add(new ListItem("[Select]", ""));

            var rec = new Recordset();
            rec.Open("sp_installation_job_maint_type", GetConn());
            while (!rec.EOF)
            {
                string id = rec.Fields("MaintTypeID");
                string desc = rec.Fields("MaintTypeDesc");
                if (string.IsNullOrWhiteSpace(id)) { rec.MoveNext(); continue; }

                string processKey = ResolveProcessKeyFromMaintType(id.Trim(), desc);
                var process = JobInstallationProcess.Get(processKey);
                if (process != null && HasProcessAccess(process))
                    CmbJobType.Items.Add(new ListItem(desc, id.Trim()));
                rec.MoveNext();
            }

            SyncProcessFromDropdowns();
        }

        private string GetPostedJobId()
        {
            return hfJobID.Value.Trim();
        }

        private void SyncProcessFromDropdowns()
        {
            string maintTypeId = CmbJobType.SelectedValue ?? "";
            string maintTypeDesc = CmbJobType.SelectedItem != null ? CmbJobType.SelectedItem.Text : "";

            if (maintTypeId == "[Select]") maintTypeId = "";
            hfMaintTypeID.Value = maintTypeId;
            txtMaintTypeDesc.Value = maintTypeDesc == "[Select]" ? "" : maintTypeDesc;

            string processKey = ResolveProcessKeyFromMaintType(maintTypeId, maintTypeDesc);
            ViewState[VsCategoryKey] = processKey;
            ViewState[VsProcessKey] = processKey;
            hfProcessKey.Value = processKey;
        }

        private static string ResolveProcessKeyFromMaintType(string maintTypeId, string maintTypeDesc)
        {
            if (string.IsNullOrWhiteSpace(maintTypeId)) return "others_maint";

            switch (maintTypeId.Trim().ToUpperInvariant())
            {
                case "MTY0000001": return "vehicle_maint";
                case "MTY0000002": return "device_maint";
                case "MTY0000003": return "gsm_maint";
                case "MTY0000004": return "others_maint";
                case "MTY0000005": return "accessories_maint";
                case "MTY0000006": return "uninstall_maint";
                case "MTY0000007": return "server_maint";
                case "MTY0000008": return "customer_maint";
                case "MTY0000009": return "sb_maint";
                case "MTY0000010": return "ub_maint";
                case "MTY0000011": return "sp_maint";
                case "MTY0000012": return "gsm_maint_suspend";
            }

            string desc = (maintTypeDesc ?? "").Trim().ToLowerInvariant();
            if (desc.Contains("vehicle")) return "vehicle_maint";
            if (desc.Contains("device")) return "device_maint";
            if (desc.Contains("reactivated") && desc.Contains("gsm")) return "gsm_maint_suspend";
            if (desc.Contains("gsm")) return "gsm_maint";
            if (desc.Contains("customer")) return "customer_maint";
            if (desc.Contains("server")) return "server_maint";
            if (desc.Contains("accessories")) return "accessories_maint";
            if (desc.Contains("uninstall")) return "uninstall_maint";
            if (desc.Contains("soft") && desc.Contains("block")) return "sb_maint";
            if (desc.Contains("unblock")) return "ub_maint";
            if (desc.Contains("suspend")) return "sp_maint";
            if (desc.Contains("reactivat")) return "re_maint";
            if (desc.Contains("other")) return "others_maint";

            return "others_maint";
        }

        private JobInstallationProcess GetCurrentProcess()
        {
            var key = ViewState[VsProcessKey] as string ?? hfProcessKey.Value;
            return JobInstallationProcess.Get(key) ?? JobInstallationProcess.Get("others_maint");
        }

        #endregion

        #region Validation

        private bool ValidateSave(out string message)
        {
            message = "";

            if (string.IsNullOrEmpty(CmbJobType.SelectedValue) || CmbJobType.SelectedValue == "[Select]")
            {
                message = "Job Maintenance Type wajib dipilih.";
                return false;
            }
            if (string.IsNullOrEmpty(GetPostedJobId()))
            {
                message = "Job ID wajib dipilih.";
                return false;
            }
            if (string.IsNullOrEmpty(hfTechnicianID.Value.Trim()))
            {
                message = "Data technician belum termuat. Pilih Job ID dan tunggu data termuat.";
                return false;
            }

            var processKey = ViewState[VsProcessKey] as string ?? hfProcessKey.Value ?? "";

            switch (processKey)
            {
                case "device_maint":
                    if (!HasSelectValue(CmbNewNoSN)) { message = "New No SN wajib dipilih."; return false; }
                    if (string.IsNullOrWhiteSpace(GetPostedDeviceServerId())) { message = "Server Name wajib dipilih."; return false; }
                    if (string.IsNullOrWhiteSpace(GetPostedDeviceUserAccessAutoId())) { message = "User Access wajib dipilih."; return false; }
                    break;
                case "gsm_maint":
                case "gsm_maint_suspend":
                    if (!HasSelectValue(CmbNewNoGSM)) { message = "New No GSM wajib dipilih."; return false; }
                    break;
                case "customer_maint":
                    if (!HasSelectValue(CmbNewCustomer)) { message = "New Customer wajib dipilih."; return false; }
                    if (string.IsNullOrWhiteSpace(GetPostedCustomerServerId())) { message = "Server Name customer wajib dipilih."; return false; }
                    break;
                case "server_maint":
                    if (!IsComboSelected(CmbNewServerName)) { message = "New Server Name wajib dipilih."; return false; }
                    break;
                case "vehicle_maint":
                    if (!HasSelectValue(CmbNewPoliceNo)) { message = "New Police No wajib dipilih."; return false; }
                    break;
            }

            if (string.IsNullOrEmpty(hfTvdID.Value.Trim()))
            {
                message = "Data job belum lengkap. Pilih Job ID dan tunggu data termuat.";
                return false;
            }

            return true;
        }

        private static bool HasSelectValue(DropDownList cmb)
        {
            return cmb != null && !string.IsNullOrEmpty(cmb.SelectedValue) && cmb.SelectedValue != "[Select]";
        }

        private string GetSelectedServerId()
        {
            var processKey = ViewState[VsProcessKey] as string ?? hfProcessKey.Value ?? "";
            if (processKey == "server_maint")
                return CmbNewServerName != null ? CmbNewServerName.SelectedValue.Trim() : "";
            if (processKey == "device_maint")
                return GetPostedDeviceServerId();
            if (processKey == "customer_maint")
                return GetPostedCustomerServerId();
            return "";
        }

        private string GetPostedDeviceServerId()
        {
            var v = Request.Form["cmbDeviceServerName"];
            return (v ?? "").Trim();
        }

        private string GetPostedDeviceUserAccessAutoId()
        {
            var v = Request.Form["cmbDeviceUserAccess"];
            return (v ?? "").Trim();
        }

        private string GetPostedCustomerServerId()
        {
            var v = Request.Form["cmbCustomerServerName"];
            return (v ?? "").Trim();
        }

        private static bool IsComboSelected(DropDownList cmb)
        {
            return cmb != null && cmb.Items.Count > 0 && cmb.SelectedValue != "[Select]" && !string.IsNullOrEmpty(cmb.SelectedValue);
        }

        #endregion

        #region Save Process

        protected void CmdSave_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            div_validation.InnerHtml = "";

            string errMsg;
            if (!ValidateSave(out errMsg))
            {
                div_validation.InnerHtml = BuildAlert(errMsg, "warning");
                return;
            }

            var process = GetCurrentProcess();
            if (!HasProcessAccess(process))
            {
                div_comment.InnerHtml = BuildAlert("Access denied for " + process.Label + ".", "danger");
                return;
            }

            try
            {
                ExecuteMaintenanceSave(process);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = BuildAlert("Proses maintenance gagal: " + ex.Message, "danger");
            }
        }

        private void ExecuteMaintenanceSave(JobInstallationProcess process)
        {
            string conn = GetConn();
            string userId = Session["ClsTypeUserID"].ToString();
            string execTechId = Session["ClsTypeUserTechnicianID"].ToString();
            string techId = hfTechnicianID.Value.Trim();
            string jobId = GetPostedJobId();
            string remark = txtRemark.Value.Trim();
            string tvdId = hfTvdID.Value.Trim();
            string installDate = DateTime.Now.ToString("yyyy-MM-dd");
            string installDateFromJob = ResolveInstallDateFromJob();
            string serverId = GetSelectedServerId();
            string sErr = "";
            bool ok = false;

            var executor = new StoredProcedureTransactionExecutor();
            using (var connection = new OleDbConnection(conn))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        switch (process.Key)
                        {
                            case "device_maint":
                                var newNoSn = CmbNewNoSN != null ? CmbNewNoSN.SelectedValue.Trim() : "";
                                var userAccessAutoId = GetPostedDeviceUserAccessAutoId();
                                executor.ExecuteInScope(connection, tx, "sp_insert_device_maint_new", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfTdtID.Value, hfNewTdtID.Value, hfDeviceID.Value, hfNewDeviceID.Value,
                                    hfTgtID.Value, techId, execTechId, jobId, installDate, "", "", remark, "", serverId, userId
                                });
                                ExecuteInterfacingUserAccessInScope(connection, tx, executor, newNoSn, userAccessAutoId, serverId, userId);
                                break;

                            case "gsm_maint":
                                executor.ExecuteInScope(connection, tx, "sp_insert_gsm_maint_new", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfNewTgtID.Value, hfGsmID.Value, hfNewGsmID.Value,
                                    techId, execTechId, jobId, installDate, "", remark, "", userId
                                });
                                break;

                            case "gsm_maint_suspend":
                                executor.ExecuteInScope(connection, tx, "sp_insert_gsm_maint_suspend", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfNewTgtID.Value, hfGsmID.Value, hfNewGsmID.Value,
                                    techId, execTechId, jobId, installDate, "", remark, "", userId
                                });
                                break;

                            case "vehicle_maint":
                                executor.ExecuteInScope(connection, tx, "sp_insert_vehicle_maint", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfNewTvaID.Value, hfVehicleID.Value, hfNewVehicleID.Value, hfTdtID.Value,
                                    hfTgtID.Value, techId, execTechId, jobId, installDate, "", remark, "", userId
                                });
                                break;

                            case "customer_maint":
                                executor.ExecuteInScope(connection, tx, "sp_insert_customer_maint", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfTdtID.Value, hfDeviceID.Value, hfNewCustID.Value, hfVehicleID.Value,
                                    hfTgtID.Value, techId, jobId, installDate, remark, serverId, userId
                                });
                                break;

                            case "server_maint":
                                executor.ExecuteInScope(connection, tx, "sp_insert_server_maint", new object[]
                                {
                                    tvdId, hfTvaID.Value, hfTdtID.Value, hfDeviceID.Value, hfTgtID.Value, techId, techId, jobId,
                                    installDate, remark, "", serverId, userId
                                });
                                break;

                            default:
                                ExecuteSimpleMaintInScope(connection, tx, executor, process, tvdId, jobId, techId, execTechId, installDate, installDateFromJob, remark, userId);
                                break;
                        }

                        tx.Commit();
                        ok = true;
                    }
                    catch (Exception ex)
                    {
                        try { tx.Rollback(); } catch { }
                        sErr = BuildExceptionMessage(ex);
                    }
                }
            }

            if (ok)
            {
                div_comment.InnerHtml = BuildAlert("Maintenance " + process.Label + " berhasil diproses.", "success");
                ClearForm();
                BindJobTypeDropdown();
                ApplyJobTypeUi();
            }
            else
            {
                div_comment.InnerHtml = BuildAlert("Proses maintenance gagal" + (string.IsNullOrEmpty(sErr) ? "." : ": " + sErr), "danger");
            }
        }

        private void ExecuteSimpleMaintInScope(
            OleDbConnection connection,
            OleDbTransaction tx,
            StoredProcedureTransactionExecutor executor,
            JobInstallationProcess process,
            string tvdId,
            string jobId,
            string techId,
            string execTechId,
            string installDate,
            string installDateFromJob,
            string remark,
            string userId)
        {
            switch (process.Key)
            {
                case "others_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_others_maint", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, execTechId, jobId, installDate, remark, "", userId
                    });
                    return;
                case "uninstall_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_uninstall_maint_new", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfDeviceID.Value, hfGsmID.Value, hfVehicleID.Value,
                        execTechId, jobId, installDate, remark, "", userId
                    });
                    return;
                case "sb_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_sb_maint", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfGsmID.Value, techId, jobId, installDate, remark, "", userId
                    });
                    return;
                case "ub_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_ub_maint", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfGsmID.Value, techId, jobId, installDate, remark, "", userId
                    });
                    return;
                case "sp_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_sp_maint", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfGsmID.Value, techId, jobId, installDateFromJob, remark, "", userId
                    });
                    return;
                case "re_maint":
                    executor.ExecuteInScope(connection, tx, "sp_insert_re_maint", new object[]
                    {
                        tvdId, hfTvaID.Value, hfTdtID.Value, hfTgtID.Value, hfGsmID.Value, techId, jobId, installDate, remark, "", userId
                    });
                    return;
                default:
                    throw new InvalidOperationException("Unsupported process.");
            }
        }

        private string ResolveInstallDateFromJob()
        {
            var dateFromJob = txtSchDate != null ? (txtSchDate.Value ?? "").Trim() : "";
            if (!string.IsNullOrWhiteSpace(dateFromJob) && dateFromJob != "-" && dateFromJob != "[Select]")
                return dateFromJob;
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        private static void ExecuteInterfacingUserAccessInScope(
            OleDbConnection connection,
            OleDbTransaction tx,
            StoredProcedureTransactionExecutor executor,
            string noSn,
            string autoId,
            string serverId,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(noSn) || string.IsNullOrWhiteSpace(autoId) || string.IsNullOrWhiteSpace(serverId))
                return;

            try
            {
                executor.ExecuteInScope(connection, tx, "sp_insert_interfacing_user_access", new object[]
                {
                    noSn.Trim(), autoId.Trim(), serverId.Trim(), userId.Trim()
                });
            }
            catch
            {
                // Backward-compatibility for older SP signature (without user id).
                executor.ExecuteInScope(connection, tx, "sp_insert_interfacing_user_access", new object[]
                {
                    noSn.Trim(), autoId.Trim(), serverId.Trim()
                });
            }
        }

        #endregion

        #region Helper

        private string GetConn()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        private string GetAccessMenu()
        {
            return Session["ClsTypeAccessMenu"] != null ? Session["ClsTypeAccessMenu"].ToString() : "";
        }

        private bool HasProcessAccess(JobInstallationProcess process)
        {
            if (process == null) return false;
            return GetAccessMenu().ToUpper().Contains(process.MenuCode);
        }

        private static string Esc(string value)
        {
            return (value ?? "").Replace("'", "''");
        }

        private static string BuildExceptionMessage(Exception ex)
        {
            if (ex == null) return "Unknown error.";
            var parts = new List<string>();
            var cur = ex;
            int depth = 0;
            while (cur != null && depth < 5)
            {
                var msg = (cur.Message ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(msg))
                    parts.Add(msg);

                var ole = cur as OleDbException;
                if (ole != null && ole.Errors != null && ole.Errors.Count > 0)
                {
                    for (int i = 0; i < ole.Errors.Count; i++)
                    {
                        var dbMsg = (ole.Errors[i].Message ?? "").Trim();
                        if (!string.IsNullOrWhiteSpace(dbMsg))
                            parts.Add(dbMsg);
                    }
                }

                cur = cur.InnerException;
                depth++;
            }
            return parts.Count > 0 ? string.Join(" | ", parts.Distinct()) : "Terjadi kesalahan saat memproses data.";
        }

        private static string EscapeSql(string value)
        {
            return Esc(value);
        }

        private static void ResetSelect(DropDownList cmb)
        {
            if (cmb == null) return;
            cmb.Items.Clear();
            cmb.Items.Add(new ListItem("[Select]", ""));
        }

        private static void EnsureSelectItem(DropDownList cmb)
        {
            if (cmb == null) return;
            if (cmb.Items.FindByValue("[Select]") == null)
                cmb.Items.Insert(0, new ListItem("[Select]", "[Select]"));
            cmb.SelectedValue = "[Select]";
        }

        private static string BuildAlert(string message, string type)
        {
            string icon = type == "success" ? "check" : type == "warning" ? "warning" : "ban";
            string title = type == "success" ? "Success!" : type == "warning" ? "Warning!" : "Failed!";
            return "<div class='alert alert-" + type + " alert-dismissible'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                   "<h4><i class='icon fa fa-" + icon + "'></i> " + title + "</h4>" + HttpUtility.HtmlEncode(message) + "</div>";
        }

        private static List<SelectListItemDto> QuerySelectItems(string sql, string valueField, string textField)
        {
            var list = new List<SelectListItemDto>();
            var rec = new Recordset();
            rec.Open(sql, GetSessionConn());
            while (!rec.EOF)
            {
                list.Add(new SelectListItemDto
                {
                    Value = rec.Fields(valueField),
                    Text = rec.Fields(textField)
                });
                rec.MoveNext();
            }
            return list;
        }

        private static string GetSessionConn()
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.Session == null || ctx.Session["ClsTypeDBConnStringSQL"] == null)
                return "";
            return ctx.Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        private static string GetSessionAccessMenu()
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.Session == null || ctx.Session["ClsTypeAccessMenu"] == null)
                return "";
            return ctx.Session["ClsTypeAccessMenu"].ToString();
        }

        private static string EnsureWebMethodSessionError()
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.Session == null)
                return "Session tidak tersedia. Silakan login ulang.";
            if (ctx.Session["ClsTypeIsLogin"] == null)
                return "Session login tidak ditemukan. Silakan login ulang.";
            if (ctx.Session["ClsTypeDBConnStringSQL"] == null || string.IsNullOrWhiteSpace(ctx.Session["ClsTypeDBConnStringSQL"].ToString()))
                return "Session koneksi database kosong. Silakan login ulang.";
            var menu = GetSessionAccessMenu();
            if (!JobInstallationProcess.HasHubAccess(menu))
                return "Access denied.";
            return "";
        }

        private static string GetSessionUserTechnicianId()
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.Session == null || ctx.Session["ClsTypeUserTechnicianID"] == null)
                return "";
            return ctx.Session["ClsTypeUserTechnicianID"].ToString().Trim();
        }

        private static string BuildMethodError(string methodName, Exception ex)
        {
            string label;
            switch ((methodName ?? "").Trim())
            {
                case "GetJobMaintList": label = "daftar Job Maintenance"; break;
                case "GetNewDeviceList": label = "data device baru"; break;
                case "GetNewGsmList": label = "data GSM baru"; break;
                case "GetNewCustomerList": label = "data customer baru"; break;
                case "GetNewVehicleList": label = "data kendaraan baru"; break;
                case "GetServerList": label = "data server"; break;
                case "GetInterfacingUserLogin": label = "data user access"; break;
                case "LoadNewItemDetail": label = "detail data terpilih"; break;
                default: label = "data"; break;
            }

            var raw = ex != null ? (ex.Message ?? "").Trim() : "";
            if (raw.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Proses memuat " + label + " melebihi batas waktu. Silakan coba beberapa saat lagi.";

            return "Terjadi kendala saat memuat " + label + ". Silakan muat ulang halaman dan coba kembali.";
        }

        private static JobInstallationProcess ResolveProcess(string categoryKey, string processKey)
        {
            if (!string.IsNullOrEmpty(processKey))
            {
                var p = JobInstallationProcess.Get(processKey);
                if (p != null) return p;
            }
            if (!string.IsNullOrEmpty(categoryKey))
            {
                var p = JobInstallationProcess.Get(categoryKey);
                if (p != null) return p;
            }
            return JobInstallationProcess.Get("others_maint");
        }

        private static JobDetailDto MapRowToJobDetail(Dictionary<string, string> row)
        {
            return new JobDetailDto
            {
                JobID = F(row, "JobID", "JobId"),
                RegDate = F(row, "RegDate", "RegisterDate"),
                SchDate = F(row, "ScheduleDate", "SchDate"),
                PoID = F(row, "PoID"),
                MaintTypeID = F(row, "MaintTypeID"),
                MaintTypeDesc = F(row, "MaintTypeDesc"),
                TechnicianID = F(row, "TechnicianID"),
                TechnicianName = F(row, "TechnicianName", "Name"),
                TechBranchName = F(row, "TechnicianBranchName", "BranchName_Technician"),
                CustomerName = F(row, "CustomerName", "fullname", "FullName"),
                BranchName = F(row, "branchname", "BranchName"),
                MarketingName = F(row, "MarketingName"),
                PoliceNo = F(row, "policeno", "PoliceNo"),
                VehicleDesc = F(row, "VehicleDesc"),
                AssetNo = F(row, "AssetNo"),
                NoSN = F(row, "nosn", "NoSN"),
                DeviceTypeDesc = F(row, "DeviceTypeDesc"),
                WarehouseName = F(row, "WarehouseName"),
                NoGSM = F(row, "msidn", "MSIDN"),
                ProviderName = F(row, "ProviderName"),
                GsmWarehouseName = F(row, "GsmSourceName", "SourceName"),
                ServerName = F(row, "ServerName"),
                ServerID = F(row, "ServerID"),
                TvdID = F(row, "TvdID", "TVDID"),
                TvaID = F(row, "TvaID", "TVAID"),
                TdtID = F(row, "TdtID", "TDTID"),
                TgtID = F(row, "TgtID", "TGTID"),
                DeviceID = F(row, "deviceid", "DeviceID"),
                GsmID = F(row, "gsmid", "GsmID"),
                VehicleID = F(row, "vehicleid", "VehicleID"),
                CustID = F(row, "CustID", "CustomerID")
            };
        }

        private static Dictionary<string, string> ReadRow(Recordset rec)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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

        private static string FirstNonEmpty(params string[] values)
        {
            foreach (var v in values)
            {
                if (!string.IsNullOrWhiteSpace(v)) return v.Trim();
            }
            return "";
        }

        #endregion

        #region WebMethods

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ApiResult GetJobMaintList(string MaintTypeID, string Search)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return new ApiResult { Success = false, Message = sessionErr };

                string conn = GetSessionConn();
                if (string.IsNullOrEmpty(conn))
                    return new ApiResult { Success = false, Message = "Session habis. Silakan login ulang." };

                if (string.IsNullOrWhiteSpace(MaintTypeID))
                    return new ApiResult { Success = false, Message = "Job Maintenance Type belum dipilih." };

                string stErr = "";
                var rec = new Recordset();
                rec.Open(
                    "sp_installation_job_maint_list '" + Esc(MaintTypeID.Trim()) + "','" + Esc(Search ?? "") + "'",
                    conn,
                    ref stErr);

                if (!string.IsNullOrEmpty(stErr))
                    return new ApiResult { Success = false, Message = stErr };

                var list = new List<JobDetailDto>();
                while (!rec.EOF)
                {
                    list.Add(MapRowToJobDetail(ReadRow(rec)));
                    rec.MoveNext();
                }
                return new ApiResult { Success = true, Data = list };
            }
            catch (Exception ex)
            {
                return new ApiResult { Success = false, Message = BuildMethodError("GetJobMaintList", ex) };
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNewDeviceList(string technicianId, string search)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                var techId = GetSessionUserTechnicianId();

                var rec = new Recordset();
                rec.Open("sp_list_device_maint_new_device_search '" + Esc(techId) + "','" + Esc(search ?? "") + "'", GetSessionConn());
                var list = ReadSelectWithExtra(rec, "NoSN", "NoSN", "DeviceTypeDesc");
                if (list == null || list.Count == 0)
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Belum ada stok Serial Number device yang dialokasikan ke akun Anda." });

                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = list });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetNewDeviceList", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNewGsmList(string technicianId, string search)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                var techId = GetSessionUserTechnicianId();

                var rec = new Recordset();
                rec.Open("sp_list_gsm_maint_new_gsm_search '" + Esc(techId) + "','" + Esc(search ?? "") + "'", GetSessionConn());
                var list = ReadSelectWithExtra(rec, "MSIDN", "MSIDN", "ProviderName");
                if (list == null || list.Count == 0)
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Belum ada stok Nomor GSM yang dialokasikan ke akun Anda." });

                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = list });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetNewGsmList", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNewCustomerList(string custId, string search)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                if (string.IsNullOrWhiteSpace(custId))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Data customer belum dapat dimuat karena Job ID belum lengkap. Silakan pilih ulang Job ID." });

                var rec = new Recordset();
                rec.Open("sp_list_customer_maint_new_customer_search '" + Esc(custId) + "','" + Esc(search ?? "") + "'", GetSessionConn());
                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = ReadSelectWithExtra(rec, "CustID", "FullName", "BranchName") });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetNewCustomerList", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetNewVehicleList(string custId, string search)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                if (string.IsNullOrWhiteSpace(custId))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Data kendaraan belum dapat dimuat karena Job ID belum lengkap. Silakan pilih ulang Job ID." });

                var rec = new Recordset();
                rec.Open("sp_list_vehicle_maint_new_vehicle_search '" + Esc(custId) + "','" + Esc(search ?? "") + "'", GetSessionConn());
                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = ReadSelectWithExtra(rec, "PoliceNo", "PoliceNo", "VehicleDesc") });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetNewVehicleList", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetServerList(string custId, string categoryKey, string processKey)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                var process = ResolveProcess(categoryKey, processKey);
                string sp = process.Key == "device_maint" ? "sp_list_device_maint_server"
                    : process.Key == "server_maint" ? "sp_list_server_maint_server"
                    : "sp_list_customer_maint_server";
                var rec = new Recordset();
                rec.Open(sp + " '" + Esc(custId ?? "") + "'", GetSessionConn());
                var list = ReadSelectWithExtra(rec, "ServerID", "ServerName", "ServerName");
                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = list });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetServerList", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetInterfacingUserLogin(string custId, string serverId)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                if (string.IsNullOrWhiteSpace(custId))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Pilih Job ID terlebih dahulu." });
                if (string.IsNullOrWhiteSpace(serverId))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = "Pilih Server Name terlebih dahulu." });

                string stErr = "";
                var rec = new Recordset();
                rec.Open(
                    "sp_installation_job_get_interfacing_user_login '" + Esc(custId.Trim()) + "','" + Esc(serverId.Trim()) + "'",
                    GetSessionConn(),
                    ref stErr);
                if (!string.IsNullOrEmpty(stErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = stErr });

                var rows = new List<Dictionary<string, string>>();
                while (!rec.EOF)
                {
                    rows.Add(ReadRow(rec));
                    rec.MoveNext();
                }
                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = rows });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("GetInterfacingUserLogin", ex) }); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string LoadNewItemDetail(string type, string jsonExtra)
        {
            try
            {
                string sessionErr = EnsureWebMethodSessionError();
                if (!string.IsNullOrEmpty(sessionErr))
                    return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = sessionErr });

                var row = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonExtra ?? "{}");
                object data = null;
                switch (type)
                {
                    case "device":
                        data = new
                        {
                            NoSN = F(row, "NoSN"),
                            DeviceTypeDesc = F(row, "DeviceTypeDesc"),
                            WarehouseName = F(row, "WarehouseName"),
                            TdtID = F(row, "TdtID"),
                            DeviceID = F(row, "DeviceID"),
                            ServerName = F(row, "ServerName")
                        };
                        break;
                    case "gsm":
                        data = new
                        {
                            NoGSM = F(row, "MSIDN"),
                            ProviderName = F(row, "ProviderName"),
                            WarehouseName = F(row, "SourceName", "WarehouseName"),
                            TgtID = F(row, "TgtID"),
                            GsmID = F(row, "GsmID")
                        };
                        break;
                    case "customer":
                        data = new
                        {
                            CustomerName = F(row, "FullName"),
                            BranchName = F(row, "BranchName"),
                            MarketingName = F(row, "MarketingName", "MasterFullName"),
                            CustID = F(row, "CustID")
                        };
                        break;
                    case "vehicle":
                        data = new
                        {
                            PoliceNo = F(row, "PoliceNo"),
                            VehicleDesc = F(row, "VehicleDesc", "VehicleDescription"),
                            AssetNo = F(row, "AssetNo"),
                            TvaID = F(row, "TvaID"),
                            VehicleID = F(row, "VehicleID")
                        };
                        break;
                    case "server":
                        data = new
                        {
                            ServerName = F(row, "ServerName", "ServerID"),
                            ServerID = F(row, "ServerID")
                        };
                        break;
                }
                return JsonConvert.SerializeObject(new ApiResult { Success = true, Data = data });
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new ApiResult { Success = false, Message = BuildMethodError("LoadNewItemDetail", ex) }); }
        }

        #endregion

        private static List<SelectListItemDto> ReadSelectWithExtra(Recordset rec, string valueField, string textField, string textField2)
        {
            var list = new List<SelectListItemDto>();
            while (!rec.EOF)
            {
                var row = ReadRow(rec);
                list.Add(new SelectListItemDto
                {
                    Value = F(row, valueField),
                    Text = F(row, textField) + (string.IsNullOrEmpty(F(row, textField2)) ? "" : " - " + F(row, textField2)),
                    Extra = JsonConvert.SerializeObject(row)
                });
                rec.MoveNext();
            }
            return list;
        }

        public class ApiResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        public class SelectListItemDto
        {
            public string Value { get; set; }
            public string Text { get; set; }
            public string Extra { get; set; }
        }

        public class JobDetailDto
        {
            public string JobID { get; set; }
            public string RegDate { get; set; }
            public string SchDate { get; set; }
            public string PoID { get; set; }
            public string MaintTypeID { get; set; }
            public string MaintTypeDesc { get; set; }
            public string TechnicianID { get; set; }
            public string TechnicianName { get; set; }
            public string TechBranchName { get; set; }
            public string CustomerName { get; set; }
            public string BranchName { get; set; }
            public string MarketingName { get; set; }
            public string PoliceNo { get; set; }
            public string VehicleDesc { get; set; }
            public string AssetNo { get; set; }
            public string NoSN { get; set; }
            public string DeviceTypeDesc { get; set; }
            public string WarehouseName { get; set; }
            public string NoGSM { get; set; }
            public string ProviderName { get; set; }
            public string GsmWarehouseName { get; set; }
            public string ServerName { get; set; }
            public string ServerID { get; set; }
            public string TvdID { get; set; }
            public string TvaID { get; set; }
            public string TdtID { get; set; }
            public string TgtID { get; set; }
            public string DeviceID { get; set; }
            public string GsmID { get; set; }
            public string VehicleID { get; set; }
            public string CustID { get; set; }
            public List<SelectListItemDto> Devices { get; set; }
        }
    }
}
