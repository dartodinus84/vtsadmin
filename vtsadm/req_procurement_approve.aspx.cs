using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_procurement_approve : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_do_procurement_approve '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListReqAppFieldSort"] = "DloID";
                ViewState["RecListReqAppDirSort"] = "DESC";
                Session["RecListReqApp"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListReqAppFieldSort"].ToString(), ViewState["RecListReqAppDirSort"].ToString());

            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewDetail()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_delivery_request_detail_approve '" + txtDloID.Value.Trim() + "'";
                ViewState["RecListReqDetailFieldSort"] = "DloID";
                ViewState["RecListReqDetailDirSort"] = "ASC";
                Session["RecListReqDetail"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecListReqDetailFieldSort"].ToString(), ViewState["RecListReqDetailDirSort"].ToString());

            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewDetailDevice()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_delivery_request_detail_approve_device '" + txtDloID.Value.Trim() + "'";
                ViewState["RecListReqDetail2FieldSort"] = "DloID";
                ViewState["RecListReqDetail2DirSort"] = "ASC";
                Session["RecListReqDetail2"] = ClType.Open_GridView(GridView3, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecListReqDetail2FieldSort"].ToString(), ViewState["RecListReqDetail2DirSort"].ToString());

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUAPPDLOTLS"))
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
                            div_comment.InnerHtml = "";
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

                txtDloID.Value = "";
                txtPurID.Value = "";
                txtDloDate.Value = "";
                txtRemark.Value = "";

                CmdSubmit.Text = "Submit";

                CmdUploadGSMMutation.Visible = false;
                CmdUploadDeviceMutation.Visible = false;
                CmdSubmit.Visible = false;
                CmdCreate.Visible = true;

                Session["ClsProcurementAppID"] = "";


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
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtDloID.Value.Trim() != "")
                {
                    Session["ClsProcurementAppID"] = txtDloID.Value.Trim();
                    CmdCreate.Visible = false;
                    //CmdSubmit.Visible = true;
                    CmdUploadGSMMutation.Visible = true;
                    CmdUploadDeviceMutation.Visible = true;
                    Open_GridViewDetail();
                    Open_GridViewDetailDevice();
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select sales order</div>";
                }
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
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListExpedituresApproveHeader"], LblPaging, ViewState["RecListExpedituresApproveHeaderFieldSort"].ToString(), ViewState["RecListExpedituresApproveHeaderDirSort"].ToString());
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
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListExpedituresApproveHeader"], ViewState["RecListExpedituresApproveHeaderFieldSort"].ToString(), ViewState["RecListExpedituresApproveHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecListExpedituresApproveHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListExpedituresApproveHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
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
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListReqDetail"], LblPagingDetail, ViewState["RecListReqDetailFieldSort"].ToString(), ViewState["RecListReqDetailDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecListReqDetail"], ViewState["RecListReqDetailFieldSort"].ToString(), ViewState["RecListReqDetailDirSort"].ToString(), e.SortExpression);
                ViewState["RecListReqDetailFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListReqDetailDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListReqDetail2"], LblPagingDetail2, ViewState["RecListReqDetail2FieldSort"].ToString(), ViewState["RecListReqDetail2DirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView3_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView3, Session["RecListReqDetail2"], ViewState["RecListReqDetail2FieldSort"].ToString(), ViewState["RecListReqDetail2DirSort"].ToString(), e.SortExpression);
                ViewState["RecListReqDetail2FieldSort"] = e.SortExpression.ToString();
                ViewState["RecListReqDetail2DirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView3_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
