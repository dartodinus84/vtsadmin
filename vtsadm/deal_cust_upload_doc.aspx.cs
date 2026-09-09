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
    public partial class deal_cust_upload_doc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBDEAL"))
                {
                    Response.Redirect("dashboard.aspx");
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            // Initialize the attachments list in session if it doesn't exist
                            if (Session["ClsTypeAttachments"] == null)
                            {
                                Session["ClsTypeAttachments"] = new List<string>();
                            }

                            Clear();
                            DisplayAttachments();
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
                lblMsg.InnerHtml = "<div class='alert alert-danger'>Error initializing page: " + ex.Message + "</div>";
            }
        }

        protected void Clear()
        {
            try
            {
                // For backward compatibility
                if (Session["ClsTypeAttacment"] != null && !string.IsNullOrEmpty(Session["ClsTypeAttacment"].ToString()))
                {
                    // Add the legacy single attachment to the list if it's not already there
                    List<string> attachments = Session["ClsTypeAttachments"] as List<string> ?? new List<string>();
                    if (!attachments.Contains(Session["ClsTypeAttacment"].ToString()))
                    {
                        attachments.Add(Session["ClsTypeAttacment"].ToString());
                        Session["ClsTypeAttachments"] = attachments;
                    }

                    // Clear the old single attachment format
                    Session["ClsTypeAttacment"] = "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger'>Error clearing data: " + ex.Message + "</div>";
            }
        }

        protected void DisplayAttachments()
        {
            try
            {
                // Get the list of attachments
                List<string> attachments = Session["ClsTypeAttachments"] as List<string> ?? new List<string>();

                // Create HTML to display the list of attachments
                if (attachments.Count > 0)
                {
                    string html = "<div class='panel panel-default'><div class='panel-heading'>Uploaded Documents</div>";
                    html += "<div class='panel-body'><ul class='list-group'>";

                    foreach (string file in attachments)
                    {
                        html += "<li class='list-group-item'>";
                        html += "<i class='fa fa-file'></i> " + file;
                        html += " <button type='button' class='btn btn-xs btn-danger' onclick=\"removeFile('" + file + "');\"><i class='fa fa-trash'></i></button>";
                        html += "</li>";
                    }

                    html += "</ul></div></div>";

                    // Display the HTML in a placeholder
                    attachmentsList.InnerHtml = html;
                }
                else
                {
                    attachmentsList.InnerHtml = "<div class='alert alert-info'>No documents uploaded yet.</div>";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger'>Error displaying attachments: " + ex.Message + "</div>";
            }
        }

        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                ClsType ClType = new ClsType();
                string sErr = "";

                // Get the list of attachments from session
                List<string> attachments = Session["ClsTypeAttachments"] as List<string> ?? new List<string>();

                // Check if there are any files to upload
                if (FileUpload1.HasFiles)
                {
                    foreach (HttpPostedFile uploadedFile in FileUpload1.PostedFiles)
                    {
                        string fileExtension = Path.GetExtension(uploadedFile.FileName).ToUpper();

                        // Check for valid file extensions
                        if (fileExtension == ".PDF" || fileExtension == ".XLS" ||
                            fileExtension == ".DOC" || fileExtension == ".XLSX" ||
                            fileExtension == ".DOCX")
                        {
                            // Generate filename with the requested pattern
                            // Format: DealAtt_yymmddhhmmss{unix}
                            string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
                            long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                            string strFileName = "DealAtt_" + timestamp + unixTime + fileExtension;
                            string strFullPath = Server.MapPath("~/Attachment/") + strFileName;

                            // Save the file
                            uploadedFile.SaveAs(strFullPath);

                            // Add to the list
                            attachments.Add(strFileName);
                        }
                        else
                        {
                            sErr += "Invalid file type: " + fileExtension + ". ";
                        }
                    }

                    // Update session
                    Session["ClsTypeAttachments"] = attachments;

                    // For backward compatibility
                    if (attachments.Count > 0)
                    {
                        Session["ClsTypeAttacment"] = string.Join(",", attachments);
                    }
                    else
                    {
                        Session["ClsTypeAttacment"] = "";
                    }

                    // Display success message
                    if (string.IsNullOrEmpty(sErr))
                    {
                        lblMsg.InnerHtml = "<div class='alert alert-success'><strong>Success!</strong> Files uploaded.</div>";
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<div class='alert alert-warning'><strong>Partial Success!</strong> Some files were not uploaded: " + sErr + "</div>";
                    }

                    // Update the display
                    DisplayAttachments();
                }
                else
                {
                    lblMsg.InnerHtml = "<div class='alert alert-warning'><strong>Warning!</strong> No files selected.</div>";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger'><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdRemove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                // Get the file to remove from the hidden field
                string fileToRemove = hdnFileToRemove.Value;

                if (!string.IsNullOrEmpty(fileToRemove))
                {
                    // Get the list from session
                    List<string> attachments = Session["ClsTypeAttachments"] as List<string> ?? new List<string>();

                    // Remove the file
                    if (attachments.Contains(fileToRemove))
                    {
                        attachments.Remove(fileToRemove);

                        // Update session
                        Session["ClsTypeAttachments"] = attachments;

                        // For backward compatibility
                        if (attachments.Count > 0)
                        {
                            Session["ClsTypeAttacment"] = string.Join(",", attachments);
                        }
                        else
                        {
                            Session["ClsTypeAttacment"] = "";
                        }

                        // Success message
                        lblMsg.InnerHtml = "<div class='alert alert-success'><strong>Success!</strong> File removed.</div>";
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<div class='alert alert-warning'><strong>Warning!</strong> File not found.</div>";
                    }

                    // Clear the hidden field
                    hdnFileToRemove.Value = "";

                    // Update the display
                    DisplayAttachments();
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger'><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }
    }
}