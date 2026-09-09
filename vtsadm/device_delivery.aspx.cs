using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_delivery : System.Web.UI.Page
    {
        private const string ViewStateDetailTable = "DeliveryDetailTable";
        private const string ViewStateUploadValidation = "DeviceUploadValidation";
        private const string SessionHeaderList = "RecDeliveryHeader";
        private const int MaxDeliveryPhotoBytes = 3 * 1024 * 1024; // 3 MB

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTDLV"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                Page.Form.Enctype = "multipart/form-data"; // required for FileUpload
                if (!IsPostBack)
                {
                    ViewState[ViewStateUploadValidation] = null;
                    if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Clear();
                        Open_GridViewHeader();
                        Open_GridViewDevice(); // Pre-load device list so it shows when modal opens
                        div_comment.InnerHtml = "";
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
                else if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    // Rebind device grid on every postback so modal rows have data-device-id when in edit mode
                    Open_GridViewDevice();
                }
                // Content is inside master UpdatePanel: must force full postback or <input type="file"> is never posted (HasFile stays false).
                ScriptManager sm = ScriptManager.GetCurrent(Page);
                if (sm != null)
                {
                    if (CmdYesSubmit != null) sm.RegisterPostBackControl(CmdYesSubmit);
                    if (CmdUploadDevice != null) sm.RegisterPostBackControl(CmdUploadDevice);
                    if (CmdSubmitUpload != null) sm.RegisterPostBackControl(CmdSubmitUpload);
                    if (CmdAddSelected != null) sm.RegisterPostBackControl(CmdAddSelected);
                }
                if (CmbExpedisi != null)
                    CmbExpedisi.Attributes["onchange"] = "loadExpedisiServices();";
                if (FileUploadDeliveryPhoto != null)
                    FileUploadDeliveryPhoto.Attributes["onchange"] = "previewDeliveryPhoto(this);";
                if (IsPostBack)
                {
                    RestoreDeliveryPhotoPreviewFromHidden();
                    if (HfDeliveryUrl != null && !string.IsNullOrWhiteSpace(HfDeliveryUrl.Value) &&
                        string.IsNullOrWhiteSpace((txtDeliveryUrl.Text ?? "").Trim()))
                        txtDeliveryUrl.Text = HfDeliveryUrl.Value.Trim();
                }
                if (CmdYesSubmit != null)
                    CmdYesSubmit.OnClientClick = "syncDeliveryUrlHidden(); closeSubmitModalAndBackdrop(); var o=document.getElementById('overlay'); if(o) o.style.display='block'; ";
                if (CmdYesDelete != null)
                {
                    string postBackRef = Page.ClientScript.GetPostBackEventReference(CmdYesDelete, "");
                    CmdYesDelete.OnClientClick = "var o=document.getElementById('overlay'); if(o) o.style.display='block'; " + postBackRef + "; return false;";
                }
            }
            catch (Exception)
            {
                Response.Redirect("login.aspx");
            }
        }

        private void Clear()
        {
            ClsType ClType = new ClsType();
            txtDeliveryID.Text = "";
            txtDeliveryDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ClType.Open_Combos(CmbWarehouseFrom, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_delivery");
            ClType.Open_Combos(CmbWarehouseTo, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_delivery");
            CmbWarehouseFrom.SelectedValue = "[Select]";
            CmbWarehouseTo.SelectedValue = "[Select]";
            ClType.Open_Combos(CmbExpedisi, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_expedisi");
            CmbExpedisi.SelectedValue = "[Select]";
            ClType.Open_Combos(CmbExpedisiCode, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_expedisi_service_by_expedisi");
            CmbExpedisiCode.SelectedValue = "[Select]";
            txtExpedisiPrice.Text = "0";
            txtExpedisiResiNo.Text = "";
            txtRemark.Text = "";
            txtDeliveryUrl.Text = "";
            if (HfDeliveryUrl != null) HfDeliveryUrl.Value = "";
            if (HfDeliveryPhoto != null) HfDeliveryPhoto.Value = "";
            if (ImgDeliveryPhoto != null) { ImgDeliveryPhoto.ImageUrl = ""; ImgDeliveryPhoto.Style["display"] = "none"; }
            txtDeviceSearch.Text = "";
            txtModalRemark.Text = "";
            txtSearch.Text = "";
            LblDeliveryIDDelete.InnerHtml = "";
            txtDeliveryIDDelete.Value = "";
            txtStatusDelete.Value = "";
            CmdSubmit.Text = "Submit";
            InitDetailTable();
            BindDetailGrid();
        }

        private DataTable GetDetailTable()
        {
            if (ViewState[ViewStateDetailTable] == null)
                InitDetailTable();
            return (DataTable)ViewState[ViewStateDetailTable];
        }

        private void InitDetailTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("DeviceID", typeof(string));
            dt.Columns.Add("NoSN", typeof(string));
            dt.Columns.Add("IMEI", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            ViewState[ViewStateDetailTable] = dt;
        }

        private void BindDetailGrid()
        {
            GridViewDetail.DataSource = GetDetailTable();
            GridViewDetail.DataBind();
        }

        protected void CmbExpedisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbExpedisi.SelectedValue == null || CmbExpedisi.SelectedValue.Trim() == "[Select]")
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbExpedisiCode, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_expedisi_service_by_expedisi");
                CmbExpedisiCode.SelectedValue = "[Select]";
                return;
            }
            ClsType ClType2 = new ClsType();
            ClType2.Open_Combos(CmbExpedisiCode, Session["ClsTypeDBConnStringSQL"].ToString(), CmbExpedisi.SelectedValue.Trim(), "sp_list_expedisi_service_by_expedisi");
            CmbExpedisiCode.SelectedValue = "[Select]";
        }

        protected void Open_GridViewHeader()
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_delivery_header '" + (txtSearch.Text ?? "").Trim().Replace("'", "''") + "'";
            ViewState["RecDeliveryHeaderFieldSort"] = "DeliveryID";
            ViewState["RecDeliveryHeaderDirSort"] = "DESC";
            Session[SessionHeaderList] = ClType.Open_GridView(GridViewHeader, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging,
                ViewState["RecDeliveryHeaderFieldSort"].ToString(), ViewState["RecDeliveryHeaderDirSort"].ToString());
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            Clear();
            Open_GridViewHeader();
            div_comment.InnerHtml = "";
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            Open_GridViewHeader();
            div_comment.InnerHtml = "";
        }

        private void Open_GridViewDevice()
        {
            try
            {
                string strSQL = "sp_list_device_available_for_delivery '" + (txtDeviceSearch.Text ?? "").Trim().Replace("'", "''") + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                GridViewDevice.DataSource = (Rec.RecData != null && Rec.RecData.Tables.Count > 0) ? Rec.RecData.Tables[0] : new DataTable();
                GridViewDevice.DataBind();
            }
            catch (Exception ex)
            {
                GridViewDevice.DataSource = new DataTable();
                GridViewDevice.DataBind();
                div_comment.InnerHtml = "<div class='alert alert-danger'>Load devices failed: " + ex.Message + "</div>";
            }
        }

        protected void CmdDeviceSearch_Click(object sender, EventArgs e)
        {
            Open_GridViewDevice();
            div_comment.InnerHtml = "";
            // Reopen modal after async postback so user sees the list
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showDeviceModal", "setTimeout(function() { $('#modal-device').modal('show'); }, 100);", true);
        }

        private void AddDeviceToDetail(string deviceId, string noSn, string imei, string remark)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) return;
            DataTable dt = GetDetailTable();
            foreach (DataRow r in dt.Rows)
                if ((r["DeviceID"] ?? "").ToString().Trim().Equals(deviceId, StringComparison.OrdinalIgnoreCase))
                    return;
            dt.Rows.Add(deviceId.Trim(), noSn ?? "", imei ?? "", remark ?? "", "");
            ViewState[ViewStateDetailTable] = dt;
            BindDetailGrid();
        }

        protected void CmdAddSelected_Click(object sender, EventArgs e)
        {
            string raw = (HfSelectedDevices.Value ?? "").Trim();
            HfSelectedDevices.Value = "";
            string curDeliveryId = (txtDeliveryID.Text ?? "").Trim();
            int added = 0;
            var existingDeviceIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(curDeliveryId))
            {
                LoadDetailFromDb(curDeliveryId);
                DataTable dtCur = GetDetailTable();
                foreach (DataRow dr in dtCur.Rows)
                    existingDeviceIds.Add((dr["DeviceID"] ?? "").ToString().Trim());
            }
            foreach (string part in raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] seg = part.Split(new[] { '|' }, StringSplitOptions.None);
                string deviceId = seg.Length > 0 ? seg[0].Trim() : "";
                string noSn = seg.Length > 1 ? seg[1].Trim() : "";
                string imei = seg.Length > 2 ? seg[2].Trim() : "";
                string remarkRaw = "";
                if (seg.Length > 3)
                {
                    try { remarkRaw = System.Web.HttpUtility.UrlDecode(seg[3] ?? ""); } catch { remarkRaw = seg[3] ?? ""; }
                    remarkRaw = (remarkRaw ?? "").Trim();
                }
                if (string.IsNullOrEmpty(deviceId)) continue;
                if (string.IsNullOrWhiteSpace(remarkRaw))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger'>Each selected device must have a remark (per-row or Remark for all).</div>";
                    ShowMessageModal();
                    return;
                }
                string remarkSql = remarkRaw.Replace("'", "''");
                if (!string.IsNullOrEmpty(curDeliveryId))
                {
                    if (existingDeviceIds.Contains(deviceId)) continue;
                    int intAff = 0;
                    string sErr = "";
                    ExecCommand ec = new ExecCommand();
                    string strSQL = "sp_delivery_add_detail '" + curDeliveryId.Replace("'", "''") + "','" + deviceId.Replace("'", "''") + "','" + remarkSql + "','" + Session["ClsTypeUserID"] + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        added++;
                        existingDeviceIds.Add(deviceId);
                    }
                }
                else
                {
                    if (existingDeviceIds.Contains(deviceId)) continue;
                    AddDeviceToDetail(deviceId, noSn, imei, remarkRaw);
                    added++;
                    existingDeviceIds.Add(deviceId);
                }
            }
            if (added > 0)
            {
                if (!string.IsNullOrEmpty(curDeliveryId)) LoadDetailFromDb(curDeliveryId);
                div_comment.InnerHtml = "<div class='alert alert-success'>" + added + " device(s) added.</div>";
                txtModalRemark.Text = "";
                Open_GridViewDevice();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "clearBackdrop", "if(typeof clearModalBackdrop==='function')clearModalBackdrop();", true);
            }
            else
                div_comment.InnerHtml = "<div class='alert alert-warning'>Select at least one device.</div>";
        }

        protected void GridViewDevice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var chkAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
                if (chkAll != null)
                    chkAll.Attributes["onclick"] = "var gv=document.getElementById('" + GridViewDevice.ClientID + "'); var chks=gv.getElementsByTagName('input'); for(var i=0;i<chks.length;i++) if(chks[i].type=='checkbox') chks[i].checked=this.checked;";
                return;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                var drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    e.Row.Attributes["data-device-id"] = (drv["DeviceID"] ?? "").ToString().Trim();
                    e.Row.Attributes["data-nosn"] = (drv["NoSN"] ?? "").ToString().Trim();
                    e.Row.Attributes["data-imei"] = (drv["IMEI"] ?? "").ToString().Trim();
                }
            }
        }

        private string GetFieldValueCaseInsensitive(Recordset rec, string colName)
        {
            try
            {
                var cols = rec.RecData.Tables[0].Columns;
                foreach (DataColumn c in cols)
                {
                    if (string.Equals(c.ColumnName, colName, StringComparison.OrdinalIgnoreCase))
                        return (rec.Fields(c.ColumnName) ?? "").Trim();
                }
            }
            catch { }
            return "";
        }

        private void LoadDetailFromDb(string deliveryId)
        {
            var dt = new DataTable();
            dt.Columns.Add("DeviceID", typeof(string));
            dt.Columns.Add("NoSN", typeof(string));
            dt.Columns.Add("IMEI", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            Recordset Rec = new Recordset();
            Rec.Open("sp_list_delivery_detail '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            if (Rec.RecordCount() > 0)
            {
                Rec.MoveFirst();
                while (!Rec.EOF)
                {
                    string noSn = GetFieldValueCaseInsensitive(Rec, "NoSN");
                    string imei = GetFieldValueCaseInsensitive(Rec, "IMEI");
                    dt.Rows.Add(Rec.Fields("DeviceID").Trim(), noSn, imei, (Rec.Fields("Remark") ?? "").Trim(), (Rec.Fields("Status") ?? "").Trim());
                    Rec.MoveNext();
                }
            }
            ViewState[ViewStateDetailTable] = dt;
            BindDetailGrid();
        }

        protected void GridViewDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "RemoveDetail") return;
            string deviceId = (e.CommandArgument ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(deviceId)) return;
            string curDeliveryId = (txtDeliveryID.Text ?? "").Trim();

            if (!string.IsNullOrEmpty(curDeliveryId))
            {
                int intAff = 0;
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                string strSQL = "sp_delete_delivery_detail '" + curDeliveryId.Replace("'", "''") + "','" + deviceId.Replace("'", "''") + "','" + Session["ClsTypeUserID"] + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    LoadDetailFromDb(curDeliveryId);
                else
                    div_comment.InnerHtml = "<div class='alert alert-danger'>" + (string.IsNullOrEmpty(sErr) ? "Cannot remove this detail." : sErr.Replace("<", "&lt;").Replace(">", "&gt;")) + "</div>";
            }
            else
            {
                DataTable dt = GetDetailTable();
                DataRow toRemove = null;
                foreach (DataRow dr in dt.Rows)
                {
                    if ((dr["DeviceID"] ?? "").ToString().Trim().Equals(deviceId, StringComparison.OrdinalIgnoreCase))
                    {
                        toRemove = dr;
                        break;
                    }
                }
                if (toRemove != null)
                {
                    dt.Rows.Remove(toRemove);
                    ViewState[ViewStateDetailTable] = dt;
                    BindDetailGrid();
                }
            }
            div_comment.InnerHtml = "";
        }

        protected void GridViewDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var lb = e.Row.FindControl("CmdRemoveDetail") as LinkButton;
            if (lb != null)
            {
                var drv = e.Row.DataItem as System.Data.DataRowView;
                if (drv != null)
                {
                    string status = (drv["Status"] ?? "").ToString().Trim();
                    lb.Visible = (status != "CL");
                }
            }
        }

        protected void GridViewHeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                string deliveryId = GridViewHeader.Rows[iRow].Cells[0].Text.Trim();
                string deliveryDate = GridViewHeader.Rows[iRow].Cells[1].Text.Trim();

                txtDeliveryID.Text = deliveryId;

                int absRowIndex = GridViewHeader.PageIndex * GridViewHeader.PageSize + iRow;
                string dUrl = "";
                string dPh = "";
                string whFromId = GetWarehouseIdByRow(absRowIndex, "WarehouseIDFrom");
                string whToId = GetWarehouseIdByRow(absRowIndex, "WarehouseIDTo");
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbWarehouseFrom, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_delivery");
                ClType.Open_Combos(CmbWarehouseTo, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_delivery");
                try { CmbWarehouseFrom.SelectedValue = whFromId; } catch { }
                try { CmbWarehouseTo.SelectedValue = whToId; } catch { }

                Recordset Rec = new Recordset();
                Rec.Open("sp_list_delivery_header '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    // Force yyyy-MM-dd for HTML5 date input (Recordset may return mm/dd/yy or culture format)
                    string rawDate = Rec.Fields("DeliveryDate").Trim();
                    DateTime d;
                    if (!string.IsNullOrEmpty(rawDate) && DateTime.TryParse(rawDate, out d))
                        txtDeliveryDate.Text = d.ToString("yyyy-MM-dd");
                    string expedisiId = Rec.Fields("ExpedisiID").Trim();
                    ClType.Open_Combos(CmbExpedisi, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_expedisi");
                    try { CmbExpedisi.SelectedValue = expedisiId; } catch { }
                    ClType.Open_Combos(CmbExpedisiCode, Session["ClsTypeDBConnStringSQL"].ToString(), expedisiId, "sp_list_expedisi_service_by_expedisi");
                    try { CmbExpedisiCode.SelectedValue = Rec.Fields("ExpedisiCode").Trim(); } catch { }
                    txtExpedisiPrice.Text = Rec.Fields("ExpedisiPrice").Trim();
                    txtExpedisiResiNo.Text = Rec.Fields("ExpedisiResiNo").Trim();
                    txtRemark.Text = Rec.Fields("Remark").Trim();
                    dUrl = GetFieldValueCaseInsensitive(Rec, "DeliveryUrl");
                    dPh = GetFieldValueCaseInsensitive(Rec, "DeliveryPhoto");
                }
                if (string.IsNullOrWhiteSpace(dUrl))
                    dUrl = GetHeaderFieldByRow(absRowIndex, "DeliveryUrl");
                if (string.IsNullOrWhiteSpace(dPh))
                    dPh = GetHeaderFieldByRow(absRowIndex, "DeliveryPhoto");
                LoadDeliveryUrlAndPhotoFromDb(deliveryId, ref dUrl, ref dPh);
                txtDeliveryUrl.Text = dUrl;
                if (HfDeliveryUrl != null) HfDeliveryUrl.Value = dUrl ?? "";
                BindDeliveryPhotoToForm(dPh);
                LoadDetailFromDb(deliveryId);
                CmdSubmit.Text = "Update";
                div_comment.InnerHtml = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollDeliveryHeader" + DateTime.UtcNow.Ticks,
                    "var el=document.getElementById('" + txtDeliveryUrl.ClientID + "'); if(el){ el.scrollIntoView({behavior:'smooth',block:'center'}); }", true);
            }
        }

        private string GetHeaderFieldByRow(int absoluteRowIndex, string colName, string defaultValue = "")
        {
            if (Session[SessionHeaderList] == null) return defaultValue;
            DataSet ds = (DataSet)Session[SessionHeaderList];
            if (ds == null || ds.Tables.Count == 0 || absoluteRowIndex >= ds.Tables[0].Rows.Count) return defaultValue;
            DataRow row = ds.Tables[0].Rows[absoluteRowIndex];
            foreach (DataColumn c in row.Table.Columns)
            {
                if (string.Equals(c.ColumnName, colName, StringComparison.OrdinalIgnoreCase))
                    return (row[c.ColumnName] ?? "").ToString().Trim();
            }
            return defaultValue;
        }

        private string GetWarehouseIdByRow(int absoluteRowIndex, string colName)
        {
            string v = GetHeaderFieldByRow(absoluteRowIndex, colName, "[Select]");
            return string.IsNullOrEmpty(v) ? "[Select]" : v;
        }

        private void BindDeliveryPhotoToForm(string webPath)
        {
            webPath = NormalizeStoredPhotoWebPath(webPath);
            if (HfDeliveryPhoto != null) HfDeliveryPhoto.Value = webPath;
            if (string.IsNullOrWhiteSpace(webPath))
            {
                if (ImgDeliveryPhoto != null)
                {
                    ImgDeliveryPhoto.ImageUrl = "";
                    ImgDeliveryPhoto.Style["display"] = "none";
                }
                return;
            }
            ShowDeliveryPhotoPreview(webPath);
        }

        /// <summary>Ensure DB/disk paths are stored and resolved as Upload/Delivery/filename.ext</summary>
        private static string NormalizeStoredPhotoWebPath(string storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath)) return "";
            string p = storedPath.Trim().Replace('\\', '/');
            if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return p;
            int idx = p.IndexOf("Upload/Delivery/", StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
                return p.Substring(idx);
            if (p.StartsWith("~/", StringComparison.Ordinal))
                return p.Substring(2);
            p = p.TrimStart('/');
            if (p.IndexOf('/') < 0 && p.IndexOf('.') >= 0)
                return "Upload/Delivery/" + p;
            return p;
        }

        private string ResolveDeliveryPhotoUrl(string storedPath)
        {
            string p = NormalizeStoredPhotoWebPath(storedPath);
            if (string.IsNullOrWhiteSpace(p)) return "";
            if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return p;
            return ResolveUrl("~/" + p);
        }

        private bool DeliveryPhotoFileExistsOnServer(string storedWebPath)
        {
            string rel = NormalizeStoredPhotoWebPath(storedWebPath);
            if (string.IsNullOrWhiteSpace(rel) ||
                rel.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                rel.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return false;
            try
            {
                string physical = Server.MapPath("~/" + rel);
                return File.Exists(physical);
            }
            catch { return false; }
        }

        /// <summary>When DB path is missing, try DeliveryID_*.ext under Upload/Delivery (file saved before insert completed).</summary>
        private string TryFindDeliveryPhotoOnDisk(string deliveryId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId)) return "";
            try
            {
                string folder = Path.Combine(Server.MapPath("~/Upload"), "Delivery");
                if (!Directory.Exists(folder)) return "";
                string prefix = deliveryId.Replace("\\", "").Replace("/", "_") + "_";
                string[] files = Directory.GetFiles(folder, prefix + "*.*");
                if (files == null || files.Length == 0) return "";
                Array.Sort(files, StringComparer.OrdinalIgnoreCase);
                string name = Path.GetFileName(files[files.Length - 1]);
                return "Upload/Delivery/" + name;
            }
            catch { return ""; }
        }

        /// <summary>After insert, rename NEW_*.ext file to {DeliveryID}_*.ext and update DB path.</summary>
        private void TryFinalizeDeliveryPhotoAfterInsert(string connSql, string newDeliveryId, string photoWebPath)
        {
            photoWebPath = NormalizeStoredPhotoWebPath(photoWebPath);
            if (string.IsNullOrWhiteSpace(newDeliveryId) || string.IsNullOrWhiteSpace(photoWebPath)) return;
            if (photoWebPath.IndexOf("/NEW_", StringComparison.OrdinalIgnoreCase) < 0 &&
                photoWebPath.IndexOf("\\NEW_", StringComparison.OrdinalIgnoreCase) < 0)
                return;
            try
            {
                string oldPhysical = Server.MapPath("~/" + photoWebPath);
                if (!File.Exists(oldPhysical)) return;
                string fileName = Path.GetFileName(oldPhysical);
                string ext = Path.GetExtension(fileName);
                string ts = "";
                int us = fileName.LastIndexOf('_');
                if (us >= 0 && us < fileName.Length - 1)
                    ts = fileName.Substring(us + 1);
                if (string.IsNullOrEmpty(ts))
                    ts = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string safeId = newDeliveryId.Replace("\\", "").Replace("/", "_");
                string newFileName = safeId + "_" + ts;
                if (!newFileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    newFileName += ext;
                string folder = Path.GetDirectoryName(oldPhysical);
                string newPhysical = Path.Combine(folder ?? "", newFileName);
                if (File.Exists(newPhysical)) File.Delete(newPhysical);
                File.Move(oldPhysical, newPhysical);
                string newWebPath = "Upload/Delivery/" + newFileName;
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand("UPDATE dbo.trx_delivery_header SET DeliveryPhoto = @Photo WHERE DeliveryID = @Id", conn))
                {
                    cmd.Parameters.Add("@Photo", SqlDbType.NVarChar, 500).Value = newWebPath;
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = newDeliveryId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* non-fatal */ }
        }

        /// <summary>Load DeliveryUrl and DeliveryPhoto from DB (SP + direct SELECT; fills whichever is still empty).</summary>
        private void LoadDeliveryUrlAndPhotoFromDb(string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            if (string.IsNullOrWhiteSpace(deliveryId)) return;
            try
            {
                string connSql = GetSqlClientConnectionString(Session["ClsTypeDBConnStringSQL"].ToString());
                using (var conn = new SqlConnection(connSql))
                {
                    conn.Open();
                    MergeDeliveryAttachmentFromDb(conn, deliveryId, ref deliveryUrl, ref deliveryPhoto);
                }
            }
            catch
            {
                Recordset H = new Recordset();
                H.Open("sp_get_delivery_header '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                if (H.RecordCount() > 0)
                {
                    H.MoveFirst();
                    string u = GetFieldValueCaseInsensitive(H, "DeliveryUrl");
                    string ph = GetFieldValueCaseInsensitive(H, "DeliveryPhoto");
                    if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u;
                    if (!string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph;
                }
            }
            deliveryPhoto = NormalizeStoredPhotoWebPath(deliveryPhoto);
            if (string.IsNullOrWhiteSpace(deliveryPhoto))
            {
                string onDisk = TryFindDeliveryPhotoOnDisk(deliveryId);
                if (!string.IsNullOrWhiteSpace(onDisk))
                    deliveryPhoto = onDisk;
            }
        }

        private static void MergeDeliveryAttachmentFromDb(SqlConnection conn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            string urlSp = "", photoSp = "";
            TryReadDeliveryAttachmentFromSp(conn, deliveryId, ref urlSp, ref photoSp);
            if (!string.IsNullOrWhiteSpace(urlSp)) deliveryUrl = urlSp;
            if (!string.IsNullOrWhiteSpace(photoSp)) deliveryPhoto = photoSp;

            string urlDirect = "", photoDirect = "";
            TryReadDeliveryAttachmentDirect(conn, deliveryId, ref urlDirect, ref photoDirect);
            if (!string.IsNullOrWhiteSpace(urlDirect)) deliveryUrl = urlDirect;
            if (!string.IsNullOrWhiteSpace(photoDirect)) deliveryPhoto = photoDirect;
        }

        private string GetPostedDeliveryUrl()
        {
            string url = (txtDeliveryUrl.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(url) && HfDeliveryUrl != null)
                url = (HfDeliveryUrl.Value ?? "").Trim();
            if (string.IsNullOrWhiteSpace(url) && Request.Form != null)
            {
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key == null) continue;
                    if (key.EndsWith("$txtDeliveryUrl", StringComparison.OrdinalIgnoreCase) ||
                        key.EndsWith("$HfDeliveryUrl", StringComparison.OrdinalIgnoreCase))
                    {
                        string v = (Request.Form[key] ?? "").Trim();
                        if (!string.IsNullOrWhiteSpace(v)) { url = v; break; }
                    }
                }
            }
            return url;
        }

        private static bool TryReadDeliveryAttachmentFromSp(SqlConnection conn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            using (var cmd = new SqlCommand("dbo.sp_get_delivery_header", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DeliveryID", SqlDbType.NVarChar, 50).Value = deliveryId;
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return false;
                    string u = ReadSqlString(r, "DeliveryUrl");
                    string ph = ReadSqlString(r, "DeliveryPhoto");
                    if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u;
                    if (!string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph;
                    return true;
                }
            }
        }

        private static void TryReadDeliveryAttachmentDirect(SqlConnection conn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            const string sql = @"SELECT ISNULL(DeliveryUrl, '') AS DeliveryUrl, ISNULL(DeliveryPhoto, '') AS DeliveryPhoto
FROM dbo.trx_delivery_header WHERE DeliveryID = @DeliveryID";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@DeliveryID", SqlDbType.NVarChar, 50).Value = deliveryId;
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;
                    string u = ReadSqlString(r, "DeliveryUrl");
                    string ph = ReadSqlString(r, "DeliveryPhoto");
                    if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u;
                    if (!string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph;
                }
            }
        }

        private static string ReadSqlString(SqlDataReader r, string colName)
        {
            try
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    if (string.Equals(r.GetName(i), colName, StringComparison.OrdinalIgnoreCase))
                        return r.IsDBNull(i) ? "" : (r.GetValue(i) ?? "").ToString().Trim();
                }
            }
            catch { }
            return "";
        }

        private static string GetDataRowString(DataRowView drv, string colName)
        {
            if (drv?.Row == null) return "";
            try
            {
                foreach (DataColumn c in drv.Row.Table.Columns)
                {
                    if (string.Equals(c.ColumnName, colName, StringComparison.OrdinalIgnoreCase))
                        return (drv.Row[c] ?? "").ToString().Trim();
                }
            }
            catch { }
            return "";
        }

        protected void GridViewHeader_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHeader.PageIndex = e.NewPageIndex;
            Open_GridViewHeader();
        }

        /// <summary>Save new delivery photo to disk; return stored web path, Hf path from DB, INVALID_EXT, or TOO_LARGE.</summary>
        private string GetDeliveryPhotoPathForSave(string curDeliveryIdForFileName)
        {
            if (FileUploadDeliveryPhoto != null && FileUploadDeliveryPhoto.HasFile)
            {
                string ext = Path.GetExtension(FileUploadDeliveryPhoto.FileName ?? "").ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                    return "INVALID_EXT";
                int len = FileUploadDeliveryPhoto.PostedFile != null ? FileUploadDeliveryPhoto.PostedFile.ContentLength : FileUploadDeliveryPhoto.FileBytes.Length;
                if (len > MaxDeliveryPhotoBytes)
                    return "TOO_LARGE";
                string folder = Path.Combine(Server.MapPath("~/Upload"), "Delivery");
                Directory.CreateDirectory(folder);
                string prefix = string.IsNullOrEmpty(curDeliveryIdForFileName) ? "NEW" : curDeliveryIdForFileName.Replace("\\", "").Replace("/", "_");
                string safe = prefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string full = Path.Combine(folder, safe);
                FileUploadDeliveryPhoto.SaveAs(full);
                string webPath = NormalizeStoredPhotoWebPath("Upload/Delivery/" + safe);
                if (HfDeliveryPhoto != null)
                    HfDeliveryPhoto.Value = webPath;
                return webPath;
            }
            return NormalizeStoredPhotoWebPath((HfDeliveryPhoto != null ? HfDeliveryPhoto.Value : "") ?? "");
        }

        private void RestoreDeliveryPhotoPreviewFromHidden()
        {
            string path = (HfDeliveryPhoto != null ? HfDeliveryPhoto.Value : "") ?? "";
            if (!string.IsNullOrWhiteSpace(path))
                ShowDeliveryPhotoPreview(path.Trim());
        }

        private void ShowDeliveryPhotoPreview(string webPath)
        {
            if (ImgDeliveryPhoto != null)
            {
                string imgUrl = ResolveDeliveryPhotoUrl(webPath);
                if (string.IsNullOrWhiteSpace(imgUrl)) return;
                ImgDeliveryPhoto.ImageUrl = imgUrl;
                ImgDeliveryPhoto.Style["display"] = "block";
            }
        }

        private const string InsertDeliveryBatchSql = @"
