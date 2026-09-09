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
    public partial class view_mst_customer : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewCustomerFieldSort";
        string sViewStateDirSort = "RecViewCustomerDirSort";
        string sSessionRecList = "RecViewCustomer";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_master_customer '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "FullName";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWLISTMSTCUST"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                //((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportVehicle);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDocument);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportPasang);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportPindah);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportLepas);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportInvoice);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportAccount);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportUser);


                //((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportVehicleXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportDocumentXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportPasangXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportPindahXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportLepasXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportInvoiceXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportAccountXls);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportUserXls);

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
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
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
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewCustomer"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Customer", strFullPath.Trim(), 50000, ref sMsg))
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
                    for (int i = 6; i <= 29; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[30].Visible = true;
                    //e.Row.Cells[26].Visible = true;
                    //e.Row.Cells[27].Visible = true;
                    /**
                    for (int i = 31; i <= 40; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    */

                    e.Row.Cells[31].Visible = false;
                    e.Row.Cells[34].Visible = false;
                    e.Row.Cells[38].Visible = false;
                    e.Row.Cells[39].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdDetails");
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[6].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[10].Text.ToString() + "','" + e.Row.Cells[11].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[12].Text.ToString() + "','" + e.Row.Cells[13].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[14].Text.ToString() + "','" + e.Row.Cells[15].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[16].Text.ToString() + "','" + e.Row.Cells[17].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[18].Text.ToString() + "','" + e.Row.Cells[19].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[20].Text.ToString() + "','" + e.Row.Cells[21].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[22].Text.ToString() + "','" + e.Row.Cells[23].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[24].Text.ToString() + "','" + e.Row.Cells[25].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[26].Text.ToString() + "','" + e.Row.Cells[27].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[28].Text.ToString() + "','" + e.Row.Cells[29].Text.ToString() + "'); return false;";


                    LinkButton CmdLog = (LinkButton)e.Row.FindControl("CmdLog");
                    CmdLog.OnClientClick = "postLog('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdVehicle = (LinkButton)e.Row.FindControl("CmdVehicle");
                    CmdVehicle.OnClientClick = "postVehicle('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdPasang = (LinkButton)e.Row.FindControl("CmdPasang");
                    CmdPasang.OnClientClick = "postPasang('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdPindah = (LinkButton)e.Row.FindControl("CmdPindah");
                    CmdPindah.OnClientClick = "postPindah('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdLepas = (LinkButton)e.Row.FindControl("CmdLepas");
                    CmdLepas.OnClientClick = "postUninstall('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdInvoice = (LinkButton)e.Row.FindControl("CmdInvoice");
                    CmdInvoice.OnClientClick = "postInvoice('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdAccount = (LinkButton)e.Row.FindControl("CmdAccount");
                    CmdAccount.OnClientClick = "postAccount('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdDocument = (LinkButton)e.Row.FindControl("CmdDocument");
                    CmdDocument.OnClientClick = "postDocument('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdUser = (LinkButton)e.Row.FindControl("CmdUser");
                    CmdUser.OnClientClick = "postUser('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    for (int i = 6; i <= 29; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[30].Visible = true;
                    //e.Row.Cells[26].Visible = true;
                    //e.Row.Cells[27].Visible = true;

                    /**
                    for (int i = 31; i <= 40; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    */

                    e.Row.Cells[31].Visible = false;
                    e.Row.Cells[34].Visible = false;
                    e.Row.Cells[38].Visible = false;
                    e.Row.Cells[39].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdExportVehicle_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerVehicle"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Vehicle", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportPasang_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerPasang"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Vehicle New Install", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportPindah_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerPindah"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Vehicle Maintenance", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportLepas_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerLepas"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Vehicle Uninstall", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportInvoice_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerInvoice"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Invoice", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportAccount_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerAccoount"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Account", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportDocument_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerDocument"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - Document", strFullPath.Trim(), 50000, ref sMsg))
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
        protected void CmdExportUser_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            ClsType clType = new ClsType();
            Recordset Rec = new Recordset();
            string strFullPath = ""; string sMsg = ""; string strFileName = "";
            DateTime dt = DateTime.Now;
            strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            strFullPath = Server.MapPath("~/Export//" + strFileName);
            Rec.RecData = Session["RecViewCustomerUser"] as System.Data.DataSet;
            if (Rec.RecordCount() > 0)
            {
                if (clType.ExportToCsvTab(Rec, "Customer - User ", strFullPath.Trim(), 50000, ref sMsg))
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

        protected void CmdExportXls_Click(object sender, EventArgs e)
        {
            try
            {
                //ClsType clType = new ClsType();
                //Recordset Rec = new Recordset();
                //string strFullPath = ""; string sMsg = ""; string strFileName = "";
                //DateTime dt = DateTime.Now;
                //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                //strFullPath = Server.MapPath("~/Export//" + strFileName);
                //Rec.RecData = Session["RecViewCustomer"] as System.Data.DataSet;
                //if (Rec.RecordCount() > 0)
                //{
                //    if (clType.ExportToCsvTab(Rec, "Customer", strFullPath.Trim(), 50000, ref sMsg))
                //    {
                //        Response.Clear();
                //        Response.ContentType = "text/plain";
                //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                //        Response.TransmitFile(strFullPath);
                //        Response.Flush();
                //        File.Delete(strFullPath);
                //        Response.End();
                //    }

                //}
                //else
                //{
                //    div_comment.InnerHtml = "No records found";
                //}
                div_comment.InnerHtml = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
                Rec.RecData = Session["RecViewCustomer"] as System.Data.DataSet;
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
        protected void CmdExportVehicleXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerVehicle"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Vehicle", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerVehicle"] as System.Data.DataSet;
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
        protected void CmdExportPasangXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerPasang"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Vehicle New Install", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerPasang"] as System.Data.DataSet;
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
        protected void CmdExportPindahXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerPindah"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Vehicle Maintenance", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerPindah"] as System.Data.DataSet;
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
        protected void CmdExportLepasXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerLepas"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Vehicle Uninstall", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerLepas"] as System.Data.DataSet;
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
        protected void CmdExportInvoiceXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerInvoice"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Invoice", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerInvoice"] as System.Data.DataSet;
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
        protected void CmdExportAccountXls_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerAccount"] as System.Data.DataSet;
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
        protected void CmdExportDocumentXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerDocument"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - Document", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerDocument"] as System.Data.DataSet;
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
        protected void CmdExportUserXls_Click(object sender, EventArgs e)
        {
            //div_comment.InnerHtml = "";
            //ClsType clType = new ClsType();
            //Recordset Rec = new Recordset();
            //string strFullPath = ""; string sMsg = ""; string strFileName = "";
            //DateTime dt = DateTime.Now;
            //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
            //strFullPath = Server.MapPath("~/Export//" + strFileName);
            //Rec.RecData = Session["RecViewCustomerUser"] as System.Data.DataSet;
            //if (Rec.RecordCount() > 0)
            //{
            //    if (clType.ExportToCsvTab(Rec, "Customer - User ", strFullPath.Trim(), 50000, ref sMsg))
            //    {
            //        Response.Clear();
            //        Response.ContentType = "text/plain";
            //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
            //        Response.TransmitFile(strFullPath);
            //        Response.Flush();
            //        File.Delete(strFullPath);
            //        Response.End();
            //    }
            //}
            //else
            //{
            //    div_comment.InnerHtml = "No records found";
            //}
            div_comment.InnerHtml = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView gv = new GridView();
            Recordset Rec = new Recordset();
            DateTime dt = DateTime.Now;
            string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
            Rec.RecData = Session["RecViewCustomerUser"] as System.Data.DataSet;
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