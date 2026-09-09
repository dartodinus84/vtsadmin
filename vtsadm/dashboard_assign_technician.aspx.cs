using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
namespace vtsadm
{
    public partial class dashboard_assign_technician : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHASSIGNTECH"))
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

        protected void Clear()
        {
            try
            {
                ClsType ClType = new ClsType();
                ClType.Open_Combos(CmbFilterType, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_dashboard_area_filter");

                CmbFilterType.SelectedValue = "[Select]";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";

            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        public static string dashboardassignnew(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataAssignNew> dJson = new List<DataAssignNew>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_installation_branch '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataAssignNew
                        {
                            BranchID = Rec.Fields("BranchID"),
                            BranchName = Rec.Fields("BranchName"),
                            CntJoNewTotal = Convert.ToDouble(Rec.Fields("CntJoNewTotal")).ToString("#,##0"),
                            CntJoNewOpen = Convert.ToDouble(Rec.Fields("CntJoNewOpen")).ToString("#,##0"),
                            CntJoNewClose = Convert.ToDouble(Rec.Fields("CntJoNewClose")).ToString("#,##0"),
                            CntJoNewTotalUnit = Convert.ToDouble(Rec.Fields("CntJoNewTotalUnit")).ToString("#,##0"),
                            CntJoNewOpenUnit = Convert.ToDouble(Rec.Fields("CntJoNewOpenUnit")).ToString("#,##0"),
                            CntJoNewCloseUnit = Convert.ToDouble(Rec.Fields("CntJoNewCloseUnit")).ToString("#,##0"),


                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });
        }

        [WebMethod]
        public static string dashboardassignmaint(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataAssignMaint> dJson = new List<DataAssignMaint>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_maintenance_branch '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataAssignMaint
                        {
                            BranchID = Rec.Fields("BranchID"),
                            BranchName = Rec.Fields("BranchName"),
                            CntJoMaintTotal = Convert.ToDouble(Rec.Fields("CntJoMaintTotal")).ToString("#,##0"),
                            CntJoMaintOpen = Convert.ToDouble(Rec.Fields("CntJoMaintOpen")).ToString("#,##0"),
                            CntJoMaintClose = Convert.ToDouble(Rec.Fields("CntJoMaintClose")).ToString("#,##0"),
                            CntJoMaintTotalUnit = Convert.ToDouble(Rec.Fields("CntJoMaintTotalUnit")).ToString("#,##0"),
                            CntJoMaintOpenUnit = Convert.ToDouble(Rec.Fields("CntJoMaintOpenUnit")).ToString("#,##0"),
                            CntJoMaintCloseUnit = Convert.ToDouble(Rec.Fields("CntJoMaintCloseUnit")).ToString("#,##0"),

                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });

        }


        [WebMethod]
        public static string dashboardassigntechniciannew(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataTechnicianAssignNew> dJson = new List<DataTechnicianAssignNew>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_installation_technician '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianAssignNew
                        {
                            AreaID = Rec.Fields("AreaID"),
                            PicAreaID = Rec.Fields("PicAreaID"),
                            JoNewTechnicianName = Rec.Fields("JoNewTechnicianName"),
                            CntJoNewAssignTotal = Convert.ToDouble(Rec.Fields("CntJoNewAssignTotal")).ToString("#,##0"),
                            CntJoNewAssignOpen = Convert.ToDouble(Rec.Fields("CntJoNewAssignOpen")).ToString("#,##0"),
                            CntJoNewAssignClose = Convert.ToDouble(Rec.Fields("CntJoNewAssignClose")).ToString("#,##0")

                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });

        }

