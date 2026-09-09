<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stock_opname_details.aspx.cs" Inherits="vtsadm.stock_opname_details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="Content/bower_components/Ionicons/css/ionicons.min.css" />
    <!-- daterange picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" />
    <!-- DataTables -->
    <link rel="stylesheet" href="Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="Content/plugins/iCheck/all.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="Content/bower_components/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.min.css" />
    <!-- Bootstrap time Picker -->
    <link rel="stylesheet" href="Content/plugins/timepicker/bootstrap-timepicker.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="Content/bower_components/select2/dist/css/select2.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
           folder instead of downloading all of them to reduce the load. -->
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
    <link rel="stylesheet" href="Content/dist/css/skins/_all-skins.min.css" />
    <link rel="stylesheet" href="Content/paginationcs.css" />
    <link rel="stylesheet" href="Content/loader.css" />
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />

    <style type="text/css">
        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        }
        
        .btn-success, .btn-danger {
            transition: opacity 0.5s ease-in-out;
        }
        
        .loading-indicator {
            display: none;
            text-align: center;
            padding: 10px;
            background-color: #f9f9f9;
            border-radius: 4px;
            margin-bottom: 10px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        
        .notification-msg {
            padding: 10px;
            border-radius: 4px;
            margin-top: 15px;
            margin-bottom: 15px;
            display: none;
        }
        
        .notification-msg.success {
            background-color: #dff0d8;
            border: 1px solid #d6e9c6;
            color: #3c763d;
        }
        
        .notification-msg.error {
            background-color: #f2dede;
            border: 1px solid #ebccd1;
            color: #a94442;
        }
        
        /* Tambahan untuk modal fix */
        .modal-force-close {
            position: absolute;
            top: 10px;
            right: 50px;
            z-index: 9999;
        }
    </style>

    <script type="text/javascript" src="Content/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="Content/bower_components/jquery-ui/jquery-ui.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script type="text/javascript" src="Content/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <!-- Tambahkan loading indicator -->
        <div id="loadingIndicator" class="loading-indicator">
            <i class="fa fa-spinner fa-spin"></i> Processing...
        </div>
        
        <!-- Notifikasi -->
        <div id="notificationSuccess" class="notification-msg success">
            <i class="fa fa-check-circle"></i> <span id="successMessage">Operation completed successfully!</span>
        </div>
        
        <div id="notificationError" class="notification-msg error">
            <i class="fa fa-exclamation-circle"></i> <span id="errorMessage">An error occurred!</span>
        </div>
        
        <div class="box-body">
            <div class="form-group form-group-sm">
                <div class="input-group input-group-sm">
                    <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-left" placeholder="Search by Serial Number ..."></asp:TextBox>
                    <span class="input-group-btn">
                        <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                    </span>
                </div>
            </div>
            <div class="form-group form-group-sm">
                <asp:Panel runat="server" ScrollBars="Auto" Height="250px">
                    <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered table-hover" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="NoSN" HeaderText="Serial Number" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="current_status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="current_status_desc" HeaderText="Status Desc" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="DeviceTypeName" HeaderText="Type" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:BoundField DataField="last_update" HeaderText="Last Update" ItemStyle-Wrap="false"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="CmdSelect" runat="server" ToolTip="Select" Text="<i class='fa fa-check-circle'></i>" Enabled="true" CssClass="btn btn-success btn-xs" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle ForeColor="#003481" BackColor="White" />
                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                        <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                    </asp:GridView>
                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                        <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                </asp:Panel>
            </div>

            <div class="form-group form-group-sm">
                <input id="txtDeviceID" runat="server" type="hidden" />
                <input id="hiddenCurrentStatus" type="hidden" name="hiddenCurrentStatus" value="" />
                <input id="hiddenNoSN" type="hidden" name="hiddenNoSN" value="" />
                <label>Serial Number (SN)</label>
                <asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="Serial Number ..." ReadOnly="false"></asp:TextBox>
            </div>

            <div class="form-group form-group-sm">
                <label>Current Status</label>
                <asp:TextBox ID="txtCurrentStatus" runat="server" class="form-control" placeholder="Current Status in System..." ReadOnly="true"></asp:TextBox>
            </div>

            <div class="form-group form-group-sm">
                <label>Physical Check</label>
                <asp:DropDownList ID="CmbPhysicalFound" runat="server" CssClass="form-control" onchange="togglePhysicalStatus();">
                    <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                    <asp:ListItem Text="Found" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Not Found" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group form-group-sm" id="divPhysicalStatus" style="display:none;">
                <label>Physical Status</label>
                <asp:DropDownList ID="CmbPhysicalStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                    <asp:ListItem Text="Register (RG)" Value="RG"></asp:ListItem>
                    <asp:ListItem Text="Mutation to Warehouse (MW)" Value="MW"></asp:ListItem>
                    <asp:ListItem Text="Mutation to Technician (MT)" Value="MT"></asp:ListItem>
                    <asp:ListItem Text="Install (IS)" Value="IS"></asp:ListItem>
                    <asp:ListItem Text="Delete (DE)" Value="DE"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group form-group-sm">
                <label>Remarks</label>
                <asp:TextBox ID="txtRemarks" runat="server" class="form-control" placeholder="Remarks..." TextMode="MultiLine" Rows="2"></asp:TextBox>
            </div>

        </div>
        <div class="box-footer">
            <asp:Button ID="Button1" CssClass="btn btn-default" runat="server" OnClick="CmdClearDetail_Click" Text="Clear" />
            <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" OnClientClick="return validateAndShowLoading();" OnClick="CmdSaveDetail_Click" Text="Save" />
            <label runat="server" id="lblMsg"></label>
        </div>

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="closeModalCompletely('#modal-messagebox');" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="closeModalCompletely('#modal-messagebox');">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <!-- Tombol force close untuk modal -->
        <button type="button" id="forceCloseButton" class="btn btn-danger modal-force-close" style="display:none;">
            Force Close Modal
        </button>

        <script type="text/javascript">
            // Fungsi validasi form dan menampilkan loading
            function validateAndShowLoading() {
                var deviceId = document.getElementById('txtDeviceID').value;
                var physicalFound = document.getElementById('CmbPhysicalFound').value;
                var physicalStatus = document.getElementById('CmbPhysicalStatus').value;

                if (deviceId.trim() === "") {
                    showNotification('error', 'Please select a device first');
                    return false;
                }

                if (physicalFound === "[Select]") {
                    showNotification('error', 'Please select whether the device is found or not');
                    return false;
                }

                if (physicalFound === "1" && physicalStatus === "[Select]") {
                    showNotification('error', 'Please select the physical status of the device');
                    return false;
                }

                // Tampilkan loading indicator
                document.getElementById("loadingIndicator").style.display = "block";
                return true;
            }

            // Fungsi untuk menampilkan notifikasi
            function showNotification(type, message) {
                // Sembunyikan semua notifikasi terlebih dahulu
                hideNotifications();

                if (type === 'success') {
                    document.getElementById('successMessage').innerText = message;
                    var notification = document.getElementById('notificationSuccess');
                    notification.style.display = 'block';
                } else {
                    document.getElementById('errorMessage').innerText = message;
                    var notification = document.getElementById('notificationError');
                    notification.style.display = 'block';
                }

                // Otomatis sembunyikan setelah 5 detik
                setTimeout(function () {
                    hideNotifications();
                }, 5000);
            }

            // Fungsi untuk menyembunyikan semua notifikasi
            function hideNotifications() {
                document.getElementById('notificationSuccess').style.display = 'none';
                document.getElementById('notificationError').style.display = 'none';
            }

            // Fungsi untuk menutup modal dengan sempurna
            function closeModalCompletely(modalId) {
                console.log("Closing modal completely: " + modalId);

                // Hentikan event
                if (event) event.preventDefault();

                // Tutup modal
                $(modalId).modal('hide');

                // Pastikan backdrop dan class modal-open dihapus
                setTimeout(function () {
                    $('.modal-backdrop').remove();
                    $('body').removeClass('modal-open');
                    $('body').css('padding-right', '');

                    // Sembunyikan modal dengan jQuery dan secara native
                    $(modalId).hide();
                    if (document.querySelector(modalId)) {
                        document.querySelector(modalId).style.display = 'none';
                    }
                }, 100);

                return false;
            }

            // Fungsi untuk menutup modal parent dengan sempurna
            function closeParentModal() {
                if (window.parent && window.parent.$) {
                    console.log("Closing parent modal");

                    // Tutup modal parent
                    window.parent.$('#modal-details').modal('hide');

                    // Hapus backdrop dan class modal-open
                    setTimeout(function () {
                        window.parent.$('.modal-backdrop').remove();
                        window.parent.$('body').removeClass('modal-open');
                        window.parent.$('body').css('padding-right', '');
                        window.parent.$('#modal-details').hide();

                        // Refresh data parent
                        if (window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad')) {
                            window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad').click();
                        }
                    }, 300);
                }
            }

            function postDetails(sDeviceID, sNoSN, sCurrentStatus, sCurrentStatusDesc) {
                if (sDeviceID != '') {
                    console.log("Setting details - DeviceID: " + sDeviceID + ", SN: " + sNoSN + ", Status: " + sCurrentStatus);

                    // Simpan ID perangkat
                    document.getElementById('txtDeviceID').value = sDeviceID;

                    // Simpan nomor seri di text field AND hidden field untuk cadangan
                    document.getElementById('txtNoSN').value = sNoSN;
                    document.getElementById('hiddenNoSN').value = sNoSN;

                    // Simpan nilai status dengan format yang konsisten
                    document.getElementById('txtCurrentStatus').value = sCurrentStatusDesc + ' (' + sCurrentStatus + ')';
                    // Simpan status asli di hidden field
                    document.getElementById('hiddenCurrentStatus').value = sCurrentStatus;

                    document.getElementById('CmbPhysicalFound').value = '[Select]';
                    document.getElementById('CmbPhysicalStatus').value = '[Select]';
                    document.getElementById('txtRemarks').value = '';
                    togglePhysicalStatus();

                    // Debug - tambahkan log untuk memastikan
                    console.log("Values set - SN field: " + document.getElementById('txtNoSN').value);
                    console.log("Values set - Hidden SN: " + document.getElementById('hiddenNoSN').value);

                    // Tampilkan notifikasi
                    showNotification('success', 'Device selected successfully: ' + sNoSN);
                }
            }

            function togglePhysicalStatus() {
                var physicalFound = document.getElementById('CmbPhysicalFound').value;
                var divPhysicalStatus = document.getElementById('divPhysicalStatus');

                if (physicalFound == '1') {
                    divPhysicalStatus.style.display = 'block';
                } else {
                    divPhysicalStatus.style.display = 'none';
                    document.getElementById('CmbPhysicalStatus').value = '[Select]';
                }
            }

            // Tambahan fungsi untuk memaksa menutup modal
            function forceCloseAllModals() {
                console.log("Force closing all modals");

                // Sembunyikan semua modal
                $('.modal').modal('hide');

                // Hapus semua backdrop
                $('.modal-backdrop').remove();

                // Reset body
                $('body').removeClass('modal-open').css('padding-right', '');

                // Sembunyikan modal secara langsung
                $('.modal').hide();

                // Notifikasi ke parent window untuk refresh
                if (window.parent && window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad')) {
                    window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad').click();
                }

                return false;
            }

            $(document).ready(function () {
                // Tambahkan tombol force close 
                $('#forceCloseButton').on('click', function () {
                    forceCloseAllModals();
                });

                // Tambahkan listener ESC key
                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) { // ESC key
                        console.log("ESC key pressed");
                        $('#forceCloseButton').show();
                        setTimeout(function () {
                            forceCloseAllModals();
                        }, 300);
                    }
                });

                // Perbaikan handler untuk tombol close modal
                $('.modal .close').on('click', function () {
                    var modalId = '#' + $(this).closest('.modal').attr('id');
                    closeModalCompletely(modalId);
                    return false;
                });

                // Setup message display and hide
                var lbMsg = document.getElementById('lblMsg');
                if (lbMsg && lbMsg.innerHTML != '') {
                    if (lbMsg.innerHTML.includes("Success")) {
                        lbMsg.className = "btn btn-success";

                        // Tampilkan notifikasi sukses
                        var successMsg = lbMsg.innerHTML.replace("<strong>Success!</strong> ", "");
                        showNotification('success', successMsg);

                        // Jika sukses, tutup modal parent setelah delay
                        setTimeout(function () {
                            closeParentModal();
                        }, 1500);
                    }
                    else {
                        lbMsg.className = "btn btn-danger";

                        // Tampilkan notifikasi error
                        var errorMsg = lbMsg.innerHTML.replace("<strong>Failed!</strong> ", "");
                        showNotification('error', errorMsg);
                    }

                    // Hilangkan loading indicator
                    document.getElementById("loadingIndicator").style.display = "none";

                    // Auto hide message after delay
                    setTimeout(function () {
                        $(lbMsg).fadeTo(500, 0).slideUp(500, function () {
                            $(this).html('').removeClass().addClass('').slideDown(0).fadeTo(0, 1);
                        });
                    }, 3000);
                }

                // Handle submit lagi dengan confirm
                $("#Button2").click(function () {
                    if (!validateAndShowLoading()) {
                        return false;
                    }
                });

                // Pastikan loading indicator tersembunyi saat awal
                document.getElementById("loadingIndicator").style.display = "none";

                // Sembunyikan notifikasi saat awal
                hideNotifications();
            });

            // Ensure physical status visibility is set on page load
            document.addEventListener('DOMContentLoaded', function () {
                togglePhysicalStatus();
            });

            // Pastikan event handler untuk parent window berjalan dengan baik
            if (window.parent && window.parent.$) {
                window.parent.$('#modal-details').on('hidden.bs.modal', function () {
                    // Pastikan backdrop dan class modal-open dihapus
                    window.parent.$('.modal-backdrop').remove();
                    window.parent.$('body').removeClass('modal-open');
                    window.parent.$('body').css('padding-right', '');

                    // Refresh data parent
                    if (window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad')) {
                        window.parent.document.getElementById('ContentPlaceHolder1_CmdLoad').click();
                    }
                });
            }
        </script>
    </form>
</body>
</html>