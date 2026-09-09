using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_receive : System.Web.UI.Page
    {
        private const string SessionHeaderList = "RecDeliveryHeader";
        private const int MaxReceivePhotoBytes = 3 * 1024 * 1024; // 3 MB

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTRCV"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                Page.Form.Enctype = "multipart/form-data";

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        txtSearch.Text = "";
                        txtReceivedSearch.Text = "";
                        txtNotAllSearch.Text = "";
                        LblModalDelivery.Text = "";
                        Open_GridViewHeader();
                        LoadNotAllReceivedList();
                        LoadReceivedList();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }

                ScriptManager sm = ScriptManager.GetCurrent(Page);
                if (sm != null)
                {
                    if (CmdConfirmReceive != null) sm.RegisterPostBackControl(CmdConfirmReceive);
                    if (CmdYesSubmitHeader != null) sm.RegisterPostBackControl(CmdYesSubmitHeader);
                    if (CmdYesReceive != null) sm.RegisterPostBackControl(CmdYesReceive);
                }
                if (FileUploadReceivePhoto != null)
                    FileUploadReceivePhoto.Attributes["onchange"] = "previewReceivePhoto(this);";
                if (IsPostBack)
                    RestoreReceivePhotoPreviewFromHidden();
            }
            catch (Exception)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void Open_GridViewHeader()
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_delivery_header_for_receive '" + (txtSearch.Text ?? "").Trim().Replace("'", "''") + "'";
            ViewState["RecDeliveryHeaderFieldSort"] = "DeliveryID";
            ViewState["RecDeliveryHeaderDirSort"] = "DESC";
            Session[SessionHeaderList] = ClType.Open_GridView(GridViewHeader, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging,
                ViewState["RecDeliveryHeaderFieldSort"].ToString(), ViewState["RecDeliveryHeaderDirSort"].ToString());
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            Open_GridViewHeader();
            div_comment.InnerHtml = "";
            UpdatePanelListDelivery.Update();
        }

        protected void CmdNotAllSearch_ServerClick(object sender, EventArgs e)
        {
            LoadNotAllReceivedList();
            div_comment.InnerHtml = "";
            UpdatePanelNotAll.Update();
        }

        protected void CmdReceivedSearch_ServerClick(object sender, EventArgs e)
        {
            LoadReceivedList();
            div_comment.InnerHtml = "";
            UpdatePanelListReceive.Update();
        }

        protected void GridViewHeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                string deliveryId = GridViewHeader.Rows[iRow].Cells[0].Text.Trim();
                LblModalDelivery.Text = deliveryId;
                ViewState["SelectedDeliveryID"] = deliveryId;
                LoadDetailForModal(deliveryId);
                div_comment.InnerHtml = "";
                UpdatePanelModal.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "showReceiveModal", "setTimeout(function() { $('#modal-receive').modal('show'); }, 150);", true);
            }
        }

        protected void GridViewNotAll_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                string deliveryId = GridViewNotAllReceived.Rows[iRow].Cells[0].Text.Trim();
                LblModalDelivery.Text = deliveryId;
                ViewState["SelectedDeliveryID"] = deliveryId;
                LoadDetailForModal(deliveryId);
                div_comment.InnerHtml = "";
                UpdatePanelModal.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "showReceiveModal", "setTimeout(function() { $('#modal-receive').modal('show'); }, 150);", true);
            }
        }

        protected void GridViewHeader_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHeader.PageIndex = e.NewPageIndex;
            Open_GridViewHeader();
            UpdatePanelListDelivery.Update();
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

        private void LoadDetailForModal(string deliveryId)
        {
            var dt = new DataTable();
            dt.Columns.Add("DeviceID", typeof(string));
            dt.Columns.Add("NoSN", typeof(string));
            dt.Columns.Add("IMEI", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("IsReceive", typeof(string));
            dt.Columns.Add("IsReceiveStatus", typeof(string));
            dt.Columns.Add("ShowReceiveButton", typeof(bool));

            Recordset Rec = new Recordset();
            Rec.Open("sp_list_delivery_detail '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() > 0)
            {
                Rec.MoveFirst();
                while (!Rec.EOF)
                {
                    string noSn = GetFieldValueCaseInsensitive(Rec, "NoSN");
                    string imei = GetFieldValueCaseInsensitive(Rec, "IMEI");
                    string isReceive = GetFieldValueCaseInsensitive(Rec, "IsReceive");
                    bool isReceived = (isReceive == "1" || isReceive.ToLower() == "true");
                    string isReceiveStatus = isReceived ? "Received" : "Pending";
                    dt.Rows.Add(
                        Rec.Fields("DeviceID").Trim(),
                        noSn,
                        imei,
                        (Rec.Fields("Remark") ?? "").Trim(),
                        isReceive,
                        isReceiveStatus,
                        !isReceived);
                    Rec.MoveNext();
                }
            }

            ViewState["SelectedDeliveryID"] = deliveryId;
            GridViewDetail.DataSource = dt;
            GridViewDetail.DataBind();

            // Build list of not-received items; show Submit Header only when all received
            var notReceived = new System.Collections.Generic.List<string>();
            bool allReceived = true;
            foreach (DataRow r in dt.Rows)
            {
                string isRec = (r["IsReceive"] ?? "").ToString().Trim();
                if (isRec != "1" && isRec.ToLower() != "true")
                {
                    allReceived = false;
                    notReceived.Add((r["DeviceID"] ?? "").ToString().Trim());
                }
            }
            if (notReceived.Count > 0)
            {
                divNotReceived.Visible = true;
                divNotReceived.Style["display"] = "block";
                divNotReceived.InnerHtml = "<strong>Not yet received:</strong> " + string.Join(", ", notReceived);
            }
            else
            {
                divNotReceived.Visible = false;
                divNotReceived.Style["display"] = "none";
            }
            bool showSubmitOnly = allReceived && dt.Rows.Count > 0;
            CmdConfirmReceive.Visible = !showSubmitOnly;
            CmdSubmitHeader.Visible = showSubmitOnly;
            divPendingInfo.Visible = !showSubmitOnly;
            divAllReceivedInfo.Visible = showSubmitOnly;

            LoadHeaderRefForModal(deliveryId);
            ScriptManager.RegisterStartupScript(this, GetType(), "toggleReceivePhotoReq",
                "toggleReceivePhotoRequired(" + (showSubmitOnly ? "true" : "false") + ");", true);
        }

        private void LoadHeaderRefForModal(string deliveryId)
        {
            if (ImgHeaderDeliveryPhoto != null) { ImgHeaderDeliveryPhoto.ImageUrl = ""; ImgHeaderDeliveryPhoto.Style["display"] = "none"; }
            if (LblDeliveryUrl != null) { LblDeliveryUrl.Text = ""; LblDeliveryUrl.Visible = false; }
            if (LblNoDeliveryUrl != null) LblNoDeliveryUrl.Visible = false;

            Recordset H = new Recordset();
            H.Open("sp_get_delivery_header '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            if (H.RecordCount() <= 0) return;
            H.MoveFirst();
            string url = GetFieldValueCaseInsensitive(H, "DeliveryUrl");
            string dPhoto = GetFieldValueCaseInsensitive(H, "DeliveryPhoto");
            string rPhoto = GetFieldValueCaseInsensitive(H, "ReceivePhoto");

            if (!string.IsNullOrWhiteSpace(url))
            {
                if (LblDeliveryUrl != null)
                {
                    LblDeliveryUrl.Text = HttpUtility.HtmlEncode(url.Trim());
                    LblDeliveryUrl.Visible = true;
                }
            }
            else if (LblNoDeliveryUrl != null)
                LblNoDeliveryUrl.Visible = true;

            if (!string.IsNullOrWhiteSpace(dPhoto) && ImgHeaderDeliveryPhoto != null)
            {
                string rel = dPhoto.Trim().TrimStart('/', '\\').Replace('\\', '/');
                ImgHeaderDeliveryPhoto.ImageUrl = ResolveUrl("~/" + rel);
                ImgHeaderDeliveryPhoto.Style["display"] = "block";
            }

            if (!string.IsNullOrWhiteSpace(rPhoto))
            {
                string normalized = NormalizeReceivePhotoPath(rPhoto);
                if (HfReceivePhoto != null) HfReceivePhoto.Value = normalized;
                ShowReceivePhotoPreview(normalized);
            }
            else
            {
                if (HfReceivePhoto != null) HfReceivePhoto.Value = "";
                if (ImgReceivePhoto != null) { ImgReceivePhoto.ImageUrl = ""; ImgReceivePhoto.Style["display"] = "none"; }
            }
        }

        private static string NormalizeReceivePhotoPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return "";
            return path.Trim().TrimStart('/', '\\').Replace('\\', '/');
        }

        private string GetReceivePhotoPathForSave()
        {
            if (FileUploadReceivePhoto != null && FileUploadReceivePhoto.HasFile)
            {
                string ext = Path.GetExtension(FileUploadReceivePhoto.FileName ?? "").ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".webp")
                    return "INVALID_EXT";
                int len = FileUploadReceivePhoto.PostedFile != null ? FileUploadReceivePhoto.PostedFile.ContentLength : FileUploadReceivePhoto.FileBytes.Length;
                if (len > MaxReceivePhotoBytes)
                    return "TOO_LARGE";
                string deliveryId = (ViewState["SelectedDeliveryID"] ?? "").ToString().Trim();
                if (string.IsNullOrEmpty(deliveryId)) return "";
                string folder = Path.Combine(Server.MapPath("~/Upload"), "Receive");
                Directory.CreateDirectory(folder);
                string safe = deliveryId.Replace("\\", "").Replace("/", "_") + "_rcv_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string full = Path.Combine(folder, safe);
                FileUploadReceivePhoto.SaveAs(full);
                string webPath = "Upload/Receive/" + safe;
                if (HfReceivePhoto != null) HfReceivePhoto.Value = webPath;
                return webPath;
            }
            return NormalizeReceivePhotoPath((HfReceivePhoto != null ? HfReceivePhoto.Value : "") ?? "");
        }

        private void RestoreReceivePhotoPreviewFromHidden()
        {
            string path = (HfReceivePhoto != null ? HfReceivePhoto.Value : "") ?? "";
            if (!string.IsNullOrWhiteSpace(path))
                ShowReceivePhotoPreview(path);
        }

        private void ShowReceivePhotoPreview(string webPath)
        {
            if (ImgReceivePhoto == null || string.IsNullOrWhiteSpace(webPath)) return;
            string rel = NormalizeReceivePhotoPath(webPath);
            ImgReceivePhoto.ImageUrl = ResolveUrl("~/" + rel);
            ImgReceivePhoto.Style["display"] = "block";
        }

        private void LoadNotAllReceivedList()
        {
            ClsType ClType = new ClsType();
            string search = (txtNotAllSearch.Text ?? "").Trim().Replace("'", "''");
            string strSQL = "sp_list_delivery_not_all_received '" + search + "'";
            ClType.Open_GridView(GridViewNotAllReceived, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblNotAllPaging);
        }

        private void LoadReceivedList()
        {
            string search = (txtReceivedSearch.Text ?? "").Trim().Replace("'", "''");
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_received_delivery_headers '" + search + "'";
            ClType.Open_GridView(GridViewReceived, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblReceivedPaging);
        }

        private void LoadReceivedDetailForModal(string deliveryId)
        {
            var dt = new DataTable();
            dt.Columns.Add("DeviceID", typeof(string));
            dt.Columns.Add("NoSN", typeof(string));
            dt.Columns.Add("IMEI", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("ReceiveRemark", typeof(string));

            Recordset Rec = new Recordset();
            Rec.Open("sp_list_received_items '" + deliveryId.Replace("'", "''") + "'", Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() > 0)
            {
                Rec.MoveFirst();
                while (!Rec.EOF)
                {
                    string noSn = GetFieldValueCaseInsensitive(Rec, "NoSN");
                    string imei = GetFieldValueCaseInsensitive(Rec, "IMEI");
                    string receiveRemark = GetFieldValueCaseInsensitive(Rec, "ReceiveRemark");
                    dt.Rows.Add(
                        Rec.Fields("DeviceID").Trim(),
                        noSn,
                        imei,
                        (Rec.Fields("Remark") ?? "").Trim(),
                        receiveRemark);
                    Rec.MoveNext();
                }
            }

            LblReceivedDetailDelivery.Text = deliveryId;
            GridViewReceivedDetail.DataSource = dt;
            GridViewReceivedDetail.DataBind();
        }

        protected void GridViewReceived_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                string deliveryId = GridViewReceived.Rows[iRow].Cells[0].Text.Trim();
                LoadReceivedDetailForModal(deliveryId);
                UpdatePanelReceivedDetail.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "showReceivedDetailModal", "setTimeout(function() { $('#modal-received-detail').modal('show'); }, 100);", true);
            }
        }

        protected void CmdSubmitHeader_Click(object sender, EventArgs e)
        {
            string deliveryId = (ViewState["SelectedDeliveryID"] ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(deliveryId))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>No delivery selected.</div>";
                return;
            }
            string photoPath = GetReceivePhotoPathForSave();
            if (photoPath == "INVALID_EXT")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: use JPG, PNG, GIF, or WebP.</div>";
                UpdatePanelModal.Update();
                return;
            }
            if (photoPath == "TOO_LARGE")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: maximum file size is 3 MB.</div>";
                UpdatePanelModal.Update();
                return;
            }
            if (string.IsNullOrEmpty(photoPath))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo is required before Submit Header. Choose a file under Receive photo.</div>";
                UpdatePanelModal.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "showReceiveModalAfterPhotoReq",
                    "setTimeout(function() { $('#modal-receive').modal('show'); }, 50);", true);
                return;
            }
            string photoSql = photoPath.Replace("'", "''");
            int intAff = 0;
            string sErr = "";
            ExecCommand ec = new ExecCommand();
            string receiveRemark = (txtReceiveRemark.Text ?? "").Trim().Replace("'", "''");
            string strSQL = "sp_submit_delivery_header '" + deliveryId.Replace("'", "''") + "','" + Session["ClsTypeUserID"] + "','" + receiveRemark + "','" + photoSql + "'";
            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
            {
                if (HfReceivePhoto != null) HfReceivePhoto.Value = "";
                LoadDetailForModal(deliveryId);
                LoadNotAllReceivedList();
                LoadReceivedList();
                Open_GridViewHeader();
                div_comment.InnerHtml = "<div class='alert alert-success'>Delivery submitted.</div>";
                UpdatePanelModal.Update();
                UpdatePanelNotAll.Update();
                UpdatePanelListReceive.Update();
                UpdatePanelListDelivery.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "clearBackdropAfterSubmit",
                    "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); $('.modal').modal('hide');", true);
            }
            else
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>" + (string.IsNullOrEmpty(sErr) ? "Error." : HttpUtility.HtmlEncode(sErr)) + "</div>";
            }
        }

        protected void CmdYesReceive_Click(object sender, EventArgs e)
        {
            string deviceId = (HfReceiveDeviceID.Value ?? "").Trim();
            if (string.IsNullOrEmpty(deviceId)) return;
            DoReceive(deviceId);
        }

        protected void GridViewDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var chkAll = e.Row.FindControl("ChkSelectAll") as CheckBox;
                if (chkAll != null)
                    chkAll.Attributes["onclick"] = "var gv=document.getElementById('" + GridViewDetail.ClientID + "'); var chks=gv.getElementsByTagName('input'); for(var i=0;i<chks.length;i++) if(chks[i].type=='checkbox' && chks[i].id.indexOf('ChkSelect')>=0 && chks[i].id.indexOf('ChkSelectAll')<0) chks[i].checked=this.checked;";
                return;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                var chk = e.Row.FindControl("ChkSelect") as CheckBox;
                if (chk != null)
                    chk.InputAttributes["data-deviceid"] = (DataBinder.Eval(e.Row.DataItem, "DeviceID") ?? "").ToString().Trim();
            }
        }

        protected void CmdConfirmReceive_Click(object sender, EventArgs e)
        {
            string deliveryId = (ViewState["SelectedDeliveryID"] ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(deliveryId))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>No delivery selected.</div>";
                return;
            }
            var deviceIds = new System.Collections.Generic.List<string>();
            foreach (GridViewRow row in GridViewDetail.Rows)
            {
                var chk = row.FindControl("ChkSelect") as CheckBox;
                if (chk != null && chk.Visible && chk.Checked)
                {
                    string deviceId = row.Cells[1].Text.Trim();
                    if (!string.IsNullOrEmpty(deviceId))
                        deviceIds.Add(deviceId);
                }
            }
            if (deviceIds.Count == 0)
            {
                div_comment.InnerHtml = "<div class='alert alert-warning'>Select at least one device to receive.</div>";
                UpdatePanelModal.Update();
                return;
            }
            string photoPath = GetReceivePhotoPathForSave();
            if (photoPath == "INVALID_EXT")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: use JPG, PNG, GIF, or WebP.</div>";
                UpdatePanelModal.Update();
                return;
            }
            if (photoPath == "TOO_LARGE")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: maximum file size is 3 MB.</div>";
                UpdatePanelModal.Update();
                return;
            }
            string photoSql = string.IsNullOrEmpty(photoPath) ? "" : photoPath.Replace("'", "''");
            int done = 0;
            string receiveRemark = (txtReceiveRemark.Text ?? "").Trim();
            foreach (string deviceId in deviceIds)
            {
                int intAff = 0;
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                string strSQL = "sp_receive_delivery_detail '" + deliveryId.Replace("'", "''") + "','" + deviceId.Replace("'", "''") + "','" + Session["ClsTypeUserID"] + "','" + receiveRemark.Replace("'", "''") + "','" + photoSql + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    done++;
            }
            LoadDetailForModal(deliveryId);
            LoadNotAllReceivedList();
            LoadReceivedList();
            div_comment.InnerHtml = "<div class='alert alert-success'>" + done + " device(s) marked as received.</div>";
            UpdatePanelModal.Update();
            UpdatePanelNotAll.Update();
            UpdatePanelListReceive.Update();
            ScriptManager.RegisterStartupScript(this, GetType(), "clearBackdropAfterReceive",
                "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); setTimeout(function(){ $('#modal-receive').modal('show'); }, 50);", true);
        }

        private void DoReceive(string deviceId)
        {
            string deliveryId = (ViewState["SelectedDeliveryID"] ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(deliveryId))
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>No delivery selected.</div>";
                return;
            }

            string photoPath = GetReceivePhotoPathForSave();
            if (photoPath == "INVALID_EXT")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: use JPG, PNG, GIF, or WebP.</div>";
                return;
            }
            if (photoPath == "TOO_LARGE")
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>Receive photo: maximum file size is 3 MB.</div>";
                return;
            }
            string photoSql = string.IsNullOrEmpty(photoPath) ? "" : photoPath.Replace("'", "''");
            int intAff = 0;
            string sErr = "";
            ExecCommand ec = new ExecCommand();
            string receiveRemark = (txtReceiveRemark.Text ?? "").Trim();
            string strSQL = "sp_receive_delivery_detail '" + deliveryId.Replace("'", "''") + "','" + deviceId.Replace("'", "''") + "','" + Session["ClsTypeUserID"] + "','" + receiveRemark.Replace("'", "''") + "','" + photoSql + "'";

            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
            {
                LoadDetailForModal(deliveryId);
                LoadNotAllReceivedList();
                LoadReceivedList();
                div_comment.InnerHtml = "<div class='alert alert-success'>Device marked as received.</div>";
                UpdatePanelModal.Update();
                UpdatePanelNotAll.Update();
                UpdatePanelListReceive.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "clearBackdropAfterReceive",
                    "var $m=$('#modal-receive'); $('.modal-backdrop').remove(); $('body').removeClass('modal-open'); if($m.length){ $m.modal('show'); }", true);
            }
            else
            {
                div_comment.InnerHtml = "<div class='alert alert-danger'>" + (string.IsNullOrEmpty(sErr) ? "Error updating." : HttpUtility.HtmlEncode(sErr)) + "</div>";
            }
        }
    }
}
