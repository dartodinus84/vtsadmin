<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_stock_opname_device.aspx.cs" Inherits="vtsadm.rpt_stock_opname_device" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section class="content-header">
        <h1>Laporan Stok Opname Device 
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Stock Opname</a></li>
            <li class="active">Device</li>
        </ol>
    </section>

    <section class="content stock-opname-page">
        <div id="pageLoader" class="page-loader" style="display: none;">
            <div class="page-loader-card">
                <div class="loader-ring"></div>
                <div class="loader-title">Memuat laporan</div>
                <div class="loader-subtitle">Mohon tunggu, data sedang diproses...</div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-solid modern-card report-filter-card">
                            <div class="box-header with-border modern-card-header">
                                <h3 class="box-title"><i class="fa fa-search"></i> Search Information</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body report-filter-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group form-group-sm modern-form-group">
                                            <label><i class="fa fa-filter"></i> Search By</label>
                                            <asp:TextBox ID="txtSearch" runat="server" class="form-control modern-input" placeholder="Search by vendor or device type ..."></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group form-group-sm modern-form-group">
                                            <label><i class="fa fa-calendar"></i> Date From</label>
                                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control modern-input" placeholder="Input date from ..."></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group form-group-sm modern-form-group">
                                            <label><i class="fa fa-calendar"></i> Date To</label>
                                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control modern-input" placeholder="Input date to ..."></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer modern-card-footer">
                                <asp:Button ID="CmdClear" CssClass="btn btn-default modern-btn modern-btn-light" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                                <asp:Button ID="CmdSearch" CssClass="btn btn-primary modern-btn modern-btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                                <small class="text-muted pull-right">
                                    <i class="fa fa-info-circle"></i> Use date filters to narrow down results
                                </small>
                            </div>
                        </div>
                        <div class="box box-solid modern-card report-table-card">
                            <div class="box-header with-border modern-card-header table-card-header">
                                <h3 class="box-title"><i class="fa fa-table"></i> Laporan Stok Opname Device</h3>
                                <div class="box-tools pull-right">
                                    <small class="text-muted">
                                        <i class="fa fa-mouse-pointer"></i> Klik pada angka untuk melihat detail device | 
                                        <i class="fa fa-arrows-h"></i> Scroll horizontal untuk melihat semua kolom
                                    </small>
                                </div>
                            </div>
                            <div class="box-body table-card-body" style="padding: 0;">
                                <div class="table-responsive-wrapper">
                                    <div class="table-scroll-container">
                                        <!-- Custom table with grouped headers -->
                                        <table class="stock-opname-table" id="stockOpnameTable">
                                            <thead class="table-header-fixed">
                                                <!-- First header row - Main groups -->
                                                <tr class="header-row-1">
                                                    <th rowspan="3" class="fixed-col col-no">NO</th>
                                                    <th rowspan="3" class="fixed-col col-supp">SUPP</th>
                                                    <th rowspan="3" class="fixed-col col-komponen">GPS KOMPONEN</th>
                                                    <th rowspan="3" class="col-stock-awal">STOCK AWAL</th>
                                                    <th colspan="3" class="group-gudang-all">Aktual Jumlah di Gudang (ALL)</th>
                                                    <th colspan="3" class="group-gudang-jakarta">Aktual Gudang Jakarta</th>
                                                    <th colspan="3" class="group-gudang-surabaya">Aktual Gudang Surabaya</th>
                                                    <th colspan="3" class="group-teknisi">Aktual Jumlah di Teknisi Teknis</th>
                                                    <th rowspan="3" class="col-stock-cust">STOCK CUST</th>
                                                    <th rowspan="3" class="col-total">Jumlah Aktual (Total)</th>
                                                </tr>
                                                <!-- Second header row - Sub groups -->
                                                <tr class="header-row-2">
                                                    <th colspan="3" class="subgroup-gudang-all">Barang di gudang (ALL)</th>
                                                    <th colspan="3" class="subgroup-gudang-jakarta">Barang di gudang Jakarta</th>
                                                    <th colspan="3" class="subgroup-gudang-surabaya">Barang di gudang Surabaya</th>
                                                    <th colspan="3" class="subgroup-teknisi">WEST & EAST AREA</th>
                                                </tr>
                                                <!-- Third header row - Individual columns -->
                                                <tr class="header-row-3">
                                                    <th class="col-gudang-all-baru">Baru</th>
                                                    <th class="col-gudang-all-second">Second</th>
                                                    <th class="col-gudang-all-disposal">Disposal</th>
                                                    <th class="col-jakarta-baru">Baru</th>
                                                    <th class="col-jakarta-second">Second</th>
                                                    <th class="col-jakarta-disposal">Disposal</th>
                                                    <th class="col-surabaya-baru">Baru</th>
                                                    <th class="col-surabaya-second">Second</th>
                                                    <th class="col-surabaya-disposal">Disposal</th>
                                                    <th class="col-teknisi-all">WEST & EAST AREA</th>
                                                    <th class="col-teknisi-west">WEST AREA</th>
                                                    <th class="col-teknisi-east">EAST AREA</th>
                                                </tr>
                                            </thead>
                                            <tbody id="reportTableBody" runat="server" class="table-body">
                                                <!-- Data will be populated here -->
                                                <tr class="loading-row">
                                                    <td colspan="18" class="table-loading">
                                                        <i class="fa fa-spinner fa-spin"></i>
                                                        Memuat data laporan...
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    
                                    <!-- Pagination info -->
                                    <div class="pagination-info">
                                        <asp:Label ID="LblPaging" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer modern-card-footer">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Button ID="CmdExport" CssClass="btn btn-success modern-btn modern-btn-success" runat="server" OnClick="CmdExport_Click" Text="📄 Export CSV" />
                                        <asp:Button ID="CmdExportXls" CssClass="btn btn-success modern-btn modern-btn-success" runat="server" OnClick="CmdExportXls_Click" Text="📊 Export XLS" />
                                    </div>
                                    <div class="col-md-6">
                                        <small class="text-muted pull-right">
                                            <i class="fa fa-info-circle"></i> 
                                            Tip: Gunakan Shift + Mouse Wheel untuk scroll horizontal cepat
                                        </small>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="CmdExport" />
                <asp:PostBackTrigger ControlID="CmdExportXls" />
                <asp:PostBackTrigger ControlID="CmdExportDetails" />
                <asp:PostBackTrigger ControlID="CmdExportDetailsXls" />
            </Triggers>
        </asp:UpdatePanel>

        <!-- Modal for Device Details - ENHANCED WITH ULTIMATE PROTECTION -->
        <div class="modal fade bs-example-modal-lg" id="modal-devicedetails" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" style="width: 95%; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close manual-close-btn" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">
                            <i class="fa fa-list-alt"></i> 
                            Detail Device - <span id="modal-title-category"></span>
                        </h4>
                        <p class="modal-subtitle">
                            <strong><i class="fa fa-building"></i> Vendor:</strong> <span id="modal-title-vendor"></span> 
                            | <strong><i class="fa fa-microchip"></i> Device Type:</strong> <span id="modal-title-devicetype"></span>
                        </p>
                        
                        <!-- DEBUG INFO (will be hidden in production) -->
                        <div id="modal-debug-info" style="display: none; font-size: 10px; color: #ccc; margin-top: 5px;">
                            Status: <span id="debug-modal-status">-</span> | 
                            Protection: <span id="debug-protection-status">-</span> | 
                            Close Attempts: <span id="debug-close-attempts">0</span>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-loading" id="modalLoading" style="display:none; text-align:center; padding:20px;">
                                        <i class="fa fa-spinner fa-spin fa-2x text-primary"></i>
                                        <br/>
                                        <span class="text-muted">Memuat detail device...</span>
                                    </div>
                                    <asp:Panel runat="server" ScrollBars="Auto" Height="400px" ID="panelDetailGrid">
                                        <asp:GridView ID="GridViewDetail" runat="server" BackColor="WhiteSmoke" Font-Size="Small" CssClass="table table-bordered table-hover table-striped" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No devices found for this category" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="15" OnPageIndexChanging="GridViewDetail_PageIndexChanging">
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <Columns>
                                                <asp:BoundField DataField="deviceid" HeaderText="Device ID" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="120px" CssClass="bg-primary text-white" />
                                                    <ItemStyle CssClass="text-monospace" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nosn" HeaderText="Serial Number" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="150px" CssClass="bg-primary text-white" />
                                                    <ItemStyle CssClass="text-monospace font-weight-bold" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="vendor" HeaderText="Vendor" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="100px" CssClass="bg-primary text-white" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="gps_type" HeaderText="GPS Type" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="120px" CssClass="bg-primary text-white" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="status_device" HeaderText="Status" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="80px" CssClass="bg-primary text-white" />
                                                    <ItemStyle CssClass="text-center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="warehouse_name" HeaderText="Warehouse" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="100px" CssClass="bg-primary text-white" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="technician_name" HeaderText="Technician" ItemStyle-Wrap="false">
                                                    <HeaderStyle Width="100px" CssClass="bg-primary text-white" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle ForeColor="#003481" BackColor="White" />
                                            <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                            <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                            <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                            <HeaderStyle Height="25px" Wrap="True" />
                                            <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                        </asp:GridView>
                                        <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                            <asp:Label ID="LblPagingDetail" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button ID="CmdExportDetails" CssClass="btn btn-success btn-sm" runat="server" OnClick="CmdExportDetails_Click" Text="📄 Export CSV" />                        
                                <asp:Button ID="CmdExportDetailsXls" CssClass="btn btn-success btn-sm" runat="server" OnClick="CmdExportDetailsXls_Click" Text="📊 Export XLS" />
                            </div>
                            <div class="col-md-6 text-right">
                                <button type="button" class="btn btn-default manual-close-btn">
                                    <i class="fa fa-times"></i> Close
                                </button>
                                <!-- DEBUG BUTTON (will be hidden in production) -->
                                <button type="button" class="btn btn-info btn-sm" onclick="debugModal()" style="display: none;" id="btn-debug">
                                    Debug
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Message modal -->
        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">
                            <i class="fa fa-info-circle"></i> Info Box
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">
                            <i class="fa fa-times"></i> Close
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden fields for modal parameters -->
        <asp:HiddenField ID="hdnModalCategory" runat="server" />
        <asp:HiddenField ID="hdnModalVendorId" runat="server" />
        <asp:HiddenField ID="hdnModalDeviceTypeId" runat="server" />
        <asp:HiddenField ID="hdnModalVendorName" runat="server" />
        <asp:HiddenField ID="hdnModalDeviceTypeName" runat="server" />

        <!-- Emergency cleanup button -->
        <button type="button" id="emergencyCleanupBtn" class="btn btn-danger" 
                style="position: fixed; top: 10px; right: 10px; z-index: 99999; display: none;">
            <i class="fa fa-exclamation-triangle"></i> Emergency Cleanup
        </button>

        <!-- Modal Protection Status Indicator -->
        <div id="modal-protection-indicator" style="position: fixed; top: 10px; left: 10px; z-index: 99999; display: none; background: rgba(0,0,0,0.8); color: white; padding: 5px 10px; border-radius: 15px; font-size: 11px;">
            🛡️ Modal Protection: <span id="protection-status">OFF</span>
        </div>
    </section>

    <style type="text/css">
        .stock-opname-page {
            background: linear-gradient(135deg, #f4f7fb 0%, #eef3f8 100%);
            padding-top: 12px;
        }

        .modern-card {
            border: 0 !important;
            border-radius: 16px !important;
            overflow: hidden;
            box-shadow: 0 14px 35px rgba(15, 23, 42, 0.08);
            background: #fff;
        }

        .modern-card-header {
            min-height: 58px;
            padding: 18px 20px !important;
            background: linear-gradient(135deg, #ffffff 0%, #f8fbff 100%) !important;
            border-bottom: 1px solid #e8eef6 !important;
        }

        .modern-card-header .box-title {
            font-size: 16px;
            font-weight: 700;
            color: #0f172a;
            letter-spacing: 0.2px;
        }

        .modern-card-header .box-title i {
            color: #2563eb;
            margin-right: 8px;
        }

        .report-filter-card {
            margin-bottom: 18px;
        }

        .report-filter-body {
            padding: 20px 20px 8px !important;
        }

        .modern-form-group label {
            color: #475569;
            font-weight: 600;
            margin-bottom: 8px;
        }

        .modern-input {
            height: 40px !important;
            border: 1px solid #dbe4ef !important;
            border-radius: 10px !important;
            box-shadow: none !important;
            color: #0f172a;
            transition: border-color 0.2s ease, box-shadow 0.2s ease;
        }

        .modern-input:focus {
            border-color: #2563eb !important;
            box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12) !important;
        }

        .modern-card-footer {
            padding: 14px 20px !important;
            background: #fbfdff !important;
            border-top: 1px solid #e8eef6 !important;
        }

        .modern-btn {
            min-height: 38px;
            padding: 8px 16px !important;
            border: 0 !important;
            border-radius: 10px !important;
            font-weight: 700;
            letter-spacing: 0.1px;
            box-shadow: 0 8px 18px rgba(15, 23, 42, 0.10);
            transition: transform 0.15s ease, box-shadow 0.15s ease, opacity 0.15s ease;
        }

        .modern-btn:hover {
            transform: translateY(-1px);
            box-shadow: 0 12px 24px rgba(15, 23, 42, 0.14);
        }

        .modern-btn-light {
            background: #eef2f7 !important;
            color: #334155 !important;
        }

        .modern-btn-primary {
            background: linear-gradient(135deg, #2563eb, #1d4ed8) !important;
            color: #fff !important;
        }

        .modern-btn-success {
            background: linear-gradient(135deg, #16a34a, #15803d) !important;
            color: #fff !important;
            margin-right: 8px;
        }

        .table-card-header small {
            color: #64748b;
        }

        .table-card-body {
            background: #ffffff;
        }

        .page-loader {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            inset: 0;
            z-index: 20000;
            display: flex;
            align-items: center;
            justify-content: center;
            background: rgba(15, 23, 42, 0.35);
            backdrop-filter: blur(5px);
        }

        .page-loader-card {
            min-width: 240px;
            padding: 28px 26px;
            text-align: center;
            border-radius: 18px;
            background: rgba(255, 255, 255, 0.96);
            box-shadow: 0 24px 60px rgba(15, 23, 42, 0.24);
        }

        .loader-ring {
            width: 48px;
            height: 48px;
            margin: 0 auto 14px;
            border: 4px solid #dbeafe;
            border-top-color: #2563eb;
            border-radius: 50%;
            animation: loaderSpin 0.8s linear infinite;
        }

        .loader-title {
            color: #0f172a;
            font-size: 15px;
            font-weight: 800;
            margin-bottom: 4px;
        }

        .loader-subtitle {
            color: #64748b;
            font-size: 12px;
        }

        @keyframes loaderSpin {
            to { transform: rotate(360deg); }
        }

        /* ===== TABLE WRAPPER AND SCROLL CONTAINER ===== */
        .table-responsive-wrapper {
            background: white;
            border-radius: 0 0 16px 16px;
            box-shadow: none;
            width: 100%;
            overflow: hidden;
        }
        
        .table-scroll-container {
            overflow-x: auto;
            overflow-y: auto;
            max-width: 100%;
            max-height: 600px;
            border: 0;
            border-radius: 0;
            position: relative;
            scrollbar-width: thin;
            scrollbar-color: #c1c1c1 #f1f1f1;
            scroll-behavior: smooth;
            -webkit-overflow-scrolling: touch;
        }
        
        /* ===== MAIN TABLE STYLING ===== */
        .stock-opname-table {
            width: 100%;
            min-width: 2000px;
            border-collapse: collapse;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 11px;
            margin: 0;
            background: white;
            position: relative;
            border: 1px solid #e8eef6;
        }
        
        /* ===== FIXED HEADER ===== */
        .table-header-fixed {
            position: sticky;
            top: 0;
            z-index: 10;
            background: white;
        }
        
        /* ===== HEADER ROW STYLING ===== */
        .header-row-1 th,
        .header-row-2 th,
        .header-row-3 th {
            position: sticky;
            top: 0;
            z-index: 5;
        }
        
        .header-row-1 th {
            background: linear-gradient(to bottom, #f8f9fa, #e9ecef) !important;
            border: 1px solid #dee2e6;
            padding: 8px 6px;
            text-align: center;
            font-weight: bold;
            font-size: 10px;
            vertical-align: middle;
            white-space: nowrap;
            color: #495057;
        }
        
        .header-row-2 th {
            background: linear-gradient(to bottom, #f1f3f4, #e9ecef) !important;
            border: 1px solid #dee2e6;
            padding: 6px 4px;
            text-align: center;
            font-weight: 600;
            font-size: 9px;
            vertical-align: middle;
            white-space: nowrap;
            color: #495057;
        }
        
        .header-row-3 th {
            background: linear-gradient(to bottom, #e9ecef, #dee2e6) !important;
            border: 1px solid #dee2e6;
            padding: 6px 4px;
            text-align: center;
            font-weight: 600;
            font-size: 9px;
            vertical-align: middle;
            white-space: nowrap;
            color: #495057;
        }
        
        /* ===== FIXED COLUMNS STYLING ===== */
        .fixed-col {
            position: sticky;
            left: 0;
            z-index: 15;
            background: linear-gradient(to bottom, #f8f9fa, #e9ecef) !important;
            box-shadow: 2px 0 5px rgba(0,0,0,0.1);
        }
        
        /* Fixed column positions */
        .col-no { 
            left: 0; 
            width: 50px; 
            z-index: 20;
        }
        .col-supp { 
            left: 50px; 
            width: 100px; 
            z-index: 19;
        }
        .col-komponen { 
            left: 150px; 
            width: 200px; 
            z-index: 18;
        }
        
        /* ===== COLUMN WIDTH SPECIFICATIONS ===== */
        .col-stock-awal, .col-registered { width: 100px; }
        .col-gudang-all-baru, .col-gudang-all-second, .col-gudang-all-disposal,
        .col-jakarta-baru, .col-jakarta-second, .col-jakarta-disposal,
        .col-surabaya-baru, .col-surabaya-second, .col-surabaya-disposal,
        .col-teknisi-all, .col-teknisi-west, .col-teknisi-east { 
            width: 80px; 
            min-width: 80px;
        }
        .col-stock-cust, .col-total { 
            width: 100px; 
            min-width: 100px;
        }
        
        /* ===== COLOR CODING FOR GROUPS ===== */
        .group-gudang-all, .subgroup-gudang-all,
        .col-gudang-all-baru, .col-gudang-all-second, .col-gudang-all-disposal,
        .group-gudang-jakarta, .subgroup-gudang-jakarta,
        .col-jakarta-baru, .col-jakarta-second, .col-jakarta-disposal {
            background: linear-gradient(to bottom, #fff9c4, #ffeb3b) !important;
            color: #333 !important;
        }
        
        .group-gudang-surabaya, .subgroup-gudang-surabaya,
        .col-surabaya-baru, .col-surabaya-second, .col-surabaya-disposal {
            background: linear-gradient(to bottom, #e3f2fd, #2196f3) !important;
            color: #333 !important;
        }
        
        .group-teknisi, .subgroup-teknisi,
        .col-teknisi-all, .col-teknisi-west, .col-teknisi-east {
            background: linear-gradient(to bottom, #fce4ec, #e91e63) !important;
            color: #333 !important;
        }
        
        .col-stock-cust {
            background: linear-gradient(to bottom, #f3e5f5, #9c27b0) !important;
            color: #333 !important;
        }
        
        .col-total {
            background: linear-gradient(to bottom, #e8f5e8, #4caf50) !important;
            color: #333 !important;
            font-weight: bold !important;
        }
        
        /* ===== DATA ROW STYLING - DIPERBAIKI ===== */
        .table-body tr {
            border-bottom: 1px solid #e0e0e0;
            min-height: 35px;
            transition: background-color 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            transform: translateY(0);
        }
        
        .table-body tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        
        .table-body tr:hover {
            background-color: #e3f2fd !important;
            transform: translateY(0) !important;
            box-shadow: 0 1px 4px rgba(0,0,0,0.1) !important;
        }
        
        .table-body td {
            border: 1px solid #e0e0e0;
            padding: 6px 4px;
            text-align: center;
            vertical-align: middle;
            font-size: 11px;
            white-space: nowrap;
            min-height: 30px;
            transition: background-color 0.2s ease-in-out;
        }
        
        /* ===== FIXED COLUMNS IN DATA ROWS ===== */
        .table-body .fixed-col {
            position: sticky;
            background: white !important;
            z-index: 14;
            box-shadow: 2px 0 5px rgba(0,0,0,0.1);
            transition: background-color 0.2s ease-in-out;
        }
        
        .table-body tr:nth-child(even) .fixed-col {
            background: #f8f9fa !important;
        }
        
        .table-body tr:hover .fixed-col {
            background: #e3f2fd !important;
        }
        
        .table-body .col-no { 
            left: 0; 
            font-weight: 600;
            color: #666;
            z-index: 17;
        }
        .table-body .col-supp { 
            left: 50px; 
            text-align: left;
            font-weight: 500;
            color: #333;
            z-index: 16;
        }
        .table-body .col-komponen { 
            left: 150px; 
            text-align: left;
            font-weight: 500;
            color: #333;
            z-index: 15;
        }
        
        /* ===== CLICKABLE CELL STYLING - DIPERBAIKI ===== */
        .clickable-cell {
            cursor: pointer;
            color: #1976d2 !important;
            text-decoration: underline;
            font-weight: 500;
            transition: all 0.2s ease-in-out;
            user-select: none;
            position: relative;
        }

        .clickable-cell:hover {
            background-color: #bbdefb !important;
            color: #0d47a1 !important;
            font-weight: bold;
            transform: scale(1.02) !important;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2) !important;
            text-decoration: none !important;
        }
        
        .clickable-cell:active {
            transform: scale(0.98) !important;
            transition: all 0.1s ease-in-out;
        }
        
        .clickable-cell:focus {
            outline: 2px solid #1976d2;
            outline-offset: -2px;
            background-color: #e3f2fd !important;
            transform: scale(1.01) !important;
            animation: none !important;
        }
        
        .clickable-cell::before {
            display: none !important;
        }
        
        /* ===== ZERO VALUE STYLING ===== */
        .zero-value {
            color: #bbb !important;
            font-style: italic;
            font-weight: normal;
            cursor: default;
            text-decoration: none;
            transform: scale(1) !important;
            transition: color 0.2s ease-in-out !important;
            min-width: 60px;
            text-align: center;
        }
        
        .zero-value:hover {
            color: #999 !important;
            background-color: #f5f5f5 !important;
            transform: scale(1) !important;
            box-shadow: none !important;
            animation: none !important;
        }
        
        .zero-value::before {
            display: none !important;
        }
        
        /* ===== TOTAL COLUMN HIGHLIGHTING ===== */
        .total-column {
            background: linear-gradient(to bottom, #e8f5e8, #c8e6c8) !important;
            font-weight: bold !important;
            color: #2e7d32 !important;
            border: 2px solid #4caf50 !important;
            transform: scale(1) !important;
            min-width: 100px;
        }
        
        .total-column.clickable-cell:hover {
            background: linear-gradient(to bottom, #a5d6a7, #66bb6a) !important;
            color: #1b5e20 !important;
            border-color: #2e7d32 !important;
            transform: scale(1.01) !important;
            animation: none !important;
        }
        
        /* ===== MODAL ENHANCEMENTS ===== */
        #modal-devicedetails {
            z-index: 1060; /* Higher than default */
        }
        
        #modal-devicedetails .modal-content {
            border: 3px solid #007bff; /* Visual indicator */
            box-shadow: 0 12px 40px rgba(0,0,0,0.4);
        }
        
        #modal-devicedetails .modal-header {
            background: linear-gradient(45deg, #007bff, #28a745);
            color: white;
            border-bottom: 2px solid rgba(255,255,255,0.2);
        }
        
        .manual-close-btn {
            background-color: rgba(255,255,255,0.2) !important;
            border: 1px solid rgba(255,255,255,0.3) !important;
            transition: all 0.2s ease !important;
        }
        
        .manual-close-btn:hover {
            background-color: rgba(255,255,255,0.3) !important;
            transform: scale(1.05) !important;
        }
        
        /* ===== PROTECTION INDICATOR ===== */
        #modal-protection-indicator {
            animation: pulse 2s infinite;
        }
        
        #modal-protection-indicator.active {
            background: rgba(40, 167, 69, 0.9) !important;
        }
        
        /* ===== DEBUG STYLES ===== */
        #modal-debug-info {
            font-family: 'Courier New', monospace;
            background: rgba(0,0,0,0.1);
            padding: 3px 6px;
            border-radius: 3px;
        }
        
        /* ===== RESPONSIVE AND OTHER STYLES ===== */
        .pagination-info {
            padding: 15px;
            background: #fbfdff;
            border-top: 1px solid #e8eef6;
            font-style: italic;
            color: #64748b;
            font-size: 12px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
        }
        
        .table-scroll-container::-webkit-scrollbar {
            width: 12px;
            height: 12px;
        }
        
        .table-scroll-container::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 6px;
        }
        
        .table-scroll-container::-webkit-scrollbar-thumb {
            background: linear-gradient(45deg, #007bff, #28a745);
            border-radius: 6px;
            border: 2px solid #f1f1f1;
        }
        
        .table-scroll-container::-webkit-scrollbar-thumb:hover {
            background: linear-gradient(45deg, #0056b3, #1e7e34);
        }
        
        .scroll-indicator {
            position: absolute;
            top: 10px;
            right: 20px;
            background: linear-gradient(45deg, #007bff, #28a745);
            color: white;
            padding: 8px 12px;
            border-radius: 20px;
            font-size: 11px;
            z-index: 25;
            opacity: 0;
            transition: all 0.3s ease;
            pointer-events: none;
            box-shadow: 0 4px 12px rgba(0,0,0,0.3);
            font-weight: 500;
        }
        
        .scroll-indicator.show {
            opacity: 0.9;
            transform: scale(1.05);
        }
        
        .table-loading {
            text-align: center;
            padding: 40px;
            color: #666;
            font-style: italic;
            position: relative;
            background: rgba(255,255,255,0.9);
        }
        
        .table-loading i {
            margin-right: 8px;
            color: #007bff;
            font-size: 18px;
        }
        
        .loading-row {
            background: #f8f9fa !important;
        }
        
        /* ===== EMERGENCY BUTTON ===== */
        #emergencyCleanupBtn {
            animation: pulse 2s infinite;
            font-weight: bold;
            border-radius: 50px;
        }
        
        @keyframes pulse {
            0% { transform: scale(1); }
            50% { transform: scale(1.1); }
            100% { transform: scale(1); }
        }
        
        /* ===== ANIMATIONS ===== */
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        @keyframes slideInFromRight {
            from { transform: translateX(50%); opacity: 0; }
            to { transform: translateX(0); opacity: 1; }
        }
        
        .table-body tr {
            animation: fadeIn 0.3s ease-in-out;
        }
        
        .scroll-indicator.show {
            animation: slideInFromRight 0.3s ease-out;
        }
        
        /* ===== ACCESSIBILITY ===== */
        .clickable-cell[tabindex="0"]:focus {
            outline: 3px solid #007bff;
            outline-offset: -3px;
            background-color: #e3f2fd !important;
            animation: none !important;
        }
        
        .table-scroll-container * {
            box-sizing: border-box;
        }
        
        .table-body {
            contain: layout style;
        }
        
        /* ===== RESPONSIVE ===== */
        @media (max-width: 1400px) {
            .stock-opname-table {
                font-size: 10px;
                min-width: 1800px;
            }
            
            .header-row-1 th, .header-row-2 th, .header-row-3 th {
                padding: 6px 3px;
                font-size: 9px;
            }
            
            .table-body td {
                padding: 5px 3px;
                font-size: 10px;
            }
            
            .col-supp { width: 90px; }
            .col-komponen { width: 180px; }
        }
        
        @media (max-width: 1200px) {
            .stock-opname-table {
                font-size: 9px;
                min-width: 1600px;
            }
            
            .header-row-1 th, .header-row-2 th, .header-row-3 th {
                padding: 4px 2px;
                font-size: 8px;
            }
            
            .table-body td {
                padding: 4px 2px;
                font-size: 9px;
            }
            
            .col-supp { width: 80px; }
            .col-komponen { width: 160px; }
            .col-gudang-all-baru, .col-gudang-all-second, .col-gudang-all-disposal,
            .col-jakarta-baru, .col-jakarta-second, .col-jakarta-disposal,
            .col-surabaya-baru, .col-surabaya-second, .col-surabaya-disposal,
            .col-teknisi-all, .col-teknisi-west, .col-teknisi-east { 
                width: 70px; 
                min-width: 70px;
            }
        }
        
        @media (max-width: 768px) {
            .table-scroll-container {
                max-height: 400px;
            }
            
            .scroll-indicator {
                font-size: 10px;
                padding: 6px 8px;
            }
        }
        
        /* ===== REDUCED MOTION ===== */
        @media (prefers-reduced-motion: reduce) {
            .clickable-cell,
            .table-body tr,
            .scroll-indicator,
            .total-column {
                animation: none !important;
                transition: none !important;
            }
            
            .clickable-cell:hover,
            .table-body tr:hover {
                transform: none !important;
            }
        }
        
        /* ===== HIGH CONTRAST ===== */
        @media (prefers-contrast: high) {
            .table-body tr:hover {
                background-color: #000 !important;
                color: #fff !important;
            }
            
            .clickable-cell {
                color: #0000ff !important;
            }
            
            .clickable-cell:hover {
                background-color: #ffff00 !important;
                color: #000 !important;
            }
        }
        
        /* ===== PRINT STYLES ===== */
        @media print {
            .table-scroll-container {
                overflow: visible !important;
                max-height: none !important;
            }
            
            .stock-opname-table {
                min-width: auto !important;
                font-size: 8px !important;
            }
            
            .scroll-indicator,
            .table-summary,
            #emergencyCleanupBtn,
            #modal-protection-indicator,
            .box-footer {
                display: none !important;
            }
            
            .clickable-cell {
                color: #000 !important;
                text-decoration: none !important;
            }
            
            .fixed-col {
                position: static !important;
                box-shadow: none !important;
            }
        }
    </style>

    <script type="text/javascript">
        // ===== PAGE REQUEST MANAGER =====
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // Add handlers for UpdatePanel
        prm.add_endRequest(endRequest);
        prm.add_beginRequest(beginRequest);

        // ===== BEGIN REQUEST HANDLER =====
        function beginRequest(sender, args) {
            // Show loading indicator
            showLoadingIndicator();
            showPageLoader();
        }

        // ===== END REQUEST HANDLER =====
        function endRequest(sender, args) {
            // Hide loading indicator
            hideLoadingIndicator();
            hidePageLoader();

            // Re-initialize page elements
            initializePageElements();

            // Check for messages
            checkAndShowMessages();
        }

        // ===== LOADING INDICATOR FUNCTIONS =====
        function showLoadingIndicator() {
            var loadingRow = document.querySelector('.loading-row');
            if (loadingRow) {
                loadingRow.style.display = 'table-row';
            }
        }

        function hideLoadingIndicator() {
            var loadingRow = document.querySelector('.loading-row');
            if (loadingRow) {
                loadingRow.style.display = 'none';
            }
        }

        function showPageLoader() {
            var loader = document.getElementById('pageLoader');
            if (loader) {
                loader.style.display = 'flex';
            }
        }

        function hidePageLoader() {
            var loader = document.getElementById('pageLoader');
            if (loader) {
                loader.style.display = 'none';
            }
        }

        // ===== MESSAGE HANDLING =====
        function checkAndShowMessages() {
            var divComment = document.getElementById('ContentPlaceHolder1_div_comment');
            if (divComment && divComment.innerHTML.trim() !== '' && divComment.innerHTML.trim() !== '&nbsp;') {
                $('#modal-messagebox').modal('show');
            }
        }

        function clearMessages() {
            var divComment = document.getElementById('ContentPlaceHolder1_div_comment');
            if (divComment) {
                divComment.innerHTML = '';
            }
        }

        // ===== PAGE INITIALIZATION =====
        function initializePageElements() {
            // Initialize tooltips
            initializeTooltips();

            // Initialize clickable cells
            initializeClickableCells();

            // Initialize table scroll
            //initializeTableScroll();

            // Initialize keyboard navigation
            initializeKeyboardNavigation();
        }

        // ===== TOOLTIP INITIALIZATION =====
        function initializeTooltips() {
            // Initialize Bootstrap tooltips if available
            if (typeof $.fn.tooltip !== 'undefined') {
                $('[data-toggle="tooltip"]').tooltip();
            }

            // Add custom tooltips for long text
            $('.col-supp, .col-komponen').each(function () {
                var $this = $(this);
                if (this.offsetWidth < this.scrollWidth) {
                    $this.attr('title', $this.text());
                }
            });
        }

        // ===== CLICKABLE CELLS INITIALIZATION =====
        function initializeClickableCells() {
            // Add hover effects
            $(document).on('mouseenter', '.clickable-cell:not(.zero-value)', function () {
                $(this).addClass('hover-effect');
            });

            $(document).on('mouseleave', '.clickable-cell', function () {
                $(this).removeClass('hover-effect');
            });

            // Add click feedback
            $(document).on('click', '.clickable-cell:not(.zero-value)', function () {
                var $this = $(this);
                $this.addClass('click-effect');

                // Remove effect after animation
                setTimeout(function () {
                    $this.removeClass('click-effect');
                }, 300);
            });

            // Prevent action on zero-value cells
            $(document).on('click', '.zero-value', function (e) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            });
        }

        // ===== TABLE SCROLL FUNCTIONS =====
        function initializeTableScroll() {
            var scrollContainer = $('.table-scroll-container');
            var scrollIndicator = null;

            if (scrollContainer.length > 0) {
                // Create scroll indicator
                scrollIndicator = $('<div class="scroll-indicator">↔ Scroll horizontal untuk melihat semua kolom</div>');
                scrollContainer.parent().append(scrollIndicator);

                // Show/hide scroll indicator based on scroll position
                scrollContainer.on('scroll', function () {
                    var maxScroll = this.scrollWidth - this.clientWidth;
                    var currentScroll = this.scrollLeft;

                    if (maxScroll > 0) {
                        if (currentScroll === 0) {
                            scrollIndicator.text('→ Scroll ke kanan untuk melihat lebih banyak');
                            scrollIndicator.addClass('show');
                        } else if (currentScroll >= maxScroll - 1) {
                            scrollIndicator.text('← Scroll ke kiri untuk melihat kolom awal');
                            scrollIndicator.addClass('show');
                        } else {
                            scrollIndicator.text('↔ Scroll horizontal untuk melihat semua kolom');
                            scrollIndicator.addClass('show');
                        }
                    } else {
                        scrollIndicator.removeClass('show');
                    }
                });

                // Initial check
                scrollContainer.trigger('scroll');

                // Enable horizontal scroll with shift + mouse wheel
                scrollContainer.on('wheel', function (e) {
                    if (e.originalEvent.shiftKey) {
                        e.preventDefault();
                        var delta = e.originalEvent.deltaY;
                        this.scrollLeft += delta;
                    }
                });
            }
        }

        // ===== KEYBOARD NAVIGATION =====
        function initializeKeyboardNavigation() {
            // Navigate between clickable cells with arrow keys
            $(document).on('keydown', '.clickable-cell', function (e) {
                var $currentCell = $(this);
                var $currentRow = $currentCell.parent();
                var cellIndex = $currentCell.index();
                var $targetCell = null;

                switch (e.keyCode) {
                    case 37: // Left arrow
                        $targetCell = $currentCell.prev('.clickable-cell');
                        break;
                    case 39: // Right arrow
                        $targetCell = $currentCell.next('.clickable-cell');
                        break;
                    case 38: // Up arrow
                        var $prevRow = $currentRow.prev('tr');
                        if ($prevRow.length > 0) {
                            $targetCell = $prevRow.find('td').eq(cellIndex);
                            if (!$targetCell.hasClass('clickable-cell')) {
                                $targetCell = $prevRow.find('.clickable-cell').first();
                            }
                        }
                        break;
                    case 40: // Down arrow
                        var $nextRow = $currentRow.next('tr');
                        if ($nextRow.length > 0) {
                            $targetCell = $nextRow.find('td').eq(cellIndex);
                            if (!$targetCell.hasClass('clickable-cell')) {
                                $targetCell = $nextRow.find('.clickable-cell').first();
                            }
                        }
                        break;
                    case 13: // Enter key
                    case 32: // Space key
                        e.preventDefault();
                        $currentCell.click();
                        return false;
                }

                if ($targetCell && $targetCell.length > 0 && $targetCell.hasClass('clickable-cell')) {
                    e.preventDefault();
                    $targetCell.focus();
                }
            });
        }

        // ===== EXPORT FUNCTIONS =====
        function beforeExport(type) {
            showPageLoader();
            setTimeout(hidePageLoader, 4000);

            // Show loading message
            var statusMsg = type === 'csv' ? 'Preparing CSV export...' : 'Preparing Excel export...';
            showExportStatus(statusMsg);
        }

        function showExportStatus(message) {
            // Create or update status message
            var statusDiv = $('#export-status');
            if (statusDiv.length === 0) {
                statusDiv = $('<div id="export-status" class="alert alert-info" style="position: fixed; top: 10px; right: 10px; z-index: 9999;"></div>');
                $('body').append(statusDiv);
            }

            statusDiv.html('<i class="fa fa-spinner fa-spin"></i> ' + message).fadeIn();

            // Auto hide after 3 seconds
            setTimeout(function () {
                statusDiv.fadeOut();
            }, 3000);
        }

        // ===== UTILITY FUNCTIONS =====
        function formatNumber(num) {
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function highlightRow(rowElement) {
            var $row = $(rowElement);
            $row.addClass('highlighted-row');

            setTimeout(function () {
                $row.removeClass('highlighted-row');
            }, 2000);
        }

        // ===== DATE RANGE VALIDATION =====
        function validateDateRange() {
            var dateFrom = document.getElementById('ContentPlaceHolder1_txtDateFrom').value;
            var dateTo = document.getElementById('ContentPlaceHolder1_txtDateTo').value;

            if (dateFrom && dateTo) {
                var fromDate = new Date(dateFrom);
                var toDate = new Date(dateTo);

                if (fromDate > toDate) {
                    alert('Tanggal awal tidak boleh lebih besar dari tanggal akhir');
                    return false;
                }
            }

            return true;
        }

        // ===== PRINT FUNCTIONALITY =====
        function printReport() {
            // Hide non-printable elements
            $('.box-footer, .box-tools, .breadcrumb').hide();

            // Expand table for printing
            $('.table-scroll-container').css({
                'max-height': 'none',
                'overflow': 'visible'
            });

            // Print
            window.print();

            // Restore elements
            $('.box-footer, .box-tools, .breadcrumb').show();
            $('.table-scroll-container').css({
                'max-height': '600px',
                'overflow': 'auto'
            });
        }

        // ===== RESPONSIVE TABLE ADJUSTMENTS =====
        function adjustTableForMobile() {
            if (window.innerWidth < 768) {
                // Add mobile-friendly classes
                $('.stock-opname-table').addClass('mobile-view');

                // Show mobile instructions
                var mobileHint = $('.mobile-hint');
                if (mobileHint.length === 0) {
                    mobileHint = $('<div class="mobile-hint alert alert-info">Swipe left/right to see all columns</div>');
                    $('.table-scroll-container').before(mobileHint);
                }
                mobileHint.show();
            } else {
                $('.stock-opname-table').removeClass('mobile-view');
                $('.mobile-hint').hide();
            }
        }

        // ===== DOCUMENT READY =====
        $(document).ready(function () {
            console.log("Stock Opname Report - Page initialized");
            hidePageLoader();

            // Initialize all page elements
            initializePageElements();

            // Check for initial messages
            checkAndShowMessages();

            // Setup export button handlers
            $('#ContentPlaceHolder1_CmdExport').on('click', function () {
                beforeExport('csv');
            });

            $('#ContentPlaceHolder1_CmdExportXls').on('click', function () {
                beforeExport('xls');
            });

            // Setup search button validation
            $('#ContentPlaceHolder1_CmdSearch').on('click', function (e) {
                if (!validateDateRange()) {
                    hidePageLoader();
                    e.preventDefault();
                    return false;
                }

                showPageLoader();
            });

            // Setup clear button
            $('#ContentPlaceHolder1_CmdClear').on('click', function () {
                clearMessages();
                showPageLoader();
            });

            // Handle responsive adjustments
            adjustTableForMobile();
            $(window).on('resize', function () {
                adjustTableForMobile();
            });

            // Setup message modal close handler
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                clearMessages();
            });

            // Add print button if needed
            if ($('#btnPrint').length === 0) {
                var printBtn = $('<button id="btnPrint" class="btn btn-default btn-sm" onclick="printReport()"><i class="fa fa-print"></i> Print</button>');
                $('.box-footer').prepend(printBtn);
            }

            // Focus on search field
            $('#ContentPlaceHolder1_txtSearch').focus();

            // Add CSS animations
            addCustomAnimations();
        });

        // ===== CUSTOM ANIMATIONS =====
        function addCustomAnimations() {
            // Add CSS for animations if not exists
            if ($('#custom-animations').length === 0) {
                var styles = `
                <style id="custom-animations">
                    .hover-effect {
                        animation: pulse 0.3s ease-in-out;
                    }
                    
                    .click-effect {
                        animation: clickPulse 0.3s ease-in-out;
                    }
                    
                    .highlighted-row {
                        animation: highlight 2s ease-in-out;
                    }
                    
                    @keyframes pulse {
                        0% { transform: scale(1); }
                        50% { transform: scale(1.02); }
                        100% { transform: scale(1); }
                    }
                    
                    @keyframes clickPulse {
                        0% { transform: scale(1); }
                        50% { transform: scale(0.98); }
                        100% { transform: scale(1); }
                    }
                    
                    @keyframes highlight {
                        0% { background-color: transparent; }
                        50% { background-color: #ffffcc; }
                        100% { background-color: transparent; }
                    }
                    
                    .mobile-view {
                        font-size: 10px !important;
                    }
                    
                    .mobile-view td, .mobile-view th {
                        padding: 2px !important;
                    }
                </style>
            `;
                $('head').append(styles);
            }
        }

        // ===== GLOBAL ERROR HANDLER =====
        window.onerror = function (msg, url, lineNo, columnNo, error) {
            console.error('Error: ', msg, '\nURL: ', url, '\nLine: ', lineNo, '\nColumn: ', columnNo, '\nError object: ', error);
            hidePageLoader();
            return false;
        };
    </script>
</asp:Content>