<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_maint.aspx.cs" Inherits="vtsadm.vehicle_maint" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-control {
            background-color: white !important;
        }

    </style>
    <section class="content-header">
        <h1>Vehicle Maintenance
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Installation</a></li>
            <li><a href="#">Maintenance</a></li>
            <li class="active">Vehicle</li>
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
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-job_order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Register Date</label>
                            <input type="text" id="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtRegDate" runat="server" class="form-control" placeholder="Register Date ..." ReadOnly="true"></asp:TextBox>--%>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vehicle Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtVehicleID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button3" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-vehicle"><i class="fa fa-search"></i></button>
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
                            <%--<asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Asset No</label>
                            <input type="text" id="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <input type="text" id="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <input type="text" id="txtCustomerID" runat="server" class="form-control" placeholder="Customer ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustomerID" runat="server" class="form-control" placeholder="Customer ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdLoadData" CssClass="btn btn-primary" runat="server" OnClick="CmdLoadData_ServerClick" Text="Load" />
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
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingUpline" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
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
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingMaster" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Device Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <input type="text" id="txtDeviceID" runat="server" class="form-control" placeholder="Device ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDeviceID" runat="server" class="form-control" placeholder="Device ID ..." ReadOnly="true"></asp:TextBox>--%>
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
                            <%--<asp:TextBox ID="txtTechnicianID" runat="server" class="form-control" placeholder="Technician ID ..." ReadOnly="true"></asp:TextBox>--%>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Gsm ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtGsmID" runat="server" class="form-control" placeholder="Gsm ID ..." ReadOnly="true"></asp:TextBox>--%>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Installation Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Date</label>
                            <input type="text" id="txtDate" runat="server" class="form-control" placeholder="Date ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label> 
                            <input type="text" id="txtRemark" runat="server" class="form-control" placeholder="Remark ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Transaction Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Tvd ID</label>
                            <input type="text" id="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtTvdID" runat="server" class="form-control" placeholder="Tvd ID ..." ReadOnly="true"></asp:TextBox>--%>
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
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingS" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Vehicle Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtNewVehicleID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-vehiclenew"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Description</label>
                            <input type="text" id="txtNewVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Police No</label>
                            <input type="text" id="txtNewPoliceNo" runat="server" class="form-control" placeholder="Police No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewPoliceNo" runat="server" class="form-control" placeholder="Police No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Asset No</label>
                            <input type="text" id="txtNewAssetNo" runat="server" class="form-control" placeholder="Asset No ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewAssetNo" runat="server" class="form-control" placeholder="Asset No ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <input type="text" id="txtNewTvaID" runat="server" class="form-control" placeholder="Tva ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewTvaID" runat="server" class="form-control" placeholder="Tva ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Status Old Vehicle</label>
                            <asp:DropDownList ID="CmbStatusOldVehicle" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>New Remark</label>
                            <asp:TextBox ID="txtNewRemark" runat="server" class="form-control" placeholder="New Remark ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
						    <label>Telegram CC</label>
                            <asp:TextBox ID="txtTelegram" runat="server" class="form-control" placeholder="Telegram ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Picture Attachment</label>
                            <p>
                                <button id="CmdUpload" type="button" class="btn btn-primary" onclick="$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image</button>
                            </p>    
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdTelegram" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-telegram').modal('show');return false;" Text="Telegram" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <input type="text" id="txtNewCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <input type="text" id="txtNewCustomerID" runat="server" class="form-control" placeholder="Customer ID ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewCustomerID" runat="server" class="form-control" placeholder="Customer ID ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtNewCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                            <%--<asp:TextBox ID="txtNewCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." ReadOnly="true"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdNewLoadData" CssClass="btn btn-primary" runat="server" OnClick="CmdNewLoadData_ServerClick" Text="Load" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List New Upline</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging">
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
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingNewUpline" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List New Master</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView5" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView5_PageIndexChanging">
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
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPagingNewMaster" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
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
                            <iframe src="vehicle_maint_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
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
                            <iframe src="vehicle_maint_job_order_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe id="iframevehicle" src="vehicle_maint_vehicle_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-vehiclenew">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">New Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframenewvehicle" src="vehicle_maint_new_vehicle_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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

                var objfr = document.getElementById('iframevehicle').contentWindow;
                var objJobID = objfr.document.getElementById('txtJobID');
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearchVehicle');
                objJobID.value = sJobID;
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }

        function postVehicleChild(sVehicleID, sVehicleDesc, sPoliceNo,
            sAssetNo, sNoSN, sMSIDN, objSelect, sTvaID, sBranchName_Cust, sCustID, sCustName, sDeviceID, sVendorName, sDeviceTypeDesc, sDeviceSourceName,
            sWarehouseName, sTdtID, sTechnicianID, sEmployeeNo, sEmployeeName, sBranchName_Technician,
            sGsmID, sProviderName, sGsmSourceName, sTgtID, sInstallDate, sRemark, sTvdID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = checkNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = checkNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = checkNbsp(sTvaID);

                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sBranchName_Cust);
                document.getElementById('ContentPlaceHolder1_txtCustomerID').value = checkNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sCustName);

                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtDeviceSourceName').value = checkNbsp(sDeviceSourceName);

                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = checkNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_txtTdtID').value = checkNbsp(sTdtID);

                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = checkNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = checkNbsp(sEmployeeNo);
                document.getElementById('ContentPlaceHolder1_txtName').value = checkNbsp(sEmployeeName);
                document.getElementById('ContentPlaceHolder1_txtTechBranchName').value = checkNbsp(sBranchName_Technician);

                document.getElementById('ContentPlaceHolder1_txtGsmID').value = checkNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = checkNbsp(sMSIDN);
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = checkNbsp(sProviderName);
                document.getElementById('ContentPlaceHolder1_txtGsmSourceName').value = checkNbsp(sGsmSourceName);

                document.getElementById('ContentPlaceHolder1_txtTgtID').value = checkNbsp(sTgtID);

                document.getElementById('ContentPlaceHolder1_txtDate').value = checkNbsp(sInstallDate);
                document.getElementById('ContentPlaceHolder1_txtRemark').value = checkNbsp(sRemark);

                document.getElementById('ContentPlaceHolder1_txtTvdID').value = checkNbsp(sTvdID);

                $('#modal-vehicle').modal('hide');
                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadData');
                objfr2.click();
            }
        }

        function postNewVehicleChild(sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sBranchName_Cust, sCustName, objSelect, sTvaID, sCustID) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_txtNewVehicleID').value = checkNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_txtNewVehicleDesc').value = checkNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_txtNewPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtNewAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtNewTvaID').value = checkNbsp(sTvaID);

                document.getElementById('ContentPlaceHolder1_txtNewCustBranchName').value = checkNbsp(sBranchName_Cust);
                document.getElementById('ContentPlaceHolder1_txtNewCustomerID').value = checkNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_txtNewCustomerName').value = checkNbsp(sCustName);

                $('#modal-vehiclenew').modal('hide');
                var objfr2 = document.getElementById('ContentPlaceHolder1_CmdNewLoadData');
                objfr2.click();
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
        endRequest();
    </script>
</asp:Content>
