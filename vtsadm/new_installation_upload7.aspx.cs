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
    public partial class new_installation_upload7 : System.Web.UI.Page
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

                if (Session["ClsTypeNewPicture7"].ToString() != "")
                {
                    ImgInstall7.ImageUrl = "~/Picture/" + Session["ClsTypeNewPicture7"].ToString();
                }
                else
                {
                    ImgInstall7.ImageUrl = "~/Picture/" + "noimage.png";
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
                if (FileUpload7.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload7.FileName.ToUpper().Trim(), 4) == "JPEG")
                    {
                        string strFileName = ClsFunc.Left(FileUpload7.FileName, (FileUpload7.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        string strFullPath = Server.MapPath("~/Picture/") + strFileName;
                        FileUpload7.SaveAs(strFullPath);
                        ImgInstall7.ImageUrl = "~/Picture/" + strFileName;
                        Session["ClsTypeNewPicture7"] = strFileName;

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
                ImgInstall7.ImageUrl = "~/Picture/" + "noimage.png";
            }
            catch (Exception ex)
            {

            }

        }
    }
}