SET NOCOUNT ON;
DECLARE @NewID NVARCHAR(50) = dbo.getID('DLV');
DECLARE @XmlDoc XML = TRY_CAST(@DetailXml AS XML);

IF LTRIM(RTRIM(ISNULL(@DetailXml, ''))) <> '' AND @XmlDoc IS NOT NULL
BEGIN
    IF EXISTS (
        SELECT 1
        FROM @XmlDoc.nodes('/d/r') n(v)
        CROSS APPLY (SELECT LTRIM(RTRIM(ISNULL(n.v.value('@DeviceID', 'NVARCHAR(50)'), ''))) AS DevId) x
        WHERE x.DevId <> ''
          AND NOT EXISTS (
              SELECT 1 FROM dbo.mst_device m WITH (NOLOCK)
              WHERE m.DeviceID = x.DevId AND ISNULL(m.Status, '') IN ('RG', 'MW')))
    BEGIN
        RAISERROR('All devices in the detail must exist in mst_device with status RG or MW.', 16, 1);
        RETURN;
    END
END

INSERT INTO dbo.trx_delivery_header (
    DeliveryID, DeliveryDate, WarehouseIDFrom, WarehouseIDTo, ExpedisiID, ExpedisiCode,
    ExpedisiPrice, ExpedisiResiNo, Remark, DeliveryUrl, DeliveryPhoto, Status, UsrUpd, DtmUpd)
