using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class user : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListUserFieldSort";
        string sViewStateDirSort = "RecListUserDirSort";
        string sSessionRecList = "RecListUser";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_user '" + txtSearch.Text.Trim() + "'";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCONFMSTUSER"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                txtRegPassword.Attributes["value"] = txtRegPassword.Text.Trim();
                txtRegConfPassword.Attributes["value"] = txtRegConfPassword.Text.Trim();

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
                txtRegUserID.Text = "";
                txtRegPassword.Attributes["value"] = "";
                txtRegConfPassword.Attributes["value"] = "";
                txtRegUserName.Text = "";
                txtRegEmail.Text = "";
                ClType.Open_Combos(CmbUserTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_user_type");
                ClType.Open_Combos(CmbTechnicianID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_user_technician");
                ClType.Open_Combos(CmbMarketingID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_user_marketing");
                CmbUserTypeID.SelectedValue = "[Select]";
                CmbTechnicianID.SelectedValue = "[Select]";
                CmbMarketingID.SelectedValue = "[Select]";
                CmbTechnicianID.Attributes.Add("disabled", "disabled");
                CmbMarketingID.Attributes.Add("disabled", "disabled");
                txtRegUserID.Attributes.Remove("disabled");

                LblUserIDDelete.InnerHtml = "";
                txtUserIDDelete.Value = "";
                txtStatusDelete.Value = "";

                LblUserIDUnlock.InnerHtml = "";
                txtUserIDUnlock.Value = "";
                txtStatusUnlock.Value = "";

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
        protected void CmdSubmit_ServerClick(object sender, EventArgs e)
        {

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
                string sUserID = ""; string sPassword = ""; string sFullName = "";
                string sEmail = ""; string sUserTypeID = ""; string sTechnicianID = ""; string sMarketingID = "";
                string sStatus = ""; string sErr = "";
                sUserID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sPassword = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sEmail = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sUserTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sTechnicianID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "LK")
                        {
                            txtRegUserID.Text = sUserID;
                            txtRegPassword.Attributes["value"] = sPassword;
                            txtRegConfPassword.Attributes["value"] = sPassword;
                            txtRegUserName.Text = sFullName;
                            txtRegEmail.Text = sEmail;
                            CmbTechnicianID.Attributes.Remove("disabled");
                            CmbMarketingID.Attributes.Remove("disabled");
                            CmbTechnicianID.SelectedValue = checkNbsp(sTechnicianID);
                            CmbMarketingID.SelectedValue = checkNbsp(sMarketingID);
                            CmbUserTypeID.SelectedValue = checkNbsp(sUserTypeID);
                            switch (sUserTypeID.Trim())
                            {
                                case "UTY0000001":
                                    CmbTechnicianID.Attributes.Add("disabled", "disabled");
                                    CmbMarketingID.Attributes.Add("disabled", "disabled");
                                    break;
                                case "UTY0000002":
                                    CmbTechnicianID.Attributes.Remove("disabled");
                                    CmbMarketingID.Attributes.Add("disabled", "disabled");
                                    break;
                                case "UTY0000003":
                                    CmbTechnicianID.Attributes.Add("disabled", "disabled");
                                    CmbMarketingID.Attributes.Remove("disabled");
                                    break;
                                default:
                                    break;
                            }
                            txtRegUserID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> User can not be edited, due to status code has been " + sStatus + "</div>";
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Visible = false;
                    for (int i = 11; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Visible = false;
                    for (int i = 11; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    LinkButton CmdUnlock = (LinkButton)e.Row.Cells[8].FindControl("CmdUnlock");
                    CmdUnlock.OnClientClick = "confirmUnlock('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[10].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";

                }
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

        private bool isValidCmb()
        {
            bool boolOK = false;
            try
            {
                if (CmbUserTypeID.SelectedItem.Value.Trim() != "[Select]")
                {
                    switch (CmbUserTypeID.SelectedItem.Value.Trim())
                    {
                        case "UTY0000001":
                            boolOK = true;
                            break;

                        case "UTY0000002":
                            if (CmbTechnicianID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                boolOK = true;
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select technician</div>";
                            }
                            break;
                        case "UTY0000003":
                            if (CmbMarketingID.SelectedItem.Value.Trim() != "[Select]")
                            {
                                boolOK = true;
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select marketing</div>";
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select user type</div>";
                }
            }
            catch (Exception ex)
            {

            }
            return boolOK;
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtRegPassword.Text.Trim() == txtRegConfPassword.Text.Trim())
                    {
                        if (isValidCmb())
                        {
                            strSQL = "sp_insert_user '" + txtRegUserID.Text.Trim() + "','" + txtRegPassword.Text.Trim() + "','" + txtRegUserName.Text.Trim() + "','" + txtRegEmail.Text.Trim() + "','" + CmbUserTypeID.SelectedItem.Value.Trim() + "','" + CmbTechnicianID.SelectedItem.Value.Trim() + "','" + CmbMarketingID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> User has been save successfully!</div>";
                                    //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                    //UpPnl.Update();
                                    //insert audit trails
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving user has been failed</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving user has been failed (" + sErr + ")</div>";
                            }
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Password and confirm are not same, please check password confirmation";
                    }
                }
                else
                {
                    if (txtRegPassword.Text.Trim() == txtRegConfPassword.Text.Trim())
                    {
                        if (isValidCmb())
                        {
                            strSQL = "sp_update_user '" + txtRegUserID.Text.Trim() + "','" + txtRegPassword.Text.Trim() + "','" + txtRegUserName.Text.Trim() + "','" + txtRegEmail.Text.Trim() + "','" + CmbUserTypeID.SelectedItem.Value.Trim() + "','" + CmbTechnicianID.SelectedItem.Value.Trim() + "','" + CmbMarketingID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> User has been update successfully!</div>";
                                    //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                    //UpPnl.Update();
                                    //insert audit trails
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update user has been failed</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update user has been failed (" + sErr + ")</div>";
                            }
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Password and confirm are not same, please check password confirmation";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update user has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmbUserTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                CmbTechnicianID.SelectedValue = "[Select]";
                CmbMarketingID.SelectedValue = "[Select]";
                switch (CmbUserTypeID.SelectedItem.Value.Trim())
                {
                    case "UTY0000001":
                        CmbTechnicianID.Attributes.Add("disabled", "disabled");
                        CmbMarketingID.Attributes.Add("disabled", "disabled");
                        break;
                    case "UTY0000002":
                        CmbTechnicianID.Attributes.Remove("disabled");
                        CmbMarketingID.Attributes.Add("disabled", "disabled");
                        break;
                    case "UTY0000003":
                        CmbTechnicianID.Attributes.Add("disabled", "disabled");
                        CmbMarketingID.Attributes.Remove("disabled");
                        break;
                    default:
                        break;
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
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "LK")
                    {
                        strSQL = "sp_delete_user '" + txtUserIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> User has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing user has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing user has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> User can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing user has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmbYesUnlock_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sErr = "";
                if (txtUserIDUnlock.Value.Trim() != "")
                {
                    if (txtStatusUnlock.Value.ToUpper().Trim() == "LK")
                    {
                        strSQL = "sp_unlocked_user '" + txtUserIDUnlock.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> User has been unlocked successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Unlocked user has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Unlocked user has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> User can not be unlocked, due to status code has been " + txtStatusUnlock.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Unlocked user has been failed (" + ex.Message + ")</div>";
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