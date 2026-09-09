using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class ref_area : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_group_area '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListAreaFieldSort"] = "AreaID";
                ViewState["RecListAreaDirSort"] = "ASC";
                Session["RecListArea"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListAreaFieldSort"].ToString(), ViewState["RecListAreaDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUREFAREA"))
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
                ClType.Open_Combos(CmbGroupAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_group_area");
                BindSupportArea("");

                txtAreaID.Text = "";
                txtAreaName.Text = "";
                CmbGroupAreaID.SelectedValue = "[Select]";
                CmbSupAreaID.SelectedValue = "[Select]";

                LblAreaID.InnerHtml = "";
                txtAreaIDDelete.Value = "";
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
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtAreaName.Text.Trim() != "")
                    {
                        strSQL = "sp_insert_ref_area '" + txtAreaName.Text.Trim() + "','" + CmbGroupAreaID.SelectedItem.Value.ToString() + "','" + CmbSupAreaID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Area has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Area has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Area has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Area Name</div>";
                    }
                }
                else
                {
                    if (txtAreaID.Text.Trim() != "")
                    {
                        strSQL = "sp_update_ref_area '" + txtAreaID.Text.Trim() + "','" + txtAreaName.Text.Trim() + "','" + CmbGroupAreaID.SelectedItem.Value.ToString() + "','" + CmbSupAreaID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Area has been update successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Area has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Area has been failed (" + sErr + ")</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update Area has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                String strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0;
                string sAreaID = "";
                string sGroupAreaID = "";
                string sSupAreaID = "";
                string sRegionName = "";
                String sAreaName = "";
                string sStatus = "";
                string sErr = "";

                GridView rowGrid = e.CommandSource as GridView;
                if (rowGrid != null && rowGrid.DataKeys[iRow] != null)
                {
                    sAreaID = rowGrid.DataKeys[iRow]["AreaID"].ToString().Trim();
                    sGroupAreaID = rowGrid.DataKeys[iRow]["AreaGroupID"].ToString().Trim();
                    sSupAreaID = rowGrid.DataKeys[iRow]["SupAreaID"].ToString().Trim();
                    sAreaName = rowGrid.DataKeys[iRow]["AreaName"].ToString().Trim();
                    sStatus = rowGrid.DataKeys[iRow]["Status"].ToString().Trim();
                    sRegionName = HttpUtility.HtmlDecode(rowGrid.Rows[iRow].Cells[2].Text).Trim();
                }

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            txtAreaID.Text = sAreaID;
                            CmbGroupAreaID.SelectedValue = sGroupAreaID;
                            BindSupportArea(CmbGroupAreaID.SelectedItem.Value.ToString());

                            ListItem selectedRegionById = CmbSupAreaID.Items.FindByValue(sSupAreaID);
                            if (selectedRegionById != null)
                            {
                                CmbSupAreaID.SelectedValue = selectedRegionById.Value;
                            }
                            else
                            {
                                // Fallback by text for legacy rows where SupAreaID in list/query is inconsistent.
                                ListItem selectedRegionByText = CmbSupAreaID.Items.FindByText(sRegionName);
                                if (selectedRegionByText != null)
                                {
                                    CmbSupAreaID.SelectedValue = selectedRegionByText.Value;
                                }
                            }
                            txtAreaName.Text = sAreaName;
                            txtAreaID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Area can not be edited, due to status code has been " + sStatus + "</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListArea"], LblPaging, ViewState["RecListAreaFieldSort"].ToString(), ViewState["RecListAreaDirSort"].ToString());
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
                if (e.Row.RowType == DataControlRowType.Header)
                {
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[5].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    string areaId = GridView2.DataKeys[e.Row.RowIndex]["AreaID"].ToString();
                    string status = GridView2.DataKeys[e.Row.RowIndex]["Status"].ToString();
                    CmdButton.OnClientClick = "confirmDelete('" + areaId + "','" + status + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BindSupportArea(string areaGroupId)
        {
            try
            {
                Recordset rec = new Recordset();
                string selectedGroupId = areaGroupId == "[Select]" ? "" : areaGroupId;
                string strSQL = "sp_get_support_area '" + selectedGroupId.Replace("'", "''") + "'";
                rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

                CmbSupAreaID.Items.Clear();
                CmbSupAreaID.Items.Add(new ListItem("[Select]", "[Select]"));

                if (rec.RecordCount() > 0)
                {
                    rec.MoveFirst();
                    while (!rec.EOF)
                    {
                        string supAreaId = rec.Fields("SupAreaID");
                        string supAreaName = rec.Fields("Name");

                        // Fallback to column index to handle provider/alias differences.
                        if (string.IsNullOrWhiteSpace(supAreaId))
                        {
                            supAreaId = rec.Fields(0);
                        }
                        if (string.IsNullOrWhiteSpace(supAreaName))
                        {
                            supAreaName = rec.Fields(2);
                        }

                        if (!string.IsNullOrWhiteSpace(supAreaId))
                        {
                            ListItem lst = new ListItem();
                            lst.Text = (supAreaName ?? "").Trim();
                            lst.Value = supAreaId.Trim();
                            CmbSupAreaID.Items.Add(lst);
                        }
                        rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Load Region failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmbGroupAreaID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSupportArea(CmbGroupAreaID.SelectedItem.Value.ToString());
                div_comment.InnerHtml = "";
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
                if (txtAreaIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_ref_area '" + txtAreaIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Area has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Area has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Area has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Area can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Area has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListArea"], ViewState["RecListAreaFieldSort"].ToString(), ViewState["RecListAreaDirSort"].ToString(), e.SortExpression);
                ViewState["RecListAreaFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListAreaDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}