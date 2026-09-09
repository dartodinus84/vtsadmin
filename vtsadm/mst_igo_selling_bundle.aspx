<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_selling_bundle.aspx.cs" Inherits="vtsadm.mst_igo_selling_bundle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">IGO TRACK</a></li>
            <li class="active">Master Selling Bundle</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Order Information</h3>
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <label>Order ID</label>
                                <div class="input-group input-group-sm">
                                    <input type="text" id="txtOrderID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                    <span class="input-group-btn">
                                        <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-order"><i class="fa fa-search"></i></button>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Sales Order Desc</label>
                                <input type="text" id="txtSalesOrderDesc" runat="server" class="form-control" placeholder="Sales Order Desc ..." readonly="readonly" />
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Seller (Marketplace)</label>
                                <input type="text" id="txtSellerDesc" runat="server" class="form-control" placeholder="Seller Name ..." readonly="readonly" />
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Marketing Source </label>
                                <input type="text" id="txtMarketingSourceDesc" runat="server" class="form-control" placeholder="Marketing Source ..." readonly="readonly" />
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Marketing Name</label>
                                <input type="text" id="txtMarketingNameDesc" runat="server" class="form-control" placeholder="Marketing Name ..." readonly="readonly" />
                            </div>
                            <div class="form-group form-group-sm">
                                <label>No Invoice</label>
                                <input type="text" id="txtInvoice" runat="server" class="form-control" placeholder="Marketing Name ..." readonly="readonly" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <input type="text" id="txtCustName" runat="server" class="form-control" placeholder="Customer Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Address</label>
                            <input type="text" id="txtCustAdd" runat="server" class="form-control" placeholder="Customer Address ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Email </label>
                            <input type="text" id="txtCustEmail" runat="server" class="form-control" placeholder="Customer Email ..." readonly="readonly" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Stock Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Stock ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtStockID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button5" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-stock-igo"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type GPS</label>
                            <input type="text" id="txtTypeDevice" runat="server" class="form-control" placeholder="Type GPS ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Serial Number GPS</label>
                            <input type="text" id="txtSN" runat="server" class="form-control" placeholder="GPS Number ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>GSM Number</label>
                            <input type="text" id="txtGSM" runat="server" class="form-control" placeholder="GSM Number ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm" style="display:none">
                            <label>Voucher / Month</label>
                            <asp:DropDownList ID="CmbPriceID" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Device</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by no sn ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="voucher_id" HeaderText="Voucher ID" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="name" HeaderText="Market" ItemStyle-Wrap="false" SortExpression="name"></asp:BoundField>
                                        <asp:BoundField DataField="seller_invoice" HeaderText="Invoice" ItemStyle-Wrap="false" SortExpression="seller_invoice"></asp:BoundField>
                                        <asp:BoundField DataField="NameVoucher" HeaderText="Voucher Name" ItemStyle-Wrap="false" SortExpression="NameVoucher"></asp:BoundField>
                                        <asp:BoundField DataField="price" HeaderText="Price" ItemStyle-Wrap="false" SortExpression="price"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="seller_cust_name"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_email" HeaderText="Customer Email" ItemStyle-Wrap="false" SortExpression="seller_cust_email"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_address" HeaderText="Customer Address" ItemStyle-Wrap="false" SortExpression="seller_cust_address"></asp:BoundField>
                                        <asp:BoundField DataField="sGenDate" HeaderText="Generate Date" ItemStyle-Wrap="false" SortExpression="sGenDate"></asp:BoundField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
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
                </div>
            </div>
        </div>
        <div class="modal fade" id="modal-submit">
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
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-submit').modal('hide');" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-stock-igo">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Stock IGO TRACK</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="mst_igo_selling_bundle_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-order">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Sales Order IGO TRACK</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="mst_igo_sales_order_bundle_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
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
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        //function postStockChild(sAutoID, sGenDate, sGpsSN, sGsmNo) {
        //    if (sAutoID != '') {

        //        document.getElementById('ContentPlaceHolder1_txtStockID').value = sAutoID;
        //        document.getElementById('ContentPlaceHolder1_txtGenDate').value = sGenDate;
        //        document.getElementById('ContentPlaceHolder1_txtSN').value = sGpsSN);
        //        document.getElementById('ContentPlaceHolder1_txtGSM').value =sGsmNo;
        //        $('#modal-stock-igo').modal('hide');

        //    }
        //}
        function postStockChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtStockID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtTypeDevice').value = sBranchName;
                document.getElementById('ContentPlaceHolder1_txtSN').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtGSM').value = sCustTypeDesc;
                $('#modal-stock-igo').modal('hide');
            }
        }
        
        function postOrderChild(s1, s2, s3, s4, s5, s6, s7, s8, s9) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_txtOrderID').value = s1;
                document.getElementById('ContentPlaceHolder1_txtSalesOrderDesc').value = s2;
                document.getElementById('ContentPlaceHolder1_txtInvoice').value = s3;
                document.getElementById('ContentPlaceHolder1_txtSellerDesc').value = s4;
                document.getElementById('ContentPlaceHolder1_txtCustName').value = s5;
                document.getElementById('ContentPlaceHolder1_txtCustEmail').value = s6;
                document.getElementById('ContentPlaceHolder1_txtCustAdd').value = s7;
                document.getElementById('ContentPlaceHolder1_txtMarketingSourceDesc').value = s8;
                document.getElementById('ContentPlaceHolder1_txtMarketingNameDesc').value = s9;
                $('#modal-order').modal('hide');
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
