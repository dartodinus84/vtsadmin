using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class expedition_service : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                string expedisiId = CmbExpedisi.SelectedValue ?? "";
                if (string.IsNullOrEmpty(expedisiId) || expedisiId == "[Select]") return;
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_expedisi_service '" + expedisiId.Replace("'", "''") + "'";
                ViewState["RecListExpedisiServiceFieldSort"] = "ExpedisiCode";
                ViewState["RecListExpedisiServiceDirSort"] = "ASC";
                Session["RecListExpedisiService"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListExpedisiServiceFieldSort"].ToString(), ViewState["RecListExpedisiServiceDirSort"].ToString());
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ClsTypeAccessMenu"] == null || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTXPDSVC"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                ClsType ClType = new ClsType();
                if (!IsPostBack && Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                {
                    ClType.Open_Combos(CmbExpedisi, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_expedisi");
                    PanelService.Visible = false;
                }
                else if (!IsPostBack)
                    Response.Redirect("login.aspx");
            }
            catch { }
        }

        protected void CmbExpedisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string expedisiId = CmbExpedisi.SelectedValue ?? "";
            if (!string.IsNullOrEmpty(expedisiId) && expedisiId != "[Select]")
            {
                PanelService.Visible = true;
                txtExpedisiID.Text = expedisiId;
                clear();
                Open_GridView();
            }
            else
                PanelService.Visible = false;
        }

        private static string CleanCellText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("\u00A0", "").Replace("&nbsp;", "").Trim();
        }

        private void clear()
        {
            txtExpedisiCode.Text = "";
            txtExpedisiName.Text = "";
            txtSendingDaysMin.Text = "";
            txtSendingDaysMax.Text = "";
            txtExpedisiCode.Enabled = true;
            txtExpedisiCode.Attributes.Remove("disabled");
            LblServiceCode.InnerHtml = "";
            txtExpedisiIDDelete.Value = "";
            txtExpedisiCodeDelete.Value = "";
            txtStatusDelete.Value = "";
            CmdSubmit.Text = "Submit";
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string expedisiId = CmbExpedisi.SelectedValue ?? "";
                if (string.IsNullOrEmpty(expedisiId) || expedisiId == "[Select]")
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'>Please select an Expedition first.</div>";
                    return;
                }
                string usr = Session["ClsTypeUserID"] != null ? Session["ClsTypeUserID"].ToString().Trim().Replace("'", "''") : "";
                string code = (txtExpedisiCode.Text ?? "").Trim().Replace("'", "''");
                string name = (txtExpedisiName.Text ?? "").Trim().Replace("'", "''");
                int daysMin = 0, daysMax = 0;
                int.TryParse(txtSendingDaysMin.Text ?? "", out daysMin);
                int.TryParse(txtSendingDaysMax.Text ?? "", out daysMax);
                int intAff = 0;
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                string strSQL;
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (string.IsNullOrEmpty(code)) { div_comment.InnerHtml = "<div class='alert alert-danger'>Please fill Service Code.</div>"; return; }
                    if (string.IsNullOrEmpty(name)) { div_comment.InnerHtml = "<div class='alert alert-danger'>Please fill Service Name.</div>"; return; }
                    strSQL = "sp_insert_expedisi_service '" + expedisiId + "','" + code + "','" + name + "'," + daysMin + "," + daysMax + ",'" + usr + "'";
                }
                else
                {
                    string oldCode = txtExpedisiCode.Text.Trim().Replace("'", "''");
                    if (string.IsNullOrEmpty(oldCode)) { div_comment.InnerHtml = "<div class='alert alert-danger'>Service Code required for update.</div>"; return; }
                    strSQL = "sp_update_expedisi_service '" + expedisiId + "','" + oldCode + "','" + name + "'," + daysMin + "," + daysMax + ",'" + usr + "'";
                }
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    clear();
                    Open_GridView();
                    div_comment.InnerHtml = "<div class='alert alert-success'>Expedition service saved successfully!</div>";
                }
                else
                    div_comment.InnerHtml = "<div class='alert alert-danger'>" + (string.IsNullOrEmpty(sErr) ? "Operation failed." : sErr) + "</div>";
            }
            catch (Exception ex) { div_comment.InnerHtml = "<div class='alert alert-danger'>" + ex.Message + "</div>"; }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = (e.CommandSource as GridView).Rows[iRow];
                string sExpedisiID = CleanCellText(row.Cells[0].Text);
                string sCode = CleanCellText(row.Cells[1].Text);
                string sName = CleanCellText(row.Cells[2].Text);
                string sDaysMin = CleanCellText(row.Cells[3].Text);
                string sDaysMax = CleanCellText(row.Cells[4].Text);
                string sStatus = CleanCellText(row.Cells[5].Text);
                if (e.CommandName.ToUpper() == "CHANGES" && sStatus.ToUpper().Trim() == "RG")
                {
                    txtExpedisiID.Text = sExpedisiID;
                    txtExpedisiCode.Text = sCode;
                    txtExpedisiName.Text = sName;
                    txtSendingDaysMin.Text = sDaysMin;
                    txtSendingDaysMax.Text = sDaysMax;
                    txtExpedisiCode.Enabled = false;
                    txtExpedisiCode.Attributes.Add("disabled", "disabled");
                    CmdSubmit.Text = "Update";
                    div_comment.InnerHtml = "";
                }
            }
            catch { }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListExpedisiService"], LblPaging, ViewState["RecListExpedisiServiceFieldSort"].ToString(), ViewState["RecListExpedisiServiceDirSort"].ToString());
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[6].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    if (CmdButton != null)
                    {
                        string sExpId = e.Row.Cells[0].Text.Replace("'", "\\'").Replace("\u00A0", "").Replace("&nbsp;", "");
                        string sCode = e.Row.Cells[1].Text.Replace("'", "\\'").Replace("\u00A0", "").Replace("&nbsp;", "");
                        string sStatus = e.Row.Cells[5].Text.Replace("'", "\\'").Replace("\u00A0", "").Replace("&nbsp;", "");
                        CmdButton.OnClientClick = "confirmDelete('" + sCode + "','" + sExpId + "','" + sCode + "','" + sStatus + "'); return false;";
                    }
                }
            }
            catch { }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtExpedisiIDDelete.Value) || string.IsNullOrEmpty(txtExpedisiCodeDelete.Value)) return;
                if (txtStatusDelete.Value.ToUpper().Trim() != "RG")
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger'>Cannot remove (Status: " + txtStatusDelete.Value + ")</div>";
                    return;
                }
                ExecCommand Ec = new ExecCommand();
                int intAff = 0;
                string sErr = "";
                string id = txtExpedisiIDDelete.Value.Trim().Replace("'", "''");
                string code = txtExpedisiCodeDelete.Value.Trim().Replace("'", "''");
                string usr = Session["ClsTypeUserID"] != null ? Session["ClsTypeUserID"].ToString().Trim().Replace("'", "''") : "";
                if (Ec.Execute("sp_delete_expedisi_service '" + id + "','" + code + "','" + usr + "'", Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                {
                    clear();
                    Open_GridView();
                    div_comment.InnerHtml = "<div class='alert alert-success'>Expedition service removed successfully!</div>";
                }
                else
                    div_comment.InnerHtml = "<div class='alert alert-danger'>" + (string.IsNullOrEmpty(sErr) ? "Delete failed." : sErr) + "</div>";
            }
            catch (Exception ex) { div_comment.InnerHtml = "<div class='alert alert-danger'>" + ex.Message + "</div>"; }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                string sNewDirSort = ClType.Gv_Sorting(GridView1, Session["RecListExpedisiService"], ViewState["RecListExpedisiServiceFieldSort"].ToString(), ViewState["RecListExpedisiServiceDirSort"].ToString(), e.SortExpression);
                ViewState["RecListExpedisiServiceFieldSort"] = e.SortExpression;
                ViewState["RecListExpedisiServiceDirSort"] = sNewDirSort;
            }
            catch { }
        }
    }
}
