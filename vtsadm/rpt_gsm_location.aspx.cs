using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class rpt_gsm_location : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecRptGsmRSFieldSort";
        string sViewStateDirSort = "RecRptGsmRSDirSort";
        string sSessionRecList = "RecRptGsmRS";
        private static string MongoGPSA = ConfigurationManager.ConnectionStrings["MongoGPSA"].ToString();

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_rpt_gsm_rs '','',''";
                ViewState[sViewStateFieldSort] = "GsmID";
                ViewState[sViewStateDirSort] = "ASC";
                
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNURPTGSMPOST"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                //((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
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

                //txtSearch.Text = "";
                ClType.Open_Combos(CmbGeofenceField, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_geofence_tipe");
                CmbGeofenceField.SelectedValue = "[Select]";
            }
            catch (Exception ex)
            {

            }
        }

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        [WebMethod]
        public static string getdatagsm(string kota, string radius, string geo_type, string lon, string lat)
        {
            string sOut = "";
           
            List<DataGsm> dJson = new List<DataGsm>();
            try
            {
                var pageData = new rpt_gsm_location();
                Recordset Rec = new Recordset();
                string strSQL = "sp_query_gsm_prepaid '"+ kota+ "', " + geo_type.ToString() + ", " + radius.ToString() + ", " + lon + ", " + lat + " ";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataGsm
                        {
                            gps_sn = Rec.Fields("gps_sn"),
                            car_plate = Rec.Fields("car_plate"),
                            customer_name = Rec.Fields("customer_name"),
                            gsm_no = Rec.Fields("gsm_no"),
                            status_gsm = Rec.Fields("status_gsm"),
                            lon = Rec.Fields("lon"),
                            lat = Rec.Fields("lat"),
                            addr = Rec.Fields("addr"),
                            kota = Rec.Fields("kota"),
                            gps_time = Rec.Fields("gps_time"),
                            driver_nm = Rec.Fields("driver_nm"),
                            phone = Rec.Fields("phone"),
                            gsm_source = Rec.Fields("gsm_source"),
                            marketing_name = Rec.Fields("marketing_name"),
                            acc = Rec.Fields("acc"),
                            speed = Rec.Fields("speed")

                        });
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }
            return JsonConvert.SerializeObject(new { data = dJson });
        }

        [WebMethod]
        public static string getdatagsm2(string kota, string radius, string geo_type, string lon, string lat)
        {
            string sOut = "";

            //List<DataGsm> dJson = new List<DataGsm>();
            var client = new MongoClient(MongoGPSA);
            var database = client.GetDatabase("GPSData");
            var filter = Builders<DataGsm>.Filter.Empty;
            var collection = database.GetCollection<DataGsm2>("last_position");
            return collection.ToString();
        }
        public class DataGsm
        {
            public string gps_sn { get; set; }
            public string car_plate { get; set; }
            public string customer_name { get; set; }
            public string gsm_no { get; set; }
            public string status_gsm { get; set; }
            public string lon { get; set; }
            public string lat { get; set; }
            public string addr { get; set; }
            public string kota { get; set; }
            public string gps_time { get; set; }
            public string driver_nm { get; set; }
            public string phone { get; set; }
            public string gsm_source { get; set; }
            public string marketing_name { get; set; }
            public string acc { get; set; }
            public string speed { get; set; }

        }

        public class DataGsm2
        {
            public string gps_sn { get; set; }
            public string nopol { get; set; }

            public string last_gps_time { get; set; }

        }

    }


}