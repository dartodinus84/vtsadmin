using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_selling_voucher : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_voucher '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListDeviceFieldSort"] = "autoid";
                ViewState["RecListDeviceDirSort"] = "DESC";
                Session["RecListDevice"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGOSALEVOUCHER"))
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
                //ClType.Open_Combos(CmbPriceID, Session["ClsTypeDBConnStringSQL"].ToString(), "'" + txtOrderID.Value.Trim() + "'", "sp_list_igo_master_price");

                txtOrderID.Value = "";
                txtSalesOrderDesc.Value = "";
                txtSellerDesc.Value = "";
                txtMarketingNameDesc.Value = "";
                txtSalesOrderDesc.Value = "";
                txtMarketingSourceDesc.Value = "";
               

                txtInvoice.Value = "";
                txtCustName.Value = "";
                txtCustAdd.Value = "";
                txtCustEmail.Value = "";
                txtCustAdd.Value = "";


                LblStockID.InnerHtml = "";
                txtStockIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
                //CmbPriceID.SelectedValue = "[Select]";
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
                    if (txtInvoice.Value.Trim() != "")
                    {
                        //if (CmbPriceID.SelectedItem.Value.Trim() != "[Select]")
                        //{
                        if (txtMarketingNameDesc.Value.Trim() != "")
                        {
                            if (txtCustName.Value.Trim() != "")
                            {
                                if (txtCustEmail.Value.Trim() != "")
                                {
                                    if (txtCustAdd.Value.Trim() != "")
                                    {
                                        //strSQL = "usp_retail_selling_voucher '" + CmbPriceID.SelectedItem.Value.ToString() + "','" + CmbMarketID.SelectedItem.Value.ToString() + "','" + txtNoInv.Text.Trim() + "','" + txtCustName.Text.Trim() + "','" + txtCustAdd.Text.Trim() + "','" + txtCustEmail.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        strSQL = "usp_retail_selling_voucher '" + txtOrderID.Value.Trim() + "','" + txtInvoice.Value.Trim() + "','" + txtCustName.Value.Trim() + "','" + txtCustAdd.Value.Trim() + "','" + txtCustEmail.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Selling Voucher has been save successfully!</div>";
                                                clear();
                                                Open_GridView();
                                            }
                                            else
                                            {
                                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + sErr + ")</div>";
                                            }
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Address</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Email</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Name</div>";
                            }
                            //}
                            //else
                            //{
                            //    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Market</div>";
                            //}
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Voucher</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Invoice Market</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Selling Voucher has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
            }
        }
        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDevice"], LblPaging, ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString());
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    //CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

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
                //Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete device has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListDevice"], ViewState["RecListDeviceFieldSort"].ToString(), ViewState["RecListDeviceDirSort"].ToString(), e.SortExpression);
                ViewState["RecListDeviceFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListDeviceDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
        //protected void CmbPriceID_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClsType ClType = new ClsType();
        //        ClType.Open_Combos(CmbPriceID, Session["ClsTypeDBConnStringSQL"].ToString(), txtOrderID.Value.Trim(), "sp_list_igo_master_price");
        //        div_comment.InnerHtml = "";
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}
