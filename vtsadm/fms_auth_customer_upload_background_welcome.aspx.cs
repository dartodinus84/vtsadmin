using System;
using System.IO;
using System.Web.UI;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_auth_customer_upload_background_welcome : System.Web.UI.Page
    {
        private const string SessionKey = "ClsTypeFMSCustomerBackgroundWelcome";
        private const string WelcomeFolder = "BackgroundWelcome";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.Form != null)
                {
                    Page.Form.Enctype = "multipart/form-data";
                }

                string sAccessMenu = Session["ClsTypeAccessMenu"] as string;
                if (string.IsNullOrEmpty(sAccessMenu) || !sAccessMenu.ToUpper().Contains("MNUFMSAUTHCUST"))
                {
                    Response.Redirect("dashboard.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    ClsType ClType = new ClsType();
                    if (Session["ClsTypeIsLogin"] != null && ClType.SudahLogon(Convert.ToBoolean(Session["ClsTypeIsLogin"])))
                    {
                        Clear();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void Clear()
        {
            SetPreviewImage(Session[SessionKey] as string);
        }

        private void SetPreviewImage(string sFileName)
        {
            if (!string.IsNullOrEmpty(sFileName))
            {
                ImgInstall.ImageUrl = ResolveUrl("~/Picture/" + WelcomeFolder + "/" + sFileName) + "?v=" + DateTime.Now.Ticks;
                ImgInstall.Visible = true;
            }
            else
            {
                ImgInstall.ImageUrl = "";
                ImgInstall.Visible = false;
            }
        }

        private string SaveAuthCustomerImage(string subFolder)
        {
            if (FileUpload1 == null || !FileUpload1.HasFile || FileUpload1.FileName == "")
            {
                throw new Exception("No file selected.");
            }

            string upper = FileUpload1.FileName.ToUpper().Trim();
            if (ClsFunc.Right(upper, 3) != "JPG" && ClsFunc.Right(upper, 4) != "JPEG"
                && ClsFunc.Right(upper, 3) != "PNG" && ClsFunc.Right(upper, 3) != "GIF"
                && ClsFunc.Right(upper, 3) != "ICO" && ClsFunc.Right(upper, 4) != "WEBP")
            {
                throw new Exception("File type not allowed. Use JPG, PNG, GIF, ICO, or WEBP.");
            }

            string ext = Path.GetExtension(FileUpload1.FileName);
            int baseLen = FileUpload1.FileName.Length - ext.Length;
            string strFileName = ClsFunc.Left(FileUpload1.FileName, baseLen) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext.ToLowerInvariant();
            string folder = string.IsNullOrEmpty(subFolder)
                ? Server.MapPath("~/Picture/")
                : Server.MapPath("~/Picture/" + subFolder.Trim().Replace('\\', '/').Trim('/') + "/");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            FileUpload1.SaveAs(Path.Combine(folder, strFileName));
            return strFileName;
        }

        protected void CmdUpload_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                lblMsg.Attributes["class"] = "";

                string strFileName = SaveAuthCustomerImage(WelcomeFolder);
                Session[SessionKey] = strFileName;
                SetPreviewImage(strFileName);

                lblMsg.InnerHtml = "<strong>Success!</strong>";
                lblMsg.Attributes["class"] = "text-success";
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong> " + ex.Message;
                lblMsg.Attributes["class"] = "text-danger";
            }
        }

        protected void CmdRemove_Click(object sender, EventArgs e)
        {
            lblMsg.InnerHtml = "";
            lblMsg.Attributes["class"] = "";
            Session[SessionKey] = "";
            SetPreviewImage("");
        }
    }
}
