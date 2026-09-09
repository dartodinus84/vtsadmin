using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using Telegram.Bot;
using System.Net;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_deal : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public job_deal()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (scriptManager != null)
                {
                    // FileUpload cannot send a file through the master page's AJAX UpdatePanel.
                    scriptManager.RegisterPostBackControl(CmdImportExcel);
                }
                
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBDEAL"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridViewHeader();
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

            }
        }

        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                txtCustID.Value = "";
                txtCustFullName.Value = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtPICName.Text = "";
                txtPICPhone.Text = "";
                txtJobActivityID.Value = "";
                txtReqDate.Text = "";
                txtPrice.Value = "";
                ClType.Open_Combos(CmbProductID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_mst_product");
                CmbProductID.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), Session["ClsTypeUserID"].ToString(), "sp_list_deal_marketing");
                CmbMarketing.SelectedValue = "[Select]";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtJobActivityIDDelete.Value = "";
                txtStatusDelete.Value = "";

                if (Session["ClsTypeUserMarketingID"].ToString() != "") {
                    CmbMarketing.Enabled = false;
                    CmbMarketing.SelectedValue = Session["ClsTypeUserMarketingID"].ToString();
                }
                Button2.Attributes.Remove("disabled");
                CmdSubmit.Text = "Submit";
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL;

                // Jika user login sebagai marketing tertentu (punya MarketingID di session),
                // tambahkan parameter MarketingID ke stored procedure agar data hanya untuk marketing tersebut.
                if (Session["ClsTypeUserMarketingID"] != null &&
                    !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
                {
                    string marketingId = Session["ClsTypeUserMarketingID"].ToString();
                    strSQL = "sp_list_header_job_deal '" + txtSearch.Value.Trim() + "','" + marketingId + "'";
                }
                else
                {
                    // User non-marketing: pakai parameter search saja (tanpa filter marketing)
                    strSQL = "sp_list_header_job_deal '" + txtSearch.Value.Trim() + "'";
                }
                ViewState["RecCreateJobTrainingHeaderFieldSort"] = "JobActivityID";
                ViewState["RecCreateJobTrainingHeaderDirSort"] = "DESC";
                Session["RecCreateJobTrainingHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdImportExcel_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";

            if (!fuImportExcel.HasFile)
            {
                ShowImportError("Pilih file template Excel terlebih dahulu.");
                return;
            }

            if (!string.Equals(Path.GetExtension(fuImportExcel.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ShowImportError("File import harus berformat .xlsx.");
                return;
            }

            List<ImportJobDealRow> rows;
            List<string> validationErrors;

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    fuImportExcel.FileContent.CopyTo(stream);
                    ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["IMPORT"];
                        if (worksheet == null)
                        {
                            ShowImportError("Sheet IMPORT tidak ditemukan.");
                            return;
                        }

                        rows = ReadImportRows(worksheet, out validationErrors);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowImportError("File Excel tidak dapat dibaca: " + ex.Message);
                return;
            }

            if (validationErrors.Count > 0)
            {
                ShowImportErrors(validationErrors);
                return;
            }

            if (rows.Count == 0)
            {
                ShowImportError("Tidak ada data import pada sheet IMPORT.");
                return;
            }

            ConnectionStringSettings sqlConnectionSettings = ConfigurationManager.ConnectionStrings["VTSADMIN"];
            if (sqlConnectionSettings == null || string.IsNullOrWhiteSpace(sqlConnectionSettings.ConnectionString))
            {
                ShowImportError("Connection string VTSADMIN tidak ditemukan.");
                return;
            }

            string connectionString = sqlConnectionSettings.ConnectionString;
            string userId = Session["ClsTypeUserID"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (ImportJobDealRow row in rows)
                    {
                        ExecuteImportRow(connection, transaction, row, userId);
                    }

                    transaction.Commit();
                    Open_GridViewHeader();
                    ShowImportSuccess(rows.Count + " baris berhasil di-import.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Stored procedure may already have rolled back the outer transaction.
                    }

                    ShowImportError(ex.Message + " Seluruh import telah di-rollback.");
                }
            }
        }

        private static List<ImportJobDealRow> ReadImportRows(ExcelWorksheet worksheet, out List<string> errors)
        {
            errors = new List<string>();
            List<ImportJobDealRow> rows = new List<ImportJobDealRow>();
            Dictionary<string, int> headers = BuildImportHeaderMap(worksheet);
            string[] requiredHeaders =
            {
                "FULLNAME", "MARKETINGID", "PRODUCTID", "ACTIVITYDATE", "FOLLOWUPDATE", "DEALSTATUSID"
            };

            foreach (string requiredHeader in requiredHeaders)
            {
                if (!headers.ContainsKey(requiredHeader))
                {
                    errors.Add("Header " + requiredHeader + " tidak ditemukan pada row 1.");
                }
            }

            if (errors.Count > 0 || worksheet.Dimension == null)
            {
                return rows;
            }

            for (int rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                if (IsImportRowEmpty(worksheet, rowNumber))
                {
                    continue;
                }

                ImportJobDealRow row = new ImportJobDealRow();
                row.RowNumber = rowNumber;
                row.FullName = GetExcelText(worksheet, rowNumber, headers, "FULLNAME");
                row.MarketingID = GetExcelText(worksheet, rowNumber, headers, "MARKETINGID");
                row.DealRemark = GetExcelText(worksheet, rowNumber, headers, "DEALREMARK");
                row.ActivityRemark = GetExcelText(worksheet, rowNumber, headers, "ACTIVITYREMARK");
                row.DealStatusID = GetExcelText(worksheet, rowNumber, headers, "DEALSTATUSID");
                row.ReqDate = GetExcelDate(worksheet, rowNumber, headers, "REQDATE", errors);
                row.ActivityDate = GetExcelDate(worksheet, rowNumber, headers, "ACTIVITYDATE", errors);
                row.FollowUpDate = GetExcelDate(worksheet, rowNumber, headers, "FOLLOWUPDATE", errors);
                row.ProductID = GetExcelInt(worksheet, rowNumber, headers, "PRODUCTID", errors);
                row.Price = GetExcelMoney(worksheet, rowNumber, headers, "PRICE", errors);

                if (string.IsNullOrWhiteSpace(row.FullName))
                {
                    errors.Add("Row " + rowNumber + ", FullName wajib diisi.");
                }

                if (string.IsNullOrWhiteSpace(row.MarketingID) || row.MarketingID.Length != 10)
                {
                    errors.Add("Row " + rowNumber + ", MarketingID wajib diisi dan harus 10 karakter.");
                }

                if (!row.ProductID.HasValue || row.ProductID.Value <= 0)
                {
                    errors.Add("Row " + rowNumber + ", ProductID wajib diisi dengan angka yang valid.");
                }

                if (!row.ActivityDate.HasValue)
                {
                    errors.Add("Row " + rowNumber + ", ActivityDate wajib diisi dengan format yyyy-mm-dd.");
                }

                if (!row.FollowUpDate.HasValue)
                {
                    errors.Add("Row " + rowNumber + ", FollowUpDate wajib diisi dengan format yyyy-mm-dd.");
                }

                if (string.IsNullOrWhiteSpace(row.DealStatusID))
                {
                    errors.Add("Row " + rowNumber + ", DealStatusID wajib diisi.");
                }

                rows.Add(row);
            }

            return rows;
        }

        private static Dictionary<string, int> BuildImportHeaderMap(ExcelWorksheet worksheet)
        {
            Dictionary<string, int> headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (worksheet.Dimension == null)
            {
                return headers;
            }

            for (int column = 1; column <= worksheet.Dimension.End.Column; column++)
            {
                string value = worksheet.Cells[1, column].Text ?? "";
                string key = value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                key = (key ?? "").Trim().ToUpperInvariant();

                if (!string.IsNullOrWhiteSpace(key) && !headers.ContainsKey(key))
                {
                    headers.Add(key, column);
                }
            }

            return headers;
        }

        private static bool IsImportRowEmpty(ExcelWorksheet worksheet, int rowNumber)
        {
            for (int column = 1; column <= worksheet.Dimension.End.Column; column++)
            {
                if (!string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, column].Text))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetExcelText(ExcelWorksheet worksheet, int rowNumber, Dictionary<string, int> headers, string header)
        {
            int column;
            return headers.TryGetValue(header, out column)
                ? (worksheet.Cells[rowNumber, column].Text ?? "").Trim()
                : "";
        }

        private static DateTime? GetExcelDate(ExcelWorksheet worksheet, int rowNumber, Dictionary<string, int> headers, string header, List<string> errors)
        {
            int column;
            if (!headers.TryGetValue(header, out column) || string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, column].Text))
            {
                return null;
            }

            object value = worksheet.Cells[rowNumber, column].Value;
            DateTime parsedDate;
            if (value is DateTime)
            {
                return (DateTime)value;
            }

            if (DateTime.TryParse(worksheet.Cells[rowNumber, column].Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                || DateTime.TryParse(worksheet.Cells[rowNumber, column].Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }

            errors.Add("Row " + rowNumber + ", " + header + " tidak valid. Gunakan format yyyy-mm-dd.");
            return null;
        }

        private static int? GetExcelInt(ExcelWorksheet worksheet, int rowNumber, Dictionary<string, int> headers, string header, List<string> errors)
        {
            string value = GetExcelText(worksheet, rowNumber, headers, header);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            int parsedValue;
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
            {
                return parsedValue;
            }

            errors.Add("Row " + rowNumber + ", " + header + " harus berupa angka.");
            return null;
        }

        private static decimal? GetExcelMoney(ExcelWorksheet worksheet, int rowNumber, Dictionary<string, int> headers, string header, List<string> errors)
        {
            string value = GetExcelText(worksheet, rowNumber, headers, header);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            decimal parsedValue;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedValue)
                || decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out parsedValue))
            {
                return parsedValue;
            }

            errors.Add("Row " + rowNumber + ", Price harus berupa angka.");
            return null;
        }

        private static void ExecuteImportRow(SqlConnection connection, SqlTransaction transaction, ImportJobDealRow row, string userId)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("dbo.sp_import_job_deal_excel", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@FullName", SqlDbType.VarChar, 255).Value = row.FullName;
                    command.Parameters.Add("@ReqDate", SqlDbType.Date).Value = row.ReqDate.HasValue ? (object)row.ReqDate.Value : DBNull.Value;
                    command.Parameters.Add("@MarketingID", SqlDbType.VarChar, 10).Value = row.MarketingID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = row.ProductID.Value;
                    command.Parameters.Add("@Price", SqlDbType.Money).Value = row.Price.HasValue ? (object)row.Price.Value : DBNull.Value;
                    command.Parameters.Add("@DealRemark", SqlDbType.VarChar, 255).Value = row.DealRemark ?? "";
                    command.Parameters.Add("@ActivityDate", SqlDbType.Date).Value = row.ActivityDate.Value;
                    command.Parameters.Add("@FollowUpDate", SqlDbType.Date).Value = row.FollowUpDate.Value;
                    command.Parameters.Add("@DealStatusID", SqlDbType.VarChar, 10).Value = row.DealStatusID;
                    command.Parameters.Add("@ActivityRemark", SqlDbType.VarChar, 255).Value = row.ActivityRemark ?? "";
                    command.Parameters.Add("@Latitude", SqlDbType.VarChar, 50).Value = "";
                    command.Parameters.Add("@Longitude", SqlDbType.VarChar, 50).Value = "";
                    command.Parameters.Add("@AddressLocation", SqlDbType.VarChar, 255).Value = "";
                    command.Parameters.Add("@UsrUpd", SqlDbType.VarChar, 50).Value = userId;
                    command.Parameters.Add("@ExistingDealAction", SqlDbType.VarChar, 10).Value = "ERROR";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Row " + row.RowNumber + ": " + ex.Message);
            }
        }

        private void ShowImportError(string message)
        {
            string html = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> "
                + HttpUtility.HtmlEncode(message) + "</div>";
            div_comment.InnerHtml = html;
            ShowImportModal();
        }

        private void ShowImportErrors(List<string> errors)
        {
            string list = string.Join("", errors.Select(error => "<li>" + HttpUtility.HtmlEncode(error) + "</li>"));
            string html = "<div class='alert alert-danger' role='alert'><strong>Validasi gagal.</strong><ul>"
                + list + "</ul></div>";
            div_comment.InnerHtml = html;
            ShowImportModal();
        }

        private void ShowImportSuccess(string message)
        {
            string html = "<div class='alert alert-success' role='alert'><strong>Success!</strong> "
                + HttpUtility.HtmlEncode(message) + "</div>";
            div_comment.InnerHtml = html;
            ShowImportModal();
        }

        private void ShowImportModal()
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "import-result-modal",
                "$(function () { $('#modal-messagebox').modal('show'); });",
                true
            );
        }

        private class ImportJobDealRow
        {
            public int RowNumber { get; set; }
            public string FullName { get; set; }
            public DateTime? ReqDate { get; set; }
            public string MarketingID { get; set; }
            public int? ProductID { get; set; }
            public decimal? Price { get; set; }
            public string DealRemark { get; set; }
            public DateTime? ActivityDate { get; set; }
            public DateTime? FollowUpDate { get; set; }
            public string DealStatusID { get; set; }
            public string ActivityRemark { get; set; }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";

                string cus = Convert.ToString(txtCustFullName.Value);
                string tanda = Convert.ToString(txtRemark.Text);
                string pic = Convert.ToString(txtPICName.Text);
                string picnumber = Convert.ToString(txtPICPhone.Text);

                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                    {
                        strSQL = "sp_submit_job_deal '" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','', '" + CmbProductID.SelectedItem.Value.Trim() + "','" + txtPrice.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job training has been successfully</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job training has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job customer training has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        strSQL = "sp_update_job_deal '" + txtJobActivityID.Value.Trim() + "','" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','', '" + CmbProductID.SelectedItem.Value.Trim() + "','" + txtPrice.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job training has been successfully</div>";
                                
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job training has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job customer training has been failed (" + sErr + ")</div>";
                        }
                    }
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateJobTrainingHeader"], LblPagingHeader, ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 8; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                    for (int i = 8; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[0].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtJobActivityIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_training '" + txtJobActivityIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job training has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sJobActivityID = "";
                string sReqDate = ""; string sMarketingName = ""; string sProductName = ""; string sRemark = "";
                string sStatus = ""; string sProductID = ""; string sMarketingID = ""; string sPrice = "";

                sJobActivityID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sReqDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sProductName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sMarketingName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sPrice = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sProductID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {

                            txtCustID.Value = sCustID;
                            txtCustFullName.Value = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;
                            txtJobActivityID.Value = ClType.CheckNbsp(sJobActivityID);
                            txtReqDate.Text = sReqDate;
                            txtPrice.Value = sPrice;

                            CmbProductID.SelectedValue = sProductID;

                            if (string.IsNullOrEmpty(sMarketingID))
                            {
                                // Default to first item
                                if (CmbMarketing.Items.Count > 0)
                                    CmbMarketing.SelectedIndex = 0;
                            }
                            else
                            {
                                // Try to find the marketing ID in the dropdown list
                                ListItem marketingItem = CmbMarketing.Items.FindByValue(sMarketingID);
                                if (marketingItem != null)
                                {
                                    CmbMarketing.SelectedValue = sMarketingID;
                                }
                                else
                                {
                                    // If not found, try to find by text
                                    marketingItem = CmbMarketing.Items.FindByText(sMarketingName);
                                    if (marketingItem != null)
                                    {
                                        CmbMarketing.SelectedValue = marketingItem.Value;
                                    }
                                    else if (CmbMarketing.Items.Count > 0)
                                    {
                                        // If still not found, select the first item
                                        CmbMarketing.SelectedIndex = 0;
                                    }
                                }
                            }


                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Button2.Style.Add("disabled", "disabled");

                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreateJobTrainingHeader"], ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateJobTrainingHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreateJobTrainingHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
       
    }
}