using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class SiteMaster : MasterPage
    {
        protected bool IsEmbedMode { get; private set; }

        private bool GetEmbedMode()
        {
            // Explicit query flags
            if (string.Equals(Request.QueryString["embed"], "1", StringComparison.OrdinalIgnoreCase)
                || string.Equals(Request.QueryString["webview"], "1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Auto-enable for training_cust when opened inside Android WebView (UA contains "; wv)")
            string currentPage = VirtualPathUtility.GetFileName(Request.Path) ?? string.Empty;
            if (currentPage.Equals("training_cust.aspx", StringComparison.OrdinalIgnoreCase))
            {
                string ua = Request.UserAgent ?? string.Empty;
                if (ua.IndexOf("; wv)", StringComparison.OrdinalIgnoreCase) >= 0
                    || ua.IndexOf("WebView", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // JSON/export action endpoints on assign-job pages must not be redirected to login HTML.
            string action = (Request.QueryString["action"] ?? string.Empty).Trim();
            string currentPage = VirtualPathUtility.GetFileName(Request.Path) ?? string.Empty;
            bool isAssignJobPage = currentPage.Equals("dashboard_assign_job.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPage.Equals("dashboard_assign_job_itsupport.aspx", StringComparison.OrdinalIgnoreCase);
            if (isAssignJobPage && !string.IsNullOrWhiteSpace(action))
            {
                return;
            }

            IsEmbedMode = GetEmbedMode();
            phLegacyTopNav.Visible = !IsEmbedMode;
            phLegacyChrome.Visible = !IsEmbedMode;
            phLegacyFooter.Visible = !IsEmbedMode;

            ClsType clType = new ClsType();
            String strHtmlMenu = "";
            try
            {
                if (!IsPostBack)
                {
                    clear();
                    if (!IsEmbedMode)
                    {
                        strHtmlMenu = clType.BuildJavaMenuBoot(Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString());
                        ul_menu.InnerHtml = strHtmlMenu;
                        txtUserName.InnerText = Session["ClsTypeUserFullName"].ToString();
                    }
                    //txtOldPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx");
            }

        }
        protected void CmdSignOut_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (Session["ClsTypeIsLogin"] != null)
                {
                    Session.Abandon();
                    Response.Redirect("~/login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login.aspx", false);
            }
        }
        public void RegisterPostBackTrigger(Control triggerOn)
        {
            ScriptManager1.RegisterPostBackControl(triggerOn);
        }

        protected void cmdYesSignOut_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (Session["ClsTypeIsLogin"] != null)
                {
                    Session.Abandon();
                    Response.Redirect("~/login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login.aspx", false);
            }
        }
        private void clear()
        {
            try
            {
                txtOldPass.Value = "";
                txtNewPass.Value = "";
                txtConfPass.Value= "";
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesChangePass_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; string sErr = ""; int intAff = 0;
                ExecCommand ec = new ExecCommand();
                strSQL = "sp_sp_change_password '" + Session["ClsTypeUserID"].ToString() + "','" + txtOldPass.Value.Trim() + "','" + txtNewPass.Value.ToString() + "','" + txtConfPass.Value.Trim() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        clear();
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='fa fa-check'></i> Success!</h4>Change password has been successfully!</div>";
                    }
                }
                else
                {
                    clear();
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='fa fa-ban'></i> Failed!</h4>Change password has been failed (" + sErr + ")</div>";
                }
            }
            catch (Exception ex)
            {
                clear();
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='fa fa-ban'></i> Failed!</h4>Change password has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}
