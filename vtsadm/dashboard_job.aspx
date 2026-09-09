<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_job.aspx.cs" Inherits="vtsadm.dashboard_job" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .dashboard-box {
            border-radius: 11px;
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.14);
        }

        .dashboard-filter .box-body {
            padding: 15px 15px 8px 15px;
        }

        .dashboard-filter .box-footer {
            padding: 10px 15px 15px 15px;
        }

        .dashboard-filter .form-group {
            margin-bottom: 12px;
        }

        .dashboard-filter label {
            margin-bottom: 6px;
            font-weight: 600;
        }

        .dashboard-context {
            margin-top: 6px;
            font-size: 13px;
            color: #606c78;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .dashboard-context span {
            font-weight: 600;
            color: #374049;
        }

        .dashboard-context-separator {
            color: #8c98a5;
            font-weight: 400 !important;
        }

        .job-summary {
            border-radius: 11px;
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.14);
            color: #fff;
            padding: 16px 18px;
            min-height: 122px;
            position: relative;
            overflow: hidden;
        }

        .job-summary .summary-caption {
            font-size: 12px;
            text-transform: none;
            opacity: .85;
            letter-spacing: .3px;
            margin-bottom: 8px;
        }

        .job-summary .summary-value {
            font-size: 36px;
            line-height: 1.1;
            margin: 0;
            font-weight: 700;
        }

        .job-summary .summary-title {
            margin-top: 7px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: 600;
        }

        .job-summary .summary-icon {
            position: absolute;
            right: 15px;
            top: 14px;
            font-size: 26px;
            opacity: .35;
        }

        .job-summary-primary {
            background: linear-gradient(120deg, #2f80ed, #2d6cc7);
        }

        .job-summary-secondary {
            background: linear-gradient(120deg, #3d8bfd, #3069c8);
        }

        .dashboard-list {
            margin-bottom: 0;
        }

        .dashboard-click-row {
            min-height: 48px;
        }

        .dashboard-click-row > a {
            display: flex !important;
            align-items: center;
            justify-content: space-between;
            min-height: 48px;
            padding: 12px 14px !important;
            transition: background-color .2s ease;
            cursor: pointer;
            color: #222d32;
        }

        .dashboard-click-row > a:hover,
        .dashboard-click-row > a:focus {
            background-color: #f4f8ff;
            color: #222d32;
        }

        .dashboard-row-right {
            display: inline-flex;
            align-items: center;
            gap: 10px;
        }

        .dashboard-badge {
            min-width: 36px;
            text-align: center;
            font-size: 12px;
            background-color: #3c8dbc !important;
        }

        .dashboard-arrow {
            color: #8a96a3;
            width: 14px;
            text-align: center;
        }

        .dashboard-empty-state {
            padding: 20px 14px;
            color: #7b8894;
            text-align: center;
            display: none;
        }

        .dashboard-hidden {
            display: none !important;
        }

        .dashboard-content {
            padding-bottom: 8px;
        }

        .section {
            margin-bottom: 24px;
        }

        .section:last-child {
            margin-bottom: 0;
        }

        .section .row {
            margin-left: -8px;
            margin-right: -8px;
        }

        .section .row > [class*="col-"] {
            padding-left: 8px;
            padding-right: 8px;
        }

        .section-grid .row > [class*="col-"] {
            margin-bottom: 16px;
        }

        @media (min-width: 768px) {
            .section-grid .row > [class*="col-"] {
                margin-bottom: 0;
            }
        }
    </style>

    <section class="content-header">
        <h1>Dashboard
        <small>Job Order</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Job Order</a></li>
        </ol>
    </section>


    <section class="content dashboard-content">
        <div class="section section-filter">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid dashboard-box dashboard-filter">
                    <div class="box-header with-border">
                        <h3 class="box-title">Filter</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Filter Type</label>
                                    <asp:DropDownList ID="CmbFilterType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Regional Name</label>
                                    <asp:DropDownList ID="CmbSupportAreaID" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="CmbSupportAreaID_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Date From</label>
                                    <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" CssClass="form-control" placeholder="Input date from ..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-6">
                                <div class="form-group form-group-sm">
                                    <label>Date To</label>
                                    <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" CssClass="form-control" placeholder="Input date to ..."></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="dashboard-context">
                            <span id="regionalContextLabel">Regional: <span id="lblRegion" runat="server"></span></span>
                            <span id="dateContextLabel" class="dashboard-hidden">
                                <span class="dashboard-context-separator">&bull;</span>
                                Date: <span id="activeDate"></span>
                            </span>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClientClick="showOverlay();" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
            </div>
        </div>
        </div>

        <div class="section section-grid section-summary">
        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="job-summary job-summary-primary">
                    <div class="summary-caption">Total Job</div>
                    <h3 class="summary-value">
                            <label id="LblCntJONew" runat="server">0</label>
                    </h3>
                    <p class="summary-title">New Installation</p>
                    <div class="summary-icon">
                        <i class="fa fa-truck"></i>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="job-summary job-summary-secondary">
                    <div class="summary-caption">Total Job</div>
                    <h3 class="summary-value">
                            <label id="LblCntJOMaint" runat="server">0</label>
                    </h3>
                    <p class="summary-title">Maintenance</p>
                    <div class="summary-icon">
                        <i class="fa fa-wrench"></i>
                    </div>
                </div>
            </div>
        </div>
        </div>

        <div class="section section-grid section-job-order">
        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="box box-solid dashboard-box">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-truck"></i>
                        New Installation
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked dashboard-list">
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToInstall('open'); return false;">
                                    <span>Open</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoNewOpen" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToInstall('process'); return false;">
                                    <span>Process</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoNewProcess" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToInstall('close'); return false;">
                                    <span>Close</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoNewClose" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="box box-solid dashboard-box">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Maintenance
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked dashboard-list">
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToMaintenance('open'); return false;">
                                    <span>Open</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoMaintOpen" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToMaintenance('process'); return false;">
                                    <span>Process</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoMaintProcess" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                            <li class="dashboard-click-row">
                                <a href="javascript:void(0);" class="js-show-loading" onclick="goToMaintenance('close'); return false;">
                                    <span>Close</span>
                                    <span class="dashboard-row-right">
                                        <span id="LblCntJoMaintClose" runat="server" class="badge dashboard-badge">0</span>
                                        <i class="fa fa-angle-right dashboard-arrow"></i>
                                    </span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        </div>

        <asp:Panel ID="pnlRegion" runat="server" CssClass="section section-regional">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="box box-solid dashboard-box">
                    <div class="box-header bg-gray-light">
                        <i class="fa fa-building-o"></i>
                        Job by Region
                    </div>
                    <div class="box-footer no-padding">
                        <ul class="nav nav-stacked dashboard-list" id="regionalList">
                            <asp:Repeater ID="rptRegion" runat="server" OnItemCommand="rptRegion_ItemCommand">
                                <ItemTemplate>
                                    <li class="dashboard-click-row regional-row">
                                        <asp:LinkButton
                                            ID="lnkRegion"
                                            runat="server"
                                            CssClass="js-regional-link"
                                            CommandName="SelectRegion"
                                            CommandArgument='<%# Eval("SupAreaID") %>'
                                            CausesValidation="false"
                                            OnClientClick="showOverlay();">
                                            <span><%# Eval("SupAreaName") %></span>
                                            <span class="dashboard-row-right">
                                                <span class="badge dashboard-badge"><%# Eval("cntJO") %></span>
                                                <i class="fa fa-angle-right dashboard-arrow"></i>
                                            </span>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="dashboard-empty-state" id="regionalEmptyState">Tidak ada data untuk filter ini</div>
                    </div>
                </div>
            </div>
        </div>
        </asp:Panel>
    </section>

    <script type="text/javascript">
        (function () {
            function parseCount(text) {
                var number = parseInt((text || "").replace(/[^0-9\-]/g, ""), 10);
                return isNaN(number) ? 0 : number;
            }

            function formatDateDisplay(valueFrom, valueTo) {
                function toPrettyDate(value) {
                    if (!value) {
                        return "-";
                    }

                    var date = new Date(value);
                    if (isNaN(date.getTime())) {
                        return value;
                    }

                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    return date.getDate() + " " + monthNames[date.getMonth()] + " " + date.getFullYear();
                }

                if (!valueFrom && !valueTo) {
                    return "";
                }

                var dateTextFrom = toPrettyDate(valueFrom);
                var dateTextTo = toPrettyDate(valueTo);
                if (valueFrom && valueTo && valueFrom === valueTo) {
                    return dateTextFrom;
                }

                return dateTextFrom + " s/d " + dateTextTo;
            }

            function syncFilterContext() {
                var regionalSelect = document.getElementById("<%= CmbSupportAreaID.ClientID %>");
                var dateFrom = document.getElementById("<%= txtDateFrom.ClientID %>");
                var dateTo = document.getElementById("<%= txtDateTo.ClientID %>");

                var selectedRegional = "ALL";
                if (regionalSelect && regionalSelect.selectedIndex >= 0) {
                    var selectedText = regionalSelect.options[regionalSelect.selectedIndex].text || "";
                    var selectedValue = regionalSelect.options[regionalSelect.selectedIndex].value || "";
                    if (selectedText && selectedText.toUpperCase() !== "[SELECT]" && selectedText.toUpperCase() !== "ALL") {
                        selectedRegional = selectedText;
                    } else if (selectedValue && selectedValue.toUpperCase() !== "[SELECT]" && selectedValue.toUpperCase() !== "ALL") {
                        selectedRegional = selectedValue;
                    }
                }

                document.getElementById("<%= lblRegion.ClientID %>").textContent = selectedRegional || "All Regional";

                var formattedDate = formatDateDisplay(dateFrom ? dateFrom.value : "", dateTo ? dateTo.value : "");
                var dateContext = document.getElementById("dateContextLabel");
                if (formattedDate) {
                    document.getElementById("activeDate").textContent = formattedDate;
                    dateContext.classList.remove("dashboard-hidden");
                } else {
                    document.getElementById("activeDate").textContent = "";
                    dateContext.classList.add("dashboard-hidden");
                }
            }

            function isRegionalAllSelected() {
                var regionalSelect = document.getElementById("<%= CmbSupportAreaID.ClientID %>");
                if (!regionalSelect || regionalSelect.selectedIndex < 0) {
                    return true;
                }

                var selectedText = (regionalSelect.options[regionalSelect.selectedIndex].text || "").toUpperCase();
                var selectedValue = (regionalSelect.options[regionalSelect.selectedIndex].value || "").toUpperCase();

                return selectedText === "ALL" || selectedValue === "ALL" || selectedText === "[SELECT]" || selectedValue === "[SELECT]" || selectedValue === "";
            }

            function sortRegionalRows() {
                var list = document.getElementById("regionalList");
                if (!list) {
                    return;
                }

                var rows = Array.prototype.slice.call(list.querySelectorAll(".regional-row"));
                rows.sort(function (a, b) {
                    var badgeA = a.querySelector(".dashboard-badge");
                    var badgeB = b.querySelector(".dashboard-badge");
                    return parseCount(badgeB ? badgeB.textContent : "0") - parseCount(badgeA ? badgeA.textContent : "0");
                });

                for (var i = 0; i < rows.length; i++) {
                    list.appendChild(rows[i]);
                }
            }

            function toggleRegionalSectionByFilter() {
                var regionalSection = document.getElementById("<%= pnlRegion.ClientID %>");
                if (!regionalSection) {
                    return;
                }

                if (isRegionalAllSelected()) {
                    regionalSection.classList.remove("dashboard-hidden");
                } else {
                    regionalSection.classList.add("dashboard-hidden");
                }
            }

            function refreshRegionalEmptyState() {
                var list = document.getElementById("regionalList");
                var emptyState = document.getElementById("regionalEmptyState");
                if (!list || !emptyState) {
                    return;
                }

                var rows = Array.prototype.slice.call(list.querySelectorAll(".regional-row"));
                var hasPositiveValue = rows.some(function (row) {
                    var badge = row.querySelector(".dashboard-badge");
                    return parseCount(badge ? badge.textContent : "0") > 0;
                });

                if (!rows.length || !hasPositiveValue) {
                    list.classList.add("dashboard-hidden");
                    emptyState.style.display = "block";
                } else {
                    list.classList.remove("dashboard-hidden");
                    emptyState.style.display = "none";
                }
            }

            function goToInstall(status) {
                var regionalSelect = document.getElementById("<%= CmbSupportAreaID.ClientID %>");
                var dateFrom = document.getElementById("<%= txtDateFrom.ClientID %>");
                var dateTo = document.getElementById("<%= txtDateTo.ClientID %>");

                var statusValue = (status || "").toLowerCase();
                var regionValue = regionalSelect ? (regionalSelect.value || "") : "";
                var regionNameValue = "";
                if (regionalSelect && regionalSelect.selectedIndex >= 0) {
                    regionNameValue = regionalSelect.options[regionalSelect.selectedIndex].text || "";
                    if (regionNameValue.toUpperCase() === "[SELECT]" || regionNameValue.toUpperCase() === "ALL") {
                        regionNameValue = "";
                    }
                }
                var fromValue = dateFrom ? (dateFrom.value || "") : "";
                var toValue = dateTo ? (dateTo.value || "") : "";

                showOverlay();
                window.location.href = "dashboard_job_list_installation.aspx?status=" + encodeURIComponent(statusValue)
                    + "&region=" + encodeURIComponent(regionValue)
                    + "&regionName=" + encodeURIComponent(regionNameValue)
                    + "&from=" + encodeURIComponent(fromValue)
                    + "&to=" + encodeURIComponent(toValue);
            }

            function goToMaintenance(status) {
                var regionalSelect = document.getElementById("<%= CmbSupportAreaID.ClientID %>");
                var dateFrom = document.getElementById("<%= txtDateFrom.ClientID %>");
                var dateTo = document.getElementById("<%= txtDateTo.ClientID %>");

                var statusValue = (status || "").toLowerCase();
                var regionValue = regionalSelect ? (regionalSelect.value || "") : "";
                var regionNameValue = "";
                if (regionalSelect && regionalSelect.selectedIndex >= 0) {
                    regionNameValue = regionalSelect.options[regionalSelect.selectedIndex].text || "";
                    if (regionNameValue.toUpperCase() === "[SELECT]" || regionNameValue.toUpperCase() === "ALL") {
                        regionNameValue = "";
                    }
                }
                var fromValue = dateFrom ? (dateFrom.value || "") : "";
                var toValue = dateTo ? (dateTo.value || "") : "";

                showOverlay();
                window.location.href = "dashboard_job_list_maintenance.aspx?status=" + encodeURIComponent(statusValue)
                    + "&region=" + encodeURIComponent(regionValue)
                    + "&regionName=" + encodeURIComponent(regionNameValue)
                    + "&from=" + encodeURIComponent(fromValue)
                    + "&to=" + encodeURIComponent(toValue);
            }

            function bindLoadingOnLinks() {
                var links = document.querySelectorAll(".js-show-loading");
                for (var i = 0; i < links.length; i++) {
                    links[i].addEventListener("click", function () {
                        showOverlay();
                    });
                }
            }

            function initDashboardInteractions() {
                syncFilterContext();
                sortRegionalRows();
                toggleRegionalSectionByFilter();
                refreshRegionalEmptyState();
                bindLoadingOnLinks();
            }

            window.goToInstall = goToInstall;
            window.goToMaintenance = goToMaintenance;
            document.addEventListener("DOMContentLoaded", initDashboardInteractions);
        })();
    </script>
</asp:Content>
