<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="installation_job_maint.aspx.cs" Inherits="vtsadm.installation_job_maint" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .ijm-page .form-control { background-color: #fff !important; }
        .ijm-page .select2-container { width: 100% !important; display: block; }
        .ijm-page .box { border-radius: 4px; margin-bottom: 16px; box-shadow: 0 1px 3px rgba(0,0,0,.08); }
        .ijm-page .box-body { padding: 18px 20px; }
        .ijm-page .box-footer { padding: 12px 20px; background: #f9f9f9; border-top: 1px solid #eee; }
        .ijm-page .form-group-sm { margin-bottom: 14px; }
        .ijm-page .form-group-sm > label { font-size: 11px; font-weight: 600; color: #666; text-transform: uppercase; letter-spacing: .03em; margin-bottom: 5px; display: block; }
        .ijm-info-group { border: 1px solid #e8e8e8; border-radius: 4px; padding: 14px 16px 10px; margin-bottom: 15px; background: #fcfcfc; height: 100%; }
        .ijm-group-title { font-size: 13px; font-weight: 700; color: #003481; margin: 0 0 12px; padding-bottom: 8px; border-bottom: 2px solid #3c8dbc; }
        .ijm-group-title i { margin-right: 6px; color: #3c8dbc; }
        .ijm-job-meta { margin-bottom: 16px; padding-bottom: 14px; border-bottom: 1px dashed #ddd; }
        .ijm-tech-meta { margin-bottom: 16px; padding-bottom: 14px; border-bottom: 1px dashed #ddd; }
        .ijm-info-row { margin-bottom: 10px; }
        .ijm-info-row:last-child { margin-bottom: 0; }
        .ijm-info-row label { font-size: 11px; color: #888; margin-bottom: 3px; display: block; font-weight: 600; text-transform: uppercase; }
        .ijm-ro, .ijm-preview-val { background: #f5f7fa !important; color: #333; font-weight: 500; border: 1px solid #e3e8ef; min-height: 30px; box-shadow: none; word-break: break-word; }
        .ijm-maint-input { background: #fffdf7; border: 1px solid #f0ad4e; border-left: 4px solid #f39c12; border-radius: 4px; padding: 16px 18px; margin-bottom: 14px; }
        .ijm-maint-input label { font-size: 13px; font-weight: 700; color: #003481; margin-bottom: 8px; display: block; }
        .ijm-maint-hint { font-size: 12px; color: #777; margin: 0 0 16px; padding: 10px 12px; background: #fff8e6; border-radius: 4px; border-left: 3px solid #f39c12; line-height: 1.5; }
        .ijm-preview-box { border: 1px solid #e8e8e8; border-radius: 4px; padding: 14px 16px 10px; background: #fafafa; margin-bottom: 14px; }
        .ijm-preview-title { font-size: 12px; font-weight: 700; color: #666; text-transform: uppercase; margin: 0 0 12px; }
        .ijm-remark-block { margin-top: 4px; padding-top: 14px; border-top: 1px dashed #ddd; }
        .ijm-loading { display: none; color: #003481; font-size: 13px; margin-top: 8px; }
        .ijm-loading.active { display: block; }
        .ijm-req { color: #dd4b39; }
        .ijm-action-btns { text-align: right; }
        .ijm-action-btns .btn { min-width: 100px; margin-left: 6px; }
        .ijm-page .ijm-maint-type-select { width: 100%; height: 34px; padding: 6px 12px; }
        .ijm-page select.select2-hidden-accessible { clip: rect(0,0,0,0) !important; position: absolute !important; width: 1px !important; height: 1px !important; overflow: hidden !important; border: 0 !important; padding: 0 !important; }
        .ijm-page .select2-container--default .select2-selection--single { height: 34px; border-color: #d2d6de; }
        .ijm-page .select2-container--default .select2-selection--single .select2-selection__rendered { line-height: 32px; padding-left: 12px; }
        .ijm-page .select2-container--default .select2-selection--single .select2-selection__arrow { height: 32px; }
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
        .ijm-attach-remove { position: absolute; top: -8px; right: -8px; width: 20px; height: 20px; border: 0; border-radius: 50%; background: #dd4b39; color: #fff; font-size: 12px; line-height: 20px; text-align: center; cursor: pointer; padding: 0; box-shadow: 0 1px 4px rgba(0,0,0,.2); }
        .ijm-job-list-table { font-size: 12px; margin-bottom: 0; }
        .ijm-job-list-table th { background: #f5f7fa; color: #003481; white-space: nowrap; }
        .ijm-job-list-table td { vertical-align: middle !important; }
        .ijm-job-list-empty { padding: 24px; text-align: center; color: #888; }
        .ijm-job-list-loading { padding: 24px; text-align: center; color: #003481; }
        .ijm-job-list-error { margin: 12px 0; }
        .ijm-job-list-search { margin-bottom: 12px; }
        #modal-ijm-job { z-index: 1060 !important; }
        body > .modal-backdrop { z-index: 1040 !important; }
        .ijm-job-id-input, .ijm-pick-input { cursor: pointer; background-color: #fff !important; }
        .ijm-pick-input { border-left: 3px solid #3c8dbc; }
        .ijm-pick-input:hover { box-shadow: 0 0 0 2px rgba(60,141,188,.12); }
        .ijm-job-modal .modal-header, .ijm-picker-modal .modal-header {
            background: linear-gradient(135deg, #003481 0%, #3c8dbc 100%);
            color: #fff;
            border-radius: 4px 4px 0 0;
        }
        .ijm-job-modal .modal-header .close, .ijm-picker-modal .modal-header .close { color: #fff; opacity: .85; }
        .ijm-acc-toolbar { margin-bottom: 10px; display: flex; flex-wrap: wrap; gap: 8px; align-items: center; }
        .ijm-acc-table { font-size: 12px; margin-bottom: 0; }
        .ijm-acc-table th { background: #f5f7fa; color: #003481; white-space: nowrap; }
        .ijm-acc-table td { vertical-align: middle !important; }
        .ijm-acc-table tbody tr:hover { background: #f9fbfd; }
        .ijm-list-loading, .ijm-list-empty { padding: 20px; text-align: center; color: #666; }
        .ijm-list-loading { color: #003481; }
        .ijm-list-error { margin: 10px 0; }
        .ijm-picker-table { font-size: 12px; margin-bottom: 0; }
        .ijm-picker-table th { background: #f5f7fa; color: #003481; white-space: nowrap; }
        .ijm-customer-row .ijm-info-group { margin-bottom: 0; }
        @media (min-width: 992px) {
            .ijm-detail-cards .ijm-info-group { min-height: 200px; margin-bottom: 0; }
            .ijm-detail-cards > [class*="col-"] { margin-bottom: 0; }
            .ijm-job-row > [class*="col-"] { margin-bottom: 0; }
            .ijm-job-meta > [class*="col-"] { margin-bottom: 0; }
            .ijm-tech-meta > [class*="col-"] { margin-bottom: 0; }
            .ijm-customer-row .ijm-info-group { min-height: 120px; }
        }
        @media (max-width: 767px) {
            .ijm-job-row > [class*="col-"],
            .ijm-detail-cards > [class*="col-"],
            .ijm-job-meta > [class*="col-"],
            .ijm-tech-meta > [class*="col-"] { margin-bottom: 12px; }
            .ijm-page { padding-bottom: 80px; }
            .ijm-page .box-body { padding: 14px 12px; }
            .ijm-page .form-control, .ijm-page .select2-container .select2-selection--single { min-height: 44px; font-size: 16px; }
            .ijm-page .ijm-maint-type-select { min-height: 44px; font-size: 16px; }
            .ijm-page .select2-container .select2-selection--single .select2-selection__rendered { line-height: 42px; }
            .ijm-upload-drop { padding: 14px 10px; }
            .ijm-upload-title { font-size: 14px; }
            .ijm-upload-btn { min-height: 36px; line-height: 24px; }
            .ijm-attach-preview { grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 8px; }
            .ijm-attach-item img { height: 84px; }
            .ijm-action-wrap { position: fixed; bottom: 0; left: 0; right: 0; z-index: 1040; margin: 0; box-shadow: 0 -2px 10px rgba(0,0,0,.12); }
            .ijm-action-wrap .box { margin: 0; border-radius: 0; border-left: 0; border-right: 0; border-bottom: 0; }
            .ijm-action-btns { display: flex; flex-direction: column-reverse; gap: 8px; text-align: center; }
            .ijm-action-btns .btn { width: 100%; margin: 0; min-height: 44px; font-size: 16px; }
        }
    </style>

    <section class="content-header">
        <h1>Maintenance Job Order<small>Unified Maintenance</small></h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">Installation</a></li>
            <li><a href="#">Maintenance</a></li>
            <li class="active">Job Order</li>
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
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="form-group form-group-sm">
                                    <label>Job Maintenance Type <span class="ijm-req">*</span></label>
                                    <asp:DropDownList ID="CmbJobType" runat="server" CssClass="form-control ijm-maint-type-select" AutoPostBack="true" OnSelectedIndexChanged="CmbJobType_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="form-group form-group-sm">
                                    <label>Job ID <span class="ijm-req">*</span></label>
                                    <div class="input-group input-group-sm">
                                        <input type="text" id="txtJobID" runat="server" class="form-control ijm-job-id-input" placeholder="Klik tombol cari..." readonly="readonly" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-primary btn-sm" id="btnOpenJobModal" title="Cari Job ID"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <input type="hidden" id="txtMaintTypeDesc" runat="server" />
                            <input type="hidden" id="hfMaintTypeID" runat="server" />
                        </div>
                    </div>
                </div>

                <!-- Maintenance Input (input data baru) -->
                <asp:Panel ID="PnlMaintInfo" runat="server">
                    <div class="box box-warning box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-edit"></i> Maintenance Input</h3>
                            <div class="box-tools pull-right">
                                <span class="label label-warning">Input</span>
                            </div>
                        </div>
                        <div class="box-body">
                            <p class="ijm-maint-hint"><i class="fa fa-info-circle"></i> Isi / pilih data <strong>baru</strong> untuk maintenance. Detail referensi job tersedia di bagian bawah.</p>

                            <asp:Panel ID="PnlMaintDevice" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 col-xs-12">
                                        <div class="ijm-maint-input">
                                            <label><i class="fa fa-hdd-o"></i> Pilih Serial Number Device Baru <span class="ijm-req">*</span></label>
                                            <div class="input-group input-group-sm">
                                                <input type="text" id="txtPickNewNoSN" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-primary btn-sm btn-ijm-open-picker" data-picker="device" title="Cari No SN Baru"><i class="fa fa-search"></i></button>
                                                </span>
                                            </div>
                                            <asp:DropDownList ID="CmbNewNoSN" runat="server" CssClass="form-control select2-ajax-new" data-type="device" style="display:none;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-plug"></i> Interfacing Access</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Server Name</label>
                                            <select id="cmbDeviceServerName" class="form-control input-sm">
                                                <option value="">[Select]</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>User Access</label>
                                            <select id="cmbDeviceUserAccess" class="form-control input-sm">
                                                <option value="">[Select]</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail Device Baru</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Device Type Description</label>
                                            <input type="text" id="txtNewDeviceTypeDesc" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <input type="text" id="txtNewWarehouseName" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlMaintGsm" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 col-xs-12">
                                        <div class="ijm-maint-input">
                                            <label><i class="fa fa-mobile"></i> Pilih Nomor GSM Baru <span class="ijm-req">*</span></label>
                                            <div class="input-group input-group-sm">
                                                <input type="text" id="txtPickNewNoGSM" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-primary btn-sm btn-ijm-open-picker" data-picker="gsm" title="Cari No GSM Baru"><i class="fa fa-search"></i></button>
                                                </span>
                                            </div>
                                            <asp:DropDownList ID="CmbNewNoGSM" runat="server" CssClass="form-control select2-ajax-new" data-type="gsm" style="display:none;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail GSM Baru</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Provider Name</label>
                                            <input type="text" id="txtNewProviderName" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <input type="text" id="txtNewGsmWarehouse" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlMaintCustomer" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 col-xs-12">
                                        <div class="ijm-maint-input">
                                            <label><i class="fa fa-user"></i> Pilih Customer Tujuan Maintenance <span class="ijm-req">*</span></label>
                                            <div class="input-group input-group-sm">
                                                <input type="text" id="txtPickNewCustomer" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-primary btn-sm btn-ijm-open-picker" data-picker="customer" title="Cari Customer Baru"><i class="fa fa-search"></i></button>
                                                </span>
                                            </div>
                                            <asp:DropDownList ID="CmbNewCustomer" runat="server" CssClass="form-control select2-ajax-new" data-type="customer" style="display:none;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-server"></i> Interfacing Server</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Server Name</label>
                                            <select id="cmbCustomerServerName" class="form-control input-sm">
                                                <option value="">[Select]</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail Customer Baru</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Branch Name</label>
                                            <input type="text" id="txtNewBranchName" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Marketing Name</label>
                                            <input type="text" id="txtNewMarketingName" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlMaintServer" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 col-xs-12">
                                        <div class="ijm-maint-input">
                                            <label><i class="fa fa-server"></i> Pilih Server Baru <span class="ijm-req">*</span></label>
                                            <asp:DropDownList ID="CmbNewServerName" runat="server" CssClass="form-control ijm-select2"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlMaintVehicle" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 col-xs-12">
                                        <div class="ijm-maint-input">
                                            <label><i class="fa fa-car"></i> Pilih Nomor Polisi Kendaraan Baru <span class="ijm-req">*</span></label>
                                            <div class="input-group input-group-sm">
                                                <input type="text" id="txtPickNewPoliceNo" class="form-control ijm-pick-input" placeholder="Klik cari..." readonly="readonly" />
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-primary btn-sm btn-ijm-open-picker" data-picker="vehicle" title="Cari Police No Baru"><i class="fa fa-search"></i></button>
                                                </span>
                                            </div>
                                            <asp:DropDownList ID="CmbNewPoliceNo" runat="server" CssClass="form-control select2-ajax-new" data-type="vehicle" style="display:none;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="ijm-preview-box">
                                    <p class="ijm-preview-title"><i class="fa fa-check-circle"></i> Detail Vehicle Baru</p>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Vehicle Description</label>
                                            <input type="text" id="txtNewVehicleDesc" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                        <div class="col-md-6 col-sm-12 col-xs-12 ijm-info-row">
                                            <label>Asset No</label>
                                            <input type="text" id="txtNewAssetNo" runat="server" class="form-control input-sm ijm-preview-val" readonly="readonly" placeholder="-" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="ijm-remark-block">
                                <div class="form-group form-group-sm">
                                    <label>Remark</label>
                                    <input type="text" id="txtRemark" runat="server" class="form-control" placeholder="Catatan maintenance (opsional)..." />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Detail Information (referensi data saat ini) -->
                <asp:Panel ID="PnlDetailInfo" runat="server">
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
                                    <input type="hidden" id="txtRegDate" runat="server" />
                                </div>
                                <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                    <label>Schedule Date</label>
                                    <div class="ijm-ro form-control input-sm" data-field="SchDate">-</div>
                                    <input type="hidden" id="txtSchDate" runat="server" />
                                </div>
                                <div class="col-md-4 col-sm-12 col-xs-12 ijm-info-row">
                                    <label>Server Name</label>
                                    <div class="ijm-ro form-control input-sm" data-field="ServerNameInfo">-</div>
                                    <input type="hidden" id="txtServerNameInfo" runat="server" />
                                </div>
                            </div>
                            <div class="row ijm-tech-meta">
                                <div class="col-xs-12">
                                    <p class="ijm-group-title"><i class="fa fa-user-md"></i> Technician</p>
                                </div>
                                <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                    <label>Technician ID</label>
                                    <div class="ijm-ro form-control input-sm" data-field="TechnicianID">-</div>
                                    <input type="hidden" id="txtTechnicianID" runat="server" />
                                    <input type="hidden" id="hfTechnicianID" runat="server" />
                                </div>
                                <div class="col-md-4 col-sm-6 col-xs-12 ijm-info-row">
                                    <label>Name</label>
                                    <div class="ijm-ro form-control input-sm" data-field="TechnicianName">-</div>
                                    <input type="hidden" id="txtTechnicianName" runat="server" />
                                </div>
                                <div class="col-md-4 col-sm-12 col-xs-12 ijm-info-row">
                                    <label>Branch Name</label>
                                    <div class="ijm-ro form-control input-sm" data-field="TechBranchName">-</div>
                                    <input type="hidden" id="txtTechBranchName" runat="server" />
                                </div>
                            </div>
                            <div class="row ijm-detail-cards">
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="ijm-info-group">
                                        <p class="ijm-group-title"><i class="fa fa-building"></i> Customer</p>
                                        <div class="ijm-info-row">
                                            <label>Customer Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="CustomerName">-</div>
                                            <input type="hidden" id="txtCustomerName" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Branch Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="BranchName">-</div>
                                            <input type="hidden" id="txtBranchName" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Marketing Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="MarketingName">-</div>
                                            <input type="hidden" id="txtMarketingName" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="ijm-info-group">
                                        <p class="ijm-group-title"><i class="fa fa-car"></i> Nopol</p>
                                        <div class="ijm-info-row">
                                            <label>Police No</label>
                                            <div class="ijm-ro form-control input-sm" data-field="PoliceNo">-</div>
                                            <input type="hidden" id="txtPoliceNo" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Vehicle Description</label>
                                            <div class="ijm-ro form-control input-sm" data-field="VehicleDesc">-</div>
                                            <input type="hidden" id="txtVehicleDesc" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Asset No</label>
                                            <div class="ijm-ro form-control input-sm" data-field="AssetNo">-</div>
                                            <input type="hidden" id="txtAssetNo" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="ijm-info-group">
                                        <p class="ijm-group-title"><i class="fa fa-hdd-o"></i> SN</p>
                                        <div class="ijm-info-row">
                                            <label>No SN</label>
                                            <div class="ijm-ro form-control input-sm" data-field="NoSNInfo">-</div>
                                            <input type="hidden" id="txtNoSNInfo" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Device Type Description</label>
                                            <div class="ijm-ro form-control input-sm" data-field="DeviceTypeDesc">-</div>
                                            <input type="hidden" id="txtDeviceTypeDesc" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="WarehouseName">-</div>
                                            <input type="hidden" id="txtWarehouseName" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="ijm-info-group">
                                        <p class="ijm-group-title"><i class="fa fa-mobile"></i> GSM</p>
                                        <div class="ijm-info-row">
                                            <label>No GSM</label>
                                            <div class="ijm-ro form-control input-sm" data-field="NoGSMInfo">-</div>
                                            <input type="hidden" id="txtNoGSMInfo" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Provider Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="ProviderName">-</div>
                                            <input type="hidden" id="txtProviderName" runat="server" />
                                        </div>
                                        <div class="ijm-info-row">
                                            <label>Warehouse Name</label>
                                            <div class="ijm-ro form-control input-sm" data-field="GsmWarehouseName">-</div>
                                            <input type="hidden" id="txtGsmWarehouseName" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Hidden Fields -->
                <input type="hidden" id="hfTvdID" runat="server" />
                <input type="hidden" id="hfTvaID" runat="server" />
                <input type="hidden" id="hfTdtID" runat="server" />
                <input type="hidden" id="hfTgtID" runat="server" />
                <input type="hidden" id="hfDeviceID" runat="server" />
                <input type="hidden" id="hfGsmID" runat="server" />
                <input type="hidden" id="hfVehicleID" runat="server" />
                <input type="hidden" id="hfCustID" runat="server" />
                <input type="hidden" id="hfNewTdtID" runat="server" />
                <input type="hidden" id="hfNewTgtID" runat="server" />
                <input type="hidden" id="hfNewTvaID" runat="server" />
                <input type="hidden" id="hfNewCustID" runat="server" />
                <input type="hidden" id="hfNewDeviceID" runat="server" />
                <input type="hidden" id="hfNewGsmID" runat="server" />
                <input type="hidden" id="hfNewVehicleID" runat="server" />
                <input type="hidden" id="hfProcessKey" runat="server" />
                <input type="hidden" id="hfJobCustID" runat="server" />
                <input type="hidden" id="hfJobID" runat="server" />
                <input type="hidden" id="hfServerID" runat="server" />

                <!-- Action -->
                <div class="ijm-action-wrap">
                    <div class="box box-solid">
                        <div class="box-footer ijm-action-btns">
                            <button type="button" class="btn btn-primary" id="btnSaveClient" disabled="disabled" title="Save aktif setelah data wajib lengkap"><i class="fa fa-save"></i> Save</button>
                            <button id="CmdCancel" type="button" class="btn btn-default" runat="server" onserverclick="CmdCancel_ServerClick"><i class="fa fa-times"></i> Cancel</button>
                            <asp:Button ID="CmdSave" runat="server" Text="Save" Style="display:none;" OnClick="CmdSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Job ID (di-sync ke body via JS agar tidak duplikat setelah UpdatePanel) -->
        <div class="modal fade ijm-job-modal" id="modal-ijm-job" tabindex="-1" role="dialog" data-backdrop="static">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-list"></i> Pilih Job ID</h4>
                    </div>
                    <div class="modal-body ijm-job-modal-body" style="max-height: 480px; overflow-y: auto;">
                        <div class="ijm-job-list-search">
                            <div class="input-group input-group-sm">
                                <input type="text" class="form-control ijm-job-search-input" placeholder="Cari Job ID atau Customer Name..." maxlength="100" autocomplete="off" />
                                <span class="input-group-btn">
                                    <button type="button" class="btn btn-primary btn-ijm-job-search" title="Cari"><i class="fa fa-search"></i> Cari</button>
                                </span>
                            </div>
                        </div>
                        <div class="ijm-job-list-loading"><i class="fa fa-spinner fa-spin"></i> Memuat data job...</div>
                        <div class="ijm-job-list-empty" style="display:none;">Tidak ada data job untuk tipe maintenance ini.</div>
                        <div class="ijm-job-list-error alert alert-danger" style="display:none;"></div>
                        <div class="table-responsive ijm-job-list-wrap" style="display:none;">
                            <table class="table table-bordered table-striped table-hover ijm-job-list-table">
                                <thead>
                                    <tr>
                                        <th>Job ID</th>
                                        <th>Reg Date</th>
                                        <th>Customer Name</th>
                                        <th>Po ID</th>
                                        <th>Device Type</th>
                                        <th>Sch Date</th>
                                        <th>Police No</th>
                                        <th class="text-center">Action</th>
                                    </tr>
                                </thead>
                                <tbody class="ijm-job-list-body"></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Tutup</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Generic Picker Modal -->
        <div class="modal fade ijm-picker-modal" id="modal-ijm-picker" tabindex="-1" role="dialog" data-backdrop="static">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-search"></i> <span id="ijmPickerTitle">Pilih</span></h4>
                    </div>
                    <div class="modal-body" style="max-height:480px;overflow-y:auto;">
                        <div class="ijm-list-search" style="margin-bottom:12px;">
                            <div class="input-group input-group-sm">
                                <input type="text" class="form-control ijm-picker-search-input" placeholder="Ketik kata kunci..." maxlength="100" autocomplete="off" />
                                <span class="input-group-btn">
                                    <button type="button" class="btn btn-primary btn-ijm-picker-search"><i class="fa fa-search"></i> Cari</button>
                                </span>
                            </div>
                        </div>
                        <div class="ijm-list-loading ijm-picker-loading"><i class="fa fa-spinner fa-spin"></i> Memuat data...</div>
                        <div class="ijm-list-empty ijm-picker-empty" style="display:none;">Tidak ada data.</div>
                        <div class="ijm-list-error alert alert-danger ijm-picker-error" style="display:none;"></div>
                        <div class="table-responsive ijm-picker-wrap" style="display:none;">
                            <table class="table table-bordered table-striped table-hover ijm-picker-table">
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
        var ijmNewExtraMap = {};
        var ijmPickerExtraMap = {};
        var ijmJobListCache = [];
        var ijmMaintSelectedFiles = [];
        var ijmPickerType = null;
        var ijmPickerCache = [];

        function ijmId(name) { return '[id$=' + name + ']'; }
        function ijmVal(name) { var el = $(ijmId(name)); return el.length ? el.val() : ''; }
        function ijmSet(name, val) { $(ijmId(name)).val(val || ''); }
        function ijmText(name, val) {
            var v = val || '-';
            $('.ijm-ro[data-field="' + name + '"]').text(v);
            ijmSet('txt' + name, val);
        }
        function ijmEscHtml(s) {
            return String(s || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        }

        function ijmHumanFileSize(bytes) {
            var b = Number(bytes || 0);
            if (b < 1024) return b + ' B';
            if (b < 1024 * 1024) return (b / 1024).toFixed(1) + ' KB';
            return (b / (1024 * 1024)).toFixed(1) + ' MB';
        }

        function ijmUpdateMaintUploadMeta(files) {
            var n = (files || []).length;
            var $meta = $('#ijmMaintUploadMeta');
            if (!$meta.length) return;
            if (!n) {
                $meta.text('Belum ada file dipilih');
                return;
            }
            var total = 0;
            files.forEach(function (f) { total += Number(f.size || 0); });
            $meta.text(n + ' file dipilih (' + ijmHumanFileSize(total) + ')');
        }

        function ijmRenderMaintAttachPreview(files) {
            var $wrap = $('#ijmMaintPicturePreview');
            if (!$wrap.length) return;
            $wrap.empty();
            if (!files || !files.length) {
                $wrap.hide();
                ijmUpdateMaintUploadMeta([]);
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
            ijmUpdateMaintUploadMeta(files);
        }

        function ijmBindMaintAttachmentEvents() {
            $('#fuMaintPictures').off('change').on('change', function () {
                var files = Array.prototype.slice.call(this.files || []);
                this.value = '';
                if (!files.length) return;
                var merged = ijmMaintSelectedFiles.concat(files);
                if (merged.length > 10) {
                    Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Maksimal 10 file gambar.' });
                    ijmRenderMaintAttachPreview(ijmMaintSelectedFiles);
                    return;
                }
                var allowed = { '.jpg': true, '.jpeg': true, '.png': true };
                for (var i = 0; i < merged.length; i++) {
                    var nm = String(merged[i].name || '');
                    var dot = nm.lastIndexOf('.');
                    var ext = (dot >= 0 ? nm.substring(dot).toLowerCase() : '');
                    if (!allowed[ext]) {
                        Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Format file harus jpg, jpeg, atau png.' });
                        return;
                    }
                }
                ijmMaintSelectedFiles = merged;
                ijmRenderMaintAttachPreview(ijmMaintSelectedFiles);
            });
            $(document).off('click.ijmMaintRemovePic', '.ijm-attach-remove').on('click.ijmMaintRemovePic', '.ijm-attach-remove', function () {
                var idx = parseInt($(this).attr('data-idx'), 10);
                if (!isNaN(idx) && ijmMaintSelectedFiles[idx]) {
                    ijmMaintSelectedFiles.splice(idx, 1);
                    ijmRenderMaintAttachPreview(ijmMaintSelectedFiles);
                }
            });
        }

        function getMaintTypeId() {
            var $cmb = $(ijmId('CmbJobType'));
            if ($cmb.length) {
                var val = $.trim($cmb.val() || '');
                if (val && val !== '[Select]') return val;
            }
            var hid = $.trim(ijmVal('hfMaintTypeID') || '');
            if (hid && hid !== '[Select]') return hid;
            return '';
        }

        function getMaintTypeDesc() {
            var $cmb = $(ijmId('CmbJobType'));
            if (!$cmb.length) return '';
            return $.trim($cmb.find('option:selected').text() || '');
        }

        function ijmSyncMaintTypeFromDropdown() {
            var id = getMaintTypeId();
            var desc = getMaintTypeDesc();
            ijmSet('hfMaintTypeID', id);
            ijmSet('txtMaintTypeDesc', desc === '[Select]' ? '' : desc);
            if (id) ijmSet('hfProcessKey', resolveProcessKey(id, desc));
        }
        function getProcessKey() { return ijmVal('hfProcessKey'); }

        function resolveProcessKey(maintTypeId, maintTypeDesc) {
            var id = (maintTypeId || '').toUpperCase();
            var map = {
                'MTY0000001': 'vehicle_maint', 'MTY0000002': 'device_maint', 'MTY0000003': 'gsm_maint',
                'MTY0000004': 'others_maint', 'MTY0000005': 'accessories_maint', 'MTY0000006': 'uninstall_maint',
                'MTY0000007': 'server_maint', 'MTY0000008': 'customer_maint', 'MTY0000009': 'sb_maint',
                'MTY0000010': 'ub_maint', 'MTY0000011': 'sp_maint', 'MTY0000012': 'gsm_maint_suspend'
            };
            if (map[id]) return map[id];
            var d = (maintTypeDesc || '').toLowerCase();
            if (d.indexOf('vehicle') >= 0) return 'vehicle_maint';
            if (d.indexOf('device') >= 0) return 'device_maint';
            if (d.indexOf('reactivated') >= 0 && d.indexOf('gsm') >= 0) return 'gsm_maint_suspend';
            if (d.indexOf('gsm') >= 0) return 'gsm_maint';
            if (d.indexOf('customer') >= 0) return 'customer_maint';
            if (d.indexOf('server') >= 0) return 'server_maint';
            if (d.indexOf('accessories') >= 0) return 'accessories_maint';
            if (d.indexOf('uninstall') >= 0) return 'uninstall_maint';
            if (d.indexOf('soft') >= 0 && d.indexOf('block') >= 0) return 'sb_maint';
            if (d.indexOf('unblock') >= 0) return 'ub_maint';
            if (d.indexOf('suspend') >= 0) return 'sp_maint';
            if (d.indexOf('reactivat') >= 0) return 're_maint';
            if (d.indexOf('other') >= 0) return 'others_maint';
            return 'others_maint';
        }

        function ijmIsMobile() { return window.innerWidth < 768; }

        function ijmSelect2Base(extra) {
            var opts = $.extend({
                width: '100%',
                allowClear: true,
                dropdownAutoWidth: false,
                dropdownCssClass: ijmIsMobile() ? 'ijm-select2-mobile' : ''
            }, extra || {});
            if (ijmIsMobile()) opts.dropdownParent = $(document.body);
            return opts;
        }

        function ijmParseApi(r) {
            if (r == null) return null;
            if (typeof r === 'object') return r;
            var raw = String(r || '');
            if (/<\s*html|<!doctype/i.test(raw)) {
                return { Success: false, Message: 'Respons server tidak valid. Silakan refresh halaman lalu coba lagi.' };
            }
            try { return JSON.parse(r); } catch (e) { return { Success: false, Message: String(r) }; }
        }

        function ijmNormalizeErrorMessage(raw, fallback) {
            var msg = '';
            if (raw == null) msg = '';
            else if (typeof raw === 'string') msg = raw;
            else if (typeof raw.get_message === 'function') msg = raw.get_message();
            else if (raw.message) msg = raw.message;
            else if (raw.Message) msg = raw.Message;
            else msg = String(raw);

            msg = $.trim(msg || '');
            if (!msg) return fallback || 'Terjadi kesalahan.';
            if (/<\s*html|<!doctype/i.test(msg)) {
                var titleMatch = msg.match(/<title[^>]*>([^<]+)<\/title>/i);
                var title = titleMatch && titleMatch[1] ? $.trim(titleMatch[1]) : '';
                if (title) return (fallback || 'Terjadi kesalahan di server.') + ' [' + title + ']';
                return fallback || 'Terjadi kesalahan di server. Silakan refresh halaman lalu coba lagi.';
            }
            if (msg.length > 520) msg = msg.substring(0, 520) + ' ...';
            return msg;
        }

        function ijmSyncJobModal() {
            var $fresh = $('.ijm-page .ijm-job-modal');
            if ($fresh.length) {
                $('body > .ijm-job-modal').remove();
                $fresh.first().appendTo('body');
            }
        }

        function ijmGetModal() {
            var $m = $('body > .ijm-job-modal');
            if (!$m.length) {
                ijmSyncJobModal();
                $m = $('body > .ijm-job-modal');
            }
            if (!$m.length) {
                $m = $('.ijm-page .ijm-job-modal').first();
            }
            return $m;
        }

        function ijmSyncPickerModal() {
            var $fresh = $('.ijm-page .ijm-picker-modal');
            if ($fresh.length) {
                $('body > .ijm-picker-modal').remove();
                $fresh.first().appendTo('body');
            }
        }

        function ijmGetPickerModal() {
            var $m = $('body > .ijm-picker-modal');
            if (!$m.length) {
                ijmSyncPickerModal();
                $m = $('body > .ijm-picker-modal');
            }
            if (!$m.length) $m = $('.ijm-page .ijm-picker-modal').first();
            return $m;
        }

        var ijmPickerDefs = {
            customer: {
                title: 'Pilih Customer Tujuan Maintenance',
                placeholder: 'Cari Customer...',
                require: function () { return !!ijmVal('hfCustID'); },
                requireMsg: 'Pilih Job ID terlebih dahulu.',
                cols: [
                    { key: 'CustID', label: 'Customer ID' },
                    { key: 'FullName', label: 'Full Name' },
                    { key: 'CustTypeDesc', label: 'Customer Type' },
                    { key: 'BranchName', label: 'Branch Name' }
                ],
                load: function (term, ok, fail) {
                    ijmCallMethod('GetNewCustomerList', { custId: ijmVal('hfCustID'), search: (term || '') }, function (data) {
                        if (!data || !data.Success) { fail((data && data.Message) || 'Gagal memuat data customer.'); return; }
                        ok(ijmNormalizePickerRows(data.Data || []));
                    }, function (err) {
                        fail(ijmNormalizeErrorMessage(err, 'Gagal memuat data customer.'));
                    });
                },
                selectId: 'CmbNewCustomer',
                inputId: 'txtPickNewCustomer'
            },
            device: {
                title: 'Pilih Serial Number Device Baru',
                placeholder: 'Cari No SN...',
                require: function () { return !!ijmVal('hfTechnicianID'); },
                requireMsg: 'Pilih Job ID terlebih dahulu.',
                cols: [
                    { key: 'DeviceID', label: 'Device ID' },
                    { key: 'NoSN', label: 'No SN' },
                    { key: 'VendorName', label: 'Vendor Name' },
                    { key: 'DeviceTypeDesc', label: 'Device Type Desc' },
                    { key: 'SourceName', label: 'Source Name' },
                    { key: 'WarehouseName', label: 'Warehouse Name' }
                ],
                load: function (term, ok, fail) {
                    ijmCallMethod('GetNewDeviceList', { technicianId: ijmVal('hfTechnicianID'), search: (term || '') }, function (data) {
                        if (!data || !data.Success) { fail((data && data.Message) || 'Gagal memuat data device.'); return; }
                        ok(ijmNormalizePickerRows(data.Data || []));
                    }, function (err) {
                        fail(ijmNormalizeErrorMessage(err, 'Gagal memuat data device.'));
                    });
                },
                selectId: 'CmbNewNoSN',
                inputId: 'txtPickNewNoSN'
            },
            gsm: {
                title: 'Pilih Nomor GSM Baru',
                placeholder: 'Cari No GSM...',
                require: function () { return !!ijmVal('hfTechnicianID'); },
                requireMsg: 'Pilih Job ID terlebih dahulu.',
                cols: [
                    { key: 'GsmID', label: 'GSM ID' },
                    { key: 'MSIDN', label: 'MSIDN' },
                    { key: 'datearrival', label: 'Date Arrival' },
                    { key: 'ProviderName', label: 'Provider Name' },
                    { key: 'SourceName', label: 'Source Name' },
                    { key: 'WarehouseName', label: 'Warehouse Name' }
                ],
                load: function (term, ok, fail) {
                    ijmCallMethod('GetNewGsmList', { technicianId: ijmVal('hfTechnicianID'), search: (term || '') }, function (data) {
                        if (!data || !data.Success) { fail((data && data.Message) || 'Gagal memuat data GSM.'); return; }
                        ok(ijmNormalizePickerRows(data.Data || []));
                    }, function (err) {
                        fail(ijmNormalizeErrorMessage(err, 'Gagal memuat data GSM.'));
                    });
                },
                selectId: 'CmbNewNoGSM',
                inputId: 'txtPickNewNoGSM'
            },
            vehicle: {
                title: 'Pilih Nomor Polisi Kendaraan Baru',
                placeholder: 'Cari Police No...',
                require: function () { return !!ijmVal('hfCustID'); },
                requireMsg: 'Pilih Job ID terlebih dahulu.',
                cols: [
                    { key: 'VehicleID', label: 'Vehicle ID' },
                    { key: 'VehicleDesc', label: 'Vehicle Desc' },
                    { key: 'PoliceNo', label: 'Police No' },
                    { key: 'AssetNo', label: 'Asset No' },
                    { key: 'BranchName', label: 'Branch Name' },
                    { key: 'FullName', label: 'Customer Name' }
                ],
                load: function (term, ok, fail) {
                    ijmCallMethod('GetNewVehicleList', { custId: ijmVal('hfCustID'), search: (term || '') }, function (data) {
                        if (!data || !data.Success) { fail((data && data.Message) || 'Gagal memuat data kendaraan.'); return; }
                        ok(ijmNormalizePickerRows(data.Data || []));
                    }, function (err) {
                        fail(ijmNormalizeErrorMessage(err, 'Gagal memuat data kendaraan.'));
                    });
                },
                selectId: 'CmbNewPoliceNo',
                inputId: 'txtPickNewPoliceNo'
            }
        };

        function ijmNormalizePickerRows(items) {
            return (items || []).map(function (it) {
                var row = {};
                if (it && it.Extra) {
                    try {
                        row = JSON.parse(it.Extra) || {};
                    } catch (e) {
                        row = {};
                    }
                }
                row.Value = it ? (it.Value || '') : '';
                row.Text = it ? (it.Text || '') : '';
                row.Extra = it ? (it.Extra || '') : '';

                // Normalisasi key agar renderer kolom konsisten.
                row.NoSN = row.NoSN || row.Value;
                row.MSIDN = row.MSIDN || row.Value;
                row.PoliceNo = row.PoliceNo || row.Value;
                row.CustID = row.CustID || row.Value;
                row.FullName = row.FullName || row.Text;
                row.SourceName = row.SourceName || row.sourcename || row.WarehouseName || row.warehousename || '';
                row.WarehouseName = row.WarehouseName || row.warehousename || '';
                row.datearrival = row.datearrival || row.DateArrival || '';
                return row;
            });
        }

        function ijmPickerEsc(s) {
            return String(s || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        }

        function ijmResetPickerList($m) {
            $m.find('.ijm-picker-loading').show();
            $m.find('.ijm-picker-empty, .ijm-picker-error, .ijm-picker-wrap').hide();
            $m.find('.ijm-picker-tbody').empty();
            $m.find('.ijm-picker-thead').empty();
        }

        function openMaintPicker(type) {
            var def = ijmPickerDefs[type];
            if (!def) return;
            if (def.require && !def.require()) {
                Swal.fire({ icon: 'warning', title: 'Perhatian', text: def.requireMsg || 'Lengkapi data sebelumnya.' });
                return;
            }
            ijmPickerType = type;
            ijmSyncPickerModal();
            var $m = ijmGetPickerModal();
            if (!$m.length) return;
            $m.find('#ijmPickerTitle').text(def.title);
            $m.find('.ijm-picker-search-input').attr('placeholder', def.placeholder || 'Cari...').val('');
            ijmResetPickerList($m);
            $m.modal({ backdrop: 'static', keyboard: true, show: true });
            setTimeout(function () { $m.find('.ijm-picker-search-input').focus(); }, 300);
            loadMaintPickerList('');
        }

        function loadMaintPickerList(term) {
            var def = ijmPickerDefs[ijmPickerType];
            if (!def) return;
            var $m = ijmGetPickerModal();
            ijmResetPickerList($m);
            def.load(term || '', function (rows) {
                $m.find('.ijm-picker-loading').hide();
                ijmPickerCache = rows || [];
                renderMaintPickerList(ijmPickerCache, def);
            }, function (msg) {
                $m.find('.ijm-picker-loading').hide();
                $m.find('.ijm-picker-error').text(ijmNormalizeErrorMessage(msg, 'Gagal memuat data.')).show();
            });
        }

        function renderMaintPickerList(rows, def) {
            var $m = ijmGetPickerModal();
            var $thead = $m.find('.ijm-picker-thead');
            var $tbody = $m.find('.ijm-picker-tbody');
            $thead.empty();
            $tbody.empty();
            if (!rows || !rows.length) {
                $m.find('.ijm-picker-empty').show();
                return;
            }
            var h = '<tr>';
            (def.cols || []).forEach(function (c) { h += '<th>' + ijmPickerEsc(c.label) + '</th>'; });
            h += '<th class="text-center">Action</th></tr>';
            $thead.html(h);

            rows.forEach(function (row, idx) {
                var tr = '<tr>';
                (def.cols || []).forEach(function (c) {
                    tr += '<td>' + ijmPickerEsc(row[c.key] || row[(c.key || '').toLowerCase()] || '') + '</td>';
                });
                tr += '<td class="text-center"><button type="button" class="btn btn-success btn-xs btn-ijm-picker-pick" data-idx="' + idx + '"><i class="fa fa-check"></i> Pilih</button></td></tr>';
                $tbody.append(tr);
            });
            $m.find('.ijm-picker-wrap').show();
        }

        function pickMaintItem(idx) {
            var row = ijmPickerCache[idx];
            var type = ijmPickerType;
            var def = ijmPickerDefs[type];
            if (!row || !def) return;
            var displayValue = type === 'customer' ? (row.FullName || row.Text || row.Value) : (row.Value || row.Text);

            ensureSelectOption(def.selectId, row.Value, row.Text || row.Value);
            $(ijmId(def.selectId)).val(row.Value).trigger('change');
            $(ijmId(def.inputId)).val(displayValue || '');
            ijmPickerExtraMap[type + '_' + row.Value] = row.Extra || '';
            ijmNewExtraMap[type + '_' + row.Value] = row.Extra || '';
            onNewItemChanged(type, row.Value);
            updateSaveButtonState();
            ijmGetPickerModal().modal('hide');
        }

        function ijmModalReset() {
            var $m = ijmGetModal();
            $m.find('.ijm-job-search-input').val('');
            $m.find('.ijm-job-list-loading').show();
            $m.find('.ijm-job-list-empty').hide();
            $m.find('.ijm-job-list-error').hide().text('');
            $m.find('.ijm-job-list-wrap').hide();
            $m.find('.ijm-job-list-body').empty();
        }

        function getJobSearchTerm() {
            return $.trim(ijmGetModal().find('.ijm-job-search-input').val() || '');
        }

        function triggerJobSearch() {
            var maintTypeId = getMaintTypeId();
            if (!maintTypeId) return;
            loadJobMaintList(maintTypeId, getJobSearchTerm());
        }

        var ijmPageUrl = '<%= ResolveUrl("~/installation_job_maint.aspx") %>';

        function ijmGetMethodUrls(methodName) {
            var urls = [];
            var pushUrl = function (u) {
                var x = $.trim(u || '');
                if (!x) return;
                if (urls.indexOf(x) < 0) urls.push(x);
            };
            pushUrl(ijmPageUrl + '/' + methodName);
            var p = (window.location && window.location.pathname) ? window.location.pathname : '';
            if (p) {
                // Route rewrite safe: /installation_job_maint or /installation_job_maint.aspx
                pushUrl(p.replace(/\/+$/, '') + '/' + methodName);
                if (p.toLowerCase().indexOf('.aspx') < 0) {
                    pushUrl(p.replace(/\/+$/, '') + '.aspx/' + methodName);
                }
            }
            pushUrl('/installation_job_maint.aspx/' + methodName);
            return urls;
        }

        function ijmCallMethod(methodName, params, onOk, onFail) {
            var urls = ijmGetMethodUrls(methodName);
            var idx = 0;
            var lastMsg = '';

            function nextTry() {
                if (idx >= urls.length) {
                    onFail(ijmNormalizeErrorMessage(lastMsg, 'Gagal memuat data.'));
                    return;
                }
                var targetUrl = urls[idx++];
                $.ajax({
                    type: 'POST',
                    url: targetUrl,
                    data: JSON.stringify(params || {}),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        var body = response && response.d;
                        var res = ijmParseApi(body);
                        if (!res || /Respons server tidak valid/i.test((res.Message || ''))) {
                            lastMsg = (res && res.Message) || 'Response tidak valid.';
                            nextTry();
                            return;
                        }
                        onOk(res);
                    },
                    error: function (xhr) {
                        var msg = 'Gagal memuat data.';
                        try {
                            var j = JSON.parse(xhr.responseText || '{}');
                            if (j && j.Message) msg = j.Message;
                        } catch (e) { }
                        lastMsg = msg;
                        nextTry();
                    }
                });
            }

            nextTry();
        }

        function ijmDestroySelect2() {
            $('.ijm-select2, .select2-ajax-new').each(function () {
                var $el = $(this);
                if ($el.hasClass('select2-hidden-accessible')) {
                    try { $el.select2('destroy'); } catch (e) { }
                }
            });
        }

        function ijmInitAllSelect2() {
            ijmDestroySelect2();
            initSelect2();
            initNewSelect2();
        }

        function initSelect2() {
            $('.ijm-select2:visible').each(function () {
                var $el = $(this);
                if ($el.hasClass('select2-hidden-accessible')) {
                    try { $el.select2('destroy'); } catch (e) { }
                }
                $el.select2(ijmSelect2Base({ placeholder: '[Select]' }));
            });
        }

        function reinitSingleSelect2(selId, extra) {
            var $s = $(ijmId(selId));
            if (!$s.length) return;
            if ($s.hasClass('select2-hidden-accessible')) $s.select2('destroy');
            $s.select2(ijmSelect2Base(extra || { placeholder: '[Select]' }));
        }

        function openJobModal() {
            ijmSyncMaintTypeFromDropdown();
            var maintTypeId = getMaintTypeId();
            if (!maintTypeId) {
                Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Pilih Job Maintenance Type terlebih dahulu.' });
                return;
            }
            ijmSyncJobModal();
            var $modal = ijmGetModal();
            if (!$modal.length) {
                Swal.fire({ icon: 'error', title: 'Error', text: 'Modal tidak ditemukan. Tekan Ctrl+F5 lalu coba lagi.' });
                return;
            }
            if ($modal.parent()[0] !== document.body) {
                $modal.appendTo('body');
            }
            ijmModalReset();
            $modal.modal({ backdrop: 'static', keyboard: true, show: true });
            setTimeout(function () { $modal.find('.ijm-job-search-input').focus(); }, 350);
            loadJobMaintList(maintTypeId);
        }

        function loadJobMaintList(maintTypeId, search) {
            var $m = ijmGetModal();
            var desc = getMaintTypeDesc();
            var term = (search != null) ? search : getJobSearchTerm();
            $m.find('.modal-title').html(
                '<i class="fa fa-list"></i> Pilih Job ID' +
                (desc ? ' <small class="text-muted">(' + ijmEscHtml(desc) + ' / ' + ijmEscHtml(maintTypeId) + ')</small>' : '')
            );
            $m.find('.ijm-job-list-loading').show();
            $m.find('.ijm-job-list-empty').hide();
            $m.find('.ijm-job-list-error').hide().text('');
            $m.find('.ijm-job-list-wrap').hide();
            $m.find('.ijm-job-list-body').empty();
            ijmCallMethod('GetJobMaintList', { MaintTypeID: maintTypeId, Search: term }, function (res) {
                $m.find('.ijm-job-list-loading').hide();
                if (!res.Success) {
                    $m.find('.ijm-job-list-error').text(ijmNormalizeErrorMessage(res.Message, 'Gagal memuat daftar job.')).show();
                    return;
                }
                ijmJobListCache = res.Data || [];
                renderJobMaintList(ijmJobListCache);
            }, function (msg) {
                $m.find('.ijm-job-list-loading').hide();
                $m.find('.ijm-job-list-error').text(ijmNormalizeErrorMessage(msg, 'Gagal memuat daftar job.')).show();
            });
        }

        function renderJobMaintList(rows) {
            var $m = ijmGetModal();
            var $body = $m.find('.ijm-job-list-body');
            $body.empty();
            if (!rows || !rows.length) {
                var term = getJobSearchTerm();
                $m.find('.ijm-job-list-empty')
                    .text(term ? 'Tidak ada data untuk pencarian "' + term + '".' : 'Tidak ada data job untuk tipe maintenance ini.')
                    .show();
                return;
            }
            rows.forEach(function (row, idx) {
                $body.append(
                    '<tr>' +
                    '<td>' + ijmEscHtml(row.JobID) + '</td>' +
                    '<td>' + ijmEscHtml(row.RegDate) + '</td>' +
                    '<td>' + ijmEscHtml(row.CustomerName) + '</td>' +
                    '<td>' + ijmEscHtml(row.PoID) + '</td>' +
                    '<td>' + ijmEscHtml(row.DeviceTypeDesc) + '</td>' +
                    '<td>' + ijmEscHtml(row.SchDate) + '</td>' +
                    '<td>' + ijmEscHtml(row.PoliceNo) + '</td>' +
                    '<td class="text-center"><button type="button" class="btn btn-success btn-xs btn-ijm-pick" data-idx="' + idx + '"><i class="fa fa-check"></i> Pilih</button></td>' +
                    '</tr>'
                );
            });
            $m.find('.ijm-job-list-wrap').show();
        }

        function selectJobFromList(idx) {
            var row = ijmJobListCache[idx];
            if (!row) return;
            ijmSet('txtJobID', row.JobID);
            ijmSet('hfJobID', row.JobID);
            fillJobDetail(row);
            ijmGetModal().modal('hide');
        }

        function ijmBindUiEvents() {
            $(document).off('click.ijmOpenJob', '#btnOpenJobModal').on('click.ijmOpenJob', '#btnOpenJobModal', function (e) {
                e.preventDefault();
                e.stopPropagation();
                openJobModal();
                return false;
            });
            $(document).off('click.ijmJobInput', '[id$=txtJobID]').on('click.ijmJobInput', '[id$=txtJobID]', function (e) {
                e.preventDefault();
                openJobModal();
                return false;
            });
            $(document).off('click.ijmPickJob', '.btn-ijm-pick').on('click.ijmPickJob', '.btn-ijm-pick', function (e) {
                e.preventDefault();
                selectJobFromList(parseInt($(this).data('idx'), 10));
                return false;
            });
            $(document).off('click.ijmJobSearch', '.btn-ijm-job-search').on('click.ijmJobSearch', '.btn-ijm-job-search', function (e) {
                e.preventDefault();
                triggerJobSearch();
                return false;
            });
            $(document).off('keypress.ijmJobSearch', '.ijm-job-search-input').on('keypress.ijmJobSearch', '.ijm-job-search-input', function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    triggerJobSearch();
                    return false;
                }
            });
            $(document).off('click.ijmSave', '#btnSaveClient').on('click.ijmSave', '#btnSaveClient', function () {
                if (!ijmCanSave()) {
                    updateSaveButtonState();
                    Swal.fire({ icon: 'warning', title: 'Perhatian', text: 'Lengkapi data wajib terlebih dahulu sebelum menyimpan.' });
                    return;
                }
                syncSelect2BeforePost();
                Swal.fire({
                    title: 'Konfirmasi',
                    text: 'Apakah Anda yakin ingin memproses maintenance ini?',
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonColor: '#3c8dbc',
                    cancelButtonColor: '#aaa',
                    confirmButtonText: 'Ya, Proses',
                    cancelButtonText: 'Batal',
                    width: ijmIsMobile() ? '90%' : 420
                }).then(function (result) {
                    if (result.isConfirmed) $(ijmId('CmdSave')).click();
                    else updateSaveButtonState();
                });
            });
            $(document).off('click.ijmOpenPicker', '.btn-ijm-open-picker').on('click.ijmOpenPicker', '.btn-ijm-open-picker', function (e) {
                e.preventDefault();
                openMaintPicker($(this).data('picker'));
                return false;
            });
            $(document).off('click.ijmPickInputOpen', '#txtPickNewNoSN, #txtPickNewNoGSM, #txtPickNewCustomer, #txtPickNewPoliceNo')
                .on('click.ijmPickInputOpen', '#txtPickNewNoSN, #txtPickNewNoGSM, #txtPickNewCustomer, #txtPickNewPoliceNo', function (e) {
                    e.preventDefault();
                    var map = {
                        txtPickNewNoSN: 'device',
                        txtPickNewNoGSM: 'gsm',
                        txtPickNewCustomer: 'customer',
                        txtPickNewPoliceNo: 'vehicle'
                    };
                    if (map[this.id]) openMaintPicker(map[this.id]);
                    return false;
                });
            $(document).off('click.ijmPickerPick', '.btn-ijm-picker-pick').on('click.ijmPickerPick', '.btn-ijm-picker-pick', function (e) {
                e.preventDefault();
                pickMaintItem(parseInt($(this).data('idx'), 10));
                return false;
            });
            $(document).off('click.ijmPickerSearch', '.btn-ijm-picker-search').on('click.ijmPickerSearch', '.btn-ijm-picker-search', function (e) {
                e.preventDefault();
                loadMaintPickerList($.trim(ijmGetPickerModal().find('.ijm-picker-search-input').val() || ''));
                return false;
            });
            $(document).off('keypress.ijmPickerSearch', '.ijm-picker-search-input').on('keypress.ijmPickerSearch', '.ijm-picker-search-input', function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    loadMaintPickerList($.trim($(this).val() || ''));
                    return false;
                }
            });
            $(document).off('change.ijmSaveState', '[id$=CmbJobType], [id$=CmbNewNoSN], [id$=CmbNewNoGSM], [id$=CmbNewCustomer], [id$=CmbNewServerName], [id$=CmbNewPoliceNo]')
                .on('change.ijmSaveState', '[id$=CmbJobType], [id$=CmbNewNoSN], [id$=CmbNewNoGSM], [id$=CmbNewCustomer], [id$=CmbNewServerName], [id$=CmbNewPoliceNo]', updateSaveButtonState);
            $('#cmbDeviceServerName').off('change.ijmDeviceServer').on('change.ijmDeviceServer', function () {
                loadDeviceUserAccessList();
                updateSaveButtonState();
            });
            $('#cmbDeviceUserAccess').off('change.ijmDeviceUser').on('change.ijmDeviceUser', updateSaveButtonState);
            $('#cmbCustomerServerName').off('change.ijmCustomerServer').on('change.ijmCustomerServer', updateSaveButtonState);
        }

        function ijmSyncUiAfterPostback() {
            ijmSyncJobModal();
            ijmSyncPickerModal();
            ijmSyncMaintTypeFromDropdown();
            if (!ijmVal('hfJobID')) {
                clearDetailPanels();
            }
            applyMaintVisibility();
            updateSaveButtonState();
        }

        function ijmAfterPostback() {
            ijmInitAllSelect2();
            ijmBindUiEvents();
            ijmBindMaintAttachmentEvents();
            ijmSyncUiAfterPostback();
        }

        function initNewSelect2() {
            // Legacy select2 AJAX flow is intentionally disabled.
            // All maintenance pickers now use modal + ijmCallMethod for stable routing.
        }

        function fillJobDetail(d) {
            if (!d) return;
            var pk = resolveProcessKey(d.MaintTypeID || getMaintTypeId(), d.MaintTypeDesc || '');
            ijmSet('hfProcessKey', pk);
            ijmText('RegDate', d.RegDate); ijmText('SchDate', d.SchDate);
            ijmSet('txtMaintTypeDesc', d.MaintTypeDesc);
            ijmText('TechnicianID', d.TechnicianID);
            ijmText('TechnicianName', d.TechnicianName);
            ijmText('TechBranchName', d.TechBranchName);
            ijmSet('hfTechnicianID', d.TechnicianID);
            ijmText('CustomerName', d.CustomerName); ijmText('BranchName', d.BranchName);
            ijmText('MarketingName', d.MarketingName); ijmText('PoliceNo', d.PoliceNo);
            ijmText('VehicleDesc', d.VehicleDesc); ijmText('AssetNo', d.AssetNo);
            ijmText('NoSNInfo', d.NoSN); ijmText('DeviceTypeDesc', d.DeviceTypeDesc);
            ijmText('WarehouseName', d.WarehouseName); ijmText('NoGSMInfo', d.NoGSM);
            ijmText('ProviderName', d.ProviderName); ijmText('GsmWarehouseName', d.GsmWarehouseName);
            ijmText('ServerNameInfo', d.ServerName);
            ijmSet('hfTvdID', d.TvdID); ijmSet('hfTvaID', d.TvaID); ijmSet('hfTdtID', d.TdtID);
            ijmSet('hfTgtID', d.TgtID); ijmSet('hfDeviceID', d.DeviceID); ijmSet('hfGsmID', d.GsmID);
            ijmSet('hfVehicleID', d.VehicleID); ijmSet('hfCustID', d.CustID); ijmSet('hfJobCustID', d.CustID);
            ijmSet('hfServerID', d.ServerID || '');
            if (getProcessKey() === 'server_maint') loadServerDropdown();
            applyMaintVisibility();
            updateSaveButtonState();
        }

        function fillDropdown(name, items, triggerChange) {
            var $c = $(ijmId(name));
            $c.empty().append('<option value="">[Select]</option>');
            (items || []).forEach(function (it) {
                if (it.Value) $c.append($('<option></option>').val(it.Value).text(it.Text || it.Value));
            });
            if ($c.hasClass('select2-hidden-accessible')) $c.trigger('change.select2');
            if (triggerChange) $c.trigger('change');
        }

        function onNewItemChanged(type, val) {
            var extra = ijmNewExtraMap[type + '_' + val];
            if (!extra) return;
            ijmCallMethod('LoadNewItemDetail', { type: type, jsonExtra: extra }, function (res) {
                if (!res.Success || !res.Data) return;
                var d = res.Data;
                if (type === 'device') {
                    ijmSet('txtNewDeviceTypeDesc', d.DeviceTypeDesc);
                    ijmSet('txtNewWarehouseName', d.WarehouseName);
                    ijmSet('hfNewTdtID', d.TdtID); ijmSet('hfNewDeviceID', d.DeviceID);
                    loadDeviceUserAccessList();
                } else if (type === 'gsm') {
                    ijmSet('txtNewProviderName', d.ProviderName);
                    ijmSet('txtNewGsmWarehouse', d.WarehouseName);
                    ijmSet('hfNewTgtID', d.TgtID); ijmSet('hfNewGsmID', d.GsmID);
                } else if (type === 'customer') {
                    ijmSet('txtNewBranchName', d.BranchName);
                    ijmSet('txtNewMarketingName', d.MarketingName); ijmSet('hfNewCustID', d.CustID);
                    loadCustomerServerList();
                } else if (type === 'vehicle') {
                    ijmSet('txtNewVehicleDesc', d.VehicleDesc);
                    ijmSet('txtNewAssetNo', d.AssetNo); ijmSet('hfNewTvaID', d.TvaID); ijmSet('hfNewVehicleID', d.VehicleID);
                }
            }, function (msg) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: ijmNormalizeErrorMessage(msg, 'Gagal memuat detail data terpilih.') });
            });
        }

        function loadServerDropdown() {
            ijmCallMethod('GetServerList', { custId: ijmVal('hfCustID'), categoryKey: getMaintTypeId(), processKey: getProcessKey() }, function (res) {
                if (!res.Success) return;
                fillDropdown('CmbNewServerName', res.Data, false);
                reinitSingleSelect2('CmbNewServerName', { placeholder: '[Select]' });
            }, function (msg) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: ijmNormalizeErrorMessage(msg, 'Gagal memuat server.') });
            });
        }

        function resetDeviceServerList() {
            $('#cmbDeviceServerName').empty().append('<option value="">[Select]</option>');
        }

        function resetDeviceUserAccessList() {
            $('#cmbDeviceUserAccess').empty().append('<option value="">[Select]</option>');
        }

        function loadDeviceServerList() {
            resetDeviceServerList();
            resetDeviceUserAccessList();
            if (getProcessKey() !== 'device_maint') return;
            var custId = $.trim(ijmVal('hfCustID') || '');
            if (!custId) return;
            ijmCallMethod('GetServerList', { custId: custId, categoryKey: getMaintTypeId(), processKey: getProcessKey() }, function (res) {
                if (!res || !res.Success) return;
                var rows = res.Data || [];
                var $s = $('#cmbDeviceServerName');
                rows.forEach(function (it) {
                    if (it && it.Value) $s.append($('<option></option>').val(it.Value).text(it.Text || it.Value));
                });
                updateSaveButtonState();
            }, function (err) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: ijmNormalizeErrorMessage(err, 'Gagal memuat server name.') });
            });
        }

        function loadDeviceUserAccessList() {
            resetDeviceUserAccessList();
            if (getProcessKey() !== 'device_maint') return;
            var custId = $.trim(ijmVal('hfCustID') || '');
            var serverId = $.trim($('#cmbDeviceServerName').val() || '');
            if (!custId || !serverId) return;
            ijmCallMethod('GetInterfacingUserLogin', { custId: custId, serverId: serverId }, function (res) {
                if (!res || !res.Success) return;
                var rows = res.Data || [];
                var $s = $('#cmbDeviceUserAccess');
                rows.forEach(function (row) {
                    var autoId = row.autoid || row.AutoID || '';
                    var userId = row.user_id || row.UserID || '';
                    var userNm = row.user_nm || row.UserNm || row.UserName || '';
                    if (!autoId) return;
                    var txt = userId || autoId;
                    if (userNm) txt += ' - ' + userNm;
                    $s.append($('<option></option>').val(autoId).text(txt));
                });
                updateSaveButtonState();
            }, function (err) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: ijmNormalizeErrorMessage(err, 'Gagal memuat user access.') });
            });
        }

        function resetCustomerServerList() {
            $('#cmbCustomerServerName').empty().append('<option value="">[Select]</option>');
        }

        function loadCustomerServerList() {
            resetCustomerServerList();
            if (getProcessKey() !== 'customer_maint') return;
            var custId = $.trim(ijmVal('hfNewCustID') || '');
            if (!custId) return;
            ijmCallMethod('GetServerList', { custId: custId, categoryKey: getMaintTypeId(), processKey: 'customer_maint' }, function (res) {
                if (!res || !res.Success) return;
                var rows = res.Data || [];
                var $s = $('#cmbCustomerServerName');
                rows.forEach(function (it) {
                    if (it && it.Value) $s.append($('<option></option>').val(it.Value).text(it.Text || it.Value));
                });
                updateSaveButtonState();
            }, function (err) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: ijmNormalizeErrorMessage(err, 'Gagal memuat server customer.') });
            });
        }

        function applyMaintVisibility() {
            var processKey = getProcessKey();
            var panels = {
                'device_maint': ['PnlMaintDevice'],
                'gsm_maint': ['PnlMaintGsm'],
                'gsm_maint_suspend': ['PnlMaintGsm'],
                'customer_maint': ['PnlMaintCustomer'],
                'server_maint': ['PnlMaintServer'],
                'vehicle_maint': ['PnlMaintVehicle']
            };
            ['PnlMaintDevice', 'PnlMaintGsm', 'PnlMaintCustomer', 'PnlMaintServer', 'PnlMaintVehicle'].forEach(function (p) {
                $(ijmId(p)).hide();
            });
            (panels[processKey] || []).forEach(function (p) { $(ijmId(p)).show(); });
            if (processKey === 'device_maint') loadDeviceServerList();
            else {
                resetDeviceServerList();
                resetDeviceUserAccessList();
            }
            if (processKey === 'customer_maint') loadCustomerServerList();
            else resetCustomerServerList();
        }

        function ensureSelectOption(selId, val, text) {
            if (!val) return;
            var $s = $(ijmId(selId));
            var esc = val.replace(/"/g, '\\"');
            if (!$s.find('option[value="' + esc + '"]').length)
                $s.append($('<option></option>').val(val).text(text || val).prop('selected', true));
            else $s.val(val);
        }

        function syncSelect2BeforePost() {
            ['CmbNewNoSN', 'CmbNewNoGSM', 'CmbNewCustomer', 'CmbNewPoliceNo', 'CmbNewServerName'].forEach(function (id) {
                var v = ijmVal(id);
                var t = $(ijmId(id)).find('option:selected').text();
                ensureSelectOption(id, v, t);
            });
        }

        function ijmHasSelectedValue(selId) {
            var v = $.trim(ijmVal(selId) || '');
            return v !== '' && v !== '[Select]';
        }

        function ijmCanSave() {
            if (!ijmHasSelectedValue('CmbJobType')) return false;
            if (!$.trim(ijmVal('hfJobID') || '')) return false;
            if (!$.trim(ijmVal('hfTechnicianID') || '')) return false;
            if (!$.trim(ijmVal('hfTvdID') || '')) return false;

            var processKey = getProcessKey();
            if (processKey === 'device_maint') {
                return ijmHasSelectedValue('CmbNewNoSN')
                    && $.trim($('#cmbDeviceServerName').val() || '') !== ''
                    && $.trim($('#cmbDeviceUserAccess').val() || '') !== '';
            }
            if (processKey === 'gsm_maint' || processKey === 'gsm_maint_suspend') return ijmHasSelectedValue('CmbNewNoGSM');
            if (processKey === 'customer_maint') {
                return ijmHasSelectedValue('CmbNewCustomer')
                    && $.trim($('#cmbCustomerServerName').val() || '') !== '';
            }
            if (processKey === 'server_maint') return ijmHasSelectedValue('CmbNewServerName');
            if (processKey === 'vehicle_maint') return ijmHasSelectedValue('CmbNewPoliceNo');

            return true;
        }

        function updateSaveButtonState() {
            $('#btnSaveClient').prop('disabled', !ijmCanSave());
        }

        function clearDetailPanels() {
            ['RegDate', 'SchDate', 'TechnicianID', 'TechnicianName', 'TechBranchName', 'CustomerName', 'BranchName', 'MarketingName', 'PoliceNo', 'VehicleDesc', 'AssetNo',
                'NoSNInfo', 'DeviceTypeDesc', 'WarehouseName', 'NoGSMInfo', 'ProviderName', 'GsmWarehouseName', 'ServerNameInfo']
                .forEach(function (f) { ijmText(f, ''); });
            ['hfTvdID', 'hfTvaID', 'hfTdtID', 'hfTgtID', 'hfDeviceID', 'hfGsmID', 'hfVehicleID', 'hfCustID', 'hfTechnicianID', 'hfJobID'].forEach(function (f) { ijmSet(f, ''); });
            ijmSet('hfNewCustID', '');
            ijmSet('hfServerID', '');
            ijmSet('txtJobID', '');
            $('#txtPickNewNoSN, #txtPickNewNoGSM, #txtPickNewCustomer, #txtPickNewPoliceNo').val('');
            resetDeviceServerList();
            resetDeviceUserAccessList();
            resetCustomerServerList();
            updateSaveButtonState();
        }

        $(document).ready(function () {
            ijmSyncJobModal();
            ijmSyncPickerModal();
            ijmAfterPostback();
            ijmRenderMaintAttachPreview([]);
            updateSaveButtonState();
            setTimeout(function () { ijmInitAllSelect2(); ijmSyncJobModal(); }, 80);

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                ijmAfterPostback();
                updateSaveButtonState();
                var msg = $(ijmId('div_comment')).html();
                var valMsg = $(ijmId('div_validation')).html();
                if (msg && msg.trim() !== '') {
                    if (msg.indexOf('alert-success') >= 0) Swal.fire({ icon: 'success', title: 'Berhasil', html: msg });
                    else Swal.fire({ icon: 'error', title: 'Gagal', html: msg });
                } else if (valMsg && valMsg.trim() !== '') {
                    Swal.fire({ icon: 'warning', title: 'Validasi', html: valMsg });
                }
            });
        });
    </script>
</asp:Content>
