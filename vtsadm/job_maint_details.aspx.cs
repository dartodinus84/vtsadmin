using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class job_maint_details : System.Web.UI.Page
    {
        public string sJobID = "";
        public string sCustID = "";
        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_job_maint_vehicle '" + sCustID + "','" + txtSearch.Text.Trim() + "'";
                Session["RecListJobMaintDetails"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBMAINT"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                sJobID = Session["ClsJobIDMaint"].ToString();
                sCustID = Session["ClsCustIDMaint"].ToString();
                //sCustID = Session["ClsJobOrderCustID"].ToString();
                //Session["ClsTypeMenuActive"] = "MNUDO";
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
                txtTvdID.Value = "";
                txtPoliceNo.Text = "";
                Open_Combos(CmbMaintTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), sCustID, "sp_list_job_order_maint_type");
                CmbMaintTypeID.SelectedValue = "[Select]";
                ddlRemarkSuspended.SelectedValue = "[Select]";
                ddlRemarkUninstalling.SelectedValue = "[Select]";
                txtRemark.Text = "";
                txtReason.Text = "";

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
                if (sJobID.Trim() != "")
                {
                    if (txtTvdID.Value.Trim() != "")
                    {
                        if (CmbMaintTypeID.SelectedItem.Value.Trim() != "[Select]")
                        {
                            // Validasi untuk tipe maintenance Suspended dan Uninstalling
                            string selectedMaintType = CmbMaintTypeID.SelectedItem.Value.Trim();
                            bool isValid = true;
                            string remarkValue = "";

                            // Periksa jika tipe maintenance adalah Suspended
                            if (selectedMaintType == "MTY0000011")
                            {
                                // Pastikan remark sudah dipilih dari dropdown Suspended
                                if (ddlRemarkSuspended.SelectedItem.Value == "[Select]")
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please select remark for Suspended maintenance type";
                                    isValid = false;
                                }
                                else
                                {
                                    remarkValue = ddlRemarkSuspended.SelectedItem.Value;
                                }
                            }
                            // Periksa jika tipe maintenance adalah Uninstalling
                            else if (selectedMaintType == "MTY0000006")
                            {
                                // Pastikan remark sudah dipilih dari dropdown Uninstalling
                                if (ddlRemarkUninstalling.SelectedItem.Value == "[Select]")
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please select remark for Uninstalling maintenance type";
                                    isValid = false;
                                }
                                else
                                {
                                    remarkValue = ddlRemarkUninstalling.SelectedItem.Value;
                                }
                            }
                            else
                            {
                                // Untuk tipe maintenance lainnya, gunakan nilai dari TextBox
                                remarkValue = txtReason.Text.Trim();
                            }

                            // Lanjutkan proses penyimpanan jika validasi berhasil
                            if (isValid)
                            {
                                Int32 intAff = 0; String strSQL = ""; string sErr = "";
                                ExecCommand ec = new ExecCommand();

                                // Gunakan remarkValue yang sudah ditentukan sesuai tipe maintenance
                                strSQL = "sp_insert_job_order_maint_detail '" + sJobID.Trim() + "','" +
                                        txtTvdID.Value.Trim() + "','" +
                                        selectedMaintType + "','" +
                                        remarkValue + "','" +
                                        txtRemark.Text.Trim() + "','" +
                                        Session["ClsTypeUserID"].ToString() + "'";

                                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                                {
                                    if (intAff > 0)
                                    {
                                        clear();
                                        Open_GridView();
                                        lblMsg.InnerHtml = "<strong>Success!</strong> Create job maintenance";
                                    }
                                }
                                else
                                {
                                    lblMsg.InnerHtml = "<strong>Failed!</strong> Create job maintenance (" + sErr + ")";
                                }
                            }
                        }
                        else
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Please select maintenance type";
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Please select vehicle first";
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
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintDetails"], LblPaging);
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
                    CmdButton.OnClientClick = "postDetails('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "'," +
                                              "'" + e.Row.Cells[2].Text.ToString() + "');return false;";
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}