using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using System.Net;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_training : System.Web.UI.Page
    {
        static ITelegramBotClient botClient;

        public job_training()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBTRAINING"))
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

        private bool IsValidTrainCategorySelected()
        {
            if (CmbTrainCategoryID.SelectedItem == null)
            {
                return false;
            }

            string categoryId = CmbTrainCategoryID.SelectedItem.Value.Trim();
            return categoryId != "" && categoryId != "[Select]";
        }

        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                txtCustID.Value = "";
                txtCustFullName.Text = "";
                txtCustTypeDesc.Text = "";
                txtCustBranchName.Text = "";
                txtTrainingID.Text = "";
                txtReqDate.Text = "";
                ClType.Open_Combos(CmbBillAble, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_billable_type");
                CmbBillAble.SelectedValue = "[Select]";
                ClType.Open_Combos(CmbTrainCategoryID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_training_category");
                CmbTrainCategoryID.SelectedValue = "[Select]";
                txtScheduleDate.Text = "";
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtTrainingIDDelete.Value = "";
                txtStatusDelete.Value = "";
                div_comment.InnerHtml = "";
                Button2.Attributes.Remove("disabled");
                CmdSubmit.Text = "Submit";
            }
            catch (Exception ex)
            {

            }
        }
        private void SetListAlert(string message, bool isError)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                div_list_alert.Visible = false;
                div_list_alert.InnerHtml = "";
                return;
            }

            string css = isError ? "alert-danger" : "alert-info";
            div_list_alert.InnerHtml = "<div class='alert " + css + "' role='alert' style='margin-bottom:10px;'>" + HttpUtility.HtmlEncode(message) + "</div>";
            div_list_alert.Visible = true;
        }

        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_header_job_training '" + txtSearch.Value.Trim() + "'";
                string sErr = "";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);

                ViewState["RecCreateJobTrainingHeaderFieldSort"] = "TrainingID";
                ViewState["RecCreateJobTrainingHeaderDirSort"] = "DESC";
                SetListAlert("", false);

                if (!string.IsNullOrEmpty(sErr))
                {
                    SetListAlert("Cannot load training list: " + sErr, true);
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Cannot load training list (" + HttpUtility.HtmlEncode(sErr) + ")</div>";
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    LblPagingHeader.Text = "";
                    Session["RecCreateJobTrainingHeader"] = null;
                    return;
                }

                var ds = Rec.RecData;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    GridView1.DataSource = new System.Data.DataTable();
                    GridView1.DataBind();
                    LblPagingHeader.Text = "";
                    Session["RecCreateJobTrainingHeader"] = ds ?? new System.Data.DataSet();
                    SetListAlert("No training orders returned. Run sp_list_header_job_training in SSMS and check trx_training_order has data.", true);
                    return;
                }

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Session["RecCreateJobTrainingHeader"] = ds;
                ClType.showPaging(ds, GridView1, LblPagingHeader);
                ClType.setSorting(GridView1, ViewState["RecCreateJobTrainingHeaderFieldSort"].ToString(), ViewState["RecCreateJobTrainingHeaderDirSort"].ToString());
            }
            catch (Exception ex)
            {
                SetListAlert("Cannot load training list: " + ex.Message, true);
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Cannot load training list (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
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

                string jodate = Convert.ToDateTime(txtScheduleDate.Text).ToString("dd/MM/yyyy");
                string cus = Convert.ToString(txtCustFullName.Text);
                string tanda = Convert.ToString(txtRemark.Text);
                string pic = Convert.ToString(txtPICName.Text);
                string picnumber = Convert.ToString(txtPICPhone.Text);

                ExecCommand ec = new ExecCommand();
                if (txtCustID.Value.Trim() != "")
                {
                    if (!IsValidTrainCategorySelected())
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Category</div>";
                        return;
                    }

                    if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                    {
                        strSQL = "sp_submit_job_training '" + txtCustID.Value.Trim() + "','" + txtReqDate.Text.Trim() + "','" + CmbBillAble.SelectedItem.Value.Trim() + "','" + txtScheduleDate.Text.Trim() + "','" + txtRemark.Text.Trim() + "','" + CmbTrainCategoryID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                dashboard_assign_job.PersistCreatedTrainingOrderRemark(
                                    Session["ClsTypeDBConnStringSQL"].ToString(),
                                    txtCustID.Value.Trim(),
                                    Session["ClsTypeUserID"].ToString(),
                                    Convert.ToDateTime(txtScheduleDate.Text),
                                    tanda);
                                dashboard_assign_job.AutoAssignCreatedTrainingJobIfNeeded(
                                    Session["ClsTypeDBConnStringSQL"].ToString(),
                                    Session["ClsTypeUserID"].ToString(),
                                    txtCustID.Value.Trim(),
                                    Convert.ToDateTime(txtScheduleDate.Text),
                                    CmbTrainCategoryID.SelectedItem.Value.Trim(),
                                    tanda);
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit job training has been successfully</div>";
                                Bot(jodate,cus, pic, picnumber, tanda);
                            }
                            else
                            {
                                string failDetail = string.IsNullOrEmpty(sErr) ? "no rows affected (rows=" + intAff + ")" : sErr;
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job training has been failed (" + HttpUtility.HtmlEncode(failDetail) + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit job customer training has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        if (!IsValidTrainCategorySelected())
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Category</div>";
                            return;
                        }

                        strSQL = "sp_update_job_training '" + txtTrainingID.Text.Trim() + "','" + txtReqDate.Text.Trim() + "','" + txtCustID.Value.Trim() + "'," +
                                 "'" + txtScheduleDate.Text.Trim() + "','" + CmbBillAble.SelectedItem.Value.Trim() + "'," +
                                 "'" + txtRemark.Text.Trim() + "','" + CmbTrainCategoryID.SelectedItem.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridViewHeader();
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update job training has been successfully</div>";
                                Bot(jodate,cus, pic, picnumber, tanda);
                            }
                            else
                            {
                                string failDetail = string.IsNullOrEmpty(sErr) ? "no rows affected (rows=" + intAff + ")" : sErr;
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update job training has been failed (" + HttpUtility.HtmlEncode(failDetail) + ")</div>";
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
                if (e.Row.Cells.Count < 15)
                {
                    return;
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 9; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    if (CmdButton != null)
                    {
                        CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
                    }
                    for (int i = 9; i <= 14; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
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
                if (txtTrainingIDDelete.Value != "" && txtStatusDelete.Value != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG" || txtStatusDelete.Value.ToUpper().Trim() == "DR")
                    {
                        strSQL = "sp_delete_job_training '" + txtTrainingIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
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
                string sCustID = ""; string sFullName = ""; string sCustType = ""; string sBranchName = ""; string sTrainingID = "";
                string sReqDate = ""; string sBillableID = ""; string sSchDate = ""; string sRemark = "";
                string sStatus = ""; string sBillableDesc = ""; string sTrainCategoryID = "";

                sTrainingID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sReqDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sSchDate = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sBillableDesc = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sCustType = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sBillableID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sTrainCategoryID = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() == "RG" || sStatus.ToUpper().Trim() == "DR" || sStatus.ToUpper().Trim() == "OP")
                        {
                            txtCustID.Value = sCustID;
                            txtCustFullName.Text = sFullName;
                            txtCustTypeDesc.Text = sCustType;
                            txtCustBranchName.Text = sBranchName;

                            txtTrainingID.Text = ClType.CheckNbsp(sTrainingID);
                            txtReqDate.Text = sReqDate;
                            CmbBillAble.SelectedValue = sBillableID;
                            txtScheduleDate.Text = sSchDate;
                            txtRemark.Text = ClType.CheckNbsp(sRemark);
                            if (!string.IsNullOrWhiteSpace(sTrainCategoryID) && CmbTrainCategoryID.Items.FindByValue(sTrainCategoryID) != null)
                            {
                                CmbTrainCategoryID.SelectedValue = sTrainCategoryID;
                            }
                            else
                            {
                                CmbTrainCategoryID.SelectedValue = "[Select]";
                            }

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
        protected void Bot(string jodate, string cus, string pic, string picnumber, string tanda)
        {
            string apitoken = "";
            string url = "";
            string chatid = "";
            try
            {
                string strSQLtelegram = "sp_list_par_global 'TelegramChatID3'";
                Recordset RecChatID = new Recordset();
                RecChatID.Open(strSQLtelegram, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecChatID.RecordCount() > 0)
                {
                    chatid += RecChatID.Fields("ParValue");
                }
                string strSQLtelegram2 = "sp_list_par_global 'TelegramApi'";
                Recordset RecApi = new Recordset();
                RecApi.Open(strSQLtelegram2, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecApi.RecordCount() > 0)
                {
                    apitoken += RecApi.Fields("ParValue");
                }

                string strSQLtelegram3 = "sp_list_par_global 'TelegramUrl'";
                Recordset RecUrl = new Recordset();
                RecUrl.Open(strSQLtelegram3, Session["ClsTypeDBConnStringSQL"].ToString());
                if (RecUrl.RecordCount() > 0)
                {
                    url += RecUrl.Fields("ParValue");
                }
                string urlString = url;
                string apiToken = apitoken;
                string chatId = chatid;
                string text = BodyTelegram(jodate, cus, pic, picnumber, tanda);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string jodate, string cus, string pic, string picnumber, string tanda)
        {
            string msg = "";
            msg += "<b>CREATE JOB ORDER TRAINING</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>PIC Name</b>\r\n";
            msg += "<b>" + pic + "</b>\r\n";
            msg += "<b>PIC Number</b>\r\n";
            msg += "<b>" + picnumber + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + tanda + "</b>\r\n";
            return msg;
        }
    }
}