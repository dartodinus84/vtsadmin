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
    public partial class gsm_activation_date : System.Web.UI.Page
    {
        protected void Open_GridView(string strBatchNo)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_gsm_upload_activation '" + strBatchNo + "'";
                Session["RecListGSMUpload"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTGSM"))
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
                txtIsUpdate.Value = "0";
                txtBatchNo.Value = "";
                txtFileName.Value = "";
                lblMsg.InnerHtml = "";
                CmdUpload.Visible = true;
                CmdUpdate.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType(); string sErr = ""; string sBatchNo = "";
                if (FileUpload1.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "CSV")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        string strFullPath = Server.MapPath("~/Upload/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);
                        if (ClType.ImportCsvGsmActivation(strFullPath, strFileName, Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString(), ref sBatchNo, ref sErr))
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListGSMUpload"], LblPaging);
            lblMsg.InnerHtml = "";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
 

        }

        protected void CmdUpdate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ExecCommand Ec = new ExecCommand(); string strSQL = ""; int iAff = 0; string sErr = "";
                lblMsg.InnerHtml = "";
                if (txtBatchNo.Value.Trim() != "")
                {
                    strSQL = "sp_update_gsm_upload_activation '" + txtBatchNo.Value.Trim() + "'";
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