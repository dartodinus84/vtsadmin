using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace vtsadm
{
    public partial class dashboard_notif_alert_detail_item : System.Web.UI.Page
    {
        private const int MaxEntries = 100;
        private const string UnknownLabel = "(Tidak diketahui)";

        private static readonly Dictionary<string, CollectionFieldMapping> FieldMappings = new Dictionary<string, CollectionFieldMapping>(StringComparer.OrdinalIgnoreCase)
        {
            { "NotifDO", new CollectionFieldMapping("tipe_data", "tgl_event") },
            { "NotifGeofence", new CollectionFieldMapping("direction", "tgl_event") },
            { "NotifMaintenance", new CollectionFieldMapping("judul", "tgl_notif") },
            { "NotifOperation", new CollectionFieldMapping("tipe_notif", "start_time") },
            { "NotifSensor", new CollectionFieldMapping("sensor_nm", "start_time") },
            { "AlarmMobil", new CollectionFieldMapping("alarm_nm", "tgl_event") }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            litStatus.Text = string.Empty;
            gvEntries.DataSource = null;
            gvEntries.DataBind();

            var collectionName = Request.QueryString["collection"];
            var customerName = Request.QueryString["company"];
            if (customerName != null)
            {
                customerName = customerName.Trim();
            }

            if (string.IsNullOrWhiteSpace(collectionName) || !FieldMappings.ContainsKey(collectionName))
            {
                litStatus.Text = "<div class='alert alert-warning'>Parameter koleksi tidak valid.</div>";
                lblTitle.Text = "Detail Notifikasi";
                lblCustomer.Text = "-";
                ConfigureNavigationLinks(null);
                return;
            }

            ConfigureNavigationLinks(collectionName);

            if (string.IsNullOrWhiteSpace(customerName))
            {
                litStatus.Text = "<div class='alert alert-warning'>Parameter customer tidak ditemukan.</div>";
                lblTitle.Text = "Detail Notifikasi - " + GetDisplayName(collectionName);
                lblCustomer.Text = "-";
                return;
            }

            lblTitle.Text = "Detail Notifikasi - " + GetDisplayName(collectionName);
            lblCustomer.Text = string.IsNullOrWhiteSpace(customerName) ? "-" : customerName;

            try
            {
                var entries = GetEntries(collectionName, customerName);
                gvEntries.DataSource = entries;
                gvEntries.DataBind();

                if (entries.Count >= MaxEntries)
                {
                    litStatus.Text = "<div class='alert alert-info'>Menampilkan " + MaxEntries + " data terbaru.</div>";
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = "<div class='alert alert-danger'>" + Server.HtmlEncode(ex.Message) + "</div>";
            }
        }

        private IList<NotificationEntry> GetEntries(string collectionName, string customerName)
        {
            var mapping = FieldMappings[collectionName];
            var database = GetDatabase();
            var collection = database.GetCollection<BsonDocument>(collectionName);

            var matchFilter = BuildMatchFilter(mapping, customerName);

            // Group by GPS SN and No. Polisi, then count
            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "gps_sn", new BsonDocument("$ifNull", new BsonArray { "$gps_sn", "" }) },
                        { "nopol", new BsonDocument("$ifNull", new BsonArray { "$nopol", "" }) }
                    }
                },
                { "total", new BsonDocument("$sum", 1) }
            });

            var projectStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "gps_sn", "$_id.gps_sn" },
                { "nopol", "$_id.nopol" },
                { "total", "$total" }
            });

            var sortStage = new BsonDocument("$sort", new BsonDocument
            {
                { "total", -1 },
                { "gps_sn", 1 },
                { "nopol", 1 }
            });

            var limitStage = new BsonDocument("$limit", MaxEntries);

            var pipeline = new List<BsonDocument>();
            
            // Add match stage for filter
            if (matchFilter != null)
            {
                pipeline.Add(new BsonDocument("$match", matchFilter));
            }
            
            pipeline.Add(groupStage);
            pipeline.Add(projectStage);
            pipeline.Add(sortStage);
            pipeline.Add(limitStage);

            var pipelineDefinition = PipelineDefinition<BsonDocument, BsonDocument>.Create(pipeline);
            var documents = collection.Aggregate(pipelineDefinition).ToList();

            var results = new List<NotificationEntry>(documents.Count);
            for (int i = 0; i < documents.Count; i++)
            {
                var document = documents[i];

                string gpsSn = GetStringValue(document, "gps_sn");
                string nopol = GetStringValue(document, "nopol");
                var totalValue = document.GetValue("total", 0);
                int total = totalValue.IsNumeric ? totalValue.ToInt32() : 0;

                results.Add(new NotificationEntry
                {
                    GpsSn = string.IsNullOrWhiteSpace(gpsSn) ? "-" : gpsSn,
                    Nopol = string.IsNullOrWhiteSpace(nopol) ? "-" : nopol,
                    Total = total
                });
            }

            return results;
        }

        private static BsonDocument BuildMatchFilter(CollectionFieldMapping mapping, string customerName)
        {
            var filters = new List<BsonDocument>();

            if (string.Equals(customerName, UnknownLabel, StringComparison.OrdinalIgnoreCase))
            {
                filters.Add(new BsonDocument("$or", new BsonArray
                {
                    new BsonDocument("company_nm", new BsonDocument("$exists", false)),
                    new BsonDocument("company_nm", BsonNull.Value),
                    new BsonDocument("company_nm", string.Empty)
                }));
            }
            else
            {
                filters.Add(new BsonDocument("company_nm", customerName));
            }

            if (!string.IsNullOrWhiteSpace(mapping.DateField))
            {
                filters.Add(new BsonDocument(mapping.DateField, new BsonDocument
                {
                    { "$gte", GetStartOfTodayUtc() },
                    { "$lte", GetEndOfTodayUtc() }
                }));
            }

            if (filters.Count == 0)
            {
                return null;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return new BsonDocument("$and", new BsonArray(filters));
        }

        private static string GetStringValue(BsonDocument document, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return null;
            }

            BsonValue value;
            if (document.TryGetValue(fieldName, out value) && !value.IsBsonNull)
            {
                return value.ToString();
            }

            return null;
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

        private static DateTime GetStartOfTodayUtc()
        {
            var now = DateTime.Now;
            var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);
            return startOfDay.ToUniversalTime();
        }

        private static DateTime GetEndOfTodayUtc()
        {
            var now = DateTime.Now;
            var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            return endOfDay.ToUniversalTime();
        }

        private static string GetDisplayName(string collectionName)
        {
            switch (collectionName)
            {
                case "NotifDO":
                    return "Notif Delivery Order";
                case "NotifGeofence":
                    return "Notif Geofence";
                case "NotifMaintenance":
                    return "Notif Maintenance";
                case "NotifOperation":
                    return "Notif Operation";
                case "NotifSensor":
                    return "Notif Sensor";
                case "AlarmMobil":
                    return "Alarm Mobil";
                default:
                    return collectionName;
            }
        }

        private void ConfigureNavigationLinks(string collectionName)
        {
            var targetUrl = string.IsNullOrWhiteSpace(collectionName)
                ? "~/dashboard_notif_alert_detail.aspx"
                : "~/dashboard_notif_alert_detail.aspx?collection=" + Server.UrlEncode(collectionName);

            lnkBack.NavigateUrl = targetUrl;
            lnkBack.Visible = true;

            lnkBreadcrumbParent.HRef = ResolveUrl(targetUrl);
        }

        private sealed class CollectionFieldMapping
        {
            public CollectionFieldMapping(string notificationField, string dateField)
            {
                NotificationField = notificationField;
                DateField = dateField;
            }

            public string NotificationField { get; }

            public string DateField { get; }
        }

        private sealed class NotificationEntry
        {
            public string GpsSn { get; set; }

            public string Nopol { get; set; }

            public int Total { get; set; }
        }
    }
}


