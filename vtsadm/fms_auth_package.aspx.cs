using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_auth_package : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSAUTHPACK"))
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
                            div_comment.InnerHtml = "";
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
        protected void Open_Combos(DropDownList cmbTemp, string stDBConn, string sSearch, string strSQL)
        {
            try
            {
                Recordset Rec = new Recordset(); string stSQL = ""; ListItem LstItem;
                stSQL = strSQL + " '" + sSearch + "'";
                Rec.Open(stSQL, stDBConn);
                cmbTemp.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                cmbTemp.Items.Add(LstItem);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();
                        LstItem.Value = Rec.Fields(0).Trim();
                        cmbTemp.Items.Add(LstItem);
                        Rec.MoveNext();
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
        
        protected void clear()
        {
            // Load App dropdown
            LoadAppDropdown();
            CmbAppID.SelectedValue = "[Select]";
            
            // Clear Package dropdown - will be populated when App is selected
            CmbPackageID.Items.Clear();
            CmbPackageID.Items.Add(new ListItem("[Select]", "[Select]"));
            CmbPackageID.SelectedValue = "[Select]";

            txtSearchAvai.Text = "";
            txtSearchSel.Text = "";
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListAuthPackageAvailable"], LblPagingA);
            div_comment.InnerHtml = "";
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListAuthPackageSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sAppID, string sPackageID, string sSearch, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sAppID + "','" + sPackageID + "','" + sSearch + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }

        protected void CmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                // Only load if both App ID and Package ID are selected
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                {
                    Open_GridViews(GridView2, "sp_list_auth_package_available", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchAvai.Text.Trim(), "RecListAuthPackageAvailable", LblPagingA);
                    Open_GridViews(GridView1, "sp_list_auth_package_selected", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchSel.Text.Trim(), "RecListAuthPackageSelected", LblPagingS);
                    div_comment.InnerHtml = "";
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select App ID and Package ID first.</div>";
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                // Clear grids after clearing form
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();
                LblPagingA.Text = "";
                LblPagingS.Text = "";
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sMenuID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        sMenuID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sMenuID != "")
                        {
                            strSQL = "sp_auth_package_selected '" + CmbAppID.SelectedItem.Value.Trim() + "','" + CmbPackageID.SelectedItem.Value.Trim() + "','" + sMenuID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_auth_package_available", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchAvai.Text.Trim(), "RecListAuthPackageAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_auth_package_selected", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchSel.Text.Trim(), "RecListAuthPackageSelected", LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving package authentication has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving package authentication has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sMenuID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "REMOVE":
                        sMenuID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sMenuID != "")
                        {
                            strSQL = "sp_auth_package_customer_removed '" + CmbAppID.SelectedItem.Value.Trim() + "','" + CmbPackageID.SelectedItem.Value.Trim() + "','" + sMenuID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_auth_package_available", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchAvai.Text.Trim(), "RecListAuthPackageAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_auth_package_selected", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchSel.Text.Trim(), "RecListAuthPackageSelected", LblPagingS);
                                    div_comment.InnerHtml = "";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing package authentication has been failed (" + sErr + ")</div>";
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving package authentication has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                // Only search if both App ID and Package ID are selected
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                {
                    Open_GridViews(GridView2, "sp_list_auth_package_available", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchAvai.Text.Trim(), "RecListAuthPackageAvailable", LblPagingA);
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select App ID and Package ID first.</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchSel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                // Only search if both App ID and Package ID are selected
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                {
                    Open_GridViews(GridView1, "sp_list_auth_package_selected", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchSel.Text.Trim(), "RecListAuthPackageSelected", LblPagingS);
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Please select App ID and Package ID first.</div>";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmbAppID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]")
                {
                    // Load Package dropdown based on selected App ID
                    Open_Combos(CmbPackageID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbAppID.SelectedItem.Value.Trim(), "sp_list_auth_package_by_app");
                    CmbPackageID.SelectedValue = "[Select]";
                }
                else
                {
                    // Clear Package dropdown if no App is selected
                    CmbPackageID.Items.Clear();
                    CmbPackageID.Items.Add(new ListItem("[Select]", "[Select]"));
                    CmbPackageID.SelectedValue = "[Select]";
                }
                
                // Clear menu grids when App changes
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();
                LblPagingA.Text = "";
                LblPagingS.Text = "";
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Loading packages has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void CmbPackageID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Only load menu grids if both App ID and Package ID are selected
                if (CmbAppID.SelectedItem.Value.Trim() != "[Select]" && CmbPackageID.SelectedItem.Value.Trim() != "[Select]")
                {
                    Open_GridViews(GridView2, "sp_list_auth_package_available", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchAvai.Text.Trim(), "RecListAuthPackageAvailable", LblPagingA);
                    Open_GridViews(GridView1, "sp_list_auth_package_selected", CmbAppID.SelectedItem.Value.Trim(), CmbPackageID.SelectedItem.Value.Trim(), txtSearchSel.Text.Trim(), "RecListAuthPackageSelected", LblPagingS);
                }
                else
                {
                    // Clear grids if Package not selected
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    LblPagingA.Text = "";
                    LblPagingS.Text = "";
                }
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
    }
}