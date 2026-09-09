<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="navbar.aspx.cs" Inherits="vtsadm.navbar" %>

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
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <style type="text/css">
        .btn-app {
            margin: -5px 0px 0px 0px;
            min-width: 70px;
            padding: 10px 5px;
            height: 50px;
            border-width: 0px;
            background-color: inherit;
        }

        .center-navbar {
            width: 25%;
            text-align: center;
            margin-left: -10px;
        }
    </style>
    <script type="text/javascript">
        function setMinDateTo(objDate) {
            document.getElementById('dtto').min = objDate.value;
        }
        function setMaxDateFrom(objDate) {
            document.getElementById('dtfrom').max = objDate.value;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="box box-solid">
            <div class="box-body">
                <table style="width: 100%;">
                    <tr>
                        <%--                        <td class="center-navbar">
                            <input type="date" id="dtfrom" onchange="setMinDateTo(this);"/>
                            <input type="date" id="dtto" onchange="setMaxDateFrom(this);" />
                        </td>--%>
                        <td class="center-navbar">
                            <a class="btn btn-app" runat="server" style="border-radius: 14px;" onclick="parent.home_click();" ><i class="fa fa-home"></i>Home</a>
                        </td>
                        <td class="center-navbar">
                            <a class="btn btn-app" style="border-radius: 14px;" onclick="parent.position_click();"><i class="fa fa-map-marker"></i>Position</a>
                        </td>
                        <td class="center-navbar">
                            <a class="btn btn-app" style="border-radius: 14px;" onclick="parent.changepass_click();"><i class="fa fa-lock"></i>Change Password</a>
                        </td>
                        <td class="center-navbar">
                            <a class="btn btn-app" style="border-radius: 14px;" onclick="parent.logout_click();"><i class="fa fa-power-off" ></i>Sign Out</a>
                        </td>

                    </tr>
                </table>
            </div>
        </div>
    </form>
    <script type="text/javascript">

    </script>
</body>
</html>
