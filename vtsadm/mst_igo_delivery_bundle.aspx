<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_delivery_bundle.aspx.cs" Inherits="vtsadm.mst_igo_delivery_bundle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Delivery Stock           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">IGO TRACK</a></li>
            <li class="active">Master Delivery Stock</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Order Information</h3>
                    </div>
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
                        <div class="form-group form-group-sm">
                            <label>Quantity</label>
                            <input type="text" id="txtCount" runat="server" class="form-control" placeholder="Marketing Name ..." readonly="readonly" />
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
                        <div class="form-group form-group-sm">
                            <label>Customer Phone </label>
                            <input type="text" id="txtCustPhone" runat="server" class="form-control" placeholder="Customer Email ..." readonly="readonly" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">List Detail Sales Order</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <asp:Panel runat="server" ScrollBars="Auto">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField DataField="order_id" HeaderText="Order ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                            <asp:BoundField DataField="ids" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                            <asp:BoundField DataField="stock_id" HeaderText="Stock ID" ItemStyle-Wrap="false" SortExpression="TvdID"></asp:BoundField>
                                            <asp:BoundField DataField="gps_sn" HeaderText="GPS No" ItemStyle-Wrap="false" SortExpression="PoliceNo"></asp:BoundField>
                                            <asp:BoundField DataField="gsm_no" HeaderText="GSM No" ItemStyle-Wrap="false" SortExpression="MaintTypeDesc"></asp:BoundField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle ForeColor="#003481" BackColor="White" />
                                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <HeaderStyle Height="20px" Wrap="True" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                    </asp:GridView>
                                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                        <asp:Label ID="LblPagingDetail" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vendor Delivery Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vendor Devilery</label>
                            <asp:DropDownList ID="CmbDeliveryID" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No Receipt</label>
                            <asp:TextBox ID="txtResi" runat="server" class="form-control" placeholder="No Receipt ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm" style="display:none">
                            <label>No Receipt</label>
                            <asp:TextBox ID="txtInvImg" runat="server" class="form-control" placeholder="No Receipt ..." required="required"></asp:TextBox>
                            <asp:TextBox ID="txtRecipt" runat="server" class="form-control" placeholder="No Receipt ..." required="required"></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdDownload" type="button" runat="server" class="btn btn-primary" onserverclick="CmdDownloadInv_ServerClick"><i class="fa fa-download"></i>&nbsp;Invoice</button>
                            <button id="CmdDownloadRecipt" type="button" runat="server" class="btn btn-primary" onserverclick="CmdDownloadRecipt_ServerClick"><i class="fa fa-download"></i>&nbsp;Recipt</button>
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                            <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                            <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility: hidden;" runat="server" onserverclick="CmdLoad_Click">1</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Delivery Bundle</h3>
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
                                        <asp:BoundField DataField="ids" HeaderText="Detail Order" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="order_id" HeaderText="Order ID" ItemStyle-Wrap="false" SortExpression="order_id"></asp:BoundField>
                                        <asp:BoundField DataField="NameExpedition" HeaderText="Expedition Name" ItemStyle-Wrap="false" SortExpression="NameExpedition"></asp:BoundField>
                                        <asp:BoundField DataField="resi" HeaderText="Recipt" ItemStyle-Wrap="false" SortExpression="resi"></asp:BoundField>
                                        <asp:BoundField DataField="gps_sn" HeaderText="GPS" ItemStyle-Wrap="false" SortExpression="gps_sn"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_no" HeaderText="GSM " ItemStyle-Wrap="false" SortExpression="gsm_no"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_name" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="seller_cust_name"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_email" HeaderText="Customer Email" ItemStyle-Wrap="false" SortExpression="seller_cust_email"></asp:BoundField>
                                        <asp:BoundField DataField="seller_cust_address" HeaderText="Customer Address" ItemStyle-Wrap="false" SortExpression="seller_cust_address"></asp:BoundField>
                                        <asp:BoundField DataField="par_value" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="par_value"></asp:BoundField>
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
                            <iframe src="mst_igo_selling_delivery_order_bundle_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
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
        <div class="modal modal-open fade" id="modal-details" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Detail Order Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="mst_igo_sales_order_details.aspx" style="width: 100%; border: none; height: 570px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function postOrderChild(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_txtOrderID').value = s1;
                document.getElementById('ContentPlaceHolder1_txtSalesOrderDesc').value = s2;
                document.getElementById('ContentPlaceHolder1_txtInvoice').value = s3;
                document.getElementById('ContentPlaceHolder1_txtSellerDesc').value = s4;
                document.getElementById('ContentPlaceHolder1_txtCustName').value = s5;
                document.getElementById('ContentPlaceHolder1_txtCustEmail').value = s6;
                document.getElementById('ContentPlaceHolder1_txtCustAdd').value = s7;
                document.getElementById('ContentPlaceHolder1_txtCustPhone').value = s8;
                document.getElementById('ContentPlaceHolder1_txtMarketingSourceDesc').value = s9;
                document.getElementById('ContentPlaceHolder1_txtMarketingNameDesc').value = s10;
                document.getElementById('ContentPlaceHolder1_txtCount').value = s11;
                document.getElementById('ContentPlaceHolder1_txtResi').value = s12;
                document.getElementById('ContentPlaceHolder1_CmbDeliveryID').value = s13;
                document.getElementById('ContentPlaceHolder1_txtInvImg').value = s14;
                document.getElementById('ContentPlaceHolder1_txtRecipt').value = s15;
                
                $('#modal-order').modal('hide');

                var objfr = document.getElementById('iframedetails').contentWindow;
                var objOrderID = objfr.document.getElementById('txtOrderID');
                var objInvoiceID = objfr.document.getElementById('txtInvoice');
                var objCustName = objfr.document.getElementById('txtCustName');
                var objCustAdd = objfr.document.getElementById('txtCustAdd');
                var objCustEmail = objfr.document.getElementById('txtCustEmail');
                var cmdSearch = objfr.document.getElementById('CmdSearch');

                objOrderID.value = s1;
                objInvoiceID.value = s3;
                objCustName.Value = s5;
                objCustAdd.Value = s6;
                objCustEmail.Value = s7;

                cmdSearch.click();

            }
        }
        function showDetails() {
            var orderid;
            orderid = document.getElementById('ContentPlaceHolder1_txtOrderID');

            if (orderid.value != '') {
                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create delivery order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }
        function endRequest(sender, args) {
            $('#modal-details').on('hidden.bs.modal', function () {
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                objLoad.click();
            });
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
