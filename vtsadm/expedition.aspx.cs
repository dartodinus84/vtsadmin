using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class expedition : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_expedisi '" + txtSearch.Text.Trim().Replace("'", "''") + "'";
                ViewState["RecListExpedisiFieldSort"] = "ExpedisiID";
                ViewState["RecListExpedisiDirSort"] = "ASC";
                Session["RecListExpedisi"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListExpedisiFieldSort"].ToString(), ViewState["RecListExpedisiDirSort"].ToString());
            }
            catch (Exception ex)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTXPD"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ClsType ClType = new ClsType();
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

        private static string CleanCellText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("\u00A0", "").Replace("&nbsp;", "").Trim();
        }

        private void clear()
        {
            try
            {
                txtExpedisiID.Text = "";
                txtExpedisiName.Text = "";
                txtPICName.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
                txtAddress.Text = "";
                txtExpedisiID.Enabled = false;
                txtExpedisiID.Attributes.Add("disabled", "disabled");

                LblExpedisiID.InnerHtml = "";
                txtExpedisiIDDelete.Value = "";
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

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int intAff = 0;
                string strSQL = "";
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                string usr = Session["ClsTypeUserID"] != null ? Session["ClsTypeUserID"].ToString().Trim().Replace("'", "''") : "";
                string name = txtExpedisiName.Text.Trim().Replace("'", "''");
                string picName = (txtPICName.Text ?? "").Trim().Replace("'", "''");
                string phone = (txtPhone.Text ?? "").Trim().Replace("'", "''");
                string email = (txtEmail.Text ?? "").Trim().Replace("'", "''");
                string address = (txtAddress.Text ?? "").Trim().Replace("'", "''");

                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Expedition Name</div>";
                        return;
                    }
                    strSQL = "sp_insert_expedisi '" + name + "','" + picName + "','" + phone + "','" + email + "','" + address + "','" + usr + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        clear();
                        Open_GridView();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Expedition has been saved successfully!</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrEmpty(sErr) ? "Saving expedition failed." : sErr) + "</div>";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtExpedisiID.Text.Trim()))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Expedition ID is required for update.</div>";
                        return;
                    }
                    string id = txtExpedisiID.Text.Trim().Replace("'", "''");
                    strSQL = "sp_update_expedisi '" + id + "','" + name + "','" + picName + "','" + phone + "','" + email + "','" + address + "','" + usr + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        clear();
                        Open_GridView();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Expedition has been updated successfully!</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrEmpty(sErr) ? "Update failed." : sErr) + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = (e.CommandSource as GridView).Rows[iRow];
                string sExpedisiID = CleanCellText(row.Cells[0].Text);
                string sExpedisiName = CleanCellText(row.Cells[1].Text);
                string sPICName = CleanCellText(row.Cells[2].Text);
                string sPhone = CleanCellText(row.Cells[3].Text);
                string sEmail = CleanCellText(row.Cells[4].Text);
                string sAddress = CleanCellText(row.Cells[5].Text);
                string sStatus = CleanCellText(row.Cells[6].Text);

                if (e.CommandName.ToUpper() == "CHANGES")
                {
                    if (sStatus.ToUpper().Trim() == "RG")
                    {
                        txtExpedisiID.Text = sExpedisiID;
                        txtExpedisiName.Text = sExpedisiName;
                        txtPICName.Text = sPICName;
                        txtPhone.Text = sPhone;
                        txtEmail.Text = sEmail;
                        txtAddress.Text = sAddress;
                        txtExpedisiID.Enabled = false;
                        txtExpedisiID.Attributes.Add("disabled", "disabled");
                        CmdSubmit.Text = "Update";
                        div_comment.InnerHtml = "";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Expedition cannot be edited (Status: " + sStatus + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListExpedisi"], LblPaging, ViewState["RecListExpedisiFieldSort"].ToString(), ViewState["RecListExpedisiDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    if (CmdButton != null)
                        CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.Replace("'", "\\'") + "','" + e.Row.Cells[6].Text.Replace("'", "\\'") + "'); return false;";
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
                if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
                    clear();
                Open_GridView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtExpedisiIDDelete.Value.Trim()))
                    return;

                if (txtStatusDelete.Value.ToUpper().Trim() != "RG")
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Expedition cannot be removed (Status: " + txtStatusDelete.Value.Trim() + ")</div>";
                    return;
                }

                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                string id = txtExpedisiIDDelete.Value.Trim().Replace("'", "''");
                string usr = Session["ClsTypeUserID"] != null ? Session["ClsTypeUserID"].ToString().Trim().Replace("'", "''") : "";
                string strSQL = "sp_delete_expedisi '" + id + "','" + usr + "'";

                if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                {
                    clear();
                    Open_GridView();
                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Expedition has been removed successfully!</div>";
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrEmpty(sErr) ? "Delete failed." : sErr) + "</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                string sNewDirSort = ClType.Gv_Sorting(GridView1, Session["RecListExpedisi"], ViewState["RecListExpedisiFieldSort"].ToString(), ViewState["RecListExpedisiDirSort"].ToString(), e.SortExpression);
                ViewState["RecListExpedisiFieldSort"] = e.SortExpression;
                ViewState["RecListExpedisiDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }
    }
}
