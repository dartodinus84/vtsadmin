<%@ Page Title=".:: EasyGo ::. VTS Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_monitoring_gsm.aspx.cs" Inherits="vtsadm.dashboard_monitoring_gsm" EnableEventValidation="false" %>

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
        <small>Monitoring gsm</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="vehicle_position.aspx">Monitoring gsm</a></li>
        </ol>
    </section>


    <section class="content">
       
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Search Information</h3>
                    </div>

                    <div class="box-body">
                        <div class="form-group form-group-sm col-md-6">
                            <label>Filter Type</label>
                            <asp:DropDownList ID="CmbFilterType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm col-md-6">
                            <label>Group Area Name</label>
                            <asp:DropDownList ID="CmbGroupAreaName" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>


                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClientClick="showOverlay();" OnClick="CmdSearch_Click" Text="Search"/>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-red-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi" runat="server">0</label>
                        </h3>
                        <p>Expired</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=0" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-green-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasiTotal" runat="server">0</label>
                        </h3>
                        <p>Total</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=total" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_1" runat="server">0</label>
                        </h3>
                        <p>Day 1</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=1" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_2" runat="server">0</label>
                        </h3>
                        <p>Day 2</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=2" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_3" runat="server">0</label>
                        </h3>
                        <p>Day 3</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=3" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_4" runat="server">0</label>
                        </h3>
                        <p>Day 4</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=4" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_5" runat="server">0</label>
                        </h3>
                        <p>Day 5</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=5" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-maroon-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_6" runat="server">0</label>
                        </h3>
                        <p>Day 6</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=6" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_7" runat="server">0</label>
                        </h3>
                        <p>Day 7</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=7" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_8" runat="server">0</label>
                        </h3>
                        <p>Day 8</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=8" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_9" runat="server">0</label>
                        </h3>
                        <p>Day 9</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=9" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_10" runat="server">0</label>
                        </h3>
                        <p>Day 10</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=10" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_11" runat="server">0</label>
                        </h3>
                        <p>Day 11</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=11" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-yellow-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_12" runat="server">0</label>
                        </h3>
                        <p>Day 12</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=12" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_13" runat="server">0</label>
                        </h3>
                        <p>Day 13</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=13" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_14" runat="server">0</label>
                        </h3>
                        <p>Day 14</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=14" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_15" runat="server">0</label>
                        </h3>
                        <p>Day 15</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=15" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_16" runat="server">0</label>
                        </h3>
                        <p>Day 16</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=16" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_17" runat="server">0</label>
                        </h3>
                        <p>Day 17</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=17" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_18" runat="server">0</label>
                        </h3>
                        <p>Day 18</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=18" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_19" runat="server">0</label>
                        </h3>
                        <p>Day 19</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=19" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_20" runat="server">0</label>
                        </h3>
                        <p>Day 20</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=20" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_21" runat="server">0</label>
                        </h3>
                        <p>Day 21</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=21" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_22" runat="server">0</label>
                        </h3>
                        <p>Day 22</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=22" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_23" runat="server">0</label>
                        </h3>
                        <p>Day 23</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=23" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-purple-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_24" runat="server">0</label>
                        </h3>
                        <p>Day 24</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=24" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_25" runat="server">0</label>
                        </h3>
                        <p>Day 25</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=25" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_26" runat="server">0</label>
                        </h3>
                        <p>Day 26</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=26" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_27" runat="server">0</label>
                        </h3>
                        <p>Day 27</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=27" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_28" runat="server">0</label>
                        </h3>
                        <p>Day 28</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=28" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_29" runat="server">0</label>
                        </h3>
                        <p>Day 29</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=29" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-2 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="lblCntDurasi_30" runat="server">0</label>
                        </h3>
                        <p>Day 30</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <a href="dashboard_monitoring_gsm_detail?type=30" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
       

        function endRequest(sender, args) {
            $(document).ready(function () {
            });
        }
        endRequest();
    </script>
</asp:Content>
