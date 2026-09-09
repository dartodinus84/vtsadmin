using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using System.Drawing;
using System.Diagnostics;

namespace vtsadm
{
    public partial class dashboard_assign_technician_detail : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewJobCreateFieldSort";
        string sViewStateDirSort = "RecViewJobCreateDirSort";
        string sSessionRecList = "RecViewJobCreate";

        protected void Open_GridView(string type, string from, string to, string areaid, string picareaid)
        {
            try
            {

                ClsType ClType = new ClsType();
                string strSQL = "sp_dashboard_assign_technician_detail '" + type + "', '" + areaid + "','" + picareaid + "','" + from + "','" + to + "'";
                ViewState[sViewStateFieldSort] = "CustID";
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
                Session["ClsTypeCustDetail"] = "";
                Session["ClsAreaCustDetail"] = "";
                Session["ClsPicCustDetail"] = "";

                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHASSIGNTECH"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportXls);

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();

                            string type = Request.QueryString["type"];
                            string from = Request.QueryString["from"];
                            string to = Request.QueryString["to"];
                            string areaid = Request.QueryString["area"];
                            string picareaid = Request.QueryString["pic"];

                            Session["ClsTypeCustDetail"] = type;
                            Session["ClsAreaCustDetail"] = areaid;
                            Session["ClsPicCustDetail"] = picareaid;

                            //Debug.WriteLine("TESTING 123" + type);
                            Open_GridView(type, from, to, areaid, picareaid);

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
                string type = Session["ClsTypeCustDetail"].ToString();
                string from = Session["ClsTypeCustDetail"].ToString();
                string to = Session["ClsTypeCustDetail"].ToString();
                string areaid = Session["ClsAreaCustDetail"].ToString();
                string picareaid = Session["ClsPicCustDetail"].ToString();

                Open_GridView(type, from, to, areaid, picareaid);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            string type = Session["ClsTypeCustDetail"].ToString();
            string from = Session["ClsTypeCustDetail"].ToString();
            string to = Session["ClsTypeCustDetail"].ToString();
            string areaid = Session["ClsAreaCustDetail"].ToString();
            string picareaid = Session["ClsPicCustDetail"].ToString();

            Open_GridView(type, from, to, areaid, picareaid);
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
                Rec.RecData = Session["RecViewJobCreate"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Job Assign Technician", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportXls_Click(object sender, EventArgs e)
        {
            try
            {

                div_comment.InnerHtml = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
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
                
            }
            catch (Exception ex)
            {

            }
        }


        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }

        protected void GridView2_RowDeleting(Object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {

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
            Rec.RecData = Session["RecViewJobCreateDetails"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Job Order New Installation Details", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportDetailsXls_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewJobCreateDetails"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                gv.DataSource = Rec.RecData;
                gv.AllowPaging = false;
                gv.DataBind();
                gv.RenderControl(hw);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.Charset = "";
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                div_comment.InnerHtml = "No records found";
            }
        }
        protected void CmdExportDone_Click(object sender, EventArgs e)
        {
            
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewJobCreateDone"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                gv.DataSource = Rec.RecData;
                gv.AllowPaging = false;
                gv.DataBind();
                gv.RenderControl(hw);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.Charset = "";
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                div_comment.InnerHtml = "No records found";
            }
        }
        protected void CmdExportDoneXls_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewJobCreateDone"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                gv.DataSource = Rec.RecData;
                gv.AllowPaging = false;
                gv.DataBind();
                gv.RenderControl(hw);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.Charset = "";
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
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