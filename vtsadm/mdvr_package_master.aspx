<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mdvr_package_master.aspx.cs" Inherits="vtsadm.mdvr_package_master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .mdvr-page { --mdvr-primary: #003481; --mdvr-accent: #3c8dbc; --mdvr-soft: #f4f8fc; --mdvr-border: #e3eaf3; }
        .mdvr-page .content-header h1 { font-weight: 700; color: var(--mdvr-primary); margin-bottom: 4px; }
        .mdvr-page .content-header h1 small { color: #7a8da3; font-weight: 500; }
        .mdvr-page .box { border: 0; border-radius: 12px; box-shadow: 0 8px 24px rgba(0, 52, 129, .08); margin-bottom: 20px; overflow: hidden; }
        .mdvr-page .box-header { background: linear-gradient(135deg, #003481 0%, #2f6fad 100%); color: #fff; border: 0; padding: 14px 18px; }
        .mdvr-page .box-header .box-title { font-size: 15px; font-weight: 700; letter-spacing: .02em; }
        .mdvr-page .box-header .box-title i { margin-right: 8px; opacity: .9; }
        .mdvr-page .box-body { padding: 20px; background: #fff; }
        .mdvr-page .box-footer { background: #f8fafc; border-top: 1px solid var(--mdvr-border); padding: 14px 18px; }
        .mdvr-page label.mdvr-label { font-size: 11px; font-weight: 700; color: #64748b; text-transform: uppercase; letter-spacing: .04em; margin-bottom: 6px; display: block; }
        .mdvr-page .form-control { border-radius: 8px; border-color: #d7e0ea; min-height: 38px; box-shadow: none; transition: border-color .15s ease, box-shadow .15s ease; }
        .mdvr-page .form-control:focus { border-color: var(--mdvr-accent); box-shadow: 0 0 0 3px rgba(60, 141, 188, .15); }
        .mdvr-page .form-group { margin-bottom: 16px; }
        .mdvr-section { border: 1px solid var(--mdvr-border); border-radius: 10px; padding: 16px; margin-top: 8px; background: linear-gradient(180deg, #fbfdff 0%, #f7fafc 100%); }
        .mdvr-section-title { font-size: 13px; font-weight: 700; color: var(--mdvr-primary); margin: 0 0 14px; padding-bottom: 10px; border-bottom: 2px solid rgba(60, 141, 188, .25); }
        .mdvr-section-title i { color: var(--mdvr-accent); margin-right: 6px; }
        .mdvr-add-panel { background: #fff; border: 1px dashed #b8cfe6; border-radius: 10px; padding: 14px; margin-bottom: 14px; }
        .mdvr-btn { border-radius: 8px; font-weight: 600; padding: 8px 16px; transition: transform .12s ease, box-shadow .12s ease; }
        .mdvr-btn:hover { transform: translateY(-1px); box-shadow: 0 4px 12px rgba(0, 0, 0, .1); }
        .mdvr-btn-primary { background: linear-gradient(135deg, #2563eb 0%, #003481 100%); border: 0; }
        .mdvr-btn-add { background: linear-gradient(135deg, #0ea5e9 0%, #0284c7 100%); border: 0; color: #fff; }
        .mdvr-btn-add:hover, .mdvr-btn-add:focus { color: #fff; }
        .mdvr-search-wrap { display: flex; gap: 8px; align-items: center; }
        .mdvr-search-wrap .form-control { border-radius: 20px 0 0 20px; }
        .mdvr-search-btn { border-radius: 0 20px 20px 0; border: 0; background: #fff; color: var(--mdvr-primary); padding: 8px 14px; }
        .mdvr-search-btn:hover { background: #eef4fb; color: var(--mdvr-primary); }
        .mdvr-table-wrap { border: 1px solid var(--mdvr-border); border-radius: 10px; overflow: hidden; }
        .mdvr-page .table { margin-bottom: 0; font-size: 12px; }
        .mdvr-page .table > thead > tr > th { background: #f1f5f9; color: var(--mdvr-primary); font-weight: 700; text-transform: uppercase; font-size: 11px; letter-spacing: .03em; border-bottom: 2px solid #dbe5f0; white-space: nowrap; }
        .mdvr-page .table > tbody > tr > td { vertical-align: middle !important; }
        .mdvr-page .table > tbody > tr:hover > td { background: #f8fbff; }
        .mdvr-page .table > tbody > tr:nth-child(even) > td { background: #fcfdff; }
        .mdvr-action-btn { border-radius: 6px; padding: 4px 10px; font-size: 11px; font-weight: 600; margin: 0 2px; }
        .mdvr-action-edit { background: #fff7ed; color: #c2410c; border: 1px solid #fed7aa; }
        .mdvr-action-edit:hover { background: #ffedd5; color: #9a3412; }
        .mdvr-action-delete { background: #fef2f2; color: #b91c1c; border: 1px solid #fecaca; }
        .mdvr-action-delete:hover { background: #fee2e2; color: #991b1b; }
        .mdvr-alert { border: 0; border-radius: 10px; padding: 12px 14px; margin-bottom: 16px; box-shadow: 0 4px 14px rgba(0, 0, 0, .06); }
        .mdvr-alert-success { background: linear-gradient(135deg, #ecfdf5 0%, #d1fae5 100%); color: #065f46; }
        .mdvr-alert-danger { background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%); color: #991b1b; }
        .mdvr-badge-id { display: inline-block; background: #e8f1fb; color: #1e4a72; font-weight: 700; padding: 3px 8px; border-radius: 6px; font-size: 11px; }
        .mdvr-badge-channel { display: inline-block; background: #ede9fe; color: #5b21b6; font-weight: 700; padding: 3px 8px; border-radius: 6px; font-size: 11px; }
        .mdvr-page-loader { display: none; position: fixed; inset: 0; z-index: 99999; background: rgba(15, 23, 42, .45); align-items: center; justify-content: center; }
        .mdvr-page-loader.is-visible { display: flex; }
        .mdvr-page-loader-box { background: #fff; border-radius: 12px; padding: 28px 36px; text-align: center; box-shadow: 0 20px 50px rgba(0, 0, 0, .2); min-width: 280px; max-width: 90vw; }
        .mdvr-page-loader-spinner { width: 44px; height: 44px; margin: 0 auto 16px; border: 4px solid #e2e8f0; border-top-color: #003481; border-radius: 50%; animation: mdvrSpin .8s linear infinite; }
        .mdvr-page-loader-text { margin: 0; font-size: 14px; font-weight: 600; color: #334155; }
        .mdvr-btn-disabled { opacity: .65; pointer-events: none; cursor: not-allowed; }
        @keyframes mdvrSpin { to { transform: rotate(360deg); } }
        @media (max-width: 991px) {
            .mdvr-page .box-body { padding: 14px; }
            .mdvr-search-wrap { width: 100% !important; margin-top: 10px; }
        }
    </style>

    <div id="mdvrPageLoader" class="mdvr-page-loader" aria-hidden="true" aria-live="polite">
        <div class="mdvr-page-loader-box">
            <div class="mdvr-page-loader-spinner"></div>
            <p class="mdvr-page-loader-text">Sedang memproses data paket MDVR...</p>
        </div>
    </div>

    <div class="mdvr-page">
        <section class="content-header">
            <h1>Master Paket MDVR <small>Kelola BOM &amp; harga paket perangkat</small></h1>
            <ol class="breadcrumb">
                <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
                <li><a href="#">Proyeksi MDVR</a></li>
                <li class="active">Master Paket</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-lg-5">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title"><i class="fa fa-cube"></i> Form Paket</h3>
                        </div>
                        <div class="box-body">
                            <asp:HiddenField ID="hfPackageID" runat="server" />
                            <div id="div_comment" runat="server"></div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="mdvr-label">Device MDVR</label>
                                        <asp:DropDownList ID="ddlDeviceMdvr" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label class="mdvr-label">Nama Paket</label>
                                        <asp:TextBox ID="txtPackageName" runat="server" CssClass="form-control" placeholder="Contoh: Paket Standard"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="mdvr-label">HPP Price</label>
                                        <asp:TextBox ID="txtHppPrice" runat="server" CssClass="form-control" placeholder="0" Text="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="mdvr-label">Selling Price</label>
                                        <asp:TextBox ID="txtSellingPrice" runat="server" CssClass="form-control" placeholder="0" Text="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <div class="form-group" style="margin-bottom: 0;">
                                        <label class="mdvr-label">Remark</label>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Catatan tambahan (opsional)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="mdvr-section">
                                <h4 class="mdvr-section-title"><i class="fa fa-list-ul"></i> Detail Kebutuhan Item</h4>

                                <div class="mdvr-add-panel">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label class="mdvr-label">Accessory</label>
                                                <asp:DropDownList ID="ddlDetailDeviceType" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-xs-4">
                                            <div class="form-group">
                                                <label class="mdvr-label">Qty</label>
                                                <asp:TextBox ID="txtDetailQty" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-4">
                                            <div class="form-group">
                                                <label class="mdvr-label">HPP</label>
                                                <asp:TextBox ID="txtDetailHppPrice" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-4">
                                            <div class="form-group">
                                                <label class="mdvr-label">Selling</label>
                                                <asp:TextBox ID="txtDetailSellingPrice" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <asp:Button ID="btnAddDetail" runat="server" CssClass="btn mdvr-btn mdvr-btn-add btn-block" Text="  Tambah ke Daftar" OnClick="btnAddDetail_Click" CausesValidation="false"
                                                OnClientClick="return mdvrPreparePostback(this, 'Menambahkan...');" />
                                        </div>
                                    </div>
                                </div>

                                <div class="mdvr-table-wrap">
                                    <asp:GridView ID="gvDetail" runat="server" CssClass="table table-hover"
                                        AutoGenerateColumns="False" EmptyDataText="Belum ada item — tambahkan accessory di atas"
                                        OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound" DataKeyNames="Seq" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Seq" HeaderText="#" ItemStyle-Width="36px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Item" />
                                            <asp:BoundField DataField="Qty" HeaderText="Qty" DataFormatString="{0:N2}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="HppPrice" HeaderText="HPP" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:BoundField DataField="SellingPrice" HeaderText="Selling" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="72px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnRemoveDetail" runat="server" CssClass="mdvr-action-btn mdvr-action-delete btn-delete-detail"
                                                        CommandName="RemoveDetail" CommandArgument='<%# Eval("Seq") %>'
                                                        OnClientClick="return mdvrConfirmRemoveDetail(this);" CausesValidation="false"><i class="fa fa-trash"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer text-right">
                            <asp:Button ID="btnClear" CssClass="btn btn-default mdvr-btn" runat="server" Text="Reset" OnClick="btnClear_Click" CausesValidation="false" />
                            <asp:Button ID="btnSave" CssClass="btn btn-primary mdvr-btn mdvr-btn-primary" runat="server" Text="Simpan Paket" OnClick="btnSave_Click" UseSubmitBehavior="false"
                                OnClientClick="return mdvrSavePackage(this);" />
                        </div>
                    </div>
                </div>

                <div class="col-lg-7">
                    <div class="box">
                        <div class="box-header" style="display:flex; align-items:center; justify-content:space-between; flex-wrap:wrap; gap:10px;">
                            <h3 class="box-title" style="margin:0;"><i class="fa fa-th-list"></i> Daftar Paket MDVR</h3>
                            <div class="mdvr-search-wrap" style="width:260px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Cari paket atau device..."></asp:TextBox>
                                <button id="btnSearch" runat="server" type="submit" class="mdvr-search-btn" onserverclick="btnSearch_ServerClick"
                                    onclick="return mdvrPreparePostback(this, 'Memuat...');"><i class="fa fa-search"></i></button>
                            </div>
                        </div>
                        <div class="box-body" style="padding:0;">
                            <div class="mdvr-table-wrap" style="border:0; border-radius:0;">
                                <asp:GridView ID="gvPackage" runat="server" CssClass="table table-hover"
                                    AutoGenerateColumns="False" EmptyDataText="Belum ada data paket MDVR"
                                    DataKeyNames="PackageID" OnRowCommand="gvPackage_RowCommand" OnRowDataBound="gvPackage_RowDataBound" GridLines="None">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID">
                                            <ItemTemplate>
                                                <span class="mdvr-badge-id"><%# Eval("PackageID") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device MDVR" />
                                        <asp:BoundField DataField="PackageName" HeaderText="Paket" />
                                        <asp:BoundField DataField="HppPrice" HeaderText="HPP" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                        <asp:BoundField DataField="SellingPrice" HeaderText="Selling" DataFormatString="{0:N0}" HtmlEncode="false" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" />
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="mdvr-action-btn mdvr-action-edit"
                                                    CommandName="EditData" CommandArgument='<%# Eval("PackageID") %>' CausesValidation="false" ToolTip="Edit"
                                                    OnClientClick="return mdvrPreparePostback(this, 'Memuat...', true);"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="mdvr-action-btn mdvr-action-delete"
                                                    CommandName="DeleteData" CommandArgument='<%# Eval("PackageID") %>'
                                                    OnClientClick="return mdvrConfirmRemovePackage(this);" CausesValidation="false" ToolTip="Hapus"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        var mdvrPageLoaderDefaultMessage = 'Sedang memproses data paket MDVR...';
        var mdvrPostbackTimeoutMs = 60000;
        var mdvrPostbackTimer = null;
        var mdvrPrmInitialized = false;
        var mdvrPostbackInProgress = false;

        function hidePageLoader() {
            mdvrPostbackInProgress = false;
            var overlay = document.getElementById('mdvrPageLoader');
            if (overlay) {
                overlay.className = 'mdvr-page-loader';
                overlay.setAttribute('aria-hidden', 'true');
            }
            document.querySelectorAll('[data-mdvr-original-text]').forEach(function (el) {
                el.value = el.getAttribute('data-mdvr-original-text');
                el.removeAttribute('data-mdvr-original-text');
                el.disabled = false;
            });
            document.querySelectorAll('[data-mdvr-original-html]').forEach(function (el) {
                el.innerHTML = el.getAttribute('data-mdvr-original-html');
                el.removeAttribute('data-mdvr-original-html');
                el.disabled = false;
                el.className = el.className.replace(/\s*mdvr-btn-disabled/g, '').replace(/\s+/g, ' ').trim();
            });
        }

        function showPageLoader(button, loadingText, overlayMessage) {
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
                    if (loadingText) button.value = loadingText;
                } else {
                    button.setAttribute('data-mdvr-original-html', button.innerHTML);
                    if (loadingText) button.innerHTML = '<i class="fa fa-spinner fa-spin"></i> ' + loadingText;
                    button.className = (button.className + ' mdvr-btn-disabled').replace(/\s+/g, ' ').trim();
                }
            }
        }

        function mdvrGetPostBackTarget(button) {
            if (!button) return '';
            if (button.name) return button.name;
            if (button.id) return button.id.replace(/_/g, '$');
            var href = button.getAttribute ? button.getAttribute('href') : '';
            if (href && href.indexOf('__doPostBack') >= 0) {
                var match = href.match(/__doPostBack\('([^']+)'/);
                if (match && match[1]) return match[1];
            }
            return '';
        }

        function mdvrIsLinkButton(button) {
            if (!button || !button.tagName) return false;
            return button.tagName.toUpperCase() === 'A';
        }

        function mdvrFireLinkPostBack(link, loadingText) {
            if (!link || mdvrPostbackInProgress) return;
            mdvrPostbackInProgress = true;
            showPageLoader(link, loadingText, null);

            var href = link.getAttribute('href') || '';
            if (href.indexOf('__doPostBack') >= 0) {
                var script = href.replace(/^javascript:/i, '').trim();
                try {
                    (new Function(script))();
                    return;
                } catch (ex) { }
            }

            var target = mdvrGetPostBackTarget(link);
            if (target) {
                __doPostBack(target, '');
                return;
            }

            mdvrPostbackInProgress = false;
            hidePageLoader();
        }

        function mdvrTriggerPostBack(button, loadingText, overlayMessage) {
            if (mdvrPostbackInProgress) return;
            if (mdvrIsLinkButton(button)) {
                mdvrFireLinkPostBack(button, loadingText);
                return;
            }
            mdvrPostbackInProgress = true;
            showPageLoader(button, loadingText, overlayMessage);
            var target = mdvrGetPostBackTarget(button);
            if (target) {
                __doPostBack(target, '');
            } else {
                mdvrPostbackInProgress = false;
                hidePageLoader();
            }
        }

        function mdvrPreparePostback(button, loadingText, skipValidate) {
            if (!skipValidate && typeof Page_ClientValidate === 'function' && !Page_ClientValidate()) {
                return false;
            }
            if (mdvrPostbackInProgress) return false;

            if (mdvrIsLinkButton(button)) {
                mdvrPostbackInProgress = true;
                showPageLoader(button, loadingText, null);
                return true;
            }

            mdvrTriggerPostBack(button, loadingText, null);
            return false;
        }

        function mdvrConfirmPostback(button, options, loadingText) {
            Swal.fire(options).then(function (result) {
                if (result.isConfirmed) {
                    mdvrTriggerPostBack(button, loadingText, null);
                }
            });
            return false;
        }

        function mdvrConfirmRemoveDetail(button) {
            return mdvrConfirmPostback(button, {
                title: 'Hapus Item?',
                text: 'Item ini akan dihapus dari detail paket MDVR.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#b91c1c',
                cancelButtonColor: '#64748b',
                confirmButtonText: 'Ya, Hapus',
                cancelButtonText: 'Batal'
            }, 'Menghapus...');
        }

        function mdvrConfirmRemovePackage(button) {
            return mdvrConfirmPostback(button, {
                title: 'Hapus Paket MDVR?',
                text: 'Paket MDVR dan detail item terkait akan dihapus.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#b91c1c',
                cancelButtonColor: '#64748b',
                confirmButtonText: 'Ya, Hapus',
                cancelButtonText: 'Batal'
            }, 'Menghapus...');
        }

        function mdvrCountDetailRows() {
            var grid = document.getElementById('<%= gvDetail.ClientID %>');
            if (!grid) return 0;
            var rows = grid.querySelectorAll('tbody tr');
            if (!rows.length) return 0;
            if (rows.length === 1 && rows[0].querySelector('td[colspan]')) return 0;
            return rows.length;
        }

        function mdvrValidateSaveForm() {
            var device = document.getElementById('<%= ddlDeviceMdvr.ClientID %>');
            var packageName = document.getElementById('<%= txtPackageName.ClientID %>');
            if (device && (!device.value || device.value === '[Select]')) {
                Swal.fire({ icon: 'warning', title: 'Validasi', text: 'Device MDVR header wajib dipilih.' });
                return false;
            }
            if (packageName && !packageName.value.trim()) {
                Swal.fire({ icon: 'warning', title: 'Validasi', text: 'PackageName wajib diisi.' });
                return false;
            }
            if (mdvrCountDetailRows() <= 0) {
                Swal.fire({ icon: 'warning', title: 'Validasi', text: 'Detail kebutuhan item wajib diisi.' });
                return false;
            }
            if (typeof Page_ClientValidate === 'function' && !Page_ClientValidate()) {
                return false;
            }
            return true;
        }

        function mdvrSavePackage(button) {
            if (!mdvrValidateSaveForm()) return false;
            var btnText = (button && button.value) ? button.value : '';
            var isUpdate = btnText.indexOf('Update') >= 0;
            var loadingText = isUpdate ? 'Mengupdate...' : 'Menyimpan...';
            if (isUpdate) {
                return mdvrConfirmPostback(button, {
                    title: 'Update Paket MDVR?',
                    text: 'Pastikan data paket dan detail item sudah benar.',
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonColor: '#003481',
                    cancelButtonColor: '#64748b',
                    confirmButtonText: 'Ya, Update',
                    cancelButtonText: 'Batal'
                }, loadingText);
            }
            mdvrTriggerPostBack(button, loadingText, null);
            return false;
        }

        function mdvrShowServerAlerts() {
            var container = document.getElementById('<%= div_comment.ClientID %>');
            if (!container) return;
            var successEl = container.querySelector('.mdvr-alert-success');
            if (successEl) {
                Swal.fire({ icon: 'success', title: 'Berhasil', text: successEl.textContent.trim(), confirmButtonColor: '#003481' });
                return;
            }
            var errorEl = container.querySelector('.mdvr-alert-danger');
            if (errorEl) {
                Swal.fire({ icon: 'error', title: 'Gagal', text: errorEl.textContent.trim(), confirmButtonColor: '#003481' });
            }
        }

        function mdvrInitPageRequestManager() {
            hidePageLoader();
            if (typeof Sys === 'undefined' || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
                mdvrShowServerAlerts();
                return;
            }
            if (!mdvrPrmInitialized) {
                mdvrPrmInitialized = true;
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(function () {
                    if (mdvrPostbackTimer) clearTimeout(mdvrPostbackTimer);
                    mdvrPostbackTimer = setTimeout(function () {
                        hidePageLoader();
                        Swal.fire({
                            icon: 'error',
                            title: 'Gagal',
                            text: 'Proses update paket MDVR gagal atau timeout. Silakan coba lagi.',
                            confirmButtonColor: '#003481'
                        });
                    }, mdvrPostbackTimeoutMs);
                });
                prm.add_endRequest(function (sender, args) {
                    if (mdvrPostbackTimer) { clearTimeout(mdvrPostbackTimer); mdvrPostbackTimer = null; }
                    hidePageLoader();
                    if (args.get_error && args.get_error()) {
                        args.set_errorHandled(true);
                        Swal.fire({
                            icon: 'error',
                            title: 'Gagal',
                            text: 'Proses update paket MDVR gagal atau timeout. Silakan coba lagi.',
                            confirmButtonColor: '#003481'
                        });
                        return;
                    }
                    mdvrShowServerAlerts();
                });
            } else {
                mdvrShowServerAlerts();
            }
        }

        if (typeof Sys !== 'undefined' && Sys.Application) {
            Sys.Application.add_load(mdvrInitPageRequestManager);
        } else {
            document.addEventListener('DOMContentLoaded', mdvrInitPageRequestManager);
        }
    </script>
</asp:Content>
