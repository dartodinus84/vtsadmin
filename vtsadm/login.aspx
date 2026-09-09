<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="vtsadm.login" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>.:: EasyGo ::. VTS Administration</title>
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

     <link rel="icon" type="image/png" href="Content/favicon.png" sizes="64x32">
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
    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.cookie.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

</head>
<body class="hold-transition login-page" style="background-image: url('Content/bg.jpg'); background-size: cover; overflow: hidden;">
    <div class="login-box" style="margin-top:120px">
        <div class="login-logo" style="font-size:28px;margin-bottom:12px;">
            <a href="#"><b>VTS </b>Administration</a>
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body" style="border-radius: 11px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
            <p class="login-box-msg">Sign in to start your session</p>

            <form id="form1" runat="server">
                <div class="form-group has-feedback">
                    <input id="txtUserID" runat="server" type="text" class="form-control" required="required" placeholder="User ID" />
                    <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                </div>
                <div class="form-group has-feedback">
                    <input id="txtPassword" runat="server" type="password" class="form-control" required="required" placeholder="Password" />
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                </div>
                <div class="row">
                    <div class="col-xs-8">
                        <label runat="server" id="lblMsg"></label>
                    </div>
                    <!-- /.col -->
                    <div class="col-xs-4" style="text-align: right;">
                        <button id="CmdLogin" runat="server" type="submit" class="btn btn-primary" onserverclick="CmdLogin_Click">Sign In</button>
                    </div>
                    <!-- /.col -->
                </div>
            </form>
            <br />
            <p class="login-box-msg">VTS Administration. Copyright &copy; 2018 <a href="login.aspx"><strong>EasyGo Indonesia.</strong></a> All rights reserved</p>
        </div>

        <!-- /.login-box-body -->
    </div>
    <!-- /.login-box -->

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.cookie.js"></script>
    <script type="text/javascript" src="Content/plugins/iCheck/icheck.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            var remember = $.cookie('remember');
            if (remember == 'true') {
                var userid = $.cookie('userid');
                $('#txtUserID').val(userid);
            }

            $("#CmdLogin").click(function () {
                var userid = $('#txtUserID').val();
                $.cookie('userid', userid, { expires: 14 });
                $.cookie('remember', true, { expires: 14 });
            });
        });

        var lbMsg = document.getElementById('lblMsg');
        var isExists = lbMsg.innerHTML;
        //console.log(isExists);
        //console.log("test " + isExists + " test");
        if (isExists != '') {
            if (isExists.includes("Success")) {
                lbMsg.className = "btn btn-success";
            }
            else {
                lbMsg.className = "btn btn-danger";
            }
            window.setTimeout(function () { $('#lblMsg').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            //$('#modal-messagebox').modal('show');
        }
    </script>
</body>
</html>
