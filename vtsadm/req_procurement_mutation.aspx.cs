using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_procurement_mutation : System.Web.UI.Page
    {

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_request_procurement '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListReqFieldSort"] = "ProcurementID";
                ViewState["RecListReqDirSort"] = "DESC";
                Session["RecListReq"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListReqFieldSort"].ToString(), ViewState["RecListReqDirSort"].ToString());

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
                string strSQL = "sp_list_delivery_request_detail '" + txtReqID.Text.Trim() + "'";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUREQTLS"))
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
                            Open_GridViewDetail();
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

                txtReqID.Text = "";
                txtReqDate.Text = "";
                CmbProcurement.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbProcurement, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_procurement_type");

                CmbBranchID.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbBranchID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_technician_branch");

                CmbWarehouse.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_warehouse_procurement");

                CmbTechnician.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_technician_procurement");

                CmbMarketing.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_marketing_procurement");

                CmbCust.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbCust, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_procurement");

                txtRemark.Text = "";


                CmbWarehouse.Attributes.Add("style", "display:none");
                LblCmbWarehouse.Attributes.Add("style", "display:none");

                CmbTechnician.Attributes.Add("style", "display:none");
                LblCmbTechnician.Attributes.Add("style", "display:none");

                CmbMarketing.Attributes.Add("style", "display:none");
                LblCmbMarketing.Attributes.Add("style", "display:none");

                CmbCust.Attributes.Add("style", "display:none");
                LblCmbCust.Attributes.Add("style", "display:none");

                CmdSubmit.Text = "Submit";
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdLoad.Visible = false;
                CmdSubmit.Visible = false;

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
                Open_GridViewDetail();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdAddDetail_Click(object sender, EventArgs e)
        {

        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListReqDetail"], LblPagingDetail, ViewState["RecListReqDetailFieldSort"].ToString(), ViewState["RecListReqDetailDirSort"].ToString());
            div_comment.InnerHtml = "";
        }
        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                div_comment.InnerHtml = "";
                if (txtReqDate.Text.Trim() != "")
                {
                    if (CmbProcurement.SelectedItem.Value.Trim() != "[Select]")
                    {
                        strSQL = "sp_insert_procurement '" + CmbProcurement.SelectedItem.Value.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbWarehouse.SelectedItem.Value.Trim() + "','" + CmbTechnician.SelectedItem.Value.Trim() + "','" + CmbMarketing.SelectedItem.Value.ToString() + "','" + CmbCust.SelectedItem.Value.ToString() + "','" + txtRemark.Text.Trim() + "','" + txtReqDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (!sErr.ToLower().Contains("duplicate"))
                            {
                                txtReqID.Text = sErr.Trim();
                                Session["ProcurementREQID"] = txtReqID.Text.Trim();
                                CmdCreate.Visible = false;
                                CmdAddDetail.Visible = true;
                                CmdSubmit.Visible = true;
                                CmdLoad.Visible = true;
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Request Order has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Order has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save Request Order has been failed (" + sErr + ")</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Date of Order Request</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewDetail();
            }
            catch (Exception ex)
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
                if (txtReqID.Text.Trim() != "")
                {
                    if (CmbProcurement.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                        {
                            strSQL = "sp_submit_request '" + txtReqID.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewDetail();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job order has been successfully</div>";
                                    Session["ProcurementREQID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job order has been failed (" + sErr + ")</div>";
                                }
                            }
                        }
                        else
                        {
                            strSQL = "sp_update_request '" + txtReqID.Text.Trim() + "','" + CmbProcurement.SelectedItem.Value.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbWarehouse.SelectedItem.Value.Trim() + "','" + CmbTechnician.SelectedItem.Value.Trim() + "','" + CmbCust.SelectedItem.Value.ToString() + "','" + CmbMarketing.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + txtReqDate.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    Open_GridViewDetail();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update Request Order has been successfully</div>";
                                    Session["ProcurementREQID"] = "";
                                    CmdCreate.Visible = true;
                                    CmdAddDetail.Visible = false;
                                    CmdSubmit.Visible = false;
                                    CmdLoad.Visible = false;
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Request Order has been failed (" + sErr + ")</div>";
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListReq"], LblPaging, ViewState["RecListReqFieldSort"].ToString(), ViewState["RecListReqDirSort"].ToString());
            div_comment.InnerHtml = "";

        }
        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 11; i <= 17; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 11; i <= 17; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[9].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[10].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "'); return false;";

                }

            }
            catch (Exception ex)
            {
            }
        }
        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtReqIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_procurement '" + txtReqIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewDetail();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Request Order has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Request Order has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Request Order has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Request Order can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Request Order has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtReqID.Text.Trim() != "" && txtSeqDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_procurement_detail '" + txtReqID.Text.Trim() + "'," + txtSeqDelete.Value.Trim() + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            Open_GridViewDetail();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Request Order detail has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Request Order detail has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Request Order detail has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete Request Order detail has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDeleteDetail");
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + e.Row.Cells[1].Text.ToString() + "'); return false;";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sProcurementID = ""; string sProcurementType = ""; string sBranchName = ""; string sWarehouseName = ""; string sTechName = "";
                string sMark = ""; string sCust = ""; string sSchDate = ""; string sStatus = ""; string sRemarks = "";
                string sProcurementTypeID = ""; string sBranchID = ""; string sWarehouseID = ""; string sTechnicianID = ""; string sCustID = "";
                string sMarketingID = ""; string sCusID = "";


                sProcurementID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();

                sRemarks = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sProcurementType = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sWarehouseID = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sTechnicianID = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sCusID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtReqID.Text = ClType.CheckNbsp(sProcurementID);
                            txtReqID.Attributes.Add("disabled", "disabled");
                            CmbProcurement.SelectedValue = sProcurementType;
                            if (sProcurementType.Equals("REQIN00001"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:none");
                                LblCmbWarehouse.Attributes.Add("style", "display:none");

                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                CmbMarketing.Attributes.Add("style", "display:block");
                                LblCmbMarketing.Attributes.Add("style", "display:block");

                                CmbCust.Attributes.Add("style", "display:block");
                                LblCmbCust.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00003"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");

                                CmbTechnician.Attributes.Add("style", "display:block");
                                LblCmbTechnician.Attributes.Add("style", "display:block");

                                CmbMarketing.Attributes.Add("style", "display:none");
                                LblCmbMarketing.Attributes.Add("style", "display:none");

                                CmbCust.Attributes.Add("style", "display:none");
                                LblCmbCust.Attributes.Add("style", "display:none");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00004"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");

                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                CmbMarketing.Attributes.Add("style", "display:none");
                                LblCmbMarketing.Attributes.Add("style", "display:none");

                                CmbCust.Attributes.Add("style", "display:none");
                                LblCmbCust.Attributes.Add("style", "display:none");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00005"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:block");
                                LblCmbWarehouse.Attributes.Add("style", "display:block");

                                CmbTechnician.Attributes.Add("style", "display:block");
                                LblCmbTechnician.Attributes.Add("style", "display:block");

                                CmbMarketing.Attributes.Add("style", "display:none");
                                LblCmbMarketing.Attributes.Add("style", "display:none");

                                CmbCust.Attributes.Add("style", "display:none");
                                LblCmbCust.Attributes.Add("style", "display:none");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00006"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:none");
                                LblCmbWarehouse.Attributes.Add("style", "display:none");

                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                CmbMarketing.Attributes.Add("style", "display:block");
                                LblCmbMarketing.Attributes.Add("style", "display:block");

                                CmbCust.Attributes.Add("style", "display:block");
                                LblCmbCust.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00007"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:none");
                                LblCmbWarehouse.Attributes.Add("style", "display:none");

                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                CmbMarketing.Attributes.Add("style", "display:block");
                                LblCmbMarketing.Attributes.Add("style", "display:block");

                                CmbCust.Attributes.Add("style", "display:block");
                                LblCmbCust.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }
                            else if (sProcurementType.Equals("REQIN00008"))
                            {
                                CmbWarehouse.Attributes.Add("style", "display:none");
                                LblCmbWarehouse.Attributes.Add("style", "display:none");

                                CmbTechnician.Attributes.Add("style", "display:none");
                                LblCmbTechnician.Attributes.Add("style", "display:none");

                                CmbMarketing.Attributes.Add("style", "display:block");
                                LblCmbMarketing.Attributes.Add("style", "display:block");

                                CmbCust.Attributes.Add("style", "display:block");
                                LblCmbCust.Attributes.Add("style", "display:block");

                                if (sBranchID == "" || sBranchID == "&nbsp;" || sBranchID == "0") { sBranchID = "[Select]"; }
                                CmbBranchID.SelectedValue = sBranchID;

                                if (sWarehouseID == "" || sWarehouseID == "&nbsp;" || sWarehouseID == "0") { sWarehouseID = "[Select]"; }
                                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                                CmbWarehouse.SelectedValue = sWarehouseID;

                                if (sTechnicianID == "" || sTechnicianID == "&nbsp;" || sTechnicianID == "0") { sTechnicianID = "[Select]"; }
                                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                                CmbTechnician.SelectedValue = sTechnicianID;

                                if (sMarketingID == "" || sMarketingID == "&nbsp;" || sMarketingID == "0") { sMarketingID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                                CmbMarketing.SelectedValue = sMarketingID;

                                if (sCusID == "" || sCusID == "&nbsp;" || sCusID == "0") { sCusID = "[Select]"; }
                                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                                CmbCust.SelectedValue = sCusID;

                            }

                            txtReqDate.Text = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemarks);
                            Session["ProcurementREQID"] = txtReqID.Text.Trim();
                            Open_GridViewDetail();

                            CmdCreate.Visible = false;
                            CmdAddDetail.Visible = true;
                            CmdSubmit.Visible = true;
                            CmdLoad.Visible = true;
                            CmdSubmit.Text = "Update";

                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Request Order can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Request Order can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecListReq"], ViewState["RecListReqFieldSort"].ToString(), ViewState["RecListReqDirSort"].ToString(), e.SortExpression);
                ViewState["RecListReqFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListReqDirSort"] = sNewDirSort;

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
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
        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        private void sendEmail(string orderid)
        {

            try
            {
                //var pageData = new mst_igo_delivery_DO();
                //Recordset RecMail = new Recordset();
                //string strSQL = "sp_get_device_delivery '" + orderid + "'";
                //RecMail.Open(strSQL, pageData.DBConnstringSQL());
                //if (RecMail.RecordCount() > 0)
                //{
                //    RecMail.MoveFirst();
                //    while (!RecMail.EOF)
                //    {
                //        string sCustName = RecMail.Fields("seller_cust_name");
                //        string sCustEmail = RecMail.Fields("seller_cust_email");
                //        string sInvoice = RecMail.Fields("seller_invoice");
                //        string sSN = RecMail.Fields("gps_sn");
                //        string sGSM = RecMail.Fields("gsm_no");
                //        string sVoucher = RecMail.Fields("voucher_code");
                //        notifEmail(sCustName, sCustEmail, sInvoice, sSN, sGSM, sVoucher);
                //        RecMail.MoveNext();
                //    }
                //}
            }

            catch (Exception ex)
            {

            }
        }
        private void notifEmail(string sCustName, string sCustEmail, string sInvoice, string sSN, string sGSM, string sVoucher)
        {
            string emailpass = "";
            string emailto = "";
            string emailto2 = "";
            string emailto3 = "";
            string emailfrom = "";
            string emailsetings = "";

            try
            {

                emailto = sCustEmail;

                string strSQLemail2 = "sp_list_par_global 'EmailFromIGO'";
                Recordset Rec2 = new Recordset();
                Rec2.Open(strSQLemail2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec2.RecordCount() > 0)
                {
                    string rec2 = Rec2.Fields("ParValue");
                    emailfrom = rec2;
                }

                string strSQLemail3 = "sp_list_par_global 'EmailPassIGO'";
                Recordset Rec3 = new Recordset();
                Rec3.Open(strSQLemail3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec3.RecordCount() > 0)
                {
                    string rec3 = Rec3.Fields("ParValue");
                    emailpass = rec3;
                }

                string strSQLemail4 = "sp_list_par_global 'EmailSettings'";
                Recordset Rec4 = new Recordset();
                Rec4.Open(strSQLemail4, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec4.RecordCount() > 0)
                {
                    string rec4 = Rec4.Fields("ParValue");
                    emailsetings = rec4;
                }

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(emailsetings);
                SmtpServer.Host = emailsetings;
                SmtpServer.UseDefaultCredentials = false;

                mail.IsBodyHtml = true;
                mail.From = new MailAddress(emailfrom);
                foreach (var address in emailto.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(new MailAddress(address));
                }
                mail.Subject = " Activation - IGO TRACKER ";
                mail.Body = BodyEmail(sCustName, sCustEmail, sInvoice, sSN, sGSM, sVoucher);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailfrom, emailpass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        private string BodyEmail(string sCustName, string sCustEmail, string sInvoice, string sSN, string sGSM, string sVoucher)
        {
            string Body = "", Header = "";
            string ContentMail = "";

            Header += "<div style='display:block;width:600px;max-width:100%;height:auto;border-radius:5px;box-sizing:border-box;margin:30px auto;border:1px solid #ddd;padding:30px'>";
            Header += "<div style='width:100%;margin:15px auto'>";
            Header += "<img style='width:100%;display:block' src='https://easygo-gps.co.id/images/header_email_igo.jpg' alt='' class='CToWUd a6T' tabindex='0'><div class='a6S' dir='ltr' style='opacity: 0.01; left: 665px; top: 297.85px;'>";
            Header += "<div id=':z8' class='T-I J-J5-Ji aQv T-I-ax7 L3 a5q' role='button' tabindex='0' aria-label='Download lampiran ' data-tooltip-class='a1V' data-tooltip='Download'>";
            Header += "<div class='aSK J-J5-Ji aYr'></div>";
            Header += "</div>";
            Header += "</div>";
            Header += "</div>";


            Header += "<div style='margin:30px auto;background:#fff;padding:0px;border-radius:5px;width:600px;max-width:100%'>";
            Header += "<span class='im'>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Dear, <b>" + sCustName + "</b></p>";
            Header += "<p style='color:#333;line-height:1.58em;text-align:left'>Terimakasih telah melakukan pembelian Device iGO TRACKER silakan melakukan aktivasi kendaraan.</p>";
            Header += "</span>";

            Header += "<div style='padding:0px;border-top:1px solid #f5f5f5;border-bottom:1px solid #f5f5f5' >";
            Header += "<h4 style='font-size:18px;line-height:1.58em;text-align:left;color:#00ab6b;margin:15px 0px'>Pembelian Device iGO TRACK";
            Header += "<table>";
            Header += "<tbody>";

            Header += "<tr style='vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>No Invoice</ td >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'> " + sInvoice + " </ td >";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Serial Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sSN + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>GSM Number</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sGSM + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'>Voucher Code</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 10px;color:#333;font-weight:normal;white-space:unset'>&nbsp;</td>";
            Header += "<td style = 'margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset'><span>" + sVoucher + "</span></td>";
            Header += "</tr>";

            Header += "<tr style = 'vertical-align:top' >";
            Header += "<td style='margin:2.5px 5px 0px 0px;font-size:14px;padding:0px 0px;color:#333;font-weight:normal;white-space:unset' colspan='3'>Disclaimer : Aktivasi voucher di lakukan setelah GPS berhasil terinstall pada kendaraan. Pastikan SN & GSM yang di input sesuai dengan data pada email di atas.</td>";
            Header += "</tr>";

            Header += "</tbody>";
            Header += "</table>";
            Header += "</h4>";
            Header += "</div>";
            string Mail = Header + Body + ContentMail;
            return Mail;
        }
        protected void CmbProcurement_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00001"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:block");
                    LblCmbMarketing.Attributes.Add("style", "display:block");

                    CmbCust.Attributes.Add("style", "display:block");
                    LblCmbCust.Attributes.Add("style", "display:block");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00003"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:block");
                    LblCmbTechnician.Attributes.Add("style", "display:block");

                    CmbMarketing.Attributes.Add("style", "display:none");
                    LblCmbMarketing.Attributes.Add("style", "display:none");

                    CmbCust.Attributes.Add("style", "display:none");
                    LblCmbCust.Attributes.Add("style", "display:none");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00004"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:none");
                    LblCmbMarketing.Attributes.Add("style", "display:none");

                    CmbCust.Attributes.Add("style", "display:none");
                    LblCmbCust.Attributes.Add("style", "display:none");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00005"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:block");
                    LblCmbWarehouse.Attributes.Add("style", "display:block");

                    CmbTechnician.Attributes.Add("style", "display:block");
                    LblCmbTechnician.Attributes.Add("style", "display:block");

                    CmbMarketing.Attributes.Add("style", "display:none");
                    LblCmbMarketing.Attributes.Add("style", "display:none");

                    CmbCust.Attributes.Add("style", "display:none");
                    LblCmbCust.Attributes.Add("style", "display:none");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00006"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:block");
                    LblCmbMarketing.Attributes.Add("style", "display:block");

                    CmbCust.Attributes.Add("style", "display:block");
                    LblCmbCust.Attributes.Add("style", "display:block");
                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00007"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:block");
                    LblCmbMarketing.Attributes.Add("style", "display:block");

                    CmbCust.Attributes.Add("style", "display:block");
                    LblCmbCust.Attributes.Add("style", "display:block");

                }
                else if (Convert.ToString(CmbProcurement.SelectedItem.Value).Equals("REQIN00008"))
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:block");
                    LblCmbMarketing.Attributes.Add("style", "display:block");

                    CmbCust.Attributes.Add("style", "display:block");
                    LblCmbCust.Attributes.Add("style", "display:block");
                }
                else
                {
                    CmbWarehouse.Attributes.Add("style", "display:none");
                    LblCmbWarehouse.Attributes.Add("style", "display:none");

                    CmbTechnician.Attributes.Add("style", "display:none");
                    LblCmbTechnician.Attributes.Add("style", "display:none");

                    CmbMarketing.Attributes.Add("style", "display:none");
                    LblCmbMarketing.Attributes.Add("style", "display:none");

                    CmbCust.Attributes.Add("style", "display:none");
                    LblCmbCust.Attributes.Add("style", "display:none");
                }

                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbBranchID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbWarehouse, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_warehouse_procurement");
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_marketing_procurement");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbWarehouseID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbTechnician, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_technician_procurement");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbMarketingID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbCust, Session["ClsTypeDBConnStringSQL"].ToString(), CmbMarketing.SelectedItem.Value.ToString(), "sp_list_customer_procurement");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

    }
}
