using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;
using System.IO;

namespace vtsadm
{
    public partial class dashboard_customer2 : System.Web.UI.Page
    {
        protected void open_dashboard_data()
        {
            try
            {
                Recordset Rec = new Recordset();
                Recordset Rec2 = new Recordset();
                Recordset Rec3 = new Recordset();
                Recordset Rec4 = new Recordset();
                Recordset Rec5 = new Recordset();

                string strSQL = "sp_dashboard_customer__total_customer";
                string strSqlCustPackage = "sp_dashboard_customer__package";
                string strSqlCustBranch = "sp_dashboard_customer__branch_status";
                string strSqlCustUnit = "sp_dashboard_customer__totalunit";
                string strSqlCustJOclose = "sp_dashboard_customer__totaljotrainingclose";

                Rec.Open(strSQL, Session["ClsTypeDBConnStringSQL"].ToString());
                Rec2.Open(strSqlCustPackage, Session["ClsTypeDBConnStringSQL"].ToString());
                Rec3.Open(strSqlCustBranch, Session["ClsTypeDBConnStringSQL"].ToString());
                Rec4.Open(strSqlCustUnit, Session["ClsTypeDBConnStringSQL"].ToString());
                Rec5.Open(strSqlCustJOclose, Session["ClsTypeDBConnStringSQL"].ToString());

                if (Rec.RecordCount() > 0)
                {
                    lblTotalCustomer.InnerHtml = Convert.ToDouble(Rec.Fields("Total")).ToString("#,##0");
                    lblJmlTMS.InnerHtml = Convert.ToDouble(Rec.Fields("Tms")).ToString("#,##0");
                    lblJmlIGO.InnerHtml = Convert.ToDouble(Rec.Fields("Igo")).ToString("#,##0");
                    lbl_tms_active.InnerHtml = Convert.ToDouble(Rec.Fields("Tms_Active")).ToString("#,##0");
                    lbl_tms_block.InnerHtml = Convert.ToDouble(Rec.Fields("Tms_Block")).ToString("#,##0");
                }

                if (Rec2.RecordCount() > 0)
                {
                    //Monitoring.InnerHtml = Convert.ToDouble(Rec2.Fields("Monitoring")).ToString("#,##0");
                    //ListPackage = Rec2;
                    //ListPackage.DataBind();
                }

                if (Rec3.RecordCount() > 0)
                {
                    lblJktActive.InnerHtml = Convert.ToDouble(Rec3.Fields("jakarta_active")).ToString();
                    lblJktBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("jakarta_block")).ToString();
                    lblSbyActive.InnerHtml = Convert.ToDouble(Rec3.Fields("surabaya_active")).ToString();
                    lblSbyBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("surabaya_block")).ToString();
                    lblMdnActive.InnerHtml = Convert.ToDouble(Rec3.Fields("medan_active")).ToString();
                    LblMdnBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("medan_block")).ToString();
                    lblPdgActive.InnerHtml = Convert.ToDouble(Rec3.Fields("padang_active")).ToString();
                    lblPdgBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("padang_block")).ToString();
                    lblPlmbActive.InnerHtml = Convert.ToDouble(Rec3.Fields("palembang_active")).ToString();
                    lblPlmbBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("palembang_block")).ToString();
                    lblSmrActive.InnerHtml = Convert.ToDouble(Rec3.Fields("semarang_active")).ToString();
                    lblSmrBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("semarang_block")).ToString();
                    lblYgtActive.InnerHtml = Convert.ToDouble(Rec3.Fields("yogyakarta_active")).ToString();
                    lblYgtBlock.InnerHtml = Convert.ToDouble(Rec3.Fields("yogyakarta_blok")).ToString();
                }

                if (Rec4.RecordCount() > 0)
                {
                    unit12.InnerHtml = Convert.ToDouble(Rec4.Fields("total_1_2")).ToString("#,##0");
                    unit35.InnerHtml = Convert.ToDouble(Rec4.Fields("total_3_5")).ToString("#,##0");
                    unit610.InnerHtml = Convert.ToDouble(Rec4.Fields("total_6_10")).ToString("#,##0");
                    unit1120.InnerHtml = Convert.ToDouble(Rec4.Fields("total_11_20")).ToString("#,##0");
                    unit2130.InnerHtml = Convert.ToDouble(Rec4.Fields("total_21_30")).ToString("#,##0");
                    unit30.InnerHtml = Convert.ToDouble(Rec4.Fields("total_30")).ToString("#,##0");
                }

                if (Rec5.RecordCount() > 0)
                {
                    joTrng12.InnerHtml = Convert.ToDouble(Rec5.Fields("total_1_2")).ToString("#,##0");
                    joTrng35.InnerHtml = Convert.ToDouble(Rec5.Fields("total_3_5")).ToString("#,##0");
                    joTrng610.InnerHtml = Convert.ToDouble(Rec5.Fields("total_6_10")).ToString("#,##0");
                    joTrng1120.InnerHtml = Convert.ToDouble(Rec5.Fields("total_11_20")).ToString("#,##0");
                    joTrng2130.InnerHtml = Convert.ToDouble(Rec5.Fields("total_21_30")).ToString("#,##0");
                    joTrng30.InnerHtml = Convert.ToDouble(Rec5.Fields("total_30")).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Open_GridView()
        {
            try
            {
                ClsType ClType = new ClsType();
                //string strSQL = "sp_list_customer '" + txtSearch.Text.Trim() + "'";
                string strSQL = "sp_dashboard_customer__package";
                ViewState["RecListCustomerFieldSort"] = "jml";
                ViewState["RecListCustomerDirSort"] = "DESC";
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
                //ClsType ClType = new ClsType();
                //string strSQL = "sp_list_customer_document '" + txtCustomerID.Text.Trim() + "'";
                //Session["RecListCustomerDocument"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingDoc);
            }
            catch (Exception ex)
            {

            }
        }
        protected void Open_GridViewServer()
        {
            try
            {
                //ClsType ClType = new ClsType();
                //string strSQL = "sp_list_customer_servers '" + txtCustomerID.Text.Trim() + "'";
                //Session["RecListCustomerServer"] = ClType.Open_GridView(GridView3, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPagingServer);
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
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUDASHCUSTOMER"))
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
                            open_dashboard_data();
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

        [WebMethod]
        public static string CutomorWarehouse()
        {
            string sOut = "";
            List<DataWarehouseCustomer> dJson = new List<DataWarehouseCustomer>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_device_warehouse";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataWarehouseCustomer
                        {
                            warehouse = Rec.Fields("WarehouseName"),
                            mw = Convert.ToInt32(Rec.Fields("warehouse")),
                            mt = Convert.ToInt32(Rec.Fields("technician")),
                            inst = Convert.ToInt32(Rec.Fields("installed")),
                            br = Convert.ToInt32(Rec.Fields("brooken"))
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

        private void clear()
        {
            //try
            //{
            //    ClsType ClType = new ClsType();
            //    ClType.Open_Combos(CmbDeviceGroupID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicegroup");
            //    ClType.Open_Combos(CmbDeviceTypeID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_devicetype");
            //    ClType.Open_Combos(CmbVendorID, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_vendor");
            //    ClType.Open_Combos(CmbSource, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_source");
            //    ClType.Open_Combos(CmbServer, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_device_server");
            //    ClType.Open_Combos(CmbIsMobile, Session["ClsTypeDBConnStringSQL"].ToString(), "", "sp_list_is_mobile");

            //    txtDeviceID.Text = "";
            //    CmbDeviceGroupID.SelectedValue = "[Select]";
            //    CmbDeviceTypeID.SelectedValue = "[Select]";
            //    txtNoSN.Text = "";
            //    txtGMT.Text = "";
            //    txtDate.Text = "";
            //    CmbSource.SelectedValue = "[Select]";
            //    CmbVendorID.SelectedValue = "[Select]";
            //    CmbServer.SelectedValue = "[Select]";
            //    CmbIsMobile.SelectedValue = "[Select]";

            //    LblDeviceID.InnerHtml = "";
            //    txtDeviceIDDelete.Value = "";
            //    txtStatusDelete.Value = "";

            //    txtVendorAddress.Text = "";
            //    CmdSubmit.Text = "Submit";
            //}
            //catch (Exception ex)
            //{

            //}
        }

        [WebMethod]
        public static string CustomerPackage()
        {
            //string sOut = "";
            List<DataPackageCustomer> dJson = new List<DataPackageCustomer>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_customer__package";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataPackageCustomer
                        {
                            package = Rec.Fields("PackageName"),
                            jml = Convert.ToInt32(Rec.Fields("jml"))
                        });
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                //sOut = "error : " + ex.Message;
            }
            return JsonConvert.SerializeObject(new { data = dJson });
        }

        [WebMethod]
        public static string ListCustomer()
        {
            //string sOut = "";
            List<DataPackageCustomer> dJson = new List<DataPackageCustomer>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_customer__package";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataPackageCustomer
                        {
                            package = Rec.Fields("PackageName"),
                            jml = Convert.ToInt32(Rec.Fields("jml"))
                        });
                        Rec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                //sOut = "error : " + ex.Message;
            }
            return JsonConvert.SerializeObject(new { data = dJson });
        }

        [WebMethod]
        public static string DeviceTechnician(string sBranchID)
        {
            string sOut = "";
            List<DataTechnicianDevice> dJson = new List<DataTechnicianDevice>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_chart_device_technician '" + sBranchID + "'";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new DataTechnicianDevice
                        {
                            technician = Rec.Fields("TechnicianName"),
                            device = Convert.ToInt32(Rec.Fields("sum_device"))
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
        public static string CustomerEasygo()
        {
            string sOut = "";
            List<JmlCusotmerEasygo> dJson = new List<JmlCusotmerEasygo>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_customer__total_customer";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new JmlCusotmerEasygo
                        {
                            totalCustomer = Convert.ToInt32(Rec.Fields("Total")),
                            jmlTms = Convert.ToInt32(Rec.Fields("Tms")),
                            tmsActive = Convert.ToInt32(Rec.Fields("Tms_Active")),
                            tmsBlock = Convert.ToInt32(Rec.Fields("Tms_Block")),
                            jmlIgo = Convert.ToInt32(Rec.Fields("Igo"))
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
        public static string CustomerBranch()
        {
            string sOut = "";
            List<JmlCusotmerBranch> dJson = new List<JmlCusotmerBranch>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_customer__branch_status";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new JmlCusotmerBranch
                        {
                            jktActive = Convert.ToInt32(Rec.Fields("jakarta_active")),
                            jktBlock = Convert.ToInt32(Rec.Fields("jakarta_block")),
                            sbyActive = Convert.ToInt32(Rec.Fields("surabaya_active")),
                            sbyBlock = Convert.ToInt32(Rec.Fields("surabaya_block")),
                            mdnActive = Convert.ToInt32(Rec.Fields("medan_active")),
                            mdnBlock = Convert.ToInt32(Rec.Fields("medan_block")),
                            pdgActive = Convert.ToInt32(Rec.Fields("padang_active")),
                            pdgBlock = Convert.ToInt32(Rec.Fields("padang_block")),
                            smrgActive = Convert.ToInt32(Rec.Fields("semarang_active")),
                            smrgBlock = Convert.ToInt32(Rec.Fields("semarang_block")),
                            plmbActive = Convert.ToInt32(Rec.Fields("palembang_active")),
                            plmbBlock = Convert.ToInt32(Rec.Fields("palembang_block")),
                            yogActive = Convert.ToInt32(Rec.Fields("yogyakarta_active")),
                            yogBlock = Convert.ToInt32(Rec.Fields("yogyakarta_block"))
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
        public static string CustomerRangeUnit()
        {
            string sOut = "";
            List<JmlCusotmerRangeUnit> dJson = new List<JmlCusotmerRangeUnit>();
            try
            {
                var pageData = new dashboard();
                Recordset Rec = new Recordset();
                string strSQL = "sp_dashboard_customer__totalunit";
                Rec.Open(strSQL, pageData.DBConnstringSQL());
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        dJson.Add(new JmlCusotmerRangeUnit
                        {
                            a = Convert.ToInt32(Rec.Fields("total_1_2")),
                            b = Convert.ToInt32(Rec.Fields("total_3_5")),
                            c = Convert.ToInt32(Rec.Fields("total_6_10")),
                            d = Convert.ToInt32(Rec.Fields("total_11_20")),
                            e = Convert.ToInt32(Rec.Fields("total_21_30")),
                            f = Convert.ToInt32(Rec.Fields("total_30"))
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

        protected void GridView2_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                Int32 iRow = Convert.ToInt32(e.CommandArgument);
                ClsType ClType = new ClsType();
                int intAff = 0;
                string sPackageName = "";
                string sJmlCustomer = "";
                sPackageName = (e.CommandSource as GridView).Rows[iRow].Cells[0].Text.Trim();
                sJmlCustomer = (e.CommandSource as GridView).Rows[iRow].Cells[1].Text.Trim();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView2_PageIndexChanging(Object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListCustomer"], LblPaging, ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString());
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

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 9; i <= 43; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 9; i <= 43; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[7].ToolTip = "Edit";
                    LinkButton CmdButton = (LinkButton)e.Row.Cells[8].FindControl("CmdDelete");
                    CmdButton.OnClientClick = "confirmDelete('" + e.Row.Cells[0].Text.ToString() + "','" + e.Row.Cells[6].Text.ToString() + "'); return false;";
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

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                //div_comment.InnerHtml = "";
                ClsType ClTye = new ClsType();
                string sNewDirSort = ClTye.Gv_Sorting(GridView2, Session["RecListCustomer"], ViewState["RecListCustomerFieldSort"].ToString(), ViewState["RecListCustomerDirSort"].ToString(), e.SortExpression);
                ViewState["RecListCustomerFieldSort"] = e.SortExpression.ToString();
                ViewState["RecListCustomerDirSort"] = sNewDirSort;
            }
            catch (Exception ex)
            {
                //div_comment.InnerHtml = "<div class='alert alert-danger' role='alert'><button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Failed!</strong> Sorting data has been failed (" + ex.Message + ")</div>";
            }
        }

        public class DataWarehouseCustomer
        {
            public string warehouse { get; set; }
            public int mw { get; set; }
            public int mt { get; set; }
            public int inst { get; set; }
            public int br { get; set; }
        }

        public class DataTechnicianCustomer
        {
            public string technician { get; set; }
            public int device { get; set; }
        }

        public class DataPackageCustomer
        {
            public string PackageName { get; set; }
            public string package { get; set; }
            public int jml { get; set; }
        }

        public class JmlCusotmerEasygo
        {
            public int tms { get; set; }
            public int igo { get; set; }
            public int tmsActive { get; set; }
            public int tmsBlock { get; set; }
            public int totalCustomer { get; set; }
            public int jmlTms { get; set; }
            public int jmlIgo { get; set; }
        }

        public class JmlCusotmerBranch
        {
            public int jktActive { get; set; }
            public int jktBlock { get; set; }
            public int sbyActive { get; set; }
            public int sbyBlock { get; set; }
            public int mdnActive { get; set; }
            public int mdnBlock { get; set; }
            public int pdgActive { get; set; }
            public int pdgBlock { get; set; }
            public int plmbActive { get; set; }
            public int plmbBlock { get; set; }
            public int smrgActive { get; set; }
            public int smrgBlock { get; set; }
            public int yogActive { get; set; }
            public int yogBlock { get; set; }
        }

        public class JmlCusotmerRangeUnit
        {
            public int a { get; set; }
            public int b { get; set; }
            public int c { get; set; }
            public int d { get; set; }
            public int e { get; set; }
            public int f { get; set; }
        }
    }
}