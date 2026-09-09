using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class req_procurement_mutation_detail : System.Web.UI.Page
    {
        public string sProcurementID = "";
        public string sCustID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUREQTLS"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                sProcurementID = Session["ProcurementREQID"].ToString();
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            lblMsg.InnerHtml = "";
                            clear();
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
        protected void Open_Combos(DropDownList cmbTemp, string stDBConn, string sSearch, string strSQL)
        {
            try
            {
                Recordset Rec = new Recordset(); string stSQL = ""; ListItem LstItem;
                stSQL = strSQL + " '" + sSearch + "'";
                Rec.Open(stSQL, stDBConn);
                cmbTemp.Items.Clear();
                LstItem = new ListItem();
                LstItem.Text = "[Select]";
                LstItem.Value = "[Select]";
                cmbTemp.Items.Add(LstItem);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        LstItem = new ListItem();
                        LstItem.Text = Rec.Fields(1).Trim();
                        LstItem.Value = Rec.Fields(0).Trim();
                        cmbTemp.Items.Add(LstItem);
                        Rec.MoveNext();
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
                txtRemarks.Text = "";
                txtQuantity.Value = "";
                ClType.Open_Combos(CmbGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_type_procurement");
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_tools");
                CmbGroupID.SelectedValue = "[None]";
                CmbTypeID.SelectedValue = "[None]";
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
                if (sProcurementID.Trim() != "")
                {
                    if (CmbTypeID.SelectedItem.Value.Trim() != "")
                    {
                        if (txtQuantity.Value.Trim() != "" && txtQuantity.Value.Trim() != "0")
                        {
                            Int32 intAff = 0; String strSQL = ""; string sErr = "";
                            ExecCommand ec = new ExecCommand();
                            strSQL = "sp_insert_procurement_detail '" + sProcurementID.Trim() + "','" + CmbGroupID.SelectedItem.Value.Trim() + "','" + CmbTypeID.SelectedItem.Value.Trim() + "','" + txtQuantity.Value.Trim() + "','" + txtRemarks.Text.Trim() + "','" + Session["ClsTypeUserID"].ToString() + "'";
                            if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                            {
                                if (intAff > 0)
                                {
                                    clear();
                                    lblMsg.InnerHtml = "<strong>Success!</strong> Create Request Details";
                                }
                            }
                            else
                            {
                                lblMsg.InnerHtml = "<strong>Failed!</strong> Create Request Details (" + sErr + ")";
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please Quantity less than 0 ";
                        }

                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select tools first";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please Create Request Header first";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> Create Request Details (" + ex.Message + ")";
            }
        }
        protected void CmbGroupID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), CmbGroupID.SelectedItem.Value.ToString(), "sp_list_tools");
                div_comment.InnerHtml = "";
            }
            catch (Exception ex)
            {
            }
        }
        //protected void CmbDeviceTypeID_TextChange(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClsType ClType = new ClsType();
        //        if (Convert.ToString(txtProcurementType.Text.Trim()).Equals("GSM"))
        //        {
        //            CmbGSMProviderID.Attributes.Add("style", "display:block");
        //            LblGSM.Attributes.Add("style", "display:block");
        //            CmbDeviceTypeID.Attributes.Add("style", "display:none");
        //            LblDevice.Attributes.Add("style", "display:none");
        //        }
        //        else
        //        {
        //            CmbGSMProviderID.Attributes.Add("style", "display:none");
        //            LblGSM.Attributes.Add("style", "display:none");
        //            CmbDeviceTypeID.Attributes.Add("style", "display:block");
        //            LblDevice.Attributes.Add("style", "display:block");
        //        }
        //        div_comment.InnerHtml = "";
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

    }
}