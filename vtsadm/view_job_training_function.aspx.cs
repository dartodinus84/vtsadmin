using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_job_training_function : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWJOBTRAINING"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridView("");
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
        private void Open_GridView(string sSearch)
        {
            if (txtTrainID.Value.Trim() != "")
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_job_training_function '" + txtTrainID.Value.Trim() + "','" + sSearch + "'";
                Session["RecViewJobTrainingFunction"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecViewJobTrainingFunction"], LblPaging);
        }

        protected void CmdSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridView(txtSearchFunc.Value.Trim());
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
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sIsYes = e.Row.Cells[4].Text.ToString();
                    string sIsNo = e.Row.Cells[5].Text.ToString();
                    RadioButton RdoYes = (RadioButton)e.Row.Cells[2].FindControl("RdoYes");
                    RadioButton RdoNo = (RadioButton)e.Row.Cells[3].FindControl("RdoNo");
                    if (sIsYes == "1")
                    {
                        RdoYes.Checked = true;
                    }
                    else
                    {
                        RdoYes.Checked = false;
                    }
                    if (sIsNo == "1")
                    {
                        RdoNo.Checked = true;
                    }
                    else
                    {
                        RdoNo.Checked = false;
                    }

                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}