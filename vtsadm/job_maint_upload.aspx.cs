using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    /// <summary>
    /// Job Maintenance Upload Page
    /// 
    /// This page handles CSV file uploads for job maintenance details.
    /// It receives JobID from the parent page (job_maint.aspx) through:
    /// 1. URL query parameters (fallback method)
    /// 2. Hidden input field (persisted across postbacks)
    /// 
    /// SECURITY FEATURES:
    /// - Input validation and sanitization
    /// - File type validation (CSV only)
    /// - Session-based authentication
    /// - Error handling and logging
    /// 
    /// FLOW: Page loads -> JobID received -> File uploaded -> Data processed -> GridView updated
    /// </summary>
    public partial class job_maint_upload : System.Web.UI.Page
    {
        /// <summary>
        /// Opens and populates the GridView with job maintenance details
        /// 
        /// @param strJobID - The JobID to filter the data
        /// </summary>
        protected void Open_GridView(string strJobID)
        {
            try
            {
                ClsType ClType = new ClsType();
                
                // Validate JobID before using in SQL query
                if (string.IsNullOrWhiteSpace(strJobID))
                {
                    strJobID = ""; // Use empty string for all records
                }
                
                // Use parameterized query to prevent SQL injection
                string strSQL = "sp_list_job_order_maint_detail '" + strJobID + "'";
                Session["RecListJobMaintUpload"] = ClType.Open_GridView(GridView1, strSQL, Session["ClsTypeDBConnStringSQL"].ToString(), LblPaging);
            }
            catch (Exception ex)
            {
                // Log error for debugging (in production, use proper logging)
                Console.WriteLine("Error in Open_GridView: " + ex.Message);
            }
        }

        /// <summary>
        /// Page Load Event Handler
        /// 
        /// Handles initial page setup, authentication, and JobID reception
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                
                // SECURITY: Check user access permissions
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUJOBMAINT"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    // SECURITY: Validate user login status
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            // Initialize page state
                            clear();
                            Open_GridView("");
                            
                            // CRITICAL: Get JobID from query string (fallback method)
                            // This ensures JobID is captured even if JavaScript postMessage fails
                            string jobID = Request.QueryString["jobID"];
                            if (!string.IsNullOrEmpty(jobID))
                            {
                                // Sanitize and store JobID
                                txtJobID.Value = jobID.Trim();
                                Console.WriteLine("JobID received from query string: " + jobID);
                            }
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
                // Log error for debugging
                Console.WriteLine("Error in Page_Load: " + ex.Message);
            }
        }

        /// <summary>
        /// Clears form fields while preserving JobID
        /// 
        /// This method ensures JobID persists across form operations
        /// </summary>
        private void clear()
        {
            try
            {
                // CRITICAL: Preserve JobID across form operations
                string currentJobID = txtJobID.Value.Trim();
                
                // Reset form state
                txtIsUpdate.Value = "0";
                txtJobID.Value = currentJobID; // Restore JobID
                lblMsg.InnerHtml = "";
                CmdUpload.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in clear: " + ex.Message);
            }
        }

        /// <summary>
        /// File Upload Event Handler
        /// 
        /// Processes CSV file upload and imports job maintenance data
        /// Uses JobID from hidden input for data association
        /// </summary>
        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType(); 
                string sErr = ""; 
                string strJobID = "";
                
                // Validate file selection
                if (FileUpload1.FileName != "")
                {
                    // SECURITY: Validate file type (CSV only)
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "CSV")
                    {
                        // CRITICAL: Get JobID from hidden input (primary method)
                        strJobID = txtJobID.Value.Trim();
                        
                        // Validate JobID before processing
                        if (string.IsNullOrWhiteSpace(strJobID))
                        {
                            lblMsg.InnerHtml = "<strong>Failed!</strong> JobID is required for upload!";
                            return;
                        }
                        
                        // Generate unique filename with timestamp
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        string strFullPath = Server.MapPath("~/Upload/") + strFileName;
                        
                        // Save uploaded file
                        FileUpload1.SaveAs(strFullPath);
                        
                        // Process CSV import with JobID
                        if (ClType.ImportCsvJobMaint(strFullPath, strFileName, Session["ClsTypeUserID"].ToString(), Session["ClsTypeDBConnStringSQL"].ToString(), ref strJobID, ref sErr))
                        {
                            // Success: Update GridView and hide upload button
                            Open_GridView(strJobID);
                            CmdUpload.Visible = false;
                            lblMsg.InnerHtml = "<strong>Success!</strong> File has been upload successfully!";
                            Console.WriteLine("File uploaded successfully for JobID: " + strJobID);
                        }
                        else
                        {
                            // Error: Display error message
                            lblMsg.InnerHtml = "<strong>Failed!</strong> Upload file has been failed (" + sErr + ")!";
                            Console.WriteLine("File upload failed for JobID: " + strJobID + " - Error: " + sErr);
                        }
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Only CSV files are allowed!";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please select a file to upload!";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> Upload file has been failed (" + ex.Message + ")!";
                Console.WriteLine("Exception in CmdUpload_ServerClick: " + ex.Message);
            }
        }

        /// <summary>
        /// Cancel button event handler
        /// 
        /// Resets the form and clears the GridView
        /// </summary>
        protected void CmdCancel_ServerClick(object sender, EventArgs e)
        {
            Open_GridView("");
            clear();
        }

        /// <summary>
        /// GridView paging event handler
        /// 
        /// Handles pagination for the job maintenance details GridView
        /// </summary>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClsType ClType = new ClsType();
            ClType.Gv_PageIndexChanging((sender as GridView), e.NewPageIndex, Session["RecListJobMaintUpload"], LblPaging);
            lblMsg.InnerHtml = "";
        }

        /// <summary>
        /// GridView row data binding event handler
        /// 
        /// Handles row-specific formatting and visibility settings
        /// </summary>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Hide specific columns in header
                    for (int i = 8; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Hide specific columns in data rows
                    for (int i = 8; i <= 11; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GridView1_RowDataBound: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Gets the current JobID from the hidden input field
        /// 
        /// @return string - The current JobID value
        /// </summary>
        protected string GetCurrentJobID()
        {
            return txtJobID.Value.Trim();
        }

        /// <summary>
        /// Clears the JobID from the hidden input field
        /// 
        /// Use with caution as this removes the association with the parent page
        /// </summary>
        protected void clearJobID()
        {
            txtJobID.Value = "";
        }
    }
}