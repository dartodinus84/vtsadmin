using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_postdeal : System.Web.UI.Page
    {
        public job_postdeal()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();

                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBDEAL"))
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
                            Open_GridViewHeader();
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
                txtCustID.Value = "";
                txtCustFullName.Value = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtPICName.Text = "";
                txtPICPhone.Text = "";
                txtJobActivityID.Value = "";
                txtReqDate.Text = "";
                ClType.Open_Combos(CmbProductID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_mst_product");
                CmbProductID.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), Session["ClsTypeUserID"].ToString(), "sp_list_deal_marketing");
                CmbMarketing.SelectedValue = "[Select]";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtJobActivityIDDelete.Value = "";
                txtStatusDelete.Value = "";

                if (Session["ClsTypeUserMarketingID"].ToString() != "")
                {
                    CmbMarketing.Enabled = false;
                    CmbMarketing.SelectedValue = Session["ClsTypeUserMarketingID"].ToString();
                }
                Button2.Attributes.Remove("disabled");
                CmdSubmit.Text = "Submit";
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL;

                // Jika user login sebagai marketing tertentu (punya MarketingID di session),
                // tambahkan parameter MarketingID ke stored procedure agar data hanya untuk marketing tersebut.
                if (Session["ClsTypeUserMarketingID"] != null &&
                    !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
                {
                    string marketingId = Session["ClsTypeUserMarketingID"].ToString();
                    strSQL = "sp_list_header_job_post_deal '" + txtSearch.Value.Trim() + "','" + marketingId + "'";
                }
                else
                {
                    // User non-marketing: pakai parameter search saja (tanpa filter marketing)
                    strSQL = "sp_list_header_job_post_deal '" + txtSearch.Value.Trim() + "'";
                }
                ViewState["RecCreateJobTrainingHeaderFieldSort"] = "JobActivityID";
                ViewState["RecCreateJobTrainingHeaderDirSort"] = "DESC";
                Session["RecCreateJobTrainingHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString());
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
                Open_GridViewHeader();
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
                int intAff = 0; string strSQL = ""; string sErr = "";

                string cus = Convert.ToString(txtCustFullName.Value);
                string tanda = Convert.ToString(txtRemark.Text);
                string pic = Convert.ToString(txtPICName.Text);
                string picnumber = Convert.ToString(txtPICPhone.Text);

                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                    {
                        strSQL = "sp_submit_job_postdeal '" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','', '" + CmbProductID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job training has been successfully</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job training has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job customer training has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        strSQL = "sp_update_job_postdeal '" + txtJobActivityID.Value.Trim() + "','" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','', '" + CmbProductID.SelectedItem.Value.Trim() + "','" + txtRemark.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job training has been successfully</div>";

                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job training has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job customer training has been failed (" + sErr + ")</div>";
                        }
                    }
                    //}
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
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecCreateJobTrainingHeader"], LblPagingHeader, ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

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
                    for (int i = 7; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[6].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";
                    for (int i = 7; i <= 13; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[0].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtJobActivityIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_postdeal '" + txtJobActivityIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Job training has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing job training has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sJobActivityID = "";
                string sReqDate = ""; string sMarketingName = ""; string sProductName = ""; string sRemark = "";
                string sStatus = ""; string sProductID = ""; string sMarketingID = ""; string sPrice = "";

                sJobActivityID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sReqDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sProductName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sMarketingName = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sProductID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {

                            txtCustID.Value = sCustID;
                            txtCustFullName.Value = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;
                            txtJobActivityID.Value = ClType.CheckNbsp(sJobActivityID);
                            txtReqDate.Text = sReqDate;

                            CmbProductID.SelectedValue = sProductID;

                            if (string.IsNullOrEmpty(sMarketingID))
                            {
                                // Default to first item
                                if (CmbMarketing.Items.Count > 0)
                                    CmbMarketing.SelectedIndex = 0;
                            }
                            else
                            {
                                // Try to find the marketing ID in the dropdown list
                                ListItem marketingItem = CmbMarketing.Items.FindByValue(sMarketingID);
                                if (marketingItem != null)
                                {
                                    CmbMarketing.SelectedValue = sMarketingID;
                                }
                                else
                                {
                                    // If not found, try to find by text
                                    marketingItem = CmbMarketing.Items.FindByText(sMarketingName);
                                    if (marketingItem != null)
                                    {
                                        CmbMarketing.SelectedValue = marketingItem.Value;
                                    }
                                    else if (CmbMarketing.Items.Count > 0)
                                    {
                                        // If still not found, select the first item
                                        CmbMarketing.SelectedIndex = 0;
                                    }
                                }
                            }


                            txtRemark.Text = ClType.CheckNbsp(sRemark);

                            Button2.Style.Add("disabled", "disabled");

                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Job training can not be edited, due to status code has been (" + ex.Message + ")</div>";
            }
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecCreateJobTrainingHeader"], ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecCreateJobTrainingHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecCreateJobTrainingHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}