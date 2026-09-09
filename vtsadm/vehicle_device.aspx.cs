using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vehicle_device : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "sp_list_vehicle_device";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                GridView1.DataSource = Rec.DataRecord();
                GridView1.DataBind();
                Session["RecListVehicleDevice"] = Rec.RecData;
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLVEHDEV"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                //CType(Page.Master, ASP.masterpage_masterpage_master).RegisterPostBackTrigger(CmdPreview) 
                SiteMaster sMaster = this.Master as SiteMaster;
                //sMaster.RegisterPostBackTrigger(CmdSubmit);
                //sMaster.RegisterPostBackTrigger(GridView2);


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
                txtVehicleID.Value = "";
                txtVehicleDesc.Text = "";
                txtPoliceNo.Text = "";
                txtAssetNo.Text = "";
                txtTvaID.Text = "";
                txtCustBranchName.Text = "";
                txtCustomerName.Text = "";
                txtUplineName.Text = "";
                txtMasterName.Text = "";
                txtDeviceID.Value = "";
                txtNoSN.Text = "";
                txtVendorName.Text = "";
                txtDeviceTypeDesc.Text = "";
                txtWarehouseName.Text = "";
                //txtTdsID.Text = "";
                txtGsmID.Value = "";
                txtMSIDN.Text = "";
                txtProviderName.Text = "";
                txtTechnicianID.Value = "";
                txtEmployeeNo.Text = "";
                txtName.Text = "";
                txtTechBranchName.Text = "";
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
            }
            catch (Exception ex)
            {

            }
        }
        //sampe sini....
        protected void CmdSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    //strSQL = "sp_insert_vehicle_device '" + txtTvaID.Text.Trim() + "','" + txtTdsID.Text.Trim() + "','" + txtVehicleID.Value.Trim() + "','" + txtDeviceID.Value.Trim() + "','" + txtGsmIaD.Value.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle device installation has been save successfully!</div>";
                            //UpdatePanel UpPnl = this.Master.FindControl("UpdatePanel2") as UpdatePanel;
                            //UpPnl.Update();
                            //insert audit trails
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving vehicle device installation has been failed</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                String strSQL = ""; ExecCommand ec = new ExecCommand(); string sErr = "";
                Int32 intAff = 0; String sTvdID = ""; string sStatus = ""; string sTvaID = ""; string sTdsID = "";
                string sVehicleID = ""; string sDeviceID = ""; string sGsmID = "";
                sTvdID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sVehicleID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sTvaID = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sDeviceID = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();
                sTdsID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();                
                sGsmID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "DELETE":
                        if (sTvdID != "")
                        {
                            strSQL = "sp_delete_vehicle_device '" + sTvdID + "','" + sTvaID + "','" + sTdsID + "','" + sVehicleID + "','" + sDeviceID + "','" + sGsmID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    Open_GridView();
                                    div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Vehicle device installation has been remove successfully!</div>";
                                }
                                else
                                {
                                    //txtError.Value = "Delete failed";
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing vehicle device insttallation has been failed!!</div>";
                                }
                            }
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
            (sender as GridView).DataSource = Session["RecListVehicleDevice"];
            (sender as GridView).PageIndex = e.NewPageIndex;
            (sender as GridView).DataBind();
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
    }
}