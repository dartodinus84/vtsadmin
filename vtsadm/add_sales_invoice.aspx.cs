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
    public partial class add_sales_invoice : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;
        public add_sales_invoice()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJURNALGNRTINVV1"))
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
                txtCustName1.Text = "";
                txtAddr.Text = "";
                txtEmail.Text = "";
                txtTransactionNo.Text = "";
                txtDueDate.Text = "";
                txtShipingDate.Text = "";
                txtMessage.Text = "";
                txtMemo.Text = "";
                txtTags.Text = "";
                txtTax.Text = "";
                txtTerm.Text = "";
                txtRate.Text = "";
                txtSalesOrderId.Value = "";
                txtCompId.Text = "";
                txtPersonId.Text = "";
                ClType.Open_Combos(selProduct, Session["ClsTypeDBConnStringSQL"].ToString(), "", "usp_get_product_jurnal");
                selProduct.SelectedValue = "[Select]";
                ClType.Open_Combos(selGpsType, Session["ClsTypeDBConnStringSQL"].ToString(),"", "usp_get_gps_type_all");
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
        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_invoice_jurnal '" + txtSearch.Value.Trim() + "'";
                ViewState["RecCreateInvoiceFieldSort"] = "invoice_id";
                ViewState["RecCreateInvoiceHeaderDirSort"] = "DESC";
                Session["RecCreateInvoiceHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreateInvoiceFieldSort"].ToString(), ViewState["RecCreateInvoiceHeaderDirSort"].ToString());
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
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }

        private object AddSalesInvoice()
        {
            try
            {
                var detail = new List<detailTransaction>();
                var data_detail = new detailTransaction
                {
                    quantity = 1,
                    rate = Convert.ToInt32(txtRate.Text.Trim()),
                    discount = 0,
                    product_name = selProduct.SelectedItem.Value.Trim(),
                    line_tax_id = 16966,
                    line_tax_name = txtTax.Text.Trim()
                };
                detail.Add(data_detail);
                string tagsName = txtTags.Text.ToString();
                List<string> tags = new List<string>(
                tagsName.Split(new string[] { "," }, StringSplitOptions.None));

                var paramSales = new paramAddSales
                {
                    transaction_date = DateTime.Now.ToString("yyyy-MM-dd"),
                    transaction_lines_attributes = detail.ToList(),
                    shipping_date = DateTime.Now.ToString("yyyy-MM-dd"),
                    shipping_price = 0,
                    shipping_address = "",
                    is_shipped = false,
                    ship_via = "",
                    reference_no = "",
                    tracking_no = "",
                    address = txtAddr.Text.Trim(),
                    term_name = txtTerm.Text.Trim(),
                    due_date = txtDueDate.Text.Trim(),
                    deposit_to_name = "",
                    deposit = 0,
                    discount_unit = 0,
                    witholding_account_name = "",
                    witholding_value = 0,
                    witholding_type = txtWithHolding.Text.Trim(),
                    discount_type_name = txtDiscType.Text.Trim(),
                    person_name = txtCustName1.Text.Trim(),
                    warehouse_name = "",
                    warehouse_code = "",
                    tags = tags,
                    email = txtEmail.Text.Trim(),
                    transaction_no = txtTransactionNo.Text.Trim(),
                    message = txtMessage.Text.Trim(),
                    memo = txtMemo.Text.Trim(),
                    custom_id = "",
                    source = "",
                    use_tax_inclusive = false,
                    tax_after_discount = true
                };

                var param = new
                {
                    sales_invoice = paramSales
                };

                var jsonString = new JavaScriptSerializer().Serialize(param);
                var client = new RestClient("https://api.jurnal.id/core/api/v1");
                var request = new RestRequest("/sales_invoices", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "*/*");
                request.AddHeader("apikey", $"28cf7da0307e7a5a3ec8781b53e6f81e");
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string content = response.Content;
                JObject joRes = JObject.Parse(content);
                var respCode = response.StatusCode.ToString();
                var respMsg = response.Content.ToString();
                return joRes;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0; string strSQL = ""; string sErr = "";
                var tags = txtTags.Text.Trim();
                ExecCommand ec = new ExecCommand();
                if (txtCustName1.Text.Trim() != "")
               {
                   if (txtTransactionNo.Text.Trim() != "")
                   {
                       if (txtDueDate.Text.Trim() != "")
                       {
                           if (selProduct.SelectedItem.Value.Trim() != "[Select]")
                           {
                                strSQL = "usp_insert_update_invoice_jurnal " + txtSalesOrderId.Value.Trim() + "," + txtCompId.Text.Trim() + "," + txtPersonId.Text.Trim()
                                + ",'" + txtCustName1.Text.Trim() + "','" + txtAddr.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + selProduct.SelectedItem.Value.Trim()
                                + "'," + txtRate.Text.Trim() + "," + txtQty.Text.Trim() + ", 16966, 'PPN', 'percent', 'percent', 'Custom', '" + tags + "','" + selGpsType.SelectedItem.Value.Trim()
                                + "','" + Session["ClsTypeUserID"].ToString() + "','" + txtTransactionNo.Text.Trim() + "','" + txtDueDate.Text.Trim() + "'"
                                + ",'" + txtMessage.Text.Trim() + "','" + txtMemo.Text.Trim() + "'";

                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        var salesInv = AddSalesInvoice();
                                        clear();
                                        Open_GridViewHeader();

                                        CmdCreate.Visible = true;
                                        CmdSubmit.Visible = false;
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit Invoice has been successfully</div>";
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit Invoice has been failed (" + sErr + ")</div>";
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
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select customer name</div>";
                       }
                   }
                   else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in Due Date</div>";
                    }
               }
               else
               {
                   div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> lease fill in transaction no</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateInvoice"], LblPagingHeader, ViewState["RecCreateInvoiceFieldSort"].ToString(), ViewState["RecCreateInvoiceDirSort"].ToString());
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
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreateInvoice"], ViewState["RecCreateInvoiceFieldSort"].ToString(), ViewState["RecCreateInvoiceDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateInvoiceFieldSort"] = e.SortExpression.ToString();
                //ViewState["RecCreatePurchaseOrderHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }

        }
    }
}


public class paramAddSales
{
    public string transaction_date { get; set; }
    public List<detailTransaction> transaction_lines_attributes { get; set; }
    public string shipping_date { get; set; }
    public int shipping_price { get; set; }
    public string shipping_address { get; set; }//GET FROM DETAIL GROUP
    public bool is_shipped { get; set; }
    public string ship_via { get; set; }
    public string reference_no { get; set; }
    public string tracking_no { get; set; }
    public string address { get; set; }
    public string term_name { get; set; }
    public string due_date { get; set; }
    public string deposit_to_name { get; set; }
    public int deposit { get; set; }
    public int discount_unit { get; set; }
    public string witholding_account_name { get; set; }
    public int witholding_value { get; set; }
    public string witholding_type { get; set; }
    public string discount_type_name { get; set; }
    public string person_name { get; set; }
    public string warehouse_name { get; set; }
    public string warehouse_code { get; set; }
    public List<string> tags { get; set; }
    public string email { get; set; }
    public string transaction_no { get; set; }
    public string message { get; set; }
    public string memo { get; set; }
    public string custom_id { get; set; }
    public string source { get; set; }
    public bool use_tax_inclusive { get; set; }
    public bool tax_after_discount { get; set; }
}


public class detailTransaction
{
    public int quantity { get; set; }
    public int rate { get; set; }
    public int discount { get; set; }
    public string product_name { get; set; }
    public int line_tax_id { get; set; }
    public string line_tax_name { get; set; }
}