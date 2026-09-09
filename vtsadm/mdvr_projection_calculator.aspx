<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mdvr_projection_calculator.aspx.cs" Inherits="vtsadm.mdvr_projection_calculator" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .mdvr-page { --mdvr-primary: #003481; --mdvr-accent: #3c8dbc; --mdvr-border: #e3eaf3; }
        .mdvr-page .content-header h1 { font-weight: 700; color: var(--mdvr-primary); margin-bottom: 4px; }
        .mdvr-page .content-header h1 small { color: #7a8da3; font-weight: 500; }
        .mdvr-page .box { border: 0; border-radius: 12px; box-shadow: 0 8px 24px rgba(0, 52, 129, .08); margin-bottom: 20px; overflow: hidden; }
        .mdvr-page .box-header { background: linear-gradient(135deg, #003481 0%, #2f6fad 100%); color: #fff; border: 0; padding: 14px 18px; }
        .mdvr-page .box-header .box-title { font-size: 15px; font-weight: 700; }
        .mdvr-page .box-header .box-title i { margin-right: 8px; }
        .mdvr-page .box-body { padding: 20px; background: #fff; }
        .mdvr-page label.mdvr-label { font-size: 11px; font-weight: 700; color: #64748b; text-transform: uppercase; letter-spacing: .04em; margin-bottom: 6px; display: block; }
        .mdvr-page .form-control { border-radius: 8px; border-color: #d7e0ea; min-height: 40px; box-shadow: none; }
        .mdvr-page .form-control:focus { border-color: var(--mdvr-accent); box-shadow: 0 0 0 3px rgba(60, 141, 188, .15); }
        .mdvr-filter-card { background: linear-gradient(180deg, #fbfdff 0%, #f4f8fc 100%); border: 1px solid var(--mdvr-border); border-radius: 12px; padding: 18px; }
        .mdvr-btn-calc { border: 0; border-radius: 10px; font-weight: 700; padding: 10px 18px; background: linear-gradient(135deg, #2563eb 0%, #003481 100%); min-height: 40px; width: 100%; }
        .mdvr-btn-calc:hover { box-shadow: 0 6px 16px rgba(0, 52, 129, .25); }
        .mdvr-info-chips { display: flex; flex-wrap: wrap; gap: 10px; margin-bottom: 18px; }
        .mdvr-chip { display: inline-flex; align-items: center; gap: 8px; background: #fff; border: 1px solid var(--mdvr-border); border-radius: 999px; padding: 8px 14px; font-size: 12px; color: #334155; box-shadow: 0 2px 8px rgba(0,0,0,.04); }
        .mdvr-chip i { color: var(--mdvr-accent); }
        .mdvr-chip strong { color: var(--mdvr-primary); font-weight: 700; }
        .mdvr-stat-grid { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: 14px; margin-bottom: 16px; }
        .mdvr-stat-card { border-radius: 12px; padding: 16px; color: #fff; position: relative; overflow: hidden; box-shadow: 0 6px 18px rgba(0,0,0,.08); }
        .mdvr-stat-card:before { content: ''; position: absolute; right: -20px; top: -20px; width: 80px; height: 80px; border-radius: 50%; background: rgba(255,255,255,.12); }
        .mdvr-stat-label { font-size: 11px; text-transform: uppercase; letter-spacing: .05em; opacity: .9; font-weight: 600; }
        .mdvr-stat-value { font-size: 22px; font-weight: 800; margin-top: 6px; line-height: 1.2; }
        .mdvr-stat-hpp { background: linear-gradient(135deg, #6366f1 0%, #4f46e5 100%); }
        .mdvr-stat-selling { background: linear-gradient(135deg, #0ea5e9 0%, #0284c7 100%); }
        .mdvr-stat-margin { background: linear-gradient(135deg, #10b981 0%, #059669 100%); }
        .mdvr-stat-marginpct { background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); }
        .mdvr-status-bar { display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 12px; padding: 14px 16px; border-radius: 10px; background: #f8fafc; border: 1px solid var(--mdvr-border); margin-bottom: 16px; }
        .mdvr-status-label { font-size: 12px; font-weight: 700; color: #64748b; text-transform: uppercase; letter-spacing: .04em; }
        .mdvr-badge-ready { background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: #fff; font-size: 13px; font-weight: 700; padding: 8px 16px; border-radius: 999px; display: inline-flex; align-items: center; gap: 6px; box-shadow: 0 4px 12px rgba(16, 185, 129, .3); }
        .mdvr-badge-not-ready { background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); color: #fff; font-size: 13px; font-weight: 700; padding: 8px 16px; border-radius: 999px; display: inline-flex; align-items: center; gap: 6px; box-shadow: 0 4px 12px rgba(239, 68, 68, .3); }
        .mdvr-table-wrap { border: 1px solid var(--mdvr-border); border-radius: 10px; overflow: hidden; }
        .mdvr-page .table { margin-bottom: 0; font-size: 12px; }
        .mdvr-page .table > thead > tr > th { background: #f1f5f9; color: var(--mdvr-primary); font-weight: 700; text-transform: uppercase; font-size: 11px; letter-spacing: .03em; border-bottom: 2px solid #dbe5f0; white-space: nowrap; }
        .mdvr-page .table > tbody > tr > td { vertical-align: middle !important; }
        .mdvr-page .table > tbody > tr:hover > td { background: #f8fbff; }
        .row-stock-shortage > td { background: #fff1f2 !important; }
        .row-stock-shortage:hover > td { background: #ffe4e6 !important; }
        .qty-shortage { font-weight: 800; color: #dc2626; }
        .mdvr-pill { display: inline-block; padding: 4px 10px; border-radius: 999px; font-size: 11px; font-weight: 700; }
        .mdvr-pill-ok { background: #dcfce7; color: #166534; }
        .mdvr-pill-bad { background: #fee2e2; color: #991b1b; }
        .mdvr-chip-ref { background: #f8fafc; border-style: dashed; color: #64748b; }
        .mdvr-alert-danger { border: 0; border-radius: 10px; padding: 12px 14px; margin-bottom: 16px; background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%); color: #991b1b; box-shadow: 0 4px 14px rgba(0,0,0,.06); }
        .mdvr-page-loader { display: none; position: fixed; inset: 0; z-index: 99999; background: rgba(15, 23, 42, .45); align-items: center; justify-content: center; }
        .mdvr-page-loader.is-visible { display: flex; }
        .mdvr-page-loader-box { background: #fff; border-radius: 12px; padding: 28px 36px; text-align: center; box-shadow: 0 20px 50px rgba(0, 0, 0, .2); min-width: 280px; max-width: 90vw; }
        .mdvr-page-loader-spinner { width: 44px; height: 44px; margin: 0 auto 16px; border: 4px solid #e2e8f0; border-top-color: #003481; border-radius: 50%; animation: mdvrSpin .8s linear infinite; }
        .mdvr-page-loader-text { margin: 0; font-size: 14px; font-weight: 600; color: #334155; }
        .mdvr-btn-disabled { opacity: .65; pointer-events: none; cursor: not-allowed; }
        @keyframes mdvrSpin { to { transform: rotate(360deg); } }
        @media (max-width: 991px) {
            .mdvr-stat-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
        }
        @media (max-width: 575px) {
            .mdvr-stat-grid { grid-template-columns: 1fr; }
        }
    </style>

    <div id="mdvrPageLoader" class="mdvr-page-loader" aria-hidden="true" aria-live="polite">
        <div class="mdvr-page-loader-box">
            <div class="mdvr-page-loader-spinner"></div>
            <p class="mdvr-page-loader-text">Sedang menghitung proyeksi MDVR...</p>
        </div>
    </div>

    <div class="mdvr-page">
        <section class="content-header">
            <h1>Kalkulator Proyeksi MDVR <small>Simulasi kebutuhan stock gudang</small></h1>
            <ol class="breadcrumb">
                <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
                <li><a href="#">Proyeksi MDVR</a></li>
                <li class="active">Kalkulator</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title"><i class="fa fa-sliders"></i> Parameter Proyeksi</h3>
                        </div>
                        <div class="box-body">
                            <div id="div_comment" runat="server"></div>

                            <div class="mdvr-filter-card">
                                <div class="row">
                                    <div class="col-md-3 col-sm-6">
                                        <div class="form-group">
                                            <label class="mdvr-label">Device MDVR</label>
                                            <asp:DropDownList ID="ddlDeviceMdvr" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDeviceMdvr_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-6">
                                        <div class="form-group">
                                            <label class="mdvr-label">Paket</label>
                                            <asp:DropDownList ID="ddlPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-6">
                                        <div class="form-group">
                                            <label class="mdvr-label">Qty Unit</label>
                                            <asp:TextBox ID="txtProjectionQty" runat="server" CssClass="form-control" TextMode="Number" Text="1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6">
                                        <div class="form-group">
                                            <label class="mdvr-label">&nbsp;</label>
                                            <asp:Button ID="btnCalculate" runat="server" CssClass="btn btn-primary mdvr-btn-calc" Text="Hitung Proyeksi" OnClick="btnCalculate_Click"
                                                OnClientClick="return mdvrPreparePostback(this, 'Menghitung...');" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Panel ID="pnlResult" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title"><i class="fa fa-bar-chart"></i> Hasil Proyeksi</h3>
                            </div>
                            <div class="box-body">
                                <div class="mdvr-info-chips">
                                    <span class="mdvr-chip"><i class="fa fa-hdd-o"></i> <strong>Device:</strong> <asp:Literal ID="litDeviceMdvr" runat="server" /></span>
                                    <span class="mdvr-chip"><i class="fa fa-cube"></i> <strong>Paket:</strong> <asp:Literal ID="litPackageName" runat="server" /></span>
                                    <span class="mdvr-chip"><i class="fa fa-calculator"></i> <strong>Qty Unit:</strong> <asp:Literal ID="litProjectionQty" runat="server" /></span>
                                    <asp:Panel ID="pnlHeaderPriceRef" runat="server" Visible="false" CssClass="mdvr-chip mdvr-chip-ref">
                                        <i class="fa fa-info-circle"></i> <strong>Ref. harga header:</strong> <asp:Literal ID="litHeaderPriceRef" runat="server" />
                                    </asp:Panel>
                                </div>

                                <div class="mdvr-stat-grid">
                                    <div class="mdvr-stat-card mdvr-stat-hpp">
                                        <div class="mdvr-stat-label">Total HPP BOM</div>
                                        <div class="mdvr-stat-value"><asp:Literal ID="litTotalHpp" runat="server" /></div>
                                    </div>
                                    <div class="mdvr-stat-card mdvr-stat-selling">
                                        <div class="mdvr-stat-label">Total Selling BOM</div>
                                        <div class="mdvr-stat-value"><asp:Literal ID="litTotalSelling" runat="server" /></div>
                                    </div>
                                    <div class="mdvr-stat-card mdvr-stat-margin">
                                        <div class="mdvr-stat-label">Gross Margin</div>
                                        <div class="mdvr-stat-value"><asp:Literal ID="litGrossMargin" runat="server" /></div>
                                    </div>
                                    <div class="mdvr-stat-card mdvr-stat-marginpct">
                                        <div class="mdvr-stat-label">Margin %</div>
                                        <div class="mdvr-stat-value"><asp:Literal ID="litGrossMarginPct" runat="server" /></div>
                                    </div>
                                </div>

                                <p style="margin:0 0 14px;font-size:12px;color:#64748b;">
                                    Harga dihitung dari total detail BOM sesuai kebutuhan proyeksi.
                                </p>

                                <div class="mdvr-status-bar">
                                    <div>
                                        <span class="mdvr-status-label">Overall Status</span>
                                        <div style="font-size:12px;color:#64748b;margin-top:4px;">
                                            Stock: <asp:Literal ID="litStockSummary" runat="server" />
                                            — READY jika semua item BOM CUKUP
                                        </div>
                                    </div>
                                    <asp:Literal ID="litOverallStatus" runat="server" />
                                </div>

                                <div class="mdvr-table-wrap">
                                    <asp:GridView ID="gvProjectionDetail" runat="server"
                                        CssClass="table table-hover"
                                        AutoGenerateColumns="False" EmptyDataText="Tidak ada data proyeksi"
                                        OnRowDataBound="gvProjectionDetail_RowDataBound" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="DetailDeviceTypeDesc" HeaderText="Item" />
                                            <asp:BoundField DataField="QtyPerUnit" HeaderText="Qty/Unit" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="TotalQtyNeed" HeaderText="Kebutuhan" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="QtyAvailable" HeaderText="Stock" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:TemplateField HeaderText="Kurang" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyShortage" runat="server"
                                                        Text='<%# Eval("QtyShortage", "{0:N0}") %>'
                                                        CssClass='<%# Convert.ToDecimal(Eval("QtyShortage")) > 0 ? "qty-shortage" : "" %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockStatus" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="HppPrice" HeaderText="HPP" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="TotalHppPrice" HeaderText="Tot. HPP" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="SellingPrice" HeaderText="Selling" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="TotalSellingPrice" HeaderText="Tot. Selling" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </section>
    </div>

    <script type="text/javascript">
        var mdvrPageLoaderDefaultMessage = 'Sedang menghitung proyeksi MDVR...';

        function showPageLoader(button, loadingText, overlayMessage, skipValidate) {
            if (!skipValidate && typeof Page_ClientValidate === 'function' && !Page_ClientValidate()) {
                return false;
            }

            var overlay = document.getElementById('mdvrPageLoader');
            if (overlay) {
                var textEl = overlay.querySelector('.mdvr-page-loader-text');
                if (textEl) {
                    textEl.textContent = overlayMessage || mdvrPageLoaderDefaultMessage;
                }
                overlay.className = 'mdvr-page-loader is-visible';
                overlay.setAttribute('aria-hidden', 'false');
            }

            if (button) {
                if (button.tagName === 'INPUT') {
                    button.setAttribute('data-mdvr-original-text', button.value);
                } else {
                    button.setAttribute('data-mdvr-original-html', button.innerHTML);
                }

                setTimeout(function () {
                    if (button.tagName === 'INPUT') {
                        if (loadingText) {
                            button.value = loadingText;
                        }
                        button.disabled = true;
                    } else if (button.tagName === 'BUTTON') {
                        if (loadingText) {
                            button.textContent = loadingText;
                        }
                        button.disabled = true;
                    } else {
                        button.className = (button.className + ' mdvr-btn-disabled').replace(/\s+/g, ' ').trim();
                        if (loadingText) {
                            button.innerHTML = loadingText;
                        }
                    }
                }, 0);
            }

            return true;
        }

        function mdvrPreparePostback(button, loadingText, skipValidate) {
            if (!skipValidate && typeof Page_ClientValidate === 'function' && !Page_ClientValidate()) {
                return false;
            }
            return showPageLoader(button, loadingText, null, !!skipValidate);
        }
    </script>
</asp:Content>
