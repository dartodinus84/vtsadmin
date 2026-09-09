using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_igo_delivery_bundle_gsm_details : System.Web.UI.Page
    {
        public string sOrderID = "";
        public string sInv = "";
        public string sName = "";
        public string sAdd = "";
        public string sEmail = "";
        public string sPhone = "";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_igo_master_stock_gsm_search '" + txtSearch.Text.Trim() + "'";
                Session["RecListJobMaintDetails"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUIGODELIVERGSM"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                sOrderID = Session["ClsJobIDMaint"].ToString();
                sInv = Session["ClsJobIDMaintInv"].ToString();
                sName = Session["ClsJobIDMaintName"].ToString();
                sAdd = Session["ClsJobIDMaintAdd"].ToString();
                sEmail = Session["ClsJobIDMaintEmail"].ToString();
                sPhone = Session["ClsJobIDMaintPhone"].ToString();

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.InnerHtml = "";
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
                txtGSM.Text = "";
                txtStockID.Value = "";

            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClearDetail_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                lblMsg.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                strSQL = "usp_retail_selling_bundle_gsm '" + sOrderID + "','" + sInv + "','" + sName + "','" + sAdd + "','" + sEmail + "','" + txtStockID.Value.Trim() + "','" + txtGSM.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        clear();
                        Open_GridView();
                        lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                }

            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + ex.Message + ")";
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintDetails"], LblPaging);
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}