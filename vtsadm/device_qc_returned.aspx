<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_qc_returned.aspx.cs" Inherits="vtsadm.device_qc_returned" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device Quality Control - Returned Device       
                <small>Batch QC Processing</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Quality Control</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">QC Returned Device</li>
        </ol>
    </section>

    <section class="content">
        <!-- SEARCH / FILTER - MULTIPLE BARCODE SCAN WITH TAGS -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-barcode"></i> Scan Barcode - Multiple Devices
                        </h3>
                    </div>
                    <div class="box-body">
                        <!-- Warehouse Filter Section -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-solid" style="margin-bottom: 15px;">
                                    <div class="box-header with-border" style="background-color: #17a2b8; color: white;">
                                        <h3 class="box-title">
                                            <i class="fa fa-building"></i> Warehouse Information
                                        </h3>
                                        <small style="color: #fff; margin-left: 10px;">(Required for QC submission)</small>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group form-group-sm">
                                                    <label>Warehouse ID <span class="text-red">*</span></label>
                                                    <div class="input-group input-group-sm">
                                                        <input type="text" id="txtWarehouseID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" style="background-color: #fff3cd; border: 2px solid #ffc107;" />
                                                        <span class="input-group-btn">
                                                            <button id="btnWarehouseSearch" runat="server" type="button" class="btn btn-block btn-warning btn-xs" data-toggle="modal" data-target="#modal-warehouse-filter">
                                                                <i class="fa fa-search"></i> Select
                                                            </button>
                                                        </span>
                                                    </div>
                                                    <small class="text-danger">
                                                        <i class="fa fa-exclamation-triangle"></i> 
                                                        <strong>Required!</strong> Please select warehouse first
                                                    </small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Scan Barcode Section -->
                        <div class="row">
                            <div class="col-md-12">
                                <label>Scanned Devices (Serial Number) <span class="badge bg-blue" id="lblScanCount">0</span></label>
                                <div class="tags-input-container" id="tagsContainer" onclick="focusScanInput();">
                                    <div class="tags-wrapper" id="tagsWrapper">
                                        <!-- Tags akan muncul di sini -->
                                    </div>
                                    <input type="text" id="txtScanInput" class="tag-input-field" 
                                           placeholder="scan serial number (NoSN) here..." 
                                           onkeypress="return handleTagInput(event);"
                                           autocomplete="off">
                                </div>
                                <asp:HiddenField ID="hdnScannedDevices" runat="server" />
                                <small class="text-muted">
                                    <i class="fa fa-info-circle"></i> 
                                    Scan Serial Number (NoSN) and press Enter to add. Click × to remove. Double-click tag to remove.
                                </small>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-md-12 text-center">
                                <button type="button" class="btn btn-warning" onclick="clearAllTags();" style="border-radius:0; margin-right:10px;">
                                    <i class="fa fa-trash"></i> Clear All
                                </button>
                                <asp:Button ID="btnSearchMultiple" runat="server" 
                                    CssClass="btn-modern btn-search-multiple" 
                                    Text="🔍 Search & Auto-Check All Devices" 
                                    OnClientClick="return validateAndSearchMultiple();" 
                                    OnClick="btnSearchMultiple_Click" />
                                
                                <div id="searchLoader" style="display:none; margin-top:10px;">
                                    <i class="fa fa-spinner fa-spin"></i> 
                                    <span class="text-info">Searching <span id="searchProgress">0</span> devices...</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- PENDING QC (dengan checkbox) -->
            <div class="col-md-12">
                <div class="box box-warning">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-clock-o"></i> Pending QC - Device Belum di-QC
                        </h3>
                        <div class="box-tools" style="display:none;">
                            <span class="badge bg-yellow">
                                <asp:Label ID="LblTotalPending" runat="server" Text="0"></asp:Label> devices
                            </span>
                        </div>
                    </div>
                    <div class="box-body no-padding">
                        <asp:UpdatePanel ID="UpdatePanelPending" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ScrollBars="Auto" style="max-height: 500px;">
                                    <asp:GridView ID="GridViewPending" runat="server" 
                                CssClass="table table-hover table-striped table-bordered table-condensed" 
                                AutoGenerateColumns="False" 
                                AllowPaging="False"
                                AllowSorting="True"
                                OnPageIndexChanging="GridViewPending_PageIndexChanging" 
                                OnSorting="GridViewPending_Sorting"
                                OnRowDataBound="GridViewPending_RowDataBound"
                                EmptyDataText="No devices pending QC"
                                GridLines="None"
                                Font-Size="Small"
                                CellPadding="4"
                                Width="100%"
                                EnableViewState="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="gv-select-col" HeaderStyle-CssClass="gv-select-col">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" ToolTip="Select All" onclick="toggleSelectAll(this);" CssClass="chk-device" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="chk-device" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" 
                                        SortExpression="DeviceID" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="NoSN" HeaderText="Serial No" 
                                        SortExpression="NoSN" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor" 
                                        SortExpression="VendorName" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Type" 
                                        SortExpression="DeviceTypeDesc" ItemStyle-Wrap="false" />

                                    <asp:TemplateField HeaderText="Status Channel" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Literal ID="litChannelStatus" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse" 
                                        SortExpression="WarehouseName" ItemStyle-Wrap="false"/>

                                    <asp:BoundField DataField="TechnicianName" HeaderText="Technician" 
                                        SortExpression="TechnicianName" ItemStyle-Wrap="false"/>
                                    
                                    <asp:BoundField DataField="TdwID" HeaderText="TdwID" ItemStyle-Wrap="false"></asp:BoundField>
                                    <asp:BoundField DataField="WarehouseID" HeaderText="WarehouseID" ItemStyle-Wrap="false"></asp:BoundField>
                                    <asp:BoundField DataField="SourceName" HeaderText="Source" ItemStyle-Wrap="false"></asp:BoundField>
                                </Columns>
                                <RowStyle ForeColor="#003481" BackColor="White" />
                                <SelectedRowStyle BackColor="#FFF3CD" Font-Bold="True" />
                                <PagerStyle Wrap="true" CssClass="pagination-ys" HorizontalAlign="Center" BorderColor="White" />
                                <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                <HeaderStyle Height="30px" BackColor="#FFC107" ForeColor="White" Font-Bold="true" />
                                <AlternatingRowStyle BackColor="#FFFAEB" />
                            </asp:GridView>
                                </asp:Panel>
                                <div class="box-footer">
                                    <asp:Label ID="LblPagingPending" runat="server" CssClass="text-muted" Style="font-style: italic; font-size: 12px;"></asp:Label>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- QC FORM & BUTTONS -->
            <div class="col-md-12">
                <div class="box box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-check-square-o"></i> QC Result for Selected Devices
                        </h3>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelQCForm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="box-body">
                        <div class="row">
                            <!-- QC RESULT -->
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>QC Result <span class="text-red">*</span></label>
                                    <asp:DropDownList ID="CmbQCResult" runat="server" class="form-control">
                                        <asp:ListItem Value="">[Select Result]</asp:ListItem>
                                        <asp:ListItem Value="2">✓ Pass (Lolos QC)</asp:ListItem>
                                        <asp:ListItem Value="1">✗ Return (Rusak)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <!-- QC NOTES -->
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label>QC Notes/Remarks <span class="text-muted">(Optional)</span></label>
                                    <asp:TextBox ID="txtQCNotes" runat="server" class="form-control" 
                                        TextMode="MultiLine" Rows="2" 
                                        placeholder="Jelaskan hasil pemeriksaan (optional)..."></asp:TextBox>
                                    <small class="text-muted">Notes are optional</small>
                                </div>
                            </div>
                        </div>
                    </div>

                            <!-- BUTTONS -->
                            <div class="box-footer text-center" style="padding: 20px;">
                                <asp:Button ID="CmdCancel" runat="server" CssClass="btn-modern btn-cancel" 
                                    Text="✕ Cancel" OnClick="CmdCancel_Click" />
                                <asp:Button ID="CmdSubmit" runat="server" CssClass="btn-modern btn-submit" 
                                    Text="✓ Submit QC Result" OnClientClick="return showSubmitConfirmation();" 
                                    OnClick="CmdSubmit_Click" />
                                <br />
                                <small class="text-muted" style="display: block; margin-top: 12px;">
                                    <i class="fa fa-info-circle"></i> 
                                    QC result will be applied to all selected devices
                                </small>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <!-- Modal Warehouse Filter -->
        <div class="modal fade bs-example-modal-lg" id="modal-warehouse-filter">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Select Warehouse</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="device_qc_returned_warehouse_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Message Box -->
        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="div_comment" runat="server"></div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Confirmation for Submit -->
        <div class="modal fade" id="modal-submit-confirmation" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modern-modal">
                    <div class="modal-header modal-header-warning">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">
                            <i class="fa fa-exclamation-triangle"></i> Konfirmasi QC
                        </h4>
                    </div>
                    <div class="modal-body text-center" style="padding: 30px;">
                        <div class="confirmation-icon">
                            <i class="fa fa-question-circle"></i>
                        </div>
                        <h3 id="confirmTitle" style="margin-top: 20px; font-weight: 600;">Anda yakin ingin submit?</h3>
                        <div id="confirmDetails" style="margin: 20px 0; padding: 20px; background: #f8f9fa; border-radius: 8px;">
                            <p style="margin: 5px 0;"><strong>Device yang dipilih:</strong> <span id="confirmDeviceCount" class="text-primary"></span></p>
                            <p style="margin: 5px 0;"><strong>QC Result:</strong> <span id="confirmQCResult" class="text-info"></span></p>
                            <p style="margin: 5px 0;"><strong>Notes:</strong> <span id="confirmNotes" class="text-muted"></span></p>
                        </div>
                        <p class="text-warning" style="font-size: 13px;">
                            <i class="fa fa-info-circle"></i> Data yang sudah disubmit tidak dapat dibatalkan
                        </p>
                    </div>
                    <div class="modal-footer" style="text-align: center; border-top: none; padding: 0 30px 30px;">
                        <button type="button" class="btn btn-default btn-lg" data-dismiss="modal" style="min-width: 120px;">
                            <i class="fa fa-times"></i> Batal
                        </button>
                        <button type="button" class="btn btn-success btn-lg" onclick="confirmSubmit();" style="min-width: 120px;">
                            <i class="fa fa-check"></i> Ya, Submit!
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal Setting Channel MDVR -->
        <div class="modal fade" id="modal-channel-setting" tabindex="-1" role="dialog">
            <div class="modal-dialog ch-modal" role="document">
                <div class="modal-content ch-modal-content">
                    <div class="modal-header ch-modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-sliders"></i> Setting Channel MDVR</h4>
                    </div>
                    <div class="modal-body">
                        <div class="ch-info-grid">
                            <div class="ch-info-item">
                                <span class="ch-info-label"><i class="fa fa-barcode"></i> NoSN</span>
                                <span id="chNoSN" class="ch-info-value ch-info-sn">-</span>
                            </div>
                            <div class="ch-info-item">
                                <span class="ch-info-label"><i class="fa fa-microchip"></i> Device Type</span>
                                <span id="chDeviceType" class="ch-info-value">-</span>
                            </div>
                            <div class="ch-info-item">
                                <span class="ch-info-label"><i class="fa fa-list-ol"></i> Max Channel</span>
                                <span id="chMaxChannel" class="ch-info-value">-</span>
                            </div>
                        </div>
                        <div id="channelSettingAlert"></div>
                        <div id="channelSettingLoading" class="ch-loading" style="display:none;">
                            <div class="ch-loading-head"><i class="fa fa-circle-o-notch fa-spin"></i> Memuat channel...</div>
                            <div id="channelSkeleton" class="ch-skeleton-wrap"></div>
                        </div>
                        <div class="table-responsive">
                            <table class="table ch-channel-table" id="channelSettingTable" style="display:none;">
                                <thead><tr><th style="width:55%;">Channel</th><th>Tipe Channel</th></tr></thead>
                                <tbody id="channelSettingRows"></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">
                            <i class="fa fa-times"></i> Close
                        </button>
                        <button type="button" class="btn btn-success ch-btn-save" id="btnSaveChannelSetting" onclick="saveChannelSetting();">
                            <i class="fa fa-save"></i> Save Setting
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <style>
            .ch-modal .ch-modal-content {
                border: none;
                border-radius: 10px;
                overflow: hidden;
                box-shadow: 0 18px 50px rgba(0, 0, 0, 0.28);
            }
            .ch-modal-header {
                background: linear-gradient(135deg, #3c8dbc 0%, #2f6f96 100%);
                color: #fff;
                border: none;
                padding: 16px 20px;
            }
            .ch-modal-header .modal-title { font-weight: 600; letter-spacing: .3px; }
            .ch-modal-header .close { color: #fff; opacity: .85; text-shadow: none; font-size: 24px; }
            .ch-modal-header .close:hover { opacity: 1; }

            .ch-info-grid {
                display: flex;
                flex-wrap: wrap;
                gap: 10px;
                margin-bottom: 14px;
            }
            .ch-info-item {
                flex: 1 1 30%;
                min-width: 120px;
                background: #f5f8fb;
                border: 1px solid #e3ecf3;
                border-radius: 8px;
                padding: 8px 12px;
            }
            .ch-info-label {
                display: block;
                font-size: 11px;
                text-transform: uppercase;
                letter-spacing: .5px;
                color: #8a99a8;
                font-weight: 600;
                margin-bottom: 2px;
            }
            .ch-info-value { display: block; font-size: 14px; font-weight: 600; color: #2b3d4f; }
            .ch-info-sn { word-break: break-all; color: #3c8dbc; }

            /* Modern skeleton loader */
            .ch-loading { padding: 4px 0 2px; }
            .ch-loading-head {
                color: #3c8dbc; font-weight: 600; font-size: 12px;
                letter-spacing: .3px; margin-bottom: 10px; text-align: center;
            }
            .ch-skel-row {
                display: flex; align-items: center; gap: 12px;
                padding: 11px 4px; border-bottom: 1px solid #f0f3f6;
            }
            .ch-skel-bar {
                border-radius: 6px;
                background: linear-gradient(90deg, #eef2f6 25%, #dfe6ed 37%, #eef2f6 63%);
                background-size: 400% 100%;
                animation: ch-shimmer 1.25s ease infinite;
            }
            .ch-skel-label { flex: 0 0 45%; height: 14px; }
            .ch-skel-input { flex: 1 1 auto; height: 32px; }
            @keyframes ch-shimmer { 0% { background-position: 100% 0; } 100% { background-position: -100% 0; } }

            .ch-channel-table { margin-bottom: 0; }
            .ch-channel-table > thead > tr > th {
                background: #f0f4f8;
                border-bottom: 2px solid #dde6ee;
                color: #5a6b7b;
                font-size: 12px;
                text-transform: uppercase;
                letter-spacing: .4px;
            }
            .ch-channel-table > tbody > tr > td { vertical-align: middle; }
            .ch-channel-table .channel-type-select { border-radius: 6px; }

            .ch-btn-save { border-radius: 6px; min-width: 140px; }
            .ch-btn-save:disabled { opacity: .75; cursor: progress; }

            /* Modern inline alert dalam modal */
            #channelSettingAlert .ch-alert {
                display: flex;
                align-items: center;
                gap: 8px;
                border: none;
                border-radius: 8px;
                padding: 10px 14px;
                margin-bottom: 12px;
                font-size: 13px;
                animation: ch-fadein .25s ease;
            }
            @keyframes ch-fadein { from { opacity: 0; transform: translateY(-4px); } to { opacity: 1; transform: none; } }
            #channelSettingAlert .ch-alert i { font-size: 16px; }
            #channelSettingAlert .ch-alert-danger { background: #fdecea; color: #c0392b; }
            #channelSettingAlert .ch-alert-success { background: #e8f6ee; color: #1e7e46; }
            #channelSettingAlert .ch-alert-warning { background: #fff5e6; color: #b9770e; }
        </style>
    </section>

    <style>
        .table-condensed > tbody > tr > td,
        .table-condensed > thead > tr > th {
            padding: 5px 8px;
            font-size: 12px;
        }
        
        /* Box Headers - NO ROUNDED */
        .box-warning .box-header {
            background-color: #f39c12;
            color: white;
            border-radius: 0 !important;
        }
        
        .box-info .box-header {
            background-color: #17a2b8;
            color: white;
            border-radius: 0 !important;
        }
        
        .box-success .box-header {
            background-color: #28a745;
            color: white;
            border-radius: 0 !important;
        }

        .box-solid .box-header {
            border-radius: 0 !important;
        }

        .box {
            border-radius: 0 !important;
        }
        
        .chk-device {
            transform: scale(1.2);
            cursor: pointer;
            vertical-align: middle;
            margin: 0 auto;
        }

        /* Ensure checkbox column centered and equal width */
        .gv-select-col { text-align: center !important; width: 50px; }

        /* Modern Search Input - NO ROUNDED */
        .search-input {
            border-radius: 0 !important;
            border: 2px solid #3498db;
            padding: 8px 12px;
            transition: all 0.3s ease;
        }

        .search-input:focus {
            border-color: #2980b9;
            box-shadow: 0 0 0 3px rgba(52, 152, 219, 0.1);
            outline: none;
        }

        /* Modern Buttons - NO ROUNDED, GRADIENT STYLE */
        .btn-modern {
            border: none;
            border-radius: 0 !important;
            padding: 12px 30px;
            font-weight: 600;
            font-size: 14px;
            letter-spacing: 0.5px;
            text-transform: uppercase;
            transition: all 0.3s ease;
            cursor: pointer;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            margin: 0 8px;
        }

        .btn-modern:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0,0,0,0.15);
        }

        .btn-modern:active {
            transform: translateY(0);
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        /* Submit Button - Green Gradient */
        .btn-submit {
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
            color: white;
            min-width: 200px;
        }

        .btn-submit:hover {
            background: linear-gradient(135deg, #218838 0%, #1aa179 100%);
            color: white;
        }

        /* Cancel Button - Gray Gradient */
        .btn-cancel {
            background: linear-gradient(135deg, #6c757d 0%, #5a6268 100%);
            color: white;
            min-width: 120px;
        }

        .btn-cancel:hover {
            background: linear-gradient(135deg, #5a6268 0%, #545b62 100%);
            color: white;
        }

        /* Search Button - Blue Gradient */
        .btn-search {
            background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
            color: white;
            border-radius: 0 !important;
            padding: 8px 20px;
            font-weight: 600;
        }

        .btn-search:hover {
            background: linear-gradient(135deg, #2980b9 0%, #21618c 100%);
            color: white;
            transform: none;
        }

        /* GridView Headers - NO ROUNDED */
        .table-bordered,
        .table-bordered > thead > tr > th,
        .table-bordered > tbody > tr > td {
            border-radius: 0 !important;
        }

        /* Input Groups - NO ROUNDED */
        .input-group .form-control:first-child,
        .input-group-btn:last-child > .btn {
            border-radius: 0 !important;
        }

        /* Search Loader Animation */
        #searchLoader {
            animation: fadeIn 0.3s ease-in;
        }

        @keyframes fadeIn {
            from { opacity: 0; }
            to { opacity: 1; }
        }

        #searchLoader .fa-spinner {
            color: #3498db;
            font-size: 16px;
            margin-right: 5px;
        }

        /* Barcode Feedback Styling */
        #barcodeFeedback {
            animation: slideInRight 0.3s ease-out;
        }

        @keyframes slideInRight {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }

        /* Highlight effect for checked row */
        .row-highlight {
            animation: pulseGreen 0.5s ease-in-out;
        }

        @keyframes pulseGreen {
            0%, 100% { background-color: transparent; }
            50% { background-color: #d4edda; }
        }

        /* Modern Modal Confirmation */
        .modern-modal {
            border: none;
            border-radius: 12px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
        }

        .modal-header-warning {
            background: linear-gradient(135deg, #f39c12 0%, #f8b739 100%);
            color: white;
            border-radius: 12px 12px 0 0;
            border: none;
            padding: 20px 30px;
        }

        .modal-header-warning .modal-title {
            font-size: 20px;
            font-weight: 600;
        }

        .modal-header-warning .close {
            color: white;
            opacity: 0.8;
            text-shadow: none;
            font-size: 28px;
        }

        .modal-header-warning .close:hover {
            opacity: 1;
        }

        .confirmation-icon {
            width: 80px;
            height: 80px;
            margin: 0 auto;
            background: linear-gradient(135deg, #ffc107 0%, #ff9800 100%);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            animation: iconPulse 2s infinite;
        }

        .confirmation-icon i {
            font-size: 45px;
            color: white;
        }

        @keyframes iconPulse {
            0%, 100% {
                transform: scale(1);
                box-shadow: 0 0 0 0 rgba(255, 193, 7, 0.7);
            }
            50% {
                transform: scale(1.05);
                box-shadow: 0 0 0 10px rgba(255, 193, 7, 0);
            }
        }

        .modal-dialog-centered {
            display: flex;
            align-items: center;
            min-height: calc(100% - 1rem);
        }

        #confirmDetails p {
            font-size: 15px;
            line-height: 1.8;
        }

        #confirmDetails strong {
            display: inline-block;
            min-width: 150px;
            text-align: left;
        }

        .modal-backdrop.in {
            opacity: 0.7;
        }

        /* ===== TAGS INPUT STYLING ===== */
        .tags-input-container {
            min-height: 120px;
            padding: 10px;
            border: 2px solid #00a65a;
            border-radius: 0;
            background-color: #f9f9f9;
            cursor: text;
            display: flex;
            flex-wrap: wrap;
            align-items: flex-start;
            gap: 8px;
        }

        .tags-input-container:focus-within {
            border-color: #008d4c;
            background-color: #fff;
            box-shadow: 0 0 0 3px rgba(0, 166, 90, 0.1);
        }

        .tags-wrapper {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            align-items: center;
        }

        .tag-item {
            display: inline-flex;
            align-items: center;
            background: linear-gradient(135deg, #00a65a 0%, #00c46a 100%);
            color: white;
            padding: 6px 12px;
            border-radius: 4px;
            font-size: 13px;
            font-weight: 600;
            font-family: 'Courier New', monospace;
            letter-spacing: 0.5px;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            animation: tagSlideIn 0.3s ease-out;
        }

        .tag-item:hover {
            background: linear-gradient(135deg, #008d4c 0%, #00a65a 100%);
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.15);
        }

        .tag-item .tag-text {
            margin-right: 8px;
        }

        .tag-item .tag-remove {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 18px;
            height: 18px;
            background-color: rgba(255,255,255,0.3);
            border-radius: 50%;
            cursor: pointer;
            transition: all 0.2s ease;
            font-size: 14px;
            line-height: 1;
        }

        .tag-item .tag-remove:hover {
            background-color: #dc3545;
            transform: scale(1.2);
        }

        .tag-input-field {
            border: none;
            outline: none;
            background: transparent;
            font-size: 13px;
            padding: 6px 8px;
            flex: 1;
            min-width: 200px;
            font-family: 'Courier New', monospace;
            color: #333;
        }

        .tag-input-field::placeholder {
            color: #999;
            font-style: italic;
        }

        .tag-input-field:disabled {
            background-color: #f5f5f5;
            cursor: not-allowed;
            color: #999;
        }

        .tag-input-field:disabled::placeholder {
            color: #dc3545;
            font-weight: 600;
        }

        @keyframes tagSlideIn {
            from {
                opacity: 0;
                transform: translateX(-20px) scale(0.8);
            }
            to {
                opacity: 1;
                transform: translateX(0) scale(1);
            }
        }

        /* Badge Counter */
        .badge.bg-blue {
            background-color: #3498db;
            font-size: 14px;
            padding: 4px 10px;
            border-radius: 3px;
            font-weight: 700;
        }

        /* Search Multiple Button */
        .btn-search-multiple {
            background: linear-gradient(135deg, #00a65a 0%, #008d4c 100%);
            color: white;
            border: none;
            border-radius: 0 !important;
            padding: 15px 40px;
            font-weight: 700;
            font-size: 16px;
            letter-spacing: 0.5px;
            text-transform: uppercase;
            transition: all 0.3s ease;
            cursor: pointer;
            box-shadow: 0 4px 6px rgba(0,0,0,0.2);
            min-width: 350px;
        }

        .btn-search-multiple:hover {
            background: linear-gradient(135deg, #008d4c 0%, #006837 100%);
            transform: translateY(-2px);
            box-shadow: 0 6px 12px rgba(0,0,0,0.3);
            color: white;
        }

        .btn-search-multiple:active {
            transform: translateY(0);
            box-shadow: 0 2px 4px rgba(0,0,0,0.2);
        }
    </style>

    <script type="text/javascript">
        // Toggle Select All Checkbox
        function toggleSelectAll(source) {
            var checkboxes = document.querySelectorAll('input[id*="chkSelect"]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }

        // Show Loader
        function showLoader() {
            document.getElementById('searchLoader').style.display = 'block';
        }

        // Hide Loader
        function hideLoader() {
            document.getElementById('searchLoader').style.display = 'none';
        }

        // ===== TAGS INPUT FUNCTIONS =====
        
        // Handle tag input (Enter key to add)
        function handleTagInput(event) {
            if (event.keyCode === 13 || event.which === 13) {
                event.preventDefault();
                addTag();
                return false;
            }
            return true;
        }

        // Add tag from input (supports multiple SN separated by space)
        function addTag() {
            // Check warehouse selected first
            var warehouseID = document.getElementById('ContentPlaceHolder1_txtWarehouseID').value.trim();
            if (warehouseID === '' || warehouseID === 'Please select ...') {
                alert('⚠ Warehouse ID is required!\n\nPlease select a warehouse first before scanning devices.');
                $('#modal-warehouse-filter').modal('show');
                return;
            }
            
            var input = document.getElementById('txtScanInput');
            var inputValue = input.value.trim().toUpperCase();
            
            if (inputValue === '') {
                showBarcodeFeedback('warning', '⚠ Please scan or enter a Serial Number');
                return;
            }
            
            // Split by space to handle multiple serial numbers
            var serialNumbers = inputValue.split(/\s+/).filter(function(sn) {
                return sn.trim() !== '';
            });
            
            if (serialNumbers.length === 0) {
                showBarcodeFeedback('warning', '⚠ Please scan or enter a Serial Number');
                return;
            }
            
            var tagsWrapper = document.getElementById('tagsWrapper');
            var existingTags = tagsWrapper.getElementsByClassName('tag-item');
            var addedCount = 0;
            var duplicateCount = 0;
            var addedSerials = [];
            var duplicateSerials = [];
            
            // Create a set of existing serial numbers for faster lookup
            var existingSerials = {};
            for (var i = 0; i < existingTags.length; i++) {
                var existingID = existingTags[i].getAttribute('data-device-id');
                existingSerials[existingID] = true;
            }
            
            // Process each serial number
            for (var j = 0; j < serialNumbers.length; j++) {
                var serialNo = serialNumbers[j].trim();
                
                if (serialNo === '') {
                    continue;
                }
                
                // Check duplicate
                if (existingSerials[serialNo]) {
                    duplicateCount++;
                    duplicateSerials.push(serialNo);
                    continue;
                }
                
                // Create tag element
                var tag = document.createElement('div');
                tag.className = 'tag-item';
                tag.setAttribute('data-device-id', serialNo);
                tag.ondblclick = function() { removeTag(this); };
                
                var tagText = document.createElement('span');
                tagText.className = 'tag-text';
                tagText.textContent = serialNo;
                
                var tagRemove = document.createElement('span');
                tagRemove.className = 'tag-remove';
                tagRemove.innerHTML = '×';
                tagRemove.onclick = function() { removeTag(tag); };
                
                tag.appendChild(tagText);
                tag.appendChild(tagRemove);
                tagsWrapper.appendChild(tag);
                
                // Add to existing set to prevent duplicates in same batch
                existingSerials[serialNo] = true;
                addedCount++;
                addedSerials.push(serialNo);
            }
            
            // Clear input and refocus
            input.value = '';
            input.focus();
            
            // Update counter and hidden field
            updateTagsCounter();
            updateHiddenField();
            
            // Show feedback
            if (addedCount > 0 && duplicateCount === 0) {
                if (addedCount === 1) {
                    showBarcodeFeedback('success', '✓ Added: ' + addedSerials[0]);
                } else {
                    showBarcodeFeedback('success', '✓ Added ' + addedCount + ' serial number(s)');
                }
            } else if (addedCount > 0 && duplicateCount > 0) {
                showBarcodeFeedback('warning', 'Added ' + addedCount + ' serial number(s). ' + duplicateCount + ' duplicate(s) skipped.');
            } else if (addedCount === 0 && duplicateCount > 0) {
                if (duplicateCount === 1) {
                    showBarcodeFeedback('warning', '⚠ Already scanned: ' + duplicateSerials[0]);
                } else {
                    showBarcodeFeedback('warning', 'All ' + duplicateCount + ' serial number(s) already scanned.');
                }
            }
        }

        // Remove tag
        function removeTag(tagElement) {
            var serialNo = tagElement.getAttribute('data-device-id');
            tagElement.remove();
            updateTagsCounter();
            updateHiddenField();
            showBarcodeFeedback('info', '🗑️ Removed: ' + serialNo);
            focusScanInput();
        }

        // Clear all tags
        function clearAllTags() {
            var tagsWrapper = document.getElementById('tagsWrapper');
            var count = tagsWrapper.getElementsByClassName('tag-item').length;
            
            if (count === 0) {
                showBarcodeFeedback('info', 'No tags to clear');
                return;
            }
            
            if (confirm('Clear all ' + count + ' scanned device(s)?')) {
                tagsWrapper.innerHTML = '';
                updateTagsCounter();
                updateHiddenField();
                showBarcodeFeedback('info', '🗑️ All tags cleared');
                focusScanInput();
            }
        }

        // Update tags counter
        function updateTagsCounter() {
            var tagsWrapper = document.getElementById('tagsWrapper');
            var count = tagsWrapper.getElementsByClassName('tag-item').length;
            document.getElementById('lblScanCount').textContent = count;
        }

        // Update hidden field for postback
        function updateHiddenField() {
            var tagsWrapper = document.getElementById('tagsWrapper');
            var tags = tagsWrapper.getElementsByClassName('tag-item');
            var serialNumbers = [];
            
            for (var i = 0; i < tags.length; i++) {
                serialNumbers.push(tags[i].getAttribute('data-device-id'));
            }
            
            var hdnField = document.getElementById('<%= hdnScannedDevices.ClientID %>');
            if (hdnField) {
                hdnField.value = serialNumbers.join(',');
            }
        }

        // Focus scan input
        function focusScanInput() {
            var input = document.getElementById('txtScanInput');
            if (input) {
                input.focus();
            }
        }

        // Validate and search multiple devices
        function validateAndSearchMultiple() {
            // Check warehouse selected first
            var warehouseID = document.getElementById('ContentPlaceHolder1_txtWarehouseID').value.trim();
            if (warehouseID === '' || warehouseID === 'Please select ...') {
                alert('⚠ Warehouse ID is required!\n\nPlease select a warehouse first before searching.');
                $('#modal-warehouse-filter').modal('show');
                return false;
            }
            
            var tagsWrapper = document.getElementById('tagsWrapper');
            var count = tagsWrapper.getElementsByClassName('tag-item').length;
            
            if (count === 0) {
                alert('⚠ No serial numbers scanned!\n\nPlease scan at least one serial number before searching.');
                focusScanInput();
                return false;
            }
            
            // Update hidden field before postback
            updateHiddenField();
            
            // Show loader
            document.getElementById('searchLoader').style.display = 'block';
            document.getElementById('searchProgress').textContent = count;
            
            return true; // Allow postback
        }

        // Auto-check devices after search (by Serial Number)
        function autoCheckMultipleDevices(serialNumbersArray) {
            setTimeout(function() {
                hideLoader();
                
                var normalizedSerials = (serialNumbersArray || []).map(function(sn) {
                    return (sn || '').toString().trim().toUpperCase();
                }).filter(function(sn) { return sn !== ''; });

                var gridView = document.getElementById('<%= GridViewPending.ClientID %>');
                if (!gridView) return;
                
                var rows = gridView.getElementsByTagName('tr');
                var foundCount = 0;
                
                // Loop each row
                for (var i = 1; i < rows.length; i++) {
                    var cells = rows[i].getElementsByTagName('td');
                    if (cells.length > 2) {
                        // cells[2] is Serial Number (NoSN) column
                        var serialNo = (cells[2].textContent || cells[2].innerText).trim().toUpperCase();
                        
                        // Check if this serial number is in our scanned list
                        if (normalizedSerials.indexOf(serialNo) !== -1) {
                                var checkbox = rows[i].querySelector('input[type="checkbox"]');
                                if (checkbox) {
                                    var wasChecked = checkbox.checked;
                                    if (!wasChecked) {
                                        checkbox.checked = true;
                                        
                                        // Highlight row only when status changes
                                        rows[i].style.backgroundColor = '#d4edda';
                                        setTimeout(function(row) {
                                            return function() { row.style.backgroundColor = ''; };
                                        }(rows[i]), 2000);
                                    }

                                    foundCount++;
                                }
                        }
                    }
                }
                
                // Show feedback
                if (foundCount > 0) {
                    showBarcodeFeedback('success', '✓ Found and checked: ' + foundCount + ' device(s)');
                } else {
                    showBarcodeFeedback('danger', '✓ Found and checked: 0 device(s). No matching pending devices.');
                }
            }, 500);
        }

        // Show barcode scan feedback
        function showBarcodeFeedback(type, message) {
            var feedbackDiv = document.getElementById('barcodeFeedback');
            if (!feedbackDiv) {
                feedbackDiv = document.createElement('div');
                feedbackDiv.id = 'barcodeFeedback';
                feedbackDiv.style.position = 'fixed';
                feedbackDiv.style.top = '80px';
                feedbackDiv.style.right = '20px';
                feedbackDiv.style.zIndex = '9999';
                feedbackDiv.style.minWidth = '300px';
                document.body.appendChild(feedbackDiv);
            }
            
            var alertClass = 'alert-warning';
            if (type === 'success') {
                alertClass = 'alert-success';
            } else if (type === 'danger') {
                alertClass = 'alert-danger';
            }
            feedbackDiv.innerHTML = '<div class="alert ' + alertClass + ' alert-dismissible" style="margin-bottom:0;">' +
                                   '<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                                   '<strong>' + message + '</strong></div>';
            feedbackDiv.style.display = 'block';
            
            setTimeout(function() {
                feedbackDiv.style.display = 'none';
            }, 3000);
        }

        // Show Submit Confirmation Modal
        function showSubmitConfirmation() {
            // Validation - Warehouse ID (MANDATORY)
            var warehouseID = document.getElementById('ContentPlaceHolder1_txtWarehouseID').value.trim();
            if (warehouseID === '' || warehouseID === 'Please select ...') {
                alert('⚠ Warehouse ID is required!\n\nPlease select a warehouse first before submitting QC.');
                $('#modal-warehouse-filter').modal('show');
                return false;
            }
            
            // Validation - Checkboxes
            var checkboxes = document.querySelectorAll('input[id*="chkSelect"]:checked');
            if (checkboxes.length === 0) {
                alert('Silakan pilih minimal 1 device!');
                return false;
            }

            var qcResult = document.getElementById('<%= CmbQCResult.ClientID %>').value;
            if (qcResult === '') {
                alert('Silakan pilih QC Result!');
                return false;
            }

            var qcNotes = document.getElementById('<%= txtQCNotes.ClientID %>').value.trim();

            // Populate confirmation modal
            var resultText = qcResult === '2' ? '✓ Pass (Lolos QC)' : '✗ Return (Rusak)';
            var resultClass = qcResult === '2' ? 'text-success' : 'text-danger';
            
            document.getElementById('confirmDeviceCount').innerHTML = '<strong>' + checkboxes.length + ' device(s)</strong>';
            document.getElementById('confirmQCResult').innerHTML = '<strong class="' + resultClass + '">' + resultText + '</strong>';
            
            if (qcNotes.length > 0) {
                document.getElementById('confirmNotes').innerHTML = '"' + qcNotes.substring(0, 100) + (qcNotes.length > 100 ? '...' : '') + '"';
            } else {
                document.getElementById('confirmNotes').innerHTML = '<span class="text-muted">(No notes)</span>';
            }

            // Show modal
            $('#modal-submit-confirmation').modal('show');
            return false; // Prevent immediate submit
        }

        // Confirm Submit - langsung submit
        function confirmSubmit() {
            $('#modal-submit-confirmation').modal('hide');
            
            // Trigger actual submit
            setTimeout(function() {
                __doPostBack('<%= CmdSubmit.UniqueID %>', '');
            }, 300);
        }

        // Modal handler
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function endRequest(sender, args) {
            hideLoader(); // Hide loader after any postback
            
            // Check if there's a message to display
            var messageDiv = document.getElementById('ContentPlaceHolder1_div_comment');
            if (messageDiv && messageDiv.innerHTML !== '') {
                var messageHtml = messageDiv.innerHTML;
                
                // Show modal
                $('#modal-messagebox').modal('show');
                
                // If it's a success message, auto reload after 2 seconds
                if (messageHtml.indexOf('alert-success') > -1) {
                    setTimeout(function() {
                        location.reload();
                    }, 2000);
                }
            }
            
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
        }
        endRequest();

        // Focus scan input on page load for barcode scanner
        window.onload = function() {
            setTimeout(function() {
                // Check if warehouse is selected
                var warehouseID = document.getElementById('ContentPlaceHolder1_txtWarehouseID');
                if (warehouseID && warehouseID.value.trim() !== '' && warehouseID.value !== 'Please select ...') {
                    focusScanInput();
                } else {
                    // Warehouse not selected, show warning in input placeholder
                    var scanInput = document.getElementById('txtScanInput');
                    if (scanInput) {
                        scanInput.placeholder = '⚠ Please select Warehouse first!';
                        scanInput.disabled = true;
                    }
                }
            }, 500);
        };
        
        // Re-enable scan input after warehouse selected
        function enableScanAfterWarehouseSelect() {
            var scanInput = document.getElementById('txtScanInput');
            if (scanInput) {
                scanInput.disabled = false;
                scanInput.placeholder = 'scan serial number (NoSN) here...';
                focusScanInput();
            }
        }

        // Callback from warehouse search modal (simplified - only WarehouseID)
        function postWarehouseFilter(sWarehouseID, sWarehouseName, sWarehouseAddress, sBranchName) {
            if (sWarehouseID != '') {
                // Set Warehouse ID only (HTML input)
                var txtWarehouseID = document.getElementById('ContentPlaceHolder1_txtWarehouseID');
                if (txtWarehouseID) {
                    txtWarehouseID.value = sWarehouseID;
                    txtWarehouseID.style.backgroundColor = '#ffffff';
                    txtWarehouseID.style.borderColor = '#00a65a';
                }
                
                // Enable scan input
                enableScanAfterWarehouseSelect();
                
                // Close modal
                $('#modal-warehouse-filter').modal('hide');
                
                // Show success feedback with warehouse name
                showBarcodeFeedback('success', '✓ Warehouse selected: ' + sWarehouseID + ' - ' + sWarehouseName);
                
                // NO POSTBACK - Warehouse is for information only, not for filtering
                // GridView remains showing ALL devices
            }
        }
    </script>

    <script type="text/javascript">
        // ===== SETTING CHANNEL MDVR =====
        (function () {
            var CH_HANDLER = 'mdvr_channel.ashx';
            var currentNoSN = '';
            var currentMaxChannel = 0;

            function chEscapeHtml(s) {
                return (s == null ? '' : String(s))
                    .replace(/&/g, '&amp;').replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
            }

            function showChannelAlert(type, msg) {
                var el = document.getElementById('channelSettingAlert');
                if (!el) { return; }
                var icons = { danger: 'fa-exclamation-circle', success: 'fa-check-circle', warning: 'fa-exclamation-triangle', info: 'fa-info-circle' };
                var icon = icons[type] || 'fa-info-circle';
                el.innerHTML = '<div class="ch-alert ch-alert-' + type + '"><i class="fa ' + icon + '"></i><span>' + chEscapeHtml(msg) + '</span></div>';
            }

            function buildChannelSkeleton(count) {
                var wrap = document.getElementById('channelSkeleton');
                if (!wrap) { return; }
                var n = Math.max(1, Math.min(parseInt(count, 10) || 4, 12));
                var html = '';
                for (var i = 0; i < n; i++) {
                    html += '<div class="ch-skel-row"><div class="ch-skel-bar ch-skel-label"></div><div class="ch-skel-bar ch-skel-input"></div></div>';
                }
                wrap.innerHTML = html;
            }

            function setChannelSaveLoading(isLoading) {
                var btn = document.getElementById('btnSaveChannelSetting');
                if (!btn) { return; }
                if (isLoading) {
                    btn.disabled = true;
                    btn.innerHTML = '<i class="fa fa-circle-o-notch fa-spin"></i> Menyimpan...';
                } else {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="fa fa-save"></i> Save Setting';
                }
            }

            function buildChannelTypeOptions(types) {
                var html = '<option value="">Tidak Digunakan</option>';
                for (var t = 0; t < types.length; t++) {
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
                for (var i = 0; i < existing.length; i++) {
                    map[String(existing[i].ChanelID)] = (existing[i].ChanelType || '').toUpperCase();
                }
                var rows = document.getElementById('channelSettingRows');
                rows.innerHTML = '';
                for (var j = 0; j < master.length; j++) {
                    var m = master[j];
                    var cur = map[String(m.ChanelID)] || '';
                    var label = m.ChanelName || ('Channel ' + m.ChanelID);
                    var code = m.ChanelCode ? (' <small class="text-muted">(' + chEscapeHtml(m.ChanelCode) + ')</small>') : '';
                    var tr = document.createElement('tr');
                    var tdLabel = document.createElement('td');
                    tdLabel.innerHTML = '<strong>' + chEscapeHtml(label) + '</strong>' + code;
                    var tdSel = document.createElement('td');
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

            function markChannelConfigured(nosn, isConfigured) {
                var cells = document.querySelectorAll('.channel-status-cell');
                for (var i = 0; i < cells.length; i++) {
                    if ((cells[i].getAttribute('data-nosn') || '') === nosn) {
                        var badge = cells[i].querySelector('.label-channel-status');
                        var btn = cells[i].querySelector('.btn-channel-setting');
                        if (isConfigured) {
                            if (badge) { badge.className = 'label-channel-status label label-success'; badge.textContent = 'Sudah disetting'; }
                            if (btn) { btn.className = 'btn-channel-setting btn btn-xs btn-primary'; btn.innerHTML = '<i class="fa fa-sliders"></i> Edit Setting'; }
                        } else {
                            if (badge) { badge.className = 'label-channel-status label label-warning'; badge.textContent = 'Belum disetting'; }
                            if (btn) { btn.className = 'btn-channel-setting btn btn-xs btn-warning'; btn.innerHTML = '<i class="fa fa-sliders"></i> Setting Channel'; }
                        }
                    }
                }
            }

            function loadChannelRows() {
                $.ajax({
                    url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                    data: { action: 'load', nosn: currentNoSN, maxchannel: currentMaxChannel }
                }).done(function (res) {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    if (!res || !res.success) {
                        showChannelAlert('danger', (res && res.message) || 'Gagal memuat channel.');
                        return;
                    }
                    renderChannelRows(res.master || [], res.existing || [], res.types || []);
                    document.getElementById('channelSettingTable').style.display = '';
                    document.getElementById('btnSaveChannelSetting').disabled = false;
                }).fail(function () {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    showChannelAlert('danger', 'Gagal terhubung ke server.');
                });
            }

            window.openChannelModal = function (nosn, deviceType, maxChannel) {
                currentNoSN = (nosn || '').toString().trim();
                currentMaxChannel = parseInt(maxChannel, 10) || 0;

                document.getElementById('chNoSN').textContent = currentNoSN || '-';
                document.getElementById('chDeviceType').textContent = deviceType || '-';
                document.getElementById('chMaxChannel').textContent = currentMaxChannel > 0 ? currentMaxChannel : '-';
                document.getElementById('channelSettingAlert').innerHTML = '';
                document.getElementById('channelSettingRows').innerHTML = '';
                document.getElementById('channelSettingTable').style.display = 'none';
                buildChannelSkeleton(currentMaxChannel > 0 ? currentMaxChannel : 4);
                document.getElementById('channelSettingLoading').style.display = 'block';
                document.getElementById('btnSaveChannelSetting').disabled = true;

                $('#modal-channel-setting').modal('show');

                if (!currentNoSN) {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    showChannelAlert('danger', 'NoSN tidak valid.');
                    return;
                }

                // Jika MaxChannel dari grid valid, langsung load. Jika tidak, ambil dari server (GPS Type max_ch).
                if (currentMaxChannel > 0) {
                    loadChannelRows();
                    return;
                }

                $.ajax({
                    url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                    data: { action: 'require', nosn: currentNoSN }
                }).done(function (res) {
                    if (!res || !res.success || !res.isRequire) {
                        document.getElementById('channelSettingLoading').style.display = 'none';
                        showChannelAlert('warning', 'Device ini tidak memerlukan setting channel MDVR atau Max Channel belum dikonfigurasi.');
                        return;
                    }
                    currentMaxChannel = parseInt(res.maxChannel, 10) || 0;
                    if (currentMaxChannel <= 0) {
                        document.getElementById('channelSettingLoading').style.display = 'none';
                        showChannelAlert('danger', 'Max Channel tidak valid. Periksa konfigurasi GPS Type (max_ch).');
                        return;
                    }
                    document.getElementById('chMaxChannel').textContent = currentMaxChannel;
                    loadChannelRows();
                }).fail(function () {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    showChannelAlert('danger', 'Gagal terhubung ke server.');
                });
            };

            window.saveChannelSetting = function () {
                if (!currentNoSN || currentMaxChannel <= 0) {
                    showChannelAlert('danger', 'NoSN atau MaxChannel tidak valid.');
                    return;
                }
                var selects = document.querySelectorAll('#channelSettingRows .channel-type-select');
                var settings = [];
                var seen = {};
                for (var i = 0; i < selects.length; i++) {
                    var cid = parseInt(selects[i].getAttribute('data-chanelid'), 10);
                    if (seen[cid]) { showChannelAlert('danger', 'ChanelID ' + cid + ' duplikat.'); return; }
                    seen[cid] = true;
                    settings.push({ ChanelID: cid, ChanelType: selects[i].value });
                }

                setChannelSaveLoading(true);
                $.ajax({
                    url: CH_HANDLER, type: 'POST', dataType: 'json',
                    data: { action: 'save', nosn: currentNoSN, maxchannel: currentMaxChannel, settings: JSON.stringify(settings) }
                }).done(function (res) {
                    setChannelSaveLoading(false);
                    if (!res || !res.success) {
                        showChannelAlert('danger', (res && res.message) || 'Gagal menyimpan setting.');
                        return;
                    }
                    markChannelConfigured(currentNoSN, !!res.isConfigured);
                    $('#modal-channel-setting').modal('hide');
                    if (typeof showBarcodeFeedback === 'function') {
                        showBarcodeFeedback('success', res.message || 'Setting channel tersimpan.');
                    }
                }).fail(function () {
                    setChannelSaveLoading(false);
                    showChannelAlert('danger', 'Gagal terhubung ke server.');
                });
            };

            // Event delegation: tombol Setting Channel / Edit Setting di grid (tahan partial postback)
            $(document).on('click', '.btn-channel-setting', function () {
                openChannelModal(this.getAttribute('data-nosn'), this.getAttribute('data-devicetype'), this.getAttribute('data-maxchannel'));
            });
        })();
    </script>
</asp:Content>
