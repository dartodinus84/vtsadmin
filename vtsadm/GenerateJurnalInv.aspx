<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateJurnalInv.aspx.cs" Inherits="vtsadm.GenerateJurnalInv" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="content-header">
    <h1>Generate Jurnal Invoice</h1>
    <ol class="breadcrumb">
        <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">Generate Jurnal Invoice</li>
    </ol>
</section>

<section class="content">
    <div class="row">
        <div class="col-md-12">

            <!-- FILTER BOX -->
            <div class="box box-solid">
                <div class="box-header with-border d-flex justify-content-between align-items-center">
                    <h3 class="box-title mb-0">Filter</h3>
                </div>
                <div class="box-body">
                    <div class="form-group">
                        <label>Search</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search transaction ..."></asp:TextBox>
                    </div>
                    
                </div>
                <div class="box-footer d-flex gap-2">
                    <button id="btnClear" type="button" class="btn btn-primary">Clear</button>
                    <button id="btnGenerateInvoice" type="button" class="btn btn-primary">Generate Invoice</button>
                    <button id="btnExportExcel" type="button" class="btn btn-success">
                        <i class="fa fa-file-excel"></i> Export Excel
                    </button>
                    <button id="btnSaveAsDraft" type="button" class="btn btn-warning">
                        <i class="fa fa-save"></i> Save as Draft
                    </button>
                    <button id="btnUploadFile" type="button" class="btn btn-info">
                        <i class="fa fa-upload"></i> Upload File
                    </button>
                    <button id="btnCreateSalesOrderJurnal" type="button" class="btn btn-primary">
                        <i class="fa fa-paper-plane"></i> Create Sales Order in Jurnal
                    </button>
                    
                </div>

            </div>

            <!-- LIST BOX -->
            <div class="box box-solid">
                <div class="box-header with-border">
                    <h3 class="box-title">List Generate Jurnal Invoice</h3>
                </div>
                <div class="box-body p-0">
                    <div class="table-responsive" style="max-height: 500px; overflow-y: auto;">
                        <table id="tblJurnal" class="table table-bordered table-striped mb-0">
                            <thead>
                                <tr>
                                    <th><input type="checkbox" id="chkSelectAll" /></th>
                                    <th>#</th>
                                    <th>ID</th>
                                    <th>ID Sales Invoice</th>
                                    <th>Transaction No</th>
                                    <th>Email</th>
                                    <th>Message</th>
                                    <th>Address</th>
                                    <th>Memo</th>
                                    <th>Sub Total</th>
                                    <th>Tax Amount</th>
                                    <th>Original Amount</th>
                                    <th>Use Tax Inclusive</th>
                                    <th>Tax After Disc</th>
                                    <th>Transaction Date</th>
                                    <th>Due Date</th>
                                    <th>Term Name</th>
                                    <th>Withholding Name</th>
                                    <th>Disc. Type Name</th>
                                    <th>Person Display Name</th>
                                    <th>Tags String</th>
                                    <th>Custom ID</th>

                                    <th>Invoice ID</th>
                                    <th>Rate</th>
                                    <th>Discount</th>
                                    <th>Quantity</th>
                                    <th>Product Name</th>
                                    <th>Line Tax ID</th>
                                    <th>Description</th>
                                    <th>Parsed Description</th>
                                    <th>Month Desc</th>
                                    <th>Year Desc</th>
                                    <th>Installment Desc</th>
                                    <th>Plafon Desc</th>
                                    <th>Cycle Desc</th>
                                    <th>Nopol</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- AJAX data here -->
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="box-footer text-right small text-muted">
                    <span id="recordCount">Displaying 0 records</span>
                </div>
            </div>

            <!-- LIST BOX NEW -->
            <div class="box box-solid">
                <div class="box-header with-border">
                    <h3 class="box-title">List Jurnal Invoice</h3>
                </div>
                <div class="box-body p-0">
                    <div class="table-responsive" style="max-height: 500px; overflow-y: auto;">
                        <table id="tblJurnalNew" class="table table-bordered table-striped mb-0">
                            <thead>
                                <tr>
                                    <th><input type="checkbox" id="chkSelectAllNew" /></th>
                                    <th>#</th>
                                    <th>ID Sales Invoice</th>
                                    <th>Transaction No</th>
                                    <th>Message</th>
                                    <th>Remaining</th>
                                    <th>Original Amount</th>
                                    <th>Cred. Memo Balance</th>
                                    <th>Use Tax Inclusive</th>
                                    <th>Tax After Disc</th>
                                    <th>Tax Amount</th>
                                    <th>Status</th>
                                    <th>Amount Receive</th>
                                    <th>Sub Total</th>
                                    <th>Transaction Date</th>
                                    <th>Due Date</th>
                                    <th>Payment Received Amount</th>
                                    <th>Transaction Status ID</th>
                                    <th>Transaction Status Name</th>
                                    <th>Term Name</th>
                                    <th>Withholding Name</th>
                                    <th>Disc. Type Name</th>
                                    <th>Customer ID</th>
                                    <th>Person ID</th>
                                    <th>Person Display Name</th>
                                    <th>Tags String</th>
                                    <th>Has Payments</th>
                                    <th>Earliest Payment Date</th>
                                    <th>Address</th>
                                    <th>Memo</th>
                                    <th>Transaction No Old</th>

                                    <th>Invoice ID</th>
                                    <th>Custom ID</th>
                                    <th>Description</th>
                                    <th>Amount</th>
                                    <th>Rate</th>
                                    <th>Discount</th>
                                    <th>Quantity</th>
                                    <th>Product ID</th>
                                    <th>Product Name</th>
                                    <th>Line Tax ID</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- AJAX data here -->
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="box-footer text-right small text-muted">
                    <span id="recordCountNew">Displaying 0 records</span>
                </div>
            </div>

        </div>
    </div>

    <!-- MODAL DETAIL -->
    <div class="modal fade" id="modalDetail">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Detail Jurnal Invoice</h4>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <table id="tblDetail" class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th>Invoice ID</th>
                                    <th>ID</th>
                                    <th>Custom ID</th>
                                    <th>Description</th>
                                    <th>Amount</th>
                                    <th>Rate</th>
                                    <th>Discount</th>
                                    <th>Quantity</th>
                                    <th>Product ID</th>
                                    <th>Product Name</th>
                                    <th>Line Tax ID</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- Detail data here -->
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL UPLOAD FILE -->
    <div class="modal fade" id="modalUpload">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Upload Excel File</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Select Excel File</label>
                        <input type="file" id="fileUpload" class="form-control" accept=".xlsx,.xls" />
                    </div>
                    <div class="alert alert-info">
                        <i class="fa fa-info-circle"></i> 
                        Please upload Excel file with proper format. The file should contain jurnal invoice data.
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnProcessUpload">Process Upload</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL -->
    <div class="modal fade" id="modal-messagebox">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Info Box</h4>
                </div>
                <div class="modal-body">
                    <div id="div_comment" runat="server"></div>
                </div>
            </div>
        </div>
    </div>

    <!-- LOADER -->
    <div id="pageLoader" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(255,255,255,0.7); z-index:9999;">
        <div style="position:absolute; top:50%; left:50%; transform:translate(-50%,-50%); text-align:center;">
            <i class="fa fa-spinner fa-spin fa-3x fa-fw text-primary"></i>
            <p style="margin-top:10px; color:#333;">Loading...</p>
        </div>
    </div>

