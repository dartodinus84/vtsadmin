<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device.aspx.cs" Inherits="vtsadm.device" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Device</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Device Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <asp:TextBox ID="txtDeviceID" runat="server" class="form-control" placeholder="Skip for new device ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Group Description</label>
                            <asp:DropDownList ID="CmbDeviceGroupID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbDeviceGroupID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type Description</label>
                            <asp:DropDownList ID="CmbDeviceTypeID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbDeviceTypeID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No SN</label>
                            <asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>IMEI</label>
                            <asp:TextBox ID="txtIMEI" runat="server" class="form-control" placeholder="IMEI ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Gmt</label>
                            <asp:TextBox ID="txtGMT" runat="server" class="form-control" placeholder="GMT ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Packing List</label>
                            <asp:TextBox ID="txtPackingListNo" runat="server" class="form-control" placeholder="Packing List ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date Arrival</label>
                            <asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source</label>
                            <asp:DropDownList ID="CmbSource" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <asp:DropDownList ID="CmbServer" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>GPS Category</label>
                            <asp:DropDownList ID="CmbIsMobile" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vendor Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vendor Name</label>
                            <asp:DropDownList ID="CmbVendorID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbVendorID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <asp:TextBox ID="txtVendorAddress" runat="server" class="form-control" placeholder="Address ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <%--<asp:Button ID="CmdSubmit2" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit2').modal('show');return false;" Text="Submit New" />--%>
                        <button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload</button>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Device</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by no sn ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="IMEI" HeaderText="IMEI" ItemStyle-Wrap="false" SortExpression="IMEI"></asp:BoundField>
                                        <asp:BoundField DataField="GMT" HeaderText="GMT" ItemStyle-Wrap="false" SortExpression="GMT"></asp:BoundField>
                                        <asp:BoundField DataField="sDateArrival" HeaderText="Date Arrival" ItemStyle-Wrap="false" SortExpression="sDateArrival"></asp:BoundField>
                                        <asp:BoundField DataField="SourceName" HeaderText="Source" ItemStyle-Wrap="false" SortExpression="SourceName"></asp:BoundField>
                                        <asp:BoundField DataField="BatchNo" HeaderText="Batch No" ItemStyle-Wrap="false" SortExpression="BatchNo"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" ItemStyle-Wrap="false" SortExpression="VendorName"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Type" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Device Group Desc" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server Name" ItemStyle-Wrap="false" SortExpression="ServerName"></asp:BoundField>
                                        <asp:BoundField DataField="ExpiredWarannty" HeaderText="Expired Warannty" ItemStyle-Wrap="false" SortExpression="ExpiredWarannty"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdUpdate" runat="server" Text="<i class='fa fa-history'></i>" ToolTip="Update Status" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="VendorID" HeaderText="Vendor ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="VendorAddress" HeaderText="Vendor Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupID" HeaderText="Device Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SourceID" HeaderText="Source ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ServerID" HeaderText="Server ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IsMobile" HeaderText="Is Mobile" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PackingListNo" HeaderText="Packing List" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
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
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-submit').modal('hide');" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>


        <%--<div class="modal fade" id="modal-submit2">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure ?</h6>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-submit2').modal('hide');" onserverclick="CmdYesSubmit2_ServerClick" id="CmdYesSubmit2">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit2').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>--%>

        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Device ID :&nbsp;</h6>
                        <label id="LblDeviceID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtDeviceIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-update">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to update status Device ID :&nbsp;</h6>
                        <label id="LblDeviceIDUpdate" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtDeviceIDUpdate" runat="server" />
                        <input type="hidden" id="txtStatusUpdate" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-update').modal('hide');" onserverclick="CmdYesUpdate_ServerClick" id="CmdYesUpdate">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-update').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-upload">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload [<a href="Export/device_upload_final.csv">Download Template</a>]</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="device_upload.aspx" style="width: 100%; border: none; height: 460px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblDeviceID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtDeviceIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function confirmUpdate(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblDeviceIDUpdate').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtDeviceIDUpdate').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusUpdate').value = sStatus;
                $("#modal-update").modal('show');
            }
        }

        function endRequest(sender, args) {
            //$('#ContentPlaceHolder1_CmbDeviceGroupID').select2();
            //$('#ContentPlaceHolder1_CmbDeviceTypeID').select2();
            //$('#ContentPlaceHolder1_CmbSource').select2();
            //$('#ContentPlaceHolder1_CmbServer').select2();
            //$('#ContentPlaceHolder1_CmbVendorID').select2();

            $('#modal-upload').on('hidden.bs.modal', function () {
                var objClear = document.getElementById('ContentPlaceHolder1_CmdClear');
                objClear.click();
            });
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
