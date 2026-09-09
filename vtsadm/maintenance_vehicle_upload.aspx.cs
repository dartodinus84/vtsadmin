using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class maintenance_vehicle_upload : Page
    {
        private const string SessionKey = "RecCarMasterBulkUpload";

        [Serializable]
        private class ExcelCarRow
        {
            public string NoSN { get; set; }
            public string NewPoliceNo { get; set; }
            public string NewNoAsset { get; set; }
            public string NewVin { get; set; }
        }

        [Serializable]
        private class CarMasterUploadRow
        {
            public int RowNo { get; set; }
            public string Status { get; set; }
            public string StatusCss { get; set; }
            public string ErrorMessage { get; set; }
            public bool CanProcess { get; set; }

            public string NoSN { get; set; }
            public string NewPoliceNo { get; set; }
            public string NewNoAsset { get; set; }
            public string NewVin { get; set; }

            public string FullName { get; set; }
            public string VehicleID { get; set; }
            public string BrandID { get; set; }
            public string ModelID { get; set; }
            public string TypeID { get; set; }
            public string VehicleDesc { get; set; }
            public string PoliceNo { get; set; }
            public string AssetNo { get; set; }
            public string Vin { get; set; }
            public string EngineNumber { get; set; }
            public string BatchNo { get; set; }
            public string VehicleTypeID { get; set; }
            public string ContainerSizeID { get; set; }
            public string IconID { get; set; }

            public string DisplayVin
            {
                get { return string.IsNullOrWhiteSpace(NewVin) ? (Vin ?? "") : NewVin; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLEDITVEH"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null && clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        ClearPreviewState();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "hideLoaderEnd", "hideLoader();", true);
        }

        protected void CmdAutoUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                HideMessage();

                if (FileUpload1 == null || !FileUpload1.HasFile)
                {
                    ShowError("File tidak ditemukan. Silakan pilih file Excel.");
                    return;
                }

                ClearPreviewState();

                string ext = Path.GetExtension(FileUpload1.FileName ?? "").ToLowerInvariant();
                if (ext != ".xlsx" && ext != ".xls")
                {
                    ShowError("Hanya file .xlsx dan .xls yang diperbolehkan.");
                    return;
                }

                List<ExcelCarRow> excelRows;
                byte[] fileBytes;
                using (MemoryStream buffer = new MemoryStream())
                {
                    FileUpload1.PostedFile.InputStream.Position = 0;
                    FileUpload1.PostedFile.InputStream.CopyTo(buffer);
                    fileBytes = buffer.ToArray();
                }

                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    excelRows = ReadExcelRows(stream, ext);
                }

                if (excelRows == null || excelRows.Count == 0)
                {
                    ShowError("File tidak memiliki data.");
                    return;
                }

                if (excelRows.All(r => string.IsNullOrWhiteSpace(r.NoSN)))
                {
                    ShowError("Unit tidak ada.");
                    return;
                }

                List<string> nosnList = excelRows
                    .Where(r => !string.IsNullOrWhiteSpace(r.NoSN))
                    .Select(r => r.NoSN)
                    .ToList();

                string nosnParam = string.Join(",", nosnList);
                Dictionary<string, DataRow> dbMap = LoadCarMasterFromDb(nosnParam);

                List<CarMasterUploadRow> previewRows = BuildPreviewRows(excelRows, dbMap);
                SavePreviewRows(previewRows);
                BindPreviewGrid(previewRows);
                UpdateSubmitVisibility(previewRows);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                ClearPreviewState();
            }
        }

        protected void CmdSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                HideMessage();
                CmdYesSubmit.Enabled = false;

                List<CarMasterUploadRow> rows = GetPreviewRows();
                if (rows == null || rows.Count == 0)
                {
                    CmdSubmit.Visible = false;
                    return;
                }

                string userId = Session["ClsTypeUserID"].ToString();
                string conn = Session["ClsTypeDBConnStringSQL"].ToString().Trim();
                ExecCommand ec = new ExecCommand();

                foreach (CarMasterUploadRow row in rows)
                {
                    if (!row.CanProcess)
                    {
                        continue;
                    }

                    Int32 intAff = 0;
                    string sErr = "";
                    string strSQL = BuildUpdateSql(row, userId);

                    if (ec.Execute(strSQL, conn, ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            row.Status = "Success";
                            row.StatusCss = "cm-badge-success";
                            row.ErrorMessage = "";
                        }
                        else
                        {
                            row.Status = "Failed";
                            row.StatusCss = "cm-badge-failed";
                            row.ErrorMessage = "Update vehicle has been failed";
                        }
                    }
                    else
                    {
                        row.Status = "Failed";
                        row.StatusCss = "cm-badge-failed";
                        row.ErrorMessage = CleanSqlError(sErr);
                    }

                    row.CanProcess = false;
                }

                SavePreviewRows(rows);
                BindPreviewGrid(rows);
                CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                CmdYesSubmit.Enabled = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            CarMasterUploadRow row = e.Row.DataItem as CarMasterUploadRow;
            if (row == null)
            {
                return;
            }

            e.Row.Cells[1].Text = BuildStatusBadge(row);

            for (int i = 2; i <= 9; i++)
            {
                e.Row.Cells[i].Text = CleanGridText(e.Row.Cells[i].Text);
            }
        }

        private static string CleanGridText(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            return HttpUtility.HtmlDecode(value)
                .Replace("\u00A0", " ")
                .Replace("&nbsp;", " ")
                .Trim();
        }

        private void UpdateSubmitVisibility(List<CarMasterUploadRow> rows)
        {
            CmdSubmit.Visible = rows != null && rows.Any(r => r.CanProcess);
        }

        private void ClearPreviewState()
        {
            Session[SessionKey] = null;
            GridView1.DataSource = null;
            GridView1.DataBind();
            LblPreviewCount.Text = "0 rows";
            PanelGrid.Visible = false;
            PanelEmpty.Visible = true;
            CmdSubmit.Visible = false;
            CmdYesSubmit.Enabled = true;
        }

        private List<CarMasterUploadRow> BuildPreviewRows(List<ExcelCarRow> excelRows, Dictionary<string, DataRow> dbMap)
        {
            List<CarMasterUploadRow> result = new List<CarMasterUploadRow>();
            int rowNo = 1;

            foreach (ExcelCarRow excel in excelRows)
            {
                if (string.IsNullOrWhiteSpace(excel.NoSN))
                {
                    continue;
                }

                CarMasterUploadRow item = new CarMasterUploadRow
                {
                    RowNo = rowNo++,
                    NoSN = excel.NoSN,
                    NewPoliceNo = excel.NewPoliceNo ?? "",
                    NewNoAsset = excel.NewNoAsset ?? "",
                    NewVin = excel.NewVin ?? ""
                };

                DataRow dbRow;
                if (dbMap.TryGetValue(excel.NoSN, out dbRow))
                {
                    item.FullName = GetRowValue(dbRow, "FullName");
                    item.VehicleID = GetRowValue(dbRow, "VehicleID");
                    item.BrandID = GetRowValue(dbRow, "BrandID");
                    item.ModelID = GetRowValue(dbRow, "ModelID");
                    item.TypeID = GetRowValue(dbRow, "TypeID");
                    item.VehicleDesc = GetRowValue(dbRow, "VehicleDesc");
                    item.PoliceNo = GetRowValue(dbRow, "PoliceNo");
                    item.AssetNo = GetRowValue(dbRow, "AssetNo");
                    item.Vin = GetRowValue(dbRow, "Vin");
                    item.EngineNumber = GetRowValue(dbRow, "EngineNumber");
                    item.BatchNo = GetRowValue(dbRow, "BatchNo");
                    item.VehicleTypeID = GetRowValue(dbRow, "VehicleTypeID");
                    item.ContainerSizeID = GetRowValue(dbRow, "ContainerSizeID");
                    item.IconID = GetRowValue(dbRow, "IconID");
                    item.Status = "Waiting for Process";
                    item.StatusCss = "cm-badge-waiting";
                    item.CanProcess = true;
                }
                else
                {
                    item.Status = "NoSN tidak ditemukan";
                    item.StatusCss = "cm-badge-warning";
                    item.CanProcess = false;
                }

                result.Add(item);
            }

            return result;
        }

        private Dictionary<string, DataRow> LoadCarMasterFromDb(string nosnParam)
        {
            Dictionary<string, DataRow> map = new Dictionary<string, DataRow>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(nosnParam))
            {
                return map;
            }

            Recordset rec = new Recordset();
            string strSQL = "crc_sp_list_carmaster '" + SqlQuote(nosnParam) + "'";
            rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

            if (rec.RecData == null || rec.RecData.Tables.Count == 0)
            {
                return map;
            }

            foreach (DataRow dr in rec.RecData.Tables[0].Rows)
            {
                string nosn = GetRowValue(dr, "NoSN");
                if (string.IsNullOrEmpty(nosn) || map.ContainsKey(nosn))
                {
                    continue;
                }
                map[nosn] = dr;
            }

            return map;
        }

        private List<ExcelCarRow> ReadExcelRows(Stream stream, string ext)
        {
            if (ext == ".xlsx")
            {
                return ReadFromXlsx(stream);
            }

            return ReadFromXls(stream);
        }

        private List<ExcelCarRow> ReadFromXlsx(Stream stream)
        {
            List<ExcelCarRow> rows = new List<ExcelCarRow>();
            stream.Position = 0;
            ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
                if (ws == null || ws.Dimension == null)
                {
                    return rows;
                }

                int lastRow = ws.Dimension.End.Row;
                for (int rowIdx = 2; rowIdx <= lastRow; rowIdx++)
                {
                    ExcelCarRow row = new ExcelCarRow
                    {
                        NoSN = GetWorksheetCellText(ws, rowIdx, 1),
                        NewPoliceNo = GetWorksheetCellText(ws, rowIdx, 2),
                        NewNoAsset = GetWorksheetCellText(ws, rowIdx, 3),
                        NewVin = GetWorksheetCellText(ws, rowIdx, 4)
                    };

                    if (IsExcelRowEmpty(row))
                    {
                        continue;
                    }

                    rows.Add(row);
                }
            }

            return rows;
        }

        private List<ExcelCarRow> ReadFromXls(Stream stream)
        {
            List<ExcelCarRow> rows = new List<ExcelCarRow>();
            string tempFile = Path.Combine(Path.GetTempPath(), "carmaster_" + Guid.NewGuid().ToString("N") + ".xls");

            try
            {
                stream.Position = 0;
                using (FileStream fs = File.Create(tempFile))
                {
                    stream.CopyTo(fs);
                }

                string[] connStrings = new[]
                {
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + tempFile + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"",
                    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\""
                };

                OleDbConnection conn = OpenExcelConnection(connStrings);
                using (conn)
                {
                    string sheetName = GetFirstSheetName(conn);
                    using (OleDbCommand cmd = new OleDbCommand("SELECT F1, F2, F3, F4 FROM [" + sheetName + "]", conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        bool isFirstRow = true;
                        while (reader.Read())
                        {
                            if (isFirstRow)
                            {
                                isFirstRow = false;
                                continue;
                            }

                            ExcelCarRow row = new ExcelCarRow
                            {
                                NoSN = GetOleDbValue(reader, 0),
                                NewPoliceNo = GetOleDbValue(reader, 1),
                                NewNoAsset = GetOleDbValue(reader, 2),
                                NewVin = GetOleDbValue(reader, 3)
                            };

                            if (IsExcelRowEmpty(row))
                            {
                                continue;
                            }

                            rows.Add(row);
                        }
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    try { File.Delete(tempFile); } catch { }
                }
            }

            return rows;
        }

        private static OleDbConnection OpenExcelConnection(string[] connStrings)
        {
            Exception lastError = null;
            foreach (string connStr in connStrings)
            {
                OleDbConnection conn = new OleDbConnection(connStr);
                try
                {
                    conn.Open();
                    return conn;
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    conn.Dispose();
                }
            }

            throw new Exception("Cannot read .xls file.", lastError);
        }

        private static string GetFirstSheetName(OleDbConnection conn)
        {
            DataTable schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schema == null || schema.Rows.Count == 0)
            {
                return "Sheet1$";
            }

            string sheet = Convert.ToString(schema.Rows[0]["TABLE_NAME"]);
            return string.IsNullOrEmpty(sheet) ? "Sheet1$" : sheet;
        }

        private static string GetOleDbValue(OleDbDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return "";
            }

            object val = reader.GetValue(index);
            return val == null ? "" : Convert.ToString(val);
        }

        private static string GetWorksheetCellText(ExcelWorksheet ws, int row, int col)
        {
            ExcelRange cell = ws.Cells[row, col];
            string text = cell.Text ?? "";
            if (text == "" && cell.Value != null)
            {
                text = Convert.ToString(cell.Value);
            }
            return text;
        }

        private static bool IsExcelRowEmpty(ExcelCarRow row)
        {
            return string.IsNullOrWhiteSpace(row.NoSN)
                && string.IsNullOrWhiteSpace(row.NewPoliceNo)
                && string.IsNullOrWhiteSpace(row.NewNoAsset)
                && string.IsNullOrWhiteSpace(row.NewVin);
        }

        private static string GetRowValue(DataRow dr, string columnName)
        {
            if (dr == null || !dr.Table.Columns.Contains(columnName) || dr[columnName] == DBNull.Value)
            {
                return "";
            }

            return Convert.ToString(dr[columnName]);
        }

        private string BuildUpdateSql(CarMasterUploadRow row, string userId)
        {
            string vin = string.IsNullOrWhiteSpace(row.NewVin) ? row.Vin : row.NewVin;

            return "sp_update_master_vehicle '" +
                SqlQuote(row.VehicleID) + "','" +
                SqlQuote(row.BrandID) + "','" +
                SqlQuote(row.ModelID) + "','" +
                SqlQuote(row.TypeID) + "','" +
                SqlQuote(row.VehicleDesc) + "','" +
                SqlQuote(row.PoliceNo) + "','" +
                SqlQuote(row.NewPoliceNo) + "','" +
                SqlQuote(row.NewNoAsset) + "','" +
                SqlQuote(vin) + "','" +
                SqlQuote(row.EngineNumber) + "','" +
                SqlQuote(row.BatchNo) + "','" +
                SqlQuote(userId) + "','" +
                SqlQuote(row.VehicleTypeID) + "','" +
                SqlQuote(row.ContainerSizeID) + "','[Select]'";
        }

        private static string SqlQuote(string value)
        {
            return (value ?? "").Replace("'", "''");
        }

        private static string CleanSqlError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return "";
            }

            string cleaned = Regex.Replace(message, @"\[Microsoft\]\[ODBC Driver \d+ for SQL Server\]\[SQL Server\]", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"\[SQL Server\]", "", RegexOptions.IgnoreCase);
            return cleaned.Trim();
        }

        private static string BuildStatusBadge(CarMasterUploadRow row)
        {
            string html = "<span class='cm-badge " + row.StatusCss + "'>" + HttpUtility.HtmlEncode(row.Status) + "</span>";
            if (!string.IsNullOrWhiteSpace(row.ErrorMessage))
            {
                html += "<span class='cm-status-error'>" + HttpUtility.HtmlEncode(row.ErrorMessage) + "</span>";
            }
            return html;
        }

        private void SavePreviewRows(List<CarMasterUploadRow> rows)
        {
            Session[SessionKey] = rows;
        }

        private List<CarMasterUploadRow> GetPreviewRows()
        {
            return Session[SessionKey] as List<CarMasterUploadRow> ?? new List<CarMasterUploadRow>();
        }

        private void BindPreviewGrid(List<CarMasterUploadRow> rows)
        {
            if (rows == null || rows.Count == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                LblPreviewCount.Text = "0 rows";
                PanelGrid.Visible = false;
                PanelEmpty.Visible = true;
                UpdateSubmitVisibility(rows);
                return;
            }

            GridView1.DataSource = rows;
            GridView1.DataBind();
            PanelGrid.Visible = true;
            PanelEmpty.Visible = false;

            LblPreviewCount.Text = rows.Count + " row" + (rows.Count == 1 ? "" : "s");
            UpdateSubmitVisibility(rows);
        }

        private void ShowError(string message)
        {
            lblMsg.Attributes["class"] = "alert alert-danger cm-alert";
            lblMsg.InnerHtml = "<strong>Failed!</strong> " + HttpUtility.HtmlEncode(message);
            lblMsg.Style["display"] = "block";
        }

        private void HideMessage()
        {
            lblMsg.InnerHtml = "";
            lblMsg.Style["display"] = "none";
        }
    }
}
