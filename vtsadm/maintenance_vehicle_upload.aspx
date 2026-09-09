<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="maintenance_vehicle_upload.aspx.cs" Inherits="vtsadm.maintenance_vehicle_upload" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Upload Car Master</title>
    <meta charset="utf-8" />
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <style type="text/css">
        body.cm-upload-page {
            background: #eef2f7;
            font-family: 'Source Sans Pro', sans-serif;
            padding: 16px;
            margin: 0;
        }

        .cm-upload-wrap {
            max-width: 100%;
        }

        .cm-card {
            background: #fff;
            border: 1px solid #e3e8ef;
            border-radius: 10px;
            box-shadow: 0 2px 12px rgba(15, 23, 42, 0.06);
            margin-bottom: 16px;
            overflow: hidden;
        }

        .cm-card-header {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 12px;
            padding: 18px 20px 14px;
            border-bottom: 1px solid #edf1f6;
            background: linear-gradient(180deg, #fafbfd 0%, #fff 100%);
        }

        .cm-card-header h3 {
            margin: 0 0 4px;
            font-size: 18px;
            font-weight: 600;
            color: #1f2937;
        }

        .cm-card-header p {
            margin: 0;
            font-size: 13px;
            color: #6b7280;
        }

        .cm-template-btn {
            border-radius: 8px;
            font-weight: 600;
            white-space: nowrap;
            border-color: #d1d9e6;
            color: #003481;
        }

        .cm-template-btn:hover,
        .cm-template-btn:focus {
            background: #f0f4fa;
            border-color: #003481;
            color: #003481;
        }

        .cm-card-body {
            padding: 18px 20px 20px;
        }

        .cm-format-box {
            background: #f8fafc;
            border: 1px solid #e8edf4;
            border-radius: 8px;
            padding: 12px 14px;
            margin-bottom: 16px;
        }

        .cm-format-box .cm-format-title {
            font-size: 12px;
            font-weight: 700;
            color: #475569;
            text-transform: uppercase;
            letter-spacing: 0.04em;
            margin-bottom: 8px;
        }

        .cm-format-cols {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
        }

        .cm-format-cols span {
            display: inline-block;
            background: #fff;
            border: 1px solid #dbe3ee;
            border-radius: 6px;
            padding: 4px 10px;
            font-size: 12px;
            color: #334155;
            font-weight: 600;
        }

        .cm-dropzone {
            position: relative;
            border: 2px dashed #c5d0df;
            border-radius: 10px;
            background: #fbfcfe;
            text-align: center;
            padding: 28px 20px;
            cursor: pointer;
            transition: border-color 0.2s ease, background 0.2s ease, box-shadow 0.2s ease;
        }

        .cm-dropzone:hover,
        .cm-dropzone.cm-dragover {
            border-color: #3c8dbc;
            background: #f3f8fd;
            box-shadow: inset 0 0 0 1px rgba(60, 141, 188, 0.15);
        }

        .cm-dropzone .cm-upload-icon {
            font-size: 34px;
            color: #3c8dbc;
            margin-bottom: 10px;
        }

        .cm-dropzone .cm-upload-title {
            margin: 0 0 4px;
            font-size: 15px;
            font-weight: 600;
            color: #1f2937;
        }

        .cm-dropzone .cm-upload-hint {
            margin: 0 0 10px;
            font-size: 12px;
            color: #64748b;
        }

        .cm-dropzone .cm-file-name {
            display: inline-block;
            max-width: 100%;
            padding: 6px 12px;
            border-radius: 999px;
            background: #eef4fb;
            color: #003481;
            font-size: 12px;
            font-weight: 600;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .cm-dropzone input[type="file"] {
            position: absolute;
            inset: 0;
            width: 100%;
            height: 100%;
            opacity: 0;
            cursor: pointer;
        }

        .cm-alert {
            border-radius: 8px;
            margin-top: 14px;
            margin-bottom: 0;
            border: 0;
            padding: 12px 14px;
            font-size: 13px;
        }

        .cm-actions {
            display: flex;
            justify-content: flex-end;
            align-items: center;
            gap: 10px;
            margin-top: 16px;
            padding-top: 14px;
            border-top: 1px solid #edf1f6;
        }

        .cm-btn-submit {
            border-radius: 8px;
            font-weight: 600;
            min-width: 120px;
            padding: 8px 18px;
            background: #003481;
            border-color: #003481;
        }

        .cm-btn-submit:hover,
        .cm-btn-submit:focus {
            background: #002a66;
            border-color: #002a66;
        }

        .cm-preview-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
            padding: 14px 20px;
            border-bottom: 1px solid #edf1f6;
            background: #fafbfd;
        }

        .cm-preview-header h4 {
            margin: 0;
            font-size: 15px;
            font-weight: 600;
            color: #1f2937;
        }

        .cm-preview-count {
            font-size: 12px;
            color: #64748b;
            background: #eef2f7;
            border-radius: 999px;
            padding: 4px 10px;
            font-weight: 600;
        }

        .cm-legend {
            display: flex;
            flex-wrap: wrap;
            gap: 8px 14px;
            padding: 10px 20px 0;
            font-size: 11px;
            color: #64748b;
        }

        .cm-legend-item {
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .cm-legend-dot {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            display: inline-block;
        }

        .cm-grid-panel {
            padding: 12px 16px 16px;
        }

        .cm-grid-panel .table {
            margin-bottom: 8px;
            border-radius: 8px;
            overflow: hidden;
        }

        .cm-grid-panel .table > thead > tr > th {
            background: #003481;
            color: #fff;
            border-color: #003481;
            font-size: 12px;
            font-weight: 600;
            white-space: nowrap;
            vertical-align: middle;
        }

        .cm-grid-panel .table > tbody > tr > td {
            font-size: 12px;
            color: #1f2937;
            vertical-align: middle;
        }

        .cm-grid-panel .table > tbody > tr:nth-child(even) > td {
            background: #f9fbfd;
        }

        .cm-grid-panel .table > tbody > tr:hover > td {
            background: #eef4fb;
        }

        .cm-grid-panel .pagination-ys a,
        .cm-grid-panel .pagination-ys span {
            border-radius: 6px;
        }

        .cm-paging {
            color: #64748b;
            font-size: 12px;
            font-style: normal;
            margin-left: 4px;
        }

        .cm-badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 999px;
            font-size: 11px;
            font-weight: 700;
            line-height: 1.2;
            white-space: nowrap;
        }

        .cm-badge-waiting { background: #495057; color: #fff; }
        .cm-badge-success { background: #28a745; color: #fff; }
        .cm-badge-failed { background: #dc3545; color: #fff; }
        .cm-badge-warning { background: #f0ad4e; color: #fff; }

        .cm-status-error {
            display: block;
            margin-top: 4px;
            font-size: 11px;
            color: #dc3545;
            font-weight: 600;
            white-space: normal;
            max-width: 220px;
        }

        .cm-empty-wrap {
            text-align: center;
            padding: 28px 16px 34px;
            color: #94a3b8;
        }

        .cm-empty-wrap i {
            font-size: 28px;
            margin-bottom: 8px;
            color: #cbd5e1;
        }

        .cm-empty-wrap p {
            margin: 0;
            font-size: 13px;
        }
    </style>
    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
</head>
<body class="cm-upload-page">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div id="overlay" style="display: none;">
            <div class="cv-spinner">
                <span class="spinner"></span>
            </div>
        </div>

        <div class="cm-upload-wrap">
            <div class="cm-card">
                <div class="cm-card-header">
                    <div>
                        <h3><i class="fa fa-upload text-primary"></i> Bulk Upload Car Master</h3>
                        <p>Update Police No, Asset No, and VIN from Excel file</p>
                    </div>
                    <a href="Export/editcarmaster.xlsx" class="btn btn-default btn-sm cm-template-btn" download="editcarmaster.xlsx">
                        <i class="fa fa-download"></i> Download Template
                    </a>
                </div>
                <div class="cm-card-body">
                    <div class="cm-format-box">
                        <div class="cm-format-title">Excel format (row 1 = header)</div>
                        <div class="cm-format-cols">
                            <span>A - NoSN</span>
                            <span>B - New PoliceNo</span>
                            <span>C - New No Asset</span>
                            <span>D - New VIN / No Rangka</span>
                        </div>
                    </div>

                    <div class="cm-dropzone" id="cmDropzone">
                        <div class="cm-upload-icon"><i class="fa fa-cloud-upload"></i></div>
                        <p class="cm-upload-title">Choose Excel file</p>
                        <p class="cm-upload-hint">Click to browse or drag file here (.xlsx / .xls only)</p>
                        <span class="cm-file-name" id="cmFileName">No file selected</span>
                        <asp:FileUpload ID="FileUpload1" runat="server" accept=".xlsx,.xls" />
                    </div>

                    <div runat="server" id="lblMsg" class="alert cm-alert" style="display: none;"></div>

                    <div class="cm-actions">
                        <asp:Button ID="CmdAutoUpload" runat="server" Text="Upload" OnClick="CmdAutoUpload_ServerClick" CssClass="hidden" Style="display: none;" UseSubmitBehavior="true" />
                        <asp:Button ID="CmdSubmit" runat="server" Text="Submit" CssClass="btn btn-primary cm-btn-submit" OnClientClick="openSubmitConfirm(); return false;" Visible="false" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>

            <div class="cm-card">
                <div class="cm-preview-header">
                    <h4><i class="fa fa-table"></i> Preview Data</h4>
                    <asp:Label ID="LblPreviewCount" runat="server" CssClass="cm-preview-count" Text="0 rows"></asp:Label>
                </div>
                <div class="cm-legend">
                    <span class="cm-legend-item"><span class="cm-legend-dot" style="background:#495057;"></span> Waiting for Process</span>
                    <span class="cm-legend-item"><span class="cm-legend-dot" style="background:#28a745;"></span> Success</span>
                    <span class="cm-legend-item"><span class="cm-legend-dot" style="background:#dc3545;"></span> Failed</span>
                    <span class="cm-legend-item"><span class="cm-legend-dot" style="background:#f0ad4e;"></span> NoSN tidak ditemukan</span>
                </div>
                <div class="cm-grid-panel">
                    <asp:Panel ID="PanelGrid" runat="server" Visible="false" Width="100%" ScrollBars="Auto">
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" Font-Size="Small" CssClass="table table-bordered table-hover" CellPadding="4" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="0" EmptyDataText="" ForeColor="#1f2937" GridLines="None" BorderWidth="0px" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="RowNo" HeaderText="No" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" HtmlEncode="false" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="FullName" HeaderText="Customer" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="NewPoliceNo" HeaderText="New Police No" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="AssetNo" HeaderText="No Asset" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="NewNoAsset" HeaderText="New No Asset" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="NoSN" HeaderText="NoSN" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="DisplayVin" HeaderText="Vin" ItemStyle-Wrap="false" />
                            </Columns>
                            <HeaderStyle Height="36px" Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="PanelEmpty" runat="server" CssClass="cm-empty-wrap">
                        <i class="fa fa-file-excel-o"></i>
                        <p>Upload Excel file to preview data here</p>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-submit" tabindex="-1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <p style="margin: 0 0 8px;">Apakah Anda yakin ingin memperbarui data Car Master dari file ini?</p>
                        <p class="text-muted" style="margin: 0; font-size: 12px;">Hanya data yang valid akan diproses. Pastikan preview sudah sesuai.</p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdYesSubmit" runat="server" Text="Yes" CssClass="btn btn-primary" OnClick="CmdSubmit_ServerClick" OnClientClick="closeSubmitConfirm(); showLoader(); return true;" UseSubmitBehavior="true" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function showLoader() {
            $('#overlay').fadeIn(200);
        }

        function hideLoader() {
            $('#overlay').fadeOut(200);
        }

        function openSubmitConfirm() {
            $('#modal-submit').modal('show');
        }

        function closeSubmitConfirm() {
            $('#modal-submit').modal('hide');
        }

        function updateFileName(input) {
            var nameEl = document.getElementById('cmFileName');
            if (!nameEl || !input) return;
            if (!input.value) {
                nameEl.textContent = 'No file selected';
                return;
            }
            var path = input.value;
            var fileName = path.split('\\').pop().split('/').pop();
            nameEl.textContent = fileName;
        }

        $(function () {
            hideLoader();

            var $file = $('#<%= FileUpload1.ClientID %>');

            $file.on('change', function () {
                updateFileName(this);
                if (!this.value) return;
                showLoader();
                document.getElementById('<%= CmdAutoUpload.ClientID %>').click();
            });
        });
    </script>
</body>
</html>
