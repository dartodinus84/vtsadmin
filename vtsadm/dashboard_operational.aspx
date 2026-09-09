<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_operational.aspx.cs" Inherits="vtsadm.dashboard_operational" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .icon { top: 0px !important; font-size: 40px !important; }
        .inner { min-height: 100px !important; padding-top: 2px !important; }
        .row-stats-eq { display: flex; flex-wrap: wrap; }
        .row-stats-eq > div { display: flex; }
        .row-stats-eq .small-box { width: 100%; display: flex; flex-direction: column; }
        .row-stats-eq .small-box .inner { flex: 1; }
        .small-box-footer { border-bottom-left-radius: 11px; border-bottom-right-radius: 11px; }
        .small-box { border-radius: 11px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); }
        .box { border-radius: 11px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); }
        .box-header { font-size: 16px; }
        .progress-thin { height: 6px; margin-bottom: 4px; }
        .period-filter-list label { margin-right: 20px; font-weight: normal; cursor: pointer; }
        .period-filter-list input { margin-right: 6px; }
        #chartOperationalWrap { height: 220px; max-height: 220px; overflow: hidden; position: relative; }
        #chartOperationalWrap canvas { max-height: 100%; width: 100% !important; }
        #chartOperationalWrap .chartjs-legend ul li { display: inline-flex; align-items: center; margin-right: 12px; }
        #chartOperationalWrap .chartjs-legend ul li span { display: inline-block; margin-right: 4px; }
        #filterLoadingOverlay { position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(255,255,255,0.7); z-index: 9999; display: none; align-items: center; justify-content: center; }
        #filterLoadingOverlay.show { display: flex !important; }
        #filterLoadingOverlay .spinner-wrap { text-align: center; }
        #filterLoadingOverlay .fa-spin { font-size: 48px; color: #3c8dbc; }
        /* Filter section: side-by-side blocks using Bootstrap grid */
        .filter-box .box-body { padding: 15px 18px; }
        .filter-row { margin: 0 -8px; }
        .filter-row .filter-block { padding: 0 8px; }
        .filter-block .control-label { margin: 0 0 6px 0; font-weight: 600; color: #333; font-size: 13px; display: block; }
        .filter-block .filter-controls { margin: 0; }
        .filter-block .filter-controls span { display: inline-block; margin-right: 16px; white-space: nowrap; }
        .filter-block .filter-controls span input { margin-right: 5px; vertical-align: middle; }
        .filter-block .filter-hint { font-size: 12px; color: #777; margin-top: 4px; display: block; }
        .filter-block .period-filter-list { margin: 0; padding: 0; list-style: none; }
        .filter-block .period-filter-list table { width: auto !important; }
        .filter-block .period-filter-list td { padding-right: 18px; padding-bottom: 0; border: none !important; vertical-align: middle; }
        .filter-block .period-filter-list label { margin: 0; font-weight: normal; cursor: pointer; }
        /* Mark current day in daterangepicker calendar */
        .daterangepicker td.today { font-weight: bold; background: #e8f4fc !important; border: 2px solid #3c8dbc !important; }
        .daterangepicker td.today.available:hover { background: #3c8dbc !important; color: #fff !important; }
    </style>
    <section class="content-header">
        <h1>Dashboard
        <small>Operational</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li class="active"><i class="fa fa-dashboard"></i>Operational</li>
        </ol>
    </section>

    <section class="content">
        <div id="filterLoadingOverlay" aria-hidden="true">
            <div class="spinner-wrap">
                <i class="fa fa-spinner fa-spin"></i>
                <p style="margin-top: 10px; font-weight: bold;">Loading...</p>
            </div>
        </div>
        <!-- Filter: Periode, Area, Options -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid filter-box">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-filter"></i> Filter</h3>
                        <div class="box-tools pull-right">
                            <asp:Panel ID="PanelManagementButton" runat="server" Visible="false">
                                <button type="button" class="btn btn-default btn-sm" onclick="openManagementModal(); return false;" title="Manajemen Tim &amp; Target"><i class="fa fa-gear"></i> Setting Manajemen</button>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row filter-row">
                            <div class="col-xs-12 col-sm-6 col-md-4 filter-block">
                                <label class="control-label">Tanggal</label>
                                <div class="filter-controls">
                                    <input type="text" id="dtRange" class="form-control" style="max-width: 240px; cursor: pointer;" placeholder="Pilih rentang tanggal" readonly="readonly" />
                                    <asp:HiddenField ID="hidDateFrom" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hidDateTo" runat="server" ClientIDMode="Static" />
                                    <asp:LinkButton ID="lnkRefreshFilter" runat="server" ClientIDMode="Static" OnClick="chkFilter_CheckedChanged" style="display:none;" Text="" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4 filter-block">
                                <label class="control-label">Area</label>
                                <div class="filter-controls">
                                    <asp:CheckBox ID="chkAreaWest" runat="server" ClientIDMode="Static" Text="West" />
                                    <asp:CheckBox ID="chkAreaEast" runat="server" ClientIDMode="Static" Text="East" />
                                </div>
                                <small class="filter-hint">Kosongkan keduanya = Semua Area</small>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4 filter-block">
                                <label class="control-label">Opsi</label>
                                <div class="filter-controls">
                                    <asp:CheckBox ID="chkExcludeWeekend" runat="server" ClientIDMode="Static" Text="Filter Sabtu Minggu" Checked="True" />
                                    <asp:CheckBox ID="chkExcludeHoliday" runat="server" ClientIDMode="Static" Text="Filter Hari Libur" Checked="True" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Stats Grid: same structure as Dashboard Operasional template, empty data -->
        <div class="row row-stats-eq">
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-blue-gradient">
                    <div class="inner">
                        <h4><span id="lblTotalCustomers" runat="server">0</span></h4>
                        <p>Total Customer</p>
                        <div class="row" style="margin-top: 8px; font-size: 12px;">
                            <div class="col-xs-6"><span class="text-muted">Prioritas</span><br /><span id="lblPriorityCount" runat="server">0</span></div>
                            <div class="col-xs-6"><span class="text-muted">Reguler</span><br /><span id="lblRegularCount" runat="server">0</span></div>
                        </div>
                    </div>
                    <div class="icon"><i class="fa fa-users"></i></div>
                    <a href="#" class="small-box-footer" data-target="#modalDetailCustomer" data-detail="totalcustomer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-green-gradient">
                    <div class="inner">
                        <h4><span id="lblTotalVisits" runat="server">0</span> <small>/ <span id="lblTargetVisits" runat="server">0</span> Target</small></h4>
                        <p>Total Visit <span id="lblVisitPeriod" runat="server">(Harian)</span></p>
                        <div class="progress progress-thin">
                            <div id="divVisitProgress" runat="server" class="progress-bar progress-bar-success" role="progressbar" style="width: 0%;">0&#37;</div>
                        </div>
                    </div>
                    <div class="icon"><i class="fa fa-shoe-prints"></i></div>
                    <a href="#" class="small-box-footer" data-target="#modalDetailVisit" data-detail="visit">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-red-gradient">
                    <div class="inner">
                        <h4><span id="lblPendingVisits" runat="server" class="count-belum-visit">0</span></h4>
                        <p>Total Belum Visit</p>
                        <p class="text-muted" style="font-size: 11px; margin-top: 4px;">Gap pencapaian tim</p>
                    </div>
                    <div class="icon"><i class="fa fa-circle-exclamation"></i></div>
                    <a href="#" class="small-box-footer" data-target="#modalDetailBelumVisit" data-detail="belumvisit">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-teal-gradient">
                    <div class="inner">
                        <p><strong>Status Customer</strong></p>
                        <div class="row" style="font-size: 12px; margin-top: 6px;">
                            <div class="col-xs-4" title="Sudah ada training"><span class="badge bg-green"><span id="lblStatusExisting" runat="server">0</span>&#37;</span><br />Existing</div>
                            <div class="col-xs-4" title="Belum pernah dilakukan training"><span class="badge bg-aqua"><span id="lblStatusNew" runat="server">0</span>&#37;</span><br />New</div>
                            <div class="col-xs-4" title="Nama perusahaan diawali &quot;Trial&quot;"><span class="badge bg-yellow"><span id="lblStatusTrial" runat="server">0</span>&#37;</span><br />Trial</div>
                        </div>
                    </div>
                    <div class="icon"><i class="fa fa-clipboard-check"></i></div>
                    <a href="#" class="small-box-footer" data-target="#modalStatusCustomer" data-detail="statuscustomer">Details <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <!-- Performa Tim IT -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-users"></i> Performa Tim IT</h3>
                        <div class="box-tools pull-right">
                            <a href="#" class="btn btn-default btn-sm" data-target="#modalDetailKunjungan" data-detail="kunjungan">Lihat Detail <i class="fa fa-arrow-right"></i></a>
                        </div>
                    </div>
                    <div class="box-body no-padding">
                        <div class="form-group form-group-sm" style="margin: 8px 10px 5px;">
                            <input type="text" id="txtSearchTeam" name="team_name_filter" class="form-control input-sm" style="width: 200px;" placeholder="Cari Nama (Enter)..." autocomplete="off" readonly onfocus="this.removeAttribute('readonly')" />
                        </div>
                        <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                            <table class="table table-striped table-condensed table-hover" style="margin-bottom: 0;">
                                <thead>
                                    <tr>
                                        <th>Nama</th>
                                        <th>Target (<span id="spanTeamPeriod" runat="server">Periode</span>)</th>
                                        <th>Realisasi</th>
                                        <th>Progress</th>
                                        <th>Sisa (Hari)</th>
                                    </tr>
                                </thead>
                                <tbody id="tbodyTeam">
                                    <tr><td colspan="5" class="text-center text-muted">Loading...</td></tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Analitik Kunjungan -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-bar-chart"></i> Analitik Kunjungan</h3>
                    </div>
                    <div class="box-body">
                        <asp:HiddenField ID="chartOperationalData" runat="server" Value="" />
                        <div id="chartOperationalWrap">
                            <div id="chartOperational" style="width:100%; height:220px;"></div>
                            <p id="chartOperationalNoData" class="text-center text-muted" style="padding-top: 80px; display:none;">Belum ada data</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Modal: Manajemen Tim & Target (AJAX - no postback) -->
    <div class="modal fade" id="modalManagement" tabindex="-1" role="dialog" aria-labelledby="modalManagementLabel">
        <div class="modal-dialog modal-lg" role="document" style="width: 90%; max-width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalManagementLabel"><i class="fa fa-users-gear"></i> Manajemen Tim & Target</h4>
                    <p class="text-muted" style="font-size: 11px; margin: 2px 0 0 0;">Atur target dan beban kerja tim IT</p>
                </div>
                <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                    <div class="form-group form-group-sm">
                        <label>Search By Nama Staff</label>
                        <div class="input-group">
                            <input type="text" id="txtSearchManagement" class="form-control" placeholder="Cari Nama Staff..." />
                            <span class="input-group-btn">
                                <button type="button" id="btnSearchManagement" class="btn btn-primary">Search</button>
                                <button type="button" id="btnClearManagement" class="btn btn-default">Clear</button>
                            </span>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-condensed" id="tblManagement" style="width:100%">
                            <thead>
                                <tr><th>Nama Staff</th><th>Role</th><th>Total Customer</th><th>Target Harian</th><th>Cycle (Hari)</th><th>Aksi</th></tr>
                            </thead>
                            <tbody id="tbodyManagement"></tbody>
                        </table>
                        <p id="pManagementEmpty" class="text-center text-muted" style="display:none">Belum ada data</p>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ECharts: use UMD build to avoid "Unexpected token export" on server -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/echarts/5.4.3/echarts.min.js"></script>
    <script type="text/javascript">
        function loadManagementTable() {
            var search = $('#txtSearchManagement').val() || '';
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetManagementList',
                data: JSON.stringify({ search: search, areaFilter: getAreaFilter() }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var res = r && r.d;
                    if (!res) { alert('Gagal memuat data'); return; }
                    if (!res.Success) { alert('Gagal memuat data: ' + (res.Error || 'Unknown error')); return; }
                    var list = res.Data || [];
                    if (!Array.isArray(list)) list = [];
                    var tbody = $('#tbodyManagement');
                    tbody.empty();
                    $('#pManagementEmpty').hide();
                    if (list.length === 0) { $('#pManagementEmpty').show(); return; }
                    for (var i = 0; i < list.length; i++) {
                        var row = list[i];
                        var hasSetting = row.SettingId && row.SettingId !== '';
                        var actions = hasSetting
                            ? '<button type="button" class="btn btn-primary btn-xs btn-edit" data-userid="' + (row.UserID || '') + '" data-settingid="' + (row.SettingId || '') + '" data-total="' + (row.TotalCust || 0) + '" data-target="' + (row.DailyTarget || 0) + '">Edit</button> '
                            + '<button type="button" class="btn btn-danger btn-xs btn-delete" data-settingid="' + (row.SettingId || '') + '">Hapus</button>'
                            : '<button type="button" class="btn btn-success btn-xs btn-simpan" data-userid="' + (row.UserID || '') + '">Simpan</button>';
                        var tr = '<tr data-userid="' + (row.UserID || '') + '" data-settingid="' + (row.SettingId || '') + '" data-total="' + (row.TotalCust || 0) + '">' +
                            '<td>' + (row.NamaStaff || '') + '</td>' +
                            '<td>' + (row.Role || '') + '</td>' +
                            '<td style="text-align:right">' + (row.TotalCust || 0) + '</td>' +
                            '<td style="text-align:right"><input type="number" class="form-control input-sm txt-target" value="' + (row.DailyTarget || 0) + '" style="width:70px" /></td>' +
                            '<td style="text-align:right">' + (row.Cycle || 0) + '</td>' +
                            '<td style="text-align:center">' + actions + '</td></tr>';
                        tbody.append(tr);
                    }
                },
                error: function (xhr, st, err) { alert('Gagal memuat data: ' + (err || st)); }
            });
        }
        function doInsert(userId, total, target) {
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/ManagementInsert',
                data: JSON.stringify({ userId: userId, totalCust: total, dailyTarget: target }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var res = r && r.d;
                    alert(res && (res.message !== undefined) ? res.message : 'Done');
                    if (res && res.success) loadManagementTable();
                },
                error: function (xhr, st, err) { alert('Gagal: ' + (err || st)); }
            });
        }
        function doUpdate(settingId, total, target) {
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/ManagementUpdate',
                data: JSON.stringify({ settingId: settingId, totalCust: total, dailyTarget: target }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var res = r && r.d;
                    alert(res && (res.message !== undefined) ? res.message : 'Done');
                    if (res && res.success) loadManagementTable();
                },
                error: function (xhr, st, err) { alert('Gagal: ' + (err || st)); }
            });
        }
        function doDelete(settingId) {
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/ManagementDelete',
                data: JSON.stringify({ settingId: settingId }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var res = r && r.d;
                    alert(res && (res.message !== undefined) ? res.message : 'Done');
                    if (res && res.success) loadManagementTable();
                },
                error: function (xhr, st, err) { alert('Gagal: ' + (err || st)); }
            });
        }
        function openManagementModal() {
            $('#modalManagement').modal('show');
            loadManagementTable();
        }
        $(document).on('click', '#btnSearchManagement, #btnClearManagement', function () {
            if ($(this).attr('id') === 'btnClearManagement') $('#txtSearchManagement').val('');
            loadManagementTable();
        });
        $(document).on('click', '.btn-simpan', function () {
            var tr = $(this).closest('tr');
            var userId = tr.data('userid') || $(this).data('userid');
            var total = parseInt(tr.data('total'), 10) || 0;
            var target = parseInt(tr.find('.txt-target').val(), 10) || 0;
            doInsert(userId, total, target);
        });
        $(document).on('click', '.btn-edit', function () {
            var tr = $(this).closest('tr');
            var settingId = parseInt(tr.data('settingid') || $(this).data('settingid'), 10);
            var total = parseInt(tr.data('total'), 10) || 0;
            var target = parseInt(tr.find('.txt-target').val(), 10) || 0;
            doUpdate(settingId, total, target);
        });
        $(document).on('click', '.btn-delete', function () {
            if (!confirm('Yakin hapus data ini?')) return;
            var settingId = parseInt($(this).data('settingid'), 10);
            doDelete(settingId);
        });
        (function () {
            function closeManagementModal() {
                $('#modalManagement').removeClass('in').css('display', 'none').attr('aria-hidden', 'true');
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css('padding-right', '');
            }
            $(document).off('click.managementClose').on('click.managementClose', '#modalManagement [data-dismiss="modal"], #modalManagement .modal-header button.close', function (e) {
                e.preventDefault();
                closeManagementModal();
            });
        })();
    </script>

    <input type="hidden" id="hidVisitDetailStaff" value="" />
    <!-- Modal: Total Belum Visit - Customer yang belum dapat kunjungan training -->
    <div class="modal fade" id="modalDetailBelumVisit" tabindex="-1" role="dialog" aria-labelledby="modalDetailBelumVisitLabel" data-backdrop="static" data-keyboard="true">
        <div class="modal-dialog modal-lg" role="document" style="width: 90%; max-width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalDetailBelumVisitLabel"><i class="fa fa-users"></i> Customer Belum Visit (Training)</h4>
                </div>
                <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                    <div class="table-responsive">
                        <table class="table table-striped table-condensed table-bordered">
                            <thead>
                                <tr>
                                    <th>Training ID</th>
                                    <th>Nama Customer</th>
                                    <th>Assignee (IT Outbound)</th>
                                    <th>PIC Name</th>
                                    <th>Contact Number</th>
                                    <th>Business Fields</th>
                                </tr>
                            </thead>
                            <tbody id="tbodyDetailBelumVisitModal"></tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal: Total Visit (new format: Tanggal, Waktu, IT Staff, Customer, Status Customer, Status Visit, Hasil) -->
    <div class="modal fade" id="modalDetailVisit" tabindex="-1" role="dialog" aria-labelledby="modalDetailVisitLabel" data-backdrop="static" data-keyboard="true">
        <div class="modal-dialog modal-lg" role="document" style="width: 90%; max-width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalDetailVisitLabel"><i class="fa fa-shoe-prints"></i> Detail Total Visit</h4>
                </div>
                <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                    <div class="table-responsive">
                        <table class="table table-striped table-condensed table-bordered">
                            <thead>
                                <tr>
                                    <th>Tanggal</th>
                                    <th>Waktu</th>
                                    <th>IT Staff</th>
                                    <th>Customer</th>
                                    <th>Status Customer</th>
                                    <th>Status Visit</th>
                                    <th>Hasil</th>
                                </tr>
                            </thead>
                            <tbody id="tbodyDetailVisitModal"></tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal: Detail Kunjungan IT (Performa Tim IT - old format: Nama Customer, Nama IT, Waktu Visit, Status, Remark, Attachment) -->
    <div class="modal fade" id="modalDetailKunjungan" tabindex="-1" role="dialog" aria-labelledby="modalDetailKunjunganLabel" data-backdrop="static" data-keyboard="true">
        <div class="modal-dialog modal-lg" role="document" style="width: 90%; max-width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalDetailKunjunganLabel"><i class="fa fa-list"></i> Detail Kunjungan IT</h4>
                </div>
                <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                    <div class="table-responsive">
                        <table class="table table-striped table-condensed table-bordered">
                            <thead>
                                <tr>
                                    <th>Nama Customer</th>
                                    <th>Marketing</th>
                                    <th>Nama IT</th>
                                    <th>Waktu Visit</th>
                                    <th>Status</th>
                                    <th>Remark / Hasil</th>
                                    <th class="text-center">Attachment</th>
                                </tr>
                            </thead>
                            <tbody id="tbodyDetailKunjunganModal"></tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                    <button type="button" class="btn btn-success btn-sm disabled" title="Belum ada data"><i class="fa fa-file-excel-o"></i> Export Data</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal: Total Customer (AJAX - no postback) -->
    <div class="modal fade" id="modalDetailCustomer" tabindex="-1" role="dialog" aria-labelledby="modalDetailCustomerLabel">
        <div class="modal-dialog modal-lg" role="document" style="width: 90%; max-width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalDetailCustomerLabel"><i class="fa fa-users"></i> Detail Customer</h4>
                </div>
                <div class="modal-body" style="max-height: 70vh; overflow-y: auto;">
                    <div class="form-group form-group-sm">
                        <label>Search By Full Name</label>
                        <div class="input-group">
                            <input type="text" id="txtSearchDetailCustomer" class="form-control" placeholder="Cari Nama Customer..." />
                            <span class="input-group-btn">
                                <button type="button" id="btnSearchDetailCustomer" class="btn btn-primary">Search</button>
                                <button type="button" id="btnClearDetailCustomer" class="btn btn-default">Clear</button>
                            </span>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-condensed" style="width:100%">
                            <thead><tr><th>Nama Customer</th><th>Tipe</th><th>Assignee</th><th>PIC Name</th><th>Contact Number</th><th>Status</th><th>Business Fields</th></tr></thead>
                            <tbody id="tbodyDetailCustomer"></tbody>
                        </table>
                        <p id="pDetailCustomerEmpty" class="text-center text-muted" style="display:none">Belum ada data</p>
                        <div id="paginationDetailCustomer" style="margin-top: 8px; display: none;">
                            <span class="text-muted" id="lblDetailCustomerPaging"></span>
                            <ul class="pagination pagination-sm pull-right" style="margin: 0;" id="paginationDetailCustomerNav"></ul>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal: Status Customer (table with paging) -->
    <div class="modal fade" id="modalStatusCustomer" tabindex="-1" role="dialog" aria-labelledby="modalStatusCustomerLabel">
        <div class="modal-dialog modal-lg" role="document" style="width: 95%; max-width: 1200px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalStatusCustomerLabel"><i class="fa fa-clipboard-check"></i> Status Customer - Training per Customer</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group form-group-sm" style="margin-bottom: 8px;">
                        <label style="margin-right: 10px;">Filter:</label>
                        <label class="checkbox-inline"><input type="checkbox" id="chkStatusExisting" value="Existing" checked /> Existing</label>
                        <label class="checkbox-inline"><input type="checkbox" id="chkStatusNew" value="New" checked /> New</label>
                        <label class="checkbox-inline"><input type="checkbox" id="chkStatusNewJO" value="New (Job Order)" checked /> New (Job Order)</label>
                        <label class="checkbox-inline"><input type="checkbox" id="chkStatusTrial" value="Trial" checked /> Trial</label>
                        <span style="margin-left: 15px;">
                            <input type="text" id="txtSearchStatusCustomer" class="form-control input-sm" style="width: 180px; display: inline-block;" placeholder="Cari Nama Customer..." autocomplete="off" />
                            <button type="button" id="btnSearchStatusCustomer" class="btn btn-primary btn-sm">Search</button>
                            <button type="button" id="btnClearStatusCustomer" class="btn btn-default btn-sm">Clear</button>
                        </span>
                    </div>
                    <div class="table-responsive" style="max-height: 60vh; overflow-y: auto;">
                        <table class="table table-bordered table-striped table-condensed" style="width:100%">
                            <thead><tr>
                                <th class="sortable" data-sort="CustomerName" data-type="text">Nama Customer <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="ITOutboundID" data-type="text">IT Outbound (ID) <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="JobOrderID" data-type="text">Job Order (ID) <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="MarketingName" data-type="text">Marketing <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="PICName" data-type="text">PIC Name <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="ContactNumber" data-type="text">Contact Number <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="BusinessFields" data-type="text">Business Fields <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="StatusCustomer" data-type="text">Status Customer <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="CustomerDate" data-type="text">Customer Date <i class="fa fa-sort"></i></th>
                                <th class="sortable" data-sort="TrainingCount" data-type="number">Jumlah Training <i class="fa fa-sort"></i></th>
                            </tr></thead>
                            <tbody id="tbodyStatusCustomer"></tbody>
                        </table>
                        <p id="pStatusTableNoData" class="text-center text-muted" style="display:none;">Belum ada data</p>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        (function () {
            var customerChartFullData = [];
            var statusCustomerSortCol = 'TrainingCount';
            var statusCustomerSortDir = 'desc';
            function getFilteredData() {
                var selected = [];
                if ($('#chkStatusExisting').is(':checked')) selected.push('Existing');
                if ($('#chkStatusNew').is(':checked')) selected.push('New');
                if ($('#chkStatusNewJO').is(':checked')) selected.push('New (Job Order)');
                if ($('#chkStatusTrial').is(':checked')) selected.push('Trial');
                var data = (customerChartFullData || []).filter(function (x) {
                    var status = (x.StatusCustomer || x.statusCustomer || 'New').toString();
                    return selected.indexOf(status) >= 0;
                });
                var key = statusCustomerSortCol;
                var dir = statusCustomerSortDir === 'asc' ? 1 : -1;
                function getCustomerDateDays(obj) {
                    var raw = obj.CustomerDateDays != null ? Number(obj.CustomerDateDays) : (obj.customerDateDays != null ? Number(obj.customerDateDays) : null);
                    if (raw != null && !isNaN(raw)) return raw;
                    var s = (obj.CustomerDate || obj.customerDate || '').toString().trim();
                    if (!s || s === '-') return 999999;
                    var num = parseInt(s, 10);
                    if (isNaN(num)) return 999999;
                    if (s.indexOf('hari') !== -1) return num;
                    if (s.indexOf('bulan') !== -1) return num * 30;
                    if (s.indexOf('tahun') !== -1) return num * 365;
                    return 999999;
                }
                return data.slice().sort(function (a, b) {
                    var va, vb, cmp;
                    if (key === 'TrainingCount') {
                        va = parseInt(a.TrainingCount != null ? a.TrainingCount : (a.trainingCount != null ? a.trainingCount : 0), 10) || 0;
                        vb = parseInt(b.TrainingCount != null ? b.TrainingCount : (b.trainingCount != null ? b.trainingCount : 0), 10) || 0;
                        cmp = (isNaN(va) ? 0 : va) - (isNaN(vb) ? 0 : vb);
                        return dir * cmp;
                    }
                    if (key === 'CustomerDate') {
                        va = getCustomerDateDays(a);
                        vb = getCustomerDateDays(b);
                        cmp = va - vb;
                        return dir * (isNaN(cmp) ? 0 : cmp);
                    }
                    va = (a[key] || a[key.replace(/([A-Z])/g, function (m) { return m.toLowerCase(); })] || '').toString();
                    vb = (b[key] || b[key.replace(/([A-Z])/g, function (m) { return m.toLowerCase(); })] || '').toString();
                    cmp = va < vb ? -1 : va > vb ? 1 : 0;
                    return dir * cmp;
                });
            }
            function updateStatusSortIcons() {
                $('#modalStatusCustomer thead th.sortable').each(function () {
                    var th = $(this);
                    var col = th.attr('data-sort') || th.data('sort');
                    var icon = th.find('i.fa');
                    icon.removeClass('fa-sort fa-sort-up fa-sort-down').addClass('fa-sort');
                    if (col === statusCustomerSortCol)
                        icon.addClass(statusCustomerSortDir === 'asc' ? 'fa-sort-up' : 'fa-sort-down');
                });
            }
            function renderStatusTable() {
                var list = getFilteredData();
                updateStatusSortIcons();
                var tbody = $('#tbodyStatusCustomer');
                var noDataEl = $('#pStatusTableNoData');
                tbody.empty();
                if (list.length === 0) { noDataEl.show(); return; }
                noDataEl.hide();
                for (var i = 0; i < list.length; i++) {
                    var x = list[i];
                    var name = (x.CustomerName || x.customerName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var itOb = (x.ITOutboundID || x.itOutboundID || '').toString().replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var jobId = (x.JobOrderID || x.jobOrderID || '').toString().replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var marketing = (x.MarketingName || x.marketingName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var picName = (x.PICName || x.picName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var contact = (x.ContactNumber || x.contactNumber || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var businessFields = (x.BusinessFields || x.businessFields || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    var status = (x.StatusCustomer || x.statusCustomer || 'New').toString();
                    var count = Math.max(0, parseInt(x.TrainingCount != null ? x.TrainingCount : (x.trainingCount != null ? x.trainingCount : 0), 10) || 0);
                    var customerDate = (x.CustomerDate || x.customerDate || '-').toString().replace(/</g, '&lt;').replace(/>/g, '&gt;');
                    tbody.append('<tr><td>' + name + '</td><td>' + itOb + '</td><td>' + jobId + '</td><td>' + marketing + '</td><td>' + picName + '</td><td>' + contact + '</td><td>' + businessFields + '</td><td>' + status + '</td><td>' + customerDate + '</td><td style="text-align:right">' + count + '</td></tr>');
                }
            }
            window.loadStatusCustomerChart = function loadStatusCustomerChart() {
                var tbody = $('#tbodyStatusCustomer');
                var noDataEl = $('#pStatusTableNoData');
                tbody.html('<tr><td colspan="10" class="text-center text-muted">Loading...</td></tr>');
                noDataEl.hide();
                var search = ($('#txtSearchStatusCustomer').val() || '').trim();
                $.ajax({
                    type: 'POST',
                    url: 'dashboard_operational.aspx/GetCustomerTrainingChart',
                    data: JSON.stringify({ search: search, areaFilter: getAreaFilter(), dateFrom: getDateFrom(), dateTo: getDateTo() }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (r) {
                        var d = r && r.d;
                        var data = (d && (d.Data || d.data));
                        if (!d || !d.Success && !d.success) {
                            tbody.html('<tr><td colspan="10" class="text-center text-danger">' + (d && (d.Error || d.error)) + '</td></tr>');
                            return;
                        }
                        if (!data || !Array.isArray(data)) {
                            customerChartFullData = [];
                            noDataEl.show();
                            tbody.empty();
                            return;
                        }
                        customerChartFullData = data;
                        renderStatusTable();
                    },
                    error: function (xhr, st, err) {
                        tbody.html('<tr><td colspan="10" class="text-center text-danger">Gagal memuat data</td></tr>');
                    }
                });
            };
            $(document).on('change', '#chkStatusExisting, #chkStatusNew, #chkStatusNewJO, #chkStatusTrial', function () { renderStatusTable(); });
            $(document).on('click', '#modalStatusCustomer thead th.sortable', function () {
                var col = $(this).attr('data-sort') || $(this).data('sort');
                if (!col) return;
                if (col === statusCustomerSortCol) statusCustomerSortDir = statusCustomerSortDir === 'asc' ? 'desc' : 'asc';
                else { statusCustomerSortCol = col; statusCustomerSortDir = (col === 'TrainingCount' ? 'desc' : 'asc'); }
                renderStatusTable();
            });
            $(document).on('click', '#btnSearchStatusCustomer', function () { loadStatusCustomerChart(); });
            $(document).on('click', '#btnClearStatusCustomer', function () { $('#txtSearchStatusCustomer').val(''); loadStatusCustomerChart(); });
            $(document).on('keypress', '#txtSearchStatusCustomer', function (e) { if (e.which === 13) { e.preventDefault(); loadStatusCustomerChart(); } });
            function closeDetailCustomerModal() {
                $('#modalDetailCustomer').removeClass('in').css('display', 'none').attr('aria-hidden', 'true');
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css('padding-right', '');
            }
            $(document).off('click.detailCustomerClose').on('click.detailCustomerClose', '#modalDetailCustomer [data-dismiss="modal"], #modalDetailCustomer .modal-header button.close', function (e) {
                e.preventDefault();
                closeDetailCustomerModal();
            });
            $('#modalStatusCustomer').on('show.bs.modal', function () { loadStatusCustomerChart(); });
        })();
        function renderChartOperational() {
            var rawId = '<%= chartOperationalData.ClientID %>';
            var raw = document.getElementById(rawId);
            var noDataEl = document.getElementById('chartOperationalNoData');
            var chartDom = document.getElementById('chartOperational');
            if (!noDataEl || !chartDom) return;
            if (raw && raw.value) {
                try {
                    var d = JSON.parse(raw.value);
                    if (d && d.labels && d.labels.length > 0 && typeof echarts !== 'undefined') {
                        noDataEl.style.display = 'none';
                        chartDom.style.display = 'block';
                        if (window.__chartOperationalInstance) try { window.__chartOperationalInstance.dispose(); } catch (e) {}
                        window.__chartOperationalInstance = echarts.init(chartDom);
                        var option = {
                            tooltip: { trigger: 'item', confine: true },
                            legend: { data: ['Realisasi', 'Target'], bottom: 0 },
                            grid: { left: '3%', right: '4%', bottom: '12%', top: '3%', containLabel: true },
                            xAxis: { type: 'category', data: d.labels, axisLabel: { rotate: 30 } },
                            yAxis: { type: 'value', min: 0 },
                            series: [
                                { name: 'Realisasi', type: 'bar', data: d.dataRealisasi || [], itemStyle: { color: '#ffa500' } },
                                { name: 'Target', type: 'bar', data: d.dataTarget || [], itemStyle: { color: '#808080' } }
                            ]
                        };
                        window.__chartOperationalInstance.setOption(option);
                        window.__chartOperationalInstance.resize();
                    } else {
                        noDataEl.style.display = 'block';
                        chartDom.style.display = 'none';
                    }
                } catch (e) {
                    noDataEl.style.display = 'block';
                    if (chartDom) chartDom.style.display = 'none';
                }
            } else {
                noDataEl.style.display = 'block';
                if (chartDom) chartDom.style.display = 'none';
            }
        }
        (function () { renderChartOperational(); })();
        if (typeof Sys !== 'undefined' && Sys.Application && Sys.Application.add_load) {
            Sys.Application.add_load(function () {
                renderChartOperational();
                if (window.__chartOperationalInstance) setTimeout(function () { window.__chartOperationalInstance.resize(); }, 100);
            });
        }
        function getDateRangeByPeriod(period) {
            var dt = new Date();
            var df = new Date();
            if (period === 'Mingguan') {
                df.setDate(dt.getDate() - 6);
            } else if (period === 'Bulanan') {
                df.setDate(1);
            } else {
                df = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate());
            }
            return { dateFrom: df.toISOString().slice(0, 10), dateTo: dt.toISOString().slice(0, 10) };
        }
        (function () {
            var ns = 'teamPerforma';
            function renderTeamPage(data) {
                var tbody = $('#tbodyTeam');
                if (!tbody.length) return;
                if (!data || data.length === 0) {
                    tbody.html('<tr><td colspan="5" class="text-center text-muted">Belum ada data</td></tr>');
                } else {
                    var html = '';
                    for (var i = 0; i < data.length; i++) {
                        var r = data[i];
                        var nama = (r.Nama || r.nama || '').replace(/"/g, '&quot;');
                        var userId = (r.UserID || r.userId || '').replace(/"/g, '&quot;');
                        var prog = Math.min(100, r.Progress || 0);
                        html += '<tr class="clickable-row" data-nama="' + nama + '" data-userid="' + userId + '" style="cursor: pointer;"><td>' + (r.Nama || '') + '</td><td>' + (r.Target || 0).toLocaleString() + '</td><td>' + (r.Realisasi || 0).toLocaleString() + '</td>';
                        html += '<td><div class="progress progress-xs"><div class="progress-bar progress-bar-success" style="width:' + prog + '%"></div></div><small>' + prog + '%</small></td>';
                        html += '<td>' + (r.Cycle || 0) + '</td></tr>';
                    }
                    tbody.html(html);
                }
            }
            function loadTeamPerforma() {
                var df = getDateFrom(), dt = getDateTo();
                var search = ($('#txtSearchTeam').val() || '').trim();
                $('#spanTeamPeriod').text((df && dt) ? (df + ' - ' + dt) : 'Periode');
                var tbody = $('#tbodyTeam');
                if (tbody.length) tbody.html('<tr><td colspan="5" class="text-center text-muted">Loading...</td></tr>');
                $.ajax({
                    type: 'POST',
                    url: 'dashboard_operational.aspx/GetTeamPerforma',
                    data: JSON.stringify({ dateFrom: df, dateTo: dt, pageIndex: 1, pageSize: 99999, search: search, excludeWeekend: $('#chkExcludeWeekend').is(':checked'), excludeHoliday: $('#chkExcludeHoliday').is(':checked'), areaFilter: getAreaFilter() }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    complete: function () {
                        window._teamPerformaLoaded = true;
                        $(document).trigger('teamPerformaLoaded');
                        if (typeof window.hideFilterLoading === 'function') window.hideFilterLoading();
                    },
                    success: function (r) {
                        if (r.d && r.d.Success) {
                            if ((r.d.TotalCount || 0) === 0 && search !== '' && tbody.length) {
                                var msg = 'Tidak ada staff yang cocok dengan \'' + $('<div/>').text(search).html() + '\'. Kosongkan pencarian untuk menampilkan semua.';
                                tbody.html('<tr><td colspan="5" class="text-center text-muted">' + msg + '</td></tr>');
                            } else {
                                renderTeamPage(r.d.Data || []);
                            }
                        } else {
                            if (tbody.length) tbody.html('<tr><td colspan="5" class="text-center text-muted">Gagal memuat data</td></tr>');
                        }
                    },
                    error: function () {
                        if (tbody.length) tbody.html('<tr><td colspan="5" class="text-center text-danger">Gagal memuat data</td></tr>');
                    }
                });
            }
            function bindTeamPerformaHandlers() {
                $(document).off('keypress.' + ns);
                $(document).on('keypress.' + ns, '#txtSearchTeam', function (e) {
                    if (e.which === 13) { e.preventDefault(); loadTeamPerforma(); return false; }
                });
            }
            window.loadTeamPerforma = loadTeamPerforma;
            bindTeamPerformaHandlers();
            loadTeamPerforma();
            if (typeof Sys !== 'undefined' && Sys.Application && Sys.Application.add_load) {
                Sys.Application.add_load(function () {
                    $('#txtSearchTeam').val('');
                    bindTeamPerformaHandlers();
                    loadTeamPerforma();
                });
            }
        })();
        function getDateFrom() { return ($('#hidDateFrom').val() || '').trim(); }
        function getDateTo() { return ($('#hidDateTo').val() || '').trim(); }
        function getAreaFilter() {
            var parts = [];
            if ($('#chkAreaWest').is(':checked')) parts.push('WEST');
            if ($('#chkAreaEast').is(':checked')) parts.push('EAST');
            return parts.join(',');
        }
        function syncDateRange() { var r = $('#dtRange').data('daterangepicker'); if (r && r.startDate && r.endDate) { $('#hidDateFrom').val(r.startDate.format('YYYY-MM-DD')); $('#hidDateTo').val(r.endDate.format('YYYY-MM-DD')); } }
        function refreshDashboardViaAjax() {
            var overlay = $('#filterLoadingOverlay');
            window._filterLoadingActive = true;
            if (overlay.length) overlay.addClass('show');
            try { sessionStorage.setItem('filterLoading', '1'); } catch (e) {}
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetOperationalDashboardData',
                data: JSON.stringify({
                    dateFrom: getDateFrom(),
                    dateTo: getDateTo(),
                    areaFilter: getAreaFilter(),
                    excludeWeekend: $('#chkExcludeWeekend').is(':checked'),
                    excludeHoliday: $('#chkExcludeHoliday').is(':checked')
                }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var d = r && r.d;
                    if (!d || !d.Success) {
                        if (overlay.length) overlay.removeClass('show');
                        if (typeof window.hideFilterLoading === 'function') window.hideFilterLoading();
                        return;
                    }
                    $('span[id$="lblTotalCustomers"]').text((d.Total || 0).toLocaleString('id-ID'));
                    $('span[id$="lblPriorityCount"]').text((d.Priority || 0).toLocaleString('id-ID'));
                    $('span[id$="lblRegularCount"]').text((d.Regular || 0).toLocaleString('id-ID'));
                    $('span[id$="lblTotalVisits"]').text((d.TotalVisit || 0).toLocaleString('id-ID'));
                    $('span[id$="lblTargetVisits"]').text((d.TargetVisit || 0).toLocaleString('id-ID'));
                    $('span[id$="lblVisitPeriod"]').text(d.VisitPeriod || '(Periode)');
                    $('span[id$="spanTeamPeriod"]').text(d.VisitPeriodDisplay || 'Periode');
                    $('span[id$="spanTeamPeriod"]').text(d.VisitPeriodDisplay || d.VisitPeriod || 'Periode');
                    var divProg = $('div[id$="divVisitProgress"]');
                    if (divProg.length) { var pct = d.VisitPct || 0; divProg.css('width', pct + '%').text(pct + '%'); }
                    $('span[id$="lblPendingVisits"], .count-belum-visit').text((d.PendingVisits || 0).toLocaleString('id-ID'));
                    $('span[id$="lblStatusExisting"]').text(d.PctExisting || 0);
                    $('span[id$="lblStatusNew"]').text(d.PctNew || 0);
                    $('span[id$="lblStatusTrial"]').text(d.PctTrial || 0);
                    var chartVal = d.ChartJson || '';
                    $('input[id$="chartOperationalData"]').val(chartVal);
                    if (typeof renderChartOperational === 'function') renderChartOperational();
                    if (typeof loadTeamPerforma === 'function') loadTeamPerforma();
                    if (overlay.length) overlay.removeClass('show');
                    if (typeof window.hideFilterLoading === 'function') window.hideFilterLoading();
                },
                error: function () {
                    if (overlay.length) overlay.removeClass('show');
                    if (typeof window.hideFilterLoading === 'function') window.hideFilterLoading();
                },
                complete: function () {
                    try { sessionStorage.removeItem('filterLoading'); } catch (e) {}
                }
            });
        }
        function initDtRangePicker() {
            var $el = $('#dtRange');
            if (!$el.length || typeof $.fn.daterangepicker === 'undefined') return;
            if ($el.data('daterangepicker')) $el.data('daterangepicker').remove();
            var df = getDateFrom(), dt = getDateTo();
            var start = (df && typeof moment !== 'undefined') ? moment(df) : moment().subtract(1, 'days');
            var end = (dt && typeof moment !== 'undefined') ? moment(dt) : moment().subtract(1, 'days');
            $el.daterangepicker({
                startDate: start,
                endDate: end,
                locale: { format: 'YYYY-MM-DD' },
                ranges: {
                    'Kemarin': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Minggu lalu': [moment().subtract(1, 'weeks').startOf('week'), moment().subtract(1, 'weeks').endOf('week')],
                    'Bulan lalu': [moment().subtract(1, 'months').startOf('month'), moment().subtract(1, 'months').endOf('month')],
                    'Tahun lalu': [moment().subtract(1, 'years').startOf('year'), moment().subtract(1, 'years').endOf('year')]
                }
            }, function (start, end, label) {
                $('#hidDateFrom').val(start.format('YYYY-MM-DD'));
                $('#hidDateTo').val(end.format('YYYY-MM-DD'));
                $el.val(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
                refreshDashboardViaAjax();
            });
            if (df && dt) $el.val(df + ' - ' + dt);
        }
        window.hideFilterLoading = function () {
            window._filterLoadingActive = false;
            $('#filterLoadingOverlay').removeClass('show');
        };
        $(function () {
            initDtRangePicker();
            syncDateRange();
            var overlay = $('#filterLoadingOverlay');
            function showFilterLoading() {
                try { sessionStorage.setItem('filterLoading', '1'); } catch (e) {}
                if (overlay.length) overlay.addClass('show');
            }
            /* Do not bind dtRange 'change' to showFilterLoading - daterangepicker can fire change on close without Apply, causing overlay to stay stuck. Apply callback handles date selection. */
            $('#chkExcludeWeekend, #chkExcludeHoliday, #chkAreaWest, #chkAreaEast').on('change', function () {
                showFilterLoading();
                refreshDashboardViaAjax();
            });
            $('form').on('submit', function () {
                var $f = $(this);
                if ($f.find('#dtRange, input[id$="chkExcludeWeekend"], input[id$="chkExcludeHoliday"], input[id$="chkAreaWest"], input[id$="chkAreaEast"]').length) {
                    showFilterLoading();
                }
            });
            if (typeof sessionStorage !== 'undefined' && sessionStorage.getItem('filterLoading') === '1') {
                sessionStorage.removeItem('filterLoading');
                window._filterLoadingActive = true;
                if (overlay.length) overlay.addClass('show');
                if (window._teamPerformaLoaded) {
                    setTimeout(function () { if (typeof window.hideFilterLoading === 'function') window.hideFilterLoading(); }, 100);
                }
            }
            $(document).on('teamPerformaLoaded', function () {
                if (window._filterLoadingActive && typeof window.hideFilterLoading === 'function') window.hideFilterLoading();
            });
        });

        function loadDetailBelumVisit() {
            var tbody = document.getElementById('tbodyDetailBelumVisitModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="6" class="text-center">Loading...</td></tr>';
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetBelumVisitCustomers',
                data: JSON.stringify({ dateFrom: getDateFrom(), dateTo: getDateTo(), areaFilter: getAreaFilter() }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var d = r && r.d;
                    var ok = d && (d.Success === true || d.success === true);
                    var data = (d && (d.Data || d.data));
                    var list = Array.isArray(data) ? data : [];
                    var cnt = (d && (d.Count != null ? d.Count : d.count));
                    if (typeof cnt !== 'number') cnt = list.length;
                    if ($('.count-belum-visit').length) $('.count-belum-visit').text((cnt | 0).toLocaleString('id-ID'));
                    if (!tbody) return;
                    if (ok && list.length > 0) {
                        var html = '';
                        for (var i = 0; i < list.length; i++) {
                            var row = list[i];
                            var tid = (row.TrainingID || row.trainingID || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            var nama = (row.NamaCustomer || row.namaCustomer || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            var assignee = (row.Assignee || row.assignee || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            var picName = (row.PICName || row.picName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            var contact = (row.ContactNumber || row.contactNumber || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            var biz = (row.BusinessFields || row.businessFields || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            html += '<tr><td>' + tid + '</td><td>' + nama + '</td><td>' + assignee + '</td><td>' + picName + '</td><td>' + contact + '</td><td>' + biz + '</td></tr>';
                        }
                        tbody.innerHTML = html;
                    } else {
                        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted">No Data</td></tr>';
                    }
                },
                error: function () {
                    if (tbody) tbody.innerHTML = '<tr><td colspan="6" class="text-center text-danger">Gagal memuat data</td></tr>';
                }
            });
        }
        function loadDetailVisit() {
            var tbody = document.getElementById('tbodyDetailVisitModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center">Loading...</td></tr>';
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetVisitDetail',
                data: JSON.stringify({ dateFrom: getDateFrom(), dateTo: getDateTo() }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var d = r && r.d;
                    var ok = d && (d.Success === true || d.success === true);
                    var data = (d && (d.Data || d.data));
                    var list = Array.isArray(data) ? data : [];
                    if (!tbody) return;
                    if (ok && list.length > 0) {
                        var html = '';
                        for (var i = 0; i < list.length; i++) {
                            var row = list[i];
                            html += '<tr><td>' + (row.Tanggal || row.tanggal || '') + '</td><td>' + (row.Waktu || row.waktu || '') + '</td><td>' + (row.ITStaff || row.itStaff || '') + '</td><td>' + (row.Customer || row.customer || '') + '</td><td>' + (row.StatusCustomer || row.statusCustomer || '') + '</td><td>' + (row.StatusVisit || row.statusVisit || '') + '</td><td>' + (row.Hasil || row.hasil || '') + '</td></tr>';
                        }
                        tbody.innerHTML = html;
                    } else {
                        tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted">Belum ada data kunjungan</td></tr>';
                    }
                },
                error: function () {
                    if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center text-danger">Gagal memuat data</td></tr>';
                }
            });
        }
        function loadDetailKunjungan() {
            var staffName = ($('#hidVisitDetailStaff').val() || '').trim();
            var tbody = document.getElementById('tbodyDetailKunjunganModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center">Loading...</td></tr>';
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetKunjunganDetail',
                data: JSON.stringify({ dateFrom: getDateFrom(), dateTo: getDateTo(), staffName: staffName }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var d = r && r.d;
                    var ok = d && (d.Success === true || d.success === true);
                    var data = (d && (d.Data || d.data));
                    var list = Array.isArray(data) ? data : [];
                    if (!tbody) return;
                    if (ok && list.length > 0) {
                        var html = '';
                        for (var i = 0; i < list.length; i++) {
                            var row = list[i];
                            var t = (row.Tanggal || row.tanggal || '');
                            var w = (row.Waktu || row.waktu || '');
                            var waktuVisit = (t + ' ' + w).trim();
                            var att = (row.Attachment || row.attachment || '').trim();
                            var attCell = att ? '<a href="Picture/' + encodeURIComponent(att) + '" target="_blank" rel="noopener" class="btn btn-xs btn-default" title="Buka lampiran"><i class="fa fa-paperclip"></i></a>' : '-';
                            var marketing = (row.MarketingName || row.marketingName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                            html += '<tr><td>' + (row.Customer || row.customer || '') + '</td><td>' + marketing + '</td><td>' + (row.ITStaff || row.itStaff || '') + '</td><td>' + waktuVisit + '</td><td>' + (row.StatusVisit || row.statusVisit || '') + '</td><td>' + (row.Hasil || row.hasil || '') + '</td><td class="text-center">' + attCell + '</td></tr>';
                        }
                        tbody.innerHTML = html;
                    } else {
                        tbody.innerHTML = '<tr><td colspan="7" class="text-center text-muted">Belum ada data kunjungan</td></tr>';
                    }
                },
                error: function () {
                    if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center text-danger">Gagal memuat data</td></tr>';
                }
            });
        }

        window.__detailCustomerData = [];
        window.__detailCustomerPage = 1;
        var PAGE_SIZE_DETAIL_CUSTOMER = 10;
        function renderDetailCustomerPage() {
            var list = window.__detailCustomerData || [];
            var page = window.__detailCustomerPage || 1;
            var pageSize = PAGE_SIZE_DETAIL_CUSTOMER;
            var totalPages = Math.max(1, Math.ceil(list.length / pageSize));
            if (page > totalPages) page = totalPages;
            window.__detailCustomerPage = page;
            var start = (page - 1) * pageSize;
            var slice = list.slice(start, start + pageSize);
            var tbody = $('#tbodyDetailCustomer');
            var emptyEl = $('#pDetailCustomerEmpty');
            var paginationEl = $('#paginationDetailCustomer');
            tbody.empty();
            if (list.length === 0) { emptyEl.show(); paginationEl.hide(); return; }
            emptyEl.hide();
            for (var i = 0; i < slice.length; i++) {
                var row = slice[i];
                var status = (row.Status || '').trim();
                if (status !== 'Existing' && status !== 'Trial' && status !== 'New') status = status || 'New';
                var picName = (row.PICName || row.picName || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                var contact = (row.ContactNumber || row.contactNumber || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                var biz = (row.BusinessFields || '').replace(/</g, '&lt;').replace(/>/g, '&gt;');
                var tr = '<tr><td>' + (row.NamaCustomer || '').replace(/</g, '&lt;') + '</td><td>' + (row.Tipe || '') + '</td><td>' + (row.Assignee || '') + '</td><td>' + picName + '</td><td>' + contact + '</td><td>' + status + '</td><td>' + biz + '</td></tr>';
                tbody.append(tr);
            }
            paginationEl.show();
            var from = list.length === 0 ? 0 : start + 1;
            var to = Math.min(start + pageSize, list.length);
            $('#lblDetailCustomerPaging').text(from + '-' + to + ' dari ' + list.length + ' data');
            var nav = $('#paginationDetailCustomerNav');
            nav.empty();
            nav.append('<li class="' + (page <= 1 ? 'disabled' : '') + '"><a href="#" data-page="prev">&laquo;</a></li>');
            var showPages = 5;
            var startPage = Math.max(1, page - Math.floor(showPages / 2));
            var endPage = Math.min(totalPages, startPage + showPages - 1);
            if (endPage - startPage + 1 < showPages) startPage = Math.max(1, endPage - showPages + 1);
            for (var p = startPage; p <= endPage; p++) {
                nav.append('<li class="' + (p === page ? 'active' : '') + '"><a href="#" data-page="' + p + '">' + p + '</a></li>');
            }
            nav.append('<li class="' + (page >= totalPages ? 'disabled' : '') + '"><a href="#" data-page="next">&raquo;</a></li>');
        }
        function loadDetailCustomer() {
            var search = ($('#txtSearchDetailCustomer').val() || '').trim();
            var tbody = $('#tbodyDetailCustomer');
            var emptyEl = $('#pDetailCustomerEmpty');
            var paginationEl = $('#paginationDetailCustomer');
            tbody.html('<tr><td colspan="7" class="text-center text-muted">Loading...</td></tr>');
            emptyEl.hide();
            paginationEl.hide();
            $.ajax({
                type: 'POST',
                url: 'dashboard_operational.aspx/GetCustomerList',
                data: JSON.stringify({ search: search, areaFilter: getAreaFilter() }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    var d = r && r.d;
                    var list = (d && d.Data) ? d.Data : (d && d.data ? d.data : []);
                    if (!Array.isArray(list)) list = [];
                    window.__detailCustomerData = list;
                    window.__detailCustomerPage = 1;
                    renderDetailCustomerPage();
                },
                error: function () { tbody.html('<tr><td colspan="7" class="text-center text-danger">Gagal memuat data</td></tr>'); }
            });
        }
        $(document).on('click', '#paginationDetailCustomerNav a[data-page]', function (e) {
            e.preventDefault();
            if ($(this).closest('li').hasClass('disabled')) return false;
            var p = $(this).data('page');
            var list = window.__detailCustomerData || [];
            var totalPages = Math.max(1, Math.ceil(list.length / PAGE_SIZE_DETAIL_CUSTOMER));
            if (p === 'prev' && window.__detailCustomerPage > 1) { window.__detailCustomerPage--; renderDetailCustomerPage(); }
            else if (p === 'next' && window.__detailCustomerPage < totalPages) { window.__detailCustomerPage++; renderDetailCustomerPage(); }
            else { var num = parseInt(p, 10); if (!isNaN(num) && num >= 1 && num <= totalPages) { window.__detailCustomerPage = num; renderDetailCustomerPage(); } }
            return false;
        });
        $(document).on('click', 'a[data-target="#modalDetailCustomer"]', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $('#modalDetailCustomer').modal('show');
            loadDetailCustomer();
            return false;
        });
        $(document).on('click', 'a[data-target="#modalStatusCustomer"]', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $('#modalStatusCustomer').modal('show');
            if (typeof window.loadStatusCustomerChart === 'function') window.loadStatusCustomerChart();
            return false;
        });
        $(document).on('click', '#btnSearchDetailCustomer', function () { loadDetailCustomer(); });
        $(document).on('click', '#btnClearDetailCustomer', function () { $('#txtSearchDetailCustomer').val(''); loadDetailCustomer(); });
        $(document).on('click', 'a[data-target="#modalDetailBelumVisit"]', function (e) {
            e.preventDefault();
            e.stopPropagation();
            syncDateRange();
            var tbody = document.getElementById('tbodyDetailBelumVisitModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="3" class="text-center">Loading...</td></tr>';
            $('#modalDetailBelumVisit').modal('show');
            loadDetailBelumVisit();
            return false;
        });
        $(document).on('click', 'a[data-target="#modalDetailVisit"]', function (e) {
            e.preventDefault();
            e.stopPropagation();
            syncDateRange();
            var tbody = document.getElementById('tbodyDetailVisitModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center">Loading...</td></tr>';
            $('#modalDetailVisit').modal('show');
            loadDetailVisit();
            return false;
        });
        $(document).on('click', 'a[data-target="#modalDetailKunjungan"]', function (e) {
            e.preventDefault();
            e.stopPropagation();
            syncDateRange();
            $('#hidVisitDetailStaff').val('');
            $('#modalDetailKunjunganLabel').text('Detail Kunjungan IT');
            var tbody = document.getElementById('tbodyDetailKunjunganModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center">Loading...</td></tr>';
            $('#modalDetailKunjungan').modal('show');
            loadDetailKunjungan();
            return false;
        });
        $(document).on('click', '#tbodyTeam tr.clickable-row', function (e) {
            e.preventDefault();
            e.stopPropagation();
            syncDateRange();
            var nama = $(this).attr('data-nama') || $(this).data('nama') || '';
            var userId = $(this).attr('data-userid') || $(this).data('userid') || '';
            $('#hidVisitDetailStaff').val((userId || nama).trim());
            $('#modalDetailKunjunganLabel').text(nama ? 'Detail Kunjungan: ' + nama : 'Detail Kunjungan IT');
            var tbody = document.getElementById('tbodyDetailKunjunganModal');
            if (tbody) tbody.innerHTML = '<tr><td colspan="7" class="text-center">Loading...</td></tr>';
            $('#modalDetailKunjungan').modal('show');
            loadDetailKunjungan();
            return false;
        });
    </script>

</asp:Content>
