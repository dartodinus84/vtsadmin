using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class vendor_special : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTVENSPECIAL"))
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
        protected void clear()
        {
            txtVendorID.Value = "";
            txtVendorName.Text = "";
            txtAdd.Text = "";
            txtLng.Text = "";
            txtLat.Text = "";
            txtSearchAvai.Text = "";
            txtSearchSel.Text = "";
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVendorSpecialAvailable"], LblPagingA);
            div_comment.InnerHtml = "";
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListVendorSpecialSelected"], LblPagingS);
            div_comment.InnerHtml = "";
        }
        protected void Open_GridViews(GridView GrdVw, string sSQL, string sVendorID, string sDeviceTypeDesc, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sVendorID + "','" + sDeviceTypeDesc + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
            }
        }

        protected void CmdLoadDevice_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView2, "sp_list_vendor_special_available", txtVendorID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVendorSpecialAvailable", LblPagingA);
                Open_GridViews(GridView1, "sp_list_vendor_special_selected", txtVendorID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVendorSpecialSelected", LblPagingS);
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
                Open_GridViews(GridView2, "sp_list_vendor_special_available", txtVendorID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVendorSpecialAvailable", LblPagingA);
                Open_GridViews(GridView1, "sp_list_vendor_special_selected", txtVendorID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVendorSpecialSelected", LblPagingS);
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
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sDeviceTypeID = ""; string sErr = "";
                switch (e.CommandName.ToUpper())
                {
                    case "SELECT":
                        sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sDeviceTypeID != "")
                        {
                            strSQL = "sp_vendor_special_selected '" + txtVendorID.Value.ToString() + "','" + sDeviceTypeID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff,ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_vendor_special_available", txtVendorID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVendorSpecialAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vendor_special_selected", txtVendorID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVendorSpecialSelected", LblPagingS);
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
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); string strSQL = ""; ExecCommand ec = new ExecCommand();
                Int32 intAff = 0; string sDeviceTypeID = "";
                switch (e.CommandName.ToUpper())
                {
                    case "REMOVE":
                        sDeviceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                        if (sDeviceTypeID != "")
                        {
                            strSQL = "sp_vendor_special_removed '" + txtVendorID.Value.ToString() + "','" + sDeviceTypeID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff))
                            {
                                if (intAff > 0)
                                {
                                    Open_GridViews(GridView2, "sp_list_vendor_special_available", txtVendorID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVendorSpecialAvailable", LblPagingA);
                                    Open_GridViews(GridView1, "sp_list_vendor_special_selected", txtVendorID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVendorSpecialSelected", LblPagingS);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void CmdSearchAvai_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridViews(GridView2, "sp_list_vendor_special_available", txtVendorID.Value.ToString(), txtSearchAvai.Text.Trim(), "RecListVendorSpecialAvailable", LblPagingA);
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
                Open_GridViews(GridView1, "sp_list_vendor_special_selected", txtVendorID.Value.ToString(), txtSearchSel.Text.Trim(), "RecListVendorSpecialSelected", LblPagingS);
            }
            catch (Exception ex)
            {

            }
        }
    }
}