        [WebMethod]
        public static string dashboardassigntechnicianmaint(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataTechnicianAssignMaint> dJson = new List<DataTechnicianAssignMaint>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_maintenance_technician '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianAssignMaint
                        {
                            AreaID = Rec.Fields("AreaID"),
                            PicAreaID = Rec.Fields("PicAreaID"),
                            JoMaintTechnicianName = Rec.Fields("JoMaintTechnicianName"),
                            CntJoMaintAssignTotal = Convert.ToDouble(Rec.Fields("CntJoMaintAssignTotal")).ToString("#,##0"),
                            CntJoMaintAssignOpen = Convert.ToDouble(Rec.Fields("CntJoMaintAssignOpen")).ToString("#,##0"),
                            CntJoMaintAssignClose = Convert.ToDouble(Rec.Fields("CntJoMaintAssignClose")).ToString("#,##0")

                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });
        }


        [WebMethod]
        public static string dashboardassignbytechniciannew(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataTechnicianAssignNew> dJson = new List<DataTechnicianAssignNew>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_installation_bytechnician '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianAssignNew
                        {
                            AreaID = Rec.Fields("AreaID"),
                            PicAreaID = Rec.Fields("PicAreaID"),
                            JoNewTechnicianName = Rec.Fields("JoNewTechnicianName"),
                            CntJoNewAssignTotal = Convert.ToDouble(Rec.Fields("CntJoNewAssignTotal")).ToString("#,##0"),
                            CntJoNewAssignOpen = Convert.ToDouble(Rec.Fields("CntJoNewAssignOpen")).ToString("#,##0"),
                            CntJoNewAssignClose = Convert.ToDouble(Rec.Fields("CntJoNewAssignClose")).ToString("#,##0")

                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });

        }
        [WebMethod]
        public static string dashboardassignbytechnicianmaint(string areaid, string from, string to)
        {
            string sOut = "";
            List<DataTechnicianAssignMaint> dJson = new List<DataTechnicianAssignMaint>();

            try
            {
                var pageData = new dashboard_assign_technician();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_assign_technician_maintenance_bytechnician '" + areaid.ToString() + "', '" + from.ToString() + "', '" + to.ToString() + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianAssignMaint
                        {
                            AreaID = Rec.Fields("AreaID"),
                            PicAreaID = Rec.Fields("PicAreaID"),
                            JoMaintTechnicianName = Rec.Fields("JoMaintTechnicianName"),
                            CntJoMaintAssignTotal = Convert.ToDouble(Rec.Fields("CntJoMaintAssignTotal")).ToString("#,##0"),
                            CntJoMaintAssignOpen = Convert.ToDouble(Rec.Fields("CntJoMaintAssignOpen")).ToString("#,##0"),
                            CntJoMaintAssignClose = Convert.ToDouble(Rec.Fields("CntJoMaintAssignClose")).ToString("#,##0")

                        });
                        Rec.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                sOut = "error : " + ex.Message;
            }

            return JsonConvert.SerializeObject(new { data = dJson });
        }
        protected void CmdClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public class DataAssignNew
        {

            public string BranchID { get; set; }
            public string BranchName { get; set; }
            public string CntJoNewTotal { get; set; }
            public string CntJoNewOpen { get; set; }
            public string CntJoNewClose { get; set; }

            public string CntJoNewTotalUnit { get; set; }
            public string CntJoNewOpenUnit { get; set; }
            public string CntJoNewCloseUnit { get; set; }

            
            

        }

        public class DataAssignMaint
        {
            public string BranchID { get; set; }
            public string BranchName { get; set; }
            public string CntJoMaintTotal { get; set; }
            public string CntJoMaintOpen { get; set; }
            public string CntJoMaintClose { get; set; }
            public string CntJoMaintTotalUnit { get; set; }
            public string CntJoMaintOpenUnit { get; set; }
            public string CntJoMaintCloseUnit { get; set; }
    
        }

        public class DataTechnicianAssignNew
        {
            public string AreaID { get; set; }
            public string PicAreaID { get; set; }
            public string JoNewTechnicianName { get; set; }
            public string CntJoNewAssignTotal { get; set; }
            public string CntJoNewAssignOpen { get; set; }
            public string CntJoNewAssignClose { get; set; }
        }

        public class DataTechnicianAssignMaint
        {
            public string AreaID { get; set; }
            public string PicAreaID { get; set; }
            public string JoMaintTechnicianName { get; set; }
            public string CntJoMaintAssignTotal { get; set; }
            public string CntJoMaintAssignOpen { get; set; }
            public string CntJoMaintAssignClose { get; set; }
        }

    }
    
}