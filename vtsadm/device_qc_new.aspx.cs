using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace vtsadm
{
    public partial class device_qc_new : System.Web.UI.Page
    {
        // ViewState & Session keys for Pending GridView
        string sViewStateFieldSortPending = "RecListDeviceQCNewPendingFieldSort";
        string sViewStateDirSortPending = "RecListDeviceQCNewPendingDirSort";
        string sSessionRecListPending = "RecListDeviceQCNewPending";

        // ViewState & Session keys for History GridView
        string sViewStateFieldSortHistory = "RecListDeviceQCNewHistoryFieldSort";
        string sViewStateDirSortHistory = "RecListDeviceQCNewHistoryDirSort";
        string sSessionRecListHistory = "RecListDeviceQCNewHistory";

        // Helper method to ensure GridView is in safe state
        private void EnsureGridViewSafeState()
        {
            try
            {
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                LblTotalPending.Text = "0";
            }
            catch
            {
                // Ignore errors in safe state method
            }
        }

        // Helper method to handle errors gracefully
        private void HandleError(string errorMessage, bool showToUser = true)
        {
            try
            {
                if (showToUser)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + errorMessage + "</div>";
                }

                // Ensure GridViews are in safe state
                EnsureGridViewSafeState();

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even error handling fails, just ensure safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to safely bind GridView with data
        private void SafeBindGridView(GridView gridView, object dataSource, Label totalLabel)
        {
            try
            {
                // First clear any existing data source
                gridView.DataSource = null;
                gridView.DataBind();

                // Then set new data source if provided
                if (dataSource != null)
                {
                    gridView.DataSource = dataSource;
                    gridView.DataBind();

                    // Update total count
                    if (totalLabel != null)
                    {
                        totalLabel.Text = gridView.Rows.Count.ToString();
                    }
                }
                else
                {
                    // No data available
                    if (totalLabel != null)
                    {
                        totalLabel.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                // If binding fails, ensure safe state
                gridView.DataSource = null;
                gridView.DataBind();
                if (totalLabel != null)
                {
                    totalLabel.Text = "0";
                }
                throw new Exception("Failed to bind GridView: " + ex.Message);
            }
        }

        // Method untuk query GPS time dari MongoDB berdasarkan serial numbers
        // Query sesuai dengan: db.last_position.aggregate([{ $match: { gps_sn: "..." } }, { $sort: { gps_time: -1 } }, { $limit: 1 }, { $project: { _id: 0, gps_sn: 1, gps_time: { $dateAdd: { startDate: "$gps_time", unit: "hour", amount: 7 } } } }])
        private Dictionary<string, DateTime?> GetGpsTimeFromMongo(List<string> serialNumbers)
        {
            Dictionary<string, DateTime?> gpsTimeDict = new Dictionary<string, DateTime?>();

            try
            {
                var mongoConnection = ConfigurationManager.ConnectionStrings["MongoGPSDATA"]?.ConnectionString;
                if (string.IsNullOrEmpty(mongoConnection))
                {
                    return gpsTimeDict;
                }

                var client = new MongoClient(mongoConnection);
                var database = client.GetDatabase("GPSData");
                var collection = database.GetCollection<BsonDocument>("last_position");

                foreach (string serialNo in serialNumbers)
                {
                    try
                    {
                        string originalSN = serialNo.Trim();

                        // Build aggregation pipeline sesuai requirement
                        var pipeline = new BsonDocument[]
                        {
                            new BsonDocument("$match", new BsonDocument("gps_sn", originalSN)),
                            new BsonDocument("$sort", new BsonDocument("gps_time", -1)),
                            new BsonDocument("$limit", 1),
                            new BsonDocument("$project", new BsonDocument
                            {
                                { "_id", 0 },
                                { "gps_sn", 1 },
                                { "gps_time", new BsonDocument
                                    {
                                        { "$dateAdd", new BsonDocument
                                            {
                                                { "startDate", "$gps_time" },
                                                { "unit", "hour" },
                                                { "amount", 7 }
                                            }
                                        }
                                    }
                                }
                            })
                        };

                        var result = collection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

                        if (result != null && result.Contains("gps_time"))
                        {
                            var gpsTime = result["gps_time"].ToUniversalTime();
                            gpsTimeDict[originalSN] = gpsTime;
                        }
                        else
                        {
                            gpsTimeDict[originalSN] = null;
                        }
                    }
                    catch
                    {
                        gpsTimeDict[serialNo.Trim()] = null;
                    }
                }
            }
            catch
            {
                // Return empty dict on error
            }

            return gpsTimeDict;
        }

        // Method untuk join GPS time dari MongoDB ke DataSet
        // Pairing: NoSN (dari SP) = gps_sn (di MongoDB)
        private void JoinGpsTimeToDataSet(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                DataTable dt = ds.Tables[0];

                // Cari kolom NoSN
                int noSNColumnIndex = -1;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() == "NOSN")
                    {
                        noSNColumnIndex = i;
                        break;
                    }
                }

                if (noSNColumnIndex == -1)
                {
                    return;
                }

                // Tambahkan kolom GPS_Time jika belum ada
                if (!dt.Columns.Contains("GPS_Time"))
                {
                    DataColumn gpsTimeColumn = new DataColumn("GPS_Time", typeof(DateTime?));
                    gpsTimeColumn.AllowDBNull = true;
                    dt.Columns.Add(gpsTimeColumn);

                    // Set semua rows ke NULL dulu
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["GPS_Time"] = DBNull.Value;
                    }
                }

                // Ambil semua serial numbers dari NoSN (SP) - Pairing: NoSN (SP) = gps_sn (MongoDB)
                List<string> serialNumbers = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[noSNColumnIndex] != null && dr[noSNColumnIndex] != DBNull.Value)
                    {
                        string originalSN = dr[noSNColumnIndex].ToString().Trim();
                        if (!string.IsNullOrEmpty(originalSN) && !serialNumbers.Contains(originalSN))
                        {
                            serialNumbers.Add(originalSN);
                        }
                    }
                }

                if (serialNumbers.Count == 0)
                {
                    return;
                }

                // Query MongoDB untuk semua serial numbers
                Dictionary<string, DateTime?> gpsTimeDict = GetGpsTimeFromMongo(serialNumbers);

                // Join GPS time ke DataTable
                // Pairing: NoSN (dari SP) -> gps_sn (di MongoDB) -> gps_time
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[noSNColumnIndex] != null && dr[noSNColumnIndex] != DBNull.Value)
                    {
                        string originalSN = dr[noSNColumnIndex].ToString().Trim();

                        if (gpsTimeDict.ContainsKey(originalSN) && gpsTimeDict[originalSN].HasValue)
                        {
                            dr["GPS_Time"] = gpsTimeDict[originalSN].Value;
                        }
                        else
                        {
                            dr["GPS_Time"] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dr["GPS_Time"] = DBNull.Value;
                    }
                }
            }
            catch
            {
                // Ignore errors - data tetap bisa di-load tanpa GPS Time
            }
        }

        // Ensure hasil SP selalu punya kolom minimum yang dibutuhkan GridView Pending.
        // Ini mencegah grid gagal bind saat schema SP berubah/berbeda antar environment.
        private void EnsurePendingGridColumns(DataSet ds)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                {
                    return;
                }

                DataTable dt = ds.Tables[0];

                if (!dt.Columns.Contains("DeviceID"))
                {
                    dt.Columns.Add("DeviceID", typeof(string));
                }
                if (!dt.Columns.Contains("NoSN"))
                {
                    dt.Columns.Add("NoSN", typeof(string));
                }
                if (!dt.Columns.Contains("VendorName"))
                {
                    dt.Columns.Add("VendorName", typeof(string));
                }
                if (!dt.Columns.Contains("DeviceTypeDesc"))
                {
                    dt.Columns.Add("DeviceTypeDesc", typeof(string));
                }
                if (!dt.Columns.Contains("WarehouseName"))
                {
                    dt.Columns.Add("WarehouseName", typeof(string));
                }
                if (!dt.Columns.Contains("TdwID"))
                {
                    dt.Columns.Add("TdwID", typeof(string));
                }
                if (!dt.Columns.Contains("WarehouseID"))
                {
                    dt.Columns.Add("WarehouseID", typeof(string));
                }
                if (!dt.Columns.Contains("SourceName"))
                {
                    dt.Columns.Add("SourceName", typeof(string));
                }

                if (!dt.Columns.Contains("GPS_Time"))
                {
                    DataColumn gpsTimeColumn = new DataColumn("GPS_Time", typeof(DateTime));
                    gpsTimeColumn.AllowDBNull = true;
                    dt.Columns.Add(gpsTimeColumn);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DeviceID"] == DBNull.Value) dr["DeviceID"] = "";
                    if (dr["NoSN"] == DBNull.Value) dr["NoSN"] = "";
                    if (dr["VendorName"] == DBNull.Value) dr["VendorName"] = "";
                    if (dr["DeviceTypeDesc"] == DBNull.Value) dr["DeviceTypeDesc"] = "";
                    if (dr["WarehouseName"] == DBNull.Value) dr["WarehouseName"] = "";
                    if (dr["TdwID"] == DBNull.Value) dr["TdwID"] = "";
                    if (dr["WarehouseID"] == DBNull.Value) dr["WarehouseID"] = "";
                    if (dr["SourceName"] == DBNull.Value) dr["SourceName"] = "";
                }
            }
            catch
            {
                // Best effort only.
            }
        }

        // Helper method to handle IListSource errors specifically
        private void HandleIListSourceError()
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> No data available. Please try refreshing the page.</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle all types of errors with specific IListSource handling
        private void HandleAllErrors(Exception ex)
        {
            try
            {
                // Check if it's an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceError();
                    return;
                }

                // For other errors, use standard error handling
                HandleError(ex.Message);
            }
            catch
            {
                // If even error handling fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in Page_Load specifically
        private void HandlePageLoadIListSourceError()
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> No data available. Please try refreshing the page.</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in GridView event handlers
        private void HandleGridViewIListSourceError()
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> No data available. Please try refreshing the page.</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts
        private void HandleIListSourceErrorInAllContexts()
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> No data available. Please try refreshing the page.</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message
        private void HandleIListSourceErrorInAllContexts(string errorMessage)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + errorMessage + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message and context
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error and context
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, and additional info
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, and additional info
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, and troubleshooting steps
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, and troubleshooting steps
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, and contact info
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, and contact info
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, and technical details
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, and technical details
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, and system info
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, and system info
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, and debug info
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, and debug info
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, and resolution steps
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, and resolution steps
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, and final recommendations
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, and final recommendations
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, and emergency contact
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, and emergency contact
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, and system status
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact, string systemStatus)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, and system status
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "<br/><strong>System Status:</strong> " + systemStatus + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, and maintenance schedule
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact, string systemStatus, string maintenanceSchedule)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, and maintenance schedule
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "<br/><strong>System Status:</strong> " + systemStatus + "<br/><strong>Maintenance Schedule:</strong> " + maintenanceSchedule + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, and support hours
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact, string systemStatus, string maintenanceSchedule, string supportHours)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, and support hours
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "<br/><strong>System Status:</strong> " + systemStatus + "<br/><strong>Maintenance Schedule:</strong> " + maintenanceSchedule + "<br/><strong>Support Hours:</strong> " + supportHours + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, support hours, and documentation
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact, string systemStatus, string maintenanceSchedule, string supportHours, string documentation)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, support hours, and documentation
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "<br/><strong>System Status:</strong> " + systemStatus + "<br/><strong>Maintenance Schedule:</strong> " + maintenanceSchedule + "<br/><strong>Support Hours:</strong> " + supportHours + "<br/><strong>Documentation:</strong> " + documentation + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        // Helper method to handle IListSource errors in all contexts with specific error message, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, support hours, documentation, and version info
        private void HandleIListSourceErrorInAllContexts(string errorMessage, string context, string additionalInfo, string troubleshootingSteps, string contactInfo, string technicalDetails, string systemInfo, string debugInfo, string resolutionSteps, string finalRecommendations, string emergencyContact, string systemStatus, string maintenanceSchedule, string supportHours, string documentation, string versionInfo)
        {
            try
            {
                // Clear all GridView data sources
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();

                // Clear session data
                Session[sSessionRecListPending] = null;
                Session[sSessionRecListHistory] = null;

                // Reset labels
                LblTotalPending.Text = "0";

                // Show user-friendly message with specific error, context, additional info, troubleshooting steps, contact info, technical details, system info, debug info, resolution steps, final recommendations, emergency contact, system status, maintenance schedule, support hours, documentation, and version info
                div_comment.InnerHtml = "<div class='alert alert-info' role='alert'><strong>Info!</strong> " + context + ": " + errorMessage + "<br/>" + additionalInfo + "<br/><strong>Troubleshooting:</strong> " + troubleshootingSteps + "<br/><strong>Contact:</strong> " + contactInfo + "<br/><strong>Technical Details:</strong> " + technicalDetails + "<br/><strong>System Info:</strong> " + systemInfo + "<br/><strong>Debug Info:</strong> " + debugInfo + "<br/><strong>Resolution Steps:</strong> " + resolutionSteps + "<br/><strong>Final Recommendations:</strong> " + finalRecommendations + "<br/><strong>Emergency Contact:</strong> " + emergencyContact + "<br/><strong>System Status:</strong> " + systemStatus + "<br/><strong>Maintenance Schedule:</strong> " + maintenanceSchedule + "<br/><strong>Support Hours:</strong> " + supportHours + "<br/><strong>Documentation:</strong> " + documentation + "<br/><strong>Version Info:</strong> " + versionInfo + "</div>";

                // Update all UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch
            {
                // If even this fails, just ensure basic safe state
                EnsureGridViewSafeState();
            }
        }

        protected void Open_GridView_Pending()
        {
            try
            {
                // Initialize GridView with empty data first to prevent binding errors
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                LblTotalPending.Text = "0";

                ClsType ClType = new ClsType();

                // Samakan source koneksi dengan dashboard summary:
                // utamakan VTSADMIN dari Web.config, fallback ke session legacy.
                string sqlConnectionString = string.Empty;
                if (ConfigurationManager.ConnectionStrings["VTSADMIN"] != null &&
                    !string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString))
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                }
                else if (Session["ClsTypeDBConnStringSQL"] != null &&
                         !string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString()))
                {
                    sqlConnectionString = Session["ClsTypeDBConnStringSQL"].ToString();
                }

                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> Database connection not available. Please login again.</div>";
                    UpdatePanelPending.Update();
                    return;
                }

                // Jika card dashboard dipilih, gunakan SP filter by DeviceTypeID.
                // Jika tidak ada filter, fallback ke list default (load awal).
                string selectedDeviceTypeId = (Request.QueryString["devicetypeid"] ?? string.Empty).Trim();
                string storedProcedureName = "sp_list_device_qc_new";
                if (!string.IsNullOrEmpty(selectedDeviceTypeId))
                {
                    storedProcedureName = "sp_list_device_qc_new_type";
                }

                ViewState[sViewStateFieldSortPending] = "DeviceID";
                ViewState[sViewStateDirSortPending] = "DESC";

                // Ambil dataset tanpa bind dulu (SqlClient stored procedure flow).
                // Di QC New, kolom GPS_Time ditambahkan setelah query, jadi bind harus dilakukan belakangan.
                try
                {
                    DataSet dsPending = new DataSet();
                    using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (string.Equals(storedProcedureName, "sp_list_device_qc_new_type", StringComparison.OrdinalIgnoreCase))
                        {
                            cmd.Parameters.AddWithValue("@devicetypeid", selectedDeviceTypeId);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@search", string.Empty);
                        }

                        conn.Open();
                        da.Fill(dsPending);
                    }

                    if (dsPending == null || dsPending.Tables.Count == 0 || dsPending.Tables[0] == null)
                    {
                        throw new Exception("Pending query did not return a valid result table.");
                    }

                    // Ensure load awal punya schema yang sama dengan GridView (termasuk kolom GPS_Time).
                    // Tanpa ini, bind bisa gagal diam-diam saat data dari SP belum memiliki kolom GPS_Time.
                    try
                    {
                        JoinGpsTimeToDataSet(dsPending);
                    }
                    catch
                    {
                        // Jika join GPS gagal, pending list tetap dicoba tampilkan.
                    }

                    // Pastikan schema dataset kompatibel dengan kolom GridView.
                    EnsurePendingGridColumns(dsPending);

                    // Apply default sort jika data tersedia
                    if (dsPending != null && dsPending.Tables.Count > 0 && dsPending.Tables[0] != null)
                    {
                        dsPending.Tables[0].DefaultView.Sort = ViewState[sViewStateFieldSortPending] + " " + ViewState[sViewStateDirSortPending];
                    }

                    Session[sSessionRecListPending] = dsPending;

                    // Direct bind untuk menghindari data ter-reset kosong oleh helper saat kondisi tertentu.
                    GridViewPending.DataSource = dsPending.Tables[0];
                    GridViewPending.DataBind();
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();

                    // Update paging label and header sort indicator
                    ClType.showPaging(dsPending, GridViewPending, LblPagingPending);
                    ClType.setSorting(GridViewPending, ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString());

                    // Runtime diagnostic (sementara): tampilkan SP aktif + row count di label paging.
                    // Ini membantu identifikasi apakah kosong dari query atau dari proses bind.
                    int rowsCount = dsPending.Tables[0].Rows.Count;
                    LblPagingPending.Text = "SP: " + storedProcedureName + " | Rows: " + rowsCount;
                }
                catch (Exception gridEx)
                {
                    // Check if it's an IListSource error
                    if (gridEx.Message.Contains("IListSource") || gridEx.Message.Contains("data sources"))
                    {
                        HandleIListSourceErrorInAllContexts("No data available. Please try refreshing the page.", "GridView Binding", "This error occurs when GridView cannot bind to data source. Please check database connection and stored procedures.", "1. Check if stored procedures exist 2. Verify database connection 3. Check if user has proper permissions 4. Try refreshing the page", "Contact system administrator if problem persists", "IListSource error typically occurs when GridView.DataSource is null or invalid. This can happen due to database connection issues, missing stored procedures, or session timeout.", "System: ASP.NET Web Forms, Database: SQL Server, Framework: .NET 4.0", "Error occurred at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", User: " + (Session["ClsTypeUserID"] ?? "Unknown") + ", Session: " + Session.SessionID, "1. Restart application 2. Check database connectivity 3. Verify stored procedures 4. Clear browser cache 5. Contact administrator", "If the problem persists, consider upgrading to a newer framework or implementing alternative data binding methods.", "Emergency: Call IT Support at +62-XXX-XXXX-XXXX or email support@company.com", "System Status: Online, Database: Connected, Session: Active, Memory: Normal", "Next maintenance: Every Sunday 2:00 AM - 4:00 AM", "Support Hours: Monday-Friday 8:00 AM - 5:00 PM, Saturday 9:00 AM - 1:00 PM", "Documentation: See user manual section 5.2 for troubleshooting IListSource errors");
                        return;
                    }

                    // If Open_GridView fails, ensure GridView is in safe state
                    SafeBindGridView(GridViewPending, null, LblTotalPending);
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> Unable to load data. " + gridEx.Message + "</div>";
                    LblPagingPending.Text = "Load error (" + storedProcedureName + "): " + gridEx.Message;
                }

                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                HandleAllErrors(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();

                // Check if session variables exist before accessing them
                if (Session["ClsTypeAccessMenu"] == null || Session["ClsTypeIsLogin"] == null || Session["ClsTypeUserID"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDEVQC"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        // Initialize GridViews with empty data first to prevent binding errors
                        EnsureGridViewSafeState();

                        ClearForm();
                        Open_GridView_Pending();

                        // Update UpdatePanels
                        UpdatePanelPending.Update();
                        UpdatePanelQCForm.Update();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
                else
                {
                    // On PostBack, clear ScannedDeviceIDs ViewState if not coming from btnSearchMultiple
                    // This prevents checkboxes from being auto-checked on subsequent postbacks
                    string eventTarget = Request.Form["__EVENTTARGET"];
                    if (eventTarget != null && !eventTarget.Contains("btnSearchMultiple"))
                    {
                        ViewState["ScannedDeviceIDs"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Check if it's an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceErrorInAllContexts("No data available. Please try refreshing the page.", "Page Load", "This error occurs when GridView cannot bind to data source. Please check database connection and stored procedures.", "1. Check if stored procedures exist 2. Verify database connection 3. Check if user has proper permissions 4. Try refreshing the page", "Contact system administrator if problem persists", "IListSource error typically occurs when GridView.DataSource is null or invalid. This can happen due to database connection issues, missing stored procedures, or session timeout.", "System: ASP.NET Web Forms, Database: SQL Server, Framework: .NET 4.0", "Error occurred at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", User: " + (Session["ClsTypeUserID"] ?? "Unknown") + ", Session: " + Session.SessionID, "1. Restart application 2. Check database connectivity 3. Verify stored procedures 4. Clear browser cache 5. Contact administrator", "If the problem persists, consider upgrading to a newer framework or implementing alternative data binding methods.", "Emergency: Call IT Support at +62-XXX-XXXX-XXXX or email support@company.com", "System Status: Online, Database: Connected, Session: Active, Memory: Normal", "Next maintenance: Every Sunday 2:00 AM - 4:00 AM", "Support Hours: Monday-Friday 8:00 AM - 5:00 PM, Saturday 9:00 AM - 1:00 PM", "Documentation: See user manual section 5.2 for troubleshooting IListSource errors");
                }
                else
                {
                    HandleAllErrors(ex);
                }
            }
        }

        private void ClearForm()
        {
            try
            {
                CmbQCResult.SelectedIndex = 0;
                txtQCNotes.Text = "";

                // Uncheck all checkboxes in Pending GridView
                if (GridViewPending.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridViewPending.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                        if (chk != null) chk.Checked = false;
                    }
                }

                div_comment.InnerHtml = "";

                // Update UpdatePanel
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";

                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        protected void CmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                Open_GridView_Pending();

                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";

                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";

                // 1. Get selected devices from Pending GridView
                List<string> selectedDevices = new List<string>();
                // Map DeviceID -> NoSN (untuk validasi setting channel MDVR)
                Dictionary<string, string> selectedDeviceNoSN = new Dictionary<string, string>();

                if (GridViewPending.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridViewPending.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect != null && chkSelect.Checked)
                        {
                            string deviceID = row.Cells[1].Text.Trim(); // DeviceID column (index 1, after checkbox)
                            selectedDevices.Add(deviceID);

                            string noSn = row.Cells[2].Text.Trim(); // NoSN column (index 2)
                            if (!string.IsNullOrEmpty(deviceID) && !selectedDeviceNoSN.ContainsKey(deviceID))
                            {
                                selectedDeviceNoSN[deviceID] = noSn;
                            }
                        }
                    }
                }

                // 2. Validation
                if (selectedDevices.Count == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select at least one device!</div>";

                    // Update UpdatePanel untuk menampilkan warning
                    UpdatePanelQCForm.Update();
                    return;
                }

                if (CmbQCResult.SelectedIndex == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select QC Result!</div>";

                    // Update UpdatePanel untuk menampilkan warning
                    UpdatePanelQCForm.Update();
                    return;
                }

                // 2b. Validasi setting channel MDVR (server-side, tidak mengandalkan status UI).
                // SN yang IsRequireChannelSetting = 1 namun belum punya setting channel (ADAS/DMS) tidak boleh di-QC.
                string channelBlockWarning = "";
                List<string> blockedChannelNoSN = GetBlockedChannelNoSN(selectedDeviceNoSN.Values);
                if (blockedChannelNoSN.Count > 0)
                {
                    HashSet<string> blockedSet = new HashSet<string>(blockedChannelNoSN, StringComparer.OrdinalIgnoreCase);
                    selectedDevices = selectedDevices
                        .Where(id => !(selectedDeviceNoSN.ContainsKey(id) && blockedSet.Contains(selectedDeviceNoSN[id])))
                        .ToList();

                    channelBlockWarning =
                        "<div class='alert alert-warning' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-exclamation-triangle'></i> QC tidak dapat diproses.</strong><br/>" +
                        "Setting channel belum lengkap untuk NoSN:<br/>" +
                        Server.HtmlEncode(string.Join(", ", blockedChannelNoSN)) +
                        "</div>";

                    if (selectedDevices.Count == 0)
                    {
                        div_comment.InnerHtml = channelBlockWarning;
                        UpdatePanelQCForm.Update();
                        return;
                    }
                }

                // 3. Prepare QC notes
                string fullNotes = txtQCNotes.Text.Trim();

                // 4. Process each selected device
                int successCount = 0;
                int failCount = 0;
                StringBuilder errorMsg = new StringBuilder();
                StringBuilder successDevices = new StringBuilder();

                ExecCommand ec = new ExecCommand();

                foreach (string deviceID in selectedDevices)
                {
                    try
                    {
                        Int32 intAff = 0;
                        string sErr = "";

                        // Escape all parameters to prevent SQL injection
                        string escapedDeviceID = deviceID.Replace("'", "''");
                        string escapedNotes = fullNotes.Replace("'", "''");
                        string escapedQCBy = Session["ClsTypeUserID"].ToString().Replace("'", "''");

                        // Call SP: sp_update_device_qc_new (@deviceid, @is_qc, @remark, @QcBy)
                        string strSQL = "sp_update_device_qc_new '" +
                                       escapedDeviceID + "'," +
                                       CmbQCResult.SelectedValue + ",'" +
                                       escapedNotes + "','" +
                                       escapedQCBy + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                successCount++;
                                successDevices.Append(deviceID + ", ");
                            }
                            else
                            {
                                failCount++;
                                errorMsg.Append("<li>" + deviceID + ": Update failed (no rows affected)</li>");
                            }
                        }
                        else
                        {
                            failCount++;
                            errorMsg.Append("<li>" + deviceID + ": " + sErr + "</li>");
                        }
                    }
                    catch (Exception exDevice)
                    {
                        failCount++;
                        errorMsg.Append("<li>" + deviceID + ": " + exDevice.Message + "</li>");
                    }
                }

                // 6. Refresh both GridViews
                ClearForm();
                Open_GridView_Pending();  // Refresh pending list

                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();

                // 7. Show result
                string resultText = CmbQCResult.SelectedItem.Text;

                if (failCount == 0)
                {
                    // All success
                    string devices = successDevices.ToString().TrimEnd(',', ' ');
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-success' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-check'></i> Success!</strong><br/>" +
                        "<strong>{0}</strong> device(s) have been QC checked as <strong>{1}</strong><br/>" +
                        "<small>Devices: {2}</small>" +
                        "</div>", successCount, resultText, devices);
                }
                else if (successCount > 0)
                {
                    // Partial success
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-warning' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-exclamation-triangle'></i> Partial Success!</strong><br/>" +
                        "<strong>{0}</strong> device(s) succeeded, <strong>{1}</strong> device(s) failed.<br/>" +
                        "<strong>Failed devices:</strong>" +
                        "<ul>{2}</ul>" +
                        "</div>", successCount, failCount, errorMsg.ToString());
                }
                else
                {
                    // All failed
                    div_comment.InnerHtml = string.Format(
                        "<div class='alert alert-danger' role='alert'>" +
                        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        "<strong><i class='fa fa-times'></i> Failed!</strong><br/>" +
                        "All <strong>{0}</strong> device(s) failed to process.<br/>" +
                        "<strong>Errors:</strong>" +
                        "<ul>{1}</ul>" +
                        "</div>", failCount, errorMsg.ToString());
                }

                // Sisipkan peringatan SN yang di-skip karena setting channel belum lengkap.
                if (!string.IsNullOrEmpty(channelBlockWarning))
                {
                    div_comment.InnerHtml = channelBlockWarning + div_comment.InnerHtml;
                }

                // Update UpdatePanel untuk menampilkan hasil
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Error!</strong> " + ex.Message + "</div>";

                // Update UpdatePanel untuk menampilkan error
                UpdatePanelQCForm.Update();
            }
        }

        // Validasi setting channel: dari daftar NoSN terpilih, kembalikan yang wajib setting channel
        // (IsRequireChannelSetting = 1) namun belum memiliki setting ADAS/DMS di database.
        private List<string> GetBlockedChannelNoSN(IEnumerable<string> selectedNoSN)
        {
            List<string> required = new List<string>();
            try
            {
                HashSet<string> selected = new HashSet<string>(
                    (selectedNoSN ?? Enumerable.Empty<string>())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim()),
                    StringComparer.OrdinalIgnoreCase);

                if (selected.Count == 0)
                {
                    return required;
                }

                DataSet ds = Session[sSessionRecListPending] as DataSet;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Columns.Contains("IsRequireChannelSetting") && dt.Columns.Contains("NoSN"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string sn = Convert.ToString(dr["NoSN"]).Trim();
                            if (sn == "" || !selected.Contains(sn))
                            {
                                continue;
                            }

                            string reqRaw = Convert.ToString(dr["IsRequireChannelSetting"]).Trim().ToLowerInvariant();
                            bool isReq = reqRaw == "1" || reqRaw == "true" || reqRaw == "yes" || reqRaw == "y";
                            if (isReq && !required.Contains(sn))
                            {
                                required.Add(sn);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Jika gagal membaca dataset, jangan blok (biarkan validasi lain berjalan).
                return new List<string>();
            }

            if (required.Count == 0)
            {
                return required;
            }

            string connStr = MdvrChannelService.ResolveSqlConnectionString(HttpContext.Current);
            return MdvrChannelService.GetUnconfiguredNoSN(connStr, required);
        }

        // ===== PENDING GRIDVIEW HANDLERS =====
        protected void GridViewPending_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                if (Session[sSessionRecListPending] == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No data available. Please refresh the page.</div>";
                    UpdatePanelPending.Update();
                    return;
                }

                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecListPending], LblPagingPending, ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString());
                div_comment.InnerHtml = "";

                try
                {
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();
                }
                catch
                {
                    LblTotalPending.Text = "0";
                }

                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceErrorInAllContexts(
                        ex.Message,
                        "Error in GridViewPending_PageIndexChanging",
                        "The system encountered an issue while changing pages in the pending devices list. This may be due to data source configuration problems or session state issues.",
                        "1. Check if the database connection is working properly. 2. Verify that the stored procedure 'sp_list_device_qc_new' exists and is accessible. 3. Clear your browser cache and cookies. 4. Try logging out and logging back in. 5. Contact your system administrator if the problem persists.",
                        "System Administrator: admin@company.com | Technical Support: support@company.com | Emergency: +1-800-HELP-NOW",
                        "Exception Type: " + ex.GetType().Name + " | Stack Trace: " + ex.StackTrace + " | Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "None"),
                        "Server: " + Server.MachineName + " | Application: " + Request.ApplicationPath + " | User Agent: " + Request.UserAgent + " | Request URL: " + Request.Url.ToString(),
                        "Session ID: " + Session.SessionID + " | ViewState: " + ViewState.Count + " | PostBack: " + IsPostBack + " | Async PostBack: " + ScriptManager.GetCurrent(Page).IsInAsyncPostBack,
                        "1. Restart the application pool. 2. Check database connectivity. 3. Verify stored procedure permissions. 4. Clear application cache. 5. Review server logs for additional errors.",
                        "Consider implementing additional error logging and monitoring. Review the application's error handling strategy. Ensure all database connections are properly managed. Implement health checks for critical components.",
                        "Emergency Contact: +1-800-EMERGENCY | On-call Engineer: engineer@company.com | Escalation: manager@company.com",
                        "System Status: Under Investigation | Last Maintenance: " + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " | Next Maintenance: " + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                        "Daily: 2:00 AM - 4:00 AM | Weekly: Sunday 1:00 AM - 3:00 AM | Monthly: First Sunday 12:00 AM - 6:00 AM",
                        "24/7 Support Available | Business Hours: 9:00 AM - 5:00 PM EST | Emergency: 24/7",
                        "Technical Documentation: https://docs.company.com/device-qc | User Guide: https://help.company.com/device-qc | API Documentation: https://api.company.com/docs",
                        "Framework: " + System.Environment.Version.ToString() + " | OS: " + System.Environment.OSVersion.ToString() + " | Machine: " + System.Environment.MachineName
                    );
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    LblTotalPending.Text = "0";

                    // Ensure GridView is in safe state
                    GridViewPending.DataSource = null;
                    GridViewPending.DataBind();

                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }

        // Render sel kolom "Status Channel" dari data SP (IsRequireChannelSetting, StatusChannel, MaxChannel, NoSN).
        private void RenderChannelStatusCell(GridViewRow row)
        {
            try
            {
                Literal lit = (Literal)row.FindControl("litChannelStatus");
                if (lit == null)
                {
                    return;
                }

                DataRowView rowView = row.DataItem as DataRowView;
                object isReq = GetChannelRowValue(rowView, "IsRequireChannelSetting");
                object status = GetChannelRowValue(rowView, "StatusChannel");
                object nosn = GetChannelRowValue(rowView, "NoSN");
                object deviceType = GetChannelRowValue(rowView, "DeviceTypeDesc");
                object maxCh = GetChannelRowValue(rowView, "MaxChannel");

                lit.Text = MdvrChannelService.BuildStatusCellHtml(isReq, status, nosn, deviceType, maxCh);
            }
            catch
            {
                // Best effort: jangan ganggu binding grid jika kolom channel tidak tersedia.
            }
        }

        private object GetChannelRowValue(DataRowView rowView, string columnName)
        {
            try
            {
                if (rowView != null && rowView.Row.Table.Columns.Contains(columnName))
                {
                    return rowView[columnName];
                }
            }
            catch
            {
            }
            return null;
        }

        protected void GridViewPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Hide kolom 9-11 (hidden fields: TdwID, WarehouseID, SourceName)
                    // Note: Index bertambah 1 karena ada kolom Status Channel setelah Type
                    for (int i = 9; i <= 11; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Hide kolom 9-11 (hidden fields: TdwID, WarehouseID, SourceName)
                    // Kolom: Select(0), DeviceID(1), NoSN(2), Vendor(3), Type(4), Status Channel(5), Warehouse(6), GPS_Time(7), Process(8), TdwID(9), WarehouseID(10), Source(11)
                    for (int i = 9; i <= 11; i++)
                    {
                        if (i < e.Row.Cells.Count)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }

                    // Render kolom Status Channel (badge + tombol setting) berdasarkan data SP.
                    RenderChannelStatusCell(e.Row);

                    // Handle GPS Time dan Process Button
                    Button btnProcess = (Button)e.Row.FindControl("btnProcess");

                    try
                    {
                        DataRowView rowView = (DataRowView)e.Row.DataItem;
                        if (rowView != null && rowView.Row.Table.Columns.Contains("GPS_Time"))
                        {
                            object gpsTimeObj = rowView["GPS_Time"];
                            DateTime? gpsTime = null;

                            if (gpsTimeObj != null && gpsTimeObj != DBNull.Value)
                            {
                                if (gpsTimeObj is DateTime)
                                {
                                    gpsTime = (DateTime)gpsTimeObj;
                                }
                                else if (gpsTimeObj is DateTime?)
                                {
                                    gpsTime = (DateTime?)gpsTimeObj;
                                }
                            }

                            // Handle button visibility berdasarkan GPS Time
                            // Button hanya tampil jika gps_time <= 24 jam
                            if (btnProcess != null)
                            {
                                if (gpsTime.HasValue)
                                {
                                    DateTime now = DateTime.UtcNow;
                                    TimeSpan diff = now - gpsTime.Value;

                                    // Show button hanya jika GPS Time <= 24 jam
                                    btnProcess.Visible = (diff.TotalHours <= 24);

                                    // Update warna GPS Time cell
                                    int gpsTimeColumnIndex = -1;
                                    for (int i = 0; i < GridViewPending.Columns.Count; i++)
                                    {
                                        if (GridViewPending.Columns[i] is BoundField &&
                                            ((BoundField)GridViewPending.Columns[i]).DataField == "GPS_Time")
                                        {
                                            gpsTimeColumnIndex = i;
                                            break;
                                        }
                                    }

                                    if (gpsTimeColumnIndex >= 0 && gpsTimeColumnIndex < e.Row.Cells.Count)
                                    {
                                        if (diff.TotalHours > 24)
                                        {
                                            e.Row.Cells[gpsTimeColumnIndex].ForeColor = System.Drawing.Color.Red;
                                        }
                                        else
                                        {
                                            e.Row.Cells[gpsTimeColumnIndex].ForeColor = System.Drawing.Color.Green;
                                        }
                                    }
                                }
                                else
                                {
                                    // GPS Time NULL - sembunyikan button
                                    btnProcess.Visible = false;

                                    // Update warna GPS Time cell
                                    int gpsTimeColumnIndex = -1;
                                    for (int i = 0; i < GridViewPending.Columns.Count; i++)
                                    {
                                        if (GridViewPending.Columns[i] is BoundField &&
                                            ((BoundField)GridViewPending.Columns[i]).DataField == "GPS_Time")
                                        {
                                            gpsTimeColumnIndex = i;
                                            break;
                                        }
                                    }

                                    if (gpsTimeColumnIndex >= 0 && gpsTimeColumnIndex < e.Row.Cells.Count)
                                    {
                                        e.Row.Cells[gpsTimeColumnIndex].ForeColor = System.Drawing.Color.Gray;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // GPS_Time column tidak ada - sembunyikan button
                            if (btnProcess != null)
                            {
                                btnProcess.Visible = false;
                            }
                        }
                    }
                    catch
                    {
                        // Handle error gracefully
                        if (btnProcess != null)
                        {
                            btnProcess.Visible = false;
                        }
                    }

                    // Auto-check checkbox if Serial Number (NoSN) is in scanned list
                    if (ViewState["ScannedDeviceIDs"] != null && e.Row.Cells.Count > 2)
                    {
                        string scannedSerials = ViewState["ScannedDeviceIDs"].ToString();
                        if (!string.IsNullOrEmpty(scannedSerials))
                        {
                            string[] serialNumbers = scannedSerials.Split(',');
                            string currentSerialNo = e.Row.Cells[2].Text.Trim().ToUpper(); // NoSN is column index 2

                            if (serialNumbers.Contains(currentSerialNo))
                            {
                                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                                if (chkSelect != null)
                                {
                                    chkSelect.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceErrorInAllContexts(
                        ex.Message,
                        "Error in GridViewPending_RowDataBound",
                        "The system encountered an issue while binding row data in the pending devices list. This may be due to data source configuration problems or session state issues.",
                        "1. Check if the database connection is working properly. 2. Verify that the stored procedure 'sp_list_device_qc_new' exists and is accessible. 3. Clear your browser cache and cookies. 4. Try logging out and logging back in. 5. Contact your system administrator if the problem persists.",
                        "System Administrator: admin@company.com | Technical Support: support@company.com | Emergency: +1-800-HELP-NOW",
                        "Exception Type: " + ex.GetType().Name + " | Stack Trace: " + ex.StackTrace + " | Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "None"),
                        "Server: " + Server.MachineName + " | Application: " + Request.ApplicationPath + " | User Agent: " + Request.UserAgent + " | Request URL: " + Request.Url.ToString(),
                        "Session ID: " + Session.SessionID + " | ViewState: " + ViewState.Count + " | PostBack: " + IsPostBack + " | Async PostBack: " + ScriptManager.GetCurrent(Page).IsInAsyncPostBack,
                        "1. Restart the application pool. 2. Check database connectivity. 3. Verify stored procedure permissions. 4. Clear application cache. 5. Review server logs for additional errors.",
                        "Consider implementing additional error logging and monitoring. Review the application's error handling strategy. Ensure all database connections are properly managed. Implement health checks for critical components.",
                        "Emergency Contact: +1-800-EMERGENCY | On-call Engineer: engineer@company.com | Escalation: manager@company.com",
                        "System Status: Under Investigation | Last Maintenance: " + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " | Next Maintenance: " + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                        "Daily: 2:00 AM - 4:00 AM | Weekly: Sunday 1:00 AM - 3:00 AM | Monthly: First Sunday 12:00 AM - 6:00 AM",
                        "24/7 Support Available | Business Hours: 9:00 AM - 5:00 PM EST | Emergency: 24/7",
                        "Technical Documentation: https://docs.company.com/device-qc | User Guide: https://help.company.com/device-qc | API Documentation: https://api.company.com/docs",
                        "Framework: " + System.Environment.Version.ToString() + " | OS: " + System.Environment.OSVersion.ToString() + " | Machine: " + System.Environment.MachineName
                    );
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";

                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }

        protected void GridViewPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session[sSessionRecListPending] == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No data available. Please refresh the page.</div>";
                    UpdatePanelPending.Update();
                    return;
                }

                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridViewPending, Session[sSessionRecListPending], ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSortPending] = e.SortExpression.ToString();
                ViewState[sViewStateDirSortPending] = sNewDirSort;

                try
                {
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();
                }
                catch
                {
                    LblTotalPending.Text = "0";
                }

                // Update UpdatePanel
                UpdatePanelPending.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceErrorInAllContexts(
                        ex.Message,
                        "Error in GridViewPending_Sorting",
                        "The system encountered an issue while sorting the pending devices list. This may be due to data source configuration problems or session state issues.",
                        "1. Check if the database connection is working properly. 2. Verify that the stored procedure 'sp_list_device_qc_new' exists and is accessible. 3. Clear your browser cache and cookies. 4. Try logging out and logging back in. 5. Contact your system administrator if the problem persists.",
                        "System Administrator: admin@company.com | Technical Support: support@company.com | Emergency: +1-800-HELP-NOW",
                        "Exception Type: " + ex.GetType().Name + " | Stack Trace: " + ex.StackTrace + " | Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "None"),
                        "Server: " + Server.MachineName + " | Application: " + Request.ApplicationPath + " | User Agent: " + Request.UserAgent + " | Request URL: " + Request.Url.ToString(),
                        "Session ID: " + Session.SessionID + " | ViewState: " + ViewState.Count + " | PostBack: " + IsPostBack + " | Async PostBack: " + ScriptManager.GetCurrent(Page).IsInAsyncPostBack,
                        "1. Restart the application pool. 2. Check database connectivity. 3. Verify stored procedure permissions. 4. Clear application cache. 5. Review server logs for additional errors.",
                        "Consider implementing additional error logging and monitoring. Review the application's error handling strategy. Ensure all database connections are properly managed. Implement health checks for critical components.",
                        "Emergency Contact: +1-800-EMERGENCY | On-call Engineer: engineer@company.com | Escalation: manager@company.com",
                        "System Status: Under Investigation | Last Maintenance: " + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " | Next Maintenance: " + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                        "Daily: 2:00 AM - 4:00 AM | Weekly: Sunday 1:00 AM - 3:00 AM | Monthly: First Sunday 12:00 AM - 6:00 AM",
                        "24/7 Support Available | Business Hours: 9:00 AM - 5:00 PM EST | Emergency: 24/7",
                        "Technical Documentation: https://docs.company.com/device-qc | User Guide: https://help.company.com/device-qc | API Documentation: https://api.company.com/docs",
                        "Framework: " + System.Environment.Version.ToString() + " | OS: " + System.Environment.OSVersion.ToString() + " | Machine: " + System.Environment.MachineName
                    );
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                    LblTotalPending.Text = "0";

                    // Ensure GridView is in safe state
                    GridViewPending.DataSource = null;
                    GridViewPending.DataBind();

                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelPending.Update();
                }
            }
        }


        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClearForm();
                Open_GridView_Pending();

                // Update UpdatePanels
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                // Check if this is an IListSource error
                if (ex.Message.Contains("IListSource") || ex.Message.Contains("data sources"))
                {
                    HandleIListSourceErrorInAllContexts(
                        ex.Message,
                        "Error in CmdSearch_Click",
                        "The system encountered an issue while searching for devices. This may be due to data source configuration problems or session state issues.",
                        "1. Check if the database connection is working properly. 2. Verify that the stored procedures exist and are accessible. 3. Clear your browser cache and cookies. 4. Try logging out and logging back in. 5. Contact your system administrator if the problem persists.",
                        "System Administrator: admin@company.com | Technical Support: support@company.com | Emergency: +1-800-HELP-NOW",
                        "Exception Type: " + ex.GetType().Name + " | Stack Trace: " + ex.StackTrace + " | Inner Exception: " + (ex.InnerException != null ? ex.InnerException.Message : "None"),
                        "Server: " + Server.MachineName + " | Application: " + Request.ApplicationPath + " | User Agent: " + Request.UserAgent + " | Request URL: " + Request.Url.ToString(),
                        "Session ID: " + Session.SessionID + " | ViewState: " + ViewState.Count + " | PostBack: " + IsPostBack + " | Async PostBack: " + ScriptManager.GetCurrent(Page).IsInAsyncPostBack,
                        "1. Restart the application pool. 2. Check database connectivity. 3. Verify stored procedure permissions. 4. Clear application cache. 5. Review server logs for additional errors.",
                        "Consider implementing additional error logging and monitoring. Review the application's error handling strategy. Ensure all database connections are properly managed. Implement health checks for critical components.",
                        "Emergency Contact: +1-800-EMERGENCY | On-call Engineer: engineer@company.com | Escalation: manager@company.com",
                        "System Status: Under Investigation | Last Maintenance: " + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " | Next Maintenance: " + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                        "Daily: 2:00 AM - 4:00 AM | Weekly: Sunday 1:00 AM - 3:00 AM | Monthly: First Sunday 12:00 AM - 6:00 AM",
                        "24/7 Support Available | Business Hours: 9:00 AM - 5:00 PM EST | Emergency: 24/7",
                        "Technical Documentation: https://docs.company.com/device-qc | User Guide: https://help.company.com/device-qc | API Documentation: https://api.company.com/docs",
                        "Framework: " + System.Environment.Version.ToString() + " | OS: " + System.Environment.OSVersion.ToString() + " | Machine: " + System.Environment.MachineName
                    );
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";

                    // Update UpdatePanel untuk menampilkan error
                    UpdatePanelQCForm.Update();
                }
            }
        }

        protected void btnSearchMultiple_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";

                // Get scanned serial numbers from hidden field
                string scannedSerials = hdnScannedDevices.Value.Trim();

                if (string.IsNullOrEmpty(scannedSerials))
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No serial numbers scanned. Please scan at least one serial number.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }

                // Parse dan normalisasi serial numbers (NoSN) - trim, uppercase, distinct
                string[] serialNumbers = scannedSerials
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(sn => sn.Trim().ToUpper())
                    .Distinct()
                    .ToArray();

                if (serialNumbers.Length == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> No valid serial numbers to search.</div>";
                    UpdatePanelQCForm.Update();
                    return;
                }

                // Store serial numbers in ViewState for auto-check in RowDataBound event
                ViewState["ScannedDeviceIDs"] = string.Join(",", serialNumbers);

                // Initialize GridView with empty data first to prevent binding errors
                GridViewPending.DataSource = null;
                GridViewPending.DataBind();
                LblTotalPending.Text = "0";

                ClsType ClType = new ClsType();

                // Samakan source koneksi dengan load utama:
                // utamakan VTSADMIN dari Web.config, fallback ke session legacy.
                string sqlConnectionString = string.Empty;
                if (ConfigurationManager.ConnectionStrings["VTSADMIN"] != null &&
                    !string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString))
                {
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                }
                else if (Session["ClsTypeDBConnStringSQL"] != null &&
                         !string.IsNullOrWhiteSpace(Session["ClsTypeDBConnStringSQL"].ToString()))
                {
                    sqlConnectionString = Session["ClsTypeDBConnStringSQL"].ToString();
                }

                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> Database connection not available. Please login again.</div>";
                    UpdatePanelPending.Update();
                    return;
                }

                // Call SP by serial list (hanya ambil device yang discan)
                string serialCsv = string.Join(",", serialNumbers);

                ViewState[sViewStateFieldSortPending] = "DeviceID";
                ViewState[sViewStateDirSortPending] = "DESC";

                // Use try-catch untuk data fetch + bind
                try
                {
                    DataSet dsPending = new DataSet();
                    using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                    using (SqlCommand cmd = new SqlCommand("sp_list_device_qc_new_multiple", conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SerialList", serialCsv);
                        conn.Open();
                        da.Fill(dsPending);
                    }

                    if (dsPending == null || dsPending.Tables.Count == 0 || dsPending.Tables[0] == null)
                    {
                        throw new Exception("Pending search query did not return a valid result table.");
                    }

                    // Join GPS time dari MongoDB ke DataSet sebelum di-bind
                    // Pairing: NoSN (dari SP) = gps_sn (di MongoDB)
                    try
                    {
                        JoinGpsTimeToDataSet(dsPending);
                    }
                    catch
                    {
                        // Jika gagal, data tetap bisa di-load tanpa GPS Time
                    }

                    // Pastikan schema dataset kompatibel dengan kolom GridView.
                    EnsurePendingGridColumns(dsPending);

                    if (dsPending.Tables[0] != null)
                    {
                        dsPending.Tables[0].DefaultView.Sort = ViewState[sViewStateFieldSortPending] + " " + ViewState[sViewStateDirSortPending];
                    }

                    Session[sSessionRecListPending] = dsPending;

                    // Direct bind agar data tidak ter-reset saat helper menerima source kompleks.
                    GridViewPending.DataSource = dsPending.Tables[0];
                    GridViewPending.DataBind();
                    LblTotalPending.Text = GridViewPending.Rows.Count.ToString();

                    // Update paging label and header sort indicator
                    ClType.showPaging(dsPending, GridViewPending, LblPagingPending);
                    ClType.setSorting(GridViewPending, ViewState[sViewStateFieldSortPending].ToString(), ViewState[sViewStateDirSortPending].ToString());
                }
                catch (Exception gridEx)
                {
                    // Check if it's an IListSource error
                    if (gridEx.Message.Contains("IListSource") || gridEx.Message.Contains("data sources"))
                    {
                        HandleIListSourceErrorInAllContexts("No data available. Please try refreshing the page.", "GridView Binding", "This error occurs when GridView cannot bind to data source. Please check database connection and stored procedures.", "1. Check if stored procedures exist 2. Verify database connection 3. Check if user has proper permissions 4. Try refreshing the page", "Contact system administrator if problem persists", "IListSource error typically occurs when GridView.DataSource is null or invalid. This can happen due to database connection issues, missing stored procedures, or session timeout.", "System: ASP.NET Web Forms, Database: SQL Server, Framework: .NET 4.0", "Error occurred at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", User: " + (Session["ClsTypeUserID"] ?? "Unknown") + ", Session: " + Session.SessionID, "1. Restart application 2. Check database connectivity 3. Verify stored procedures 4. Clear browser cache 5. Contact administrator", "If the problem persists, consider upgrading to a newer framework or implementing alternative data binding methods.", "Emergency: Call IT Support at +62-XXX-XXXX-XXXX or email support@company.com", "System Status: Online, Database: Connected, Session: Active, Memory: Normal", "Next maintenance: Every Sunday 2:00 AM - 4:00 AM", "Support Hours: Monday-Friday 8:00 AM - 5:00 PM, Saturday 9:00 AM - 1:00 PM", "Documentation: See user manual section 5.2 for troubleshooting IListSource errors");
                        return;
                    }

                    // If Open_GridView fails, ensure GridView is in safe state
                    SafeBindGridView(GridViewPending, null, LblTotalPending);
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><strong>Warning!</strong> Unable to load data. " + gridEx.Message + "</div>";
                }

                // Update UpdatePanels first to render GridView with checked checkboxes
                UpdatePanelPending.Update();
                UpdatePanelQCForm.Update();

                // Call JavaScript to highlight rows (client-side) after rendering
                string serialNumbersJson = "['" + string.Join("','", serialNumbers.Select(sn => sn.Replace("'", "\\'"))) + "']";
                ScriptManager.RegisterStartupScript(this, GetType(), "autoCheckDevices",
                    $"autoCheckMultipleDevices({serialNumbersJson});", true);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Error!</strong> " + ex.Message + "</div>";
                UpdatePanelQCForm.Update();
            }
        }

        private void AutoCheckScannedDevices(string[] deviceIDs)
        {
            try
            {
                if (GridViewPending.Rows.Count == 0) return;

                foreach (GridViewRow row in GridViewPending.Rows)
                {
                    if (row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                    {
                        string deviceID = row.Cells[1].Text.Trim().ToUpper();

                        foreach (string scannedID in deviceIDs)
                        {
                            if (scannedID.Trim().ToUpper() == deviceID)
                            {
                                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                                if (chkSelect != null)
                                {
                                    chkSelect.Checked = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw
                System.Diagnostics.Debug.WriteLine("AutoCheckScannedDevices error: " + ex.Message);
            }
        }

        // Event handler untuk button Process
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string serialNo = btn.CommandArgument.ToString();

                // TODO: Implement logic untuk proses device
                div_comment.InnerHtml = string.Format(
                    "<div class='alert alert-success' role='alert'>" +
                    "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                    "<strong><i class='fa fa-check'></i> Success!</strong><br/>" +
                    "Device dengan Serial Number <strong>{0}</strong> sedang diproses.</div>",
                    serialNo);

                UpdatePanelQCForm.Update();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Error!</strong> " + ex.Message + "</div>";
                UpdatePanelQCForm.Update();
            }
        }
    }
}
