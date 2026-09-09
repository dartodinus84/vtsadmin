using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class technician : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_technician '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListTechnicianFieldSort"] = "TechnicianID";
                ViewState["RecListTechnicianDirSort"] = "ASC";
                Session["RecListTechnician"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListTechnicianFieldSort"].ToString(), ViewState["RecListTechnicianDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTTECH"))
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
                ClType.Open_Combos(CmbBranchID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_technician_branch");
                ClType.Open_Combos(CmbPicAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_pic_technician_area");

                txtTechnicianID.Text = "";
                txtEmployeeNo.Text = "";
                txtName.Text = "";
                CmbBranchID.SelectedValue = "[Select]";
                CmbPicAreaID.SelectedValue = "[Select]";
                txtAddress.Text = "";

                LblTechnicianID.InnerHtml = "";
                txtTechnicianIDDelete.Value = "";
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
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (txtName.Text.Trim() != "")
                    {
                        strSQL = "sp_insert_technician '" + txtEmployeeNo.Text.Trim() + "','" + txtName.Text.Trim() + "','" + CmbBranchID.SelectedItem.Value.ToString() + "','" + CmbPicAreaID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Technician has been save successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving technician has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving technician has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Employee Name</div>";
                    }
                }
                else
                {
                    if (txtTechnicianID.Text.Trim() != "")
                    {
                        strSQL = "sp_update_technician '" + txtTechnicianID.Text.Trim() + "','" + txtEmployeeNo.Text.Trim() + "','" + txtName.Text.Trim() + "','" + CmbBranchID.SelectedItem.Value.ToString() + "','" + CmbPicAreaID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Technician has been update successfully!</div>";
                                //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                                //UpPnl.Update();
                                //insert audit trails
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update technician has been failed</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update technician has been failed (" + sErr + ")</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update technician has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                string sTechnicianID = ""; string sEmployeeNo = ""; string sName = ""; string sBranchID = ""; string sBranchName = ""; string sPicAreaID = ""; string sPicAreaName = "";
                string sAddress = ""; string sStatus = ""; string sErr = "";
                sTechnicianID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sEmployeeNo = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sPicAreaName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[8].Text.Trim();
                sAddress = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sPicAreaID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG")
                        {
                            txtTechnicianID.Text = sTechnicianID;
                            txtEmployeeNo.Text = sEmployeeNo;
                            txtName.Text = sName;
                            CmbBranchID.SelectedValue = sBranchID;
                            CmbPicAreaID.SelectedValue = sPicAreaID;
                            txtAddress.Text = sAddress;
                            txtTechnicianID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Technician can not be edited, due to status code has been " + sStatus + "</div>";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTechnician"], LblPaging, ViewState["RecListTechnicianFieldSort"].ToString(), ViewState["RecListTechnicianDirSort"].ToString());
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

        protected void CmbBranchID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                setAddress(txtAddress, "sp_get_technician_address_branch '" + CmbBranchID.SelectedItem.Value.ToString() + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void setAddress(TextBox txtBox, string strSQL, string sDBConn)
        {
            try
            {
                Recordset Rec = new Recordset();
                txtBox.Text = "";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    txtBox.Text = Rec.Fields(0);
                }
            }
            catch (Exception)
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
                    for (int i = 8; i <= 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 8; i <= 9; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[6].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[7].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

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
                if (txtTechnicianIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_technician '" + txtTechnicianIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Technician has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing technician has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing technician has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Technician can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing technician has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListTechnician"], ViewState["RecListTechnicianFieldSort"].ToString(), ViewState["RecListTechnicianDirSort"].ToString(), e.SortExpression);
                ViewState["RecListTechnicianFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListTechnicianDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}