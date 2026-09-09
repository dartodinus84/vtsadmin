using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_vehicle '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListVehicleFieldSort"] = "VehicleID";
                ViewState["RecListVehicleDirSort"] = "DESC";
                Session["RecListVehicle"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTVEH"))
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
        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbVehicleType, Session["ClsTypeDBConnStringSQL"].ToString(), "JenisTruk", "sp_list_par_global");
                ClType.Open_Combos(CmbContainerSize, Session["ClsTypeDBConnStringSQL"].ToString(), "ContainerSize", "sp_list_par_global");
                ClType.Open_Combos(CmbBrandID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_brand");
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_type");
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_typevehicle_model");
                ClType.Open_Combos(CmbIconVehicle, Session["ClsTypeDBConnStringSQL"].ToString(), "VehicleIcon", "sp_list_par_global");


                txtVehID.Text = "";
                CmbBrandID.SelectedValue = "[None]";
                CmbModelID.SelectedValue = "[None]";
                CmbTypeID.SelectedValue = "[None]";
                txtVehDesc.Text = "";
                txtPoliceNo.Text = "";
                txtAssetNo.Text = "";
                CmbVehicleType.SelectedValue = "[None]";
                CmbContainerSize.SelectedValue = "[None]";
                CmbIconVehicle.SelectedValue = "[None]";
                txtEngineNumber.Text = "";
                txtVin.Text = "";

                LblVehicleID.InnerHtml = "";
                txtVehicleIDDelete.Value = "";
                txtStatusDelete.Value = "";

                CmdSubmit.Text = "Submit";
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
                div_comment.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtPoliceNo.Text.Trim() != "")
                    {
                        if (CmbVehicleType.SelectedItem.Value.Trim() != "[Select]" && CmbContainerSize.SelectedItem.Value.Trim() != "[Select]" && CmbBrandID.SelectedItem.Value.Trim() != "[Select]" && CmbTypeID.SelectedItem.Value.Trim() != "[Select]" && CmbModelID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            strSQL = "sp_insert_vehicle '" + CmbBrandID.SelectedItem.Value.Trim() + "','" + CmbModelID.SelectedItem.Value.Trim() + "','" + CmbTypeID.SelectedItem.Value.Trim() + "','" + txtVehDesc.Text.Trim() + "','" + txtPoliceNo.Text.Trim() + "','" + txtAssetNo.Text.Trim() + "','" + txtVin.Text.Trim() + "','" + txtEngineNumber.Text.Trim() + "','','" + Session["ClsTypeUserID"].ToString() + "','" + CmbVehicleType.SelectedItem.Value.Trim() + "','" + CmbContainerSize.SelectedItem.Value.Trim() + "','" + CmbIconVehicle.SelectedItem.Value.Trim() + "'";

                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    txtVehDesc.Text = "";
                                    txtPoliceNo.Text = "";
                                    txtAssetNo.Text = "";
                                    txtVin.Text = "";
                                    txtEngineNumber.Text = "";

                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle has been save successfully!</div>";
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle has been failed</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vehicle type and container size</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Police No</div>";
                    }
                }
                else
                {
                    if (txtVehID.Text.Trim() != "")
                    {
                        if (CmbVehicleType.SelectedItem.Value.Trim() != "[Select]" && CmbContainerSize.SelectedItem.Value.Trim() != "[Select]" && CmbBrandID.SelectedItem.Value.Trim() != "[Select]" && CmbTypeID.SelectedItem.Value.Trim() != "[Select]" && CmbModelID.SelectedItem.Value.Trim() != "[Select]")
                        {

                            strSQL = "sp_update_vehicle '" + txtVehID.Text.Trim() + "','" + CmbBrandID.SelectedItem.Value.Trim() + "','" + CmbModelID.SelectedItem.Value.Trim() + "','" + CmbTypeID.SelectedItem.Value.Trim() + "','" + txtVehDesc.Text.Trim() + "','" + txtPoliceNo.Text.Trim() + "','" + txtAssetNo.Text.Trim() + "','" + txtVin.Text.Trim() + "','" + txtEngineNumber.Text.Trim() + "','','" + Session["ClsTypeUserID"].ToString() + "','" + CmbVehicleType.SelectedItem.Value.Trim() + "','" + CmbContainerSize.SelectedItem.Value.Trim() + "','" + CmbIconVehicle.SelectedItem.Value.Trim() + "'";

                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    txtVehDesc.Text = "";
                                    txtPoliceNo.Text = "";
                                    txtAssetNo.Text = "";
                                    txtVin.Text = "";
                                    txtEngineNumber.Text = "";
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle has been update successfully!</div>";
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update vehicle has been failed</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update vehicle has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select vehicle type and container size</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update vehicle has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sVehID = ""; string sBrand = ""; string sModel = ""; string sType = ""; string sVehDesc = "";
                string sPoliceNo = ""; string sAssetNo = ""; string sStatus = ""; string sVehicleTypeID = ""; string sContainerSizeID = "";
                string sBrandID = ""; string sModelID = ""; string sTypeID = ""; string sVehDescs = ""; string sIconVehicle = ""; string sVin = ""; string sEngineNumber = "";

                sVehID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sVehDesc = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();

                sBrand = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sModel = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sType = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();

                sPoliceNo = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sAssetNo = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();

                sVin = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sEngineNumber = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();

                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();

                sVehicleTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sContainerSizeID = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sBrandID = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sModelID = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sIconVehicle = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE" && sStatus.ToUpper().Trim() != "IS")
                        {
                            ClsType ClType = new ClsType();
                            txtVehID.Text = sVehID;
                            txtVehDesc.Text = sVehDesc;
                            txtPoliceNo.Text = sPoliceNo;
                            txtAssetNo.Text = sAssetNo;
                            txtVin.Text = sVin;
                            txtEngineNumber.Text = sEngineNumber;

                            if (sVehicleTypeID == "" || sVehicleTypeID == "&nbsp;") { sVehicleTypeID = "[Select]"; }
                            CmbVehicleType.SelectedValue = sVehicleTypeID;

                            if (sContainerSizeID == "" || sContainerSizeID == "&nbsp;") { sContainerSizeID = "[Select]"; }
                            CmbContainerSize.SelectedValue = sContainerSizeID;

                            if (sIconVehicle == "" || sIconVehicle == "&nbsp;" || sIconVehicle == "0") { sIconVehicle = "[Select]"; }
                            CmbIconVehicle.SelectedValue = sIconVehicle;

                            CmbBrandID.SelectedValue = sBrandID;

                            ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBrandID.SelectedItem.Value.ToString(), "sp_get_typevehicle_type");
                            CmbTypeID.SelectedValue = sTypeID;

                            ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value.ToString(), "sp_get_typevehicle_model");
                            CmbModelID.SelectedValue = sModelID;

                            CmdSubmit.Text = "Update";
                            txtVehID.Attributes.Add("disabled", "disabled");
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Vehicle can not be edited, due to status code has been " + sStatus + "</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVehicle"], LblPaging, ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString());
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
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 15; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }


                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 15; i <= 20; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    e.Row.Cells[13].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[15].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[12].Text.ToString() + "'); return false;";
                }
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

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtVehicleIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_vehicle '" + txtVehicleIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Vehicle can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListVehicle"], ViewState["RecListVehicleFieldSort"].ToString(), ViewState["RecListVehicleDirSort"].ToString(), e.SortExpression);
                ViewState["RecListVehicleFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListVehicleDirSort"] = sNewDirSort;
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
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBrandID.SelectedItem.Value.ToString(), "sp_get_typevehicle_type");
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value.ToString(), "sp_get_typevehicle_model");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        protected void CmbTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbModelID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbTypeID.SelectedItem.Value, "sp_get_typevehicle_model");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
    }
}