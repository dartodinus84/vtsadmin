using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace vtsadm.App_Code
{
    public sealed class StoredProcedureTransactionExecutor
    {
        private static readonly Regex ProcNamePattern = new Regex(@"^[A-Za-z_][A-Za-z0-9_\.]*$", RegexOptions.Compiled);
        private const int DefaultCommandTimeoutSeconds = 60;

        public bool Execute(
            string connectionString,
            string procedureName,
            IEnumerable<object> parameters,
            ref int affectedRows,
            ref string errorMessage)
        {
            errorMessage = "";
            affectedRows = 0;

            try
            {
                ValidateConnectionString(connectionString);
                ValidateProcedureName(procedureName);

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    affectedRows = ExecuteInternal(connection, null, procedureName, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool ExecuteSingleInTransaction(
            string connectionString,
            string procedureName,
            IEnumerable<object> parameters,
            ref int affectedRows,
            ref string errorMessage)
        {
            errorMessage = "";
            affectedRows = 0;

            try
            {
                ValidateConnectionString(connectionString);
                ValidateProcedureName(procedureName);

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            affectedRows = ExecuteInternal(connection, transaction, procedureName, parameters);
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            try { transaction.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public int ExecuteInScope(
            OleDbConnection connection,
            OleDbTransaction transaction,
            string procedureName,
            IEnumerable<object> parameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open) throw new InvalidOperationException("Connection must be open.");
            ValidateProcedureName(procedureName);

            return ExecuteInternal(connection, transaction, procedureName, parameters);
        }

        private static int ExecuteInternal(
            OleDbConnection connection,
            OleDbTransaction transaction,
            string procedureName,
            IEnumerable<object> parameters)
        {
            using (var command = new OleDbCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = DefaultCommandTimeoutSeconds;
                if (transaction != null)
                    command.Transaction = transaction;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                        command.Parameters.AddWithValue("?", parameter ?? DBNull.Value);
                }

                return command.ExecuteNonQuery();
            }
        }

        private static void ValidateConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Database connection string is empty.");
        }

        private static void ValidateProcedureName(string procedureName)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("Stored procedure name is required.");
            if (!ProcNamePattern.IsMatch(procedureName))
                throw new ArgumentException("Invalid stored procedure name.");
        }
    }
}
