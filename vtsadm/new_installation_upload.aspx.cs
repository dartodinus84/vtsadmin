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
    public partial class new_installation_upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUINSTALLNEW"))
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
                if (Session["ClsTypeNewPicture"].ToString() != "")
                {
                    ImgInstall.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture"].ToString();
                }
                else
                {
               
                    ImgInstall.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture2"].ToString() != "")
                {
                    ImgInstall2.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture2"].ToString();
                }
                else
                {
                    ImgInstall2.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture3"].ToString() != "")
                {
                    ImgInstall3.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture3"].ToString();
                }
                else
                {
                    ImgInstall3.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture4"].ToString() != "")
                {
                    ImgInstall4.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture4"].ToString();
                }
                else
                {
                    ImgInstall4.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture5"].ToString() != "")
                {
                    ImgInstall5.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture5"].ToString();
                }
                else
                {
                    ImgInstall5.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture6"].ToString() != "")
                {
                    ImgInstall6.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture6"].ToString();
                }
                else
                {
                    ImgInstall6.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture7"].ToString() != "")
                {
                    ImgInstall7.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture7"].ToString();
                }
                else
                {
                    ImgInstall7.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture8"].ToString() != "")
                {
                    ImgInstall8.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture8"].ToString();
                }
                else
                {
                    ImgInstall8.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture9"].ToString() != "")
                {
                    ImgInstall9.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture9"].ToString();
                }
                else
                {
                    ImgInstall9.ImageUrl = "~/Picture/" + "noimage.png";
                }

                if (Session["ClsTypeNewPicture10"].ToString() != "")
                {
                    ImgInstall10.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture10"].ToString();
                }
                else
                {
                    ImgInstall10.ImageUrl = "~/Picture/" + "noimage.png";
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
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);
                        ImgInstall.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture"] = strFileName;
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

                    if (ClsFunc.Right(FileUpload2.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload2.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload2.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload2.FileName, (FileUpload2.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload2.SaveAs(strFullPath);
                        ImgInstall2.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture2"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload3.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload3.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload3.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload3.FileName, (FileUpload3.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload3.SaveAs(strFullPath);
                        ImgInstall3.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture3"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload4.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload4.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload4.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload4.FileName, (FileUpload4.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload4.SaveAs(strFullPath);
                        ImgInstall4.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture4"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload5.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload5.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload5.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload5.FileName, (FileUpload5.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload5.SaveAs(strFullPath);
                        ImgInstall5.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture5"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload6.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload6.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload6.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload6.FileName, (FileUpload6.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload6.SaveAs(strFullPath);
                        ImgInstall6.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture6"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload7.FileName, (FileUpload7.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload7.SaveAs(strFullPath);
                        ImgInstall7.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture7"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload8.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload8.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload8.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload8.FileName, (FileUpload8.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload8.SaveAs(strFullPath);
                        ImgInstall8.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture8"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload9.FileName, (FileUpload9.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload9.SaveAs(strFullPath);
                        ImgInstall9.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture9"] = strFileName;

                    }

                    if (ClsFunc.Right(FileUpload10.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload10.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload10.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload10.FileName, (FileUpload10.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload10.SaveAs(strFullPath);
                        ImgInstall10.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture10"] = strFileName;

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
                ImgInstall.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall2.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall3.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall4.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall5.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall6.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall7.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall8.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall9.ImageUrl = "~/Picture/" + "noimage.png";
                ImgInstall10.ImageUrl = "~/Picture/" + "noimage.png";


            }
            catch(Exception ex)
            {

            }

        }
    }
}