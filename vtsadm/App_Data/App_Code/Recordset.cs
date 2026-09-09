using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace vtsadm.App_Code
{
    public class Recordset
    {
        public System.Data.DataSet RecData = new System.Data.DataSet();
        private System.Data.CommandType CommType;
        private OleDbCommand OleComm;
        private OleDbConnection OleConn = new OleDbConnection();
        private OleDbDataAdapter OleAdap = new OleDbDataAdapter();
        public long AbsolutePosition;
        public bool EOF;

        public enum DatabaseType
        {
            SQL_Client = 0,
            ORA_Client = 1
        }

        public int FieldCount()
        {
            int intRes = 0;
            try
            {
                intRes = RecData.Tables[0].Columns.Count;
            }
            catch
            {
            }

            return intRes;
        }

        public int FieldCount(ref string stErr)
        {
            int intRes = 0;
            try
            {
                intRes = RecData.Tables[0].Columns.Count;
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }

            return intRes;
        }

        public long RecordCount()
        {
            long lngRes = 0;
            try
            {
                lngRes = RecData.Tables[0].Rows.Count;
            }
            catch
            {
            }

            return lngRes;
        }

        public long RecordCount(ref string stErr)
        {
            long lngRes = 0;
            try
            {
                lngRes = RecData.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }

            return lngRes;
        }

        /*public void Open(string strSQL, string ConnString, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            try
            {
                OleDbConnection OleConn = new OleDbConnection(ConnString);
                if (OpenConnection(OleConn, ConnString))
                {
                    OleComm = new OleDbCommand(strSQL, OleConn);
                    OleComm.CommandType = System.Data.CommandType.Text;
                    OleAdap = new OleDbDataAdapter();
                    OleAdap.SelectCommand = OleComm;
                    OleAdap.Fill(RecData, "Table1");
                    if (RecordCount() > 0)
                        EOF = false;
                    else
                        EOF = true;
                }
            }
            catch (Exception ex)
            {

            }
        }*/
        /*public void Open(string strSQL, string ConnString, ref string stErr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            try
            {
                OleDbConnection OleConn = new OleDbConnection(ConnString);
                if (OpenConnection(OleConn, ConnString))
                {
                    OleComm = new OleDbCommand(strSQL, OleConn);
                    OleComm.CommandType = System.Data.CommandType.Text;
                    OleAdap = new OleDbDataAdapter();
                    OleAdap.SelectCommand = OleComm;
                    OleAdap.Fill(RecData, "Table1");
                    if (RecordCount() > 0)
                        EOF = false;
                    else
                        EOF = true;
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }*/
        public void Open(string strSQL, string ConnString, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            try
            {
                using (OleDbConnection OleConn = new OleDbConnection(ConnString))
                {
                    OleConn.Open();

                    string procName = strSQL.Split(' ')[0];
                    string paramString = strSQL.Substring(procName.Length).Trim();

                    List<object> paramList = ParseSqlParams(paramString);

                    using (OleDbCommand cmd = new OleDbCommand(procName, OleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var param in paramList)
                        {
                            cmd.Parameters.AddWithValue("?", param ?? DBNull.Value);
                        }

                        OleAdap = new OleDbDataAdapter(cmd);
                        RecData = new DataSet();
                        OleAdap.Fill(RecData, "Table1");

                        EOF = RecordCount() == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                EOF = true;
            }
        }
        public void Open(string strSQL, string ConnString, ref string stErr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            try
            {
                using (OleDbConnection OleConn = new OleDbConnection(ConnString))
                {
                    OleConn.Open();

                    string procName = strSQL.Split(' ')[0];
                    string paramString = strSQL.Substring(procName.Length).Trim();

                    List<object> paramList = ParseSqlParams(paramString);

                    using (OleDbCommand cmd = new OleDbCommand(procName, OleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var param in paramList)
                        {
                            cmd.Parameters.AddWithValue("?", param ?? DBNull.Value);
                        }

                        OleAdap = new OleDbDataAdapter(cmd);
                        RecData = new DataSet();
                        OleAdap.Fill(RecData, "Table1");

                        EOF = RecordCount() == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
                EOF = true;
            }
        }
        public List<object> ParseSqlParams(string paramSection)
        {
            List<object> result = new List<object>();

            if (string.IsNullOrWhiteSpace(paramSection))
                return result;

            int i = 0;
            while (i < paramSection.Length)
            {
                while (i < paramSection.Length && (paramSection[i] == ' ' || paramSection[i] == ','))
                    i++;

                if (i >= paramSection.Length)
                    break;

                string val = "";

                if (paramSection[i] == '\'')
                {
                    i++;
                    int start = i;

                    while (i < paramSection.Length && paramSection[i] != '\'')
                        i++;

                    val = paramSection.Substring(start, i - start);
                    i++;
                }
                else
                {
                    int start = i;
                    while (i < paramSection.Length && paramSection[i] != ',')
                        i++;

                    val = paramSection.Substring(start, i - start).Trim();
                }

                if (ContainsSqlInjectionPattern(val))
                    throw new Exception($"Possible SQL injection attempt in parameter: '{val}'");

                result.Add(val);

                while (i < paramSection.Length && (paramSection[i] == ',' || paramSection[i] == ' '))
                    i++;
            }

            return result;
        }
        private bool ContainsSqlInjectionPattern(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string lowered = input.ToLowerInvariant();

            string[] dangerousTokens = new[] {
                ";", "--", "/*", "*/", "xp_", "drop ", "alter ", "exec ", "insert ", "update ", "delete ", "shutdown"
            };

            foreach (string token in dangerousTokens)
            {
                if (lowered.Contains(token))
                    return true;
            }

            return false;
        }
        /*public List<object> ParseSqlParams(string paramSection)
        {
            List<object> result = new List<object>();
            if (string.IsNullOrWhiteSpace(paramSection)) return result;

            int i = 0;
            while (i < paramSection.Length)
            {
                while (i < paramSection.Length && (paramSection[i] == ' ' || paramSection[i] == ','))
                    i++;

                if (i >= paramSection.Length) break;

                if (paramSection[i] == '\'')
                {
                    int start = ++i;
                    while (i < paramSection.Length && paramSection[i] != '\'') i++;
                    string val = paramSection.Substring(start, i - start);
                    result.Add(val);
                    i++; 
                } else
                {
                    int start = i;
                    while (i < paramSection.Length && paramSection[i] != ',') i++;
                    string val = paramSection.Substring(start, i - start).Trim();
                    result.Add(val);
                }

                // Move past comma if any
                while (i < paramSection.Length && (paramSection[i] == ',' || paramSection[i] == ' '))
                    i++;
            }

            return result;
        }*/
        /*public void Open(string strSQL, string ConnString, ref string stErr, Dictionary<string, object> parameters, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            stErr = "";
            try
            {
                OleDbConnection OleConn = new OleDbConnection(ConnString);
                if (OpenConnection(OleConn, ConnString))
                {
                    string procName = strSQL.Split(' ')[0];
                    string paramString = strSQL.Substring(procName.Length).Trim();

                    List<object> paramList = ParseSqlParams(paramString);

                    OleComm = new OleDbCommand(procName, OleConn);
                    OleComm.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        OleComm.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    OleAdap = new OleDbDataAdapter();
                    OleAdap.SelectCommand = OleComm;

                    RecData = new DataSet();
                    OleAdap.Fill(RecData, "Table1");

                    EOF = RecordCount() == 0;
                }

            } catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }*/
        public string Fields(string FieldString)
        {
            string strField;
            try
            {
                if (FieldString.Trim() != "")
                {
                    strField = RecData.Tables["Table1"].Rows[Convert.ToInt32(AbsolutePosition)][FieldString.Trim()].ToString();
                }
                else
                {
                    strField = null;
                }
            }
            catch
            {
                strField = null;
            }

            return strField;
        }

        public string Fields(string FieldString, ref string stErr)
        {
            string strField;
            try
            {
                if (FieldString.Trim() != "")
                {
                    strField = RecData.Tables["Table1"].Rows[Convert.ToInt32(AbsolutePosition)][FieldString.Trim()].ToString();
                }
                else
                {
                    strField = null;
                }
                stErr = "test";
            }
            catch (Exception ex)
            {
                strField = null;
                stErr = ex.Message;
            }

            return strField;
        }

        public string Fields(int FieldIndex)
        {
            string strField;
            try
            {
                strField = RecData.Tables["Table1"].Rows[Convert.ToInt32(AbsolutePosition)][FieldIndex].ToString();
            }
            catch
            {
                strField = null;
            }

            return strField;
        }

        public string Fields(int FieldIndex, ref string stErr)
        {
            string strField;
            try
            {
                strField = RecData.Tables["Table1"].Rows[Convert.ToInt32(AbsolutePosition)][FieldIndex].ToString();
            }
            catch (Exception ex)
            {
                strField = null;
                stErr = ex.Message;
            }

            return strField;
        }

        public string FieldName(int FieldIndex)
        {
            string strField;
            try
            {
                strField = RecData.Tables["Table1"].Columns[FieldIndex].ColumnName.ToString();
            }
            catch
            {
                strField = null;
            }

            return strField;
        }

        public string FieldName(int FieldIndex, ref string stErr)
        {
            string strField;
            try
            {
                strField = RecData.Tables["Table1"].Columns[FieldIndex].ColumnName.ToString();
            }
            catch (Exception ex)
            {
                strField = null;
                stErr = ex.Message;
            }

            return strField;
        }

        public string FieldName(string FieldString)
        {
            string strField;
            try
            {
                if (FieldString.Trim() != "")
                {
                    strField = RecData.Tables["Table1"].Columns[FieldString.Trim()].ColumnName.ToString();
                }
                else
                    strField = null;
            }
            catch
            {
                strField = null;
            }

            return strField;
        }

        public string FieldName(string FieldString, ref string stErr)
        {
            string strField;
            try
            {
                if (FieldString.Trim() != "")
                {
                    strField = RecData.Tables["Table1"].Columns[FieldString.Trim()].ColumnName.ToString();
                }
                else
                    strField = null;
            }
            catch (Exception ex)
            {
                strField = null;
                stErr = ex.Message;
            }

            return strField;
        }

        public void MoveLast()
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = RecordCount() - 1;
                    if (RecordCount() - 1 < AbsolutePosition)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch
            {
            }
        }

        public void MoveLast(ref string stErr)
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = RecordCount() - 1;
                    if (RecordCount() - 1 < AbsolutePosition)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }

        public void MoveFirst()
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = 0;
                    if (RecordCount() - 1 < AbsolutePosition)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch
            {
            }
        }

        public void MoveFirst(ref string stErr)
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = 0;
                    if (RecordCount() - 1 < AbsolutePosition)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }

        public void MoveNext()
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = AbsolutePosition + 1;
                    if (AbsolutePosition > RecordCount() - 1)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch
            {
            }
        }

        public void MoveNext(ref string stErr)
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    AbsolutePosition = AbsolutePosition + 1;
                    if (AbsolutePosition > RecordCount() - 1)
                        EOF = true;
                    else
                        EOF = false;
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }

        public DataTable DataRecord()
        {
            var dtRes = new DataTable();
            try
            {
                dtRes = RecData.Tables["Table1"];
            }
            catch
            {
            }

            return dtRes;
        }

        public DataTable DataRecord(ref string stErr)
        {
            var dtRes = new DataTable();
            try
            {
                dtRes = RecData.Tables["Table1"];
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }

            return dtRes;
        }

        public void Close()
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    if (CloseConnection(OleConn))
                    {
                        RecData.Clear();
                    }
                }
            }
            catch
            {
            }
        }

        public void Close(ref string stErr)
        {
            try
            {
                if (RecData.Tables["Table1"].Rows.Count > 0)
                {
                    if (CloseConnection(OleConn))
                    {
                        RecData.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                stErr = ex.Message;
            }
        }

        private bool OpenConnection(OleDbConnection OleConn, string ConnString)
        {
            bool BoolOK = true;
            try
            {
                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close();
                    OleConn.ConnectionString = ConnString;
                    OleConn.Open();
                }
            }
            catch
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        private bool OpenConnection(OleDbConnection OleConn, string ConnString, ref string stErr)
        {
            bool BoolOK = true;
            try
            {
                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close();
                    OleConn.ConnectionString = ConnString;
                    OleConn.Open();
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                stErr = ex.Message;
            }

            return BoolOK;
        }

        private bool CloseConnection(OleDbConnection OleConn)
        {
            bool BoolOK = true;
            try
            {
                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close();
                    OleConn = null;
                }
            }
            catch
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        private bool CloseConnection(OleDbConnection OleConn, ref string stErr)
        {
            bool BoolOK = true;
            try
            {
                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close();
                    OleConn = null;
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                stErr = ex.Message;
            }

            return BoolOK;
        }
    }
}