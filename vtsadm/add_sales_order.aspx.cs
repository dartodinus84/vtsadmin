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
using System.Data.SqlClient;

using vtsadm.App_Code;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Web.Script.Serialization;

namespace vtsadm
{
    public partial class add_sales_order : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public add_sales_order()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJURNALMSTSO"))
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
                            Open_GridViewSo();
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
                txtCustName.Value = "";
                txtPersonId.Text = "";
                txtCompId.Text = "";
                txtSalesOrderId.Text = "";
                txtAddr.Text = "";
                txtEmail.Text = "";
                txtTags.Text = "";
                txtTax.Text = "";
                txtTerm.Text = "";
                txtRate.Text = "";
                ClType.Open_Combos(selProduct, Session["ClsTypeDBConnStringSQL"].ToString(),"", "usp_get_product_jurnal");
                selProduct.SelectedValue = "[Select]";
                ClType.Open_Combos(selGpsType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "usp_get_gps_type_all");
                selGpsType.SelectedValue = "[Select]";
                txtSearch.Value = "";
                CmdSubmit.Text = "Submit";
                CmdCreate.Visible = true;
                //CmdLoad.Visible = false;
                CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        protected void Open_GridViewSo()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_sales_order_jurnal '" + txtSearch.Value.Trim() + "'";
                ViewState["RecSalesOrderFieldSort"] = "sales_order_id";
                ViewState["RecSalesOrderDirSort"] = "DESC";
                Session["RecSalesOrder"] = ClType.Open_GridView(GridViewSo, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecSalesOrderFieldSort"].ToString(), ViewState["RecSalesOrderDirSort"].ToString());
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
                Open_GridViewSo();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewSo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            //ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreatePurchaseOrder"], LblPagingDetail, ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                var tags = txtTags.Text.Trim();
                ExecCommand ec = new ExecCommand();
                if (txtCustName.Value.Trim() != "")
               {
                  if (selProduct.SelectedItem.Value.Trim() != "[Select]")
                  {

                        strSQL = "usp_insert_update_sales_order_jurnal 0," + txtCompId.Text.Trim() + "," + txtPersonId.Text.Trim()
                            + ",'" + txtCustName.Value.Trim() + "','" + txtAddr.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + selProduct.SelectedItem.Value.Trim()
                            + "'," + txtRate.Text.Trim() + "," + txtQty.Text.Trim() + ", 16966, 'PPN', 'percent', 'percent', 'Custom', '" + tags + "','" + selGpsType.SelectedItem.Value.Trim()
                            + "','" + Session["ClsTypeUserID"].ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewSo();

                                CmdCreate.Visible = true;
                                CmdSubmit.Visible = false;
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit sales order has been successfully</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit purchase order has been failed (" + sErr + ")</div>";
                            }
                        }
                    }
                  else
                  {
                      div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Product Name</div>";
                  }  
               }
               else
               {
                   div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select in customer name</div>";
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
                Open_GridViewSo();
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
                int intAff = 0; string strSQL = ""; string sErr = ""; int autoid;
                var tags = txtTags.Text.Trim();
                ExecCommand ec = new ExecCommand();
                if (txtPersonId.Text.Trim() != "")
                {
                    if (txtPersonId.Text.Trim() != "")
                    {
                        strSQL = "usp_insert_update_sales_order_jurnal " + txtSalesOrderId.Text.Trim() + "," + txtCompId.Text.Trim() + "," + txtPersonId.Text.Trim()
                            + ",'" + txtCustName.Value.Trim() + "','" + txtAddr.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + selProduct.SelectedItem.Value.Trim()
                            + "'," + txtRate.Text.Trim() + "," + txtQty.Text.Trim() + ", 16966, 'PPN', 'percent', 'percent', 'Custom', '" + tags + "','" + selGpsType.SelectedItem.Value.Trim()
                            + "','" + Session["ClsTypeUserID"].ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {

                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit sales order has been successfully</div>";
                                clear();
                                Open_GridViewSo();
                                
                                CmdCreate.Visible = true;
                                CmdSubmit.Visible = false;
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit sales order has been failed (" + sErr + ")</div>";
                            }
                        }
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select in customer name</div>";
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
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewSo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridViewSo_RowDataBound(object sender, GridViewRowEventArgs e)
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
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[11].ToolTip = "Edit";
                    //LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    //CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
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

        protected void GridViewSo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustName = ""; string sPersonId= ""; string sCompId = ""; string sSalesOrderId = ""; string sAddr = "";
                string sEmail= ""; string sTags = ""; string sQty = ""; string sRate = "";string sProduct= ""; string sGpsType = "";
                sSalesOrderId = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sCompId = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sPersonId = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sCustName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sAddr = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sEmail = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                sProduct = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sQty = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sRate = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sTags = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sGpsType = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        txtCustName.Value = sCustName;
                        txtPersonId.Text = sPersonId;
                        txtCompId.Text = sCompId;
                        txtSalesOrderId.Text = sSalesOrderId;
                        txtAddr.Text = sAddr;
                        txtEmail.Text = sEmail;
                        txtTags.Text = sTags;
                        txtRate.Text = sRate;
                        txtQty.Text = sQty;
                        selProduct.SelectedValue = sProduct;
                        selGpsType.SelectedValue = sGpsType;
                        
                        Open_GridViewSo();
                        CmdCreate.Visible = false;
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Purchase order can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }

        protected void GridViewSo_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                //string sNewDirSort = ClTye.Gv_Sorting(GridViewSo, Session["RecCreatePurchaseOrder"], ViewState["RecCreatePurchaseOrderFieldSort"].ToString(), ViewState["RecCreatePurchaseOrderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreatePurchaseOrderFieldSort"] = e.SortExpression.ToString();
                //ViewState["RecCreatePurchaseOrderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}
