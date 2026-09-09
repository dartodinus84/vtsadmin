<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_install_device.aspx.cs" Inherits="vtsadm.view_install_device" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Installation Device
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Installation</a></li>
            <li class="active">Device</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Search Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Search By</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by any fields ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date From</label>
                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Input date from ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date To</label>
                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Input date to ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <asp:CheckBox ID="chkIncludeINTP" runat="server" Text="Include INTP Server" />
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClientClick="showOverlay();" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Installation Device</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke"  AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        
                                        <asp:BoundField DataField="policeno" HeaderText="Police No" ItemStyle-Wrap="false" SortExpression="policeno"></asp:BoundField>
                                        <asp:BoundField DataField="fullname" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="fullname"></asp:BoundField>
                                        <asp:BoundField DataField="cust_marketingname" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="cust_marketingname"></asp:BoundField>
                                        <asp:BoundField DataField="device_status" HeaderText="Status Device" ItemStyle-Wrap="false" SortExpression="device_status"></asp:BoundField>
                                        <asp:BoundField DataField="nosn" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="nosn"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_status" HeaderText="Status Gsm" ItemStyle-Wrap="false" SortExpression="gsm_status"></asp:BoundField>
                                        <asp:BoundField DataField="gsmno" HeaderText="MSIDN" ItemStyle-Wrap="false" SortExpression="gsmno"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_type" HeaderText="Type gsm_type" ItemStyle-Wrap="false" SortExpression="TypeName"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_activationdate" HeaderText="Activation Date" ItemStyle-Wrap="false" SortExpression="gsm_activationdate"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_expdate" HeaderText="Expired Date" ItemStyle-Wrap="false" SortExpression="gsm_expdate"></asp:BoundField>
                                        <asp:BoundField DataField="mis_status" HeaderText="Status Installation" ItemStyle-Wrap="false" SortExpression="mis_status"></asp:BoundField>
                                        <asp:BoundField DataField="gps_time" HeaderText="GPS Time" ItemStyle-Wrap="false" SortExpression="gps_time"></asp:BoundField>
                                        <asp:BoundField DataField="sinkron_server" HeaderText="Is Mirroring" ItemStyle-Wrap="false" SortExpression="sinkron_server"></asp:BoundField>
                                        <asp:BoundField DataField="servername" HeaderText="Server Name" ItemStyle-Wrap="false" SortExpression="servername"></asp:BoundField>
                                        <asp:BoundField DataField="cust_expireddate" HeaderText="Exp Waranty" ItemStyle-Wrap="false" SortExpression="cust_expireddate"></asp:BoundField>


                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdAcc" runat="server" Text="<i class='fa fa-cog'></i>" ToolTip="Accessories" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPic" runat="server" Text="<i class='fa fa-file-image-o'></i>" ToolTip="Picture" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="tvdid" HeaderText="tvdid" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="assetno" HeaderText="Asset No" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_type" HeaderText="Device Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_warehouseid" HeaderText="Device Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_warehouse" HeaderText="Device Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_vendorname" HeaderText="Vendor Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_providerid" HeaderText="Provider ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_provider" HeaderText="Provider Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_warehouseid" HeaderText="Gsm Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_warehouse" HeaderText="Gsm Warehouse Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="cust_address" HeaderText="Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="cust_type" HeaderText="Customer Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="marketingid" HeaderText="Marketing ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="serverid" HeaderText="Server ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="custid" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="technicianid" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_technician" HeaderText="Technician Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="mis_usrupd" HeaderText="User Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="mis_dtmupd" HeaderText="Date Update" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="device_sourcename" HeaderText="Device Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_sourcename" HeaderText="Gsm Source Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="installdate" HeaderText="Installation Date" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="picturefilename" HeaderText="Picture" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="vehicleid" HeaderText="Vehicle ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="vehicledesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="uplinecustid" HeaderText="Upline Cust ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="uplinename" HeaderText="Upline Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="mastercustid" HeaderText="Master Cust ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="mastername" HeaderText="Master Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="deviceid" HeaderText="DeviceID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="gsmid" HeaderText="GsmID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Channel" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Literal ID="litChannelStatus" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
                  </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-pic">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture Attachment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div id="divpic" class="form-group form-group-sm" style="height: 300px; text-align: center;">
                                <img id="ImgInstall" src="_blank" style="height: 100%; width: 100%;">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-acc">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Accessories</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframeacc" src="view_install_device_acc.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
        <!-- Modal Setting Channel MDVR (referensi device_qc) -->
        <div class="modal fade" id="modal-channel-setting" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-sliders"></i> Setting Channel MDVR</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <label>NoSN</label>
                            <p id="chNoSN" class="form-control-static">-</p>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type</label>
                            <p id="chDeviceType" class="form-control-static">-</p>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Max Channel</label>
                            <p id="chMaxChannel" class="form-control-static">-</p>
                        </div>
                        <div id="channelSettingAlert"></div>
                        <div id="channelSettingLoading" style="display:none; margin-bottom:8px;">
                            <i class="fa fa-circle-o-notch fa-spin"></i> Memuat channel...
                        </div>
                        <div class="table-responsive">
                            <table class="table table-bordered" id="channelSettingTable" style="display:none;">
                                <thead><tr><th style="width:55%;">Channel</th><th>Tipe Channel</th></tr></thead>
                                <tbody id="channelSettingRows"></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">
                            <i class="fa fa-times"></i> Close
                        </button>
                        <button type="button" class="btn btn-success" id="btnSaveChannelSetting" onclick="saveViewChannelSetting();" disabled="disabled">
                            <i class="fa fa-save"></i> Save Setting
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Installation Device Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Device Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Device ID</label>
                                            <p id="LblDeviceID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>No SN</label>
                                            <p id="LblNoSN" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>GPS Time</label>
                                            <p id="LblGPSTime" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Device Type</label>
                                            <p id="LblDeviceType" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Source Name</label>
                                            <p id="LblDeviceSourceName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Vendor Name</label>
                                            <p id="LblVendorName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Warehouse Name</label>
                                            <p id="LblDeviceWarehouseName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatusDesc" runat="server" class="form-control-static"></p>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Gsm Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Gsm ID</label>
                                            <p id="LblGsmID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>MSIDN</label>
                                            <p id="LblMSIDN" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Provider Name</label>
                                            <p id="LblProviderName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Source Name</label>
                                            <p id="LblGsmSourceName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Warehouse Name</label>
                                            <p id="LblGsmWarehouseName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Install Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Installation Date</label>
                                            <p id="LblDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Is Mirroring</label>
                                            <p id="LblIsMirroring" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Vehicle Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Vehicle ID</label>
                                            <p id="LblVehicleID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Vehicle Desc</label>
                                            <p id="LblVehicleDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Police No</label>
                                            <p id="LblPoliceNo" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Asset No</label>
                                            <p id="LblAssetNo" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Technician Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Technician ID</label>
                                            <p id="LblTechnicianID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Name</label>
                                            <p id="LblTechnicianName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Customer Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Customer ID</label>
                                            <p id="LblCustID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Full Name</label>
                                            <p id="LblFullName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Customer Type</label>
                                            <p id="LblCustomerTypeDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Address</label>
                                            <textarea id="txtAddress" runat="server" class="form-control" style="background-color: white;" rows="1" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Marketing Name</label>
                                            <p id="LblMarketingName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Server Name</label>
                                            <p id="LblServerName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Upline Name</label>
                                            <p id="LblUplineName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Master Name</label>
                                            <p id="LblMasterName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function CheckNbsp(sbuff) {
            var sOut;
            if (sbuff == "&nbsp;") {
                sOut = "";
            }
            else {
                sOut = sbuff;
            }
            return sOut;
        }
        function postAcc(sTvdID) {
            //if (sTvdID != '') {
            $('#modal-acc').modal('show');
            var objfr = document.getElementById('iframeacc').contentWindow;
            var objTvdID = objfr.document.getElementById('txtTvdID');
            var cmdSearchAcc = objfr.document.getElementById('CmdSearchAcc');
            objTvdID.value = sTvdID;
            cmdSearchAcc.click();
            //}
        }
        function postPic(picFileName) {
            console.log(picFileName);
            if (picFileName != '') {
                $('#modal-pic').modal('show');
                var img1 = document.getElementById('ImgInstall');
                img1.src = "Picture/" + picFileName;
                //img1.he
            }
        }
        function postDetails(policeno, fullname, cust_marketingname, device_status, nosn, gsm_status, gsmno, gsm_type, gsm_activationdate, gsm_expdate, mis_status, gps_time,
            sinkron_server, servername, cust_expireddate, sbtndetail, sbtnacs, sbtnpic, tvdid, assetno, device_type, device_warehouseid, device_warehouse, device_vendorname, gsm_providerid, gsm_provider, gsm_warehouseid, gsm_warehouse, cust_address, cust_type, marketingid, serverid, custid, technicianid, device_technician, mis_usrupd, mis_dtmupd, device_sourcename, gsm_sourcename, installdate, picturefilename, vehicleid, vehicledesc, uplinecustid, uplinename, mastercustid, mastername, deviceid, gsmid) {
      
            if (vehicleid != '') {
                console.log(sinkron_server);
                console.log(servername);
                console.log(cust_expireddate);
                console.log(tvdid);
                console.log(assetno);
                console.log(device_type);

                document.getElementById('ContentPlaceHolder1_LblDeviceID').innerText = CheckNbsp(deviceid);
                document.getElementById('ContentPlaceHolder1_LblNoSN').innerText = CheckNbsp(nosn);
                document.getElementById('ContentPlaceHolder1_LblGPSTime').innerText = CheckNbsp(gps_time);
                document.getElementById('ContentPlaceHolder1_LblDeviceType').innerText = CheckNbsp(device_type);
                document.getElementById('ContentPlaceHolder1_LblDeviceSourceName').innerText = CheckNbsp(device_sourcename);
                document.getElementById('ContentPlaceHolder1_LblVendorName').innerText = CheckNbsp(device_vendorname);
                document.getElementById('ContentPlaceHolder1_LblDeviceWarehouseName').innerText = CheckNbsp(gsm_providerid);
                document.getElementById('ContentPlaceHolder1_LblStatusDesc').innerText = CheckNbsp(mis_status);

                document.getElementById('ContentPlaceHolder1_LblDate').innerText = CheckNbsp(installdate);
                document.getElementById('ContentPlaceHolder1_LblIsMirroring').innerText = CheckNbsp(sinkron_server);

                document.getElementById('ContentPlaceHolder1_LblGsmID').innerText = CheckNbsp(gsmid);
                document.getElementById('ContentPlaceHolder1_LblMSIDN').innerText = CheckNbsp(gsmno);
                document.getElementById('ContentPlaceHolder1_LblProviderName').innerText = CheckNbsp(gsm_provider);
                document.getElementById('ContentPlaceHolder1_LblGsmSourceName').innerText = CheckNbsp(gsm_sourcename);
                document.getElementById('ContentPlaceHolder1_LblGsmWarehouseName').innerText = CheckNbsp(gsm_warehouse);

                document.getElementById('ContentPlaceHolder1_LblTechnicianID').innerText = CheckNbsp(technicianid);
                document.getElementById('ContentPlaceHolder1_LblTechnicianName').innerText = CheckNbsp(device_technician);

                document.getElementById('ContentPlaceHolder1_LblVehicleID').innerText = CheckNbsp(vehicleid);
                document.getElementById('ContentPlaceHolder1_LblVehicleDesc').innerText = CheckNbsp(vehicledesc);
                document.getElementById('ContentPlaceHolder1_LblPoliceNo').innerText = CheckNbsp(policeno);
                document.getElementById('ContentPlaceHolder1_LblAssetNo').innerText = CheckNbsp(assetno);

                document.getElementById('ContentPlaceHolder1_LblCustID').textContent = CheckNbsp(custid);
                document.getElementById('ContentPlaceHolder1_LblFullName').innerText = CheckNbsp(fullname);
                document.getElementById('ContentPlaceHolder1_LblCustomerTypeDesc').innerText = CheckNbsp(cust_type);
                document.getElementById('ContentPlaceHolder1_txtAddress').textContent = CheckNbsp(cust_address);

                document.getElementById('ContentPlaceHolder1_LblMarketingName').innerText = CheckNbsp(cust_marketingname);
                document.getElementById('ContentPlaceHolder1_LblServerName').innerText = CheckNbsp(servername);
                document.getElementById('ContentPlaceHolder1_LblUplineName').innerText = CheckNbsp(uplinename);
                document.getElementById('ContentPlaceHolder1_LblMasterName').innerText = CheckNbsp(mastername);
                $('#modal-details').modal('show');
            }
        }

        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }

        // ===== Setting Channel MDVR (referensi device_qc / mdvr_channel.ashx) =====
        var CH_HANDLER = 'mdvr_channel.ashx';
        var viewChannelNoSN = '';
        var viewChannelMax = 0;
        var viewChannelDeviceType = '';

        function chEscapeHtml(s) {
            return (s == null ? '' : String(s))
                .replace(/&/g, '&amp;').replace(/</g, '&lt;')
                .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        }

        function showViewChannelAlert(type, msg) {
            var el = document.getElementById('channelSettingAlert');
            if (!el) { return; }
            var cls = (type === 'success') ? 'alert-success' : (type === 'warning' ? 'alert-warning' : 'alert-danger');
            el.innerHTML = '<div class="alert ' + cls + '" style="padding:8px;margin-bottom:8px;">' + chEscapeHtml(msg) + '</div>';
        }

        function buildChannelTypeOptions(types) {
            var html = '<option value="">Tidak Digunakan</option>';
            for (var t = 0; t < (types || []).length; t++) {
                var c = (types[t].ChanelTypeCode || '').toString();
                if (!c) { continue; }
                var nm = (types[t].ChanelTypeName || c).toString();
                html += '<option value="' + chEscapeHtml(c) + '">' + chEscapeHtml(nm) + '</option>';
            }
            return html;
        }

        function renderChannelRows(master, existing, types) {
            types = types || [];
            var validByUpper = {};
            for (var t = 0; t < types.length; t++) {
                var vc = (types[t].ChanelTypeCode || '').toString();
                if (vc) { validByUpper[vc.toUpperCase()] = vc; }
            }
            var optionsHtml = buildChannelTypeOptions(types);
            var map = {};
            for (var i = 0; i < (existing || []).length; i++) {
                map[String(existing[i].ChanelID)] = (existing[i].ChanelType || '').toUpperCase();
            }
            var rows = document.getElementById('channelSettingRows');
            rows.innerHTML = '';
            for (var j = 0; j < (master || []).length; j++) {
                var m = master[j];
                var cur = map[String(m.ChanelID)] || '';
                var label = m.ChanelName || ('Channel ' + m.ChanelID);
                var code = m.ChanelCode ? (' <small class="text-muted">(' + chEscapeHtml(m.ChanelCode) + ')</small>') : '';
                var tr = document.createElement('tr');
                var tdLabel = document.createElement('td');
                tdLabel.innerHTML = '<strong>' + chEscapeHtml(label) + '</strong>' + code;
                var tdSel = document.createElement('td');
                var sel = document.createElement('select');
                sel.className = 'form-control input-sm channel-type-select';
                sel.setAttribute('data-chanelid', m.ChanelID);
                sel.innerHTML = optionsHtml;
                sel.value = validByUpper[cur] ? validByUpper[cur] : '';
                tdSel.appendChild(sel);
                tr.appendChild(tdLabel);
                tr.appendChild(tdSel);
                rows.appendChild(tr);
            }
        }

        function loadViewChannelRows() {
            var loading = document.getElementById('channelSettingLoading');
            var tbl = document.getElementById('channelSettingTable');
            var btn = document.getElementById('btnSaveChannelSetting');
            document.getElementById('channelSettingAlert').innerHTML = '';
            if (loading) { loading.style.display = 'block'; }
            if (tbl) { tbl.style.display = 'none'; }
            if (btn) { btn.disabled = true; }

            $.ajax({
                url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                data: { action: 'load', nosn: viewChannelNoSN, maxchannel: viewChannelMax }
            }).done(function (res) {
                if (loading) { loading.style.display = 'none'; }
                if (!res || !res.success) {
                    showViewChannelAlert('danger', (res && res.message) || 'Gagal memuat channel.');
                    return;
                }
                renderChannelRows(res.master || [], res.existing || [], res.types || []);
                if (tbl) { tbl.style.display = ''; }
                if (btn) { btn.disabled = false; }
            }).fail(function () {
                if (loading) { loading.style.display = 'none'; }
                showViewChannelAlert('danger', 'Gagal terhubung ke server.');
            });
        }

        function openViewChannelModal(nosn, deviceType, maxChannel) {
            nosn = CheckNbsp(nosn || '').toString().trim();
            deviceType = CheckNbsp(deviceType || '').toString().trim();
            var maxFromGrid = parseInt(maxChannel, 10) || 0;
            if (!nosn) {
                alert('NoSN tidak valid.');
                return;
            }

            viewChannelNoSN = nosn;
            viewChannelDeviceType = deviceType;
            viewChannelMax = maxFromGrid;

            document.getElementById('chNoSN').textContent = nosn;
            document.getElementById('chDeviceType').textContent = deviceType || '-';
            document.getElementById('chMaxChannel').textContent = viewChannelMax > 0 ? viewChannelMax : '-';
            document.getElementById('channelSettingAlert').innerHTML = '';
            document.getElementById('channelSettingRows').innerHTML = '';
            document.getElementById('channelSettingTable').style.display = 'none';
            document.getElementById('channelSettingLoading').style.display = 'block';
            document.getElementById('btnSaveChannelSetting').disabled = true;

            $('#modal-channel-setting').modal('show');

            // Jika MaxChannel sudah dari grid (IsRequireChannelSetting), langsung load.
            // Fallback: cek require via ashx.
            if (viewChannelMax > 0) {
                loadViewChannelRows();
                return;
            }

            $.ajax({
                url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                data: { action: 'require', nosn: nosn }
            }).done(function (res) {
                if (!res || !res.success || !res.isRequire) {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    showViewChannelAlert('warning', 'Device ini tidak memerlukan setting channel MDVR.');
                    return;
                }
                viewChannelMax = parseInt(res.maxChannel, 10) || 0;
                if (viewChannelMax <= 0) {
                    document.getElementById('channelSettingLoading').style.display = 'none';
                    showViewChannelAlert('warning', 'Max Channel tidak valid.');
                    return;
                }
                document.getElementById('chMaxChannel').textContent = viewChannelMax;
                loadViewChannelRows();
            }).fail(function () {
                document.getElementById('channelSettingLoading').style.display = 'none';
                showViewChannelAlert('danger', 'Gagal terhubung ke server.');
            });
        }

        function saveViewChannelSetting() {
            if (!viewChannelNoSN || viewChannelMax <= 0) {
                showViewChannelAlert('danger', 'NoSN atau MaxChannel tidak valid.');
                return;
            }
            var selects = document.querySelectorAll('#channelSettingRows .channel-type-select');
            var settings = [];
            var seen = {};
            for (var i = 0; i < selects.length; i++) {
                var cid = parseInt(selects[i].getAttribute('data-chanelid'), 10);
                if (seen[cid]) {
                    showViewChannelAlert('danger', 'ChanelID ' + cid + ' duplikat.');
                    return;
                }
                seen[cid] = true;
                settings.push({ ChanelID: cid, ChanelType: selects[i].value });
            }

            var btn = document.getElementById('btnSaveChannelSetting');
            if (btn) {
                btn.disabled = true;
                btn.innerHTML = '<i class="fa fa-circle-o-notch fa-spin"></i> Menyimpan...';
            }

            $.ajax({
                url: CH_HANDLER, type: 'POST', dataType: 'json',
                data: {
                    action: 'save',
                    nosn: viewChannelNoSN,
                    maxchannel: viewChannelMax,
                    settings: JSON.stringify(settings)
                }
            }).done(function (res) {
                if (btn) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="fa fa-save"></i> Save Setting';
                }
                if (!res || !res.success) {
                    showViewChannelAlert('danger', (res && res.message) || 'Gagal menyimpan setting.');
                    return;
                }
                showViewChannelAlert('success', res.message || 'Setting channel tersimpan.');
                markViewChannelConfigured(viewChannelNoSN, !!res.isConfigured);
            }).fail(function () {
                if (btn) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="fa fa-save"></i> Save Setting';
                }
                showViewChannelAlert('danger', 'Gagal terhubung ke server.');
            });
        }

        function markViewChannelConfigured(nosn, isConfigured) {
            var cells = document.querySelectorAll('.channel-status-cell');
            for (var i = 0; i < cells.length; i++) {
                if ((cells[i].getAttribute('data-nosn') || '') === nosn) {
                    var badge = cells[i].querySelector('.label-channel-status');
                    var btn = cells[i].querySelector('.btn-channel-setting');
                    if (isConfigured) {
                        if (badge) { badge.className = 'label-channel-status label label-success'; badge.textContent = 'Sudah disetting'; }
                        if (btn) { btn.className = 'btn-channel-setting btn btn-xs btn-primary'; btn.innerHTML = '<i class="fa fa-sliders"></i> Edit Setting'; }
                    } else {
                        if (badge) { badge.className = 'label-channel-status label label-warning'; badge.textContent = 'Belum disetting'; }
                        if (btn) { btn.className = 'btn-channel-setting btn btn-xs btn-warning'; btn.innerHTML = '<i class="fa fa-sliders"></i> Setting Channel'; }
                    }
                }
            }
        }

        // Event delegation sama seperti device_qc (tombol dari BuildStatusCellHtml)
        $(document).on('click', '.btn-channel-setting', function () {
            openViewChannelModal(
                this.getAttribute('data-nosn'),
                this.getAttribute('data-devicetype'),
                this.getAttribute('data-maxchannel')
            );
        });

        endRequest();
    </script>
</asp:Content>
