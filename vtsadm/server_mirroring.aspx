<%@ Page Title="Server Mirroring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="server_mirroring.aspx.cs" Inherits="vtsadm.server_mirroring" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&family=IBM+Plex+Mono:wght@400;500&display=swap" />
    <style>
        .sm-page { --orange:#f97316; --orange-hover:#ea6a0b; --orange-soft:#fff3ea; --orange-border:#fdba74; --ink:#1e293b; --secondary:#334155; --muted:#64748b; --subtle:#94a3b8; --border:#e2e8f0; --divider:#eef2f7; --bg:#f5f7fa; --surface:#fff; --green:#16a34a; --green-soft:#ecfdf3; --amber:#d97706; --amber-soft:#fff7e6; font:14px/1.45 Inter,"Segoe UI",system-ui,sans-serif; color:var(--ink); }
        .sm-page * { box-sizing:border-box; }
        .sm-page .mono { font-family:"IBM Plex Mono",Consolas,monospace; }
        .content-wrapper { background:var(--bg,#f5f7fa) !important; }
        .sm-page .sm-head { display:flex; align-items:flex-start; justify-content:space-between; gap:24px; margin-bottom:14px; }
        .sm-page .sm-head h1 { font-size:20px; line-height:1.2; margin:0 0 6px; letter-spacing:-.3px; color:#1e293b; }
        .sm-page .sm-head p { margin:0; color:#64748b; font-size:12.5px; }
        .sm-btn { border:1px solid #e2e8f0; background:#fff; color:#334155; height:36px; border-radius:8px; padding:0 13px; font-size:12.5px; font-weight:650; display:inline-flex; align-items:center; justify-content:center; gap:7px; cursor:pointer; }
        .sm-btn:hover { background:#f8fafc; }
        .sm-btn.primary { background:#f97316; border-color:#f97316; color:#fff; }
        .sm-btn.primary:hover { background:#ea6a0b; }
        .sm-btn svg { width:15px; height:15px; stroke:currentColor; }
        .sm-kpis { display:grid; grid-template-columns:repeat(3,minmax(0,1fr)); gap:12px; margin-bottom:13px; }
        .sm-card, .sm-panel { background:#fff; border:1px solid #e2e8f0; border-radius:10px; }
        .sm-kpi { padding:11px 14px; min-height:80px; }
        .sm-kpi-top { display:flex; justify-content:space-between; align-items:center; color:#64748b; font-size:11.5px; }
        .sm-kpi-ico { width:28px; height:28px; border-radius:7px; background:#f8fafc; display:grid; place-items:center; color:#64748b; }
        .sm-kpi-ico svg { width:15px; stroke:currentColor; fill:none; }
        .sm-kpi-val { font-size:21px; font-weight:730; line-height:1.15; margin-top:4px; letter-spacing:-.6px; }
        .sm-kpi-foot { margin-top:2px; color:#94a3b8; font-size:10.5px; }
        .sm-panel-head { display:flex; justify-content:space-between; align-items:center; padding:11px 15px; border-bottom:1px solid #eef2f7; }
        .sm-panel-title strong { display:block; font-size:12.5px; color:#1e293b; }
        .sm-panel-title small { font-size:10px; color:#64748b; }
        .sm-panel-body { padding:12px 15px 15px; }
        .sm-filter { display:grid; grid-template-columns:minmax(170px,1fr) minmax(135px,.65fr) minmax(170px,.85fr) auto auto; gap:8px; align-items:end; padding-bottom:12px; border-bottom:1px solid #eef2f7; }
        .sm-filter label { display:block; margin-bottom:5px; color:#334155; font-size:10.5px; font-weight:650; }
        .sm-select, .sm-input { width:100%; height:34px; border:1px solid #e2e8f0; border-radius:8px; background:#fff; color:#334155; padding:0 11px; font-size:12px; outline:none; }
        .sm-select:focus, .sm-input:focus { border-color:#fdba74; box-shadow:0 0 0 3px rgba(249,115,22,.09); }
        .sm-filter .sm-btn { height:34px; }
        .sm-toolbar { display:flex; align-items:center; justify-content:space-between; gap:12px; padding:11px 0 8px; }
        .sm-search { position:relative; width:320px; max-width:50%; }
        .sm-search svg { position:absolute; left:10px; top:9px; width:15px; stroke:#94a3b8; fill:none; }
        .sm-search .sm-input { height:33px; padding-left:32px; }
        .sm-tools { position:relative; }
        .sm-tools-btn { height:32px; border:0; border-radius:7px; background:#f1f5f9; color:#334155; padding:0 11px; font-size:11px; font-weight:700; cursor:pointer; display:inline-flex; align-items:center; gap:6px; }
        .sm-tools-btn svg { width:12px; height:12px; stroke:currentColor; fill:none; }
        .sm-tools-menu { display:none; position:absolute; z-index:25; right:0; top:38px; width:260px; background:#fff; border:1px solid #e2e8f0; border-radius:8px; overflow:hidden; }
        .sm-tools-menu.show { display:block; }
        .sm-tool-title { padding:10px 12px 7px; font-size:10.5px; font-weight:700; color:#334155; }
        .sm-tool-group { padding:7px 12px 6px; border-top:1px solid #eef2f7; background:#f8fafc; color:#728097; font-size:9.5px; font-weight:750; letter-spacing:.06em; }
        .sm-col-opt { display:flex; align-items:center; gap:7px; padding:5px 12px; font-size:10.5px; color:#334155; cursor:pointer; }
        .sm-col-opt:hover { background:#f8fafc; }
        .sm-col-opt input { accent-color:#f97316; }
        .sm-export { display:grid; grid-template-columns:1fr 1fr; gap:6px; padding:8px 12px 11px; }
        .sm-export-btn { height:29px; border:1px solid #e2e8f0; border-radius:7px; background:#fff; color:#334155; font-size:10.5px; font-weight:650; cursor:pointer; }
        .sm-export-btn:hover { border-color:#fdba74; color:#f97316; background:#fff3ea; }
        .sm-table-wrap { overflow:auto; border:1px solid #e2e8f0; border-radius:8px; min-height:240px; }
        .sm-page table { width:100%; border-collapse:collapse; min-width:1050px; }
        .sm-page th { background:#f8fafc; color:#64748b; font-size:10px; text-transform:uppercase; letter-spacing:.035em; text-align:left; padding:8px 10px; border-bottom:1px solid #e2e8f0; font-weight:700; }
        .sm-page td { padding:9px 10px; border-bottom:1px solid #eef2f7; font-size:11px; color:#334155; vertical-align:middle; }
        .sm-page tbody tr:last-child td { border-bottom:0; }
        .sm-page tbody tr:hover { background:#fcfcfd; }
        .sm-sort { color:#cbd5e1; font-size:10px; margin-left:3px; cursor:pointer; }
        .sm-rowno { color:#94a3b8; text-align:center; }
        .sm-server { display:inline-flex; align-items:center; gap:6px; padding:4px 7px; border-radius:6px; background:#f8fafc; border:1px solid #e2e8f0; font-size:11px; margin:2px 3px 2px 0; }
        .sm-dot { width:6px; height:6px; border-radius:50%; background:#16a34a; display:inline-block; }
        .sm-badge { display:inline-flex; align-items:center; gap:6px; padding:3px 8px; border-radius:99px; font-size:10.5px; font-weight:650; }
        .sm-badge.on { background:#ecfdf3; color:#16a34a; }
        .sm-badge.paused { background:#fff7e6; color:#d97706; }
        .sm-toggle { width:32px; height:18px; border-radius:99px; background:#cbd5e1; position:relative; border:0; cursor:pointer; }
        .sm-toggle:after { content:""; position:absolute; top:3px; left:3px; width:12px; height:12px; border-radius:50%; background:#fff; }
        .sm-toggle.on { background:#f97316; }
        .sm-toggle.on:after { left:17px; }
        .sm-edit { height:27px; padding:0 8px; border:1px solid #e2e8f0; border-radius:7px; background:#fff; color:#334155; font-size:10.5px; font-weight:650; cursor:pointer; }
        .sm-edit:hover { color:#f97316; border-color:#fdba74; background:#fff3ea; }
        .sm-pager { display:flex; align-items:center; justify-content:space-between; margin-top:12px; color:#64748b; font-size:11px; }
        .sm-pages { display:flex; gap:4px; }
        .sm-page-btn { width:28px; height:27px; border:1px solid #e2e8f0; background:#fff; border-radius:6px; color:#64748b; font-size:11px; cursor:pointer; }
        .sm-page-btn.active { border-color:#fdba74; background:#fff3ea; color:#f97316; font-weight:700; }
        .sm-overlay, .sm-overlay *, .sm-loading, .sm-loading *, .sm-toast { box-sizing:border-box; font-family:Inter,"Segoe UI",system-ui,sans-serif; }
        .sm-overlay { display:none; position:fixed; z-index:10050; left:0; top:0; right:0; bottom:0; width:100%; height:100%; background:rgba(15,23,42,.52); align-items:center; justify-content:center; padding:24px; }
        body.sm-modal-open { overflow:hidden; }
        .sm-overlay.show { display:flex !important; }
        .sm-loading { display:none; position:fixed; z-index:10100; left:0; top:0; right:0; bottom:0; width:100%; height:100%; background:rgba(15,23,42,.52); align-items:center; justify-content:center; }
        .sm-loading.show { display:flex !important; }
        .sm-loading-box { background:#fff; border:1px solid #e2e8f0; border-radius:14px; padding:22px 26px; min-width:240px; text-align:center; box-shadow:0 24px 60px rgba(15,23,42,.28); }
        .sm-spinner { width:32px; height:32px; border:3px solid #e2e8f0; border-top-color:#f97316; border-radius:50%; margin:0 auto 12px; animation:sm-spin .8s linear infinite; }
        .sm-loading-text { color:#334155; font-size:13px; font-weight:600; }
        @keyframes sm-spin { to { transform:rotate(360deg); } }
        .sm-btn[disabled] { opacity:.65; cursor:not-allowed; }
        .sm-btn.busy .sm-btn-spinner { display:inline-block; }
        .sm-btn-spinner { display:none; width:14px; height:14px; border:2px solid rgba(255,255,255,.35); border-top-color:#fff; border-radius:50%; animation:sm-spin .7s linear infinite; }
        .sm-dialog { width:640px; max-width:calc(100vw - 32px); max-height:calc(100vh - 32px); background:#fff; border-radius:16px; overflow:visible; position:relative; box-shadow:0 28px 80px rgba(15,23,42,.32); }
        .sm-dialog-head { padding:18px 22px; border-bottom:1px solid #e2e8f0; display:flex; justify-content:space-between; align-items:flex-start; gap:16px; }
        .sm-dialog-head-main { display:flex; align-items:flex-start; gap:12px; }
        .sm-dialog-ico { width:40px; height:40px; border-radius:10px; background:#fff3ea; color:#f97316; display:grid; place-items:center; flex:none; }
        .sm-dialog-ico svg { width:20px; height:20px; stroke:currentColor; fill:none; stroke-width:1.8; }
        .sm-dialog-head h2 { font-size:16px; margin:0 0 4px; font-weight:700; letter-spacing:-.2px; }
        .sm-dialog-head p { margin:0; color:#64748b; font-size:12px; line-height:1.4; }
        .sm-close { border:0; background:#f8fafc; width:32px; height:32px; border-radius:8px; color:#64748b; cursor:pointer; display:grid; place-items:center; flex:none; }
        .sm-close:hover { background:#f1f5f9; color:#1e293b; }
        .sm-close svg { width:16px; height:16px; stroke:currentColor; fill:none; stroke-width:2; }
        .sm-dialog-body { padding:16px 22px 8px; }
        .sm-fields { display:grid; grid-template-columns:1fr; gap:12px; }
        .sm-field-card { border:1px solid #e2e8f0; border-radius:12px; padding:13px 14px 12px; background:#fff; }
        .sm-field-card:focus-within { border-color:#fdba74; box-shadow:0 0 0 3px rgba(249,115,22,.08); }
        .sm-field-top { display:flex; align-items:center; gap:8px; margin-bottom:8px; }
        .sm-field-num { width:20px; height:20px; border-radius:50%; background:#fff3ea; color:#f97316; display:grid; place-items:center; font-size:10px; font-weight:700; flex:none; }
        .sm-label { display:block; color:#334155; font-size:12px; font-weight:600; margin:0; }
        .sm-req { color:#dc2626; }
        .sm-help { color:#94a3b8; font-size:11px; margin-top:6px; }
        .sm-chips { display:flex; flex-wrap:wrap; gap:6px; margin-top:8px; min-height:0; }
        .sm-chip { display:inline-flex; align-items:center; gap:5px; padding:3px 8px; border-radius:99px; background:#f8fafc; border:1px solid #e2e8f0; color:#334155; font-size:11px; }
        .sm-chip.mono { font-family:"IBM Plex Mono",Consolas,monospace; }
        .sm-multi { position:relative; }
        .sm-trigger { width:100%; min-height:42px; border:1px solid #e2e8f0; border-radius:10px; background:#fff; color:#334155; padding:8px 40px 8px 12px; text-align:left; font-size:13px; cursor:pointer; position:relative; display:flex; align-items:center; }
        .sm-trigger-text { overflow:hidden; text-overflow:ellipsis; white-space:nowrap; width:100%; }
        .sm-chevron { position:absolute; right:12px; top:50%; width:16px; height:16px; margin-top:-8px; stroke:#64748b; fill:none; stroke-width:2; pointer-events:none; }
        .sm-multi.open .sm-chevron { transform:rotate(180deg); }
        .sm-trigger:disabled { background:#f8fafc; color:#94a3b8; cursor:not-allowed; }
        .sm-multi.open .sm-trigger { border-color:#fdba74; box-shadow:0 0 0 3px rgba(249,115,22,.09); }
        .sm-menu { display:none; position:absolute; z-index:1100; left:0; right:0; top:48px; background:#fff; border:1px solid #e2e8f0; border-radius:10px; overflow:hidden; box-shadow:0 16px 40px rgba(15,23,42,.16); }
        .sm-multi.drop-up .sm-menu { top:auto; bottom:48px; }
        .sm-multi.open .sm-menu { display:block; }
        .sm-menu-search { padding:8px; border-bottom:1px solid #eef2f7; position:relative; }
        .sm-menu-search:before { content:""; position:absolute; z-index:1; left:19px; top:19px; width:11px; height:11px; border:1.7px solid #94a3b8; border-radius:50%; }
        .sm-menu-search:after { content:""; position:absolute; z-index:1; left:29px; top:30px; width:6px; height:1.7px; background:#94a3b8; transform:rotate(45deg); }
        .sm-menu-search .sm-input { padding-left:32px; background:#f8fafc; height:35px; }
        .sm-options { max-height:220px; overflow-y:auto; }
        .sm-option { display:flex; align-items:center; gap:9px; padding:9px 11px; border-bottom:1px solid #eef2f7; font-size:12px; color:#334155; cursor:pointer; }
        .sm-option:last-child { border-bottom:0; }
        .sm-option:hover { background:#f8fafc; }
        .sm-option.all { font-weight:700; background:#fff3ea; color:#f97316; }
        .sm-option input { accent-color:#f97316; }
        .sm-empty { padding:20px; text-align:center; color:#94a3b8; font-size:12px; }
        .sm-check { margin-left:auto; color:#f97316; font-weight:800; }
        .sm-dialog-foot { padding:14px 22px; border-top:1px solid #e2e8f0; display:flex; justify-content:space-between; align-items:center; gap:12px; background:#fafbfc; border-radius:0 0 16px 16px; }
        .sm-dialog-foot small { color:#64748b; font-size:11px; }
        .sm-dialog-foot .sm-btn { min-width:88px; }
        .sm-dialog-loading { display:none; position:absolute; inset:0; z-index:20; background:rgba(255,255,255,.82); border-radius:16px; align-items:center; justify-content:center; }
        .sm-dialog-loading.show { display:flex; }
        .sm-toast { position:fixed; right:24px; bottom:24px; z-index:10150; background:#1e293b; color:#fff; border-radius:10px; padding:12px 16px; font-size:13px; display:none; box-shadow:0 12px 32px rgba(15,23,42,.25); }
        .sm-toast.show { display:block; }
        @media (max-width:1100px) {
            .sm-kpis { grid-template-columns:repeat(2,1fr); }
            .sm-filter { grid-template-columns:repeat(3,1fr); }
            .sm-filter .sm-btn { width:100%; }
        }
        @media (max-width:900px) {
            .sm-dialog { width:min(760px,95vw); }
        }
    </style>

    <section class="content-header">
        <h1>System <small>Server Mirroring</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li>System</li>
            <li class="active">Server Mirroring</li>
        </ol>
    </section>

    <section class="content sm-page">
        <div class="sm-head">
            <div>
                <h1>Server Mirroring</h1>
                <p>Kelola pengiriman salinan data GPS ke server tujuan berdasarkan company atau vehicle.</p>
            </div>
            <button type="button" class="sm-btn primary" id="addBtn">
                <svg viewBox="0 0 24 24" fill="none"><path d="M12 5v14M5 12h14"/></svg>
                Tambah Pengaturan
            </button>
        </div>

        <section class="sm-kpis">
            <div class="sm-card sm-kpi">
                <div class="sm-kpi-top"><span>Company Dimirror</span><div class="sm-kpi-ico"><svg viewBox="0 0 24 24"><path d="M4 20V7l8-4 8 4v13M8 10h2m4 0h2m-8 4h2m4 0h2"/></svg></div></div>
                <div class="sm-kpi-val" id="kpiCompany">0</div>
                <div class="sm-kpi-foot" id="kpiCompanyFoot">dari 0 company aktif</div>
            </div>
            <div class="sm-card sm-kpi">
                <div class="sm-kpi-top"><span>Vehicle Dimirror</span><div class="sm-kpi-ico"><svg viewBox="0 0 24 24"><path d="M3 7h13v9H3zM16 10h3l2 3v3h-5z"/></svg></div></div>
                <div class="sm-kpi-val" id="kpiVehicle">0</div>
                <div class="sm-kpi-foot" id="kpiVehicleFoot">dari 0 vehicle aktif</div>
            </div>
            <div class="sm-card sm-kpi">
                <div class="sm-kpi-top"><span>Server Mirror Aktif</span><div class="sm-kpi-ico"><svg viewBox="0 0 24 24"><path d="M5 5h14v5H5zM5 14h14v5H5z"/></svg></div></div>
                <div class="sm-kpi-val" id="kpiServer">0</div>
                <div class="sm-kpi-foot" id="kpiServerFoot">dari 0 server tersedia</div>
            </div>
        </section>

        <section class="sm-panel">
            <div class="sm-panel-head">
                <div class="sm-panel-title">
                    <strong>Daftar Mirroring per Vehicle</strong>
                    <small>Setiap baris menampilkan vehicle, company, dan server mirror terkait.</small>
                </div>
            </div>
            <div class="sm-panel-body">
                <div class="sm-filter">
                    <div>
                        <label>Company</label>
                        <select class="sm-select" id="companyTableFilter"><option value="all">Semua company</option></select>
                    </div>
                    <div>
                        <label>Status</label>
                        <select class="sm-select" id="statusFilter">
                            <option value="all">Semua status</option>
                            <option value="active">Aktif</option>
                            <option value="paused">Dijeda</option>
                        </select>
                    </div>
                    <div>
                        <label>Server mirror</label>
                        <select class="sm-select" id="serverTableFilter"><option value="all">Semua server mirror</option></select>
                    </div>
                    <button type="button" class="sm-btn primary" id="applyFilterBtn">Apply</button>
                    <button type="button" class="sm-btn" id="clearFilterBtn">Clear</button>
                </div>
                <div class="sm-toolbar">
                    <div class="sm-search">
                        <svg viewBox="0 0 24 24"><circle cx="11" cy="11" r="7"/><path d="m20 20-4-4"/></svg>
                        <input class="sm-input" id="search" placeholder="Search vehicle, ID, company, atau server..." />
                    </div>
                    <div class="sm-tools">
                        <button type="button" class="sm-tools-btn" id="toolsBtn">Tools <svg viewBox="0 0 24 24"><path d="M6 9l6 6 6-6"/></svg></button>
                        <div class="sm-tools-menu" id="toolsMenu">
                            <div class="sm-tool-title">Columns</div>
                            <label class="sm-col-opt"><input type="checkbox" id="toggleAllColumns" checked /> <strong>Toggle All</strong></label>
                            <div class="sm-tool-group">KOLOM TABEL</div>
                            <div id="columnOptions"></div>
                            <div class="sm-tool-group">EXPORT</div>
                            <div class="sm-export">
                                <button type="button" class="sm-export-btn" data-export="CSV">CSV</button>
                                <button type="button" class="sm-export-btn" data-export="Excel">Excel</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="sm-table-wrap">
                    <table>
                        <thead id="thead"></thead>
                        <tbody id="tbody"></tbody>
                    </table>
                </div>
                <div class="sm-pager">
                    <span>Menampilkan <b id="visibleRows">0</b> dari <b id="totalRows">0</b> pengaturan</span>
                    <div class="sm-pages" id="pager"></div>
                </div>
            </div>
        </section>
    </section>

    <div class="sm-overlay" id="smModalOverlay">
        <div class="sm-dialog">
            <div class="sm-dialog-loading" id="smDialogLoading">
                <div class="sm-loading-box">
                    <div class="sm-spinner"></div>
                    <div class="sm-loading-text" id="smDialogLoadingText">Memuat...</div>
                </div>
            </div>
            <div class="sm-dialog-head">
                <div class="sm-dialog-head-main">
                    <div class="sm-dialog-ico">
                        <svg viewBox="0 0 24 24"><path d="M5 5h14v5H5zM5 14h14v5H5z"/><path d="M8 7.5h.01M8 16.5h.01"/></svg>
                    </div>
                    <div>
                        <h2 id="modalTitle">Tambah Pengaturan Mirroring</h2>
                        <p>Pilih company, vehicle yang dicakup, dan server mirror.</p>
                    </div>
                </div>
                <button type="button" class="sm-close" id="closeBtn" aria-label="Tutup">
                    <svg viewBox="0 0 24 24"><path d="M6 6l12 12M18 6L6 18"/></svg>
                </button>
            </div>
            <div class="sm-dialog-body">
                <div class="sm-fields">
                    <div class="sm-field-card">
                        <div class="sm-field-top">
                            <span class="sm-field-num">1</span>
                            <label class="sm-label">Company <span class="sm-req">*</span></label>
                        </div>
                        <div class="sm-multi" id="companyMulti">
                            <button type="button" class="sm-trigger" id="companyTrigger">
                                <span class="sm-trigger-text">Pilih company</span>
                                <svg class="sm-chevron" viewBox="0 0 24 24"><path d="M6 9l6 6 6-6"/></svg>
                            </button>
                            <div class="sm-menu">
                                <div class="sm-menu-search"><input class="sm-input" id="companySearch" placeholder="Cari company..." /></div>
                                <div class="sm-options" id="companyList"></div>
                            </div>
                        </div>
                        <div class="sm-help">Daftar vehicle akan mengikuti company yang dipilih.</div>
                    </div>
                    <div class="sm-field-card">
                        <div class="sm-field-top">
                            <span class="sm-field-num">2</span>
                            <label class="sm-label">Vehicle <span class="sm-req">*</span></label>
                        </div>
                        <div class="sm-multi" id="vehicleMulti">
                            <button type="button" class="sm-trigger" id="vehicleTrigger" disabled>
                                <span class="sm-trigger-text">Pilih company terlebih dahulu</span>
                                <svg class="sm-chevron" viewBox="0 0 24 24"><path d="M6 9l6 6 6-6"/></svg>
                            </button>
                            <div class="sm-menu">
                                <div class="sm-menu-search"><input class="sm-input" id="vehicleSearch" placeholder="Cari nomor polisi atau ID vehicle..." /></div>
                                <div class="sm-options" id="vehicleList"></div>
                            </div>
                        </div>
                        <div class="sm-chips" id="vehicleChips"></div>
                        <div class="sm-help">Pilih semua vehicle atau beberapa vehicle tertentu.</div>
                    </div>
                    <div class="sm-field-card">
                        <div class="sm-field-top">
                            <span class="sm-field-num">3</span>
                            <label class="sm-label">Server mirror <span class="sm-req">*</span></label>
                        </div>
                        <div class="sm-multi" id="serverMulti">
                            <button type="button" class="sm-trigger" id="serverTrigger">
                                <span class="sm-trigger-text">Pilih server mirror</span>
                                <svg class="sm-chevron" viewBox="0 0 24 24"><path d="M6 9l6 6 6-6"/></svg>
                            </button>
                            <div class="sm-menu">
                                <div class="sm-menu-search"><input class="sm-input" id="serverSearch" placeholder="Cari server..." /></div>
                                <div class="sm-options" id="serverList"></div>
                            </div>
                        </div>
                        <div class="sm-chips" id="serverChips"></div>
                        <div class="sm-help">Pilih satu atau beberapa server mirror.</div>
                    </div>
                </div>
            </div>
            <div class="sm-dialog-foot">
                <small>Semua field wajib diisi.</small>
                <div>
                    <button type="button" class="sm-btn" id="cancelBtn">Batal</button>
                    <button type="button" class="sm-btn primary" id="saveBtn">
                        <span class="sm-btn-spinner"></span>
                        <span class="sm-btn-label">Simpan Pengaturan</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="sm-loading" id="smLoading" aria-live="polite">
        <div class="sm-loading-box">
            <div class="sm-spinner"></div>
            <div class="sm-loading-text" id="smLoadingText">Memuat...</div>
        </div>
    </div>
    <div class="sm-toast" id="smToast">Pengaturan mirroring berhasil disimpan.</div>

    <script type="text/javascript">
        (function () {
            var servers = [];
            var companies = [];
            var rows = [];
            var vehiclesByCompany = [];
            var applied = { company: "all", status: "all", server: "all" };
            var currentPage = 1;
            var pageSize = 10;
            var sortKey = "updated";
            var sortDir = "desc";
            var editingId = null;
            var selectedCompany = "";
            var selectedVehicles = {};
            var selectedServers = {};
            var allVehicles = false;
            var loadingCount = 0;
            var saveBusy = false;
            var columnDefs = [
                { key: "rowno", label: "#" },
                { key: "vehicle", label: "Nopol" },
                { key: "vehicleId", label: "Vehicle ID" },
                { key: "company", label: "Company" },
                { key: "server", label: "Server Mirror" },
                { key: "status", label: "Status" },
                { key: "updated", label: "Terakhir Diubah" },
                { key: "active", label: "Aktif" },
                { key: "actions", label: "Aksi" }
            ];
            var visibleColumns = {};
            columnDefs.forEach(function (c) { visibleColumns[c.key] = true; });

            function fmt(n) {
                return String(n || 0).replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
            function mountOverlays() {
                ["smModalOverlay", "smLoading", "smToast"].forEach(function (id) {
                    var el = document.getElementById(id);
                    if (el && el.parentNode !== document.body) document.body.appendChild(el);
                });
            }
            function isModalOpen() {
                var m = document.getElementById("smModalOverlay");
                return m && m.classList.contains("show");
            }
            function setTriggerText(id, text) {
                var el = document.getElementById(id);
                if (!el) return;
                var span = el.querySelector(".sm-trigger-text");
                if (span) span.textContent = text;
                else el.textContent = text;
            }
            function renderChips(id, items, mono) {
                var box = document.getElementById(id);
                if (!box) return;
                if (!items || !items.length) { box.innerHTML = ""; return; }
                box.innerHTML = items.map(function (t) {
                    return '<span class="sm-chip' + (mono ? " mono" : "") + '">' + escapeHtml(t) + "</span>";
                }).join("");
            }
            function post(method, data, ok, fail, loadingMsg) {
                showLoading(loadingMsg || "Memproses...");
                $.ajax({
                    type: "POST",
                    url: "server_mirroring.aspx/" + method,
                    data: JSON.stringify(data || {}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        hideLoading();
                        var res = r && r.d ? r.d : r;
                        if (ok) ok(res);
                    },
                    error: function (xhr) {
                        hideLoading();
                        var msg = "Terjadi kesalahan.";
                        try {
                            var parsed = JSON.parse(xhr.responseText);
                            msg = (parsed && (parsed.Message || parsed.message)) || msg;
                        } catch (e) { }
                        if (fail) fail(msg); else showToast(msg);
                    }
                });
            }
            function showLoading(msg) {
                loadingCount++;
                var text = document.getElementById("smLoadingText");
                var dtext = document.getElementById("smDialogLoadingText");
                if (text) text.textContent = msg || "Memuat...";
                if (dtext) dtext.textContent = msg || "Memuat...";
                if (isModalOpen()) {
                    var dlg = document.getElementById("smDialogLoading");
                    if (dlg) dlg.classList.add("show");
                } else {
                    var box = document.getElementById("smLoading");
                    if (box) box.classList.add("show");
                }
            }
            function hideLoading() {
                loadingCount = Math.max(0, loadingCount - 1);
                if (loadingCount === 0) {
                    var box = document.getElementById("smLoading");
                    var dlg = document.getElementById("smDialogLoading");
                    if (box) box.classList.remove("show");
                    if (dlg) dlg.classList.remove("show");
                }
            }
            function setSaveBusy(on) {
                saveBusy = on;
                var btn = document.getElementById("saveBtn");
                var cancel = document.getElementById("cancelBtn");
                var close = document.getElementById("closeBtn");
                if (btn) {
                    btn.disabled = on;
                    if (btn.classList) {
                        if (on) btn.classList.add("busy"); else btn.classList.remove("busy");
                    }
                    var label = btn.querySelector(".sm-btn-label");
                    if (label) label.textContent = on ? "Menyimpan..." : "Simpan Pengaturan";
                }
                if (cancel) cancel.disabled = on;
                if (close) close.disabled = on;
            }
            function showToast(msg) {
                var t = document.getElementById("smToast");
                t.textContent = msg;
                t.classList.add("show");
                setTimeout(function () { t.classList.remove("show"); }, 2200);
            }
            function visibleKeys() {
                return columnDefs.filter(function (c) { return visibleColumns[c.key]; });
            }
            function filteredRows() {
                var q = (document.getElementById("search").value || "").toLowerCase();
                return rows.filter(function (r) {
                    var hay = [(r.vehiclePlate || ""), (r.vehicleId || ""), (r.companyName || ""), (r.companyId || ""), (r.mirrorServers || []).join(" ")].join(" ").toLowerCase();
                    if (q && hay.indexOf(q) < 0) return false;
                    if (applied.company !== "all" && r.companyId !== applied.company) return false;
                    if (applied.status !== "all" && r.status !== applied.status) return false;
                    if (applied.server !== "all" && (r.mirrorServers || []).indexOf(applied.server) < 0) return false;
                    return true;
                }).sort(function (a, b) {
                    var av = "", bv = "";
                    if (sortKey === "vehicle") { av = a.vehiclePlate || ""; bv = b.vehiclePlate || ""; }
                    else if (sortKey === "vehicleId") { av = a.vehicleId || ""; bv = b.vehicleId || ""; }
                    else if (sortKey === "company") { av = a.companyName || ""; bv = b.companyName || ""; }
                    else if (sortKey === "server") { av = (a.mirrorServers || []).join(","); bv = (b.mirrorServers || []).join(","); }
                    else if (sortKey === "status") { av = a.status || ""; bv = b.status || ""; }
                    else { av = a.updatedAt || ""; bv = b.updatedAt || ""; }
                    if (av < bv) return sortDir === "asc" ? -1 : 1;
                    if (av > bv) return sortDir === "asc" ? 1 : -1;
                    return 0;
                });
            }
            function renderKpis(k) {
                k = k || {};
                document.getElementById("kpiCompany").textContent = fmt(k.companyMirrored);
                document.getElementById("kpiCompanyFoot").textContent = "dari " + fmt(k.companyActive) + " company aktif";
                document.getElementById("kpiVehicle").textContent = fmt(k.vehicleMirrored);
                document.getElementById("kpiVehicleFoot").textContent = "dari " + fmt(k.vehicleActive) + " vehicle aktif";
                document.getElementById("kpiServer").textContent = fmt(k.serverActive);
                document.getElementById("kpiServerFoot").textContent = "dari " + fmt(k.serverTotal || servers.length) + " server tersedia";
                var searchPh = document.getElementById("serverSearch");
                if (searchPh) searchPh.placeholder = "Cari dari " + (k.serverTotal || servers.length) + " server...";
            }
            function renderFilters() {
                var companySel = document.getElementById("companyTableFilter");
                var serverSel = document.getElementById("serverTableFilter");
                var cVal = companySel.value || "all";
                var sVal = serverSel.value || "all";
                companySel.innerHTML = '<option value="all">Semua company</option>' + companies.map(function (c) {
                    return '<option value="' + escapeHtml(c.id) + '">' + escapeHtml(c.name) + "</option>";
                }).join("");
                serverSel.innerHTML = '<option value="all">Semua server mirror</option>' + servers.map(function (s) {
                    return '<option value="' + escapeHtml(s) + '">' + escapeHtml(s) + "</option>";
                }).join("");
                companySel.value = cVal;
                serverSel.value = sVal;
            }
            function escapeHtml(v) {
                return String(v == null ? "" : v)
                    .replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
            }
            function renderTable() {
                var all = filteredRows();
                var totalPages = Math.max(1, Math.ceil(all.length / pageSize));
                if (currentPage > totalPages) currentPage = totalPages;
                var start = (currentPage - 1) * pageSize;
                var pageRows = all.slice(start, start + pageSize);
                var cols = visibleKeys();
                document.getElementById("thead").innerHTML = "<tr>" + cols.map(function (c) {
                    var sort = (c.key !== "rowno" && c.key !== "active" && c.key !== "actions") ? '<span class="sm-sort" data-sort="' + c.key + '">↕</span>' : "";
                    return '<th data-col="' + c.key + '">' + c.label + sort + "</th>";
                }).join("") + "</tr>";
                if (!pageRows.length) {
                    document.getElementById("tbody").innerHTML = '<tr><td colspan="' + (cols.length || 1) + '" style="text-align:center;padding:30px;color:#64748b">Data tidak ditemukan.</td></tr>';
                } else {
                    document.getElementById("tbody").innerHTML = pageRows.map(function (r, i) {
                        var cells = {
                            rowno: '<td class="sm-rowno">' + (start + i + 1) + "</td>",
                            vehicle: '<td><strong class="mono">' + escapeHtml(r.vehiclePlate || "-") + "</strong></td>",
                            vehicleId: '<td class="mono">' + escapeHtml(r.vehicleId || "") + "</td>",
                            company: "<td><strong style='font-size:11.5px'>" + escapeHtml(r.companyName || "") + "</strong><br><small class='mono' style='color:#94a3b8'>" + escapeHtml(r.companyId || "") + "</small></td>",
                            server: "<td>" + (r.mirrorServers || []).map(function (s) { return '<span class="sm-server mono"><i class="sm-dot"></i>' + escapeHtml(s) + "</span>"; }).join("") + "</td>",
                            status: '<td><span class="sm-badge ' + (r.status === "active" ? "on" : "paused") + '"><i class="sm-dot" style="background:currentColor"></i>' + (r.status === "active" ? "Aktif" : "Dijeda") + "</span></td>",
                            updated: "<td>" + escapeHtml(r.updatedAt || "") + "<br><small style='color:#94a3b8'>oleh " + escapeHtml(r.updatedBy || "Admin") + "</small></td>",
                            active: '<td><button type="button" class="sm-toggle ' + (r.status === "active" ? "on" : "") + '" data-toggle="' + escapeHtml(r.id) + '" aria-label="Toggle status"></button></td>',
                            actions: '<td><button type="button" class="sm-edit" data-edit="' + escapeHtml(r.id) + '">Edit</button></td>'
                        };
                        return "<tr>" + cols.map(function (c) { return cells[c.key]; }).join("") + "</tr>";
                    }).join("");
                }
                document.getElementById("visibleRows").textContent = pageRows.length;
                document.getElementById("totalRows").textContent = all.length;
                var pager = [];
                pager.push('<button type="button" class="sm-page-btn" data-page="' + Math.max(1, currentPage - 1) + '">‹</button>');
                var startPage = Math.max(1, currentPage - 3);
                var endPage = Math.min(totalPages, startPage + 6);
                if (endPage - startPage < 6) startPage = Math.max(1, endPage - 6);
                for (var p = startPage; p <= endPage; p++) {
                    pager.push('<button type="button" class="sm-page-btn ' + (p === currentPage ? "active" : "") + '" data-page="' + p + '">' + p + "</button>");
                }
                pager.push('<button type="button" class="sm-page-btn" data-page="' + Math.min(totalPages, currentPage + 1) + '">›</button>');
                document.getElementById("pager").innerHTML = pager.join("");
                Array.prototype.forEach.call(document.querySelectorAll("[data-sort]"), function (el) {
                    el.onclick = function () {
                        var key = el.getAttribute("data-sort");
                        if (sortKey === key) sortDir = sortDir === "asc" ? "desc" : "asc";
                        else { sortKey = key; sortDir = "asc"; }
                        renderTable();
                    };
                });
                Array.prototype.forEach.call(document.querySelectorAll("[data-page]"), function (el) {
                    el.onclick = function () { currentPage = parseInt(el.getAttribute("data-page"), 10) || 1; renderTable(); };
                });
                Array.prototype.forEach.call(document.querySelectorAll("[data-toggle]"), function (el) {
                    el.onclick = function () {
                        var id = el.getAttribute("data-toggle");
                        var row = rows.filter(function (x) { return x.id === id; })[0];
                        if (!row) return;
                        var next = row.status === "active" ? "paused" : "active";
                        post("UpdateStatus", { id: id, status: next }, function (res) {
                            if (!res || !res.success) { showToast((res && res.message) || "Gagal mengubah status."); return; }
                            showToast(res.message);
                            loadPage();
                        }, function (msg) {
                            showToast(msg);
                        }, next === "active" ? "Mengaktifkan pengaturan..." : "Menjeda pengaturan...");
                    };
                });
                Array.prototype.forEach.call(document.querySelectorAll("[data-edit]"), function (el) {
                    el.onclick = function () { openModal(el.getAttribute("data-edit")); };
                });
            }
            function renderColumnOptions() {
                document.getElementById("columnOptions").innerHTML = columnDefs.map(function (c) {
                    return '<label class="sm-col-opt"><input type="checkbox" data-column="' + c.key + '" ' + (visibleColumns[c.key] ? "checked" : "") + " /> " + c.label + "</label>";
                }).join("");
                Array.prototype.forEach.call(document.querySelectorAll("[data-column]"), function (input) {
                    input.onchange = function () {
                        visibleColumns[input.getAttribute("data-column")] = input.checked;
                        document.getElementById("toggleAllColumns").checked = columnDefs.every(function (c) { return visibleColumns[c.key]; });
                        renderTable();
                    };
                });
            }
            function closeMenus() {
                Array.prototype.forEach.call(document.querySelectorAll(".sm-multi"), function (x) { x.classList.remove("open", "drop-up"); });
            }
            function positionDropdown(box) {
                box.classList.remove("drop-up");
                requestAnimationFrame(function () {
                    var rect = box.getBoundingClientRect();
                    var menu = box.querySelector(".sm-menu");
                    var needed = Math.min(menu.scrollHeight, 270) + 50;
                    if (window.innerHeight - rect.bottom < needed && rect.top > window.innerHeight - rect.bottom) box.classList.add("drop-up");
                });
            }
            function renderCompanies(q) {
                q = (q || "").toLowerCase();
                var matches = companies.filter(function (c) { return (c.name || "").toLowerCase().indexOf(q) >= 0 || (c.id || "").toLowerCase().indexOf(q) >= 0; });
                document.getElementById("companyList").innerHTML = matches.map(function (c) {
                    return '<div class="sm-option" data-company="' + escapeHtml(c.id) + '"><span>' + escapeHtml(c.name) + "</span>" + (selectedCompany === c.id ? '<span class="sm-check">✓</span>' : "") + "</div>";
                }).join("") || '<div class="sm-empty">Company tidak ditemukan</div>';
                Array.prototype.forEach.call(document.querySelectorAll("[data-company]"), function (item) {
                    item.onclick = function () {
                        selectedCompany = item.getAttribute("data-company");
                        selectedVehicles = {};
                        allVehicles = false;
                        var found = companies.filter(function (c) { return c.id === selectedCompany; })[0];
                        setTriggerText("companyTrigger", found ? found.name : "Pilih company");
                        document.getElementById("companyMulti").classList.remove("open");
                        document.getElementById("vehicleSearch").value = "";
                        renderCompanies(document.getElementById("companySearch").value);
                        loadVehicles(selectedCompany);
                    };
                });
            }
            function updateVehicleTrigger() {
                var trigger = document.getElementById("vehicleTrigger");
                if (!selectedCompany) {
                    trigger.disabled = true;
                    setTriggerText("vehicleTrigger", "Pilih company terlebih dahulu");
                    renderChips("vehicleChips", []);
                    return;
                }
                trigger.disabled = false;
                var keys = Object.keys(selectedVehicles);
                if (allVehicles) setTriggerText("vehicleTrigger", "Semua vehicle");
                else if (!keys.length) setTriggerText("vehicleTrigger", "Pilih vehicle");
                else if (keys.length <= 2) setTriggerText("vehicleTrigger", keys.map(function (id) { return selectedVehicles[id]; }).join(", "));
                else setTriggerText("vehicleTrigger", keys.length + " vehicle dipilih");
                renderChips("vehicleChips", allVehicles ? ["Semua vehicle"] : keys.map(function (id) { return selectedVehicles[id]; }), true);
            }
            function renderVehicleOptions(q) {
                var trigger = document.getElementById("vehicleTrigger");
                var list = document.getElementById("vehicleList");
                if (!selectedCompany) {
                    trigger.disabled = true;
                    setTriggerText("vehicleTrigger", "Pilih company terlebih dahulu");
                    list.innerHTML = "";
                    renderChips("vehicleChips", []);
                    return;
                }
                trigger.disabled = false;
                q = (q || "").toLowerCase();
                var matches = vehiclesByCompany.filter(function (v) {
                    return (v.plate || "").toLowerCase().indexOf(q) >= 0 || (v.id || "").toLowerCase().indexOf(q) >= 0;
                });
                list.innerHTML = '<label class="sm-option all"><input type="checkbox" value="all" ' + (allVehicles ? "checked" : "") + " /> Semua vehicle</label>" +
                    matches.map(function (v) {
                        return '<label class="sm-option"><input type="checkbox" value="' + escapeHtml(v.id) + '" ' + (!allVehicles && selectedVehicles[v.id] ? "checked" : "") + ' /> <span class="mono">' + escapeHtml(v.plate || v.id) + "</span></label>";
                    }).join("") + (matches.length ? "" : '<div class="sm-empty">Vehicle tidak ditemukan</div>');
                Array.prototype.forEach.call(list.querySelectorAll("input"), function (input) {
                    input.onchange = function () {
                        if (input.value === "all") {
                            allVehicles = input.checked;
                            selectedVehicles = {};
                        } else {
                            allVehicles = false;
                            if (input.checked) {
                                var found = vehiclesByCompany.filter(function (v) { return v.id === input.value; })[0];
                                selectedVehicles[input.value] = found ? (found.plate || found.id) : input.value;
                            } else delete selectedVehicles[input.value];
                        }
                        renderVehicleOptions(document.getElementById("vehicleSearch").value);
                        document.getElementById("vehicleMulti").classList.add("open");
                        updateVehicleTrigger();
                    };
                });
                updateVehicleTrigger();
            }
            function updateServerTrigger() {
                var keys = Object.keys(selectedServers);
                setTriggerText("serverTrigger", keys.length === 0 ? "Pilih server mirror" : (keys.length <= 2 ? keys.join(", ") : keys.length + " server dipilih"));
                renderChips("serverChips", keys, true);
            }
            function renderServers(q) {
                q = (q || "").toLowerCase();
                var matches = servers.filter(function (s) { return s.toLowerCase().indexOf(q) >= 0; });
                document.getElementById("serverList").innerHTML = matches.map(function (s) {
                    return '<label class="sm-option"><input type="checkbox" value="' + escapeHtml(s) + '" ' + (selectedServers[s] ? "checked" : "") + ' /> <span class="mono">' + escapeHtml(s) + "</span></label>";
                }).join("") + (matches.length ? "" : '<div class="sm-empty">Server tidak ditemukan</div>');
                Array.prototype.forEach.call(document.querySelectorAll("#serverList input"), function (input) {
                    input.onchange = function () {
                        if (input.checked) selectedServers[input.value] = true;
                        else delete selectedServers[input.value];
                        updateServerTrigger();
                    };
                });
            }
            function loadVehicles(companyId, after) {
                vehiclesByCompany = [];
                renderVehicleOptions("");
                if (!companyId) return;
                var trigger = document.getElementById("vehicleTrigger");
                trigger.disabled = true;
                setTriggerText("vehicleTrigger", "Memuat vehicle...");
                post("GetVehicles", { companyId: companyId }, function (res) {
                    if (!res || !res.success) {
                        setTriggerText("vehicleTrigger", "Pilih vehicle");
                        trigger.disabled = false;
                        showToast((res && res.message) || "Gagal memuat vehicle.");
                        return;
                    }
                    vehiclesByCompany = res.items || [];
                    renderVehicleOptions(document.getElementById("vehicleSearch").value);
                    if (after) after();
                }, function (msg) {
                    setTriggerText("vehicleTrigger", "Pilih vehicle");
                    trigger.disabled = false;
                    showToast(msg);
                }, "Memuat daftar vehicle...");
            }
            function resetForm() {
                selectedCompany = "";
                selectedVehicles = {};
                selectedServers = {};
                allVehicles = false;
                vehiclesByCompany = [];
                document.getElementById("companySearch").value = "";
                document.getElementById("vehicleSearch").value = "";
                document.getElementById("serverSearch").value = "";
                setTriggerText("companyTrigger", "Pilih company");
                updateVehicleTrigger();
                updateServerTrigger();
                renderCompanies();
                renderVehicleOptions("");
                renderServers();
            }
            function openModal(id) {
                editingId = id || null;
                resetForm();
                document.getElementById("smModalOverlay").classList.add("show");
                document.body.classList.add("sm-modal-open");
                if (!id) {
                    document.getElementById("modalTitle").textContent = "Tambah Pengaturan Mirroring";
                } else {
                    document.getElementById("modalTitle").textContent = "Edit Pengaturan Mirroring";
                    var row = rows.filter(function (r) { return r.id === id; })[0];
                    if (row) {
                        selectedCompany = row.companyId;
                        setTriggerText("companyTrigger", row.companyName);
                        renderCompanies();
                        (row.mirrorServers || []).forEach(function (s) { selectedServers[s] = true; });
                        updateServerTrigger();
                        renderServers();
                        loadVehicles(selectedCompany, function () {
                            selectedVehicles = {};
                            allVehicles = false;
                            selectedVehicles[row.vehicleId] = row.vehiclePlate || row.vehicleId;
                            renderVehicleOptions("");
                        });
                    }
                }
            }
            function closeModal() {
                if (saveBusy) return;
                document.getElementById("smModalOverlay").classList.remove("show");
                document.body.classList.remove("sm-modal-open");
                closeMenus();
            }
            function exportData(kind) {
                var data = filteredRows();
                var cols = visibleKeys().filter(function (c) { return c.key !== "active" && c.key !== "actions"; });
                var lines = [cols.map(function (c) { return c.label; }).join(",")];
                data.forEach(function (r, i) {
                    var map = {
                        rowno: i + 1,
                        vehicle: r.vehiclePlate || "",
                        vehicleId: r.vehicleId || "",
                        company: (r.companyName || "") + " (" + (r.companyId || "") + ")",
                        server: (r.mirrorServers || []).join(" | "),
                        status: r.status === "active" ? "Aktif" : "Dijeda",
                        updated: (r.updatedAt || "") + " " + (r.updatedBy || "")
                    };
                    lines.push(cols.map(function (c) { return '"' + String(map[c.key] || "").replace(/"/g, '""') + '"'; }).join(","));
                });
                var blob = new Blob(["\ufeff" + lines.join("\n")], { type: kind === "Excel" ? "application/vnd.ms-excel" : "text/csv;charset=utf-8;" });
                var a = document.createElement("a");
                a.href = URL.createObjectURL(blob);
                a.download = kind === "Excel" ? "server-mirroring.xls" : "server-mirroring.csv";
                a.click();
                showToast("Data diekspor ke " + kind + ".");
            }
            function loadPage() {
                post("GetPageData", {}, function (res) {
                    if (!res || !res.success) { showToast((res && res.message) || "Gagal memuat data."); return; }
                    servers = res.servers || [];
                    companies = res.companies || [];
                    rows = res.rows || [];
                    renderKpis(res.kpis);
                    renderFilters();
                    renderColumnOptions();
                    renderTable();
                    renderCompanies();
                    renderServers();
                }, function (msg) {
                    showToast(msg);
                }, "Memuat data mirroring...");
            }

            document.getElementById("addBtn").onclick = function () { openModal(); };
            document.getElementById("closeBtn").onclick = document.getElementById("cancelBtn").onclick = closeModal;
            document.getElementById("smModalOverlay").onclick = function (e) { if (e.target.id === "smModalOverlay") closeModal(); };
            document.getElementById("applyFilterBtn").onclick = function () {
                applied.company = document.getElementById("companyTableFilter").value;
                applied.status = document.getElementById("statusFilter").value;
                applied.server = document.getElementById("serverTableFilter").value;
                currentPage = 1;
                renderTable();
            };
            document.getElementById("clearFilterBtn").onclick = function () {
                document.getElementById("search").value = "";
                document.getElementById("companyTableFilter").value = "all";
                document.getElementById("statusFilter").value = "all";
                document.getElementById("serverTableFilter").value = "all";
                applied = { company: "all", status: "all", server: "all" };
                currentPage = 1;
                renderTable();
            };
            document.getElementById("search").oninput = function () { currentPage = 1; renderTable(); };
            document.getElementById("toggleAllColumns").onchange = function (e) {
                columnDefs.forEach(function (c) { visibleColumns[c.key] = e.target.checked; });
                renderColumnOptions();
                renderTable();
            };
            document.getElementById("toolsBtn").onclick = function (e) {
                e.stopPropagation();
                document.getElementById("toolsMenu").classList.toggle("show");
            };
            document.getElementById("toolsMenu").onclick = function (e) { e.stopPropagation(); };
            document.addEventListener("click", function () { document.getElementById("toolsMenu").classList.remove("show"); });
            Array.prototype.forEach.call(document.querySelectorAll("[data-export]"), function (b) {
                b.onclick = function () { exportData(b.getAttribute("data-export")); };
            });
            Array.prototype.forEach.call(document.querySelectorAll(".sm-trigger"), function (trigger) {
                trigger.onclick = function () {
                    if (trigger.disabled) return;
                    var box = trigger.closest(".sm-multi");
                    var willOpen = !box.classList.contains("open");
                    closeMenus();
                    if (willOpen) {
                        box.classList.add("open");
                        positionDropdown(box);
                        setTimeout(function () {
                            var inp = box.querySelector(".sm-menu-search input");
                            if (inp) inp.focus();
                        }, 0);
                    }
                };
            });
            document.addEventListener("click", function (e) {
                if (!e.target.closest || !e.target.closest(".sm-multi")) closeMenus();
            });
            document.getElementById("companySearch").oninput = function (e) { renderCompanies(e.target.value); };
            document.getElementById("vehicleSearch").oninput = function (e) { renderVehicleOptions(e.target.value); };
            document.getElementById("serverSearch").oninput = function (e) { renderServers(e.target.value); };
            document.getElementById("saveBtn").onclick = function () {
                if (saveBusy) return;
                var vehicleIds = Object.keys(selectedVehicles);
                var mirrorServers = Object.keys(selectedServers);
                if (!selectedCompany || (!allVehicles && !vehicleIds.length) || !mirrorServers.length) {
                    showToast("Pilih company, vehicle, dan server mirror.");
                    return;
                }
                setSaveBusy(true);
                post("SaveConfiguration", {
                    payload: JSON.stringify({
                        id: editingId,
                        companyId: selectedCompany,
                        allVehicles: allVehicles,
                        vehicleIds: vehicleIds,
                        mirrorServers: mirrorServers
                    })
                }, function (res) {
                    setSaveBusy(false);
                    if (!res || !res.success) { showToast((res && res.message) || "Gagal menyimpan."); return; }
                    closeModal();
                    showToast(res.message);
                    loadPage();
                }, function (msg) {
                    setSaveBusy(false);
                    showToast(msg);
                }, allVehicles ? "Menyimpan pengaturan untuk semua vehicle..." : "Menyimpan pengaturan...");
            };
            mountOverlays();
            loadPage();
        })();
    </script>
</asp:Content>
