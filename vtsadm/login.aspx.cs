using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class login : System.Web.UI.Page
    {
        private void clear()
        {
            lblMsg.InnerHtml = "";
            txtUserID.Value = "";
            txtPassword.Value = "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clear();
            }
        }
        protected void CmdLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType(); string sErr = "";
                Session["ClsTypeDBConnStringSQL"] = ConfigurationManager.ConnectionStrings["VTSAdminDB"].ToString();
                Session["ClsTypeAppPath"] = Request.PhysicalApplicationPath.Trim();
                string sUserIP = Request.UserHostAddress;
                if (ClType.appLogin(txtUserID.Value.Trim(), txtPassword.Value.Trim(), sUserIP, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr))
                {
                    Session["ClsTypeIsLogin"] = true;
                    Session["ClsTypeUserID"] = ClType.sUserID;
                    Session["ClsTypeUserFullName"] = ClType.sUserFullName;
                    Session["ClsTypeUserTechnicianID"] = ClType.sUserTechnicianID;
                    Session["ClsTypeUserMarketingID"] = ClType.sUserMarketingID;
                    Session["ClsTypeUserGroupID"] = ClType.sGroupID;
                    Session["ClsTypeAccessMenu"] = ClType.sAccessMenu;
                    Response.Redirect("dashboard.aspx", false);
                }
                else
                {
                    txtUserID.Value = "";
                    txtPassword.Value = "";
                    lblMsg.InnerHtml = sErr;
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = ex.Message;
            }
        }
    }
}