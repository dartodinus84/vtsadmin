<%@ Page Title=".:: EasyGo ::. VTS Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_calendar.aspx.cs" Inherits="vtsadm.dashboard_calendar" EnableEventValidation="false" %>
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
        <small>Calendar</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="vehicle_position.aspx">Position</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntOpen" runat="server">1,000</label>
                        </h3>
                        <p>Open</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <a href="view_assign_technician.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntDone" runat="server">1,000</label>
                        </h3>
                        <p>Done</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <a href="view_assign_technician.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntLate" runat="server">1,000</label>
                        </h3>
                        <p>Duedate</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <a href="view_assign_technician.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntAvail" runat="server">1,000</label>
                        </h3>
                        <p>Technician Avaliable</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-user"></i>
                    </div>
                    <a href="view_avaliable_technician.aspx" class="small-box-footer" target="_blank">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-2 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Maintenance Branch
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="#">Jakarta <span id="lblJKT1" runat="server" class="pull-right badge bg-aqua">5</span></a></li>
                            <li style="height: 46px;"><a href="#">Medan<span id="lblMDN1" runat="server" class="pull-right badge bg-yellow">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Semarang<span id="lblSMG1" runat="server" class="pull-right badge bg-blue">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Surabaya<span id="lblSBY1" runat="server" class="pull-right badge bg-green">12</span></a></li>
                            <li style="height: 46px;"><a href="#">Padang<span id="lblPDG1" runat="server" class="pull-right badge bg-maroon">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Palembang<span id="lblPLG1" runat="server" class="pull-right badge bg-maroon">842</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-lg-2 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        New Installment Branch
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="#">Jakarta <span id="lblJKT" runat="server" class="pull-right badge bg-aqua">5</span></a></li>
                            <li style="height: 46px;"><a href="#">Medan<span id="lblMDN" runat="server" class="pull-right badge bg-yellow">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Semarang<span id="lblSMG" runat="server" class="pull-right badge bg-blue">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Surabaya<span id="lblSBY" runat="server" class="pull-right badge bg-green">12</span></a></li>
                            <li style="height: 46px;"><a href="#">Padang<span id="lblPDG" runat="server" class="pull-right badge bg-maroon">842</span></a></li>
                            <li style="height: 46px;"><a href="#">Palembang<span id="lblPLG" runat="server" class="pull-right badge bg-maroon">842</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-lg-8 col-xs-12">
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
   <%-- <script type="application/javascript">
        var $calendar;
        $.ajax({
            type: "POST",
            url: "dashboard_calendar.aspx/technicianSchedule",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (r) {
                var json = JSON.parse(r.d);
                data = json.data;
                $("#container").simpleCalendar({
                    data: data,
                    displayEvent: true,
                    disableEmptyDetails: true,
                    displayYear: true,
                    months: ['january', 'february', 'march', 'april', 'may', 'june', 'july', 'august', 'september', 'october', 'november', 'december'],
                    days: ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'],
                    fixedStartDay: true,
                    onInit: function (calendar) { },
                    onMonthChange: function (month, year) { },
                    onDateSelect: function (date, events) { },

                });
            },
            error: function (e) {
                console.log(e);
            }
        });
    </script>--%>
    <script>
        var $calendar;
        $.ajax({
            type: "POST",
            url: "dashboard_calendar.aspx/technicianSchedule",
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