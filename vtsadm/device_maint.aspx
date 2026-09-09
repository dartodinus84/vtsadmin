<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_maint.aspx.cs" Inherits="vtsadm.device_maint" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }
        /* Chrome/WebKit: iframe di dalam modal Bootstrap sering tampil blank */
        #modal-devicenew.fade .modal-dialog,
        #modal-device.fade .modal-dialog,
        #modal-job_order.fade .modal-dialog {
            -webkit-transform: none;
            transform: none;
        }
        #modal-devicenew .modal-body,
        #modal-device .modal-body {
            min-height: 360px;
        }
    </style>
    <section class="content-header">
        <h1>Device Maintenance
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Installation</a></li>
            <li><a href="#">Maintenance</a></li>
            <li class="active">Device</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Job Order Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Job ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtJobID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_order" onclick="ensureJobOrderIframe();"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Register Date</label>
                            <input type="text" id="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtJobCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtJobCustName" runat="server" class="form-control" placeholder="Customer Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Po ID</label>
                            <input type="text" id="txtPoID" runat="server" class="form-control" placeholder="Po ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtPoID" runat="server" class="form-control" placeholder="Po ID ..." readonly="readonly"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <input type="text" id="txtSchDate" runat="server" class="form-control" placeholder="Schedule Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtSchDate" runat="server" class="form-control" placeholder="Schedule Date ..." readonly="readonly"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Device Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtDeviceID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-device" onclick="ensureDeviceIframe();"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No SN</label>
                            <input type="text" id="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="No SN ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vendor Name</label>
                            <input type="text" id="txtVendorName" runat="server" class="form-control" placeholder="Vendor Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtVendorName" runat="server" class="form-control" placeholder="Vendor Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type Description</label>
                            <input type="text" id="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <input type="text" id="txtDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Name</label>
                            <input type="text" id="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tdt ID</label>
                            <input type="text" id="txtTdtID" runat="server" class="form-control" placeholder="Tdt ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTdtID" runat="server" class="form-control" placeholder="Tdt ID ..."></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdLoadData" type="button" class="btn btn-primary" runat="server" onserverclick="CmdLoadData_ServerClick">Load</button>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vehicle Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <input type="text" id="txtVehicleID" runat="server" class="form-control" placeholder="Vehicle ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtVehicleID" runat="server" class="form-control" placeholder="Vehicle ID ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Description</label>
                            <input type="text" id="txtVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Police No</label>
                            <input type="text" id="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Asset No</label>
                            <input type="text" id="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <input type="text" id="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." required="required"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <input type="text" id="txtCustID" runat="server" class="form-control" placeholder="Customer ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustID" runat="server" class="form-control" placeholder="Customer ID ..." required="required"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." required="required"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Upline</h3>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Master</h3>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Technician Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Technician ID</label>
                            <input type="text" id="txtTechnicianID" runat="server" class="form-control" placeholder="Technician ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTechnicianID" runat="server" class="form-control" placeholder="Technician ID ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Employee No</label>
                            <input type="text" id="txtEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Name</label>
                            <input type="text" id="txtName" runat="server" class="form-control" placeholder="Technician Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Technician Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtTechBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTechBranchName" runat="server" class="form-control" placeholder="Branch Name ..."></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Gsm ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtGsmID" runat="server" class="form-control" placeholder="Gsm ID ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>MSIDN</label>
                            <input type="text" id="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Provider Name</label>
                            <input type="text" id="txtProviderName" runat="server" class="form-control" placeholder="Provider Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtProviderName" runat="server" class="form-control" placeholder="Provider Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <input type="text" id="txtGsmSourceName" runat="server" class="form-control" placeholder="Source Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtGsmSourceName" runat="server" class="form-control" placeholder="Source Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tgt ID</label>
                            <input type="text" id="txtTgtID" runat="server" class="form-control" placeholder="Tgt ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTgtID" runat="server" class="form-control" placeholder="Tgt ID ..."></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Installation Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Date</label>
                            <input type="text" id="txtDate" runat="server" class="form-control" placeholder="Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <input type="text" id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Telegram CC</label>
                            <input type="text" id="txtTelegram" runat="server" class="form-control" placeholder="Telegram ..." />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warannty Date</label>
                            <input type="text" id="txtWaranty" runat="server" class="form-control" placeholder="Date ..." />
                            <%--<asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date ..."></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Transaction Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Tvd ID</label>
                            <input type="text" id="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..."></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Accessories Selected</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView4" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView4_PageIndexChanging">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
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

                <div id="box-server" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Server Information</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <asp:DropDownList ID="CmbCustServerID" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="CmbCustServerID_TextChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Device Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtNewDeviceID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" onclick="openNewDeviceSearch();return false;"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No SN</label>
                            <input type="text" id="txtNewNoSN" runat="server" class="form-control" placeholder="No SN ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewNoSN" runat="server" class="form-control" placeholder="No SN ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vendor Name</label>
                            <input type="text" id="txtNewVendorName" runat="server" class="form-control" placeholder="Vendor Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewVendorName" runat="server" class="form-control" placeholder="Vendor Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type Description</label>
                            <input type="text" id="txtNewDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <input type="text" id="txtNewDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewDeviceSourceName" runat="server" class="form-control" placeholder="Source Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Name</label>
                            <input type="text" id="txtNewWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..."></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tdt ID</label>
                            <input type="text" id="txtNewTdtID" runat="server" class="form-control" placeholder="Tdt ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewTdtID" runat="server" class="form-control" placeholder="Tdt ID ..."></asp:TextBox>--%>
                        </div>
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
                            <button type="button" class="btn btn-primary btn-sm" id="btnSaveChannelSetting" onclick="saveMaintChannelSetting();" disabled="disabled">
                                <i class="fa fa-save"></i>&nbsp;Save Setting Channel
                            </button>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Status Old Device</label>
                            <asp:DropDownList ID="CmbStatusOldDevice" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>New Remark</label>
                            <asp:TextBox ID="txtNewRemark" runat="server" class="form-control" placeholder="New Remark ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Picture Attachment</label>
                            <p>
                                <button id="CmdUpload" type="button" class="btn btn-primary" onclick="ensurePictureIframe();$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image</button>
                            </p>
                            
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Send Notif to Customer <input type="checkbox" id="textCheckNotif" runat="server" value="1" onclick = "checkNotif(this)" /></label>
                            <div id="dvEmailNotif" style="display: none">
                                <input type="text" id="txtEmailNotif" runat="server" class="form-control" placeholder="Email ..." />
                            </div>
                            
                        </div>

                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdUser" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-useraccess').modal('show');return false;" Text="User Access" />
                        <asp:Button ID="CmdTelegram" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-telegram').modal('show');return false;" Text="Telegrams" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
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
                        <img src="Content/ajax-loader2.gif" style="display: none;" id="iload" />
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
                            <iframe id="iframepicture" src="about:blank" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
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
                            <iframe id="iframejoborder" src="about:blank" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe id="iframedevice" src="about:blank" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-devicenew">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">New Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtSearchNewDevice" class="form-control" placeholder="Search By No SN ..." onkeydown="if(event.keyCode===13){loadNewDeviceSearch();return false;}" />
                                <span class="input-group-btn">
                                    <button type="button" class="btn btn-primary btn-xs" onclick="loadNewDeviceSearch();"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm" style="max-height: 300px; overflow: auto;">
                            <table class="table table-bordered" id="tblNewDeviceSearch" style="margin-bottom: 0;">
                                <thead>
                                    <tr>
                                        <th>Device ID</th>
                                        <th>No SN</th>
                                        <th>Vendor Name</th>
                                        <th>Device Type Desc</th>
                                        <th>Source Name</th>
                                        <th>Warehouse Name</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody id="tblNewDeviceSearchBody">
                                    <tr><td colspan="7">Ketik No SN lalu klik Search.</td></tr>
                                </tbody>
                            </table>
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
                                <asp:GridView ID="GridView5" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="100">
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
        
        
        function checkNotif(obj) {
            var checkBox = obj.checked;
 
            if (checkBox == true) {
                $("#dvEmailNotif").show();
            } else {
                $("#dvEmailNotif").hide();
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function ensurePictureIframe() {
            var fr = document.getElementById('iframepicture');
            if (!fr) { return; }
            var src = (fr.getAttribute('src') || '');
            if (src === '' || src === 'about:blank') {
                fr.src = 'device_maint_upload.aspx';
            }
        }

        function ensureJobOrderIframe() {
            var fr = document.getElementById('iframejoborder');
            if (!fr) { return; }
            var src = (fr.getAttribute('src') || '');
            if (src === '' || src === 'about:blank') {
                fr.src = 'device_maint_job_order_search.aspx';
            }
            // preload device search supaya siap saat Job Order dipilih
            ensureDeviceIframe();
        }

        function ensureDeviceIframe(onReady) {
            var fr = document.getElementById('iframedevice');
            if (!fr) {
                if (typeof onReady === 'function') { onReady(null); }
                return;
            }
            var src = (fr.getAttribute('src') || '');
            if (src === '' || src === 'about:blank') {
                if (typeof onReady === 'function') {
                    fr.onload = function () {
                        fr.onload = null;
                        onReady(fr);
                    };
                }
                fr.src = 'device_maint_device_search.aspx';
                return;
            }
            if (typeof onReady === 'function') { onReady(fr); }
        }

        function reloadNewDeviceIframe() {
            openNewDeviceSearch();
        }

        function openNewDeviceSearch() {
            // Jangan auto-query SP di sini (SP kosong ~20 dtk). Data hanya setelah Search.
            var body = document.getElementById('tblNewDeviceSearchBody');
            if (body) {
                body.innerHTML = '<tr><td colspan="7">Ketik No SN lalu klik Search.</td></tr>';
            }
            $('#modal-devicenew').modal('show');
            setTimeout(function () {
                var inp = document.getElementById('txtSearchNewDevice');
                if (inp) { inp.focus(); }
            }, 300);
        }

        function loadNewDeviceSearch() {
            var q = '';
            var inp = document.getElementById('txtSearchNewDevice');
            if (inp) { q = (inp.value || '').toString().trim(); }
            var body = document.getElementById('tblNewDeviceSearchBody');
            if (!q) {
                if (body) {
                    body.innerHTML = '<tr><td colspan="7">Isi No SN dulu, lalu klik Search.</td></tr>';
                }
                if (inp) { inp.focus(); }
                return;
            }
            if (body) {
                body.innerHTML = '<tr><td colspan="7"><i class="fa fa-circle-o-notch fa-spin"></i> Searching...</td></tr>';
            }
            $.ajax({
                url: 'device_maint_new_device.ashx',
                type: 'GET',
                dataType: 'json',
                cache: false,
                timeout: 30000,
                data: { search: q }
            }).done(function (res) {
                if (!body) { return; }
                if (!res || !res.ok) {
                    body.innerHTML = '<tr><td colspan="7">' + ((res && res.message) || 'Gagal memuat device.') + '</td></tr>';
                    return;
                }
                var rows = res.rows || [];
                if (rows.length === 0) {
                    body.innerHTML = '<tr><td colspan="7">No items to display</td></tr>';
                    return;
                }
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    var r = rows[i];
                    html += '<tr>';
                    html += '<td>' + (r.DeviceID || '') + '</td>';
                    html += '<td>' + (r.NoSN || '') + '</td>';
                    html += '<td>' + (r.vendorname || '') + '</td>';
                    html += '<td>' + (r.devicetypedesc || '') + '</td>';
                    html += '<td>' + (r.sourcename || '') + '</td>';
                    html += '<td>' + (r.warehousename || '') + '</td>';
                    html += '<td style="text-align:center;"><button type="button" class="btn btn-success btn-xs" onclick="selectNewDeviceRow(this);"><i class="fa fa-share"></i></button></td>';
                    html += '</tr>';
                }
                body.innerHTML = html;
                var trs = body.getElementsByTagName('tr');
                for (var j = 0; j < trs.length; j++) {
                    trs[j].setAttribute('data-row', JSON.stringify(rows[j]));
                }
            }).fail(function (xhr, status) {
                var msg = (status === 'timeout') ? 'Request timeout. Coba search lagi.' : 'Gagal terhubung ke server.';
                if (body) {
                    body.innerHTML = '<tr><td colspan="7">' + msg + '</td></tr>';
                }
            });
        }

        function selectNewDeviceRow(btn) {
            var tr = btn;
            while (tr && tr.tagName !== 'TR') { tr = tr.parentNode; }
            if (!tr) { return; }
            var raw = tr.getAttribute('data-row');
            if (!raw) { return; }
            var r = JSON.parse(raw);
            postNewDeviceChild(r.DeviceID, r.NoSN, r.vendorname, r.devicetypedesc, r.sourcename, r.warehousename, r.TdtID);
        }

        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function postJobOrderChild(sJobID, sRegDate, sFullName, sPoID, sDeviceTypeDesc, sSchDate, sPoliceNo, objCmd, sCustID) {
            if (sJobID != '') {
                document.getElementById('ContentPlaceHolder1_txtJobID').value = checkNbsp(sJobID);
                document.getElementById('ContentPlaceHolder1_txtRegDate').value = checkNbsp(sRegDate);
                document.getElementById('ContentPlaceHolder1_txtJobCustName').value = checkNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtPoID').value = checkNbsp(sPoID);
                document.getElementById('ContentPlaceHolder1_txtSchDate').value = checkNbsp(sSchDate);
                $('#modal-job_order').modal('hide');

                ensureDeviceIframe(function (frEl) {
                    try {
                        var objfr = (frEl || document.getElementById('iframedevice')).contentWindow;
                        var objJobID = objfr.document.getElementById('txtJobID');
                        var objCustID = objfr.document.getElementById('txtCustID');
                        var cmdSearch = objfr.document.getElementById('CmdSearchDevice');
                        if (objJobID) { objJobID.value = sJobID; }
                        if (objCustID) { objCustID.value = sCustID; }
                        if (cmdSearch) { cmdSearch.click(); }
                    } catch (e) { }
                });
            }
        }

        function postDeviceChild(sDeviceID, sNoSN, sDeviceTypeDesc, sDeviceSourceName, sWarehouseName, sPoliceNo, sMSIDN, objSelect, sVendorName, sTdtID, sVehicleID, sVehicleDesc,
            sAssetNo, sTvaID, sBranchName_Cust, sCustName, sTechnicianID, sEmployeeNo, sEmployeeName, sBranchName_Technician,
            sGsmID, sProviderName, sGsmSouceName, sTgtID, sInstallDate, sRemark, sTvdID, sCustID, sWarantyDate) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtDeviceSourceName').value = checkNbsp(sDeviceSourceName);
                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = checkNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_txtTdtID').value = checkNbsp(sTdtID);

                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = checkNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = checkNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = checkNbsp(sTvaID);

                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sBranchName_Cust);
                document.getElementById('ContentPlaceHolder1_txtCustID').value = checkNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sCustName);

                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = checkNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = checkNbsp(sEmployeeNo);
                document.getElementById('ContentPlaceHolder1_txtName').value = checkNbsp(sEmployeeName);
                document.getElementById('ContentPlaceHolder1_txtTechBranchName').value = checkNbsp(sBranchName_Technician);

                document.getElementById('ContentPlaceHolder1_txtGsmID').value = checkNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = checkNbsp(sMSIDN);
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = checkNbsp(sProviderName);
                document.getElementById('ContentPlaceHolder1_txtGsmSourceName').value = checkNbsp(sGsmSouceName);
                document.getElementById('ContentPlaceHolder1_txtTgtID').value = checkNbsp(sTgtID);

                document.getElementById('ContentPlaceHolder1_txtDate').value = checkNbsp(sInstallDate);
                document.getElementById('ContentPlaceHolder1_txtRemark').value = checkNbsp(sRemark);

                document.getElementById('ContentPlaceHolder1_txtTvdID').value = checkNbsp(sTvdID);
                document.getElementById('ContentPlaceHolder1_txtWaranty').value = checkNbsp(sWarantyDate);

                $('#modal-device').modal('hide');

                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadData');
                objfr2.click();
            }
        }

        function postNewDeviceChild(sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sSourceName, sWarehouseName, sTdtID) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_txtNewDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNewNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtNewVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtNewDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtNewDeviceSourceName').value = checkNbsp(sSourceName);
                document.getElementById('ContentPlaceHolder1_txtNewWarehouseName').value = checkNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_txtNewTdtID').value = checkNbsp(sTdtID);

                $('#modal-devicenew').modal('hide');

                if (typeof refreshMaintChannelSetting === 'function') {
                    refreshMaintChannelSetting();
                }
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
        }

        // ===== Setting Channel MDVR (New Device Information) =====
        // Reuse mdvr_channel.ashx (load/save/require). Tidak mengubah flow submit maint.
        var CH_HANDLER = 'mdvr_channel.ashx';
        var maintChannelNoSN = '';
        var maintChannelMax = 0;

        function chEscapeHtml(s) {
            return (s == null ? '' : String(s))
                .replace(/&/g, '&amp;').replace(/</g, '&lt;')
                .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        }

        function hideMaintChannelSetting() {
            maintChannelNoSN = '';
            maintChannelMax = 0;
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

        function loadMaintChannelRows() {
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
                data: { action: 'load', nosn: maintChannelNoSN, maxchannel: maintChannelMax }
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

        function refreshMaintChannelSetting() {
            var nosnEl = document.getElementById('ContentPlaceHolder1_txtNewNoSN');
            var nosn = nosnEl ? (nosnEl.value || '').toString().trim() : '';
            if (!nosn) {
                hideMaintChannelSetting();
                return;
            }

            $.ajax({
                url: CH_HANDLER, type: 'GET', dataType: 'json', cache: false,
                data: { action: 'require', nosn: nosn }
            }).done(function (res) {
                if (!res || !res.success || !res.isRequire) {
                    hideMaintChannelSetting();
                    return;
                }
                maintChannelNoSN = nosn;
                maintChannelMax = parseInt(res.maxChannel, 10) || 0;
                if (maintChannelMax <= 0) {
                    hideMaintChannelSetting();
                    return;
                }
                document.getElementById('pnlChannelSetting').style.display = '';
                loadMaintChannelRows();
            }).fail(function () {
                hideMaintChannelSetting();
            });
        }

        function saveMaintChannelSetting() {
            if (!maintChannelNoSN || maintChannelMax <= 0) {
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
                    nosn: maintChannelNoSN,
                    maxchannel: maintChannelMax,
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
