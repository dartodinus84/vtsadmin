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
    public partial class customer_upload_doc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClsType ClType = new ClsType();
                if (!Session["ClsTypeAccessMenu"].ToString().ToUpper().Contains("MNUMSTCUST"))
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
                if (Session["ClsTypeAttacment"].ToString() != "")
                {
                    //ImgInstall.Text = "~/Attachment/" + Session["ClsTypeAttacment"].ToString();
                }
                else
                {

                    //ImgInstall.Text = "~/Attachment/" + "noimage.png";
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
                ClsType ClType = new ClsType(); string sErr = ""; string sBatchNo = "";

                if (FileUpload1.FileName != "")
                {
                    if (ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "JPG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "PNG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "JPEG" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3) == "PDF" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "XLSX" || ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 4) == "DOCX")
                    {
                        string strFileName = ClsFunc.Left(FileUpload1.FileName, (FileUpload1.FileName.Length - 4)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + ClsFunc.Right(FileUpload1.FileName.ToUpper().Trim(), 3);
                        string strFullPath = Server.MapPath("~/Attachment/") + strFileName;
                        FileUpload1.SaveAs(strFullPath);
                        //ImgInstall.Text = "~/Attachment/" + strFileName;
                        Session["ClsTypeAttacment"] = strFileName;
                        lblMsg.InnerHtml = "<strong>Success!</strong>";

                    }
                    else {
                        lblMsg.InnerHtml = "<strong>Failed!</strong> Upload file has been failed (" + sErr + ")!";
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
                //ImgInstall.Text = "~/Attachment/" + "noimage.png";

            }
            catch (Exception ex)
            {

            }

        }
    }
}