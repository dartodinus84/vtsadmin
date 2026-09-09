using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using Telegram.Bot.Types;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class postdeal_cust : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBDEAL"))
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
                            div_comment.InnerHtml = "";
                            CmdSubmit.Visible = false;
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
                else
                {
                    UpdateSubmitButtonVisibility();
                }
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
        }

        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbDealStatusID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_postdeal_status");

                txtJobActivityID.Value = "";
                txtReqDate.Value = "";
                txtCustName.Value = "";
                txtProductName.Value = "";
                txtActivityDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFollowUpDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPICName.Value = "";
                txtPICPhone.Value = "";
                txtActivityCode.Value = "";
                txtMarketingName.Value = "";

                txtCustBranchName.Value = "";
                txtCustID.Value = "";
                CmbDealStatusID.SelectedValue = "[Select]";

                rblClosingStatus.SelectedValue = "YES";

                // Reset location fields
                hfLatitude.Value = "";
                hfLongitude.Value = "";

                LblActivityID.InnerHtml = "";
                txtActivityIDDelete.Value = "";
                txtStatusDelete.Value = "";

                // Clear both the legacy single image and the new list
                Session["ClsTypeDealPicture"] = "";
                Session["ClsTypeDealPictures"] = new List<string>();

                // Clear both the legacy single attachment and the new list
                Session["ClsTypeAttacment"] = "";
                Session["ClsTypeAttachments"] = new List<string>();

                CmdSubmit.Text = "Submit";
                txtRemark.Value = "";
                CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
        }

        private void UpdateSubmitButtonVisibility()
        {
            CmdSubmit.Visible = !string.IsNullOrEmpty(txtJobActivityID.Value);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 10; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    CmdPic.OnClientClick = "postPic('" + e.Row.Cells[10].Text.ToString() + "'); return false;";

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[9].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[11].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'); return false;";

                    for (int i = 10; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridViews(GridView2, "sp_get_postdeal_customer_detail", txtJobActivityID.Value.ToString(), "RecListDealCustDetail", LblPaging);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
        }
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0;

                int jobactivityid = Convert.ToInt32(txtJobActivityID.Value);
                string remark = Convert.ToString(txtRemark.Value);

                // Convert dates to DateTime objects first, then use SQL Server's expected format
                DateTime activityDateTime = !string.IsNullOrEmpty(txtActivityDate.Text) ? DateTime.Parse(txtActivityDate.Text) : DateTime.Now;
                DateTime followupDateTime = !string.IsNullOrEmpty(txtFollowUpDate.Text) ? DateTime.Parse(txtFollowUpDate.Text) : DateTime.Now;

                // Format dates specifically for SQL Server (yyyyMMdd is unambiguous)
                string activitydate = activityDateTime.ToString("yyyyMMdd");
                string followupdate = followupDateTime.ToString("yyyyMMdd");

                string latitude = hfLatitude.Value;
                string longitude = hfLongitude.Value;

                // Get pictures from session - handle both the new list and legacy single image
                string picturesList = "";
                if (Session["ClsTypeDealPictures"] != null)
                {
                    var imagesList = (List<string>)Session["ClsTypeDealPictures"];
                    picturesList = string.Join(",", imagesList);
                }
                else if (Session["ClsTypeDealPicture"] != null)
                {
                    picturesList = Session["ClsTypeDealPicture"].ToString();
                }

                // Get attachments from session - handle both the new list and legacy single attachment
                string attachmentsList = "";
                if (Session["ClsTypeAttachments"] != null)
                {
                    var attachments = (List<string>)Session["ClsTypeAttachments"];
                    attachmentsList = string.Join(",", attachments);
                }
                else if (Session["ClsTypeAttacment"] != null)
                {
                    attachmentsList = Session["ClsTypeAttacment"].ToString();
                }

                string strSQL = "";
                string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    // Check if required fields are filled
                    if (txtJobActivityID.Value.Trim() != "" && txtPICName.Value.Trim() != "")
                    {
                        // Use the correct SP parameters for deal_cust context
                        strSQL = "sp_submit_postdeal_customer " + jobactivityid + ",'" + activitydate + "','" + followupdate + "','" + remark + "'," +
                                   "'" + CmbDealStatusID.SelectedItem.Value.ToString() + "','" + picturesList + "','" + attachmentsList + "','" + latitude + "','" + longitude + "','" + rblClosingStatus.SelectedValue.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                Open_GridViews(GridView2, "sp_get_postdeal_customer_detail", txtJobActivityID.Value.ToString(), "RecListDealCustDetail", LblPaging);
                                div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>deal customer has been save successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving deal customer has been failed< (" + sErr + ")/div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save deal customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in required fields</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving deal customer has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void Open_GridViews(GridView GrdVw, string sSQL, string sTrainingID, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + sTrainingID + "'";
                Session[sSessionName] = ClType.Open_GridView(GrdVw, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception)
            {
                // Exception handling (empty in original)
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListDealCustDetail"], LblPaging);
            div_comment.InnerHtml = "";
        }

        protected void CmdLoadDeal_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridViews(GridView2, "sp_get_postdeal_customer_detail", txtJobActivityID.Value.ToString(), "RecListDealCustDetail", LblPaging);
                div_comment.InnerHtml = "";
                UpdateSubmitButtonVisibility();
            }
            catch (Exception ex)
            {
                // Exception handling (empty in original)
            }
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

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtActivityIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_postdeal_customer '" + txtActivityIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                Open_GridViews(GridView2, "sp_get_postdeal_customer_detail", txtJobActivityID.Value.ToString(), "RecListDealCustDetail", LblPaging);
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> deal customer has been remove successfully!</div>";
                            }
                            else
                            {
                                //txtError.Value = "Delete failed";
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing deal customer has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete deal customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> deal customer can not be deleted, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Delete deal customer has been failed (" + ex.Message + ")</div>";
            }
        }

    }
}