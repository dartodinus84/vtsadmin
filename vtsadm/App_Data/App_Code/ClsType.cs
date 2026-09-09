using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace vtsadm.App_Code
{
    public class ClsType
    {
        public string sGroupID;
        public string sGroupName;
        public string sUserID;
        public string sUserFullName;
        public string sAccessMenu;
        public string sUserTechnicianID;
        public string sUserMarketingID;

        public enum ErrCode
        {
            SUCCESS = 0,
            NEW_USER = 1,
            PASS_EXPIRED = 2,
            OTHER_ERROR = 3
        }


        public enum TrxType
        {
            ProjectCode = 0,
            BusinessUnit = 1,
            ProjectType = 2,
            Status = 3,
            Task = 4,
            Rating = 5
        }

        public enum HistoryType
        {
            ProjectHis = 0,
            TaskHis = 1
        }

        public enum TypeAcc
        {
            ICBS = 0,
            SCORE = 1
        }
        public void setAttributes(TextBox txtBox, string strSQL, string sDBConn)
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
        public string CheckNbsp(string sIn)
        {
            string sOut = sIn;
            try
            {
                if (sIn.ToUpper().Trim() == "&NBSP;")
                {
                    sOut = "";
                }
            }
            catch (Exception ex)
            {
                sOut = "";
            }
            return sOut;
        }
        public void Gv_PageIndexChanging(GridView Gv, int pageIndex, object dsRec, Label LblPaging, string sFieldSort = "", string sDirSort = "")
        {
            try
            {
                if (sFieldSort != "")
                {
                    (dsRec as DataSet).Tables[0].DefaultView.Sort = sFieldSort + " " + sDirSort;
                }
                Gv.DataSource = (dsRec as DataSet).Tables[0];
                Gv.PageIndex = pageIndex;
                Gv.DataBind();
                showPaging((dsRec as DataSet), Gv, LblPaging);
                if (sFieldSort != "")
                {
                    setSorting(Gv, sFieldSort, sDirSort);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void Open_Combos(object cmbServerID, string v1, object p, string v2)
        {
            throw new NotImplementedException();
        }

        public string Gv_Sorting(GridView Gv, object dsRec, string sCurrentFieldSort = "", string sCurrentDirSort = "", string sNewFieldSort = "")
        {
            string sNewDirSort = "";
            try
            {
                sNewDirSort = getSortDirection(sCurrentFieldSort, sCurrentDirSort, sNewFieldSort);
                Recordset Recs = new Recordset();
                Recs.RecData = (System.Data.DataSet)dsRec;
                if (Recs.RecData != null)
                {
                    Recs.RecData.Tables[0].DefaultView.Sort = sNewFieldSort + " " + sNewDirSort;
                    Gv.DataSource = Recs.RecData.Tables[0];
                    Gv.DataBind();
                    setSorting(Gv, sNewFieldSort, sNewDirSort);
                }
            }
            catch
            {
                sNewDirSort = "ASC";
            }
            return sNewDirSort;
        }
        private string getSortDirection(string currentField, string currentDir, string newField)
        {
            string newDir = "";
            try
            {
                if (currentField.ToUpper().Trim() == newField.ToUpper().Trim())
                {
                    if (currentDir.ToUpper().Trim() == "ASC")
                    {
                        newDir = "DESC";
                    }
                    else
                    {
                        newDir = "ASC";
                    }
                }
                else
                {
                    newDir = "ASC";
                }
            }
            catch
            {
                newDir = "ASC";
            }

            return newDir;
        }

        internal void Open_Combos(object cmbIT, string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        public DataSet Open_GridView(GridView Gv, string strSQL, string sDBConn, Label lblPaging, string sFieldSort = "", string sDirSort = "")
        {
            DataSet dsOut;
            try
            {
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, sDBConn);
                var ds = Rec.RecData;

                // Guard against DataSet with 0 tables.
                // GridView.DataBind() will throw: "The IListSource does not contain any data sources."
                if (ds == null || ds.Tables.Count == 0)
                {
                    var dtEmpty = new System.Data.DataTable();
                    Gv.DataSource = dtEmpty;
                    Gv.DataBind();

                    dsOut = ds ?? new DataSet();
                    showPaging(dsOut, Gv, lblPaging);
                    return dsOut;
                }

                Gv.DataSource = ds;
                Gv.DataBind();
                dsOut = ds;
                showPaging(dsOut, Gv, lblPaging);
                if (sFieldSort != "")
                {
                    setSorting(Gv, sFieldSort, sDirSort);
                }
            }
            catch (Exception ex)
            {
                // Ensure grid is cleared on errors (avoid inconsistent state).
                try
                {
                    Gv.DataSource = null;
                    Gv.DataBind();
                }
                catch { }

                dsOut = null;
            }
            return dsOut;
        }
        public void setSorting(GridView Gv, string sFieldSort, string sDirSort)
        {
            try
            {
                foreach (DataControlField dcf in Gv.Columns)
                {
                    if (dcf.SortExpression.ToUpper().Trim() == sFieldSort.ToUpper().Trim())
                    {
                        if (sDirSort.ToUpper().Trim() == "ASC")
                        {
                            dcf.HeaderStyle.CssClass = "gv_header_sort_asc";
                        }
                        else
                        {
                            dcf.HeaderStyle.CssClass = "gv_header_sort_desc";
                        }
                    }
                    else
                    {
                        dcf.HeaderStyle.CssClass = "";
                    }
                }
            }
            catch
            {

            }
        }
        public void Open_Combos(DropDownList cmbTemp, string stDBConn, string sSearch, string strSQL)
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
        public void Open_Combos2(DropDownList cmbTemp, string stDBConn, string strSQL)
        {
            try
            {
                Recordset Rec = new Recordset(); string stSQL = ""; ListItem LstItem;
                stSQL = strSQL;
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
        public void showPaging(object dsRec, GridView Gv, Label LblPaging)
        {
            try
            {
                Recordset Rec = new Recordset();
                Rec.RecData = (dsRec as DataSet);
                long _TotalRecs = Rec.RecordCount();
                int _CurrentRecStart = Gv.PageIndex * Gv.PageSize + 1;
                int _CurrentRecEnd = Gv.PageIndex * Gv.PageSize + Gv.Rows.Count;
                if (_TotalRecs == 0) { _CurrentRecStart = 0; }
                LblPaging.Text = string.Format("Displaying {0} to {1} of {2} records found", _CurrentRecStart, _CurrentRecEnd, _TotalRecs);
            }
            catch (Exception ex)
            {

            }
        }
        //public void audit_trails(string sUserID, string sPage, string sProcess, string sDBConn)
        //{
        //    try
        //    {
        //        ExecCommand EC = new ExecCommand(); Int32 intAff = 0;
        //        string strSQL = "";
        //        strSQL = "insert into audit_trails values ('" + sUserID.Trim() + "',getdate(),'" + sPage.Trim() + "','" + sProcess.Trim() + "')";
        //        if (EC.Execute(strSQL, sDBConn, ref intAff))
        //        {
        //            if (intAff > 0)
        //            {
        //                //do something
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public bool appLogin(string stUserID, string sPassword, string sIP, string sDBConn, ref string sErr)
        {
            bool boolOK = false;
            try
            {
                string strSQL = ""; ExecCommand Ec = new ExecCommand(); int iAff = 0; Recordset Rec = new Recordset();
                strSQL = "sp_app_login '" + stUserID + "','" + sPassword + "','" + sIP + "'";
                Ec.Execute(strSQL, sDBConn, ref iAff, ref sErr);
                if (sErr == "")
                {
                    strSQL = "sp_app_login_get_attr '" + stUserID + "'";
                    Rec.Open(strSQL, sDBConn, ref sErr);
                    if (Rec.RecordCount() > 0)
                    {
                        Rec.MoveFirst();
                        sGroupID = Rec.Fields("GroupID");
                        sGroupName = Rec.Fields("GroupName");
                        sUserID = Rec.Fields("UserID");
                        sUserFullName = Rec.Fields("FullName");
                        sUserTechnicianID = Rec.Fields("TechnicianID");
                        sUserMarketingID = Rec.Fields("MarketingID");
                        while (!Rec.EOF)
                        {
                            sAccessMenu = sAccessMenu + "|" + Rec.Fields("MenuID");
                            Rec.MoveNext();
                        }
                        boolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
                boolOK = false;
            }
            return boolOK;
        }
        public string getParameter(string sType, string sCode, string sDBConn)
        {
            string sOut = "";
            Recordset Rec = new Recordset();
            string strSQL = "";
            try
            {
                strSQL = "sp_GET_PARAMETER '" + sType.Trim() + "','" + sCode.Trim() + "'";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    sOut = Rec.Fields("ParValue");
                }
            }
            catch (Exception ex)
            {
                sOut = ex.Message;
            }

            return sOut.Trim();
        }

        //public void ResetCombo(DropDownList objDD, string strDefaultValue)
        //{
        //    try
        //    {
        //        ListItem LstItem = new ListItem();
        //        LstItem.Text = strDefaultValue.Trim();
        //        objDD.SelectedIndex = objDD.Items.IndexOf(LstItem);
        //        objDD.SelectedValue = strDefaultValue.Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public bool IsNotSQLInject(string strValue, ref string strErr)
        {
            bool BoolOK = true;
            try
            {
                if (strValue.IndexOf(",", 1) > 0)
                {
                    BoolOK = false;
                    strErr = "Invalid string value (SQL injection potential)";
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                strErr = ex.Message;
            }

            return BoolOK;
        }

        public string getLastLogon(string strUserID, string strConn)
        {
            string strLogon = "";
            try
            {
                Recordset Rec = new Recordset();
                string strSQL;

                strSQL = "SELECT LastLogon FROM TblUserAuth WHERE UserID='" + strUserID.Trim() + "'";
                Rec.Open(strSQL, strConn.Trim());
                if (Rec.RecordCount() > 0)
                    strLogon = Rec.Fields("LastLogon");
            }
            catch (Exception ex)
            {
                strLogon = "";
            }

            return strLogon.Trim();
        }

        public void WriteINI(string strsection, string strKey, string strkeyvalue, string strfullpath)
        {
            try
            {
                ClsFunc.WritePrivateProfileString(strsection, strKey.ToUpper(), strkeyvalue, strfullpath);
            }
            catch (Exception ex)
            {
            }
        }

        public string ReadIni(string strAppName, string strKeyName, string strFileName, ref string sErr)
        {
            System.Text.StringBuilder strINIreturn = new System.Text.StringBuilder(255);
            try
            {
                ClsFunc.GetPrivateProfileString(strAppName, strKeyName, "", strINIreturn, strINIreturn.Capacity, strFileName);
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }

            return strINIreturn.ToString();
        }

        //public void ComboSelectedIndex(DropDownList Cmb, string strValue)
        //{
        //    try
        //    {
        //        ListItem ListCmb = new ListItem();
        //        ListCmb.Text = strValue.Trim();
        //        Cmb.SelectedIndex = Cmb.Items.IndexOf(ListCmb);
        //        Cmb.SelectedValue = strValue.Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        Cmb.SelectedIndex = 0;
        //    }
        //}

        public bool CheckUserAuthentication(string strUserID, string strDBConn, ref string stUserName, ref string stBranchID, ref string stBranchName, ref string sErr)
        {
            bool BoolOK = false;
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "";
                string stStatus = "";

                strSQL = "sp_GET_USER_STATUS '" + strUserID.Trim() + "'";
                Rec.Open(strSQL, strDBConn);
                if (Rec.RecordCount() > 0)
                {
                    stUserName = Rec.Fields("UserName");
                    stBranchID = Rec.Fields("Branch_ID");
                    stBranchName = Rec.Fields("Branch_Name");
                    stStatus = Rec.Fields("Status");
                    if (stStatus.ToUpper().Trim() == "A")
                    {
                        BoolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                sErr = ex.Message;
            }

            return BoolOK;
        }

        public string getMatrix(string strUserID, string strDBConn)
        {
            string strMatrix = "";
            try
            {
                Recordset Rec = new Recordset();
                Recordset Rec1 = new Recordset();
                string strSQL = "";

                if (strUserID.Trim() != "")
                {
                    strSQL = "sp_GET_MATRIX '" + strUserID.Trim() + "'";
                    Rec.Open(strSQL, strDBConn.Trim());
                    if (Rec.RecordCount() > 0)
                    {
                        Rec.MoveFirst();
                        strMatrix = Rec.Fields("GroupID") + "|";
                        while (!Rec.EOF)
                        {
                            if (strMatrix.Trim() == "")
                                strMatrix = Rec.Fields("MenuID");
                            else
                                strMatrix = strMatrix.Trim() + ";" + Rec.Fields("MenuID");
                            Rec.MoveNext();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strMatrix = "";
            }

            return strMatrix.Trim();
        }

        public bool SudahLogon(bool isLogon)
        {
            bool BoolOK = false;
            try
            {
                if (isLogon == true)
                {
                    BoolOK = true;
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        public string BuildJavaMenuBoot(string sUserID, string strDBConn)
        {
            string strHTML = "";
            try
            {
                Recordset Rec = new Recordset(); int i = 0;
                string strSQL = "sp_get_menus '" + sUserID + "'"; bool IsStart = true; string sMenuParent = "Start"; Int32 iLngMenu = 1;
                string sBg = "bg-orange-active|bg-maroon-gradient|bg-blue-gradient|bg-green-gradient|bg-aqua-gradient|bg-yellow-gradient|bg-fuchsia-active|" +
                             "bg-light-blue-gradient|bg-red-gradient";
                string[] sBgs = sBg.Split('|');

                Rec.Open(strSQL, strDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        if (sMenuParent != Rec.Fields("ParentID"))
                        {
                            if (!IsStart)
                            {
                                strHTML = strHTML + "\r\n" + "</ul></li>";
                                if ((iLngMenu - 1) > Rec.Fields("MenuPos").Length)
                                {
                                    for (int x = 0; x <= (((iLngMenu - 1) - Rec.Fields("MenuPos").Length) - 1); x++)
                                    {
                                        strHTML = strHTML + "\r\n" + "</ul></li>";
                                    }
                                }
                            }
                            else
                            {
                                IsStart = false;
                            }
                            strHTML = strHTML + "\r\n" + "<li Class='treeview'><a href='" + Rec.Fields("MenuURL") + "'><i class='" + Rec.Fields("MenuIcon") + "'></i> <span> " + Rec.Fields("MenuName") + "</span><span Class='pull-right-container'><i Class='fa fa-angle-left pull-right'></i></span></a>";
                            strHTML = strHTML + "\r\n" + "<ul class='treeview-menu'>";
                            sMenuParent = Rec.Fields("MenuID");
                        }
                        else
                        {
                            if (Rec.Fields("IsParent") == "1")
                            {
                                strHTML = strHTML + "\r\n" + "<li Class='treeview'><a href='" + Rec.Fields("MenuURL") + "'><i class='" + Rec.Fields("MenuIcon") + "'></i> <span> " + Rec.Fields("MenuName") + "</span><span Class='pull-right-container'><i Class='fa fa-angle-left pull-right'></i></span></a>";
                                strHTML = strHTML + "\r\n" + "<ul class='treeview-menu'>";
                                sMenuParent = Rec.Fields("MenuID");
                            }
                            else
                            {
                                strHTML = strHTML + "\r\n" + "<li><a href='" + Rec.Fields("MenuURL") + "'><i class='" + Rec.Fields("MenuIcon") + "'></i>" + Rec.Fields("MenuName") + "<span class='pull-right-container'><small class='label pull-right " + sBgs[i].ToString().Trim() + "'>" + Rec.Fields("cSum") + "</small></span>" + "</a></li>";
                            }
                        }
                        iLngMenu = Rec.Fields("MenuPos").Length;
                        Rec.MoveNext();
                        i++; if (i == 8) { i = 0; }
                    }
                    strHTML = strHTML + "\r\n" + "</ul></li>";
                }
                strHTML = strHTML + "\r\n" + "</ul></li>";
                strHTML = strHTML + "\r\n" + "<li><a href='#' onclick='changepass_click();'><i class='fa fa-lock'></i><span>Change Password</span></a></li>";
                strHTML = strHTML + "\r\n" + "<li><a href='#' onclick='logout_click();'><i class='fa fa fa-power-off'></i><span>Sign Out</span></a></li>";
            }
            catch
            {
                strHTML = "";
            }

            return strHTML.Trim();
        }


        public string BuildJavaFMSMenuBoot(string sCompanyID, string sServerID, string strDBConn)
        {
            string strHTML = "";
            try
            {
                Recordset Rec = new Recordset(); int i = 0;
                string strSQL = "sp_get_fms_menus '" + sCompanyID + "','" + sServerID + "'"; bool IsStart = true; string sMenuParent = "Start"; Int32 iLngMenu = 1;
                string sBg = "bg-orange-active|bg-maroon-gradient|bg-blue-gradient|bg-green-gradient|bg-aqua-gradient|bg-yellow-gradient|bg-fuchsia-active|" +
                             "bg-light-blue-gradient|bg-red-gradient";
                string[] sBgs = sBg.Split('|');

                Rec.Open(strSQL, strDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        if (sMenuParent != Rec.Fields("ParentID"))
                        {
                            if (!IsStart)
                            {
                                strHTML = strHTML + "\r\n" + "</ul></div></li>";
                                if ((iLngMenu - 1) > Rec.Fields("MenuPos").Length)
                                {
                                    for (int x = 0; x <= (((iLngMenu - 1) - Rec.Fields("MenuPos").Length) - 1); x++)
                                    {
                                        strHTML = strHTML + "\r\n" + "</ul></div></li>";
                                    }
                                }
                            }
                            else
                            {
                                IsStart = false;
                            }
                            strHTML = strHTML + "\r\n" + "<li class='kt-menu__item  kt-menu__item--submenu' aria-haspopup='true' data-ktmenu-submenu-toggle='hover'><a href='" + Rec.Fields("MenuURL") + "' class='kt-menu__link kt-menu__toggle'><i class='" + Rec.Fields("MenuIcon") + "'><span></span></i><span class='kt-menu__link-text'>" + Rec.Fields("MenuName") + "</span><i class='kt-menu__ver-arrow la la-angle-right'></i></a>";
                            strHTML = strHTML + "\r\n" + "<div class='kt-menu__submenu'><span class='kt-menu__arrow'></span><ul class='kt-menu__subnav'>";
                            sMenuParent = Rec.Fields("MenuID");
                        }
                        else
                        {
                            if (Rec.Fields("IsParent") == "1")
                            {
                                strHTML = strHTML + "\r\n" + "<li class='kt-menu__item  kt-menu__item--submenu' aria-haspopup='true' data-ktmenu-submenu-toggle='hover'><a href='" + Rec.Fields("MenuURL") + "' class='kt-menu__link kt-menu__toggle'><i class='" + Rec.Fields("MenuIcon") + "'><span></span></i><span class='kt-menu__link-text'>" + Rec.Fields("MenuName") + "</span><i class='kt-menu__ver-arrow la la-angle-right'></i></a>";
                                strHTML = strHTML + "\r\n" + "<div class='kt-menu__submenu'><span class='kt-menu__arrow'></span><ul class='kt-menu__subnav'>";
                                sMenuParent = Rec.Fields("MenuID");
                            }
                            else
                            {
                                strHTML = strHTML + "\r\n" + "<li class='kt-menu__item ' aria-haspopup='true'><a href='" + Rec.Fields("MenuURL") + "' class='kt-menu__link'><i class='" + Rec.Fields("MenuIcon") + "'><span></span></i><span class='kt-menu__link-text'>" + Rec.Fields("MenuName") + "</span></a></li>";
                            }
                        }
                        iLngMenu = Rec.Fields("MenuPos").Length;
                        Rec.MoveNext();
                        i++; if (i == 8) { i = 0; }
                    }
                    strHTML = strHTML + "\r\n" + "</ul></div></li>";
                }
                //strHTML = strHTML + "\r\n" + "</ul></li>";
                strHTML = strHTML + "\r\n" + "<li><a href='#' onclick='changepass_click();'><i class='fa fa-lock'></i><span>Change Password</span></a></li>";
                strHTML = strHTML + "\r\n" + "<li><a href='#' onclick='logout_click();'><i class='fa fa fa-power-off'></i><span>Sign Out</span></a></li>";
            }
            catch
            {
                strHTML = "";
            }

            return strHTML.Trim();
        }


        public string BuildDashMenu(string sUserID, string strDBConn)
        {
            string strHTML = "";
            try
            {
                Recordset Rec = new Recordset(); int i = 0;
                string strSQL = "sp_get_menus_parent '" + sUserID + "'";
                string sBg = "bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient|bg-aqua-gradient|bg-black-gradient|bg-fuchsia-active|bg-light-blue-gradient|bg-maroon-gradient|bg-orange-active|bg-red-gradient|bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient|bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient|bg-aqua-gradient|bg-black-gradient|bg-fuchsia-active|bg-light-blue-gradient|bg-maroon-gradient|bg-orange-active|bg-red-gradient|bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient|bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient|bg-aqua-gradient|bg-black-gradient|bg-fuchsia-active|bg-light-blue-gradient|bg-maroon-gradient|bg-orange-active|bg-red-gradient|bg-orange-active|bg-red-gradient|bg-blue-gradient|bg-green-gradient";
                string[] sBgs = sBg.Split('|');

                Rec.Open(strSQL, strDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        if (strHTML == "")
                        {
                            strHTML = "<a id='" + Rec.Fields("MenuID") + "' class='btn btn-app " + sBgs[i].ToString().Trim() + "' style='border-radius:14px;'><i class='" + Rec.Fields("MenuIcon") + "'></i>" + Rec.Fields("MenuName") + "</a>";
                        }
                        else
                        {
                            strHTML = strHTML + "\r\n" + "<a id='" + Rec.Fields("MenuID") + "' class='btn btn-app " + sBgs[i].ToString().Trim() + "' style='border-radius:14px;'><i class='" + Rec.Fields("MenuIcon") + "'></i>" + Rec.Fields("MenuName") + "</a>";
                        }
                        Rec.MoveNext();
                        i++;
                    }
                }
            }
            catch
            {
                strHTML = "";
            }

            return strHTML.Trim();
        }

        public string getMenus(string sUserID, string sDBConn)
        {
            string sOut = "";
            try
            {
                string strSQL = "";
                Recordset Rec = new Recordset();

                strSQL = "sp_get_menus '" + sUserID.Trim() + "'";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        sOut = sOut + "|" + Rec.Fields("menu_id").ToUpper();
                        Rec.MoveNext();
                    }
                }
                else
                    sOut = "";
            }
            catch (Exception ex)
            {
                sOut = "";
            }

            return sOut.Trim();
        }

        //public bool AuditLog(string xUserID, string DBConn, string xPage, string xOperation, string xDesc = "")
        //{
        //    bool BoolOK = true;
        //    try
        //    {
        //        ExecCommand EC = new ExecCommand(); Int32 intAff = 0;
        //        string strSQL;

        //        strSQL = "INSERT INTO AuditLog values (getdate(),'" + xUserID.Trim() + "'," + "'" + xPage.Trim() + "','" + xOperation.Trim() + "','" + xDesc.Trim() + "')";
        //        EC.Execute(strSQL, DBConn, ref intAff);
        //    }
        //    catch (Exception ex)
        //    {
        //        BoolOK = false;
        //    }

        //    return BoolOK;
        //}

        public bool RegisterUser(string strUser, string strName, string ConnString, string sUnit, string sLastLogon, string sStatus, string sUnitCode)
        {
            bool BoolOK = true;
            try
            {
                Recordset sRec = new Recordset(); Int32 intAff = 0;
                ExecCommand sEC = new ExecCommand();
                string strSQL = "";
                strSQL = "select a.*,b.UnitName from tbl_User a left join tbl_Unit b on a.unit=b.unit where a.UserID='" + strUser.Trim() + "'";
                sRec.Open(strSQL, ConnString);
                if (sRec.RecordCount() > 0)
                {
                    sUnitCode = sRec.Fields("Unit").Trim();
                    sUnit = sRec.Fields("UnitName").Trim();
                    sLastLogon = sRec.Fields("LastLogon").Trim();
                    sStatus = sRec.Fields("Status").Trim();
                }
                else
                {
                    strSQL = "Insert into tbl_User values ('" + strUser.Trim() + "','" + strName.Trim() + "','',null,'I','SYSTEM',getdate())";
                    sEC.Execute(strSQL, ConnString, ref intAff);
                    sUnitCode = "";
                    sUnit = "";
                    sLastLogon = "";
                    sStatus = "I";
                }
                strSQL = "Update tbl_User set LastLogon=getdate() where UserID='" + strUser.Trim() + "'";
                sEC.Execute(strSQL, ConnString, ref intAff);
            }
            catch (Exception ex)
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        public bool GenerateFileDefferedUpload(string sBranch, string sDateAmort, string sDBConn, string sFullPath, ref string sErr, bool sUpdate = false)
        {
            bool BoolOK = false;
            StreamWriter sW = new StreamWriter(sFullPath, false);
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "";
                if (sUpdate == false)
                    strSQL = "sp_generate_deffered_amortization '" + sDateAmort.Trim() + "','" + sBranch.Trim() + "'";
                else
                    strSQL = "sp_generate_deffered_amortization_1 '" + sDateAmort.Trim() + "','" + sBranch.Trim() + "'";

                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    while (!Rec.EOF)
                    {
                        sW.WriteLine(Rec.Fields("sOuts"));
                        Rec.MoveNext();
                    }
                }
                BoolOK = true;
            }
            catch (Exception ex)
            {
                BoolOK = false;
                sErr = ex.Message;
            }

            if (sW != null)
            {
                sW.Close();
            }
            return BoolOK;
        }

        public bool GenerateTrxFile(string sBatchNo, string stUserID, string sDBConn, string sFullPath, string sFullPathRpt, ref string stErr)
        {
            bool BoolOK = true;
            StreamWriter sW = new StreamWriter(sFullPath, false);
            StreamWriter sW_Rpt = new StreamWriter(sFullPathRpt, false);
            try
            {
                Recordset Rec = new Recordset();
                string strSQL = "";

                strSQL = "sp_get_recordset_mcap '" + sBatchNo.Trim() + "','" + stUserID.Trim() + "'";
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    sW_Rpt.WriteLine("<html><head id='Head1'><title>	.:: CTBC Bank ::. Management Information System</title><script type='text/javascript' language='javascript'>function print_click(obj){obj.style.visibility='hidden';window.print();obj.style.visibility='';}</script></head><body><table cellspacing='0' style='width:100%'><tr><td style='height:30px;width:183px;'><img src='../Images/logoz.jpg' /></td><td style='width:auto;text-align:right;'><img id='img_print' src='../Images/button/print3.gif' onclick='print_click(img_print);' /></td></table><table cellspacing='0' style='width:100%;font-family:Tahoma;font-size:11px;'></tr><tr style='height:15px;'><td colspan='9'></td></tr><tr><td colspan='9' style='font-size:13px;font-weight:bold;'>FINANCIAL CONTROL - PSAK ADJUSTMENT</td></tr><tr><td colspan='4'>MCAP Upload</td><td colspan='2'>Batch No : " + ClsFunc.Right("0000" + sBatchNo.Trim(), 4) + "</td><td colspan='3' style='text-align:right;'>Batch Date : " + Convert.ToDateTime(Rec.Fields("batch_date").Trim()).ToString("dd MMM yyyy") + "</td></tr><tr style='height:5px;'><td colspan='9'><hr /></td></tr><tr style='font-weight:bold;'><td>Reference No</td><td>Account Code</td><td>Currency</td><td>Trx Type</td><td style='text-align:right;padding-right:5px;'>Amount</td><td>Value Date Live</td><td>Value Date Backup</td><td>Narrative</td><td>BU</td></tr><tr style='height:5px;'><td colspan='9'><hr /></td></tr>");
                    while (!Rec.EOF)
                    {
                        sW.WriteLine(Rec.Fields("Outs").Trim());
                        sW_Rpt.WriteLine("<tr style='font-weight:normal;'><td>" + Rec.Fields("ref_no").Trim() + "</td><td>" + Rec.Fields("account_code").Trim() + "</td><td>" + Rec.Fields("ccy").Trim() + "</td><td>" + Rec.Fields("trx_type").Trim() + "</td><td style='text-align:right;padding-right:5px;'>" + System.Convert.ToDouble(Rec.Fields("amount").Trim()).ToString("#,##0.00") + "</td><td>" + Rec.Fields("value_date_live").Trim() + "</td><td>" + Rec.Fields("value_date_backup").Trim() + "</td><td>" + Rec.Fields("narrative").Trim() + "</td><td>" + Rec.Fields("bu").Trim() + "</td></tr>");
                        Rec.MoveNext();
                    }
                    Rec.MoveLast();
                    sW_Rpt.Write("<tr><td colspan='9'><hr /></td></tr><tr><td colspan='2'></td><td>Total</td><td>Credit</td><td style='text-align:right;padding-right:5px;'>" + System.Convert.ToDouble(Rec.Fields("Total_Amounts_Credit").Trim()).ToString("#,##0.00") + "</td><td colspan='4'></td></tr><tr><td colspan='2'></td><td>Total</td><td>Debit</td><td style='text-align:right;padding-right:5px;'>" + System.Convert.ToDouble(Rec.Fields("Total_Amounts_Debit").Trim()).ToString("#,##0.00") + "</td><td colspan='4'></td></tr><tr style='height:50px;'><td colspan='9'></td></tr></table><table cellspacing='0' style='width:100%;font-family:Tahoma;font-size:11px;'><tr><td style='text-align:center;width:50%;'>Maker</td><td style='text-align:center;width:50%;'>Approval</td></tr><tr style='height:75px;'><td colspan='2'></td></tr><tr><td style='text-align:center;width:50%;'>" + Rec.Fields("maker").Trim() + "</td><td style='text-align:center;width:50%;'>" + Rec.Fields("approver").Trim() + "</td></tr>");
                    sW_Rpt.Write("</table></body></html>");
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                stErr = ex.Message;
            }

            if (sW != null)
            {
                sW.Close();
            }

            if (sW_Rpt != null)
            {
                sW_Rpt.Close();
            }

            return BoolOK;
        }

        public bool ExportToCsvTab(Recordset Rec, string Title, string FullPathDestination, long LimitFile, ref string strMsg)
        {
            bool BoolOK = false;
            try
            {
                long SumFld; long SumRec; string StrFld; string strValue; long O; string strFileOutput;
                long intFile; long intStart; long intFinish;

                Rec.MoveFirst();
                strValue = "";
                StrFld = "";
                SumFld = 0;
                SumFld = Rec.FieldCount();
                SumRec = Rec.RecordCount();
                //LimitFile = 50000
                intFile = (SumRec / LimitFile);
                if ((SumRec % LimitFile) > 0)
                {
                    intFile = intFile + 1;
                }

                if (SumFld > 0)
                {
                    for (O = 1; O <= intFile; O++)
                    {
                        StrFld = "";
                        for (int i = 0; i <= (SumFld - 1); i++)
                        {
                            StrFld = StrFld + @"""" + Rec.FieldName(i).ToString().Trim() + @"""" + ",";
                        }
                        intStart = ((O - 1) * LimitFile) + 1;
                        if ((O * LimitFile) > SumRec) { intFinish = SumRec; } else { intFinish = (O * LimitFile); }

                        strFileOutput = FullPathDestination.Trim();
                        using (StreamWriter sw = new StreamWriter(strFileOutput))
                        {
                            if (Title.Trim() != "")
                            {
                                sw.WriteLine(Title);
                            }
                            StrFld = StrFld.Trim();
                            StrFld = ClsFunc.Left(StrFld, (StrFld.Length) - 1);
                            sw.WriteLine(StrFld);
                            Rec.AbsolutePosition = intStart - 1;
                            while (!Rec.EOF)
                            {
                                strValue = "";
                                for (var x = 0; x <= (SumFld - 1); x++)
                                {
                                    strValue = strValue + @"""" + Rec.Fields(x).ToString() + @"""" + ",";
                                }

                                strValue = strValue.Trim();
                                strValue = ClsFunc.Left(strValue, (strValue.Length) - 1);
                                sw.WriteLine(strValue);
                                if (Rec.AbsolutePosition == intFinish)
                                {
                                    break;
                                }
                                Rec.MoveNext();

                            }
                        }
                        strMsg = strMsg + " " + strFileOutput.Trim();
                    }
                    strMsg = "Export to :" + " " + strMsg.Trim() + " " + "done !";
                    BoolOK = true;
                }
                else
                {
                    strMsg = "Export Failed !";
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                strMsg = ex.Message;
            }

            return BoolOK;
        }

        //public bool ExportToXls(Recordset Rec, string Title, string FullPathDestination, long LimitFile, ref string strMsg)
        //{
        //    bool BoolOK = false;
        //    try
        //    {

        //        GridView gv = new GridView();
        //        StringWriter sw = new StringWriter();
        //        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //        gv.DataSource = Rec.RecData;
        //        gv.RenderControl(hw);


        //        long SumFld; long SumRec; string StrFld; string strValue; long O; string strFileOutput;
        //        long intFile; long intStart; long intFinish;

        //        Rec.MoveFirst();
        //        strValue = "";
        //        StrFld = "";
        //        SumFld = 0;
        //        SumFld = Rec.FieldCount();
        //        SumRec = Rec.RecordCount();
        //        //LimitFile = 50000
        //        intFile = (SumRec / LimitFile);
        //        if ((SumRec % LimitFile) > 0)
        //        {
        //            intFile = intFile + 1;
        //        }

        //        if (SumFld > 0)
        //        {
        //            for (O = 1; O <= intFile; O++)
        //            {
        //                StrFld = "";
        //                for (int i = 0; i <= (SumFld - 1); i++)
        //                {
        //                    StrFld = StrFld + @"""" + Rec.FieldName(i).ToString().Trim() + @"""" + ",";
        //                }
        //                intStart = ((O - 1) * LimitFile) + 1;
        //                if ((O * LimitFile) > SumRec) { intFinish = SumRec; } else { intFinish = (O * LimitFile); }

        //                strFileOutput = FullPathDestination.Trim();
        //                using (StreamWriter sw = new StreamWriter(strFileOutput))
        //                {
        //                    if (Title.Trim() != "")
        //                    {
        //                        sw.WriteLine(Title);
        //                    }
        //                    StrFld = StrFld.Trim();
        //                    StrFld = ClsFunc.Left(StrFld, (StrFld.Length) - 1);
        //                    sw.WriteLine(StrFld);
        //                    Rec.AbsolutePosition = intStart - 1;
        //                    while (!Rec.EOF)
        //                    {
        //                        strValue = "";
        //                        for (var x = 0; x <= (SumFld - 1); x++)
        //                        {
        //                            strValue = strValue + @"""" + Rec.Fields(x).ToString() + @"""" + ",";
        //                        }

        //                        strValue = strValue.Trim();
        //                        strValue = ClsFunc.Left(strValue, (strValue.Length) - 1);
        //                        sw.WriteLine(strValue);
        //                        if (Rec.AbsolutePosition == intFinish)
        //                        {
        //                            break;
        //                        }
        //                        Rec.MoveNext();

        //                    }
        //                }
        //                strMsg = strMsg + " " + strFileOutput.Trim();
        //            }
        //            strMsg = "Export to :" + " " + strMsg.Trim() + " " + "done !";
        //            BoolOK = true;
        //        }
        //        else
        //        {
        //            strMsg = "Export Failed !";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BoolOK = false;
        //        strMsg = ex.Message;
        //    }

        //    return BoolOK;
        //}

        public string getBatchNoUpload(string sBatchCode, string sDBConn)
        {
            string sOut = "";
            try
            {
                string strSQL = "sp_get_batch_no_upload '" + sBatchCode.Replace("'", "''") + "'";
                Recordset Rec = new Recordset();
                Rec.Open(strSQL, sDBConn);
                if (Rec.RecordCount() > 0)
                {
                    sOut = Rec.Fields("BatchNo");
                }
                Rec.Close();
            }
            catch (Exception)
            {
                sOut = "";
            }

            return sOut;
        }

        public bool ImportCsvGsmMutation(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("BGM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(strValue))
                        {
                            if (!strValue.ToUpper().Contains("MSIDN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sqlCmd = $"sp_insert_gsm_mutation_upload '{stSplit[0]}','{stBatchNo}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{sFileName}','{sUserID}'";
                                        Ec.Execute(sqlCmd, sDBConn, ref iReff);
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }


        public bool ImportCsvDeviceMutation(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("BDM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(strValue))
                        {
                            if (!strValue.ToUpper().Contains("NOSN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sqlCmd = $"sp_insert_device_mutation_upload '{stSplit[0]}','{stBatchNo}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{sFileName}','{sUserID}'";
                                        Ec.Execute(sqlCmd, sDBConn, ref iReff);
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }


        public bool ImportCsvGsmMutationProcurement(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                string strSQL = "";
                bool isTrueFile = false;
                int i = 0;
                int iReff = 0;
                stBatchNo = getBatchNoUpload("BGM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("MSIDN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (stSplit[0].ToString() != "")
                                    {
                                        string singleCall = $"sp_insert_gsm_mutation_upload_approve '{stSplit[0]}','{stSplit[1]}','{stBatchNo}','{stSplit[2]}','{stSplit[3]}','{stSplit[4]}','{sFileName}','{sUserID}'";

                                        if (i <= 100)
                                        {
                                            strSQL += " \r\n " + singleCall;
                                            i++;
                                        }
                                        else
                                        {
                                            Ec.Execute(strSQL.Trim(), sDBConn, ref iReff);
                                            strSQL = singleCall;
                                            i = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(strSQL))
                    {
                        Ec.Execute(strSQL.Trim(), sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvDeviceMutationProcurement(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                string strSQL = "";
                bool isTrueFile = false;
                int i = 0;
                int iReff = 0;
                stBatchNo = getBatchNoUpload("BDM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("NOSN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (stSplit[0].ToString() != "")
                                    {
                                        string singleCall = $"sp_insert_device_mutation_upload_approve '{stSplit[0]}','{stSplit[1]}','{stBatchNo}','{stSplit[2]}','{stSplit[3]}','{stSplit[4]}','{sFileName}','{sUserID}'";

                                        if (i <= 100)
                                        {
                                            strSQL += " \r\n " + singleCall;
                                            i++;
                                        }
                                        else
                                        {
                                            Ec.Execute(strSQL.Trim(), sDBConn, ref iReff);
                                            strSQL = singleCall;
                                            i = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(strSQL))
                    {
                        Ec.Execute(strSQL.Trim(), sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvGsm(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("BGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("MSIDN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (stSplit.Length >= 9 && !string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sqlCmd = $"sp_insert_gsm_upload '{stSplit[0]}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{stBatchNo}'," +
                                                        $"'{stSplit[4]}','{stSplit[5]}','{stSplit[6]}','{stSplit[7]}','{stSplit[8]}','{sFileName}','{sUserID}'";

                                        //Ec.Execute(sqlCmd, sDBConn, ref iReff); // Eksekusi satu per satu
                                        string execErr = "";
                                        bool ok = Ec.Execute(sqlCmd, sDBConn, ref iReff, ref execErr);

                                        if (!ok)
                                        {
                                            boolOK = false;
                                            sErr = string.IsNullOrWhiteSpace(execErr) ? "Gagal eksekusi stored procedure." : execErr;
                                            break; // stop proses, biar status balik false dan kebaca di UI
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }


        public bool ImportCsvDevice(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("BDU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("NOSN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (stSplit.Length >= 11 && !string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sqlCmd = $"sp_insert_device_upload '{stSplit[0]}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{stSplit[4]}'," +
                                                        $"'{stSplit[5]}','{stBatchNo}','{stSplit[6]}','{stSplit[7]}','{stSplit[8]}','{stSplit[9]}','{stSplit[10]}','{stSplit[11]}','{stSplit[12]}','{sFileName}','{sUserID}'";

                                        // Eksekusi satu per satu, karena ExecCommand.Execute hanya mendukung satu SP per call
                                        //Ec.Execute(sqlCmd, sDBConn, ref iReff);
                                        string execErr = "";
                                        bool ok = Ec.Execute(sqlCmd, sDBConn, ref iReff, ref execErr);

                                        if (!ok)
                                        {
                                            boolOK = false;
                                            sErr = string.IsNullOrWhiteSpace(execErr) ? "Gagal eksekusi stored procedure." : execErr;
                                            break; // stop proses, biar status balik false dan kebaca di UI
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public DataSet GetDataset(string sql, string connStr)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    conn.Open();
                    da.Fill(ds);
                }
            }

            return ds;
        }

        public bool ImportCsvJobMaint(string sFile, string sFileName, string sUserID, string sDBConn, ref string strJobID, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("SN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sqlCmd = $"sp_insert_job_order_maint_detail_upload '{strJobID}', '{stSplit[0]}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{sUserID}'";

                                        Ec.Execute(sqlCmd, sDBConn, ref iReff); // Eksekusi satu per satu
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }
        public bool ImportCsvGsmProcurement(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = ""; string[] stSplit; ExecCommand Ec = new ExecCommand(); string strSQL = ""; bool isTrueFile = false;
                int i = 0; int iReff = 0;
                stBatchNo = getBatchNoUpload("BGU", sDBConn);
                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("MSIDN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.ToUpper().Contains(";") ? strValue.Split(';') : strValue.Split(',');
                                    if (stSplit[0].ToString() != "")
                                    {
                                        string sql = "dbo.sp_insert_gsm_upload_procurement '" + stSplit[0] + "','" + stSplit[1] + "','" + stSplit[2] + "','" + stSplit[3] + "','" + stSplit[4] + "','" + stBatchNo + "','" +
                                                     stSplit[5] + "','" + stSplit[6] + "','" + stSplit[7] + "','" + stSplit[8] + "','" + sFileName + "','" + sUserID + "'";

                                        if (i <= 100)
                                        {
                                            strSQL += sql + "; ";
                                            i++;
                                        }
                                        else
                                        {
                                            Ec.Execute(strSQL, sDBConn, ref iReff);
                                            strSQL = sql + "; ";
                                            i = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                    if (strSQL != "")
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }
                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvDeviceProcurement(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = ""; string[] stSplit; ExecCommand Ec = new ExecCommand(); string strSQL = ""; bool isTrueFile = false;
                int i = 0; int iReff = 0;
                stBatchNo = getBatchNoUpload("BDU", sDBConn);
                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (strValue.Trim() != "")
                        {
                            if (!strValue.ToUpper().Contains("NOSN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.ToUpper().Contains(";") ? strValue.Split(';') : strValue.Split(',');
                                    if (stSplit[0].ToString() != "")
                                    {
                                        string sql = "dbo.sp_insert_device_upload_procurement '" + stSplit[0] + "','" + stSplit[1] + "','" + stSplit[2] + "','" + stSplit[3] + "','" + stSplit[4] + "','" +
                                                     stSplit[5] + "','" + stSplit[6] + "','" + stBatchNo + "','" + stSplit[7] + "','" + stSplit[8] + "','" +
                                                     stSplit[9] + "','" + stSplit[10] + "','" + stSplit[11] + "','" + sFileName + "','" + sUserID + "'";

                                        if (i <= 100)
                                        {
                                            strSQL += sql + "; ";
                                            i++;
                                        }
                                        else
                                        {
                                            Ec.Execute(strSQL, sDBConn, ref iReff);
                                            strSQL = sql + "; ";
                                            i = 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                    if (strSQL != "")
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }
                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }


        public bool ImportCsvStock(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("OPN", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();

                        if (!string.IsNullOrWhiteSpace(strValue))
                        {
                            if (!strValue.ToUpper().Contains("MSIDN"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                    {
                                        string sql = $"dbo.sp_insert_igo_stock_upload '{stSplit[0]}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{stSplit[4]}','{stSplit[5]}','{stBatchNo}'";
                                        Ec.Execute(sql, sDBConn, ref iReff);
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvVehicle(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int iReff = 0;

                stBatchNo = getBatchNoUpload("BVU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();

                        if (!string.IsNullOrWhiteSpace(strValue))
                        {
                            if (!strValue.ToUpper().Contains("POLICENO"))
                            {
                                if (isTrueFile)
                                {
                                    stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                    if (!string.IsNullOrWhiteSpace(stSplit[4]))
                                    {
                                        string sql = $"dbo.sp_insert_vehicle_upload '{stSplit[0]}','{stSplit[1]}','{stSplit[2]}','{stSplit[3]}','{stSplit[4]}'," +
                                                     $"'{stSplit[5]}','{stSplit[6]}','{stSplit[7]}','{stSplit[8]}','{stBatchNo}','{stSplit[9]}','{stSplit[10]}','{sFileName}','{sUserID}'";

                                        Ec.Execute(sql, sDBConn, ref iReff);
                                    }
                                }
                            }
                            else
                            {
                                isTrueFile = true;
                            }
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }


        public bool ImportCsvDeviceDO(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("BDU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("NOSN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_device_upload_delivery_order '" + stSplit[0] + "','" + stSplit[1] + "'," +
                                                       "'" + stSplit[2] + "','" + stSplit[3] + "','" + stSplit[4] + "'," +
                                                       "'" + stSplit[5] + "','" + stSplit[6] + "','" + stSplit[7] + "','" + stBatchNo + "','" + stSplit[8] + "'," +
                                                       "'" + stSplit[9] + "','" + stSplit[10] + "','" + stSplit[11] + "','" + stSplit[12] + "','" + sFileName + "','" + sUserID + "'";

                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1; // mulai batch baru dengan current row
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvGsmDO(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("BGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_gsm_upload_delivery_order '" + stSplit[0] + "','" + stSplit[1] + "'," +
                                                       "'" + stSplit[2] + "','" + stSplit[3] + "','" + stSplit[4] + "','" + stSplit[5] + "','" + stBatchNo + "'," +
                                                       "'" + stSplit[6] + "','" + stSplit[7] + "','" + stSplit[8] + "','" + stSplit[9] + "','" + sFileName + "','" + sUserID + "'";

                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1; // mulai batch baru
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvDeviceMutationDO(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("BDM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("NOSN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_device_mutation_upload_do '" + stSplit[0] + "','" + stSplit[1] + "','" + stSplit[2] + "','" + stBatchNo + "','" + stSplit[3] + "','" + stSplit[4] + "','" + stSplit[5] + "','" + sFileName + "','" + sUserID + "'";

                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvGsmMutationDO(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("BGM", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_gsm_mutation_upload_do '" + stSplit[0] + "','" + stSplit[1] + "','" + stSplit[2] + "','" + stBatchNo + "','" + stSplit[3] + "','" + stSplit[4] + "','" + stSplit[5] + "','" + sFileName + "','" + sUserID + "'";

                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvJoDetail(string sFile, string sFileName, string sJobID, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("BJU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("NOSN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_job_main_detail_upload '" + stSplit[0] + "','" + stBatchNo + "','" + stSplit[1] + "','" + stSplit[2] + "','" + stSplit[3] + "','" + sJobID + "','" + sFileName + "','" + sUserID + "'";
                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvGsmProvider(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                // pastikan stErr sudah tidak null
                if (sErr == null) sErr = "";

                stBatchNo = getBatchNoUpload("XGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_gsm_upload_provider '" + stSplit[0] + "','" + stBatchNo + "','" + sFileName + "','" + sUserID + "'";
                                    Ec.Execute(singleSQL, sDBConn, ref iReff, ref sErr);
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        public bool ImportCsvGsmActivation(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                if (sErr == null) sErr = "";

                stBatchNo = getBatchNoUpload("XGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    // Tetap pakai SP yang sama, tanpa kumpulan batch SQL
                                    string singleSQL = "dbo.sp_insert_gsm_upload_activation '" + stSplit[0] + "','" + stBatchNo + "','" + sFileName + "','" + stSplit[1] + "','" + sUserID + "'";
                                    Ec.Execute(singleSQL, sDBConn, ref iReff, ref sErr);
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }

        /**
        public bool ImportCsvGsmProvider(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("XGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_gsm_upload_provider '" + stSplit[0] + "','" + stBatchNo + "','" + sFileName + "','" + sUserID + "'";
                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }
       

        public bool ImportCsvGsmActivation(string sFile, string sFileName, string sUserID, string sDBConn, ref string stBatchNo, ref string sErr)
        {
            bool boolOK = true;
            try
            {
                string strValue = "", strSQL = "";
                string[] stSplit;
                ExecCommand Ec = new ExecCommand();
                bool isTrueFile = false;
                int i = 0, iReff = 0;

                stBatchNo = getBatchNoUpload("XGU", sDBConn);

                using (StreamReader sr = new StreamReader(sFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strValue = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(strValue)) continue;

                        if (!strValue.ToUpper().Contains("MSIDN"))
                        {
                            if (isTrueFile)
                            {
                                stSplit = strValue.Contains(";") ? strValue.Split(';') : strValue.Split(',');

                                if (!string.IsNullOrWhiteSpace(stSplit[0]))
                                {
                                    string singleSQL = "dbo.sp_insert_gsm_upload_activation '" + stSplit[0] + "','" + stBatchNo + "','" + sFileName + "','" + stSplit[1] + "','" + sUserID + "'";
                                    strSQL += (strSQL != "" ? ";" : "") + singleSQL;
                                    i++;

                                    if (i > 100)
                                    {
                                        Ec.Execute(strSQL, sDBConn, ref iReff);
                                        strSQL = "";
                                        i = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTrueFile = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(strSQL))
                    {
                        Ec.Execute(strSQL, sDBConn, ref iReff);
                    }
                }

                if (!isTrueFile)
                {
                    boolOK = false;
                    sErr = "Invalid upload file";
                }
            }
            catch (Exception ex)
            {
                boolOK = false;
                sErr = ex.Message;
            }

            return boolOK;
        }
        */
    }

}