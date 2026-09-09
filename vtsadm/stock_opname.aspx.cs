using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class stock_opname : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                // Validasi akses menu
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUSTOCKOPNAME"))
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
                            Open_GridViewHeader();
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
                else
                {
                    // Jika postback dan opname ID ada, refresh data detail
                    if (!string.IsNullOrEmpty(txtOpnameID.Text))
                    {
                        Open_GridView();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        private void clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                txtLocationID.Value = "";
                txtLocationName.Text = "";
                CmbLocationType.SelectedValue = "[Select]";
                txtOpnameID.Text = "";
                txtOpnameDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtRemark.Text = "";
                txtSearch.Value = "";
                txtOpnameIDDelete.Value = "";
                CmdSubmit.Text = "Submit";
                Button2.Attributes.Remove("disabled");
                CmdCreate.Visible = true;
                CmdAddDetail.Visible = false;
                CmdLoad.Visible = false;
                CmdSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void Open_GridViewHeader()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_stock_opname_device_header '" + txtSearch.Value.Trim() + "'";
                ViewState["RecStockOpnameHeaderFieldSort"] = "opname_id";
                ViewState["RecStockOpnameHeaderDirSort"] = "DESC";
                Session["RecStockOpnameHeader"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingHeader, ViewState["RecStockOpnameHeaderFieldSort"].ToString(), ViewState["RecStockOpnameHeaderDirSort"].ToString());
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                string strSQL = "sp_list_stock_opname_device_detail '" + txtOpnameID.Text.Trim() + "'";
                ViewState["RecStockOpnameDetailFieldSort"] = "detail_id";
                ViewState["RecStockOpnameDetailDirSort"] = "DESC";

                // Debugging untuk memeriksa parameter
                System.Diagnostics.Debug.WriteLine("Open_GridView called with OpnameID: " + txtOpnameID.Text.Trim());

                Session["RecStockOpnameDetail"] = ClType.Open_GridView(GridView2, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDetail, ViewState["RecStockOpnameDetailFieldSort"].ToString(), ViewState["RecStockOpnameDetailDirSort"].ToString());

                // Debugging untuk memeriksa hasil query
                if (GridView2.Rows.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("GridView2 populated with " + GridView2.Rows.Count + " rows");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("GridView2 has no rows");

                    // Query tambahan untuk debug
                    Recordset Rec = new Recordset();
                    string queryCheck = "SELECT COUNT(*) as total FROM trx_stock_opname_device_detail WHERE opname_id = '" + txtOpnameID.Text.Trim() + "' AND is_deleted = 0";
                    string errMsg = "";
                    Rec.Open(queryCheck, Session["ClsTypeDBConnStringSQL"].ToString(), ref errMsg);
                    if (Rec.RecordCount() > 0)
                    {
                        Rec.MoveFirst();
                        int count = Convert.ToInt32(Rec.Fields("total"));
                        System.Diagnostics.Debug.WriteLine("Database count check: " + count + " records found for opname_id " + txtOpnameID.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
                System.Diagnostics.Debug.WriteLine("Error in Open_GridView: " + ex.Message);
            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                clear();
                Open_GridView();
                Open_GridViewHeader();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                if (txtLocationID.Value.Trim() != "")
                {
                    if (CmbLocationType.SelectedItem.Value.Trim() != "[Select]")
                    {
                        if (txtOpnameDate.Value.Trim() != "")
                        {
                            Int32 intAff = 0; String strSQL = ""; string sErr = "";
                            ExecCommand ec = new ExecCommand();
                            strSQL = "sp_insert_stock_opname_device_header '" +
                                     txtOpnameDate.Value.Trim() + "','" +
                                     CmbLocationType.SelectedItem.Value.Trim() + "','" +
                                     txtLocationID.Value.Trim() + "','" +
                                     txtRemark.Text.Trim() + "','" +
                                     Session["ClsTypeUserID"].ToString() + "'";

                            Recordset Rec = new Recordset();
                            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);
                            if (Rec.RecordCount() > 0)
                            {
                                Rec.MoveFirst();
                                txtOpnameID.Text = Rec.Fields("opname_id").ToString();
                                Session["ClsOpnameID"] = txtOpnameID.Text.Trim();
                                Session["ClsLocationType"] = CmbLocationType.SelectedItem.Value.Trim();
                                Session["ClsLocationID"] = txtLocationID.Value.Trim();
                                CmdCreate.Visible = false;
                                CmdAddDetail.Visible = true;
                                CmdSubmit.Visible = true;
                                CmdLoad.Visible = true;

                                // Setelah membuat header, panggil Open_GridView untuk menginisialisasi GridView2
                                Open_GridView();
                            }
                            else
                            {
                                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Create stock opname header has been failed (" + sErr + ")</div>";
                            }
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select opname date</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select location type</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Please select location</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Open_GridView();
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdYesSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 intAff = 0; String strSQL = ""; string sErr = "";

                int opnameId = 0;
                if (int.TryParse(txtOpnameID.Text.Trim(), out opnameId))
                {
                    if (CmdSubmit.Text.ToUpper() == "SUBMIT")
                    {
                        strSQL = "sp_submit_stock_opname_device " + opnameId + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                        ExecCommand ec = new ExecCommand();
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            clear();
                            Open_GridView();
                            Open_GridViewHeader();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Submit stock opname has been successfully</div>";
                            Session["ClsOpnameID"] = "";
                            CmdCreate.Visible = true;
                            CmdAddDetail.Visible = false;
                            CmdSubmit.Visible = false;
                            CmdLoad.Visible = false;
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Submit stock opname has been failed (" + sErr + ")</div>";
                        }
                    }
                    else
                    {
                        strSQL = "sp_update_stock_opname_device_header " +
                                 opnameId + ",'" +
                                 txtOpnameDate.Value.Trim() + "','" +
                                 CmbLocationType.SelectedItem.Value.Trim() + "','" +
                                 txtLocationID.Value.Trim() + "','" +
                                 txtRemark.Text.Trim() + "','" +
                                 Session["ClsTypeUserID"].ToString() + "'";

                        ExecCommand ec = new ExecCommand();
                        if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString().Trim(), ref intAff, ref sErr))
                        {
                            clear();
                            Open_GridView();
                            Open_GridViewHeader();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Update stock opname has been successfully</div>";
                            Session["ClsOpnameID"] = "";
                            CmdCreate.Visible = true;
                            CmdAddDetail.Visible = false;
                            CmdSubmit.Visible = false;
                            CmdLoad.Visible = false;
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Update stock opname has been failed (" + sErr + ")</div>";
                        }
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Invalid Opname ID</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Dapatkan detail ID dari baris yang dihapus
                string detailId = GridView2.DataKeys[e.RowIndex].Value.ToString();
                txtDetailIDDelete.Value = detailId;

                // Panggil fungsi penghapusan
                DeleteDetail(detailId);
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
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
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecStockOpnameHeader"], LblPagingHeader, ViewState["RecStockOpnameHeaderFieldSort"].ToString(), ViewState["RecStockOpnameHeaderDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // Handle row editing
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 9; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "'); return false;";
                    for (int i = 9; i <= 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string strSQL = ""; ExecCommand ec = new ExecCommand(); int intAff = 0; string sErr = "";
                if (txtOpnameIDDelete.Value != "")
                {
                    strSQL = "sp_delete_stock_opname_device_header " + txtOpnameIDDelete.Value.Trim() + ",'" + Session["ClsTypeUserID"].ToString() + "'";
                    if (ec.Execute(strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr))
                    {
                        if (intAff > 0)
                        {
                            clear();
                            Open_GridViewHeader();
                            div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Stock opname has been removed successfully!</div>";
                        }
                        else
                        {
                            div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing stock opname has been failed!!</div>";
                        }
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing stock opname has been failed (" + sErr + ")</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing stock opname has been failed (" + ex.Message + ")</div>";
            }
        }

        // Fungsi untuk menghapus detail - DIPERBARUI
        private void DeleteDetail(string detailId)
        {
            try
            {
                int opnameId = 0;
                int detailIdInt = 0;
                string sErr = "";
                string userId = Session["ClsTypeUserID"]?.ToString() ?? "SYSTEM";

                if (int.TryParse(txtOpnameID.Text.Trim(), out opnameId) &&
                    int.TryParse(detailId, out detailIdInt))
                {
                    System.Diagnostics.Debug.WriteLine("Deleting detail - OpnameID: " + opnameId + ", DetailID: " + detailIdInt);

                    // Cek apakah data ada sebelum dihapus (tambahkan is_deleted = 0)
                    string checkSQL = "SELECT COUNT(*) as cnt FROM trx_stock_opname_device_detail " +
                                     "WHERE opname_id = " + opnameId + " AND detail_id = " + detailIdInt + " AND is_deleted = 0";

                    Recordset recBefore = new Recordset();
                    recBefore.Open(checkSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);

                    int countBefore = 0;
                    if (recBefore.RecordCount() > 0)
                    {
                        recBefore.MoveFirst();
                        countBefore = Convert.ToInt32(recBefore.Fields("cnt"));
                    }

                    // Gunakan stored procedure untuk soft delete
                    string deleteSQL = "sp_delete_stock_opname_device_detail " + opnameId + "," + detailIdInt + ",'" + userId + "'";

                    ExecCommand ec = new ExecCommand();
                    int intAff = 0;
                    bool execResult = ec.Execute(deleteSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref intAff, ref sErr);

                    // Cek apakah data masih ada setelah dihapus
                    Recordset recAfter = new Recordset();
                    recAfter.Open(checkSQL, Session["ClsTypeDBConnStringSQL"].ToString(), ref sErr);

                    int countAfter = 0;
                    if (recAfter.RecordCount() > 0)
                    {
                        recAfter.MoveFirst();
                        countAfter = Convert.ToInt32(recAfter.Fields("cnt"));
                    }

                    // Refresh GridView
                    Open_GridView();

                    // Tentukan hasil operasi
                    if (countBefore > countAfter)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Success!</strong> Stock opname detail has been removed successfully!</div>";
                    }
                    else if (countBefore == 0)
                    {
                        div_comment.InnerHtml = "<div class='alert alert-warning' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Warning!</strong> Record not found.</div>";
                    }
                    else if (!string.IsNullOrEmpty(sErr))
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + sErr + "</div>";
                    }
                    else
                    {
                        div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Removing stock opname detail has been failed! No rows affected.</div>";
                    }
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Invalid opname ID or detail ID format</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Stock opname detail can not be deleted, (" + ex.Message + ")</div>";
            }
        }

        protected void CmdYesDetail_ServerClick(object sender, EventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                string detailId = txtDetailIDDelete.Value.Trim();

                if (!string.IsNullOrEmpty(detailId))
                {
                    DeleteDetail(detailId);
                }
                else
                {
                    div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Detail ID is empty</div>";
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Stock opname detail can not be deleted, (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Handle row deleting
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 7; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    LinkButton CmdButton = (LinkButton)e.Row.Cells[6].FindControl("CmdDeleteDetail");
                    string detailId = e.Row.Cells[0].Text.ToString();
                    CmdButton.OnClientClick = "confirmDeleteDetail('" + detailId + "'); return false;";

                    // Warnai baris sesuai status match
                    string statusMatch = e.Row.Cells[11].Text;
                    if (statusMatch == "Mismatch")
                    {
                        e.Row.CssClass = "danger";
                    }
                    else if (statusMatch == "Not Found")
                    {
                        e.Row.CssClass = "warning";
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 7; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                Int32 iRow = Convert.ToInt32(e.CommandArgument); ClsType ClType = new ClsType();
                string sLocationID = ""; string sLocationName = ""; string sLocationType = "";
                string sOpnameID = ""; string sOpnameDate = ""; string sRemark = "";

                sOpnameID = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sOpnameDate = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
                sLocationType = (e.CommandSource as GridView).Rows[iRow].Cells[2].Text.Trim();
                sLocationName = (e.CommandSource as GridView).Rows[iRow].Cells[3].Text.Trim();
                sLocationID = (e.CommandSource as GridView).Rows[iRow].Cells[9].Text.Trim();
                sRemark = (e.CommandSource as GridView).Rows[iRow].Cells[10].Text.Trim();

                switch (e.CommandName.ToUpper())
                {
                    case "CHANGES":
                        txtLocationID.Value = sLocationID;
                        txtLocationName.Text = sLocationName;
                        CmbLocationType.SelectedValue = sLocationType;

                        txtOpnameID.Text = ClType.CheckNbsp(sOpnameID);

                        // Convert date from dd/MM/yyyy to yyyy-MM-dd for HTML5 date input
                        if (!string.IsNullOrEmpty(sOpnameDate))
                        {
                            string[] dateParts = sOpnameDate.Split('/');
                            if (dateParts.Length == 3)
                            {
                                txtOpnameDate.Value = dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0];
                            }
                            else
                            {
                                txtOpnameDate.Value = sOpnameDate;
                            }
                        }

                        txtRemark.Text = ClType.CheckNbsp(sRemark);

                        Session["ClsOpnameID"] = txtOpnameID.Text.Trim();
                        Session["ClsLocationType"] = sLocationType;
                        Session["ClsLocationID"] = sLocationID;
                        Open_GridView();
                        Button2.Style.Add("disabled", "disabled");
                        CmdCreate.Visible = false;
                        CmdAddDetail.Visible = true;
                        CmdSubmit.Visible = true;
                        CmdLoad.Visible = true;
                        CmdSubmit.Text = "Update";
                        div_comment.InnerHtml = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Stock opname can not be edited, (" + ex.Message + ")</div>";
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecStockOpnameDetail"], LblPagingDetail, ViewState["RecStockOpnameDetailFieldSort"].ToString(), ViewState["RecStockOpnameDetailDirSort"].ToString());
            div_comment.InnerHtml = "";
        }

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecStockOpnameDetail"], ViewState["RecStockOpnameDetailFieldSort"].ToString(), ViewState["RecStockOpnameDetailDirSort"].ToString(), e.SortExpression);
                ViewState["RecStockOpnameDetailFieldSort"] = e.SortExpression.ToString();
                ViewState["RecStockOpnameDetailDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView1, Session["RecStockOpnameHeader"], ViewState["RecStockOpnameHeaderFieldSort"].ToString(), ViewState["RecStockOpnameHeaderDirSort"].ToString(), e.SortExpression);
                ViewState["RecStockOpnameHeaderFieldSort"] = e.SortExpression.ToString();
                ViewState["RecStockOpnameHeaderDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }
    }
}