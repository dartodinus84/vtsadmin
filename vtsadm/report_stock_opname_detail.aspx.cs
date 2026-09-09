using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class report_stock_opname_detail : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecReportOpnameDetailFieldSort";
        string sViewStateDirSort = "RecReportOpnameDetailDirSort";
        string sSessionRecList = "RecReportOpnameDetail";
        int opnameId = 0;
        string opnameCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Pastikan akses menu valid
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAME"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                // Dapatkan opname_id dari query string
                if (Request.QueryString["opname_id"] != null)
                {
                    if (int.TryParse(Request.QueryString["opname_id"], out opnameId))
                    {
                        if (lblOpnameCode != null)
                        {
                            lblOpnameCode.InnerText = opnameId.ToString(); // Default ke ID jika kode belum didapat
                        }
                    }
                    else
                    {
                        ShowError("Invalid opname ID format");
                        return;
                    }
                }
                else
                {
                    ShowError("No opname ID provided");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        ClsType ClType = new ClsType();
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            ViewState[sViewStateFieldSort] = "detail_id";
                            ViewState[sViewStateDirSort] = "ASC";
                            LoadOpnameDetails();
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
                ShowError("Error initializing page: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            // Pastikan errorContainer dan errorMessage ada
            if (errorContainer != null && errorMessage != null)
            {
                errorContainer.Visible = true;
                errorMessage.InnerText = message;
            }
            else
            {
                // Fallback jika kontrol tidak ditemukan
                Response.Write("<div class='alert alert-danger'>" + message + "</div>");
            }
        }

        private void LoadOpnameDetails()
        {
            try
            {
                ClsType ClType = new ClsType();
                string sErr = "";

                // Validasi parameter
                if (opnameId <= 0)
                {
                    ShowError("Invalid opname ID");
                    return;
                }

                // Buat SQL dengan parameter yang sudah divalidasi
                string strSQL = "sp_report_stock_opname_device_detail " + opnameId;

                // Buat instance Recordset secara eksplisit
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);

                // Cek apakah ada error saat mengambil data
                if (!string.IsNullOrEmpty(sErr))
                {
                    ShowError("Database error: " + sErr);
                    return;
                }

                // Simpan recordset ke session
                Session[sSessionRecList] = Rec.RecData;

                // Periksa apakah recordset memiliki data
                if (Rec.RecordCount() > 0)
                {
                    // Ambil opname_code jika tersedia
                    Rec.MoveFirst();
                    if (Rec.FieldCount() > 8) // Pastikan ada kolom opname_code (indeks 8)
                    {
                        opnameCode = Rec.Fields("opname_code").ToString();
                        if (lblOpnameCode != null)
                        {
                            lblOpnameCode.InnerText = opnameCode;
                        }
                    }

                    // Bind data ke GridView
                    GridView1.DataSource = Rec.RecData;
                    GridView1.DataBind();
                    ClType.showPaging(Rec.RecData, GridView1, LblPaging);
                }
                else
                {
                    // Jika tidak ada data, tampilkan GridView kosong dengan pesan
                    DataTable emptyTable = new DataTable();
                    foreach (DataControlField field in GridView1.Columns)
                    {
                        if (field is BoundField)
                        {
                            string fieldName = ((BoundField)field).DataField;
                            emptyTable.Columns.Add(fieldName, typeof(string));
                        }
                    }
                    GridView1.DataSource = emptyTable;
                    GridView1.DataBind();

                    if (LblPaging != null)
                    {
                        LblPaging.Text = "No records found";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading data: " + ex.Message);
            }
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Session[sSessionRecList] != null)
                {
                    ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList],
                        LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
                }
                else
                {
                    // Jika session kosong, buka kembali data
                    LoadOpnameDetails();
                }
            }
            catch (Exception ex)
            {
                ShowError("Paging error: " + ex.Message);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Tampilkan status physical_found dengan lebih jelas
                    if (e.Row.Cells[4].Text == "1")
                    {
                        e.Row.Cells[4].Text = "True";
                    }
                    else if (e.Row.Cells[4].Text == "0")
                    {
                        e.Row.Cells[4].Text = "False";
                        e.Row.CssClass = "warning"; // Warna kuning untuk tidak ditemukan
                    }

                    // Tampilkan status dengan lebih jelas
                    string currentStatus = e.Row.Cells[3].Text;
                    string physicalStatus = e.Row.Cells[5].Text;

                    // Ubah kode status ke deskripsi yang lebih jelas
                    e.Row.Cells[3].Text = GetStatusDescription(currentStatus);
                    if (!string.IsNullOrEmpty(physicalStatus))
                    {
                        e.Row.Cells[5].Text = GetStatusDescription(physicalStatus);
                    }

                    // Tambahkan badge status jika kolom tersedia
                    Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                    if (lblStatus != null)
                    {
                        if (e.Row.Cells[4].Text == "False")
                        {
                            lblStatus.Text = "Not Found";
                            lblStatus.CssClass = "status-badge badge-warning";
                        }
                        else if (currentStatus != physicalStatus)
                        {
                            lblStatus.Text = "Mismatch";
                            lblStatus.CssClass = "status-badge badge-danger";
                        }
                        else
                        {
                            lblStatus.Text = "Match";
                            lblStatus.CssClass = "status-badge badge-success";
                        }
                    }

                    // Warnai baris sesuai status
                    if (e.Row.Cells[4].Text == "False")
                    {
                        e.Row.CssClass = "warning"; // Warna kuning untuk tidak ditemukan
                    }
                    else if (currentStatus != physicalStatus)
                    {
                        e.Row.CssClass = "danger"; // Warna merah untuk status tidak cocok
                    }
                    else
                    {
                        e.Row.CssClass = "success"; // Warna hijau untuk status cocok
                    }
                }
            }
            catch (Exception ex)
            {
                // Jangan tampilkan error di sini karena akan muncul untuk setiap baris
                // Cukup log error jika ada sistem logging
            }
        }

        // Helper method untuk mendapatkan deskripsi status
        private string GetStatusDescription(string statusCode)
        {
            switch (statusCode)
            {
                case "RG": return "Register";
                case "MW": return "Mutation to Warehouse";
                case "MT": return "Mutation to Technician";
                case "IS": return "Install";
                case "DE": return "Delete";
                default: return statusCode;
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ClsType ClTye = new ClsType();
                if (Session[sSessionRecList] != null)
                {
                    string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session[sSessionRecList],
                        ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                    ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                    ViewState[sViewStateDirSort] = sNewDirSort;
                }
                else
                {
                    // Jika session kosong, buka kembali data
                    LoadOpnameDetails();
                }
            }
            catch (Exception ex)
            {
                ShowError("Sorting error: " + ex.Message);
            }
        }

        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[sSessionRecList] == null)
                {
                    ShowError("No data to export");
                    return;
                }

                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;

                // Gunakan opname_code untuk nama file jika tersedia
                string filePrefix = !string.IsNullOrEmpty(opnameCode) ? opnameCode : opnameId.ToString();
                strFileName = "StockOpnameDetail_" + filePrefix + "_" + dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);

                // Pastikan direktori Export ada
                string exportDir = Server.MapPath("~/Export//");
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }

                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Stock Opname Detail - " + filePrefix, strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }
                    else
                    {
                        ShowError("Export failed: " + sMsg);
                    }
                }
                else
                {
                    ShowError("No records to export");
                }
            }
            catch (Exception ex)
            {
                ShowError("Export error: " + ex.Message);
            }
        }
    }
}