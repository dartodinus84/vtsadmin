<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="new_install.aspx.cs" Inherits="vtsadm.new_install" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }

    </style>
    <section class="content-header">
        <h1>New Installation
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Installation</a></li>
            <li class="active">New Installation</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-4 col-xs-12">
                <div id="box-jo" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Order Information</h3>
                        <div class="box-tools pull-right">
                            <button id="btnid" type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Job ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtJobID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Register Date</label>
                            <input type="text" id="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtJobCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtJobCustName" runat="server" class="form-control" placeholder="Customer Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Po ID</label>
                            <input type="text" id="txtPoID" runat="server" class="form-control" placeholder="Po ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtPoID" runat="server" class="form-control" placeholder="Po ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="text" id="txtSchDate" runat="server" class="form-control" placeholder="Schedule Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtSchDate" runat="server" class="form-control" placeholder="Schedule Date ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div id="box-vehicle" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vehicle Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtVehicleID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-vehicle"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Description</label>
                            <input type="text" id="txtVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Police No</label>
                            <input type="text" id="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." required="required" ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Asset No</label>
                            <input type="text" id="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <input type="text" id="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." required="required" ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div id="box-customer" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." ReadOnly="true"></asp:TextBox>--%>
                            <input type="hidden" id="txtCustID" runat="server" />
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdLoadCustomer" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadCustomer_ServerClick" Text="Load" />
                        <asp:Button ID="CmdLoadAll" CssClass="btn btn-primary" runat="server" Style="visibility:hidden;" OnClick="CmdLoadAll_Click" Text="Load All" />
                    </div>
                </div>
                <div id="box-server" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Server Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <asp:DropDownList ID="CmbCustServerID" AutoPostBack="true" runat="server" CssClass="form-control" OnTextChanged="CmbCustServerID_TextChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>


            </div>
            <div class="col-md-4 col-xs-12">
                <div id="box-upline" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Upline</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="UplineID" HeaderText="Upline ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineFullName" HeaderText="Upline Full Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineAddress" HeaderText="Upline Address" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingUpline" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div id="box-master" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Master</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView3" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView3_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="UplineID" HeaderText="Upline ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="UplineFullName" HeaderText="Upline Full Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MasterID" HeaderText="Master ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MasterFullName" HeaderText="Master Full Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MasterAddress" HeaderText="Master Address" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingMaster" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div id="box-technician" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Technician Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Technician ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtTechnicianID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button3" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-technician"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Employee No</label>
                            <input type="text" id="txtEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Name</label>
                            <input type="text" id="txtName" runat="server" class="form-control" placeholder="Technician Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Technician Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtTechBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTechBranchName" runat="server" class="form-control" placeholder="Branch Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div id="box-device" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Device Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtDeviceID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-device"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No SN</label>
                            <input type="text" id="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vendor Name</label>
                            <input type="text" id="txtVendorName" runat="server" class="form-control" placeholder="Vendor Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtVendorName" runat="server" class="form-control" placeholder="Vendor Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Brand</label>
                            <input type="text" id="txtDeviceBrand" runat="server" class="form-control" placeholder="Device Brand ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Model</label>
                            <input type="text" id="txtDeviceModel" runat="server" class="form-control" placeholder="Device Model ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type Description</label>
                            <input type="text" id="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <input type="text" id="txtDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Name</label>
                            <input type="text" id="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tdt ID</label>
                            <input type="text" id="txtTdtID" runat="server" class="form-control" placeholder="Tdt ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTdtID" runat="server" class="form-control" placeholder="Tdt ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <input type="text" id="txtDeviceServerName" runat="server" class="form-control" placeholder="Server Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceServerName" runat="server" class="form-control" placeholder="Server Name ..." ReadOnly="true"></asp:TextBox>--%>
                            <input type="hidden" id="txtDeviceServerID" runat="server" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Contrac Time</label>
                            <input type="text" id="txtContract" runat="server" class="form-control" placeholder="Contract Time ..." readonly="readonly" />
                        </div>
                    </div>
                </div>


            </div>
            <div class="col-md-4 col-xs-12">
                <div id="box-gsm" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button4" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-gsm"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>MSIDN</label>
                            <input type="text" id="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Provider Name</label>
                            <input type="text" id="txtProviderName" runat="server" class="form-control" placeholder="Provider Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtProviderName" runat="server" class="form-control" placeholder="Provider Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <input type="text" id="txtGsmSourceName" runat="server" class="form-control" placeholder="Source Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtGsmSourceName" runat="server" class="form-control" placeholder="Source Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tgt ID</label>
                            <input type="text" id="txtTgtID" runat="server" class="form-control" placeholder="Tgt ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTgtID" runat="server" class="form-control" placeholder="Tgt ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div id="box-installation" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Installation Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div id="pnlChannelSetting" class="form-group form-group-sm" style="display: none;">
                            <label>Setting Channel MDVR</label>
                            <div id="channelSettingAlert"></div>
                            <div id="channelSettingLoading" style="display: none; margin-bottom: 8px;">
                                <i class="fa fa-circle-o-notch fa-spin"></i> Memuat channel...
                            </div>
                            <table class="table table-bordered" id="channelSettingTable" style="display: none; margin-bottom: 8px;">
                                <thead>
                                    <tr>
                                        <th style="width: 55%;">Channel</th>
                                        <th>Tipe Channel</th>
                                    </tr>
                                </thead>
                                <tbody id="channelSettingRows"></tbody>
                            </table>
                            <button type="button" class="btn btn-primary btn-sm" id="btnSaveChannelSetting" onclick="saveInstallChannelSetting();" disabled="disabled">
                                <i class="fa fa-save"></i>&nbsp;Save Setting Channel
                            </button>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date</label>
                            <%--<input type="text" id="txtDate" runat="server" class="form-control" placeholder="Date ..." readonly="readonly" />--%>
                            <asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <input type="text" id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." />
                            
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Relay</label>
                            <select id="txtRelay" runat="server" class="form-control">
                                <option value="1">Yes</option>
                                <option value="0">No</option>
                            </select>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Telegram CC</label>
                            <input type="text" id="txtTelegram" runat="server" class="form-control" placeholder="Telegram ..." />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Picture Attachment</label>
                            <p>
                                <button id="CmdUpload1" type="button" class="btn btn-primary" onclick="$('#modal-picture1').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 1</button>
                                <button id="CmdUpload2" type="button" class="btn btn-primary" onclick="$('#modal-picture2').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 2</button>
                                <button id="CmdUpload3" type="button" class="btn btn-primary" onclick="$('#modal-picture3').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 3</button>
                                <button id="CmdUpload4" type="button" class="btn btn-primary" onclick="$('#modal-picture4').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 4</button>
                                <button id="CmdUpload5" type="button" class="btn btn-primary" onclick="$('#modal-picture5').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 5</button>
                            </p>
                            <p>
                                <button id="CmdUpload6" type="button" class="btn btn-primary" onclick="$('#modal-picture6').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 6</button>
                                <button id="CmdUpload7" type="button" class="btn btn-primary" onclick="$('#modal-picture7').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 7</button>
                                <button id="CmdUpload8" type="button" class="btn btn-primary" onclick="$('#modal-picture8').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 8</button>
                                <button id="CmdUpload9" type="button" class="btn btn-primary" onclick="$('#modal-picture9').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 9</button>
                                <button id="CmdUpload10" type="button" class="btn btn-primary" onclick="$('#modal-picture10').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image 10</button>
                            </p>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdUser" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-useraccess').modal('show');return false;" Text="User Access" />
                        <asp:Button ID="CmdTelegram" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-telegram').modal('show');return false;" Text="Telegrams" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <!-- <asp:Button ID="CmdSubmitDev" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit-dev').modal('show');return false;" Text="Submit Dev" /> -->
                    </div>
                </div>
                <div id="box-transaction" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Transaction Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Tvd ID</label>
                            <input type="text" id="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdAddAccessories" CssClass="btn btn-primary" runat="server" OnClientClick="return addAcc();" OnClick="CmdAddAccessories_ServerClick" Text="Add Accessories" />  <!---->
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Accessories Available</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchAvai" runat="server" class="form-control pull-right" placeholder="Search by No SN ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearchAvai" runat="server" type="button" class="btn btn-primary" onserverclick="CmdSearchAvai_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-success btn-xs" Text="<i class='fa fa-check'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Select"></asp:ButtonField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingA" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Accessories Selected</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearchSel" runat="server" class="form-control pull-right" placeholder="Search by No SN ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearchSel" runat="server" type="button" class="btn btn-primary" onserverclick="CmdSearchSel_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView4" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView4_PageIndexChanging" OnRowCommand="GridView4_RowCommand">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-danger btn-xs" Text="<i class='fa fa-close'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Remove"></asp:ButtonField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingS" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-submit-dev" data-keyboard="false" data-backdrop="static">
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
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iloaddev"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdSubmitYesDev_ServerClick" id="CmdSubmitYesDev">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit-dev').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-submit" data-keyboard="false" data-backdrop="static">
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
                        <img src="Content/ajax-loader2.gif" style="display:none;" id="iload"/>
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload.aspx" style="width: 100%; border: none; height: 3280px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload1.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture2">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload2.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture3">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload3.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture4">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload4.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture5">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload5.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>



        <div class="modal fade bs-example-modal-lg" id="modal-picture6">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload6.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture7">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload7.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture8">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload8.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture9">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload9.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture10">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="new_installation_upload10.aspx" style="width: 100%; border: none; min-height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-job_order_acc">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order Accessories</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="new_installation_job_order_acc_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-job_order">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Job Order</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="new_installation_job_order_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-vehicle">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframevehicle" src="new_installation_vehicle_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-technician">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="new_installation_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-device">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedevice" src="new_installation_device_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-gsm">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">GSM</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframegsm" src="new_installation_gsm_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="$('.modal-backdrop').remove();">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Info Box</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" id="div_comment" runat="server">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" onclick="$('.modal-backdrop').remove();" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-useraccess">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">User Access</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView5" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="300">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="autoid" HeaderText="Auto ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="user_id" HeaderText="User ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="user_nm" HeaderText="User Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingUserAccess" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-telegram">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Telegram Account</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto" Height="350px">
                                <asp:GridView ID="GridView11" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100" OnRowDataBound="GridView11_RowDataBound">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TelegramName" HeaderText="Telegram Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AccountID" HeaderText="AccountID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Check" HeaderStyle-CssClass="gv-header-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk1" runat="server" Enabled="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPagingTelegram" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
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

        function addAcc() {
            var boolOK = false;
            var sTvdID = $('#ContentPlaceHolder1_txtTvdID').val();
            console.log(sTvdID);
            if (sTvdID != "undefined" && sTvdID != "") {
                console.log('undefined');
                boolOK = true;
            }
            else {
                $('#modal-job_order_acc').modal('show');
                console.log('kosong');
                boolOK = false;
            }

            return boolOK;
        }

        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }
        function postJobOrderAccChild(sJobID, sFullName, sPoliceNo, sNoSN, sMsidn, sInstallDate, objCmd,
            sRegDate, sPOID, sSchDate, sVehID, sVehDesc, sAssetNo, sTvaID, sBranchName, sServerID,
            sTechID, sTechName, sEmpNo, sDeviceID, sVendorName, sDeviceTypeDesc, sSourceID, sWarehouseID, sTdtID, sGsmID,
            sProviderName, sTgtID, sRemark, sTvdID, sCustID) {
            if (sJobID != '') {
                document.getElementById('ContentPlaceHolder1_txtJobID').value = checkNbsp(sJobID);
                document.getElementById('ContentPlaceHolder1_txtRegDate').value = checkNbsp(sRegDate);
                document.getElementById('ContentPlaceHolder1_txtJobCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtPoID').value = checkNbsp(sPOID);
                document.getElementById('ContentPlaceHolder1_txtSchDate').value = checkNbsp(sSchDate);

                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = checkNbsp(sVehID);
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = checkNbsp(sVehDesc);
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = checkNbsp(sTvaID);

                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtCustID').value = checkNbsp(sCustID);


                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = checkNbsp(sTechID);
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = checkNbsp(sEmpNo);
                document.getElementById('ContentPlaceHolder1_txtName').value = checkNbsp(sTechName);
                document.getElementById('ContentPlaceHolder1_txtTechBranchName').value = checkNbsp(sBranchName);

                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtDeviceSourceName').value = checkNbsp(sSourceID);
                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = checkNbsp(sWarehouseID);
                document.getElementById('ContentPlaceHolder1_txtTdtID').value = checkNbsp(sTdtID);
                document.getElementById('ContentPlaceHolder1_txtDeviceServerName').value = checkNbsp(sServerID);

                document.getElementById('ContentPlaceHolder1_txtGsmID').value = checkNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = checkNbsp(sMsidn);
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = checkNbsp(sProviderName);
                document.getElementById('ContentPlaceHolder1_txtTgtID').value = checkNbsp(sTgtID);

                document.getElementById('ContentPlaceHolder1_txtDate').value = checkNbsp(sInstallDate);
                document.getElementById('ContentPlaceHolder1_txtRemark').value = checkNbsp(sRemark);
                document.getElementById('ContentPlaceHolder1_txtTvdID').value = checkNbsp(sTvdID);

                var cmdLoadAll = document.getElementById('ContentPlaceHolder1_CmdLoadAll');
                cmdLoadAll.click();

                $('#modal-job_order_acc').modal('hide');

                if (typeof refreshInstallChannelSetting === 'function') {
                    refreshInstallChannelSetting();
                }

                //var objServerID = document.getElementById('ContentPlaceHolder1_CmbCustServerID');
                //console.log(objServerID);
                //objServerID.value = sServerID;

                //var cmdAdd = document.getElementById('ContentPlaceHolder1_CmdAddAccessories');
                //cmdAdd.click();
            }
        }
        function postJobOrderChild(sJobID, sRegDate, sFullName, sPOID, sDeviceTypeDesc, sSchDate, objCmd, sCustID) {
            if (sJobID != '') {
                document.getElementById('ContentPlaceHolder1_txtJobID').value = checkNbsp(sJobID);
                document.getElementById('ContentPlaceHolder1_txtRegDate').value = checkNbsp(sRegDate);
                document.getElementById('ContentPlaceHolder1_txtJobCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtPoID').value = checkNbsp(sPOID);
                document.getElementById('ContentPlaceHolder1_txtSchDate').value = checkNbsp(sSchDate);

                $('#modal-job_order').modal('hide');

                var objfr = document.getElementById('iframevehicle').contentWindow;
                var objJobID = objfr.document.getElementById('txtJobID');
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearchVehicle');
                objCustID.value = sCustID;
                cmdSearch.click();
                //var objfr1 = document.getElementById('ContentPlaceHolder1_CmdLoadCustomer');
                //objfr1.click();
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }
        function postVehicleChild(sTvaID, sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sCustBranchName, sCustName, sCustID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = checkNbsp(sTvaID);
                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = checkNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = checkNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sCustBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sCustName);
                document.getElementById('ContentPlaceHolder1_txtCustID').value = checkNbsp(sCustID);

                $('#modal-vehicle').modal('hide');

                var objfr1 = document.getElementById('ContentPlaceHolder1_CmdLoadCustomer');
                objfr1.click();
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postDeviceChild(sTdtID, sDeviceID, sNoSN, sVendorName, sDeviceBrand, sDeviceModel, sDeviceTypeDesc, sSourceName, sWarehouseName, sServerID, sServerName, sContract) {
            if (sTdtID != '') {
                document.getElementById('ContentPlaceHolder1_txtTdtID').value = checkNbsp(sTdtID);
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtDeviceBrand').value = checkNbsp(sDeviceBrand);
                document.getElementById('ContentPlaceHolder1_txtDeviceModel').value = checkNbsp(sDeviceModel);
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtDeviceSourceName').value = checkNbsp(sSourceName);
                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = checkNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_txtDeviceServerID').value = checkNbsp(sServerID);
                document.getElementById('ContentPlaceHolder1_txtDeviceServerName').value = checkNbsp(sServerName);
                document.getElementById('ContentPlaceHolder1_txtContract').value = checkNbsp(sContract);

                $('#modal-device').modal('hide');

                if (typeof refreshInstallChannelSetting === 'function') {
                    refreshInstallChannelSetting();
                }

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postTechnicianChild(sTechnicianID, sEmpNo, sName, sBranchName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = checkNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = checkNbsp(sEmpNo);
                document.getElementById('ContentPlaceHolder1_txtName').value = checkNbsp(sName);
                document.getElementById('ContentPlaceHolder1_txtTechBranchName').value = checkNbsp(sBranchName);

                $('#modal-technician').modal('hide');

                var objfr1 = document.getElementById('iframedevice').contentWindow;
                var objJobID = objfr1.document.getElementById('txtJobID');
                var objTechID = objfr1.document.getElementById('txtTechnicianID');
                var cmdSearch = objfr1.document.getElementById('CmdSearchDevice');
                objJobID.value = document.getElementById('ContentPlaceHolder1_txtJobID').value;
                objTechID.value = sTechnicianID;
                cmdSearch.click();

                var objfr2 = document.getElementById('iframegsm').contentWindow;
                var objTechIDGsm = objfr2.document.getElementById('txtTechnicianID');
                var cmdSearchGsm = objfr2.document.getElementById('CmdSearchGsm');
                objTechIDGsm.value = sTechnicianID;
                cmdSearchGsm.click();
                //alert(objtest);
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postGsmChild(sTgtID, sGsmID, sMSIDN, sProviderName, sSourceName) {
            if (sTgtID != '') {
                document.getElementById('ContentPlaceHolder1_txtTgtID').value = checkNbsp(sTgtID);
                document.getElementById('ContentPlaceHolder1_txtGsmID').value = checkNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = checkNbsp(sMSIDN);
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = checkNbsp(sProviderName);
                document.getElementById('ContentPlaceHolder1_txtGsmSourceName').value = checkNbsp(sSourceName);

                $('#modal-gsm').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function endRequest(sender, args) {
            $('#modal-messagebox').on('hidden.bs.modal', function () {
                document.body.style.paddingRight = '0px';
            });
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
            //allboxslideup();

        }


        function allboxslideup() {
            //document.getElementById('box-jo').className = "box box-solid collapsed-box";
            document.getElementById('box-vehicle').className = "box box-solid collapsed-box";
            document.getElementById('box-customer').className = "box box-solid collapsed-box";
            document.getElementById('box-server').className = "box box-solid collapsed-box";
            document.getElementById('box-upline').className = "box box-solid collapsed-box";
            document.getElementById('box-master').className = "box box-solid collapsed-box";
            document.getElementById('box-technician').className = "box box-solid collapsed-box";
            document.getElementById('box-device').className = "box box-solid collapsed-box";
            document.getElementById('box-gsm').className = "box box-solid collapsed-box";
            document.getElementById('box-installation').className = "box box-solid collapsed-box";
            document.getElementById('box-transaction').className = "box box-solid collapsed-box";
        }

        // ===== Setting Channel MDVR (Installation Information) =====
        // Reuse mdvr_channel.ashx (load/save/require). Tidak mengubah flow submit install.
        var CH_HANDLER = 'mdvr_channel.ashx';
        var installChannelNoSN = '';
        var installChannelMax = 0;

        function chEscapeHtml(s) {
            return (s == null ? '' : String(s))
                .replace(/&/g, '&amp;').replace(/</g, '&lt;')
                .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        }

        function hideInstallChannelSetting() {
            installChannelNoSN = '';
            installChannelMax = 0;
            var pnl = document.getElementById('pnlChannelSetting');
            if (pnl) { pnl.style.display = 'none'; }
            var alertEl = document.getElementById('channelSettingAlert');
            if (alertEl) { alertEl.innerHTML = ''; }
            var rows = document.getElementById('channelSettingRows');
            if (rows) { rows.innerHTML = ''; }
            var tbl = document.getElementById('channelSettingTable');
            if (tbl) { tbl.style.display = 'none'; }
            var btn = document.getElementById('btnSaveChannelSetting');
            if (btn) { btn.disabled = true; }
            var loading = document.getElementById('channelSettingLoading');
            if (loading) { loading.style.display = 'none'; }
        }

        function showChannelAlert(type, msg) {
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

        function loadInstallChannelRows() {
            var loading = document.getElementById('channelSettingLoading');
            var tbl = document.getElementById('channelSettingTable');
            var btn = document.getElementById('btnSaveChannelSetting');
            var alertEl = document.getElementById('channelSettingAlert');
            if (alertEl) { alertEl.innerHTML = ''; }
            if (loading) { loading.style.display = 'block'; }
            if (tbl) { tbl.style.display = 'none'; }
            if (btn) { btn.disabled = true; }

            $.ajax({
                url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                data: { action: 'load', nosn: installChannelNoSN, maxchannel: installChannelMax }
            }).done(function (res) {
                if (loading) { loading.style.display = 'none'; }
                if (!res || !res.success) {
                    showChannelAlert('danger', (res && res.message) || 'Gagal memuat channel.');
                    return;
                }
                renderChannelRows(res.master || [], res.existing || [], res.types || []);
                if (tbl) { tbl.style.display = ''; }
                if (btn) { btn.disabled = false; }
            }).fail(function () {
                if (loading) { loading.style.display = 'none'; }
                showChannelAlert('danger', 'Gagal terhubung ke server.');
            });
        }

        function refreshInstallChannelSetting() {
            var nosnEl = document.getElementById('ContentPlaceHolder1_txtNoSN');
            var nosn = nosnEl ? (nosnEl.value || '').toString().trim() : '';
            if (!nosn) {
                hideInstallChannelSetting();
                return;
            }

            $.ajax({
                url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                data: { action: 'require', nosn: nosn }
            }).done(function (res) {
                if (!res || !res.success || !res.isRequire) {
                    hideInstallChannelSetting();
                    return;
                }
                installChannelNoSN = nosn;
                installChannelMax = parseInt(res.maxChannel, 10) || 0;
                if (installChannelMax <= 0) {
                    hideInstallChannelSetting();
                    return;
                }
                document.getElementById('pnlChannelSetting').style.display = '';
                loadInstallChannelRows();
            }).fail(function () {
                hideInstallChannelSetting();
            });
        }

        function saveInstallChannelSetting() {
            if (!installChannelNoSN || installChannelMax <= 0) {
                showChannelAlert('danger', 'NoSN atau MaxChannel tidak valid.');
                return;
            }
            var selects = document.querySelectorAll('#channelSettingRows .channel-type-select');
            var settings = [];
            var seen = {};
            for (var i = 0; i < selects.length; i++) {
                var cid = parseInt(selects[i].getAttribute('data-chanelid'), 10);
                if (seen[cid]) {
                    showChannelAlert('danger', 'ChanelID ' + cid + ' duplikat.');
                    return;
                }
                seen[cid] = true;
                settings.push({ ChanelID: cid, ChanelType: selects[i].value });
            }

            var btn = document.getElementById('btnSaveChannelSetting');
            if (btn) {
                btn.disabled = true;
                btn.innerHTML = '<i class="fa fa-circle-o-notch fa-spin"></i>&nbsp;Menyimpan...';
            }

            $.ajax({
                url: CH_HANDLER, type: 'POST', dataType: 'json',
                data: {
                    action: 'save',
                    nosn: installChannelNoSN,
                    maxchannel: installChannelMax,
                    settings: JSON.stringify(settings)
                }
            }).done(function (res) {
                if (btn) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="fa fa-save"></i>&nbsp;Save Setting Channel';
                }
                if (!res || !res.success) {
                    showChannelAlert('danger', (res && res.message) || 'Gagal menyimpan setting.');
                    return;
                }
                showChannelAlert('success', res.message || 'Setting channel tersimpan.');
            }).fail(function () {
                if (btn) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="fa fa-save"></i>&nbsp;Save Setting Channel';
                }
                showChannelAlert('danger', 'Gagal terhubung ke server.');
            });
        }

        endRequest();
    </script>
</asp:Content>