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
    public partial class rpt_job_maint_gsms : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecRptJobMaintGsmFieldSort";
        string sViewStateDirSort = "RecRptJobMaintGsmDirSort";
        string sSessionRecList = "RecRptJobMaintGsm";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_rpt_job_maint_gsm_news '" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "','" + CmbDeviceType.SelectedItem.Value.Trim() + "','" + CmbBranch.SelectedItem.Value.Trim() + "'";
                ViewState[sViewStateFieldSort] = "JobID";
                ViewState[sViewStateDirSort] = "ASC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNURPTJOBMAINTGSM"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
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
                ClsType ClType = new ClsType();
                txtSearch.Text = "";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                ClType.Open_Combos(CmbDeviceType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "[sp_list_device_type]");
                CmbDeviceType.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbBranch, Session["ClsTypeDBConnStringSQL"].ToString(), "", "[sp_list_branch]");
                CmbBranch.SelectedValue = "[Select]";
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }

        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Job Order Details Maintenance Gsm", strFullPath.Trim(), 50000, ref sMsg))
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
                    div_comment.InnerHtml = "No records found";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.Header)
                //{
                //    for (int i = 5; i <= 8; i++)
                //    {
                //        e.Row.Cells[i].Visible = false;
                //    }
                //}
                //else if (e.Row.RowType == DataControlRowType.DataRow)
                //{

                //    for (int i = 5; i <= 8; i++)
                //    {
                //        e.Row.Cells[i].Visible = false;
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}