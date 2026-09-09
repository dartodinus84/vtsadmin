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
    public partial class deal_picture_upload : System.Web.UI.Page
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
                            InitializeImagesList();
                            DisplayImages();
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
                lblMsg.InnerHtml = "<strong>Error:</strong> " + ex.Message;
            }
        }

        protected void Clear()
        {
            try
            {
                if (Session["ClsTypeDealPicture"].ToString() != "")
                {
                    ImgInstall.ImageUrl = "~/Picture/" + Session["ClsTypeDealPicture"].ToString();
                }
                else
                {
                    ImgInstall.ImageUrl = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InitializeImagesList()
        {
            // Initialize image list if it doesn't exist
            if (Session["ClsTypeDealPictures"] == null)
            {
                Session["ClsTypeDealPictures"] = new List<string>();
            }

            // For backward compatibility
            if (Session["ClsTypeDealPicture"] != null && !string.IsNullOrEmpty(Session["ClsTypeDealPicture"].ToString()))
            {
                // Add the old single image to the list if it's not already there
                string oldImage = Session["ClsTypeDealPicture"].ToString();
                var imagesList = (List<string>)Session["ClsTypeDealPictures"];
                if (!imagesList.Contains(oldImage))
                {
                    imagesList.Add(oldImage);
                }
                // Clear the old session variable
                Session["ClsTypeDealPicture"] = "";
            }
        }

        private void DisplayImages()
        {
            try
            {
                var imagesList = (List<string>)Session["ClsTypeDealPictures"];

                // Clear existing controls
                pnlImages.Controls.Clear();

                if (imagesList.Count > 0)
                {
                    // Create a literal to display the images in a grid
                    Literal imagesLiteral = new Literal();
                    string imagesHtml = "<div class='row'>";

                    int index = 0;
                    foreach (string imageName in imagesList)
                    {
                        imagesHtml += "<div class='col-md-4 mb-3'>";
                        imagesHtml += "<div class='card'>";
                        imagesHtml += $"<img src='~/Picture/{imageName}' class='card-img-top' alt='Uploaded image' />";
                        imagesHtml += "<div class='card-body'>";
                        imagesHtml += $"<button type='button' class='btn btn-danger btn-sm' onclick=\"removeImage({index}); return false;\">Remove</button>";
                        imagesHtml += "</div></div></div>";
                        index++;
                    }

                    imagesHtml += "</div>";
                    imagesLiteral.Text = imagesHtml;
                    pnlImages.Controls.Add(imagesLiteral);

                    // Also update the legacy control for backward compatibility
                    if (imagesList.Count > 0)
                    {
                        ImgInstall.ImageUrl = "~/Picture/" + imagesList[0];
                    }
                }
                else
                {
                    ImgInstall.ImageUrl = "";
                }

                // Update the count label
                lblImageCount.Text = $"Total Images: {imagesList.Count}";
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Error displaying images:</strong> " + ex.Message;
            }
        }

        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";

                if (FileUpload1.HasFile)
                {
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToUpper();

                    if (fileExtension == ".JPG" || fileExtension == ".JPEG" || fileExtension == ".PNG")
                    {
                        long unixTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                        // Create filename with Deal_ prefix, date format, and unix timestamp
                        string strFileName = "Deal_" + DateTime.Now.ToString("yyMMddhhmmss") + unixTimestamp.ToString() + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;

                        FileUpload1.SaveAs(strFullPath);

                        // Add to the list
                        var imagesList = (List<string>)Session["ClsTypeDealPictures"];
                        imagesList.Add(strFileName);

                        // For backward compatibility, also update the single image session variable
                        Session["ClsTypeDealPicture"] = string.Join(",", imagesList);

                        lblMsg.InnerHtml = "<div class='alert alert-success alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Success!</strong> Image uploaded successfully.</div>";

                        DisplayImages();
                    }
                    else
                    {
                        lblMsg.InnerHtml = "<div class='alert alert-danger alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Failed!</strong> Only JPG, JPEG, or PNG files are allowed.</div>";
                    }
                }
                else
                {
                    lblMsg.InnerHtml = "<div class='alert alert-warning alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Warning!</strong> Please select a file to upload.</div>";
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void RemoveImage(int index)
        {
            try
            {
                var imagesList = (List<string>)Session["ClsTypeDealPictures"];

                if (index >= 0 && index < imagesList.Count)
                {
                    imagesList.RemoveAt(index);
                    Session["ClsTypeDealPicture"] = string.Join(",", imagesList);

                    lblMsg.InnerHtml = "<div class='alert alert-success alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Success!</strong> Image removed.</div>";
                }

                DisplayImages();
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        protected void CmdRemove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";

                // Clear the image list
                Session["ClsTypeDealPictures"] = new List<string>();
                Session["ClsTypeDealPicture"] = "";

                ImgInstall.ImageUrl = "";
                DisplayImages();

                lblMsg.InnerHtml = "<div class='alert alert-success alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Success!</strong> All images removed.</div>";
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<div class='alert alert-danger alert-dismissible'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong>Failed!</strong> " + ex.Message + "</div>";
            }
        }

        [System.Web.Services.WebMethod]
        public static void RemoveImageByIndex(int index)
        {
            try
            {
                var imagesList = (List<string>)HttpContext.Current.Session["ClsTypeDealPictures"];

                if (index >= 0 && index < imagesList.Count)
                {
                    imagesList.RemoveAt(index);
                    HttpContext.Current.Session["ClsTypeDealPicture"] = string.Join(",", imagesList);
                }
            }
            catch (Exception)
            {
                // Handle exception
            }
        }
    }
}