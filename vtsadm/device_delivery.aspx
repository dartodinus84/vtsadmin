<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="device_delivery.aspx.cs" CodeBehind="device_delivery.aspx.cs" Inherits="vtsadm.device_delivery" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device
            <small>Delivery</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">Delivery</li>
        </ol>
    </section>

    <asp:HiddenField ID="HfCloseDeviceModal" runat="server" Value="" />
    <asp:HiddenField ID="HfSelectedDevices" runat="server" Value="" />
    <section class="content">
        <!-- Card 1: Delivery Header -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Delivery Header</h3>
                    </div>
                    <div class="box-body">
                        <p class="text-muted small" style="margin-bottom:12px;"><span class="text-danger">*</span> All fields are required. Add at least one device with remark in Delivery Detail before submit.</p>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDeliveryID" runat="server" style="display:none;" />
                                <div class="form-group form-group-sm">
                                    <label>Delivery Date <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDeliveryDate" runat="server" TextMode="Date" class="form-control" placeholder="Delivery Date ..."></asp:TextBox>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Warehouse From <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="CmbWarehouseFrom" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Warehouse To <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="CmbWarehouseTo" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Expedisi <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="CmbExpedisi" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group form-group-sm">
                                    <label>Expedisi Service / Code <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="CmbExpedisiCode" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Expedisi Price <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtExpedisiPrice" runat="server" class="form-control" placeholder="0" Text="0"></asp:TextBox>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Expedisi Resi No <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtExpedisiResiNo" runat="server" class="form-control" placeholder="Resi number ..."></asp:TextBox>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Remark <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..." TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Reference URL <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDeliveryUrl" runat="server" CssClass="form-control" placeholder="Reference text or link ..." TextMode="SingleLine" onchange="syncDeliveryUrlHidden();" onblur="syncDeliveryUrlHidden();" />
                                    <asp:HiddenField ID="HfDeliveryUrl" runat="server" />
                                </div>
                                <div class="form-group form-group-sm">
                                    <label>Delivery photo <span class="text-danger">*</span></label>
                                    <asp:HiddenField ID="HfDeliveryPhoto" runat="server" />
                                    <asp:FileUpload ID="FileUploadDeliveryPhoto" runat="server" CssClass="form-control" accept="image/*" onchange="previewDeliveryPhoto(this);" />
                                    <asp:Image ID="ImgDeliveryPhoto" runat="server" CssClass="img-thumbnail" style="max-height:120px;margin-top:6px;display:none;" AlternateText="Delivery photo" />
                                    <p class="text-muted small" style="margin-top:4px;">Saved to Upload/Delivery on submit. JPG, PNG, GIF, or WebP. Max 3 MB.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_ServerClick" Text="Clear" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="syncDeliveryUrlHidden(); $('#modal-submit').modal('show');return false;" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Card 2: Delivery Detail -->
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Delivery Detail</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <button type="button" class="btn btn-success" data-toggle="modal" data-target="#modal-device">Add Device</button>
                            <button type="button" class="btn btn-info" data-toggle="modal" data-target="#modal-upload-device">Upload Device</button>
                            <a href="Export/device_delivery_template.xlsx" class="btn btn-default btn-sm" download>Download Template (XLSX)</a>
                        </div>
                        <asp:Panel runat="server" ScrollBars="Auto">
                            <asp:GridView ID="GridViewDetail" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" EmptyDataText="No detail lines. Add above or load a delivery."
                                OnRowCommand="GridViewDetail_RowCommand" OnRowDataBound="GridViewDetail_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                    <asp:BoundField DataField="NoSN" HeaderText="No SN" />
                                    <asp:BoundField DataField="IMEI" HeaderText="IMEI" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark *" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CmdRemoveDetail" runat="server" CssClass="btn btn-danger btn-xs" Text="Remove" CommandName="RemoveDetail" CommandArgument='<%# Eval("DeviceID") %>' OnClientClick="return confirm('Remove this line?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
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
                        <asp:Panel runat="server" ScrollBars="Auto">
                            <asp:GridView ID="GridViewHeader" runat="server" BackColor="WhiteSmoke" Font-Size="Small" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="False"
                                EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10"
                                OnRowCommand="GridViewHeader_RowCommand" OnPageIndexChanging="GridViewHeader_PageIndexChanging" OnRowDataBound="GridViewHeader_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="DeliveryID" HeaderText="Delivery ID" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" />
                                    <asp:BoundField DataField="WarehouseFromName" HeaderText="From" />
                                    <asp:BoundField DataField="WarehouseToName" HeaderText="To" />
                                    <asp:BoundField DataField="ExpedisiName" HeaderText="Expedisi" />
                                    <asp:BoundField DataField="ExpedisiCode" HeaderText="Service" />
                                    <asp:BoundField DataField="ExpedisiResiNo" HeaderText="Resi No" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Attachment" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="button" runat="server" id="BtnViewAttachment" class="btn btn-info btn-xs btn-view-attachment" value="View" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-warning btn-xs" Text="Edit" CommandName="Select" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CmdDelete" runat="server" CssClass="btn btn-danger btn-xs" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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

        <!-- Modal: Add Device (from mst_device, exclude trx_device_mutation_warehouse) -->
        <div class="modal fade" id="modal-device" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Add Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div class="input-group">
                                <asp:TextBox ID="txtDeviceSearch" runat="server" CssClass="form-control" placeholder="Search Device ID, NoSN, IMEI ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button ID="CmdDeviceSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="CmdDeviceSearch_Click" />
                                </span>
                            </div>
                        </div>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300">
                            <asp:GridView ID="GridViewDevice" runat="server" CssClass="table table-bordered table-condensed" AutoGenerateColumns="False"
                                EmptyDataText="No devices found. Search or ensure device is not in warehouse mutation."
                                DataKeyNames="DeviceID" OnRowDataBound="GridViewDevice_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                    <asp:BoundField DataField="NoSN" HeaderText="No SN" />
                                    <asp:BoundField DataField="IMEI" HeaderText="IMEI" />
                                    <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type" />
                                    <asp:TemplateField HeaderText="Remark *">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRowRemark" runat="server" CssClass="form-control input-sm row-remark" placeholder="Remark *" style="min-width:120px;"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <div class="form-group form-group-sm" style="margin-top: 18px;">
                            <label>Remark for all selected devices</label>
                            <asp:TextBox ID="txtModalRemark" runat="server" CssClass="form-control" placeholder="Used when a row remark is empty" title="Optional shortcut when every device gets the same remark"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdAddSelected" runat="server" CssClass="btn btn-success" Text="Add Selected" OnClick="CmdAddSelected_Click" UseSubmitBehavior="false" OnClientClick="return collectAndAddDevices();" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
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
                        <h6 class="modal-title">Save this delivery?</h6>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdYesSubmit" runat="server" class="btn btn-default" Text="Yes" OnClick="CmdYesSubmit_ServerClick" UseSubmitBehavior="true" />
                        <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Delete delivery </h6>
                        <label id="LblDeliveryIDDelete" runat="server"></label> ?
                        <input type="hidden" id="txtDeliveryIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdYesDelete" runat="server" class="btn btn-default" Text="Yes" OnClick="CmdYesDelete_ServerClick" />
                        <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-attachment-view" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Delivery Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <p class="text-muted small" style="margin-bottom:12px;">Delivery ID: <strong id="LblAttachmentDeliveryId"></strong></p>
                        <div class="form-group form-group-sm">
                            <label>Reference URL</label>
                            <div id="DivAttachmentUrl" class="well well-sm" style="margin-bottom:0;word-break:break-all;white-space:pre-wrap;">(none)</div>
                        </div>
                        <div class="form-group form-group-sm" style="margin-bottom:0;">
                            <label class="control-label" style="display:block;width:100%;margin-bottom:6px;">Delivery photo</label>
                            <div id="DivAttachmentPhoto" style="display:block;width:100%;">
                                <span id="LblAttachmentNoPhoto" class="text-muted small" style="display:block;">(none)</span>
                                <img id="ImgAttachmentPreview" src="" alt="Delivery photo" class="img-thumbnail" style="display:none;max-height:240px;margin-top:6px;clear:both;" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info</h4>
                    </div>
                    <div class="modal-body">
                        <div id="div_comment" runat="server"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Upload Device (inside Content1 - no Site.Master change needed) -->
        <div class="modal fade" id="modal-upload-device" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                        <h4 class="modal-title">Upload Device</h4>
                    </div>
                    <div class="modal-body">
                        <p class="text-muted small">Upload Excel (XLSX) with columns: <strong>DeviceID</strong>, <strong>Remark</strong> (required). <a href="Export/device_delivery_template.xlsx" download>Download template</a></p>
                        <div id="DivUploadError" runat="server" class="alert alert-danger" role="alert" style="display:none;"></div>
                        <div class="form-group form-group-sm">
                            <asp:FileUpload ID="FileUploadDevice" runat="server" CssClass="form-control" accept=".xlsx" />
                        </div>
                        <div class="form-group form-group-sm">
                            <asp:Button ID="CmdUploadDevice" runat="server" CssClass="btn btn-primary" Text="Upload &amp; Validate" OnClick="CmdUploadDevice_Click" UseSubmitBehavior="true" />
                        </div>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="250" ID="PanelUploadValidation">
                            <asp:GridView ID="GridViewUploadValidation" runat="server" CssClass="table table-bordered table-condensed" AutoGenerateColumns="False" EnableViewState="false"
                                EmptyDataText="Upload a file and click Validate. Valid rows (OK) can be submitted."
                                OnRowDataBound="GridViewUploadValidation_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="RowNo" HeaderText="#" />
                                    <asp:BoundField DataField="DeviceID" HeaderText="Device ID" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                    <asp:BoundField DataField="StatusText" HeaderText="Status" />
                                    <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdSubmitUpload" runat="server" CssClass="btn btn-success" Text="Submit Valid Devices" OnClick="CmdSubmitUpload_Click" UseSubmitBehavior="true" Visible="false" />
                        <asp:Button ID="CmdClearUpload" runat="server" CssClass="btn btn-default" Text="Close" OnClick="CmdClearUpload_Click" style="display:none;" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function syncDeliveryUrlHidden() {
            var u = document.getElementById('<%= txtDeliveryUrl.ClientID %>');
            var h = document.getElementById('<%= HfDeliveryUrl.ClientID %>');
            if (u && h) h.value = (u.value || '').trim();
        }
        function confirmDelete(deliveryId, status) {
            if (deliveryId) {
                document.getElementById('<%= LblDeliveryIDDelete.ClientID %>').innerHTML = deliveryId;
                document.getElementById('<%= txtDeliveryIDDelete.ClientID %>').value = deliveryId;
                document.getElementById('<%= txtStatusDelete.ClientID %>').value = status;
                $('#modal-delete').modal('show');
            }
        }
        function getAttachmentModal() {
            var $all = $('#modal-attachment-view');
            if ($all.length > 1)
                $all.slice(0, -1).remove();
            var $m = $('#modal-attachment-view').last();
            if ($m.length && !$m.parent().is('body'))
                $m.appendTo(document.body);
            return $m;
        }
        function openAttachmentModal() {
            var $m = getAttachmentModal();
            if ($m.length) $m.modal('show');
        }
        function populateAttachmentModalLoading(deliveryId) {
            var $m = getAttachmentModal();
            if (!$m.length) return;
            $m.find('#LblAttachmentDeliveryId').text(deliveryId || '');
            $m.find('#DivAttachmentUrl').text('Loading...');
            $m.find('#ImgAttachmentPreview').removeAttr('src').hide();
            $m.find('#LblAttachmentNoPhoto').text('Loading photo...').show();
        }
        function attachmentText(data, names) {
            if (!data) return '';
            for (var i = 0; i < names.length; i++) {
                var v = data[names[i]];
                if (v != null && String(v).trim() !== '') return String(v).trim();
            }
            return '';
        }
        function populateAttachmentModal(deliveryId, url, photoUrl) {
            var $m = getAttachmentModal();
            if (!$m.length) return;
            $m.find('#LblAttachmentDeliveryId').text(deliveryId || '');
            $m.find('#DivAttachmentUrl').text(url ? url : '(none)');
            var $img = $m.find('#ImgAttachmentPreview');
            var $emptyPh = $m.find('#LblAttachmentNoPhoto');
            if (photoUrl) {
                var src = photoUrl + (photoUrl.indexOf('?') >= 0 ? '&' : '?') + 't=' + new Date().getTime();
                $img.off('error').on('error', function () {
                    $img.hide();
                    $emptyPh.text('(photo not found on server)').show();
                });
                $emptyPh.hide();
                $img.attr('src', src).css({ display: 'block', width: 'auto', maxWidth: '100%' }).show();
            } else {
                $img.removeAttr('src').hide();
                $emptyPh.text('(none)').show();
            }
        }
        function applyAttachmentResponse(deliveryId, gridUrl, gridPhotoUrl, r) {
            var data = (r && r.d) ? r.d : (r || {});
            var url = attachmentText(data, ['referenceUrl', 'url', 'Url', 'ReferenceUrl']) || (gridUrl || '');
            var photoUrl = attachmentText(data, ['photoUrl', 'PhotoUrl']) || (gridPhotoUrl || '');
            populateAttachmentModal((data.deliveryId || data.DeliveryId || deliveryId), url, photoUrl);
        }
        function showDeliveryAttachment(deliveryId, gridUrl, gridPhotoUrl) {
            deliveryId = (deliveryId || '').trim();
            gridUrl = (gridUrl || '').trim();
            gridPhotoUrl = (gridPhotoUrl || '').trim();
            if (!deliveryId) return;
            populateAttachmentModalLoading(deliveryId);
            openAttachmentModal();
            if (typeof PageMethods === 'undefined' || !PageMethods.GetDeliveryAttachment) {
                populateAttachmentModal(deliveryId, gridUrl || '', gridPhotoUrl || '');
                return;
            }
            PageMethods.GetDeliveryAttachment(deliveryId, function (r) {
                applyAttachmentResponse(deliveryId, gridUrl, gridPhotoUrl, r);
            }, function () {
                populateAttachmentModal(deliveryId, gridUrl || '', gridPhotoUrl || '');
            });
        }
        function previewDeliveryPhoto(input) {
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
            var img = document.getElementById('<%= ImgDeliveryPhoto.ClientID %>');
            var reader = new FileReader();
            reader.onload = function (e) {
                if (img) { img.src = e.target.result; img.style.display = 'block'; }
            };
            reader.readAsDataURL(f);
        }
        function loadExpedisiServices() {
            var sel = document.getElementById('<%= CmbExpedisi.ClientID %>');
            var codeSel = document.getElementById('<%= CmbExpedisiCode.ClientID %>');
            if (!codeSel) return;
            var expId = (sel && sel.value) ? sel.value : '[Select]';
            if (expId === '[Select]' || !expId) {
                codeSel.options.length = 0;
                codeSel.options.add(new Option('[Select]', '[Select]'));
                return;
            }
            if (typeof PageMethods === 'undefined' || !PageMethods.GetExpedisiServices) return;
            PageMethods.GetExpedisiServices(expId, function (r) {
                codeSel.options.length = 0;
                if (r && r.items) {
                    for (var i = 0; i < r.items.length; i++)
                        codeSel.options.add(new Option(r.items[i].text, r.items[i].value));
                } else {
                    codeSel.options.add(new Option('[Select]', '[Select]'));
                }
            }, function () {
                codeSel.options.length = 0;
                codeSel.options.add(new Option('[Select]', '[Select]'));
            });
        }
        function clearModalBackdrop() {
            document.body.classList.remove('modal-open');
            document.body.style.paddingRight = '';
            var backs = document.querySelectorAll('.modal-backdrop');
            for (var i = 0; i < backs.length; i++) { if (backs[i].parentNode) backs[i].parentNode.removeChild(backs[i]); }
            var m = document.getElementById('modal-device');
            if (m) { m.classList.remove('in'); m.style.display = 'none'; }
        }
        function closeSubmitModalAndBackdrop() {
            var m = document.getElementById('modal-submit');
            if (m) { m.classList.remove('in'); m.style.display = 'none'; }
            clearModalBackdrop();
        }
        function initDeviceDeliveryClient() {
            getAttachmentModal();
            $(document).off('click.ddlViewAtt', '.btn-view-attachment').on('click.ddlViewAtt', '.btn-view-attachment', function (e) {
                e.preventDefault();
                e.stopPropagation();
                var $b = $(this);
                showDeliveryAttachment($b.attr('data-delivery-id'), $b.attr('data-delivery-url'), $b.attr('data-photo-url'));
                return false;
            });
            var fu = document.getElementById('<%= FileUploadDeliveryPhoto.ClientID %>');
            if (fu) fu.onchange = function () { previewDeliveryPhoto(this); };
            var exp = document.getElementById('<%= CmbExpedisi.ClientID %>');
            if (exp) exp.onchange = function () { loadExpedisiServices(); };
            syncDeliveryUrlHidden();
        }
        $(document).ready(initDeviceDeliveryClient);
        (function () {
            var prm = typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager.getInstance();
            if (prm) {
                prm.add_beginRequest(function () {
                    var o = document.getElementById('overlay');
                    if (o) o.style.display = 'block';
                });
                prm.add_endRequest(function () {
                    var o = document.getElementById('overlay');
                    if (o) o.style.display = 'none';
                    initDeviceDeliveryClient();
                });
            }
        })();
        function collectAndAddDevices() {
            var remarkAllEl = document.getElementById('<%= txtModalRemark.ClientID %>');
            var remarkAll = (remarkAllEl && remarkAllEl.value) ? remarkAllEl.value.trim() : '';
            var grid = document.getElementById('<%= GridViewDevice.ClientID %>');
            if (!grid) { clearModalBackdrop(); __doPostBack('<%= CmdAddSelected.UniqueID %>', ''); return false; }
            var parts = [];
            var rows = grid.getElementsByTagName('tr');
            for (var i = 1; i < rows.length; i++) {
                var chk = rows[i].querySelector('input[type="checkbox"]');
                if (!chk || chk.name.indexOf('ChkSelect') < 0 || chk.id.indexOf('ChkSelectAll') >= 0 || !chk.checked) continue;
                var did = rows[i].getAttribute('data-device-id') || '';
                var nosn = rows[i].getAttribute('data-nosn') || '';
                var imei = rows[i].getAttribute('data-imei') || '';
                var rowRemark = rows[i].querySelector('input.row-remark');
                var r = (rowRemark && rowRemark.value) ? rowRemark.value.trim() : '';
                if (!r) r = remarkAll;
                if (!r) { alert('Enter a remark on each selected row or fill Remark for all selected devices.'); return false; }
                if (did) parts.push(did + '|' + (nosn || '') + '|' + (imei || '') + '|' + encodeURIComponent(r || ''));
            }
            if (parts.length === 0) { alert('Select at least one device.'); return false; }
            document.getElementById('<%= HfSelectedDevices.ClientID %>').value = parts.join(',');
            clearModalBackdrop();
            __doPostBack('<%= CmdAddSelected.UniqueID %>', '');
            return false;
        }
        if (document.readyState === 'complete') clearModalBackdrop(); else window.addEventListener('load', clearModalBackdrop);
    </script>
</asp:Content>
