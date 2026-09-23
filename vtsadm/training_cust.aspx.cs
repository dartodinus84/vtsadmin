using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telegram.Bot;
using Telegram.Bot.Types;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class training_cust : System.Web.UI.Page
    {
        private const string TrainCategoryTrainingId = "TRC0000001";
        private const string TrainCategoryVisitId = "TRC0000002";

        static ITelegramBotClient botClient;

        public training_cust()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Full postback required: FileUpload does not work inside UpdatePanel async postback (especially WebView)
                SiteMaster masterPage = Master as SiteMaster;
                if (masterPage != null)
                {
                    masterPage.RegisterPostBackTrigger(CmdUploadPicture);
                    masterPage.RegisterPostBackTrigger(CmdRemovePicture);
                    masterPage.RegisterPostBackTrigger(CmdYesSubmit);
                    masterPage.RegisterPostBackTrigger(CmdYesRemark);
                }

                if (IsPostBack)
                {
                    EnsureDropdownsLoaded();
                    EnsureFunctionGridLoaded();
                }

                ClsType ClType = new ClsType();
                if (!dashboard_assign_job.CurrentUserCanAccessCloseJobTraining())
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
                            Open_GridViewFunc();
                            div_comment.InnerHtml = "";
                            BindTrainingPicturePreview();
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
                    BindTrainingPicturePreview();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetTrainingPictureSession()
        {
            return Session["ClsTypeNewPictureTraining"] == null
                ? ""
                : Session["ClsTypeNewPictureTraining"].ToString();
        }

        private static string SafeTrim(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        private string GetInputValue(HtmlInputText control)
        {
            return control == null ? "" : SafeTrim(control.Value);
        }

        private string GetHiddenValue(HtmlInputHidden control)
        {
            return control == null ? "" : SafeTrim(control.Value);
        }

        private string GetTextAreaValue(HtmlTextArea control)
        {
            return control == null ? "" : SafeTrim(control.Value);
        }

        private string SqlLiteral(string value)
        {
            return (value ?? "").Replace("'", "''");
        }

        private string GetSessionUserId()
        {
            return Session["ClsTypeUserID"] == null ? "" : Session["ClsTypeUserID"].ToString();
        }

        private string GetDbConnectionString()
        {
            return Session["ClsTypeDBConnStringSQL"] == null
                ? ""
                : Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }

        private void EnsureDropdownsLoaded()
        {
            try
            {
                string conn = GetDbConnectionString();
                if (conn == "")
                {
                    return;
                }

                ClsType clType = new ClsType();
                if (CmbBusinessField.Items.Count == 0)
                {
                    clType.Open_Combos(CmbBusinessField, conn, "", "sp_list_customer_business_field");
                }
                if (CmbTrainCategoryID.Items.Count == 0)
                {
                    clType.Open_Combos(CmbTrainCategoryID, conn, "", "sp_list_training_category");
                }
            }
            catch
            {
            }
        }

        private void EnsureFunctionGridLoaded()
        {
            try
            {
                if (GridView1.Rows.Count > 0)
                {
                    return;
                }

                DataSet ds = Session["RecListTrainingFunction"] as DataSet;
                if (ds != null && ds.Tables.Count > 0)
                {
                    GridView1.DataSource = ds.Tables[0];
                    GridView1.DataBind();
                    new ClsType().showPaging(ds, GridView1, LblPagingFunc);
                    return;
                }

                Open_GridViewFunc();
            }
            catch
            {
            }
        }

        private string GetSelectedDropDownValue(DropDownList dropdown)
        {
            if (dropdown == null)
            {
                return "";
            }
            if (dropdown.SelectedItem != null)
            {
                return SafeTrim(dropdown.SelectedItem.Value);
            }
            return SafeTrim(dropdown.SelectedValue);
        }

        private void BindTrainingPicturePreview()
        {
            try
            {
                string pic = GetTrainingPictureSession();
                if (!string.IsNullOrEmpty(pic))
                {
                    ImgTrainingUpload.ImageUrl = "~/Picture/" + pic;
                    ImgTrainingUpload.Visible = true;
                }
                else
                {
                    ImgTrainingUpload.ImageUrl = "";
                    ImgTrainingUpload.Visible = false;
                }
            }
            catch
            {
                ImgTrainingUpload.ImageUrl = "";
                ImgTrainingUpload.Visible = false;
            }
        }

        private void ShowPictureModalAfterPostBack()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showPictureModal",
                "$('#modal-picture').modal('show');", true);
        }

        protected void CmdUploadPicture_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblUploadMsg.InnerHtml = "";
                if (FileUploadTraining == null || !FileUploadTraining.HasFile)
                {
                    lblUploadMsg.InnerHtml = "<strong>Failed!</strong> Please select an image file.";
                    ShowPictureModalAfterPostBack();
                    return;
                }

                string fileName = FileUploadTraining.FileName ?? "";
                string ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblUploadMsg.InnerHtml = "<strong>Failed!</strong> Only JPG/JPEG/PNG allowed.";
                    ShowPictureModalAfterPostBack();
                    return;
                }

                string baseName = Path.GetFileNameWithoutExtension(fileName);
                if (string.IsNullOrWhiteSpace(baseName))
                {
                    baseName = "training";
                }
                string strFileName = baseName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                string pictureDir = Server.MapPath("~/Picture/");
                if (!Directory.Exists(pictureDir))
                {
                    Directory.CreateDirectory(pictureDir);
                }
                string strFullPath = Path.Combine(pictureDir, strFileName);
                FileUploadTraining.SaveAs(strFullPath);

                Session["ClsTypeNewPictureTraining"] = strFileName;
                BindTrainingPicturePreview();
                lblUploadMsg.InnerHtml = "<strong>Success!</strong>";
                ShowPictureModalAfterPostBack();
            }
            catch (Exception ex)
            {
                lblUploadMsg.InnerHtml = "<strong>Failed!</strong> " + HttpUtility.HtmlEncode(ex.Message);
                ShowPictureModalAfterPostBack();
            }
        }

        protected void CmdRemovePicture_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblUploadMsg.InnerHtml = "";
                Session["ClsTypeNewPictureTraining"] = "";
                BindTrainingPicturePreview();
                lblUploadMsg.InnerHtml = "<strong>Success!</strong> Picture removed.";
                ShowPictureModalAfterPostBack();
            }
            catch (Exception ex)
            {
                lblUploadMsg.InnerHtml = "<strong>Failed!</strong> " + HttpUtility.HtmlEncode(ex.Message);
                ShowPictureModalAfterPostBack();
            }
        }

        private bool IsValidTrainCategorySelected()
        {
            string categoryId = GetSelectedDropDownValue(CmbTrainCategoryID);
            return categoryId != "" && categoryId != "[Select]";
        }

        private string FormatDateForTelegram(string raw)
        {
            DateTime dt;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }
            if (DateTime.TryParse(raw, out dt))
            {
                return dt.ToString("dd/MM/yyyy");
            }
            return raw.Trim();
        }

        private string FormatDateForInput(string raw)
        {
            DateTime dt;
            if (DateTime.TryParse(raw, out dt))
            {
                return dt.ToString("yyyy-MM-dd");
            }
            return "";
        }

        private string GetRowValue(DataRow row, params string[] columns)
        {
            if (row == null || row.Table == null || columns == null)
            {
                return "";
            }

            foreach (string column in columns)
            {
                if (string.IsNullOrWhiteSpace(column) || !row.Table.Columns.Contains(column))
                {
                    continue;
                }

                string value = SafeTrim(Convert.ToString(row[column]));
                if (!string.IsNullOrWhiteSpace(value) && value != "&nbsp;")
                {
                    return value;
                }
            }

            return "";
        }

        private string FindCategoryIdByKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return string.Empty;
            }

            foreach (ListItem item in CmbTrainCategoryID.Items)
            {
                if (item.Value == "[Select]")
                {
                    continue;
                }

                if (item.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return item.Value;
                }
            }

            return string.Empty;
        }

        private string ResolveTrainCategoryDropdownId(string categoryId, string categoryName)
        {
            categoryId = SafeTrim(categoryId);
            categoryName = SafeTrim(categoryName);

            if (categoryId != "" && categoryId != "[Select]")
            {
                ListItem byId = CmbTrainCategoryID.Items.FindByValue(categoryId);
                if (byId != null)
                {
                    return byId.Value;
                }

                foreach (ListItem item in CmbTrainCategoryID.Items)
                {
                    if (string.Equals(item.Value, categoryId, StringComparison.OrdinalIgnoreCase))
                    {
                        return item.Value;
                    }
                }
            }

            if (categoryName != "")
            {
                foreach (ListItem item in CmbTrainCategoryID.Items)
                {
                    if (item.Value == "[Select]")
                    {
                        continue;
                    }

                    if (string.Equals(item.Text, categoryName, StringComparison.OrdinalIgnoreCase))
                    {
                        return item.Value;
                    }
                }

                foreach (ListItem item in CmbTrainCategoryID.Items)
                {
                    if (item.Value == "[Select]")
                    {
                        continue;
                    }

                    string itemText = item.Text ?? "";
                    if (itemText.IndexOf(categoryName, StringComparison.OrdinalIgnoreCase) >= 0
                        || categoryName.IndexOf(itemText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return item.Value;
                    }
                }

                if (categoryName.IndexOf("visit", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return FindCategoryIdByKeyword("visit");
                }

                if (categoryName.IndexOf("train", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return FindCategoryIdByKeyword("train");
                }
            }

            if (categoryId != "" && categoryId != "[Select]")
            {
                if (categoryId.IndexOf("visit", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return FindCategoryIdByKeyword("visit");
                }

                if (categoryId.IndexOf("train", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return FindCategoryIdByKeyword("train");
                }
            }

            return string.Empty;
        }

        private void ApplyTrainCategorySelection(string categoryId, string categoryName)
        {
            EnsureDropdownsLoaded();
            string resolvedId = ResolveTrainCategoryDropdownId(categoryId, categoryName);
            if (resolvedId == "" || resolvedId == "[Select]")
            {
                return;
            }

            CmbTrainCategoryID.ClearSelection();
            ListItem item = CmbTrainCategoryID.Items.FindByValue(resolvedId);
            if (item == null)
            {
                foreach (ListItem candidate in CmbTrainCategoryID.Items)
                {
                    if (string.Equals(candidate.Value, resolvedId, StringComparison.OrdinalIgnoreCase))
                    {
                        item = candidate;
                        break;
                    }
                }
            }

            if (item == null)
            {
                return;
            }

            item.Selected = true;
        }

        private DataRow LoadTrainingOrderAssignRow(string trainingId, string userId, string conn)
        {
            string sql =
                "SELECT TOP 1 "
                + "ISNULL(NULLIF(LTRIM(RTRIM(t.TrainCategoryID)), ''), NULLIF(LTRIM(RTRIM(ja.DeviceTypeID)), '')) AS TrainCategoryID, "
                + "ISNULL(NULLIF(LTRIM(RTRIM(cat.TrainCategoryDesc)), ''), ISNULL(catAssign.TrainCategoryDesc, '')) AS TrainCategoryDesc, "
                + "t.ScheduleDate, "
                + "ja.SchDate AS AssignDate, "
                + "ISNULL(itA.Name, ISNULL(itA.UserID, '')) AS AssignTrainerName, "
                + "ISNULL(itU.Name, ISNULL(itU.UserID, '')) AS UserTrainerName "
                + "FROM trx_training_order t WITH (NOLOCK) "
                + "LEFT JOIN ref_train_category cat WITH (NOLOCK) ON cat.TrainCategoryID = t.TrainCategoryID "
                + "OUTER APPLY ( "
                + "  SELECT TOP 1 d.SchDate, d.TechnicianID, d.DeviceTypeID "
                + "  FROM trx_job_assign_detail d WITH (NOLOCK) "
                + "  WHERE LTRIM(RTRIM(d.JobID)) = LTRIM(RTRIM(t.TrainingID)) "
                + "    AND ISNULL(d.Status, '') NOT IN ('DE') "
                + "  ORDER BY CASE WHEN EXISTS ( "
                + "    SELECT 1 FROM mst_itsupport i WITH (NOLOCK) "
                + "    WHERE i.ITID = d.TechnicianID "
                + "      AND LTRIM(RTRIM(i.UserID)) = '" + SqlLiteral(userId) + "' "
                + "      AND ISNULL(i.Status, '') NOT IN ('DE','BL')) THEN 0 ELSE 1 END, d.DtmUpd DESC "
                + ") ja "
                + "LEFT JOIN ref_train_category catAssign WITH (NOLOCK) ON catAssign.TrainCategoryID = ja.DeviceTypeID "
                + "LEFT JOIN mst_itsupport itA WITH (NOLOCK) ON itA.ITID = ja.TechnicianID AND ISNULL(itA.Status, '') NOT IN ('DE','BL') "
                + "LEFT JOIN mst_itsupport itU WITH (NOLOCK) ON LTRIM(RTRIM(itU.UserID)) = '" + SqlLiteral(userId) + "' AND ISNULL(itU.Status, '') NOT IN ('DE','BL') "
                + "WHERE LTRIM(RTRIM(t.TrainingID)) = '" + SqlLiteral(trainingId) + "'";

            string sErr = "";
            Recordset rec = new Recordset();
            rec.Open(sql, conn, ref sErr);
            if (!string.IsNullOrEmpty(sErr) || rec.RecordCount() <= 0 || rec.RecData == null || rec.RecData.Tables.Count == 0)
            {
                return null;
            }

            return rec.RecData.Tables[0].Rows[0];
        }

        private DataRow LoadSavedTrainingCustomerRow(string trainingId, string conn)
        {
            string sErr = "";
            Recordset rec = new Recordset();
            rec.Open(
                "sp_view_job_training '" + SqlLiteral(trainingId) + "','','2000-01-01','2099-12-31'",
                conn,
                ref sErr);
            if (!string.IsNullOrEmpty(sErr) || rec.RecordCount() <= 0 || rec.RecData == null || rec.RecData.Tables.Count == 0)
            {
                return null;
            }

            return rec.RecData.Tables[0].Rows[0];
        }

        private void BindTrainingInformation(string trainingId)
        {
            trainingId = SafeTrim(trainingId);
            if (trainingId == "")
            {
                return;
            }

            EnsureDropdownsLoaded();

            string pickedCategoryId = GetHiddenValue(hfPickedTrainCategoryID);
            string pickedCategoryDesc = GetHiddenValue(hfPickedTrainCategoryDesc);
            string postedCategoryId = GetSelectedDropDownValue(CmbTrainCategoryID);

            txtTrainingDate.Text = "";
            txtTrainers.Value = "";
            txtAttendances.Value = "";
            txtRemark.Value = "";
            if (CmbTrainCategoryID.Items.FindByValue("[Select]") != null)
            {
                CmbTrainCategoryID.SelectedValue = "[Select]";
            }

            string conn = GetDbConnectionString();
            if (conn == "")
            {
                return;
            }

            string userId = GetSessionUserId();
            DataRow orderRow = LoadTrainingOrderAssignRow(trainingId, userId, conn);
            DataRow savedRow = LoadSavedTrainingCustomerRow(trainingId, conn);

            string categoryId = GetRowValue(orderRow, "TrainCategoryID");
            string categoryName = GetRowValue(orderRow, "TrainCategoryDesc", "TrainingCategoryName", "TrainCategoryName");
            if (categoryId == "")
            {
                categoryId = GetRowValue(savedRow, "TrainCategoryID", "TrainingCategoryID");
            }
            if (categoryName == "")
            {
                categoryName = GetRowValue(savedRow, "TrainCategoryDesc", "TrainingCategoryName", "TrainCategoryName", "CategoryName", "TrainingCategoryName");
            }
            if (categoryId == "")
            {
                categoryId = pickedCategoryId;
            }
            if (categoryId == "" && postedCategoryId != "" && postedCategoryId != "[Select]")
            {
                categoryId = postedCategoryId;
            }
            if (categoryName == "")
            {
                categoryName = pickedCategoryDesc;
            }
            ApplyTrainCategorySelection(categoryId, categoryName);

            string trainingDate = GetRowValue(savedRow, "sTrainingDate", "TrainingDate");
            if (trainingDate == "")
            {
                trainingDate = GetRowValue(orderRow, "AssignDate");
            }
            if (trainingDate == "")
            {
                trainingDate = GetRowValue(orderRow, "ScheduleDate");
            }
            if (trainingDate == "")
            {
                trainingDate = GetInputValue(txtSchDate);
            }
            txtTrainingDate.Text = FormatDateForInput(trainingDate);

            string trainers = GetRowValue(savedRow, "Trainers");
            if (trainers == "")
            {
                trainers = GetRowValue(orderRow, "AssignTrainerName");
            }
            if (trainers == "")
            {
                trainers = GetRowValue(orderRow, "UserTrainerName");
            }
            if (trainers == "")
            {
                trainers = userId;
            }
            txtTrainers.Value = trainers;
            txtAttendances.Value = GetRowValue(savedRow, "Attendances");

            txtRemark.Value = GetRowValue(savedRow, "RemarkTraining", "Remark");

            Session["ClsTypeNewPictureTraining"] = GetRowValue(savedRow, "PictureFileName");
            BindTrainingPicturePreview();
        }

        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbBusinessField, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_business_field");
                ClType.Open_Combos(CmbTrainCategoryID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_training_category"); 
                txtTrainingID.Value = "";
                txtReqDate.Value = "";
                txtCustName.Value = "";
                txtSchDate.Value = "";

                txtCustBranchName.Value = "";
                txtCustomerName.Value = "";
                txtCustID.Value = "";

                txtTrainingDate.Text = "";
                txtTrainers.Value = "";
                txtAttendances.Value = "";
                txtRemark.Value = "";
                if (CmbBusinessField.Items.FindByValue("[Select]") != null)
                {
                    CmbBusinessField.SelectedValue = "[Select]";
                }
                if (CmbTrainCategoryID.Items.FindByValue("[Select]") != null)
                {
                    CmbTrainCategoryID.SelectedValue = "[Select]";
                }
                hfPickedTrainCategoryID.Value = "";
                hfPickedTrainCategoryDesc.Value = "";
                
                Session["ClsTypeNewPictureTraining"] = "";
                lblUploadMsg.InnerHtml = "";
                BindTrainingPicturePreview();
                CmdSubmit.Text = "Submit";
                CmdRemark.Text = "Add Note";
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
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Visible = false;

                    // Unique group per row so Yes/No work independently (shared GroupName breaks the checklist).
                    RadioButton RdoYes = (RadioButton)e.Row.FindControl("RdoYes");
                    RadioButton RdoNo = (RadioButton)e.Row.FindControl("RdoNo");
                    string groupName = "FuncCheck_" + e.Row.RowIndex;
                    if (RdoYes != null)
                    {
                        RdoYes.GroupName = groupName;
                    }
                    if (RdoNo != null)
                    {
                        RdoNo.GroupName = groupName;
                    }
                }

                
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (e.Row.Cells.Count > 7)
                    {
                        e.Row.Cells[7].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdPic = (LinkButton)e.Row.FindControl("CmdPic");
                    string picName = e.Row.Cells.Count > 7 ? e.Row.Cells[7].Text.ToString() : "";
                    if (CmdPic != null)
                    {
                        CmdPic.OnClientClick = "postPic('" + picName.Replace("'", "\\'") + "'); return false;";
                        if (string.IsNullOrWhiteSpace(picName) || picName == "&nbsp;")
                        {
                            CmdPic.Enabled = false;
                            CmdPic.CssClass = "btn btn-default btn-xs";
                        }
                    }

                    if (e.Row.Cells.Count > 7)
                    {
                        e.Row.Cells[7].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void saveFunction(string sTrainID)
        {
            try
            {
                div_comment.InnerHtml = "";
                string sFunctionID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    sFunctionID = GridView1.Rows[i].Cells[0].Text.ToString();
                    RadioButton RdoYes = (RadioButton)GridView1.Rows[i].Cells[2].FindControl("RdoYes");
                    RadioButton RdoNo = (RadioButton)GridView1.Rows[i].Cells[3].FindControl("RdoNo");
                    TextBox txRemark = (TextBox)GridView1.Rows[i].Cells[4].FindControl("txtFunctionRemark");

                    string yesVal = (RdoYes != null && RdoYes.Checked).ToString();
                    string noVal = (RdoNo != null && RdoNo.Checked).ToString();
                    string remarkVal = txRemark != null ? txRemark.Text.Trim() : "";

                    strSQL = "sp_insert_training_customer_function '" + sTrainID + "','" + sFunctionID + "','" + yesVal + "','" + noVal + "','" + remarkVal + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewFunc()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_training_function";
                Session["RecListTrainingFunction"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingFunc);
            }
            catch (Exception ex)
            {

            }
        }
        protected bool checkFunction()
        {
            bool boolOK = false;
            try
            {
                div_comment.InnerHtml = "";
                if (GridView1.Rows.Count != 0)
                {
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        RadioButton RdoYes = (RadioButton)GridView1.Rows[i].Cells[2].FindControl("RdoYes");
                        RadioButton RdoNo = (RadioButton)GridView1.Rows[i].Cells[3].FindControl("RdoNo");
                        bool yesChecked = RdoYes != null && RdoYes.Checked;
                        bool noChecked = RdoNo != null && RdoNo.Checked;
                        if (yesChecked || noChecked)
                        {
                            boolOK = true;
                        }
                    }
                }
                else
                {
                    boolOK = true;
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
            }
            return boolOK;
        }
        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridViewFunc();
                Open_GridViews(GridView2, "sp_list_trx_training_notes", txtTrainingID.Value.ToString(), "RecListTrainingNotes", LblPaging);
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        private string ExtractTrhId(string sErr)
        {
            if (string.IsNullOrWhiteSpace(sErr))
            {
                return "";
            }
            // sp_submit_training_customer returns new TrainID via: raiserror(@trhid,16,1)
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(sErr, @"TRH[0-9A-Za-z]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return m.Success ? m.Value : "";
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                int intAff = 0;

                string jono = Convert.ToString(txtTrainingID.Value);
                string jodate = FormatDateForTelegram(txtSchDate.Value);
                string trainingdate = FormatDateForTelegram(txtTrainingDate.Text);
                string cus = Convert.ToString(txtCustName.Value);
                string tanda = Convert.ToString(txtRemark.Value);
                string trainer = Convert.ToString(txtTrainers.Value);

                string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    string trainingId = GetInputValue(txtTrainingID);
                    string trainers = GetTextAreaValue(txtTrainers);
                    string attendances = GetTextAreaValue(txtAttendances);
                    string remark = GetTextAreaValue(txtRemark);
                    string trainingDate = SafeTrim(txtTrainingDate.Text);
                    string userId = GetSessionUserId();
                    string conn = GetDbConnectionString();

                    if (trainingId == "" || trainers == "" || attendances == "")
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in Training ID, Trainers and Attendances</div>";
                        return;
                    }
                    if (trainingDate == "")
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in Training Date</div>";
                        return;
                    }
                    if (!IsValidTrainCategorySelected())
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select Category</div>";
                        return;
                    }
                    if (!checkFunction())
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in the function menu</div>";
                        return;
                    }
                    if (userId == "" || conn == "")
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Session expired. Please login again.</div>";
                        return;
                    }

                    string closeDeniedMessage;
                    if (!dashboard_assign_job.CurrentUserCanCloseAssignedTrainingJob(trainingId, out closeDeniedMessage))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>"
                            + HttpUtility.HtmlEncode(closeDeniedMessage) + "</div>";
                        return;
                    }

                    string categoryId = GetSelectedDropDownValue(CmbTrainCategoryID);
                    string businessFieldId = GetSelectedDropDownValue(CmbBusinessField);
                    if (businessFieldId == "")
                    {
                        businessFieldId = "[Select]";
                    }

                    // Matches sp_submit_training_customer params:
                    // @trainingid,@trainingdate,@starttime,@duration,@trainers,@attendances,
                    // @traincategory,@remark,@picturefilename,@businessfieldid,@usrupd
                    strSQL = "sp_submit_training_customer '" + SqlLiteral(trainingId) + "','" + SqlLiteral(trainingDate) + "','',0," +
                             "'" + SqlLiteral(trainers) + "','" + SqlLiteral(attendances) + "','" + SqlLiteral(categoryId) + "','" + SqlLiteral(remark) + "'," +
                             "'" + SqlLiteral(GetTrainingPictureSession()) + "','" + SqlLiteral(businessFieldId) + "','" + SqlLiteral(userId) + "'";

                    bool execOk = ec.Execute(strSQL, conn, ref intAff, ref sErr);

                    // SP signals success by raiserror(@trhid,16,1) — Execute returns false and sErr contains TrainID (TRH...).
                    string trhId = ExtractTrhId(sErr);
                    if (!execOk && !string.IsNullOrEmpty(trhId))
                    {
                        saveFunction(trhId);
                        clear();
                        Open_GridViewFunc();
                        Open_GridViews(GridView2, "sp_list_trx_training_notes", "", "RecListTrainingNotes", LblPaging);

                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Customer training has been save successfully!</div>";
                        Bot(jono, jodate, trainingdate, cus, tanda, trainer);
                    }
                    else
                    {
                        string failMsg = string.IsNullOrWhiteSpace(sErr) ? "Unknown error" : sErr;
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving customer training has been failed (" + HttpUtility.HtmlEncode(failMsg) + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Saving customer training has been failed (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
            }
        }

        protected void CmdYesRemark_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();

                if (CmdRemark.Text.ToUpper() == "ADD NOTE")
                {
                    string trainingId = GetInputValue(txtTrainingID);
                    string custId = GetHiddenValue(txtCustID);
                    string remark = GetTextAreaValue(txtRemark);
                    string categoryId = GetSelectedDropDownValue(CmbTrainCategoryID);
                    string userId = GetSessionUserId();
                    string conn = GetDbConnectionString();

                    if (trainingId == "" || custId == "" || remark == "")
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please fill in Training ID, Customer and Remark</div>";
                        return;
                    }
                    if (!IsValidTrainCategorySelected())
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Please select Category</div>";
                        return;
                    }
                    if (userId == "" || conn == "")
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Session expired. Please login again.</div>";
                        return;
                    }

                    strSQL = "sp_submit_training_notes '" + SqlLiteral(trainingId) + "','" + SqlLiteral(custId) + "','" + SqlLiteral(categoryId) + "','" + SqlLiteral(GetTrainingPictureSession()) + "','" + SqlLiteral(remark) + "','" + SqlLiteral(userId) + "'";
                    bool execOk = ec.Execute(strSQL, conn, ref intAff, ref sErr);

                    // OleDb often returns -1 for SP inserts; treat Execute success as saved.
                    if (execOk)
                    {
                        Open_GridViews(GridView2, "sp_list_trx_training_notes", trainingId, "RecListTrainingNotes", LblPaging);
                        div_comment.InnerHtml = "<div class='alert alert-success alert-dismissible'><h4><i class='icon fa fa-check'></i> Success!</h4>Add Notes training has been save successfully!</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Add Notes has been failed (" + HttpUtility.HtmlEncode(sErr) + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Add Notes training has been failed (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
            }
        }

        private void EnsureNotesGridColumns(System.Data.DataTable dt)
        {
            if (dt == null)
            {
                return;
            }
            string[] required = new string[]
            {
                "TrainingID", "CustomerName", "TrainingCategoryName", "Remark",
                "UsrUpd", "DtmUpd", "PictureFileName"
            };
            foreach (string col in required)
            {
                if (!dt.Columns.Contains(col))
                {
                    dt.Columns.Add(col, typeof(string));
                }
            }
        }

        protected void Open_GridViews(GridView GrdVw, string sSQL, string sTrainingID, string sSessionName, Label LblPaging)
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = sSQL + " '" + (sTrainingID ?? "").Replace("'", "''") + "'";
                string sErr = "";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);

                if (!string.IsNullOrEmpty(sErr))
                {
                    GrdVw.DataSource = new System.Data.DataTable();
                    GrdVw.DataBind();
                    LblPaging.Text = "";
                    Session[sSessionName] = null;
                    div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cannot load remark list (" + HttpUtility.HtmlEncode(sErr) + ")</div>";
                    return;
                }

                System.Data.DataSet ds = Rec.RecData;
                if (ds == null || ds.Tables.Count == 0)
                {
                    System.Data.DataTable dtEmpty = new System.Data.DataTable();
                    EnsureNotesGridColumns(dtEmpty);
                    GrdVw.DataSource = dtEmpty;
                    GrdVw.DataBind();
                    LblPaging.Text = "";
                    Session[sSessionName] = ds ?? new System.Data.DataSet();
                    return;
                }

                EnsureNotesGridColumns(ds.Tables[0]);
                GrdVw.DataSource = ds;
                GrdVw.DataBind();
                Session[sSessionName] = ds;
                ClType.showPaging(ds, GrdVw, LblPaging);
            }
            catch (Exception ex)
            {
                try
                {
                    GrdVw.DataSource = new System.Data.DataTable();
                    GrdVw.DataBind();
                }
                catch { }
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cannot load remark list (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListTrainingNotes"], LblPaging);
            div_comment.InnerHtml = "";
        }

        protected void CmdLoadTraining_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string trainingId = GetInputValue(txtTrainingID);
                BindTrainingInformation(trainingId);
                Open_GridViews(GridView2, "sp_list_trx_training_notes", trainingId, "RecListTrainingNotes", LblPaging);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger alert-dismissible'><h4><i class='icon fa fa-ban'></i> Failed!</h4>Cannot load remark list (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
            }
        }

        protected void Bot(string jono, string jodate, string trainingdate, string cus, string tanda, string trainer)
        {
            string apitoken = "";
            string url = "";
            string chatid = "";
            try
            {
                string strSQLtelegram = "sp_list_par_global 'TelegramChatID2'";
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

                string text = BodyTelegram(jono,jodate, trainingdate, cus, tanda, trainer);
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebClient webclient = new WebClient();
                webclient.DownloadString(urlString);
            }
            catch (Exception ex)
            {

            }
        }
        private string BodyTelegram(string jono ,string jodate, string trainingdate, string cus, string tanda, string trainer)
        {
            string msg = "";
            msg += "<b>JOB ORDER CUSTOMER TRAINING SUCCESS</b>\r\n";
            msg += "<b>Training Job Order</b>\r\n";
            msg += "<b>" + jono + "</b>\r\n";
            msg += "<b>Customer</b>\r\n";
            msg += "<b>" + cus + "</b>\r\n";
            msg += "<b>Schedule Date</b>\r\n";
            msg += "<b>" + jodate + "</b>\r\n";
            msg += "<b>Training Date</b>\r\n";
            msg += "<b>" + trainingdate + "</b>\r\n";
            msg += "<b>Remark</b>\r\n";
            msg += "<b>" + tanda + "</b>\r\n";
            msg += "<b>Trainers</b>\r\n";
            msg += "<b>" + trainer + "</b>\r\n";
            return msg;
        }
    }
}