using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_purchase_order_done : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWPODETAIL"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewDone("");
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
        private void Open_GridViewDone(string sSearch)
        {
            if (txtPoID.Value.Trim() != "")
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_purchase_order_done '" + txtPoID.Value.Trim() + "','" + sSearch + "'";
                Session["RecViewPoDetailDone"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecViewPoDetailDone"], LblPaging);
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewDone(txtSearchLog.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
    }
}