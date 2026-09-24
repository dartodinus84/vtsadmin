using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_training_customer_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                string accessMenu = Session["ClsTypeAccessMenu"] == null
                    ? string.Empty
                    : Session["ClsTypeAccessMenu"].ToString().ToUpperInvariant();
                if (!accessMenu.Contains("MNUJOBTRAINING") && !accessMenu.Contains("MNUDASHASSIGNJOB"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Open_GridViewCust("");
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

        protected void CmdSearchCust_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViewCust(txtSearchCust.Value.Trim());
            }
            catch (Exception ex)
            {

            }
        }
        private void Open_GridViewCust(string sFullName)
        {
            ClsType ClType = new ClsType();
            string strSQL = "sp_list_job_training_customer_search '" + sFullName + "'";
            Session["RecListJobTrainingCustomerSearch"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
        }
        protected void GridView1_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobTrainingCustomerSearch"], LblPaging);
        }

        private static string CleanGridCellValue(ClsType clType, object value)
        {
            string raw = HttpUtility.HtmlDecode(Convert.ToString(value) ?? string.Empty).Trim();
            return clType.CheckNbsp(raw);
        }

        private static string JsStringLiteral(string value)
        {
            return "'" + HttpUtility.JavaScriptStringEncode(value ?? string.Empty) + "'";
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    DataRowView rowView = e.Row.DataItem as DataRowView;
                    if (rowView == null)
                    {
                        return;
                    }

                    ClsType clType = new ClsType();
                    string custId = CleanGridCellValue(clType, rowView["CustID"]);
                    string fullName = CleanGridCellValue(clType, rowView["FullName"]);
                    string custTypeDesc = CleanGridCellValue(clType, rowView["CustTypeDesc"]);
                    string branchName = CleanGridCellValue(clType, rowView["BranchName"]);
                    string picName = CleanGridCellValue(clType, rowView["PICName1"]);
                    string picPhone = CleanGridCellValue(clType, rowView["MobilePhone1"]);
                    if (string.IsNullOrWhiteSpace(picPhone))
                    {
                        picPhone = CleanGridCellValue(clType, rowView["OfficePhone1"]);
                    }

                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdSelect");
                    if (CmdButton == null)
                    {
                        return;
                    }

                    CmdButton.OnClientClick = string.Format(
                        "var postCustChildFn=null;"
                        + "try{{"
                        + "var w=window;while(w&&!postCustChildFn){{if(w.postCustChild){{postCustChildFn=w.postCustChild;break;}}w=(w!==w.parent)?w.parent:null;}}"
                        + "if(!postCustChildFn&&window.top&&window.top.postCustChild){{postCustChildFn=window.top.postCustChild;}}"
                        + "}}catch(ex){{}}"
                        + "if(postCustChildFn){{postCustChildFn({0},{1},{2},{3},{4},{5});}}"
                        + "return false;",
                        JsStringLiteral(custId),
                        JsStringLiteral(fullName),
                        JsStringLiteral(custTypeDesc),
                        JsStringLiteral(branchName),
                        JsStringLiteral(picName),
                        JsStringLiteral(picPhone));
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {

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