</section>

<script type="text/javascript">
    var selectedItems = [];
    var selectedItemsNew = [];
    
    // Define loadJurnal function outside document.ready so it can be accessed globally
    function loadJurnal() {
            $("#pageLoader").show();
            $("#tblJurnal tbody").html("<tr><td colspan='36' class='text-center'>Loading...</td></tr>");
            
            $.ajax({
                type: "POST",
                url: "GenerateJurnalInv.aspx/GetJurnalHeader",
                data: JSON.stringify({
                    search: $("#<%= txtSearch.ClientID %>").val() || ""
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var data = JSON.parse(response.d);
                        var html = "";
                        
                        if (data && data.error) {
                            html = "<tr><td colspan='36' class='text-center text-danger'>Error: " + data.error + "</td></tr>";
                            $("#recordCount").text("Error loading data");
                            $("#tblJurnal tbody").html(html);
                            $("#pageLoader").hide();
                            return;
                        }

                        if (Array.isArray(data) && data.length > 0) {
                            $.each(data, function (i, item) {
                                var id = item.id_sales_invoice || (i + 1);
                                html += "<tr>";
                                html += "<td><input type='checkbox' class='chkItem' value='" + id + "' /></td>";
                                html += "<td>" + (i + 1) + "</td>";
                                html += "<td>" + (item.id || '') + "</td>";
                                html += "<td>" + id + "</td>";
                                html += "<td>" + (item.transaction_no || '') + "</td>";
                                html += "<td>" + (item.email || '') + "</td>";
                                html += "<td style='max-width:500px; word-wrap:break-word; white-space:normal;'>" + (item.message || '') + "</td>";
                                html += "<td>" + (item.address || '') + "</td>";
                                html += "<td>" + (item.memo || '') + "</td>";
                                html += "<td>" + (item.subtotal || '') + "</td>";
                                html += "<td>" + (item.tax_amount || '') + "</td>";
                                html += "<td>" + (item.original_amount || '') + "</td>";
                                html += "<td>" + (item.use_tax_inclusive || '') + "</td>";
                                html += "<td>" + (item.tax_after_discount || '') + "</td>";
                                html += "<td>" + (item.transaction_date || '') + "</td>";
                                html += "<td>" + (item.due_date || '') + "</td>";
                                html += "<td>" + (item.term_name || '') + "</td>";
                                html += "<td>" + (item.witholding_type || '') + "</td>";
                                html += "<td>" + (item.discount_type_name || '') + "</td>";
                                html += "<td>" + (item.person_display_name || '') + "</td>";
                                html += "<td>" + (item.tags_string || '') + "</td>";
                                html += "<td>" + (item.custom_id || '') + "</td>";

                                html += "<td>" + (item.invoice_id || '') + "</td>";
                                html += "<td>" + (item.rate || '') + "</td>";
                                html += "<td>" + (item.discount || '') + "</td>";
                                html += "<td>" + (item.quantity || '') + "</td>";
                                html += "<td>" + (item.product_name || '') + "</td>";
                                html += "<td>" + (item.line_tax_id || '') + "</td>";
                                html += "<td>" + (item.description || '') + "</td>";
                                html += "<td>" + (item.parsed_description || '') + "</td>";
                                html += "<td>" + (item.month_desc || '') + "</td>";
                                html += "<td>" + (item.year_desc || '') + "</td>";
                                html += "<td>" + (item.installment_desc || '') + "</td>";
                                html += "<td>" + (item.plafon_desc || '') + "</td>";
                                html += "<td>" + (item.cycle_desc || '') + "</td>";
                                html += "<td>" + (item.nopol || '') + "</td>";
                                html += "</tr>";
                            });
                            $("#recordCount").text("Displaying " + data.length + " records");
                        } else {
                            html = "<tr><td colspan='36' class='text-center'>No data found.</td></tr>";
                            $("#recordCount").text("No records found");
                        }
                        $("#tblJurnal tbody").html(html);
                    } catch (e) {
                        console.error("Error parsing response:", e);
                        $("#tblJurnal tbody").html("<tr><td colspan='36' class='text-center text-danger'>Error parsing data</td></tr>");
                        $("#recordCount").text("Error loading data");
                    }
                    $("#pageLoader").hide();
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", xhr.responseText);
                    $("#tblJurnal tbody").html("<tr><td colspan='36' class='text-center text-danger'>Error loading data: " + error + "</td></tr>");
                    $("#recordCount").text("Error loading data");
                    $("#pageLoader").hide();
                }
            });
    }

    // Define loadJurnalNew function outside document.ready so it can be accessed globally
    function loadJurnalNew() {
            $("#pageLoader").show();
            $("#tblJurnalNew tbody").html("<tr><td colspan='33' class='text-center'>Loading...</td></tr>");
            
            $.ajax({
                type: "POST",
                url: "GenerateJurnalInv.aspx/GetJurnalHeaderNew",
                data: JSON.stringify({
                    search: $("#<%= txtSearch.ClientID %>").val() || ""
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var data = JSON.parse(response.d);
                        var html = "";
                        
                        if (data && data.length > 0) {
                            $.each(data, function (i, item) {
                                var id = item.id_sales_invoice || (i + 1);
                                html += "<tr>";
                                html += "<td><input type='checkbox' class='chkItemNew' value='" + id + "' /></td>";
                                html += "<td>" + (i + 1) + "</td>";
                                html += "<td>" + id + "</td>";
                                html += "<td>" + (item.transaction_no || '') + "</td>";
                                html += "<td style='max-width:500px; word-wrap:break-word; white-space:normal;'>" + item.message + "</td>";
                                html += "<td>" + (item.remaining || '') + "</td>";
                                html += "<td>" + (item.original_amount || '') + "</td>";
                                html += "<td>" + (item.credit_memo_balance || '') + "</td>";
                                html += "<td>" + (item.use_tax_inclusive || '') + "</td>";
                                html += "<td>" + (item.tax_after_discount || '') + "</td>";
                                html += "<td>" + (item.tax_amount || '') + "</td>";
                                html += "<td>" + (item.status || '') + "</td>";
                                html += "<td>" + (item.amount_receive || '') + "</td>";
                                html += "<td>" + (item.subtotal || '') + "</td>";
                                html += "<td>" + (item.transaction_date || '') + "</td>";
                                html += "<td>" + (item.due_date || '') + "</td>";
                                html += "<td>" + (item.payment_received_amount || '') + "</td>";
                                html += "<td>" + (item.transaction_status_id || '') + "</td>";
                                html += "<td>" + (item.transaction_status_name || '') + "</td>";
                                html += "<td>" + (item.term_name || '') + "</td>";
                                html += "<td>" + (item.witholding_type || '') + "</td>";
                                html += "<td>" + (item.discount_type_name || '') + "</td>";
                                html += "<td>" + (item.custom_id || '') + "</td>";
                                html += "<td>" + (item.person_id || '') + "</td>";
                                html += "<td>" + (item.person_display_name || '') + "</td>";
                                html += "<td>" + (item.tags_string || '') + "</td>";
                                html += "<td>" + (item.has_payments || '') + "</td>";
                                html += "<td>" + (item.earliest_payment_date || '') + "</td>";
                                html += "<td>" + (item.address || '') + "</td>";
                                html += "<td>" + (item.memo || '') + "</td>";
                                html += "<td>" + (item.transaction_no_old || '') + "</td>";

                                html += "<td>" + (item.invoice_id || '') + "</td>";
                                html += "<td>" + (item.custom_id_detail || '') + "</td>";
                                html += "<td>" + (item.description || '') + "</td>";
                                html += "<td>" + (item.amount || '') + "</td>";
                                html += "<td>" + (item.rate || '') + "</td>";
                                html += "<td>" + (item.discount || '') + "</td>";
                                html += "<td>" + (item.quantity || '') + "</td>";
                                html += "<td>" + (item.product_id || '') + "</td>";
                                html += "<td>" + (item.product_name || '') + "</td>";
                                html += "<td>" + (item.line_tax_id || '') + "</td>";
                                html += "</tr>";
                            });
                            $("#recordCountNew").text("Displaying " + data.length + " records");
                        } else {
                            html = "<tr><td colspan='33' class='text-center'>No data found.</td></tr>";
                            $("#recordCountNew").text("No records found");
                        }
                        $("#tblJurnalNew tbody").html(html);
                    } catch (e) {
                        console.error("Error parsing response:", e);
                        $("#tblJurnalNew tbody").html("<tr><td colspan='33' class='text-center text-danger'>Error parsing data</td></tr>");
                        $("#recordCountNew").text("Error loading data");
                    }
                    $("#pageLoader").hide();
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", xhr.responseText);
                    $("#tblJurnalNew tbody").html("<tr><td colspan='33' class='text-center text-danger'>Error loading data: " + error + "</td></tr>");
                    $("#recordCountNew").text("Error loading data");
                    $("#pageLoader").hide();
                }
            });
    }

    $(document).ready(function () {
        
        // Initialize empty table - data will load when Generate Invoice button is clicked
        $("#tblJurnal tbody").html("<tr><td colspan='36' class='text-center'>Click 'Generate Invoice' to load data.</td></tr>");
        
        // Initialize empty table for new table - data will load when Load New Table button is clicked
        $("#tblJurnalNew tbody").html("<tr><td colspan='33' class='text-center'>Click 'Load New Table' to load data.</td></tr>");

        // Select All functionality
        $("#chkSelectAll").change(function() {
            $(".chkItem").prop('checked', $(this).prop('checked'));
            updateSelectedItems();
        });

        // Individual checkbox change
        $(document).on('change', '.chkItem', function() {
            updateSelectedItems();
        });

        // Select All functionality for new table
        $("#chkSelectAllNew").change(function() {
            $(".chkItemNew").prop('checked', $(this).prop('checked'));
            updateSelectedItemsNew();
        });

        // Individual checkbox change for new table
        $(document).on('change', '.chkItemNew', function() {
            updateSelectedItemsNew();
        });

        function updateSelectedItems() {
            selectedItems = [];
            $(".chkItem:checked").each(function() {
                selectedItems.push($(this).val());
            });
        }

        function updateSelectedItemsNew() {
            selectedItemsNew = [];
            $(".chkItemNew:checked").each(function() {
                selectedItemsNew.push($(this).val());
            });
        }

        // Detail button click
        $(document).on('click', '.btnDetail', function(e) {
            e.preventDefault();
            var id = $(this).data('id');
            console.log("Detail button clicked for ID:", id);
            
            if (!id || id === 'undefined' || id === '') {
                alert("Invalid ID for detail view");
                return;
            }
            
            loadDetail(id);
        });

        function loadDetail(id_sales_invoice) {
            console.log("Loading detail for ID:", id_sales_invoice);
            $("#pageLoader").show();
            
            // Clear previous data
            $("#tblDetail tbody").html("<tr><td colspan='11' class='text-center'>Loading...</td></tr>");
            
            $.ajax({
                type: "POST",
                url: "GenerateJurnalInv.aspx/GetJurnalDetail",
                data: JSON.stringify({ id_sales_invoice: parseInt(id_sales_invoice) }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("Detail response:", response);
                    try {
                        var data = JSON.parse(response.d);
                        var html = "";
                        
                        if (data && data.length > 0) {
                            $.each(data, function (i, item) {
                                html += "<tr>";
                                html += "<td>" + (item.invoice_id || '') + "</td>";
                                html += "<td>" + (item.id || '') + "</td>";
                                html += "<td>" + (item.custom_id_detail || '') + "</td>";
                                html += "<td>" + (item.description || '') + "</td>";
                                html += "<td>" + (item.amount || '') + "</td>";
                                html += "<td>" + (item.rate || '') + "</td>";
                                html += "<td>" + (item.discount || '') + "</td>";
                                html += "<td>" + (item.quantity || '') + "</td>";
                                html += "<td>" + (item.product_id || '') + "</td>";
                                html += "<td>" + (item.product_name || '') + "</td>";
                                html += "<td>" + (item.line_tax_id || '') + "</td>";
                                html += "</tr>";
                            });
                        } else {
                            html = "<tr><td colspan='11' class='text-center'>No detail data found for ID: " + id_sales_invoice + "</td></tr>";
                        }
                        $("#tblDetail tbody").html(html);
                        $("#modalDetail").modal('show');
                    } catch (e) {
                        console.error("Error parsing detail response:", e);
                        console.error("Raw response:", response);
                        $("#tblDetail tbody").html("<tr><td colspan='11' class='text-center text-danger'>Error parsing detail data: " + e.message + "</td></tr>");
                        $("#modalDetail").modal('show');
                    }
                    $("#pageLoader").hide();
                },
                error: function (xhr, status, error) {
                    console.error("Detail AJAX Error:", error);
                    console.error("Status:", status);
                    console.error("Response Text:", xhr.responseText);
                    $("#tblDetail tbody").html("<tr><td colspan='11' class='text-center text-danger'>Error loading detail data: " + error + "</td></tr>");
                    $("#modalDetail").modal('show');
                    $("#pageLoader").hide();
                }
            });
        }

        // Save as Draft
        $("#btnSaveAsDraft").click(function () {
            if (selectedItems.length === 0) {
                alert("Please select items to save as draft.");
                return;
            }

            if (confirm("Are you sure you want to save selected items as draft?")) {
                $("#pageLoader").show();
                
                // Get selected invoice IDs
                var selectedIds = [];
                $(".chkItem:checked").each(function() {
                    var invoiceId = parseInt($(this).val()) || 0;
                    if (invoiceId > 0) {
                        selectedIds.push(invoiceId);
                    }
                });

                if (selectedIds.length === 0) {
                    alert("No valid invoice IDs selected for save as draft.");
                    $("#pageLoader").hide();
                    return;
                }

                // Convert array to comma-separated string
                var invoiceIdList = selectedIds.join(',');
                console.log("Sending invoice IDs for save as draft:", invoiceIdList);

                $.ajax({
                    type: "POST",
                    url: "GenerateJurnalInv.aspx/SaveAsDraftJurnalInvoice",
                    data: JSON.stringify({ invoiceIdList: invoiceIdList }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        try {
                            var result = JSON.parse(response.d);
                            console.log("Save as draft response:", result);
                            
                            if (result.status_code === 200) {
                                alert("Success: " + result.message);
                                loadJurnal(); // Reload data
                            } else {
                                alert("Error: " + result.message);
                            }
                        } catch (e) {
                            console.error("Error parsing save as draft response:", e);
                            alert("Error processing response: " + e.message);
                        }
                        $("#pageLoader").hide();
                    },
                    error: function (xhr, status, error) {
                        console.error("Save as draft AJAX error:", error);
                        console.error("Response text:", xhr.responseText);
                        alert("Error saving as draft: " + error);
                        $("#pageLoader").hide();
                    }
                });
            }
        });

        // Upload File
        $("#btnUploadFile").click(function () {
            $("#modalUpload").modal('show');
        });

        $("#btnProcessUpload").click(function () {
            var fileInput = $("#fileUpload")[0];
            if (!fileInput.files[0]) {
                alert("Please select a file to upload.");
                return;
            }

            var file = fileInput.files[0];
            
            // Validate file type
            var allowedTypes = ['.xlsx', '.xls'];
            var fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
            if (allowedTypes.indexOf(fileExtension) === -1) {
                alert("Please select a valid Excel file (.xlsx or .xls)");
                return;
            }

            // Validate file size (max 10MB)
            if (file.size > 10 * 1024 * 1024) {
                alert("File size must be less than 10MB");
                return;
            }

            var formData = new FormData();
            formData.append('file', file);

            $("#pageLoader").show();
            console.log("Uploading file:", file.name, "Size:", file.size, "bytes");
            
            $.ajax({
                type: "POST",
                url: "UploadJurnalInvoice.ashx",
                data: formData,
                processData: false,
                contentType: false,
                dataType: "json",
                success: function (response) {
                    try {
                        var payload = (response && typeof response.d !== "undefined") ? response.d : response;
                        var result = (typeof payload === "string") ? JSON.parse(payload) : payload;
                        console.log("Upload response:", result);

                        if (result && result.data) {
                            alert("File uploaded successfully! Found " + result.data.length + " records.");
                            $("#modalUpload").modal('hide');
                            // You can process the uploaded data here
                            loadJurnal(); // Reload data to show uploaded items
                        } else if (result && result.error) {
                            alert("Upload error: " + result.error);
                        } else {
                            alert("Upload completed but no data returned");
                        }
                    } catch (e) {
                        console.error("Error parsing upload response:", e);
                        console.error("Raw response:", response);
                        alert("Error processing upload response: " + e.message);
                    }
                    $("#pageLoader").hide();
                },
                error: function (xhr, status, error) {
                    console.error("Upload AJAX error:", error);
                    console.error("Status:", status);
                    console.error("Response text:", xhr.responseText);
                    alert("Error uploading file: " + error);
                    $("#pageLoader").hide();
                }
            });
        });

        $("#btnGenerateInvoice").click(function () {
            loadJurnal();
            loadJurnalNew(); // Also load the new table
        });

        $("#btnCreateSalesOrderJurnal").click(function () {
            // collect selected invoice IDs from the 'List Jurnal Invoice' (new) table
            var hasData = $("#tblJurnalNew tbody tr td input.chkItemNew").length > 0;
            var selectedIds = [];
            $(".chkItemNew:checked").each(function () {
                var invoiceId = parseInt($(this).val()) || 0;
                if (invoiceId > 0) selectedIds.push(invoiceId);
            });

            if (!hasData) {
                alert("Tidak ada data pada List Jurnal Invoice.");
                return;
            }

            if (selectedIds.length === 0) {
                alert("Silakan pilih data pada List Jurnal Invoice.");
                return;
            }

            if (!confirm("Create Sales Order in Jurnal untuk " + selectedIds.length + " data terpilih?")) {
                return;
            }

            $("#pageLoader").show();
            $.ajax({
                type: "POST",
                url: "GenerateJurnalInv.aspx/CreateSalesOrderInJurnal",
                data: JSON.stringify({ invoiceIds: selectedIds }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    try {
                        var result = JSON.parse(response.d);
                        if (result.status_code === 200) {
                            var message = "Success: " + result.message;
                            if (result.invoice_count) {
                                message += "\nProcessed " + result.invoice_count + " invoice(s).";
                            }
                            alert(message);
                            
                            // Reload data untuk update status sync
                            loadJurnalNew();
                        } else {
                            alert("Error: " + result.message);
                        }
                    } catch (e) {
                        console.error("Error parsing CreateSalesOrderInJurnal response:", e);
                        console.error("Raw response:", response);
                        alert("Error processing response: " + e.message);
                    }
                    $("#pageLoader").hide();
                },
                error: function (xhr, status, error) {
                    console.error("CreateSalesOrderInJurnal AJAX error:", error);
                    console.error("Response text:", xhr.responseText);
                    alert("Error creating Sales Order in Jurnal: " + error);
                    $("#pageLoader").hide();
                }
            });
        });

        $("#btnLoadNewTable").click(function () {
            loadJurnalNew();
        });

        $("#btnClear").click(function () {
            $("#<%= txtSearch.ClientID %>").val("");
            $("#tblJurnal tbody").html("<tr><td colspan='36' class='text-center'>Click 'Generate Invoice' to load data.</td></tr>");
            $("#tblJurnalNew tbody").html("<tr><td colspan='33' class='text-center'>Click 'Load New Table' to load data.</td></tr>");
        });

        $("#btnExportExcel").click(function () {
            // Check if there's data to export
            if ($("#tblJurnal tbody tr").length > 0 && !$("#tblJurnal tbody tr").hasClass("text-center")) {
                window.location.href = 'GenerateJurnalInv.aspx?export=1';
            } else {
                alert("No data available for export. Please search for data first.");
            }
        });
        
        // Enter key on search field
        $("#<%= txtSearch.ClientID %>").keypress(function(e) {
            if(e.which == 13) { // Enter key
                $("#btnGenerateInvoice").click();
            }
        });
    });

    
</script>

</asp:Content>
