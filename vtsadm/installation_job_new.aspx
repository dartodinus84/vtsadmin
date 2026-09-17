<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="installation_job_new.aspx.cs" Inherits="vtsadm.installation_job_new" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .ijm-page { overflow-x: hidden; }
        .ijm-page .form-control { background-color: #fff !important; }
        .ijm-page .box { border-radius: 8px; margin-bottom: 16px; box-shadow: 0 1px 3px rgba(0,0,0,.08); }
        .ijm-page .box-header .box-title { white-space: normal; line-height: 1.3; }
        .ijm-page .box-body { padding: 18px 20px; }
        .ijm-page .box-footer { padding: 12px 20px; background: #f9f9f9; border-top: 1px solid #eee; }
        .ijm-page .form-group-sm { margin-bottom: 14px; }
        .ijm-page .form-group-sm > label { font-size: 11px; font-weight: 600; color: #666; text-transform: uppercase; letter-spacing: .03em; margin-bottom: 5px; display: block; }
        .ijm-page .ijm-search-group { width: 100%; }
        .ijm-job-summary-wrap { margin-top: 8px; border-top: 1px dashed #d9e3ef; padding-top: 12px; }
        .ijm-job-summary-title { font-size: 12px; font-weight: 700; color: #003481; margin: 0 0 8px; text-transform: uppercase; letter-spacing: .03em; }
        .ijm-job-summary-table { font-size: 12px; margin-bottom: 0; }
        .ijm-job-summary-table th { background: #f5f8fc; color: #003481; white-space: nowrap; }
        .ijm-job-summary-table tbody tr.ijm-job-summary-row-closed > td { background-color: #ecf7ed !important; }
        .ijm-job-summary-table tbody tr.ijm-job-summary-row-open > td { background-color: #fffbea !important; }
        .ijm-job-summary-table tbody tr.ijm-job-summary-row-closed:hover > td { background-color: #dff2e3 !important; }
        .ijm-job-summary-table tbody tr.ijm-job-summary-row-open:hover > td { background-color: #fff3cd !important; }
        .ijm-job-status-label { font-size: 11px; font-weight: 600; display: inline-block; min-width: 78px; text-align: center; padding: 4px 8px; white-space: nowrap; }
        .ijm-job-status-label .fa { margin-right: 4px; font-size: 10px; }
        .ijm-job-status-label .fa-circle { font-size: 8px; vertical-align: middle; }
        .ijm-job-summary-empty { padding: 0; }
        .ijm-job-summary-card { border: 1px solid #d8e4f2; border-radius: 8px; background: linear-gradient(180deg, #fbfdff 0%, #f6faff 100%); padding: 14px 16px; display: flex; align-items: flex-start; gap: 12px; }
        .ijm-job-summary-icon { width: 34px; height: 34px; border-radius: 50%; background: #e8f1fb; color: #2f7db5; display: flex; align-items: center; justify-content: center; font-size: 16px; flex: 0 0 34px; }
        .ijm-job-summary-head { font-size: 13px; font-weight: 700; color: #244d74; margin: 1px 0 3px; }
        .ijm-job-summary-text { font-size: 12px; color: #64788d; margin: 0; line-height: 1.45; }
        .ijm-job-summary-state-loading .ijm-job-summary-icon { background: #e8f5ff; color: #1d76b8; }
        .ijm-job-summary-state-error .ijm-job-summary-icon { background: #fdecea; color: #dd4b39; }
        .ijm-info-group { border: 1px solid #e8e8e8; border-radius: 4px; padding: 14px 16px 10px; margin-bottom: 15px; background: #fcfcfc; height: 100%; }
        .ijm-group-title { font-size: 13px; font-weight: 700; color: #003481; margin: 0 0 12px; padding-bottom: 8px; border-bottom: 2px solid #3c8dbc; }
        .ijm-group-title i { margin-right: 6px; color: #3c8dbc; }
        .ijm-job-meta { margin-bottom: 16px; padding-bottom: 14px; border-bottom: 1px dashed #ddd; }
        .ijm-tech-meta { margin-bottom: 16px; padding-bottom: 14px; border-bottom: 1px dashed #ddd; }
        .ijm-info-row { margin-bottom: 10px; }
        .ijm-info-row:last-child { margin-bottom: 0; }
        .ijm-info-row label,
        .ijm-maint-input .ijm-info-row label { font-size: 11px; color: #888; margin-bottom: 3px; display: block; font-weight: 600; text-transform: uppercase; }
        .ijm-ro, .ijm-preview-val { background: #f5f7fa !important; color: #333; font-weight: 500; border: 1px solid #e3e8ef; min-height: 30px; box-shadow: none; word-break: break-word; }
        .ijm-maint-input { background: #fffdf7; border: 1px solid #f0ad4e; border-left: 4px solid #f39c12; border-radius: 8px; padding: 16px 18px; margin-bottom: 14px; }
        .ijm-maint-input > label { font-size: 13px; font-weight: 700; color: #003481; margin-bottom: 8px; display: block; }
        .ijm-maint-hint { font-size: 12px; color: #777; margin: 0 0 16px; padding: 10px 12px; background: #fff8e6; border-radius: 4px; border-left: 3px solid #f39c12; line-height: 1.5; }
        .ijm-preview-box { border: 1px solid #e8e8e8; border-radius: 4px; padding: 14px 16px 10px; background: #fafafa; margin-top: 12px; }
        .ijm-preview-title { font-size: 12px; font-weight: 700; color: #666; text-transform: uppercase; margin: 0 0 12px; }
        .ijm-req { color: #dd4b39; }
        .ijm-action-btns { text-align: right; }
        .ijm-action-btns .btn { min-width: 100px; margin-left: 6px; }
        .ijm-job-id-input, .ijm-pick-input { cursor: pointer; background-color: #fff !important; }
        .ijm-pick-input { border-left: 3px solid #3c8dbc; }
        .ijm-pick-input:hover { box-shadow: 0 0 0 2px rgba(60,141,188,.12); }
        .ijm-upload-input { position: absolute; left: -9999px; opacity: 0; width: 1px; height: 1px; }
        .ijm-upload-drop { display: block; border: 2px dashed #b9d8f3; background: linear-gradient(180deg, #f8fcff 0%, #f3f9ff 100%); border-radius: 10px; padding: 16px; text-align: center; cursor: pointer; transition: all .18s ease; margin-bottom: 8px; }
        .ijm-upload-drop:hover { border-color: #3c8dbc; box-shadow: 0 0 0 3px rgba(60,141,188,.12); }
        .ijm-upload-title { display: block; font-size: 13px; font-weight: 700; color: #003481; margin-bottom: 4px; }
        .ijm-upload-meta { display: block; font-size: 12px; color: #5f6f81; margin-bottom: 10px; }
        .ijm-upload-btn { display: inline-block; border-radius: 20px; padding: 6px 14px; background: #3c8dbc; color: #fff; font-size: 12px; font-weight: 600; }
        .ijm-attach-hint { font-size: 12px; color: #777; margin-top: 6px; }
        .ijm-attach-preview { margin-top: 10px; display: grid; grid-template-columns: repeat(auto-fill, minmax(116px, 1fr)); gap: 10px; }
        .ijm-attach-item { border: 1px solid #dfe7f0; border-radius: 8px; padding: 6px; background: #fff; position: relative; box-shadow: 0 1px 2px rgba(0,0,0,.04); }
        .ijm-attach-item img { width: 100%; height: 88px; object-fit: cover; border-radius: 6px; display: block; }
        .ijm-attach-name { font-size: 10px; color: #556577; margin-top: 6px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
        .ijm-attach-remove { position: absolute; top: -8px; right: -8px; width: 22px; height: 22px; border: 0; border-radius: 50%; background: #dd4b39; color: #fff; font-size: 14px; line-height: 20px; text-align: center; cursor: pointer; padding: 0; box-shadow: 0 1px 4px rgba(0,0,0,.2); }
        .ijm-picker-modal { z-index: 1060 !important; }
        body > .modal-backdrop { z-index: 1040 !important; }
        .ijm-picker-modal .form-control { background-color: #fff !important; }
        .ijm-picker-modal .modal-header {
            background: linear-gradient(135deg, #003481 0%, #3c8dbc 100%);
            color: #fff;
            border-radius: 8px 8px 0 0;
        }
        .ijm-picker-modal .modal-header .close { color: #fff; opacity: .85; }
        .ijm-picker-body { max-height: 480px; overflow-y: auto; -webkit-overflow-scrolling: touch; }
        .ijm-acc-toolbar { margin-bottom: 10px; display: flex; flex-wrap: wrap; gap: 8px; align-items: center; }
        .ijm-acc-search { flex: 1 1 240px; max-width: 320px; }
        .ijm-acc-req-hint { font-size: 12px; font-weight: 600; }
        .ijm-acc-req-hint.ijm-acc-req-ok { color: #3c763d; }
        .ijm-acc-req-hint.ijm-acc-req-warn { color: #c87f0a; }
        .ijm-acc-req-hint.ijm-acc-req-err { color: #dd4b39; }
        .ijm-acc-table { font-size: 12px; margin-bottom: 0; }
        .ijm-acc-table th { background: #f5f7fa; color: #003481; white-space: nowrap; }
        .ijm-acc-table td { vertical-align: middle !important; }
        .ijm-acc-table tbody tr:hover { background: #f9fbfd; }
        .ijm-acc-wrap { max-height: 340px; overflow-y: auto; border: 1px solid #edf2f7; border-radius: 4px; }
        .ijm-list-loading, .ijm-list-empty { padding: 20px; text-align: center; color: #666; }
        .ijm-list-loading { color: #003481; }
        .ijm-list-error { margin: 10px 0; }
        .ijm-picker-table { font-size: 12px; margin-bottom: 0; }
        .ijm-picker-table th { background: #f5f7fa; color: #003481; white-space: nowrap; }
        .ijm-customer-row .ijm-info-group { margin-bottom: 0; }
        #pnlChannelSetting { margin-bottom: 16px; }
        #pnlChannelSetting .table { background: #fff; }
        .ijm-channel-wrap { margin-bottom: 8px; }
        .ijm-tech-pick-row { margin-top: 0; margin-left: -8px; margin-right: -8px; display: flex; flex-wrap: wrap; align-items: flex-end; }
        .ijm-tech-pick-row > [class*="col-"] { padding-left: 8px; padding-right: 8px; }
        .ijm-tech-pick-row .form-group-sm { margin-bottom: 0; }
        .ijm-tech-pick-row .form-group-sm > label { min-height: 16px; }
        .ijm-tech-pick-row .ijm-search-group { width: 100%; }
        .ijm-tech-pick-row .ijm-ro {
            display: flex;
            align-items: center;
            min-height: 30px;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        @media (min-width: 992px) {
            .ijm-detail-cards .ijm-info-group { min-height: 160px; margin-bottom: 0; }
            .ijm-detail-cards > [class*="col-"] { margin-bottom: 0; }
            .ijm-job-row > [class*="col-"] { margin-bottom: 0; }
            .ijm-job-meta > [class*="col-"] { margin-bottom: 0; }
            .ijm-tech-meta > [class*="col-"] { margin-bottom: 0; }
            .ijm-install-row > [class*="col-"] { margin-bottom: 0; }
            .ijm-customer-row .ijm-info-group { min-height: 120px; }
            .ijm-tech-pick-row > [class*="col-"] { margin-bottom: 0; }
        }

        @media (min-width: 768px) {
            .ijm-pick-grid {
                display: flex;
                flex-wrap: wrap;
            }
            .ijm-pick-grid > [class*="col-"] {
                display: flex;
                flex-direction: column;
            }
            .ijm-pick-grid .ijm-maint-input {
                display: flex;
                flex-direction: column;
                flex: 1 1 auto;
                height: 100%;
                margin-bottom: 0;
            }
            .ijm-pick-grid .ijm-preview-box { flex: 1 1 auto; }
        }

        @media (max-width: 991px) {
            .content-header { padding: 12px 12px 0; }
            .content-header > h1 { font-size: 20px; margin: 0 0 8px; }
            .ijm-page { padding: 10px 8px 110px; }
            .ijm-page .box { margin-bottom: 12px; }
            .ijm-page .box-body { padding: 14px 12px; }
            .ijm-page .box-header { padding: 10px 12px; }
            .ijm-page .box-header .box-tools { display: none; }
            .ijm-action-wrap {
                position: fixed;
                left: 0;
                right: 0;
                bottom: 0;
                z-index: 1040;
                margin: 0;
                box-shadow: 0 -4px 16px rgba(0,0,0,.14);
            }
            .ijm-action-wrap .box { margin: 0; border-radius: 0; border-left: 0; border-right: 0; border-bottom: 0; }
            .ijm-action-wrap .box-footer { padding: 10px 12px; }
            .ijm-action-btns { display: flex; flex-direction: column-reverse; gap: 8px; text-align: center; }
            .ijm-action-btns .btn { width: 100%; margin: 0; min-height: 44px; font-size: 16px; }
            body.embed-mode .ijm-action-wrap { bottom: 0; padding-bottom: env(safe-area-inset-bottom, 0px); }
            body.embed-mode .ijm-page { padding-bottom: 96px; }
            .ijm-pick-grid > [class*="col-"] { margin-bottom: 12px; }
            .ijm-tech-pick-row > [class*="col-"] { margin-bottom: 10px; }
        }

        @media (max-width: 767px) {
            .content-header .breadcrumb { display: none; }
            .ijm-page { padding-bottom: 160px; }
            .ijm-action-wrap { bottom: 60px; }
            .ijm-job-row > [class*="col-"],
            .ijm-job-meta > [class*="col-"],
            .ijm-tech-meta > [class*="col-"],
            .ijm-customer-row > [class*="col-"],
            .ijm-install-row > [class*="col-"],
            .ijm-tech-pick-row > [class*="col-"],
            .ijm-install-fields > [class*="col-"] { margin-bottom: 12px; }
            .ijm-maint-input { padding: 14px 12px; margin-bottom: 0; }
            .ijm-page .form-control,
            .ijm-page select.form-control,
            .ijm-page textarea.form-control { min-height: 44px; font-size: 16px; }
            .ijm-page .input-group-sm > .form-control,
            .ijm-page .input-group-sm > .input-group-btn > .btn {
                height: 44px;
                padding: 8px 12px;
                font-size: 16px;
                line-height: 1.35;
            }
            .ijm-page .input-group-sm > .input-group-btn > .btn { min-width: 48px; }
            .ijm-ro, .ijm-preview-val { min-height: 44px; padding-top: 10px; }
            .ijm-job-summary-card { padding: 12px; }
            .ijm-job-summary-head { font-size: 13px; }
            .ijm-upload-drop { padding: 16px 10px; }
            .ijm-upload-title { font-size: 14px; }
            .ijm-upload-btn { min-height: 36px; line-height: 24px; }
            .ijm-attach-preview { grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 8px; }
            .ijm-attach-item img { height: 92px; }
            .ijm-acc-search { max-width: none; width: 100%; flex: 1 1 100%; }
            .ijm-acc-toolbar .btn { min-height: 40px; }
            .ijm-acc-wrap { max-height: none; }
            .ijm-stack-wrap {
                border: 0 !important;
                max-height: none !important;
                overflow: visible !important;
            }
            .ijm-stack-table thead { display: none; }
            .ijm-stack-table,
            .ijm-stack-table tbody,
            .ijm-stack-table tr,
            .ijm-stack-table td {
                display: block;
                width: 100% !important;
            }
            .ijm-stack-table tbody tr {
                margin-bottom: 10px;
                border: 1px solid #d8e4f2;
                border-radius: 8px;
                overflow: hidden;
                background: #fff;
                box-shadow: 0 1px 2px rgba(0,0,0,.04);
            }
            .ijm-stack-table tbody tr:last-child { margin-bottom: 0; }
            .ijm-stack-table td {
                display: flex;
                justify-content: space-between;
                align-items: flex-start;
                gap: 10px;
                padding: 9px 12px !important;
                border: 0 !important;
                border-bottom: 1px solid #eef3f8 !important;
                text-align: right;
                white-space: normal;
                word-break: break-word;
                vertical-align: top !important;
            }
            .ijm-stack-table td:last-child { border-bottom: 0 !important; }
            .ijm-stack-table td:before {
                content: attr(data-label);
                font-weight: 700;
                font-size: 11px;
                color: #003481;
                text-transform: uppercase;
                letter-spacing: .03em;
                text-align: left;
                flex: 0 0 42%;
            }
            .ijm-job-summary-table tbody tr.ijm-job-summary-row-closed > td,
            .ijm-job-summary-table tbody tr.ijm-job-summary-row-open > td,
            .ijm-acc-table tbody tr:hover { background: transparent !important; }
            .ijm-job-summary-table tbody tr.ijm-job-summary-row-closed { background: #ecf7ed; }
            .ijm-job-summary-table tbody tr.ijm-job-summary-row-open { background: #fffbea; }
            .ijm-picker-modal .modal-dialog {
                width: auto;
                margin: 8px;
            }
            .ijm-picker-modal .modal-content { border-radius: 10px; }
            .ijm-picker-body { max-height: calc(100vh - 150px) !important; }
            .ijm-picker-table .btn-ijn-pick { width: 100%; min-height: 40px; }
            .ijm-picker-modal .form-control { min-height: 44px; font-size: 16px; }
            .ijm-picker-modal .input-group-sm > .form-control,
            .ijm-picker-modal .input-group-sm > .input-group-btn > .btn {
                height: 44px;
                padding: 8px 12px;
                font-size: 16px;
                line-height: 1.35;
            }
            .ijm-picker-modal .input-group-sm > .input-group-btn > .btn { min-width: 48px; }
            #channelSettingTable thead { display: none; }
            #channelSettingTable,
            #channelSettingTable tbody,
            #channelSettingTable tr,
            #channelSettingTable td { display: block; width: 100%; }
            #channelSettingTable tr {
                margin-bottom: 8px;
                border: 1px solid #d8e4f2;
                border-radius: 8px;
                overflow: hidden;
            }
            #channelSettingTable td { border: 0; padding: 8px 12px; }
            #btnSaveChannelSetting { width: 100%; min-height: 44px; }
            .ijm-acc-chk,
            #ijmAccCheckAllHead { width: 20px; height: 22px; }
            .ijm-attach-remove { width: 28px; height: 28px; line-height: 26px; font-size: 16px; top: -6px; right: -6px; }
        }
    </style>

    <section class="content-header">
        <h1>New Installation Job</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">Installation</a></li>
            <li class="active">New Install</li>
        </ol>
    </section>

    <section class="content ijm-page">
        <div id="div_validation" runat="server"></div>
        <div id="div_comment" runat="server" style="display:none;"></div>

        <div class="row">
            <div class="col-md-12 col-xs-12">

                <!-- Job Information -->
                <div class="box box-primary box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-briefcase"></i> Job Information</h3>
                        <div class="box-tools pull-right">
                            <span class="label label-primary">Required</span>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row ijm-job-row">
                            <div class="col-lg-5 col-md-7 col-xs-12">
                                <div class="form-group form-group-sm">
                                    <label>Job ID <span class="ijm-req">*</span></label>
                                    <div class="input-group input-group-sm ijm-search-group">
                                        <input type="text" id="txtJobID" class="form-control ijm-job-id-input" placeholder="Klik tombol cari..." readonly="readonly" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-primary btn-sm btn-ijn-open" data-picker="job" title="Cari Job ID"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ijm-job-summary-wrap">
                            <p class="ijm-job-summary-title"><i class="fa fa-list-alt"></i> Job Detail Information</p>
                            <div id="ijmJobSummaryEmpty" class="ijm-job-summary-empty"></div>
                            <div class="table-responsive ijm-stack-wrap" id="ijmJobSummaryWrap" style="display:none;">
                                <table class="table table-bordered table-striped ijm-job-summary-table ijm-stack-table">
                                    <thead>
                                        <tr>
                                            <th>Device Group</th>
                                            <th>Device Type</th>
                                            <th class="text-right">Qty Request</th>
                                            <th class="text-right">Qty Done</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody id="ijmJobSummaryBody"></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Installation Input -->
                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-edit"></i> Installation Input</h3>
                        <div class="box-tools pull-right">
                            <span class="label label-warning">Input</span>
                        </div>
                    </div>
                    <div class="box-body">
                        <p class="ijm-maint-hint"><i class="fa fa-info-circle"></i> Pilih data instalasi berikut secara berurutan. Detail referensi job tersedia di bagian bawah.</p>

                        <div class="row ijm-install-row">
                            <div class="col-md-12 col-xs-12">
                                <div class="ijm-maint-input">
                                    <div class="row ijm-tech-pick-row">
                                        <div class="col-md-6 col-sm-12 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label><i class="fa fa-user-md"></i> Technician ID <span class="ijm-req">*</span></label>
                                                <div class="input-group input-group-sm ijm-search-group">
                                                    <input type="text" id="txtTechnicianID" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                                    <span class="input-group-btn">
                                                        <button type="button" class="btn btn-primary btn-sm btn-ijn-open" data-picker="tech" title="Cari Technician"><i class="fa fa-search"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Name</label>
                                                <div class="ijm-ro form-control input-sm" data-field="TechnicianName">-</div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Branch Name</label>
                                                <div class="ijm-ro form-control input-sm" data-field="TechBranchName">-</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row ijm-install-row ijm-pick-grid">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="ijm-maint-input">
                                    <label><i class="fa fa-car"></i> Police No <span class="ijm-req">*</span></label>
                                    <div class="input-group input-group-sm ijm-search-group">
                                        <input type="text" id="txtPoliceNo" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-primary btn-sm btn-ijn-open" data-picker="vehicle"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                    <div class="ijm-preview-box">
                                        <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail Kendaraan</p>
                                        <div class="ijm-info-row">
                                            <label>Vehicle Description</label>
                                            <input type="text" id="txtVehicleDesc" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Asset No</label>
                                            <input type="text" id="txtAssetNo" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="ijm-maint-input">
                                    <label><i class="fa fa-hdd-o"></i> No SN <span class="ijm-req">*</span></label>
                                    <div class="input-group input-group-sm ijm-search-group">
                                        <input type="text" id="txtNoSN" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-primary btn-sm btn-ijn-open" data-picker="device"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                    <div class="ijm-preview-box">
                                        <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail Device</p>
                                        <div class="ijm-info-row">
                                            <label>Device Type Description</label>
                                            <input type="text" id="txtDeviceTypeDesc" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <input type="text" id="txtWarehouseName" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-12 col-sm-12 col-xs-12">
                                <div class="ijm-maint-input">
                                    <label><i class="fa fa-mobile"></i> No GSM <span class="ijm-req">*</span></label>
                                    <div class="input-group input-group-sm ijm-search-group">
                                        <input type="text" id="txtNoGSM" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-primary btn-sm btn-ijn-open" data-picker="gsm"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                    <div class="ijm-preview-box">
                                        <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail GSM</p>
                                        <div class="ijm-info-row">
                                            <label>Provider Name</label>
                                            <input type="text" id="txtProviderName" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <input type="text" id="txtGsmWarehouseName" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row ijm-install-row">
                            <div class="col-md-12 col-xs-12">
                                <div class="ijm-maint-input">
                                    <p class="ijm-group-title"><i class="fa fa-info-circle"></i> Installation Information</p>
                                    <div id="pnlChannelSetting" class="form-group form-group-sm" style="display: none;">
                                        <label>Setting Channel MDVR</label>
                                        <div id="channelSettingAlert"></div>
                                        <div id="channelSettingLoading" style="display: none; margin-bottom: 8px;">
                                            <i class="fa fa-circle-o-notch fa-spin"></i> Memuat channel...
                                        </div>
                                        <div class="table-responsive ijm-channel-wrap">
                                        <table class="table table-bordered" id="channelSettingTable" style="display: none; margin-bottom: 8px;">
                                            <thead>
                                                <tr>
                                                    <th style="width: 55%;">Channel</th>
                                                    <th>Tipe Channel</th>
                                                </tr>
                                            </thead>
                                            <tbody id="channelSettingRows"></tbody>
                                        </table>
                                        </div>
                                        <button type="button" class="btn btn-primary btn-sm" id="btnSaveChannelSetting" disabled="disabled">
                                            <i class="fa fa-save"></i>&nbsp;Save Setting Channel
                                        </button>
                                    </div>
                                    <div class="row ijm-install-fields">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Server Name <span class="ijm-req">*</span></label>
                                                <select id="cmbServerName" class="form-control input-sm">
                                                    <option value="">[Select]</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>User Akses</label>
                                                <select id="cmbUserAccess" class="form-control input-sm">
                                                    <option value="">[Select]</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Install Date <span class="ijm-req">*</span></label>
                                                <input type="date" id="txtInstallDate" class="form-control input-sm" />
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Relay</label>
                                                <select id="cmbRelay" class="form-control input-sm">
                                                    <option value="1">Yes</option>
                                                    <option value="0" selected="selected">No</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Remarks</label>
                                                <textarea id="txtRemarks" class="form-control input-sm" rows="3" maxlength="500" placeholder="Catatan instalasi..."></textarea>
                                            </div>
                                        </div>
                                        <div class="col-md-12 col-xs-12">
                                            <div class="form-group form-group-sm">
                                                <label>Picture Attachment</label>
                                                <input type="file" id="fuInstallPictures" class="ijm-upload-input" accept=".jpg,.jpeg,.png,image/jpeg,image/png" multiple="multiple" />
                                                <label for="fuInstallPictures" class="ijm-upload-drop">
                                                    <span class="ijm-upload-title"><i class="fa fa-cloud-upload"></i> Browse / Drop Gambar</span>
                                                    <span class="ijm-upload-meta" id="ijmUploadMeta">Belum ada file dipilih</span>
                                                    <span class="ijm-upload-btn">Pilih File</span>
                                                </label>
                                                <div class="ijm-attach-hint">Maksimal 10 file. Format: jpg, jpeg, png.</div>
                                                <div id="ijmPicturePreview" class="ijm-attach-preview" style="display:none;"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Accessories -->
                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-puzzle-piece"></i> List Accessories</h3>
                    </div>
                    <div class="box-body">
                        <div class="ijm-acc-toolbar">
                            <div class="input-group input-group-sm ijm-acc-search">
                                <input type="text" id="txtAccSearch" class="form-control" placeholder="Cari No SN..." maxlength="50" />
                                <span class="input-group-btn">
                                    <button type="button" class="btn btn-warning btn-sm" id="btnAccSearch"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                            <button type="button" class="btn btn-default btn-sm" id="btnAccCheckAll"><i class="fa fa-check-square-o"></i> Check All</button>
                            <button type="button" class="btn btn-default btn-sm" id="btnAccUncheckAll"><i class="fa fa-square-o"></i> Uncheck All</button>
                            <span class="text-muted" id="ijmAccCount" style="font-size:12px;"></span>
                            <span id="ijmAccReqHint" class="ijm-acc-req-hint" style="display:none;"></span>
                        </div>
                        <div class="ijm-list-loading ijm-acc-loading"><i class="fa fa-spinner fa-spin"></i> Memuat accessories...</div>
                        <div class="ijm-list-empty ijm-acc-empty" style="display:none;">Lengkapi data instalasi untuk memuat list accessories.</div>
                        <div class="ijm-list-error alert alert-danger ijm-acc-error" style="display:none;"></div>
                        <div class="table-responsive ijm-acc-wrap ijm-stack-wrap" style="display:none;">
                            <table class="table table-bordered table-striped table-hover ijm-acc-table ijm-stack-table">
                                <thead>
                                    <tr>
                                        <th class="text-center" style="width:44px;">
                                            <input type="checkbox" id="ijmAccCheckAllHead" title="Check all" />
                                        </th>
                                        <th>Device ID</th>
                                        <th>No SN</th>
                                        <th>Device Type Desc</th>
                                        <th>Status</th>
                                    </tr>
                                </thead>
                                <tbody id="ijmAccBody"></tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Detail Information -->
                <div class="box box-solid ijm-detail-ref">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-info-circle"></i> Detail Information</h3>
                        <div class="box-tools pull-right">
                            <span class="label label-default">Referensi</span>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row ijm-job-meta">
                            <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                <label>Register Date</label>
                                <div class="ijm-ro form-control input-sm" data-field="RegDate">-</div>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                <label>Schedule Date</label>
                                <div class="ijm-ro form-control input-sm" data-field="SchDate">-</div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-xs-12 ijm-info-row">
                                <label>SO ID</label>
                                <div class="ijm-ro form-control input-sm" data-field="SoID">-</div>
                            </div>
                        </div>

                        <div class="row ijm-tech-meta">
                            <div class="col-xs-12">
                                <p class="ijm-group-title"><i class="fa fa-user-md"></i> Technician</p>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                <label>Technician ID</label>
                                <div class="ijm-ro form-control input-sm" data-field="TechnicianID">-</div>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                <label>Name</label>
                                <div class="ijm-ro form-control input-sm" data-field="TechnicianName">-</div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-xs-12 ijm-info-row">
                                <label>Branch Name</label>
                                <div class="ijm-ro form-control input-sm" data-field="TechBranchName">-</div>
                            </div>
                        </div>

                        <div class="row ijm-customer-row">
                            <div class="col-md-6 col-sm-12 col-xs-12">
                                <div class="ijm-info-group">
                                    <p class="ijm-group-title"><i class="fa fa-building"></i> Customer</p>
                                    <div class="row">
                                        <div class="col-md-12 col-xs-12 ijm-info-row">
                                            <label>Customer Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="CustomerName">-</div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12 ijm-info-row">
                                            <label>Branch Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="BranchName">-</div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12 ijm-info-row">
                                            <label>Marketing Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="MarketingName">-</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Actions -->
                <div class="ijm-action-wrap">
                    <div class="box box-solid">
                        <div class="box-footer ijm-action-btns">
                            <button type="button" class="btn btn-primary" id="btnSaveClient" disabled="disabled" title="Save akan diaktifkan setelah integrasi submit"><i class="fa fa-save"></i> Save</button>
                            <button id="CmdCancel" type="button" class="btn btn-default" runat="server" onserverclick="CmdCancel_ServerClick"><i class="fa fa-times"></i> Cancel</button>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!-- Hidden FK -->
        <input type="hidden" id="hfCustID" />
        <input type="hidden" id="hfJobID" />
        <input type="hidden" id="hfTechnicianID" />
        <input type="hidden" id="hfTvaID" />
        <input type="hidden" id="hfVehicleID" />
        <input type="hidden" id="hfTdtID" />
        <input type="hidden" id="hfDeviceID" />
        <input type="hidden" id="hfTgtID" />
        <input type="hidden" id="hfGsmID" />
        <input type="hidden" id="hfTvdID" />
        <input type="hidden" id="hfTechName" />
        <input type="hidden" id="hfPoID" />
        <input type="hidden" id="hfDeviceServerID" />
        <input type="hidden" id="hfDeviceContractTime" />
        <input type="hidden" id="hfDeviceBrand" />
        <input type="hidden" id="hfDeviceModel" />
        <input type="hidden" id="hfSelectedAcc" />

        <!-- Generic Picker Modal -->
        <div class="modal fade ijm-picker-modal" id="modal-ijn-picker" tabindex="-1" role="dialog" data-backdrop="static">
            <div class="modal-dialog modal-lg ijm-picker-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-search"></i> <span id="ijmPickerTitle">Pilih</span></h4>
                    </div>
                    <div class="modal-body ijm-picker-body">
                        <div class="ijm-list-search" style="margin-bottom:12px;">
                            <div class="input-group input-group-sm ijm-search-group">
                                <input type="text" class="form-control ijm-picker-search-input" placeholder="Ketik kata kunci..." maxlength="100" autocomplete="off" />
                                <span class="input-group-btn">
                                    <button type="button" class="btn btn-primary btn-ijn-picker-search"><i class="fa fa-search"></i> Cari</button>
                                </span>
                            </div>
                        </div>
                        <div class="ijm-list-loading ijm-picker-loading"><i class="fa fa-spinner fa-spin"></i> Memuat data...</div>
                        <div class="ijm-list-empty ijm-picker-empty" style="display:none;">Tidak ada data.</div>
                        <div class="ijm-list-error alert alert-danger ijm-picker-error" style="display:none;"></div>
                        <div class="table-responsive ijm-picker-wrap ijm-stack-wrap" style="display:none;">
                            <table class="table table-bordered table-striped table-hover ijm-picker-table ijm-stack-table">
                                <thead class="ijm-picker-thead"></thead>
                                <tbody class="ijm-picker-tbody"></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        (function () {
            var ijnPageUrl = '<%= ResolveUrl("~/installation_job_new.aspx") %>';
            var ijnSessionUserGroupId = '<%= (Session["ClsTypeUserGroupID"] ?? "").ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'") %>';
            var ijnSessionTechnicianId = '<%= (Session["ClsTypeUserTechnicianID"] ?? "").ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'") %>';
            var ijnPickerType = null;
            var ijnPickerCache = [];
            var ijnAccCache = [];
            var ijnJobSummaryRows = [];
            var ijnSelectedPictureFiles = [];

            var ijnPickerDefs = {
                job: {
                    title: 'Pilih Job ID',
                    placeholder: 'Cari Customer / Job ID / Device Type...',
                    method: 'GetNewInstallJobList',
                    params: function (term) { return { Search: term }; },
                    require: function () { return true; },
                    requireMsg: '',
                    cols: [
                        { key: 'JobID', label: 'Job ID' },
                        { key: 'RegDate', label: 'Reg Date' },
                        { key: 'FullName', label: 'Customer Name' },
                        { key: 'PoID', label: 'SO ID' },
                        { key: 'DeviceTypeDesc', label: 'Device Type' },
                        { key: 'ScheduleDate', label: 'Sch Date' }
                    ],
                    onPick: onPickJob
                },
                tech: {
                    title: 'Pilih Technician ID',
                    placeholder: 'Cari nama technician...',
                    method: 'GetNewInstallTechnicianList',
                    params: function (term) { return { Search: term }; },
                    require: function () { return true; },
                    cols: [
                        { key: 'TechnicianID', label: 'Technician ID' },
                        { key: 'EmployeeNo', label: 'Employee No' },
                        { key: 'Name', label: 'Name' },
                        { key: 'BranchName', label: 'Branch Name' }
                    ],
                    onPick: onPickTech
                },
                vehicle: {
                    title: 'Pilih Police No',
                    placeholder: 'Cari Police No...',
                    method: 'GetNewInstallVehicleList',
                    params: function (term) { return { CustID: ijnVal('hfCustID'), PoliceNo: term }; },
                    require: function () { return !!ijnVal('hfCustID'); },
                    requireMsg: 'Pilih Job ID terlebih dahulu.',
                    cols: [
                        { key: 'PoliceNo', label: 'Police No' },
                        { key: 'VehicleDesc', label: 'Vehicle Desc' },
                        { key: 'AssetNo', label: 'Asset No' },
                        { key: 'BranchName', label: 'Branch Name' },
                        { key: 'CustName', label: 'Customer Name' }
                    ],
                    onPick: onPickVehicle
                },
                device: {
                    title: 'Pilih No SN',
                    placeholder: 'Cari No SN...',
                    method: 'GetNewInstallDeviceList',
                    params: function (term) {
                        return { JobID: ijnVal('hfJobID'), TechnicianID: ijnVal('hfTechnicianID'), NoSN: term };
                    },
                    require: function () { return ijnVal('hfJobID') && ijnVal('hfTechnicianID'); },
                    requireMsg: 'Pilih Job ID dan Technician ID terlebih dahulu.',
                    cols: [
                        { key: 'NoSN', label: 'No SN' },
                        { key: 'DeviceTypeDesc', label: 'Device Type Desc' },
                        { key: 'WarehouseName', label: 'Warehouse Name' },
                        { key: 'VendorName', label: 'Vendor' }
                    ],
                    onPick: onPickDevice
                },
                gsm: {
                    title: 'Pilih No GSM',
                    placeholder: 'Cari MSIDN...',
                    method: 'GetNewInstallGsmList',
                    params: function (term) { return { TechnicianID: ijnVal('hfTechnicianID'), MSIDN: term }; },
                    require: function () { return !!ijnVal('hfTechnicianID'); },
                    requireMsg: 'Pilih Technician ID terlebih dahulu.',
                    cols: [
                        { key: 'MSIDN', label: 'No GSM' },
                        { key: 'ProviderName', label: 'Provider Name' },
                        { key: 'SourceName', label: 'Warehouse Name' },
                        { key: 'WarehouseName', label: 'Warehouse' }
                    ],
                    onPick: onPickGsm
                }
            };

            function ijnEsc(s) {
                return String(s || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
            }
            function ijnVal(id) { return $.trim($('#' + id).val() || ''); }
            function ijnSet(id, v) { $('#' + id).val(v || ''); }
            function ijnEquals(a, b) { return $.trim(a || '').toUpperCase() === $.trim(b || '').toUpperCase(); }
            function ijnContains(a, b) {
                return $.trim(a || '').toUpperCase().indexOf($.trim(b || '').toUpperCase()) >= 0;
            }
            function isLockedTechGroup() {
                return ijnContains(ijnSessionUserGroupId, 'TECH') || ijnEquals(ijnSessionUserGroupId, 'TH');
            }
            function ijnGetQueryParam(name) {
                if (!name) return '';
                try {
                    var url = new URL(window.location.href);
                    return $.trim(url.searchParams.get(name) || '');
                } catch (e) {
                    var pattern = new RegExp('[?&]' + name + '=([^&#]*)', 'i');
                    var m = pattern.exec(window.location.search || '');
                    return m ? decodeURIComponent((m[1] || '').replace(/\+/g, ' ')) : '';
                }
            }
            function ijnNormalizeJobId(value) {
                return String(value || '').trim().replace(/;$/, '');
            }
            function ijnGetJobIdFromUrl() {
                var raw = '';
                try {
                    var params = new URLSearchParams(window.location.search || '');
                    params.forEach(function (val, key) {
                        if (!raw && key && key.toLowerCase() === 'jobid') raw = val;
                    });
                    if (!raw) {
                        raw = params.get('JobID') || params.get('jobid') || params.get('JOBID') || '';
                    }
                } catch (e) {
                    var m = /[?&]jobid=([^&#]*)/i.exec(window.location.search || '');
                    if (m) raw = decodeURIComponent((m[1] || '').replace(/\+/g, ' '));
                }
                return ijnNormalizeJobId(raw);
            }
            function ijmRo(field, v) {
                $('.ijm-ro[data-field="' + field + '"]').text(v || '-');
            }

            function clearJobDetailInfo() {
                ['RegDate', 'SchDate', 'SoID', 'CustomerName', 'BranchName', 'MarketingName'].forEach(function (f) { ijmRo(f, ''); });
                ijnSet('hfPoID', '');
                clearJobSummary();
                updateSaveButtonState();
            }

            function clearJobSummary() {
                ijnJobSummaryRows = [];
                $('#ijmJobSummaryBody').empty();
                $('#ijmJobSummaryWrap').hide();
                renderJobSummaryState('empty', 'Pilih Job ID untuk menampilkan detail item pekerjaan.', 'Device Group, Device Type, Qty Request, dan Qty Done akan tampil otomatis setelah Job ID dipilih.');
                updateAccCount();
            }

            function renderJobSummaryState(state, title, text) {
                var icon = 'fa-info-circle';
                if (state === 'loading') icon = 'fa-spinner fa-spin';
                else if (state === 'error') icon = 'fa-exclamation-triangle';
                else if (state === 'empty') icon = 'fa-list-alt';

                var cls = 'ijm-job-summary-card';
                if (state === 'loading') cls += ' ijm-job-summary-state-loading';
                if (state === 'error') cls += ' ijm-job-summary-state-error';

                $('#ijmJobSummaryEmpty').show().html(
                    '<div class="' + cls + '">' +
                    '<div class="ijm-job-summary-icon"><i class="fa ' + icon + '"></i></div>' +
                    '<div><p class="ijm-job-summary-head">' + ijnEsc(title || '') + '</p>' +
                    '<p class="ijm-job-summary-text">' + ijnEsc(text || '') + '</p></div>' +
                    '</div>'
                );
            }

            function normalizeJobDetailStatus(status) {
                return String(status || '').trim().toUpperCase();
            }

            function isClosedJobDetail(status) {
                var value = normalizeJobDetailStatus(status);
                return value === 'CL' || value === 'CLOSE' || value === 'CLOSED';
            }

            function renderJobDetailStatusCell(rawStatus) {
                if (isClosedJobDetail(rawStatus)) {
                    return '<span class="label label-success ijm-job-status-label"><i class="fa fa-check"></i>Closed</span>';
                }
                var label = String(rawStatus || '').trim() || 'Opened';
                return '<span class="label label-warning ijm-job-status-label"><i class="fa fa-circle"></i>' + ijnEsc(label) + '</span>';
            }

            function loadJobSummary(jobId) {
                if (!jobId) { clearJobSummary(); return; }
                ijnJobSummaryRows = [];
                $('#ijmJobSummaryBody').empty();
                $('#ijmJobSummaryWrap').hide();
                renderJobSummaryState('loading', 'Memuat detail job...', 'Mohon tunggu, sistem sedang mengambil informasi item pekerjaan.');
                updateAccCount();
                ijnCall('GetNewInstallJobCreateDetails', { JobID: jobId }, function (res) {
                    var rows = res.Data || [];
                    ijnJobSummaryRows = rows;
                    if (!rows.length) {
                        renderJobSummaryState('empty', 'Detail job belum tersedia.', 'Belum ada data Device Group/Device Type untuk Job ID ini.');
                        updateAccCount();
                        return;
                    }
                    var $body = $('#ijmJobSummaryBody');
                    rows.forEach(function (row) {
                        var dg = ijnEsc(cell(row, ['DeviceGroupDesc']));
                        var dt = ijnEsc(cell(row, ['DeviceTypeDesc']));
                        var q = ijnEsc(cell(row, ['Quantity']));
                        var qd = ijnEsc(cell(row, ['QuantityDone']));
                        var rawStatus = cell(row, ['ValueStatus', 'Status']);
                        var closed = isClosedJobDetail(rawStatus);
                        var rowCls = closed ? 'ijm-job-summary-row-closed' : 'ijm-job-summary-row-open';
                        var statusHtml = renderJobDetailStatusCell(rawStatus);
                        $body.append(
                            '<tr class="' + rowCls + '">' +
                            '<td data-label="Device Group">' + dg + '</td>' +
                            '<td data-label="Device Type">' + dt + '</td>' +
                            '<td class="text-right" data-label="Qty Request">' + q + '</td>' +
                            '<td class="text-right" data-label="Qty Done">' + qd + '</td>' +
                            '<td data-label="Status">' + statusHtml + '</td>' +
                            '</tr>'
                        );
                    });
                    $('#ijmJobSummaryEmpty').hide();
                    $('#ijmJobSummaryWrap').show();
                    updateAccCount();
                }, function () {
                    ijnJobSummaryRows = [];
                    renderJobSummaryState('error', 'Gagal memuat detail job.', 'Silakan coba lagi beberapa saat, atau pilih ulang Job ID.');
                    updateAccCount();
                });
            }

            function resetServerOptions() {
                var $s = $('#cmbServerName');
                $s.empty().append('<option value="">[Select]</option>');
            }

            function resetUserAccessOptions() {
                var $s = $('#cmbUserAccess');
                $s.empty().append('<option value="">[Select]</option>');
            }

            function resetInstallationInfo() {
                resetServerOptions();
                resetUserAccessOptions();
                ijnSet('txtInstallDate', '');
                ijnSet('txtRemarks', '');
                $('#cmbRelay').val('0');
                $('#fuInstallPictures').val('');
                ijnSelectedPictureFiles = [];
                $('#ijmPicturePreview').empty().hide();
                $('#ijmUploadMeta').text('Belum ada file dipilih');
            }

            function loadServerList() {
                var cust = ijnVal('hfCustID');
                resetServerOptions();
                resetUserAccessOptions();
                if (!cust) return;
                ijnCall('GetNewInstallServerList', { CustID: cust }, function (res) {
                    var rows = res.Data || [];
                    var $s = $('#cmbServerName');
                    rows.forEach(function (row) {
                        var id = cell(row, ['ServerID']);
                        var name = cell(row, ['ServerName']);
                        if (id) $s.append('<option value="' + ijnEsc(id) + '">' + ijnEsc(name || id) + '</option>');
                    });
                    updateSaveButtonState();
                }, function (msg) {
                    Swal.fire({ icon: 'error', title: 'Gagal', text: msg || 'Gagal memuat daftar server.' });
                });
            }

            function loadUserAccessList() {
                var cust = ijnVal('hfCustID');
                var server = ijnVal('cmbServerName');
                resetUserAccessOptions();
                if (!cust || !server) return;
                ijnCall('GetInterfacingUserLogin', { CustID: cust, ServerID: server }, function (res) {
                    var rows = res.Data || [];
                    var $s = $('#cmbUserAccess');
                    rows.forEach(function (row) {
                        var autoId = cell(row, ['autoid', 'AutoID']);
                        var userId = cell(row, ['user_id', 'UserID']);
                        var userNm = cell(row, ['user_nm', 'UserNm', 'UserName']);
                        if (autoId) {
                            var text = (userId || autoId) + (userNm ? (' - ' + userNm) : '');
                            $s.append('<option value="' + ijnEsc(autoId) + '">' + ijnEsc(text) + '</option>');
                        }
                    });
                }, function (msg) {
                    Swal.fire({ icon: 'error', title: 'Gagal', text: msg || 'Gagal memuat user akses.' });
                });
            }

            function clearTechInfo() {
                ijnSet('txtTechnicianID', '');
                ijnSet('hfTechnicianID', '');
                ijnSet('hfTechName', '');
                ijmRo('TechnicianID', '');
                ijmRo('TechnicianName', '');
                ijmRo('TechBranchName', '');
                clearDeviceGsm();
                updateSaveButtonState();
            }

            function loadMarketingName(tvaId) {
                if (!tvaId) { ijmRo('MarketingName', ''); return; }
                ijnCall('GetNewInstallMasterInfo', { TvaID: tvaId }, function (res) {
                    var d = res.Data || {};
                    ijmRo('MarketingName', d.MarketingName || d.MasterFullName || '');
                }, function () { ijmRo('MarketingName', ''); });
            }

            function ijnCall(method, params, ok, fail) {
                $.ajax({
                    type: 'POST',
                    url: ijnPageUrl + '/' + method,
                    data: JSON.stringify(params || {}),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (r) {
                        var res = (r && r.d != null) ? (typeof r.d === 'object' ? r.d : JSON.parse(r.d)) : null;
                        if (!res || !res.Success) { fail((res && res.Message) || 'Gagal memuat data.'); return; }
                        ok(res);
                    },
                    error: function () { fail('Gagal memuat data.'); }
                });
            }

            function ijnCallPromise(method, params) {
                return new Promise(function (resolve, reject) {
                    ijnCall(method, params, resolve, reject);
                });
            }

            function ijnSyncPickerModal() {
                var $fresh = $('.ijm-page .ijm-picker-modal');
                if ($fresh.length) {
                    $('body > .ijm-picker-modal').remove();
                    $fresh.first().appendTo('body');
                }
            }
            function ijnGetPickerModal() {
                var $m = $('body > .ijm-picker-modal');
                if (!$m.length) { ijnSyncPickerModal(); $m = $('body > .ijm-picker-modal'); }
                if (!$m.length) $m = $('.ijm-page .ijm-picker-modal').first();
                return $m;
            }

            function openPicker(type) {
                var def = ijnPickerDefs[type];
                if (!def) return;
                if (def.require && !def.require()) {
                    Swal.fire({ icon: 'warning', title: 'Perhatian', text: def.requireMsg || 'Lengkapi data sebelumnya.' });
                    return;
                }
                ijnPickerType = type;
                ijnSyncPickerModal();
                var $m = ijnGetPickerModal();
                if (!$m.length) return;
                $m.find('#ijmPickerTitle').text(def.title);
                $m.find('.ijm-picker-search-input').attr('placeholder', def.placeholder).val('');
                ijnResetPickerList($m);
                $m.modal({ backdrop: 'static', keyboard: true, show: true });
                setTimeout(function () { $m.find('.ijm-picker-search-input').focus(); }, 300);
                loadPickerList('');
            }

            function ijnResetPickerList($m) {
                $m.find('.ijm-picker-loading').show();
                $m.find('.ijm-picker-empty, .ijm-picker-error, .ijm-picker-wrap').hide();
                $m.find('.ijm-picker-tbody').empty();
            }

            function loadPickerList(term) {
                var def = ijnPickerDefs[ijnPickerType];
                if (!def) return;
                var $m = ijnGetPickerModal();
                ijnResetPickerList($m);
                ijnCall(def.method, def.params(term || ''), function (res) {
                    $m.find('.ijm-picker-loading').hide();
                    ijnPickerCache = res.Data || [];
                    renderPickerList(ijnPickerCache, def);
                }, function (msg) {
                    $m.find('.ijm-picker-loading').hide();
                    $m.find('.ijm-picker-error').text(msg).show();
                });
            }

            function renderPickerList(rows, def) {
                var $m = ijnGetPickerModal();
                var $thead = $m.find('.ijm-picker-thead');
                var $tbody = $m.find('.ijm-picker-tbody');
                $thead.empty();
                $tbody.empty();
                if (!rows.length) {
                    $m.find('.ijm-picker-empty').show();
                    return;
                }
                var h = '<tr>';
                def.cols.forEach(function (c) { h += '<th>' + ijnEsc(c.label) + '</th>'; });
                h += '<th class="text-center">Action</th></tr>';
                $thead.html(h);
                rows.forEach(function (row, idx) {
                    var tr = '<tr>';
                    def.cols.forEach(function (c) {
                        tr += '<td data-label="' + ijnEsc(c.label) + '">' + ijnEsc(row[c.key] || row[c.key.toLowerCase()] || '') + '</td>';
                    });
                    tr += '<td class="text-center" data-label="Action"><button type="button" class="btn btn-success btn-xs btn-ijn-pick" data-idx="' + idx + '"><i class="fa fa-check"></i> Pilih</button></td></tr>';
                    $tbody.append(tr);
                });
                $m.find('.ijm-picker-wrap').show();
            }

            function cell(row, keys) {
                for (var i = 0; i < keys.length; i++) {
                    if (row[keys[i]] != null && row[keys[i]] !== '') return row[keys[i]];
                    var lk = keys[i].toLowerCase();
                    for (var k in row) { if (k.toLowerCase() === lk && row[k]) return row[k]; }
                }
                return '';
            }

            function onPickJob(row) {
                ijnSet('txtJobID', cell(row, ['JobID']));
                ijnSet('hfJobID', cell(row, ['JobID']));
                ijnSet('hfCustID', cell(row, ['CustID']));
                ijnSet('hfPoID', cell(row, ['PoID']));
                ijmRo('RegDate', cell(row, ['RegDate']));
                ijmRo('SchDate', cell(row, ['ScheduleDate', 'SchDate']));
                ijmRo('SoID', cell(row, ['PoID']));
                ijmRo('CustomerName', cell(row, ['FullName', 'CustomerName']));
                ijmRo('BranchName', '');
                ijmRo('MarketingName', '');
                clearTechInfo();
                clearVehicleDeviceGsm();
                resetInstallationInfo();
                loadServerList();
                loadJobSummary(ijnVal('hfJobID'));
                updateSaveButtonState();
            }

            function onPickTech(row) {
                ijnSet('txtTechnicianID', cell(row, ['TechnicianID']));
                ijnSet('hfTechnicianID', cell(row, ['TechnicianID']));
                ijnSet('hfTechName', cell(row, ['Name']));
                ijmRo('TechnicianID', cell(row, ['TechnicianID']));
                ijmRo('TechnicianName', cell(row, ['Name']));
                ijmRo('TechBranchName', cell(row, ['BranchName']));
                clearDeviceGsm();
                loadAccessories();
                updateSaveButtonState();
            }

            function onPickVehicle(row) {
                ijnSet('txtPoliceNo', cell(row, ['PoliceNo']));
                ijnSet('txtVehicleDesc', cell(row, ['VehicleDesc']));
                ijnSet('txtAssetNo', cell(row, ['AssetNo']));
                ijnSet('hfTvaID', cell(row, ['TvaID']));
                ijnSet('hfVehicleID', cell(row, ['VehicleID']));
                ijmRo('BranchName', cell(row, ['BranchName']));
                ijmRo('CustomerName', cell(row, ['CustName', 'FullName']));
                loadMarketingName(cell(row, ['TvaID']));
                resolveTvdAndAcc();
                updateSaveButtonState();
            }

            function clearVehicleDeviceGsm() {
                ['txtPoliceNo', 'txtVehicleDesc', 'txtAssetNo', 'txtNoSN', 'txtDeviceTypeDesc', 'txtWarehouseName', 'txtNoGSM', 'txtProviderName', 'txtGsmWarehouseName'].forEach(function (id) { ijnSet(id, ''); });
                ['hfTvaID', 'hfVehicleID', 'hfTdtID', 'hfDeviceID', 'hfTgtID', 'hfGsmID', 'hfTvdID'].forEach(function (id) { ijnSet(id, ''); });
                ijmRo('BranchName', '');
                ijmRo('MarketingName', '');
                clearAccList();
                hideInstallChannelSetting();
                updateSaveButtonState();
            }

            function onPickDevice(row) {
                ijnSet('txtNoSN', cell(row, ['NoSN']));
                ijnSet('txtDeviceTypeDesc', cell(row, ['DeviceTypeDesc']));
                ijnSet('txtWarehouseName', cell(row, ['WarehouseName']));
                ijnSet('hfTdtID', cell(row, ['TdtID']));
                ijnSet('hfDeviceID', cell(row, ['DeviceID']));
                ijnSet('hfDeviceServerID', cell(row, ['ServerID']));
                ijnSet('hfDeviceContractTime', cell(row, ['ContractTime']));
                ijnSet('hfDeviceBrand', cell(row, ['DeviceBrand']));
                ijnSet('hfDeviceModel', cell(row, ['DeviceModel']));
                resolveTvdAndAcc();
                updateSaveButtonState();
                refreshInstallChannelSetting();
            }

            function onPickGsm(row) {
                ijnSet('txtNoGSM', cell(row, ['MSIDN']));
                ijnSet('txtProviderName', cell(row, ['ProviderName']));
                ijnSet('txtGsmWarehouseName', cell(row, ['SourceName', 'WarehouseName']));
                ijnSet('hfTgtID', cell(row, ['TgtID']));
                ijnSet('hfGsmID', cell(row, ['GsmID']));
                resolveTvdAndAcc();
                updateSaveButtonState();
            }

            function clearDeviceGsm() {
                ['txtNoSN', 'txtDeviceTypeDesc', 'txtWarehouseName', 'txtNoGSM', 'txtProviderName', 'txtGsmWarehouseName'].forEach(function (id) { ijnSet(id, ''); });
                ['hfTdtID', 'hfDeviceID', 'hfTgtID', 'hfGsmID', 'hfTvdID', 'hfDeviceServerID', 'hfDeviceContractTime', 'hfDeviceBrand', 'hfDeviceModel'].forEach(function (id) { ijnSet(id, ''); });
                clearAccList();
                hideInstallChannelSetting();
                updateSaveButtonState();
            }

            function canSave() {
                return !!(ijnVal('hfJobID') &&
                    ijnVal('hfPoID') &&
                    ijnVal('hfTvaID') &&
                    ijnVal('hfVehicleID') &&
                    ijnVal('hfTdtID') &&
                    ijnVal('hfDeviceID') &&
                    ijnVal('hfTgtID') &&
                    ijnVal('hfGsmID') &&
                    ijnVal('hfTechnicianID') &&
                    ijnVal('cmbServerName') &&
                    ijnVal('cmbUserAccess') &&
                    ijnVal('txtInstallDate'));
            }

            function updateSaveButtonState() {
                $('#btnSaveClient').prop('disabled', !canSave());
            }

            function parseIntSafe(v) {
                var n = parseInt($.trim(v || ''), 10);
                return isNaN(n) ? 0 : n;
            }

            function normalizeDeviceGroupKey(row) {
                return {
                    id: String(cell(row, ['DeviceGroupID']) || '').trim().toUpperCase(),
                    desc: String(cell(row, ['DeviceGroupDesc']) || '').trim().toUpperCase()
                };
            }
            function isGpsDeviceGroup(row) {
                var g = normalizeDeviceGroupKey(row);
                return g.id === 'GPS' || g.desc === 'GPS';
            }
            function isAccessoriesDeviceGroup(row) {
                var g = normalizeDeviceGroupKey(row);
                return g.id === 'ACS' || g.desc === 'ACCESSORIES';
            }
            function getJobQtyRequestTotals() {
                var gps = 0;
                var acc = 0;
                (ijnJobSummaryRows || []).forEach(function (row) {
                    var qty = parseIntSafe(cell(row, ['Quantity']));
                    if (qty < 0) qty = 0;
                    if (isGpsDeviceGroup(row)) gps += qty;
                    else if (isAccessoriesDeviceGroup(row)) acc += qty;
                });
                return { gps: gps, acc: acc };
            }
            function getRequiredAccessoriesCount() {
                var totals = getJobQtyRequestTotals();
                if (totals.gps <= 0) return 0;
                if (totals.acc <= 0) return 0;
                return Math.round(totals.acc / totals.gps);
            }
            function getSelectedAccessoriesCount() {
                return $('.ijm-acc-chk:checked').length;
            }
            function validateAccessoriesSelection() {
                var required = getRequiredAccessoriesCount();
                var selected = getSelectedAccessoriesCount();
                if (required <= 0) {
                    return { ok: true, required: 0, selected: selected, remaining: 0 };
                }
                var remaining = required - selected;
                if (remaining < 0) remaining = 0;
                return {
                    ok: selected >= required,
                    required: required,
                    selected: selected,
                    remaining: remaining
                };
            }
            function accessoriesRequirementMessage(result) {
                return 'Accessories wajib dipilih. Required: ' + result.required + ', Selected: ' + result.selected + ', Remaining: ' + result.remaining + '.';
            }

            function readPictureFileHeader(file, len) {
                return new Promise(function (resolve, reject) {
                    var reader = new FileReader();
                    reader.onload = function () { resolve(new Uint8Array(reader.result || [])); };
                    reader.onerror = function () { reject('Gagal membaca file gambar.'); };
                    reader.readAsArrayBuffer(file.slice(0, len || 16));
                });
            }

            function isAllowedPictureHeader(bytes) {
                if (!bytes || bytes.length < 2) return false;
                if (bytes[0] === 0xFF && bytes[1] === 0xD8) return true;
                return bytes.length >= 4 && bytes[0] === 0x89 && bytes[1] === 0x50 && bytes[2] === 0x4E && bytes[3] === 0x47;
            }

            function resolvePictureFileExt(file, headerBytes) {
                if (headerBytes && headerBytes.length >= 2 && headerBytes[0] === 0xFF && headerBytes[1] === 0xD8) return '.jpg';
                if (headerBytes && headerBytes.length >= 4 && headerBytes[0] === 0x89) return '.png';
                var dot = file.name.lastIndexOf('.');
                var ext = (dot >= 0 ? file.name.substring(dot).toLowerCase() : '');
                if (!ext && file.type === 'image/jpeg') ext = '.jpg';
                if (!ext && file.type === 'image/png') ext = '.png';
                return ext;
            }

            function normalizePictureFileName(file, ext) {
                var safeExt = ext === '.jpeg' ? '.jpg' : ext;
                if (!safeExt) return file.name || 'image.jpg';
                if (/\.(jpe?g|png)$/i.test(file.name || '')) return file.name;
                var base = String(file.name || 'image').replace(/\.[^.]+$/, '') || 'image';
                return base + safeExt;
            }

            function fileToBase64(file) {
                return new Promise(function (resolve, reject) {
                    var reader = new FileReader();
                    reader.onload = function () {
                        var out = String(reader.result || '');
                        var idx = out.indexOf(',');
                        resolve(idx >= 0 ? out.substring(idx + 1) : out);
                    };
                    reader.onerror = function () { reject('Gagal membaca file gambar.'); };
                    reader.readAsDataURL(file);
                });
            }

            function humanFileSize(bytes) {
                var b = Number(bytes || 0);
                if (b < 1024) return b + ' B';
                if (b < 1024 * 1024) return (b / 1024).toFixed(1) + ' KB';
                return (b / (1024 * 1024)).toFixed(1) + ' MB';
            }

            function updateUploadMeta(files) {
                var n = (files || []).length;
                if (!n) {
                    $('#ijmUploadMeta').text('Belum ada file dipilih');
                    return;
                }
                var total = 0;
                files.forEach(function (f) { total += Number(f.size || 0); });
                $('#ijmUploadMeta').text(n + ' file dipilih (' + humanFileSize(total) + ')');
            }

            function renderPicturePreview(files) {
                var $wrap = $('#ijmPicturePreview');
                $wrap.empty();
                if (!files || !files.length) {
                    $wrap.hide();
                    updateUploadMeta([]);
                    return;
                }
                files.forEach(function (f, idx) {
                    var url = URL.createObjectURL(f);
                    var $item = $('<div class="ijm-attach-item"></div>');
                    var $img = $('<img alt="preview" />').attr('src', url);
                    $img.on('load', function () { URL.revokeObjectURL(url); });
                    $item.append($('<button type="button" class="ijm-attach-remove" title="Hapus">&times;</button>').attr('data-idx', idx));
                    $item.append($img);
                    $item.append($('<div class="ijm-attach-name"></div>').text(f.name || 'image'));
                    $wrap.append($item);
                });
                $wrap.show();
                updateUploadMeta(files);
            }

            function buildPicturePayload() {
                var files = ijnSelectedPictureFiles || [];
                if (files.length > 10) return Promise.reject('Maksimal 10 file gambar.');
                var allowed = { '.jpg': true, '.jpeg': true, '.png': true };
                for (var i = 0; i < files.length; i++) {
                    if (files[i].size > 5 * 1024 * 1024) return Promise.reject('Ukuran tiap file maksimal 5 MB.');
                }
                return Promise.all(files.map(function (f) {
                    return readPictureFileHeader(f, 16).then(function (header) {
                        if (!isAllowedPictureHeader(header)) {
                            return Promise.reject('Format file harus jpg, jpeg, atau png.');
                        }
                        var ext = resolvePictureFileExt(f, header);
                        if (!allowed[ext]) {
                            return Promise.reject('Format file harus jpg, jpeg, atau png.');
                        }
                        return fileToBase64(f).then(function (b64) {
                            return { FileName: normalizePictureFileName(f, ext), ContentBase64: b64 };
                        });
                    });
                }));
            }

            function submitSave() {
                if (!canSave()) {
                    Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Lengkapi data wajib (Job, Technician, Vehicle, Device, GSM, Server Name, User Akses, dan Install Date) terlebih dahulu.' });
                    return;
                }

                var accCheck = validateAccessoriesSelection();
                if (!accCheck.ok) {
                    Swal.fire({ icon: 'warning', title: 'Perhatian', text: accessoriesRequirementMessage(accCheck) });
                    updateSaveButtonState();
                    return;
                }

                $('#btnSaveClient').prop('disabled', true);
                buildPicturePayload()
                    .then(function (filePayload) {
                        var payload = {
                            TvaID: ijnVal('hfTvaID'),
                            PoID: ijnVal('hfPoID'),
                            VehicleID: ijnVal('hfVehicleID'),
                            TdtID: ijnVal('hfTdtID'),
                            DeviceID: ijnVal('hfDeviceID'),
                            TgtID: ijnVal('hfTgtID'),
                            GsmID: ijnVal('hfGsmID'),
                            NoGSM: ijnVal('txtNoGSM'),
                            NoSN: ijnVal('txtNoSN'),
                            DeviceTypeDesc: ijnVal('txtDeviceTypeDesc'),
                            DeviceBrand: ijnVal('hfDeviceBrand'),
                            DeviceModel: ijnVal('hfDeviceModel'),
                            TechnicianID: ijnVal('hfTechnicianID'),
                            JobID: ijnVal('hfJobID'),
                            ServerIDInstall: ijnVal('cmbServerName'),
                            ServerIDDevice: ijnVal('hfDeviceServerID'),
                            UserAccessAutoID: ijnVal('cmbUserAccess'),
                            InstallDate: ijnVal('txtInstallDate'),
                            Remarks: ijnVal('txtRemarks'),
                            Contrac: String(parseIntSafe(ijnVal('hfDeviceContractTime'))),
                            IsRelay: ijnVal('cmbRelay') || '0',
                            PictureFiles: filePayload,
                            SelectedAccessoriesJson: ijnVal('hfSelectedAcc')
                        };
                        return ijnCallPromise('SaveNewInstallation', payload);
                    })
                    .then(function (res) {
                        Swal.fire({ icon: 'success', title: 'Success', text: (res && res.Message) ? res.Message : 'New installation berhasil disimpan.' });
                        clearJobDetailInfo();
                        clearTechInfo();
                        clearVehicleDeviceGsm();
                        ['txtJobID', 'hfJobID', 'hfCustID', 'hfPoID'].forEach(function (id) { ijnSet(id, ''); });
                        resetInstallationInfo();
                    })
                    .catch(function (msg) {
                        Swal.fire({ icon: 'error', title: 'Gagal', text: msg || 'Gagal menyimpan data.' });
                    })
                    .then(function () {
                        updateSaveButtonState();
                    });
            }

            function resolveTvdAndAcc() {
                var tva = ijnVal('hfTvaID'), tdt = ijnVal('hfTdtID'), tgt = ijnVal('hfTgtID'), tech = ijnVal('hfTechnicianID');
                if (!tva || !tdt || !tgt || !tech) {
                    loadAccessories();
                    return;
                }
                ijnCall('ResolveTvdId', { TvaID: tva, TdtID: tdt, TgtID: tgt, TechnicianID: tech }, function (res) {
                    var d = res.Data || {};
                    ijnSet('hfTvdID', d.TvdID || d.tvdID || '');
                    loadAccessories();
                }, function () { loadAccessories(); });
            }

            function clearAccList() {
                ijnAccCache = [];
                ijnSet('hfSelectedAcc', '');
                $('.ijm-acc-loading').hide();
                $('.ijm-acc-wrap, .ijm-acc-error').hide();
                $('.ijm-acc-empty').show().text('Pilih Job ID dan Technician untuk memuat list accessories.');
                $('#ijmAccBody').empty();
                $('#ijmAccCount').text('');
                updateAccRequirementHint();
            }

            function loadAccessories() {
                var job = ijnVal('hfJobID'), tech = ijnVal('hfTechnicianID');
                if (!job || !tech) {
                    clearAccList();
                    return;
                }
                var term = $.trim($('#txtAccSearch').val() || '');
                $('.ijm-acc-loading').show();
                $('.ijm-acc-empty, .ijm-acc-error, .ijm-acc-wrap').hide();
                $('#ijmAccBody').empty();
                ijnCall('GetNewInstallAccessoriesList', { JobID: job, TechnicianID: tech, Search: term }, function (res) {
                    $('.ijm-acc-loading').hide();
                    ijnAccCache = res.Data || [];
                    renderAccessories(ijnAccCache);
                }, function (msg) {
                    $('.ijm-acc-loading').hide();
                    $('.ijm-acc-error').text(msg).show();
                });
            }

            function renderAccessories(rows) {
                var $body = $('#ijmAccBody');
                $body.empty();
                if (!rows.length) {
                    $('.ijm-acc-empty').show().text('Tidak ada accessories tersedia.');
                    $('#ijmAccCount').text('');
                    ijnSet('hfSelectedAcc', '');
                    updateAccRequirementHint();
                    return;
                }
                rows.forEach(function (row, idx) {
                    var deviceId = cell(row, ['DeviceID']);
                    var noSn = cell(row, ['NoSN']);
                    var tdtId = cell(row, ['TdtID']);
                    $body.append(
                        '<tr>' +
                        '<td class="text-center" data-label="Pilih"><input type="checkbox" class="ijm-acc-chk" data-idx="' + idx + '" data-deviceid="' + ijnEsc(deviceId) + '" data-tdtid="' + ijnEsc(tdtId) + '" /></td>' +
                        '<td data-label="Device ID">' + ijnEsc(deviceId) + '</td>' +
                        '<td data-label="No SN">' + ijnEsc(noSn) + '</td>' +
                        '<td data-label="Device Type Desc">' + ijnEsc(cell(row, ['DeviceTypeDesc'])) + '</td>' +
                        '<td data-label="Status">' + ijnEsc(cell(row, ['Status'])) + '</td>' +
                        '</tr>'
                    );
                });
                $('.ijm-acc-wrap').show();
                syncAccCheckAllState();
                updateAccCount();
            }

            function syncAccCheckAllState() {
                var $c = $('.ijm-acc-chk');
                var all = $c.length > 0 && $c.filter(':checked').length === $c.length;
                $('#ijmAccCheckAllHead').prop('checked', all);
            }
            function updateAccCount() {
                var n = $('.ijm-acc-chk:checked').length;
                var t = $('.ijm-acc-chk').length;
                $('#ijmAccCount').text(t ? (n + ' / ' + t + ' dipilih') : '');
                var sel = [];
                $('.ijm-acc-chk:checked').each(function () {
                    var i = parseInt($(this).data('idx'), 10);
                    if (ijnAccCache[i]) sel.push(ijnAccCache[i]);
                });
                ijnSet('hfSelectedAcc', sel.length ? JSON.stringify(sel) : '');
                updateAccRequirementHint();
            }
            function updateAccRequirementHint() {
                var $hint = $('#ijmAccReqHint');
                var required = getRequiredAccessoriesCount();
                if (required <= 0) {
                    $hint.hide().text('').removeClass('ijm-acc-req-ok ijm-acc-req-warn ijm-acc-req-err');
                    return;
                }
                var result = validateAccessoriesSelection();
                $hint.show().text(accessoriesRequirementMessage(result));
                $hint.removeClass('ijm-acc-req-ok ijm-acc-req-warn ijm-acc-req-err');
                $hint.addClass(result.ok ? 'ijm-acc-req-ok' : (result.selected > 0 ? 'ijm-acc-req-warn' : 'ijm-acc-req-err'));
            }
            function setAllAccChecked(checked) {
                $('.ijm-acc-chk').prop('checked', checked);
                syncAccCheckAllState();
                updateAccCount();
            }

            // ===== Setting Channel MDVR (reuse mdvr_channel.ashx). Tidak mengubah flow Save/Submit. =====
            var CH_HANDLER = 'mdvr_channel.ashx';
            var installChannelNoSN = '';
            var installChannelMax = 0;

            function chEscapeHtml(s) {
                return (s == null ? '' : String(s))
                    .replace(/&/g, '&amp;').replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
            }

            function hideInstallChannelSetting() {
                installChannelNoSN = '';
                installChannelMax = 0;
                var pnl = document.getElementById('pnlChannelSetting');
                if (pnl) { pnl.style.display = 'none'; }
                var alertEl = document.getElementById('channelSettingAlert');
                if (alertEl) { alertEl.innerHTML = ''; }
                var rows = document.getElementById('channelSettingRows');
                if (rows) { rows.innerHTML = ''; }
                var tbl = document.getElementById('channelSettingTable');
                if (tbl) { tbl.style.display = 'none'; }
                var btn = document.getElementById('btnSaveChannelSetting');
                if (btn) { btn.disabled = true; }
                var loading = document.getElementById('channelSettingLoading');
                if (loading) { loading.style.display = 'none'; }
            }

            function showChannelAlert(type, msg) {
                var el = document.getElementById('channelSettingAlert');
                if (!el) { return; }
                var cls = (type === 'success') ? 'alert-success' : (type === 'warning' ? 'alert-warning' : 'alert-danger');
                el.innerHTML = '<div class="alert ' + cls + '" style="padding:8px;margin-bottom:8px;">' + chEscapeHtml(msg) + '</div>';
            }

            function buildChannelTypeOptions(types) {
                var html = '<option value="">Tidak Digunakan</option>';
                for (var t = 0; t < (types || []).length; t++) {
                    var c = (types[t].ChanelTypeCode || '').toString();
                    if (!c) { continue; }
                    var nm = (types[t].ChanelTypeName || c).toString();
                    html += '<option value="' + chEscapeHtml(c) + '">' + chEscapeHtml(nm) + '</option>';
                }
                return html;
            }

            function renderChannelRows(master, existing, types) {
                types = types || [];
                var validByUpper = {};
                for (var t = 0; t < types.length; t++) {
                    var vc = (types[t].ChanelTypeCode || '').toString();
                    if (vc) { validByUpper[vc.toUpperCase()] = vc; }
                }
                var optionsHtml = buildChannelTypeOptions(types);
                var map = {};
                for (var i = 0; i < (existing || []).length; i++) {
                    map[String(existing[i].ChanelID)] = (existing[i].ChanelType || '').toUpperCase();
                }
                var rows = document.getElementById('channelSettingRows');
                if (!rows) { return; }
                rows.innerHTML = '';
                for (var j = 0; j < (master || []).length; j++) {
                    var m = master[j];
                    var cur = map[String(m.ChanelID)] || '';
                    var label = m.ChanelName || ('Channel ' + m.ChanelID);
                    var code = m.ChanelCode ? (' <small class="text-muted">(' + chEscapeHtml(m.ChanelCode) + ')</small>') : '';
                    var tr = document.createElement('tr');
                    var tdLabel = document.createElement('td');
                    tdLabel.setAttribute('data-label', 'Channel');
                    tdLabel.innerHTML = '<strong>' + chEscapeHtml(label) + '</strong>' + code;
                    var tdSel = document.createElement('td');
                    tdSel.setAttribute('data-label', 'Tipe Channel');
                    var sel = document.createElement('select');
                    sel.className = 'form-control input-sm channel-type-select';
                    sel.setAttribute('data-chanelid', m.ChanelID);
                    sel.innerHTML = optionsHtml;
                    sel.value = validByUpper[cur] ? validByUpper[cur] : '';
                    tdSel.appendChild(sel);
                    tr.appendChild(tdLabel);
                    tr.appendChild(tdSel);
                    rows.appendChild(tr);
                }
            }

            function loadInstallChannelRows() {
                var loading = document.getElementById('channelSettingLoading');
                var tbl = document.getElementById('channelSettingTable');
                var btn = document.getElementById('btnSaveChannelSetting');
                var alertEl = document.getElementById('channelSettingAlert');
                if (alertEl) { alertEl.innerHTML = ''; }
                if (loading) { loading.style.display = 'block'; }
                if (tbl) { tbl.style.display = 'none'; }
                if (btn) { btn.disabled = true; }

                $.ajax({
                    url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                    data: { action: 'load', nosn: installChannelNoSN, maxchannel: installChannelMax }
                }).done(function (res) {
                    if (loading) { loading.style.display = 'none'; }
                    if (!res || !res.success) {
                        showChannelAlert('danger', (res && res.message) || 'Gagal memuat channel.');
                        return;
                    }
                    renderChannelRows(res.master || [], res.existing || [], res.types || []);
                    if (tbl) { tbl.style.display = ''; }
                    if (btn) { btn.disabled = false; }
                }).fail(function () {
                    if (loading) { loading.style.display = 'none'; }
                    showChannelAlert('danger', 'Gagal terhubung ke server.');
                });
            }

            function refreshInstallChannelSetting() {
                var nosnEl = document.getElementById('txtNoSN');
                var nosn = nosnEl ? (nosnEl.value || '').toString().trim() : '';
                if (!nosn) {
                    hideInstallChannelSetting();
                    return;
                }

                $.ajax({
                    url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                    data: { action: 'require', nosn: nosn }
                }).done(function (res) {
                    if (!res || !res.success || !res.isRequire) {
                        hideInstallChannelSetting();
                        return;
                    }
                    installChannelNoSN = nosn;
                    installChannelMax = parseInt(res.maxChannel, 10) || 0;
                    if (installChannelMax <= 0) {
                        hideInstallChannelSetting();
                        return;
                    }
                    var pnl = document.getElementById('pnlChannelSetting');
                    if (pnl) { pnl.style.display = ''; }
                    loadInstallChannelRows();
                }).fail(function () {
                    hideInstallChannelSetting();
                });
            }

            function saveInstallChannelSetting() {
                if (!installChannelNoSN || installChannelMax <= 0) {
                    showChannelAlert('danger', 'NoSN atau MaxChannel tidak valid.');
                    return;
                }
                var selects = document.querySelectorAll('#channelSettingRows .channel-type-select');
                var settings = [];
                var seen = {};
                for (var i = 0; i < selects.length; i++) {
                    var cid = parseInt(selects[i].getAttribute('data-chanelid'), 10);
                    if (seen[cid]) {
                        showChannelAlert('danger', 'ChanelID ' + cid + ' duplikat.');
                        return;
                    }
                    seen[cid] = true;
                    settings.push({ ChanelID: cid, ChanelType: selects[i].value });
                }

                var btn = document.getElementById('btnSaveChannelSetting');
                if (btn) {
                    btn.disabled = true;
                    btn.innerHTML = '<i class="fa fa-circle-o-notch fa-spin"></i>&nbsp;Menyimpan...';
                }

                $.ajax({
                    url: CH_HANDLER, type: 'POST', dataType: 'json',
                    data: {
                        action: 'save',
                        nosn: installChannelNoSN,
                        maxchannel: installChannelMax,
                        settings: JSON.stringify(settings)
                    }
                }).done(function (res) {
                    if (btn) {
                        btn.disabled = false;
                        btn.innerHTML = '<i class="fa fa-save"></i>&nbsp;Save Setting Channel';
                    }
                    if (!res || !res.success) {
                        showChannelAlert('danger', (res && res.message) || 'Gagal menyimpan setting.');
                        return;
                    }
                    showChannelAlert('success', res.message || 'Setting channel tersimpan.');
                }).fail(function () {
                    if (btn) {
                        btn.disabled = false;
                        btn.innerHTML = '<i class="fa fa-save"></i>&nbsp;Save Setting Channel';
                    }
                    showChannelAlert('danger', 'Gagal terhubung ke server.');
                });
            }

            function bindEvents() {
                $(document).off('click.ijnOpen', '.btn-ijn-open').on('click.ijnOpen', '.btn-ijn-open', function (e) {
                    e.preventDefault();
                    var picker = $(this).data('picker');
                    openPicker(picker);
                });
                $(document).off('click.ijnPickInput', '.ijm-pick-input, .ijm-job-id-input').on('click.ijnPickInput', '.ijm-pick-input, .ijm-job-id-input', function () {
                    var id = this.id;
                    var map = { txtJobID: 'job', txtTechnicianID: 'tech', txtPoliceNo: 'vehicle', txtNoSN: 'device', txtNoGSM: 'gsm' };
                    if (map[id]) openPicker(map[id]);
                });
                $(document).off('click.ijnPick', '.btn-ijn-pick').on('click.ijnPick', '.btn-ijn-pick', function () {
                    var row = ijnPickerCache[parseInt($(this).data('idx'), 10)];
                    var def = ijnPickerDefs[ijnPickerType];
                    if (row && def && def.onPick) {
                        def.onPick(row);
                        ijnGetPickerModal().modal('hide');
                    }
                });
                $(document).off('click.ijnPickerSearch', '.btn-ijn-picker-search').on('click.ijnPickerSearch', '.btn-ijn-picker-search', function () {
                    loadPickerList($.trim(ijnGetPickerModal().find('.ijm-picker-search-input').val() || ''));
                });
                $(document).off('keypress.ijnPickerSearch', '.ijm-picker-search-input').on('keypress.ijnPickerSearch', '.ijm-picker-search-input', function (e) {
                    if (e.which === 13) { e.preventDefault(); loadPickerList($.trim($(this).val() || '')); }
                });
                $('#btnAccSearch').off('click').on('click', loadAccessories);
                $('#txtAccSearch').off('keypress').on('keypress', function (e) { if (e.which === 13) { e.preventDefault(); loadAccessories(); } });
                $('#btnAccCheckAll').off('click').on('click', function () { setAllAccChecked(true); });
                $('#btnAccUncheckAll').off('click').on('click', function () { setAllAccChecked(false); });
                $('#ijmAccCheckAllHead').off('change').on('change', function () { setAllAccChecked($(this).prop('checked')); });
                $(document).off('change.ijnAcc', '.ijm-acc-chk').on('change.ijnAcc', '.ijm-acc-chk', function () {
                    syncAccCheckAllState();
                    updateAccCount();
                });
                $('#cmbServerName').off('change').on('change', function () {
                    loadUserAccessList();
                    updateSaveButtonState();
                });
                $('#cmbUserAccess, #txtInstallDate').off('change').on('change', updateSaveButtonState);
                $('#txtInstallDate').off('keyup').on('keyup', updateSaveButtonState);
                $('#fuInstallPictures').off('change').on('change', function () {
                    var files = Array.prototype.slice.call(this.files || []);
                    this.value = '';
                    if (!files.length) return;
                    var merged = ijnSelectedPictureFiles.concat(files);
                    if (merged.length > 10) {
                        Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Maksimal 10 file gambar.' });
                        renderPicturePreview(ijnSelectedPictureFiles);
                        return;
                    }
                    ijnSelectedPictureFiles = merged;
                    renderPicturePreview(ijnSelectedPictureFiles);
                });
                $(document).off('click.ijnRemovePic', '.ijm-attach-remove').on('click.ijnRemovePic', '.ijm-attach-remove', function () {
                    var idx = parseInt($(this).attr('data-idx'), 10);
                    if (!isNaN(idx) && ijnSelectedPictureFiles[idx]) {
                        ijnSelectedPictureFiles.splice(idx, 1);
                        renderPicturePreview(ijnSelectedPictureFiles);
                    }
                });
                $(document).off('click.ijnSaveChannel', '#btnSaveChannelSetting').on('click.ijnSaveChannel', '#btnSaveChannelSetting', function (e) {
                    e.preventDefault();
                    saveInstallChannelSetting();
                });
                $('#btnSaveClient').off('click').on('click', function () {
                    Swal.fire({
                        title: 'Konfirmasi',
                        text: 'Apakah Anda yakin ingin menyimpan new installation ini?',
                        icon: 'question',
                        showCancelButton: true,
                        confirmButtonText: 'Ya, Simpan',
                        cancelButtonText: 'Batal'
                    }).then(function (r) {
                        if (r.isConfirmed) submitSave();
                        else updateSaveButtonState();
                    });
                });
            }

            function autoPickJobById(jobId) {
                var wantedJobId = $.trim(jobId || '');
                if (!wantedJobId) return Promise.resolve(false);
                return ijnCallPromise('GetNewInstallJobList', { Search: wantedJobId })
                    .then(function (res) {
                        var rows = (res && res.Data) ? res.Data : [];
                        var selected = null;
                        for (var i = 0; i < rows.length; i++) {
                            var rowJobId = cell(rows[i], ['JobID']);
                            if (ijnEquals(rowJobId, wantedJobId)) {
                                selected = rows[i];
                                break;
                            }
                        }
                        if (!selected) return false;
                        onPickJob(selected);
                        return true;
                    })
                    .catch(function () { return false; });
            }

            function autoPickTechnicianById(technicianId) {
                var wantedTechId = $.trim(technicianId || '');
                if (!wantedTechId) return Promise.resolve(false);
                var pickFromRows = function (rows) {
                    rows = rows || [];
                    var selected = null;
                    for (var i = 0; i < rows.length; i++) {
                        var rowTechId = cell(rows[i], ['TechnicianID']);
                        if (ijnEquals(rowTechId, wantedTechId)) {
                            selected = rows[i];
                            break;
                        }
                    }
                    if (!selected && rows.length === 1) selected = rows[0];
                    if (selected) {
                        onPickTech(selected);
                        return true;
                    }
                    return false;
                };
                return ijnCallPromise('GetNewInstallTechnicianList', { Search: wantedTechId })
                    .then(function (res) {
                        var rows = (res && res.Data) ? res.Data : [];
                        if (pickFromRows(rows)) return true;
                        return ijnCallPromise('GetNewInstallTechnicianList', { Search: '' })
                            .then(function (res2) {
                                var rows2 = (res2 && res2.Data) ? res2.Data : [];
                                if (pickFromRows(rows2)) return true;

                                // Final fallback: keep Technician ID populated from session
                                // even when lookup list does not return rows.
                                ijnSet('txtTechnicianID', wantedTechId);
                                ijnSet('hfTechnicianID', wantedTechId);
                                ijnSet('hfTechName', '');
                                ijmRo('TechnicianID', wantedTechId);
                                ijmRo('TechnicianName', '');
                                ijmRo('TechBranchName', '');
                                clearDeviceGsm();
                                loadAccessories();
                                updateSaveButtonState();
                                return true;
                            });
                    })
                    .catch(function () {
                        ijnSet('txtTechnicianID', wantedTechId);
                        ijnSet('hfTechnicianID', wantedTechId);
                        ijnSet('hfTechName', '');
                        ijmRo('TechnicianID', wantedTechId);
                        ijmRo('TechnicianName', '');
                        ijmRo('TechBranchName', '');
                        clearDeviceGsm();
                        loadAccessories();
                        updateSaveButtonState();
                        return true;
                    });
            }

            function initDefaultSelection() {
                var jobId = ijnGetJobIdFromUrl();
                var isTechUser = isLockedTechGroup();
                var defaultTechId = $.trim(ijnSessionTechnicianId || '');

                var chain = Promise.resolve();
                if (jobId) {
                    chain = chain.then(function () { return autoPickJobById(jobId); });
                }
                if (isTechUser && defaultTechId) {
                    chain = chain.then(function () { return autoPickTechnicianById(defaultTechId); });
                }
                return chain;
            }

            $(document).ready(function () {
                ijnSyncPickerModal();
                bindEvents();
                clearAccList();
                resetInstallationInfo();
                clearJobSummary();
                updateSaveButtonState();
                initDefaultSelection();
                if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        ijnSyncPickerModal();
                        bindEvents();
                        updateSaveButtonState();
                    });
                }
            });
        })();
    </script>
</asp:Content>
