using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class fms_auth_customer_upload_fav : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUFMSAUTHCUST"))
                {
                    Response.Redirect("dashboard.aspx");
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
        protected void Clear()
        {
            try
            {
                if (Session["ClsTypeFMSCustomerFavicon"].ToString() != "")
                {
                    ImgInstall.ImageUrl = "~/Picture/" + Session["ClsTypeFMSCustomerFavicon"].ToString();
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
        protected void CmdUpload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                ClsType ClType = new ClsType();
                if (FileUpload1.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "PNG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);
                        ImgInstall.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeFMSCustomerFavicon"] = strFileName;
                        lblMsg.InnerHtml = "<strong>Success!</strong>";
                        //FileUpload1

                        //var oFn = FileUpload1.FileName;
                        //if (strFileName != "" && ImageText.Text != "")
                        //{
                        //    //var fn = $"{oFn.Replace(".jpg", "")}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg";
                        //    //var path = $"{Server.MapPath("~/Picture/")}{fn}";
                        //    File.WriteAllBytes(strFullPath, Convert.FromBase64String(ImageText.Text));
                        //    ImgInstall.ImageUrl = "~/Picture/" + strFileName;
                        //    Session["ClsTypeNewPicture"] = strFileName;
                        //    lblMsg.InnerHtml = "<strong>Success!</strong>";

                        //    //Session["ClsTypeNewPicture"] = fn;
                        //    //lblMsg.InnerHtml = "<strong>Success!</strong>";
                        //}


                        //System.Drawing.Image img = System.Drawing.Image.FromFile(strFullPath);
                        //System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                        ////int h = img.Height;
                        //// int w = img.Width;
                        //int width = 70;
                        //int X = img.Width;
                        //int Y = img.Height;
                        //int height = (int)((width * Y) / X);
                        ////  System.Drawing.Image oThumbnail = img.GetThumbnailImage(100, Convert.ToInt32((img.Height / (img.Width / 100))), myCallback, IntPtr.Zero);
                        //System.Drawing.Image oThumbnail = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                        //img.Dispose();
                        //img = null;
                        //oThumbnail.Save(Response.OutputStream, ImageFormat.Png);


                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong>";
            }
        }

        protected void CmdRemove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.InnerHtml = "";
                ImgInstall.ImageUrl = "";
                Session["ClsTypeFMSCustomerFavicon"] = "";
            }
            catch (Exception ex)
            {

            }

        }
    }
}