<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_sales_order.aspx.cs" Inherits="vtsadm.mst_igo_sales_order" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Sales Order           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">IGO TRACKER</a></li>
            <li class="active">Master Sales Order</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Sales Order Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm" style="display: none">
                            <label>Order ID Order</label>
                            <asp:TextBox ID="txtOrderID" runat="server" class="form-control" placeholder="Order ID ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type Sales Order</label>
                            <asp:DropDownList ID="CmbTypeSalesID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbTypeSalesID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Seller (Marketplace)</label>
                            <asp:DropDownList ID="CmbMarketID" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Mitra</label>
                            <asp:DropDownList ID="CmbSourceMarekting" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbSourceMarekting_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Mitra Name</label>
                            <asp:DropDownList ID="CmbMarketing" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vendor Delivery</label>
                            <asp:DropDownList ID="CmbDeliveryID" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>

                        <div class="form-group form-group-sm">
                            <label>Vendor Recipt</label>
                            <asp:TextBox ID="txtResi" runat="server" class="form-control" placeholder="Expidition Recipt ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No Invoice</label>
                            <asp:TextBox ID="txtNoInv" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Quantity</label>
                            <asp:TextBox ID="txtCount" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label for="txtQty12">Relay 12 V</label>
                                <asp:TextBox ID="txtQty12" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQty24">Relay 24 V</label>
                                <asp:TextBox ID="txtQty24" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label id="admDivCheck" runat="server" style="display: none">Amount / Pcs</label>
                            <asp:DropDownList ID="CmdPriceItem" Style="display: none" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
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
                            <asp:TextBox ID="txtCustName" runat="server" class="form-control" placeholder="Customer Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Address</label>
                            <asp:TextBox ID="txtCustAdd" runat="server" class="form-control" placeholder="Customer Address ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Email</label>
                            <asp:TextBox ID="txtCustEmail" runat="server" class="form-control" placeholder="Customer Email ..." required="required" TextMode="Email"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Phone</label>
                            <asp:TextBox ID="txtCustPhone" runat="server" class="form-control" placeholder="Customer Phone ..." required="required"></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdUpload" type="button" class="btn btn-primary" onclick="$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Invoice</button>
                            <button id="CmdUploadResi" type="button" class="btn btn-primary" onclick="$('#modal-picture-resi').modal('show');"><i class="fa fa-upload"></i>&nbsp;Recipt</button>
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
                        <h3 class="box-title">List Sales Order</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by customer name ..."></asp:TextBox>
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
                                        <asp:BoundField DataField="order_id" HeaderText="Order ID" ItemStyle-Wrap="false" SortExpression="order_id"></asp:BoundField>
                                        <asp:BoundField DataField="order_type_desc" HeaderText="Order Type" ItemStyle-Wrap="false" SortExpression="order_type_desc"></asp:BoundField>
                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" ItemStyle-Wrap="false" SortExpression="quantity"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>
                                        <asp:BoundField DataField="seller_invoice" HeaderText="Invoice" ItemStyle-Wrap="false" SortExpression="seller_invoice"></asp:BoundField>
                                        <asp:BoundField DataField="price" HeaderText="Price" ItemStyle-Wrap="false" SortExpression="price"></asp:BoundField>

                                        <asp:BoundField DataField="seller_cust_name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="seller_cust_name"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_address" HeaderText="Customer Address" ItemStyle-Wrap="false" SortExpression="seller_cust_address"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_email" HeaderText="Customer Email" ItemStyle-Wrap="false" SortExpression="seller_cust_email"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_phone" HeaderText="Customer Phone" ItemStyle-Wrap="false" SortExpression="seller_cust_phone"></asp:BoundField>
                                        <asp:BoundField DataField="sGenDate" HeaderText="Generate Date" ItemStyle-Wrap="false" SortExpression="sGenDate"></asp:BoundField>

                                        <asp:BoundField DataField="SourceMarketing" HeaderText="Marketing Source" ItemStyle-Wrap="false" SortExpression="SourceMarketing"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing Name" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                        <asp:BoundField DataField="seller_id" HeaderText="" ItemStyle-Wrap="false" SortExpression="seller_id"></asp:BoundField>
                                        <asp:BoundField DataField="order_type" HeaderText="" ItemStyle-Wrap="false" SortExpression="order_type"></asp:BoundField>
                                        <asp:BoundField DataField="mitra_type" HeaderText="" ItemStyle-Wrap="false" SortExpression="mitra_type"></asp:BoundField>

                                        <asp:BoundField DataField="mitra_id" HeaderText="" ItemStyle-Wrap="false" SortExpression="mitra_id"></asp:BoundField>
                                        <asp:BoundField DataField="expedition_id" HeaderText="" ItemStyle-Wrap="false" SortExpression="relay_12v"></asp:BoundField>
                                        <asp:BoundField DataField="resi" HeaderText="" ItemStyle-Wrap="false" SortExpression="relay_24v"></asp:BoundField>
                                        <asp:BoundField DataField="relay_12v" HeaderText="" ItemStyle-Wrap="false" SortExpression="relay_12v"></asp:BoundField>
                                        <asp:BoundField DataField="relay_24v" HeaderText="" ItemStyle-Wrap="false" SortExpression="relay_24v"></asp:BoundField>
                                        <asp:BoundField DataField="amount_item" HeaderText="" ItemStyle-Wrap="false" SortExpression="amount_item"></asp:BoundField>

                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
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
               <%--     <div class="modal-footer">
                        <img src="Content/ajax-loader2.gif" style="display: none;" id="iload" />
                        <button type="button" class="btn btn-default btn-confirmasi" runat="server" onclick="$('.btn-confirmasi').attr('disabled','disabled');$('button.close').hide();$('#iload').show();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary btn-confirmasi" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>--%>
                               <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesSubmit();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
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
                            <iframe src="mst_igo_sales_order_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-picture-resi">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="mst_igo_sales_order_upload_resi.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
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
        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Device ID :&nbsp;</h6>
                        <label id="LblSalesOrderID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtSalesOrderIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function buttonYesSubmit() {
            $("#modal-submit").modal('hide');
        }

        function confirmSubmit() {
            var objJobID = document.getElementById('ContentPlaceHolder1_txtJobID');
            if (objJobID.value != '') {
                document.getElementById('ContentPlaceHolder1_LblJobIDSubmit').innerHTML = objJobID.value;
                $("#modal-submit").modal('show');
            }
        }

        function postStockChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtStockID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtTypeDevice').value = sBranchName;
                document.getElementById('ContentPlaceHolder1_txtSN').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtGSM').value = sCustTypeDesc;
                $('#modal-stock-igo').modal('hide');
            }
        }
        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblSalesOrderID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtSalesOrderIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
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
