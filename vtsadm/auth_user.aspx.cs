using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class auth_user : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListAuthUserFieldSort";
        string sViewStateDirSort = "RecListAuthUserDirSort";
        string sSessionRecList = "RecListAuthUser";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_user_authentication '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "UserID";
                ViewState[sViewStateDirSort] = "ASC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCONFAUTHUSER"))
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
                ClType.Open_Combos(CmbUserID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_auth_user");
                CmbUserID.SelectedValue = "[Select]";
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtUserType.Text = "";
                ClType.Open_Combos(CmbGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_auth_groupid");
                CmbGroupID.SelectedValue = "[Select]";
                txtGroupName.Text = "";
                CmbUserID.Attributes.Remove("disabled");

                LblUserIDDelete.InnerHtml = "";
                txtUserIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
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
        private string checkNbsp(string sIn)
        {
            string sOut = "";
            try
            {
                if (sIn.ToUpper().Trim() == "&NBSP;")
                {
                    sOut = "[Select]";
                }
                else
                {
                    sOut = sIn;
                }
            }
            catch (Exception ex)
            {

            }
            return sOut;
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sUserID = ""; string sFullName = ""; string sEmail = ""; string sUserType = "";
                string sGroupID = ""; string sGroupName = "";
                string sStatus = ""; 
                sUserID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sEmail = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sUserType = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sGroupID = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sGroupName = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            CmbUserID.SelectedValue = checkNbsp(sUserID);
                            txtFullName.Text = sFullName;
                            txtEmail.Text = sEmail;
                            txtUserType.Text = sUserType;
                            CmbGroupID.SelectedValue = checkNbsp(sGroupID);
                            txtGroupName.Text = sGroupName;
                            CmbUserID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Authentication user can not be edited, due to status code has been " + sStatus + "</div>";
                        }
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (CmbUserID.SelectedItem.Value.Trim() != "[Select]" && CmbGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        strSQL = "sp_insert_auth_user '" + CmbUserID.SelectedItem.Value.Trim() + "','" + CmbGroupID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication user has been save successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving authentication user has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving authentication user has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select user id and group id </div>";
                    }
                }
                else
                {
                    if (CmbUserID.SelectedItem.Value.Trim() != "[Select]" && CmbGroupID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        strSQL = "sp_update_auth_user '" + CmbUserID.SelectedItem.Value.Trim() + "','" + CmbGroupID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication user has been update successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update authentication user has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update authentication user has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select user id and group id </div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update authentication user has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmbUserID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (CmbUserID.SelectedItem.Value.Trim() != "[Select]")
                {
                    Recordset Rec = new Recordset(); string strSQL = "";
                    strSQL = "sp_get_attr_userid '" + CmbUserID.SelectedItem.Value.Trim() + "'";
                    Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim());
                    if (Rec.RecordCount() > 0)
                    {
                        txtFullName.Text = Rec.Fields("FullName").Trim();
                        txtEmail.Text = Rec.Fields("FullName").Trim();
                        txtUserType.Text = Rec.Fields("UserTypeDesc").Trim();
                    }
                    else
                    {
                        txtFullName.Text = "";
                        txtEmail.Text = "";
                        txtUserType.Text = "";
                    }
                }
                else
                {
                    txtFullName.Text = "";
                    txtEmail.Text = "";
                    txtUserType.Text = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (CmbGroupID.SelectedItem.Value.Trim() != "[Select]")
                {
                    txtGroupName.Text = CmbGroupID.SelectedItem.Text.Trim();
                }
                else
                {
                    txtGroupName.Text = "";
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
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sErr = "";

                if (txtUserIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_auth_user '" + txtUserIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication user has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication user has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication user has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Authentication user can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch(Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication user has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[10].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}