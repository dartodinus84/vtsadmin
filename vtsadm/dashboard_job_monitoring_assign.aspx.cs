using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_job_monitoring_assign : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecListTechnicianFieldSort";
        string sViewStateDirSort = "RecListTechnicianDirSort";
        string sSessionRecList = "RecListTechnician";
        string sSessionRecScheduleList = "RecListSchedule";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_rpt_summary_device_warehouse '',''";
                ViewState[sViewStateFieldSort] = "DeviceTypeDesc";
                ViewState[sViewStateDirSort] = "ASC";

                Session[sSessionRecScheduleList] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();

                string filterType = Session["SessionFilterType"].ToString();
                string dateFrom = Session["SessionDateFrom"].ToString();
                string dateTo = Session["SessionDateTo"].ToString();

                string strSQL = "sp_dashboard_jo_alokasi_cnt '" + Session["ClsTypeUserTechnicianID"].ToString() + "'";
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    LblCntJoOpen.InnerHtml = Convert.ToDouble(Rec.Fields("CntJoOpen")).ToString("#,##0");
                    LblCntNotAllocation.InnerHtml = Convert.ToDouble(Rec.Fields("CntNotAllocation")).ToString("#,##0");
                    LblCntAllocation.InnerHtml = Convert.ToDouble(Rec.Fields("CntAllocation")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }



        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
            Open_GridView();
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SessionDateFrom"] = txtDateFrom.Text.Trim();
                Session["SessionDateTo"] = txtDateTo.Text.Trim();
                Session["SessionFilterType"] = CmbFilterType.SelectedItem.Value.Trim();

                open_dashboard_data();
                Open_GridView();
            }
            catch (Exception ex)
            {

            }
        }

       

        protected void Clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbFilterType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_dashboard_filter");

                txtDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

                CmbFilterType.SelectedValue = "[Select]";
                LblCntJoOpen.InnerText = "0";
                LblCntNotAllocation.InnerText = "0";
                LblCntAllocation.InnerText = "0";


                Session["SessionDateFrom"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionDateTo"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionFilterType"] = "";

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHJO"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            open_dashboard_data();
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

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecScheduleList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session[sSessionRecScheduleList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }

        
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {

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

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
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