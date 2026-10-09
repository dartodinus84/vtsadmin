<%@ Page Title=".:: EasyGo ::. Summary Stok Teknisi"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="dashboard_stok_teknisi.aspx.cs"
    Inherits="vtsadm.dashboard_stok_teknisi"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        :root {
            --stok-primary: #2f80b8;
            --stok-primary-dark: #246a99;
            --stok-accent: #f39c12;
            --stok-border: #e6ecf1;
            --stok-bg: #f5f8fb;
            --stok-text: #2c3e50;
            --stok-muted: #7f8c9a;
        }

        .stok-page .content-header h1 {
            font-weight: 700;
            color: var(--stok-text);
        }

        .stok-page .content-header h1 small {
            display: block;
            margin-top: 4px;
            font-size: 13px;
            font-weight: 400;
            color: var(--stok-muted);
        }

        .stok-tip {
            background: #eef6fc;
            border: 1px solid #d5e8f6;
            border-left: 4px solid var(--stok-primary);
            border-radius: 4px;
            padding: 10px 14px;
            margin-bottom: 15px;
            color: #35566d;
            font-size: 13px;
        }

        .stok-tip i {
            margin-right: 6px;
            color: var(--stok-primary);
        }

        .stok-box {
            border: 1px solid var(--stok-border);
            border-radius: 6px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
            overflow: hidden;
        }

        .stok-box > .box-header {
            background: linear-gradient(180deg, #ffffff 0%, #f8fbfd 100%);
            border-bottom: 1px solid var(--stok-border);
            padding: 12px 15px;
        }

        .stok-box > .box-header .box-title {
            font-size: 16px;
            font-weight: 700;
            color: var(--stok-text);
        }

        .stok-box > .box-header .box-title .fa {
            margin-right: 6px;
            color: var(--stok-primary);
        }

        .stok-box-help {
            display: block;
            margin-top: 3px;
            font-size: 12px;
            font-weight: 400;
            color: var(--stok-muted);
        }

        .stok-box > .box-body {
            background: #fff;
        }

        .stok-table > thead > tr > th {
            background: #f7fafc;
            color: #4a5d6d;
            border-bottom: 1px solid var(--stok-border) !important;
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 0.02em;
            white-space: nowrap;
        }

        .stok-table > tbody > tr > td {
            vertical-align: middle !important;
        }

        .row-highlight-orange {
            background-color: #fff4e0 !important;
            font-weight: 700;
        }

        .row-highlight-orange td {
            border-color: #f5c97a !important;
        }

        .filter-panel {
            background: var(--stok-bg);
            border: 1px solid var(--stok-border);
            border-radius: 4px;
            padding: 12px 12px 4px;
            margin-bottom: 12px;
        }

        .filter-panel .form-group {
            margin-bottom: 10px;
        }

        .filter-panel label {
            font-weight: 600;
            margin-bottom: 4px;
            display: block;
            color: #4a5d6d;
            font-size: 12px;
        }

        .box-filter-actions {
            margin: 0 0 12px 0;
        }

        .box-filter-actions .btn {
            margin-right: 6px;
            margin-bottom: 6px;
            min-width: 110px;
        }

        .table-scroll-detail {
            max-height: 420px;
            overflow: auto;
            border: 1px solid var(--stok-border);
            border-radius: 4px;
            background: #fff;
        }

        .table-scroll-detail table {
            margin-bottom: 0;
        }

        .table-scroll-detail thead th {
            position: sticky;
            top: 0;
            background: #f7fafc;
            z-index: 2;
            border-bottom: 2px solid #dbe4ec;
        }

        .stok-total-row td {
            background: #eef6fc !important;
            border-top: 2px solid #c9e2f4 !important;
            font-weight: 700;
        }

        a.qty-link {
            display: inline-block;
            min-width: 42px;
            padding: 3px 10px;
            border-radius: 14px;
            background: #e8f3fb;
            color: var(--stok-primary-dark) !important;
            font-weight: 700;
            text-decoration: none !important;
            border: 1px solid #c9e2f4;
            cursor: pointer;
            transition: background 0.15s ease, color 0.15s ease;
        }

        a.qty-link:hover,
        a.qty-link:focus {
            background: var(--stok-primary);
            color: #fff !important;
            border-color: var(--stok-primary);
        }

        .qty-plain {
            color: #555;
            font-weight: 600;
        }

        #stokUnitModalFixed .modal-content {
            border-radius: 6px;
            border: none;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
        }

        #stokUnitModalFixed .modal-header {
            background: linear-gradient(180deg, #ffffff 0%, #f5f9fc 100%);
            border-bottom: 1px solid var(--stok-border);
        }

        #stokUnitModalFixed .modal-title {
            font-weight: 700;
            color: var(--stok-text);
        }

        #stokUnitModalFixed .js-stok-subtitle {
            margin-bottom: 10px !important;
            color: var(--stok-muted);
            font-size: 13px;
        }

        #stokUnitModalTemplate {
            display: none !important;
        }

        .stok-unit-actions {
            margin-bottom: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .stok-unit-scroll {
            max-height: 420px;
            overflow: auto;
            border: 1px solid var(--stok-border);
            border-radius: 4px;
        }

        .stok-unit-scroll thead th {
            position: sticky;
            top: 0;
            background: #f7fafc;
            z-index: 2;
        }

        .stok-unit-pager {
            margin-top: 12px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 8px;
            padding-top: 8px;
            border-top: 1px solid var(--stok-border);
        }

        .stok-unit-pager .btn[disabled] {
            opacity: 0.5;
            pointer-events: none;
        }

        .stok-unit-page-numbers {
            display: inline-flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 4px;
            margin: 0 6px;
        }

        .stok-unit-page-numbers .btn-page {
            min-width: 34px;
            padding: 3px 8px;
        }

        .stok-unit-page-numbers .btn-page.active {
            background-color: var(--stok-primary);
            border-color: var(--stok-primary-dark);
            color: #fff;
            font-weight: 700;
        }

        .stok-unit-page-numbers .page-ellipsis {
            padding: 0 4px;
            color: #777;
        }

        .stok-teknisi-loader,
        #stokTeknisiLoader,
        #stokTeknisiLoaderFixed {
            position: fixed !important;
            top: 0 !important;
            left: 0 !important;
            width: 100% !important;
            height: 100% !important;
            z-index: 99999 !important;
            display: none;
            background: rgba(20, 32, 44, 0.45);
        }

        .stok-teknisi-loader .stok-loader-box,
        #stokTeknisiLoader .stok-loader-box,
        #stokTeknisiLoaderFixed .stok-loader-box {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: #fff;
            border-radius: 8px;
            padding: 20px 28px;
            min-width: 240px;
            text-align: center;
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.25);
        }

        .stok-teknisi-loader .stok-loader-box .fa,
        #stokTeknisiLoader .stok-loader-box .fa,
        #stokTeknisiLoaderFixed .stok-loader-box .fa {
            font-size: 30px;
            color: #2f80b8;
            margin-bottom: 10px;
        }

        .stok-teknisi-loader .stok-loader-text,
        #stokTeknisiLoader .stok-loader-text,
        #stokTeknisiLoaderFixed .stok-loader-text {
            font-weight: 600;
            color: #2c3e50;
        }

        #stokUnitModalFixed .js-stok-loading {
            display: none;
            text-align: center;
            padding: 10px 0 12px;
            color: var(--stok-primary);
            font-weight: 600;
            background: #eef6fc;
            border-radius: 4px;
            margin-bottom: 8px;
        }

        @media (max-width: 767px) {
            .box-filter-actions .btn {
                width: 100%;
                margin-right: 0;
            }

            .stok-unit-pager {
                flex-direction: column;
                align-items: stretch;
            }
        }
    </style>

    <div id="stokTeknisiLoader" class="stok-teknisi-loader" aria-live="polite" aria-busy="true">
        <div class="stok-loader-box">
            <i class="fa fa-spinner fa-spin" aria-hidden="true"></i>
            <div class="stok-loader-text" id="stokTeknisiLoaderText">Memuat data...</div>
        </div>
    </div>
    <script type="text/javascript">
        // Stub awal — diganti implementasi penuh di bawah halaman
        if (typeof window.showStokTeknisiLoader !== 'function') {
            window.showStokTeknisiLoader = function (message) {
                try {
                    var loader = document.getElementById('stokTeknisiLoaderFixed');
                    if (!loader) {
                        loader = document.createElement('div');
                        loader.id = 'stokTeknisiLoaderFixed';
                        loader.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;z-index:99999;display:none;background:rgba(20,32,44,0.45);';
                        loader.innerHTML = '<div style="position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);background:#fff;border-radius:8px;padding:20px 28px;min-width:240px;text-align:center;box-shadow:0 8px 24px rgba(0,0,0,0.25);"><i class="fa fa-spinner fa-spin" style="font-size:30px;color:#2f80b8;margin-bottom:10px;display:block;"></i><div class="stok-loader-text" style="font-weight:600;color:#2c3e50;">Memuat data...</div></div>';
                        document.body.appendChild(loader);
                    }
                    var text = loader.querySelector('.stok-loader-text');
                    if (text) text.textContent = message || 'Memuat data...';
                    loader.style.display = 'block';
                } catch (e) { }
                return true;
            };
            window.hideStokTeknisiLoader = function () {
                try {
                    var loader = document.getElementById('stokTeknisiLoaderFixed');
                    if (loader) loader.style.display = 'none';
                } catch (e) { }
            };
        }
    </script>

    <div class="stok-page">
        <section class="content-header">
            <h1>
                Summary Stok Teknisi
                <small>Ringkasan stok device GPS &amp; aksesoris di teknisi / kantor</small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Dashboard</a></li>
                <li class="active">Summary Stok Teknisi</li>
            </ol>
        </section>

        <section class="content">
            <% if (!string.IsNullOrEmpty(ErrorMessage)) { %>
            <div class="row">
                <div class="col-md-12">
                    <div class="alert alert-danger">
                        <strong><i class="fa fa-exclamation-triangle"></i> Gagal memuat data.</strong>
                        <br />
                        <%= ErrorMessage %>
                    </div>
                </div>
            </div>
            <% } %>

            <div class="stok-tip">
                <i class="fa fa-lightbulb-o"></i>
                <strong>Cara pakai:</strong>
                lihat ringkasan di atas → filter detail di bawah →
                <strong>klik angka Qty biru</strong> untuk membuka daftar unit (DeviceID / NoSN).
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="box box-solid stok-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-mobile"></i> Ringkasan Device (GPS)
                                <span class="stok-box-help">Klik Qty untuk melihat daftar unit per status</span>
                            </h3>
                        </div>
                        <div class="box-body table-responsive no-padding">
                            <table class="table table-bordered table-striped table-hover stok-table">
                                <thead>
                                    <tr>
                                        <th>Status</th>
                                        <th class="text-right">Qty</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= DeviceSummaryRowsHtml %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box box-solid stok-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-puzzle-piece"></i> Ringkasan Aksesoris
                                <span class="stok-box-help">Klik Qty (Stok Tersedia / Terpakai / Sisa) untuk detail unit</span>
                            </h3>
                        </div>
                        <div class="box-body table-responsive no-padding">
                            <table class="table table-bordered table-striped table-hover stok-table">
                                <thead>
                                    <tr>
                                        <th>Indikator</th>
                                        <th class="text-right">Qty</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= AksesorisSummaryRowsHtml %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="box box-solid stok-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-wrench"></i> Detail Stok Alat per Teknisi
                                <span class="stok-box-help">Pilih Branch dulu (mengisi daftar Teknisi), lalu filter & Tampilkan. Klik Qty / TOTAL untuk detail unit.</span>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="filter-panel">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group form-group-sm">
                                            <label>Branch</label>
                                            <asp:DropDownList ID="ddlBranchAlat" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlBranchAlat_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group form-group-sm">
                                            <label>Teknisi</label>
                                            <asp:DropDownList ID="ddlTeknisi" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group form-group-sm">
                                            <label>Type Alat</label>
                                            <asp:DropDownList ID="ddlTypeAlat" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group form-group-sm">
                                            <label>Status Stok</label>
                                            <asp:DropDownList ID="ddlStatusBucket" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group form-group-sm">
                                            <label>Status Device</label>
                                            <asp:DropDownList ID="ddlStatusDevice" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-filter-actions">
                                <asp:Button ID="btnTampilkanAlat" runat="server" CssClass="btn btn-primary btn-sm" Text="Tampilkan" OnClick="btnTampilkanAlat_Click" OnClientClick="showStokTeknisiLoader('Memuat filter stok alat...'); return true;" />
                                <asp:Button ID="btnExportAlatXls" runat="server" CssClass="btn btn-success btn-sm" Text="Export XLS" OnClick="btnExportAlatXls_Click" OnClientClick="showStokTeknisiLoader('Menyiapkan export XLS...'); setTimeout(function(){ hideStokTeknisiLoader(); }, 2500); return true;" />
                            </div>
                            <div class="table-responsive table-scroll-detail">
                                <table class="table table-bordered table-striped table-hover stok-table">
                                    <thead>
                                        <tr>
                                            <th>Teknisi</th>
                                            <th>Type Alat</th>
                                            <th>Status</th>
                                            <th>Status Device</th>
                                            <th class="text-right">Qty</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%= DeviceDetailRowsHtml %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box box-solid stok-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <i class="fa fa-cubes"></i> Detail Stok Aksesoris per Teknisi
                                <span class="stok-box-help">Pilih Branch dulu, lalu Teknisi (atau SEMUA). Klik Masuk / Terpakai / Sisa untuk detail unit.</span>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="filter-panel">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group form-group-sm">
                                            <label>Branch</label>
                                            <asp:DropDownList ID="ddlBranchAksesoris" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlBranchAksesoris_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group form-group-sm">
                                            <label>Teknisi Request</label>
                                            <asp:DropDownList ID="ddlTeknisiAksesoris" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-filter-actions">
                                <asp:Button ID="btnTampilAksesoris" runat="server" CssClass="btn btn-primary btn-sm" Text="Tampilkan" OnClick="btnTampilAksesoris_Click" OnClientClick="showStokTeknisiLoader('Memuat filter stok aksesoris...'); return true;" />
                                <asp:Button ID="btnTampilSisaAksesoris" runat="server" CssClass="btn btn-warning btn-sm" Text="Tampilkan Sisa Stok" OnClick="btnTampilSisaAksesoris_Click" OnClientClick="showStokTeknisiLoader('Memuat sisa stok aksesoris...'); return true;" ToolTip="Hanya baris dengan Sisa &gt; 0" />
                                <asp:Button ID="btnExportAksesorisXls" runat="server" CssClass="btn btn-success btn-sm" Text="Export XLS" OnClick="btnExportAksesorisXls_Click" OnClientClick="showStokTeknisiLoader('Menyiapkan export XLS...'); setTimeout(function(){ hideStokTeknisiLoader(); }, 2500); return true;" />
                            </div>
                            <div class="table-responsive table-scroll-detail">
                                <table class="table table-bordered table-striped table-hover stok-table">
                                    <thead>
                                        <tr>
                                            <th>Teknisi Request</th>
                                            <th>Aksesoris</th>
                                            <th class="text-right">Qty Cut Off</th>
                                            <th class="text-right">Masuk</th>
                                            <th class="text-right">Terpakai</th>
                                            <th class="text-right">Kembali</th>
                                            <th class="text-right">Sisa</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%= AksesorisDetailRowsHtml %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <!-- Template modal (akan di-clone ke body sekali, di luar UpdatePanel) -->
    <div class="modal fade" id="stokUnitModalTemplate" tabindex="-1" role="dialog" aria-hidden="true" style="display:none;">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title js-stok-title"><i class="fa fa-list-alt"></i> Detail Unit</h4>
                </div>
                <div class="modal-body">
                    <p class="text-muted js-stok-subtitle"></p>
                    <div class="stok-unit-actions">
                        <span class="text-muted" style="font-size:12px;">
                            <i class="fa fa-info-circle"></i> Status = bucket UI &nbsp;|&nbsp; Status Device = kode <code>mst_device</code>
                        </span>
                        <button type="button" class="btn btn-success btn-sm js-stok-export">
                            <i class="fa fa-file-excel-o"></i> Export XLS
                        </button>
                    </div>
                    <div class="js-stok-loading" style="display:none;text-align:center;padding:10px 0 12px;color:#2f80b8;font-weight:600;background:#eef6fc;border-radius:4px;margin-bottom:8px;">
                        <i class="fa fa-spinner fa-spin" aria-hidden="true"></i> Memuat detail unit...
                    </div>
                    <div class="alert alert-danger js-stok-error" style="display: none;"></div>
                    <div class="table-responsive stok-unit-scroll">
                        <table class="table table-bordered table-striped table-condensed table-hover stok-table">
                            <thead>
                                <tr>
                                    <th style="width: 40px;">#</th>
                                    <th>DeviceID</th>
                                    <th>NoSN</th>
                                    <th>Teknisi</th>
                                    <th>Type</th>
                                    <th>Status</th>
                                    <th>Status Device</th>
                                </tr>
                            </thead>
                            <tbody class="js-stok-body">
                                <tr><td colspan="7" class="text-center text-muted">Memuat...</td></tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="stok-unit-pager">
                        <div class="text-muted js-stok-pager-info">-</div>
                        <div style="display:flex;align-items:center;flex-wrap:wrap;">
                            <button type="button" class="btn btn-default btn-sm js-stok-prev"><i class="fa fa-angle-left"></i> Prev</button>
                            <div class="stok-unit-page-numbers js-stok-pages"></div>
                            <button type="button" class="btn btn-default btn-sm js-stok-next">Next <i class="fa fa-angle-right"></i></button>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        window.showStokTeknisiLoader = function (message) {
            try {
                var loader = document.getElementById('stokTeknisiLoaderFixed');
                if (!loader) {
                    loader = document.createElement('div');
                    loader.id = 'stokTeknisiLoaderFixed';
                    loader.className = 'stok-teknisi-loader';
                    loader.setAttribute('aria-live', 'polite');
                    // Inline style supaya tidak tergantung CSS di dalam UpdatePanel
                    loader.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;z-index:99999;display:none;background:rgba(20,32,44,0.45);';
                    loader.innerHTML =
                        '<div style="position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);background:#fff;border-radius:8px;padding:20px 28px;min-width:240px;text-align:center;box-shadow:0 8px 24px rgba(0,0,0,0.25);">' +
                        '<i class="fa fa-spinner fa-spin" style="font-size:30px;color:#2f80b8;margin-bottom:10px;display:block;"></i>' +
                        '<div class="stok-loader-text" style="font-weight:600;color:#2c3e50;">Memuat data...</div>' +
                        '</div>';
                    document.body.appendChild(loader);
                }

                var text = loader.querySelector('.stok-loader-text');
                if (text) {
                    text.textContent = message || 'Memuat data...';
                }

                loader.style.display = 'block';
                loader.style.visibility = 'visible';
                loader.style.opacity = '1';
            } catch (e) { }
            return true;
        };

        window.hideStokTeknisiLoader = function () {
            try {
                var loader = document.getElementById('stokTeknisiLoaderFixed') ||
                    document.getElementById('stokTeknisiLoader');
                if (loader) {
                    loader.style.display = 'none';
                }
            } catch (e) { }
        };

        (function bindStokTeknisiUpdatePanelLoader() {
            var tries = 0;
            function attach() {
                tries += 1;
                if (typeof Sys === 'undefined' || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
                    if (tries < 40) {
                        window.setTimeout(attach, 100);
                    }
                    return;
                }
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (!prm || window.__stokTeknisiPrmLoaderBound) {
                    return;
                }
                window.__stokTeknisiPrmLoaderBound = true;
                prm.add_beginRequest(function () {
                    window.showStokTeknisiLoader('Memuat data...');
                });
                prm.add_endRequest(function () {
                    window.hideStokTeknisiLoader();
                    try { $('#overlay').hide(); } catch (e) { }
                });
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', attach);
            } else {
                attach();
            }
        })();

        (function () {
            if (window.__stokTeknisiQtyDrilldownBound) {
                return;
            }
            window.__stokTeknisiQtyDrilldownBound = true;

            var state = {
                panel: '',
                technicianId: '',
                deviceTypeId: '',
                statusBucket: '',
                metric: '',
                deviceStatus: '',
                branchId: '',
                title: '',
                page: 1,
                pageSize: 10,
                totalCount: 0,
                loading: false,
                xhr: null
            };

            function escapeHtml(value) {
                return String(value == null ? '' : value)
                    .replace(/&/g, '&amp;')
                    .replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;')
                    .replace(/"/g, '&quot;')
                    .replace(/'/g, '&#39;');
            }

            function apiUrl(actionName) {
                // Bridge ?action= di OnPreInit (bukan /WebMethod) agar tidak macet di FriendlyUrls/UpdatePanel
                var path = window.location.pathname || '';
                var slash = path.lastIndexOf('/');
                var base = (slash >= 0 ? path.substring(0, slash + 1) : '') + 'dashboard_stok_teknisi.aspx';
                return base + '?action=' + encodeURIComponent(actionName || 'unit_detail');
            }

            function clearPageLoaders() {
                if (typeof hideStokTeknisiLoader === 'function') {
                    hideStokTeknisiLoader();
                }
                try {
                    var fixed = document.getElementById('stokTeknisiLoaderFixed');
                    if (fixed) fixed.style.display = 'none';
                    var legacy = document.getElementById('stokTeknisiLoader');
                    if (legacy) legacy.style.display = 'none';
                    $('#overlay').hide();
                } catch (e) { }
            }

            function ensureFixedModal() {
                var existing = document.getElementById('stokUnitModalFixed');
                if (existing) {
                    return existing;
                }

                var tpl = document.getElementById('stokUnitModalTemplate');
                if (!tpl) {
                    return null;
                }

                var clone = tpl.cloneNode(true);
                clone.id = 'stokUnitModalFixed';
                clone.style.display = '';
                clone.removeAttribute('aria-hidden');
                document.body.appendChild(clone);

                // Hapus template di dalam panel supaya tidak bentrok setelah postback
                tpl.parentNode && tpl.parentNode.removeChild(tpl);

                return clone;
            }

            function $modal() {
                ensureFixedModal();
                return $('#stokUnitModalFixed');
            }

            function setModalLoading(isLoading) {
                var el = $modal().find('.js-stok-loading')[0];
                if (el) {
                    el.style.display = isLoading ? 'block' : 'none';
                }
            }

            function setError(message) {
                var el = $modal().find('.js-stok-error')[0];
                if (!el) return;
                if (message) {
                    el.style.display = '';
                    el.textContent = message;
                } else {
                    el.style.display = 'none';
                    el.textContent = '';
                }
            }

            function renderPageNumbers(totalPages) {
                var container = $modal().find('.js-stok-pages')[0];
                if (!container) return;

                if (!state.totalCount || totalPages < 1) {
                    container.innerHTML = '';
                    return;
                }

                var windowSize = 5;
                var start = Math.max(1, state.page - 2);
                var end = Math.min(totalPages, start + windowSize - 1);
                start = Math.max(1, end - windowSize + 1);

                var html = '';
                function pageBtn(page, active) {
                    return '<button type="button" class="btn btn-default btn-sm btn-page' +
                        (active ? ' active' : '') +
                        '" data-page="' + page + '"' +
                        (state.loading || active ? ' disabled' : '') +
                        '>' + page + '</button>';
                }

                if (start > 1) {
                    html += pageBtn(1, state.page === 1);
                    if (start > 2) {
                        html += '<span class="page-ellipsis">...</span>';
                    }
                }

                for (var p = start; p <= end; p++) {
                    html += pageBtn(p, p === state.page);
                }

                if (end < totalPages) {
                    if (end < totalPages - 1) {
                        html += '<span class="page-ellipsis">...</span>';
                    }
                    html += pageBtn(totalPages, state.page === totalPages);
                }

                container.innerHTML = html;
            }

            function updatePager() {
                var totalPages = Math.max(1, Math.ceil((state.totalCount || 0) / state.pageSize));
                var info = $modal().find('.js-stok-pager-info')[0];
                if (info) {
                    info.textContent = 'Halaman ' + state.page + ' / ' + totalPages +
                        ' | Total ' + (state.totalCount || 0).toLocaleString('id-ID') + ' unit';
                }
                var btnPrev = $modal().find('.js-stok-prev')[0];
                var btnNext = $modal().find('.js-stok-next')[0];
                if (btnPrev) btnPrev.disabled = state.page <= 1 || state.loading;
                if (btnNext) btnNext.disabled = state.page >= totalPages || state.loading || state.totalCount === 0;
                renderPageNumbers(totalPages);
            }

            function downloadHtmlAsXls(html, fileName) {
                if (!html) {
                    setError('Konten export kosong.');
                    return false;
                }
                try {
                    var blob = new Blob(['\ufeff' + html], { type: 'application/vnd.ms-excel' });
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = fileName || ('detail_unit_' + Date.now() + '.xls');
                    a.style.display = 'none';
                    document.body.appendChild(a);
                    a.click();
                    window.setTimeout(function () {
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);
                    }, 500);
                    return true;
                } catch (e) {
                    setError('Browser menolak download file. Coba browser lain / izinkan download.');
                    return false;
                }
            }

            function parseAjaxError(xhr, fallback) {
                var msg = fallback || 'Terjadi kesalahan.';
                try {
                    if (xhr && xhr.responseJSON && xhr.responseJSON.Message) {
                        return xhr.responseJSON.Message;
                    }
                    if (xhr && xhr.responseText) {
                        var parsed = JSON.parse(xhr.responseText);
                        if (parsed && parsed.Message) return parsed.Message;
                        if (parsed && parsed.d && parsed.d.message) return parsed.d.message;
                    }
                } catch (e) { }
                if (xhr && (xhr.statusText === 'timeout' || xhr.status === 0)) {
                    return 'Request timeout / terputus. Coba lagi.';
                }
                return msg;
            }

            function exportDetailXls() {
                if (!state.panel) {
                    setError('Tidak ada data detail untuk di-export.');
                    return;
                }

                // Jangan blokir export meski list masih loading
                setError('');
                setModalLoading(true);
                clearPageLoaders();

                var payload = {
                    panel: state.panel,
                    technicianId: state.technicianId || 'ALL',
                    deviceTypeId: state.deviceTypeId || 'ALL',
                    statusBucket: state.statusBucket || '',
                    metric: state.metric || '',
                    deviceStatus: state.deviceStatus || 'ALL',
                    branchId: state.branchId || 'ALL'
                };

                var requestUrl = apiUrl('unit_export');
                $.ajax({
                    type: 'POST',
                    url: requestUrl,
                    data: JSON.stringify(payload),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'text',
                    cache: false,
                    timeout: 120000,
                    success: function (raw) {
                        setModalLoading(false);
                        clearPageLoaders();
                        var data = null;
                        try {
                            var text = String(raw || '').replace(/^\uFEFF/, '').trim();
                            if (!text || text.charAt(0) === '<') {
                                throw new Error('Response export bukan JSON. Refresh halaman lalu coba lagi.');
                            }
                            var response = JSON.parse(text);
                            data = response && typeof response.d !== 'undefined' ? response.d : response;
                        } catch (parseErr) {
                            setError((parseErr && parseErr.message) || 'Gagal parse response export.');
                            return;
                        }
                        if (!data || !data.success) {
                            setError((data && data.message) || 'Gagal export detail unit.');
                            return;
                        }
                        if (!downloadHtmlAsXls(data.html || '', data.fileName || 'detail_unit.xls')) {
                            return;
                        }
                        if (data.truncated) {
                            setError(data.message || 'Export berhasil (data terpotong batas maksimum).');
                        }
                    },
                    error: function (xhr) {
                        setModalLoading(false);
                        clearPageLoaders();
                        setError(parseAjaxError(xhr, 'Gagal export detail unit.'));
                    }
                });
            }

            function renderRows(rows) {
                var body = $modal().find('.js-stok-body')[0];
                if (!body) return;
                if (!rows || !rows.length) {
                    body.innerHTML = '<tr><td colspan="7" class="text-center text-muted">Tidak ada data unit.</td></tr>';
                    return;
                }
                var start = ((state.page - 1) * state.pageSize) + 1;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    var r = rows[i] || {};
                    html += '<tr>';
                    html += '<td>' + (start + i) + '</td>';
                    html += '<td>' + escapeHtml(r.DeviceID || '-') + '</td>';
                    html += '<td>' + escapeHtml(r.NoSN || '-') + '</td>';
                    html += '<td>' + escapeHtml(r.Teknisi || '-') + '</td>';
                    html += '<td>' + escapeHtml(r.TypeAlat || '-') + '</td>';
                    html += '<td>' + escapeHtml(r.Status || '-') + '</td>';
                    html += '<td><span class="label label-default">' + escapeHtml(r.DeviceStatus || '-') + '</span></td>';
                    html += '</tr>';
                }
                body.innerHTML = html;
            }

            function showModal() {
                var $m = $modal();
                if (!$m.length) return;
                if ($m.parent()[0] !== document.body) {
                    $m.appendTo('body');
                }
                if ($m.modal) {
                    $m.modal('show');
                } else {
                    $m.show();
                }
            }

            function finishLoad() {
                state.loading = false;
                state.xhr = null;
                setModalLoading(false);
                updatePager();
            }

            function loadPage(page) {
                if (state.xhr) {
                    try { state.xhr.abort(); } catch (e) { }
                    state.xhr = null;
                }

                state.loading = true;
                state.page = page < 1 ? 1 : page;
                setError('');
                setModalLoading(true);
                var body = $modal().find('.js-stok-body')[0];
                if (body) {
                    body.innerHTML = '<tr><td colspan="7" class="text-center text-muted">Memuat...</td></tr>';
                }
                updatePager();

                var payload = {
                    panel: state.panel,
                    technicianId: state.technicianId || 'ALL',
                    deviceTypeId: state.deviceTypeId || 'ALL',
                    statusBucket: state.statusBucket || '',
                    metric: state.metric || '',
                    deviceStatus: state.deviceStatus || 'ALL',
                    branchId: state.branchId || 'ALL',
                    pageNumber: state.page,
                    pageSize: state.pageSize
                };

                var requestUrl = apiUrl('unit_detail');
                state.xhr = $.ajax({
                    type: 'POST',
                    url: requestUrl,
                    data: JSON.stringify(payload),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'text',
                    cache: false,
                    timeout: 45000,
                    success: function (raw) {
                        clearPageLoaders();
                        finishLoad();
                        var data = null;
                        try {
                            var text = String(raw || '').replace(/^\uFEFF/, '').trim();
                            if (!text || text.charAt(0) === '<') {
                                throw new Error('Response bukan JSON. URL: ' + requestUrl);
                            }
                            var response = JSON.parse(text);
                            data = response && typeof response.d !== 'undefined' ? response.d : response;
                        } catch (parseErr) {
                            renderRows([]);
                            state.totalCount = 0;
                            updatePager();
                            setError((parseErr && parseErr.message) || 'Gagal parse response detail.');
                            return;
                        }
                        if (!data || !data.success) {
                            renderRows([]);
                            state.totalCount = 0;
                            updatePager();
                            setError((data && data.message) || 'Gagal memuat detail unit.');
                            return;
                        }
                        state.totalCount = data.totalCount || 0;
                        renderRows(data.rows || []);
                        updatePager();
                    },
                    error: function (xhr, status) {
                        if (status === 'abort') {
                            return;
                        }
                        clearPageLoaders();
                        finishLoad();
                        renderRows([]);
                        state.totalCount = 0;
                        updatePager();
                        setError(parseAjaxError(xhr, 'Gagal memuat detail unit. Silakan coba lagi.'));
                    },
                    complete: function (xhr, status) {
                        clearPageLoaders();
                        if (status !== 'abort' && state.loading) {
                            finishLoad();
                        }
                    }
                });
            }

            function openModalFromLink(link) {
                clearPageLoaders();

                state.panel = link.getAttribute('data-panel') || '';
                state.technicianId = link.getAttribute('data-tech') || 'ALL';
                state.deviceTypeId = link.getAttribute('data-type') || 'ALL';
                state.statusBucket = link.getAttribute('data-bucket') || '';
                state.metric = link.getAttribute('data-metric') || '';
                state.deviceStatus = link.getAttribute('data-devstatus') || 'ALL';
                state.branchId = link.getAttribute('data-branch') || 'ALL';
                state.title = link.getAttribute('data-title') || 'Detail Unit';
                state.page = 1;
                state.totalCount = 0;
                state.loading = false;

                var $m = $modal();
                if (!$m.length) {
                    window.alert('Modal detail tidak tersedia. Refresh halaman lalu coba lagi.');
                    return;
                }
                $m.find('.js-stok-title').html('<i class="fa fa-list-alt"></i> ' + escapeHtml(state.title));
                $m.find('.js-stok-subtitle').text(link.getAttribute('data-subtitle') || '');

                showModal();
                loadPage(1);
            }

            $(document).on('click.stokTeknisiQty', 'a.qty-link', function (e) {
                e.preventDefault();
                e.stopPropagation();
                openModalFromLink(this);
            });

            $(document).on('click.stokTeknisiQty', '#stokUnitModalFixed .js-stok-prev', function () {
                if (state.page > 1) loadPage(state.page - 1);
            });

            $(document).on('click.stokTeknisiQty', '#stokUnitModalFixed .js-stok-next', function () {
                var totalPages = Math.max(1, Math.ceil((state.totalCount || 0) / state.pageSize));
                if (state.page < totalPages) loadPage(state.page + 1);
            });

            $(document).on('click.stokTeknisiQty', '#stokUnitModalFixed .js-stok-pages .btn-page', function () {
                var page = parseInt($(this).attr('data-page'), 10);
                if (!isNaN(page) && page !== state.page) {
                    loadPage(page);
                }
            });

            $(document).on('click.stokTeknisiQty', '#stokUnitModalFixed .js-stok-export', function () {
                exportDetailXls();
            });

            // Bersihkan modal orphan dari session sebelumnya
            $(function () {
                $('#stokUnitModal').remove();
                ensureFixedModal();
            });
        })();
    </script>
</asp:Content>
