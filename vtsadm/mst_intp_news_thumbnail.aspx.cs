using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vtsadm.App_Code;

namespace vtsadm
{
    public partial class mst_intp_news_thumbnail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINTPMSTNEWS"))
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
                if (Session["ClsTypeNewsThumbnail"].ToString() != "")
                {
                    ImgInstall.ImageUrl = "~/Picture/intp/news/thumbnail/" + Session["ClsTypeNewsThumbnail"].ToString();
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
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "JPEG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "PNG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/intp/news/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);

                        string strFullPathThumb = Server.MapPath("~/Picture/intp/news/thumbnail/") + strFileName;
                        FileUpload1.SaveAs(strFullPathThumb);

                        System.Drawing.Image obj;
                        Bitmap newimg;
                        obj = System.Drawing.Image.FromFile(strFullPath);

                        ImageFormat imgformat = obj.RawFormat;
                        newimg = new Bitmap(obj, 702, 651);
                        newimg.Save(strFullPathThumb, imgformat);
                        ImgInstall.ImageUrl = "~/Picture/intp/news/thumbnail/" + strFileName;
                        Session["ClsTypeNewsThumbnail"] = "https://vtsadmin.easygo-gps.co.id/picture/intp/news/thumbnail/" + strFileName;
                        lblMsg.InnerHtml = "<strong>Success!</strong>";

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
            }
            catch (Exception ex)
            {

            }

        }
    }
}