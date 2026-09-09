using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class postdeal_cust_job_postdeal_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBDEAL"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewJobTraining("");
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

        private void Open_GridViewJobTraining(string sName)
        {
            ClsType ClType = new ClsType();
            //string strSQL = "sp_list_postdeal_customer_job_postdeal_search '" + sName + "'";
            string strSQL;

            if (Session["ClsTypeUserMarketingID"] != null &&
                    !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
            {
                // SP butuh UserID, MarketingID di-derive di SP
                string userID = Session["ClsTypeUserID"].ToString();
                strSQL = "sp_list_postdeal_customer_job_postdeal_search '" + sName + "','" + userID + "'";
            }
            else
            {
                // User non-marketing: pakai parameter search saja (tanpa filter marketing)
                strSQL = "sp_list_postdeal_customer_job_postdeal_search '" + sName + "'";
            }

            Session["RecListTrainingCustJobTrainingSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTrainingCustJobTrainingSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false; // JobActivityID (hidden, but sent to parent)
                    // ActivityCode shown
                    e.Row.Cells[5].Visible = false; // CustID
                    e.Row.Cells[6].Visible = false; // CustBranchName
                    e.Row.Cells[7].Visible = false; // BusinessFieldID
                    e.Row.Cells[8].Visible = false; // PICName
                    // MobilePhone shown as PIC Phone
                    e.Row.Cells[10].Visible = false; // MarketingName hidden
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    // Kirim nilai ke parent sesuai urutan yang diharapkan:
                    // postJobDealChild(JobActivityID, ActivityCode, ReqDate, FullName, ProductName,
                    //                  CustID, CustBranchName, BusinessFieldID, PICName, MobilePhone, MarketingName)
                    CmdButton.OnClientClick = "parent.postJobDealChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "');return false;";

                    e.Row.Cells[0].Visible = false; // JobActivityID
                    e.Row.Cells[5].Visible = false; // CustID
                    e.Row.Cells[6].Visible = false; // CustBranchName
                    e.Row.Cells[7].Visible = false; // BusinessFieldID
                    e.Row.Cells[8].Visible = false; // PICName
                    e.Row.Cells[10].Visible = false; // MarketingName
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchJobTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewJobTraining(txtSearchJobTraining.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
    }
}