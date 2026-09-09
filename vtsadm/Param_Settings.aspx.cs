using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class Param_Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSETPAR"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_Gridview1();
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

        protected void Open_Gridview1()
        {
            ClsType ClType = new ClsType();
            string strSQL = $"sp_get_list_param '{txt_company_name.Text.Trim()}', '{txt_param_type.Text.Trim()}'";
            ViewState["RecListParamFieldSort"] = "par_type";
            ViewState["RecListParamDirSort"] = "ASC";
            Session["RecListParam"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingParam, ViewState["RecListParamFieldSort"].ToString(), ViewState["RecListParamDirSort"].ToString());
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListParam"], LblPagingParam, ViewState["RecListParamFieldSort"].ToString(), ViewState["RecListParamDirSort"].ToString());
            clear();
            
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_Gridview1();
                comment_save.InnerHtml = "";
                div_comment.InnerHtml = "";
                txtCompanyID.Value = "";
                txtCompID.Value = "";
                txtCompanyName.Text = "";
                txtParamType.Text = "";
                txtParamCode.Text = "";
                txtParamValue.Text = "";
                CmdSave.Style.Remove("display");
                CmdEdit.Style.Add("display", "none");
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdDelete");
                    LinkButton CmdEdit = (LinkButton)e.Row.FindControl("CmdEdit");
                    CmdEdit.OnClientClick = $"postEdit({e.Row.Cells[0].Text.ToString()}, {e.Row.Cells[1].Text.ToString()}, '{e.Row.Cells[2].Text.ToString()}', '{e.Row.Cells[3].Text.ToString()}', '{e.Row.Cells[4].Text.ToString()}', '{e.Row.Cells[5].Text.ToString()}', '{e.Row.Cells[6].Text.ToString()}'); return false;";
                    CmdButton.OnClientClick = "postDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = $"selectCustomer('{e.Row.Cells[0].Text.ToString()}', '{e.Row.Cells[1].Text.ToString()}')";
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            comment_save.InnerHtml = "";
            try
            {
                string strSQL = "";
                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                if (txtParamIDDelete.Value.Trim() != "")
                {
                    strSQL = "sp_delete_param " + txtParamIDDelete.Value.Trim();
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Param has been removed successfully!</div>";

                            Open_Gridview1();
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing param has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing param has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing param has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSave_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            ExecCommand Ec = new ExecCommand();
            int intAff = 0;
            string sErr = "";

            var company_id = txtCompID.Value.Trim();
            var company_nm = txtCompanyName.Text.Trim();
            var par_type = txtParamType.Text.Trim();
            var par_code = txtParamCode.Text.Trim();
            var par_value = txtParamValue.Text.Trim();
            var par_note = txtParamNote.Text.Trim();

            if(string.IsNullOrEmpty(company_id) || string.IsNullOrEmpty(company_nm))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Company information cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(par_type))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Type cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(par_code))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Code cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(par_value))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Value cannot empty!!</div>";
            }
            else
            {
                try
                {
                    strSQL = $"sp_insert_param {company_id}, '{par_type}', '{par_code}', '{par_value}', '{par_note}'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            comment_save.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!!</strong> Insert param success</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.Text = "";
                            txtParamNote.Text = "";
                            Open_Gridview1();
                        }
                        else
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert param has been failed ({sErr})</div>";
                        }
                    }
                    else
                    {
                        comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert param has been failed ({sErr})</div>";
                    }
                }
                catch (Exception ex)
                {
                    comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Insert param has been failed ({sErr})</div>";
                }
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            txtAutoId.Value = "";
            comment_save.InnerHtml = "";
            div_comment.InnerHtml = "";
            txtCompanyID.Value = "";
            txtCompID.Value = "";
            txtCompanyName.Text = "";
            txtParamType.Text = "";
            txtParamCode.Text = "";
            txtParamValue.Text = "";
            txtParamNote.Text = "";
            CmdEdit.Style.Add("display", "none");
        }

        protected void clear()
        {
            txtAutoId.Value = "";
            comment_save.InnerHtml = "";
            div_comment.InnerHtml = "";
            txtCompanyID.Value = "";
            txtCompID.Value = "";
            txtCompanyName.Text = "";
            txtParamType.Text = "";
            txtParamCode.Text = "";
            txtParamValue.Text = "";
            txtParamNote.Text = "";
            CmdEdit.Style.Add("display", "none");
        }

        public void CmdYesUpdate_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            ExecCommand Ec = new ExecCommand();
            int intAff = 0;
            string sErr = "";

            string autoid = txtAutoId.Value;
            string company_id = txtCompID.Value;
            string company_nm = txtCompanyName.Text.Trim();
            string param_type = txtParamType.Text.Trim();
            string param_code = txtParamCode.Text.Trim();
            string param_value = txtParamValue.Text.Trim();
            string param_note = txtParamNote.Text.Trim();

            if (string.IsNullOrEmpty(company_id) || string.IsNullOrEmpty(company_nm))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Company information cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(param_type))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Type cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(param_code))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Code cannot empty!!</div>";
            }
            else if (string.IsNullOrEmpty(param_value))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Param Value cannot empty!!</div>";
            } else if (string.IsNullOrEmpty(autoid))
            {
                comment_save.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>auto_id is empty!! (Internal error)</div>";
            } else
            {
                try
                {
                    strSQL = $"usp_update_param {autoid}, {company_id}, '{param_type}', '{param_code}', '{param_value}', '{param_note}'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Update Success!</strong> Param {autoid} has been updated successfully!</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.Text = "";
                            txtParamNote.Text = "";
                            CmdSave.Style.Remove("display");
                            CmdEdit.Style.Add("display", "none");
                            Open_Gridview1();
                        }
                        else
                        {
                            comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update param has been failed ({sErr})</div>";
                            div_comment.InnerHtml = "";
                            txtCompanyID.Value = "";
                            txtCompID.Value = "";
                            txtCompanyName.Text = "";
                            txtParamType.Text = "";
                            txtParamCode.Text = "";
                            txtParamValue.Text = "";
                            txtParamNote.Text = "";
                            CmdSave.Style.Remove("display");
                            CmdEdit.Style.Add("display", "none");
                            Open_Gridview1();
                        }
                    }
                    else
                    {
                        comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update param has been failed ({sErr})</div>";
                        div_comment.InnerHtml = "";
                        txtCompanyID.Value = "";
                        txtCompID.Value = "";
                        txtCompanyName.Text = "";
                        txtParamType.Text = "";
                        txtParamCode.Text = "";
                        txtParamValue.Text = "";
                        txtParamNote.Text = "";
                        CmdSave.Style.Remove("display");
                        CmdEdit.Style.Add("display", "none");
                        Open_Gridview1();
                    }
                }
                catch (Exception ex)
                {
                    comment_save.InnerHtml = $"<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!!</strong> Update param has been failed ({sErr})</div>";
                    div_comment.InnerHtml = "";
                    txtCompanyID.Value = "";
                    txtCompID.Value = "";
                    txtCompanyName.Text = "";
                    txtParamType.Text = "";
                    txtParamCode.Text = "";
                    txtParamValue.Text = "";
                    txtParamNote.Text = "";
                    CmdSave.Style.Remove("display");
                    CmdEdit.Style.Add("display", "none");
                    Open_Gridview1();
                }
            }
        }
    }
}