<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_qc_report.aspx.cs" Inherits="vtsadm.device_qc_report" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device QC Report
            <small>Quality Control Reporting</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Quality Control</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">QC Report</li>
        </ol>
    </section>

    <section class="content">
        <!-- FILTER SECTION -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-filter"></i> Filter Report
                        </h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Start Date <span class="text-red">*</span></label>
                                    <asp:TextBox ID="txtStartDate" runat="server" 
                                        CssClass="form-control" 
                                        TextMode="Date" 
                                        required="required"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>End Date <span class="text-red">*</span></label>
                                    <asp:TextBox ID="txtEndDate" runat="server" 
                                        CssClass="form-control" 
                                        TextMode="Date" 
                                        required="required"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Device Type</label>
                                    <asp:DropDownList ID="ddlDeviceType" runat="server" 
                                        CssClass="form-control">
                                        <asp:ListItem Value="">[All Device Types]</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3" id="divUserQC" runat="server">
                                <div class="form-group">
                                    <label>User QC</label>
                                    <asp:DropDownList ID="ddlUserQC" runat="server" 
                                        CssClass="form-control">
                                        <asp:ListItem Value="">[All Users]</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" 
                                    CssClass="btn btn-primary btn-modern" 
                                    Text="Tampilkan Data" 
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" 
                                    CssClass="btn btn-default" 
                                    Text="Clear" 
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- LOADING INDICATOR -->
        <asp:UpdatePanel ID="UpdatePanelLoader" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlLoader" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="alert alert-info text-center">
                                <i class="fa fa-spinner fa-spin"></i> Loading data, please wait...
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- REPORT SECTION - MANAGER -->
        <asp:Panel ID="pnlManagerReport" runat="server" Visible="false">
            <!-- SUMMARY REPORT -->
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-list-alt"></i> Summary Report - Total per Device Type
                            </h3>
                        </div>
                        <div class="box-body no-padding">
                            <asp:UpdatePanel ID="UpdatePanelSummary" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvSummary" runat="server" 
                                        CssClass="table table-hover table-striped table-bordered" 
                                        AutoGenerateColumns="True" 
                                        AllowPaging="False"
                                        EmptyDataText="No data available"
                                        GridLines="None"
                                        Width="100%">
                                        <RowStyle CssClass="table-row" />
                                        <HeaderStyle BackColor="#3c8dbc" ForeColor="White" Font-Bold="true" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" />
                                    </asp:GridView>
                                    <div class="box-footer">
                                        <asp:Label ID="LblPagingSummary" runat="server" CssClass="text-muted" Style="font-style: italic; font-size: 12px;"></asp:Label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <!-- USER REPORT -->
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-users"></i> Per User Report - Device Type per User QC
                            </h3>
                        </div>
                        <div class="box-body no-padding">
                            <asp:UpdatePanel ID="UpdatePanelUser" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvUser" runat="server" 
                                        CssClass="table table-hover table-striped table-bordered" 
                                        AutoGenerateColumns="True" 
                                        AllowPaging="True"
                                        PageSize="20"
                                        OnPageIndexChanging="gvUser_PageIndexChanging"
                                        OnRowCommand="gvUser_RowCommand"
                                        OnRowDataBound="gvUser_RowDataBound"
                                        EmptyDataText="No data available"
                                        GridLines="None"
                                        Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnViewDetail" runat="server" 
                                                        Text="View Detail" 
                                                        CssClass="btn btn-sm btn-primary" 
                                                        CommandName="ViewDetail" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                        <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <RowStyle CssClass="table-row" />
                                        <HeaderStyle BackColor="#00c0ef" ForeColor="White" Font-Bold="true" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" />
                                    </asp:GridView>
                                    <div class="box-footer">
                                        <asp:Label ID="LblPagingUser" runat="server" CssClass="text-muted" Style="font-style: italic; font-size: 12px;"></asp:Label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- REPORT SECTION - USER QC -->
        <asp:Panel ID="pnlUserQCReport" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-warning">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-user"></i> My QC Report
                            </h3>
                        </div>
                        <div class="box-body no-padding">
                            <asp:UpdatePanel ID="UpdatePanelUserQC" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvUserQC" runat="server" 
                                        CssClass="table table-hover table-striped table-bordered" 
                                        AutoGenerateColumns="False" 
                                        AllowPaging="True"
                                        PageSize="20"
                                        OnPageIndexChanging="gvUserQC_PageIndexChanging"
                                        EmptyDataText="No data available"
                                        GridLines="None"
                                        Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" />
                                            <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Description" />
                                            <asp:BoundField DataField="TotalQTY" HeaderText="Total QTY" 
                                                ItemStyle-HorizontalAlign="Right" 
                                                DataFormatString="{0:N0}" />
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                        <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <RowStyle CssClass="table-row" />
                                        <HeaderStyle BackColor="#f39c12" ForeColor="White" Font-Bold="true" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <!-- DETAIL REPORT FOR USER QC -->
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-list"></i> Detail QC Data
                            </h3>
                        </div>
                        <div class="box-body no-padding">
                            <asp:UpdatePanel ID="UpdatePanelUserQCDetail" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvUserQCDetail" runat="server" 
                                        CssClass="table table-hover table-striped table-bordered" 
                                        AutoGenerateColumns="False" 
                                        AllowPaging="True"
                                        PageSize="20"
                                        OnPageIndexChanging="gvUserQCDetail_PageIndexChanging"
                                        EmptyDataText="No data available"
                                        GridLines="None"
                                        Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="LogID" HeaderText="Log ID" />
                                            <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                            <asp:BoundField DataField="NoSN" HeaderText="Serial Number" />
                                            <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" />
                                            <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type" />
                                            <asp:BoundField DataField="QcBy" HeaderText="QC By" />
                                            <asp:BoundField DataField="QcDate" HeaderText="QC Date" 
                                                DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                        <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <RowStyle CssClass="table-row" />
                                        <HeaderStyle BackColor="#28a745" ForeColor="White" Font-Bold="true" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- YEARLY REPORT TAB -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-default">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-calendar"></i> Yearly Report
                        </h3>
                    </div>
                    <div class="box-body no-padding">
                        <asp:UpdatePanel ID="UpdatePanelYearly" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvYearly" runat="server" 
                                    CssClass="table table-hover table-striped table-bordered" 
                                    AutoGenerateColumns="True" 
                                    AllowPaging="True"
                                    PageSize="20"
                                    OnPageIndexChanging="gvYearly_PageIndexChanging"
                                    EmptyDataText="No data available"
                                    GridLines="None"
                                    Width="100%">
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                    <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <RowStyle CssClass="table-row" />
                                    <HeaderStyle BackColor="#6c757d" ForeColor="White" Font-Bold="true" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL DETAIL -->
        <div class="modal fade modal-detail" id="modalDetail" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">
                            <i class="fa fa-list"></i> Detail QC Data
                        </h4>
                    </div>
                    <div class="modal-body modal-detail-body">
                        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvDetail" runat="server" 
                                    CssClass="table table-hover table-striped table-bordered" 
                                    AutoGenerateColumns="False" 
                                    AllowPaging="True"
                                    PageSize="10"
                                    OnPageIndexChanging="gvDetail_PageIndexChanging"
                                    EmptyDataText="No data available"
                                    GridLines="None"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                        <asp:BoundField DataField="NoSN" HeaderText="Serial Number" />
                                        <asp:BoundField DataField="DeviceType" HeaderText="Device Type" />
                                        <asp:BoundField DataField="QcBy" HeaderText="QC By" />
                                        <asp:BoundField DataField="QcDate" HeaderText="QC Date" 
                                            DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                                    <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <RowStyle CssClass="table-row" />
                                    <HeaderStyle BackColor="#007bff" ForeColor="White" Font-Bold="true" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MESSAGE BOX -->
        <div class="modal fade" id="modalMessageBox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Info</h4>
                    </div>
                    <div class="modal-body">
                        <div id="divMessage" runat="server"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <style>
        /* Perlebar modal Detail QC Data dan aktifkan scroll di dalam body */
        .modal-detail .modal-dialog {
            width: 95%;
            max-width: 1300px;
        }

        .modal-detail-body {
            max-height: 70vh;
            overflow-y: auto;
            overflow-x: auto;
        }

        .btn-modern {
            border-radius: 0 !important;
            padding: 8px 20px;
            font-weight: 600;
        }

        .box {
            border-radius: 0 !important;
        }

        .box-header {
            border-radius: 0 !important;
        }

        .table-bordered,
        .table-bordered > thead > tr > th,
        .table-bordered > tbody > tr > td {
            border-radius: 0 !important;
        }

        .pagination-ys {
            padding: 0;
        }

        .pagination-ys table > tbody > tr > td {
            display: inline;
        }

        .pagination-ys table > tbody > tr > td > a,
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }

        .pagination-ys table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .pagination-ys table > tbody > tr > td > a:hover,
        .pagination-ys table > tbody > tr > td > span:hover,
        .pagination-ys table > tbody > tr > td > a:focus,
        .pagination-ys table > tbody > tr > td > span:focus {
            color: #23527c;
            background-color: #eee;
            border-color: #ddd;
        }
    </style>

    <script type="text/javascript">
        function initModalDetail() {
            // Pastikan event tidak terdaftar dua kali
            $('#modalDetail').off('shown.bs.modal hidden.bs.modal');

            $('#modalDetail').on('shown.bs.modal', function () {
                // Tempat inisialisasi tambahan jika perlu
            });

            $('#modalDetail').on('hidden.bs.modal', function () {
                // Bersihkan backdrop & class agar layar tidak tetap gelap
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            });
        }

        // Inisialisasi awal
        $(function () {
            initModalDetail();
        });

        // Re‑init setiap ada partial postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            initModalDetail();
        });
    </script>
</asp:Content>

