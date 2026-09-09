<%@ Page Title=".:: EasyGo ::. Device Stock Monitoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_device_stock_monitoring.aspx.cs" Inherits="vtsadm.dashboard_device_stock_monitoring" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .summary-card {
            border-radius: 8px;
            color: #fff;
            min-height: 120px;
            padding: 18px;
            margin-bottom: 15px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
        }

        .summary-card h3 {
            margin: 0 0 5px 0;
            font-size: 30px;
            font-weight: 700;
        }

        .summary-card p {
            margin: 0;
            font-size: 14px;
            opacity: 0.95;
        }

        .summary-card-blue {
            background: #3c8dbc;
        }

        .summary-card-green {
            background: #00a65a;
        }

        .summary-card-orange {
            background: #f39c12;
        }

        .status-pill {
            color: #fff;
            border-radius: 12px;
            padding: 4px 10px;
            font-weight: 600;
            display: inline-block;
            min-width: 80px;
            text-align: center;
            font-size: 12px;
        }

        .status-safe {
            background-color: #00a65a;
        }

        .status-low {
            background-color: #f39c12;
        }

        .status-critical {
            background-color: #dd4b39;
        }
    </style>

    <section class="content-header">
        <h1>Device Stock Monitoring Dashboard</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li class="active">Device Stock Monitoring</li>
        </ol>
    </section>

    <section class="content">
        <% if (!string.IsNullOrEmpty(ErrorMessage)) { %>
        <div class="row">
            <div class="col-md-12">
                <div class="alert alert-danger">
                    <strong>Failed to load stock monitoring data.</strong>
                    <br />
                    <%= ErrorMessage %>
                </div>
            </div>
        </div>
        <% } %>

        <div class="row">
            <div class="col-md-4">
                <div class="summary-card summary-card-blue">
                    <h3><%= TotalDeviceTypes.ToString("N0") %></h3>
                    <p>Total Device Types</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="summary-card summary-card-green">
                    <h3><%= TotalAvailableDevices.ToString("N0") %></h3>
                    <p>Total Available Devices</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="summary-card summary-card-orange">
                    <h3><%= TotalDeviceTypesBelowMinimum.ToString("N0") %></h3>
                    <p>Total Device Types Below Minimum</p>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-8">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Stock Monitoring Table</h3>
                    </div>
                    <div class="box-body table-responsive">
                        <table class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th>Device Type</th>
                                    <th class="text-right">Available Stock</th>
                                    <th class="text-right">Minimum Stock</th>
                                    <th>Stock Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <%= StockTableRowsHtml %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Low Stock Alert (LOW / CRITICAL)</h3>
                    </div>
                    <div class="box-body table-responsive">
                        <table class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>Device Type</th>
                                    <th class="text-right">Available</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <%= LowStockRowsHtml %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
