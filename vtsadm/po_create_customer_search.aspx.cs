using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class po_create_customer_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUPOCREATE"))
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

        protected void CmdSearchCust_Click(object sender, EventArgs e)
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
            string strSQL = "sp_list_purchase_create_customer_search_new '" + sFullName + "'";
            Session["RecListPurchaseCreateCustomerSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListPurchaseCreateCustomerSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
			//dandy
			 if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[4].Visible = false;
                }

                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                
					 e.Row.Cells[4].Visible = false;
			 //end
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                          CmdButton.OnClientClick = "parent.postCustChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
             //dandy
			                                  "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "');return false;";
			 //end
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}