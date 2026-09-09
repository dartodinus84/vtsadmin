using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_install_device_acc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWINSTALLDEV"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewAcc("");
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

        protected void CmdSearchAcc_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewAcc(txtSearchAcc.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewAcc(string sSearch)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_view_installation_device_acc '" + txtTvdID.Value.Trim() + "','" + sSearch + "'";
            Session["RecViewInstallDeviceAcc"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);            
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecViewInstallDeviceAcc"], LblPaging);
        }
    }
}