<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_eseal_rent.aspx.cs" Inherits="vtsadm.dashboard_eseal_rent" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .eseal-dashboard {
            padding-bottom: 20px;
            color: #1a1a18;
            overflow-x: hidden;
        }

        .eseal-dashboard,
        .eseal-dashboard * {
            box-sizing: border-box;
        }

        .eseal-topbar {
            background: #ffffff;
            border: 1px solid #e5e3df;
            border-radius: 12px;
            min-height: 60px;
            padding: 14px 18px;
            margin-bottom: 16px;
            display: flex;
            align-items: center;
            gap: 14px;
            flex-wrap: wrap;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
        }

        .eseal-brand {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .eseal-brand-icon {
            width: 36px;
            height: 36px;
            border-radius: 8px;
            background: linear-gradient(135deg, #f97316, #ea580c);
            display: inline-flex;
            align-items: center;
            justify-content: center;
            color: #fff;
            font-size: 16px;
            flex-shrink: 0;
        }

        .eseal-brand-name {
            font-size: 16px;
            font-weight: 700;
            line-height: 1.1;
            letter-spacing: -.2px;
        }

        .eseal-brand-sub {
            font-size: 12px;
            color: #9b9b93;
            margin-top: 2px;
        }

        .summary-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 12px;
            margin-bottom: 14px;
        }

        .summary-card {
            background: #fff;
            border: 1px solid #e5e3df;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, .08);
            padding: 16px 18px;
            position: relative;
            overflow: hidden;
            transition: transform .2s ease, box-shadow .2s ease;
        }

        .summary-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 18px rgba(0, 0, 0, 0.1);
        }

        .summary-card:before {
            content: "";
            height: 3px;
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
        }

        .card-daily:before { background: #f97316; }
        .card-weekly:before { background: #fb923c; }
        .card-monthly:before { background: #f59e0b; }
        .card-total:before { background: #ea580c; }

        .eseal-summary-title {
            margin: 0 0 7px;
            color: #9b9b93;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: .6px;
            font-weight: 700;
        }

        .eseal-summary-value {
            color: #1f1f1d;
            font-size: 30px;
            font-weight: 800;
            line-height: 1;
            margin: 0;
        }

        .eseal-summary-desc {
            margin: 8px 0 0;
            font-size: 12px;
            color: #7b7b74;
            line-height: 1.4;
        }

        .eseal-summary-icon {
            position: absolute;
            right: 14px;
            top: 50%;
            transform: translateY(-50%);
            font-size: 28px;
            color: rgba(249, 115, 22, 0.18);
        }

        .section-box {
            background: #fff;
            border: 1px solid #e5e3df;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, .08);
            margin-bottom: 14px;
            overflow: hidden;
        }

        .section-header {
            padding: 12px 16px;
            border-bottom: 1px solid #e5e3df;
            background: #fafaf8;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .section-title {
            margin: 0;
            font-size: 13px;
            font-weight: 700;
            color: #1a1a18;
        }

        .section-header-right {
            margin-left: auto;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .panel-badge {
            font-size: 11px;
            background: #fff4e6;
            color: #c2410c;
            padding: 2px 8px;
            border-radius: 20px;
            font-weight: 600;
        }

        .eseal-filter-wrap {
            padding: 14px 16px;
        }

        .eseal-filter-grid {
            display: grid;
            grid-template-columns: 2fr 1fr 1fr 1fr auto;
            gap: 10px;
            align-items: end;
        }

        .eseal-filter-field label {
            display: block;
            margin: 0 0 5px;
            font-size: 11px;
            font-weight: 700;
            color: #6f6f68;
            text-transform: uppercase;
            letter-spacing: .35px;
        }

        .eseal-filter-field .form-control {
            height: 34px;
            border-radius: 7px;
            border: 1px solid #d1cfc9;
            box-shadow: none;
            font-size: 12px;
        }

        .eseal-filter-actions {
            display: flex;
            gap: 8px;
            align-items: center;
        }

        .eseal-btn {
            border-radius: 7px;
            min-width: 74px;
            font-size: 12px;
            padding: 7px 12px;
            font-weight: 600;
            min-height: 34px;
        }

        .btn-eseal-search {
            background: #f97316;
            border-color: #ea580c;
            color: #fff;
        }

        .btn-eseal-search:hover,
        .btn-eseal-search:focus {
            background: #ea580c;
            border-color: #c2410c;
            color: #fff;
        }

        .perf-chart-grid {
            display: grid;
            grid-template-columns: 1fr 1.4fr;
            gap: 12px;
            padding: 14px 16px;
        }

        .perf-chart-panel {
            border: 1px solid #eceae6;
            border-radius: 12px;
            padding: 14px;
            background: #fff;
        }

        .perf-chart-panel-title {
            margin: 0 0 10px;
            font-size: 13px;
            font-weight: 700;
            color: #1a1a18;
        }

        .perf-chart-panel-heading {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
            margin-bottom: 10px;
        }

        .perf-chart-panel-heading .perf-chart-panel-title {
            margin: 0;
        }

        .perf-chart-select {
            min-width: 140px;
            height: 32px;
            border: 1px solid #e5e3df;
            border-radius: 8px;
            background: #fff;
            color: #1a1a18;
            font-size: 12px;
            font-weight: 600;
            padding: 0 10px;
        }

        .eseal-type-chart-wrap {
            display: flex;
            align-items: center;
            gap: 12px;
            min-height: 260px;
        }

        .eseal-type-chart-canvas {
            flex: 1 1 55%;
            min-width: 0;
            height: 260px;
        }

        .eseal-type-legend {
            flex: 1 1 45%;
            display: flex;
            flex-direction: column;
            gap: 14px;
            min-width: 140px;
        }

        .eseal-type-legend-item {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .eseal-type-legend-top {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px;
        }

        .eseal-type-legend-label {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            font-size: 12px;
            font-weight: 600;
            color: #3f3f3a;
        }

        .eseal-type-legend-dot {
            width: 10px;
            height: 10px;
            border-radius: 3px;
            flex-shrink: 0;
        }

        .eseal-type-legend-value {
            font-size: 12px;
            font-weight: 700;
            color: #1a1a18;
            white-space: nowrap;
        }

        .eseal-type-legend-bar {
            width: 100%;
            height: 6px;
            border-radius: 999px;
            background: #f3f1ed;
            overflow: hidden;
        }

        .eseal-type-legend-bar > span {
            display: block;
            height: 100%;
            border-radius: 999px;
        }

        .perf-chart-canvas {
            width: 100%;
            height: 280px;
        }

        .eseal-table-wrap {
            padding: 0 16px 14px;
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .eseal-table {
            width: 100%;
            min-width: 1200px;
            border-collapse: collapse;
            font-size: 12px;
        }

        .eseal-table thead th {
            background: #fafaf8;
            border-bottom: 1px solid #e5e3df;
            padding: 10px 8px;
            text-align: left;
            font-size: 11px;
            font-weight: 700;
            color: #5c5c55;
            white-space: nowrap;
        }

        .eseal-table tbody td {
            border-bottom: 1px solid #f0eeea;
            padding: 10px 8px;
            vertical-align: middle;
            color: #1f1f1d;
        }

        .eseal-table tbody tr:hover {
            background: #fffaf5;
        }

        .eseal-btn-action {
            border-radius: 6px;
            font-size: 11px;
            font-weight: 600;
            padding: 5px 10px;
            min-height: 30px;
            line-height: 1.2;
            touch-action: manipulation;
        }

        .eseal-btn-detail {
            background: #fff7ed;
            border: 1px solid #fdba74;
            color: #c2410c;
        }

        .eseal-status-badge {
            display: inline-block;
            border-radius: 20px;
            padding: 4px 10px;
            font-size: 10px;
            font-weight: 700;
            white-space: nowrap;
        }

        .eseal-status-lengkap { background: #dcfce7; color: #166534; }
        .eseal-status-belum { background: #fff7ed; color: #c2410c; }

        .eseal-export-btn {
            border-radius: 7px;
            font-size: 12px;
            font-weight: 600;
            padding: 6px 12px;
            background: #fff;
            border: 1px solid #fdba74;
            color: #c2410c;
        }

        .eseal-export-btn:hover {
            background: #fff7ed;
            color: #9a3412;
        }

        .empty-text {
            padding: 16px;
            text-align: center;
            color: #9b9b93;
            font-size: 12px;
        }

        #esealRentDetailModal .modal-dialog {
            width: 96%;
            max-width: 1100px;
            margin: 30px auto;
        }

        #esealRentDetailModal .modal-content {
            border-radius: 12px;
            border: 1px solid #e5e3df;
        }

        #esealRentDetailModal .modal-header {
            border-bottom: 1px solid #eceae6;
            padding: 14px 18px;
        }

        #esealRentDetailModal .modal-title {
            font-size: 16px;
            font-weight: 700;
            color: #1a1a18;
        }

        #esealRentDetailModal .eseal-detail-subtitle {
            display: inline-block;
            margin-top: 4px;
            font-size: 12px;
            font-weight: 600;
            color: #c2410c;
            background: #fff7ed;
            border: 1px solid #fdba74;
            border-radius: 999px;
            padding: 2px 10px;
        }

        #esealRentDetailModal .modal-body {
            max-height: calc(100vh - 210px);
            overflow-y: auto;
            padding: 16px 18px;
        }

        #esealRentDetailModal .modal-footer {
            border-top: 1px solid #eceae6;
            padding: 12px 18px;
        }

        .eseal-detail-section {
            border: 1px solid #eceae6;
            border-radius: 10px;
            background: #fcfcfb;
            padding: 12px 14px;
            margin-bottom: 12px;
        }

        .eseal-detail-section-title {
            margin: 0 0 10px;
            font-size: 13px;
            font-weight: 700;
            color: #1a1a18;
        }

        .eseal-detail-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 8px 16px;
        }

        .eseal-detail-field-label {
            display: block;
            font-size: 11px;
            color: #8f8f89;
            margin-bottom: 2px;
        }

        .eseal-detail-field-value {
            display: block;
            font-size: 13px;
            font-weight: 600;
            color: #1f1f1d;
            word-break: break-word;
        }

        .eseal-detail-summary-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 8px;
        }

        .eseal-detail-summary-card {
            border: 1px solid #eceae6;
            border-radius: 8px;
            background: #fff;
            padding: 10px 12px;
        }

        .eseal-detail-summary-card .eseal-detail-field-label {
            margin-bottom: 4px;
        }

        .eseal-detail-table-wrap {
            overflow-x: auto;
        }

        .eseal-detail-table {
            width: 100%;
            border-collapse: collapse;
            min-width: 720px;
        }

        .eseal-detail-table th,
        .eseal-detail-table td {
            border-bottom: 1px solid #eceae6;
            padding: 8px 10px;
            font-size: 12px;
            text-align: left;
            white-space: nowrap;
        }

        .eseal-detail-table th {
            background: #fff7ed;
            color: #9a3412;
            font-weight: 700;
        }

        .eseal-detail-empty,
        .eseal-detail-error {
            padding: 14px;
            text-align: center;
            color: #9b9b93;
            font-size: 12px;
        }

        .eseal-detail-error {
            color: #b91c1c;
            background: #fef2f2;
            border-radius: 8px;
        }

        .eseal-detail-loading {
            display: none;
            text-align: center;
            padding: 28px 12px;
            color: #6f6f68;
            font-size: 13px;
        }

        .eseal-detail-loading .fa {
            margin-right: 6px;
            color: #f97316;
        }

        #esealRentDetailOverlay {
            display: none;
            position: fixed;
            inset: 0;
            background: rgba(255, 255, 255, 0.55);
            z-index: 2000;
        }

        #esealRentDetailOverlay .cv-spinner {
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        #esealRentDetailOverlay .spinner {
            width: 40px;
            height: 40px;
            border: 3px solid #fdba74;
            border-top-color: #ea580c;
            border-radius: 50%;
            animation: eseal-spin 0.8s linear infinite;
        }

        @keyframes eseal-spin {
            to { transform: rotate(360deg); }
        }

        @media (max-width: 991px) {
            .eseal-detail-summary-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }
        }

        @media (max-width: 767px) {
            .eseal-detail-grid,
            .eseal-detail-summary-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 1199px) {
            .eseal-filter-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .eseal-filter-actions {
                grid-column: 1 / -1;
            }

            .summary-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .perf-chart-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 767px) {
            .eseal-type-chart-wrap {
                flex-direction: column;
            }

            .eseal-type-chart-canvas,
            .eseal-type-legend {
                width: 100%;
            }

            .perf-chart-panel-heading {
                flex-direction: column;
                align-items: flex-start;
            }

            .perf-chart-select {
                width: 100%;
            }

            .summary-grid {
                grid-template-columns: 1fr;
            }

            .eseal-filter-grid {
                grid-template-columns: 1fr;
            }

            .eseal-filter-actions {
                flex-direction: column;
                width: 100%;
            }

            .eseal-filter-actions .eseal-btn {
                width: 100%;
            }

            .eseal-summary-value {
                font-size: 26px;
            }

            .perf-chart-canvas {
                height: 240px;
            }
        }
    </style>

    <section class="content-header">
        <h1>Dashboard <small>E-Seal Rent</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li class="active">E-Seal Rent</li>
        </ol>
    </section>

    <section class="content eseal-dashboard">
        <div class="eseal-topbar">
            <div class="eseal-brand">
                <div class="eseal-brand-icon" aria-hidden="true"><i class="fa fa-lock"></i></div>
                <div>
                    <div class="eseal-brand-name">Dashboard E-Seal Rent</div>
                    <div class="eseal-brand-sub">Monitoring paket sewa e-seal berdasarkan periode, tipe sewa, dan status evidence.</div>
                </div>
            </div>
        </div>

        <div class="summary-grid">
            <div class="summary-card card-daily">
                <i class="fa fa-calendar-o eseal-summary-icon" aria-hidden="true"></i>
                <p class="eseal-summary-title">SO Daily</p>
                <p class="eseal-summary-value"><span id="lblSoDaily" runat="server">0</span></p>
                <p class="eseal-summary-desc">Paket sewa harian aktif pada periode</p>
            </div>
            <div class="summary-card card-weekly">
                <i class="fa fa-calendar eseal-summary-icon" aria-hidden="true"></i>
                <p class="eseal-summary-title">SO Weekly</p>
                <p class="eseal-summary-value"><span id="lblSoWeekly" runat="server">0</span></p>
                <p class="eseal-summary-desc">Paket sewa mingguan aktif pada periode</p>
            </div>
            <div class="summary-card card-monthly">
                <i class="fa fa-calendar-check-o eseal-summary-icon" aria-hidden="true"></i>
                <p class="eseal-summary-title">SO Monthly</p>
                <p class="eseal-summary-value"><span id="lblSoMonthly" runat="server">0</span></p>
                <p class="eseal-summary-desc">Paket sewa bulanan aktif pada periode</p>
            </div>
            <div class="summary-card card-total">
                <i class="fa fa-cubes eseal-summary-icon" aria-hidden="true"></i>
                <p class="eseal-summary-title">Total Package</p>
                <p class="eseal-summary-value"><span id="lblTotalPackage" runat="server">0</span></p>
                <p class="eseal-summary-desc">Seluruh paket sewa pada periode</p>
            </div>
        </div>

        <div class="section-box">
            <div class="section-header">
                <h4 class="section-title">Filter Data</h4>
            </div>
            <div class="eseal-filter-wrap">
                <div class="eseal-filter-grid">
                    <div class="eseal-filter-field">
                        <label for="<%= txtSearch.ClientID %>">Cari Data</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Customer / PIC / No HP / No Polisi / No SN" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="eseal-filter-field">
                        <label for="<%= ddlRentalType.ClientID %>">Tipe Sewa</label>
                        <asp:DropDownList ID="ddlRentalType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Semua" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Daily" Text="Daily"></asp:ListItem>
                            <asp:ListItem Value="Weekly" Text="Weekly"></asp:ListItem>
                            <asp:ListItem Value="Monthly" Text="Monthly"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="eseal-filter-field">
                        <label for="<%= ddlEvidenceStatus.ClientID %>">Status Evidence</label>
                        <asp:DropDownList ID="ddlEvidenceStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Semua" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="lengkap" Text="Evidence Lengkap"></asp:ListItem>
                            <asp:ListItem Value="belum_lengkap" Text="Belum Lengkap"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="eseal-filter-field">
                        <label for="<%= txtPeriode.ClientID %>">Periode</label>
                        <asp:TextBox ID="txtPeriode" runat="server" CssClass="form-control js-eseal-periode-picker" placeholder="MMMM yyyy" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="eseal-filter-actions">
                        <asp:Button ID="btnCari" runat="server" CssClass="btn btn-eseal-search eseal-btn" Text="Cari" OnClick="btnCari_Click" />
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-default eseal-btn" Text="Reset" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="section-box perf-chart-section">
            <div class="section-header">
                <h4 class="section-title">Grafik Monitoring</h4>
                <span class="panel-badge">Periode: <span id="lblChartPeriode" runat="server">-</span></span>
            </div>
            <div class="perf-chart-grid">
                <div class="perf-chart-panel">
                    <h5 class="perf-chart-panel-title">Distribusi Paket Sewa Berdasarkan Tipe</h5>
                    <div class="eseal-type-chart-wrap">
                        <div id="esealRentTypeChart" class="eseal-type-chart-canvas" role="img" aria-label="Grafik distribusi paket sewa berdasarkan tipe"></div>
                        <div id="esealRentTypeLegend" class="eseal-type-legend" aria-hidden="true"></div>
                    </div>
                </div>
                <div class="perf-chart-panel">
                    <div class="perf-chart-panel-heading">
                        <h5 class="perf-chart-panel-title">Aktivitas Rental (Jumlah Paket)</h5>
                        <select id="esealActivityRangeSelect" class="perf-chart-select" aria-label="Rentang aktivitas rental">
                            <option value="7" selected="selected">7 Hari Terakhir</option>
                            <option value="14">14 Hari Terakhir</option>
                            <option value="30">30 Hari Terakhir</option>
                            <option value="all">Periode Aktif</option>
                        </select>
                    </div>
                    <div id="esealRentDailyChart" class="perf-chart-canvas" role="img" aria-label="Grafik aktivitas rental jumlah paket"></div>
                </div>
            </div>
        </div>

        <div class="section-box">
            <div class="section-header">
                <h4 class="section-title">Detail Informasi Sewa</h4>
                <div class="section-header-right">
                    <span class="panel-badge"><span id="lblGridCount" runat="server">0</span> data</span>
                    <asp:Button ID="btnExportCsv" runat="server" CssClass="eseal-export-btn" Text="Export CSV" OnClick="btnExportCsv_Click" />
                </div>
            </div>
            <div class="eseal-table-wrap">
                <table class="eseal-table">
                    <thead>
                        <tr>
                            <th>Nama Customer</th>
                            <th>Nama PIC</th>
                            <th>No. HP PIC</th>
                            <th>Tipe Sewa</th>
                            <th>Total Unit</th>
                            <th>Tanggal Order</th>
                            <th>Tanggal Expired</th>
                            <th>Harga</th>
                            <th>Status Evidence</th>
                            <th>Aksi</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptDetail" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Customer") %></td>
                                    <td><%# Eval("Pic") %></td>
                                    <td><%# Eval("PhoneNo") %></td>
                                    <td><%# Eval("RentalType") %></td>
                                    <td><%# Eval("TotalUnitDisplay") %></td>
                                    <td><%# Eval("OrderDateDisplay") %></td>
                                    <td><%# Eval("ExpiredDateDisplay") %></td>
                                    <td><%# Eval("PriceDisplay") %></td>
                                    <td><%# Eval("EvidenceStatusHtml") %></td>
                                    <td><%# Eval("ActionHtml") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <div id="lblGridEmpty" runat="server" class="empty-text" visible="false">Tidak ada data sewa sesuai filter yang dipilih.</div>
            </div>
        </div>

        <asp:HiddenField ID="hfChartTypeJson" runat="server" />
        <asp:HiddenField ID="hfChartDailyJson" runat="server" />
        <asp:HiddenField ID="hfPeriodValue" runat="server" />
    </section>

    <div class="modal fade" id="esealRentDetailModal" tabindex="-1" role="dialog" aria-labelledby="esealRentDetailModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="esealRentDetailModalLabel">Detail Informasi Sewa</h4>
                    <span class="eseal-detail-subtitle" id="esealDetailPoNumber">-</span>
                </div>
                <div class="modal-body">
                    <div class="eseal-detail-loading" id="esealDetailLoading">
                        <i class="fa fa-spinner fa-spin" aria-hidden="true"></i> Memuat detail sewa...
                    </div>
                    <div id="esealDetailError" class="eseal-detail-error" style="display:none;"></div>
                    <div id="esealDetailContent" style="display:none;">
                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Informasi Sales Order</h5>
                            <div class="eseal-detail-grid">
                                <div><span class="eseal-detail-field-label">PoID</span><span class="eseal-detail-field-value" id="esealHdrPoID">-</span></div>
                                <div><span class="eseal-detail-field-label">Nomor SO</span><span class="eseal-detail-field-value" id="esealHdrPONumber">-</span></div>
                                <div><span class="eseal-detail-field-label">Tanggal Order</span><span class="eseal-detail-field-value" id="esealHdrOrderDate">-</span></div>
                                <div><span class="eseal-detail-field-label">Tipe Sewa</span><span class="eseal-detail-field-value" id="esealHdrRentalType">-</span></div>
                                <div><span class="eseal-detail-field-label">Tanggal Expired</span><span class="eseal-detail-field-value" id="esealHdrExpiredDate">-</span></div>
                                                                <div><span class="eseal-detail-field-label">Status SO</span><span class="eseal-detail-field-value" id="esealHdrPoStatus">-</span></div>
                            </div>
                        </div>

                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Informasi Customer</h5>
                            <div class="eseal-detail-grid">
                                <div><span class="eseal-detail-field-label">Nama Customer</span><span class="eseal-detail-field-value" id="esealHdrCustomerName">-</span></div>
                                <div><span class="eseal-detail-field-label">Nama PIC</span><span class="eseal-detail-field-value" id="esealHdrPicName">-</span></div>
                                <div><span class="eseal-detail-field-label">No. HP PIC</span><span class="eseal-detail-field-value" id="esealHdrPicMobilePhone">-</span></div>
                                <div><span class="eseal-detail-field-label">Telepon Kantor</span><span class="eseal-detail-field-value" id="esealHdrOfficePhone">-</span></div>
                            </div>
                        </div>

                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Ringkasan Sewa</h5>
                            <div class="eseal-detail-summary-grid">
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Item</span><span class="eseal-detail-field-value" id="esealHdrTotalItem">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Unit</span><span class="eseal-detail-field-value" id="esealHdrTotalUnit">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Quantity Selesai</span><span class="eseal-detail-field-value" id="esealHdrTotalQuantityDone">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Job</span><span class="eseal-detail-field-value" id="esealHdrTotalJob">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Job Selesai</span><span class="eseal-detail-field-value" id="esealHdrTotalClosedJob">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Job Belum Selesai</span><span class="eseal-detail-field-value" id="esealHdrTotalPendingJob">0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Status Evidence</span><span class="eseal-detail-field-value" id="esealHdrEvidenceStatus">-</span></div>
                            </div>
                        </div>

                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Ringkasan Biaya</h5>
                            <div class="eseal-detail-summary-grid">
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Harga</span><span class="eseal-detail-field-value" id="esealHdrTotalPrice">Rp0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Biaya Instalasi</span><span class="eseal-detail-field-value" id="esealHdrTotalInstallFee">Rp0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Total Biaya Bulanan</span><span class="eseal-detail-field-value" id="esealHdrTotalMonthlyFee">Rp0</span></div>
                                <div class="eseal-detail-summary-card"><span class="eseal-detail-field-label">Grand Total</span><span class="eseal-detail-field-value" id="esealHdrGrandTotal">Rp0</span></div>
                            </div>
                        </div>

                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Detail Item</h5>
                            <div class="eseal-detail-table-wrap">
                                <table class="eseal-detail-table">
                                    <thead>
                                        <tr>
                                            <th>No.</th>
                                            <th>Quantity</th>
                                            <th>Quantity Done</th>
                                            <th>Harga</th>
                                            <th>Biaya Instalasi</th>
                                            <th>Biaya Bulanan</th>
                                            <th>Subtotal Harga</th>
                                            <th>Total</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody id="esealDetailItemsBody"></tbody>
                                </table>
                            </div>
                            <div id="esealDetailItemsEmpty" class="eseal-detail-empty" style="display:none;">Detail item tidak tersedia.</div>
                        </div>

                        <div class="eseal-detail-section">
                            <h5 class="eseal-detail-section-title">Daftar Job</h5>
                            <div class="eseal-detail-table-wrap">
                                <table class="eseal-detail-table">
                                    <thead>
                                        <tr>
                                            <th>No.</th>
                                            <th>Job ID</th>
                                            <th>Status</th>
                                            <th>Keterangan</th>
                                            <th>Status Evidence</th>
                                        </tr>
                                    </thead>
                                    <tbody id="esealDetailJobsBody"></tbody>
                                </table>
                            </div>
                            <div id="esealDetailJobsEmpty" class="eseal-detail-empty" style="display:none;">Belum ada job untuk Sales Order ini.</div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>

    <div id="esealRentDetailOverlay">
        <div class="cv-spinner"><span class="spinner"></span></div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/echarts/5.4.3/echarts.min.js"></script>
    <script type="text/javascript">
        (function () {
            var TYPE_COLORS = {
                Daily: "#c2410c",
                Weekly: "#f97316",
                Monthly: "#fdba74"
            };

            var esealChartState = {
                typeInstance: null,
                dailyInstance: null,
                activityRows: [],
                eventsBound: false
            };

            function disposeEsealChart(key) {
                if (esealChartState[key]) {
                    try {
                        esealChartState[key].dispose();
                    } catch (ex) { }
                    esealChartState[key] = null;
                }
            }

            function parseChartJson(hiddenId) {
                var el = document.getElementById(hiddenId);
                if (!el || !el.value) {
                    return [];
                }
                try {
                    return JSON.parse(el.value);
                } catch (ex) {
                    return [];
                }
            }

            function escapeHtml(value) {
                return String(value == null ? "" : value)
                    .replace(/&/g, "&amp;")
                    .replace(/</g, "&lt;")
                    .replace(/>/g, "&gt;")
                    .replace(/"/g, "&quot;")
                    .replace(/'/g, "&#39;");
            }

            function normalizeTypeChartPayload(raw) {
                if (!raw) {
                    return { totalPackage: 0, items: [] };
                }

                if (Object.prototype.toString.call(raw) === "[object Array]") {
                    var sum = 0;
                    for (var i = 0; i < raw.length; i++) {
                        sum += parseInt(raw[i].total, 10) || 0;
                    }
                    return { totalPackage: sum, items: raw };
                }

                return {
                    totalPackage: parseInt(raw.totalPackage, 10) || 0,
                    items: raw.items || []
                };
            }

            function renderTypeLegend(rows, totalPackage) {
                var legendDom = document.getElementById("esealRentTypeLegend");
                if (!legendDom) {
                    return;
                }

                var html = "";
                for (var i = 0; i < rows.length; i++) {
                    var name = rows[i].label || "-";
                    var value = parseInt(rows[i].total, 10) || 0;
                    var pct = totalPackage > 0 ? ((value * 100) / totalPackage) : 0;
                    var pctText = pct.toFixed(1).replace(".", ",") + "%";
                    var color = TYPE_COLORS[name] || "#f97316";
                    var width = Math.max(0, Math.min(100, pct));

                    html += '<div class="eseal-type-legend-item">'
                        + '<div class="eseal-type-legend-top">'
                        + '<span class="eseal-type-legend-label">'
                        + '<span class="eseal-type-legend-dot" style="background:' + color + ';"></span>'
                        + escapeHtml(name)
                        + '</span>'
                        + '<span class="eseal-type-legend-value">' + value + ' (' + pctText + ')</span>'
                        + '</div>'
                        + '<div class="eseal-type-legend-bar"><span style="width:' + width + '%;background:' + color + ';"></span></div>'
                        + '</div>';
                }

                legendDom.innerHTML = html;
            }

            function renderEsealTypeChart(payload) {
                var chartDom = document.getElementById("esealRentTypeChart");
                if (!chartDom || typeof echarts === "undefined") {
                    return;
                }

                var normalized = normalizeTypeChartPayload(payload);
                var rows = normalized.items || [];
                var totalPackage = normalized.totalPackage || 0;
                var data = [];

                for (var i = 0; i < rows.length; i++) {
                    data.push({
                        name: rows[i].label || "-",
                        value: parseInt(rows[i].total, 10) || 0,
                        itemStyle: { color: TYPE_COLORS[rows[i].label] || "#f97316" }
                    });
                }

                renderTypeLegend(rows, totalPackage);

                disposeEsealChart("typeInstance");
                esealChartState.typeInstance = echarts.init(chartDom);
                esealChartState.typeInstance.setOption({
                    tooltip: {
                        trigger: "item",
                        confine: true,
                        formatter: function (params) {
                            var pct = totalPackage > 0
                                ? (((params.value || 0) * 100) / totalPackage).toFixed(1).replace(".", ",")
                                : "0,0";
                            return params.name + "<br/>" + params.value + " paket (" + pct + "%)";
                        }
                    },
                    series: [{
                        type: "pie",
                        radius: ["58%", "78%"],
                        center: ["50%", "50%"],
                        avoidLabelOverlap: true,
                        itemStyle: { borderColor: "#fff", borderWidth: 3 },
                        label: { show: false },
                        labelLine: { show: false },
                        data: data
                    }],
                    title: {
                        text: "{a|Total}\n{b|" + totalPackage + "}\n{c|Paket}",
                        left: "center",
                        top: "middle",
                        textAlign: "center",
                        textStyle: {
                            rich: {
                                a: { fontSize: 12, fontWeight: 600, fill: "#9b9b93", lineHeight: 18 },
                                b: { fontSize: 28, fontWeight: 800, fill: "#1a1a18", lineHeight: 34 },
                                c: { fontSize: 12, fontWeight: 600, fill: "#9b9b93", lineHeight: 18 }
                            }
                        }
                    }
                }, true);
                esealChartState.typeInstance.resize();
            }

            function sliceActivityRows(rows) {
                var select = document.getElementById("esealActivityRangeSelect");
                var range = select ? select.value : "7";
                if (!rows || !rows.length || range === "all") {
                    return rows || [];
                }

                var days = parseInt(range, 10) || 7;
                if (rows.length <= days) {
                    return rows;
                }
                return rows.slice(rows.length - days);
            }

            function renderEsealDailyChart(rows) {
                var chartDom = document.getElementById("esealRentDailyChart");
                if (!chartDom || typeof echarts === "undefined") {
                    return;
                }

                var viewRows = sliceActivityRows(rows);
                var labels = [];
                var daily = [];
                var weekly = [];
                var monthly = [];

                for (var i = 0; i < viewRows.length; i++) {
                    labels.push(viewRows[i].label || "-");
                    daily.push(parseInt(viewRows[i].daily, 10) || 0);
                    weekly.push(parseInt(viewRows[i].weekly, 10) || 0);
                    monthly.push(parseInt(viewRows[i].monthly, 10) || 0);
                }

                disposeEsealChart("dailyInstance");
                esealChartState.dailyInstance = echarts.init(chartDom);
                esealChartState.dailyInstance.setOption({
                    color: [TYPE_COLORS.Daily, TYPE_COLORS.Weekly, TYPE_COLORS.Monthly],
                    tooltip: { trigger: "axis", confine: true },
                    legend: {
                        top: 0,
                        right: 0,
                        icon: "circle",
                        itemWidth: 8,
                        itemHeight: 8,
                        textStyle: { fontSize: 11, color: "#6f6f68" }
                    },
                    grid: { left: "2%", right: "2%", bottom: "4%", top: 36, containLabel: true },
                    xAxis: {
                        type: "category",
                        boundaryGap: false,
                        data: labels,
                        axisLine: { lineStyle: { color: "#e5e3df" } },
                        axisTick: { show: false },
                        axisLabel: { fontSize: 10, color: "#6f6f68", rotate: labels.length > 12 ? 30 : 0 }
                    },
                    yAxis: {
                        type: "value",
                        min: 0,
                        minInterval: 1,
                        axisLabel: { fontSize: 10, color: "#6f6f68" },
                        splitLine: { lineStyle: { color: "#f0eeea" } }
                    },
                    series: [
                        {
                            name: "Daily",
                            type: "line",
                            data: daily,
                            smooth: false,
                            symbol: "rect",
                            symbolSize: 7,
                            lineStyle: { width: 2.5 }
                        },
                        {
                            name: "Weekly",
                            type: "line",
                            data: weekly,
                            smooth: false,
                            symbol: "rect",
                            symbolSize: 7,
                            lineStyle: { width: 2.5 }
                        },
                        {
                            name: "Monthly",
                            type: "line",
                            data: monthly,
                            smooth: false,
                            symbol: "rect",
                            symbolSize: 7,
                            lineStyle: { width: 2.5 }
                        }
                    ]
                }, true);
                esealChartState.dailyInstance.resize();
            }

            function renderEsealCharts() {
                var typePayload = parseChartJson("<%= hfChartTypeJson.ClientID %>");
                esealChartState.activityRows = parseChartJson("<%= hfChartDailyJson.ClientID %>");
                if (!esealChartState.activityRows || Object.prototype.toString.call(esealChartState.activityRows) !== "[object Array]") {
                    esealChartState.activityRows = [];
                }
                renderEsealTypeChart(typePayload);
                renderEsealDailyChart(esealChartState.activityRows);
            }

            window.renderEsealCharts = renderEsealCharts;

            function initEsealPeriodePicker() {
                if (typeof $ === "undefined" || !$.fn || !$.fn.datepicker) {
                    return;
                }

                var picker = $("#<%= txtPeriode.ClientID %>");
                if (!picker.length) {
                    return;
                }

                try {
                    picker.datepicker("destroy");
                } catch (ex) { }

                picker.datepicker({
                    format: "MM yyyy",
                    language: "id",
                    minViewMode: 1,
                    autoclose: true,
                    orientation: "bottom auto"
                }).on("changeDate", function () {
                    var hidden = document.getElementById("<%= hfPeriodValue.ClientID %>");
                    if (!hidden) {
                        return;
                    }
                    var raw = picker.val() || "";
                    var parts = raw.split(" ");
                    if (parts.length >= 2) {
                        var monthMap = {
                            januari: "01", februari: "02", maret: "03", april: "04",
                            mei: "05", juni: "06", juli: "07", agustus: "08",
                            september: "09", oktober: "10", november: "11", desember: "12"
                        };
                        var monthKey = (parts[0] || "").toLowerCase();
                        var year = parts[1] || "";
                        if (monthMap[monthKey] && year) {
                            hidden.value = year + "-" + monthMap[monthKey];
                        }
                    }
                });
            }

            function bindEsealDashboardEvents() {
                if (esealChartState.eventsBound) {
                    return;
                }
                esealChartState.eventsBound = true;

                window.addEventListener("resize", function () {
                    if (esealChartState.typeInstance) {
                        esealChartState.typeInstance.resize();
                    }
                    if (esealChartState.dailyInstance) {
                        esealChartState.dailyInstance.resize();
                    }
                });

                var rangeSelect = document.getElementById("esealActivityRangeSelect");
                if (rangeSelect) {
                    rangeSelect.addEventListener("change", function () {
                        renderEsealDailyChart(esealChartState.activityRows);
                    });
                }
            }

            function initEsealDashboard() {
                initEsealPeriodePicker();
                renderEsealCharts();
                bindEsealDashboardEvents();
            }

            if (document.readyState === "loading") {
                document.addEventListener("DOMContentLoaded", initEsealDashboard);
            } else {
                initEsealDashboard();
            }

            if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    initEsealDashboard();
                });
            }
        })();
    </script>

    <script type="text/javascript">
        (function () {
            var detailState = {
                requestSeq: 0,
                loading: false,
                currentPoId: "",
                bound: false
            };

            function escapeHtml(value) {
                return String(value == null ? "" : value)
                    .replace(/&/g, "&amp;")
                    .replace(/</g, "&lt;")
                    .replace(/>/g, "&gt;")
                    .replace(/"/g, "&quot;")
                    .replace(/'/g, "&#39;");
            }

            function displayText(value) {
                if (value == null || value === undefined) {
                    return "-";
                }
                var text = String(value).trim();
                return text ? text : "-";
            }

            function formatNumber(value) {
                var n = Number(value);
                if (!isFinite(n)) {
                    n = 0;
                }
                return Math.round(n).toLocaleString("id-ID");
            }

            function formatRp(value) {
                return "Rp" + formatNumber(value);
            }

            function setText(id, value) {
                var el = document.getElementById(id);
                if (el) {
                    el.textContent = displayText(value);
                }
            }

            function setHtml(id, html) {
                var el = document.getElementById(id);
                if (el) {
                    el.innerHTML = html;
                }
            }

            function evidenceBadgeHtml(code) {
                var normalized = String(code || "").toUpperCase();
                if (normalized === "COMPLETE") {
                    return '<span class="eseal-status-badge eseal-status-lengkap">Evidence Lengkap</span>';
                }
                return '<span class="eseal-status-badge eseal-status-belum">Belum Lengkap</span>';
            }

            function jobEvidenceBadgeHtml(isComplete) {
                if (Number(isComplete) === 1) {
                    return '<span class="eseal-status-badge eseal-status-lengkap">Selesai</span>';
                }
                return '<span class="eseal-status-badge eseal-status-belum">Belum Selesai</span>';
            }

            function showDetailLoading(isLoading) {
                var loading = document.getElementById("esealDetailLoading");
                var content = document.getElementById("esealDetailContent");
                var error = document.getElementById("esealDetailError");
                var overlay = document.getElementById("esealRentDetailOverlay");

                if (loading) {
                    loading.style.display = isLoading ? "block" : "none";
                }
                if (content && isLoading) {
                    content.style.display = "none";
                }
                if (error && isLoading) {
                    error.style.display = "none";
                    error.textContent = "";
                }
                if (overlay) {
                    overlay.style.display = isLoading ? "block" : "none";
                }
            }

            function clearDetailModal() {
                setText("esealDetailPoNumber", "-");
                setText("esealHdrPoID", "-");
                setText("esealHdrPONumber", "-");
                setText("esealHdrOrderDate", "-");
                setText("esealHdrRentalType", "-");
                setText("esealHdrExpiredDate", "-");
                setText("esealHdrPoStatus", "-");
                setText("esealHdrCustomerName", "-");
                setText("esealHdrPicName", "-");
                setText("esealHdrPicMobilePhone", "-");
                setText("esealHdrOfficePhone", "-");
                setText("esealHdrTotalItem", "0");
                setText("esealHdrTotalUnit", "0");
                setText("esealHdrTotalQuantityDone", "0");
                setText("esealHdrTotalJob", "0");
                setText("esealHdrTotalClosedJob", "0");
                setText("esealHdrTotalPendingJob", "0");
                setHtml("esealHdrEvidenceStatus", "-");
                setText("esealHdrTotalPrice", "Rp0");
                setText("esealHdrTotalInstallFee", "Rp0");
                setText("esealHdrTotalMonthlyFee", "Rp0");
                setText("esealHdrGrandTotal", "Rp0");

                var itemsBody = document.getElementById("esealDetailItemsBody");
                var jobsBody = document.getElementById("esealDetailJobsBody");
                if (itemsBody) {
                    itemsBody.innerHTML = "";
                }
                if (jobsBody) {
                    jobsBody.innerHTML = "";
                }

                var itemsEmpty = document.getElementById("esealDetailItemsEmpty");
                var jobsEmpty = document.getElementById("esealDetailJobsEmpty");
                if (itemsEmpty) {
                    itemsEmpty.style.display = "none";
                }
                if (jobsEmpty) {
                    jobsEmpty.style.display = "none";
                }
            }

            function showDetailError(message) {
                var error = document.getElementById("esealDetailError");
                var content = document.getElementById("esealDetailContent");
                if (content) {
                    content.style.display = "none";
                }
                if (error) {
                    error.style.display = "block";
                    error.textContent = message || "Gagal memuat detail sewa. Silakan coba lagi.";
                }
            }

            function renderItems(items) {
                var body = document.getElementById("esealDetailItemsBody");
                var empty = document.getElementById("esealDetailItemsEmpty");
                var table = body ? body.parentNode : null;
                var tableWrap = table ? table.parentNode : null;
                if (!body || !empty) {
                    return;
                }

                body.innerHTML = "";
                if (!items || !items.length) {
                    empty.style.display = "block";
                    if (tableWrap && tableWrap.style) {
                        tableWrap.style.display = "none";
                    }
                    return;
                }

                empty.style.display = "none";
                if (tableWrap && tableWrap.style) {
                    tableWrap.style.display = "";
                }

                var html = "";
                for (var i = 0; i < items.length; i++) {
                    var item = items[i] || {};
                    html += "<tr>"
                        + "<td>" + (i + 1) + "</td>"
                        + "<td>" + escapeHtml(formatNumber(item.Quantity)) + "</td>"
                        + "<td>" + escapeHtml(formatNumber(item.QuantityDone)) + "</td>"
                        + "<td>" + escapeHtml(formatRp(item.Price)) + "</td>"
                        + "<td>" + escapeHtml(formatRp(item.InstallFee)) + "</td>"
                        + "<td>" + escapeHtml(formatRp(item.MonthlyFee)) + "</td>"
                        + "<td>" + escapeHtml(formatRp(item.SubTotalPrice)) + "</td>"
                        + "<td>" + escapeHtml(formatRp(item.TotalAmount)) + "</td>"
                        + "<td>" + escapeHtml(displayText(item.Status)) + "</td>"
                        + "</tr>";
                }
                body.innerHTML = html;
            }

            function renderJobs(jobs) {
                var body = document.getElementById("esealDetailJobsBody");
                var empty = document.getElementById("esealDetailJobsEmpty");
                var table = body ? body.parentNode : null;
                var tableWrap = table ? table.parentNode : null;
                if (!body || !empty) {
                    return;
                }

                body.innerHTML = "";
                if (!jobs || !jobs.length) {
                    empty.style.display = "block";
                    if (tableWrap && tableWrap.style) {
                        tableWrap.style.display = "none";
                    }
                    return;
                }

                empty.style.display = "none";
                if (tableWrap && tableWrap.style) {
                    tableWrap.style.display = "";
                }

                var html = "";
                for (var i = 0; i < jobs.length; i++) {
                    var job = jobs[i] || {};
                    html += "<tr>"
                        + "<td>" + (i + 1) + "</td>"
                        + "<td>" + escapeHtml(displayText(job.JobID)) + "</td>"
                        + "<td>" + escapeHtml(displayText(job.Status)) + "</td>"
                        + "<td>" + escapeHtml(displayText(job.JobStatusDescription)) + "</td>"
                        + "<td>" + jobEvidenceBadgeHtml(job.IsEvidenceComplete) + "</td>"
                        + "</tr>";
                }
                body.innerHTML = html;
            }

            function renderDetail(data) {
                var header = data && data.header ? data.header : null;
                if (!header) {
                    showDetailError((data && data.message) || "Data sewa tidak ditemukan.");
                    return;
                }

                var content = document.getElementById("esealDetailContent");
                var error = document.getElementById("esealDetailError");
                if (error) {
                    error.style.display = "none";
                    error.textContent = "";
                }
                if (content) {
                    content.style.display = "block";
                }

                setText("esealDetailPoNumber", displayText(header.PONumber));
                setText("esealHdrPoID", header.PoID);
                setText("esealHdrPONumber", header.PONumber);
                setText("esealHdrOrderDate", header.OrderDate);
                setText("esealHdrRentalType", header.RentalType);
                setText("esealHdrExpiredDate", header.ExpiredDate);
                setText("esealHdrPoStatus", header.PoStatus);
                setText("esealHdrCustomerName", header.CustomerName);
                setText("esealHdrPicName", header.PicName);
                setText("esealHdrPicMobilePhone", header.PicMobilePhone);
                setText("esealHdrOfficePhone", header.OfficePhone);
                setText("esealHdrTotalItem", formatNumber(header.TotalItem));
                setText("esealHdrTotalUnit", formatNumber(header.TotalUnit));
                setText("esealHdrTotalQuantityDone", formatNumber(header.TotalQuantityDone));
                setText("esealHdrTotalJob", formatNumber(header.TotalJob));
                setText("esealHdrTotalClosedJob", formatNumber(header.TotalClosedJob));
                setText("esealHdrTotalPendingJob", formatNumber(header.TotalPendingJob));
                setHtml("esealHdrEvidenceStatus", evidenceBadgeHtml(header.EvidenceStatusCode));
                setText("esealHdrTotalPrice", formatRp(header.TotalPrice));
                setText("esealHdrTotalInstallFee", formatRp(header.TotalInstallFee));
                setText("esealHdrTotalMonthlyFee", formatRp(header.TotalMonthlyFee));
                setText("esealHdrGrandTotal", formatRp(header.GrandTotal));

                renderItems(data.items || []);
                renderJobs(data.jobs || []);
            }

            function openEsealRentDetail(poId) {
                var normalizedPoId = String(poId || "").trim();
                if (!normalizedPoId) {
                    alert("PoID tidak valid.");
                    return;
                }

                // Cegah request ganda untuk PoID yang sama saat masih loading.
                if (detailState.loading && detailState.currentPoId === normalizedPoId) {
                    return;
                }

                detailState.loading = true;
                detailState.currentPoId = normalizedPoId;
                var requestSeq = ++detailState.requestSeq;

                clearDetailModal();
                showDetailLoading(true);

                if (typeof $ !== "undefined" && $("#esealRentDetailModal").modal) {
                    $("#esealRentDetailModal").modal("show");
                }

                $.ajax({
                    type: "POST",
                    url: "dashboard_eseal_rent.aspx/GetEsealRentDetail",
                    data: JSON.stringify({ poId: normalizedPoId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (requestSeq !== detailState.requestSeq) {
                            return;
                        }

                        showDetailLoading(false);
                        var data = response && response.d ? response.d : null;
                        if (!data || !data.success) {
                            clearDetailModal();
                            showDetailError((data && data.message) || "Data sewa tidak ditemukan.");
                            return;
                        }

                        renderDetail(data);
                    },
                    error: function () {
                        if (requestSeq !== detailState.requestSeq) {
                            return;
                        }
                        showDetailLoading(false);
                        clearDetailModal();
                        showDetailError("Gagal memuat detail sewa. Silakan coba lagi.");
                    },
                    complete: function () {
                        if (requestSeq === detailState.requestSeq) {
                            detailState.loading = false;
                            detailState.currentPoId = "";
                            var overlay = document.getElementById("esealRentDetailOverlay");
                            if (overlay) {
                                overlay.style.display = "none";
                            }
                        }
                    }
                });
            }

            function bindEsealDetailEvents() {
                if (detailState.bound) {
                    return;
                }
                detailState.bound = true;

                document.addEventListener("click", function (event) {
                    var target = event.target;
                    if (!target) {
                        return;
                    }

                    var button = target.closest ? target.closest(".eseal-btn-detail") : null;
                    if (!button && target.className && String(target.className).indexOf("eseal-btn-detail") >= 0) {
                        button = target;
                    }
                    if (!button) {
                        return;
                    }

                    event.preventDefault();
                    openEsealRentDetail(button.getAttribute("data-po-id"));
                });
            }

            if (document.readyState === "loading") {
                document.addEventListener("DOMContentLoaded", bindEsealDetailEvents);
            } else {
                bindEsealDetailEvents();
            }

            if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    bindEsealDetailEvents();
                });
            }

            window.openEsealRentDetail = openEsealRentDetail;
        })();
    </script>
</asp:Content>
