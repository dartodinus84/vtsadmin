using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class stock_opname_location_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAME"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            // Default to WAREHOUSE if the location type is already selected in parent form
                            if (Session["ClsLocationType"] != null && !string.IsNullOrEmpty(Session["ClsLocationType"].ToString()))
                            {
                                CmbLocationType.SelectedValue = Session["ClsLocationType"].ToString();
                            }

                            LoadLocations();
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
                // Handle error
            }
        }

        private void LoadLocations()
        {
            try
            {
                if (CmbLocationType.SelectedValue != "[Select]")
                {
                    string strSQL = "";

                    if (CmbLocationType.SelectedValue == "WAREHOUSE")
                    {
                        strSQL = "sp_list_warehouse_for_opname '" + txtSearch.Text.Trim() + "'";
                    }
                    else if (CmbLocationType.SelectedValue == "TECHNICIAN")
                    {
                        strSQL = "sp_list_technician_for_opname '" + txtSearch.Text.Trim() + "'";
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        ClsType ClType = new ClsType();
                        Session["RecLocationSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
                    }
                }
                else
                {
                    // Clear grid if no location type selected
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        protected void CmbLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadLocations();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                LoadLocations();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecLocationSearch"], LblPaging);
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string locationId = e.Row.Cells[0].Text;
                    string locationName = e.Row.Cells[1].Text;
                    string locationType = CmbLocationType.SelectedValue;

                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "postLocationToParent('" + locationId + "','" +
                                             locationName + "','" + locationType + "'); return false;";
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }
}