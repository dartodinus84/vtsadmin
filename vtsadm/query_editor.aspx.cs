using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class query_editor : System.Web.UI.Page
    {
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

        [WebMethod]
        public static string ExecuteData(string query_text, string server_name)
        {
            string myObjectJson = "";
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var connectionString = "";

            try
            {
                if (query_text != null)
                {
                    //connectionString = "Provider=SQLOLEDB.1;Data Source=147.139.137.216;Initial Catalog=" + server_name + ";User ID=sa;Password=v1s10n2019;";
                    connectionString = "Provider=SQLOLEDB.1;Data Source=147.139.137.216;Initial Catalog=" + server_name + ";User ID=sa;Password=easygotracking;";
                    ClsType ClType = new ClsType(); Recordset Rec = new Recordset();
                    string strSQL = query_text;
                    if (query_text.ToLower().Contains("select"))
                    {
                        OleDbConnection conn = new OleDbConnection(connectionString);
                        var query = "DECLARE @query NVARCHAR(MAX) = (" + query_text + " for json path) select @query as json_path";
                        try
                        {
                            if (conn.State == System.Data.ConnectionState.Closed)
                            {
                                conn.Open();
                            }
                            //conn.Open();
                            OleDbCommand cmd = new OleDbCommand();
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            myObjectJson = (string)cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {

                            myObjectJson = ex.Message;
                        }
                        finally
                        {
                            if (conn.State == System.Data.ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                        }
                    }
                    else if (query_text.ToLower().Contains("update") || query_text.ToLower().Contains("delete") || query_text.ToLower().Contains("insert"))
                    {
                        var nameQuery = "";
                        if (query_text.ToLower().Contains("update"))
                        {
                            nameQuery = "UPDATE";
                        }
                        if (query_text.ToLower().Contains("delete"))
                        {
                            nameQuery = "DELETE";
                        }
                        if (query_text.ToLower().Contains("insert"))
                        {
                            nameQuery = "INSERT";
                        }
                        OleDbConnection dbConnections = new OleDbConnection(connectionString);
                        try
                        {
                            if (dbConnections.State == System.Data.ConnectionState.Closed)
                            {
                                dbConnections.Open();
                            }
                            var cmd = new OleDbCommand
                            {
                                CommandText = query_text
                            };
                            var affected = cmd.ExecuteNonQuery();
                            if (affected > 0)
                            {

                                myObjectJson = $"{affected} {nameQuery} changed";
                            }
                            else
                            {
                                myObjectJson = $"{affected} {nameQuery} not changed";
                            }
                        }
                        catch (Exception ex)
                        {
                            myObjectJson = ex.Message;
                        }
                        finally
                        {
                            if (dbConnections.State == System.Data.ConnectionState.Open)
                            {
                                dbConnections.Close();
                            }
                            dbConnections.Dispose();
                        }

                    }


                }

            }
            catch (Exception ex)
            {
                myObjectJson = "";
            }
            return myObjectJson;
        }

        protected void submitQueryGridView_Click(object sender, EventArgs e)
        {
            var query = querySQL.Value;
        }

        //protected void Open_GridView(string query_sql,string server_name)
        //{
        //    try
        //    {
        //        ClsType ClType = new ClsType();
        //        string strSQL = query_sql;
        //        var connectionString = "Provider=SQLOLEDB.1;Data Source=147.139.137.216;Initial Catalog=" + server_name + ";User ID=sa;Password=easygotracking;";
        //        ClType.Open_GridView(GridView2, strSQL, connectionString, lblPaging, ViewState[sViewStateFieldSort].ToString(), ViewState[sViewStateDirSort].ToString());
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}