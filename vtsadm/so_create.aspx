<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="so_create.aspx.cs" Inherits="vtsadm.so_create" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Purchase Order 
            <small>Create</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Sales Order</a></li>
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
                            <label>Customer ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtCustID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-customer"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtCustFullName" runat="server" class="form-control" placeholder="Full Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Type</label>
                            <asp:TextBox ID="txtCustTypeDesc" runat="server" class="form-control" placeholder="Customer Type ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail Purchase Order</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="PoID" HeaderText="PO ID" ItemStyle-Wrap="false" SortExpression="PoID"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Device Group ID" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type ID" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Req" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                        <asp:BoundField DataField="QuantityDone" HeaderText="Qty Job" ItemStyle-Wrap="false" SortExpression="QuantityDone"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdCloseDetail" runat="server" Text="<i class='fa fa-exclamation'></i>" ToolTip="Close" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>    

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
                                <div style="margin-top: -18px; margin-bottom: 12px;margin-left:10px;"><asp:Label id="LblPagingDetail" runat="server" style="color: #003481;font-style:italic;font-size:13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Header Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>PO ID</label>
                            <asp:TextBox ID="txtPoID" runat="server" class="form-control" placeholder="Skip for new purchase order ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PO Date</label>
                            <asp:TextBox ID="txtPoDate" TextMode="Date" runat="server" class="form-control" placeholder="PO Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PO Number</label>
                            <asp:TextBox ID="txtPoNumber" runat="server" class="form-control" placeholder="PO Number ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>PO Type</label>
                            <asp:DropDownList ID="CmbPoType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Contract Time (Months)</label>
                            <input type="number" min="1" max="60" id="txtContractTime" runat="server" class="form-control" placeholder="Contract Time ..." onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="reset" class="btn btn-primary" runat="server" onserverclick="CmdClear_Click">Clear</button>
                        <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                        <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="confirmSubmit(); return false;" Text="Submit" />
                        <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility:hidden" runat="server" onserverclick="CmdLoad_Click">1</button>                        
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
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView1_RowDeleting" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" OnSorting="GridView1_Sorting" >
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="PoID" HeaderText="PO ID" ItemStyle-Wrap="false" SortExpression="PoID"></asp:BoundField>
                                        <asp:BoundField DataField="sPoDate" HeaderText="PO Date" ItemStyle-Wrap="false" SortExpression="sPoDate"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="PoNumber" HeaderText="PO No" ItemStyle-Wrap="false" SortExpression="PoNumber"></asp:BoundField>
                                        <asp:BoundField DataField="PoTypeDesc" HeaderText="PO Type" ItemStyle-Wrap="false" SortExpression="PoTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="ContractTime" HeaderText="Contract Time" ItemStyle-Wrap="false" SortExpression="ContractTime"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Customer Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="PoTypeID" HeaderText="PO Type ID" ItemStyle-Wrap="false"></asp:BoundField>                                        
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

        <div class="modal fade" id="modal-submit">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to submit PO ID :&nbsp;</h6><label id="LblPoIDSubmit" runat="server"></label>&nbsp;?
                        <p><asp:CheckBox ID="ChkJo" runat="server" Checked="false" Text="&nbsp;&nbsp;Auto Generate Job Order" /></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesSubmit();" onserverclick="CmdYesSubmit_ServerClick" id="CmdYesSubmit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-close-detail">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-xs-8">
                                    <h6 class="modal-title">Are you sure to close sequence :&nbsp;</h6>
                                </div>
                                <div class="col-xs-4" style="text-align:left;">
                                    <label id="LblSeqClose" runat="server"></label>&nbsp;?
                                </div>                                
                            </div>
                            <input type="text" id="txtRemarkClose" runat="server" class="form-control" placeholder="Close Remark ..." />
                            <input type="hidden" id="txtSeqClose" runat="server" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesClose();" onserverclick="CmdYesClose_ServerClick" id="CmdYesClose">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-close-detail').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="modal fade" id="modal-delete-detail">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete sequence :&nbsp;</h6><label id="LblSeq" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtSeqDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesDetail();" onserverclick="CmdYesDetail_ServerClick" id="CmdYesDetail">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-detail').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal-delete-header">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete PO ID :&nbsp;</h6><label id="LblPoID" runat="server"></label>&nbsp;?
                        <input type="hidden" id="txtPoIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYes();" onserverclick="CmdYes_ServerClick" id="CmdYes">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-header').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-customer">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Customer</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="so_create_customer_search.aspx" style="width: 100%; border: none; height: 350px;" overflow:hidden;" scrolling="no"></iframe>
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
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"> <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Detail Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="so_create_details.aspx" style="width: 100%; border: none; height: 415px; overflow:hidden;" scrolling="yes"></iframe>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

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

    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function postCustChild(sCustID, sFullName, sCustTypeDesc, sBranchName) {
            if (sCustID != '') {
                document.getElementById('ContentPlaceHolder1_txtCustID').value = sCustID;
                document.getElementById('ContentPlaceHolder1_txtCustFullName').value = sFullName;
                document.getElementById('ContentPlaceHolder1_txtCustTypeDesc').value = sCustTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = sBranchName;
                $('#modal-customer').modal('hide');
            }
        }

        function showDetails() {
            var poid;
            poid = document.getElementById('ContentPlaceHolder1_txtPoID');

            if (poid.value != '') {
                var objfr = document.getElementById("iframedetails").contentWindow;
                var objPoID = objfr.document.getElementById("txtPoID");
                var cmdClear = objfr.document.getElementById("Button1");
                objPoID.value = poid.value;
                cmdClear.click();
                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create purchase order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }

        function buttonYesClose() {
            $("#modal-close-detail").modal('hide');
        }

        function confirmCloseDetail(sText) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblSeqClose').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtSeqClose').value = sText;
                $("#modal-close-detail").modal('show');
            }
        }

        function buttonYesSubmit() {
            $("#modal-submit").modal('hide');
        }

        function confirmSubmit() {
            var objPoID = document.getElementById('ContentPlaceHolder1_txtPoID');
            if (objPoID.value != '') {
                document.getElementById('ContentPlaceHolder1_LblPoIDSubmit').innerHTML = objPoID.value;
                $("#modal-submit").modal('show');
            }
        }

        function buttonYesDetail() {
            $("#modal-delete-detail").modal('hide');
        }

        function confirmDeleteDetail(sText) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblSeq').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtSeqDelete').value = sText;
                $("#modal-delete-detail").modal('show');
            }
        }

        function buttonYes() {
            $("#modal-delete-header").modal('hide');
        }

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblPoID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtPoIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete-header").modal('show');
            }
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
