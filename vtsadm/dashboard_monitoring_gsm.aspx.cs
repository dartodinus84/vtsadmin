using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class dashboard_monitoring_gsm : System.Web.UI.Page
    {
        protected void open_dashboard_data(string filterType, string GroupAreaName)
        {
            try
            {
                
                Recordset Rec = new Recordset();

                string strSQL = "";

                if (filterType == "UNPR")
                {
                    strSQL = "sp_dashboard_monitoring_gsm_uninstall  '" + Session["ClsTypeUserGroupID"].ToString() + "', '" + Session["ClsTypeUserID"].ToString() + "','" + filterType + "','" + GroupAreaName + "','1',''";
                }
                else if (filterType == "UNPO")
                {
                    strSQL = "sp_dashboard_monitoring_gsm_uninstall  '" + Session["ClsTypeUserGroupID"].ToString() + "', '" + Session["ClsTypeUserID"].ToString() + "','" + filterType + "','" + GroupAreaName + "','1',''";
                }
                else {
                    strSQL = "sp_dashboard_monitoring_gsm  '" + Session["ClsTypeUserGroupID"].ToString() + "', '" + Session["ClsTypeUserID"].ToString() + "','" + GroupAreaName + "','1',''";
                }

                lblCntDurasi_1.InnerText = "0";
                lblCntDurasi_2.InnerText = "0";
                lblCntDurasi_3.InnerText = "0";
                lblCntDurasi_4.InnerText = "0";
                lblCntDurasi_5.InnerText = "0";
                lblCntDurasi_6.InnerText = "0";
                lblCntDurasi_7.InnerText = "0";
                lblCntDurasi_8.InnerText = "0";
                lblCntDurasi_9.InnerText = "0";
                lblCntDurasi_10.InnerText = "0";
                lblCntDurasi_11.InnerText = "0";
                lblCntDurasi_12.InnerText = "0";
                lblCntDurasi_13.InnerText = "0";
                lblCntDurasi_14.InnerText = "0";
                lblCntDurasi_15.InnerText = "0";
                lblCntDurasi_16.InnerText = "0";
                lblCntDurasi_17.InnerText = "0";
                lblCntDurasi_18.InnerText = "0";
                lblCntDurasi_19.InnerText = "0";
                lblCntDurasi_20.InnerText = "0";
                lblCntDurasi_21.InnerText = "0";
                lblCntDurasi_22.InnerText = "0";
                lblCntDurasi_23.InnerText = "0";
                lblCntDurasi_24.InnerText = "0";
                lblCntDurasi_25.InnerText = "0";
                lblCntDurasi_26.InnerText = "0";
                lblCntDurasi_27.InnerText = "0";
                lblCntDurasi_28.InnerText = "0";
                lblCntDurasi_29.InnerText = "0";
                lblCntDurasi_30.InnerText = "0";
                lblCntDurasi.InnerText = "0";
                lblCntDurasiTotal.InnerText = "0";

                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                if (Rec.RecordCount() > 0)
                {
                    lblCntDurasi_1.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_1")).ToString("#,##0");
                    lblCntDurasi_2.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_2")).ToString("#,##0");
                    lblCntDurasi_3.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_3")).ToString("#,##0");
                    lblCntDurasi_4.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_4")).ToString("#,##0");
                    lblCntDurasi_5.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_5")).ToString("#,##0");
                    lblCntDurasi_6.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_6")).ToString("#,##0");
                    lblCntDurasi_7.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_7")).ToString("#,##0");
                    lblCntDurasi_8.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_8")).ToString("#,##0");
                    lblCntDurasi_9.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_9")).ToString("#,##0");
                    lblCntDurasi_10.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_10")).ToString("#,##0");
                    lblCntDurasi_11.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_11")).ToString("#,##0");
                    lblCntDurasi_12.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_12")).ToString("#,##0");
                    lblCntDurasi_13.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_13")).ToString("#,##0");
                    lblCntDurasi_14.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_14")).ToString("#,##0");
                    lblCntDurasi_15.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_15")).ToString("#,##0");
                    lblCntDurasi_16.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_16")).ToString("#,##0");
                    lblCntDurasi_17.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_17")).ToString("#,##0");
                    lblCntDurasi_18.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_18")).ToString("#,##0");
                    lblCntDurasi_19.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_19")).ToString("#,##0");
                    lblCntDurasi_20.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_20")).ToString("#,##0");
                    lblCntDurasi_21.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_21")).ToString("#,##0");
                    lblCntDurasi_22.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_22")).ToString("#,##0");
                    lblCntDurasi_23.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_23")).ToString("#,##0");
                    lblCntDurasi_24.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_24")).ToString("#,##0");
                    lblCntDurasi_25.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_25")).ToString("#,##0");
                    lblCntDurasi_26.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_26")).ToString("#,##0");
                    lblCntDurasi_27.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_27")).ToString("#,##0");
                    lblCntDurasi_28.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_28")).ToString("#,##0");
                    lblCntDurasi_29.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_29")).ToString("#,##0");
                    lblCntDurasi_30.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi_30")).ToString("#,##0");
                    lblCntDurasi.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasi")).ToString("#,##0");
                    lblCntDurasiTotal.InnerHtml = Convert.ToDouble(Rec.Fields("CntDurasiTotal")).ToString("#,##0");
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {

                Session["SessionFilterType"] = CmbFilterType.SelectedItem.Value.Trim();
                Session["SessionGroupAreaName"] = CmbGroupAreaName.SelectedItem.Value.Trim();
                open_dashboard_data(Session["SessionFilterType"].ToString(), Session["SessionGroupAreaName"].ToString());
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
                ClType.Open_Combos(CmbFilterType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_dashboard_gsm_filter");
                ClType.Open_Combos(CmbGroupAreaName, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_get_group_area");

                lblCntDurasi_1.InnerText = "0";
                lblCntDurasi_2.InnerText = "0";
                lblCntDurasi_3.InnerText = "0";
                lblCntDurasi_4.InnerText = "0";
                lblCntDurasi_5.InnerText = "0";
                lblCntDurasi_6.InnerText = "0";
                lblCntDurasi_7.InnerText = "0";
                lblCntDurasi_8.InnerText = "0";
                lblCntDurasi_9.InnerText = "0";
                lblCntDurasi_10.InnerText = "0";
                lblCntDurasi_11.InnerText = "0";
                lblCntDurasi_12.InnerText = "0";
                lblCntDurasi_13.InnerText = "0";
                lblCntDurasi_14.InnerText = "0";
                lblCntDurasi_15.InnerText = "0";
                lblCntDurasi_16.InnerText = "0";
                lblCntDurasi_17.InnerText = "0";
                lblCntDurasi_18.InnerText = "0";
                lblCntDurasi_19.InnerText = "0";
                lblCntDurasi_20.InnerText = "0";
                lblCntDurasi_21.InnerText = "0";
                lblCntDurasi_22.InnerText = "0";
                lblCntDurasi_23.InnerText = "0";
                lblCntDurasi_24.InnerText = "0";
                lblCntDurasi_25.InnerText = "0";
                lblCntDurasi_26.InnerText = "0";
                lblCntDurasi_27.InnerText = "0";
                lblCntDurasi_28.InnerText = "0";
                lblCntDurasi_29.InnerText = "0";
                lblCntDurasi_30.InnerText = "0";
                lblCntDurasi.InnerText = "0";
                lblCntDurasiTotal.InnerText = "0";

                Session["SessionFilterType"] = "";
                Session["SessionGroupAreaName"] = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string strHtmlMenu = "";
                ClsType ClType = new ClsType();
                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
                            open_dashboard_data("", "");
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

        public string DBConnstringSQL()
        {
            return Session["ClsTypeDBConnStringSQL"].ToString().Trim();
        }
    }

}