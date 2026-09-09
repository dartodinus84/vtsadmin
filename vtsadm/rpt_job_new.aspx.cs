using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class rpt_job_new : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecRptJobCreateFieldSort";
        string sViewStateDirSort = "RecRptJobCreateDirSort";
        string sSessionRecList = "RecRptJobCreate";
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                DateTime dateFrom = DateTime.ParseExact(txtDateFrom.Text.Trim(), "yyyy-MM-dd", culture);
                DateTime dateTo = DateTime.ParseExact(txtDateTo.Text.Trim(), "yyyy-MM-dd", culture);

                string strSQL = "sp_rpt_job_create_group_area '" + txtSearch.Text.Trim() + "','"+ CmbGroupAreaID.SelectedItem.Value.ToString() + "', '"+ CmbAreaID.SelectedItem.Value.ToString() + "', '" + dateFrom.ToString("yyyy-MM-dd", culture) + "','" + dateTo.ToString("yyyy-MM-dd", culture) + "','" + CmbSearchBy.SelectedItem.Value.ToString() + "'";
                ViewState[sViewStateFieldSort] = "JobID";
                ViewState[sViewStateDirSort] = "ASC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNURPTJOBNEW"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportXls);
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView();
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
                ClType.Open_Combos(CmbGroupAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_group_area");
                ClType.Open_Combos(CmbAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_ref_area_by_group_area");
                CmbSearchBy.SelectedValue = "ALL";
                CmbGroupAreaID.SelectedValue = "[Select]";
                CmbAreaID.SelectedValue = "[Select]";
                DateTime firstDateOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                
                txtSearch.Text = "";
                txtDateFrom.Text = firstDateOfMonth.ToString("yyyy-MM-dd", culture);
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd", culture);
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }

        protected void CmbGroupAreaID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbGroupAreaID.SelectedItem.Value.ToString(), "sp_list_ref_area_by_group_area");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = PrepareExportData(Session[sSessionRecList] as DataSet);
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Job Order Details New Installation", strFullPath.Trim(), 50000, ref sMsg))
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
                    div_comment.InnerHtml = "No records found";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdExportXls_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
                Rec.RecData = PrepareExportData(Session[sSessionRecList] as DataSet);
                if (Rec.RecordCount() > 0)
                {
                    gv.RowDataBound += GvExport_RowDataBound;
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .xlDate { mso-number-format:'yyyy-mm-dd hh:mm'; } .xlDateOnly { mso-number-format:'yyyy-mm-dd'; } .xlDateText { mso-number-format:\@; } </style>";
                    Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\"><head>");
                    Response.Write(style);
                    Response.Write("</head><body>");
                    Response.Output.Write(sw.ToString());
                    Response.Write("</body></html>");
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_comment.InnerHtml = "No records found";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 6; i <= 10; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    for (int i = 6; i <= 10; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private List<int> exportDateColumnIndexes = new List<int>();
        private List<int> exportDateOnlyColumnIndexes = new List<int>();
        private List<int> exportTextDateColumnIndexes = new List<int>();

        private DataSet PrepareExportData(DataSet source)
        {
            exportDateColumnIndexes = new List<int>();
            exportDateOnlyColumnIndexes = new List<int>();
            exportTextDateColumnIndexes = new List<int>();
            if (source == null || source.Tables.Count == 0)
            {
                return source;
            }

            DataSet export = source.Copy();
            DataTable table = export.Tables[0];
            List<string> dateColumns = new List<string>();
            List<string> dateTimeColumns = new List<string>();

            foreach (DataColumn col in table.Columns)
            {
                if (IsDateTimeKeepTimeColumn(col.ColumnName))
                {
                    dateTimeColumns.Add(col.ColumnName);
                }
                else if (col.DataType == typeof(DateTime) || col.DataType == typeof(DateTimeOffset) || IsExportDateColumn(col.ColumnName))
                {
                    dateColumns.Add(col.ColumnName);
                }
            }

            foreach (string colName in dateColumns.Distinct().ToList())
            {
                ReplaceColumnWithExcelSerial(table, colName);
            }

            foreach (string colName in dateTimeColumns.Distinct().ToList())
            {
                ReplaceColumnWithFormattedDateTime(table, colName);
            }

            return export;
        }

        private static bool IsExportDateColumn(string columnName)
        {
            string name = (columnName ?? "").Trim().ToLowerInvariant();
            return name.Contains("date") || name.Contains("dtm") || name.Contains("tgl");
        }

        private void ReplaceColumnWithExcelSerial(DataTable table, string columnName)
        {
            DataColumn sourceCol = table.Columns[columnName];
            int ordinal = sourceCol.Ordinal;
            string tempName = columnName + "__serial";

            table.Columns.Add(tempName, typeof(double));
            foreach (DataRow row in table.Rows)
            {
                DateTime? dt = ParseExportDate(row[columnName]);
                if (dt.HasValue)
                {
                    row[tempName] = dt.Value.ToOADate();
                }
            }

            table.Columns.Remove(columnName);
            table.Columns[tempName].ColumnName = columnName;
            table.Columns[columnName].SetOrdinal(ordinal);
            if (IsDateOnlyExportColumn(columnName))
            {
                exportDateOnlyColumnIndexes.Add(ordinal);
            }
            else
            {
                exportDateColumnIndexes.Add(ordinal);
            }
        }

        private static bool IsDateOnlyExportColumn(string columnName)
        {
            string name = (columnName ?? "").Trim().ToLowerInvariant();
            return name == "tgl_mis" || name == "tgl_pasang" || name == "regdate" || name == "scheduledate";
        }

        private static bool IsDateTimeKeepTimeColumn(string columnName)
        {
            string name = (columnName ?? "").Trim().ToLowerInvariant();
            return name == "tgl_job_header";
        }

        private static string GetDateTimeKeepTimeFormat(string columnName)
        {
            return "yyyy-MM-dd HH:mm:ss";
        }

        private void ReplaceColumnWithFormattedDateTime(DataTable table, string columnName)
        {
            DataColumn sourceCol = table.Columns[columnName];
            int ordinal = sourceCol.Ordinal;
            string tempName = columnName + "__fmt";
            string format = GetDateTimeKeepTimeFormat(columnName);

            table.Columns.Add(tempName, typeof(string));
            foreach (DataRow row in table.Rows)
            {
                DateTime? dt = ParseExportDate(row[columnName]);
                if (dt.HasValue)
                {
                    row[tempName] = dt.Value.ToString(format, CultureInfo.InvariantCulture);
                }
            }

            table.Columns.Remove(columnName);
            table.Columns[tempName].ColumnName = columnName;
            table.Columns[columnName].SetOrdinal(ordinal);
            exportTextDateColumnIndexes.Add(ordinal);
        }

        private static DateTime? ParseExportDate(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is DateTime)
            {
                return (DateTime)value;
            }

            if (value is DateTimeOffset)
            {
                return ((DateTimeOffset)value).DateTime;
            }

            string raw = ConvertAmPmTimeTo24Hour(value.ToString().Trim());
            if (raw == "")
            {
                return null;
            }

            DateTime dt;
            string[] formats = new string[] { "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy", "yyyy-MM-dd" };
            if (DateTime.TryParseExact(raw, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }

            if (DateTime.TryParse(raw, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt)
                || DateTime.TryParse(raw, new CultureInfo("en-US"), DateTimeStyles.None, out dt)
                || DateTime.TryParse(raw, new CultureInfo("id-ID"), DateTimeStyles.None, out dt))
            {
                return dt;
            }

            return null;
        }

        private static string ConvertAmPmTimeTo24Hour(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return raw;
            }

            return Regex.Replace(raw, @"\b(\d{1,2}):(\d{2})(?::(\d{2}))?\s*(AM|PM)\b", match =>
            {
                int hour = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                int minute = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                string meridian = match.Groups[4].Value.ToUpperInvariant();

                if (meridian == "AM")
                {
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
                else if (hour != 12)
                {
                    hour += 12;
                }

                if (match.Groups[3].Success)
                {
                    return string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}:{2}", hour, minute, match.Groups[3].Value);
                }

                return string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", hour, minute);
            }, RegexOptions.IgnoreCase);
        }

        private void GvExport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            foreach (int i in exportDateColumnIndexes)
            {
                if (i < e.Row.Cells.Count)
                {
                    e.Row.Cells[i].Attributes["class"] = "xlDate";
                    e.Row.Cells[i].Attributes["style"] = "mso-number-format:'yyyy-mm-dd hh:mm'";
                }
            }
            foreach (int i in exportDateOnlyColumnIndexes)
            {
                if (i < e.Row.Cells.Count)
                {
                    e.Row.Cells[i].Attributes["class"] = "xlDateOnly";
                    e.Row.Cells[i].Attributes["style"] = "mso-number-format:'yyyy-mm-dd'";
                }
            }
            foreach (int i in exportTextDateColumnIndexes)
            {
                if (i < e.Row.Cells.Count)
                {
                    e.Row.Cells[i].Attributes["class"] = "xlDateText";
                    e.Row.Cells[i].Attributes["style"] = "mso-number-format:\\@";
                    e.Row.Cells[i].Attributes["x:str"] = e.Row.Cells[i].Text;
                }
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}