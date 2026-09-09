using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace vtsadm.App_Code
{
    public class ClsStockOpname
    {
        // Status perangkat
        public enum DeviceStatus
        {
            RG, // Register
            MW, // Mutation to Warehouse
            MT, // Mutation to Technician
            IS, // Install
            DE  // Delete
        }

        public enum LocationType
        {
            WAREHOUSE,
            TECHNICIAN
        }

        /// <summary>
        /// Membuat ID opname baru berdasarkan tanggal
        /// </summary>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>ID opname baru</returns>
        public int CreateNewOpnameId(string sDBConn)
        {
            int newOpnameId = 0;
            string strSQL = "";
            string sErr = "";

            try
            {
                Recordset Rec = new Recordset();
                // Menambahkan filter is_deleted = 0
                strSQL = "SELECT ISNULL(MAX(opname_id), 0) + 1 AS new_id FROM trx_stock_opname_device WHERE is_deleted = 0";
                Rec.Open(strSQL, sDBConn, ref sErr);

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    newOpnameId = Convert.ToInt32(Rec.Fields(0));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating new opname ID: " + ex.Message);
            }

            return newOpnameId;
        }

        /// <summary>
        /// Menyimpan header stok opname
        /// </summary>
        /// <param name="opnameDate">Tanggal opname</param>
        /// <param name="locationType">Tipe lokasi (WAREHOUSE/TECHNICIAN)</param>
        /// <param name="locationId">ID lokasi</param>
        /// <param name="remark">Keterangan</param>
        /// <param name="userId">ID user</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>ID opname yang dibuat</returns>
        public int SaveOpnameHeader(DateTime opnameDate, string locationType, string locationId,
                                  string remark, string userId, string sDBConn)
        {
            int opnameId = 0;
            string strSQL = "";
            int intAff = 0;
            string sErr = "";
            ExecCommand Ec = new ExecCommand();

            try
            {
                // Begin Transaction using stored procedure
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Execute the stored procedure
                Recordset Rec = new Recordset();
                strSQL = "sp_insert_stock_opname_device_header '" +
                         opnameDate.ToString("yyyy-MM-dd") + "','" +
                         locationType + "','" +
                         locationId + "','" +
                         remark + "','" +
                         userId + "'";

                Rec.Open(strSQL, sDBConn, ref sErr);

                if (Rec.RecordCount() > 0)
                {
                    Rec.MoveFirst();
                    opnameId = Convert.ToInt32(Rec.Fields("opname_id"));

                    // Commit Transaction
                    strSQL = "COMMIT TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                else
                {
                    // Rollback Transaction
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                    throw new Exception("Failed to create opname header: " + sErr);
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on error
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                catch { /* Ignore errors in rollback */ }

                throw new Exception("Error saving opname header: " + ex.Message);
            }

            return opnameId;
        }

        /// <summary>
        /// Menambahkan detail stok opname
        /// </summary>
        /// <param name="opnameId">ID opname</param>
        /// <param name="deviceId">ID perangkat</param>
        /// <param name="nosn">Nomor seri perangkat</param>
        /// <param name="currentStatus">Status di sistem</param>
        /// <param name="physicalFound">Ditemukan fisik (true/false)</param>
        /// <param name="physicalStatus">Status fisik</param>
        /// <param name="remarks">Keterangan</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <param name="sErr">Output error message</param>
        /// <returns>True jika berhasil</returns>
        public bool SaveOpnameDetail(int opnameId, string deviceId, string nosn, string currentStatus,
                                   bool physicalFound, string physicalStatus, string remarks,
                                   string sDBConn, out string sErr)
        {
            bool result = false;
            string strSQL = "";
            int intAff = 0;
            sErr = "";
            ExecCommand Ec = new ExecCommand();

            try
            {
                // Begin Transaction
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                if (!string.IsNullOrEmpty(sErr))
                {
                    throw new Exception("Error beginning transaction: " + sErr);
                }

                // Dapatkan user ID dari session
                string userId = HttpContext.Current.Session["ClsTypeUserID"]?.ToString() ?? "SYSTEM";

                // Append parameters for the stored procedure
                strSQL = "DECLARE @ErrorMsg NVARCHAR(4000) " +
                         "BEGIN TRY " +
                         "   EXEC sp_insert_stock_opname_device_detail " +
                         opnameId + ",'" +
                         deviceId + "','" +
                         nosn + "','" +
                         currentStatus + "'," +
                         (physicalFound ? "1" : "0") + "," +
                         (physicalFound ? "'" + physicalStatus + "'" : "NULL") + "," +
                         (string.IsNullOrEmpty(remarks) ? "NULL" : "'" + remarks + "'") + "," +
                         "'" + userId + "'" + // Menambahkan parameter user_id
                         " END TRY " +
                         "BEGIN CATCH " +
                         "   SELECT @ErrorMsg = ERROR_MESSAGE() " +
                         "   RAISERROR(@ErrorMsg, 16, 1) " +
                         "END CATCH";

                // Execute the command with error handling
                bool execResult = Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Check for success or error
                if (execResult && string.IsNullOrEmpty(sErr))
                {
                    // Commit Transaction if no errors
                    strSQL = "COMMIT TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    result = true;
                }
                else
                {
                    // Rollback Transaction on error
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                    // If no specific error message was captured, provide a default
                    if (string.IsNullOrEmpty(sErr))
                    {
                        sErr = "Failed to add opname detail";
                    }

                    result = false;
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on exception
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff);
                }
                catch { /* Ignore errors in rollback */ }

                sErr = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Overload for compatibility with existing code
        /// </summary>
        public bool SaveOpnameDetail(int opnameId, string deviceId, string nosn, string currentStatus,
                                   bool physicalFound, string physicalStatus, string remarks,
                                   string sDBConn)
        {
            string sErr;
            return SaveOpnameDetail(opnameId, deviceId, nosn, currentStatus, physicalFound, physicalStatus, remarks, sDBConn, out sErr);
        }

        /// <summary>
        /// Menghapus header stok opname
        /// </summary>
        /// <param name="opnameId">ID opname</param>
        /// <param name="userId">ID user</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>True jika berhasil</returns>
        public bool DeleteOpnameHeader(int opnameId, string userId, string sDBConn)
        {
            bool result = false;
            string strSQL = "";
            int intAff = 0;
            string sErr = "";
            ExecCommand Ec = new ExecCommand();

            try
            {
                // Begin Transaction
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Delete header and its details
                strSQL = "sp_delete_stock_opname_device_header " + opnameId + ",'" + userId + "'";

                if (Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        // Commit Transaction
                        strSQL = "COMMIT TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        result = true;
                    }
                    else
                    {
                        // Rollback Transaction
                        strSQL = "ROLLBACK TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        throw new Exception("Failed to delete opname header: " + sErr);
                    }
                }
                else
                {
                    // Rollback Transaction
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    throw new Exception("Error executing SP: " + sErr);
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on error
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                catch { /* Ignore errors in rollback */ }

                throw new Exception("Error deleting opname header: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Menghapus detail stok opname
        /// </summary>
        /// <param name="opnameId">ID opname</param>
        /// <param name="detailId">ID detail</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>True jika berhasil</returns>
        public bool DeleteOpnameDetail(int opnameId, int detailId, string sDBConn)
        {
            bool result = false;
            string strSQL = "";
            int intAff = 0;
            string sErr = "";
            ExecCommand Ec = new ExecCommand();
            string userId = HttpContext.Current.Session["ClsTypeUserID"]?.ToString() ?? "SYSTEM";

            try
            {
                // Begin Transaction
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Delete detail record with user_id parameter
                strSQL = "sp_delete_stock_opname_device_detail " + opnameId + "," + detailId + ",'" + userId + "'";

                if (Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        // Commit Transaction
                        strSQL = "COMMIT TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        result = true;
                    }
                    else
                    {
                        // Rollback Transaction
                        strSQL = "ROLLBACK TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        throw new Exception("Failed to delete opname detail: " + sErr);
                    }
                }
                else
                {
                    // Rollback Transaction
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    throw new Exception("Error executing SP: " + sErr);
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on error
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                catch { /* Ignore errors in rollback */ }

                throw new Exception("Error deleting opname detail: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Mengupdate header stok opname
        /// </summary>
        /// <param name="opnameId">ID opname</param>
        /// <param name="opnameDate">Tanggal opname</param>
        /// <param name="locationType">Tipe lokasi (WAREHOUSE/TECHNICIAN)</param>
        /// <param name="locationId">ID lokasi</param>
        /// <param name="remark">Keterangan</param>
        /// <param name="userId">ID user</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>True jika berhasil</returns>
        public bool UpdateOpnameHeader(int opnameId, DateTime opnameDate, string locationType, string locationId,
                                    string remark, string userId, string sDBConn)
        {
            bool result = false;
            string strSQL = "";
            int intAff = 0;
            string sErr = "";
            ExecCommand Ec = new ExecCommand();

            try
            {
                // Begin Transaction
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Update header data
                strSQL = "sp_update_stock_opname_device_header " +
                         opnameId + ",'" +
                         opnameDate.ToString("yyyy-MM-dd") + "','" +
                         locationType + "','" +
                         locationId + "','" +
                         remark + "','" +
                         userId + "'";

                if (Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr))
                {
                    if (intAff > 0)
                    {
                        // Commit Transaction
                        strSQL = "COMMIT TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        result = true;
                    }
                    else
                    {
                        // Rollback Transaction
                        strSQL = "ROLLBACK TRANSACTION";
                        Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                        throw new Exception("Failed to update opname header: " + sErr);
                    }
                }
                else
                {
                    // Rollback Transaction
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    throw new Exception("Error executing SP: " + sErr);
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on error
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                catch { /* Ignore errors in rollback */ }

                throw new Exception("Error updating opname header: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Submit stok opname untuk memperbarui status perangkat
        /// </summary>
        /// <param name="opnameId">ID opname</param>
        /// <param name="userId">ID user</param>
        /// <param name="sDBConn">Koneksi database</param>
        /// <returns>True jika berhasil</returns>
        public bool SubmitOpname(int opnameId, string userId, string sDBConn)
        {
            bool result = false;
            string strSQL = "";
            int intAff = 0;
            string sErr = "";
            ExecCommand Ec = new ExecCommand();

            try
            {
                // Begin Transaction
                strSQL = "BEGIN TRANSACTION";
                Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);

                // Submit opname and update device statuses
                strSQL = "sp_submit_stock_opname_device " + opnameId + ",'" + userId + "'";

                if (Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr))
                {
                    // Commit Transaction regardless of rows affected
                    // as the SP might not update any rows if there are no changes to be made
                    strSQL = "COMMIT TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    result = true;
                }
                else
                {
                    // Rollback Transaction
                    strSQL = "ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                    throw new Exception("Error executing SP: " + sErr);
                }
            }
            catch (Exception ex)
            {
                // Ensure rollback on error
                try
                {
                    strSQL = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";
                    Ec.Execute(strSQL, sDBConn, ref intAff, ref sErr);
                }
                catch { /* Ignore errors in rollback */ }

                throw new Exception("Error submitting opname: " + ex.Message);
            }

            return result;
        }
    }
}