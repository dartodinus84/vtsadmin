using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;

namespace vtsadm
{
    public class UploadJurnalInvoice : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                if (context.Request.Files.Count == 0)
                {
                    WriteJson(context, new { error = "No file uploaded." });
                    return;
                }

                var file = context.Request.Files[0];
                if (file == null || file.ContentLength == 0)
                {
                    WriteJson(context, new { error = "No file uploaded." });
                    return;
                }

                ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");

                var records = new List<JurnalInvoiceHeader>();
                using (var stream = new MemoryStream())
                {
                    file.InputStream.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
                        if (worksheet == null)
                        {
                            WriteJson(context, new { error = "Worksheet not found in Excel file." });
                            return;
                        }

                        var headerMap = BuildHeaderMap(worksheet);
                        if (headerMap.Count == 0)
                        {
                            WriteJson(context, new { error = "Header row not found. Pastikan baris pertama berisi nama kolom." });
                            return;
                        }

                        int lastRow = worksheet.Dimension?.End.Row ?? 0;
                        for (int row = 2; row <= lastRow; row++)
                        {
                            if (IsRowEmpty(worksheet, row))
                            {
                                continue;
                            }

                            var item = new JurnalInvoiceHeader
                            {
                                id_sales_invoice = GetLong(worksheet, row, headerMap, "id_sales_invoice"),
                                transaction_no = GetString(worksheet, row, headerMap, "transaction_no"),
                                email = GetString(worksheet, row, headerMap, "email"),
                                message = GetString(worksheet, row, headerMap, "message"),
                                address = GetString(worksheet, row, headerMap, "address"),
                                memo = GetString(worksheet, row, headerMap, "memo"),
                                subtotal = GetString(worksheet, row, headerMap, "subtotal"),
                                tax_amount = GetString(worksheet, row, headerMap, "tax_amount"),
                                original_amount = GetString(worksheet, row, headerMap, "original_amount"),
                                use_tax_inclusive = GetString(worksheet, row, headerMap, "use_tax_inclusive"),
                                tax_after_discount = GetString(worksheet, row, headerMap, "tax_after_discount"),
                                transaction_date = GetDateString(worksheet, row, headerMap, "transaction_date"),
                                due_date = GetDateString(worksheet, row, headerMap, "due_date"),
                                term_name = GetString(worksheet, row, headerMap, "term_name"),
                                witholding_type = GetString(worksheet, row, headerMap, "witholding_type"),
                                discount_type_name = GetString(worksheet, row, headerMap, "discount_type_name"),
                                person_display_name = GetString(worksheet, row, headerMap, "person_display_name"),
                                tags_string = GetString(worksheet, row, headerMap, "tags_string"),
                                custom_id = GetString(worksheet, row, headerMap, "custom_id"),

                                invoice_id = GetLong(worksheet, row, headerMap, "invoice_id"),
                                rate = GetString(worksheet, row, headerMap, "rate"),
                                discount = GetString(worksheet, row, headerMap, "discount"),
                                quantity = GetInt(worksheet, row, headerMap, "quantity"),
                                product_name = GetString(worksheet, row, headerMap, "product_name"),
                                line_tax_id = GetLong(worksheet, row, headerMap, "line_tax_id"),
                                description = GetString(worksheet, row, headerMap, "description"),
                                parsed_description = GetString(worksheet, row, headerMap, "parsed_description"),
                                month_desc = GetString(worksheet, row, headerMap, "month_desc"),
                                year_desc = GetString(worksheet, row, headerMap, "year_desc"),
                                installment_desc = GetInt(worksheet, row, headerMap, "installment_desc"),
                                plafon_desc = GetInt(worksheet, row, headerMap, "plafon_desc"),
                                cycle_desc = GetInt(worksheet, row, headerMap, "cycle_desc"),
                                nopol = GetString(worksheet, row, headerMap, "nopol")
                            };

                            if (item.id_sales_invoice == 0 && item.invoice_id == 0)
                            {
                                continue;
                            }

                            records.Add(item);
                        }
                    }
                }

                if (records.Count == 0)
                {
                    WriteJson(context, new { error = "Tidak ada data valid di file Excel." });
                    return;
                }

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
                            foreach (var item in records)
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
                            message = $"Successfully processed {records.Count} record(s) from Excel file: {file.FileName}";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            message = $"Error processing Excel file: {ex.Message}";
                            status_code = 100;
                        }
                    }
                }

                WriteJson(context, new
                {
                    status_code = status_code,
                    message = message,
                    data = records
                });
            }
            catch (Exception ex)
            {
                WriteJson(context, new { error = ex.Message });
            }
        }

        public bool IsReusable => false;

        private static void WriteJson(HttpContext context, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            context.Response.Write(json);
        }

        private static Dictionary<string, int> BuildHeaderMap(ExcelWorksheet worksheet)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int lastCol = worksheet.Dimension?.End.Column ?? 0;
            for (int col = 1; col <= lastCol; col++)
            {
                var header = worksheet.Cells[1, col].Text?.Trim();
                if (!string.IsNullOrWhiteSpace(header) && !map.ContainsKey(header))
                {
                    map[header] = col;
                }
            }
            return map;
        }

        private static bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            int lastCol = worksheet.Dimension?.End.Column ?? 0;
            for (int col = 1; col <= lastCol; col++)
            {
                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                {
                    return false;
                }
            }
            return true;
        }

        private static string GetString(ExcelWorksheet worksheet, int row, Dictionary<string, int> map, string column)
        {
            int col;
            if (!map.TryGetValue(column, out col))
            {
                return "";
            }
            return worksheet.Cells[row, col].Text?.Trim() ?? "";
        }

        private static long GetLong(ExcelWorksheet worksheet, int row, Dictionary<string, int> map, string column)
        {
            var text = GetString(worksheet, row, map, column);
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }
            long value;
            if (long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            if (long.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
            {
                return value;
            }
            return 0;
        }

        private static int GetInt(ExcelWorksheet worksheet, int row, Dictionary<string, int> map, string column)
        {
            var text = GetString(worksheet, row, map, column);
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }
            int value;
            if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            if (int.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
            {
                return value;
            }
            return 0;
        }

        private static string GetDateString(ExcelWorksheet worksheet, int row, Dictionary<string, int> map, string column)
        {
            int col;
            if (!map.TryGetValue(column, out col))
            {
                return "";
            }

            var cell = worksheet.Cells[row, col];
            DateTime dt;
            if (cell.Value is DateTime)
            {
                dt = (DateTime)cell.Value;
                return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var text = cell.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            double oaDate;
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out oaDate))
            {
                try
                {
                    var date = DateTime.FromOADate(oaDate);
                    return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch
                {
                    return text;
                }
            }

            if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
            {
                return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return text;
        }
    }
}
