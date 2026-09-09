using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;

using vtsadm.App_Code;
using System.Net.NetworkInformation;

namespace vtsadm
{
    public partial class po_edit : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public po_edit()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                //Session["ClsTypeMenuActive"] = "MNUDO";
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView();
                            Open_GridViewHeader();
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
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                //txtCustTypeDesc.Text = "";
                //txtCustBranchName.Text = "";
                txtPoID.Text = "";
                txtJobID.Text = "";
                //txtPoNumber.Text = "";
                txtSearch.Value = "";
                txtPoIDDelete.Value = "";
                txtStatusDelete.Value = "";
                txtDetailDeviceTypeDelete.Value = "";
                txtQuantity.Value = "";
                txtEditJobID.Value = "";
                // txtSeqClose.Value = "";
                CmdAddDetail.Visible = false;
                ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_po_device_type");
                CmbDeviceTypeID.SelectedValue = "[Select]";
                //ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_purchase_order_device_group");
                //ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbDeviceGroupID.SelectedItem.Value.ToString(), "sp_list_purchase_order_device_type");

            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_po_jo  '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreatePurchaseOrderHeaderFieldSort"] = "PoID";
                ViewState["RecCreatePurchaseOrderHeaderDirSort"] = "DESC";
                Session["RecCreatePurchaseOrderHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_po_jo_detail '" + txtPoID.Text.Trim() + "'";
                ViewState["RecCreatePurchaseOrderFieldSort"] = "PoID";
                ViewState["RecCreatePurchaseOrderDirSort"] = "DESC";
                Session["RecCreatePurchaseOrder"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString());
                SetJobIDFromDetail();
            }
            catch (Exception ex)
            {

            }
        }

        private string GetJobIDFromDetail(string deviceTypeID = "")
        {
            try
            {
                DataSet ds = Session["RecCreatePurchaseOrder"] as DataSet;
                if (ds == null || ds.Tables.Count == 0)
                {
                    return "";
                }

                DataTable dt = ds.Tables[0];
                if (!dt.Columns.Contains("JobID"))
                {
                    return "";
                }

                if (!string.IsNullOrWhiteSpace(deviceTypeID) && dt.Columns.Contains("DeviceTypeID"))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["DeviceTypeID"].ToString().Trim().Equals(deviceTypeID.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            return row["JobID"].ToString().Trim();
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["JobID"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        private void SetJobIDFromDetail()
        {
            try
            {
                ClsType ClType = new ClsType();
                string sJobID = GetJobIDFromDetail();
                txtJobID.Text = ClType.CheckNbsp(sJobID);
                Session["ClsJobID"] = txtJobID.Text.Trim();
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
                Open_GridView();
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdAddDetail_Click(object sender, EventArgs e)
        {

        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreatePurchaseOrder"], LblPagingDetail, ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }


        protected void CmdLoad_Click(object sender, EventArgs e)
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

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViewHeader();
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreatePurchaseOrderHeader"], LblPagingHeader, ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[5].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[3].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[4].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[2].Text.ToString() + "'); return false;";
                    e.Row.Cells[5].Visible = false;
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
                if (txtPoIDDelete.Value != "")
                {
                    strSQL = "sp_delete_purchase_create '" + txtPoIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        clear();
                        Open_GridViewHeader();
                        Open_GridView();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase order has been remove successfully!</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Removing purchase order has been failed." : sErr) + "</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be removed, PoID empty</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing purchase order has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                string sDeviceTypeID = txtDetailDeviceTypeDelete.Value.Trim();
                if (txtPoID.Text.Trim() != "" && sDeviceTypeID != "")
                {
                    strSQL = "sp_delete_purchase_order_detail '" + txtPoID.Text.Trim() + "','" + sDeviceTypeID + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        Open_GridView();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Purchase order detail berhasil dihapus.</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Delete purchase order detail has been failed." : sErr) + "</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> PoID atau Device Type tidak valid.</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete purchase order detail has been failed (" + ex.Message + ")</div>";
            }
        }

        private bool IsDeleteAllowed(object isDeleteVal)
        {
            if (isDeleteVal == null || isDeleteVal == DBNull.Value)
            {
                return false;
            }

            if (isDeleteVal is bool)
            {
                return (bool)isDeleteVal;
            }

            string sVal = isDeleteVal.ToString().Trim();
            return sVal == "1" || sVal.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ClsType ClType = new ClsType();
                    string sJobIDRaw = ClType.CheckNbsp(e.Row.Cells[1].Text.Trim());
                    string sJobID = sJobIDRaw.Replace("'", "\\'");
                    string sQty = ClType.CheckNbsp(e.Row.Cells[5].Text.Trim()).Replace("'", "\\'");
                    string sQtyDone = ClType.CheckNbsp(e.Row.Cells[6].Text.Trim()).Replace("'", "\\'");
                    string sDeviceTypeDesc = ClType.CheckNbsp(e.Row.Cells[4].Text.Trim()).Replace("'", "\\'");
                    string sDeviceTypeID = ClType.CheckNbsp(e.Row.Cells[11].Text.Trim()).Replace("'", "\\'");

                    LinkButton CmdEditQty = (LinkButton)e.Row.Cells[8].FindControl("CmdEditQty");
                    LinkButton CmdEditDevice = (LinkButton)e.Row.Cells[9].FindControl("CmdEditDevice");
                    CmdEditQty.OnClientClick = "confirmEditQuantity('" + sQty + "', '" + sDeviceTypeID + "', '" + sJobID + "', '" + sQtyDone + "'); return false;";
                    CmdEditDevice.OnClientClick = "confirmEditDevice('" + sDeviceTypeDesc + "', '" + sDeviceTypeID + "', '" + sJobID + "'); return false;";

                    LinkButton CmdDeleteDetail = (LinkButton)e.Row.Cells[10].FindControl("CmdDeleteDetail");
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    bool allowDelete = drv.Row.Table.Columns.Contains("IsDelete") && IsDeleteAllowed(drv["IsDelete"]);
                    if (allowDelete)
                    {
                        CmdDeleteDetail.Visible = true;
                        CmdDeleteDetail.OnClientClick = "confirmDeleteDetail('" + sDeviceTypeID + "'); return false;";
                    }
                    else
                    {
                        CmdDeleteDetail.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();

                string sPoID = "";
                string sCustID = "";
                string sFullName = "";
                string sStatus = "";

                sPoID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP" || sStatus.ToUpper().Trim() == "CL")
                        {
                            txtCustID.Value = ClType.CheckNbsp(sCustID);
                            txtCustFullName.Text = ClType.CheckNbsp(sFullName);
                            txtPoID.Text = ClType.CheckNbsp(sPoID);

                            Session["ClsPoID"] = txtPoID.Text.Trim();

                            Open_GridView();
                            CmdAddDetail.Visible = true;
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();


                //string sPoID = "";
                //string sJobID = "";
                //string sSeq = "";
                //string sDeviceGroupID = "";
                string sDeviceTypeID = "";
                // string sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                string sQuantity = "";
                string sQuantityDone = "";
                //string sStatus = "";


                //SESSION
                Session["ClsDeviceTypeID"] = sDeviceTypeID;


                //sDeviceGroupID = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sQuantity = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sQuantityDone = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();


            }
            catch (Exception ex)
            {

                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Error (" + ex.Message + ")</div>";

            }


        }

        protected void CmdYesEditQty_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                ClsType ClType = new ClsType();
                string sDeviceTypeID = txtDeviceTypeID.Value.Trim();
                string sJobID = ClType.CheckNbsp(txtEditJobID.Value.Trim());
                int iQuantity = 0;

                if (string.IsNullOrEmpty(sJobID))
                {
                    sJobID = GetJobIDFromDetail(sDeviceTypeID);
                }

                int.TryParse(txtQuantity.Value.Trim(), out iQuantity);

                strSQL = "sp_update_qty_purchase_order_detail '" + txtPoID.Text.Trim() + "','" + sJobID + "','" + sDeviceTypeID + "','" + iQuantity + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                {
                    Open_GridView();
                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Edited Quantity Request Purchase order has been successfully!</div>";
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Edit Quantity Request purchase order has been failed." : sErr) + "</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong>Edited purchase order detail has been failed (" + ex.Message + ")</div>";
            }
        }



        protected void CmdYesEditDeviceType_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                ClsType ClType = new ClsType();
                string sDeviceTypeIDOld = ClType.CheckNbsp(txtDeviceTypeID.Value.Trim());
                string sJobID = ClType.CheckNbsp(txtEditJobID.Value.Trim());
                string sDeviceTypeIDNew = CmbDeviceTypeID.SelectedItem == null ? "" : ClType.CheckNbsp(CmbDeviceTypeID.SelectedItem.Value.Trim());
                if (string.IsNullOrEmpty(sJobID))
                {
                    sJobID = GetJobIDFromDetail(sDeviceTypeIDOld);
                }

                strSQL = "sp_update_device_purchase_order_detail '" + txtPoID.Text.Trim() + "','" + sJobID + "','" + sDeviceTypeIDOld + "','" + sDeviceTypeIDNew + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                {
                    Open_GridView();
                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Edited Device Type has been successfully!</div>";
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + (string.IsNullOrWhiteSpace(sErr) ? "Edited Device Type has been failed." : sErr) + "</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong>Edited purchase order detail has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecCreatePurchaseOrder"], ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreatePurchaseOrderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreatePurchaseOrderDirSort"] = sNewDirSort;
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreatePurchaseOrderHeader"], ViewState["RecCreatePurchaseOrderHeaderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreatePurchaseOrderHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreatePurchaseOrderHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }

    }
}