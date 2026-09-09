<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pr_dlo_create_approve.aspx.cs" Inherits="vtsadm.pr_dlo_create_approve" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>BAST
            <small>Approve</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">BAST</a></li>
            <li class="active">Approve</li>
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
                            <label>BAST ID</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by BAST ID ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List BAST Ready To Approve</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small"
                                    CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False"
                                    Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481"
                                    GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="10"
                                    OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting">
                                    <Columns>
                                        <asp:BoundField DataField="DloID" HeaderText="BAST ID" SortExpression="DloID"></asp:BoundField>
                                        <asp:BoundField DataField="DloNumber" HeaderText="BAST Number" SortExpression="DloNumber"></asp:BoundField>
                                        <asp:BoundField DataField="DloDate" HeaderText="BAST Date" SortExpression="DloDate"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName"></asp:BoundField>
                                        <asp:BoundField DataField="TotalQty" HeaderText="Total Qty" SortExpression="TotalQty"></asp:BoundField>
                                        <asp:BoundField DataField="QtyDone" HeaderText="Qty Done" SortExpression="QtyDone"></asp:BoundField>
                                        <asp:BoundField DataField="QtyRemaining" HeaderText="Qty Remaining" SortExpression="QtyRemaining"></asp:BoundField>
                                        <asp:BoundField DataField="TotalLine" HeaderText="Total Line" SortExpression="TotalLine"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetail" runat="server" Text="<i class='fa fa-search'></i>" ToolTip="View Detail" CssClass="btn btn-default btn-xs"
                                                    CommandName="VIEWDETAIL" CommandArgument='<%# Eval("DloID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdApprove" runat="server" Text="<i class='fa fa-check'></i>" ToolTip="Approve" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
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

        <div class="modal modal-open fade" id="modal-messagebox">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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
        <div class="modal fade" id="modal-detail">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">DLO Detail</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row" style="margin-bottom: 8px;">
                            <div class="col-sm-4"><strong>BAST Number:</strong> <asp:Label ID="LblDtlDloNumber" runat="server" Text="-" /></div>
                            <div class="col-sm-4"><strong>BAST Date:</strong> <asp:Label ID="LblDtlDloDate" runat="server" Text="-" /></div>
                            <div class="col-sm-4"><strong>Vendor:</strong> <asp:Label ID="LblDtlVendorName" runat="server" Text="-" /></div>
                        </div>
                        <div class="row" style="margin-bottom: 8px;">
                            <div class="col-sm-4"><strong>Warehouse:</strong> <asp:Label ID="LblDtlWarehouseID" runat="server" Text="-" /></div>
                            <div class="col-sm-4"><strong>PO ID:</strong> <asp:Label ID="LblDtlPurID" runat="server" Text="-" /></div>
                            <div class="col-sm-4"><strong>Status:</strong> <asp:Label ID="LblDtlStatus" runat="server" Text="-" /></div>
                        </div>
                        <div class="row" style="margin-bottom: 12px;">
                            <div class="col-sm-12"><strong>Last Update:</strong> <asp:Label ID="LblDtlLastUpdate" runat="server" Text="-" /></div>
                        </div>
                        <div class="row" style="margin-bottom: 12px;">
                            <div class="col-sm-3"><strong>Total Qty:</strong> <asp:Label ID="LblDtlTotalQty" runat="server" Text="0" /></div>
                            <div class="col-sm-3"><strong>Qty Done:</strong> <asp:Label ID="LblDtlQtyDone" runat="server" Text="0" /></div>
                            <div class="col-sm-3"><strong>Qty Remaining:</strong> <asp:Label ID="LblDtlQtyRemaining" runat="server" Text="0" /></div>
                            <div class="col-sm-3"><strong>Total Line:</strong> <asp:Label ID="LblDtlTotalLine" runat="server" Text="0" /></div>
                        </div>
                        <asp:GridView ID="GridViewDetail" runat="server" CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="false" EmptyDataText="No detail items">
                            <Columns>
                                <asp:BoundField DataField="Seq" HeaderText="Seq" />
                                <asp:BoundField DataField="DeviceGroupID" HeaderText="Device Group" />
                                <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type" />
                                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                                <asp:BoundField DataField="QuantityDone" HeaderText="Qty Done" />
                                <asp:BoundField DataField="QtyRemaining" HeaderText="Qty Remaining" />
                                <asp:BoundField DataField="DeviceCount" HeaderText="Device Count" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-approve">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to Approve BAST ID :&nbsp;</h6>
                        <label id="LblDLOApprove" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtDLOApprove" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-approve').modal('hide');" onserverclick="CmdYesApprove_ServerClick" id="CmdYesApprove">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-approve').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function confirmApprove(sDloID) {
            if (sDloID != '') {
                document.getElementById('ContentPlaceHolder1_LblDLOApprove').innerHTML = sDloID;
                document.getElementById('ContentPlaceHolder1_txtDLOApprove').value = sDloID;
                $("#modal-approve").modal('show');
            }
        }

        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
