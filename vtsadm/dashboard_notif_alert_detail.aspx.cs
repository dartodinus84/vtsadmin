using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace vtsadm
{
    public partial class dashboard_notif_alert_detail : System.Web.UI.Page
    {
        private const int TopLimit = 10;

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
                LoadData();
            }
        }

        private void LoadData()
        {
            litStatus.Text = string.Empty;
            gvDetail.DataSource = null;
            gvDetail.DataBind();

            var collectionName = Request.QueryString["collection"];
            if (string.IsNullOrWhiteSpace(collectionName) || !IsValidCollection(collectionName))
            {
                litStatus.Text = "<div class='alert alert-warning'>Parameter koleksi tidak valid.</div>";
                lblTitle.Text = "Top 10 Customer";
                return;
            }

            try
            {
                var topCustomers = GetTopCustomers(collectionName);
                gvDetail.DataSource = topCustomers;
                gvDetail.DataBind();

                lblTitle.Text = "Top 10 Customer - " + GetDisplayName(collectionName);
            }
            catch (Exception ex)
            {
                litStatus.Text = "<div class='alert alert-danger'>" + Server.HtmlEncode(ex.Message) + "</div>";
                lblTitle.Text = "Top 10 Customer";
            }
        }

        private static bool IsValidCollection(string collectionName)
        {
            for (int i = 0; i < Collections.Length; i++)
            {
                if (string.Equals(Collections[i], collectionName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private IList<TopCustomerRow> GetTopCustomers(string collectionName)
        {
            var database = GetDatabase();
            var collection = database.GetCollection<BsonDocument>(collectionName);

            var todayFilter = BuildTodayDateFilter(collectionName);

            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "customer", new BsonDocument("$ifNull", new BsonArray { "$company_nm", "(Tidak diketahui)" }) }
                    }
                },
                { "total", new BsonDocument("$sum", 1) }
            });

            var projectStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "customer", "$_id.customer" },
                { "total", "$total" }
            });

            var sortStage = new BsonDocument("$sort", new BsonDocument
            {
                { "total", -1 },
                { "customer", 1 }
            });

            var limitStage = new BsonDocument("$limit", TopLimit);

            var pipeline = new List<BsonDocument>();
            if (todayFilter != null)
            {
                var matchStage = new BsonDocument("$match", todayFilter);
                pipeline.Add(matchStage);
            }
            pipeline.Add(groupStage);
            pipeline.Add(projectStage);
            pipeline.Add(sortStage);
            pipeline.Add(limitStage);
            
            var pipelineDefinition = PipelineDefinition<BsonDocument, BsonDocument>.Create(pipeline);
            var documents = collection.Aggregate(pipelineDefinition).ToList();

            var results = new List<TopCustomerRow>(documents.Count);
            for (int i = 0; i < documents.Count; i++)
            {
                var document = documents[i];

                string customerName = null;
                BsonValue customerValue;
                if (document.TryGetValue("customer", out customerValue) && !customerValue.IsBsonNull)
                {
                    customerName = customerValue.ToString();
                }

                if (string.IsNullOrWhiteSpace(customerName))
                {
                    customerName = "(Tidak diketahui)";
                }

                var totalValue = document.GetValue("total", 0);
                var total = totalValue.IsNumeric ? totalValue.ToInt32() : 0;

                results.Add(new TopCustomerRow
                {
                    CustomerName = customerName,
                    Total = total
                });
            }

            return results;
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

        private sealed class TopCustomerRow
        {
            public string CustomerName { get; set; }

            public int Total { get; set; }
        }

        protected string GetEntryUrl(object customerName)
        {
            var collectionName = Request.QueryString["collection"];
            if (string.IsNullOrWhiteSpace(collectionName) || !IsValidCollection(collectionName))
            {
                return "#";
            }

            var customer = Convert.ToString(customerName) ?? string.Empty;
            return "~/dashboard_notif_alert_detail_item.aspx?collection="
                + Server.UrlEncode(collectionName)
                + "&company="
                + Server.UrlEncode(customer);
        }
    }
}

