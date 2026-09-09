<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_qc.aspx.cs" Inherits="vtsadm.device_qc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>
            Device Quality Control
            <small>Container Tab QC Device</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Quality Control</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">Device QC</li>
        </ol>
    </section>

    <section class="content device-qc-page">
        <div class="box box-solid device-qc-box">
            <div class="box-header with-border">
                <h3 class="box-title"><i class="fa fa-clone"></i> QC Workspace</h3>
            </div>

            <div class="box-body device-qc-body">
                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
                    <asp:Literal ID="litError" runat="server"></asp:Literal>
                </asp:Panel>

                <div class="qc-tabs" role="tablist" aria-label="Device QC Tabs">
                    <button type="button"
                        class="qc-tab is-active"
                        id="qc-tab-new"
                        data-tab="new"
                        data-target="qc-frame-new"
                        data-src="device_qc_new.aspx?embed=1"
                        role="tab"
                        aria-selected="true"
                        aria-controls="qc-frame-new">
                        QC New Device
                    </button>
                    <button type="button"
                        class="qc-tab"
                        id="qc-tab-returned"
                        data-tab="returned"
                        data-target="qc-frame-returned"
                        data-src="device_qc_returned.aspx?embed=1"
                        role="tab"
                        aria-selected="false"
                        aria-controls="qc-frame-returned">
                        QC Return Device
                    </button>
                </div>

                <div class="qc-dashboard is-active" id="qc-dashboard-new">
                    <div class="dashboard-collapse-container">
                        <button type="button" class="dashboard-collapse-header" id="dashboard-header-new" onclick="toggleDashboard('new');" aria-expanded="false">
                            <span>Dashboard Summary - QC New Device</span>
                            <i class="dashboard-toggle-icon fas fa-chevron-down" id="dashboard-icon-new"></i>
                        </button>
                        <div class="dashboard-collapse-content collapsed" id="dashboard-content-new">
                            <div class="row">
                                <asp:Repeater ID="rptNewSummary" runat="server" OnItemCommand="rptNewSummary_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col-md-3 col-sm-6">
                                            <asp:LinkButton ID="lnkFilterNew"
                                                runat="server"
                                                CssClass="qc-summary-link"
                                                CommandName="FilterNew"
                                                CommandArgument='<%# Eval("DeviceTypeID") %>'
                                                CausesValidation="false">
                                                <div class="small-box bg-aqua qc-summary-card"
                                                    data-devicetypeid="<%# Eval("DeviceTypeID") %>">
                                                    <div class="inner">
                                                        <h3><%# Eval("TotalNotQc") %></h3>
                                                        <p><%# Eval("DeviceTypeDesc") %></p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-microchip"></i>
                                                    </div>
                                                </div>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="qc-dashboard" id="qc-dashboard-returned">
                    <div class="dashboard-collapse-container">
                        <button type="button" class="dashboard-collapse-header" id="dashboard-header-returned" onclick="toggleDashboard('returned');" aria-expanded="false">
                            <span>Dashboard Summary - QC Return Device</span>
                            <i class="dashboard-toggle-icon fas fa-chevron-down" id="dashboard-icon-returned"></i>
                        </button>
                        <div class="dashboard-collapse-content collapsed" id="dashboard-content-returned">
                            <div class="row">
                                <asp:Repeater ID="rptReturnedSummary" runat="server" OnItemCommand="rptReturnedSummary_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col-md-3 col-sm-6">
                                            <asp:LinkButton ID="lnkFilterReturnedByType"
                                                runat="server"
                                                CssClass="qc-summary-link"
                                                CommandName="FilterByType"
                                                CommandArgument='<%# Eval("DeviceTypeID") %>'
                                                CausesValidation="false">
                                                <div class="small-box bg-yellow qc-summary-card" data-devicetypeid="<%# Eval("DeviceTypeID") %>">
                                                    <div class="inner">
                                                        <h3><%# Eval("TotalNotQc") %></h3>
                                                        <p><%# Eval("DeviceTypeDesc") %></p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-refresh"></i>
                                                    </div>
                                                </div>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="qc-frame-panels" id="qc-frame-panels">
                    <div class="qc-frame-loading" id="qc-frame-loading" style="display: none;">
                        <i class="fa fa-spinner fa-spin"></i> Loading data...
                    </div>
                    <iframe id="qc-frame-new"
                        class="qc-frame is-active"
                        title="QC New Device"
                        data-loaded="false"
                        loading="eager">
                    </iframe>
                    <iframe id="qc-frame-returned"
                        class="qc-frame"
                        title="QC Return Device"
                        data-loaded="false"
                        loading="lazy">
                    </iframe>
                </div>
            </div>
        </div>
    </section>

    <style>
        .device-qc-page,
        .device-qc-box,
        .device-qc-body {
            height: 100%;
        }

        .device-qc-box {
            margin-bottom: 0;
            border-radius: 0;
        }

        .device-qc-body {
            display: flex;
            flex-direction: column;
            padding: 10px;
            gap: 10px;
        }

        .qc-tabs {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            border-bottom: 1px solid #d2d6de;
            padding-bottom: 8px;
        }

        .qc-tab {
            border: 1px solid #d2d6de;
            background-color: #f4f4f4;
            color: #444;
            padding: 10px 16px;
            font-size: 14px;
            font-weight: 600;
            border-radius: 0;
            cursor: pointer;
            transition: background-color 0.2s ease, color 0.2s ease, border-color 0.2s ease;
        }

        .qc-tab:hover {
            background-color: #e7e7e7;
        }

        .qc-tab.is-active {
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: #fff;
        }

        .qc-frame-panels {
            width: 100%;
            min-height: 420px;
            border: 1px solid #d2d6de;
            background-color: #fff;
            padding: 0;
            position: relative;
        }

        .qc-dashboard {
            display: none;
            margin-bottom: 0;
        }

        .qc-dashboard.is-active {
            display: block;
        }

        .dashboard-collapse-container {
            border: 1px solid #d2d6de;
            background-color: #fff;
        }

        .dashboard-collapse-header {
            width: 100%;
            border: 0;
            border-bottom: 1px solid #d2d6de;
            background-color: #f7f7f7;
            color: #444;
            text-align: left;
            padding: 10px 14px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            font-weight: 600;
            cursor: pointer;
        }

        .dashboard-toggle-icon {
            font-size: 16px;
            line-height: 1;
            transition: transform 0.2s ease;
        }

        .dashboard-toggle-icon.is-expanded {
            transform: rotate(180deg);
        }

        .dashboard-collapse-content {
            overflow: hidden;
            transition: max-height 0.25s ease;
        }

        .dashboard-collapse-content.collapsed {
            max-height: 0;
        }

        .dashboard-collapse-content.expanded {
            max-height: 3000px;
            padding: 10px 10px 0 10px;
        }

        .qc-frame {
            width: 100%;
            min-height: 100vh;
            border: 0;
            display: none;
            background-color: #fff;
        }

        .qc-frame.is-active {
            display: block;
        }

        .qc-summary-card {
            cursor: pointer;
            transition: transform 0.15s ease, box-shadow 0.15s ease;
        }

        .qc-summary-link,
        .qc-summary-link:hover,
        .qc-summary-link:focus {
            color: inherit;
            text-decoration: none;
            display: block;
        }

        .qc-summary-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 14px rgba(0, 0, 0, 0.2);
        }

        .qc-summary-card.is-selected {
            outline: 3px solid #fff;
            box-shadow: 0 0 0 3px #367fa9;
        }

        .qc-frame-loading {
            position: absolute;
            top: 12px;
            right: 12px;
            z-index: 2;
            background-color: rgba(255, 255, 255, 0.95);
            border: 1px solid #d2d6de;
            padding: 6px 10px;
            font-size: 12px;
            color: #367fa9;
        }
    </style>

    <script type="text/javascript">
        (function () {
            var STORAGE_KEY = "device_qc_active_tab";
            var DASHBOARD_STATE_KEY_PREFIX = "device_qc_dashboard_";
            var tabs = [];
            var frames = [];
            var activeTabName = "new";
            var activeNewDeviceTypeId = null;
            var activeReturnedDeviceTypeId = null;

            function getTabs() {
                tabs = Array.prototype.slice.call(document.querySelectorAll(".qc-tab"));
                frames = Array.prototype.slice.call(document.querySelectorAll(".qc-frame"));
            }

            function getFrameHeight(frame) {
                try {
                    var doc = frame.contentDocument || frame.contentWindow.document;
                    if (!doc) {
                        return 0;
                    }

                    var body = doc.body;
                    var html = doc.documentElement;
                    return Math.max(
                        body ? body.scrollHeight : 0,
                        body ? body.offsetHeight : 0,
                        html ? html.clientHeight : 0,
                        html ? html.scrollHeight : 0,
                        html ? html.offsetHeight : 0
                    );
                } catch (error) {
                    return 0;
                }
            }

            function applyFrameHeight(frame) {
                if (!frame) {
                    return;
                }

                var calculatedHeight = getFrameHeight(frame);
                if (calculatedHeight > 0) {
                    frame.style.height = calculatedHeight + "px";
                } else {
                    frame.style.height = "100vh";
                }
            }

            function resizeActiveFrame() {
                for (var i = 0; i < frames.length; i++) {
                    if (frames[i].classList.contains("is-active")) {
                        applyFrameHeight(frames[i]);
                        return;
                    }
                }
            }

            function ensureFrameLoaded(tabElement) {
                var frameId = tabElement.getAttribute("data-target");
                var src = tabElement.getAttribute("data-src");
                var frame = document.getElementById(frameId);

                if (!frame || !src) {
                    return;
                }

                if (tabElement.getAttribute("data-tab") === "new" && activeNewDeviceTypeId) {
                    src = buildNewFrameSrc(activeNewDeviceTypeId);
                }
                if (tabElement.getAttribute("data-tab") === "returned" && activeReturnedDeviceTypeId) {
                    src = buildReturnedFrameSrc(activeReturnedDeviceTypeId);
                }

                if (frame.getAttribute("data-loaded") !== "true") {
                    frame.addEventListener("load", function () {
                        applyFrameHeight(frame);
                        setFrameLoading(false);
                    });
                    frame.setAttribute("src", src);
                    frame.setAttribute("data-loaded", "true");
                }
            }

            function buildNewFrameSrc(deviceTypeId) {
                var baseSrc = "device_qc_new.aspx?embed=1";
                if (!deviceTypeId) {
                    return baseSrc;
                }

                return baseSrc + "&devicetypeid=" + encodeURIComponent(deviceTypeId);
            }

            function setFrameLoading(isLoading) {
                var loadingEl = document.getElementById("qc-frame-loading");
                if (!loadingEl) {
                    return;
                }

                loadingEl.style.display = isLoading ? "block" : "none";
            }

            function setSelectedSummaryCard(selectedCard) {
                var cards = document.querySelectorAll("#qc-dashboard-new .qc-summary-card");
                for (var i = 0; i < cards.length; i++) {
                    cards[i].classList.remove("is-selected");
                }

                if (selectedCard) {
                    selectedCard.classList.add("is-selected");
                }
            }

            function setSelectedReturnedSummaryCard(deviceTypeId) {
                var cards = document.querySelectorAll("#qc-dashboard-returned .qc-summary-card");
                for (var i = 0; i < cards.length; i++) {
                    cards[i].classList.remove("is-selected");

                    if (deviceTypeId && cards[i].getAttribute("data-devicetypeid") === deviceTypeId) {
                        cards[i].classList.add("is-selected");
                    }
                }
            }

            function loadByDeviceType(deviceTypeId, sourceCard) {
                if (!deviceTypeId) {
                    return;
                }

                var frame = document.getElementById("qc-frame-new");
                if (!frame) {
                    return;
                }

                activeNewDeviceTypeId = deviceTypeId;
                setSelectedSummaryCard(sourceCard || null);
                setFrameLoading(true);
                frame.setAttribute("src", buildNewFrameSrc(deviceTypeId));
                frame.setAttribute("data-loaded", "true");
            }

            function buildReturnedFrameSrc(deviceTypeId) {
                var baseSrc = "device_qc_returned.aspx?embed=1";
                if (!deviceTypeId) {
                    return baseSrc;
                }

                return baseSrc + "&DeviceTypeID=" + encodeURIComponent(deviceTypeId);
            }

            function openReturnedByDeviceType(deviceTypeId) {
                var frame = document.getElementById("qc-frame-returned");
                if (!frame) {
                    return;
                }

                activeReturnedDeviceTypeId = deviceTypeId || null;
                setFrameLoading(true);
                frame.setAttribute("src", buildReturnedFrameSrc(deviceTypeId));
                frame.setAttribute("data-loaded", "true");
                activateTab("returned", true);
                setSelectedReturnedSummaryCard(deviceTypeId);
            }

            function activateDashboard(tabName) {
                var dashboards = document.querySelectorAll(".qc-dashboard");
                for (var i = 0; i < dashboards.length; i++) {
                    dashboards[i].classList.remove("is-active");
                }

                var dashboard = document.getElementById("qc-dashboard-" + tabName);
                if (dashboard) {
                    dashboard.classList.add("is-active");
                }

                applyDashboardState(tabName, getDashboardExpandedState(tabName));
            }

            function getDashboardExpandedState(tabName) {
                try {
                    return localStorage.getItem(DASHBOARD_STATE_KEY_PREFIX + tabName) === "expanded";
                } catch (error) {
                    return false;
                }
            }

            function setDashboardExpandedState(tabName, isExpanded) {
                try {
                    localStorage.setItem(DASHBOARD_STATE_KEY_PREFIX + tabName, isExpanded ? "expanded" : "collapsed");
                } catch (error) {
                }
            }

            function applyDashboardState(tabName, isExpanded) {
                var content = document.getElementById("dashboard-content-" + tabName);
                var icon = document.getElementById("dashboard-icon-" + tabName);
                var header = document.getElementById("dashboard-header-" + tabName);

                if (!content || !icon || !header) {
                    return;
                }

                if (isExpanded) {
                    content.classList.remove("collapsed");
                    content.classList.add("expanded");
                    icon.classList.remove("fa-chevron-down");
                    icon.classList.add("fa-chevron-up");
                    icon.classList.add("is-expanded");
                    header.setAttribute("aria-expanded", "true");
                } else {
                    content.classList.remove("expanded");
                    content.classList.add("collapsed");
                    icon.classList.remove("fa-chevron-up");
                    icon.classList.remove("is-expanded");
                    icon.classList.add("fa-chevron-down");
                    header.setAttribute("aria-expanded", "false");
                }
            }

            function toggleDashboard(tabName) {
                var content = document.getElementById("dashboard-content-" + tabName);
                if (!content) {
                    return;
                }

                var willExpand = content.classList.contains("collapsed");
                applyDashboardState(tabName, willExpand);
                setDashboardExpandedState(tabName, willExpand);
            }

            function activateTab(tabName, shouldPersist) {
                var selectedTab = null;
                activeTabName = tabName;

                tabs.forEach(function (tab) {
                    var isTarget = tab.getAttribute("data-tab") === tabName;
                    tab.classList.toggle("is-active", isTarget);
                    tab.setAttribute("aria-selected", isTarget ? "true" : "false");

                    if (isTarget) {
                        selectedTab = tab;
                    }
                });

                frames.forEach(function (frame) {
                    var isTargetFrame = selectedTab && frame.id === selectedTab.getAttribute("data-target");
                    frame.classList.toggle("is-active", !!isTargetFrame);
                });

                if (selectedTab) {
                    ensureFrameLoaded(selectedTab);
                    activateDashboard(tabName);
                    resizeActiveFrame();

                    if (shouldPersist) {
                        try {
                            localStorage.setItem(STORAGE_KEY, tabName);
                        } catch (error) {
                        }
                    }
                }
            }

            function bindEvents() {
                tabs.forEach(function (tab) {
                    tab.addEventListener("click", function () {
                        var tabName = tab.getAttribute("data-tab");
                        activateTab(tabName, true);
                    });
                });

                window.addEventListener("resize", function () {
                    resizeActiveFrame();
                });

                window.setInterval(function () {
                    if (activeTabName) {
                        resizeActiveFrame();
                    }
                }, 1200);
            }

            function init() {
                getTabs();
                bindEvents();

                var defaultTab = "new";
                var savedTab = null;

                try {
                    savedTab = localStorage.getItem(STORAGE_KEY);
                } catch (error) {
                }

                var targetTab = savedTab === "returned" ? "returned" : defaultTab;
                applyDashboardState("new", getDashboardExpandedState("new"));
                applyDashboardState("returned", getDashboardExpandedState("returned"));
                activateTab(targetTab, false);
            }

            window.toggleDashboard = toggleDashboard;
            window.loadByDeviceType = loadByDeviceType;
            window.openReturnedByDeviceType = openReturnedByDeviceType;

            if (document.readyState === "loading") {
                document.addEventListener("DOMContentLoaded", init);
            } else {
                init();
            }
        })();
    </script>
</asp:Content>
