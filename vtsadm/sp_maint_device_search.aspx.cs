using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class sp_maint_device_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLMAINTSP"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewDevice("");
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

        protected void CmdSearchDevice_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewDevice(txtSearchDevice.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewDevice(string sSearch)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_sp_maint_device_search '" + txtCustID.Value.Trim() + "','" + txtJobID.Value.Trim() + "','" + sSearch + "'";
            Session["RecListSPMaintDeviceSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListSPMaintDeviceSearch"], LblPaging);
        }

        private string SanitizeForJs(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            string s = value.Replace('\u00A0', ' ').Replace("&#160;", " ").Trim();
            if (s == "&nbsp;")
            {
                return "";
            }

            return s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", " ").Replace("\n", " ");
        }

        private string CellJs(GridViewRow row, int index)
        {
            return SanitizeForJs(row.Cells[index].Text);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 8; i <= 26; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postDeviceChild('" + CellJs(e.Row, 0) + "','" + CellJs(e.Row, 1) + "','" + CellJs(e.Row, 2) + "'," +
                                              "'" + CellJs(e.Row, 3) + "','" + CellJs(e.Row, 4) + "'," +
                                              "'" + CellJs(e.Row, 5) + "','" + CellJs(e.Row, 6) + "'," +
                                              "'" + CellJs(e.Row, 7) + "','" + CellJs(e.Row, 8) + "'," +
                                              "'" + CellJs(e.Row, 9) + "','" + CellJs(e.Row, 10) + "'," +
                                              "'" + CellJs(e.Row, 11) + "','" + CellJs(e.Row, 12) + "'," +
                                              "'" + CellJs(e.Row, 13) + "','" + CellJs(e.Row, 14) + "'," +
                                              "'" + CellJs(e.Row, 15) + "','" + CellJs(e.Row, 16) + "'," +
                                              "'" + CellJs(e.Row, 17) + "','" + CellJs(e.Row, 18) + "'," +
                                              "'" + CellJs(e.Row, 19) + "','" + CellJs(e.Row, 20) + "'," +
                                              "'" + CellJs(e.Row, 21) + "','" + CellJs(e.Row, 22) + "'," +
                                              "'" + CellJs(e.Row, 23) + "','" + CellJs(e.Row, 24) + "'," +
                                              "'" + CellJs(e.Row, 25) + "','" + CellJs(e.Row, 26) + "');return false;";
                    for (int i = 8; i <= 26; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}