using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class setting_telegram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUOPTDOPRICE"))
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
                ClType.Open_Combos(CmbProvinceOrigin, Session["ClsTypeDBConnStringSQL"].ToString(), "ProvinceID", "sp_list_par_global");
                ClType.Open_Combos(CmbVehicleTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "JenisTruk", "sp_list_par_global");
                
                txtCustID.Value = "";
                txtCustName.Text = "";
                txtCustType.Text = "";
                txtCustBranch.Text = "";
                CmbProvinceOrigin.SelectedValue = "[Select]";
                CmbVehicleTypeID.SelectedValue = "[Select]";
                txtMinKm.Text = "";
                txtMaxKm.Text = "";
                txtPrice.Text = "";

                txtPriceIDDelete.Value = "";
                txtStatusDelete.Value = "";
                Button2.Attributes.Remove("disabled");
                CmdSubmit.Text = "Submit";
                //CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer_telegram '" + txtSearch.Value.Trim() + "'";
                ViewState["RecTelegramSort"] = "CustID";
                ViewState["RecTelegramDirSort"] = "DESC";
                Session["RecPricingDO"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecTelegramSort"].ToString(), ViewState["RecTelegramDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
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
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (CmbProvinceOrigin.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (CmbVehicleTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            if (txtMinKm.Text.Trim() != "" && txtMaxKm.Text.Trim() != "")
                            {
                                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                                {
                                    strSQL = "sp_submit_pricing_do '" + txtCustID.Value.Trim() + "','" + CmbProvinceOrigin.SelectedItem.Value.Trim() + "','" + CmbVehicleTypeID.SelectedItem.Value.Trim() + "'," + txtMinKm.Text.Trim() + "," + txtMaxKm.Text.Trim() + "," + txtPrice.Text.Trim() + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            Open_GridView();
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit delivery pricing has been successfully</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit delivery pricing has been failed (" + sErr + ")</div>";
                                        }
                                    }
                                }
                                else
                                {
                                    strSQL = "sp_update_pricing_do '" + txtCustID.Value.Trim() + "','" + CmbProvinceOrigin.SelectedItem.Value.Trim() + "','" + CmbVehicleTypeID.SelectedItem.Value.Trim() + "'," + txtMinKm.Text.Trim() + "," + txtMaxKm.Text.Trim() + "," + txtPrice.Text.Trim() + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        if (intAff > 0)
                                        {
                                            clear();
                                            Open_GridView();
                                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job training has been successfully</div>";
                                        }
                                        else
                                        {
                                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job training has been failed (" + sErr + ")</div>";
                                        }
                                    }

                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Min & Max km can not be null or 0</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vehicle type</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select province of origin</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select customer</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update delivery pricing are failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecPricingDO"], LblPagingHeader, ViewState["RecPricingDOFieldSort"].ToString(), ViewState["RecPricingDODirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 10; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[8].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";
                    for (int i = 10; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtPriceIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_pricing_do '" + txtPriceIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Delivery pricing has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing delivery pricing has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing delivery pricing has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delivery pricing can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing delivery pricing has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sPriceID = "";
                string sProvinceOriginID = ""; string sVehicleTypeID = ""; string sMinKm = ""; string sMaxKm = "";
                string sStatus = ""; string sPrice = "";

                sPriceID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sProvinceOriginID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sVehicleTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sMinKm = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sMaxKm = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sPrice = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            
                            txtCustID.Value = sCustID;
                            txtCustName.Text = sFullName;
                            txtCustType.Text = sCustType;
                            txtCustBranch.Text = sBranchName;
                            CmbProvinceOrigin.SelectedValue = sProvinceOriginID;
                            CmbVehicleTypeID.SelectedValue = sVehicleTypeID;
                            txtMinKm.Text = sMinKm;
                            txtMaxKm.Text = sMaxKm;
                            txtPrice.Text = sPrice;
                            Button2.Style.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delivery pricing can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delivery pricing can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecPricingDO"], ViewState["RecPricingDOFieldSort"].ToString(), ViewState["RecPricingDODirSort"].ToString(), e.SortExpression);
                ViewState["RecPricingDOFieldSort"] = e.SortExpression.ToString();
                ViewState["RecPricingDODirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}