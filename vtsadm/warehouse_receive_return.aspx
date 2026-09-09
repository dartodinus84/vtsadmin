<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="warehouse_receive_return.aspx.cs" Inherits="vtsadm.warehouse_receive_return" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Warehouse Receive Return      
                <small>Batch Processing</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Warehouse</a></li>
            <li class="active">Receive Return</li>
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
                                    Scan Serial Number (NoSN) and press Enter to add. Multiple SN can be entered separated by space. Click &times; to remove. Double-click tag to remove.
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
                                    Text="Search & Auto-Check All Devices" 
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
            <!-- PENDING (dengan checkbox) -->
            <div class="col-md-12">
                <div class="box box-warning">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-clock-o"></i> Pending - Device Belum di-Receive
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
                                EmptyDataText="No devices pending"
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
                    <div class="box-footer text-center" style="padding: 20px;">
                        <asp:Button ID="CmdSubmit" runat="server" CssClass="btn btn-success btn-lg" 
                            Text="Submit Receive Return" OnClick="CmdSubmit_Click" />
                        <br />
                        <small class="text-muted" style="display: block; margin-top: 12px;">
                            <i class="fa fa-info-circle"></i> 
                            Submit selected devices to warehouse
                        </small>
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
            var input = document.getElementById('txtScanInput');
            var inputValue = input.value.trim().toUpperCase();
            
            if (inputValue === '') {
                showBarcodeFeedback('warning', 'Please scan or enter a Serial Number');
                return;
            }
            
            // Split by space to handle multiple serial numbers
            var serialNumbers = inputValue.split(/\s+/).filter(function(sn) {
                return sn.trim() !== '';
            });
            
            if (serialNumbers.length === 0) {
                showBarcodeFeedback('warning', 'Please scan or enter a Serial Number');
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
                tagRemove.innerHTML = 'X';
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
                    showBarcodeFeedback('success', 'Added: ' + addedSerials[0]);
                } else {
                    showBarcodeFeedback('success', 'Added ' + addedCount + ' serial number(s)');
                }
            } else if (addedCount > 0 && duplicateCount > 0) {
                showBarcodeFeedback('warning', 'Added ' + addedCount + ' serial number(s). ' + duplicateCount + ' duplicate(s) skipped.');
            } else if (addedCount === 0 && duplicateCount > 0) {
                if (duplicateCount === 1) {
                    showBarcodeFeedback('warning', 'Already scanned: ' + duplicateSerials[0]);
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
            showBarcodeFeedback('info', 'Removed: ' + serialNo);
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
                showBarcodeFeedback('info', 'All tags cleared');
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
            var tagsWrapper = document.getElementById('tagsWrapper');
            var count = tagsWrapper.getElementsByClassName('tag-item').length;
            
            if (count === 0) {
                alert('Warning: No serial numbers scanned!\n\nPlease scan at least one serial number before searching.');
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
                    showBarcodeFeedback('success', 'Found and checked: ' + foundCount + ' device(s)');
                } else {
                    showBarcodeFeedback('danger', 'Found and checked: 0 device(s). No matching pending devices.');
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
                focusScanInput();
            }, 500);
        };
    </script>
</asp:Content>

