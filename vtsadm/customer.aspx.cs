using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class customer : System.Web.UI.Page
    {
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer '" + txtSearch.Text.Trim() + "'";
                ViewState["RecListCustomerFieldSort"] = "CustID";
                ViewState["RecListCustomerDirSort"] = "ASC";
                Session["RecListCustomer"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewDoc()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer_document '" + txtCustomerID.Text.Trim() + "'";
                Session["RecListCustomerDocument"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDoc);
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewServer()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_customer_servers '" + txtCustomerID.Text.Trim() + "'";
                Session["RecListCustomerServer"] = ClType.Open_GridView(GridView3, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingServer);
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTCUST"))
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
                            Open_GridView();
                            Open_GridViewDoc();
                            Open_GridViewServer();
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
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_marketing");
                ClType.Open_Combos(CmbITS, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_it");
                ClType.Open_Combos(CmbITOutbound, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_customer_it");
                ClType.Open_Combos(CmbSupportAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_list_support_area");
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
                txtBillingAddr.Value = "";
                txtTaxAddr.Value = "";
                txtShippingAddr.Value = "";

                txtPICName1.Text = "";
                txtPICPosition1.Text = "";
                txtPICName2.Text = "";
                txtPICPosition2.Text = "";
                txtPICName3.Text = "";
                txtPICPosition3.Text = "";
                txtPICName4.Text = "";
                txtPICPosition4.Text = "";
                txtOfficePhone1.Text = "";
                txtOfficePhone2.Text = "";
                txtOfficePhone3.Text = "";
                txtOfficePhone4.Text = "";
                txtMobilePhone1.Text = "";
                txtMobilePhone2.Text = "";
                txtMobilePhone3.Text = "";
                txtMobilePhone4.Text = "";
                txtEmail1.Text = "";
                txtEmail2.Text = "";
                txtEmail3.Text = "";
                txtEmail4.Text = "";

                txtCustGroupID.Value = "";
                CmbMarketing.SelectedValue = "[Select]";
                CmbITS.SelectedValue = "[Select]";
                CmbITOutbound.SelectedValue = "[Select]";
                CmbSupportAreaID.SelectedValue = "[Select]";
                CmbBusinessField.SelectedValue = "[Select]";
                CmbBusinessSubField.SelectedValue = "[Select]";
                CmbBusinessSubField.Enabled = false;
                
                CmdUpdateUser.Attributes.Add("style", "display:none");
                admDivCheck.Attributes.Add("style", "display:none");
                divNewUserID.Attributes.Add("style", "display:none");


                txtBusinessFields.Value = "";
                txtVehicleType.Value = "";
                txtOperationalArea.Value = "";

                LblCustID.InnerHtml = "";
                txtCustIDDelete.Value = "";
                txtStatusDelete.Value = "";

                textAttachName.Text = "";
                textAttachDesc.Text = "";
                //textAttachUrl.Text = "";
                txtPeriodStart.Text = "";
                txtPeriodEnd.Text = "";
                txtSyncFms.Value = "";

                Session["ClsTypeAttacment"] = "";

                txtSearch.Text = "";
                CmdSubmit.Text = "Submit";
                txtNewUserID.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtOldPassword.Value = "";
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
                Open_GridViewDoc();
                Open_GridViewServer();
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {

            }
        }
        private bool checkUserPass(ref string sErr)
        {
            bool BoolOK = false;
            try
            {
                if (txtUserID.Text.Trim() != "" && txtUserName.Text.Trim() != "" && txtPassword.Text.Trim() != "" && txtConfirmPassword.Text.Trim() != "")
                {
                    if (txtPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                    {
                        BoolOK = true;
                    }
                    else
                    {
                        sErr = "Password and confirm password should match";
                    }
                }
                else
                {
                    sErr = "Userid, Username, password and confirm password should be filled";
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
            }
            return BoolOK;
        }

        private string getPasswordForSave()
        {
            string sPass = txtPassword.Text.Trim();
            if (sPass == "")
            {
                sPass = txtOldPassword.Value.Trim();
            }
            return sPass;
        }
        private bool checkServer()
        {
            bool BoolOK = false;
            try
            {
                div_comment.InnerHtml = "";
                string sServerID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView3.Rows.Count; i++)
                {
                    sServerID = GridView3.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView3.Rows[i].Cells[3].FindControl("Chk1");
                    if (ChkBox.Checked)
                    {
                        BoolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
            }
            return BoolOK;
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
            if (subId != "" && subId != "[Select]")
            {
                EnsureComboItem(CmbBusinessSubField, subId, subId);
                CmbBusinessSubField.SelectedValue = subId;
            }
            else
            {
                CmbBusinessSubField.SelectedValue = "[Select]";
            }
        }

        private static void EnsureComboItem(DropDownList combo, string value, string text)
        {
            if (combo == null || string.IsNullOrWhiteSpace(value) || value == "[Select]") return;
            if (combo.Items.FindByValue(value) != null) return;
            string label = string.IsNullOrWhiteSpace(text) || text == "&nbsp;" ? value : text;
            combo.Items.Add(new ListItem(label, value));
        }

        private static void SafeSetCombo(DropDownList combo, string value)
        {
            if (combo == null) return;
            string raw = (value ?? "").Trim();
            if (raw == "" || raw == "&nbsp;")
            {
                if (combo.Items.FindByValue("[Select]") != null)
                    combo.SelectedValue = "[Select]";
                return;
            }
            if (combo.Items.FindByValue(raw) != null)
            {
                combo.SelectedValue = raw;
                return;
            }
            if (combo.Items.FindByValue("[Select]") != null)
                combo.SelectedValue = "[Select]";
        }

        private bool validateBusinessFields(ref string sErr, bool requireSubField)
        {
            if (CmbBusinessField.SelectedItem == null
                || CmbBusinessField.SelectedItem.Value == ""
                || CmbBusinessField.SelectedItem.Value == "[Select]")
            {
                sErr = "Business Fields harus di isi";
                return false;
            }

            if (requireSubField)
            {
                if (CmbBusinessSubField.SelectedItem == null
                    || !CmbBusinessSubField.Enabled
                    || CmbBusinessSubField.SelectedItem.Value == ""
                    || CmbBusinessSubField.SelectedItem.Value == "[Select]")
                {
                    sErr = "Sub Business Fields harus di isi";
                    return false;
                }
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

        private void saveattachment(string CustID, string AttachName, string AttachDesc, string AttachUrl, string PeriodStart, string PeriodEnd)
        {
            try
            {
                if (CustID != "")
                {
                    Int32 intAff = 0; String strSQL = ""; string sErr = "";
                    ExecCommand ec = new ExecCommand();

                    strSQL = "sp_insert_customer_attachment '" + CustID + "','" + AttachName + "','" + AttachDesc + "','" + AttachUrl + "','" + PeriodStart + "','" + PeriodEnd + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr);
                }

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
                    if (!validateBusinessFields(ref sErr, true))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed (" + sErr + ")</div>";
                        return;
                    }
                    if (checkServer())
                    {
                        if (checkUserPass(ref sErr))
                        {
                            strSQL = "sp_insert_customer '" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbCustTypeID.SelectedItem.Value.ToString() + "'," +
                                     "'" + CmbIDType.SelectedItem.Value.ToString() + "'," +
                                     "'" + txtIDNo.Text.Trim() + "','" + txtFullName.Text.Trim() + "','" + txtAddress.Value.ToString() + "','" + txtBillingAddr.Value.Trim() + "'," +
                                     "'" + txtTaxAddr.Value.Trim() + "','" + txtShippingAddr.Value.Trim() + "','" + txtPICName1.Text.Trim() + "','" + txtPICPosition1.Text.Trim() + "'," +
                                     "'" + txtPICName2.Text.Trim() + "','" + txtPICPosition2.Text.Trim() + "','" + txtPICName3.Text.Trim() + "','" + txtPICPosition3.Text.Trim() + "'," +
                                     "'" + txtPICName4.Text.Trim() + "','" + txtPICPosition4.Text.Trim() + "','" + txtOfficePhone1.Text.Trim() + "','" + txtOfficePhone2.Text.Trim() + "'," +
                                     "'" + txtOfficePhone3.Text.Trim() + "','" + txtOfficePhone4.Text.Trim() + "','" + txtMobilePhone1.Text.Trim() + "'," +
                                     "'" + txtMobilePhone2.Text.Trim() + "','" + txtMobilePhone3.Text.Trim() + "','" + txtMobilePhone4.Text.Trim() + "'," +
                                     "'" + txtEmail1.Text.Trim() + "','" + txtEmail2.Text.Trim() + "','" + txtEmail3.Text.Trim() + "','" + txtEmail4.Text.Trim() + "'," +
                                     "'" + txtCustGroupID.Value.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','" + txtUserID.Text.Trim() + "'," +
                                             "'" + txtUserName.Text.Trim() + "','" + getPasswordForSave() + "','" + CmbBusinessField.SelectedItem.Value.ToString() + "','" + txtOperationalArea.Value.Trim() + "','" + txtVehicleType.Value.Trim() + "','" + CmbServiceTypeID.SelectedItem.Value.ToString() + "','" + CmbITS.SelectedItem.Value.ToString() + "','" + CmbITOutbound.SelectedItem.Value.ToString() + "','" + CmbSupportAreaID.SelectedItem.Value.ToString() + "','" + txtSyncFms.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "','" + CmbBusinessSubField.SelectedItem.Value.ToString() + "'";
                            if (!ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (sErr.ToUpper().Contains("CUS"))
                                {
                                    saveDoc(sErr);
                                    saveServer(sErr);
                                    strSQL = "sp_insert_interfacing_customer '" + sErr.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "','" + txtUserID.Text.Trim() + "'," +
                                             "'" + txtUserName.Text.Trim() + "','" + getPasswordForSave() + "'";
                                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                    {
                                        clear();
                                        Open_GridView();
                                        Open_GridViewDoc();
                                        Open_GridViewServer();

                                        saveattachment(sErr.Trim(), textAttachName.Text.Trim(), textAttachDesc.Text.Trim(), Session["ClsTypeAttacment"].ToString(), txtPeriodStart.Text.Trim(), txtPeriodEnd.Text.Trim());

                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Customer has been save successfully!</div>";
                                    }
                                    else
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Interfacing saving customer has been failed (" + sErr + ")</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed (" + sErr + ")</div>";
                                }
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Saving customer has been failed (Please select server first)</div>";
                    }
                }
                else
                {
                    if (!validateBusinessFields(ref sErr, false))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update customer has been failed (" + sErr + ")</div>";
                        return;
                    }
                    updateServer(txtCustomerID.Text.Trim());

                    string updSubFieldId = (CmbBusinessSubField.SelectedItem == null) ? "[Select]" : CmbBusinessSubField.SelectedItem.Value.ToString();
                    strSQL = "sp_update_customer '" + txtCustomerID.Text.Trim() + "','" + CmbBranchID.SelectedItem.Value.Trim() + "','" + CmbCustTypeID.SelectedItem.Value.ToString() + "'," +
                             "'" + CmbIDType.SelectedItem.Value.ToString() + "'," +
                             "'" + txtIDNo.Text.Trim() + "','" + txtFullName.Text.Trim() + "','" + txtAddress.Value.ToString() + "','" + txtBillingAddr.Value.Trim() + "'," +
                             "'" + txtTaxAddr.Value.Trim() + "','" + txtShippingAddr.Value.Trim() + "','" + txtPICName1.Text.Trim() + "','" + txtPICPosition1.Text.Trim() + "'," +
                             "'" + txtPICName2.Text.Trim() + "','" + txtPICPosition2.Text.Trim() + "','" + txtPICName3.Text.Trim() + "','" + txtPICPosition3.Text.Trim() + "'," +
                             "'" + txtPICName4.Text.Trim() + "','" + txtPICPosition4.Text.Trim() + "','" + txtOfficePhone1.Text.Trim() + "','" + txtOfficePhone2.Text.Trim() + "'," +
                             "'" + txtOfficePhone3.Text.Trim() + "','" + txtOfficePhone4.Text.Trim() + "','" + txtMobilePhone1.Text.Trim() + "'," +
                             "'" + txtMobilePhone2.Text.Trim() + "','" + txtMobilePhone3.Text.Trim() + "','" + txtMobilePhone4.Text.Trim() + "','" + txtEmail1.Text.Trim() + "'," +
                             "'" + txtEmail2.Text.Trim() + "','" + txtEmail3.Text.Trim() + "','" + txtEmail4.Text.Trim() + "'," +
                             "'" + txtCustGroupID.Value.Trim() + "','" + CmbMarketing.SelectedItem.Value.Trim() + "','" + txtUserID.Text.Trim() + "'," +
                             "'" + txtUserName.Text.Trim() + "','" + getPasswordForSave() + "','" + CmbBusinessField.SelectedItem.Value.ToString() + "','" + txtOperationalArea.Value.Trim() + "','" + txtVehicleType.Value.Trim() + "','" + CmbServiceTypeID.SelectedItem.Value.ToString() + "','" + CmbITS.SelectedItem.Value.ToString() + "','" + CmbITOutbound.SelectedItem.Value.ToString() + "','" + CmbSupportAreaID.SelectedItem.Value.ToString() + "','" + txtSyncFms.Value.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "','" + updSubFieldId + "'";

                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            saveDoc(txtCustomerID.Text.Trim());
                            saveattachment(txtCustomerID.Text.Trim(), textAttachName.Text.Trim(), textAttachDesc.Text.Trim(), Session["ClsTypeAttacment"].ToString(), txtPeriodStart.Text.Trim(), txtPeriodEnd.Text.Trim());
                            clear();
                            Open_GridView();
                            Open_GridViewDoc();
                            Open_GridViewServer();
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
        protected void CmdUpdateUser_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Int32 intAff = 0;
                string sErr = "";
                string strSQL = "";
                ExecCommand ec = new ExecCommand();

                if (txtCustomerID.Text.Trim() != "")
                {
                    if (txtUserID.Text.Trim() != "")
                    {
                        if (txtUserName.Text.Trim() != "")
                        {
                            if (txtPassword.Text.Trim() != "" || txtOldPassword.Value.Trim() != "")
                            {
                                if (txtPassword.Text.Trim() != "" && txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Password and confirm password should match</div>";
                                    return;
                                }

                                string custID = txtCustomerID.Text.Trim().Replace("'", "''");
                                string userID = Session["ClsTypeUserID"].ToString().Trim().Replace("'", "''");
                                string userIDServer = txtUserID.Text.Trim().Replace("'", "''");
                                string userNameServer = txtUserName.Text.Trim().Replace("'", "''");
                                string passServer = getPasswordForSave().Replace("'", "''");
                                string newUserID = txtNewUserID.Text.Trim().Replace("'", "''");

                                strSQL = "sp_update_interfacing_user_customer '" + custID + "','" + userID + "','" + userIDServer + "'," +
                                "'" + userNameServer + "','" + passServer + "','" + newUserID + "'";

                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff == 0)
                                    {
                                        div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> User update executed but no row was affected.</div>";
                                    }
                                    else
                                    {
                                        clear();
                                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Customer has been update successfully!</div>";
                                    }
                                }
                                else
                                {
                                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update customer has been failed (" + sErr + ")</div>";
                                }
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer Password</div>";

                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer User Name</div>";
                        }

                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please fill Customer User ID</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select Customer</div>";

                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Save or update customer has been failed (" + ex.Message + ")</div>";
            }

        }

        protected void saveServer(string sCustID)
        {
            try
            {
                div_comment.InnerHtml = "";
                string sServerID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView3.Rows.Count; i++)
                {

                    sServerID = GridView3.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView3.Rows[i].Cells[3].FindControl("Chk1");

                    strSQL = "sp_insert_customer_server '" + sCustID + "','" + sServerID + "','" + ChkBox.Checked.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void updateServer(string sCustID)
        {
            try
            {
                div_comment.InnerHtml = "";
                string sServerID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView3.Rows.Count; i++)
                {

                    sServerID = GridView3.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView3.Rows[i].Cells[3].FindControl("Chk1");

                    strSQL = "sp_update_customer_server '" + sCustID + "','" + sServerID + "','" + ChkBox.Checked.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";

                    //Debug.WriteLine("DEBUG : " + strSQL);

                    Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void saveDoc(string sCustID)
        {
            try
            {
                div_comment.InnerHtml = "";
                string sDocID = ""; string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    sDocID = GridView1.Rows[i].Cells[0].Text.ToString();
                    CheckBox ChkBox = (CheckBox)GridView1.Rows[i].Cells[2].FindControl("Chk1");
                    strSQL = "sp_insert_customer_document '" + sCustID + "','" + sDocID + "','" + ChkBox.Checked.ToString() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                    Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref iAff);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                ClsType ClType = new ClsType();
                int is_syncfms = 0;  int intAff = 0; string sCustID = ""; string sFullName = ""; string sIDType = ""; string sIDNumber = ""; string sCustTypeID = "";
                string sCustTypeDesc = ""; string sBranchID = ""; string sBranchName = ""; string sBranchAddress = ""; string sAddress = "";
                string sBillingAddr = ""; string sTaxAddress = ""; string sShipmentAddress = ""; string sPICName1 = ""; string sPICPosition1 = "";
                string sPICName2 = ""; string sPICPosition2 = ""; string sPICName3 = ""; string sPICPosition3 = ""; string sPICName4 = "";
                string sPICPosition4 = ""; string sOfficePhone1 = ""; string sOfficePhone2 = ""; string sOfficePhone3 = ""; string sOfficePhone4 = "";
                string sMobilePhone1 = ""; string sMobilePhone2 = ""; string sMobilePhone3 = ""; string sMobilePhone4 = ""; string sEmail1 = "";
                string sEmail2 = ""; string sEmail3 = ""; string sEmail4 = ""; string sCustGroupID = ""; string sMarketingID = "";
                string sUserID_Server = ""; string sUserName_Server = ""; string sPass_Server = ""; string sBusiness = ""; string sOperational = "";
                string sStatus = ""; string sErr = ""; string sVehicleType = "";
                string sAttachName = ""; string sAttachDesc = ""; string sAttachUrl = ""; string sPeriodStart = ""; string sPeriodEnd = ""; string sServiceTypeID = ""; string sITS = ""; string sITOutbound = ""; string sSupportAreaID; string sBusinessField = ""; string sBusinessSubField = ""; string sSyncFms = ""; 

                sCustID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sFullName = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sIDType = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sIDNumber = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sCustTypeDesc = (e.CommandSource as GridView).Rows[iRow].Cells[4].Text.Trim();
                sBranchName = (e.CommandSource as GridView).Rows[iRow].Cells[5].Text.Trim();
                sSyncFms = (e.CommandSource as GridView).Rows[iRow].Cells[6].Text.Trim();
                sStatus = (e.CommandSource as GridView).Rows[iRow].Cells[7].Text.Trim();

                sBranchAddress = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();
                sBranchID = (e.CommandSource as GridView).Rows[iRow].Cells[11].Text.Trim();
                sCustTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[12].Text.Trim();
          
                sAddress = (e.CommandSource as GridView).Rows[iRow].Cells[13].Text.Trim();
                sBillingAddr = (e.CommandSource as GridView).Rows[iRow].Cells[14].Text.Trim();
                sTaxAddress = (e.CommandSource as GridView).Rows[iRow].Cells[15].Text.Trim();
                sShipmentAddress = (e.CommandSource as GridView).Rows[iRow].Cells[16].Text.Trim();
                sPICName1 = (e.CommandSource as GridView).Rows[iRow].Cells[17].Text.Trim();
                sPICPosition1 = (e.CommandSource as GridView).Rows[iRow].Cells[18].Text.Trim();
                sPICName2 = (e.CommandSource as GridView).Rows[iRow].Cells[19].Text.Trim();
                sPICPosition2 = (e.CommandSource as GridView).Rows[iRow].Cells[20].Text.Trim();
                sPICName3 = (e.CommandSource as GridView).Rows[iRow].Cells[21].Text.Trim();
                sPICPosition3 = (e.CommandSource as GridView).Rows[iRow].Cells[22].Text.Trim();
                sPICName4 = (e.CommandSource as GridView).Rows[iRow].Cells[23].Text.Trim();
                sPICPosition4 = (e.CommandSource as GridView).Rows[iRow].Cells[24].Text.Trim();
                sOfficePhone1 = (e.CommandSource as GridView).Rows[iRow].Cells[25].Text.Trim();
                sOfficePhone2 = (e.CommandSource as GridView).Rows[iRow].Cells[26].Text.Trim();
                sOfficePhone3 = (e.CommandSource as GridView).Rows[iRow].Cells[27].Text.Trim();
                sOfficePhone4 = (e.CommandSource as GridView).Rows[iRow].Cells[28].Text.Trim();
                sMobilePhone1 = (e.CommandSource as GridView).Rows[iRow].Cells[29].Text.Trim();
                sMobilePhone2 = (e.CommandSource as GridView).Rows[iRow].Cells[30].Text.Trim();
                sMobilePhone3 = (e.CommandSource as GridView).Rows[iRow].Cells[31].Text.Trim();
                sMobilePhone4 = (e.CommandSource as GridView).Rows[iRow].Cells[32].Text.Trim();
                sEmail1 = (e.CommandSource as GridView).Rows[iRow].Cells[33].Text.Trim();
                sEmail2 = (e.CommandSource as GridView).Rows[iRow].Cells[34].Text.Trim();
                sEmail3 = (e.CommandSource as GridView).Rows[iRow].Cells[35].Text.Trim();
                sEmail4 = (e.CommandSource as GridView).Rows[iRow].Cells[36].Text.Trim();
                sMarketingID = (e.CommandSource as GridView).Rows[iRow].Cells[37].Text.Trim();
                sCustGroupID = (e.CommandSource as GridView).Rows[iRow].Cells[38].Text.Trim();
                sUserID_Server = (e.CommandSource as GridView).Rows[iRow].Cells[39].Text.Trim();
                sUserName_Server = (e.CommandSource as GridView).Rows[iRow].Cells[40].Text.Trim();
                sPass_Server = (e.CommandSource as GridView).Rows[iRow].Cells[41].Text.Trim();
                sBusiness = (e.CommandSource as GridView).Rows[iRow].Cells[42].Text.Trim();
                sOperational = (e.CommandSource as GridView).Rows[iRow].Cells[43].Text.Trim();
                sVehicleType = (e.CommandSource as GridView).Rows[iRow].Cells[44].Text.Trim();

                sAttachName = (e.CommandSource as GridView).Rows[iRow].Cells[45].Text.Trim();
                sAttachDesc = (e.CommandSource as GridView).Rows[iRow].Cells[46].Text.Trim();
                sAttachUrl = (e.CommandSource as GridView).Rows[iRow].Cells[47].Text.Trim();
                sPeriodStart = (e.CommandSource as GridView).Rows[iRow].Cells[48].Text.Trim();
                sPeriodEnd = (e.CommandSource as GridView).Rows[iRow].Cells[49].Text.Trim();
                sServiceTypeID = (e.CommandSource as GridView).Rows[iRow].Cells[50].Text.Trim();
                sITS = (e.CommandSource as GridView).Rows[iRow].Cells[51].Text.Trim();
                sITOutbound = (e.CommandSource as GridView).Rows[iRow].Cells[52].Text.Trim();
                sBusinessField = (e.CommandSource as GridView).Rows[iRow].Cells[53].Text.Trim();
                sSupportAreaID = (e.CommandSource as GridView).Rows[iRow].Cells[54].Text.Trim();
                sBusinessSubField = (e.CommandSource as GridView).Rows[iRow].Cells[55].Text.Trim();
                if (sSyncFms == "YES") {
                    is_syncfms = 1;
                }

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        if (sStatus.ToUpper().Trim() != "DE")
                        {
                            txtCustomerID.Text = sCustID;
                            txtFullName.Text = sFullName;
                            SafeSetCombo(CmbCustTypeID, sCustTypeID);
                            SafeSetCombo(CmbIDType, sIDType);
                            txtIDNo.Text = ClType.CheckNbsp(sIDNumber);
                            SafeSetCombo(CmbBranchID, sBranchID);
                            txtBranchAddress.Text = sBranchAddress;
                            txtAddress.Value = sAddress;
                            txtSyncFms.Value= is_syncfms.ToString();
                            txtBillingAddr.Value = ClType.CheckNbsp(sBillingAddr);
                            txtTaxAddr.Value = ClType.CheckNbsp(sTaxAddress);
                            txtShippingAddr.Value = ClType.CheckNbsp(sShipmentAddress);
                            txtPICName1.Text = ClType.CheckNbsp(sPICName1);
                            txtPICPosition1.Text = ClType.CheckNbsp(sPICPosition1);
                            txtPICName2.Text = ClType.CheckNbsp(sPICName2);
                            txtPICPosition2.Text = ClType.CheckNbsp(sPICPosition2);
                            txtPICName3.Text = ClType.CheckNbsp(sPICName3);
                            txtPICPosition3.Text = ClType.CheckNbsp(sPICPosition3);
                            txtPICName4.Text = ClType.CheckNbsp(sPICName4);
                            txtPICPosition4.Text = ClType.CheckNbsp(sPICPosition4);
                            txtOfficePhone1.Text = ClType.CheckNbsp(sOfficePhone1);
                            txtOfficePhone2.Text = ClType.CheckNbsp(sOfficePhone2);
                            txtOfficePhone3.Text = ClType.CheckNbsp(sOfficePhone3);
                            txtOfficePhone4.Text = ClType.CheckNbsp(sOfficePhone4);
                            txtMobilePhone1.Text = ClType.CheckNbsp(sMobilePhone1);
                            txtMobilePhone2.Text = ClType.CheckNbsp(sMobilePhone2);
                            txtMobilePhone3.Text = ClType.CheckNbsp(sMobilePhone3);
                            txtMobilePhone4.Text = ClType.CheckNbsp(sMobilePhone4);
                            txtEmail1.Text = ClType.CheckNbsp(sEmail1);
                            txtEmail2.Text = ClType.CheckNbsp(sEmail2);
                            txtEmail3.Text = ClType.CheckNbsp(sEmail3);
                            txtEmail4.Text = ClType.CheckNbsp(sEmail4);
                            txtCustGroupID.Value = ClType.CheckNbsp(sCustGroupID);
                            SafeSetCombo(CmbMarketing, ClType.CheckNbsp(sMarketingID));
                            SafeSetCombo(CmbITS, ClType.CheckNbsp(sITS));
                            string bizFieldId = ClType.CheckNbsp(sBusinessField);
                            EnsureComboItem(CmbBusinessField, bizFieldId, ClType.CheckNbsp(sBusiness));
                            SafeSetCombo(CmbBusinessField, bizFieldId);
                            if (CmbBusinessField.SelectedItem == null || CmbBusinessField.SelectedItem.Value == "[Select]")
                                bizFieldId = "[Select]";
                            else
                                bizFieldId = CmbBusinessField.SelectedItem.Value;
                            LoadBusinessSubFieldCombo(bizFieldId, ClType.CheckNbsp(sBusinessSubField));
                            SafeSetCombo(CmbITOutbound, ClType.CheckNbsp(sITOutbound));
                            SafeSetCombo(CmbSupportAreaID, ClType.CheckNbsp(sSupportAreaID));
                            txtUserID.Text = ClType.CheckNbsp(sUserID_Server);
                            txtUserName.Text = ClType.CheckNbsp(sUserName_Server);
                            txtOldPassword.Value = ClType.CheckNbsp(sPass_Server);
                            txtPassword.Text = "";
                            txtConfirmPassword.Text = "";
                            txtNewUserID.Text = "";
                            txtBusinessFields.Value = ClType.CheckNbsp(sBusiness);
                            txtOperationalArea.Value = ClType.CheckNbsp(sOperational);
                            txtVehicleType.Value = ClType.CheckNbsp(sVehicleType);

                            textAttachName.Text = ClType.CheckNbsp(sAttachName);
                            textAttachDesc.Text = ClType.CheckNbsp(sAttachDesc);
                            //textAttachUrl.Text = ClType.CheckNbsp(sAttachUrl);
                            txtPeriodStart.Text = ClType.CheckNbsp(sPeriodStart);
                            txtPeriodEnd.Text = ClType.CheckNbsp(sPeriodEnd);
                            SafeSetCombo(CmbServiceTypeID, sServiceTypeID);

                            CmdUpdateUser.Attributes.Add("style", "display:block");
                            admDivCheck.Attributes.Add("style", "display:block");
                            divNewUserID.Attributes.Add("style", "display:block");

                            Open_GridViewDoc();
                            Open_GridViewServer();
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
                ClType.Open_Combos(CmbMarketing, Session["ClsTypeDBConnStringSQL"].ToString(), CmbBranchID.SelectedItem.Value.ToString(), "sp_list_customer_marketing");
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
                    for (int i = 10; i <= 55; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 10; i <= 55; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[8].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[9].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[7].Text.ToString() + "'); return false;";
                }
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
                    e.Row.Cells[3].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sIsAttach = e.Row.Cells[3].Text.ToString();
                    if (sIsAttach == "1")
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[2].FindControl("Chk1");
                        ChkBox.Checked = true;
                    }
                    else
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[2].FindControl("Chk1");
                        ChkBox.Checked = false;
                    }
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[3].Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sIsAttach = e.Row.Cells[4].Text.ToString();
                    if (sIsAttach == "1")
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = true;
                    }
                    else
                    {
                        CheckBox ChkBox = (CheckBox)e.Row.Cells[3].FindControl("Chk1");
                        ChkBox.Checked = false;
                    }
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[4].Visible = false;
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
                Open_GridViewDoc();
                Open_GridViewServer();
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
                        strSQL = "sp_delete_customer '" + txtCustIDDelete.Value.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                        if (Ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                        {
                            if (intAff > 0)
                            {
                                clear();
                                Open_GridView();
                                Open_GridViewDoc();
                                Open_GridViewServer();
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
    }
}