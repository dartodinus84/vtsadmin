<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_po_summary.aspx.cs" Inherits="vtsadm.rpt_po_summary" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Sales Order - Summary Performance
                <small>Report</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Report</a></li>
            <li><a href="#">Sales Order</a></li>
            <li class="active">Summary Performance</li>
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
                            <label>Branch</label>
                            <asp:DropDownList ID="CmbBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Sales Order - Summary Performance</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                       <asp:BoundField DataField="MarketingName" HeaderText="Marketing Name" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                       <asp:BoundField DataField="bulanan" HeaderText="Marketing Target / Bulan" ItemStyle-Wrap="false" SortExpression="bulanan"></asp:BoundField>
                                       <asp:BoundField DataField="January" HeaderText="January" ItemStyle-Wrap="false" SortExpression="January"></asp:BoundField>
                                       <asp:BoundField DataField="February" HeaderText="February" ItemStyle-Wrap="false" SortExpression="February"></asp:BoundField>
                                       <asp:BoundField DataField="Maret" HeaderText="Maret" ItemStyle-Wrap="false" SortExpression="Maret"></asp:BoundField>
                                       <asp:BoundField DataField="April" HeaderText="April" ItemStyle-Wrap="false" SortExpression="April"></asp:BoundField>
                                       <asp:BoundField DataField="Mei" HeaderText="Mei" ItemStyle-Wrap="false" SortExpression="Mei"></asp:BoundField>
                                       <asp:BoundField DataField="Juni" HeaderText="Juni" ItemStyle-Wrap="false" SortExpression="Juni"></asp:BoundField>
                                       <asp:BoundField DataField="Juli" HeaderText="Juli" ItemStyle-Wrap="false" SortExpression="Juli"></asp:BoundField>
                                       <asp:BoundField DataField="Agustus" HeaderText="Agustus" ItemStyle-Wrap="false" SortExpression="Agustus"></asp:BoundField>
                                       <asp:BoundField DataField="September" HeaderText="September" ItemStyle-Wrap="false" SortExpression="September"></asp:BoundField>
                                       <asp:BoundField DataField="Oktober" HeaderText="Oktober" ItemStyle-Wrap="false" SortExpression="Oktober"></asp:BoundField>
                                       <asp:BoundField DataField="November" HeaderText="November" ItemStyle-Wrap="false" SortExpression="November"></asp:BoundField>
                                       <asp:BoundField DataField="Desember" HeaderText="Desember" ItemStyle-Wrap="false" SortExpression="Desember"></asp:BoundField>
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
