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
    public partial class new_installation_upload9 : System.Web.UI.Page
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

                if (Session["ClsTypeNewPicture9"].ToString() != "")
                {
                    ImgInstall9.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture9"].ToString();
                }
                else
                {
                    ImgInstall9.ImageUrl = "~/Picture/" + "noimage.png";
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
                if (FileUpload9.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload9.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload9.FileName, (FileUpload9.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload9.SaveAs(strFullPath);
                        ImgInstall9.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture9"] = strFileName;

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
                ImgInstall9.ImageUrl = "~/Picture/" + "noimage.png";
            }
            catch (Exception ex)
            {

            }

        }
    }
}