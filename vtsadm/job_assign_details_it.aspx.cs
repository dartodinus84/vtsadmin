using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_assign_details_it : System.Web.UI.Page
    {
        public string sAssignID = "";
        public string sJobID = "";
        public string sCustID = "";

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_it_assign_detail '" + txtSearch.Text.Trim() + "','" + txtScheduleDate.Value.Trim() + "'";
                Session["RecListJobAssignDetails"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBASSIGN"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                sAssignID = Session["ClsAssignIDAssign"].ToString();
                sJobID = Session["ClsJobIDAssign"].ToString();
                sCustID = Session["ClsJobCustIDAssign"].ToString();

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
                txtTechnicianID.Value = "";
                txtName.Text = "";
                txtTechnicianID2.Text = "";
                txtRemark.Text = "";

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
                if (sAssignID.Trim() != "")
                {
                    if (sJobID.Trim() != "")
                    {
                        if (txtTechnicianID.Value.Trim() != "")
                        {
                            if (txtName.Text.Trim() != "")
                            {
                                if (txtScheduleDate.Value.Trim() != "")
                                {
                                    if (txtTimeSchedule.Value.Trim() != "")
                                    {
                                        Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                        ExecCommand ec = new ExecCommand();
                                        string SchDate = txtScheduleDate.Value + " " + txtTimeSchedule.Value;
                                        strSQL = "sp_insert_job_order_assign_detail '" + sAssignID.Trim() + "','" + sJobID.Trim() + "','" + txtTechnicianID.Value.Trim() + "','" + SchDate.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                        {
                                            if (intAff > 0)
                                            {
                                                clear();
                                                Open_GridView();
                                                lblMsg.InnerHtml = "<strong>Success!</strong> Create Assign Job Order";
                                            }
                                        }
                                        else
                                        {
                                            lblMsg.InnerHtml = "<strong>Failed!</strong> Create Assign Job Order (" + sErr + ")";
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select Schedule Time ";
                                    }
                                }
                                else
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please select Schedule Date ";
                                }

                            }
                            else
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Please select Technician Name ";
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please select Technician first";
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please create Assign Job Order Header first";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please create job header first";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobAssignDetails"], LblPaging);
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
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[0].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[1].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}