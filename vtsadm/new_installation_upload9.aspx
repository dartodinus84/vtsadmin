<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="new_installation_upload9.aspx.cs" Inherits="vtsadm.new_installation_upload9" %>

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
    <!-- AdminLTE Skins. Choose a skin from the css/skins
           folder instead of downloading all of them to reduce the load. -->
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
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
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="form-group col-lg-12">
                                                <label>File</label>
                                                <asp:FileUpload ID="FileUpload9" runat="server" />
                                                <asp:TextBox ID="ImageText9" runat="server" CssClass="hidden" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
            <div class="col-md-12" id="boxpic9">
                <div class="box box-solid" style="background-color: #f7f7f7;">
                    <div class="box-body">
                        <div id="divpic9" class="form-group form-group-sm" style="height:200px;text-align:center;">                            
                            <asp:Image ID="ImgInstall9" runat="server" BorderWidth="0px" height="100%"/>
                        </div>

                    </div>
                </div>
            </div>

        </div>
        <script type="text/javascript">
            
            var lbMsg = document.getElementById('lblMsg');
            var isExists = lbMsg.innerHTML;


            if (isExists != '') {
                if (isExists.includes("Success")) {
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