using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Diagnostics;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_telegram_bot : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public mst_telegram_bot()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_master_telegram_id '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListTelegramFieldSort"] = "TelegramID";
                ViewState["RecListTelegramDirSort"] = "ASC";
                Session["RecListTelegram"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListTelegramFieldSort"].ToString(), ViewState["RecListTelegramDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTTELEBOT"))
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
                            Open_GridView();

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

                txtCompanyID.Text = "";
                txtCompanyName.Text = "";
                txtTelegramID.Text = "";
                txtTelegramName.Text = "";
                txtUserName.Text = "";
                CmbGroupBotID.SelectedValue = "[Select]";
                LblTelegramID.InnerHtml = "";
                txtTelegramIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Visible = false;

                ClType.Open_Combos(CmbGroupBotID, Session["ClsTypeDBConnStringSQL"].ToString(), "'" + txtCompanyID.Text.ToString() + "'", "sp_list_master_telegram_bot");

            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int intAff = 0; string strSQL = ""; string sErr = "";
                string TelegramName = "";

                TelegramName = Convert.ToString(txtTelegramName.Text);

                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "UPDATE")
                {
                    if (txtTelegramID.Text.Trim() != "")
                    {

                        strSQL = "sp_update_master_telegram_id '" + txtTelegramID.Text.Trim() + "','" + CmbGroupBotID.SelectedItem.Value.ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Telegram Account has been update successfully!</div>";
                                Open_GridView();
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Telegram Account has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Telegram Account has been failed (" + sErr + ")</div>";
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update Telegram Account has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                String strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0;
                String sCompanyID = "";
                String sCompanyName = "";
                string sTelegramID = "";
                string sTelegramName = "";
                string sUserName = "";
                string sGroupBotID = "";
                string sErr = "";

                sCompanyID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sCompanyName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sTelegramID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sTelegramName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sUserName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sGroupBotID = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        txtCompanyID.Text = sCompanyID;
                        txtCompanyName.Text = sCompanyName;
                        txtTelegramID.Text = sTelegramID;
                        txtTelegramName.Text = sTelegramName;
                        txtUserName.Text = sUserName;
                        ClType.Open_Combos(CmbGroupBotID, Session["ClsTypeDBConnStringSQL"].ToString(), sCompanyID, "sp_list_master_telegram_bot");
                        CmdSubmit.Text = "Update";
                        CmdSubmit.Visible = true;
                        div_comment.InnerHtml = "";
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTelegram"], LblPaging, ViewState["RecListTelegramFieldSort"].ToString(), ViewState["RecListTelegramDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }
        protected void GridView2_RowDeleting(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {

                div_comment.InnerHtml = "";
                if (txtSearch.Text.Trim() == "")
                {
                    clear();
                }
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[6].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtTelegramIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_telegram '" + txtTelegramIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Telegram Account has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Telegram Account has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Telegram Account has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Telegram Account can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Telegram Account has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListTelegram"], ViewState["RecListTelegramFieldSort"].ToString(), ViewState["RecListTelegramDirSort"].ToString(), e.SortExpression);
                ViewState["RecListTelegramFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListTelegramDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        
    }
}