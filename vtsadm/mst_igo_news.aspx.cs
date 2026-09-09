using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_news : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListGenerateNewsFieldSort";
        string sViewStateDirSort = "RecListGenerateNewsDirSort";
        string sSessionRecList = "RecListGenerateNews";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_news '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "autoid";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGOMSTNEWS"))
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
                txtSearch.Text = "";
                txtTitle.Text = "";
                txtDetails.Text = "";
                txtNewsID.Text = "";
                Session["ClsTypeNewsImage"] = "";
                Session["ClsTypeNewsThumbnail"] = "";
                CmdSubmit.Text = "Submit";
                CmdSubmit.Visible = true;
                ClType.Open_Combos(CmbCompanyID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_companyid");
                CmbCompanyID.SelectedValue = "[Select]";

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
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    //if (txtNewsID.Text.Trim() != "")
                    //{
                        if (Session["ClsTypeNewsThumbnail"] != "")
                        {
                            if (Session["ClsTypeNewsImage"] != "")
                            {
                                if (CmbCompanyID.SelectedItem.Value.Trim() != "[Select]")
                                {
                                    var details = txtDetails.Text.Replace("\r\n", "<br />").Replace("\n", "<br />");
                                    strSQL = "usp_retail_create_generate_news '" + CmbCompanyID.SelectedItem.Value.ToString() + "','" + txtTitle.Text.Trim() + "','" + details + "','" + Session["ClsTypeNewsImage"] + "','" + Session["ClsTypeNewsThumbnail"] + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            Open_GridView();
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Editing News Igo Tracker has been save successfully!</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Customer</div>";
                                }
                                
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , Please Upload Image ! </div>";

                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , Please Upload Thumbnail ! </div>";
                        }
                    //}
                    //else
                    //{
                    //    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , News ID didn't selected </div>";
                    //}
                }
                else
                {
                    if (txtNewsID.Text.Trim() != "")
                    {
                        if (Session["ClsTypeNewsThumbnail"] != "")
                        {
                            if (Session["ClsTypeNewsImage"] != "")
                            {
                                if (CmbCompanyID.SelectedItem.Value.Trim() != "[Select]") {
                                    var details = txtDetails.Text.Replace("\r\n", "<br />").Replace("\n", "<br />");
                                    strSQL = "usp_retail_generate_news '" + txtNewsID.Text.Trim() + "','" + CmbCompanyID.SelectedItem.Value.ToString() + "','" + txtTitle.Text.Trim() + "','" + details + "','" + Session["ClsTypeNewsImage"] + "','" + Session["ClsTypeNewsThumbnail"] + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            Open_GridView();
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Editing News Igo Tracker has been save successfully!</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Customer</div>";
                                }

                                
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , Please Upload Image ! </div>";

                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , Please Upload Thumbnail ! </div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed , News ID didn't selected </div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Editing News Igo Tracker has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sNewsID = ""; string sTitle = ""; string sDetail = ""; string sCompanyID = "";
                sNewsID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sTitle = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sDetail = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sCompanyID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        txtNewsID.Text = sNewsID;
                        txtTitle.Text = sTitle;
                        txtDetails.Text = sDetail;
                        txtNewsID.Attributes.Add("disabled", "disabled");
                        CmbCompanyID.SelectedValue = sCompanyID;
                        CmdSubmit.Visible = true;
                        CmdSubmit.Text = "Update";
                        div_comment.InnerHtml = "";
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> News can not be edited or deleted, (" + ex.Message + ")</div>";
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
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;

                    e.Row.Cells[5].ToolTip = "Edit";

                    LinkButton CmdDelete = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'); return false;";

                    LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    CmdPic.OnClientClick = "postPic('" + e.Row.Cells[9].Text.ToString() + "'); return false;";

                    LinkButton CmdPic2 = (LinkButton)e.Row.FindControl("CmdPic2");
                    CmdPic2.OnClientClick = "postPic2('" + e.Row.Cells[10].Text.ToString() + "'); return false;";

                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;

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
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtPromoIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "1" || txtStatusDelete.Value.ToUpper().Trim() == "0")
                    {
                        strSQL = "usp_igo_generate_news_delete '" + txtPromoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> News has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing News has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing News has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Promo can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Promo has been failed (" + ex.Message + ")</div>";
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