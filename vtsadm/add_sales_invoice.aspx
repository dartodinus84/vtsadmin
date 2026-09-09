<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="add_sales_invoice.aspx.cs" Inherits="vtsadm.add_sales_invoice" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-tagsinput/0.8.0/bootstrap-tagsinput.css" integrity="sha512-xmGTNt20S0t62wHLmQec2DauG9T+owP9e6VU8GigI0anN7OXLip9i7IwEhelasml2osdxX71XcYm6BQunTQeQg==" crossorigin="anonymous" />
    <style type="text/css">
        .bootstrap-tagsinput .tag {
           background: red;
           padding: 4px;
           font-size: 14px;
        }
        .bootstrap-tagsinput input {
           width: 1000px;
        }
    </style>
    <section class="content-header">
        <h1>Sales Invoice 
            <small>Create</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Sales Invoice </a></li>
            <li class="active">Create</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Sales Order ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtSalesOrderId" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-sales-order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm" style="display:none;">
                            <asp:TextBox ID="txtPersonId" runat="server" class="form-control" placeholder="Person ID ..." required="required" disabled="" ></asp:TextBox>
                            <asp:TextBox ID="txtCompId" runat="server" class="form-control" placeholder="Comp ID ..." required="required" disabled="" ></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustName1" runat="server" class="form-control" placeholder="Customer Address ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Address</label>
                            <asp:TextBox ID="txtAddr" runat="server" class="form-control" placeholder="Customer Address ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Customer Email ..." required="required" disabled=""></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Detail Invoice</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Product Name</label>
                            <asp:DropDownList ID="selProduct" runat="server" CssClass="form-control" disabled=""></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Amount / Rate</label>
                            <asp:TextBox ID="txtRate" runat="server" class="form-control" required="required" disabled="" ></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Qty</label>
                            <asp:TextBox ID="txtQty" runat="server" class="form-control" required="required" disabled="" ></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Line Tax</label>
                            <asp:TextBox ID="txtTax" runat="server" class="form-control" value="PPN" required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>With Holding Type</label>
                            <asp:TextBox ID="txtWithHolding" runat="server" class="form-control" value="percent" required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Discount Type</label>
                            <asp:TextBox ID="txtDiscType" runat="server" class="form-control" value="percent" required="required" disabled=""></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Transaction Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Transaction No</label>
                            <asp:TextBox ID="txtTransactionNo" runat="server" class="form-control" placeholder="Transaction No ..." required="required" ></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Shipping Date</label>
                            <asp:TextBox ID="txtShipingDate" runat="server" class="form-control" placeholder="Transaction No ..." required="required" disabled="" ></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Due Date</label>
                            <asp:TextBox ID="txtDueDate" TextMode="Date" runat="server" class="form-control" placeholder="Due Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Message</label>
                            <asp:TextBox ID="txtMessage" TextMode="multiline" Rows="3" runat="server" class="form-control" placeholder="Message ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Memo</label>
                            <asp:TextBox ID="txtMemo" TextMode="multiline" Rows="3" runat="server" class="form-control" placeholder="Memo ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tags</label>
                            <asp:TextBox ID="txtTags" TextMode="multiline" Rows="3" runat="server" class="form-control" placeholder="Tags ..." disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Gps Type</label>
                            <asp:DropDownList ID="selGpsType" runat="server" CssClass="form-control" disabled=""></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Term</label>
                            <asp:TextBox ID="txtTerm" runat="server" class="form-control" value="Custom" required="required" disabled=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="confirmSubmit(); return false;" Text="Submit" />                 
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List PO</h3>
                        <div class="box-tools">
                            <div class="input-group input-group-sm" style="width: 200px;">
                                <input type="text" id="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by any fields ..." />
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" onserverclick="CmdSearch_ServerClick"><i class="fa fa-search"></i></button>
                                </span>
                            </div>                                                     
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView1_RowDeleting" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting" >
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="invoice_id" HeaderText="ID" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="transaction_no" HeaderText="Transaction No" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="transaction_date" HeaderText="Transaction Date" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="due_date" HeaderText="Due Date" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="shipping_date" HeaderText="Shipping Date" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="sales_order_id" HeaderText="Sales Order ID" ItemStyle-Wrap="false" SortExpression="sales_order_id"></asp:BoundField>
                                        <asp:BoundField DataField="company_id" HeaderText="Comp ID" ItemStyle-Wrap="false" SortExpression="company_id"></asp:BoundField>
                                        <asp:BoundField DataField="person_id" HeaderText="Person ID" ItemStyle-Wrap="false" SortExpression="person_id"></asp:BoundField>
                                        <asp:BoundField DataField="person_name" HeaderText="Person Name" ItemStyle-Wrap="false" SortExpression="person_name"></asp:BoundField>
                                        <asp:BoundField DataField="person_address" HeaderText="Person Address" ItemStyle-Wrap="false" SortExpression="person_address"></asp:BoundField>
                                        <asp:BoundField DataField="person_email" HeaderText="Person Email" ItemStyle-Wrap="false" SortExpression="person_email"></asp:BoundField>
                                        <asp:BoundField DataField="product_name" HeaderText="Product Name" ItemStyle-Wrap="false" SortExpression="product_name"></asp:BoundField>
                                        <asp:BoundField DataField="qty" HeaderText="Qty" ItemStyle-Wrap="false" SortExpression="qty"></asp:BoundField>
                                        <asp:BoundField DataField="amount" HeaderText="Amount" ItemStyle-Wrap="false" SortExpression="amount"></asp:BoundField>
                                        <asp:BoundField DataField="tags" HeaderText="Tags" ItemStyle-Wrap="false" SortExpression="tags"></asp:BoundField>
                                        <asp:BoundField DataField="memo" HeaderText="Memo" ItemStyle-Wrap="false" SortExpression="tags"></asp:BoundField>       
                                        <asp:BoundField DataField="message" HeaderText="Message" ItemStyle-Wrap="false" SortExpression="tags"></asp:BoundField>                                             
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px;margin-left:10px;"><asp:Label id="LblPagingHeader" runat="server" style="color: #003481;font-style:italic;font-size:13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="modal fade bs-example-modal-lg" id="modal-sales-order">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Sales Order</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="sales_order_jurnal_search.aspx" style="width: 100%; border: none; height: 350px;" overflow:hidden;" scrolling="no"></iframe>
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-tagsinput/0.8.0/bootstrap-tagsinput.min.js" integrity="sha512-9UR1ynHntZdqHnwXKTaOm1s6V9fExqejKvg5XMawEMToW4sSw+3jtLrYfZPijvnwnnE8Uol1O9BcAskoxgec+g==" crossorigin="anonymous"></script>
        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);
            
             function postSalesOrderChild(sId, sCompId, sPersonId, sName, sAddr, sEmail, sProduct, sAmount, sQty, sTags, sGpsType, sShipingDate) {
                 if (sName != '') {
                     document.getElementById('ContentPlaceHolder1_txtCustName1').value = sName;
                     document.getElementById('ContentPlaceHolder1_txtAddr').value = sAddr;
                     document.getElementById('ContentPlaceHolder1_txtEmail').value = sEmail;
                     document.getElementById('ContentPlaceHolder1_txtSalesOrderId').value = sId;
                     document.getElementById('ContentPlaceHolder1_txtCompId').value = sCompId;
                     document.getElementById('ContentPlaceHolder1_txtPersonId').value = sPersonId;

                     document.getElementById('ContentPlaceHolder1_selProduct').value = sProduct;
                     document.getElementById('ContentPlaceHolder1_txtRate').value = sAmount;
                     document.getElementById('ContentPlaceHolder1_txtQty').value = sQty;
                     document.getElementById('ContentPlaceHolder1_txtCompId').value = sCompId;
                     document.getElementById('ContentPlaceHolder1_selGpsType').value = sGpsType;
                     document.getElementById('ContentPlaceHolder1_txtTags').value = sTags;
                     document.getElementById('ContentPlaceHolder1_txtShipingDate').value = sShipingDate;

                     $('#modal-sales-order').modal('hide');
                 }
             }


            $(document).ready(function(){        
                var tagInputEle = $('#ContentPlaceHolder1_txtTags');
                tagInputEle.tagsinput();
            });
            
            function confirmSubmit() {
                $("#modal-submit").modal('show');
            }

            function buttonYesSubmit() {
                $("#modal-submit").modal('hide');
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
    </section>
</asp:Content>


