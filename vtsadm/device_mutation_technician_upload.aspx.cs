using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_mutation_technician_upload : System.Web.UI.Page
    {
        private const int HiddenColumnStartIndex = 7;
        private const int HiddenColumnEndIndex = 9;
        private const string SessionGridKey = "RecListDevMutUpload";

        private const string MsgUploadValidated =
            "File berhasil diupload dan divalidasi. Silakan cek preview terlebih dahulu, lalu klik <strong>Submit Request</strong> untuk membuat request mutasi teknisi.";

        private const string MsgRequestCreated =
            "Request mutasi teknisi berhasil dibuat untuk data yang valid.";

        private const string MsgNoValidData =
            "Tidak ada data valid untuk diproses.";

        private const string MsgPreviewInfo =
            "Data dengan status failed tidak akan diproses saat Submit Request.";

        protected void Open_GridView(string strBatchNo)
        {
            try
            {
                ClsType clType = new ClsType();
                string strSQL = "sp_list_trx_device_mutation_technician_upload '" + strBatchNo + "'";
                Session[SessionGridKey] = clType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
                UpdatePreviewState();
            }
            catch (Exception ex)
            {
                ShowAlert("danger", "Gagal memuat preview data (" + ex.Message + ").");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTDEVTECH"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView("");
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
                ShowAlert("danger", "Gagal memuat halaman (" + ex.Message + ").");
            }
        }

        private void clear()
        {
            try
            {
                txtBatchNo.Value = string.Empty;
                txtFileName.Value = string.Empty;
                ClearAlert();
                CmdUpload.Visible = true;
                CmdError.Visible = false;
                CmdSubmitRequest.Visible = false;
                lblPreviewInfo.Visible = false;
                lblSubmitWarning.Visible = false;
                lblSubmitWarning.InnerHtml = string.Empty;
                SetSubmitRequestEnabled(false);
            }
            catch (Exception)
            {
            }
        }

        private void ShowAlert(string alertType, string message)
        {
            lblMsg.Attributes["data-alert"] = alertType;
            if (alertType == "info" || alertType == "warning" || alertType == "success")
            {
                lblMsg.InnerHtml = message;
            }
            else
            {
                lblMsg.InnerHtml = HttpUtility.HtmlEncode(message);
            }
        }

        private void ClearAlert()
        {
            lblMsg.InnerHtml = string.Empty;
            lblMsg.Attributes.Remove("data-alert");
        }

        private void GetUploadStatusCounts(out int successCount, out int failedCount)
        {
            successCount = 0;
            failedCount = 0;

            DataSet ds = Session[SessionGridKey] as DataSet;
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string uploadStatus = Convert.ToString(row["UploadStatus"]).Trim();
                if (uploadStatus.Equals("success", StringComparison.OrdinalIgnoreCase))
                {
                    successCount++;
                }
                else if (uploadStatus.Equals("failed", StringComparison.OrdinalIgnoreCase))
                {
                    failedCount++;
                }
            }
        }

        private void SetSubmitRequestEnabled(bool enabled)
        {
            if (enabled)
            {
                CmdSubmitRequest.Attributes.Remove("disabled");
            }
            else
            {
                CmdSubmitRequest.Attributes["disabled"] = "disabled";
            }
        }

        private void UpdatePreviewState()
        {
            bool hasBatch = !string.IsNullOrWhiteSpace(txtBatchNo.Value);
            CmdSubmitRequest.Visible = hasBatch;
            lblPreviewInfo.Visible = hasBatch;

            if (!hasBatch)
            {
                lblSubmitWarning.Visible = false;
                lblSubmitWarning.InnerHtml = string.Empty;
                SetSubmitRequestEnabled(false);
                return;
            }

            lblPreviewInfo.InnerHtml = MsgPreviewInfo;
            int successCount;
            int failedCount;
            GetUploadStatusCounts(out successCount, out failedCount);

            if (successCount > 0)
            {
                SetSubmitRequestEnabled(true);
                lblSubmitWarning.Visible = false;
                lblSubmitWarning.InnerHtml = string.Empty;
            }
            else
            {
                SetSubmitRequestEnabled(false);
                lblSubmitWarning.Visible = true;
                lblSubmitWarning.InnerHtml = MsgNoValidData;
            }
        }

        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CmdError.Visible = false;
                ClearAlert();

                if (string.IsNullOrWhiteSpace(FileUpload1.FileName))
                {
                    ShowAlert("warning", "Silakan pilih file CSV terlebih dahulu.");
                    return;
                }

                if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) != "CSV")
                {
                    ShowAlert("danger", "Format file tidak valid. Gunakan file CSV.");
                    return;
                }

                ClsType clType = new ClsType();
                string sErr = string.Empty;
                string sBatchNo = string.Empty;
                string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string strFullPath = Server.MapPath("~/Upload/") + strFileName;
                FileUpload1.SaveAs(strFullPath);

                if (clType.ImportCsvDeviceMutation(strFullPath, strFileName, Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString(), ref sBatchNo, ref sErr))
                {
                    txtBatchNo.Value = sBatchNo;
                    txtFileName.Value = strFileName;
                    Open_GridView(sBatchNo);
                    CmdUpload.Visible = false;
                    ShowAlert("info", MsgUploadValidated);
                }
                else
                {
                    ShowAlert("danger", "Upload & Validate gagal (" + sErr + ").");
                }
            }
            catch (Exception ex)
            {
                ShowAlert("danger", "Upload & Validate gagal (" + ex.Message + ").");
            }
        }

        protected void CmdCancel_ServerClick(object sender, EventArgs e)
        {
            Open_GridView("");
            clear();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType clType = new ClsType();
            clType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[SessionGridKey], LblPaging);
            ClearAlert();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                {
                    HideInternalColumns(e.Row);

                    if (e.Row.RowType != DataControlRowType.DataRow)
                    {
                        return;
                    }

                    string uploadStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UploadStatus"));
                    if (uploadStatus.Equals("failed", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Row.CssClass = "upload-row-failed";
                    }

                    if (e.Row.Cells.Count > 6)
                    {
                        ApplyStatusBadge(e.Row.Cells[4], uploadStatus);
                        ApplyStatusBadge(e.Row.Cells[6], Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void HideInternalColumns(GridViewRow row)
        {
            if (row.Cells.Count <= HiddenColumnEndIndex)
            {
                return;
            }

            for (int i = HiddenColumnStartIndex; i <= HiddenColumnEndIndex; i++)
            {
                row.Cells[i].Visible = false;
            }
        }

        private void ApplyStatusBadge(TableCell cell, string statusValue)
        {
            string status = (statusValue ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(status))
            {
                cell.Text = "-";
                return;
            }

            string badgeClass = "neutral";
            string statusLower = status.ToLowerInvariant();

            if (statusLower == "success" || statusLower == "active" || statusLower == "ok")
            {
                badgeClass = "success";
            }
            else if (statusLower == "failed" || statusLower == "fail" || statusLower == "error")
            {
                badgeClass = "failed";
            }
            else if (statusLower == "pending" || statusLower == "warning")
            {
                badgeClass = statusLower;
            }

            cell.Text = string.Format("<span class=\"status-badge {0}\">{1}</span>", badgeClass, HttpUtility.HtmlEncode(status));
        }

        protected void CmdError_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset rec = new Recordset();
                string strFullPath;
                string sMsg = string.Empty;
                string strFileName = txtFileName.Value.Trim();
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                string strSQL = "sp_get_data_device_mutation_upload_failed_update '" + txtBatchNo.Value.Trim() + "'";
                rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(rec, "", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }
                }
                else
                {
                    ShowAlert("warning", "Tidak ada data failed untuk diunduh.");
                }
            }
            catch (Exception ex)
            {
                ShowAlert("danger", "Gagal mengunduh data failed (" + ex.Message + ").");
            }
        }

        private void ShowSubmitRequestResult(string batchNo, string errMsg)
        {
            Recordset rec = new Recordset();
            string strSQL = "sp_get_data_device_mutation_upload_failed_update '" + batchNo + "'";
            rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
            long failedUpdateCount = rec.RecordCount();
            string errDetail = string.IsNullOrEmpty(errMsg) ? string.Empty : " (" + errMsg + ")";

            Open_GridView(batchNo);

            int successCount;
            int failedCount;
            GetUploadStatusCounts(out successCount, out failedCount);

            // SP uses SET NOCOUNT ON, so ExecuteNonQuery returns -1 instead of row count.
            // Determine outcome from refreshed upload rows instead.
            if (successCount > 0)
            {
                if (failedUpdateCount > 0)
                {
                    CmdError.Visible = true;
                    ShowAlert("warning", MsgRequestCreated + " Beberapa baris gagal diproses" + errDetail + ".");
                }
                else
                {
                    clear();
                    Open_GridView("");
                    ShowAlert("success", MsgRequestCreated);
                }
            }
            else
            {
                CmdError.Visible = failedUpdateCount > 0;
                ShowAlert("danger", "Submit Request gagal. Tidak ada data valid yang diproses" + errDetail + ".");
            }
        }

        protected void CmdSubmitRequest_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CmdError.Visible = false;
                ClearAlert();

                if (string.IsNullOrWhiteSpace(txtBatchNo.Value))
                {
                    ShowAlert("warning", "Batch upload tidak ditemukan. Silakan upload file terlebih dahulu.");
                    return;
                }

                int successCount;
                int failedCount;
                GetUploadStatusCounts(out successCount, out failedCount);

                if (successCount == 0)
                {
                    ShowAlert("warning", MsgNoValidData);
                    UpdatePreviewState();
                    return;
                }

                string batchNo = txtBatchNo.Value.Trim();
                string strSQL = "sp_insert_device_mutation_upload_update '" + batchNo + "'";
                ExecCommand ec = new ExecCommand();
                int affectedRows = 0;
                string sErr = string.Empty;

                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref affectedRows, ref sErr))
                {
                    ShowSubmitRequestResult(batchNo, sErr);
                }
                else
                {
                    CmdError.Visible = true;
                    ShowAlert("danger", "Submit Request gagal (" + sErr + ").");
                }
            }
            catch (Exception ex)
            {
                ShowAlert("danger", "Submit Request gagal (" + ex.Message + ").");
            }
        }
    }
}
