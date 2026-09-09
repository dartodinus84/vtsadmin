using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_expeditures_approve : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_request_procurement_approve '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListReqAppFieldSort"] = "ProcurementID";
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
                string strSQL = "sp_list_delivery_request_detail '" + txtProcurementID.Value.Trim() + "'";
                ViewState["RecListReqDetailFieldSort"] = "ProcurementID";
                ViewState["RecListReqDetailDirSort"] = "ASC";
                Session["RecListReqDetail"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecListReqDetailFieldSort"].ToString(), ViewState["RecListReqDetailDirSort"].ToString());

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUAPPREQTLS"))
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

                txtProcurementID.Value = "";
                txtProcurementDesc.Value = "";
                txtBranchDesc.Value = "";
                txtWarehouseDesc.Value = "";
                txtTechnicanDesc.Value = "";
                txtMarketingDesc.Value = "";
                txtCustomerDesc.Value = "";

                CmdSubmit.Text = "Submit";

                CmdUploadGSMMutation.Visible = false;
                CmdUploadDeviceMutation.Visible = false;
                CmdSubmit.Visible = false;
                CmdCreate.Visible = true;

                Session["ClsProcurementID"] = "";


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
                if (txtProcurementID.Value.Trim() != "")
                {
                    Session["ClsProcurementID"] = txtProcurementID.Value.Trim();
                    CmdCreate.Visible = false;
                    //CmdSubmit.Visible = true;
                    CmdUploadGSMMutation.Visible = true;
                    CmdUploadDeviceMutation.Visible = true;
                    Open_GridViewDetail();
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

                //if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                //{
                //    if (txtProcurementID.Value.Trim() != "")
                //    {
                //        if (txtQtyFinal12V.Text.Trim() != "" && qty12v >= qty12vfinal)
                //        {
                //            if (txtQtyFinal24V.Text.Trim() != "" && qty24v >= qty24vfinal)
                //            {
                //                if (txtQtyFinalDevice.Text.Trim() != "" && qtydevice >= qtydevicefinal)
                //                {

                //                    strSQL = "sp_update_procurement_approve '" + txtProcurementID.Value.Trim() + "','" + txtQtyFinalDevice.Text.Trim() + "','0','" + txtQtyFinal12V.Text.Trim() + "','" + txtQtyFinal12V.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                //                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                //                    {
                //                        if (intAff > 0)
                //                        {
                //                            clear();
                //                            Open_GridView();
                //                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong>  Approve Procurement has been save successfully!</div>";
                //                        }
                //                        else
                //                        {
                //                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong>Approve Procurement has been failed</div>";
                //                        }
                //                    }
                //                    else
                //                    {
                //                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Procurement has been failed (" + sErr + ")</div>";
                //                    }

                //                }
                //                else
                //                {
                //                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of Device or Quantity Device Final has been bigger than request</div>";
                //                }
                //            }
                //            else
                //            {
                //                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of Relay 12 V or Quantity 12 V Final has been bigger than request</div>";
                //            }
                //        }
                //        else
                //        {
                //            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of Relay 24 V or Quantity 24 V Final has been bigger than request</div>";
                //        }
                //    }
                //    else
                //    {
                //        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Select Procurement ID</div>";
                //    }
                //}
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update device has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListReqApp"], LblPaging, ViewState["RecListReqAppFieldSort"].ToString(), ViewState["RecListReqAppDirSort"].ToString());
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListReqApp"], ViewState["RecListReqAppFieldSort"].ToString(), ViewState["RecListReqAppDirSort"].ToString(), e.SortExpression);
                ViewState["RecListReqAppFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListReqAppDirSort"] = sNewDirSort;
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

    }
}
