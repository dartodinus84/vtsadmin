<%@ Page Title=".:: EasyGo ::. VTS Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_igo.aspx.cs" Inherits="vtsadm.dashboard_igo" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .avgspeed-label {
            font-size: 20px;
        }

        .vehicle-label {
            font-size: 32px;
        }

        .icon {
            top: 0px !important;
            font-size: 40px !important;
        }

        .inner {
            height: 80px !important;
            padding-top: 2px !important;
        }

        .box-header, .with-border {
            height: 50px;
        }

        .small-box-footer {
            border-bottom-left-radius: 11px;
            border-bottom-right-radius: 11px;
        }

        .small-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .btn-app {
            border: 0px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .card-body {
            border-radius: 11px;
            height: 100px;
        }

        .card-header {
            background-size: cover;
            overflow: hidden;
            margin-left: 5%;
            top: -7px;
            width: 90%;
            border-radius: 11px;
            height: 100px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box-group-body-dash {
            background-color: #e9edf2;
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box-body {
            border-radius: 11px;
        }

        .box-group-header-dash {
            border-top-left-radius: 11px;
            border-top-right-radius: 11px;
        }

        .box-group-dash {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .col-card-dash {
            margin-top: 10px;
            padding-left: 5px;
            padding-right: 5px;
        }

        .button {
            background: url('Content/bg11.jpg'); /* Green */
            border: none;
            color: white;
            padding: 8px 16px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 12px;
            margin: 4px 2px;
            -webkit-transition-duration: 0.4s; /* Safari */
            transition-duration: 0.4s;
            cursor: pointer;
            border-radius: 6px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .buttonInner {
            color: white;
        }

            .buttonInner:hover {
                color: lightgrey;
            }

        .info-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            min-height: 60px;
        }

        .info-box-icon {
            border-bottom-left-radius: 11px;
            border-top-left-radius: 11px;
            /*box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);*/
            height: 60px;
            font-size: 34px;
            line-height: 60px;
        }

        .info-box-text {
            text-transform: unset !important;
        }
    </style>
    <section class="content-header">
        <h1>Dashboard
        <small>iGO Tracker</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#">iGO Tracker</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-lg-3 col-xs-6">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="fa fa-cog"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Stock</span>
                        <span class="info-box-number" runat="server" id="lblStock">1,410</span>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="info-box">
                    <span class="info-box-icon bg-green"><i class="fa fa-shopping-cart"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Sale</span>
                        <span class="info-box-number" runat="server" id="lblSale">1,410</span>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow"><i class="fa fa-user"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Customer</span>
                        <span class="info-box-number" runat="server" id="lblCust">1,410</span>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="info-box">
                    <span class="info-box-icon bg-red"><i class="fa fa-close"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Expired</span>
                        <span class="info-box-number" runat="server" id="lblDeactive">1,410</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Schedule
                    </div>
                    <div class="box-body">
                        <div id="container"></div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-xs-12">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Latest Orders</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div class="table-responsive">
                            <table class="table no-margin">
                                <thead>
                                    <tr>
                                        <th>Order ID</th>
                                        <th>Item</th>
                                        <th>Status</th>
                                        <th>Popularity</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR9842</a></td>
                                        <td>Call of Duty IV</td>
                                        <td><span class="label label-success">Shipped</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#00a65a" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR1848</a></td>
                                        <td>Samsung Smart TV</td>
                                        <td><span class="label label-warning">Pending</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#f39c12" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR7429</a></td>
                                        <td>iPhone 6 Plus</td>
                                        <td><span class="label label-danger">Delivered</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#f56954" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR7429</a></td>
                                        <td>Samsung Smart TV</td>
                                        <td><span class="label label-info">Processing</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#00c0ef" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR1848</a></td>
                                        <td>Samsung Smart TV</td>
                                        <td><span class="label label-warning">Pending</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#f39c12" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR7429</a></td>
                                        <td>iPhone 6 Plus</td>
                                        <td><span class="label label-danger">Delivered</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#f56954" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><a href="pages/examples/invoice.html">OR9842</a></td>
                                        <td>Call of Duty IV</td>
                                        <td><span class="label label-success">Shipped</span></td>
                                        <td>
                                            <div class="sparkbar" data-color="#00a65a" data-height="20">
                                                <canvas style="display: inline-block; width: 34px; height: 20px; vertical-align: top;" width="34" height="20"></canvas>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.table-responsive -->
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer clearfix">
                        <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-left">Place New Order</a>
                        <a href="javascript:void(0)" class="btn btn-sm btn-default btn-flat pull-right">View All Orders</a>
                    </div>
                    <!-- /.box-footer -->
                </div>
            </div>
        </div>
    </section>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/2.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Content/bower_components/animated-event-calendar/dist/simple-calendar.css">
    <script src="Content/bower_components/animated-event-calendar/src/jquery.simple-calendar.js"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        var socket = new WebSocket('ws://localhost:8081/');
        // When a connection is made
        socket.onopen = function () {
            console.log('Opened connection 🎉');

            // send data to the server
            var json = JSON.stringify({ message: 'Hello 👋' });
            socket.send(json);
        }

        // When data is received
        socket.onmessage = function (event) {
            console.log(event.data);
        }

        // A connection could not be made
        socket.onerror = function (event) {
            console.log(event);
        }

        // A connection was closed
        socket.onclose = function (code, reason) {
            console.log(code, reason);
        }

        // Close the connection when the window is closed
        window.addEventListener('beforeunload', function () {
            socket.close();
        });


        function endRequest(sender, args) {

            console.log(window.screen.width);
            if (window.screen.width > 720) {
                document.getElementById("container").style.height = '100%';
            } else {
                document.getElementById("container").style.height = '100%';
            }
            console.log(document.getElementById("container"));

        }
        endRequest();
    </script>
    <script>
        var $calendar;
        $.ajax({
            type: "POST",
            url: "dashboard_igo.aspx/technicianIGOSchedule",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (r) {
                var json = JSON.parse(r.d);
                data = json.data;
                $(document).ready(function () {
                    let container = $("#container").simpleCalendar({
                        fixedStartDay: 0, // begin weeks by sunday
                        disableEmptyDetails: true,
                        events: data,
                    });
                    $calendar = container.data('plugin_simpleCalendar')
                });
            },
            error: function (e) {
                console.log(e);
            }
        });
    </script>
</asp:Content>
