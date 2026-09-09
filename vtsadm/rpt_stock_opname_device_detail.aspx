<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_stock_opname_device_detail.aspx.cs" Inherits="vtsadm.rpt_stock_opname_device_detail" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section class="content-header">
        <h1>Detail Stock Opname Device 
                <small><asp:Label ID="lblDetailCategory" runat="server"></asp:Label></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="rpt_stock_opname_device.aspx">Stock Opname Device</a></li>
            <li class="active">Detail</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i class="fa fa-list-alt"></i> 
                            Device Detail - <asp:Label ID="lblCategory" runat="server" Font-Bold="true"></asp:Label>
                        </h3>
                        <div class="box-tools pull-right">
                            <asp:Button ID="btnBack" runat="server" Text="← Kembali" CssClass="btn btn-default btn-sm" OnClick="btnBack_Click" />
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="info-box bg-aqua">
                                    <span class="info-box-icon"><i class="fa fa-building"></i></span>
                                    <div class="info-box-content">
                                        <span class="info-box-text">Vendor</span>
                                        <span class="info-box-number"><asp:Label ID="lblVendorName" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="info-box bg-green">
                                    <span class="info-box-icon"><i class="fa fa-microchip"></i></span>
                                    <div class="info-box-content">
                                        <span class="info-box-text">Device Type</span>
                                        <span class="info-box-number"><asp:Label ID="lblDeviceTypeName" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="table-responsive">
                            <asp:GridView ID="GridViewDetail" runat="server" BackColor="WhiteSmoke" Font-Size="Small" 
                                CssClass="table table-bordered table-hover table-striped" CellPadding="2" Width="100%" 
                                AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" 
                                EmptyDataText="No devices found for this category" ForeColor="#003481" 
                                GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="20" 
                                OnPageIndexChanging="GridViewDetail_PageIndexChanging">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <Columns>
                                    <asp:BoundField DataField="deviceid" HeaderText="Device ID" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                        <ItemStyle CssClass="text-monospace" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nosn" HeaderText="Serial Number" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                        <ItemStyle CssClass="text-monospace font-weight-bold" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vendor" HeaderText="Vendor" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="gps_type" HeaderText="GPS Type" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="status_device" HeaderText="Status" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="warehouse_name" HeaderText="Warehouse" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="technician_name" HeaderText="Technician" ItemStyle-Wrap="false">
                                        <HeaderStyle CssClass="bg-primary text-white" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle ForeColor="#003481" BackColor="White" />
                                <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                <PagerSettings PageButtonCount="5" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                <HeaderStyle Height="25px" Wrap="True" BackColor="#3c8dbc" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                            </asp:GridView>
                            <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                <asp:Label ID="LblPagingDetail" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdExportDetails" CssClass="btn btn-success" runat="server" OnClick="CmdExportDetails_Click" Text="📄 Export CSV" />                        
                        <asp:Button ID="CmdExportDetailsXls" CssClass="btn btn-success" runat="server" OnClick="CmdExportDetailsXls_Click" Text="📊 Export XLS" />
                        <asp:Label ID="lblStatus" runat="server" CssClass="text-muted pull-right"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Hidden div for messages -->
        <div id="div_comment" runat="server" style="display:none;"></div>
    </section>

    <style type="text/css">
        .info-box {
            margin-bottom: 15px;
        }
        
        .text-monospace {
            font-family: 'Courier New', monospace;
        }
        
        .font-weight-bold {
            font-weight: bold !important;
        }
        
        .bg-primary {
            background-color: #3c8dbc !important;
        }
        
        .text-white {
            color: white !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            // Prevent empty modal from showing
            var divComment = document.getElementById('ContentPlaceHolder1_div_comment');
            if (divComment) {
                divComment.style.display = 'none';
            }
        });
    </script>
</asp:Content>