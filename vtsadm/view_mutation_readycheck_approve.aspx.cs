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
    public partial class view_mutation_readycheck_approve : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewMutationReadycheckFieldSort";
        string sViewStateDirSort = "RecViewMutationReadycheckDirSort";
        string sSessionRecList = "RecViewMutationReadycheck";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_mutation_readycheck_approve '" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "ReadycheckID";
                ViewState[sViewStateDirSort] = "DESC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch (Exception ex)
            {
                // Exception handling
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                //if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWMUTREADYCHECKAPR"))
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWMUTDEVICEAPR"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportWarehouse);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportTechnician);
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
                // Exception handling
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
                // Exception handling
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                // Handler for GridView row commands
            }
            catch (Exception ex)
            {
                // Exception handling
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
                // Exception handling
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
                // Exception handling
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 11; i <= 24; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
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
                                              "'" + e.Row.Cells[24].Text.ToString() + "'); return false;";

                    LinkButton CmdDelete = (LinkButton)e.Row.FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    CmdApprove.OnClientClick = "confirmApprove('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    for (int i = 11; i <= 24; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Exception handling
            }
        }
        protected void CmdExportWarehouse_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = "";
                string sMsg = "";
                string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewMutationReadycheckWarehouse"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Mutation Readycheck Warehouse", strFullPath.Trim(), 50000, ref sMsg))
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
                // Exception handling
            }
        }
        protected void CmdExportTechnician_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = "";
                string sMsg = "";
                string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewMutationReadycheckTechnician"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Mutation Readycheck Technician", strFullPath.Trim(), 50000, ref sMsg))
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
                // Exception handling
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0;
                string strSQL = "";
                string sErr = "";
                ExecCommand ec = new ExecCommand();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving or mutation warehouse has been failed!! (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = "";
                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                string ReadycheckID = "";
                //string ReadycheckID = txtReadycheckIDApprove.Value.Trim();

                if (ReadycheckID != "")
                {
                    strSQL = "sp_approve_readycheck_mutation_technician '" + ReadycheckID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Readycheck mutation technician has been approve successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve readycheck mutation technician has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve readycheck mutation technician has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve readycheck mutation technician has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = "";
                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                string ReadycheckID = "";
                //string ReadycheckID = txtReadycheckIDDelete.Value.Trim();

                if (ReadycheckID != "")
                {
                    strSQL = "sp_cancel_readycheck_mutation_technician '" + ReadycheckID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Readycheck mutation technician has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing readycheck mutation technician has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing readycheck mutation technician has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing readycheck mutation technician has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}