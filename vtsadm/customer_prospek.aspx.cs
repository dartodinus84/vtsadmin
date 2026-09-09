using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class customer_prospek : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                //string strSQL = "sp_list_customer_prospek '" + txtSearch.Text.Trim() + "'";
                string strSQL;

                if (Session["ClsTypeUserMarketingID"] != null &&
                        !string.IsNullOrEmpty(Session["ClsTypeUserMarketingID"].ToString()))
                {
                    string userID = Session["ClsTypeUserID"].ToString();
                    strSQL = "sp_list_customer_prospek '" + txtSearch.Text.Trim() + "','" + userID + "'";
                }
                else
                {
                    // User non-marketing: pakai parameter search saja (tanpa filter marketing)
                    strSQL = "sp_list_customer_prospek '" + txtSearch.Text.Trim() + "'";
                }

                ViewState["RecListCustomerFieldSort"] = "CustID";
                ViewState["RecListCustomerDirSort"] = "ASC";
                Session["RecListCustomer"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString());
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTCUSTPRK"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                // FileUpload di dalam UpdatePanel master butuh full postback + multipart.
                Page.Form.Enctype = "multipart/form-data";
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (scriptManager != null)
                {
                    if (CmdUploadExcel != null)
                        scriptManager.RegisterPostBackControl(CmdUploadExcel);
                    if (CmdDownloadTemplate != null)
                        scriptManager.RegisterPostBackControl(CmdDownloadTemplate);
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "";
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

                ClType.Open_Combos(CmbCustTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_customer_type");
                ClType.Open_Combos(CmbIDType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_id_type");
                ClType.Open_Combos(CmbBranchID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_branch");
                ClType.Open_Combos(CmbBusinessField, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_business_field");
                LoadBusinessSubFieldCombo("[Select]");
                ClType.Open_Combos(CmbServiceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_service_type");

                txtCustomerID.Text = "";
                txtFullName.Text = "";
                CmbCustTypeID.SelectedValue = "[Select]";
                CmbIDType.SelectedValue = "[Select]";
                CmbServiceTypeID.SelectedValue = "[Select]";
                txtIDNo.Text = "";
                CmbBranchID.SelectedValue = "[Select]";
                txtBranchAddress.Text = "";
                txtAddress.Value = "";

                txtPICName.Text = "";
                txtPICPosition.Text = "";
                txtOfficePhone.Text = "";
                txtMobilePhone.Text = "";
                txtEmail.Text = "";
                
                CmbBusinessField.SelectedValue = "[Select]";
                CmbBusinessSubField.SelectedValue = "[Select]";
                CmbBusinessSubField.Enabled = false;

                txtVehicleType.Value = "";
                txtOperationalArea.Value = "";

                LblCustID.InnerHtml = "";
                txtCustIDDelete.Value = "";
                txtStatusDelete.Value = "";

                txtSearch.Text = "";
                CmdSubmit.Text = "Submit";
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                clear();
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        
        private void LoadBusinessSubFieldCombo(string businessFieldId, string selectedSubFieldId = "[Select]")
        {
            ClsType ClType = new ClsType();
            string parentId = string.IsNullOrWhiteSpace(businessFieldId) ? "[Select]" : businessFieldId.Trim();
            if (parentId == "" || parentId == "[Select]")
            {
                CmbBusinessSubField.Items.Clear();
                CmbBusinessSubField.Items.Add(new ListItem("[Select]", "[Select]"));
                CmbBusinessSubField.SelectedValue = "[Select]";
                CmbBusinessSubField.Enabled = false;
                return;
            }

            ClType.Open_Combos(CmbBusinessSubField, Session["ClsTypeDBConnStringSQL"].ToString(), parentId, "sp_list_customer_business_sub_field");
            CmbBusinessSubField.Enabled = true;

            string subId = string.IsNullOrWhiteSpace(selectedSubFieldId) ? "[Select]" : ClType.CheckNbsp(selectedSubFieldId.Trim());
            if (subId != "" && subId != "[Select]" && CmbBusinessSubField.Items.FindByValue(subId) != null)
            {
                CmbBusinessSubField.SelectedValue = subId;
            }
            else
            {
                CmbBusinessSubField.SelectedValue = "[Select]";
            }
        }

        private bool validateBusinessFields(ref string sErr)
        {
            if (CmbBusinessField.SelectedItem == null
                || CmbBusinessField.SelectedItem.Value == ""
                || CmbBusinessField.SelectedItem.Value == "[Select]")
            {
                sErr = "Business Fields harus di isi";
                return false;
            }

            if (CmbBusinessSubField.SelectedItem == null
                || !CmbBusinessSubField.Enabled
                || CmbBusinessSubField.SelectedItem.Value == ""
                || CmbBusinessSubField.SelectedItem.Value == "[Select]")
            {
                sErr = "Sub Business Fields harus di isi";
                return false;
            }

            return true;
        }

        protected void CmbBusinessField_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadBusinessSubFieldCombo(CmbBusinessField.SelectedItem.Value.ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                ExecCommand ec = new ExecCommand();
                if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                {
                    if (!validateBusinessFields(ref sErr))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed (" + sErr + ")</div>";
                        return;
                    }
                    strSQL = "sp_insert_customer_prospek '" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbCustTypeID.SelectedItem.Value.ToString() + "'," +
                        "'" + CmbIDType.SelectedItem.Value.ToString() + "','" + txtIDNo.Text.Trim() + "','" + txtFullName.Text.Trim() + "'," +
                        "'" + txtAddress.Value.ToString() + "','" + txtPICName.Text.Trim() + "','" + txtPICPosition.Text.Trim() + "'," +
                        "'" + txtOfficePhone.Text.Trim() + "','" + txtMobilePhone.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + CmbBusinessField.SelectedItem.Value.ToString() +
                        "','" + txtOperationalArea.Value.Trim() + "','" + txtVehicleType.Value.Trim() + "','" + CmbServiceTypeID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "','" + CmbBusinessSubField.SelectedItem.Value.ToString() + "'";

                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Customer has been save successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Interfacing saving customer has been failed (" + sErr + ")</div>";
                    }
                }
                else
                {
                    if (!validateBusinessFields(ref sErr))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update customer has been failed (" + sErr + ")</div>";
                        return;
                    }

                    strSQL = "sp_update_customer_prospek '" + txtCustomerID.Text.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbCustTypeID.SelectedItem.Value.ToString() + "'," +
                        "'" + CmbIDType.SelectedItem.Value.ToString() + "','" + txtIDNo.Text.Trim() + "','" + txtFullName.Text.Trim() + "'," +
                        "'" + txtAddress.Value.ToString() + "','" + txtPICName.Text.Trim() + "','" + txtPICPosition.Text.Trim() + "'," +
                        "'" + txtOfficePhone.Text.Trim() + "','" + txtMobilePhone.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + CmbBusinessField.SelectedItem.Value.ToString() +
                        "','" + txtOperationalArea.Value.Trim() + "','" + txtVehicleType.Value.Trim() + "','" + CmbServiceTypeID.SelectedItem.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "','" + CmbBusinessSubField.SelectedItem.Value.ToString() + "'";

                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {

                        if (intAff > 0)
                        {
                            clear();
                            Open_GridView();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Customer has been update successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update customer has been failed</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update customer has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update customer has been failed (" + ex.Message + ")</div>";
            }
        }
        

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                ClsType ClType = new ClsType();
                int intAff = 0; string sCustID = ""; string sFullName = ""; string sIDType = ""; string sIDNumber = ""; string sCustTypeID = "";
                string sCustTypeDesc = ""; string sBranchID = ""; string sBranchName = ""; string sBranchAddress = ""; string sAddress = "";
                string sPICName = ""; string sPICPosition = "";
                string sOfficePhone = ""; 
                string sMobilePhone = ""; string sEmail = "";
                string sBusiness = ""; string sOperational = "";
                string sStatus = ""; string sErr = ""; string sVehicleType = "";
                string sServiceTypeID = ""; string sBusinessField = ""; string sBusinessSubField = "";

                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sIDType = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sIDNumber = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sCustTypeDesc = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();

                sBranchAddress = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sCustTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sAddress = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
                sPICName = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sPICPosition = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sOfficePhone = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sMobilePhone = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sEmail = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sOperational = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sVehicleType = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sBusiness = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();
                sBusinessField = (e.CommandSource as GridView).Rows[iRow].Cells[21].Text.Trim();
                sServiceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[22].Text.Trim();
                sBusinessSubField = (e.CommandSource as GridView).Rows[iRow].Cells[23].Text.Trim();
                

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE")
                        {
                            txtCustomerID.Text = sCustID;
                            txtFullName.Text = sFullName;
                            CmbCustTypeID.SelectedValue = sCustTypeID;
                            CmbIDType.SelectedValue = sIDType;
                            txtIDNo.Text = ClType.CheckNbsp(sIDNumber);
                            CmbBranchID.SelectedValue = sBranchID;
                            txtBranchAddress.Text = sBranchAddress;
                            txtAddress.Value = sAddress;
                            txtPICName.Text = ClType.CheckNbsp(sPICName);
                            txtPICPosition.Text = ClType.CheckNbsp(sPICPosition);
                            txtOfficePhone.Text = ClType.CheckNbsp(sOfficePhone);
                            txtMobilePhone.Text = ClType.CheckNbsp(sMobilePhone);
                            txtEmail.Text = ClType.CheckNbsp(sEmail);
                            string bizFieldId = ClType.CheckNbsp(sBusinessField);
                            if (CmbBusinessField.Items.FindByValue(bizFieldId) != null)
                            {
                                CmbBusinessField.SelectedValue = bizFieldId;
                            }
                            else
                            {
                                CmbBusinessField.SelectedValue = "[Select]";
                                bizFieldId = "[Select]";
                            }
                            LoadBusinessSubFieldCombo(bizFieldId, ClType.CheckNbsp(sBusinessSubField));
                            txtOperationalArea.Value = ClType.CheckNbsp(sOperational);
                            txtVehicleType.Value = ClType.CheckNbsp(sVehicleType);
                            CmbServiceTypeID.SelectedValue = sServiceTypeID;

                            txtCustomerID.Attributes.Add("disabled", "disabled");
                            CmdSubmit.Text = "Update";
                            div_comment.InnerHtml = "";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Customer can not be edited, due to status code has been " + sStatus + "</div>";
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListCustomer"], LblPaging, ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView2_RowEditing(Object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

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

        protected void CmbBranchID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                setAddress(txtBranchAddress, "sp_get_customer_address_branch '" + CmbBranchID.SelectedItem.Value.ToString() + "'", Session["ClsTypeDBConnStringSQL"].ToString());
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void setAddress(TextBox txtBox, string strSQL, string sDBConn)
        {
            try
            {
                Recordset Rec = new Recordset();
                txtBox.Text = "";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    txtBox.Text = Rec.Fields(0);
                }
            }
            catch (Exception)
            {
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 9; i <= 54; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 9; i <= 54; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[9].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[9].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
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

        protected void CmdYesDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand();
                int intAff = 0; string sErr = "";
                if (txtCustIDDelete.Value.Trim() != "")
                {
                    if (txtStatusDelete.Value.ToUpper().Trim() == "RG")
                    {
                        strSQL = "sp_delete_customer_prospek '" + txtCustIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                               
                                div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Customer has been remove successfully!</div>";
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing customer has been failed!!</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Customer can not be removed, due to status code has been " + txtStatusDelete.Value.Trim() + "</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing customer has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListCustomer"], ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString(), e.SortExpression);
                ViewState["RecListCustomerFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListCustomerDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }

        private class UploadProspekRow
        {
            public int RowNo { get; set; }
            public string BranchID { get; set; }
            public string CustTypeID { get; set; }
            public string IDType { get; set; }
            public string IDNumber { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string PICName { get; set; }
            public string PICPosition { get; set; }
            public string OfficePhone { get; set; }
            public string MobilePhone { get; set; }
            public string Email { get; set; }
            public string BusinessFieldID { get; set; }
            public string BusinessSubFieldID { get; set; }
            public string OperationalArea { get; set; }
            public string VehicleType { get; set; }
            public string ServiceTypeID { get; set; }
            public int IsValid { get; set; }
            public string StatusText { get; set; }
            public string Reason { get; set; }
        }

        private static readonly string[] UploadRequiredHeaders = new[]
        {
            "BRANCHID", "CUSTTYPEID", "IDTYPE", "IDNUMBER", "FULLNAME",
            "BUSINESSFIELDID", "BUSINESSSUBFIELDID", "OPERATIONALAREA", "VEHICLETYPE", "SERVICETYPEID"
        };

        protected void CmdDownloadTemplate_Click(object sender, EventArgs e)
        {
            string templatePath = Server.MapPath("~/Export/Template_Customer_Prospek.xlsx");
            if (!string.IsNullOrEmpty(templatePath) && File.Exists(templatePath))
            {
                SendXlsxFile(templatePath, "Template_Customer_Prospek.xlsx");
                return;
            }

            byte[] fileBytes;
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");
                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("IMPORT");
                    string[] headers =
                    {
                        "BranchID", "CustTypeID", "IDType", "IDNumber", "FullName", "Address",
                        "PICName", "PICPosition", "OfficePhone", "MobilePhone", "Email",
                        "BusinessFieldID", "BusinessSubFieldID", "OperationalArea", "VehicleType", "ServiceTypeID"
                    };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                        ws.Cells[1, i + 1].Style.Font.Bold = true;
                    }

                    ws.Cells[2, 1].Value = FirstComboValue(CmbBranchID);
                    ws.Cells[2, 2].Value = FirstComboValue(CmbCustTypeID);
                    ws.Cells[2, 3].Value = FirstComboValue(CmbIDType);
                    ws.Cells[2, 4].Value = "1234567890";
                    ws.Cells[2, 5].Value = "PT. Contoh Customer";
                    ws.Cells[2, 6].Value = "Jl. Contoh No. 1";
                    ws.Cells[2, 7].Value = "Budi";
                    ws.Cells[2, 8].Value = "Manager";
                    ws.Cells[2, 9].Value = "021123456";
                    ws.Cells[2, 10].Value = "08123456789";
                    ws.Cells[2, 11].Value = "budi@example.com";
                    string sampleFieldId = FirstComboValue(CmbBusinessField);
                    string sampleSubId = FirstSubFieldId(sampleFieldId);

                    ws.Cells[2, 12].Value = sampleFieldId;
                    ws.Cells[2, 13].Value = sampleSubId;
                    ws.Cells[2, 14].Value = "Jakarta";
                    ws.Cells[2, 15].Value = "Truck";
                    ws.Cells[2, 16].Value = FirstComboValue(CmbServiceTypeID);
                    ws.Cells.AutoFitColumns();

                    AddLookupSheet(package, "Branch", CmbBranchID);
                    AddLookupSheet(package, "CustType", CmbCustTypeID);
                    AddLookupSheet(package, "IDType", CmbIDType);
                    AddLookupSheet(package, "BusinessField", CmbBusinessField);
                    AddBusinessSubFieldLookupSheet(package);
                    AddLookupSheet(package, "ServiceType", CmbServiceTypeID);

                    fileBytes = package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                ShowUploadModalWithMessage("Gagal membuat template: " + ex.Message);
                return;
            }

            SendXlsxBytes("Template_Customer_Prospek.xlsx", fileBytes);
        }

        private void SendXlsxFile(string filePath, string fileName)
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                ShowUploadModalWithMessage("Template Excel tidak ditemukan atau kosong di server.");
                return;
            }

            BeginXlsxResponse(fileName, fileInfo.Length);
            Response.TransmitFile(filePath);
            EndXlsxResponse();
        }

        private void SendXlsxBytes(string fileName, byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                ShowUploadModalWithMessage("Template Excel kosong, download dibatalkan.");
                return;
            }

            BeginXlsxResponse(fileName, fileBytes.Length);
            Response.BinaryWrite(fileBytes);
            EndXlsxResponse();
        }

        private void BeginXlsxResponse(string fileName, long contentLength)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.AddHeader("Content-Length", contentLength.ToString());
        }

        private void EndXlsxResponse()
        {
            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static string FirstComboValue(DropDownList combo)
        {
            if (combo == null) return "";
            foreach (ListItem item in combo.Items)
            {
                if (item.Value != "[Select]" && !string.IsNullOrWhiteSpace(item.Value))
                    return item.Value;
            }
            return "";
        }

        private static void AddLookupSheet(ExcelPackage package, string sheetName, DropDownList combo)
        {
            var ws = package.Workbook.Worksheets.Add(sheetName);
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Name";
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 2].Style.Font.Bold = true;

            int row = 2;
            foreach (ListItem item in combo.Items)
            {
                if (item.Value == "[Select]") continue;
                ws.Cells[row, 1].Value = item.Value;
                ws.Cells[row, 2].Value = item.Text;
                row++;
            }
            ws.Cells.AutoFitColumns();
        }

        protected void CmdUploadExcel_Click(object sender, EventArgs e)
        {
            div_comment.InnerHtml = "";
            if (DivUploadError != null)
            {
                DivUploadError.Visible = false;
                DivUploadError.InnerHtml = "";
                DivUploadError.Style["display"] = "none";
            }

            if (FileUploadExcel == null || !FileUploadExcel.HasFile)
            {
                ShowUploadModalWithMessage("Pilih file Excel (.xlsx) terlebih dahulu.");
                return;
            }

            string ext = Path.GetExtension(FileUploadExcel.FileName ?? "").ToLowerInvariant();
            if (ext != ".xlsx")
            {
                ShowUploadModalWithMessage("Hanya file .xlsx yang diperbolehkan.");
                return;
            }

            List<UploadProspekRow> rows;
            try
            {
                using (var stream = new MemoryStream())
                {
                    FileUploadExcel.FileContent.CopyTo(stream);
                    stream.Position = 0;
                    rows = ParseProspekExcel(stream);
                }
            }
            catch (Exception ex)
            {
                ShowUploadModalWithMessage("Gagal membaca Excel: " + ex.Message);
                return;
            }

            if (rows.Count == 0)
            {
                ShowUploadModalWithMessage("Tidak ada data pada sheet. Pastikan header dan isi baris sudah benar.");
                return;
            }

            ValidateUploadRows(rows);

            string userId = Session["ClsTypeUserID"].ToString();
            string conn = Session["ClsTypeDBConnStringSQL"].ToString().Trim();
            ExecCommand ec = new ExecCommand();
            int okCount = 0;
            int errCount = 0;

            foreach (var row in rows)
            {
                if (row.IsValid != 1)
                {
                    errCount++;
                    continue;
                }

                Int32 intAff = 0;
                string sErr = "";
                string strSQL = BuildInsertProspekSql(row, userId);
                if (ec.Execute(strSQL, conn, ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        row.StatusText = "OK";
                        row.Reason = "";
                        okCount++;
                    }
                    else
                    {
                        row.IsValid = 0;
                        row.StatusText = "Error";
                        row.Reason = "Insert failed";
                        errCount++;
                    }
                }
                else
                {
                    row.IsValid = 0;
                    row.StatusText = "Error";
                    row.Reason = string.IsNullOrWhiteSpace(sErr) ? "Insert failed" : sErr;
                    errCount++;
                }
            }

            BindUploadResult(rows);
            Open_GridView();

            string msg = string.Format("Import selesai. Berhasil: {0}, Gagal: {1}.", okCount, errCount);
            string alertClass = errCount > 0 ? "alert-warning" : "alert-success";
            div_comment.InnerHtml = "<div class='alert " + alertClass + "' role='alert'>" + HttpUtility.HtmlEncode(msg) + "</div>";
            ScriptManager.RegisterStartupScript(this, GetType(), "showUploadModalOK" + DateTime.UtcNow.Ticks,
                "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); $('body').css('padding-right',''); setTimeout(function(){ $('#modal-upload-excel').modal('show'); }, 100);", true);
        }

        private List<UploadProspekRow> ParseProspekExcel(Stream stream)
        {
            var rows = new List<UploadProspekRow>();
            ExcelPackage.License.SetNonCommercialOrganization("VTS Admin");
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets["IMPORT"]
                    ?? (package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null);
                if (ws == null || ws.Dimension == null)
                {
                    return rows;
                }

                Dictionary<string, int> headers = BuildHeaderMap(ws);
                foreach (string required in UploadRequiredHeaders)
                {
                    if (!headers.ContainsKey(required))
                    {
                        throw new Exception("Header " + required + " tidak ditemukan pada baris 1.");
                    }
                }

                int rowNo = 1;
                for (int r = 2; r <= ws.Dimension.End.Row; r++)
                {
                    if (IsExcelRowEmpty(ws, r)) continue;

                    var row = new UploadProspekRow
                    {
                        RowNo = rowNo++,
                        BranchID = GetCell(ws, r, headers, "BRANCHID"),
                        CustTypeID = GetCell(ws, r, headers, "CUSTTYPEID"),
                        IDType = GetCell(ws, r, headers, "IDTYPE"),
                        IDNumber = GetCell(ws, r, headers, "IDNUMBER"),
                        FullName = GetCell(ws, r, headers, "FULLNAME"),
                        Address = GetCell(ws, r, headers, "ADDRESS"),
                        PICName = GetCell(ws, r, headers, "PICNAME"),
                        PICPosition = GetCell(ws, r, headers, "PICPOSITION"),
                        OfficePhone = GetCell(ws, r, headers, "OFFICEPHONE"),
                        MobilePhone = GetCell(ws, r, headers, "MOBILEPHONE"),
                        Email = GetCell(ws, r, headers, "EMAIL"),
                        BusinessFieldID = GetCell(ws, r, headers, "BUSINESSFIELDID"),
                        BusinessSubFieldID = GetCell(ws, r, headers, "BUSINESSSUBFIELDID"),
                        OperationalArea = GetCell(ws, r, headers, "OPERATIONALAREA"),
                        VehicleType = GetCell(ws, r, headers, "VEHICLETYPE"),
                        ServiceTypeID = GetCell(ws, r, headers, "SERVICETYPEID"),
                        IsValid = 1,
                        StatusText = "OK",
                        Reason = ""
                    };
                    rows.Add(row);
                }
            }
            return rows;
        }

        private void ValidateUploadRows(List<UploadProspekRow> rows)
        {
            var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenIdNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var reasons = new List<string>();

                row.BranchID = ResolveComboValue(CmbBranchID, row.BranchID);
                row.CustTypeID = ResolveComboValue(CmbCustTypeID, row.CustTypeID);
                row.IDType = ResolveComboValue(CmbIDType, row.IDType);
                row.ServiceTypeID = ResolveComboValue(CmbServiceTypeID, row.ServiceTypeID);
                row.BusinessFieldID = ResolveComboValue(CmbBusinessField, row.BusinessFieldID);
                row.BusinessSubFieldID = ResolveSubFieldValue(row.BusinessFieldID, row.BusinessSubFieldID);

                if (string.IsNullOrWhiteSpace(row.FullName))
                    reasons.Add("FullName wajib");
                if (string.IsNullOrWhiteSpace(row.BranchID) || !ComboHasValue(CmbBranchID, row.BranchID))
                    reasons.Add("BranchID tidak valid");
                if (string.IsNullOrWhiteSpace(row.CustTypeID) || !ComboHasValue(CmbCustTypeID, row.CustTypeID))
                    reasons.Add("CustTypeID tidak valid");
                if (string.IsNullOrWhiteSpace(row.IDType) || !ComboHasValue(CmbIDType, row.IDType))
                    reasons.Add("IDType tidak valid");
                if (string.IsNullOrWhiteSpace(row.IDNumber))
                    reasons.Add("IDNumber wajib");
                if (string.IsNullOrWhiteSpace(row.OperationalArea))
                    reasons.Add("OperationalArea wajib");
                if (string.IsNullOrWhiteSpace(row.VehicleType))
                    reasons.Add("VehicleType wajib");
                if (string.IsNullOrWhiteSpace(row.ServiceTypeID) || !ComboHasValue(CmbServiceTypeID, row.ServiceTypeID))
                    reasons.Add("ServiceTypeID tidak valid");

                if (string.IsNullOrWhiteSpace(row.BusinessFieldID) || row.BusinessFieldID == "[Select]"
                    || !ComboHasValue(CmbBusinessField, row.BusinessFieldID))
                {
                    reasons.Add("BusinessFieldID tidak valid");
                }

                if (string.IsNullOrWhiteSpace(row.BusinessSubFieldID) || row.BusinessSubFieldID == "[Select]")
                {
                    reasons.Add("BusinessSubFieldID wajib");
                }
                else if (ComboHasValue(CmbBusinessField, row.BusinessFieldID)
                    && !SubFieldBelongsToField(row.BusinessFieldID, row.BusinessSubFieldID))
                {
                    reasons.Add("BusinessSubFieldID tidak valid untuk BusinessFieldID");
                }

                if (!string.IsNullOrWhiteSpace(row.FullName) && !seenNames.Add(row.FullName.Trim()))
                    reasons.Add("Duplikat FullName di file");
                if (!string.IsNullOrWhiteSpace(row.IDNumber) && !seenIdNumbers.Add(row.IDNumber.Trim()))
                    reasons.Add("Duplikat IDNumber di file");

                if (reasons.Count > 0)
                {
                    row.IsValid = 0;
                    row.StatusText = "Error";
                    row.Reason = string.Join("; ", reasons);
                }
                else
                {
                    row.IsValid = 1;
                    row.StatusText = "OK";
                    row.Reason = "";
                }
            }
        }

        private static bool ComboHasValue(DropDownList combo, string value)
        {
            if (combo == null || string.IsNullOrWhiteSpace(value)) return false;
            return combo.Items.FindByValue(value.Trim()) != null
                && value.Trim() != "[Select]";
        }

        private static string ResolveComboValue(DropDownList combo, string input)
        {
            if (combo == null || string.IsNullOrWhiteSpace(input)) return (input ?? "").Trim();
            string raw = input.Trim();
            if (raw == "[Select]") return raw;
            ListItem byValue = combo.Items.FindByValue(raw);
            if (byValue != null && byValue.Value != "[Select]") return byValue.Value;
            foreach (ListItem item in combo.Items)
            {
                if (item.Value != "[Select]" && string.Equals((item.Text ?? "").Trim(), raw, StringComparison.OrdinalIgnoreCase))
                    return item.Value;
            }
            return raw;
        }

        private class SubFieldLookup
        {
            public HashSet<string> Ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public Dictionary<string, string> NameToId = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public List<KeyValuePair<string, string>> Items = new List<KeyValuePair<string, string>>();
        }

        private Dictionary<string, SubFieldLookup> _subFieldMap;

        private SubFieldLookup GetSubFieldLookup(string fieldId)
        {
            if (_subFieldMap == null)
                _subFieldMap = new Dictionary<string, SubFieldLookup>(StringComparer.OrdinalIgnoreCase);

            string parentId = (fieldId ?? "").Trim();
            if (parentId == "" || parentId == "[Select]")
                return new SubFieldLookup();

            SubFieldLookup lookup;
            if (_subFieldMap.TryGetValue(parentId, out lookup))
                return lookup;

            lookup = new SubFieldLookup();
            DropDownList tmp = new DropDownList();
            ClsType clType = new ClsType();
            clType.Open_Combos(tmp, Session["ClsTypeDBConnStringSQL"].ToString(), parentId, "sp_list_customer_business_sub_field");
            foreach (ListItem item in tmp.Items)
            {
                if (item.Value == "[Select]" || string.IsNullOrWhiteSpace(item.Value)) continue;
                string id = item.Value.Trim();
                lookup.Ids.Add(id);
                string name = (item.Text ?? "").Trim();
                lookup.Items.Add(new KeyValuePair<string, string>(id, name));
                if (name != "" && !lookup.NameToId.ContainsKey(name))
                    lookup.NameToId[name] = id;
            }
            _subFieldMap[parentId] = lookup;
            return lookup;
        }

        private string ResolveSubFieldValue(string fieldId, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return (input ?? "").Trim();
            string raw = input.Trim();
            if (raw == "[Select]" || !ComboHasValue(CmbBusinessField, fieldId)) return raw;

            SubFieldLookup lookup = GetSubFieldLookup(fieldId);
            if (lookup.Ids.Contains(raw)) return raw;
            string mapped;
            if (lookup.NameToId.TryGetValue(raw, out mapped)) return mapped;
            return raw;
        }

        private bool SubFieldBelongsToField(string fieldId, string subId)
        {
            if (string.IsNullOrWhiteSpace(fieldId) || string.IsNullOrWhiteSpace(subId)) return false;
            return GetSubFieldLookup(fieldId).Ids.Contains(subId.Trim());
        }

        private string FirstSubFieldId(string fieldId)
        {
            foreach (string id in GetSubFieldLookup(fieldId).Ids)
                return id;
            return "";
        }

        private void AddBusinessSubFieldLookupSheet(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("BusinessSubField");
            ws.Cells[1, 1].Value = "BusinessSubFieldID";
            ws.Cells[1, 2].Value = "BusinessSubFieldName";
            ws.Cells[1, 3].Value = "BusinessFieldID";
            ws.Cells[1, 4].Value = "BusinessFieldName";
            for (int c = 1; c <= 4; c++)
                ws.Cells[1, c].Style.Font.Bold = true;

            int row = 2;
            foreach (ListItem field in CmbBusinessField.Items)
            {
                if (field.Value == "[Select]" || string.IsNullOrWhiteSpace(field.Value)) continue;
                foreach (KeyValuePair<string, string> sub in GetSubFieldLookup(field.Value).Items)
                {
                    ws.Cells[row, 1].Value = sub.Key;
                    ws.Cells[row, 2].Value = sub.Value;
                    ws.Cells[row, 3].Value = field.Value;
                    ws.Cells[row, 4].Value = field.Text;
                    row++;
                }
            }
            ws.Cells.AutoFitColumns();
        }

        private string BuildInsertProspekSql(UploadProspekRow row, string userId)
        {
            return "sp_insert_customer_prospek '" + SqlText(row.BranchID) + "','" + SqlText(row.CustTypeID) + "'," +
                "'" + SqlText(row.IDType) + "','" + SqlText(row.IDNumber) + "','" + SqlText(row.FullName) + "'," +
                "'" + SqlText(row.Address) + "','" + SqlText(row.PICName) + "','" + SqlText(row.PICPosition) + "'," +
                "'" + SqlText(row.OfficePhone) + "','" + SqlText(row.MobilePhone) + "','" + SqlText(row.Email) + "'," +
                "'" + SqlText(row.BusinessFieldID) + "','" + SqlText(row.OperationalArea) + "','" + SqlText(row.VehicleType) + "'," +
                "'" + SqlText(row.ServiceTypeID) + "','" + SqlText(userId) + "','" + SqlText(row.BusinessSubFieldID) + "'";
        }

        private static string SqlText(string value)
        {
            return (value ?? "").Replace("'", "''");
        }

        private static Dictionary<string, int> BuildHeaderMap(ExcelWorksheet ws)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int col = 1; col <= ws.Dimension.End.Column; col++)
            {
                string header = (ws.Cells[1, col].Text ?? "").Trim().ToUpperInvariant();
                if (!string.IsNullOrEmpty(header) && !map.ContainsKey(header))
                    map[header] = col;
            }
            return map;
        }

        private static string GetCell(ExcelWorksheet ws, int row, Dictionary<string, int> headers, string key)
        {
            if (!headers.ContainsKey(key)) return "";
            return (ws.Cells[row, headers[key]].Text ?? "").Trim();
        }

        private static bool IsExcelRowEmpty(ExcelWorksheet ws, int row)
        {
            for (int col = 1; col <= ws.Dimension.End.Column; col++)
            {
                if (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Text))
                    return false;
            }
            return true;
        }

        private void BindUploadResult(List<UploadProspekRow> rows)
        {
            var dt = new DataTable();
            dt.Columns.Add("RowNo", typeof(int));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("IDNumber", typeof(string));
            dt.Columns.Add("BusinessFieldID", typeof(string));
            dt.Columns.Add("BusinessSubFieldID", typeof(string));
            dt.Columns.Add("IsValid", typeof(int));
            dt.Columns.Add("StatusText", typeof(string));
            dt.Columns.Add("Reason", typeof(string));

            foreach (var row in rows)
            {
                dt.Rows.Add(row.RowNo, row.FullName, row.IDNumber, row.BusinessFieldID, row.BusinessSubFieldID, row.IsValid, row.StatusText, row.Reason);
            }

            GridViewUploadResult.DataSource = dt;
            GridViewUploadResult.DataBind();
        }

        private void ShowUploadModalWithMessage(string errorMsg)
        {
            if (DivUploadError != null)
            {
                DivUploadError.Visible = !string.IsNullOrEmpty(errorMsg);
                DivUploadError.InnerHtml = !string.IsNullOrEmpty(errorMsg) ? HttpUtility.HtmlEncode(errorMsg) : "";
                DivUploadError.Style["display"] = DivUploadError.Visible ? "block" : "none";
            }
            div_comment.InnerHtml = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "showUploadModal" + DateTime.UtcNow.Ticks,
                "$('.modal-backdrop').remove(); $('body').removeClass('modal-open'); $('body').css('padding-right',''); setTimeout(function(){ $('#modal-upload-excel').modal('show'); }, 100);", true);
        }

        protected void GridViewUploadResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (drv == null) return;
            int isValid = Convert.ToInt32(drv["IsValid"] ?? 0);
            e.Row.ForeColor = isValid == 1
                ? System.Drawing.Color.DarkGreen
                : System.Drawing.Color.DarkRed;
        }
    }
}