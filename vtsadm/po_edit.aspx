<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="po_edit.aspx.cs" Inherits="vtsadm.po_edit" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Purchase Order 
            <small>Create</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Purchase Order</a></li>
            <li class="active">Edit</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Customer ID</label>
                            <input type="text" id="txtCustID" runat="server" class="form-control" placeholder="Customer ID ..." readonly="readonly" required="required" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Full Name</label>
                            <asp:TextBox ID="txtCustFullName" runat="server" class="form-control" placeholder="Full Name ..." required="readonly" disabled=""></asp:TextBox>
                        </div>

                         <div class="form-group form-group-sm">
                             <label>JOB ID</label>
                             <asp:TextBox ID="txtJobID" runat="server" class="form-control" placeholder="JOB ID...." required="required" disabled=""></asp:TextBox>
                         </div>
                      

                        <div class="form-group form-group-sm">
                            <label>PO ID</label>
                            <asp:TextBox ID="txtPoID" runat="server" class="form-control" placeholder="PO ID...." required="required" disabled=""></asp:TextBox>
                        </div>

                        <input type="hidden" id="txtEditDevice" runat="server" />
                        <input type="hidden" id="txtDeviceTypeID" runat="server" />
                        <input type="hidden" id="txtEditJobID" runat="server" />
                        <!-- Button add details -->
                        <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();"><i class="fa fa-plus-circle m-3"></i> &nbsp Add Details</button>
                        <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility:hidden" runat="server" onserverclick="CmdLoad_Click">1</button>

                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail Purchase Order</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound" OnRowCommand="GridView2_RowCommand" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="PoID" HeaderText="PO ID" ItemStyle-Wrap="false" SortExpression="PoID"></asp:BoundField>
                 
                                        <asp:BoundField DataField="JobID" HeaderText="Job ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupID" HeaderText="Device Group" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty Req" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                        <asp:BoundField DataField="QuantityDone" HeaderText="Qty Done" ItemStyle-Wrap="false" SortExpression="QuantityDone"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdEditQty" runat="server" Text="<i class='fa fa-edit'></i>" ToolTip="EditQty" Enabled="true" CssClass="btn btn btn-info btn-xs" CommandName="EditDevice" />
                                            </ItemTemplate>
                                        </asp:TemplateField>   
                                        
                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                              <ItemTemplate>
                                                  <asp:LinkButton ID="CmdEditDevice" runat="server"  Text="<i class='fa fa-truck'></i>" ToolTip="EditDevice" Enabled="true" CssClass="btn btn btn-info btn-xs"   />
                                              </ItemTemplate>
                                          </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" ItemStyle-Wrap="false" SortExpression="DeviceTypeID"></asp:BoundField>
                                        <asp:BoundField DataField="IsDelete" HeaderText="IsDelete" ItemStyle-Wrap="false" SortExpression="IsDelete"></asp:BoundField>
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
                                        <asp:BoundField DataField="FullName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                                                               
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
        
        <!-- modal edit qty -->
        <div class="modal fade" id="modal-edit-detail-qty">
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
                                    <h6 class="modal-title">Are you sure to Edit Qty Req :&nbsp;</h6>
                                </div>
                                <div class="col-xs-4" style="text-align:left;">
                                    <label id="LblEditQty" runat="server"></label>&nbsp;?
                                </div>                                
                            </div>
                            <p class="help-block" style="margin-bottom:6px;">Qty Done (min): <label id="LblEditQtyDone" runat="server"></label></p>
                            <input type="text" id="txtQuantity" runat="server" class="form-control" placeholder="New Quantity Request..." />
                            <input type="hidden" id="txtEditQty" runat="server" />
                            <input type="hidden" id="txtEditQtyDone" runat="server" />
                            
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesQty();" onserverclick="CmdYesEditQty_ServerClick" id="CmdYesEditQty">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-edit-detail-qty').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal edit device type -->
        <div class="modal fade" id="modal-edit-detail-deviceType">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <div class="row">
                                <div class="col-xs-12">
                                    <h6 class="modal-title">Are you sure to Edit Device Type :  <label id="LblEditDevice" runat="server"></label>?</h6>
                                </div>
                                <div class="" style="text-align:left;">
                                   
                                </div>                                
                            </div>
                            <label>Device Type</label>
                            <asp:DropDownList ID="CmbDeviceTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesDevice();" onserverclick="CmdYesEditDeviceType_ServerClick" id="CmdYesEditDeviceType">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-edit-detail-deviceType').modal('hide');">No</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal create add details -->
        <div class="modal fade" id="modal-details" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"><i class="fa fa-plus-circle"></i> Add Detail Information</h4>
                    </div>
                    <div class="modal-body" style="padding:0;">
                        <iframe id="iframedetails" src="po_add_details_pojo.aspx" style="width:100%;border:none;height:300px;display:block;" scrolling="auto"></iframe>
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
                        <h6 class="modal-title">Yakin ingin menghapus detail purchase order ini?</h6>
                        <asp:HiddenField ID="txtDetailDeviceTypeDelete" runat="server" />
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
            var poid = document.getElementById('ContentPlaceHolder1_txtPoID');

            if (poid.value != '') {
                var objfr = document.getElementById("iframedetails").contentWindow;
                var objPoID = objfr.document.getElementById("txtPoID");
                var cmdClear = objfr.document.getElementById("Button1");
                if (objPoID) {
                    objPoID.value = poid.value;
                }
                if (cmdClear) {
                    cmdClear.click();
                }

                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please edit purchase order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }

        function buttonYesQty() {
            $("#modal-edit-detail-qty").modal('hide');
        }

        function buttonYesDevice() {
            $("#modal-edit-detail-deviceType").modal('hide');
        }

        function confirmEditQuantity(sText, sDeviceTypeID, sJobID, sQtyDone) {
            if (sText != '') {
                var qtyDone = parseInt(sQtyDone, 10);
                if (isNaN(qtyDone)) {
                    qtyDone = 0;
                }

                document.getElementById('ContentPlaceHolder1_LblEditQty').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_LblEditQtyDone').innerHTML = qtyDone;
                document.getElementById('ContentPlaceHolder1_txtEditQty').value = sText;
                document.getElementById('ContentPlaceHolder1_txtEditQtyDone').value = qtyDone;
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeID').value = sDeviceTypeID;
                document.getElementById('ContentPlaceHolder1_txtEditJobID').value = sJobID;
                $("#modal-edit-detail-qty").modal('show');
            }
        }

        function confirmEditDevice(sText, sDeviceTypeID, sJobID) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblEditDevice').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtEditDevice').value = sText
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeID').value = sDeviceTypeID;
                document.getElementById('ContentPlaceHolder1_txtEditJobID').value = sJobID;
                //document.getElementById('ContentPlaceHolder1_txtDeviceGroup').value = sDeviceGroupID;
                //document.getElementById('ContentPlaceHolder1_CmbDeviceGroupID').value = sCmbDeviceGroupID;
                //document.getElementById('ContentPlaceHolder1_CmbDeviceTypeID').value = sCmbDeviceTypeID;

                $("#modal-edit-detail-deviceType").modal('show');
            }
        }

        function buttonYesSubmit() {
            $("#modal-submit").modal('hide');
        }

        function confirmSubmit() {
            var objPoID = document.getElementById('ContentPlaceHolder1_txtPoID');
            var objJobID = document.getElementById('ContentPlaceHolder1_txtJobID');

            if (objPoID.value != '' && objJobID.value != '') {
                document.getElementById('ContentPlaceHolder1_LblPoIDSubmit').innerHTML = objPoID.value;
                document.getElementById('ContentPlaceHolder1_LblJobIDSubmit').innerHTML = objJobID.value;
                $("#modal-submit").modal('show');
            }
        }

        function buttonYesDetail() {
            $("#modal-delete-detail").modal('hide');
        }

        function confirmDeleteDetail(sDeviceTypeID) {
            if (sDeviceTypeID != '') {
                document.getElementById('ContentPlaceHolder1_txtDetailDeviceTypeDelete').value = sDeviceTypeID;
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
            $('#modal-details').off('hidden.bs.modal').on('hidden.bs.modal', function () {
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                if (objLoad) {
                    objLoad.click();
                }
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
