using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class training_cust_job_training_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBTRAINING"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewJobTraining("");
                            //Open_GridViewDevice(txtTechnicianID.Value.Trim(), txtSearchDevice.Value.Trim());
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

        private static string EscapeForJs(string value)
        {
            return HttpUtility.JavaScriptStringEncode(Convert.ToString(value) ?? string.Empty);
        }

        private static string GetDataRowString(DataRowView row, string columnName)
        {
            if (row == null || row.Row == null || row.Row.Table == null || !row.Row.Table.Columns.Contains(columnName))
            {
                return string.Empty;
            }

            return Convert.ToString(row[columnName]) ?? string.Empty;
        }

        private static string GetResolvedTrainCategoryId(DataRowView row)
        {
            string categoryId = GetDataRowString(row, "TrainCategoryID").Trim();
            if (categoryId == "" || categoryId == "[Select]")
            {
                categoryId = GetDataRowString(row, "AssignCategoryID").Trim();
            }

            return categoryId;
        }

        private static string GetResolvedTrainCategoryDesc(DataRowView row)
        {
            string categoryDesc = GetDataRowString(row, "TrainCategoryDesc").Trim();
            if (categoryDesc == "")
            {
                categoryDesc = GetDataRowString(row, "AssignCategoryDesc").Trim();
            }

            return categoryDesc;
        }

        private void Open_GridViewJobTraining(string sName)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_trainig_customer_job_training_search '" + sName + "','" + Session["ClsTypeUserID"].ToString() + "'";
            Session["RecListTrainingCustJobTrainingSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTrainingCustJobTrainingSearch"], LblPaging);
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 5; i <= 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView row = e.Row.DataItem as DataRowView;
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    CmdButton.OnClientClick = "parent.postJobTrainingChild("
                        + "'" + EscapeForJs(GetDataRowString(row, "TrainingID")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "sReqDate")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "FullName")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "sSchDate")) + "',"
                        + "'" + EscapeForJs(e.Row.Cells[4].Text) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "CustID")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "CustBranchName")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "BusinessFieldID")) + "',"
                        + "'" + EscapeForJs(GetResolvedTrainCategoryId(row)) + "',"
                        + "'" + EscapeForJs(GetResolvedTrainCategoryDesc(row)) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "sAssignDate")) + "',"
                        + "'" + EscapeForJs(GetDataRowString(row, "AssignTrainer")) + "');return false;";
                    for (int i = 5; i <= 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdSearchJobTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewJobTraining(txtSearchJobTraining.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
    }
}