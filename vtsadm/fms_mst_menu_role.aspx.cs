using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_mst_menu_role : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListFMSMenuRoleFieldSort";
        string sViewStateDirSort = "RecListFMSMenuRoleDirSort";
        string sSessionRecList = "RecListFMSMenuRole";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "SELECT AppID, MenuID, RoleID, RoleName, Status, UsrUpd, CONVERT(VARCHAR(19), DtmUpd, 120) AS DtmUpd " +
                                "FROM fms_mst_menu_role WHERE 1=1 ";
                if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                {
                    strSQL += "AND RoleName LIKE '%" + txtSearch.Text.Trim() + "%' ";
                }
                strSQL += "ORDER BY " + ViewState[sViewStateFieldSort].ToString() + " " + ViewState[sViewStateDirSort].ToString();

                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch (Exception)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSMASTERMENU"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            ViewState[sViewStateFieldSort] = "RoleID";
                            ViewState[sViewStateDirSort] = "ASC";
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
            catch (Exception)
            {
            }
        }

        private void LoadAppDropdown()
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;

                Rec.Open("sp_list_app_setting", Session["ClsTypeDBConnStringSQL"].ToString());

                CmbAppID.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                CmbAppID.Items.Add(LstItem);

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();
                        LstItem.Value = Rec.Fields(0).Trim();
                        CmbAppID.Items.Add(LstItem);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                CmbAppID.Items.Clear();
                CmbAppID.Items.Add(new ListItem("[Select]", "[Select]"));
            }
        }

        private void clear()
        {
            try
            {
                LoadAppDropdown();
                CmbAppID.SelectedValue = "[Select]";
                txtMenuID.Text = "";
                txtRoleID.Text = "";
                txtRoleName.Text = "";
                txtMenuID.Attributes.Add("disabled", "disabled");
                txtRoleID.Attributes.Add("disabled", "disabled");
                LblRoleIDDelete.InnerHtml = "";
                txtRoleIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Text = "Submit";
                div_comment.InnerHtml = "";
            }
            catch (Exception)
            {
            }
        }

        protected void CmbAppID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]")
                {
                    txtMenuID.Attributes.Remove("disabled");
                    txtRoleID.Attributes.Remove("disabled");
                }
                else
                {
                    txtMenuID.Attributes.Add("disabled", "disabled");
                    txtRoleID.Attributes.Add("disabled", "disabled");
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> App ID selection has been failed (" + ex.Message + ")</div>";
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
            catch (Exception)
            {
            }
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridView();
            }
            catch (Exception)
            {
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int iRow = Convert.ToInt32(e.CommandArgument);
                string sAppID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                string sMenuID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                string sRoleID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                string sRoleName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                string sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            LoadAppDropdown();
                            CmbAppID.SelectedValue = sAppID;
                            txtMenuID.Text = sMenuID;
                            txtRoleID.Text = sRoleID;
                            txtRoleName.Text = (sRoleName == "&nbsp;" ? "" : sRoleName);
                            txtMenuID.Attributes.Add("disabled", "disabled");
                            txtRoleID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Data can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();

                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && txtMenuID.Text.Trim() != "" && txtRoleID.Text.Trim() != "" && txtRoleName.Text.Trim() != "")
                    {
                        strSQL = "sp_fms_mst_menu_role_insert '" + CmbAppID.SelectedItem.Value.Trim() + "','" + txtMenuID.Text.Trim() + "','" + txtRoleID.Text.Trim() + "','" + txtRoleName.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Role has been saved successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving role has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving role has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please complete App ID, Menu ID, Role ID and Role Name</div>";
                    }
                }
                else
                {
                    if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && txtMenuID.Text.Trim() != "" && txtRoleID.Text.Trim() != "" && txtRoleName.Text.Trim() != "")
                    {
                        strSQL = "sp_fms_mst_menu_role_update '" + CmbAppID.SelectedItem.Value.Trim() + "','" + txtMenuID.Text.Trim() + "','" + txtRoleID.Text.Trim() + "','" + txtRoleName.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Role has been updated successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update role has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update role has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please complete App ID, Menu ID, Role ID and Role Name</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update role has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtRoleIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_fms_mst_menu_role_delete '" + txtRoleIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Role has been removed successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing role has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing role has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Role can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing role has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception)
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
