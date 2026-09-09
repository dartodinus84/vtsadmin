<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_mst_customer.aspx.cs" Inherits="vtsadm.view_mst_customer" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Customer           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Master</a></li>
            <li class="active">Customer</li>
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
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Customer</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="CustID" HeaderText="Cust ID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Cust Type" ItemStyle-Wrap="false" SortExpression="CustTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Wrap="false" SortExpression="Address"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>

                                        <asp:BoundField DataField="MarketingName" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDName" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDType" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDNumber" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="Long" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Lat" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName1" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition1" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PICName2" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="PICPosition2" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone1" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone1" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Email1" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BillingAddress" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="TaxAddress" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="ShipmentAddress" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustGroupID" HeaderText="Status" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="PeriodStart" HeaderText="Start" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PeriodEnd" HeaderText="End" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AttachName" HeaderText="Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AttachDesc" HeaderText="Desc" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AttachUrl" HeaderText="Url" ItemStyle-Wrap="false"></asp:BoundField>

                                        <asp:BoundField DataField="TotalUnit" HeaderText="Total Unit" ItemStyle-Wrap="false" SortExpression="TotalUnit"></asp:BoundField>

                                       <%-- <asp:BoundField DataField="person_id_jurnal" HeaderText="Jurnal ID" ItemStyle-Wrap="false" SortExpression="TotalUnit"></asp:BoundField>
                                        <asp:BoundField DataField="display_name" HeaderText="Jurnal Customer" ItemStyle-Wrap="false" SortExpression="TotalUnit"></asp:BoundField>--%>


                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdLog" runat="server" Text="<i class='fa fa-history'></i>" ToolTip="Log" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdVehicle" runat="server" Text="<i class='fa fa-truck'></i>" ToolTip="Vehicle" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDocument" runat="server" Text="<i class='fa fa-file-text-o'></i>" ToolTip="Document" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPasang" runat="server" Text="<i class='fa fa-truck'></i>" ToolTip="New Installment Vehicle" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdPindah" runat="server" Text="<i class='fa fa-truck'></i>" ToolTip="Maintenance Vehicle" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdLepas" runat="server" Text="<i class='fa fa-truck'></i>" ToolTip="Uninstall Vehicle" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdAccount" runat="server" Text="<i class='fa fa-user'></i>" ToolTip="Acccount Jurnal" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdInvoice" runat="server" Text="<i class='fa fa-file-text-o'></i>" ToolTip="Invoice" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdUser" runat="server" Text="<i class='fa fa-user'></i>" ToolTip="User Account" Enabled="true" CssClass="btn btn-success btn-xs" />
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
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
                        

                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-document">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Document</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedocument" src="view_customer_document.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportDocument" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDocument_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportDocumentXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportDocumentXls_Click" Text="Export XLS" />
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
                        <h4 class="modal-title">List Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframevehicle" src="view_customer_vehicle.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportVehicle" CssClass="btn btn-primary" runat="server" OnClick="CmdExportVehicle_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportVehicleXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportVehicleXls_Click" Text="Export XLS" />

                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-pasang">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframepasang" src="view_customer_pasang.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportPasang" CssClass="btn btn-primary" runat="server" OnClick="CmdExportPasang_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportPasangXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportPasangXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-pindah">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframepindah" src="view_customer_pindah.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportPindah" CssClass="btn btn-primary" runat="server" OnClick="CmdExportPindah_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportPindahXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportPindahXls_Click" Text="Export XLS" />
                   
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-lepas">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelepas" src="view_customer_uninstall.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportLepas" CssClass="btn btn-primary" runat="server" OnClick="CmdExportLepas_Click" Text="Export" />
                        <asp:Button ID="CmdExportLepasXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportLepasXls_Click" Text="Export" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-invoice">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Invoice</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframeinvoice" src="view_customer_invoice.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportInvoice" CssClass="btn btn-primary" runat="server" OnClick="CmdExportInvoice_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportInvoiceXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportInvoiceXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-account">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Account</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframeaccount" src="view_customer_account.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportAccount" CssClass="btn btn-primary" runat="server" OnClick="CmdExportAccount_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportAccountXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportAccountXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-user">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List User</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframeuser" src="view_customer_user.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportUser" CssClass="btn btn-primary" runat="server" OnClick="CmdExportUser_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportUserXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportUserXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-log">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Log Customer</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelog" src="view_customer_log.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">


                        <div class="row">
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
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Identity Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Customer Type</label>
                                            <p id="LblCustTypeDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>ID Type</label>
                                            <p id="LblIDType" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>ID Number</label>
                                            <p id="LblIDNumber" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="col-sm-3">

                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Branch Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Branch Name</label>
                                            <p id="LblBranchName" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Address Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Address</label>
                                            <textarea id="txtAddress" runat="server" class="form-control" style="background-color: white;" rows="2" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Billing Address</label>
                                            <textarea id="txtBillingAddr" runat="server" class="form-control" style="background-color: white;" rows="2" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Tax Address</label>
                                            <textarea id="txtTaxAddr" runat="server" class="form-control" style="background-color: white;" rows="2" disabled></textarea>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Shipment Address</label>
                                            <textarea id="txtShipmentAddr" runat="server" class="form-control" style="background-color: white;" rows="2" disabled></textarea>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">PIC Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>PIC Name 1</label>
                                            <p id="LblPICName1" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>PIC Position 1</label>
                                            <p id="LblPICPosition1" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>PIC Name 2</label>
                                            <p id="LblPICName2" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>PIC Position 2</label>
                                            <p id="LblPICPosition2" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Document Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Start Date</label>
                                            <p id="LblStartDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>End Date</label>
                                            <p id="LblEndDate" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Doc Name</label>
                                            <p id="LblDocName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Doc Desc</label>
                                            <p id="LblDocDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        
                                        
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Others Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Office Phone</label>
                                            <p id="LblOfficePhone" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Mobile Phone</label>
                                            <p id="LblMobilePhone" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Email</label>
                                            <p id="LblEmail" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Group ID</label>
                                            <p id="LblCustGroupID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Marketing Name</label>
                                            <p id="LblMarketingName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatus" runat="server" class="form-control-static"></p>
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
            </div>
        </div>
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

        function postLog(sCustID) {
            if (sCustID != '') {
                $('#modal-log').modal('show');
                var objfr = document.getElementById('iframelog').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objCustID.value = sCustID;
                cmdSearchLog.click();
            }
        }

        function postDocument(sCustID) {
            if (sCustID != '') {
                $('#modal-document').modal('show');
                var objfr = document.getElementById('iframedocument').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }

        function postUser(sCustID) {
            if (sCustID != '') {
                $('#modal-user').modal('show');
                var objfr = document.getElementById('iframeuser').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }

        function postVehicle(sCustID) {
            if (sCustID != '') {
                $('#modal-vehicle').modal('show');
                var objfr = document.getElementById('iframevehicle').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postPasang(sCustID) {
            if (sCustID != '') {
                $('#modal-pasang').modal('show');
                var objfr = document.getElementById('iframepasang').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postPindah(sCustID) {
            if (sCustID != '') {
                $('#modal-pindah').modal('show');
                var objfr = document.getElementById('iframepindah').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postUninstall(sCustID) {
            if (sCustID != '') {
                $('#modal-lepas').modal('show');
                var objfr = document.getElementById('iframelepas').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postInvoice(sCustID) {
            if (sCustID != '') {
                $('#modal-invoice').modal('show');
                var objfr = document.getElementById('iframeinvoice').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postAccount(sCustID) {
            if (sCustID != '') {
                $('#modal-account').modal('show');
                var objfr = document.getElementById('iframeaccount').contentWindow;
                var objCustID = objfr.document.getElementById('txtCustID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objCustID.value = sCustID;
                cmdSearch.click();
            }
        }
        function postDetails(sCustID, sFullName, sCustType, sBranchName, sAddress, sStatus, sMarketingName,
            sIDName, sIDType, sIDNumber, sBranchID, sLong, sLat, sPICName1, sPICPosition1, sPICName2, sPICPosition2,
            sOfficePhone, sMobilePhone, sEmail, sBillingAddr, sTaxAddr, sShipmentAddr, sMarketingID, sCustGroupID, sStartDate, sEndDate, sDocName, sDocDesc, sDocUrl) {
            if (sCustID != '') {

              
                document.getElementById('ContentPlaceHolder1_LblCustID').innerText = sCustID;
                document.getElementById('ContentPlaceHolder1_LblFullName').innerText = sFullName;
                document.getElementById('ContentPlaceHolder1_LblCustTypeDesc').innerText = sCustType;
                document.getElementById('ContentPlaceHolder1_LblIDType').innerText = sIDType;
                document.getElementById('ContentPlaceHolder1_LblIDNumber').innerText = CheckNbsp(sIDNumber);
                document.getElementById('ContentPlaceHolder1_LblBranchName').innerText = sBranchName;
                document.getElementById('ContentPlaceHolder1_txtAddress').textContent = sAddress;
                document.getElementById('ContentPlaceHolder1_txtBillingAddr').textContent = sBillingAddr;
                document.getElementById('ContentPlaceHolder1_txtTaxAddr').textContent = sTaxAddr;
                document.getElementById('ContentPlaceHolder1_txtShipmentAddr').textContent = sShipmentAddr;
                document.getElementById('ContentPlaceHolder1_LblCustGroupID').textContent = CheckNbsp(sCustGroupID);
                document.getElementById('ContentPlaceHolder1_LblMarketingName').innerText = sMarketingName;
                document.getElementById('ContentPlaceHolder1_LblPICName1').innerText = CheckNbsp(sPICName1);
                document.getElementById('ContentPlaceHolder1_LblPICPosition1').innerText = CheckNbsp(sPICPosition1);
                document.getElementById('ContentPlaceHolder1_LblPICName2').innerText = CheckNbsp(sPICName2);
                document.getElementById('ContentPlaceHolder1_LblPICPosition2').innerText = CheckNbsp(sPICPosition2);
                document.getElementById('ContentPlaceHolder1_LblOfficePhone').innerText = CheckNbsp(sOfficePhone);
                document.getElementById('ContentPlaceHolder1_LblMobilePhone').innerText = CheckNbsp(sMobilePhone);
                document.getElementById('ContentPlaceHolder1_LblEmail').innerText = CheckNbsp(sEmail);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);

                document.getElementById('ContentPlaceHolder1_LblStartDate').innerText = sStartDate;
                document.getElementById('ContentPlaceHolder1_LblEndDate').innerText = sEndDate;
                document.getElementById('ContentPlaceHolder1_LblDocName').innerText = sDocName;
                document.getElementById('ContentPlaceHolder1_LblDocDesc').innerText = sDocDesc;
                //document.getElementById('ContentPlaceHolder1_LblDocUrl').innerText = sDocUrl;

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
        endRequest();
    </script>
</asp:Content>

