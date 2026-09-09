using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_auth_customer : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListAuthCustomerFieldSort";
        string sViewStateDirSort = "RecListAuthCustomerDirSort";
        string sSessionRecList = "RecListAuthCustomer";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer_authentication '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "DtmUpd";
                ViewState[sViewStateDirSort] = "DESC";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSAUTHCUST"))
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
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                ClType.Open_Combos(CmbServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_fms_auth_server");
                CmbServerID.SelectedValue = "[Select]";

                // Load App dropdown
                LoadAppDropdown();
                CmbAppID.SelectedValue = "[Select]";

                // Clear Package dropdown - will be populated when App is selected
                CmbPackageID.Items.Clear();
                CmbPackageID.Items.Add(new ListItem("[Select]", "[Select]"));
                CmbPackageID.SelectedValue = "[Select]";

                txtPackageName.Text = "";

                LblCustIDDelete.InnerHtml = "";
                LblServerIDDelete.InnerHtml = "";
                txtCustIDDelete.Value = "";
                txtServerIDDelete.Value = "";
                txtStatusDelete.Value = "";
                txtAppIDDelete.Value = "";
                Session["ClsTypeFMSCustomerLogo"] = "";
                Session["ClsTypeFMSCustomerFavicon"] = "";
                Session["ClsTypeFMSCustomerBackground"] = "";
                Session["ClsTypeFMSCustomerBackgroundWelcome"] = "";
                CmdSubmit.Text = "Submit";
            }
            catch (Exception ex)
            {

            }
        }

        private string GetSessionText(string key)
        {
            return Session[key] == null ? "" : Session[key].ToString();
        }

        private void SafeSetSelectedValue(DropDownList cmb, string value)
        {
            string v = checkNbsp(value == null ? "" : value.Trim());
            if (cmb.Items.FindByValue(v) != null)
            {
                cmb.SelectedValue = v;
            }
            else if (cmb.Items.FindByValue("[Select]") != null)
            {
                cmb.SelectedValue = "[Select]";
            }
        }

        private string CleanGridText(string value)
        {
            string v = value == null ? "" : value.Trim();
            if (v == "" || v.ToUpper() == "&NBSP;" || v == "\u00A0")
            {
                return "";
            }
            return v.Replace("\u00A0", "").Trim();
        }

        private string EscapeJs(string value)
        {
            return (value ?? "").Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
        }

        private bool IsExecuteSuccess(bool executed, int affectedRows)
        {
            // OleDb + dynamic SQL (exec(@strsql)) often returns -1 even when update succeeds
            return executed && affectedRows != 0;
        }

        private void LoadAppDropdown()
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;
                
                // Execute stored procedure without parameters
                Rec.Open("sp_list_app_setting", Session["ClsTypeDBConnStringSQL"].ToString());
                
                // Clear and initialize dropdown
                CmbAppID.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                CmbAppID.Items.Add(LstItem);
                
                // Populate dropdown if data exists
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();  // Field index 1 = name
                        LstItem.Value = Rec.Fields(0).Trim();  // Field index 0 = app_id
                        CmbAppID.Items.Add(LstItem);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                // If error, at least ensure dropdown has [Select] option
                CmbAppID.Items.Clear();
                CmbAppID.Items.Add(new ListItem("[Select]", "[Select]"));
            }
        }

        private void LoadAppDropdownForEdit()
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;
                
                // Execute stored procedure without parameters
                Rec.Open("sp_list_app_setting", Session["ClsTypeDBConnStringSQL"].ToString());
                
                // Clear and initialize dropdown
                CmbAppID.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                CmbAppID.Items.Add(LstItem);
                
                // Populate dropdown if data exists (no debugging for edit mode)
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();  // Field index 1 = name
                        LstItem.Value = Rec.Fields(0).Trim();  // Field index 0 = app_id
                        CmbAppID.Items.Add(LstItem);
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                // If error in edit mode, just ensure dropdown has [Select] option (no debugging)
                CmbAppID.Items.Clear();
                CmbAppID.Items.Add(new ListItem("[Select]", "[Select]"));
            }
        }

        protected void CmdLoad_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_fms_auth_server");
                CmbServerID.SelectedValue = "[Select]";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        private string checkNbsp(string sIn)
        {
            string sOut = "";
            try
            {
                if (sIn.ToUpper().Trim() == "&NBSP;")
                {
                    sOut = "[Select]";
                }
                else
                {
                    sOut = sIn;
                }
            }
            catch (Exception ex)
            {

            }
            return sOut;
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sCustID = ""; string sFullName = ""; string sServerID = ""; string sServerName = "";
                string sPackageID = ""; string sPackageName = "";
                string sStatus = ""; string sAppID;
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sServerID = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sServerName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sPackageID = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sPackageName = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sAppID = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            txtCustID.Value = checkNbsp(sCustID);
                            txtCustFullName.Text = sFullName;
                            ClsType ClType = new ClsType();
                            ClType.Open_Combos(CmbServerID, Session["ClsTypeDBConnStringSQL"].ToString(), txtCustID.Value.Trim(), "sp_list_fms_auth_server");
                            SafeSetSelectedValue(CmbServerID, sServerID);

                            // Load App dropdown and select the correct App ID
                            LoadAppDropdownForEdit();
                            SafeSetSelectedValue(CmbAppID, sAppID);

                            // Load Package dropdown based on selected App ID
                            if (sAppID.Trim() != "" && sAppID.ToUpper().Trim() != "&NBSP;")
                            {
                                ClType.Open_Combos(CmbPackageID, Session["ClsTypeDBConnStringSQL"].ToString(), checkNbsp(sAppID).Trim(), "sp_list_auth_package_by_app");
                                SafeSetSelectedValue(CmbPackageID, sPackageID);
                                txtPackageName.Text = sPackageName;
                            }
                            else
                            {
                                // If no App ID, use original method
                                ClType.Open_Combos(CmbPackageID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_auth_package");
                                SafeSetSelectedValue(CmbPackageID, sPackageID);
                                txtPackageName.Text = sPackageName;
                            }

                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Authentication customer can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Load edit data has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
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

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtSearch.Text.Trim() == "")
                {
                    clear();
                }
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtCustID.Value.Trim() != "" && CmbServerID.SelectedItem.Value.Trim() != "[Select]" && CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        // SP expects: custid, serverid, appid, packageid, logo, favicon, background, backgroundwelcome, usrupd
                        strSQL = "sp_insert_auth_customer '" + txtCustID.Value.Trim() + "','" + CmbServerID.SelectedItem.Value.Trim() + "','" + CmbAppID.SelectedItem.Value.Trim() + "','" + CmbPackageID.SelectedItem.Value.Trim() + "','" + GetSessionText("ClsTypeFMSCustomerLogo") + "','" + GetSessionText("ClsTypeFMSCustomerFavicon") + "','" + GetSessionText("ClsTypeFMSCustomerBackground") + "','" + GetSessionText("ClsTypeFMSCustomerBackgroundWelcome") + "','" + GetSessionText("ClsTypeUserID") + "'";
                        bool okInsert = ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr);
                        if (IsExecuteSuccess(okInsert, intAff))
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication customer has been save successfully!</div>";
                        }
                        else if (okInsert)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving authentication customer has been failed (no rows affected)</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving authentication customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select customer id, app id and package id </div>";
                    }
                }
                else
                {
                    if (txtCustID.Value.Trim() != "" && CmbServerID.SelectedItem.Value.Trim() != "[Select]" && CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                    {
                        // SP expects: custid, serverid, appid, packageid, logo, favicon, background, backgroundwelcome, usrupd
                        strSQL = "sp_update_auth_customer '" + txtCustID.Value.Trim() + "','" + CmbServerID.SelectedItem.Value.Trim() + "','" + CmbAppID.SelectedItem.Value.Trim() + "','" + CmbPackageID.SelectedItem.Value.Trim() + "','" + GetSessionText("ClsTypeFMSCustomerLogo") + "','" + GetSessionText("ClsTypeFMSCustomerFavicon") + "','" + GetSessionText("ClsTypeFMSCustomerBackground") + "','" + GetSessionText("ClsTypeFMSCustomerBackgroundWelcome") + "','" + GetSessionText("ClsTypeUserID") + "'";
                        bool okUpdate = ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr);
                        if (IsExecuteSuccess(okUpdate, intAff))
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication customer has been update successfully!</div>";
                        }
                        else if (okUpdate)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update authentication customer has been failed (no rows affected)</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update authentication customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select customer id, app id and package id </div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update authentication customer has been failed (" + ex.Message + ")</div>";
            }
        }

        //protected void CmbUserID_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        div_comment.InnerHtml = "";
        //        if (CmbUserID.SelectedItem.Value.Trim() != "[Select]")
        //        {
        //            Recordset Rec = new Recordset(); string strSQL = "";
        //            strSQL = "sp_get_attr_userid '" + CmbUserID.SelectedItem.Value.Trim() + "'";
        //            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim());
        //            if (Rec.RecordCount() > 0)
        //            {
        //                txtFullName.Text = Rec.Fields("FullName").Trim();
        //                txtEmail.Text = Rec.Fields("FullName").Trim();
        //                txtUserType.Text = Rec.Fields("UserTypeDesc").Trim();
        //            }
        //            else
        //            {
        //                txtFullName.Text = "";
        //                txtEmail.Text = "";
        //                txtUserType.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            txtFullName.Text = "";
        //            txtEmail.Text = "";
        //            txtUserType.Text = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        protected void CmbAppID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClType = new ClsType();
                
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]")
                {
                    // Load Package dropdown based on selected App ID
                    ClType.Open_Combos(CmbPackageID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbAppID.SelectedItem.Value.Trim(), "sp_list_auth_package_by_app");
                    CmbPackageID.SelectedValue = "[Select]";
                }
                else
                {
                    // Clear Package dropdown if no App is selected
                    CmbPackageID.Items.Clear();
                    CmbPackageID.Items.Add(new ListItem("[Select]", "[Select]"));
                    CmbPackageID.SelectedValue = "[Select]";
                }
                
                // Clear Package Name when App changes
                txtPackageName.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbPackageID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                {
                    txtPackageName.Text = CmbPackageID.SelectedItem.Text.Trim();
                }
                else
                {
                    txtPackageName.Text = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sErr = "";

                if (txtCustIDDelete.Value.Trim() != "" && txtServerIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        string sCustID = CleanGridText(txtCustIDDelete.Value);
                        string sServerID = CleanGridText(txtServerIDDelete.Value);
                        string sAppID = CleanGridText(txtAppIDDelete.Value);
                        if (sAppID == "")
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication customer has been failed (App ID is empty)</div>";
                            return;
                        }
                        strSQL = "sp_delete_auth_customer '" + sCustID + "','" + sServerID + "','" + sAppID + "','" + GetSessionText("ClsTypeUserID") + "'";
                        bool okDelete = ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr);
                        if (IsExecuteSuccess(okDelete, intAff))
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Authentication customer has been remove successfully!</div>";
                        }
                        else if (okDelete)
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication customer has been failed (no rows affected)</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Authentication customer can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing authentication customer has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {                
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 14; i <= 18; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[11].FindControl("CmdDelete");
                    string sCustID = EscapeJs(CleanGridText(e.Row.Cells[0].Text));
                    string sServerID = EscapeJs(CleanGridText(e.Row.Cells[2].Text));
                    string sStatus = EscapeJs(CleanGridText(e.Row.Cells[7].Text));
                    string sAppID = EscapeJs(CleanGridText(e.Row.Cells[18].Text));
                    CmdButton.OnClientClick = "confirmDelete('" + sCustID + "','" + sServerID + "','" + sStatus + "','" + sAppID + "'); return false;";

                    LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    CmdPic.OnClientClick = "postPic('" + EscapeJs(CleanGridText(e.Row.Cells[15].Text)) + "'); return false;";

                    LinkButton CmdFav = (LinkButton)e.Row.FindControl("CmdFavicon");
                    CmdFav.OnClientClick = "postFavicon('" + EscapeJs(CleanGridText(e.Row.Cells[16].Text)) + "'); return false;";
                    
                    LinkButton CmdBackground = (LinkButton)e.Row.FindControl("CmdBackground");
                    CmdBackground.OnClientClick = "postBackground('" + EscapeJs(CleanGridText(e.Row.Cells[17].Text)) + "'); return false;";


                    for (int i = 14; i <= 18; i++)
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