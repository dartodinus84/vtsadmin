using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_dlo_create_details_device : System.Web.UI.Page
    {
        private class UploadSnRow
        {
            public string DeviceGroupID = "";
            public string DeviceTypeID = "";
            public string NoSN = "";
            public string PackingList = "";
            public string BatchNo = "";
            public string Remark = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                var access = Session["ClsTypeAccessMenu"].ToString().ToUpper();
                if (!access.Contains("MNUPRCREATE") && !access.Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.Text = "";
                            txtDloID.Value = (Request.QueryString["DloID"] ?? "").Trim();
                            txtSeq.Value = (Request.QueryString["Seq"] ?? "").Trim();
                            txtDloIDView.Text = txtDloID.Value;
                            txtSeqView.Text = txtSeq.Value;
                            clear();
                            Open_GridView();
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
                txtNoSN.Text = "";
                txtPackingList.Text = "";
                txtBatchNo.Text = "";
                txtRemark.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        private void Open_GridView()
        {
            try
            {
                if ((txtDloID.Value ?? "").Trim() == "" || (txtSeq.Value ?? "").Trim() == "")
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    return;
                }
                string strSQL = "sp_list_dlo_detail_device '" + txtDloID.Value.Trim().Replace("'", "''") + "'," + txtSeq.Value.Trim();
                Recordset rec = new Recordset();
                rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (rec.RecData != null && rec.RecData.Tables.Count > 0)
                    GridView1.DataSource = rec.RecData.Tables[0];
                else
                    GridView1.DataSource = null;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdClearDetail_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if ((txtDloID.Value ?? "").Trim() != "" && (txtSeq.Value ?? "").Trim() != "")
                {
                    if ((txtNoSN.Text ?? "").Trim() != "")
                    {
                        Int32 intAff = 0; String strSQL = ""; string sErr = "";
                        ExecCommand ec = new ExecCommand();
                        strSQL = "sp_insert_dlo_detail_device '" + txtDloID.Value.Trim().Replace("'", "''") + "'," + txtSeq.Value.Trim().Replace("'", "''") + ",'" +
                            txtNoSN.Text.Trim().Replace("'", "''") + "','" + txtPackingList.Text.Trim().Replace("'", "''") + "','" + txtBatchNo.Text.Trim().Replace("'", "''") + "','" +
                            txtRemark.Text.Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            clear();
                            Open_GridView();
                            lblMsg.Text = "<strong>Success!</strong> Device SN berhasil ditambahkan";
                        }
                        else
                        {
                            lblMsg.Text = "<strong>Failed!</strong> " + sErr;
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "<strong>Failed!</strong> Konteks BAST diperlukan";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "<strong>Failed!</strong> " + ex.Message;
            }
        }

        protected void CmdUploadSN_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if ((txtDloID.Value ?? "").Trim() == "" || (txtSeq.Value ?? "").Trim() == "")
                {
                    lblMsg.Text = "<strong>Failed!</strong> Konteks BAST diperlukan";
                    return;
                }
                if (FileUpload1 == null || !FileUpload1.HasFile)
                {
                    lblMsg.Text = "<strong>Failed!</strong> Please choose file first";
                    return;
                }

                string ext = Path.GetExtension(FileUpload1.FileName ?? "").ToLowerInvariant();
                List<UploadSnRow> rows = new List<UploadSnRow>();

                if (ext == ".xlsx")
                {
                    rows = ReadSnFromXlsx(FileUpload1.FileContent);
                }
                else if (ext == ".csv")
                {
                    rows = ReadSnFromCsv(FileUpload1.FileContent);
                }
                else
                {
                    lblMsg.Text = "<strong>Failed!</strong> File extension is not supported. Use .xlsx or .csv";
                    return;
                }

                if (rows.Count == 0)
                {
                    lblMsg.Text = "<strong>Failed!</strong> No SN data found in file";
                    return;
                }

                int success = 0;
                int failed = 0;
                string firstErr = "";

                foreach (UploadSnRow row in rows)
                {
                    if ((row.NoSN ?? "").Trim() == "") continue;

                    Int32 intAff = 0;
                    string sErr = "";
                    string strSQL = "sp_insert_dlo_detail_device '" + txtDloID.Value.Trim().Replace("'", "''") + "'," + txtSeq.Value.Trim().Replace("'", "''") + ",'" +
                        row.NoSN.Trim().Replace("'", "''") + "','" + (row.PackingList ?? "").Trim().Replace("'", "''") + "','" + (row.BatchNo ?? "").Trim().Replace("'", "''") + "','" +
                        (row.Remark ?? "").Trim().Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";

                    ExecCommand ec = new ExecCommand();
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        success++;
                    }
                    else
                    {
                        failed++;
                        if (firstErr == "") firstErr = sErr;
                    }
                }

                Open_GridView();
                if (failed == 0)
                {
                    lblMsg.Text = "<strong>Success!</strong> Upload SN berhasil. Total insert: " + success;
                }
                else
                {
                    lblMsg.Text = "<strong>Warning!</strong> Upload selesai. Success: " + success + ", Failed: " + failed + (firstErr == "" ? "" : ". First error: " + HttpUtility.HtmlEncode(firstErr));
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "<strong>Failed!</strong> " + ex.Message;
            }
        }

        private List<UploadSnRow> ReadSnFromCsv(Stream stream)
        {
            List<UploadSnRow> rows = new List<UploadSnRow>();
            stream.Position = 0;
            using (StreamReader sr = new StreamReader(stream))
            {
                bool firstRowChecked = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == null) continue;
                    string trimmed = line.Trim();
                    if (trimmed == "") continue;

                    string[] parts = line.Split(',');
                    if (parts.Length == 1 && line.IndexOf(';') >= 0) parts = line.Split(';');
                    if (parts.Length == 1 && line.IndexOf('\t') >= 0) parts = line.Split('\t');

                    if (!firstRowChecked)
                    {
                        firstRowChecked = true;
                        string h = (parts.Length > 0 ? parts[0] : "").Trim().ToUpperInvariant();
                        if (h == "DEVICEGROUPID" || h == "NOSN" || h == "NO SN" || h == "SN") continue;
                    }

                    UploadSnRow row = new UploadSnRow();
                    if (parts.Length >= 6)
                    {
                        row.DeviceGroupID = parts[0].Trim();
                        row.DeviceTypeID = parts[1].Trim();
                        row.NoSN = parts[2].Trim();
                        row.PackingList = parts[3].Trim();
                        row.BatchNo = parts[4].Trim();
                        row.Remark = parts[5].Trim();
                    }
                    else
                    {
                        row.NoSN = parts.Length > 0 ? parts[0].Trim() : "";
                        row.PackingList = parts.Length > 1 ? parts[1].Trim() : "";
                        row.BatchNo = parts.Length > 2 ? parts[2].Trim() : "";
                        row.Remark = parts.Length > 3 ? parts[3].Trim() : "";
                    }
                    if (row.NoSN != "") rows.Add(row);
                }
            }
            return rows;
        }

        private List<UploadSnRow> ReadSnFromXlsx(Stream stream)
        {
            List<UploadSnRow> rows = new List<UploadSnRow>();
            stream.Position = 0;
            ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
                if (worksheet == null || worksheet.Dimension == null) return rows;

                int firstRow = worksheet.Dimension.Start.Row;
                int lastRow = worksheet.Dimension.End.Row;
                int lastCol = worksheet.Dimension.End.Column;

                bool skipHeader = false;
                string firstCell = (worksheet.Cells[firstRow, 1].Text ?? "").Trim().ToUpperInvariant();
                if (firstCell == "DEVICEGROUPID" || firstCell == "NOSN" || firstCell == "NO SN" || firstCell == "SN") skipHeader = true;

                for (int rowIdx = firstRow; rowIdx <= lastRow; rowIdx++)
                {
                    if (skipHeader && rowIdx == firstRow) continue;
                    UploadSnRow row = new UploadSnRow();
                    if (lastCol >= 6)
                    {
                        row.DeviceGroupID = (worksheet.Cells[rowIdx, 1].Text ?? "").Trim();
                        row.DeviceTypeID = (worksheet.Cells[rowIdx, 2].Text ?? "").Trim();
                        row.NoSN = (worksheet.Cells[rowIdx, 3].Text ?? "").Trim();
                        row.PackingList = (worksheet.Cells[rowIdx, 4].Text ?? "").Trim();
                        row.BatchNo = (worksheet.Cells[rowIdx, 5].Text ?? "").Trim();
                        row.Remark = (worksheet.Cells[rowIdx, 6].Text ?? "").Trim();
                    }
                    else
                    {
                        row.NoSN = (worksheet.Cells[rowIdx, 1].Text ?? "").Trim();
                        row.PackingList = (worksheet.Cells[rowIdx, 2].Text ?? "").Trim();
                        row.BatchNo = (worksheet.Cells[rowIdx, 3].Text ?? "").Trim();
                        row.Remark = (worksheet.Cells[rowIdx, 4].Text ?? "").Trim();
                    }
                    if (row.NoSN != "") rows.Add(row);
                }
            }
            return rows;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() != "DELETESN") return;
                string noSN = (e.CommandArgument ?? "").ToString().Trim();
                if (noSN == "") return;

                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                strSQL = "sp_delete_dlo_detail_device '" + txtDloID.Value.Trim().Replace("'", "''") + "'," + txtSeq.Value.Trim().Replace("'", "''") + ",'" + noSN.Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString() + "'";
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    Open_GridView();
                    lblMsg.Text = "<strong>Success!</strong> Device SN berhasil dihapus";
                }
                else
                {
                    lblMsg.Text = "<strong>Failed!</strong> " + sErr;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "<strong>Failed!</strong> " + ex.Message;
            }
        }
    }
}
