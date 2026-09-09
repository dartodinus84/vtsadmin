using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class pr_dlo_create_approve : System.Web.UI.Page
    {
        private const string ValidationValid = "VALID";
        string sViewStateFieldSort = "RecDLOApproveFieldSort";
        string sViewStateDirSort = "RecDLOApproveDirSort";
        string sSessionRecList = "RecDLOApprove";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType clType = new ClsType();
                var access = (Session["ClsTypeAccessMenu"] ?? "").ToString().ToUpper();
                if (!access.Contains("MNUPRCREATE") && !access.Contains("MNUPOCREATE"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (clType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
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
            catch
            {
            }
        }

        protected void Open_GridView()
        {
            try
            {
                ClsType clType = new ClsType();
                string strSQL = "sp_get_dlo_approve_summary '" + txtSearch.Text.Trim().Replace("'", "''") + "'";
                ViewState[sViewStateFieldSort] = "DloID";
                ViewState[sViewStateDirSort] = "DESC";
                Session[sSessionRecList] = clType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            }
            catch
            {
            }
        }

        private void clear()
        {
            try
            {
                txtSearch.Text = "";
            }
            catch
            {
            }
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Open_GridView();
                div_comment.InnerHtml = "";
            }
            catch
            {
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            clear();
            Open_GridView();
            div_comment.InnerHtml = "";
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType clType = new ClsType();
            clType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session[sSessionRecList], LblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType clType = new ClsType();
                string sNewDirSort = clType.Gv_Sorting(GridView1, Session[sSessionRecList], ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString(), e.SortExpression);
                ViewState[sViewStateFieldSort] = e.SortExpression.ToString();
                ViewState[sViewStateDirSort] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Sorting data failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton cmdApprove = (LinkButton)e.Row.FindControl("CmdApprove");
                    if (cmdApprove != null && e.Row.Cells.Count > 0)
                    {
                        string sDloID = (e.Row.Cells[0].Text ?? "").Replace("&nbsp;", "").Replace("'", "\\'");
                        cmdApprove.OnClientClick = "confirmApprove('" + sDloID + "'); return false;";
                    }
                }
            }
            catch
            {
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if ((e.CommandName ?? "").ToUpper() != "VIEWDETAIL")
                    return;

                string dloId = (e.CommandArgument ?? "").ToString().Trim();
                if (dloId == "")
                    return;

                LoadDetailModal(dloId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showDloDetail", "$('#modal-detail').modal('show');", true);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Load detail failed (" + ex.Message + ")</div>";
            }
        }

        private void LoadDetailModal(string dloId)
        {
            LblDtlDloNumber.Text = "-";
            LblDtlDloDate.Text = "-";
            LblDtlVendorName.Text = "-";
            LblDtlWarehouseID.Text = "-";
            LblDtlPurID.Text = "-";
            LblDtlStatus.Text = "-";
            LblDtlLastUpdate.Text = "-";
            LblDtlTotalQty.Text = "0";
            LblDtlQtyDone.Text = "0";
            LblDtlQtyRemaining.Text = "0";
            LblDtlTotalLine.Text = "0";
            GridViewDetail.DataSource = null;
            GridViewDetail.DataBind();

            string idSafe = dloId.Replace("'", "''");

            Recordset recHeader = new Recordset();
            recHeader.Open("sp_get_dlo_status_approve '" + idSafe + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            if (recHeader.RecordCount() > 0)
            {
                LblDtlDloNumber.Text = (recHeader.Fields("DloNumber") ?? "-").ToString();
                LblDtlDloDate.Text = (recHeader.Fields("DloDate") ?? "-").ToString();
                LblDtlVendorName.Text = (recHeader.Fields("VendorName") ?? "-").ToString();
                LblDtlWarehouseID.Text = (recHeader.Fields("WarehouseID") ?? "-").ToString();
                LblDtlPurID.Text = (recHeader.Fields("PurID") ?? "-").ToString();
                LblDtlStatus.Text = (recHeader.Fields("Status") ?? "-").ToString();
                string usrUpd = (recHeader.Fields("UsrUpd") ?? "-").ToString();
                string dtmUpd = (recHeader.Fields("DtmUpd") ?? "-").ToString();
                LblDtlLastUpdate.Text = usrUpd + " / " + dtmUpd;
            }

            Recordset recSummary = new Recordset();
            recSummary.Open("sp_get_dlo_approve_summary '" + idSafe + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            if (recSummary.RecordCount() > 0)
            {
                LblDtlTotalQty.Text = (recSummary.Fields("TotalQty") ?? "0").ToString();
                LblDtlQtyDone.Text = (recSummary.Fields("QtyDone") ?? "0").ToString();
                LblDtlQtyRemaining.Text = (recSummary.Fields("QtyRemaining") ?? "0").ToString();
                LblDtlTotalLine.Text = (recSummary.Fields("TotalLine") ?? "0").ToString();
            }

            Recordset recDetail = new Recordset();
            recDetail.Open("sp_list_dlo_approve_detail '" + idSafe + "'", Session["ClsTypeDBConnStringSQL"].ToString());
            if (recDetail.RecData != null && recDetail.RecData.Tables.Count > 0)
                GridViewDetail.DataSource = recDetail.RecData.Tables[0];
            else
                GridViewDetail.DataSource = null;
            GridViewDetail.DataBind();
        }

        protected void CmdYesApprove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string dloId = txtDLOApprove.Value.Trim();
                if (dloId == "") return;

                string strSQLValidate = "sp_validate_dlo_for_approval '" + dloId.Replace("'", "''") + "'";
                Recordset recValidate = new Recordset();
                recValidate.Open(strSQLValidate, Session["ClsTypeDBConnStringSQL"].ToString());
                if (recValidate.RecordCount() <= 0)
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Data DLO tidak ditemukan.</div>";
                    return;
                }

                string validation = "";
                if (recValidate.RecData.Tables[0].Columns.Contains("ValidationResult"))
                    validation = (recValidate.Fields("ValidationResult") ?? "").Trim();
                else if (recValidate.RecData.Tables[0].Columns.Count > 0)
                    validation = (recValidate.Fields(0) ?? "").Trim();

                if (!validation.Equals(ValidationValid, StringComparison.OrdinalIgnoreCase))
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> DLO tidak bisa di-approve (" + validation + ").</div>";
                    return;
                }

                int intAff = 0;
                string sErr = "";
                string strSQL = "sp_submit_dlo_order_approval '" + dloId.Replace("'", "''") + "','" + Session["ClsTypeUserID"].ToString().Replace("'", "''") + "'";
                ExecCommand ec = new ExecCommand();
                if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><strong>Success!</strong> DLO approved successfully.</div>";
                        clear();
                        Open_GridView();
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-info' role='alert'>No data updated. DLO mungkin sudah close.</div>";
                        Open_GridView();
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Approve DLO failed (" + sErr + ")</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><strong>Failed!</strong> Approve DLO failed (" + ex.Message + ")</div>";
            }
        }
    }
}
