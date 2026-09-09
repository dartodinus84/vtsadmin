<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_po_sum.aspx.cs" Inherits="vtsadm.rpt_po_sum" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Purchase Order - Summary 
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Purchase Order</a></li>
            <li class="active">Summary</li>
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
                        <h3 class="box-title">List Job Order - Maintenance</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="FullName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="IDcus" HeaderText="Customer Code" ItemStyle-Wrap="false" SortExpression="IDcus"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch / Project" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="PICName1" HeaderText="Contac Person (PIC)" ItemStyle-Wrap="false" SortExpression="PICName1"></asp:BoundField>
                                        <asp:BoundField DataField="PICPosition1" HeaderText="Position (PIC)" ItemStyle-Wrap="false" SortExpression="PICPosition1"></asp:BoundField>
                                        <asp:BoundField DataField="OfficePhone1" HeaderText="Office Phone" ItemStyle-Wrap="false" SortExpression="OfficePhone1"></asp:BoundField>
                                        <asp:BoundField DataField="MobilePhone1" HeaderText="Mobile Phone" ItemStyle-Wrap="false" SortExpression="MobilePhone1"></asp:BoundField>
                                        <asp:BoundField DataField="Email1" HeaderText="Email" ItemStyle-Wrap="false" SortExpression="Email1"></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address Information" ItemStyle-Wrap="false" SortExpression="Address"></asp:BoundField>
                                       
                                        <%-- <asp:BoundField DataField="BillingAddress" HeaderText="Address Billing" ItemStyle-Wrap="false" SortExpression="BillingAddress"></asp:BoundField>
                                        <asp:BoundField DataField="ShipmentAddress" HeaderText="Address Shipment" ItemStyle-Wrap="false" SortExpression="ShipmentAddress"></asp:BoundField>
                                        <asp:BoundField DataField="TaxAddress" HeaderText="Address Tax" ItemStyle-Wrap="false" SortExpression="TaxAddress"></asp:BoundField>
                                       --%>
                                        
                                        <asp:BoundField DataField="IDType" HeaderText="ID Card" ItemStyle-Wrap="false" SortExpression="IDType"></asp:BoundField>
                                        <asp:BoundField DataField="IDNumber" HeaderText="ID Number" ItemStyle-Wrap="false" SortExpression="IDNumber"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity PO" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="New / Repeat" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                        <asp:BoundField DataField="PoTypeDesc" HeaderText="Type PO" ItemStyle-Wrap="false" SortExpression="PoTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ContractTime" HeaderText="Contract" ItemStyle-Wrap="false" SortExpression="ContractTime"></asp:BoundField>
                                        <asp:BoundField DataField="PoDate" HeaderText="PO Date" ItemStyle-Wrap="false" SortExpression="PoDate"></asp:BoundField>
                                        <asp:BoundField DataField="InstallFee" HeaderText="Install Fee" ItemStyle-Wrap="false" SortExpression="InstallFee"></asp:BoundField>
                                        <asp:BoundField DataField="MonthlyFee" HeaderText="Monthly Fee" ItemStyle-Wrap="false" SortExpression="MonthlyFee"></asp:BoundField>
                                        <asp:BoundField DataField="ContractTime" HeaderText="Contract" ItemStyle-Wrap="false" SortExpression="ContractTime"></asp:BoundField>
                                        <asp:BoundField DataField="InstallFeePPN" HeaderText="Install Fee PPN" ItemStyle-Wrap="false" SortExpression="InstallFeePPN"></asp:BoundField>
                                        <asp:BoundField DataField="MonthlyFeePPN" HeaderText="Install Monthly PPN" ItemStyle-Wrap="false" SortExpression="MonthlyFeePPN"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Customer Type" ItemStyle-Wrap="false" SortExpression="CustTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ServerName" HeaderText="Server Name" ItemStyle-Wrap="false" SortExpression="ServerName"></asp:BoundField>
                                        
                                        
                                        <asp:BoundField DataField="JobReport" HeaderText="Job Report" ItemStyle-Wrap="false" SortExpression="JobReport"></asp:BoundField>
                                        <asp:BoundField DataField="MonthlyReport" HeaderText="Monthly Report" ItemStyle-Wrap="false" SortExpression="MonthlyReport"></asp:BoundField>
                                        <asp:BoundField DataField="LampiranPKS" HeaderText="Lampiran PKS" ItemStyle-Wrap="false" SortExpression="LampiranPKS"></asp:BoundField>
                                        <asp:BoundField DataField="PurchaseOrder" HeaderText="Purchase Order" ItemStyle-Wrap="false" SortExpression="PurchaseOrder"></asp:BoundField>
                                        <asp:BoundField DataField="NPWP" HeaderText="NPWP" ItemStyle-Wrap="false" SortExpression="NPWP"></asp:BoundField>
                                        <asp:BoundField DataField="ENova" HeaderText="E-Nova Pajak" ItemStyle-Wrap="false" SortExpression="ENova"></asp:BoundField>
                                        <asp:BoundField DataField="BASTPengirimin" HeaderText="Bast Pengiriman" ItemStyle-Wrap="false" SortExpression="BASTPengirimin"></asp:BoundField>
                                        <asp:BoundField DataField="BASTPemakaian" HeaderText="Bast Pemakaian" ItemStyle-Wrap="false" SortExpression="BASTPemakaian"></asp:BoundField>
                                        <asp:BoundField DataField="SuratPerintahBayar" HeaderText="Surat Perintah Bayar" ItemStyle-Wrap="false" SortExpression="SuratPerintahBayar"></asp:BoundField>
                                        <asp:BoundField DataField="SSEPajak" HeaderText="Surat Perintah Bayar" ItemStyle-Wrap="false" SortExpression="SSEPajak"></asp:BoundField>
                                        <asp:BoundField DataField="TandaTerimaInvoice" HeaderText="Surat Perintah Bayar" ItemStyle-Wrap="false" SortExpression="TandaTerimaInvoice"></asp:BoundField>
                                        <asp:BoundField DataField="Kwitansi" HeaderText="Surat Perintah Bayar" ItemStyle-Wrap="false" SortExpression="Kwitansi"></asp:BoundField>
                                        <asp:BoundField DataField="SpecimentPajak" HeaderText="Surat Perintah Bayar" ItemStyle-Wrap="false" SortExpression="SpecimentPajak"></asp:BoundField>
                                        <asp:BoundField DataField="BillingGSM" HeaderText="Billing GSM" ItemStyle-Wrap="false" SortExpression="BillingGSM"></asp:BoundField>
                                        <asp:BoundField DataField="PackingList" HeaderText="Packing List" ItemStyle-Wrap="false" SortExpression="PackingList"></asp:BoundField>
                                        <asp:BoundField DataField="DokumenTambahan" HeaderText="Dokumen Tambahan" ItemStyle-Wrap="false" SortExpression="DokumenTambahan"></asp:BoundField>
                                       
                                        <asp:BoundField DataField="MarektingName" HeaderText="Marekting Name" ItemStyle-Wrap="false" SortExpression="MarektingName"></asp:BoundField>
                                     
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
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export" />
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
