using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_delivery_order : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewProcurementFieldSort";
        string sViewStateDirSort = "RecViewProcurementDirSort";
        string sSessionRecList = "RecViewProcurement";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_dlo '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "DloID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWPROCU"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDetails);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDone);
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
                txtSearch.Text = "";
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
                Rec.RecData = Session["RecViewProcurement"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Delivery Order", strFullPath.Trim(), 50000, ref sMsg))
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
                if (e.Row.RowType == DataControlRowType.Header)
                {
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdProcurementDetails = (LinkButton)e.Row.FindControl("CmdProcurementDetails");
                    CmdProcurementDetails.OnClientClick = "postProcurementDetails('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdDone = (LinkButton)e.Row.FindControl("CmdProcurementDone");
                    CmdDone.OnClientClick = "postProcurementDone('" + e.Row.Cells[0].Text.ToString() + "'); return false;";
                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdExportDetails_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewProcurementDetails"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Delivery Order Details", strFullPath.Trim(), 50000, ref sMsg))
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

        protected void CmdExportDone_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewProcurementDone"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Delivery Order Done", strFullPath.Trim(), 50000, ref sMsg))
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