VALUES (
    @NewID, @DeliveryDate, @WarehouseIDFrom, @WarehouseIDTo, @ExpedisiID, @ExpedisiCode,
    @ExpedisiPrice, @ExpedisiResiNo, @Remark,
    NULLIF(LTRIM(RTRIM(ISNULL(@DeliveryUrl, ''))), ''),
    NULLIF(LTRIM(RTRIM(ISNULL(@DeliveryPhoto, ''))), ''),
    'RG', @UsrUpd, GETDATE());

IF LTRIM(RTRIM(ISNULL(@DetailXml, ''))) <> '' AND @XmlDoc IS NOT NULL
BEGIN
    INSERT INTO dbo.trx_delivery_detail (DeliveryID, DeviceID, Remark, Status, UsrUpd, DtmUpd, IsReceive)
    SELECT @NewID,
           x.DevId,
           ISNULL(n.v.value('@Remark', 'NVARCHAR(500)'), ''),
           'RG',
           @UsrUpd,
           GETDATE(),
           0
    FROM @XmlDoc.nodes('/d/r') n(v)
    CROSS APPLY (SELECT LTRIM(RTRIM(ISNULL(n.v.value('@DeviceID', 'NVARCHAR(50)'), ''))) AS DevId) x
    INNER JOIN dbo.mst_device m WITH (NOLOCK) ON m.DeviceID = x.DevId AND ISNULL(m.Status, '') IN ('RG', 'MW')
    WHERE x.DevId <> '';
