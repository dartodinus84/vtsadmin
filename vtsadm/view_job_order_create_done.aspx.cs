using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_job_order_create_done : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWJOBCREATE"))
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    /**
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    */
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /**
                    for (int i = 14; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[12].ToolTip = "Edit";
                    */

                    if (e.Row.Cells[9].Text.ToString() == "DE") { 
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6B0AA");
                    }
                    

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Open_GridViewDone(string sSearch)
        {
            if (txtJobID.Value.Trim() != "")
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_job_create_done '" + txtJobID.Value.Trim() + "','" + sSearch + "'";
                Session["RecViewJobCreateDone"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecViewJobCreateDone"], LblPaging);
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