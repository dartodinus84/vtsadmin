using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_transfer_approve_gsm : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_transfer_approve_gsm '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListProcurementApproveHeaderFieldSort"] = "ProcurementID";
                ViewState["RecListProcurementApproveHeaderDirSort"] = "DESC";
                Session["RecListProcurementApproveHeader"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListProcurementApproveHeaderFieldSort"].ToString(), ViewState["RecListProcurementApproveHeaderDirSort"].ToString());

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUAPPTFGSM"))
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
                txtWarehouseDesc.Value = "";
                txtDeviceDesc.Value = "";
                txtQtyDevice.Value = "";
                txtGSMDesc.Value = "";
                txtQtyGSM.Value = "";
                txtQtyRelay12V.Value = "";
                txtQtyRelay24V.Value = "";

                txtQtyFinalGsm.Text = "0";
                txtQtyFinal12V.Text = "0";
                txtQtyFinal24V.Text = "0";

                int qtydevice = 0;
                int qtygsm = 0;
                int qty12v = 0;
                int qty24v = 0;

                int qtydevicefinal = 0;
                int qtygsmfinal = 0;
                int qty12vfinal = 0;
                int qty24vfinal = 0;


                CmdSubmit.Text = "Submit";
                
                CmdUploadGSMMutation.Visible = false;
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
                    Session["ClsTransferID"] = txtProcurementID.Value.Trim();
                    CmdCreate.Visible = false;
                    CmdSubmit.Visible = true;
                    CmdUploadGSMMutation.Visible = true;


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
                int qtydevice = Convert.ToInt32(txtQtyDevice.Value.Trim());
                int qtygsm = Convert.ToInt32(txtQtyGSM.Value.Trim());
                int qty12v = Convert.ToInt32(txtQtyRelay12V.Value.Trim());
                int qty24v = Convert.ToInt32(txtQtyRelay24V.Value.Trim());

                int qtygsmfinal = Convert.ToInt32(txtQtyFinalGsm.Text.Trim());
                int qty12vfinal = Convert.ToInt32(txtQtyFinal12V.Text.Trim());
                int qty24vfinal = Convert.ToInt32(txtQtyFinal24V.Text.Trim());

                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtProcurementID.Value.Trim() != "")
                    {
                        if (txtQtyFinal12V.Text.Trim() != "" && qty12v >= qty12vfinal)
                        {
                            if (txtQtyFinal24V.Text.Trim() != "" && qty24v >= qty24vfinal)
                            {

                                if (txtQtyFinalGsm.Text.Trim() != "" && qtygsm >= qtygsmfinal)
                                {
                                    strSQL = "sp_update_procurement_approve '" + txtProcurementID.Value.Trim() + "','0','" + txtQtyFinalGsm.Text.Trim() + "','" + txtQtyFinal12V.Text.Trim() + "','" + txtQtyFinal12V.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            Open_GridView();
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong>  Approve Transfer has been save successfully!</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong>Approve Transfer has been failed</div>";
                                        }
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve Transfer has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of GSM or Quantity GSM Final has been bigger than request</div>";
                                }

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of Relay 12 V or Quantity 12 V Final has been bigger than request</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Fill Quantity Final of Relay 24 V or Quantity 24 V Final has been bigger than request</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please Select Transfer ID</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update Transfer has been failed (" + ex.Message + ")</div>";
            }
        }


        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListProcurementApproveHeader"], LblPaging, ViewState["RecListProcurementApproveHeaderFieldSort"].ToString(), ViewState["RecListProcurementApproveHeaderDirSort"].ToString());
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

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListProcurementApproveHeader"], ViewState["RecListProcurementApproveHeaderFieldSort"].ToString(), ViewState["RecListProcurementApproveHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecListProcurementApproveHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListProcurementApproveHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
    }
}
