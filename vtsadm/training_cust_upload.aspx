<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="training_cust_upload.aspx.cs" Inherits="vtsadm.training_cust_upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <style type="text/css">
        html, body, form {
            display: block !important;
            visibility: visible !important;
            height: auto !important;
            margin: 0;
            padding: 0;
            background: #f7f7f7;
        }

        .box, .box-body {
            display: block !important;
            visibility: visible !important;
        }
    </style>
    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>File Input</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" accept="image/*,.jpg,.jpeg,.png,image/jpeg,image/png" />
                            <asp:TextBox ID="ImageText" runat="server" CssClass="hidden" />
                        </div>
                        <div class="box-footer">
                            <button id="CmdUpload" runat="server" type="button" class="btn btn-primary" onserverclick="CmdUpload_ServerClick">Upload</button>
                            <button id="CmdRemove" runat="server" type="button" class="btn btn-primary" onserverclick="CmdRemove_ServerClick">Remove</button>
                            <div runat="server" id="lblMsg"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic" class="form-group form-group-sm" style="height:195px;text-align:center;">                            
                            <asp:Image ID="ImgInstall" runat="server" BorderWidth="0px" height="100%"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            var lbMsg = document.getElementById('lblMsg');
            var isExists = lbMsg ? lbMsg.innerHTML : '';
            if (isExists != '') {
                if (isExists.indexOf('Success') >= 0) {
                    lbMsg.className = "btn btn-success";
                }
                else {
                    lbMsg.className = "btn btn-danger";
                }
                window.setTimeout(function () { $('#lblMsg').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            }
        </script>
    </form>
</body>
</html>
