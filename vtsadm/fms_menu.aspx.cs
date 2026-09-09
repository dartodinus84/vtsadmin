using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_menu : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListFMSMenuFieldSort";
        string sViewStateDirSort = "RecListFMSMenuDirSort";
        string sSessionRecList = "RecListFMSMenu";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_fms_menu '" + txtSearch.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "MenuID";
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSMASTERMENU"))
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
        private void LoadAppDropdown()
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;
                
                // Execute stored procedure to get app list
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
                        LstItem.Text = Rec.Fields(1).Trim();  // Field index 1 = app name
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

        private void LoadParentDropdown(string appID)
        {
            try
            {
                Recordset Rec = new Recordset();
                ListItem LstItem;
                
                // Clear and initialize dropdown
                CmbParentID.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                CmbParentID.Items.Add(LstItem);
                
                // Jika App ID valid, load parent menu berdasarkan App ID
                if (appID != null && appID.Trim() != "" && appID.Trim() != "[Select]")
                {
                    // Execute stored procedure dengan parameter appID
                    Rec.Open("sp_list_fms_parent '" + appID + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                    
                    // Populate dropdown if data exists
                    if (Rec.RecordCount() > 0)
                    {
                        Rec.MoveFirst();
                        while (!Rec.EOF)
                        {
                            LstItem = new ListItem();
                            LstItem.Text = Rec.Fields(1).Trim();
                            LstItem.Value = Rec.Fields(0).Trim();
                            CmbParentID.Items.Add(LstItem);
                            Rec.MoveNext();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If error, at least ensure dropdown has [Select] option
                CmbParentID.Items.Clear();
                CmbParentID.Items.Add(new ListItem("[Select]", "[Select]"));
            }
        }
        
        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                
                // Load App dropdown
                LoadAppDropdown();
                CmbAppID.SelectedValue = "[Select]";
                
                txtMenuID.Text = "";
                txtMenuName.Text = "";
                TextMenuDesc.Text = "";
                txtMenuUrl.Text = "";
                txtMenuIcon.Text = "";
                ClType.Open_Combos(CmbIsParent, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_parent_type");
                LoadParentDropdown("");
                CmbIsParent.SelectedValue = "[Select]";
                CmbParentID.SelectedValue = "[Select]";
                txtMenuPos.Text = "";
                txtMenuID.Attributes.Remove("disabled");
                LblMenuIDDelete.InnerHtml = "";
                txtAppIDDelete.Value = "";
                txtMenuIDDelete.Value = "";
                txtStatusDelete.Value = "";
                CmdSubmit.Text = "Submit";
                Session["ClsTypeMenuTMSImage"] = "";
                Session["ClsTypeMenuTMSThumbnail"] = "";
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

        protected void CmbAppID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]")
                {
                    // App ID selected - bisa lanjut ke input Menu
                    txtMenuID.Attributes.Remove("disabled");
                    
                    // Load Parent ID dropdown berdasarkan App ID
                    LoadParentDropdown(CmbAppID.SelectedItem.Value.Trim());
                }
                else
                {
                    // No App selected - disable Menu ID input
                    txtMenuID.Attributes.Add("disabled", "disabled");
                    
                    // Reset Parent ID dropdown
                    LoadParentDropdown("");
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> App ID selection has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sMenuID = ""; string sAppID = ""; string sMenuName = ""; string sMenuUrl = "";
                string sMenuIcon = ""; string sIsParent = ""; string sParentID = ""; string sMenuPos = "";
                string sStatus = ""; string sMenuDesc = "";
                sAppID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sMenuID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sMenuName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sMenuUrl = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sMenuIcon = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sIsParent = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sParentID = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sMenuPos = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sMenuDesc = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            // Load App dropdown first
                            LoadAppDropdown();
                            
                            // Set App ID
                            CmbAppID.SelectedValue = sAppID;
                            
                            // Load Parent ID dropdown berdasarkan App ID
                            LoadParentDropdown(sAppID);
                            
                            txtMenuID.Text = sMenuID;
                            txtMenuName.Text = sMenuName;
                            TextMenuDesc.Text = sMenuDesc;
                            txtMenuUrl.Text = sMenuUrl;
                            txtMenuIcon.Text = sMenuIcon;
                            CmbIsParent.SelectedValue = checkNbsp(sIsParent);
                            CmbParentID.SelectedValue = checkNbsp(sParentID);
                            txtMenuPos.Text = sMenuPos;
                            txtMenuID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Menu can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

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
                    if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && txtMenuID.Text.Trim() != "")
                    {
                        if (CmbIsParent.SelectedItem.Value.Trim() != "[Select]")
                        {
                            strSQL = "sp_insert_fms_menu '" + CmbAppID.SelectedItem.Value.Trim() + "','" + txtMenuID.Text.Trim() + "','" + txtMenuName.Text.Trim() + "','" + TextMenuDesc.Text.Trim() + "','" + txtMenuUrl.Text.Trim() + "','" + Session["ClsTypeMenuTMSImage"] + "','" + Session["ClsTypeMenuTMSThumbnail"] + "','" + txtMenuIcon.Text.Trim() + "','" + CmbIsParent.SelectedItem.Value.Trim() + "','" + CmbParentID.SelectedItem.Value.Trim() + "','" + txtMenuPos.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Menu has been save successfully!</div>";
                                    //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                    //UpPnl.Update();
                                    //insert audit trails
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving menu has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving menu has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select isParent</div>";
                        }
                    }
                    else
                    {
                        if (CmbAppID.SelectedItem.Value.Trim() == "[Select]")
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select App ID first</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in Menu ID</div>";
                        }
                    }
                }
                else
                {
                    if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && txtMenuID.Text.Trim() != "")
                    {
                        if (CmbIsParent.SelectedItem.Value.Trim() != "[Select]")
                        {
                            strSQL = "sp_update_fms_menu '" + CmbAppID.SelectedItem.Value.Trim() + "','" + txtMenuID.Text.Trim() + "','" + txtMenuName.Text.Trim() + "','" + TextMenuDesc.Text.Trim() + "','" + txtMenuUrl.Text.Trim() + "','" + Session["ClsTypeMenuTMSImage"] + "','" + Session["ClsTypeMenuTMSThumbnail"] + "','" + txtMenuIcon.Text.Trim() + "','" + CmbIsParent.SelectedItem.Value.Trim() + "','" + CmbParentID.SelectedItem.Value.Trim() + "','" + txtMenuPos.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Menu has been update successfully!</div>";
                                    //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                    //UpPnl.Update();
                                    //insert audit trails
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update menu has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update menu has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select isParent</div>";
                        }
                    }
                    else
                    {
                        if (CmbAppID.SelectedItem.Value.Trim() == "[Select]")
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select App ID first</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill in Menu ID</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update menu has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sErr = "";
                if (txtMenuIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_fms_menu '" + txtAppIDDelete.Value.Trim() + "','" + txtMenuIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Menu has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing menu has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing menu has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Menu can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing menu has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[16].Visible = false;
                    e.Row.Cells[17].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[12].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[8].Text.ToString() + "'); return false;";

                    LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    CmdPic.OnClientClick = "postPic('" + e.Row.Cells[16].Text.ToString() + "'); return false;";

                    LinkButton CmdPic2 = (LinkButton)e.Row.FindControl("CmdPic2");
                    CmdPic2.OnClientClick = "postPic2('" + e.Row.Cells[17].Text.ToString() + "'); return false;";

                    e.Row.Cells[16].Visible = false;
                    e.Row.Cells[17].Visible = false;
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