using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class gsm_mutation_technician_upload : System.Web.UI.Page
    {
        protected void Open_GridView(string strBatchNo)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_trx_gsm_mutation_technician_upload '" + strBatchNo + "'";
                Session["RecListGsmMutUpload"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMUTGSMTECH"))
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
                            Open_GridView("");
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
                txtBatchNo.Value = "";
                txtFileName.Value = "";
                lblMsg.InnerHtml = "";
                CmdUpload.Visible = true;
                CmdUpdate.Visible = false;
                CmdError.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CmdError.Visible = false;
                ClsType ClType = new ClsType(); string sErr = ""; string sBatchNo = "";
                if (FileUpload1.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "CSV")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        string strFullPath = Server.MapPath("~/Upload/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);
                        if (ClType.ImportCsvGsmMutation(strFullPath, strFileName, Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString(), ref sBatchNo, ref sErr))
                        {
                            txtBatchNo.Value = sBatchNo;
                            txtFileName.Value = strFileName;
                            Open_GridView(sBatchNo);
                            CmdUpload.Visible = false;
                            CmdUpdate.Visible = true;
                            lblMsg.InnerHtml = "<strong>Success!</strong> File has been upload successfully!";
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Upload file has been failed (" + sErr + ")!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> Upload file has been failed (" + ex.Message + ")!";
            }
        }

        protected void CmdCancel_ServerClick(object sender, EventArgs e)
        {
            Open_GridView("");
            clear();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListGsmMutUpload"], LblPaging);
            lblMsg.InnerHtml = "";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 4; i <= 6; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 4; i <= 6; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdError_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset(); string strSQL = "";
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                strFileName = txtFileName.Value.Trim();
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                strSQL = "sp_get_data_gsm_mutation_upload_failed_update '" + txtBatchNo.Value.Trim() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "No records found";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CmdError.Visible = false;
                ExecCommand Ec = new ExecCommand(); string strSQL = ""; int iAff = 0; string sErr = "";
                lblMsg.InnerHtml = "";
                if (txtBatchNo.Value.Trim() != "")
                {
                    strSQL = "sp_insert_gsm_mutation_upload_update '" + txtBatchNo.Value.Trim() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref iAff, ref sErr))
                    {
                        if (iAff > 0)
                        {
                            clear();
                            Open_GridView("");
                            lblMsg.InnerHtml = "<strong>Success!</strong> All data has been update successfully!";
                        }
                    }
                    else
                    {
                        CmdError.Visible = true;
                        lblMsg.InnerHtml = "<strong>Failed!</strong> All data has been failed to update (" + sErr + ")!";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> All data has been failed to update (" + ex.Message + ")!";
            }
        }
    }
}