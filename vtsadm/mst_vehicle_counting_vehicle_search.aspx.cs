using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_vehicle_counting_vehicle_search : System.Web.UI.Page
    {
        private const string SessionRecListKey = "RecListVehicleSearch";
        private const string ViewStateFieldSortKey = "VehicleFieldSort";
        private const string ViewStateDirSortKey = "VehicleDirSort";
        private const string ViewStateCompanyKey = "VehicleCompanyId";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETPAR"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                if (!IsPostBack)
                {
                    string companyId = Request.QueryString["company_id"];
                    ViewState[ViewStateCompanyKey] = companyId;

                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewVehicle(companyId, "");
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

        protected void CmdSearchVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                string companyId = ViewState[ViewStateCompanyKey] == null ? "" : ViewState[ViewStateCompanyKey].ToString();
                Open_GridViewVehicle(companyId, txtSearchVehicle.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }

        private void Open_GridViewVehicle(string companyId, string search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    LblPagingVehicle.Text = "Company is not selected";
                    return;
                }

                int companyIdInt;
                if (!int.TryParse(companyId, out companyIdInt))
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    LblPagingVehicle.Text = "Invalid company identifier";
                    return;
                }

                string trimmedSearch = (search ?? string.Empty).Trim();

                ClsType ClType = new ClsType();
                string strSQL = $"sp_get_list_vehicle_by_company {companyIdInt}";
                DataSet ds = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingVehicle);

                if (ds == null || ds.Tables.Count == 0)
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    LblPagingVehicle.Text = "No items to display";
                    Session[SessionRecListKey] = null;
                    ViewState[ViewStateFieldSortKey] = "vehicle_id";
                    ViewState[ViewStateDirSortKey] = "ASC";
                    return;
                }

                DataSet finalDataset = ds;

                if (!string.IsNullOrEmpty(trimmedSearch))
                {
                    DataTable table = ds.Tables[0];
                    DataView dv = table.DefaultView;
                    string escaped = EscapeForRowFilter(trimmedSearch);
                    dv.RowFilter = $"vehicle_id LIKE '%{escaped}%' OR car_plate LIKE '%{escaped}%' OR gps_sn LIKE '%{escaped}%'";
                    DataTable filteredTable = dv.ToTable();
                    GridView1.DataSource = filteredTable;
                    GridView1.DataBind();

                    finalDataset = new DataSet();
                    finalDataset.Tables.Add(filteredTable);
                    ClType.showPaging(finalDataset, GridView1, LblPagingVehicle);
                }

                Session[SessionRecListKey] = finalDataset;
                ViewState[ViewStateFieldSortKey] = "vehicle_id";
                ViewState[ViewStateDirSortKey] = "ASC";
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Session[SessionRecListKey] == null)
                {
                    GridView1.PageIndex = 0;
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    return;
                }

                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[SessionRecListKey], LblPagingVehicle, ViewState[ViewStateFieldSortKey]?.ToString() ?? "", ViewState[ViewStateDirSortKey]?.ToString() ?? "");
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
                    LinkButton cmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    string vehicleId = HttpUtility.JavaScriptStringEncode(CleanCellText(e.Row.Cells[0].Text));
                    string carPlate = HttpUtility.JavaScriptStringEncode(CleanCellText(e.Row.Cells[1].Text));
                    string gpsSn = HttpUtility.JavaScriptStringEncode(CleanCellText(e.Row.Cells[2].Text));
                    cmdButton.OnClientClick = $"parent.postVehicleChild('{vehicleId}','{carPlate}','{gpsSn}'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static string CleanCellText(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string decoded = HttpUtility.HtmlDecode(value) ?? string.Empty;
            decoded = decoded.Replace("\u00A0", " ").Trim();
            return decoded;
        }

        private static string EscapeForRowFilter(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
        }
    }
}

