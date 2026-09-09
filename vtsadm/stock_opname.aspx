<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="stock_opname.aspx.cs" Inherits="vtsadm.stock_opname" EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Stock Opname <small>Device</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Inventory</a></li>
            <li class="active">Stock Opname Device</li>
        </ol>
    </section>

    <section class="content">
        <!-- Loading indicator yang lebih baik -->
        <div id="pageLoadingIndicator" style="display:none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.5); z-index: 9999;">
            <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white; padding: 20px; border-radius: 8px; box-shadow: 0 5px 15px rgba(0,0,0,0.3); text-align: center; min-width: 200px;">
                <div class="loading-spinner" style="margin: 0 auto 15px auto; width: 50px; height: 50px; border: 5px solid #f3f3f3; border-top: 5px solid #3c8dbc; border-radius: 50%; animation: spin 1s linear infinite;"></div>
                <h4 style="margin: 0; color: #444; font-size: 18px;">Processing request...</h4>
            </div>
        </div>
        
        <!-- Emergency button untuk menutup modal -->
        <button type="button" id="emergencyCloseButton" class="btn btn-danger" 
                style="position: fixed; top: 10px; right: 10px; z-index: 9999; display: none;">
            Force Close All Modals
        </button>
        
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Location Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Location Type</label>
                            <asp:DropDownList ID="CmbLocationType" runat="server" CssClass="form-control" onchange="return updateLocationID(this);">
                                <asp:ListItem Text="[Select]" Value="[Select]"></asp:ListItem>
                                <asp:ListItem Text="Warehouse" Value="WAREHOUSE"></asp:ListItem>
                                <asp:ListItem Text="Technician" Value="TECHNICIAN"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Location ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtLocationID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-location"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Location Name</label>
                            <asp:TextBox ID="txtLocationName" runat="server" class="form-control" placeholder="Location Name ..." required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail Device</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered table-hover" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting" DataKeyNames="detail_id">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="detail_id" HeaderText="Detail ID" ItemStyle-Wrap="false" SortExpression="detail_id"></asp:BoundField>
                                        <asp:BoundField DataField="device_id" HeaderText="Device ID" ItemStyle-Wrap="false" SortExpression="device_id"></asp:BoundField>
                                        <asp:BoundField DataField="nosn" HeaderText="SN" ItemStyle-Wrap="false" SortExpression="nosn"></asp:BoundField>
                                        <asp:BoundField DataField="current_status_desc" HeaderText="System Status" ItemStyle-Wrap="false" SortExpression="current_status_desc"></asp:BoundField>
                                        <asp:BoundField DataField="physical_found_desc" HeaderText="Found" ItemStyle-Wrap="false" SortExpression="physical_found_desc"></asp:BoundField>
                                        <asp:BoundField DataField="physical_status_desc" HeaderText="Physical Status" ItemStyle-Wrap="false" SortExpression="physical_status_desc"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-trash'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="current_status" HeaderText="Current Status" ItemStyle-Wrap="false" SortExpression="current_status"></asp:BoundField>
                                        <asp:BoundField DataField="physical_found" HeaderText="PhysicalFound" ItemStyle-Wrap="false" SortExpression="physical_found"></asp:BoundField>
                                        <asp:BoundField DataField="physical_status" HeaderText="PhysicalStatus" ItemStyle-Wrap="false" SortExpression="physical_status"></asp:BoundField>
                                        <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Wrap="false" SortExpression="remarks"></asp:BoundField>
                                        <asp:BoundField DataField="status_match" HeaderText="Status Match" ItemStyle-Wrap="false" SortExpression="status_match"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="True" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px;margin-left:10px;"><asp:Label id="LblPagingDetail" runat="server" style="color: #003481;font-style:italic;font-size:13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Header Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Opname ID</label>
                            <asp:TextBox ID="txtOpnameID" runat="server" class="form-control" placeholder="Skip for new opname ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Opname Date</label>
                            <input type="date" id="txtOpnameDate" runat="server" class="form-control" placeholder="Opname Date ..."/>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..." TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-default" runat="server" onserverclick="CmdClear_Click"><i class="fa fa-eraser"></i> Clear</button>
                        <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click"><i class="fa fa-save"></i> Create</button>
                        <button id="CmdAddDetail" type="button" class="btn btn-success" runat="server" onclick="return showDetails();"><i class="fa fa-plus"></i> Add Details</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-warning" runat="server" OnClientClick="confirmSubmit(); return false;" Text="Submit" />
                        <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility:hidden;" runat="server" onserverclick="CmdLoad_Click">1</button>                        
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Stock Opname</h3>
                        <div class="box-tools">
                            <div class="input-group input-group-sm" style="width:200px;">
                                <input type="text" id="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by any fields ..." />
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>                                                     
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered table-hover" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView1_RowDeleting" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" OnSorting="GridView1_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="opname_id" HeaderText="Opname ID" ItemStyle-Wrap="false" SortExpression="opname_id"></asp:BoundField>
                                        <asp:BoundField DataField="sOpnameDate" HeaderText="Opname Date" ItemStyle-Wrap="false" SortExpression="sOpnameDate"></asp:BoundField>
                                        <asp:BoundField DataField="location_type" HeaderText="Location Type" ItemStyle-Wrap="false" SortExpression="location_type"></asp:BoundField>
                                        <asp:BoundField DataField="LocationName" HeaderText="Location Name" ItemStyle-Wrap="false" SortExpression="LocationName"></asp:BoundField>
                                        <asp:BoundField DataField="TotalDevice" HeaderText="Total Device" ItemStyle-Wrap="false" SortExpression="TotalDevice"></asp:BoundField>
                                        <asp:BoundField DataField="FoundDevice" HeaderText="Found" ItemStyle-Wrap="false" SortExpression="FoundDevice"></asp:BoundField>
                                        <asp:BoundField DataField="created_by" HeaderText="Created By" ItemStyle-Wrap="false" SortExpression="created_by"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-trash'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="location_id" HeaderText="Location ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NotFoundDevice" HeaderText="Not Found" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MismatchDevice" HeaderText="Mismatch" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px;margin-left:10px;"><asp:Label id="LblPagingHeader" runat="server" style="color: #003481;font-style:italic;font-size:13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-submit">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to submit Opname ID :&nbsp;</h6><label id="LblOpnameIDSubmit" runat="server"></label>&nbsp;?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesSubmit();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="cleanupModal('#modal-submit');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete-detail">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete detail ID :&nbsp;</h6><label id="LblDetailID" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtDetailIDDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesDetail();" onserverclick="CmdYesDetail_ServerClick" id="CmdYesDetail">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="cleanupModal('#modal-delete-detail');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete-header">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Opname ID :&nbsp;</h6><label id="LblOpnameID" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtOpnameIDDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYes();" onserverclick="CmdYes_ServerClick" id="CmdYes">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="cleanupModal('#modal-delete-header');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-location">
            <div class="modal-dialog modal-lg" style="width: 90%; max-width: 900px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Location</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="stock_opname_location_search.aspx" style="width: 100%; border: none; height: 500px; overflow:hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="cleanupModal('#modal-location');">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- /.modal -->
        <div class="modal modal-open fade" id="modal-details" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog" style="width: 90%; max-width: 900px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"> 
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title">Add Device Check</h4>
  se
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="about:blank" style="width: 100%; border: none; height: 800px; overflow:hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="cleanupModal('#modal-messagebox');" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="cleanupModal('#modal-messagebox');">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </section>

    <style>
    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }

    .loading-spinner {
        display: inline-block;
        width: 50px;
        height: 50px;
        border: 5px solid rgba(240, 240, 240, 0.8);
        border-radius: 50%;
        border-top-color: #3c8dbc;
        animation: spin 1s ease-in-out infinite;
    }
    </style>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        // Fungsi untuk menampilkan dan menyembunyikan loader dengan animasi
        function showLoader() {
            $('#pageLoadingIndicator').fadeIn(300);
        }

        function hideLoader() {
            $('#pageLoadingIndicator').fadeOut(300);
        }

        // Show/hide loading indicator
        prm.add_beginRequest(function () {
            showLoader();
        });

        prm.add_endRequest(function (sender, args) {
            hideLoader();

            // Tangani jika ada error di response
            if (args && args.get_error() != undefined) {
                // Notifikasi error ke user
                var errorMessage = args.get_error().message;
                $('#pageLoadingIndicator').hide();
                alert("Error occurred: " + errorMessage);
                args.set_errorHandled(true);
            }

            checkAndCleanupModals();
        });

        // Fungsi untuk membersihkan modal dengan sempurna
        function cleanupModal(modalId) {
            // Hentikan event propagation
            if (event) {
                event.preventDefault();
                event.stopPropagation();
            }

            console.log("Cleaning up modal: " + modalId);

            // Tutup modal
            $(modalId).modal('hide');

            // Hapus semua backdrop
            setTimeout(function () {
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open');
                $('body').css('padding-right', '');
                $(modalId).hide();
            }, 300);

            return false;
        }

        // Perbaikan untuk menutup modal dengan paksa
        function forceCloseModal(modalId) {
            console.log("Force closing modal: " + modalId);

            // Tampilkan loading dulu
            showLoader();

            // Tutup modal dengan jQuery
            $(modalId).modal('hide');

            // Hapus backdrop dan class modal secara paksa
            setTimeout(function () {
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css('padding-right', '');
                $(modalId).hide();

                // Refresh data grid jika diperlukan
                if (document.getElementById('ContentPlaceHolder1_CmdLoad')) {
                    document.getElementById('ContentPlaceHolder1_CmdLoad').click();
                }

                // Sembunyikan loader setelah proses selesai
                setTimeout(function () {
                    hideLoader();
                }, 500);
            }, 100);
        }

        // Periksa dan bersihkan modal yang tersisa
        function checkAndCleanupModals() {
            // Jika tidak ada modal yang sedang terbuka tapi masih ada backdrop, bersihkan
            if ($('.modal:visible').length === 0 && $('.modal-backdrop').length > 0) {
                console.log("Cleaning up leftover modal backdrops");
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open');
                $('body').css('padding-right', '');
            }
        }

        function postLocationChild(sLocationID, sLocationName, sLocationType) {
            if (sLocationID != '') {
                document.getElementById('ContentPlaceHolder1_txtLocationID').value = sLocationID;
                document.getElementById('ContentPlaceHolder1_txtLocationName').value = sLocationName;
                document.getElementById('ContentPlaceHolder1_CmbLocationType').value = sLocationType;
                cleanupModal('#modal-location');
            }
        }

        function updateLocationID(objDD) {
            document.getElementById('ContentPlaceHolder1_txtLocationID').value = "";
            document.getElementById('ContentPlaceHolder1_txtLocationName').value = "";
            return false;
        }

        function showDetails() {
            var opnameId;
            opnameId = document.getElementById('ContentPlaceHolder1_txtOpnameID');

            if (opnameId.value != '') {
                // Reset iframe content first
                var iframe = document.getElementById('iframedetails');
                iframe.src = 'about:blank';

                // Setelah delay, muat URL yang sebenarnya - ini membantu mencegah masalah caching
                setTimeout(function () {
                    iframe.src = 'stock_opname_details.aspx?t=' + new Date().getTime();

                    // Tampilkan modal dengan pengaturan yang benar
                    $('#modal-details').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                }, 100);
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create stock opname header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }

        function buttonYesSubmit() {
            cleanupModal('#modal-submit');
            // Tampilkan indikator loading
            showLoader();
        }

        function confirmSubmit() {
            var objOpnameID = document.getElementById('ContentPlaceHolder1_txtOpnameID');
            if (objOpnameID.value != '') {
                document.getElementById('ContentPlaceHolder1_LblOpnameIDSubmit').innerHTML = objOpnameID.value;
                $("#modal-submit").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        }

        function buttonYesDetail() {
            cleanupModal('#modal-delete-detail');
            // Tampilkan indikator loading
            showLoader();
            // Tambahkan pesan info untuk memberi tahu pengguna tentang pemrosesan
            document.getElementById('ContentPlaceHolder1_div_comment').innerHTML =
                '<div class="alert alert-info" role="alert"><i class="fa fa-spinner fa-spin"></i> Processing delete request...</div>';
        }

        function confirmDeleteDetail(sText) {
            if (sText != '') {
                console.log("Confirming delete for detail ID: " + sText);
                document.getElementById('ContentPlaceHolder1_LblDetailID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtDetailIDDelete').value = sText;
                $("#modal-delete-detail").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        }

        function buttonYes() {
            cleanupModal('#modal-delete-header');
            // Tampilkan indikator loading
            showLoader();
        }

        function confirmDelete(sText) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblOpnameID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtOpnameIDDelete').value = sText;
                $("#modal-delete-header").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        }

        function endRequest(sender, args) {
            // Sembunyikan indikator loading
            hideLoader();

            // Periksa dan bersihkan modal yang tersisa
            checkAndCleanupModals();

            // Atur ulang event handler untuk modal
            setupModalHandlers();

            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }

        function setupModalHandlers() {
            // Reset handler untuk modal details
            $('#modal-details').off('hidden.bs.modal').on('hidden.bs.modal', function () {
                console.log("Modal details closed - refreshing data");
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                if (objLoad) {
                    setTimeout(function () {
                        objLoad.click();
                    }, 300);
                }

                // Pastikan modal benar-benar tertutup
                checkAndCleanupModals();

                // Reset iframe untuk memastikan reload saat dibuka kembali
                var iframe = document.getElementById('iframedetails');
                if (iframe) {
                    iframe.src = 'about:blank';
                }
            });

            // Fix untuk modal details
            $('#modal-details').off('hide.bs.modal').on('hide.bs.modal', function () {
                cleanupModal('#modal-details');
                return false; // Hentikan pengaturan bawaan hide
            });

            // Fix untuk modal messagebox
            $('#modal-messagebox').off('hidden.bs.modal').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
                checkAndCleanupModals();
            });

            // Fix untuk modal lainnya
            $('.modal').each(function () {
                var modalId = '#' + $(this).attr('id');
                $(this).off('hidden.bs.modal').on('hidden.bs.modal', function () {
                    checkAndCleanupModals();
                });
            });

            // Tambahkan handler untuk tombol close
            $('.modal .close').off('click').on('click', function () {
                var modalId = '#' + $(this).closest('.modal').attr('id');
                cleanupModal(modalId);
                return false;
            });

            // Perbaikan untuk tombol close di modal details
            $('#modal-details .close').off('click').on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();

                showLoader();

                // Tutup modal langsung tanpa memanggil cleanupModal
                $('#modal-details').hide();

                // Hapus backdrop secara manual
                setTimeout(function () {
                    $('.modal-backdrop').remove();
                    $('body').removeClass('modal-open').css('padding-right', '');

                    // Refresh data
                    var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                    if (objLoad) {
                        objLoad.click();
                    }

                    setTimeout(function () {
                        hideLoader();
                    }, 500);
                }, 300);

                return false;
            });

            // Perbaikan untuk tombol Force Close
            $('#btnForceClose').off('click').on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();

                showLoader();

                // Tutup modal secara langsung
                $('#modal-details').hide();
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css('padding-right', '');

                // Refresh data
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                if (objLoad) {
                    setTimeout(function () {
                        objLoad.click();

                        setTimeout(function () {
                            hideLoader();
                        }, 500);
                    }, 100);
                } else {
                    hideLoader();
                }

                return false;
            });
        }

        // Handler untuk tombol emergency close
        function setupEmergencyCloseButton() {
            $('#emergencyCloseButton').off('click').on('click', function () {
                showLoader();

                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css('padding-right', '');
                $('.modal').hide();

                // Refresh data
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                if (objLoad) {
                    setTimeout(function () {
                        objLoad.click();

                        setTimeout(function () {
                            hideLoader();
                        }, 500);
                    }, 300);
                } else {
                    hideLoader();
                }

                // Sembunyikan tombol
                $(this).hide();

                return false;
            });

            // Tambahkan listener ESC key
            $(document).off('keydown.emergency').on('keydown.emergency', function (e) {
                if (e.keyCode === 27) { // ESC key
                    // Jika ada modal terbuka tapi terjebak, tampilkan tombol emergency
                    if ($('.modal:visible').length > 0 || $('.modal-backdrop').length > 0) {
                        $('#emergencyCloseButton').show();
                    }
                }
            });
        }

        // Initial load - make sure we call the load button to refresh GridView2
        $(document).ready(function () {
            var opnameId = document.getElementById('ContentPlaceHolder1_txtOpnameID');
            if (opnameId && opnameId.value != '') {
                console.log("Document ready - refreshing data for opname ID: " + opnameId.value);
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                if (objLoad) {
                    objLoad.click();
                }
            }

            // Initialize modal handling
            setupModalHandlers();

            // Initialize emergency close button
            setupEmergencyCloseButton();

            // Inisialisasi datepicker dengan tanggal saat ini jika kosong
            var dateField = document.getElementById('ContentPlaceHolder1_txtOpnameDate');
            if (dateField && dateField.value === '') {
                var today = new Date();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0');
                var yyyy = today.getFullYear();
                dateField.value = yyyy + '-' + mm + '-' + dd;
            }

            // Perbaikan untuk modal yang tidak bisa ditutup
            $(document).on('click', '.modal-backdrop', function () {
                checkAndCleanupModals();
            });

            // Safety timeout untuk loader
            $(document).ajaxStart(function () {
                showLoader();
            }).ajaxStop(function () {
                hideLoader();
            }).ajaxError(function () {
                hideLoader();
            });

            // Safety timeout - jika loader tidak berhenti dalam 10 detik, paksa berhenti
            $(document).on('click', 'button, input[type="button"], input[type="submit"]', function () {
                setTimeout(function () {
                    if ($('#pageLoadingIndicator').is(':visible')) {
                        hideLoader();
                    }
                }, 10000);
            });
        });

        // Pastikan modal backdrop dibersihkan saat halaman dimuat
        $(window).on('load', function () {
            checkAndCleanupModals();
        });

        endRequest();
    </script>
</asp:Content>