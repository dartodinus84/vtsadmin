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
    public partial class vehicle_upload : System.Web.UI.Page
    {
        protected void Open_GridView(string strBatchNo)
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_list_vehicle_upload '" + strBatchNo + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                GridView1.DataSource = Rec.DataRecord();
                GridView1.DataBind();
                Session["RecListVehicleUpload"] = Rec.RecData;
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTVEH"))
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
                            lblMsg.InnerHtml = "";
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
                        if (ClType.ImportCsvVehicle(strFullPath, strFileName, Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString(), ref sBatchNo, ref sErr))
                        {
                            txtBatchNo.Value = sBatchNo;
                            txtFileName.Value = strFileName;
                            Open_GridView(sBatchNo);
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

        protected void CmdSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                CmdError.Visible = false;
                ExecCommand Ec = new ExecCommand(); string strSQL = ""; int iAff = 0; string sErr = "";
                lblMsg.InnerHtml = "";
                if (txtBatchNo.Value.Trim() != "")
                {
                    strSQL = "sp_insert_vehicle_upload_submit '" + txtBatchNo.Value.Trim() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref iAff, ref sErr))
                    {
                        if (iAff > 0)
                        {
                            clear();
                            Open_GridView("");
                            lblMsg.InnerHtml = "<strong>Success!</strong> All data has been submit successfully!";
                        }
                    }
                    else
                    {
                        CmdError.Visible = true;
                        lblMsg.InnerHtml = "<strong>Failed!</strong> All data has been failed to submit (" + sErr + ")!";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> All data has been failed to submit (" + ex.Message + ")!";
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).DataSource = Session["RecListVehicleUpload"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
            lblMsg.InnerHtml = "";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 8; i <= 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 9; i++)
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
                strSQL = "sp_get_data_vehicle_upload_failed '" + txtBatchNo.Value.Trim() + "'";
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
    }
}