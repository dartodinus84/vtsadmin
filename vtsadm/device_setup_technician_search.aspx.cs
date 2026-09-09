using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class device_setup_technician_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETDEV"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                //CType(Page.Master, ASP.masterpage_masterpage_master).RegisterPostBackTrigger(CmdPreview) 
                SiteMaster sMaster = this.Master as SiteMaster;
                //sMaster.RegisterPostBackTrigger(CmdSubmit);
                //sMaster.RegisterPostBackTrigger(GridView2);


                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewTechnician("");
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

        protected void CmdSearchTechnician_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewTechnician(txtSearchTechnician.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewTechnician(string sName)
        {
            Recordset Rec = new Recordset();
            string strSQL = "sp_list_device_setup_technician_search '" + sName + "'";
            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
            GridView1.DataSource = Rec.DataRecord();
            GridView1.DataBind();
            Session["RecListDeviceSetupTechnicianSearch"] = Rec.RecData;
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListDeviceSetupTechnicianSearch"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button CmdButton = (Button)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postTechnicianChild('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}