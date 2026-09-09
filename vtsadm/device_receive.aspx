<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="device_receive.aspx.cs" CodeBehind="device_receive.aspx.cs" Inherits="vtsadm.device_receive" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device
            <small>Receive</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">Receive</li>
        </ol>
    </section>

    <section class="content">
        <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="CmdConfirmReceive" />
                <asp:PostBackTrigger ControlID="CmdYesSubmitHeader" />
                <asp:PostBackTrigger ControlID="CmdYesReceive" />
            </Triggers>
            <ContentTemplate>
        <!-- Top: List Delivery -->
        <asp:UpdatePanel ID="UpdatePanelListDelivery" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Delivery</h3>
                        <div class="box-tools" style="width: 200px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="CmdSearch" runat="server" type="button" class="btn btn-primary" OnClick="CmdSearch_ServerClick" Text="Search" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:Panel runat="server" ScrollBars="Auto" style="max-height: 400px; overflow-y: auto;">
                            <asp:GridView ID="GridViewHeader" runat="server" BackColor="WhiteSmoke" Font-Size="Small" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="False"
                                EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10"
                                OnRowCommand="GridViewHeader_RowCommand" OnPageIndexChanging="GridViewHeader_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="DeliveryID" HeaderText="Delivery ID" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                    <asp:BoundField DataField="WarehouseFromName" HeaderText="From" />
                                    <asp:BoundField DataField="WarehouseToName" HeaderText="To" />
                                    <asp:BoundField DataField="ExpedisiName" HeaderText="Expedisi" />
                                    <asp:BoundField DataField="ExpedisiCode" HeaderText="Service" />
                                    <asp:BoundField DataField="ExpedisiResiNo" HeaderText="Resi No" />
                                    <asp:BoundField DataField="ReceiveProgress" HeaderText="Receive" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info btn-xs" Text="Select" CommandName="Select" />
                                </Columns>
                                <RowStyle ForeColor="#003481" BackColor="White" />
                                <PagerStyle CssClass="pagination-ys" />
                                <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                <HeaderStyle CssClass="pagination-ys" />
                                <AlternatingRowStyle BackColor="#f9f9f9" />
                            </asp:GridView>
                            <asp:Label ID="LblPaging" runat="server" CssClass="text-muted small"></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Card: Not Receive All (multi-detail, some received, some pending) -->
        <asp:UpdatePanel ID="UpdatePanelNotAll" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Not Receive All</h3>
                        <div class="box-tools" style="width: 200px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtNotAllSearch" runat="server" class="form-control pull-right" placeholder="Search ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="CmdNotAllSearch" runat="server" type="button" class="btn btn-primary" OnClick="CmdNotAllSearch_ServerClick" Text="Search" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:Panel runat="server" ScrollBars="Auto" style="max-height: 400px; overflow-y: auto;">
                            <asp:GridView ID="GridViewNotAllReceived" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
                                EmptyDataText="No deliveries with pending items. All received."
                                OnRowCommand="GridViewNotAll_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DeliveryID" HeaderText="Delivery ID" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                    <asp:BoundField DataField="WarehouseFromName" HeaderText="From" />
                                    <asp:BoundField DataField="WarehouseToName" HeaderText="To" />
                                    <asp:BoundField DataField="ReceiveProgress" HeaderText="Receive (done/total)" />
                                    <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info btn-xs" Text="Select" CommandName="Select" />
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="LblNotAllPaging" runat="server" CssClass="text-muted small"></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Bottom: List Receive (only received items - IsReceive=1) -->
        <asp:UpdatePanel ID="UpdatePanelListReceive" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Receive</h3>
                        <div class="box-tools" style="width: 200px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtReceivedSearch" runat="server" class="form-control pull-right" placeholder="Search received ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="CmdReceivedSearch" runat="server" type="button" class="btn btn-primary" OnClick="CmdReceivedSearch_ServerClick" Text="Search" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="div_comment" runat="server"></div>
                        <asp:Panel runat="server" ScrollBars="Auto" style="max-height: 400px; overflow-y: auto;">
                            <asp:GridView ID="GridViewReceived" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
                                EmptyDataText="No received deliveries. Select a delivery and use Receive in the modal."
                                OnRowCommand="GridViewReceived_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DeliveryID" HeaderText="Delivery ID" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                    <asp:BoundField DataField="WarehouseFromName" HeaderText="From" />
                                    <asp:BoundField DataField="WarehouseToName" HeaderText="To" />
                                    <asp:BoundField DataField="ReceivedCount" HeaderText="Device" />
                                    <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info btn-xs" Text="Detail" CommandName="Select" />
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="LblReceivedPaging" runat="server" CssClass="text-muted small"></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Modal: Receive (detail lines + Receive button + Receive Remark) -->
        <asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="modal" id="modal-receive" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Receive - <asp:Label ID="LblModalDelivery" runat="server" /></h4>
                    </div>
                    <div class="modal-body">
                        <div id="divNotReceived" runat="server" class="alert alert-warning" style="display:none;"></div>
                        <div class="form-group form-group-sm">
                            <label>Receive Remark</label>
                            <asp:TextBox ID="txtReceiveRemark" runat="server" CssClass="form-control" placeholder="Receive remark ..." TextMode="MultiLine" Rows="2" title="Input remark here"></asp:TextBox>
                        </div>
                        <asp:Panel ID="PanelDeliveryRef" runat="server" CssClass="well well-sm" style="margin-bottom:12px;">
                            <strong>Delivery reference</strong>
                            <div class="form-group form-group-sm" style="margin-top:8px;margin-bottom:4px;">
                                <label class="text-muted small">Reference URL</label>
                                <asp:Label ID="LblDeliveryUrl" runat="server" CssClass="small" style="display:block;word-break:break-all;white-space:pre-wrap;"></asp:Label>
                                <asp:Label ID="LblNoDeliveryUrl" runat="server" CssClass="text-muted small" Text="(none)" Visible="false"></asp:Label>
                            </div>
                            <label class="text-muted small" style="display:block;margin-bottom:4px;">Delivery photo</label>
                            <asp:Image ID="ImgHeaderDeliveryPhoto" runat="server" CssClass="img-thumbnail" style="max-height:100px;display:none;" AlternateText="Delivery photo" />
                        </asp:Panel>
                        <div class="form-group form-group-sm">
                            <label class="control-label" style="display:block;width:100%;margin-bottom:6px;">Receive photo <span id="SpanReceivePhotoRequired" class="text-danger" style="display:none;">*</span></label>
                            <asp:HiddenField ID="HfReceivePhoto" runat="server" />
                            <asp:FileUpload ID="FileUploadReceivePhoto" runat="server" CssClass="form-control" accept="image/*" onchange="previewReceivePhoto(this);" />
                            <div id="DivReceivePhotoPreview" style="display:block;width:100%;margin-top:6px;">
                                <asp:Image ID="ImgReceivePhoto" runat="server" CssClass="img-thumbnail" style="max-height:120px;display:none;" AlternateText="Receive photo" />
                            </div>
                            <p class="text-muted small" style="margin-top:4px;">Saved to Upload/Receive on submit. Required before Submit Header. JPG, PNG, GIF, WebP. Max 3 MB.</p>
                        </div>
                        <asp:Panel ID="divPendingInfo" runat="server" CssClass="text-muted small">
                            <p><strong>Check the devices to mark as received, then click Confirm.</strong> Use the checkbox in the table header to select or clear all.</p>
                        </asp:Panel>
                        <asp:Panel ID="divAllReceivedInfo" runat="server" CssClass="alert alert-info" Visible="false">
                            <strong>All devices are received.</strong> Upload a receive photo (proof), add a remark if needed, then click Submit Header to close this delivery.
                        </asp:Panel>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300">
                            <asp:GridView ID="GridViewDetail" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
                                EmptyDataText="No detail lines."
                                OnRowDataBound="GridViewDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelect" runat="server" Visible='<%# Eval("ShowReceiveButton") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                    <asp:BoundField DataField="NoSN" HeaderText="No SN" />
                                    <asp:BoundField DataField="IMEI" HeaderText="IMEI" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                    <asp:BoundField DataField="IsReceiveStatus" HeaderText="Status" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdConfirmReceive" runat="server" CssClass="btn btn-success" Text="Confirm" OnClick="CmdConfirmReceive_Click" />
                        <asp:Button ID="CmdSubmitHeader" runat="server" CssClass="btn btn-primary" Text="Submit Header" OnClientClick="confirmSubmitHeader(); return false;" Visible="false" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Modal: Confirm Submit Header -->
        <div class="modal fade" id="modal-confirm-submit-header" tabindex="-1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Submit this delivery? All items must be received.</h6>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdYesSubmitHeader" runat="server" CssClass="btn btn-default" Text="Yes" OnClick="CmdSubmitHeader_Click" />
                        <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Confirm Receive -->
        <div class="modal fade" id="modal-confirm-receive" tabindex="-1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Mark device <label id="LblReceiveDeviceID" runat="server"></label> as received?</h6>
                        <asp:HiddenField ID="HfReceiveDeviceID" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdYesReceive" runat="server" CssClass="btn btn-default" Text="Yes" OnClick="CmdYesReceive_Click" />
                        <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Received Detail (List Receive - detail rows for a delivery) -->
        <asp:UpdatePanel ID="UpdatePanelReceivedDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="modal fade" id="modal-received-detail" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Received Detail - <asp:Label ID="LblReceivedDetailDelivery" runat="server" /></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ScrollBars="Auto">
                            <asp:GridView ID="GridViewReceivedDetail" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
                                EmptyDataText="No received items for this delivery.">
                                <Columns>
                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                    <asp:BoundField DataField="NoSN" HeaderText="No SN" />
                                    <asp:BoundField DataField="IMEI" HeaderText="IMEI" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                    <asp:BoundField DataField="ReceiveRemark" HeaderText="Receive Remark" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </section>

    <script type="text/javascript">
        function previewReceivePhoto(input) {
            if (!input || !input.files || !input.files[0]) return;
            var f = input.files[0];
            if (!/^image\//i.test(f.type)) {
                alert('Please choose a JPG, PNG, GIF, or WebP image.');
                input.value = '';
                return;
            }
            if (f.size > 3 * 1024 * 1024) {
                alert('Maximum file size is 3 MB.');
                input.value = '';
                return;
            }
            var img = document.getElementById('<%= ImgReceivePhoto.ClientID %>');
            var reader = new FileReader();
            reader.onload = function (e) {
                if (img) { img.src = e.target.result; img.style.display = 'block'; }
            };
            reader.readAsDataURL(f);
        }
        function toggleReceivePhotoRequired(show) {
            var el = document.getElementById('SpanReceivePhotoRequired');
            if (el) el.style.display = show ? 'inline' : 'none';
        }
        function showReceiveModal() {
            $('#modal-receive').modal('show');
        }
        function confirmSubmitHeader() {
            var input = document.getElementById('<%= FileUploadReceivePhoto.ClientID %>');
            var hf = document.getElementById('<%= HfReceivePhoto.ClientID %>');
            var hasFile = input && input.files && input.files.length > 0;
            var hasSaved = hf && hf.value && hf.value.trim();
            if (!hasFile && !hasSaved) {
                alert('Receive photo is required before Submit Header. Choose a file under Receive photo.');
                return false;
            }
            $('#modal-confirm-submit-header').modal('show');
            return false;
        }
        function confirmReceive(deviceId) {
            if (deviceId) {
                document.getElementById('<%= LblReceiveDeviceID.ClientID %>').innerHTML = deviceId;
                document.getElementById('<%= HfReceiveDeviceID.ClientID %>').value = deviceId;
                $('#modal-confirm-receive').modal('show');
            }
            return false;
        }
        function showReceivedDetailModal() {
            $('#modal-received-detail').modal('show');
        }
    </script>
</asp:Content>
