using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace vtsadm.App_Code
{
    public class ExecCommand
    {
        public enum DatabaseType
        {
            SQL_Client = 0,
            ORA_Client = 1
        }

        private bool OpenConnection(OleDbConnection OleConn, string DBConnStr)
        {
            bool BoolOK = true;
            try
            {
                string strConnString;
                strConnString = DBConnStr.Trim();

                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close(); OleConn.Dispose();
                }
                OleConn.ConnectionString = strConnString;
                OleConn.Open();
            }
            catch
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        private bool OpenConnection(OleDbConnection OleConn, string DBConnStr, ref string stErr)
        {
            bool BoolOK = true;
            try
            {
                string strConnString;
                strConnString = DBConnStr.Trim();

                if (OleConn.State == System.Data.ConnectionState.Open)
                {
                    OleConn.Close(); OleConn.Dispose();
                }
                OleConn.ConnectionString = strConnString;
                OleConn.Open();
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
                    OleConn.Close();
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
                    OleConn.Close();
            }
            catch (Exception ex)
            {
                BoolOK = false;
                stErr = ex.Message;
            }

            return BoolOK;
        }

        /*public bool Execute(string strSQL, string DBConnStr, ref Int32 AffectRows, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            try
            {
                OleDbConnection SQLConn = new OleDbConnection();
                if (OpenConnection(SQLConn, DBConnStr))
                {
                    OleDbCommand SQLComm = new OleDbCommand(strSQL, SQLConn);
                    SQLComm.CommandTimeout = 60;
                    AffectRows = SQLComm.ExecuteNonQuery();
                    CloseConnection(SQLConn);
                    BoolOK = true;
                }
            } catch (Exception ex)
            {
                BoolOK = false;
            }

            return BoolOK;
        }*/
        /*public bool Execute(string strSQL, string DBConnStr, ref Int32 AffectRows, ref string strErr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            try
            {
                OleDbConnection SQLConn = new OleDbConnection();
                if (OpenConnection(SQLConn, DBConnStr, ref strErr))
                {
                    OleDbCommand SQLComm = new OleDbCommand(strSQL, SQLConn);
                    SQLComm.CommandTimeout = 60;
                    AffectRows = SQLComm.ExecuteNonQuery();
                    CloseConnection(SQLConn);
                    BoolOK = true;
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
                strErr = ex.Message;
            }

            return BoolOK;
        }*/
        public bool Execute(string strSQL, string DBConnStr, ref Int32 AffectRows, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            AffectRows = 0;

            try
            {
                
                string procName = "";
                List<object> paramList = new List<object>();


                int idxSpace = strSQL.IndexOf(' ');
                if (idxSpace > 0)
                {
                    procName = strSQL.Substring(0, idxSpace).Trim();
                    string paramSection = strSQL.Substring(idxSpace + 1).Trim();

                    paramList = ParseSqlParams(paramSection);
                }
                else
                {
                    procName = strSQL.Trim();
                }

                using (OleDbConnection OleConn = new OleDbConnection(DBConnStr))
                {
                    OleConn.Open();

                    using (OleDbCommand OleComm = new OleDbCommand(procName, OleConn))
                    {
                        OleComm.CommandType = CommandType.StoredProcedure;
                        OleComm.CommandTimeout = 60;

                        foreach (var param in paramList)
                        {
                            OleComm.Parameters.AddWithValue("?", param ?? DBNull.Value);
                        }

                        AffectRows = OleComm.ExecuteNonQuery();
                        BoolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        public bool Execute(string strSQL, string DBConnStr, ref int AffectRows, ref string strErr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            strErr = "";
            AffectRows = 0;

            try
            {
                string procName = "";
                List<object> paramList = new List<object>();

                int idxSpace = strSQL.IndexOf(' ');
                if (idxSpace > 0)
                {
                    procName = strSQL.Substring(0, idxSpace).Trim();
                    string paramSection = strSQL.Substring(idxSpace + 1).Trim();

                    paramList = ParseSqlParams(paramSection);
                }
                else
                {
                    procName = strSQL.Trim();
                }

                using (OleDbConnection OleConn = new OleDbConnection(DBConnStr))
                {
                    OleConn.Open();

                    using (OleDbCommand OleComm = new OleDbCommand(procName, OleConn))
                    {
                        OleComm.CommandType = CommandType.StoredProcedure;
                        OleComm.CommandTimeout = 60;

                        foreach (var param in paramList)
                        {
                            OleComm.Parameters.AddWithValue("?", param ?? DBNull.Value);
                        }

                        AffectRows = OleComm.ExecuteNonQuery();
                        BoolOK = true;
                    }
                }
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                BoolOK = false;
            }

            return BoolOK;
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
                ";", "/*", "*/", "xp_", "drop ", "alter ", "exec ", "insert ", "update ", "delete ", "shutdown"
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
                while (i < paramSection.Length && (paramSection[i] == ',' || paramSection[i] == ' '))
                    i++;
            }

            return result;
        }*/
        /*public bool Execute(string storedProcName, string DBConnStr, ref int AffectRows, ref string strErr, Dictionary<string, object> parameters, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            strErr = "";
            AffectRows = 0;

            try
            {
                using (OleDbConnection OleConn = new OleDbConnection(DBConnStr))
                {
                    OleConn.Open();
                    using (OleDbCommand OleComm = new OleDbCommand(storedProcName, OleConn))
                    {
                        OleComm.CommandType = CommandType.StoredProcedure;
                        OleComm.CommandTimeout = 60;
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                OleComm.Parameters.AddWithValue("?", param.Value ?? DBNull.Value);
                            }
                        }
                        AffectRows = OleComm.ExecuteNonQuery();
                        BoolOK = true;
                    }
                }
            } catch (Exception ex)
            {
                BoolOK = false;
                strErr = ex.Message;
            }

            return BoolOK;
        }*/
        public bool CheckConnection(string stDBConnStr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            try
            {
                OleDbConnection SQLConn = new OleDbConnection();
                if (OpenConnection(SQLConn, stDBConnStr))
                {
                    CloseConnection(SQLConn);
                    BoolOK = true;
                }
            }
            catch
            {
                BoolOK = false;
            }

            return BoolOK;
        }

        public bool CheckConnection(string stDBConnStr, ref string stErr, DatabaseType DB_Type = DatabaseType.SQL_Client)
        {
            bool BoolOK = false;
            try
            {
                OleDbConnection SQLConn = new OleDbConnection();
                if (OpenConnection(SQLConn, stDBConnStr, ref stErr))
                {
                    CloseConnection(SQLConn);
                    BoolOK = true;
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