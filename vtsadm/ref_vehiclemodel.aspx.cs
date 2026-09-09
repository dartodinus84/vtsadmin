using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using static System.Net.Mime.MediaTypeNames;

namespace vtsadm
{
    public partial class ref_vehiclemodel : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_typevehicle_model '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListTypeVehicleModelFieldSort"] = "VehicleModelID";
                ViewState["RecListTypeVehicleModelDirSort"] = "ASC";
                Session["RecListTypeVehicleModel"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListTypeVehicleModelFieldSort"].ToString(), ViewState["RecListTypeVehicleModelDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUREFMODEL"))
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
                ClType.Open_Combos(CmbVehicleBrandID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_brand");
                ClType.Open_Combos(CmbVehicleTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_type_ref");

                txtVehicleModelID.Text = "";
                txtModelVehicle.Text = "";

                CmbVehicleBrandID.SelectedValue = "[Select]";
                CmbVehicleTypeID.SelectedValue = "[Select]";

                CmbIsEv.SelectedValue = "0";
                txtEvPowerConsumption.Text = "";
                txtEvRange.Text = "";
                txtEvCapacity.Text = "";

                LblVehicleModelID.InnerHtml = "";
                txtVehicleModelIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";

                div_comment.InnerHtml = "";
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtModelVehicle.Text.Trim() != "")
                    {
                        strSQL = "sp_insert_typevehicle_model_new '" + txtModelVehicle.Text.Trim() + "','" + CmbVehicleBrandID.SelectedItem.Value.ToString() + "','" + CmbVehicleTypeID.SelectedItem.Value.ToString() + "','" + CmbIsEv.SelectedValue + "', '" + txtEvPowerConsumption.Text.Trim() + "', '" + txtEvRange.Text.Trim() + "','" + txtEvCapacity.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Model Vehicle has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Model Vehicle has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving Model Vehicle has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Model Vehicle Name</div>";
                    }
                }
                else
                {
                    if (txtVehicleModelID.Text.Trim() != "")
                    {
                        strSQL = "sp_update_typevehicle_model_new '" + txtVehicleModelID.Text.Trim() + "','" + txtModelVehicle.Text.Trim() + "','" + CmbVehicleBrandID.SelectedItem.Value.ToString() + "','" + CmbVehicleTypeID.SelectedItem.Value.ToString() + "','" + CmbIsEv.SelectedValue + "', '" + txtEvPowerConsumption.Text.Trim() + "', '" + txtEvRange.Text.Trim() + "', '" + txtEvCapacity.Text.Trim() + "', '"+ Session["ClsTypeUserID"].ToString() + "'";


                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Model Vehicle has been update successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Model Vehicle has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update Model Vehicle has been failed (" + sErr + ")</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update Model Vehicle has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                String strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0;
                string sVehicleModelID = "";
                String sModelVehicle = "";
                string sVehicleBrandID = "";
                string sVehicleTypeID = "";
                string sTypeVehicle = "";

                string sIsEv = "0";
                string sEvPowerConsumption = "0";
                string sEvRange = "0";
                string sEvCapacity = "0";

                string sStatus = "";
                string sErr = "";

                sVehicleModelID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sVehicleBrandID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sVehicleTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sModelVehicle = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();

                sIsEv = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sEvPowerConsumption = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sEvRange = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sEvCapacity = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();

                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();


                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            
                            txtVehicleModelID.Text = sVehicleModelID;
                            CmbVehicleBrandID.SelectedValue = sVehicleBrandID;
                            CmbVehicleTypeID.SelectedValue = sVehicleTypeID;
                            txtModelVehicle.Text = sModelVehicle;

                            if (sIsEv == "1" || sIsEv.ToUpper() == "YES" || sIsEv.ToUpper() == "TRUE")
                                CmbIsEv.SelectedValue = "1";
                            else
                                CmbIsEv.SelectedValue = "0";
                            txtEvPowerConsumption.Text = sEvPowerConsumption;
                            txtEvRange.Text = sEvRange;
                            txtEvCapacity.Text = sEvCapacity;

                            txtVehicleModelID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Model Vehicle can not be edited, due to status code has been " + sStatus + "</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTypeVehicleModel"], LblPaging, ViewState["RecListTypeVehicleModelFieldSort"].ToString(), ViewState["RecListTypeVehicleModelDirSort"].ToString());
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[10].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[12].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[10].Text.ToString() + "'); return false;";
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
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtVehicleModelIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_typevehicle_model '" + txtVehicleModelIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Model Vehicle has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Model Vehicle has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Model Vehicle has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Model Vehicle can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing Model Vehicle has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListTypeVehicleModel"], ViewState["RecListTypeVehicleModelFieldSort"].ToString(), ViewState["RecListTypeVehicleModelDirSort"].ToString(), e.SortExpression);
                ViewState["RecListTypeVehicleModelFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListTypeVehicleModelDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
        protected void CmbBrandID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbVehicleTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbVehicleBrandID.SelectedItem.Value.ToString(), "sp_get_typevehicle_type");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
    }
}