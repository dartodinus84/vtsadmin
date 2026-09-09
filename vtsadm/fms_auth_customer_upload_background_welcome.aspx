<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fms_auth_customer_upload_background_welcome.aspx.cs" Inherits="vtsadm.fms_auth_customer_upload_background_welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css" />
    <!-- daterange picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" />
    <!-- DataTables -->
    <link rel="stylesheet" href="Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="Content/plugins/iCheck/all.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css" />
    <!-- Bootstrap time Picker -->
    <link rel="stylesheet" href="Content/plugins/timepicker/bootstrap-timepicker.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="Content/bower_components/select2/dist/css/select2.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
  <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />

    <style type="text/css">
        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        }
    </style>

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/jquery-ui/jquery-ui.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>File Input</label>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <div class="box-footer">
                            <asp:Button ID="CmdUpload" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="CmdUpload_Click" CausesValidation="false" UseSubmitBehavior="true" />
                            <asp:Button ID="CmdRemove" runat="server" CssClass="btn btn-primary" Text="Remove" OnClick="CmdRemove_Click" CausesValidation="false" UseSubmitBehavior="false" />
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
                        <div id="divpic" class="form-group form-group-sm" style="height:195px;text-align:center;line-height:195px;">                            
                            <asp:Image ID="ImgInstall" runat="server" BorderWidth="0px" Style="max-height:195px;max-width:100%;vertical-align:middle;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            (function () {
                var fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
                var previewImg = document.getElementById('<%= ImgInstall.ClientID %>');
                var lbMsg = document.getElementById('<%= lblMsg.ClientID %>');

                if (fileInput && previewImg) {
                    fileInput.addEventListener('change', function () {
                        if (!this.files || !this.files[0]) {
                            return;
                        }

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            previewImg.src = e.target.result;
                            previewImg.style.display = 'inline';
                        };
                        reader.onerror = function () {
                            if (lbMsg) {
                                lbMsg.innerHTML = '<strong>Failed to read file!</strong>';
                                lbMsg.className = 'text-danger';
                            }
                        };
                        reader.readAsDataURL(this.files[0]);
                    });
                }

                if (lbMsg && lbMsg.innerHTML.trim() !== '') {
                    window.setTimeout(function () {
                        lbMsg.style.opacity = '0.4';
                    }, 2500);
                }
            })();
        </script>
    </form>
</body>
</html>
