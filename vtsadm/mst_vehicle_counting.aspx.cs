using System;
using System.Net;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_vehicle_counting : System.Web.UI.Page
    {
        private bool _zoningAjaxHandled = false;

        private static bool IsZoningAjaxAction(string action)
        {
            return VehicleCountingZoningService.IsAjaxAction(action);
        }

        private string GetZoningAjaxAction()
        {
            return VehicleCountingZoningService.GetAction(HttpContext.Current);
        }

        private void HandleAjaxAction(string action)
        {
            _zoningAjaxHandled = true;
            object result = VehicleCountingZoningService.HandleAction(HttpContext.Current, action);
            WriteZoningJson(result);
        }

        private void WriteZoningJson(object payload)
        {
            _zoningAjaxHandled = true;
            VehicleCountingZoningService.WriteJson(HttpContext.Current, payload);
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected override void OnPreInit(EventArgs e)
        {
            string action = GetZoningAjaxAction();
            if (IsZoningAjaxAction(action))
            {
                EnableViewState = false;
            }

            base.OnPreInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_zoningAjaxHandled)
            {
                return;
            }

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (_zoningAjaxHandled)
            {
                return;
            }

            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = VehicleCountingZoningService.GetAction(HttpContext.Current);
            if (!string.IsNullOrWhiteSpace(action) && VehicleCountingZoningService.IsAjaxAction(action))
            {
                HandleAjaxAction(action);
                return;
            }

            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETPAR"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_Gridview1();
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

        protected void Open_Gridview1()
        {
            ClsType ClType = new ClsType();
            string strSQL = $"sp_get_list_vehicle_counting '{txt_company_name.Text.Trim()}', '{txt_param_type.Text.Trim()}'";
            ViewState["RecListParamFieldSort"] = "vehicle_id";
            ViewState["RecListParamDirSort"] = "ASC";
            Session["RecListParam"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingParam, ViewState["RecListParamFieldSort"].ToString(), ViewState["RecListParamDirSort"].ToString());
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListParam"], LblPagingParam, ViewState["RecListParamFieldSort"].ToString(), ViewState["RecListParamDirSort"].ToString());
            clear();
            
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_Gridview1();
                comment_save.InnerHtml = "";
                div_comment.InnerHtml = "";
                txtCompanyID.Value = "";
                txtCompID.Value = "";
                txtCompanyName.Text = "";
                txtParamType.Text = "";
                txtVehiclePlate.Text = "";
                txtParamCode.Text = "";
                txtParamValue.ClearSelection();
                CmdSave.Style.Remove("display");
                CmdEdit.Style.Add("display", "none");
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdDelete");
                    LinkButton CmdEdit = (LinkButton)e.Row.FindControl("CmdEdit");

                    DataRowView rowView = e.Row.DataItem as DataRowView;
                    if (rowView != null)
                    {
                        string id = HttpUtility.JavaScriptStringEncode(rowView["id"]?.ToString() ?? "");
                        string companyId = HttpUtility.JavaScriptStringEncode(rowView["company_id"]?.ToString() ?? "");
                        string companyName = HttpUtility.JavaScriptStringEncode(rowView["company_nm"]?.ToString() ?? "");
                        string vehicleId = HttpUtility.JavaScriptStringEncode(rowView["vehicle_id"]?.ToString() ?? "");
                        string carPlate = HttpUtility.JavaScriptStringEncode(rowView["car_plate"]?.ToString() ?? "");
                        string gpsSn = HttpUtility.JavaScriptStringEncode(rowView["gps_sn"]?.ToString() ?? "");
                        string channel = HttpUtility.JavaScriptStringEncode(rowView["channel"]?.ToString() ?? "");

                        CmdEdit.OnClientClick = $"postEdit('{id}', '{companyId}', '{companyName}', '{vehicleId}', '{carPlate}', '{gpsSn}', '{channel}'); return false;";
                        CmdButton.OnClientClick = $"postDelete('{id}'); return false;";
                    }
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = $"selectCustomer('{e.Row.Cells[0].Text.ToString()}', '{e.Row.Cells[1].Text.ToString()}')";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            comment_save.InnerHtml = "";
            try
            {
                string strSQL = "";
                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                if (txtParamIDDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_vehicle_counting " + txtParamIDDelete.Value.Trim();
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle counting has been removed successfully!</div>";

                            Open_Gridview1();
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle counting has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle counting has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle counting has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSave_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            ExecCommand Ec = new ExecCommand();
            int intAff = 0;
            string sErr = "";

            var company_id = txtCompID.Value.Trim();
            var company_nm = txtCompanyName.Text.Trim();
            var vehicle_id = txtParamType.Text.Trim();
            var gps_sn = txtParamCode.Text.Trim();
            
            // Get selected channels from CheckBoxList
            List<string> selectedChannels = new List<string>();
            foreach (ListItem item in txtParamValue.Items)
            {
                if (item.Selected)
                {
                    selectedChannels.Add(item.Value);
                }
            }
            var channel = string.Join(",", selectedChannels);

            if(string.IsNullOrEmpty(company_id) || string.IsNullOrEmpty(company_nm))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Company information cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(vehicle_id))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Vehicle ID cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(gps_sn))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>GPS SN cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(channel))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Channel cannot empty!!</div>";
            }
            else
            {
                try
                {
                    strSQL = $"sp_insert_vehicle_counting {company_id}, '{vehicle_id}', '{gps_sn}', '{channel}'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            comment_save.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!!</strong> Insert vehicle counting success</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtVehiclePlate.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.ClearSelection();
                            Open_Gridview1();
                        }
                        else
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert vehicle counting has been failed ({sErr})</div>";
                        }
                    }
                    else
                    {
                        comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert vehicle counting has been failed ({sErr})</div>";
                    }
                }
                catch (Exception ex)
                {
                    comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert vehicle counting has been failed ({sErr})</div>";
                }
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            txtAutoId.Value = "";
            comment_save.InnerHtml = "";
            div_comment.InnerHtml = "";
            txtCompanyID.Value = "";
            txtCompID.Value = "";
            txtCompanyName.Text = "";
            txtParamType.Text = "";
            txtVehiclePlate.Text = "";
            txtParamCode.Text = "";
            txtParamValue.ClearSelection();
            CmdEdit.Style.Add("display", "none");
        }

        protected void clear()
        {
            txtAutoId.Value = "";
            comment_save.InnerHtml = "";
            div_comment.InnerHtml = "";
            txtCompanyID.Value = "";
            txtCompID.Value = "";
            txtCompanyName.Text = "";
            txtParamType.Text = "";
            txtVehiclePlate.Text = "";
            txtParamCode.Text = "";
            txtParamValue.ClearSelection();
            CmdEdit.Style.Add("display", "none");
        }

        public void CmdYesUpdate_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            ExecCommand Ec = new ExecCommand();
            int intAff = 0;
            string sErr = "";

            string autoid = txtAutoId.Value;
            string company_id = txtCompID.Value;
            string company_nm = txtCompanyName.Text.Trim();
            string vehicle_id = txtParamType.Text.Trim();
            string gps_sn = txtParamCode.Text.Trim();
            
            // Get selected channels from CheckBoxList
            List<string> selectedChannels = new List<string>();
            foreach (ListItem item in txtParamValue.Items)
            {
                if (item.Selected)
                {
                    selectedChannels.Add(item.Value);
                }
            }
            string channel = string.Join(",", selectedChannels);

            if (string.IsNullOrEmpty(company_id) || string.IsNullOrEmpty(company_nm))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Company information cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(vehicle_id))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Vehicle ID cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(gps_sn))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>GPS SN cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(channel))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Channel cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(autoid))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>auto_id is empty!! (Internal error)</div>";
            } else
            {
                try
                {
                    strSQL = $"usp_update_vehicle_counting {autoid}, {company_id}, '{vehicle_id}', '{gps_sn}', '{channel}'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Update Success!</strong> Vehicle counting {autoid} has been updated successfully!</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtVehiclePlate.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.ClearSelection();
                            CmdSave.Style.Remove("display");
                            CmdEdit.Style.Add("display", "none");
                            Open_Gridview1();
                        }
                        else
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update vehicle counting has been failed ({sErr})</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtVehiclePlate.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.ClearSelection();
                            CmdSave.Style.Remove("display");
                            CmdEdit.Style.Add("display", "none");
                            Open_Gridview1();
                        }
                    }
                    else
                    {
                        comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update vehicle counting has been failed ({sErr})</div>";
                        div_comment.InnerHtml = "";
                        txtCompanyID.Value = "";
                        txtCompID.Value = "";
                        txtCompanyName.Text = "";
                        txtParamType.Text = "";
                        txtVehiclePlate.Text = "";
                        txtParamCode.Text = "";
                        txtParamValue.ClearSelection();
                        CmdSave.Style.Remove("display");
                        CmdEdit.Style.Add("display", "none");
                        Open_Gridview1();
                    }
                }
                catch (Exception ex)
                {
                    comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update vehicle counting has been failed ({sErr})</div>";
                    div_comment.InnerHtml = "";
                    txtCompanyID.Value = "";
                    txtCompID.Value = "";
                    txtCompanyName.Text = "";
                    txtParamType.Text = "";
                    txtVehiclePlate.Text = "";
                    txtParamCode.Text = "";
                    txtParamValue.ClearSelection();
                    CmdSave.Style.Remove("display");
                    CmdEdit.Style.Add("display", "none");
                    Open_Gridview1();
                }
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static VehicleInfo GetVehicleInfo(string companyId, string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(companyId) || string.IsNullOrWhiteSpace(vehicleId))
            {
                return null;
            }

            int companyIdInt;
            if (!int.TryParse(companyId.Trim(), out companyIdInt))
            {
                return null;
            }

            HttpContext context = HttpContext.Current;
            string connectionString = context?.Session?["ClsTypeDBConnStringSQL"]?.ToString();

            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            try
            {
                string trimmedVehicleId = vehicleId.Trim();
                string sErr = "";
                Recordset rec = new Recordset();
                rec.Open($"sp_get_list_vehicle_by_company {companyIdInt}", connectionString, ref sErr);

                if (!string.IsNullOrEmpty(sErr) || rec.RecData == null || rec.RecData.Tables.Count == 0)
                {
                    return null;
                }

                DataTable table = rec.RecData.Tables[0];
                VehicleInfo info = null;

                foreach (DataRow row in table.Rows)
                {
                    string rowVehicleId = row.ItemArray.Length > 0 ? row[0]?.ToString().Trim() : string.Empty;
                    if (!string.IsNullOrEmpty(rowVehicleId) && rowVehicleId.Equals(trimmedVehicleId, StringComparison.OrdinalIgnoreCase))
                    {
                        info = new VehicleInfo
                        {
                            VehicleId = rowVehicleId,
                            CarPlate = row.ItemArray.Length > 1 ? row[1]?.ToString().Trim() : string.Empty,
                            GpsSn = row.ItemArray.Length > 2 ? row[2]?.ToString().Trim() : string.Empty
                        };
                        break;
                    }
                }

                if (info == null && table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    info = new VehicleInfo
                    {
                        VehicleId = row.ItemArray.Length > 0 ? row[0]?.ToString().Trim() : string.Empty,
                        CarPlate = row.ItemArray.Length > 1 ? row[1]?.ToString().Trim() : string.Empty,
                        GpsSn = row.ItemArray.Length > 2 ? row[2]?.ToString().Trim() : string.Empty
                    };
                }

                return info;
            }
            catch
            {
                return null;
            }
        }

        public class VehicleInfo
        {
            public string VehicleId { get; set; }
            public string CarPlate { get; set; }
            public string GpsSn { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ZoningApiResponse GetZoning(int id)
        {
            if (id <= 0)
            {
                return ZoningApiResponse.Fail("ID Vehicle Counting wajib diisi.");
            }

            HttpContext context = HttpContext.Current;
            string connectionString = VehicleCountingZoningService.GetSqlClientConnectionString(context);
            if (string.IsNullOrEmpty(connectionString))
            {
                return ZoningApiResponse.Fail("Koneksi database tidak tersedia.");
            }

            try
            {
                var record = VehicleCountingZoningService.LoadZoningRecord(id, connectionString);
                if (record == null)
                {
                    return ZoningApiResponse.Fail("Data Vehicle Counting tidak ditemukan.");
                }

                return ZoningApiResponse.Ok("Data zoning berhasil dimuat.", record);
            }
            catch (SqlException ex)
            {
                return ZoningApiResponse.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return ZoningApiResponse.Fail(ex.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ZoningApiResponse SaveZoning(int id, string zonePoints)
        {
            return MapServiceResultToApiResponse(VehicleCountingZoningService.SaveZoningCore(HttpContext.Current, id, zonePoints));
        }

        private static ZoningApiResponse MapServiceResultToApiResponse(object result)
        {
            if (result == null)
            {
                return ZoningApiResponse.Fail("Response zoning kosong.");
            }

            try
            {
                dynamic payload = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result));
                if (payload == null)
                {
                    return ZoningApiResponse.Fail("Response zoning tidak valid.");
                }

                bool success = payload.success != null && Convert.ToBoolean(payload.success);
                string message = payload.message != null ? Convert.ToString(payload.message) : "";
                if (success)
                {
                    return ZoningApiResponse.Ok(message, payload.data);
                }

                return ZoningApiResponse.Fail(string.IsNullOrWhiteSpace(message) ? "Gagal memproses zoning." : message);
            }
            catch (Exception ex)
            {
                return ZoningApiResponse.Fail(ex.Message);
            }
        }


        public class ZoningApiResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
            public object data { get; set; }

            public static ZoningApiResponse Ok(string message, object data)
            {
                return new ZoningApiResponse { success = true, message = message, data = data };
            }

            public static ZoningApiResponse Fail(string message)
            {
                return new ZoningApiResponse { success = false, message = message, data = null };
            }
        }

        public class ZoningRecord
        {
            public int id { get; set; }
            public int company_id { get; set; }
            public string company_name { get; set; }
            public string vehicle_id { get; set; }
            public string nopol { get; set; }
            public string gps_sn { get; set; }
            public string channel { get; set; }
            public string next_channel { get; set; }
            public string zone_points { get; set; }
        }
    }
}
