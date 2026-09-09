using System;
using System.Data;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class approval_tms_connect : System.Web.UI.Page
    {
        private const string SessionGridKey = "RecListApprovalTmsConnect";
        private const string ViewStateSortField = "RecListApprovalTmsConnectFieldSort";
        private const string ViewStateSortDir = "RecListApprovalTmsConnectDirSort";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTTMSCON"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            OpenGridview();
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
            catch
            {
            }
        }

        protected void OpenGridview()
        {
            ClsType clType = new ClsType();
            string searchValue = SanitizeParamValue(txtSearch.Text);
            string strSql = $"sp_tbl_pc_register_search '{searchValue}'";

            ViewState[ViewStateSortField] = "customer_company_nm";
            ViewState[ViewStateSortDir] = "ASC";

            Session[SessionGridKey] = clType.Open_GridView(
                GridView1,
                strSql,
                Session["ClsTypeDBConnStringSQL"].ToString(),
                LblPagingParam,
                ViewState[ViewStateSortField].ToString(),
                ViewState[ViewStateSortDir].ToString());
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridView1.PageIndex = 0;
                OpenGridview();
                div_comment.InnerHtml = string.Empty;
            }
            catch
            {
            }
        }

        protected void CmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = string.Empty;
                GridView1.PageIndex = 0;
                OpenGridview();
                div_comment.InnerHtml = string.Empty;
            }
            catch
            {
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType clType = new ClsType();
            clType.Gv_PageIndexChanging(
                sender as GridView,
                e.NewPageIndex,
                Session[SessionGridKey],
                LblPagingParam,
                ViewState[ViewStateSortField].ToString(),
                ViewState[ViewStateSortDir].ToString());
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                DataRowView rowView = e.Row.DataItem as DataRowView;
                if (rowView == null)
                {
                    return;
                }

                string rawEmail = Convert.ToString(rowView["email"] ?? string.Empty).Trim();
                string rawStatus = Convert.ToString(rowView["status_verifikasi"] ?? string.Empty).Trim();
                string statusUpper = rawStatus.ToUpper();

                HyperLink lnkEmail = e.Row.FindControl("LnkEmail") as HyperLink;
                if (lnkEmail != null)
                {
                    if (!string.IsNullOrWhiteSpace(rawEmail) && !rawEmail.Equals("&nbsp;", StringComparison.OrdinalIgnoreCase))
                    {
                        lnkEmail.Text = Server.HtmlEncode(rawEmail);
                        lnkEmail.NavigateUrl = "mailto:" + rawEmail;
                    }
                    else
                    {
                        lnkEmail.Text = "-";
                        lnkEmail.NavigateUrl = string.Empty;
                    }
                }

                Label lblStatus = e.Row.FindControl("LblStatus") as Label;
                if (lblStatus != null)
                {
                    if (statusUpper == "ACTIVE")
                    {
                        lblStatus.Text = "ACTIVE";
                        lblStatus.CssClass = "label label-success";
                    }
                    else
                    {
                        lblStatus.Text = string.IsNullOrWhiteSpace(rawStatus) ? "-" : Server.HtmlEncode(rawStatus);
                        lblStatus.CssClass = "label label-default";
                    }
                }

                LinkButton cmdActivate = e.Row.FindControl("CmdActivate") as LinkButton;
                if (cmdActivate != null)
                {
                    cmdActivate.Visible = statusUpper != "ACTIVE";
                    cmdActivate.CssClass = "btn btn-success btn-xs";
                }
            }
            catch
            {
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "ACTIVATE", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                long id;
                if (!long.TryParse(Convert.ToString(e.CommandArgument), out id) || id <= 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> ID data tidak valid.</div>";
                    return;
                }

                int affectedRows;
                string errMsg;
                bool activated = ActivateData(id, out affectedRows, out errMsg);

                if (activated && affectedRows > 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Aktivasi data berhasil.</div>";
                    OpenGridview();
                }
                else
                {
                    string failReason = string.IsNullOrWhiteSpace(errMsg) ? "Data tidak berubah atau sudah ACTIVE." : errMsg;
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Aktivasi data gagal (" + Server.HtmlEncode(failReason) + ").</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Aktivasi data gagal (" + Server.HtmlEncode(ex.Message) + ").</div>";
            }
        }

        private bool ActivateData(long id, out int affectedRows, out string errMsg)
        {
            affectedRows = 0;
            errMsg = string.Empty;

            try
            {
                string strSql = $"sp_tbl_pc_register_verifikasi_active {id}";
                Recordset rec = new Recordset();
                rec.Open(strSql, Session["ClsTypeDBConnStringSQL"].ToString(), ref errMsg);

                if (!string.IsNullOrWhiteSpace(errMsg))
                {
                    return false;
                }

                if (rec.RecData != null && rec.RecData.Tables.Count > 0 && rec.RecData.Tables[0].Rows.Count > 0)
                {
                    string affectedValue = Convert.ToString(rec.RecData.Tables[0].Rows[0]["affected_rows"]);
                    int parsedAffected;
                    if (int.TryParse(affectedValue, out parsedAffected))
                    {
                        affectedRows = parsedAffected;
                    }
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        private string SanitizeParamValue(string input)
        {
            string value = input ?? string.Empty;
            value = value.Trim();
            return value.Replace("'", "''");
        }
    }
}
