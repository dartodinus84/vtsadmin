using Newtonsoft.Json; // pastikan sudah di-import
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Security.Cryptography;
using System.Globalization;

namespace vtsadm
{
    public partial class GenerateJurnalInv : System.Web.UI.Page
    {
        string sSessionRecList = "RecGenerateJurnal";
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();

                // Perbaikan: Cek session sebelum mengakses
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTCUST"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            // Dipanggil AJAX, bukan di sini.
                            div_comment.InnerHtml = "";

                            // Jika mau export langsung pakai query ?export=1
                            if (Request.QueryString["export"] == "1")
                            {
                                CmdExportXls_ServerClick();
                            }
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
                div_comment.InnerHtml = "Error: " + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetJurnalHeader(string search = "")
        {
            var list = new List<JurnalInvoiceHeader>();
            try
            {
                var allConnections = System.Configuration.ConfigurationManager.ConnectionStrings;
                if (allConnections == null)
                {
                    return JsonConvert.SerializeObject(new { error = "ConnectionStrings is null. Check web.config loading." });
                }

                var connSettings = allConnections["JURNALDB"];
                if (connSettings == null || string.IsNullOrWhiteSpace(connSettings.ConnectionString))
                {
                    return JsonConvert.SerializeObject(new { error = "Missing connection string: JURNALDB" });
                }
                string connStr = connSettings.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_get_list_jurnal_header_v1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@search", search ?? "");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            var columns = GetColumnSet(dr);
                            while (dr.Read())
                            {
                                var item = new JurnalInvoiceHeader
                                {
                                    // Header fields dari SP v1
                                    id = GetInt64(dr, columns, "id"),
                                    id_sales_invoice = GetInt64(dr, columns, "id_sales_invoice"),
                                    transaction_no = GetString(dr, columns, "transaction_no"),
                                    email = GetString(dr, columns, "email"),
                                    message = GetString(dr, columns, "message"),
                                    address = GetString(dr, columns, "address"),
                                    memo = GetString(dr, columns, "memo"),
                                    subtotal = GetString(dr, columns, "subtotal"),
                                    tax_amount = GetString(dr, columns, "tax_amount"),
                                    original_amount = GetString(dr, columns, "original_amount"),
                                    use_tax_inclusive = GetString(dr, columns, "use_tax_inclusive"),
                                    tax_after_discount = GetString(dr, columns, "tax_after_discount"),
                                    transaction_date = GetString(dr, columns, "transaction_date"),
                                    due_date = GetString(dr, columns, "due_date"),
                                    term_name = GetString(dr, columns, "term_name"),
                                    witholding_type = GetString(dr, columns, "witholding_type"),
                                    discount_type_name = GetString(dr, columns, "discount_type_name"),
                                    person_display_name = GetString(dr, columns, "person_display_name"),
                                    tags_string = GetString(dr, columns, "tags_string"),
                                    custom_id = GetString(dr, columns, "custom_id"),

                                    // Detail fields dari SP v1
                                    invoice_id = GetInt64(dr, columns, "invoice_id"),
                                    rate = GetString(dr, columns, "rate"),
                                    discount = GetString(dr, columns, "discount"),
                                    quantity = GetInt32(dr, columns, "quantity"),
                                    product_name = GetString(dr, columns, "product_name"),
                                    line_tax_id = GetInt64(dr, columns, "line_tax_id"),
                                    description = GetString(dr, columns, "description"),
                                    parsed_description = GetString(dr, columns, "parsed_description"),

                                    // Detail create (read-only)
                                    month_desc = GetString(dr, columns, "month_desc"),
                                    year_desc = GetString(dr, columns, "year_desc"),
                                    installment_desc = GetInt32(dr, columns, "installment_desc"),
                                    plafon_desc = GetInt32(dr, columns, "plafon_desc"),
                                    cycle_desc = GetInt32(dr, columns, "cycle_desc"),
                                    nopol = GetString(dr, columns, "nopol")
                                };

                                list.Add(item);
                            }
                        }
                    }
                }

