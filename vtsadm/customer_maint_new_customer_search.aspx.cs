using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class customer_maint_new_customer_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTCUST"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewCust("");
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

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewCust(txtSearchCust.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewCust(string sFullName)
        {
            ClsType ClType = new ClsType();
            string custId = Session["CustomerMaintCustID"] != null ? Session["CustomerMaintCustID"].ToString().Trim() : "";
            string strSQL = "sp_list_customer_maint_new_customer_search '" + custId + "','" + sFullName + "'";
            Session["RecListCustomerMaintNewCustomerSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListCustomerMaintNewCustomerSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    string custID = rowView["CustID"] != null && rowView["CustID"] != DBNull.Value ? rowView["CustID"].ToString().Trim() : "";
                    string fullName = rowView["FullName"] != null && rowView["FullName"] != DBNull.Value ? rowView["FullName"].ToString().Trim() : "";
                    string branchName = rowView["BranchName"] != null && rowView["BranchName"] != DBNull.Value ? rowView["BranchName"].ToString().Trim() : "";
                    string custIDJs = EscapeForJs(custID);
                    string fullNameJs = EscapeForJs(fullName);
                    string branchNameJs = EscapeForJs(branchName);
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postNewCustomerChild('" + custIDJs + "','" + fullNameJs + "','" + branchNameJs + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static string EscapeForJs(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", " ").Replace("\n", " ");
        }
    }
}