END

SELECT @NewID AS DeliveryID;";

        private const string UpdateDeliveryHeaderSql = @"
UPDATE dbo.trx_delivery_header
SET DeliveryDate = @DeliveryDate,
    WarehouseIDFrom = @WarehouseIDFrom,
    WarehouseIDTo = @WarehouseIDTo,
    ExpedisiID = @ExpedisiID,
    ExpedisiCode = @ExpedisiCode,
    ExpedisiPrice = @ExpedisiPrice,
    ExpedisiResiNo = @ExpedisiResiNo,
    Remark = @Remark,
    DeliveryUrl = NULLIF(LTRIM(RTRIM(ISNULL(@DeliveryUrl, ''))), ''),
    DeliveryPhoto = NULLIF(LTRIM(RTRIM(ISNULL(@DeliveryPhoto, ''))), ''),
    UsrUpd = @UsrUpd,
    DtmUpd = GETDATE()
WHERE DeliveryID = @DeliveryID";

        private static void AddDecimalParameter(SqlCommand cmd, string name, decimal value)
        {
            var p = cmd.Parameters.Add(name, SqlDbType.Decimal);
            p.Precision = 18;
            p.Scale = 2;
            p.Value = value;
        }

        private static void BindInsertDeliveryParameters(SqlCommand cmd, DateTime dDate, string whFrom, string whTo,
            string expId, string expCode, decimal price, string resi, string remark, string dUrl, string dPhotoPath,
            string detailXml, string user)
        {
            cmd.Parameters.Add("@DeliveryDate", SqlDbType.Date).Value = dDate.Date;
            cmd.Parameters.Add("@WarehouseIDFrom", SqlDbType.NVarChar, 50).Value = whFrom ?? "";
            cmd.Parameters.Add("@WarehouseIDTo", SqlDbType.NVarChar, 50).Value = whTo ?? "";
            cmd.Parameters.Add("@ExpedisiID", SqlDbType.NVarChar, 50).Value = expId ?? "";
            cmd.Parameters.Add("@ExpedisiCode", SqlDbType.NVarChar, 50).Value = expCode ?? "";
            AddDecimalParameter(cmd, "@ExpedisiPrice", price);
            cmd.Parameters.Add("@ExpedisiResiNo", SqlDbType.NVarChar, 100).Value = resi ?? "";
            cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 500).Value = remark ?? "";
            cmd.Parameters.Add("@DeliveryUrl", SqlDbType.NVarChar, 500).Value = dUrl ?? "";
            cmd.Parameters.Add("@DeliveryPhoto", SqlDbType.NVarChar, 500).Value =
                string.IsNullOrWhiteSpace(dPhotoPath) ? (object)DBNull.Value : dPhotoPath;
            cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = detailXml ?? "";
            cmd.Parameters.Add("@UsrUpd", SqlDbType.NVarChar, 50).Value = user ?? "";
        }

        private static void BindUpdateDeliveryHeaderParameters(SqlCommand cmd, string deliveryId, DateTime dDate,
            string whFrom, string whTo, string expId, string expCode, decimal price, string resi, string remark,
            string dUrl, string dPhotoPath, string user)
        {
            cmd.Parameters.Add("@DeliveryID", SqlDbType.NVarChar, 50).Value = deliveryId ?? "";
            cmd.Parameters.Add("@DeliveryDate", SqlDbType.Date).Value = dDate.Date;
            cmd.Parameters.Add("@WarehouseIDFrom", SqlDbType.NVarChar, 50).Value = whFrom ?? "";
            cmd.Parameters.Add("@WarehouseIDTo", SqlDbType.NVarChar, 50).Value = whTo ?? "";
            cmd.Parameters.Add("@ExpedisiID", SqlDbType.NVarChar, 50).Value = expId ?? "";
            cmd.Parameters.Add("@ExpedisiCode", SqlDbType.NVarChar, 50).Value = expCode ?? "";
            AddDecimalParameter(cmd, "@ExpedisiPrice", price);
            cmd.Parameters.Add("@ExpedisiResiNo", SqlDbType.NVarChar, 100).Value = resi ?? "";
            cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 500).Value = remark ?? "";
            cmd.Parameters.Add("@DeliveryUrl", SqlDbType.NVarChar, 500).Value = dUrl ?? "";
            cmd.Parameters.Add("@DeliveryPhoto", SqlDbType.NVarChar, 500).Value =
                string.IsNullOrWhiteSpace(dPhotoPath) ? (object)DBNull.Value : dPhotoPath;
            cmd.Parameters.Add("@UsrUpd", SqlDbType.NVarChar, 50).Value = user ?? "";
        }
        private static string GetSqlClientConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return connectionString;
            try
            {
                return new SqlConnectionStringBuilder(connectionString.Trim()).ConnectionString;
            }
            catch
            {
                var pairs = new List<string>();
                foreach (var part in connectionString.Split(';'))
                {
                    var p = part.Trim();
                    if (string.IsNullOrEmpty(p)) continue;
                    var eq = p.IndexOf('=');
                    var key = (eq >= 0 ? p.Substring(0, eq).Trim() : p).ToUpperInvariant();
                    if (key == "PROVIDER") continue;
                    pairs.Add(p);
                }
                string rebuilt = string.Join(";", pairs);
                try
                {
                    return new SqlConnectionStringBuilder(rebuilt).ConnectionString;
                }
                catch
                {
                    return rebuilt;
                }
            }
        }

        public static string GetSqlClientConnectionStringForSession(string connectionString)
            => GetSqlClientConnectionString(connectionString);

        public static void LoadAttachmentFromDb(SqlConnection conn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
            => MergeDeliveryAttachmentFromDb(conn, deliveryId, ref deliveryUrl, ref deliveryPhoto);

        private static void TryReadAttachmentDirectOleDb(string dbConn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            if (string.IsNullOrWhiteSpace(dbConn) || string.IsNullOrWhiteSpace(deliveryId)) return;
            string idEsc = deliveryId.Trim().Replace("'", "''");
            try
            {
                using (var conn = new OleDbConnection(dbConn))
                {
                    conn.Open();
                    string sql = "SELECT ISNULL(DeliveryUrl, '') AS DeliveryUrl, ISNULL(DeliveryPhoto, '') AS DeliveryPhoto " +
                                 "FROM dbo.trx_delivery_header WHERE DeliveryID = '" + idEsc + "'";
                    using (var cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read()) return;
                            string u = ReadOleDbString(r, "DeliveryUrl");
                            if (string.IsNullOrEmpty(u) && r.FieldCount > 0 && !r.IsDBNull(0))
                                u = (r.GetValue(0) ?? "").ToString().Trim();
                            string ph = ReadOleDbString(r, "DeliveryPhoto");
                            if (string.IsNullOrEmpty(ph) && r.FieldCount > 1 && !r.IsDBNull(1))
                                ph = (r.GetValue(1) ?? "").ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u.Trim();
                            if (!string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph.Trim();
                        }
                    }
                }
            }
            catch { }
        }

        private static string ReadOleDbString(OleDbDataReader r, string colName)
        {
            try
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    if (string.Equals(r.GetName(i), colName, StringComparison.OrdinalIgnoreCase))
                        return r.IsDBNull(i) ? "" : (r.GetValue(i) ?? "").ToString().Trim();
                }
            }
            catch { }
            return "";
        }

        /// <summary>Load DeliveryUrl and DeliveryPhoto — direct table SELECT first, then SP/SqlClient.</summary>
        public static void LoadAttachmentUsingRecordset(string dbConn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(dbConn)) return;
            deliveryId = deliveryId.Trim();

            // 1) Direct SELECT (same OleDb session as grid) — reads DeliveryUrl even when SP omits that column
            TryReadAttachmentDirectOleDb(dbConn, deliveryId, ref deliveryUrl, ref deliveryPhoto);

            // 2) SP fallback for any field still empty
            if (string.IsNullOrWhiteSpace(deliveryUrl) || string.IsNullOrWhiteSpace(deliveryPhoto))
            {
                string idEsc = deliveryId.Replace("'", "''");
                try
                {
                    Recordset H = new Recordset();
                    H.Open("sp_get_delivery_header '" + idEsc + "'", dbConn);
                    if (H.RecordCount() > 0)
                    {
                        H.MoveFirst();
                        if (string.IsNullOrWhiteSpace(deliveryUrl))
                        {
                            string u = ReadRecordsetField(H, "DeliveryUrl");
                            if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u.Trim();
                        }
                        if (string.IsNullOrWhiteSpace(deliveryPhoto))
                        {
                            string ph = ReadRecordsetField(H, "DeliveryPhoto");
                            if (!string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph.Trim();
                        }
                    }
                }
                catch { }
            }

            // 3) SqlClient fallback
            if (string.IsNullOrWhiteSpace(deliveryUrl) || string.IsNullOrWhiteSpace(deliveryPhoto))
            {
                try
                {
                    string connSql = GetSqlClientConnectionString(dbConn);
                    using (var conn = new SqlConnection(connSql))
                    {
                        conn.Open();
                        if (string.IsNullOrWhiteSpace(deliveryUrl))
                        {
                            string u = "", ph = deliveryPhoto;
                            TryReadDeliveryAttachmentDirect(conn, deliveryId, ref u, ref ph);
                            if (!string.IsNullOrWhiteSpace(u)) deliveryUrl = u;
                            if (string.IsNullOrWhiteSpace(deliveryPhoto) && !string.IsNullOrWhiteSpace(ph)) deliveryPhoto = ph;
                        }
                        else
                        {
                            MergeDeliveryAttachmentFromDb(conn, deliveryId, ref deliveryUrl, ref deliveryPhoto);
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>Load DeliveryUrl and DeliveryPhoto for attachment view (Recordset + SqlClient).</summary>
        public static void LoadDeliveryAttachmentData(string dbConn, string deliveryId, ref string deliveryUrl, ref string deliveryPhoto)
        {
            deliveryUrl = "";
            deliveryPhoto = "";
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(dbConn)) return;
            LoadAttachmentUsingRecordset(dbConn, deliveryId.Trim(), ref deliveryUrl, ref deliveryPhoto);
        }

        public static string NormalizeStoredPhotoWebPathStatic(string storedPath)
            => NormalizeStoredPhotoWebPath(storedPath);

        /// <summary>Reads DeliveryID from insert batch SELECT.</summary>
        private static string ExecuteInsertDeliveryReader(SqlCommand cmd)
        {
            using (var r = cmd.ExecuteReader())
            {
                if (!r.Read()) return "";
                object o = r["DeliveryID"];
                return (o != null && o != DBNull.Value) ? o.ToString().Trim() : "";
            }
        }

        /// <summary>Insert header + detail lines via parameterized SQL (no dependency on sp_insert_delivery signature on server).</summary>
        private bool TryInsertDelivery(string connSql, DateTime dDate, string whFrom, string whTo,
            string expId, string expCode, decimal price, string resi, string remark, string dUrl, string dPhotoPath,
            string detailXml, string user, out string newId, out string errMsg)
        {
            newId = "";
            errMsg = "";
            try
            {
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand(InsertDeliveryBatchSql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;
                    BindInsertDeliveryParameters(cmd, dDate, whFrom, whTo, expId, expCode, price, resi, remark, dUrl, dPhotoPath, detailXml, user);
                    conn.Open();
                    newId = ExecuteInsertDeliveryReader(cmd);
                    if (string.IsNullOrEmpty(newId))
                    {
                        errMsg = "Insert returned no DeliveryID.";
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMsg = FormatDeliverySqlError(ex);
                return false;
            }
        }

        private bool TryUpdateDeliveryHeader(string connSql, string deliveryId, DateTime dDate, string whFrom, string whTo,
            string expId, string expCode, decimal price, string resi, string remark, string dUrl, string dPhotoPath, string user, out string errMsg)
        {
            errMsg = "";
            try
            {
                using (var conn = new SqlConnection(connSql))
                using (var cmd = new SqlCommand(UpdateDeliveryHeaderSql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    BindUpdateDeliveryHeaderParameters(cmd, deliveryId, dDate, whFrom, whTo, expId, expCode, price, resi, remark, dUrl, dPhotoPath, user);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        errMsg = "Delivery not found or not updated.";
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMsg = FormatDeliverySqlError(ex);
                return false;
            }
        }

        private static string FormatDeliverySqlError(Exception ex)
        {
            var sqlEx = ex as SqlException ?? ex.InnerException as SqlException;
            if (sqlEx != null && sqlEx.Message.IndexOf("DeliveryUrl", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Database column DeliveryUrl is missing. Run trx_delivery_header_add_columns.sql on the database.";
            return ex.Message;
        }

        protected void GridViewHeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            string deliveryId = e.Row.Cells[0].Text.Trim();
            string status = e.Row.Cells[7].Text.Trim();
            var drv = e.Row.DataItem as DataRowView;
            var btnAtt = e.Row.FindControl("BtnViewAttachment") as HtmlInputButton;
            if (btnAtt != null)
            {
                string id = GetDataRowString(drv, "DeliveryID");
                if (string.IsNullOrEmpty(id)) id = deliveryId;
                string url = GetDataRowString(drv, "DeliveryUrl");
                string photoUrl = ResolveDeliveryPhotoUrl(GetDataRowString(drv, "DeliveryPhoto"));
                btnAtt.Attributes["data-delivery-id"] = id;
                btnAtt.Attributes["data-delivery-url"] = url;
                btnAtt.Attributes["data-photo-url"] = photoUrl;
            }
            var lb = e.Row.FindControl("CmdDelete") as LinkButton;
            if (lb != null)
                lb.OnClientClick = "confirmDelete('" + deliveryId.Replace("'", "\\'") + "','" + status.Replace("'", "\\'") + "'); return false;";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetDeliveryAttachment(string deliveryId)
        {
            deliveryId = (deliveryId ?? "").Trim();
            if (string.IsNullOrEmpty(deliveryId))
                return new { ok = false };
            var ctx = HttpContext.Current;
            if (ctx?.Session["ClsTypeDBConnStringSQL"] == null)
                return new { ok = false };
            string url = "";
            string photoPath = "";
            string dbConn = ctx.Session["ClsTypeDBConnStringSQL"].ToString();
            LoadDeliveryAttachmentData(dbConn, deliveryId, ref url, ref photoPath);
            photoPath = NormalizeStoredPhotoWebPath(photoPath);
            if (string.IsNullOrWhiteSpace(photoPath))
                photoPath = TryFindDeliveryPhotoOnDiskStatic(deliveryId);
            string photoUrl = "";
            if (!string.IsNullOrWhiteSpace(photoPath))
                photoUrl = ResolveDeliveryPhotoUrlStatic(photoPath);
            return new { ok = true, deliveryId, referenceUrl = (url ?? "").Trim(), photoUrl };
        }

        public static string TryFindDeliveryPhotoOnDiskStatic(string deliveryId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId)) return "";
            var ctx = HttpContext.Current;
            if (ctx == null) return "";
            try
            {
                string folder = Path.Combine(ctx.Server.MapPath("~/Upload"), "Delivery");
                if (!Directory.Exists(folder)) return "";
                string prefix = deliveryId.Replace("\\", "").Replace("/", "_") + "_";
                string[] files = Directory.GetFiles(folder, prefix + "*.*");
                if (files == null || files.Length == 0) return "";
                Array.Sort(files, StringComparer.OrdinalIgnoreCase);
                return "Upload/Delivery/" + Path.GetFileName(files[files.Length - 1]);
            }
            catch { return ""; }
        }

        public static bool DeliveryPhotoFileExistsStatic(string storedWebPath)
        {
            string rel = NormalizeStoredPhotoWebPath(storedWebPath);
            if (string.IsNullOrWhiteSpace(rel) ||
                rel.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                rel.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return false;
            var ctx = HttpContext.Current;
            if (ctx == null) return false;
            try
            {
                if (File.Exists(ctx.Server.MapPath("~/" + rel)))
                    return true;
                string fileName = Path.GetFileName(rel);
                if (!string.IsNullOrEmpty(fileName))
                {
                    string alt = Path.Combine(ctx.Server.MapPath("~/Upload"), "Delivery", fileName);
                    return File.Exists(alt);
                }
            }
            catch { }
            return false;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetExpedisiServices(string expedisiId)
        {
            expedisiId = (expedisiId ?? "").Trim();
            var items = new List<object> { new { value = "[Select]", text = "[Select]" } };
            var ctx = HttpContext.Current;
            if (ctx?.Session["ClsTypeDBConnStringSQL"] == null)
                return new { ok = false, items };
            try
            {
                Recordset Rec = new Recordset();
                string search = expedisiId == "[Select]" ? "" : expedisiId.Replace("'", "''");
                Rec.Open("sp_list_expedisi_service_by_expedisi '" + search + "'", ctx.Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        items.Add(new { value = Rec.Fields(0).Trim(), text = Rec.Fields(1).Trim() });
                        Rec.MoveNext();
                    }
                }
            }
            catch { }
            return new { ok = true, items };
        }

        private static string ReadRecordsetField(Recordset rec, string colName)
        {
            try
            {
                foreach (DataColumn c in rec.RecData.Tables[0].Columns)
                {
                    if (string.Equals(c.ColumnName, colName, StringComparison.OrdinalIgnoreCase))
                        return (rec.Fields(c.ColumnName) ?? "").Trim();
                }
            }
            catch { }
            return "";
        }

        public static string ResolveDeliveryPhotoUrlStatic(string storedPath)
        {
            string p = NormalizeStoredPhotoWebPath(storedPath);
            if (string.IsNullOrWhiteSpace(p)) return "";
            if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return p;
            var ctx = HttpContext.Current;
            if (ctx == null) return "/" + p;
            return VirtualPathUtility.ToAbsolute("~/" + p);
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            string curDeliveryId = (txtDeliveryID.Text ?? "").Trim();
            bool isEdit = !string.IsNullOrEmpty(curDeliveryId);

            // Save posted file on submit and/or reuse path already on record (edit mode).
            string dPhotoPath = GetDeliveryPhotoPathForSave(curDeliveryId);
            if (dPhotoPath == "INVALID_EXT")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Delivery photo: use JPG, PNG, GIF, or WebP.</div>";
                ShowMessageModal();
                return;
            }
            if (dPhotoPath == "TOO_LARGE")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Delivery photo: maximum file size is 3 MB.</div>";
                ShowMessageModal();
                return;
            }
            dPhotoPath = dPhotoPath ?? "";

            DateTime dDate;
            if (!DateTime.TryParse(txtDeliveryDate.Text.Trim(), out dDate))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Invalid delivery date.</div>";
                ShowMessageModal();
                return;
            }
            if (CmbWarehouseFrom.SelectedValue == null || CmbWarehouseFrom.SelectedValue.Trim() == "[Select]")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Select Warehouse From.</div>";
                ShowMessageModal();
                return;
            }
            if (CmbWarehouseTo.SelectedValue == null || CmbWarehouseTo.SelectedValue.Trim() == "[Select]")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Select Warehouse To.</div>";
                ShowMessageModal();
                return;
            }
            if (CmbExpedisi.SelectedValue == null || CmbExpedisi.SelectedValue.Trim() == "[Select]")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Select Expedisi.</div>";
                ShowMessageModal();
                return;
            }
            if (CmbExpedisiCode.SelectedValue == null || CmbExpedisiCode.SelectedValue.Trim() == "[Select]")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Select Expedisi Service / Code.</div>";
                ShowMessageModal();
                return;
            }
            DataTable dtDetailCheck = GetDetailTable();
            int detailCountCheck = 0;
            foreach (DataRow dr in dtDetailCheck.Rows)
            {
                if (!string.IsNullOrEmpty((dr["DeviceID"] ?? "").ToString().Trim()))
                    detailCountCheck++;
            }
            if (!isEdit && detailCountCheck == 0)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Add at least one device to the delivery detail.</div>";
                ShowMessageModal();
                return;
            }
            if (string.IsNullOrWhiteSpace(dPhotoPath))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Delivery photo is required.</div>";
                ShowMessageModal();
                return;
            }
            decimal price = 0;
            if (string.IsNullOrWhiteSpace(txtExpedisiPrice.Text) || !decimal.TryParse(txtExpedisiPrice.Text.Trim(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out price))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Enter Expedisi Price (use 0 if none).</div>";
                ShowMessageModal();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtExpedisiResiNo.Text))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Enter Expedisi Resi No.</div>";
                ShowMessageModal();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRemark.Text))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Enter Remark.</div>";
                ShowMessageModal();
                return;
            }
            string urlRaw = GetPostedDeliveryUrl();
            if (string.IsNullOrWhiteSpace(urlRaw))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Enter Reference URL.</div>";
                ShowMessageModal();
                return;
            }
            string dUrl = urlRaw.Trim();
            if (HfDeliveryUrl != null) HfDeliveryUrl.Value = dUrl;

            string user = Session["ClsTypeUserID"].ToString();
            string whFrom = CmbWarehouseFrom.SelectedValue.Trim();
            string whTo = CmbWarehouseTo.SelectedValue.Trim();
            string expId = CmbExpedisi.SelectedValue.Trim();
            string expCode = CmbExpedisiCode.SelectedValue.Trim();
            string resi = (txtExpedisiResiNo.Text ?? "").Trim();
            string remark = (txtRemark.Text ?? "").Trim();

            string connSql = GetSqlClientConnectionString(Session["ClsTypeDBConnStringSQL"].ToString());

            if (!string.IsNullOrEmpty(curDeliveryId))
            {
                string sErr = "";
                if (TryUpdateDeliveryHeader(connSql, curDeliveryId, dDate, whFrom, whTo, expId, expCode, price, resi, remark, dUrl, dPhotoPath, user, out sErr))
                {
                    div_comment.InnerHtml = "<div class='alert alert-success'>Delivery updated successfully.</div>";
                    Clear();
                    Open_GridViewHeader();
                    ShowMessageModal();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger'>Update failed: " + HttpUtility.HtmlEncode(sErr) + "</div>";
                    ShowMessageModal();
                }
                return;
            }

            DataTable dt = dtDetailCheck;
            var sb = new System.Text.StringBuilder();
            sb.Append("<d>");
            int detailCount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string deviceId = (dr["DeviceID"] ?? "").ToString().Trim();
                if (string.IsNullOrEmpty(deviceId)) continue;
                detailCount++;
                string detailRemark = (dr["Remark"] ?? "").ToString().Trim();
                if (string.IsNullOrWhiteSpace(detailRemark))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger'>Each device line must have a remark.</div>";
                    ShowMessageModal();
                    return;
                }
                detailRemark = detailRemark.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
                sb.Append("<r DeviceID=\"").Append(deviceId.Replace("\"", "&quot;")).Append("\" Remark=\"").Append(detailRemark).Append("\"/>");
            }
            sb.Append("</d>");
            string detailXml = sb.ToString();

            string newId = "";
            string insErr = "";
            if (!TryInsertDelivery(connSql, dDate, whFrom, whTo, expId, expCode, price, resi, remark, dUrl, dPhotoPath, detailXml, user, out newId, out insErr))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Failed to create delivery: " + HttpUtility.HtmlEncode(string.IsNullOrEmpty(insErr) ? "Unknown error." : insErr) + "</div>";
                ShowMessageModal();
                return;
            }
            TryFinalizeDeliveryPhotoAfterInsert(connSql, newId, dPhotoPath);
            div_comment.InnerHtml = "<div class='alert alert-success'>Delivery " + newId + " created with " + detailCount + " detail line(s).</div>";
            Clear();
            Open_GridViewHeader();
            ShowMessageModal();
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            string id = txtDeliveryIDDelete.Value.Trim();
            string status = txtStatusDelete.Value.Trim();
            if (string.IsNullOrEmpty(id)) return;
            if (status == "DE")
            {
                div_comment.InnerHtml = "<div class='alert alert-warning'>Delivery already deleted.</div>";
                ShowMessageModal();
                return;
            }
            int intAff = 0;
            string sErr = "";
            string strSQL = "sp_delete_delivery_header '" + id.Replace("'", "''") + "','" + Session["ClsTypeUserID"] + "'";
            ExecCommand ec = new ExecCommand();
            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
            {
                div_comment.InnerHtml = "<div class='alert alert-success'>Delivery deleted.</div>";
                Clear();
                Open_GridViewHeader();
                ShowMessageModal();
            }
            else
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Delete failed: " + sErr + "</div>";
                ShowMessageModal();
            }
        }

        private void ShowMessageModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg" + DateTime.UtcNow.Ticks, "$('#modal-messagebox').modal('show');", true);
        }

        // --- Upload Device ---
        private class UploadDeviceRow
        {
            public string DeviceID;
            public string Remark;
        }

        private DataTable GetUploadValidationTable()
        {
            if (ViewState[ViewStateUploadValidation] == null)
                return null;
            return (DataTable)ViewState[ViewStateUploadValidation];
        }

        private static string UnquoteCsvField(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim();
            if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
                return value.Substring(1, value.Length - 2).Replace("\"\"", "\"").Trim();
            return value;
        }

        private static void ParseCsvLine(string line, char[] sep, int idxDevice, int idxRemark, out string deviceId, out string remark)
        {
            string[] parts = line.Split(sep, StringSplitOptions.None);
            deviceId = parts.Length > idxDevice ? UnquoteCsvField(parts[idxDevice] ?? "") : "";
            remark = parts.Length > idxRemark ? UnquoteCsvField(parts[idxRemark] ?? "") : "";
        }

        private static void ParseCsvLineSpaceSeparated(string line, out string deviceId, out string remark)
        {
            line = line.Trim();
            int firstSpace = line.IndexOf(' ');
            if (firstSpace <= 0)
            {
                deviceId = UnquoteCsvField(line);
                remark = "";
                return;
            }
            deviceId = UnquoteCsvField(line.Substring(0, firstSpace));
            remark = UnquoteCsvField(line.Substring(firstSpace + 1));
        }

        private List<UploadDeviceRow> ParseDeviceFile(Stream stream, string ext)
        {
            var rows = new List<UploadDeviceRow>();
            stream.Position = 0;
            if (ext == ".xlsx")
            {
                ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");
                using (var package = new ExcelPackage(stream))
                {
                    var ws = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
                    if (ws == null || ws.Dimension == null) return rows;
                    var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    int lastCol = ws.Dimension.End.Column;
                    for (int col = 1; col <= lastCol; col++)
                    {
                        var header = (ws.Cells[1, col].Text ?? "").Trim();
                        if (!string.IsNullOrEmpty(header) && !map.ContainsKey(header))
                            map[header] = col;
                    }
                    int colDeviceId = map.ContainsKey("DeviceID") ? map["DeviceID"] : (map.ContainsKey("Device ID") ? map["Device ID"] : 1);
                    int colRemark = map.ContainsKey("Remark") ? map["Remark"] : 2;
                    int lastRow = ws.Dimension.End.Row;
                    for (int row = 2; row <= lastRow; row++)
                    {
                        string deviceId = (ws.Cells[row, colDeviceId].Text ?? "").Trim();
                        if (string.IsNullOrEmpty(deviceId)) continue;
                        string remark = colRemark > 0 && colRemark <= lastCol ? (ws.Cells[row, colRemark].Text ?? "").Trim() : "";
                        rows.Add(new UploadDeviceRow { DeviceID = deviceId, Remark = remark });
                    }
                }
                return rows;
            }
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                string firstLine = sr.ReadLine();
                if (firstLine == null) return rows;
                firstLine = firstLine.TrimStart('\uFEFF');
                char[] sep = (firstLine.IndexOf(',') >= 0) ? new[] { ',' }
                    : (firstLine.IndexOf('\t') >= 0) ? new[] { '\t' }
                    : (firstLine.IndexOf(';') >= 0) ? new[] { ';' }
                    : null;
                int idxDevice = 0, idxRemark = 1;
                bool useSpaceSep = (sep == null);
                if (!useSpaceSep)
                {
                    string[] firstParts = firstLine.Split(sep, StringSplitOptions.None);
                    string firstCol = UnquoteCsvField(firstParts.Length > 0 ? firstParts[0] : "").ToUpperInvariant();
                    bool hasHeader = (firstCol == "DEVICEID" || firstCol == "DEVICE ID");
                    if (hasHeader)
                    {
                        for (int i = 0; i < firstParts.Length; i++)
                        {
                            string h = UnquoteCsvField(firstParts[i] ?? "").ToUpperInvariant();
                            if (h == "DEVICEID" || h == "DEVICE ID") idxDevice = i;
                            else if (h == "REMARK") idxRemark = i;
                        }
                    }
                    else
                    {
                        string deviceId, remark;
                        ParseCsvLine(firstLine, sep, idxDevice, idxRemark, out deviceId, out remark);
                        if (!string.IsNullOrEmpty(deviceId))
                            rows.Add(new UploadDeviceRow { DeviceID = deviceId, Remark = remark });
                    }
                }
                else
                {
                    string firstToken = firstLine.Trim().Split(new[] { ' ' }, 2)[0];
                    string firstCol = UnquoteCsvField(firstToken).ToUpperInvariant();
                    bool hasHeader = (firstCol == "DEVICEID" || firstCol == "DEVICE ID");
                    if (!hasHeader)
                    {
                        string deviceId, remark;
                        ParseCsvLineSpaceSeparated(firstLine, out deviceId, out remark);
                        if (!string.IsNullOrEmpty(deviceId))
                            rows.Add(new UploadDeviceRow { DeviceID = deviceId, Remark = remark });
                    }
                }

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    line = line.TrimStart('\uFEFF');
                    string deviceId, remark;
                    if (useSpaceSep)
                        ParseCsvLineSpaceSeparated(line, out deviceId, out remark);
                    else
                        ParseCsvLine(line, sep, idxDevice, idxRemark, out deviceId, out remark);
                    if (string.IsNullOrEmpty(deviceId)) continue;
                    rows.Add(new UploadDeviceRow { DeviceID = deviceId, Remark = remark });
                }
            }
            return rows;
        }

        private void ShowUploadModalWithMessage(string errorMsg)
        {
            if (DivUploadError != null)
            {
                DivUploadError.Visible = !string.IsNullOrEmpty(errorMsg);
                DivUploadError.InnerHtml = !string.IsNullOrEmpty(errorMsg) ? HttpUtility.HtmlEncode(errorMsg) : "";
                DivUploadError.Style["display"] = DivUploadError.Visible ? "block" : "none";
            }
            CmdSubmitUpload.Visible = false;
            div_comment.InnerHtml = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showUploadModal" + DateTime.UtcNow.Ticks,
                "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); $('body').css('padding-right',''); setTimeout(function(){ $('#modal-upload-device').modal('show'); }, 100);", true);
        }

        protected void CmdUploadDevice_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            if (DivUploadError != null) { DivUploadError.Visible = false; DivUploadError.InnerHtml = ""; DivUploadError.Style["display"] = "none"; }
            if (FileUploadDevice == null || !FileUploadDevice.HasFile)
            {
                ShowUploadModalWithMessage("Please select an Excel file first.");
                return;
            }
            string ext = Path.GetExtension(FileUploadDevice.FileName ?? "").ToLowerInvariant();
            if (ext != ".xlsx")
            {
                ShowUploadModalWithMessage("Use an Excel file (.xlsx) only.");
                return;
            }
            List<UploadDeviceRow> parsed;
            try
            {
                parsed = ParseDeviceFile(FileUploadDevice.FileContent, ext);
            }
            catch (Exception ex)
            {
                ShowUploadModalWithMessage("Parse error: " + ex.Message);
                return;
            }
            if (parsed.Count == 0)
            {
                ShowUploadModalWithMessage("No DeviceID rows found. Use columns DeviceID, Remark.");
                return;
            }
            // Build XML for SP
            var sb = new StringBuilder();
            sb.Append("<d>");
            foreach (var r in parsed)
            {
                string id = (r.DeviceID ?? "").Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
                sb.Append("<r DeviceID=\"").Append(id).Append("\"/>");
            }
            sb.Append("</d>");
            string xml = sb.ToString().Replace("'", "''");

            Recordset rec = new Recordset();
            try
            {
                rec.Open("sp_validate_devices_for_delivery '" + xml + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            }
            catch (Exception ex)
            {
                ShowUploadModalWithMessage("Validation error: " + ex.Message);
                return;
            }
            var validMap = new Dictionary<string, Tuple<int, string>>(StringComparer.OrdinalIgnoreCase);
            if (rec.RecData != null && rec.RecData.Tables.Count > 0)
            {
                var dt = rec.RecData.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    string deviceId = (dr["DeviceID"] ?? "").ToString().Trim();
                    int isValid = Convert.ToInt32(dr["IsValid"] ?? 0);
                    string reason = (dr["Reason"] ?? "").ToString().Trim();
                    if (!validMap.ContainsKey(deviceId))
                        validMap[deviceId] = Tuple.Create(isValid, reason);
                }
            }

            var resultDt = new DataTable();
            resultDt.Columns.Add("RowNo", typeof(int));
            resultDt.Columns.Add("DeviceID", typeof(string));
            resultDt.Columns.Add("Remark", typeof(string));
            resultDt.Columns.Add("IsValid", typeof(int));
            resultDt.Columns.Add("StatusText", typeof(string));
            resultDt.Columns.Add("Reason", typeof(string));
            int rowNo = 1;
            var seenDeviceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in parsed)
            {
                bool isDuplicate = seenDeviceIds.Contains(r.DeviceID);
                if (!isDuplicate) seenDeviceIds.Add(r.DeviceID);
                int isValid;
                string reason;
                if (isDuplicate)
                {
                    isValid = 0;
                    reason = "Duplicate";
                }
                else
                {
                    var t = validMap.ContainsKey(r.DeviceID) ? validMap[r.DeviceID] : Tuple.Create(0, "NotFound");
                    isValid = t.Item1;
                    reason = t.Item2 == "OK" ? "" : t.Item2;
                    if (isValid == 1 && string.IsNullOrWhiteSpace(r.Remark))
                    {
                        isValid = 0;
                        reason = string.IsNullOrEmpty(reason) ? "EmptyRemark" : reason + ";EmptyRemark";
                    }
                }
                resultDt.Rows.Add(rowNo++, r.DeviceID, r.Remark ?? "", isValid,
                    isValid == 1 ? "OK" : "Error",
                    reason);
            }
            ViewState[ViewStateUploadValidation] = resultDt;
            GridViewUploadValidation.DataSource = resultDt;
            GridViewUploadValidation.DataBind();

            int okCount = 0, errCount = 0;
            foreach (DataRow dr in resultDt.Rows)
            {
                if (Convert.ToInt32(dr["IsValid"]) == 1) okCount++; else errCount++;
            }
            CmdSubmitUpload.Visible = (errCount == 0 && okCount > 0);
            string msg = errCount > 0
                ? string.Format("Valid: {0}, Invalid: {1}. Fix all invalid rows (NotInMstDevice, InvalidStatus, AlreadyInWarehouse, AlreadyInDelivery, Duplicate, EmptyRemark) and re-upload before Submit. No devices will be added until all rows are valid.", okCount, errCount)
                : string.Format("All {0} row(s) are valid. Click Submit Valid Devices to add them.", okCount);
            div_comment.InnerHtml = "<div class='alert alert-info'>" + HttpUtility.HtmlEncode(msg) + "</div>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showUploadModalOK" + DateTime.UtcNow.Ticks,
                "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); $('body').css('padding-right',''); setTimeout(function(){ $('#modal-upload-device').modal('show'); }, 100);", true);
        }

        protected void CmdSubmitUpload_Click(object sender, EventArgs e)
        {
            DataTable dt = GetUploadValidationTable();
            if (dt == null || dt.Rows.Count == 0)
            {
                div_comment.InnerHtml = "<div class='alert alert-warning'>Upload and validate first.</div>";
                return;
            }
            var validRows = new List<DataRow>();
            int invalidCount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["IsValid"]) == 1)
                    validRows.Add(dr);
                else
                    invalidCount++;
            }
            // Do not insert any if there are invalid rows — ask user to fix file first
            if (invalidCount > 0)
            {
                div_comment.InnerHtml = "<div class='alert alert-warning'><strong>No devices were added.</strong> Please fix invalid rows first (NotInMstDevice, InvalidStatus, AlreadyInWarehouse, AlreadyInDelivery) and re-upload the file. All rows must be valid before Submit.</div>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showUploadModal", "setTimeout(function(){ $('#modal-upload-device').modal('show'); }, 100);", true);
                return;
            }
            if (validRows.Count == 0)
            {
                div_comment.InnerHtml = "<div class='alert alert-warning'>No valid devices to add.</div>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showUploadModal", "setTimeout(function(){ $('#modal-upload-device').modal('show'); }, 100);", true);
                return;
            }
            string curDeliveryId = (txtDeliveryID.Text ?? "").Trim();
            var existingDeviceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(curDeliveryId))
            {
                LoadDetailFromDb(curDeliveryId);
                DataTable dtCur = GetDetailTable();
                foreach (DataRow dr in dtCur.Rows)
                    existingDeviceIds.Add((dr["DeviceID"] ?? "").ToString().Trim());
            }
            else
            {
                foreach (DataRow dr in GetDetailTable().Rows)
                    existingDeviceIds.Add((dr["DeviceID"] ?? "").ToString().Trim());
            }
            int added = 0;
            foreach (DataRow dr in validRows)
            {
                string deviceId = (dr["DeviceID"] ?? "").ToString().Trim();
                string remark = (dr["Remark"] ?? "").ToString().Trim().Replace("'", "''");
                if (existingDeviceIds.Contains(deviceId)) continue;
                if (!string.IsNullOrEmpty(curDeliveryId))
                {
                    int intAff = 0;
                    string sErr = "";
                    ExecCommand ec = new ExecCommand();
                    string strSQL = "sp_delivery_add_detail '" + curDeliveryId.Replace("'", "''") + "','" + deviceId.Replace("'", "''") + "','" + remark + "','" + Session["ClsTypeUserID"] + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        added++;
                        existingDeviceIds.Add(deviceId);
                    }
                }
                else
                {
                    AddDeviceToDetail(deviceId, "", "", remark);
                    added++;
                    existingDeviceIds.Add(deviceId);
                }
            }
            if (added > 0)
            {
                if (!string.IsNullOrEmpty(curDeliveryId)) LoadDetailFromDb(curDeliveryId);
                ViewState[ViewStateUploadValidation] = null;
                GridViewUploadValidation.DataSource = null;
                GridViewUploadValidation.DataBind();
                div_comment.InnerHtml = "<div class='alert alert-success'>" + added + " device(s) added.</div>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeUploadModal", "window.skipClearUpload = true; $('#modal-upload-device').modal('hide');", true);
            }
            else
                div_comment.InnerHtml = "<div class='alert alert-warning'>No new devices added (all may already be in detail).</div>";
        }

        protected void CmdClearUpload_Click(object sender, EventArgs e)
        {
            ViewState[ViewStateUploadValidation] = null;
            GridViewUploadValidation.DataSource = null;
            GridViewUploadValidation.DataBind();
            CmdSubmitUpload.Visible = false;
        }

        protected void GridViewUploadValidation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || e.Row.DataItem == null) return;
            var drv = e.Row.DataItem as DataRowView;
            if (drv == null) return;
            int isValid = Convert.ToInt32(drv["IsValid"] ?? 0);
            if (isValid == 1)
                e.Row.Style["background-color"] = "#d4edda"; // light green
            else
                e.Row.Style["background-color"] = "#f8d7da"; // light red
        }
    }
}