                // Simpan ke Session untuk export (jaga jika Session null)
                var dt = list.ToDataTable();
                var ds = new DataSet();
                ds.Tables.Add(dt);
                var context = HttpContext.Current;
                if (context != null && context.Session != null)
                {
                    context.Session["RecGenerateJurnal"] = ds;
                }

                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.ToString() });
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetJurnalHeaderNew(string search = "")
        {
            var list = new List<JurnalInvoiceHeader>();
            try
            {
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_get_list_jurnal_header_new", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@search", search ?? "");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new JurnalInvoiceHeader
                                {
                                    // Header fields dari SP - SEMUA FIELD DIMAPPING
                                    id_sales_invoice = dr["id_sales_invoice"] != DBNull.Value ? Convert.ToInt64(dr["id_sales_invoice"]) : 0,
                                    transaction_no = dr["transaction_no"]?.ToString(),
                                    token = dr["token"]?.ToString(),
                                    email = dr["email"]?.ToString(),
                                    message = dr["message"]?.ToString(),
                                    address = dr["address"]?.ToString(),
                                    memo = dr["memo"]?.ToString(),
                                    remaining = dr["remaining"]?.ToString(),
                                    original_amount = dr["original_amount"]?.ToString(),
                                    credit_memo_balance = dr["credit_memo_balance"]?.ToString(),
                                    use_tax_inclusive = dr["use_tax_inclusive"]?.ToString(),
                                    tax_after_discount = dr["tax_after_discount"]?.ToString(),
                                    tax_amount = dr["tax_amount"]?.ToString(),
                                    status = dr["status"]?.ToString(),
                                    amount_receive = dr["amount_receive"]?.ToString(),
                                    subtotal = dr["subtotal"]?.ToString(),
                                    created_at = dr["created_at"] != DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue,
                                    deleted_at = dr["deleted_at"]?.ToString(),
                                    deletable = dr["deletable"]?.ToString(),
                                    editable = dr["editable"]?.ToString(),
                                    transaction_date = dr["transaction_date"]?.ToString(),
                                    due_date = dr["due_date"]?.ToString(),
                                    payment_received_amount = dr["payment_received_amount"]?.ToString(),
                                    transaction_status_id = dr["transaction_status_id"] != DBNull.Value ? Convert.ToInt64(dr["transaction_status_id"]) : 0,
                                    transaction_status_name = dr["transaction_status_name"]?.ToString(),
                                    term_name = dr["term_name"]?.ToString(),
                                    witholding_type = dr["witholding_type"]?.ToString(),
                                    discount_type_name = dr["discount_type_name"]?.ToString(),
                                    custom_id = dr["custom_id"]?.ToString(),
                                    person_id = dr["person_id"] != DBNull.Value ? Convert.ToInt64(dr["person_id"]) : 0,
                                    person_display_name = dr["person_display_name"]?.ToString(),
                                    tags_string = dr["tags_string"]?.ToString(),
                                    has_payments = dr["has_payments"]?.ToString(),
                                    earliest_payment_date = dr["earliest_payment_date"]?.ToString(),
                                    updated_at = dr["updated_at"] != DBNull.Value ? Convert.ToDateTime(dr["updated_at"]) : DateTime.MinValue,
                                    dtmupd = dr["dtmupd"] != DBNull.Value ? Convert.ToDateTime(dr["dtmupd"]) : DateTime.MinValue,
                                    transaction_no_old = dr["transaction_no_old"]?.ToString(),

                                    // Detail fields dari SP (karena SP melakukan LEFT JOIN)
                                    invoice_id = dr["invoice_id"] != DBNull.Value ? Convert.ToInt64(dr["invoice_id"]) : 0,
                                    id = dr["id"] != DBNull.Value ? Convert.ToInt64(dr["id"]) : 0,
                                    custom_id_detail = dr["custom_id_detail"]?.ToString(),
                                    description = dr["description"]?.ToString(),
                                    amount = dr["amount"]?.ToString(),
                                    rate = dr["rate"]?.ToString(),
                                    discount = dr["discount"]?.ToString(),
                                    quantity = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0,
                                    product_id = dr["product_id"] != DBNull.Value ? Convert.ToInt64(dr["product_id"]) : 0,
                                    product_name = dr["product_name"]?.ToString(),
                                    line_tax_id = dr["line_tax_id"] != DBNull.Value ? Convert.ToInt64(dr["line_tax_id"]) : 0
                                };

                                list.Add(item);
                            }
                        }
                    }
                }

                // Simpan ke Session untuk export
                var dt = list.ToDataTable();
                var ds = new DataSet();
                ds.Tables.Add(dt);
                HttpContext.Current.Session["RecGenerateJurnalNew"] = ds;

                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        [WebMethod(EnableSession = true)]
        public static List<JurnalInvoiceHeaderDetail> GetJurnalDetail(int id_sales_invoice)
        {
            var details = new List<JurnalInvoiceHeaderDetail>();
            if (id_sales_invoice <= 0)
            {
                System.Diagnostics.Debug.WriteLine("GetJurnalDetail: ID tidak valid.");
                return details;
            }
            try
            {
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_get_list_jurnal_detail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_sales_invoice", id_sales_invoice);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var detail = new JurnalInvoiceHeaderDetail
                                {
                                    invoice_id = dr["invoice_id"] != DBNull.Value ? Convert.ToInt64(dr["invoice_id"]) : 0,
                                    id = dr["id"] != DBNull.Value ? Convert.ToInt64(dr["id"]) : 0,
                                    custom_id_detail = dr["custom_id"]?.ToString(),
                                    description = dr["description"]?.ToString(),
                                    amount = dr["amount"]?.ToString(),
                                    rate = dr["rate"]?.ToString(),
                                    discount = dr["discount"]?.ToString(),
                                    product_id = dr["product_id"] != DBNull.Value ? Convert.ToInt64(dr["product_id"]) : 0,
                                    product_name = dr["product_name"]?.ToString(),
                                    line_tax_id = dr["line_tax_id"] != DBNull.Value ? Convert.ToInt64(dr["line_tax_id"]) : 0,
                                    quantity = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0
                                };
                                details.Add(detail);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetJurnalDetail error: " + ex.Message);
            }
            return details;
        }

        [WebMethod(EnableSession = true)]
        public static string SaveAsDraftJurnalInvoice(string invoiceIdList)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"SaveAsDraftJurnalInvoice called with invoiceIdList: {invoiceIdList}");
                
                if (string.IsNullOrEmpty(invoiceIdList))
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "No invoice IDs provided" });
                }

                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                string message = "";
                int status_code = 100;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_jurnal_invoice_create", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@invoice_id_list", invoiceIdList);

                        try
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine($"Stored procedure executed. Rows affected: {rowsAffected}");

                            if (rowsAffected > 0)
                            {
                                status_code = 200;
                                message = $"Successfully saved {rowsAffected} invoice(s) as draft.";
                            }
                            else
                            {
                                status_code = 100;
                                message = "No records were saved. Please verify the selected Invoice IDs and try again.";
                            }
                        }
                        catch (Exception ex)
                        {
                            message = $"Error executing stored procedure: {ex.Message}";
                            status_code = 100;
                            System.Diagnostics.Debug.WriteLine($"Stored procedure error: {ex.Message}");
                        }
                    }
                }

                var result = JsonConvert.SerializeObject(new { status_code, message });
                System.Diagnostics.Debug.WriteLine($"SaveAsDraftJurnalInvoice returning: {result}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveAsDraftJurnalInvoice error: {ex.Message}");
                return JsonConvert.SerializeObject(new { status_code = 100, message = ex.Message });
            }
        }

        [WebMethod(EnableSession = true)]
        public static string UploadFile()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("UploadFile WebMethod called");
                
                var httpContext = HttpContext.Current;
                
                if (httpContext.Request.Files.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No files found in request");
                    return JsonConvert.SerializeObject(new { error = "No file uploaded." });
                }
                
                var file = httpContext.Request.Files[0];
                
                if (file == null || file.ContentLength == 0)
                {
                    System.Diagnostics.Debug.WriteLine("File is null or empty");
                    return JsonConvert.SerializeObject(new { error = "No file uploaded." });
                }

                System.Diagnostics.Debug.WriteLine($"File received: {file.FileName}, Size: {file.ContentLength} bytes");

                // TODO: Implement Excel parsing with EPPlus library
                // For now, return sample data to demonstrate the structure
                // In production, you would parse the Excel file and call the stored procedure for each row
                
                var sampleData = new List<JurnalInvoiceHeader>
                {
                    new JurnalInvoiceHeader
                    {
                        id_sales_invoice = 1,
                        transaction_no = "TRX-001",
                        message = "Sample transaction from uploaded file: " + file.FileName,
                        remaining = "1000.00",
                        original_amount = "1000.00",
                        status = "Active",
                        subtotal = "900.00",
                        transaction_date = DateTime.Now.ToString("yyyy-MM-dd"),
                        custom_id = "CUST001",
                        person_display_name = "Sample Customer"
                    }
                };

                // Call stored procedure for each row (sample implementation)
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                string message = "";
                int status_code = 100;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in sampleData)
                            {
                                using (SqlCommand cmd = new SqlCommand("vtsadmin_jurnal_invoice_create_from_excel", conn, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    // HEADER parameters (sesuai SP terbaru)
                                    cmd.Parameters.AddWithValue("@id_sales_invoice", item.id_sales_invoice);
                                    cmd.Parameters.AddWithValue("@transaction_no", item.transaction_no ?? "");
                                    cmd.Parameters.AddWithValue("@email", item.email ?? "");
                                    cmd.Parameters.AddWithValue("@message", item.message ?? "");
                                    cmd.Parameters.AddWithValue("@address", item.address ?? "");
                                    cmd.Parameters.AddWithValue("@memo", item.memo ?? "");
                                    cmd.Parameters.AddWithValue("@subtotal", item.subtotal ?? "0");
                                    cmd.Parameters.AddWithValue("@tax_amount", item.tax_amount ?? "0");
                                    cmd.Parameters.AddWithValue("@original_amount", item.original_amount ?? "0");
                                    cmd.Parameters.AddWithValue("@use_tax_inclusive", item.use_tax_inclusive ?? "false");
                                    cmd.Parameters.AddWithValue("@tax_after_discount", item.tax_after_discount ?? "false");
                                    cmd.Parameters.AddWithValue("@transaction_date", item.transaction_date ?? DateTime.Now.ToString("dd/MM/yyyy"));
                                    cmd.Parameters.AddWithValue("@due_date", item.due_date ?? "");
                                    cmd.Parameters.AddWithValue("@term_name", item.term_name ?? "");
                                    cmd.Parameters.AddWithValue("@witholding_type", item.witholding_type ?? "");
                                    cmd.Parameters.AddWithValue("@discount_type_name", item.discount_type_name ?? "");
                                    cmd.Parameters.AddWithValue("@person_display_name", item.person_display_name ?? "");
                                    cmd.Parameters.AddWithValue("@tags_string", item.tags_string ?? "");
                                    cmd.Parameters.AddWithValue("@custom_id", item.custom_id ?? "");

                                    // DETAIL parameters (sesuai SP terbaru)
                                    cmd.Parameters.AddWithValue("@invoice_id", item.invoice_id > 0 ? item.invoice_id : item.id_sales_invoice);
                                    cmd.Parameters.AddWithValue("@id", item.id);
                                    cmd.Parameters.AddWithValue("@rate", item.rate ?? "0");
                                    cmd.Parameters.AddWithValue("@discount", item.discount ?? "0");
                                    cmd.Parameters.AddWithValue("@quantity", item.quantity);
                                    cmd.Parameters.AddWithValue("@product_name", item.product_name ?? "");
                                    cmd.Parameters.AddWithValue("@line_tax_id", item.line_tax_id);
                                    cmd.Parameters.AddWithValue("@description", !string.IsNullOrEmpty(item.parsed_description) ? item.parsed_description : (item.description ?? ""));
                                    cmd.Parameters.AddWithValue("@month_desc", item.month_desc ?? "");
                                    cmd.Parameters.AddWithValue("@year_desc", item.year_desc ?? "");
                                    cmd.Parameters.AddWithValue("@installment_desc", item.installment_desc);
                                    cmd.Parameters.AddWithValue("@plafon_desc", item.plafon_desc);
                                    cmd.Parameters.AddWithValue("@cycle_desc", item.cycle_desc);
                                    cmd.Parameters.AddWithValue("@nopol", item.nopol ?? "");

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            
                            transaction.Commit();
                            status_code = 200;
                            message = $"Successfully processed {sampleData.Count} record(s) from Excel file: {file.FileName}";
                            System.Diagnostics.Debug.WriteLine("Excel upload transaction committed successfully");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            message = $"Error processing Excel file: {ex.Message}";
                            status_code = 100;
                            System.Diagnostics.Debug.WriteLine($"Excel upload transaction rolled back: {ex.Message}");
                        }
                    }
                }

                var resultJson = JsonConvert.SerializeObject(new { 
                    status_code = status_code, 
                    message = message, 
                    data = sampleData 
                });
                System.Diagnostics.Debug.WriteLine($"UploadFile returning result for file: {file.FileName}");
                return resultJson;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UploadFile error: {ex.Message}");
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }


        [WebMethod(EnableSession = true)]
        public static string GetInvoiceDataForSalesOrder(string invoiceIdList)
        {
            var list = new List<JurnalInvoiceHeader>();
            try
            {
                if (string.IsNullOrEmpty(invoiceIdList))
                {
                    return JsonConvert.SerializeObject(new { error = "Invoice ID list tidak boleh kosong" });
                }

                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_get_jurnal_invoice_for_sales_order", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@invoice_id_list", invoiceIdList);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new JurnalInvoiceHeader
                                {
                                    // Header mapping
                                    id_sales_invoice = dr["id_sales_invoice"] != DBNull.Value ? Convert.ToInt64(dr["id_sales_invoice"]) : 0,
                                    transaction_no = dr["transaction_no"]?.ToString(),
                                    token = dr["token"]?.ToString(),
                                    email = dr["email"]?.ToString(),
                                    message = dr["message"]?.ToString(),
                                    address = dr["address"]?.ToString(),
                                    memo = dr["memo"]?.ToString(),
                                    remaining = dr["remaining"]?.ToString(),
                                    original_amount = dr["original_amount"]?.ToString(),
                                    credit_memo_balance = dr["credit_memo_balance"]?.ToString(),
                                    use_tax_inclusive = dr["use_tax_inclusive"]?.ToString(),
                                    tax_after_discount = dr["tax_after_discount"]?.ToString(),
                                    tax_amount = dr["tax_amount"]?.ToString(),
                                    status = dr["status"]?.ToString(),
                                    amount_receive = dr["amount_receive"]?.ToString(),
                                    subtotal = dr["subtotal"]?.ToString(),
                                    created_at = dr["created_at"] != DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : DateTime.MinValue,
                                    deleted_at = dr["deleted_at"]?.ToString(),
                                    deletable = dr["deletable"]?.ToString(),
                                    editable = dr["editable"]?.ToString(),
                                    transaction_date = dr["transaction_date"]?.ToString(),
                                    due_date = dr["due_date"]?.ToString(),
                                    payment_received_amount = dr["payment_received_amount"]?.ToString(),
                                    transaction_status_id = dr["transaction_status_id"] != DBNull.Value ? Convert.ToInt64(dr["transaction_status_id"]) : 0,
                                    transaction_status_name = dr["transaction_status_name"]?.ToString(),
                                    term_name = dr["term_name"]?.ToString(),
                                    witholding_type = dr["witholding_type"]?.ToString(),
                                    discount_type_name = dr["discount_type_name"]?.ToString(),
                                    custom_id = dr["custom_id"]?.ToString(),
                                    person_id = dr["person_id"] != DBNull.Value ? Convert.ToInt64(dr["person_id"]) : 0,
                                    person_display_name = dr["person_display_name"]?.ToString(),
                                    tags_string = dr["tags_string"]?.ToString(),
                                    has_payments = dr["has_payments"]?.ToString(),
                                    earliest_payment_date = dr["earliest_payment_date"]?.ToString(),
                                    is_sync = dr["is_sync"] != DBNull.Value ? Convert.ToByte(dr["is_sync"]) : (byte)0,
                                    updated_at = dr["updated_at"] != DBNull.Value ? Convert.ToDateTime(dr["updated_at"]) : DateTime.MinValue,
                                    dtmupd = dr["dtmupd"] != DBNull.Value ? Convert.ToDateTime(dr["dtmupd"]) : DateTime.MinValue,

                                    // Detail mapping
                                    invoice_id = dr["invoice_id"] != DBNull.Value ? Convert.ToInt64(dr["invoice_id"]) : 0,
                                    id = dr["id"] != DBNull.Value ? Convert.ToInt64(dr["id"]) : 0,
                                    custom_id_detail = dr["custom_id_detail"]?.ToString(),
                                    description = dr["description"]?.ToString(),
                                    amount = dr["amount"]?.ToString(),
                                    rate = dr["rate"]?.ToString(),
                                    discount = dr["discount"]?.ToString(),
                                    quantity = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0,
                                    product_id = dr["product_id"] != DBNull.Value ? Convert.ToInt64(dr["product_id"]) : 0,
                                    product_name = dr["product_name"]?.ToString(),
                                    line_tax_id = dr["line_tax_id"] != DBNull.Value ? Convert.ToInt64(dr["line_tax_id"]) : 0
                                };
                                list.Add(item);
                            }
                        }
                    }
                }

                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        [WebMethod(EnableSession = true)]
        public static string UpdateSyncStatus(string invoiceIdList, byte isSync, string syncResponse)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus called with: invoiceIdList={invoiceIdList}, isSync={isSync}, syncResponse={syncResponse}");
                
                if (string.IsNullOrEmpty(invoiceIdList))
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "Invoice ID list tidak boleh kosong" });
                }

                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["JURNALDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("vtsadmin_update_jurnal_sync_status", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@invoice_id_list", invoiceIdList);
                        cmd.Parameters.AddWithValue("@is_sync", isSync);
                        cmd.Parameters.AddWithValue("@sync_response", syncResponse ?? "");

                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {rowsAffected} rows affected");
                        
                        return JsonConvert.SerializeObject(new { 
                            status_code = 200, 
                            message = $"Successfully updated {rowsAffected} invoice(s) sync status",
                            rows_affected = rowsAffected
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return JsonConvert.SerializeObject(new { status_code = 100, message = ex.Message });
            }
        }

        [WebMethod(EnableSession = true)]
        public static string CreateSalesOrderInJurnal(List<int> invoiceIds)
        {
            try
            {
                if (invoiceIds == null || invoiceIds.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "No invoice IDs selected." });
                }

                string invoiceIdList = string.Join(",", invoiceIds);
                
                // 1. Ambil data invoice dari database
                var invoiceDataResponse = GetInvoiceDataForSalesOrder(invoiceIdList);
                var invoiceData = JsonConvert.DeserializeObject<List<JurnalInvoiceHeader>>(invoiceDataResponse);
                
                if (invoiceData == null || invoiceData.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "No invoice data found." });
                }

                // 2. Build payload dari data asli
                var firstInvoice = invoiceData.FirstOrDefault();
                if (firstInvoice == null)
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "Invalid invoice data." });
                }

                // Group data by invoice untuk multiple invoice
                var groupedData = invoiceData.GroupBy(x => x.id_sales_invoice);
                var transactionLines = new List<object>();

                foreach (var group in groupedData)
                {
                    foreach (var item in group.Where(x => x.invoice_id > 0))
                    {
                        transactionLines.Add(new
                        {
                            quantity = item.quantity > 0 ? item.quantity : 1,
                            rate = !string.IsNullOrEmpty(item.rate) ? decimal.Parse(item.rate) : 0,
                            discount = !string.IsNullOrEmpty(item.discount) ? decimal.Parse(item.discount) : 0,
                            product_name = !string.IsNullOrEmpty(item.product_name) ? item.product_name : "Penjualan",
                            line_tax_id = item.line_tax_id > 0 ? item.line_tax_id : 3198,
                            line_tax_name = "ppn"
                        });
                    }
                }

                // Jika tidak ada detail, gunakan data header
                if (transactionLines.Count == 0)
                {
                    transactionLines.Add(new
                    {
                        quantity = 1,
                        rate = !string.IsNullOrEmpty(firstInvoice.original_amount) ? decimal.Parse(firstInvoice.original_amount) : 12000000,
                        discount = 0,
                        product_name = "Penjualan",
                        line_tax_id = 3198,
                        line_tax_name = "ppn"
                    });
                }

                var payload = new
                {
                    sales_invoice = new
                    {
                        transaction_date = !string.IsNullOrEmpty(firstInvoice.transaction_date) ? firstInvoice.transaction_date : DateTime.Now.ToString("yyyy-MM-dd"),
                        transaction_lines_attributes = transactionLines.ToArray(),
                        shipping_date = DateTime.Now.ToString("yyyy-MM-dd"),
                        shipping_price = 0,
                        shipping_address = firstInvoice.address ?? "",
                        is_shipped = true,
                        ship_via = "ship",
                        reference_no = firstInvoice.transaction_no ?? "",
                        tracking_no = "",
                        address = firstInvoice.address ?? "",
                        term_name = !string.IsNullOrEmpty(firstInvoice.term_name) ? firstInvoice.term_name : "Cash on Delivery",
                        due_date = !string.IsNullOrEmpty(firstInvoice.due_date) ? firstInvoice.due_date : DateTime.Now.ToString("yyyy-MM-dd"),
                        deposit_to_name = "Kas",
                        deposit = 0,
                        discount_unit = 0,
                        witholding_account_name = "Kas",
                        witholding_value = 0,
                        witholding_type = !string.IsNullOrEmpty(firstInvoice.witholding_type) ? firstInvoice.witholding_type : "percent",
                        discount_type_name = !string.IsNullOrEmpty(firstInvoice.discount_type_name) ? firstInvoice.discount_type_name : "percent",
                        person_name = firstInvoice.person_display_name ?? "",
                        warehouse_name = "",
                        warehouse_code = "",
                        tags = !string.IsNullOrEmpty(firstInvoice.tags_string) ? firstInvoice.tags_string.Split(',') : new string[] {},
                        email = firstInvoice.email ?? "",
                        transaction_no = firstInvoice.transaction_no ?? "",
                        message = firstInvoice.message ?? "",
                        memo = firstInvoice.memo ?? "",
                        custom_id = firstInvoice.id_sales_invoice.ToString(),
                        source = "VTS ADMIN SYSTEM",
                        use_tax_inclusive = firstInvoice.use_tax_inclusive == "true",
                        tax_after_discount = firstInvoice.tax_after_discount == "true"
                    }
                };

                var json = JsonConvert.SerializeObject(payload);

                var apikey = System.Configuration.ConfigurationManager.AppSettings["JurnalApiKey"];
                var apiBase = System.Configuration.ConfigurationManager.AppSettings["JurnalApiBaseUrl"] ?? "https://api.jurnal.id";
                var timeoutSetting = System.Configuration.ConfigurationManager.AppSettings["HttpTimeoutSeconds"];
                int timeoutSeconds = 60;
                if (!string.IsNullOrWhiteSpace(timeoutSetting))
                {
                    int.TryParse(timeoutSetting, out timeoutSeconds);
                    if (timeoutSeconds <= 0) timeoutSeconds = 60;
                }

                if (string.IsNullOrWhiteSpace(apikey))
                {
                    return JsonConvert.SerializeObject(new { status_code = 100, message = "Missing Jurnal API Key (JurnalApiKey)." });
                }

                var url = $"{apiBase}/public/jurnal/api/v1/sales_invoices";

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };

                    // Add headers sesuai format curl
                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Content-Type", "application/json");
                    request.Headers.Add("apikey", apikey);

                    using (var response = httpClient.SendAsync(request).Result)
                    {
                        var body = response.Content != null ? response.Content.ReadAsStringAsync().Result : string.Empty;

                        if (response.IsSuccessStatusCode)
                        {
                            System.Diagnostics.Debug.WriteLine($"Calling UpdateSyncStatus for success: {invoiceIdList}");
                            var updateResult = UpdateSyncStatus(invoiceIdList, (byte)1, "Success: " + body);
                            System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {updateResult}");

                            return JsonConvert.SerializeObject(new {
                                status_code = 200,
                                message = "Sales Order successfully sent to Jurnal.",
                                response = body,
                                invoice_count = invoiceIds.Count,
                                invoice_data = invoiceData
                            });
                        }
                        else
                        {
                            string invoiceIdListOnFail = string.Join(",", invoiceIds);
                            System.Diagnostics.Debug.WriteLine($"Calling UpdateSyncStatus for failure: {invoiceIdListOnFail}");
                            var updateResult = UpdateSyncStatus(invoiceIdListOnFail, (byte)0, "Failed: " + body);
                            System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {updateResult}");

                            return JsonConvert.SerializeObject(new { status_code = 100, message = $"HTTP {(int)response.StatusCode} {response.StatusCode}", error = body });
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                try
                {
                    string invoiceIdList = string.Join(",", invoiceIds);
                    string errorMessage = "";
                    
                    using (var resp = (HttpWebResponse)wex.Response)
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        var body = reader.ReadToEnd();
                        errorMessage = $"HTTP {(int)resp.StatusCode} {resp.StatusCode}: {body}";
                    }
                    
                    // Update status sync ke database jika gagal
                    System.Diagnostics.Debug.WriteLine($"Calling UpdateSyncStatus for failure: {invoiceIdList}");
                    var updateResult = UpdateSyncStatus(invoiceIdList, (byte)0, "Failed: " + errorMessage);
                    System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {updateResult}");
                    
                    return JsonConvert.SerializeObject(new { status_code = 100, message = errorMessage });
                }
                catch
                {
                    string invoiceIdList = string.Join(",", invoiceIds);
                    string errorMessage = wex.Message;
                    
                    // Update status sync ke database jika gagal
                    System.Diagnostics.Debug.WriteLine($"Calling UpdateSyncStatus for failure: {invoiceIdList}");
                    var updateResult = UpdateSyncStatus(invoiceIdList, (byte)0, "Failed: " + errorMessage);
                    System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {updateResult}");
                    
                    return JsonConvert.SerializeObject(new { status_code = 100, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                string invoiceIdList = string.Join(",", invoiceIds);
                string errorMessage = ex.Message;
                
                // Update status sync ke database jika gagal
                System.Diagnostics.Debug.WriteLine($"Calling UpdateSyncStatus for failure: {invoiceIdList}");
                var updateResult = UpdateSyncStatus(invoiceIdList, (byte)0, "Failed: " + errorMessage);
                System.Diagnostics.Debug.WriteLine($"UpdateSyncStatus result: {updateResult}");
                
                return JsonConvert.SerializeObject(new { status_code = 100, message = errorMessage });
            }
        }



        protected void CmdExportXls_ServerClick()
        {
            try
            {
                // Perbaikan: Cek session sebelum mengakses
                if (Session[sSessionRecList] == null)
                {
                    div_comment.InnerHtml = "No data available for export. Please search for data first.";
                    return;
                }

                Recordset Rec = new Recordset();
                Rec.RecData = Session[sSessionRecList] as DataSet;

                if (Rec != null && Rec.RecordCount() > 0)
                {
                    GridView gv = new GridView();
                    var exportTable = Rec.RecData.Tables[0];
                    var orderedColumns = new[]
                    {
                        "id_sales_invoice",
                        "transaction_no",
                        "email",
                        "message",
                        "address",
                        "memo",
                        "subtotal",
                        "tax_amount",
                        "original_amount",
                        "use_tax_inclusive",
                        "tax_after_discount",
                        "transaction_date",
                        "due_date",
                        "term_name",
                        "witholding_type",
                        "discount_type_name",
                        "person_display_name",
                        "tags_string",
                        "custom_id",
                        "invoice_id",
                        "rate",
                        "discount",
                        "quantity",
                        "product_name",
                        "line_tax_id",
                        "description",
                        "parsed_description",
                        "month_desc",
                        "year_desc",
                        "installment_desc",
                        "plafon_desc",
                        "cycle_desc",
                        "nopol"
                    };

                    var reordered = new DataTable();
                    foreach (var col in orderedColumns)
                    {
                        if (exportTable.Columns.Contains(col))
                        {
                            reordered.Columns.Add(col, exportTable.Columns[col].DataType);
                        }
                    }

                    if (reordered.Columns.Count > 0)
                    {
                        foreach (DataRow row in exportTable.Rows)
                        {
                            var newRow = reordered.NewRow();
                            foreach (DataColumn col in reordered.Columns)
                            {
                                newRow[col.ColumnName] = row[col.ColumnName];
                            }
                            reordered.Rows.Add(newRow);
                        }
                        exportTable = reordered;
                    }

                    gv.DataSource = exportTable;
                    gv.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Jurnal_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";

                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gv.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_comment.InnerHtml = "No records found.";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "Export Error: " + ex.Message;
            }
        }

        protected void CmdExportXls_Click(object sender, EventArgs e)
        {
            CmdExportXls_ServerClick();
        }

        public override void VerifyRenderingInServerForm(Control control) { }
    }

    // HMAC helper for Mekari Jurnal API
    // Authorization scheme:
    //   StringToSign:
    //     date: <date>\n
    //     METHOD PathAndQuery HTTP/1.1
    //   signature = Base64(HMAC-SHA256(secret, StringToSign))
    //   Header:
    //     Authorization: hmac username="<clientId>", algorithm="hmac-sha256", headers="date request-line", signature="<signature>"
    //     Date: <date>
    //     Accept: application/json
    //     Connection: keep-alive
    public partial class GenerateJurnalInv : System.Web.UI.Page
    {
        private static HashSet<string> GetColumnSet(SqlDataReader reader)
        {
            var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }
            return columns;
        }

        private static string GetString(SqlDataReader reader, HashSet<string> columns, string columnName)
        {
            if (!columns.Contains(columnName))
            {
                return "";
            }

            var value = reader[columnName];
            return value == DBNull.Value ? "" : value.ToString();
        }

        private static int GetInt32(SqlDataReader reader, HashSet<string> columns, string columnName)
        {
            if (!columns.Contains(columnName))
            {
                return 0;
            }

            var value = reader[columnName];
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            if (value is int)
            {
                return (int)value;
            }

            if (value is long)
            {
                var l = (long)value;
                return l > int.MaxValue ? int.MaxValue : (int)l;
            }

            if (value is decimal)
            {
                var dec = (decimal)value;
                return dec > int.MaxValue ? int.MaxValue : (int)dec;
            }

            if (value is double)
            {
                var dbl = (double)value;
                return dbl > int.MaxValue ? int.MaxValue : (int)dbl;
            }

            var str = value.ToString();
            int parsedInt;
            if (int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedInt))
            {
                return parsedInt;
            }

            decimal parsedDec;
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedDec))
            {
                return parsedDec > int.MaxValue ? int.MaxValue : (int)parsedDec;
            }

            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, out parsedDec))
            {
                return parsedDec > int.MaxValue ? int.MaxValue : (int)parsedDec;
            }

            return 0;
        }

        private static long GetInt64(SqlDataReader reader, HashSet<string> columns, string columnName)
        {
            if (!columns.Contains(columnName))
            {
                return 0;
            }

            var value = reader[columnName];
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            if (value is long)
            {
                return (long)value;
            }

            if (value is int)
            {
                return (int)value;
            }

            if (value is decimal)
            {
                return (long)(decimal)value;
            }

            if (value is double)
            {
                return (long)(double)value;
            }

            var str = value.ToString();
            long parsedLong;
            if (long.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedLong))
            {
                return parsedLong;
            }

            decimal parsedDec;
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedDec))
            {
                return (long)parsedDec;
            }

            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, out parsedDec))
            {
                return (long)parsedDec;
            }

            return 0;
        }

        private static void AddHmacHeaders(HttpRequestMessage request, string clientId, string clientSecret)
        {
            // RFC1123 date in UTC
            string date = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            // Build request-line: METHOD <PathAndQuery> HTTP/1.1
            string method = request.Method.Method.ToUpperInvariant();
            string pathAndQuery = request.RequestUri.PathAndQuery;
            string requestLine = $"{method} {pathAndQuery} HTTP/1.1";

            // Build string to sign
            // Note: exactly two lines with a newline in between
            string stringToSign = $"date: {date}\n{requestLine}";

            // Compute HMAC-SHA256 signature (Base64)
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret)))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                string signature = Convert.ToBase64String(signatureBytes);

                // Set headers
                request.Headers.TryAddWithoutValidation("Date", date);
                request.Headers.TryAddWithoutValidation("Accept", "application/json");
                request.Headers.TryAddWithoutValidation("Connection", "keep-alive");

                string authHeader = $"hmac username=\"{clientId}\", algorithm=\"hmac-sha256\", headers=\"date request-line\", signature=\"{signature}\"";
                request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            }
        }
    }

    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> data)
        {
            var table = new DataTable();
            if (data == null || data.Count == 0)
                return table;

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }

    public class JurnalInvoiceHeader
    {
        public long id_sales_invoice { get; set; }
        public string transaction_no { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string message { get; set; }
        public string address { get; set; }
        public string memo { get; set; }
        public string remaining { get; set; }
        public string original_amount { get; set; }
        public string credit_memo_balance { get; set; }
        public string use_tax_inclusive { get; set; }
        public string tax_after_discount { get; set; }
        public string tax_amount { get; set; }
        public string status { get; set; }
        public string amount_receive { get; set; }
        public string subtotal { get; set; }
        public DateTime created_at { get; set; }
        public string deleted_at { get; set; }
        public string deletable { get; set; }
        public string editable { get; set; }
        public string transaction_date { get; set; }
        public string due_date { get; set; }
        public string payment_received_amount { get; set; }
        public long transaction_status_id { get; set; }
        public string transaction_status_name { get; set; }
        public string term_name { get; set; }
        public string witholding_type { get; set; }
        public string discount_type_name { get; set; }
        public string custom_id { get; set; }
        public long person_id { get; set; }
        public string person_display_name { get; set; }
        public string tags_string { get; set; }
        public string has_payments { get; set; }
        public string earliest_payment_date { get; set; }
        public string transaction_no_old { get; set; }
        public byte is_sync { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime dtmupd { get; set; }

        // Detail properties
        public long invoice_id { get; set; }
        public long id { get; set; }
        public string custom_id_detail { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string rate { get; set; }
        public string discount { get; set; }
        public long product_id { get; set; }
        public string product_name { get; set; }
        public long line_tax_id { get; set; }
        public int quantity { get; set; }
        public List<JurnalInvoiceHeaderDetail> detail { get; set; }
        public string parsed_description { get; set; }
        public string month_desc { get; set; }
        public string year_desc { get; set; }
        public int installment_desc { get; set; }
        public int plafon_desc { get; set; }
        public int cycle_desc { get; set; }
        public string nopol { get; set; }
    }

    public class JurnalInvoiceHeaderDetail
    {
        public long invoice_id { get; set; }
        public long id { get; set; }
        public string custom_id_detail { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string rate { get; set; }
        public string discount { get; set; }
        public long product_id { get; set; }
        public string product_name { get; set; }
        public long line_tax_id { get; set; }
        public int quantity { get; set; }
    }

    public class JurnalInvoiceDetail
    {
        public long invoice_id { get; set; }
        public long id { get; set; }
        public string custom_id { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string rate { get; set; }
        public string discount { get; set; }
        public long product_id { get; set; }
        public string product_name { get; set; }
        public long line_tax_id { get; set; }
        public int quantity { get; set; }
        public DateTime dtmupd { get; set; }
    }
}