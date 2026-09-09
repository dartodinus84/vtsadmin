using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class view_mutation_device_approve : System.Web.UI.Page
    {
        string sViewStateFieldSort = "RecViewMutationDeviceFieldSort";
        string sViewStateDirSort = "RecViewMutationDeviceDirSort";
        string sSessionRecList = "RecViewMutationDevice";
        private bool CanBulkApprove
        {
            get
            {
                string groupId = Convert.ToString(Session["ClsTypeUserGroupID"]).Trim();
                return string.Equals(groupId, "TECHMIS", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(groupId, "LM", StringComparison.OrdinalIgnoreCase);
            }
        }

        private void ConfigureBulkApprovalControls()
        {
            bool canBulkApprove = CanBulkApprove;
            PnlBulkApprove.Visible = canBulkApprove;

            if (GridView2.Columns.Count > 0)
            {
                GridView2.Columns[GridView2.Columns.Count - 1].Visible = canBulkApprove;
            }
        }

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_view_mutation_device_approve '" + Session["ClsTypeUserTechnicianID"].ToString() + "','" + txtSearch.Text.Trim() + "','" + txtDateFrom.Text.Trim() + "','" + txtDateTo.Text.Trim() + "'";
                ViewState[sViewStateFieldSort] = "DeviceID";
                ViewState[sViewStateDirSort] = "DESC";
                Session[sSessionRecList] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUVIEWMUTDEVICEAPR"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                ConfigureBulkApprovalControls();
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExport);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportWarehouse);
                ((SiteMaster)this.Page.Master).RegisterPostBackTrigger(CmdExportTechnician);
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
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
                txtSearch.Text = "";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
            }
            catch (Exception ex)
            {

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
        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }
        protected void CmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                //ClsType clType = new ClsType();
                //Recordset Rec = new Recordset();
                //string strFullPath = ""; string sMsg = ""; string strFileName = "";
                //DateTime dt = DateTime.Now;
                //strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                //strFullPath = Server.MapPath("~/Export//" + strFileName);
                //Rec.RecData = Session["RecViewMutationDevice"] as System.Data.DataSet;
                //if (Rec.RecordCount() > 0)
                //{
                //    if (clType.ExportToCsvTab(Rec, "Mutation Device", strFullPath.Trim(), 50000, ref sMsg))
                //    {
                //        Response.Clear();
                //        Response.ContentType = "text/plain";
                //        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                //        Response.TransmitFile(strFullPath);
                //        Response.Flush();
                //        File.Delete(strFullPath);
                //        Response.End();
                //    }

                //}
                //else
                //{
                //    div_comment.InnerHtml = "No records found";
                //}
                div_comment.InnerHtml = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView gv = new GridView();
                Recordset Rec = new Recordset();
                DateTime dt = DateTime.Now;
                string strFileName = dt.ToString("yyyyMMddHHmmss") + ".xls";
                Rec.RecData = Session[sSessionRecList] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    gv.DataSource = Rec.RecData;
                    gv.AllowPaging = false;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.Charset = "";
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_comment.InnerHtml = "No records found";
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
                    for (int i = 11; i <= 24; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdButton = (LinkButton)e.Row.FindControl("CmdDetails");
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "','" + e.Row.Cells[3].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[4].Text.ToString() + "','" + e.Row.Cells[5].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[6].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[8].Text.ToString() + "','" + e.Row.Cells[9].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[10].Text.ToString() + "','" + e.Row.Cells[11].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[12].Text.ToString() + "','" + e.Row.Cells[13].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[14].Text.ToString() + "','" + e.Row.Cells[15].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[16].Text.ToString() + "','" + e.Row.Cells[17].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[18].Text.ToString() + "','" + e.Row.Cells[19].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[20].Text.ToString() + "','" + e.Row.Cells[21].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[22].Text.ToString() + "','" + e.Row.Cells[23].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[24].Text.ToString() + "'); return false;";

                    LinkButton CmdDelete = (LinkButton)e.Row.FindControl("CmdDelete");
                    CmdDelete.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";

                    LinkButton CmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    CmdApprove.OnClientClick = "confirmApprove('" + e.Row.Cells[0].Text.ToString() + "'); return false;";
             
                    for (int i = 11; i <= 24; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    CheckBox ChkApproveSelected = (CheckBox)e.Row.FindControl("ChkApproveSelected");
                    if (ChkApproveSelected != null)
                    {
                        ChkApproveSelected.Visible = CanBulkApprove;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void CmdExportWarehouse_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewMutationDeviceWarehouse"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Mutation Device Warehouse", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }

                }
                else
                {
                    div_comment.InnerHtml = "No records found";
                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void CmdExportTechnician_Click(object sender, EventArgs e)
        {
            try
            {
                ClsType clType = new ClsType();
                Recordset Rec = new Recordset();
                string strFullPath = ""; string sMsg = ""; string strFileName = "";
                DateTime dt = DateTime.Now;
                strFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
                strFullPath = Server.MapPath("~/Export//" + strFileName);
                Rec.RecData = Session["RecViewMutationDeviceTechnician"] as System.Data.DataSet;
                if (Rec.RecordCount() > 0)
                {
                    if (clType.ExportToCsvTab(Rec, "Mutation Device Technician", strFullPath.Trim(), 50000, ref sMsg))
                    {
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("content-disposition", "attachment;filename=\"" + strFileName + "\"");
                        Response.TransmitFile(strFullPath);
                        Response.Flush();
                        File.Delete(strFullPath);
                        Response.End();
                    }

                }
                else
                {
                    div_comment.InnerHtml = "No records found";
                }
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
        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; string strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving or mutation warehouse has been failed!! (" + ex.Message + ")</div>";
            }
        }
        protected void CmdYesApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string sErr = "";
                string DeviceID = txtDeviceIDApprove.Value.Trim();

                if (DeviceID != "")
                {
                    if (ExecuteSingleApprovalWithLock(DeviceID, Session["ClsTypeUserID"].ToString(), out sErr))
                    {
                        clear();
                        Open_GridView();
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device mutation technician has been approve successfully!</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve device mutation technician has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Approve device mutation technician has been failed (" + ex.Message + ")</div>";
            }
        }
        private bool ExecuteSingleApprovalWithLock(string deviceId, string userId, out string errorMessage)
        {
            errorMessage = "";
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        AcquireApprovalLock(connection, transaction, "DeviceMutationApproval:" + deviceId);
                        AcquireApprovalLock(connection, transaction, "DeviceMutationApprovalLogId");

                        if (!HasEligibleApprovalRequest(connection, transaction, deviceId))
                        {
                            throw new InvalidOperationException("Device mutation request is no longer eligible for approval.");
                        }

                        using (SqlCommand command = new SqlCommand("dbo.sp_approve_device_mutation_technician", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 120;
                            command.Parameters.Add(new SqlParameter("@deviceid", SqlDbType.VarChar, 10) { Value = deviceId });
                            command.Parameters.Add(new SqlParameter("@usrupd", SqlDbType.VarChar, 50) { Value = userId });
                            command.ExecuteNonQuery();
                        }

                        if (HasEligibleApprovalRequest(connection, transaction, deviceId))
                        {
                            throw new InvalidOperationException("Approval did not complete because the device request is no longer eligible.");
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }

                        errorMessage = ex.Message;
                        return false;
                    }
                }
            }
        }

        private string GetSqlConnectionString()
        {
            string connectionString = Session["ClsTypeDBConnStringSQL"].ToString().Trim();
            int providerStart = connectionString.IndexOf("Provider=", StringComparison.OrdinalIgnoreCase);
            if (providerStart >= 0)
            {
                int providerEnd = connectionString.IndexOf(';', providerStart);
                connectionString = providerEnd >= 0
                    ? connectionString.Remove(providerStart, providerEnd - providerStart + 1)
                    : connectionString.Remove(providerStart);
            }

            return connectionString;
        }

        private void AcquireApprovalLock(SqlConnection connection, SqlTransaction transaction, string resource)
        {
            using (SqlCommand command = new SqlCommand("sys.sp_getapplock", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Resource", SqlDbType.NVarChar, 255) { Value = resource });
                command.Parameters.Add(new SqlParameter("@LockMode", SqlDbType.NVarChar, 32) { Value = "Exclusive" });
                command.Parameters.Add(new SqlParameter("@LockOwner", SqlDbType.NVarChar, 32) { Value = "Transaction" });
                command.Parameters.Add(new SqlParameter("@LockTimeout", SqlDbType.Int) { Value = 0 });
                command.Parameters.Add(new SqlParameter("@DbPrincipal", SqlDbType.NVarChar, 128) { Value = "public" });
                SqlParameter returnValue = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.ExecuteNonQuery();

                if (Convert.ToInt32(returnValue.Value) < 0)
                {
                    throw new InvalidOperationException("Device approval is being processed by another request.");
                }
            }
        }

        private bool HasEligibleApprovalRequest(SqlConnection connection, SqlTransaction transaction, string deviceId)
        {
            const string sql = @"SELECT CASE WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.trx_device_mutation_warehouse AS w WITH (UPDLOCK, HOLDLOCK)
                    INNER JOIN dbo.trx_device_mutation_technician_request AS r WITH (UPDLOCK, HOLDLOCK)
                        ON r.TdwID = w.TdwID
                    WHERE w.DeviceID = @deviceid
                      AND w.Status = 'RG'
                      AND r.Status IN ('DR', 'DM')
                ) THEN 1 ELSE 0 END";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.Add(new SqlParameter("@deviceid", SqlDbType.VarChar, 10) { Value = deviceId });
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }
        protected void CmdApproveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";

                if (!CanBulkApprove)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Access denied!</strong> You are not authorized to bulk approve device mutations.</div>";
                    return;
                }

                List<string> selectedDeviceIds = new List<string>();
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox ChkApproveSelected = row.FindControl("ChkApproveSelected") as CheckBox;
                    if (ChkApproveSelected != null && ChkApproveSelected.Checked && row.Cells.Count > 0)
                    {
                        string deviceId = HttpUtility.HtmlDecode(row.Cells[0].Text).Trim();
                        if (!string.IsNullOrEmpty(deviceId) && deviceId != "&nbsp;" && deviceId.Length <= 10)
                        {
                            selectedDeviceIds.Add(deviceId);
                        }
                    }
                }

                selectedDeviceIds = selectedDeviceIds.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                if (selectedDeviceIds.Count == 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Select at least one device to approve.</div>";
                    return;
                }

                string userId = Convert.ToString(Session["ClsTypeUserID"]).Trim();
                if (string.IsNullOrEmpty(userId))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Login user was not found.</div>";
                    return;
                }

                DataTable result = ExecuteBulkApproval(selectedDeviceIds, userId, Convert.ToString(Session["ClsTypeUserGroupID"]).Trim());
                int successCount = result.AsEnumerable().Count(row => row.Field<bool>("IsSuccess"));
                int failedCount = result.Rows.Count - successCount;

                Open_GridView();
                div_comment.InnerHtml = BuildBulkApprovalMessage(result, successCount, failedCount);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Bulk approval failed (" + HttpUtility.HtmlEncode(ex.Message) + ")</div>";
            }
        }

        private DataTable ExecuteBulkApproval(IEnumerable<string> deviceIds, string userId, string groupId)
        {
            DataTable deviceTable = new DataTable();
            deviceTable.Columns.Add("DeviceID", typeof(string));
            foreach (string deviceId in deviceIds)
            {
                deviceTable.Rows.Add(deviceId);
            }

            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            using (SqlCommand command = new SqlCommand("dbo.sp_approve_device_mutation_technician_bulk", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 120;
                command.Parameters.Add(new SqlParameter("@DeviceIDs", SqlDbType.Structured)
                {
                    TypeName = "dbo.DeviceMutationApprovalListType",
                    Value = deviceTable
                });
                command.Parameters.Add(new SqlParameter("@usrupd", SqlDbType.VarChar, 50) { Value = userId });
                command.Parameters.Add(new SqlParameter("@ClsTypeUserGroupID", SqlDbType.VarChar, 20) { Value = groupId });

                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        private string BuildBulkApprovalMessage(DataTable result, int successCount, int failedCount)
        {
            string message = "<div class='alert " + (failedCount == 0 ? "alert-success" : "alert-warning") + "' role='alert'><strong>Bulk approval completed.</strong> "
                + successCount + " succeeded, " + failedCount + " failed.";

            if (failedCount > 0)
            {
                IEnumerable<string> failures = result.AsEnumerable()
                    .Where(row => !row.Field<bool>("IsSuccess"))
                    .Select(row => HttpUtility.HtmlEncode(row.Field<string>("DeviceID") + ": " + row.Field<string>("ResultMessage")));
                message += "<br/>" + string.Join("<br/>", failures);
            }

            return message + "</div>";
        }
        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                string DeviceID = txtDeviceIDDelete.Value.Trim();
                
                if (DeviceID != "")
                {
                    strSQL = "sp_cancel_device_mutation_technician '" + DeviceID + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Device mutation technician has been remove successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation technician has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation technician has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing device mutation technician has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}