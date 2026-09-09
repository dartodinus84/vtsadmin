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
    public partial class mst_tms_promo_image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUTMSMSTPROM"))
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
                if (Session["ClsTypePromoTMSImage"].ToString() != "")
                {
                    ImgInstall.ImageUrl = "~/Picture/tms/promo/image/" + Session["ClsTypePromoTMSImage"].ToString();
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
                        string strFullPath = Server.MapPath("~/Picture/tms/promo/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);

                        string strFullPathThumb = Server.MapPath("~/Picture/tms/promo/image/") + strFileName;
                        FileUpload1.SaveAs(strFullPathThumb);

                        System.Drawing.Image obj;
                        Bitmap newimg;
                        obj = System.Drawing.Image.FromFile(strFullPath);

                        ImageFormat imgformat = obj.RawFormat;
                        newimg = new Bitmap(obj, 852, 649);
                        newimg.Save(strFullPathThumb, imgformat);


                        Session["ClsTypePromoTMSImage"] = "https://vtsadmin.easygo-gps.co.id/picture/tms/promo/image/" + strFileName;

                        lblMsg.InnerHtml = "<strong>Success!</strong>";

                        //string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        //string strFullPath = Server.MapPath("~/Picture/tms/promo/image/") + strFileName;
                        //FileUpload1.SaveAs(strFullPath);
                        //ImgInstall.ImageUrl = "~/Picture/tms/promo/image/" + strFileName;
                        //Session["ClsTypePromoTMSImage"] = strFileName;
                        //lblMsg.InnerHtml = "<strong>Success!</strong>";


                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerHtml = "<strong>Failed!</strong>";
            }
        }

        protected void ResizeImage(string file, int height, int width)
        {
            try
            {
                System.Drawing.Image obj1;
                Bitmap newimg1;
                obj1 = System.Drawing.Image.FromFile(file);

                ImageFormat imgformat1 = obj1.RawFormat;
                newimg1 = new Bitmap(obj1, height, width);
                newimg1.Save(file, imgformat1);
                Session["ClsTypeNewsImage"] = file;

            }
            catch (Exception ex)
            {

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