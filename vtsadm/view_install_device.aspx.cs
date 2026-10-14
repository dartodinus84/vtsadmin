using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Driver;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_install_device : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewInstallDeviceFieldSort";
        string sViewStateDirSort = "RecViewInstallDeviceDirSort";
        string sSessionRecList = "RecViewInstallDevice";

        private const string ColNosn = "nosn";
        private const string ColGpsTime = "gps_time";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL;
                if (chkIncludeINTP.Checked)
                {
                    strSQL = "sp_view_installation_device_intp '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                }
                else
                {
                    strSQL = "sp_view_installation_device2 '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                }
                ViewState[sViewStateFieldSort] = "installdate";
                ViewState[sViewStateDirSort] = "DESC";
                DataSet ds = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
                ApplyMongoGpsTime(ds);
                ApplyChannelStatus(ds);
                Session[sSessionRecList] = ds;
                RebindGridViewAfterEnrichment(ds);
            }
            catch (Exception ex)
            {

            }
        }

        private void RebindGridViewAfterEnrichment(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                DataTable dt = ds.Tables[0];
                string sortField = ViewState[sViewStateFieldSort]?.ToString() ?? "";
                string sortDir = ViewState[sViewStateDirSort]?.ToString() ?? "";
                try
                {
                    if (!string.IsNullOrEmpty(sortField) && dt.Columns.Contains(sortField))
                    {
                        dt.DefaultView.Sort = sortField + " " + sortDir;
                    }
                }
                catch
                {
                    dt.DefaultView.Sort = "";
                }

                GridView2.DataSource = dt.DefaultView;
                GridView2.DataBind();
            }
            catch
            {
            }
        }

        private void ApplyChannelStatus(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                DataTable dt = ds.Tables[0];
                if (!dt.Columns.Contains("IsRequireChannelSetting"))
                {
                    return;
                }

                if (!dt.Columns.Contains("StatusChannel"))
                {
                    dt.Columns.Add("StatusChannel", typeof(string));
                }

                string connStr = MdvrChannelService.ResolveSqlConnectionString(HttpContext.Current);
                if (string.IsNullOrEmpty(connStr))
                {
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    string reqRaw = Convert.ToString(row["IsRequireChannelSetting"]).Trim().ToLowerInvariant();
                    bool isRequire = (reqRaw == "1" || reqRaw == "true" || reqRaw == "yes" || reqRaw == "y");
                    if (!isRequire)
                    {
                        row["StatusChannel"] = "Tidak diperlukan";
                        continue;
                    }

                    string nosn = "";
                    if (dt.Columns.Contains("nosn"))
                    {
                        nosn = Convert.ToString(row["nosn"]).Trim();
                    }
                    else if (dt.Columns.Contains("NoSN"))
                    {
                        nosn = Convert.ToString(row["NoSN"]).Trim();
                    }

                    if (string.IsNullOrEmpty(nosn))
                    {
                        row["StatusChannel"] = "Belum disetting";
                        continue;
                    }

                    try
                    {
                        row["StatusChannel"] = MdvrChannelService.IsChannelConfigured(connStr, nosn)
                            ? "Sudah disetting"
                            : "Belum disetting";
                    }
                    catch
                    {
                        row["StatusChannel"] = "Belum disetting";
                    }
                }
            }
            catch
            {
            }
        }

        private void ApplyMongoGpsTime(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                DataTable dt = ds.Tables[0];
                // Ganti kolom gps_time jadi string writable — buang nilai SP
                EnsureMongoGpsTimeColumn(dt);

                if (!dt.Columns.Contains(ColNosn))
                {
                    return;
                }

                var distinctNosn = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in dt.Rows)
                {
                    string nosn = GetNosnFromRow(row);
                    if (!string.IsNullOrEmpty(nosn))
                    {
                        distinctNosn.Add(nosn);
                    }
                }

                if (distinctNosn.Count == 0)
                {
                    return;
                }

                Dictionary<string, DateTime> gpsTimeMap = FetchLastGpsTimesFromMongo(distinctNosn.ToList());
                foreach (DataRow row in dt.Rows)
                {
                    string nosn = GetNosnFromRow(row);
                    if (string.IsNullOrEmpty(nosn))
                    {
                        continue;
                    }

                    DateTime gpsTime;
                    if (gpsTimeMap.TryGetValue(nosn, out gpsTime))
                    {
                        row[ColGpsTime] = gpsTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Buang gps_time dari SP; sediakan kolom string kosong untuk diisi Mongo saja.
        /// </summary>
        private static void EnsureMongoGpsTimeColumn(DataTable dt)
        {
            if (dt.Columns.Contains(ColGpsTime))
            {
                DataColumn oldCol = dt.Columns[ColGpsTime];
                int ordinal = oldCol.Ordinal;
                string tmpName = ColGpsTime + "__mongo";
                dt.Columns.Add(tmpName, typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row[tmpName] = DBNull.Value;
                }
                dt.Columns.Remove(ColGpsTime);
                dt.Columns[tmpName].ColumnName = ColGpsTime;
                dt.Columns[ColGpsTime].SetOrdinal(ordinal);
            }
            else
            {
                dt.Columns.Add(ColGpsTime, typeof(string));
            }
        }

        // Mapping: SP result nosn -> MongoDB last_position.gps_sn
        private static string GetNosnFromRow(DataRow row)
        {
            if (row == null || row[ColNosn] == null || row[ColNosn] == DBNull.Value)
            {
                return null;
            }

            string nosn = row[ColNosn].ToString().Trim();
            return string.IsNullOrEmpty(nosn) ? null : nosn;
        }

        /// <summary>
        /// Pakai MongoGPSDATA (sama device_qc_new / dashboard notif).
        /// </summary>
        private static string ResolveMongoConnectionString()
        {
            ConnectionStringSettings gpsData = ConfigurationManager.ConnectionStrings["MongoGPSDATA"];
            if (gpsData != null && !string.IsNullOrWhiteSpace(gpsData.ConnectionString))
            {
                return gpsData.ConnectionString;
            }

            return null;
        }

        private Dictionary<string, DateTime> FetchLastGpsTimesFromMongo(List<string> distinctNosn)
        {
            var result = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string mongoConnection = ResolveMongoConnectionString();
                if (string.IsNullOrWhiteSpace(mongoConnection))
                {
                    return result;
                }

                var client = new MongoClient(mongoConnection);
                var database = client.GetDatabase("GPSData");
                var collection = database.GetCollection<BsonDocument>("last_position");

                // Batch (jangan 1 query / SN — 245 row bisa timeout → semua GPS Time kosong)
                const int batchSize = 200;
                for (int i = 0; i < distinctNosn.Count; i += batchSize)
                {
                    var batch = distinctNosn.Skip(i).Take(batchSize).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
                    if (batch.Count == 0)
                    {
                        continue;
                    }

                    FillGpsTimesFromBatch(collection, batch, result);
                }
            }
            catch
            {
            }

            return result;
        }

        private static void FillGpsTimesFromBatch(
            IMongoCollection<BsonDocument> collection,
            List<string> batch,
            Dictionary<string, DateTime> result)
        {
            try
            {
                // $in string + NumberLong agar match tipe apapun di Mongo
                var snValues = new BsonArray();
                foreach (string sn in batch)
                {
                    snValues.Add(sn);
                    long snLong;
                    if (long.TryParse(sn, out snLong))
                    {
                        snValues.Add(snLong);
                    }
                }

                var pipeline = new[]
                {
                    new BsonDocument("$match", new BsonDocument("gps_sn", new BsonDocument("$in", snValues))),
                    new BsonDocument("$sort", new BsonDocument
                    {
                        { "gps_time", -1 },
                        { "last_gps_time", -1 }
                    }),
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", "$gps_sn" },
                        { "gps_time", new BsonDocument("$first", "$gps_time") },
                        { "last_gps_time", new BsonDocument("$first", "$last_gps_time") }
                    }),
                    new BsonDocument("$project", new BsonDocument
                    {
                        { "_id", 0 },
                        { "gps_sn", "$_id" },
                        { "gps_time", new BsonDocument("$dateAdd", new BsonDocument
                            {
                                { "startDate", new BsonDocument("$ifNull", new BsonArray { "$gps_time", "$last_gps_time" }) },
                                { "unit", "hour" },
                                { "amount", 7 }
                            })
                        }
                    })
                };

                List<BsonDocument> docs = collection.Aggregate<BsonDocument>(pipeline).ToList();
                foreach (BsonDocument doc in docs)
                {
                    if (doc == null || !doc.Contains("gps_sn") || !doc.Contains("gps_time") || doc["gps_time"].IsBsonNull)
                    {
                        continue;
                    }

                    string snKey = doc["gps_sn"].ToString().Trim();
                    if (string.IsNullOrEmpty(snKey))
                    {
                        continue;
                    }

                    DateTime utc = doc["gps_time"].ToUniversalTime();
                    result[snKey] = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, DateTimeKind.Unspecified);
                }
            }
            catch
            {
                // Fallback Find per SN di batch ini saja
                foreach (string sn in batch)
                {
                    DateTime? gpsTime = TryGetGpsTimeByFind(collection, sn);
                    if (gpsTime.HasValue)
                    {
                        result[sn] = gpsTime.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Fallback: Find by gps_sn, ambil gps_time (atau last_gps_time), +7 di C#.
        /// </summary>
        private static DateTime? TryGetGpsTimeByFind(IMongoCollection<BsonDocument> collection, string sn)
        {
            try
            {
                var snOr = new List<FilterDefinition<BsonDocument>>
                {
                    Builders<BsonDocument>.Filter.Eq("gps_sn", sn)
                };
                long snLong;
                if (long.TryParse(sn, out snLong))
                {
                    snOr.Add(Builders<BsonDocument>.Filter.Eq("gps_sn", snLong));
                }

                var filter = Builders<BsonDocument>.Filter.Or(snOr);
                BsonDocument doc = collection.Find(filter)
                    .Sort(Builders<BsonDocument>.Sort.Descending("gps_time").Descending("last_gps_time"))
                    .Limit(1)
                    .Project(Builders<BsonDocument>.Projection
                        .Include("gps_sn")
                        .Include("gps_time")
                        .Include("last_gps_time"))
                    .FirstOrDefault();

                if (doc == null)
                {
                    return null;
                }

                BsonValue raw = null;
                if (doc.Contains("gps_time") && !doc["gps_time"].IsBsonNull)
                {
                    raw = doc["gps_time"];
                }
                else if (doc.Contains("last_gps_time") && !doc["last_gps_time"].IsBsonNull)
                {
                    raw = doc["last_gps_time"];
                }

                DateTime? parsed = ParseLastGpsTime(raw);
                if (!parsed.HasValue)
                {
                    return null;
                }

                return parsed.Value.AddHours(7);
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? ParseLastGpsTime(BsonValue value)
        {
            if (value == null || value.IsBsonNull)
            {
                return null;
            }

            switch (value.BsonType)
            {
                case BsonType.DateTime:
                    DateTime utc = value.ToUniversalTime();
                    return new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, DateTimeKind.Unspecified);
                case BsonType.String:
                    DateTime dt;
                    if (DateTime.TryParse(value.AsString, out dt))
                    {
                        return dt;
                    }
                    return null;
                default:
                    return null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWINSTALLDEV"))
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
                            div_comment.InnerHtml = "";
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
                txtSearch.Text = "";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                chkIncludeINTP.Checked = false;
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

        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewInstallDevice"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Installation Device", strFullPath.Trim(), 50000, ref sMsg))
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
                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
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
                    for (int i = 18; i <= 48; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdDetails");
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[6].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[10].Text.ToString() + "','" + e.Row.Cells[11].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[12].Text.ToString() + "','" + e.Row.Cells[13].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[14].Text.ToString() + "','" + e.Row.Cells[15].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[16].Text.ToString() + "','" + e.Row.Cells[17].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[18].Text.ToString() + "','" + e.Row.Cells[19].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[20].Text.ToString() + "','" + e.Row.Cells[21].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[22].Text.ToString() + "','" + e.Row.Cells[23].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[24].Text.ToString() + "','" + e.Row.Cells[25].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[26].Text.ToString() + "','" + e.Row.Cells[27].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[28].Text.ToString() + "','" + e.Row.Cells[29].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[30].Text.ToString() + "','" + e.Row.Cells[31].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[32].Text.ToString() + "','" + e.Row.Cells[33].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[34].Text.ToString() + "','" + e.Row.Cells[35].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[36].Text.ToString() + "','" + e.Row.Cells[37].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[38].Text.ToString() + "','" + e.Row.Cells[39].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[40].Text.ToString() + "','" + e.Row.Cells[41].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[42].Text.ToString() + "','" + e.Row.Cells[43].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[44].Text.ToString() + "','" + e.Row.Cells[45].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[46].Text.ToString() + "','" + e.Row.Cells[47].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[48].Text.ToString() + "'); return false;";

                    LinkButton CmdAcc = (LinkButton)e.Row.FindControl("CmdAcc");
                    CmdAcc.OnClientClick = "postAcc('" + e.Row.Cells[18].Text.ToString() + "'); return false;";

                     LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    CmdPic.OnClientClick = "postPic('" + e.Row.Cells[40].Text.ToString() + "'); return false;";

                    // Setting Channel MDVR — pola sama device_qc (BuildStatusCellHtml)
                    Literal litChannel = (Literal)e.Row.FindControl("litChannelStatus");
                    if (litChannel != null)
                    {
                        DataRowView drv = e.Row.DataItem as DataRowView;
                        object isReq = null;
                        object status = null;
                        object nosn = null;
                        object deviceType = null;
                        object maxCh = null;
                        if (drv != null)
                        {
                            if (drv.Row.Table.Columns.Contains("IsRequireChannelSetting"))
                            {
                                isReq = drv["IsRequireChannelSetting"];
                            }
                            if (drv.Row.Table.Columns.Contains("StatusChannel"))
                            {
                                status = drv["StatusChannel"];
                            }
                            if (drv.Row.Table.Columns.Contains("nosn"))
                            {
                                nosn = drv["nosn"];
                            }
                            else if (drv.Row.Table.Columns.Contains("NoSN"))
                            {
                                nosn = drv["NoSN"];
                            }
                            if (drv.Row.Table.Columns.Contains("device_type"))
                            {
                                deviceType = drv["device_type"];
                            }
                            if (drv.Row.Table.Columns.Contains("MaxChannel"))
                            {
                                maxCh = drv["MaxChannel"];
                            }
                        }
                        litChannel.Text = MdvrChannelService.BuildStatusCellHtml(isReq, status, nosn, deviceType, maxCh);
                    }

                    for (int i = 18; i <= 48; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

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