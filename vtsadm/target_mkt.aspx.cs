using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class target_mkt : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListTargetMktFieldSort";
        string sViewStateDirSort = "RecListTargetMktDirSort";
        string sSessionRecList = "RecListTargetMkt";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_target_marketing '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "TargetMktID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUTARGETMKT"))
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
                txtTargetID.Text = "";
                ClType.Open_Combos(CmbYear, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_year");
                CmbYear.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMarketingID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_marketings");
                CmbMarketingID.SelectedValue = "[Select]";
                txtFrom.Value = "";
                txtTo.Value = "";
                txtValue.Text = "";
                txtRemark.Text = "";

                LblTargetMktID.InnerHtml = "";
                txtTargetMktIDDelete.Value = "";
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
                Int32 intAff = 0; String strSQL = "";string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    strSQL = "sp_insert_target_marketing '" + CmbYear.SelectedItem.Value.Trim() + "','" + CmbMarketingID.SelectedItem.Value.Trim() + "'," +
                             "'" + txtFrom.Value.Trim() + "','" + txtTo.Value.Trim() + "'," + txtValue.Text.Trim() + ",'" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Target marketing has been save successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving target marketing has been failed</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving target marketing has been failed (" + sErr + ")</div>";
                    }
                }
                else
                {
                    if (txtTargetID.Text.Trim() != "")
                    {
                        strSQL = "sp_update_target_marketing '" + txtTargetID.Text.Trim() + "','" + CmbYear.SelectedItem.Value.Trim() + "','" + CmbMarketingID.SelectedItem.Value.Trim() + "'," +
                             "'" + txtFrom.Value.Trim() + "','" + txtTo.Value.Trim() + "'," + txtValue.Text.Trim() + ",'" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Target marketing has been update successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update target marketing has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update target marketing has been failed (" + sErr + ")</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Insert or update target marketing has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sTargetID = ""; string sYear = ""; string sMarketingID = ""; string sPeriodFrom = "";string sPeriodTo = "";string sValue = "";
                string sRemark = "";
                sTargetID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sYear = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sPeriodFrom = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sPeriodTo = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sValue = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        txtTargetID.Text = sTargetID;
                        CmbYear.SelectedValue = sYear;
                        CmbMarketingID.SelectedValue = sMarketingID;
                        txtFrom.Value = sPeriodFrom;
                        txtTo.Value = sPeriodTo;
                        txtValue.Text = sValue;
                        txtRemark.Text = sRemark;
                        txtTargetID.Attributes.Add("disabled", "disabled");
                        CmdSubmit.Text = "Update";
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

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 11; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[9].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[10].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                    for (int i = 11; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
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
                if (txtTargetID.Text.Trim() != "")
                {
                    strSQL = "sp_delete_target_marketing '" + txtTargetID.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Target marketing has been remove successfully!</div>";
                        }
                        else
                        {
                            //txtError.Value = "Delete failed";
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing target marketing has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing target marketing has been failed (" + sErr + ")</div>";
                    }
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing target marketing has been failed (" + ex.Message + ")</div>";
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