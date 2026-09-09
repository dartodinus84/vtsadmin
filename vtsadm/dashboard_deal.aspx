<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_deal.aspx.cs" Inherits="vtsadm.dashboard_deal" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .metric-card {
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            padding: 20px;
            margin-bottom: 20px;
            background-color: #fff;
        }
    
        .metric-title {
            font-size: 14px;
            color: #666;
            margin-bottom: 10px;
        }
    
        .metric-value {
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 8px;
        }
    
        .metric-change {
            font-size: 12px;
        }
    
        .metric-change.positive {
            color: #28a745;
        }
    
        .metric-change.negative {
            color: #dc3545;
        }
    
        .deal-card {
            border-left: 4px solid #007bff;
            border-radius: 4px;
            padding: 15px;
            margin-bottom: 10px;
            background-color: #fff;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
            cursor: pointer; /* Menambahkan cursor pointer */
        }
    
        .deal-title {
            font-weight: bold;
            margin-bottom: 5px;
        }
    
        .deal-company {
            color: #666;
            font-size: 12px;
            margin-bottom: 5px;
        }
    
        .deal-marketing {
            color: #666;
            font-size: 12px;
            margin-bottom: 4px;
        }
    
        .deal-duration {
            color: #888;
            font-size: 11px;
            margin-bottom: 8px;
            font-style: italic;
        }
    
        .deal-marketing i, .deal-duration i {
            margin-right: 4px;
        }
    
        /* Tambahan CSS untuk badge expired */
        .deal-expired {
            color: #dc3545;
            font-size: 11px;
            font-weight: bold;
            margin-bottom: 4px;
            font-style: normal;
        }
    
        .deal-amount {
            font-weight: bold;
        }
    
        .deal-date {
            color: #888;
            font-size: 12px;
            text-align: right;
        }
    
        .stage-container {
            margin-bottom: 20px;
        }
    
        .stage-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }
    
        .stage-title {
            font-weight: bold;
            display: flex;
            align-items: center;
        }
    
        .stage-dot {
            display: inline-block;
            width: 12px;
            height: 12px;
            border-radius: 50%;
            margin-right: 8px;
        }
    
        .stage-count {
            background-color: rgba(0, 0, 0, 0.05);
            border-radius: 12px;
            padding: 4px 10px;
            font-size: 12px;
            font-weight: 600;
        }
    
        .prospekt-dot { background-color: #36b9cc; }
        .prospekt-count { background-color: rgba(54, 185, 204, 0.15); color: #36b9cc; }
    
        .leads-dot { background-color: #4e73df; }
        .leads-count { background-color: rgba(78, 115, 223, 0.15); color: #4e73df; }
    
        .visit-dot { background-color: #6f42c1; }
        .visit-count { background-color: rgba(111, 66, 193, 0.15); color: #6f42c1; }
    
        .presentasi-dot { background-color: #fd7e14; }
        .presentasi-count { background-color: rgba(253, 126, 20, 0.15); color: #fd7e14; }
    
        .trial-dot { background-color: #f6c23e; }
        .trial-count { background-color: rgba(246, 194, 62, 0.15); color: #f6c23e; }
    
        .closed-won-dot { background-color: #1cc88a; }
        .closed-won-count { background-color: rgba(28, 200, 138, 0.15); color: #1cc88a; }
    
        .closed-lost-dot { background-color: #e74a3b; }
        .closed-lost-count { background-color: rgba(231, 74, 59, 0.15); color: #e74a3b; }

        .stage-cards {
            min-height: 300px;
            max-height: 600px;
            overflow-y: auto;
        }

        .filter-container {
            display: flex;
            gap: 15px;
            margin-bottom: 20px;
        }

        .filter-group {
            flex: 1;
        }
    
        .pipeline-stages {
            display: flex;
            flex-wrap: wrap;
        }
    
        .pipeline-stage {
            width: 14.28%; /* 100% / 7 categories */
            padding: 0 8px;
        }
    
        /* Timeline Styling */
        .timeline {
            position: relative;
            margin: 0 0 30px 0;
            padding: 0;
            list-style: none;
        }
    
        .timeline:before {
            content: '';
            position: absolute;
            top: 0;
            bottom: 0;
            width: 4px;
            background: #ddd;
            left: 31px;
            margin: 0;
            border-radius: 2px;
        }
    
        .timeline > li {
            position: relative;
            margin-right: 10px;
            margin-bottom: 15px;
        }
    
        .timeline > li:before,
        .timeline > li:after {
            content: " ";
            display: table;
        }
    
        .timeline > li:after {
            clear: both;
        }
    
        .timeline > li > .timeline-item {
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
            border-radius: 3px;
            margin-top: 0;
            background: #fff;
            color: #444;
            margin-left: 60px;
            margin-right: 15px;
            padding: 0;
            position: relative;
        }
    
        .timeline > li > .timeline-item > .time {
            color: #999;
            float: right;
            padding: 10px;
            font-size: 12px;
        }
    
        .timeline > li > .timeline-item > .timeline-header {
            margin: 0;
            color: #555;
            border-bottom: 1px solid #f4f4f4;
            padding: 10px;
            font-size: 16px;
            font-weight: 600;
        }
    
        .timeline > li > .timeline-item > .timeline-body {
            padding: 10px;
        }
    
        .timeline > li > .fa {
            width: 30px;
            height: 30px;
            font-size: 15px;
            line-height: 30px;
            position: absolute;
            color: #fff;
            background: #d2d6de;
            border-radius: 50%;
            text-align: center;
            left: 18px;
            top: 0;
        }
    
        /* Color variations for different status */
        .timeline-header.prospect { background-color: rgba(54, 185, 204, 0.15); }
        .timeline-header.leads { background-color: rgba(78, 115, 223, 0.15); }
        .timeline-header.visit { background-color: rgba(111, 66, 193, 0.15); }
        .timeline-header.presentasi { background-color: rgba(253, 126, 20, 0.15); }
        .timeline-header.trial { background-color: rgba(246, 194, 62, 0.15); }
        .timeline-header.closed-won { background-color: rgba(28, 200, 138, 0.15); }
        .timeline-header.closed-lost { background-color: rgba(231, 74, 59, 0.15); }
    
        .fa.prospect { background-color: #36b9cc; }
        .fa.leads { background-color: #4e73df; }
        .fa.visit { background-color: #6f42c1; }
        .fa.presentasi { background-color: #fd7e14; }
        .fa.trial { background-color: #f6c23e; }
        .fa.closed-won { background-color: #1cc88a; }
        .fa.closed-lost { background-color: #e74a3b; }
    
        /* Responsive adjustments */
        @media (max-width: 1200px) {
            .pipeline-stage {
                width: 25%;
            }
        }
    
        @media (max-width: 992px) {
            .pipeline-stage {
                width: 33.33%;
            }
        }
    
        @media (max-width: 768px) {
            .pipeline-stage {
                width: 50%;
            }
        }
    
        @media (max-width: 576px) {
            .pipeline-stage {
                width: 100%;
            }
        }
    </style>

    <section class="content-header">
        <h1>Sales Pipeline
            <small>Deal Monitoring</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-chart-pie"></i>Sales Pipeline</a></li>
        </ol>
    </section>

    <section class="content">
        <!-- Alert Messages -->
        <div class="row">
            <div class="col-md-12">
                <asp:Panel ID="div_comment" runat="server"></asp:Panel>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Filter Date Range</h3>
                    </div>
                    <div class="box-body">
                        <div class="filter-container">
                            <div class="filter-group">
                                <label>Date From</label>
                                <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Select start date..."></asp:TextBox>
                            </div>
                            <div class="filter-group">
                                <label>Date To</label>
                                <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Select end date..."></asp:TextBox>
                            </div>
                            <div class="filter-group">
                                <label>Marketing</label>
                                <asp:DropDownList ID="ddlMarketing" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-default" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClientClick="showOverlay();" OnClick="CmdSearch_Click" Text="Apply Filter"/>
                        <asp:Button ID="CmdExportExcel" CssClass="btn btn-success" runat="server" OnClick="CmdExportExcel_Click" Text="Export Excel" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-3 col-md-6">
                <div class="metric-card">
                    <div class="metric-title">Total Pipeline Value</div>
                    <div class="metric-value">Rp <asp:Literal ID="LblCntDealNew" runat="server">0</asp:Literal></div>
                    <div class="metric-change">
                        <asp:Literal ID="LblPctDealNewIndicator" runat="server"><span class="positive">+</span></asp:Literal>
                        <asp:Literal ID="LblPctDealNew" runat="server">0</asp:Literal>% dari bulan sebelumnya
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="metric-card">
                    <div class="metric-title">Active Deals</div>
                    <div class="metric-value"><asp:Literal ID="LblCntDealActive" runat="server">0</asp:Literal></div>
                    <div class="metric-change">
                        <asp:Literal ID="LblPctDealActiveIndicator" runat="server"><span class="positive">+</span></asp:Literal>
                        <asp:Literal ID="LblPctDealActive" runat="server">0</asp:Literal> dari bulan sebelumnya
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="metric-card">
                    <div class="metric-title">Avg. Deal Size</div>
                    <div class="metric-value">Rp <asp:Literal ID="LblAvgDealSize" runat="server">0</asp:Literal> </div>
                    <div class="metric-change">
                        <asp:Literal ID="LblPctAvgDealSizeIndicator" runat="server"><span class="positive">+</span></asp:Literal>
                        <asp:Literal ID="LblPctAvgDealSize" runat="server">0</asp:Literal>% dari bulan sebelumnya
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="metric-card">
                    <div class="metric-title">Win Rate</div>
                    <div class="metric-value"><asp:Literal ID="LblWinRate" runat="server">0</asp:Literal>%</div>
                    <div class="metric-change">
                        <asp:Literal ID="LblPctWinRateIndicator" runat="server"><span class="positive">+</span></asp:Literal>
                        <asp:Literal ID="LblPctWinRate" runat="server">0</asp:Literal>% dari bulan sebelumnya
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="pipeline-stages">
                    <!-- Prospect Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot prospekt-dot"></span>
                                    Prospect
                                </div>
                                <div class="stage-count prospekt-count"><asp:Literal ID="LblCntDealProspect" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="prospectCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Leads Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot leads-dot"></span>
                                    Leads
                                </div>
                                <div class="stage-count leads-count"><asp:Literal ID="LblCntDealLeads" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="leadsCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Visit Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot visit-dot"></span>
                                    Visit
                                </div>
                                <div class="stage-count visit-count"><asp:Literal ID="LblCntDealVisit" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="visitCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Presentasi Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot presentasi-dot"></span>
                                    Presentasi
                                </div>
                                <div class="stage-count presentasi-count"><asp:Literal ID="LblCntDealPresentasi" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="presentasiCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Trial Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot trial-dot"></span>
                                    Trial
                                </div>
                                <div class="stage-count trial-count"><asp:Literal ID="LblCntDealTrial" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="trialCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Closed Won Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot closed-won-dot"></span>
                                    Close Won
                                </div>
                                <div class="stage-count closed-won-count"><asp:Literal ID="LblCntDealWon" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="wonCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- Closed Lost Section -->
                    <div class="pipeline-stage">
                        <div class="stage-container">
                            <div class="stage-header">
                                <div class="stage-title">
                                    <span class="stage-dot closed-lost-dot"></span>
                                    Close Lost
                                </div>
                                <div class="stage-count closed-lost-count"><asp:Literal ID="LblCntDealLost" runat="server">0</asp:Literal></div>
                            </div>
                            <div class="stage-cards">
                                <asp:Literal ID="lostCards" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Detail Deal dengan Timeline -->
        <div class="modal fade" id="dealDetailModal" tabindex="-1" role="dialog" aria-labelledby="dealDetailModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="dealDetailModalLabel">Detail Deal</h4>
                    </div>
                    <div class="modal-body">
                        <!-- Informasi dasar deal -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Informasi Deal</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <table class="table table-striped">
                                                    <tr>
                                                        <td>Activity Code</td>
                                                        <td id="modal-activity-code"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Customer Name</td>
                                                        <td id="modal-customer-name"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Marketing Name</td>
                                                        <td id="modal-marketing-name"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>SoID</td>
                                                        <td id="modal-po-id"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total Unit</td>
                                                        <td id="modal-total-unit"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-md-6">
                                                <table class="table table-striped">
                                                    <tr>
                                                        <td>Product Name</td>
                                                        <td id="modal-product-name"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Price</td>
                                                        <td id="modal-price"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Registration Date</td>
                                                        <td id="modal-reg-date"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total Installed Units</td>
                                                        <td id="modal-total-installed-units"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Total Uninstalled Units</td>
                                                        <td id="modal-total-uninstalled-units"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Timeline history deal -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Deal History Timeline</h3>
                                    </div>
                                    <div class="box-body">
                                        <ul class="timeline" id="deal-timeline">
                                            <!-- Timeline items will be added here via JavaScript -->
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Loading Screen -->
        <div id="overlay">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        $(document).ready(function () {
            // Hide loading overlay when page is fully loaded
            $("#overlay").hide();
        });

        function showOverlay() {
            $("#overlay").show();
        }

        function showDealDetail(jobActivityId) {
            // Show loading
            showOverlay();

            // AJAX call to get deal details
            $.ajax({
                type: "POST",
                url: "dashboard_deal.aspx/GetDealDetail",
                data: JSON.stringify({ JobActivityId: jobActivityId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Hide loading
                    $("#overlay").hide();

                    var data = response.d;

                    // Populate basic deal information
                    $("#modal-activity-code").text(data.ActivityCode);
                    $("#modal-customer-name").text(data.FullName);
                    $("#modal-marketing-name").text(data.MarketingName);
                    $("#modal-product-name").text(data.ProductName);
                    $("#modal-price").text("Rp " + formatNumber(data.Price));
                    $("#modal-reg-date").text(formatDate(data.RegDate));
                    $("#modal-po-id").text(data.PoID || '');
                    $("#modal-total-unit").text(data.TotalUnit != null ? data.TotalUnit : '');
                    $("#modal-total-installed-units").text(data.TotalInstalledUnits != null ? data.TotalInstalledUnits : '');
                    $("#modal-total-uninstalled-units").text(data.TotalUninstalledUnits != null ? data.TotalUninstalledUnits : '');

                    // Clear existing timeline
                    $("#deal-timeline").empty();

                    // Populate timeline with history items
                    if (data.Timeline && data.Timeline.length > 0) {
                        $.each(data.Timeline, function (index, item) {
                            var statusClass = getStatusClass(item.DealStatusName.toLowerCase());
                            var timelineIcon = getTimelineIcon(item.DealStatusName.toLowerCase());

                            var timelineItem =
                                '<li>' +
                                '<i class="fa ' + timelineIcon + ' ' + statusClass + '"></i>' +
                                '<div class="timeline-item">' +
                                '<span class="time"><i class="fa fa-clock-o"></i> ' + formatDate(item.StatusChangeDate) + '</span>' +
                                '<h3 class="timeline-header ' + statusClass + '">' + item.DealStatusName + '</h3>' +
                                '<div class="timeline-body">' +
                                '<p><strong>Activity Date:</strong> ' + formatDate(item.ActivityDate) + '</p>' +
                                '<p><strong>Address Location:</strong> ' + (item.AddressLocation || '-') + '</p>' +
                                '<p><strong>Remarks:</strong> ' + (item.Remark || 'No remarks') + '</p>' +
                                '</div>' +
                                '</div>' +
                                '</li>';

                            $("#deal-timeline").append(timelineItem);
                        });
                    } else {
                        $("#deal-timeline").append('<li><div class="timeline-item"><div class="timeline-body">No history available</div></div></li>');
                    }

                    // Show the modal
                    $("#dealDetailModal").modal("show");
                },
                error: function (xhr, status, error) {
                    // Hide loading
                    $("#overlay").hide();
                    console.error("Error fetching deal details:", error);
                    alert("Failed to load deal details. Please try again.");
                }
            });
        }

        function getStatusClass(statusName) {
            if (statusName.includes('prospect')) return 'prospect';
            if (statusName.includes('lead')) return 'leads';
            if (statusName.includes('visit')) return 'visit';
            if (statusName.includes('presentasi')) return 'presentasi';
            if (statusName.includes('trial')) return 'trial';
            if (statusName.includes('won')) return 'closed-won';
            if (statusName.includes('lost')) return 'closed-lost';
            return '';
        }

        function getTimelineIcon(statusName) {
            if (statusName.includes('prospect')) return 'fa-search';
            if (statusName.includes('lead')) return 'fa-user-plus';
            if (statusName.includes('visit')) return 'fa-car';
            if (statusName.includes('presentasi')) return 'fa-file-powerpoint-o';
            if (statusName.includes('trial')) return 'fa-flask';
            if (statusName.includes('won')) return 'fa-check-circle';
            if (statusName.includes('lost')) return 'fa-times-circle';
            return 'fa-circle';
        }

        function formatNumber(num) {
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }

        function formatDate(dateString) {
            var date = new Date(parseInt(dateString.substr(6)));
            return date.toLocaleDateString('id-ID', { day: '2-digit', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' });
        }
    </script>
</asp:Content>