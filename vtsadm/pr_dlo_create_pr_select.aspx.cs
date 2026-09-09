using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_dlo_create_pr_select : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                var access = Session["ClsTypeAccessMenu"].ToString().ToUpper();
                if (!access.Contains("MNUPRCREATE") && !access.Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        Open_GridViewPr("");
                    else
                        Response.Redirect("login.aspx");
                }
            }
            catch (Exception ex) { }
        }

        protected void CmdSearchPr_Click(object sender, EventArgs e)
        {
            try { Open_GridViewPr(txtSearchPr.Value.Trim()); }
            catch (Exception ex) { }
        }

        private void Open_GridViewPr(string search)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_header_pur_dlo_create '" + search.Replace("'", "''") + "'";
            Recordset rec = new Recordset();
            rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
            if (rec.RecData != null && rec.RecData.Tables.Count > 0)
                GridView1.DataSource = rec.RecData.Tables[0];
            else
                GridView1.DataSource = null;
            GridView1.DataBind();
            Session["RecListPrSelect"] = rec.RecData;
            ClType.showPaging(rec.RecData, GridView1, LblPaging);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListPrSelect"], LblPaging);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    // Pass to parent: PurID(0), PurNumber(1), sPurDate(2), VendorName(3), VendorID(4)
                    string s0 = e.Row.Cells[0].Text.Trim().Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
                    string s1 = e.Row.Cells[1].Text.Trim().Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
                    string s2 = e.Row.Cells[2].Text.Trim().Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
                    string s3 = e.Row.Cells[3].Text.Trim().Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
                    string s4 = e.Row.Cells[4].Text.Trim().Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
                    CmdButton.OnClientClick = "parent.postPrChild('" + s0 + "','" + s1 + "','" + s2 + "','" + s3 + "','" + s4 + "');return false;";
                }
            }
            catch (Exception ex) { }
        }
    }
}
