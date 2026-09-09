<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_igo_payment.aspx.cs" Inherits="vtsadm.view_igo_payment" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Payment           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">iGO Track</a></li>
            <li><a href="#">View</a></li>
            <li class="active">Payment</li>
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
                            <label>Payment Type</label>
                            <asp:DropDownList ID="CmbPaymentType" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Payment</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="order_no" HeaderText="No Order" ItemStyle-Wrap="false" SortExpression="order_no"></asp:BoundField>
                                        <asp:BoundField DataField="invoice_id" HeaderText="Invoice ID" ItemStyle-Wrap="false" SortExpression="invoice_id"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_name" HeaderText="Sales Order Name" ItemStyle-Wrap="false" SortExpression="seller_cust_name"></asp:BoundField>
                                        <asp:BoundField DataField="customer_name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="customer_name"></asp:BoundField>
                                        <asp:BoundField DataField="msisdn" HeaderText="Serial Number" ItemStyle-Wrap="false" SortExpression="msisdn"></asp:BoundField>
                                        <asp:BoundField DataField="car_plate" HeaderText="Car Plate" ItemStyle-Wrap="false" SortExpression="car_plate"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_no" HeaderText="GSM" ItemStyle-Wrap="false" SortExpression="gsm_no"></asp:BoundField>
                                        <asp:BoundField DataField="dtmupd" HeaderText="Date" ItemStyle-Wrap="false" SortExpression="dtmupd"></asp:BoundField>
                                        <asp:BoundField DataField="amount" HeaderText="Amount" ItemStyle-Wrap="false" DataFormatString="{0:###,###,###.00}" SortExpression="amount"></asp:BoundField>
                                        <asp:BoundField DataField="price_detail" HeaderText="Detail" ItemStyle-Wrap="false" SortExpression="price_detail"></asp:BoundField>
                                        <asp:BoundField DataField="payment_no" HeaderText="Type Payment" ItemStyle-Wrap="false" SortExpression="payment_no"></asp:BoundField>
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
                       <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
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
        function confirmResend(s1) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_LblIdsID').innerHTML = s1;
                document.getElementById('ContentPlaceHolder1_txtLblIdsIDResend').value = s1;
                $("#modal-resend").modal('show');
            }
        }
        function confirmView(s1) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_LblIdsID2').innerHTML = s1;
                document.getElementById('ContentPlaceHolder1_txtLblIdsIDResend').value = s1;
                $("#modal-view").modal('show');
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
