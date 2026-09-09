using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace vtsadm
{
    public partial class device_qc : System.Web.UI.Page
    {
        private const string ViewStateDeviceTypeFilterKey = "DeviceTypeID";
        private const string ViewStateDeviceTypeFilterNewKey = "DeviceTypeID_New";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDashboard();
            }
        }

        protected void rptReturnedSummary_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "FilterByType", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string deviceTypeId = (e.CommandArgument ?? string.Empty).ToString().Trim();
            ViewState[ViewStateDeviceTypeFilterKey] = deviceTypeId;

            string safeDeviceTypeId = HttpUtility.JavaScriptStringEncode(deviceTypeId);
            string startupScript = "openReturnedByDeviceType('" + safeDeviceTypeId + "');";
            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "filterReturnedByType", startupScript, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "filterReturnedByType", startupScript, true);
            }
        }

        protected void rptNewSummary_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "FilterNew", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string deviceTypeId = (e.CommandArgument ?? string.Empty).ToString().Trim();
            ViewState[ViewStateDeviceTypeFilterNewKey] = deviceTypeId;

            string safeDeviceTypeId = HttpUtility.JavaScriptStringEncode(deviceTypeId);
            string startupScript = "loadByDeviceType('" + safeDeviceTypeId + "', null);";

            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "filterNewByType", startupScript, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "filterNewByType", startupScript, true);
            }
        }

        private void BindDashboard()
        {
            try
            {
                DataTable dtNew = GetDataNew();
                DataTable dtReturned = GetDataReturned();

                rptNewSummary.DataSource = dtNew;
                rptNewSummary.DataBind();

                rptReturnedSummary.DataSource = dtReturned;
                rptReturnedSummary.DataBind();

                pnlError.Visible = false;
                litError.Text = string.Empty;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "<strong>Error!</strong> " + Server.HtmlEncode(ex.Message);
            }
        }

        private DataTable GetDataNew()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                string sqlConnectionString = GetSqlConnectionString();
                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    throw new Exception("Database connection not available. Please login again.");
                }

                conn = new SqlConnection(sqlConnectionString);
                cmd = new SqlCommand("dbo.sp_count_device_not_qc_new_by_type", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);

                DataView dv = dt.DefaultView;
                dv.Sort = "TotalNotQc DESC";
                return dv.ToTable();
            }
            finally
            {
                if (dr != null) dr.Close();
                if (cmd != null) cmd.Dispose();
                if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
                if (conn != null) conn.Dispose();
            }
        }

        private DataTable GetDataReturned()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                string sqlConnectionString = GetSqlConnectionString();
                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    throw new Exception("Database connection not available. Please login again.");
                }

                conn = new SqlConnection(sqlConnectionString);
                cmd = new SqlCommand("dbo.sp_count_device_not_qc_returned_by_type", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                dr = cmd.ExecuteReader();
                dt.Load(dr);

                DataView dv = dt.DefaultView;
                dv.Sort = "TotalNotQc DESC";
                return dv.ToTable();
            }
            finally
            {
                if (dr != null) dr.Close();
                if (cmd != null) cmd.Dispose();
                if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
                if (conn != null) conn.Dispose();
            }
        }

        private string GetSqlConnectionString()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings["VTSADMIN"] != null &&
                    !string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString))
                {
                    return ConfigurationManager.ConnectionStrings["VTSADMIN"].ConnectionString;
                }

                if (Session["ClsTypeDBConnStringSQL"] == null)
                {
                    return string.Empty;
                }

                string rawConnectionString = Session["ClsTypeDBConnStringSQL"].ToString();
                if (string.IsNullOrWhiteSpace(rawConnectionString))
                {
                    return string.Empty;
                }

                // Legacy session string can be OLEDB style (Provider=...); strip it for SqlClient usage.
                string[] parts = rawConnectionString.Split(';');
                System.Text.StringBuilder sanitized = new System.Text.StringBuilder();
                for (int i = 0; i < parts.Length; i++)
                {
                    string item = parts[i];
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    if (item.TrimStart().StartsWith("Provider=", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    sanitized.Append(item).Append(";");
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sanitized.ToString());
                return builder.ConnectionString;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
