<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="device_mutation_technician_upload.aspx.cs" Inherits="vtsadm.device_mutation_technician_upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" />

    <style type="text/css">
        body {
            font-family: 'Inter', 'Source Sans Pro', sans-serif;
            background: #f4f6fb;
            margin: 0;
            padding: 12px;
            color: #1e293b;
        }

        .upload-shell {
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .upload-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            box-shadow: 0 1px 3px rgba(15, 23, 42, 0.06);
            overflow: hidden;
        }

        .upload-card-header {
            padding: 14px 16px 0;
            font-size: 13px;
            font-weight: 600;
            color: #475569;
            letter-spacing: 0.02em;
            text-transform: uppercase;
        }

        .upload-card-body {
            padding: 14px 16px 16px;
        }

        .file-picker input[type="file"] {
            max-width: 100%;
            padding: 8px 10px;
            border: 1px dashed #cbd5e1;
            border-radius: 10px;
            background: #f8fafc;
            font-size: 13px;
        }

        .upload-actions,
        .preview-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            margin-top: 14px;
        }

        .upload-actions .btn,
        .preview-actions .btn {
            border-radius: 8px;
            font-weight: 600;
            font-size: 13px;
            padding: 8px 14px;
            border: none;
            transition: all 0.15s ease;
        }

        .btn-upload {
            background: #2563eb;
            color: #fff;
        }

        .btn-upload:hover {
            background: #1d4ed8;
            color: #fff;
        }

        .btn-submit-request {
            background: #059669;
            color: #fff;
        }

        .btn-submit-request:hover:not([disabled]) {
            background: #047857;
            color: #fff;
        }

        .btn-submit-request[disabled] {
            background: #94a3b8;
            cursor: not-allowed;
            opacity: 0.85;
        }

        .btn-cancel {
            background: #e2e8f0;
            color: #334155;
        }

        .btn-cancel:hover {
            background: #cbd5e1;
            color: #1e293b;
        }

        .btn-error {
            background: #f59e0b;
            color: #fff;
        }

        .btn-error:hover {
            background: #d97706;
            color: #fff;
        }

        .upload-alert {
            margin-top: 12px;
            padding: 10px 12px;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 500;
            display: none;
        }

        .upload-alert.is-info {
            display: block;
            background: #eff6ff;
            color: #1d4ed8;
            border: 1px solid #bfdbfe;
        }

        .upload-alert.is-success {
            display: block;
            background: #ecfdf5;
            color: #047857;
            border: 1px solid #a7f3d0;
        }

        .upload-alert.is-danger {
            display: block;
            background: #fef2f2;
            color: #b91c1c;
            border: 1px solid #fecaca;
        }

        .upload-alert.is-warning {
            display: block;
            background: #fffbeb;
            color: #b45309;
            border: 1px solid #fde68a;
        }

        .preview-info {
            margin-bottom: 10px;
            padding: 8px 10px;
            border-radius: 8px;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            color: #475569;
            font-size: 12px;
            line-height: 1.5;
        }

        .preview-warning {
            margin-top: 10px;
            padding: 8px 10px;
            border-radius: 8px;
            background: #fffbeb;
            border: 1px solid #fde68a;
            color: #b45309;
            font-size: 12px;
            font-weight: 600;
        }

        .upload-table-wrap {
            overflow-x: auto;
            border-radius: 10px;
            border: 1px solid #e2e8f0;
        }

        .upload-preview-table {
            margin-bottom: 0 !important;
            font-size: 12px;
            min-width: 900px;
        }

        .upload-preview-table th {
            background: #f8fafc !important;
            color: #475569 !important;
            font-weight: 600 !important;
            border-bottom: 1px solid #e2e8f0 !important;
            white-space: nowrap;
            padding: 10px 12px !important;
        }

        .upload-preview-table td {
            vertical-align: middle !important;
            padding: 9px 12px !important;
            border-color: #f1f5f9 !important;
            color: #1e293b;
        }

        .upload-preview-table tr:nth-child(even) td {
            background: #fafbfc;
        }

        .upload-row-failed td {
            background-color: #fde2e2 !important;
            color: #991b1b !important;
            font-weight: 600;
        }

        .upload-message-cell {
            max-width: 280px;
            white-space: normal;
            word-break: break-word;
            line-height: 1.45;
        }

        .status-badge {
            display: inline-block;
            padding: 3px 10px;
            border-radius: 999px;
            font-size: 11px;
            font-weight: 700;
            letter-spacing: 0.02em;
            text-transform: lowercase;
        }

        .status-badge.success {
            background: #dcfce7;
            color: #166534;
        }

        .status-badge.failed {
            background: #fee2e2;
            color: #991b1b;
        }

        .status-badge.pending,
        .status-badge.warning {
            background: #fef3c7;
            color: #92400e;
        }

        .status-badge.neutral {
            background: #e2e8f0;
            color: #475569;
        }

        .upload-paging {
            margin-top: 8px;
            color: #64748b !important;
            font-style: italic;
            font-size: 12px !important;
        }

        #overlay .spinner {
            margin-right: 0;
            border-top-color: #2563eb;
        }
    </style>

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="overlay">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>

        <div class="upload-shell">
            <div class="upload-card">
                <div class="upload-card-header">Upload CSV</div>
                <div class="upload-card-body">
                    <input type="hidden" runat="server" id="txtBatchNo" />
                    <input type="hidden" runat="server" id="txtFileName" />
                    <div class="file-picker">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
                    <div class="upload-actions">
                        <button id="CmdUpload" runat="server" type="button" class="btn btn-upload" onclick="showOverlay();" onserverclick="CmdUpload_ServerClick">
                            <i class="fa fa-cloud-upload"></i> Upload &amp; Validate
                        </button>
                        <button id="CmdCancel" runat="server" type="button" class="btn btn-cancel" onclick="showOverlay();" onserverclick="CmdCancel_ServerClick">
                            <i class="fa fa-times"></i> Cancel
                        </button>
                        <button id="CmdError" runat="server" type="button" class="btn btn-error" onclick="showOverlay();" onserverclick="CmdError_ServerClick">
                            <i class="fa fa-download"></i> Download Failed Rows
                        </button>
                    </div>
                    <div runat="server" id="lblMsg" class="upload-alert"></div>
                </div>
            </div>

            <div class="upload-card">
                <div class="upload-card-header">Preview</div>
                <div class="upload-card-body">
                    <div runat="server" id="lblPreviewInfo" class="preview-info" visible="false">
                        Data dengan status failed tidak akan diproses saat Submit Request.
                    </div>
                    <div class="upload-table-wrap">
                        <asp:GridView ID="GridView1" runat="server"
                            CssClass="table table-bordered upload-preview-table"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            PageSize="5"
                            GridLines="None"
                            BorderWidth="0"
                            EmptyDataText="No items to display"
                            OnRowDataBound="GridView1_RowDataBound"
                            OnPageIndexChanging="GridView1_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="UploadStatus" HeaderText="Upload Status" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="UploadMessage" HeaderText="Upload Message" ItemStyle-CssClass="upload-message-cell" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse ID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" />
                            </Columns>
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Left" />
                            <PagerSettings PageButtonCount="3" FirstPageText="&laquo;" LastPageText="&raquo;" Mode="NumericFirstLast" />
                        </asp:GridView>
                    </div>
                    <asp:Label ID="LblPaging" runat="server" CssClass="upload-paging" />
                    <div runat="server" id="lblSubmitWarning" class="preview-warning" visible="false"></div>
                    <div class="preview-actions">
                        <button id="CmdSubmitRequest" runat="server" type="button" class="btn btn-submit-request" onclick="showOverlay();" onserverclick="CmdSubmitRequest_ServerClick">
                            <i class="fa fa-paper-plane"></i> Submit Request
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function showOverlay() {
                $('#overlay').stop(true, true).fadeIn(200);
            }

            function hideOverlay() {
                $('#overlay').stop(true, true).fadeOut(200);
            }

            function applyAlertStyle() {
                var lbMsg = document.getElementById('<%= lblMsg.ClientID %>');
                if (!lbMsg || !lbMsg.innerHTML) return;

                var alertType = lbMsg.getAttribute('data-alert') || 'danger';
                lbMsg.className = 'upload-alert is-' + alertType;

                window.setTimeout(function () {
                    $(lbMsg).fadeTo(400, 0, function () {
                        $(this).slideUp(300, function () {
                            this.innerHTML = '';
                            this.removeAttribute('data-alert');
                            this.style.display = 'none';
                            this.style.opacity = 1;
                        });
                    });
                }, 5000);
            }

            $(function () {
                hideOverlay();
                applyAlertStyle();
            });
        </script>
    </form>
</body>
</html>
