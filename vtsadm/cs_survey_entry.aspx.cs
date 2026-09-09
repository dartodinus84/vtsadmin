using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using System.IO;


namespace vtsadm
{
    public partial class cs_survey_entry : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_device_new '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListDeviceFieldSort"] = "DeviceID";
                ViewState["RecListDeviceDirSort"] = "DESC";
                Session["RecListDevice"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTDEV"))
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
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicegroup");
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicetype");
                ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_vendor");
                ClType.Open_Combos(CmbSource, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_source");
                ClType.Open_Combos(CmbServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_server");
                ClType.Open_Combos(CmbIsMobile, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_is_mobile");

                txtDeviceID.Text = "";
                CmbDeviceGroupID.SelectedValue = "[Select]";
                CmbDeviceTypeID.SelectedValue = "[Select]";
                txtNoSN.Text = "";
                txtIMEI.Text = "";
                txtGMT.Text = "";
                txtPackingListNo.Text = "";
                txtDate.Text = "";

                CmbSource.SelectedValue = "[Select]";
                CmbVendorID.SelectedValue = "[Select]";
                CmbServer.SelectedValue = "[Select]";
                CmbIsMobile.SelectedValue = "[Select]";

                LblDeviceID.InnerHtml = "";
                txtDeviceIDDelete.Value = "";
                txtStatusDelete.Value = "";

                LblDeviceIDUpdate.InnerHtml = "";
                txtDeviceIDUpdate.Value = "";
                txtStatusUpdate.Value = "";

                txtVendorAddress.Text = "";
                CmdSubmit.Text = "Submit";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtNoSN.Text.Trim() != "")
                    {
                        if (CmbDeviceTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbVendorID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (CmbSource.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (CmbServer.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        if (CmbIsMobile.SelectedItem.Value.Trim() != "[Select]")
                                        {
                                            strSQL = "sp_insert_device '" + txtNoSN.Text.Trim() + "','" + txtIMEI.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "', '"+ txtPackingListNo.Text.Trim() + "', '" + txtDate.Text.Trim() + "','" + CmbSource.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + CmbIsMobile.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                            {
                                                if (intAff > 0)
                                                {

                                                    clear();
                                                    Open_GridView();
                                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been save successfully!</div>";
                                                    //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                                    //UpPnl.Update();
                                                    //insert audit trails
                                                }
                                                else
                                                {
                                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device has been failed</div>";
                                                }
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save device has been failed (" + sErr + ")</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select is Mobile</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select server</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select source</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vendor</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select device type</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill No SN</div>";
                    }
                }
                else
                {
                    if (txtDeviceID.Text.Trim() != "")
                    {
                        if (CmbVendorID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (CmbSource.SelectedItem.Value.Trim() != "[Select]")
                            {
                                if (CmbServer.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    if (CmbIsMobile.SelectedItem.Value.Trim() != "[Select]")
                                    {
                                        strSQL = "sp_update_device '" + txtDeviceID.Text.Trim() + "','" + txtNoSN.Text.Trim() + "','" + txtIMEI.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "', '"+ txtPackingListNo.Text.Trim() + "', '" + txtDate.Text.Trim() + "','" + CmbSource.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + CmbIsMobile.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been update successfully!</div>";
                                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                                //UpPnl.Update();
                                                //insert audit trails
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update device has been failed</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update device has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select is Mobile</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select server</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select source</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vendor</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select device type</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
            }
        }

        //private void sendApi()
        //{
        //    try
        //    {
        //        string strSQL = "sp_get_cleansing_at10 '1'";
        //        Recordset Rec = new Recordset();
        //        Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
        //        if (Rec.RecordCount() > 0)
        //        {
        //            Rec.MoveFirst();
        //            while (!Rec.EOF)
        //            {
        //                string sErr = "";
        //                string noImei = Rec.Fields("msisdn");
        //                string merk = "EASYGO";
        //                string model = "";
        //                string tipe = "AT-10";

        //                EsealAdd(noImei, merk, model, tipe, ref sErr, ref sErr);
        //                Rec.MoveNext();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //protected void CmdYesSubmit2_ServerClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Int32 intAff = 0; String strSQL = ""; string sErr = "";
        //        ExecCommand ec = new ExecCommand();
        //        if (CmdSubmit2.Text.ToUpper() == "SUBMIT NEW")
        //        {
        //            //strSQL = "sp_insert_device '" + txtNoSN.Text.Trim() + "','" + CmbVendorID.SelectedItem.Value.ToString() + "','" + CmbDeviceTypeID.SelectedItem.Value.ToString() + "','" + txtGMT.Text.Trim() + "','" + txtDate.Text.Trim() + "','" + CmbSource.SelectedItem.Value.Trim() + "','','" + CmbServer.SelectedItem.Value.Trim() + "','" + CmbIsMobile.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
        //            //if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
        //            //{
        //            //    if (intAff > 0)
        //            //    {

        //                    sendApi();
        //                    //clear();
        //                    //Open_GridView();
        //                    //div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been save successfully!</div>";
        //            //    }
        //            //    else
        //            //    {
        //            //        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving device has been failed</div>";
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save device has been failed (" + sErr + ")</div>";
        //            //}
        //        }
        //        else
        //        {
        //            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill No SN</div>";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
        //    }
        //}

        //private string getJwtAuth(string sUserID, string sPassword, string sAppID, string sUrl)
        //{
        //    string strJwt = "";
        //    try
        //    {
        //        string sCred = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(sUserID.Trim() + ":" + sPassword.Trim()));
        //        WebClient wc = new WebClient();
        //        wc.QueryString.Add("app_id", sAppID);
        //        wc.Headers.Add("Accept", "application/json");
        //        wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + sCred);
        //        var response = wc.DownloadString(sUrl);
        //        var resJwt = new ResultJwt();
        //        JsonConvert.PopulateObject(response, resJwt);
        //        strJwt = resJwt.jwt.ToString().Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        strJwt = "";
        //    }

        //    return strJwt.Trim();
        //}
        //private bool EsealAdd(string noImei, string merk, string model, string tipe, ref string statusMessage, ref string outMessage)
        //{
        //    var isValid = true;
        //    try
        //    {
        //        string connection = Session["ClsTypeDBConnStringSQL"].ToString();
        //        string url_api = "";
        //        string url_jwt = "";
        //        string app_id = "";
        //        string vendorid = "";
        //        string token = "";
        //        string gps_sn = noImei;
        //        string message = "";
        //        string sJwt = "";
        //        string sUserID = "";
        //        string sPassword = "";

        //        string strSQLAPI = "sp_get_api_config 'BCESEAL-ADD'";
        //        Recordset RecAPI = new Recordset();
        //        RecAPI.Open(strSQLAPI, connection);

        //        if (RecAPI.RecordCount() > 0)
        //        {
        //            url_api = RecAPI.Fields("url_api");
        //            url_jwt = RecAPI.Fields("url_req_jwt");
        //            app_id = RecAPI.Fields("app_id");
        //            vendorid = RecAPI.Fields("vendorid");
        //            token = RecAPI.Fields("token");
        //            sUserID = RecAPI.Fields("userid");
        //            sPassword = RecAPI.Fields("password");

        //            Uri uri = new Uri(url_api);
        //            var param = new ParamEsealAdd();
        //            param.idVendor = vendorid;
        //            param.merk = merk;
        //            param.model = model;
        //            param.noImei = noImei;
        //            param.tipe = tipe;
        //            param.token = token;
        //            //sJwt = "eyJraWQiOiJzc29zIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJjZWlzYSIsImF1ZCI6WyJiZGM5NTUzZS1hN2FiLTQ2Y2UtOTE4OS02YjYxMTk4NWIzOTAiLCJiZWFjdWthaSJdLCJuYmYiOjE2NDk2ODYzNjksImlzcyI6ImJlYWN1a2FpLmdvLmlkIiwiZXhwIjoxNjQ5Njg2NjY5LCJpYXQiOjE2NDk2ODYzNjksImFwcF9pZCI6ImJkYzk1NTNlLWE3YWItNDZjZS05MTg5LTZiNjExOTg1YjM5MCJ9.fUjvJ1jN - 1ADzz_eQr5t1XDKfXAs5TOgcVc5BzaGPZKBNv1_tvHQuuN87prrw6NOlfuDKL6llKy22aGMFaQip97uRaTcBbRSQoxFNcjNqdv89CPiEgBNHN0dv04kIUvnVPhCcrMttVxl59dQVvgq - 9ZNexOaha7MyJTnwORhpKCUzl3MXY0HnWtCRu_N_vYTPtqPFg1XBxh - 5Ug4v1g1qI6KvDrovCIaEObn3jVRKd7LijEUgTTlTJRdnWVP1AgJN3givvILCVpd - fNwyZsK - RhI7vW6QVVQO70QgbfIlSOOwpOqBXDNkAWaYzY12HazSuWO0Km3Da1euOnphJfAUA";
        //            if (sJwt == "")
        //            {
        //                sJwt = getJwtAuth(sUserID, sPassword, app_id, url_jwt);
        //            }

        //            //string sJwt = "eyJraWQiOiJzc29zIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJjZWlzYSIsImF1ZCI6WyJiZGM5NTUzZS1hN2FiLTQ2Y2UtOTE4OS02YjYxMTk4NWIzOTAiLCJiZWFjdWthaSJdLCJuYmYiOjE2NDk2ODQ5NjQsImlzcyI6ImJlYWN1a2FpLmdvLmlkIiwiZXhwIjoxNjQ5Njg1MjY0LCJpYXQiOjE2NDk2ODQ5NjQsImFwcF9pZCI6ImJkYzk1NTNlLWE3YWItNDZjZS05MTg5LTZiNjExOTg1YjM5MCJ9.HpWKr_YCl2Lie1aDe6xyP1ELbboJ9hcMyxa5v5UO6j8uaZtutv7czAQTvOk - cRJNtDQCGP50uihy3YAeL57z2I4DfXNtMvniRcZfXBFHizRuhVNjzDKMogPKXOGn9Yxe_yDc1VCWHMlz9becPi9DyYwh2R - HDeKH77LH0BTLM2iLGlqqcJPOAaV9ygOT8auHwud_NV2ONmFfSpLG_PJdynoMmxzl9RJIyzr9mYuswV4EHMOiJYqrUZ_nbdJDokFzW0rHxf9GXK9j5Q404iBnUqA6ZZG06mgbJoKwECIy97FZ4tjW_NMmFqhdqOQAYsuWeblRXDjnWqfm57K8lDz1ZA";
        //            var rawData = JsonConvert.SerializeObject(param);
        //            WebClient webclient = new WebClient();
        //            webclient.UseDefaultCredentials = true;
        //            webclient.Headers.Add("Accept", "application/json");
        //            webclient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + sJwt);
        //            webclient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //            var response = webclient.UploadString(uri, "POST", rawData);
        //            var myObject = new ResponseEsealAdd();
        //            JsonConvert.PopulateObject(response, myObject);
        //            var status = myObject.status.ToString().ToLower();
        //            outMessage = myObject.status;
        //            statusMessage = myObject.message;
        //            var idEseal = myObject.item.idEseal;
        //            var merkEseal = myObject.item.merk;
        //            var tipeEseal = myObject.item.tipe;
        //            var modelEseal = myObject.item.model;
        //            var idVendor = myObject.item.idVendor;
        //            var noImeiEseal = myObject.item.noImei;
        //            var statusEseal = myObject.item.status;
        //            var wkRekam = myObject.item.wkRekam;
        //            var wkUpdate = myObject.item.wkUpdate;
        //            if (myObject.status.ToString().ToLower() == "success")
        //            {
        //                var strSQL = "sp_save_log_api_eseal_add '" + idEseal + "','" + merkEseal + "','" + modelEseal + "','" + tipeEseal + "','" + idVendor + "','" + noImeiEseal + "','" + statusEseal + "','" + wkRekam + "','" + wkUpdate + "','" + outMessage + "','" + statusMessage + "'";
        //                ExecCommand ec = new ExecCommand();
        //                int intAff = 0;
        //                string strSQLLOG = ""; string sErr = "";
        //                if (ec.Execute(strSQL, connection.ToString().Trim(), ref intAff, ref sErr))
        //                {
        //                    if (intAff > 0)
        //                    {
        //                        outMessage = outMessage.TrimStart(',') + "," + sErr;
        //                    }
        //                }
        //            }


        //            //url_api = RecAPI.Fields("url_api");
        //            //vendorid = RecAPI.Fields("vendorid");
        //            //token = RecAPI.Fields("token");

        //            //Uri uri = new Uri(url_api);
        //            //var param = new ParamEsealAdd();
        //            //param.idVendor = vendorid;
        //            //param.merk = merk;
        //            //param.model = model;
        //            //param.noImei = noImei;
        //            //param.tipe = tipe;
        //            //param.token = token;

        //            //var rawData = JsonConvert.SerializeObject(param);
        //            //WebClient webclient = new WebClient();
        //            //webclient.UseDefaultCredentials = true;
        //            //webclient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
        //            //webclient.Headers.Add("User-Agent: Other");
        //            //webclient.Headers.Add("Beacukai-Api-Key", "094d8768-0644-479b-a9cf-fdec763112eb");
        //            //webclient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
        //            ////ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //            //var response = webclient.UploadString(uri, "POST", rawData);
        //            //var myObject = new ResponseEsealAdd();
        //            //JsonConvert.PopulateObject(response, myObject);
        //            //var status = myObject.status.ToString().ToLower();
        //            //outMessage = myObject.status;
        //            //statusMessage = myObject.message;
        //            //var idEseal = myObject.item.idEseal;
        //            //var merkEseal = myObject.item.merk;
        //            //var tipeEseal = myObject.item.tipe;
        //            //var modelEseal = myObject.item.model;
        //            //var idVendor = myObject.item.idVendor;
        //            //var noImeiEseal = myObject.item.noImei;
        //            //var statusEseal = myObject.item.status;
        //            //var wkRekam = myObject.item.wkRekam;
        //            //var wkUpdate = myObject.item.wkUpdate;
        //            //if (myObject.status.ToString().ToLower() == "success")
        //            //{
        //            //    var strSQL = "sp_save_log_api_eseal_add '" + idEseal + "','" + merkEseal + "','" + modelEseal + "','" + tipeEseal + "','" + idVendor + "','" + noImeiEseal + "','" + statusEseal + "','" + wkRekam + "','" + wkUpdate + "','" + outMessage + "','" + statusMessage + "'";
        //            //    ExecCommand ec = new ExecCommand();
        //            //    int intAff = 0;
        //            //    string strSQLLOG = ""; string sErr = "";
        //            //    if (ec.Execute(strSQL, connection.ToString().Trim(), ref intAff, ref sErr))
        //            //    {
        //            //        if (intAff > 0)
        //            //        {
        //            //            outMessage = outMessage.TrimStart(',') + "," + sErr;
        //            //        }
        //            //    }
        //            //}

        //        }
        //        else
        //        {
        //            message = "config Api tidak di temukan";
        //            isValid = false;
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            var response = ex.Response;
        //            var dataStream = response.GetResponseStream();
        //            var reader = new StreamReader(dataStream);
        //            var details = reader.ReadToEnd();
        //            outMessage = outMessage + "," + details;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return isValid;
        //}

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                var row = (e.CommandSource as Control)?.NamingContainer as GridViewRow;
                if (row == null)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Unable to read selected row.</div>";
                    return;
                }

                Int32 iRow = row.RowIndex;
                string sDeviceID = ""; string sNoSN = ""; string sIMEI = ""; string sVendorID = ""; string sDeviceTypeID = ""; string sDeviceGroupID = ""; string sPackingListNo;
                string sGMT = ""; string sVendorAddress = ""; string sStatus = ""; string sSourceID = ""; string sServerID = ""; string sDateArrival = "";
                string sIsMobileID = "";
                sDeviceID = row.Cells[0].Text.Trim();
                sNoSN = row.Cells[1].Text.Trim();
                sIMEI = row.Cells[2].Text.Trim();
                sGMT = row.Cells[3].Text.Trim();
                sDateArrival = row.Cells[4].Text.Trim();
                sStatus = row.Cells[12].Text.Trim();

                sVendorID = row.Cells[16].Text.Trim();
                sVendorAddress = row.Cells[17].Text.Trim();
                sDeviceTypeID = row.Cells[18].Text.Trim();
                sDeviceGroupID = row.Cells[19].Text.Trim();
                sSourceID = row.Cells[20].Text.Trim();
                sServerID = row.Cells[21].Text.Trim();
                sIsMobileID = row.Cells[22].Text.Trim();
                sPackingListNo = row.Cells[23].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE" && sStatus.ToUpper().Trim() != "IS")
                        {
                            txtDeviceID.Text = sDeviceID;
                            CmbDeviceGroupID.SelectedValue = sDeviceGroupID;
                            ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_device_devicetype");
                            CmbDeviceTypeID.SelectedValue = sDeviceTypeID;
                            ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceTypeID.SelectedItem.Value.ToString(), "sp_list_device_vendor");
                            CmbVendorID.SelectedValue = sVendorID;
                            txtNoSN.Text = sNoSN;
                            txtIMEI.Text = sIMEI;
                            txtPackingListNo.Text = sPackingListNo;
                            txtGMT.Text = sGMT;
                            txtDate.Text = sDateArrival;
                            CmbSource.SelectedValue = sSourceID;
                            CmbServer.SelectedValue = sServerID;
                            txtVendorAddress.Text = sVendorAddress;
                            txtDeviceID.Attributes.Add("disabled", "disabled");
                            if (sIsMobileID == "" || sIsMobileID == "&nbsp;" || sIsMobileID == "0") { sIsMobileID = "[Select]"; }
                            CmbIsMobile.SelectedValue = sIsMobileID;
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited 1, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                //div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be edited or deleted, (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDevice"], LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }
        protected void GridView2_RowDeleting(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbDeviceTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceTypeID.SelectedItem.Value.ToString(), "sp_list_device_vendor");
                txtVendorAddress.Text = "";
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbDeviceGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_device_devicetype");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbVendorID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                setAddress(txtVendorAddress, "sp_get_device_address_vendor '" + CmbVendorID.SelectedItem.Value.ToString() + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void setAddress(TextBox txtBox, string strSQL, string sDBConn)
        {
            try
            {
                Recordset Rec = new Recordset();
                txtBox.Text = "";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    txtBox.Text = Rec.Fields(0);
                }
            }
            catch (Exception)
            {
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 16; i <= 23; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[3].Visible = false;
                    //e.Row.Cells[8].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 16; i <= 23; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[3].Visible = false;
                    //e.Row.Cells[8].Visible = false;
                    e.Row.Cells[12].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[14].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[12].Text.ToString() + "'); return false;";

                    LinkButton CmdButtonUpdate = (LinkButton)e.Row.Cells[14].FindControl("CmdUpdate");
                    CmdButtonUpdate.OnClientClick = "confirmUpdate('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[12].Text.ToString() + "'); return false;";

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
                if (txtSearch.Text.Trim() == "")
                {
                    clear();
                }
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtDeviceIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_device '" + txtDeviceIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                //Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Device can not be deleted, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtDeviceIDUpdate.Value.Trim() != "")
                {
                    strSQL = "sp_update_device_status '" + txtDeviceIDUpdate.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device has been updated successfully!</div>";
                        }
                        else
                        {
                            //txtError.Value = "Delete failed";
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Updated  Device has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Updated Device has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Updated Device has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListDevice"], ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString(), e.SortExpression);
                ViewState["RecListDeviceFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListDeviceDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }

        public partial class ParamEsealAdd
        {
            public string idVendor { get; set; }
            public string merk { get; set; }
            public string model { get; set; }
            public string noImei { get; set; }
            public string tipe { get; set; }
            public string token { get; set; }
        }
        public partial class ResponseEsealAdd
        {
            public string status { get; set; }
            public string message { get; set; }
            public ResponseItemEsealAdd item { get; set; }
        }
        public partial class ResponseItemEsealAdd
        {
            public string idEseal { get; set; }
            public string merk { get; set; }
            public string model { get; set; }
            public string tipe { get; set; }
            public string idVendor { get; set; }
            public string noImei { get; set; }
            public string status { get; set; }
            public string wkRekam { get; set; }
            public string wkUpdate { get; set; }
        }
    }
}
