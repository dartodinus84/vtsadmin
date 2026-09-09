using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class training_cust_upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (Session["ClsTypeAccessMenu"] == null
                    || !Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUCUSTJOBTRAINING"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["ClsTypeIsLogin"] != null)
                    {
                        if (ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                        {
                            Clear();
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

        private string GetPictureSession()
        {
            return Session["ClsTypeNewPictureTraining"] == null
                ? ""
                : Session["ClsTypeNewPictureTraining"].ToString();
        }

        protected void Clear()
        {
            try
            {
                string pic = GetPictureSession();
                if (!string.IsNullOrEmpty(pic))
                {
                    ImgInstall.ImageUrl = "~/Picture/" + pic;
                    ImgInstall.Visible = true;
                }
                else
                {
                    ImgInstall.ImageUrl = "";
                    ImgInstall.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                if (FileUpload1 == null || !FileUpload1.HasFile)
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Please select an image file.";
                    return;
                }

                string fileName = FileUpload1.FileName ?? "";
                string ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblMsg.InnerHtml = "<strong>Failed!</strong> Only JPG/JPEG/PNG allowed.";
                    return;
                }

                string baseName = Path.GetFileNameWithoutExtension(fileName);
                if (string.IsNullOrWhiteSpace(baseName))
                {
                    baseName = "training";
                }
                string strFileName = baseName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                string pictureDir = Server.MapPath("~/Picture/");
                if (!Directory.Exists(pictureDir))
                {
                    Directory.CreateDirectory(pictureDir);
                }
                string strFullPath = Path.Combine(pictureDir, strFileName);
                FileUpload1.SaveAs(strFullPath);
                ImgInstall.ImageUrl = "~/Picture/" + strFileName;
                ImgInstall.Visible = true;
                Session["ClsTypeNewPictureTraining"] = strFileName;
                lblMsg.InnerHtml = "<strong>Success!</strong>";
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> " + HttpUtility.HtmlEncode(ex.Message);
            }
        }

        protected void CmdRemove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                Session["ClsTypeNewPictureTraining"] = "";
                ImgInstall.ImageUrl = "";
                ImgInstall.Visible = false;
                lblMsg.InnerHtml = "<strong>Success!</strong> Picture removed.";
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> " + HttpUtility.HtmlEncode(ex.Message);
            }
        }
    }
}
