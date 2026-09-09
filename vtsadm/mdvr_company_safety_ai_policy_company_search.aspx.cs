using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mdvr_company_safety_ai_policy_company_search : Page
    {
        private const string SessionGridKey = "RecListMdvrCompanySafetyAiPolicyCompanySearch";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ClsTypeIsLogin"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            ClsType clType = new ClsType();
            if (!clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Open_GridViewCust("");
            }
        }

        protected void CmdSearchCust_Click(object sender, EventArgs e)
        {
            Open_GridViewCust(txtSearchCompany.Value.Trim());
        }

        private void Open_GridViewCust(string search)
        {
            ClsType clType = new ClsType();
            string safe = (search ?? string.Empty).Replace("'", "''");
            string strSQL = "sp_get_list_all_customer '" + safe + "'";
            Session[SessionGridKey] = clType.Open_GridView(
                GridView1,
                strSQL,
                Session["ClsTypeDBConnStringSQL"].ToString(),
                LblPaging);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType clType = new ClsType();
            clType.Gv_PageIndexChanging(sender as GridView, e.NewPageIndex, Session[SessionGridKey], LblPaging);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            LinkButton cmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
            if (cmdButton == null)
            {
                return;
            }

            object dataItem = e.Row.DataItem;
            string companyId = Convert.ToString(DataBinder.Eval(dataItem, "company_id")).Trim();
            string companyName = Convert.ToString(DataBinder.Eval(dataItem, "company_nm")).Trim();
            companyId = HttpUtility.JavaScriptStringEncode(companyId);
            companyName = HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlDecode(companyName));
            cmdButton.OnClientClick = "parent.postCustChild('" + companyId + "','" + companyName + "');return false;";
        }
    }
}
