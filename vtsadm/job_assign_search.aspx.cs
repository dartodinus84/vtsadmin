using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_assign_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBASSIGN"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridViewJobOrder(txtSearchJobOrder.Value.Trim(), CmbJobGroup.SelectedValue.Trim());
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
        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();

                ClType.Open_Combos(CmbJobGroup, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_jo_type");
                CmbJobGroup.SelectedValue = "[Select]";

            }
            catch (Exception e)
            {
            }
        }
        private void Open_GridViewJobOrder(string sSearch, string sJoType)
        {
            string jotype = "";
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "";

                string strSQLJO = "sp_list_jo_type '" + sJoType + "'";
                Recordset Recchatid = new Recordset();
                Recchatid.Open(strSQLJO, Session["ClsTypeDBConnStringSQL"].ToString());

                if (sJoType == "MT")
                    strSQL += "sp_list_maint_job_order_search '" + sSearch + "'";
                else if (sJoType == "NEW")
                    strSQL += "sp_list_new_installation_job_order_search '" + sSearch + "'";
                else if (sJoType == "TR")
                    strSQL += "sp_list_trainig_customer_job_training_search_assign '" + sSearch + "'";

                else
                    strSQL += "sp_list_new_installation_job_order_search_assign '" + sSearch + "'";

                Session["RecListJobOrderSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception e)
            {

            }
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobOrderSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 7; i <= 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postJobOrderChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[6].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "');return false;";
                    for (int i = 7; i <= 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchJobOrder_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewJobOrder(txtSearchJobOrder.Value.Trim(), CmbJobGroup.SelectedValue.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmbJobGroup_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbJobGroup, Session["ClsTypeDBConnStringSQL"].ToString(), CmbJobGroup.SelectedValue.Trim(), "sp_list_jo_type");
            }
            catch (Exception ex)
            {
            }
        }

    }
}