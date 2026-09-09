using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace vtsadm
{
    public partial class dashboard_notif_alert : System.Web.UI.Page
    {
        private static readonly string[] Collections =
        {
            "NotifDO",
            "NotifGeofence",
            "NotifMaintenance",
            "NotifOperation",
            "NotifSensor",
            "AlarmMobil"
        };

        private static readonly Dictionary<string, string> CollectionDisplayNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "NotifDO", "Notif Delivery Order" },
            { "NotifGeofence", "Notif Geofence" },
            { "NotifMaintenance", "Notif Maintenance" },
            { "NotifOperation", "Notif Operation" },
            { "NotifSensor", "Notif Sensor" },
            { "AlarmMobil", "Alarm Mobil" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            litStatus.Text = string.Empty;

            try
            {
                var data = GetSummaryData();
                gvNotificationSummary.DataSource = data;
                gvNotificationSummary.DataBind();

                rptSummary.DataSource = data;
                rptSummary.DataBind();
            }
            catch (Exception ex)
            {
                gvNotificationSummary.DataSource = new List<NotificationSummary>();
                gvNotificationSummary.DataBind();

                rptSummary.DataSource = new List<NotificationSummary>();
                rptSummary.DataBind();

                litStatus.Text = "<div class='alert alert-danger'>" + Server.HtmlEncode(ex.Message) + "</div>";
            }
        }

        private IList<NotificationSummary> GetSummaryData()
        {
            var database = GetDatabase();

            var results = new List<NotificationSummary>(Collections.Length);
            for (int i = 0; i < Collections.Length; i++)
            {
                var collectionName = Collections[i];
                var collection = database.GetCollection<BsonDocument>(collectionName);
                var total = CountDocuments(collection, collectionName);

                results.Add(new NotificationSummary
                {
                    CollectionName = collectionName,
                    DisplayName = GetDisplayName(collectionName),
                    Total = total
                });
            }

            return results;
        }

        private static int CountDocuments(IMongoCollection<BsonDocument> collection, string collectionName)
        {
            var todayFilter = BuildTodayDateFilter(collectionName);
            
            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", 1 },
                { "total", new BsonDocument("$sum", 1) }
            });

            var projectStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "total", "$total" }
            });

            var pipeline = new List<BsonDocument>();
            if (todayFilter != null)
            {
                var matchStage = new BsonDocument("$match", todayFilter);
                pipeline.Add(matchStage);
            }
            pipeline.Add(groupStage);
            pipeline.Add(projectStage);
            
            var pipelineDefinition = PipelineDefinition<BsonDocument, BsonDocument>.Create(pipeline);
            var document = collection.Aggregate(pipelineDefinition).FirstOrDefault();

            if (document == null)
            {
                return 0;
            }

            var totalValue = document.GetValue("total", 0);
            return totalValue.IsNumeric ? totalValue.ToInt32() : 0;
        }

        private static string GetDisplayName(string collectionName)
        {
            string displayName;
            if (!CollectionDisplayNames.TryGetValue(collectionName, out displayName))
            {
                displayName = collectionName;
            }

            return displayName;
        }

        private static IMongoDatabase GetDatabase()
        {
            var mongoConnection = ConfigurationManager.ConnectionStrings["MongoGPSDATA"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(mongoConnection))
            {
                throw new InvalidOperationException("Connection string 'MongoGPSDATA' tidak ditemukan di Web.config.");
            }

            var client = new MongoClient(mongoConnection);
            return client.GetDatabase("GPSData");
        }

        private static BsonDocument BuildTodayDateFilter(string collectionName)
        {
            var now = DateTime.Now;
            var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            // Convert to UTC for MongoDB ISODate
            var startOfDayUtc = startOfDay.ToUniversalTime();
            var endOfDayUtc = endOfDay.ToUniversalTime();

            string dateField = GetDateFieldForCollection(collectionName);
            if (string.IsNullOrWhiteSpace(dateField))
            {
                return null;
            }

            return new BsonDocument
            {
                { dateField, new BsonDocument
                    {
                        { "$gte", new BsonDateTime(startOfDayUtc) },
                        { "$lte", new BsonDateTime(endOfDayUtc) }
                    }
                }
            };
        }

        private static string GetDateFieldForCollection(string collectionName)
        {
            switch (collectionName)
            {
                case "NotifDO":
                case "NotifGeofence":
                case "AlarmMobil":
                    return "tgl_event";
                case "NotifMaintenance":
                    return "tgl_notif";
                case "NotifOperation":
                case "NotifSensor":
                    return "start_time";
                default:
                    return null;
            }
        }

        private sealed class NotificationSummary
        {
            public string CollectionName { get; set; }

            public string DisplayName { get; set; }

            public int Total { get; set; }
        }

        protected string GetDetailUrl(object collectionName)
        {
            var name = Convert.ToString(collectionName);
            if (string.IsNullOrWhiteSpace(name))
            {
                return "#";
            }

            return "~/dashboard_notif_alert_detail.aspx?collection=" + Server.UrlEncode(name);
        }
    }
}

