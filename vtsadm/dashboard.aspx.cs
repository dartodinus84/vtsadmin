using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard : System.Web.UI.Page
    {

        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_small_box '" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblCntCustomer.InnerHtml = Convert.ToDouble(Rec.Fields("cntCustomer")).ToString("#,##0");
                    lblCntVehicle.InnerHtml = Convert.ToDouble(Rec.Fields("cntVehicle")).ToString("#,##0");
                    lblCntDevice.InnerHtml = Convert.ToDouble(Rec.Fields("cntDevice")).ToString("#,##0");
                    lblCntGsm.InnerHtml = Convert.ToDouble(Rec.Fields("cntGsm")).ToString("#,##0");
                    LblCntTechnician.InnerHtml = Convert.ToDouble(Rec.Fields("cntTechnician")).ToString("#,##0");
                    LblCntMarketing.InnerHtml = Convert.ToDouble(Rec.Fields("cntMarketing")).ToString("#,##0");
                    LblCntPO.InnerHtml = Convert.ToDouble(Rec.Fields("cntPO")).ToString("#,##0");
                    LblCntJONew.InnerHtml = Convert.ToDouble(Rec.Fields("cntJONew")).ToString("#,##0");
                    LblCntJOMaint.InnerHtml = Convert.ToDouble(Rec.Fields("cntJOMaint")).ToString("#,##0");
                    LblCntJOTraining.InnerHtml = Convert.ToDouble(Rec.Fields("cntJOTraining")).ToString("#,##0");
                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void open_dashboard_unit()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_monitoring_unit 1 ";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntUnitActive.InnerHtml = Convert.ToDouble(Rec.Fields("unit_active")).ToString("#,##0");
                    LblCntUnitInactive.InnerHtml = Convert.ToDouble(Rec.Fields("unit_inactive")).ToString("#,##0");
                    LblCntUnitDelay.InnerHtml = Convert.ToDouble(Rec.Fields("unit_delay")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }

        //Try
        //    Dim strSQL As String = "", Rec As New Recordset, sTName As String = "", sSum As String = ""
        //    strSQL = "sp_get_dashboard_data"
        //    Rec.Open(strSQL, Session("ClsTypeDBConnStringSQL").ToString())
        //    If Rec.RecordCount > 0 Then
        //        Rec.MoveFirst()
        //        Do While Not Rec.EOF
        //            sTName = Rec.Fields("tname").ToString()
        //            sSum = Rec.Fields("summary").ToString()
        //            Select Case sTName.ToUpper.Trim()
        //                Case "CUSTOMER"
        //                    lblCntCustomer.InnerText = sSum.ToString()
        //                Case "DESTINATION"
        //                    lblCntDestination.InnerText = sSum.ToString()
        //                Case "PLACES"
        //                    lblCntPlace.InnerText = sSum.ToString()
        //                Case "POOL"
        //                    lblCntPool.InnerText = sSum.ToString()
        //                Case "POOLVEHICLE"
        //                    lblCntPoolVehicle.InnerText = sSum.ToString()
        //                Case "SCHEDULE"
        //                    lblCntSchedule.InnerText = sSum.ToString()
        //                Case "VEHICLE"
        //                    lblCntVehicle.InnerText = sSum.ToString()
        //            End Select
        //            Rec.MoveNext()
        //        Loop
        //    End If
        //Catch ex As Exception

        //End Try

        protected void Clear()
        {
            try
            {
                lblCntCustomer.InnerText = "0";
                lblCntVehicle.InnerText = "0";
                lblCntDevice.InnerText = "0";
                lblCntGsm.InnerText = "0";
                LblCntTechnician.InnerText = "0";
                LblCntMarketing.InnerText = "0";
                LblCntPO.InnerText = "0";
                LblCntJONew.InnerText = "0";
                LblCntJOMaint.InnerText = "0";
                LblCntJOTraining.InnerText = "0";

                LblCntUnitActive.InnerText = "0";
                LblCntUnitInactive.InnerText = "0";
                LblCntUnitDelay.InnerText = "0";

            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strHtmlMenu = "";
                ClsType ClType = new ClsType();
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            open_dashboard_data();
                            open_dashboard_unit();


                            strHtmlMenu = ClType.BuildDashMenu(Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString());
                            PnlMenu.InnerHtml = strHtmlMenu;
                            //create_dash_menu();
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

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {

        }

        protected void CmdViewJONew_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("view_job_create.aspx", true);
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx");

            }
        }

        //    protected void create_dash_menu()
        //    {
        //        try
        //        {
        //                        <a class="btn btn-app bg-red-gradient" style="border-radius: 14px;"><i class="fa fa-edit"></i>Edit</a>


        //                        <a class="btn btn-app bg-blue-gradient" style="border-radius: 14px;">
        //                            <i class="fa fa-play"></i>Play
        //                        </a>
        //                        <a class="btn btn-app bg-green-gradient" style="border-radius: 14px;">
        //                            <i class="fa fa-repeat"></i>Repeat
        //                        </a>
        //                        <a class="btn btn-app bg-aqua-gradient" style="border-radius: 14px;">
        //                            <i class="fa fa-pause"></i>Pause
        //                        </a>
        //                        <a class="btn btn-app bg-black-gradient" style="border-radius: 14px;">
        //                            <i class="fa fa-save"></i>Save
        //                        </a>
        //                        <a class="btn btn-app bg-fuchsia-active" style="border-radius: 14px;">
        //                            <span class="badge bg-yellow">3</span>
        //                            <i class="fa fa-bullhorn"></i>Notifications
        //                        </a>
        //                        <a class="btn btn-app bg-light-blue-gradient" style="border-radius: 14px;">
        //                            <span class="badge bg-green">300</span>
        //                            <i class="fa fa-barcode"></i>Products
        //                        </a>
        //                        <a class="btn btn-app bg-maroon-gradient" style="border-radius: 14px;">
        //                            <span class="badge bg-purple">891</span>
        //                            <i class="fa fa-users"></i>Users
        //                        </a>
        //                        <a class="btn btn-app bg-navy-active" style="border-radius: 14px;">
        //                            <span class="badge bg-teal">67</span>
        //                            <i class="fa fa-inbox"></i>Orders
        //                        </a>
        //                        <a class="btn btn-app bg-olive-active" style="border-radius: 14px;">
        //                            <span class="badge bg-aqua">12</span>
        //                            <i class="fa fa-envelope"></i>Inbox
        //                        </a>
        //                        <a class="btn btn-app bg-orange-active" style="border-radius: 14px;">
        //                            <span class="badge bg-red">531</span>
        //                            <i class="fa fa-heart-o"></i>Likes
        //                        </a>
        //}
        //        catch(Exception ex)
        //        {

        //        }
        //    }

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        [WebMethod]
        public static string marketingPerformance()
        {
            string sOut = "";
            List<DataMarketingPerformance> dJson = new List<DataMarketingPerformance>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_marketing_performance";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {                    
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataMarketingPerformance
                        {
                            Marketing = Rec.Fields("MarketingName"),
                            Target = Convert.ToInt32(Rec.Fields("s_target")),
                            PO = Convert.ToInt32(Rec.Fields("sum_po")),
                            Realisasi = Convert.ToInt32(Rec.Fields("sum_installed"))
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

        //protected void CmdConsume_ServerClick(object sender, EventArgs e)
        //{
        //    //Uri uri = new Uri("http://localhost:9092");
        //    //string topicName = "chat-message";
        //    //var options = new KafkaOptions(uri);
        //    //var brokerRouter = new BrokerRouter(options);
        //    //var consumer = new Consumer(new ConsumerOptions(topicName, brokerRouter));
        //    //foreach (var msg in consumer.Consume())
        //    //{
        //    //    strMessage = Encoding.UTF8.GetString(msg.Value);

        //    //}

        //    //string ip = "127.0.0.1";
        //    //int port = 8081;
        //    //var server = new TcpListener(IPAddress.Parse(ip), port);

        //    //server.Start();
        //    //Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);

        //    //TcpClient client = server.AcceptTcpClient();
        //    //Console.WriteLine("A client connected.");

        //    //NetworkStream stream = client.GetStream();

        //    //// enter to an infinite cycle to be able to handle every change in stream
        //    //while (true)
        //    //{
        //    //    while (!stream.DataAvailable) ;
        //    //    while (client.Available < 3) ; // match against "get"

        //    //    byte[] bytes = new byte[client.Available];
        //    //    stream.Read(bytes, 0, client.Available);
        //    //    string s = Encoding.UTF8.GetString(bytes);

        //    //    if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
        //    //    {
        //    //        Console.WriteLine("=====Handshaking from client=====\n{0}", s);

        //    //        // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
        //    //        // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
        //    //        // 3. Compute SHA-1 and Base64 hash of the new value
        //    //        // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
        //    //        string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
        //    //        string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        //    //        byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
        //    //        string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

        //    //        // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
        //    //        byte[] response = Encoding.UTF8.GetBytes(
        //    //            "HTTP/1.1 101 Switching Protocols\r\n" +
        //    //            "Connection: Upgrade\r\n" +
        //    //            "Upgrade: websocket\r\n" +
        //    //            "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

        //    //        stream.Write(response, 0, response.Length);
        //    //    }
        //    //    else
        //    //    {
        //    //        bool fin = (bytes[0] & 0b10000000) != 0,
        //    //            mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"

        //    //        int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
        //    //            msglen = bytes[1] - 128, // & 0111 1111
        //    //            offset = 2;

        //    //        if (msglen == 126)
        //    //        {
        //    //            // was ToUInt16(bytes, offset) but the result is incorrect
        //    //            msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
        //    //            offset = 4;
        //    //        }
        //    //        else if (msglen == 127)
        //    //        {
        //    //            Console.WriteLine("TODO: msglen == 127, needs qword to store msglen");
        //    //            // i don't really know the byte order, please edit this
        //    //            // msglen = BitConverter.ToUInt64(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2], bytes[9], bytes[8], bytes[7], bytes[6] }, 0);
        //    //            // offset = 10;
        //    //        }

        //    //        if (msglen == 0)
        //    //            Console.WriteLine("msglen == 0");
        //    //        else if (mask)
        //    //        {
        //    //            byte[] decoded = new byte[msglen];
        //    //            byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
        //    //            offset += 4;

        //    //            for (int i = 0; i < msglen; ++i)
        //    //                decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]);

        //    //            string text = Encoding.UTF8.GetString(decoded);
        //    //            Console.WriteLine("{0}", text);
        //    //        }
        //    //        else
        //    //            Console.WriteLine("mask bit not set");

        //    //        Console.WriteLine();
        //    //    }
        //    //}
        //}
    }

    public class DataMarketingPerformance
    {
        public string Marketing { get; set; }
        public int Target { get; set; }
        public int PO { get; set; }
        public int Realisasi { get; set; }
    }
}