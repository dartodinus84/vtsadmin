<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_jo.aspx.cs" Inherits="vtsadm.dashboard_jo" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .icon {
            top: 0px !important;
            font-size: 40px !important;
        }

        .inner {
            height: 80px !important;
            padding-top: 2px !important;
        }

        .small-box-footer {
            border-bottom-left-radius: 11px;
            border-bottom-right-radius: 11px;
        }

        .small-box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .box {
            border-radius: 11px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }

        .widget-user-2 .widget-user-header {
            border-top-left-radius: 11px;
            border-top-right-radius: 11px;
            height: 60px;
            padding-top: 10px;
        }

        .box-header {
            font-size: 16px;
        }

        li-format {
            height: 45px !important;
        }
    </style>

    <section class="content-header">
        <h1>Dashboard
        <small>Job Order</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Job Order</a></li>
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
                        <div class="form-group form-group-sm">
                            <label>Filter Type</label>
                            <asp:DropDownList ID="CmbFilterType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                         <div class="form-group form-group-sm">
                             <label>Group Area Name</label>
                             <asp:DropDownList ID="CmbGroupAreaName" runat="server" CssClass="form-control"></asp:DropDownList>
                         </div>

                        <div class="form-group form-group-sm">
                            <label>Date From</label>
                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Input date from ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date To</label>
                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Input date to ..."></asp:TextBox>
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
            <div class="col-lg-6 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntJONew" runat="server">0</label>
                        </h3>
                        <p>JO - New Installation</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-truck"></i>
                    </div>
                    <a href="#" class="small-box-footer"> &nbsp;</a>
                </div>
            </div>
             <div class="col-lg-6 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <h3>
                            <label id="LblCntJOMaint" runat="server">0</label>
                        </h3>
                        <p>JO - Maintenance</p>
                    </div>
                    <div class="icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                    <a href="#" class="small-box-footer"> &nbsp;</a>
                </div>
            </div>
            
        </div>
        <div class="row">

            <div class="col-lg-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-truck"></i>
                        Job Order - New Installation
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="view_job_create_open.aspx">Open <span id="LblCntJoNewOpen" runat="server" class="pull-right badge bg-aqua">0</span></a></li>
                            <li style="height: 46px;"><a href="view_job_create_close.aspx">Close<span id="LblCntJoNewClose" runat="server" class="pull-right badge bg-green">0</span></a></li>
                            <li style="height: 46px;"><a href="#">%<span id="LblCntJoNewSla" runat="server" class="pull-right badge bg-red">0</span></a></li>
                        </ul>

                    </div>
                </div>
            </div>

            <div class="col-lg-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-truck"></i>
                        Unit - New Installation
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="#">Open <span id="LblCntJoUnitOpen" runat="server" class="pull-right badge bg-aqua">0</span></a></li>
                            <li style="height: 46px;"><a href="#">Close<span id="LblCntJoUnitClose" runat="server" class="pull-right badge bg-green">0</span></a></li>
                            <li style="height: 46px;"><a href="#">SLA<span id="LblCntJoUnitSla" runat="server" class="pull-right badge bg-red">0</span></a></li>
                            
                        </ul>

                    </div>
                </div>
            </div>

            <div class="col-lg-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Job Order - Maintenance
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked">
                            <li style="height: 46px;"><a href="view_job_maint_open.aspx">Open <span id="LblCntJoMaintOpen" runat="server" class="pull-right badge bg-aqua">0</span></a></li>
                            <li style="height: 46px;"><a href="view_job_maint_close.aspx">Close<span id="LblCntJoMaintClose" runat="server" class="pull-right badge bg-green">0</span></a></li>
                            <li style="height: 46px;"><a href="#">SLA<span id="LblCntJoMaintSla" runat="server" class="pull-right badge bg-red">0</span></a></li>
                        </ul>
                    </div>
                </div>

            </div>

        </div>

        <!-- Loading Screen -->
        <div id="overlay">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>
    </section>
 
    <script type="text/javascript">

        $(document).ready(function () {
           
            var LblCntJoNewOpen = $('#<%=LblCntJoNewOpen.ClientID%>').text().replace(/[. ,](\d\d\d\D|\d\d\d$)/g, '$1');
            var LblCntJoNewClose = $('#<%=LblCntJoNewClose.ClientID%>').text().replace(/[. ,](\d\d\d\D|\d\d\d$)/g, '$1');

            var LblCntJoNewTotal = parseInt(LblCntJoNewOpen) + parseInt(LblCntJoNewClose);
            var LblCntJoNewSla = (parseInt(LblCntJoNewClose) / parseInt(LblCntJoNewTotal)) * 100;
            $('#LblCntJoNewSla').html(Math.ceil(LblCntJoNewSla));

        });
    </script>
</asp:Content>
