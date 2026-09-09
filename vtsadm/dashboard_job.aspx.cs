using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_job : System.Web.UI.Page
    {
        protected void LoadDashboard()
        {
            try
            {
                string filterType = GetSessionFilterValue("SessionFilterType");
                string regionId = GetSessionFilterValue("SessionGroupAreaName");
                string dateFrom = GetSessionFilterValue("SessionDateFrom");
                string dateTo = GetSessionFilterValue("SessionDateTo");

                BindSummary(filterType, regionId, dateFrom, dateTo);
                BindInstallation(filterType, regionId, dateFrom, dateTo);
                BindMaintenance(filterType, regionId, dateFrom, dateTo);
                BindRegion(filterType, regionId, dateFrom, dateTo);

                pnlRegion.Visible = string.IsNullOrWhiteSpace(regionId);
                SetRegionTitle();
            }
            catch (Exception ex)
            {
                SetDefaultSummary();
                SetDefaultInstallation();
                SetDefaultMaintenance();
                rptRegion.DataSource = null;
                rptRegion.DataBind();
                pnlRegion.Visible = true;
            }
        }

        protected void open_dashboard_data()
        {
            LoadDashboard();
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
            open_dashboard_data();
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SessionDateFrom"] = txtDateFrom.Text.Trim();
                Session["SessionDateTo"] = txtDateTo.Text.Trim();
                Session["SessionFilterType"] = NormalizeFilterValue(CmbFilterType.SelectedItem.Value.Trim());
                Session["SessionGroupAreaName"] = NormalizeFilterValue(CmbSupportAreaID.SelectedItem.Value.Trim());

                LoadDashboard();
            }
            catch (Exception ex)
            {

            }
        }


        protected void Clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbFilterType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_dashboard_filter");
                ClType.Open_Combos(CmbSupportAreaID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_list_support_area");

                ListItem defaultRegion = CmbSupportAreaID.Items.FindByText("[Select]");
                if (defaultRegion == null)
                {
                    CmbSupportAreaID.Items.Insert(0, new ListItem("[Select]", ""));
                }
                else
                {
                    defaultRegion.Value = "";
                }

                txtDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

                CmbFilterType.SelectedValue = "[Select]";
                CmbSupportAreaID.SelectedValue = "";
               
                Session["SessionDateFrom"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionDateTo"] = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SessionFilterType"] = "";
                Session["SessionGroupAreaName"] = "";
                SetRegionTitle();

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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHJO"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            LoadDashboard();
                            SetRegionTitle();
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

        protected void CmbSupportAreaID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SessionGroupAreaName"] = NormalizeFilterValue(CmbSupportAreaID.SelectedValue.Trim());
            Session["SessionFilterType"] = NormalizeFilterValue(CmbFilterType.SelectedValue.Trim());
            Session["SessionDateFrom"] = txtDateFrom.Text.Trim();
            Session["SessionDateTo"] = txtDateTo.Text.Trim();
            SetRegionTitle();
            LoadDashboard();
        }

        protected void rptRegion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "SelectRegion", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string selectedRegion = e.CommandArgument == null ? "" : e.CommandArgument.ToString().Trim();
            if (!TrySelectRegion(selectedRegion))
            {
                CmbSupportAreaID.SelectedValue = "";
            }

            Session["SessionGroupAreaName"] = NormalizeFilterValue(CmbSupportAreaID.SelectedValue.Trim());
            Session["SessionFilterType"] = NormalizeFilterValue(CmbFilterType.SelectedValue.Trim());
            Session["SessionDateFrom"] = txtDateFrom.Text.Trim();
            Session["SessionDateTo"] = txtDateTo.Text.Trim();

            LoadDashboard();
        }

        private void SetRegionTitle()
        {
            if (CmbSupportAreaID.SelectedItem == null ||
                CmbSupportAreaID.SelectedValue == "" ||
                CmbSupportAreaID.SelectedItem.Text == "[Select]")
            {
                lblRegion.InnerText = "All Regional";
            }
            else
            {
                lblRegion.InnerText = CmbSupportAreaID.SelectedItem.Text;
            }
        }

        private void BindSummary(string filterType, string regionId, string dateFrom, string dateTo)
        {
            Recordset Rec = new Recordset();
            string strSQL = "sp_dashboard_job_summary_by_type '" + filterType + "','" + regionId + "','" + dateFrom + "','" + dateTo + "'";
            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() > 0)
            {
                LblCntJONew.InnerText = FormatCount(Rec.Fields("cntJONew"));
                LblCntJOMaint.InnerText = FormatCount(Rec.Fields("cntJOMaint"));
                return;
            }

            SetDefaultSummary();
        }

        private void BindInstallation(string filterType, string regionId, string dateFrom, string dateTo)
        {
            Recordset Rec = new Recordset();
            string strSQL = "sp_dashboard_job_summary_installation '" + filterType + "','" + regionId + "','" + dateFrom + "','" + dateTo + "'";
            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() > 0)
            {
                LblCntJoNewOpen.InnerText = FormatCount(Rec.Fields("JobOpen"));
                LblCntJoNewProcess.InnerText = FormatCount(Rec.Fields("JobProcess"));
                LblCntJoNewClose.InnerText = FormatCount(Rec.Fields("JobClose"));
                return;
            }

            SetDefaultInstallation();
        }

        private void BindMaintenance(string filterType, string regionId, string dateFrom, string dateTo)
        {
            Recordset Rec = new Recordset();
            string strSQL = "sp_dashboard_job_summary_maintenance '" + filterType + "','" + regionId + "','" + dateFrom + "','" + dateTo + "'";
            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() > 0)
            {
                LblCntJoMaintOpen.InnerText = FormatCount(Rec.Fields("JobOpen"));
                LblCntJoMaintProcess.InnerText = FormatCount(Rec.Fields("JobProcess"));
                LblCntJoMaintClose.InnerText = FormatCount(Rec.Fields("JobClose"));
                return;
            }

            SetDefaultMaintenance();
        }

        private void BindRegion(string filterType, string regionId, string dateFrom, string dateTo)
        {
            Recordset Rec = new Recordset();
            string strSQL = "sp_dashboard_job_summary_by_region '" + filterType + "','" + regionId + "','" + dateFrom + "','" + dateTo + "'";
            Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());

            if (Rec.RecordCount() <= 0)
            {
                rptRegion.DataSource = null;
                rptRegion.DataBind();
                return;
            }

            DataTable dtRegion = Rec.DataRecord();
            if (dtRegion != null && dtRegion.Rows.Count > 0 && dtRegion.Columns.Contains("cntJO"))
            {
                DataView view = dtRegion.DefaultView;
                view.Sort = "cntJO DESC";
                rptRegion.DataSource = view;
            }
            else
            {
                rptRegion.DataSource = dtRegion;
            }

            rptRegion.DataBind();
        }

        private void SetDefaultSummary()
        {
            LblCntJONew.InnerText = "0";
            LblCntJOMaint.InnerText = "0";
        }

        private void SetDefaultInstallation()
        {
            LblCntJoNewOpen.InnerText = "0";
            LblCntJoNewProcess.InnerText = "0";
            LblCntJoNewClose.InnerText = "0";
        }

        private void SetDefaultMaintenance()
        {
            LblCntJoMaintOpen.InnerText = "0";
            LblCntJoMaintProcess.InnerText = "0";
            LblCntJoMaintClose.InnerText = "0";
        }

        private string NormalizeFilterValue(string value)
        {
            string normalized = value == null ? "" : value.Trim();
            if (normalized == "[Select]") return "";
            return normalized;
        }

        private string GetSessionFilterValue(string sessionName)
        {
            object sessionValue = Session[sessionName];
            return sessionValue == null ? "" : Convert.ToString(sessionValue).Trim();
        }

        private bool TrySelectRegion(string regionValue)
        {
            string normalizedRegion = NormalizeFilterValue(regionValue);
            if (string.IsNullOrEmpty(normalizedRegion))
            {
                CmbSupportAreaID.SelectedValue = "";
                return true;
            }

            ListItem optionByValue = CmbSupportAreaID.Items.FindByValue(normalizedRegion);
            if (optionByValue != null)
            {
                CmbSupportAreaID.ClearSelection();
                optionByValue.Selected = true;
                return true;
            }

            foreach (ListItem item in CmbSupportAreaID.Items)
            {
                if (string.Equals(item.Text.Trim(), normalizedRegion, StringComparison.OrdinalIgnoreCase))
                {
                    CmbSupportAreaID.ClearSelection();
                    item.Selected = true;
                    return true;
                }
            }

            return false;
        }

        private string FormatCount(string value)
        {
            double parsedValue;
            if (double.TryParse(value, out parsedValue))
            {
                return parsedValue.ToString("#,##0");
            }

            return "0";
        }

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
        
    }
  
}