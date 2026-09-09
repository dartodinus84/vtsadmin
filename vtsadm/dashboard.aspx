<%@ Page Title=".:: EasyGo ::. VTS Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="vtsadm.dashboard" EnableEventValidation="false" %>

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
        <small>detail of applications</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="vehicle_position.aspx">Position</a></li>
        </ol>
    </section>


    <section class="content">
        <%--        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-body" style="height: 55px;">
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Date</label>
                                    </span>
                                    <input type="date" id="txtDate" class="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Time</label>
                                    </span>
                                    <input type="time" id="txtTime" class="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <button id="CmdSearch" class="btn btn-primary">Search</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <div class="col-md-3 col-xs-3">
                            <h4 class="box-title">Lintasan 1</h4>
                        </div>
                        <div class="col-md-4 col-xs-3">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Avg. Speed (km/h)</label>
                                    </span>
                                    <input type="text" id="txtLin1Avg" class="form-control" placeholder="Average Speed ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Total Vehicles</label>
                                    </span>
                                    <input type="text" id="txtLin1Veh" class="form-control" placeholder="Total Vehicles ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1 col-xs-3">
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-primary btn-xs" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-primary btn-xs" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin1Veh1">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin1Avg1">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cawang - Bekasi <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-2 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin1Veh2">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin1Avg2">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Bekasi - Cikopo <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-2 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin1Veh3">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin1Avg3">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cikopo - Subang <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-2 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin1Veh4">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin1Avg4">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Subang - Palimanan <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin1Veh5">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin1Avg5">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Palimanan - Pejagan <i class="fa fa-circle" style="color: red;"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <div class="col-md-3 col-xs-3">
                            <h4 class="box-title">Lintasan 2</h4>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Avg. Speed (km/h)</label>
                                    </span>
                                    <input type="text" id="txtLin2Avg" class="form-control" placeholder="Average Speed ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Total Vehicles</label>
                                    </span>
                                    <input type="text" id="txtLin2Veh" class="form-control" placeholder="Total Vehicles ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1 col-xs-1">
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-primary btn-xs" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-primary btn-xs" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin2Veh1">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin2Avg1">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Karawang - Sadang <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin2Veh2">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin2Avg2">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Sadang - Jatiluhur <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin2Veh3">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin2Avg3">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Jatiluhur - Padalarang <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin2Veh4">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin2Avg4">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Padalarang - Cileunyi <i class="fa fa-circle" style="color: red;"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <div class="col-md-3 col-xs-3">
                            <h4 class="box-title">Lintasan 3</h4>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Avg. Speed (km/h)</label>
                                    </span>
                                    <input type="text" id="txtLin3Avg" class="form-control" placeholder="Average Speed ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Total Vehicles</label>
                                    </span>
                                    <input type="text" id="txtLin3Veh" class="form-control" placeholder="Total Vehicles ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1 col-xs-1">
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-primary btn-xs" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-primary btn-xs" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh1">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg1">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cikopo - Jomin <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh2">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg2">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Jomin - Ciasem <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh3">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg3">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Ciasem - Pamanukan <i class="fa fa-arrow-circle-down" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh7">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg7">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Brebes - Tegal <i class="fa fa-circle" style="color: red;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh6">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg6">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cirebon - Brebes <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh5">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg5">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Losarang - Cirebon <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin3Veh4">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin3Avg4">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Pamanukan - Losarang <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <div class="col-md-3 col-xs-3">
                            <h4 class="box-title">Lintasan 4</h4>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Avg. Speed (km/h)</label>
                                    </span>
                                    <input type="text" id="txtLin4Avg" class="form-control" placeholder="Average Speed ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4">
                            <div class="form-group form-group-sm">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-btn">
                                        <label class="btn btn-primary btn-xs">Total Vehicles</label>
                                    </span>
                                    <input type="text" id="txtLin4Veh" class="form-control" placeholder="Total Vehicles ..." />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1 col-xs-1">
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-primary btn-xs" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-primary btn-xs" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh1">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg1">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cileunyi - Cicalengka <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh2">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg2">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Cicalengka - Nagrek - Limbangan <i class="fa fa-arrow-circle-right" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-4 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh3">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg3">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Limbangan - Malangbong <i class="fa fa-arrow-circle-down" style="color: greenyellow;"></i></a>
                            </div>
                        </div>




                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh7">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg7">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Ciamis - Banjar <i class="fa fa-circle" style="color: red;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh6">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg6">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Tasikmalaya - Ciamis <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh5">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg5">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Ciawi - Tasikmalaya <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6">
                            <div class="small-box bg-blue-gradient">
                                <div class="inner">
                                    <label class="vehicle-label" id="lblLin4Veh4">100</label>&nbsp;
                                    <p>
                                        <label class="avgspeed-label" id="lblLin4Avg4">60</label>&nbsp;km/h
                                    </p>
                                    <div class="icon">
                                        <i class="fa fa-truck"></i>
                                    </div>
                                </div>
                                <a href="#" class="small-box-footer">Malangbong - Ciawi <i class="fa fa-arrow-circle-left" style="color: greenyellow;"></i></a>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>--%>

        <%--<div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header">
                        <i class="fa fa-bar-chart"></i>
                        Chat Message
                    </div>
                    <div class="box-body">
                        <textarea id="txtMessage" runat="server" class="form-control" rows="2"></textarea>
                    </div>
                    <div class="box-footer">
                        <button id="CmdConsume" type="button" class="btn btn-primary" runat="server" onserverclick="CmdConsume_ServerClick">Consume</button>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-lg-12 col-xs-12">
                <div id="carousel-example-generic" class="carousel slide" data-ride="carousel" style="border-radius: 11px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); max-height: 200px; min-height: 100px;">
                    <!-- Indicators -->
                    <ol class="carousel-indicators">
                        <li data-target="#carousel-example-generic" data-slide-to="0" class="active"></li>
                        <li data-target="#carousel-example-generic" data-slide-to="1"></li>
                        <li data-target="#carousel-example-generic" data-slide-to="2"></li>
                    </ol>

                    <!-- Wrapper for slides -->
                    <div class="carousel-inner" role="listbox" style="border-radius: 11px; max-height: 200px; min-height: 100px;">
                        <div class="item active">
                            <img src="Content/others/img/header-1.jpg" alt="..." style="height: 100%; width: 100%;">
                            <div class="carousel-caption">
                            </div>
                        </div>
                        <div class="item">
                            <img src="Content/others/img/header-2.jpg" alt="..." style="height: 100%; width: 100%;">
                            <div class="carousel-caption">
                            </div>
                        </div>
                        <div class="item">
                            <img src="Content/others/img/header-3.jpg" alt="..." style="height: 100%; width: 100%;">
                            <div class="carousel-caption">
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Controls -->
                <a class="left carousel-control" href="#carousel-example-generic" role="button" data-slide="prev">
                    <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="right carousel-control" href="#carousel-example-generic" role="button" data-slide="next">
                    <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
                &nbsp;
            </div>
        </div>

        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntCustomer" runat="server">1,000</label>
                        </h3>
                        <p>Customer</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person"></i>
                    </div>
                    <a href="view_customer.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-green-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntVehicle" runat="server">1,000</label>
                        </h3>
                        <p>Vehicle</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-truck"></i>
                    </div>
                    <a href="view_vehicle.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDevice" runat="server">1,000</label>
                        </h3>
                        <p>Device</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-cubes"></i>
                    </div>
                    <a href="view_device.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>

                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntGsm" runat="server">1,000</label>
                        </h3>
                        <p>Gsm</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-phone"></i>
                    </div>
                    <a href="view_gsm.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntTechnician" runat="server">1,000</label>
                        </h3>
                        <p>Technician</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <a href="view_technician.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntMarketing" runat="server">1,000</label>
                        </h3>
                        <p>Marketing</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-users"></i>
                    </div>
                    <a href="view_marketing.aspx" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="fa fa-cart-plus"></i></span>
                    <div class="info-box-content">
                        <a href="po_create.aspx" class="info-box-text">Purchase Order</a>
                        <span id="LblCntPO" runat="server" class="info-box-number">0</span>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-red-gradient"><i class="fa fa-calendar-plus-o"></i></span>
                    <div class="info-box-content">
                        <a href="job_create.aspx" class="info-box-text">Job Order - New Installation</a>
                        <span id="LblCntJONew" runat="server" class="info-box-number">0</span>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-green-gradient"><i class="fa fa-calendar-check-o"></i></span>
                    <div class="info-box-content">
                        <a href="job_maint.aspx" class="info-box-text">Job Order - Maintenance</a>
                        <span id="LblCntJOMaint" runat="server" class="info-box-number"></span>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow-gradient"><i class="fa fa-graduation-cap"></i></span>
                    <div class="info-box-content">
                        <a href="job_training.aspx" class="info-box-text">Job Order - Training</a>
                        <span id="LblCntJOTraining" runat="server" class="info-box-number"></span>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-4 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="fa fa-truck"></i></span>
                    <div class="info-box-content">
                        <a href="view_unit_active.aspx" class="info-box-text">Unit Active</a>
                        <span id="LblCntUnitActive" runat="server" class="info-box-number">0</span>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-red-gradient"><i class="fa fa-truck"></i></span>
                    <div class="info-box-content">
                        <a href="view_unit_inactive.aspx" class="info-box-text">Unit Inactive</a>
                        <span id="LblCntUnitInactive" runat="server" class="info-box-number">0</span>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow-gradient"><i class="fa fa-truck"></i></span>
                    <div class="info-box-content">
                        <a href="view_unit_delay.aspx" class="info-box-text">Unit Delay</a>
                        <span id="LblCntUnitDelay" runat="server" class="info-box-number">0</span>
                    </div>
                </div> 
            </div>
            
        </div>


        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header">
                        <i class="fa fa-list-alt"></i>
                        Menu
                    </div>
                    <div class="box-body" style="overflow-x: scroll;">
                        <div id="PnlMenu" runat="server" style="width: 1620px !important;">
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header">
                        <i class="fa fa-bar-chart"></i>
                        Marketing Performance
                    </div>
                    <div class="box-body">
                        <div id="chartdiv" style="height: 300px;">
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="col-md-3">
                <div class="box box-solid">
                    <div class="box-header">
                        <strong>Detail Chiller</strong>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6">
                            <div class="form-group form-group-sm" style="text-align: center;">
                                <label style="font-size: 16px;">Actual</label>
                                <p>
                                    <label id="lblChillerActual" style="font-size: 26px;">10</label>
                                </p>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group form-group-sm" style="text-align: center;">
                                <label style="font-size: 16px;">Normal</label>
                                <p>
                                    <label id="lblChillerNormal" style="font-size: 26px;">10</label>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header">
                        <strong>Detail Frezzer</strong>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6">
                            <div class="form-group form-group-sm" style="text-align: center;">
                                <label style="font-size: 16px;">Actual</label>
                                <p>
                                    <label id="lblFreezerActual" style="font-size: 26px;">10</label>
                                </p>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group form-group-sm" style="text-align: center;">
                                <label style="font-size: 16px;">Normal</label>
                                <p>
                                    <label id="lblFreezerNormal" style="font-size: 26px;">10</label>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>

        <div class="row">
            <div class="col-md-3 col-xs-12 box-job-order">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Job Order</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner" onclick="location.href = 'http://localhost:51638/job_create.aspx';">New Installation</button>
                        <button class="button buttonInner">Maintenance</button>
                    </div>
                </div>
            </div>

            <div class="col-md-9 col-xs-12 box-master">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Master</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">Customer</button>
                        <button class="button buttonInner">Device</button>
                        <button class="button buttonInner">Gsm</button>
                        <button class="button buttonInner">Vehicle</button>
                        <button class="button buttonInner">Warehouse</button>
                        <button class="button buttonInner">Technician</button>
                        <button class="button buttonInner">Marketing</button>
                        <button class="button buttonInner">Vendor - Maintenance</button>
                        <button class="button buttonInner">Vendor - Specialization</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-7 col-xs-12 box-reference">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Reference</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">Branch</button>
                        <button class="button buttonInner">Customer Type</button>
                        <button class="button buttonInner">Device Type</button>
                        <button class="button buttonInner">Provider</button>
                        <button class="button buttonInner">ID Type</button>
                        <button class="button buttonInner">Log Status</button>
                        <button class="button buttonInner">Server</button>
                        <button class="button buttonInner">Device Group</button>
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-xs-12 box-mutation">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Mutation</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">Dvc - Warehouse</button>
                        <button class="button buttonInner">Dvc - Technician</button>
                        <button class="button buttonInner">Gsm - Warehouse</button>
                        <button class="button buttonInner">Gsm - Technician</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 col-xs-12 box-setup">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Setup</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">Vehicle - Customer</button>
                        <button class="button buttonInner">Vehicle - Upline</button>
                        <button class="button buttonInner">Vehicle - Master</button>
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-xs-12 box-installation">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Installation</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">New</button>
                        <button class="button buttonInner">Maint - Device</button>
                        <button class="button buttonInner">Maint - Gsm</button>
                        <button class="button buttonInner">Maint - Vehicle</button>
                        <button class="button buttonInner">Maint - Accessories</button>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-xs-12 box-invoice">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>Invoice</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">New</button>
                        <button class="button buttonInner">Maintenance</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12 box-view">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>View</strong>
                    </div>
                    <div class="box-body">
                        <button id="CmdViewJONew" class="button buttonInner" runat="server" onserverclick="CmdViewJONew_ServerClick">Job Order - New</button>
                        <button class="button buttonInner">Job Order - Maint</button>
                        <button class="button buttonInner">Customer</button>
                        <button class="button buttonInner">Device</button>
                        <button class="button buttonInner">Gsm</button>
                        <button class="button buttonInner">Vehicle</button>
                        <button class="button buttonInner">Warehouse</button>
                        <button class="button buttonInner">Technician</button>
                        <button class="button buttonInner">Marketing</button>
                        <button class="button buttonInner">Vendor</button>
                        <button class="button buttonInner">Mtn - Device</button>
                        <button class="button buttonInner">Mtn - Gsm</button>
                        <button class="button buttonInner">Assign - Vehicle</button>
                        <button class="button buttonInner">Installation</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12 box-user-access">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <strong>User Access</strong>
                    </div>
                    <div class="box-body">
                        <button class="button buttonInner">User</button>
                        <button class="button buttonInner">Group</button>
                        <button class="button buttonInner">Menu</button>
                        <button class="button buttonInner">Authentication - Group</button>
                        <button class="button buttonInner">Authentication - User</button>
                    </div>
                </div>
            </div>
        </div>
        <!--
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid box-group-dash">
                    <div class="box-header bg-gray-light box-group-header-dash">
                        <strong>Device</strong>
                    </div>
                    <div class="box-body">
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-6 col-card-dash">
                            <div class="box box-solid box-group-body-dash">
                                <div class="box-header card-header" style="background-image: url(picture/at5.jpg);">
                                </div>
                                <div class="box-body card-body">
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                    <h6>testing</h6>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        -->

        <%--<div class="row">
            <div class="col-md-4 connectedSortable">
                <div class="box box-solid">
                    <div class="box-header no-border">
                        <i class="fa fa-bar-chart-o"></i>
                        <h3 class="box-title">Customer By Branch</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="BarChartCustByBranch" style="height: 220px;"></div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 connectedSortable">
                <!-- Donut chart -->
                <div class="box box-solid">
                    <!--  bg-light-blue-gradient -->
                    <div class="box-header no-border">
                        <i class="fa fa-bar-chart-o"></i>
                        <h3 class="box-title">Device By Warehouse</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="BarChartDeviceByWarehouse" style="height: 220px;"></div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 connectedSortable">
                <!-- Donut chart -->
                <div class="box box-solid">
                    <!--  bg-light-blue-gradient -->
                    <div class="box-header no-border">
                        <i class="fa fa-bar-chart-o"></i>
                        <h3 class="box-title">Gsm By Warehouse</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="BarChartGsmByWarehouse" style="height: 220px;"></div>
                    </div>
                </div>
            </div>
        </div>--%>
        <!-- /.row -->


        <%--      <div class="row">
        <div class="col-md-4">
          <!-- Widget: user widget style 1 -->
          <div class="box box-widget widget-user">
            <!-- Add the bg color to the header using any of the bg-* classes -->
            <div class="widget-user-header bg-black-active">
              <!--
              <div class="widget-user-image">
                <img class="img-circle" src="../dist/img/user7-128x128.jpg" alt="User Avatar">
              </div>
              -->
              <!-- /.widget-user-image -->
              <h3 class="widget-user-username">Information</h3>
              <h5 class="widget-user-desc">Information Details</h5>
            </div>
            <div class="box-footer no-padding">
              <ul class="nav nav-stacked">
                <li><a href="#">Information 1 <span class="pull-right badge bg-blue"><asp:Label id="Label5" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 2 <span class="pull-right badge bg-aqua"><asp:Label id="Label6" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 3 <span class="pull-right badge bg-green"><asp:Label id="Label7" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 4 <span class="pull-right badge bg-red"><asp:Label id="Label8" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
              </ul>
            </div>
          </div>
          <!-- /.widget-user -->
        </div>
        <!-- /.col -->
        <div class="col-md-4">
          <!-- Widget: user widget style 1 -->
          <div class="box box-widget widget-user">
            <!-- Add the bg color to the header using any of the bg-* classes -->
            <div class="widget-user-header bg-green-gradient">
              <!--
              <div class="widget-user-image">
                <img class="img-circle" src="../dist/img/user7-128x128.jpg" alt="User Avatar">
              </div>
              -->
              <!-- /.widget-user-image -->
              <h3 class="widget-user-username">Information</h3>
              <h5 class="widget-user-desc">Information Details</h5>
            </div>
            <div class="box-footer no-padding">
              <ul class="nav nav-stacked">
                <li><a href="#">Information 1 <span class="pull-right badge bg-blue"><asp:Label id="Label17" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 2 <span class="pull-right badge bg-aqua"><asp:Label id="Label18" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 3 <span class="pull-right badge bg-green"><asp:Label id="Label19" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 4 <span class="pull-right badge bg-red"><asp:Label id="Label20" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
              </ul>
            </div>
          </div>
          <!-- /.widget-user -->
        </div>
        <!-- /.col -->        
        <div class="col-md-4">
          <!-- Widget: user widget style 1 -->
          <div class="box box-widget widget-user">
            <!-- Add the bg color to the header using any of the bg-* classes -->
            <div class="widget-user-header bg-maroon-gradient">
              <!--
              <div class="widget-user-image">
                <img class="img-circle" src="../dist/img/user7-128x128.jpg" alt="User Avatar">
              </div>
              -->
              <!-- /.widget-user-image -->
              <h3 class="widget-user-username">Information</h3>
              <h5 class="widget-user-desc">Information Details</h5>
            </div>
            <div class="box-footer no-padding">
              <ul class="nav nav-stacked">
                <li><a href="#">Information 1 <span class="pull-right badge bg-blue"><asp:Label id="Label1" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 2 <span class="pull-right badge bg-aqua"><asp:Label id="Label2" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 3 <span class="pull-right badge bg-green"><asp:Label id="Label3" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
                <li><a href="#">Information 4 <span class="pull-right badge bg-red"><asp:Label id="Label4" CssClass="FormatLabelNum" runat="server" Text="&nbsp;"></asp:Label></span></a></li>
              </ul>
            </div>
          </div>
          <!-- /.widget-user -->
        </div>
        <!-- /.col -->   
      </div>
      <!-- /.row -->--%>
    </section>



    <script src="https://www.amcharts.com/lib/4/core.js"></script>
    <script src="https://www.amcharts.com/lib/4/charts.js"></script>
    <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        
        am4core.ready(function () {

            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartdiv", am4charts.XYChart);
            chart.data = null;
            $.ajax({
                type: "POST",
                url: "dashboard.aspx/marketingPerformance",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    var json = JSON.parse(r.d);
                    chart.data = json.data;
                },
                error: function (e) {
                    console.log(e);
                }
            });
            if (window.screen.width > 720) {
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            }
            else {
                var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.renderer.inversed = true;
            }
            categoryAxis.dataFields.category = "Marketing";
            //categoryAxis.title.text = "Performances";
            
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 20;
            categoryAxis.renderer.labels.template.fontSize = 11;

            if (window.screen.width > 720) {
                categoryAxis.renderer.labels.template.rotation = -70;
                categoryAxis.renderer.labels.template.horizontalCenter = "center";
                categoryAxis.renderer.labels.template.location = 0.1;
            }

            categoryAxis.renderer.cellStartLocation = 0.1;
            categoryAxis.renderer.cellEndLocation = 0.9;

            if (window.screen.width > 720) {
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            }
            else {
                var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
            }
            //valueAxis.renderer.minWidth = 50;
            //valueAxis.renderer.opposite = true;
            valueAxis.min = 0;

            var dt = new Date();
            valueAxis.title.text = "Units - " + dt.getFullYear();
            valueAxis.renderer.labels.template.fontSize = 9;

            function createSeries(field, name, stacked) {
                var series = chart.series.push(new am4charts.ColumnSeries());
                if (window.screen.width > 720) {
                    series.dataFields.valueY = field;
                    series.dataFields.categoryX = "Marketing";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueY}[/]";
                }
                else {
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "Marketing";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                }
                series.columns.template.width = am4core.percent(85);
                series.columns.template.column.cornerRadiusTopLeft = 3;
                series.columns.template.column.cornerRadiusTopRight = 3;
                series.columns.template.column.fillOpacity = 0.8;

                var seriesLabel = series.bullets.push(new am4charts.LabelBullet());
                if (window.screen.width > 720) {
                    seriesLabel.label.text = "{valueY}";
                    seriesLabel.verticalCenter = "center";
                    seriesLabel.dy = -8;          
                }
                else {
                    seriesLabel.label.text = "{valueX}";
                    seriesLabel.label.horizontalCenter = "center";
                    seriesLabel.label.dx = 8;
                }
                seriesLabel.label.hideOversized = false;
                seriesLabel.label.truncate = false;
                seriesLabel.label.fontSize = 10;
            }

            function createLine(field, name, stacked) {
                var line = chart.series.push(new am4charts.LineSeries())
                if (window.screen.width > 720) {
                    line.dataFields.valueY = field;
                    line.dataFields.categoryX = "Marketing";
                }
                else {
                    line.dataFields.valueX = field;
                    line.dataFields.categoryY = "Marketing";
                }
                line.name = name;
                line.bullets.push(new am4charts.CircleBullet());    
                line.strokeWidth = 2;
                line.stroke = new am4core.InterfaceColorSet().getFor("alternativeBackground");
                line.strokeOpacity = 0.5;

                var lineLabel = line.bullets.push(new am4charts.LabelBullet());
                if (window.screen.width > 720) {
                    lineLabel.label.text = "{valueY}";
                    lineLabel.verticalCenter = "center";
                    lineLabel.dy = -12;
                }
                else {
                    lineLabel.label.text = "{valueX}";
                    lineLabel.label.horizontalCenter = "center";
                    lineLabel.label.dx = 10;
                }
                lineLabel.label.hideOversized = false;
                lineLabel.label.truncate = false;
                lineLabel.label.fontSize = 10;
            }

            createLine("Target", "Target", false);
            createSeries("PO", "PO", false);
            createSeries("Realisasi", "Realisasi", false);
            chart.legend = new am4charts.Legend();
            chart.legend.fontSize = 12;

        }); 


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
                document.getElementById("chartdiv").style.height = '400px';
            } else {
                document.getElementById("chartdiv").style.height = '850px';
            }
            console.log(document.getElementById("chartdiv"));
            //$('#modal-messagebox').on('hidden.bs.modal', function () {
            //    document.body.style.paddingRight = '0px';
            //});

            //var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            //if (isExists != '') {
            //    //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
            //    $('#modal-messagebox').modal('show');
            //}

            $(document).ready(function () {
                $(".box-job-order").hide();
                $(".box-master").hide();
                $(".box-reference").hide();
                $(".box-mutation").hide();
                $(".box-setup").hide();
                $(".box-installation").hide();
                $(".box-invoice").hide();
                $(".box-view").hide();
                $(".box-user-access").hide();


                $('#MNUJOB').click(function () {
                    $(".box-job-order").show();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUMST').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").show();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUREF').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").show();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUMUT').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").show();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUSET').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").show();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUINSTALL').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").show();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUINV').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").show();
                    $(".box-view").hide();
                    $(".box-user-access").hide();
                });

                $('#MNUVIEW').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").show();
                    $(".box-user-access").hide();
                });

                $('#MNUCONF').click(function () {
                    $(".box-job-order").hide();
                    $(".box-master").hide();
                    $(".box-reference").hide();
                    $(".box-mutation").hide();
                    $(".box-setup").hide();
                    $(".box-installation").hide();
                    $(".box-invoice").hide();
                    $(".box-view").hide();
                    $(".box-user-access").show();
                });

            });

        }
        endRequest();
    </script>
</asp:Content>
