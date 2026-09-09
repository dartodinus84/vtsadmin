using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_package : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListFMSPackageFieldSort";
        string sViewStateDirSort = "RecListFMSPackageDirSort";
        string sSessionRecList = "RecListFMSPackage";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_package";
                ViewState[sViewStateFieldSort] = "PackageID";
                ViewState[sViewStateDirSort] = "ASC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Loading package list has been failed (" + ex.Message + ").");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSMASTERPACK"))
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
                else
                {
                    ApplyPackageIdFieldState();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Page load has been failed (" + ex.Message + ").");
            }
        }

        private void LoadAppDropdown()
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;

                Rec.Open("sp_list_app_setting", Session["ClsTypeDBConnStringSQL"].ToString());

                CmbAppID.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                CmbAppID.Items.Add(LstItem);

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();
                        LstItem.Value = Rec.Fields(0).Trim();
                        CmbAppID.Items.Add(LstItem);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                CmbAppID.Items.Clear();
                CmbAppID.Items.Add(new ListItem("[Select]", "[Select]"));
                ShowMessage("Failed", "Loading App ID has been failed (" + ex.Message + ").");
            }
        }

        private void LoadNextPackageID()
        {
            try
            {
                Recordset rec = new Recordset();
                string strSQL = "SELECT ISNULL(MAX(CAST(RIGHT(PackageID, 7) AS INT)), 0) + 1 AS NextNo FROM fms_mst_package WITH(NOLOCK) WHERE PackageID LIKE 'PAC%'";
                rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (rec.RecordCount() > 0)
                {
                    int nextNo = Convert.ToInt32(rec.Fields("NextNo"));
                    txtPackageID.Text = "PAC" + nextNo.ToString("0000000");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Generate Package ID has been failed (" + ex.Message + ").");
            }
        }

        private void SetPackageIdEditable(bool editable)
        {
            txtPackageID.ReadOnly = !editable;
            txtPackageID.BackColor = editable ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(244, 244, 244);
            txtPackageID.Attributes["placeholder"] = editable ? "Package ID ..." : (CmdSubmit.Text.ToUpper() == "UPDATE" ? "Package ID" : "Select App ID first ...");
        }

        private void ApplyPackageIdFieldState()
        {
            bool isNewInput = CmdSubmit.Text.ToUpper() == "SUBMIT";
            bool hasApp = CmbAppID.SelectedValue.Trim() != "[Select]";
            SetPackageIdEditable(isNewInput && hasApp);
        }

        private string Sanitize(string value)
        {
            return (value ?? string.Empty).Trim().Replace("'", "''");
        }

        private void ShowMessage(string title, string message)
        {
            string safeMessage = HttpUtility.HtmlEncode(message);
            string alertHtml = "<div class='alert alert-danger' role='alert' style='margin-bottom:10px;'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>" + HttpUtility.HtmlEncode(title) + "!</strong> " + safeMessage + "</div>";

            if (title.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                alertHtml = "<div class='alert alert-success' role='alert' style='margin-bottom:10px;'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> " + safeMessage + "</div>";
            }
            else if (title.Equals("Warning", StringComparison.OrdinalIgnoreCase))
            {
                alertHtml = "<div class='alert alert-warning' role='alert' style='margin-bottom:10px;'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> " + safeMessage + "</div>";
            }

            div_comment.InnerHtml = alertHtml;
        }

        private void ClearMessage()
        {
            div_comment.InnerHtml = string.Empty;
        }

        private void clear()
        {
            try
            {
                LoadAppDropdown();
                CmbAppID.SelectedValue = "[Select]";

                txtPackageID.Text = string.Empty;
                txtPackageName.Text = string.Empty;
                txtPackageDesc.Text = string.Empty;
                LblPackageIDDelete.InnerHtml = string.Empty;
                txtPackageIDDelete.Value = string.Empty;
                txtStatusDelete.Value = string.Empty;
                txtAppIDDelete.Value = string.Empty;
                CmdSubmit.Text = "Submit";
                ApplyPackageIdFieldState();
                ClearMessage();
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Clear form has been failed (" + ex.Message + ").");
            }
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Clear form has been failed (" + ex.Message + ").");
            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClearMessage();

                string appId = CmbAppID.SelectedValue.Trim();
                string packageId = txtPackageID.Text.Trim();
                string packageName = txtPackageName.Text.Trim();
                string packageDesc = txtPackageDesc.Text.Trim();
                string userId = Session["ClsTypeUserID"].ToString();
                bool isInsert = CmdSubmit.Text.ToUpper() == "SUBMIT";

                if (appId == "[Select]")
                {
                    ShowMessage("Failed", "Please select App ID first.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(packageId))
                {
                    ShowMessage("Failed", "Package ID is empty. Please select App ID to generate Package ID.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    ShowMessage("Failed", "Please fill in Package Name.");
                    return;
                }

                string strSQL = isInsert
                    ? "sp_insert_package '" + Sanitize(appId) + "','" + Sanitize(packageId) + "','" + Sanitize(packageName) + "','" + Sanitize(packageDesc) + "','" + Sanitize(userId) + "'"
                    : "sp_update_package '" + Sanitize(appId) + "','" + Sanitize(packageId) + "','" + Sanitize(packageName) + "','" + Sanitize(packageDesc) + "','" + Sanitize(userId) + "'";

                ExecCommand ec = new ExecCommand();
                int intAff = 0;
                string sErr = string.Empty;

                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        clear();
                        Open_GridView();
                        ShowMessage("Success", isInsert ? "Package has been saved successfully." : "Package has been updated successfully.");
                    }
                    else
                    {
                        ShowMessage("Failed", (isInsert ? "Saving" : "Updating") + " package has been failed. No data was changed.");
                    }
                }
                else
                {
                    string errDetail = string.IsNullOrWhiteSpace(sErr) ? "Unknown database error." : sErr;
                    ShowMessage("Failed", (isInsert ? "Saving" : "Updating") + " package has been failed (" + errDetail + ").");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Insert or update package has been failed (" + ex.Message + ").");
            }
        }

        protected void CmbAppID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearMessage();

                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]")
                {
                    if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                    {
                        LoadNextPackageID();
                    }
                }
                else
                {
                    txtPackageID.Text = string.Empty;
                }

                ApplyPackageIdFieldState();
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "App ID selection has been failed (" + ex.Message + ").");
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sPackageID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                string sPackageName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                string sPackageDesc = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                string sAppID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        LoadAppDropdown();
                        txtPackageID.Text = sPackageID;
                        txtPackageName.Text = sPackageName;
                        txtPackageDesc.Text = sPackageDesc;

                        if (CmbAppID.Items.FindByValue(sAppID) != null)
                        {
                            CmbAppID.SelectedValue = sAppID;
                        }

                        CmdSubmit.Text = "Update";
                        ApplyPackageIdFieldState();
                        ClearMessage();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Load package data for edit has been failed (" + ex.Message + ").");
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            ClearMessage();
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

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClearMessage();

                if (txtPackageIDDelete.Value.Trim() != "")
                {
                    string strSQL = "sp_delete_package '" + Sanitize(txtAppIDDelete.Value.Trim()) + "','" + Sanitize(txtPackageIDDelete.Value.Trim()) + "','" + Sanitize(Session["ClsTypeUserID"].ToString()) + "'";
                    ExecCommand ec = new ExecCommand();
                    int intAff = 0;
                    string sErr = string.Empty;

                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            ShowMessage("Success", "Package has been removed successfully.");
                        }
                        else
                        {
                            ShowMessage("Failed", "Removing package has been failed. No data was changed.");
                        }
                    }
                    else
                    {
                        string errDetail = string.IsNullOrWhiteSpace(sErr) ? "Unknown database error." : sErr;
                        ShowMessage("Failed", "Removing package has been failed (" + errDetail + ").");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Removing package has been failed (" + ex.Message + ").");
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 9; i <= 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[4].Text.ToString() + "'); return false;";

                    for (int i = 9; i <= 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ClearMessage();
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                ShowMessage("Failed", "Sorting data has been failed (" + ex.Message + ").");
            }
        }
    }
}
