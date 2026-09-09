<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_procurement_mutation.aspx.cs" Inherits="vtsadm.req_procurement_mutation" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Request Order - Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Request Order - Goods </li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Delivery Order Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Request Order ID</label>
                            <asp:TextBox ID="txtReqID" runat="server" class="form-control" placeholder="Skip for new Delivery Order ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Request Order Date</label>
                            <asp:TextBox ID="txtReqDate" TextMode="Date" runat="server" class="form-control" placeholder="DO Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type Request</label>
                            <asp:DropDownList ID="CmbProcurement" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbProcurement_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:DropDownList ID="CmbBranchID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbBranchID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label id="LblCmbWarehouse" runat="server" style="display: none">Warehouse Name</label>
                            <asp:DropDownList ID="CmbWarehouse" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbWarehouseID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label id="LblCmbTechnician" runat="server" style="display: none">Technician Name</label>
                            <asp:DropDownList ID="CmbTechnician" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label id="LblCmbMarketing" runat="server" style="display: none">Marketing Name</label>
                            <asp:DropDownList ID="CmbMarketing" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbMarketingID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label id="LblCmbCust" runat="server" style="display: none">Customer Name</label>
                            <asp:DropDownList ID="CmbCust" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                            <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                            <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility: hidden;" runat="server" onserverclick="CmdLoad_Click">1</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">List Detail Request Order</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <asp:Panel runat="server" ScrollBars="Auto">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField DataField="DeviceTypeID" HeaderText="Tools ID" ItemStyle-Wrap="false" SortExpression="DeviceTypeID"></asp:BoundField>
                                            <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                            <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Group Tools" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                            <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Type Tools" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                            <asp:BoundField DataField="QuantityDone" HeaderText="Quantity Done" ItemStyle-Wrap="false" SortExpression="QuantityDone"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
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
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Request Order</h3>
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
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDeleting="GridView2_RowDeleting" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnRowCommand="GridView2_RowCommand" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcurementID" HeaderText="Delivery Order ID" ItemStyle-Wrap="false" SortExpression="DloID"></asp:BoundField>
                                        <asp:BoundField DataField="ProcurementTypeDesc" HeaderText="Procurement Type Desc" ItemStyle-Wrap="false" SortExpression="ProcurementTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Wrap="false" SortExpression="BranchName"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing Name" ItemStyle-Wrap="false" SortExpression="MarketingName"></asp:BoundField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Wrap="false" SortExpression="CustomerName"></asp:BoundField>
                                        <asp:BoundField DataField="SchDate" HeaderText="SchDate" ItemStyle-Wrap="false" SortExpression="SchDate"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
                                        <asp:BoundField DataField="ProcurementType" HeaderText="Procurement Type" ItemStyle-Wrap="false" SortExpression="ProcurementType"></asp:BoundField>
                                        <asp:BoundField DataField="BranchID" HeaderText="Branch" ItemStyle-Wrap="false" SortExpression="BranchID"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse" ItemStyle-Wrap="false" SortExpression="WarehouseID"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician" ItemStyle-Wrap="false" SortExpression="TechnicianID"></asp:BoundField>
                                        <asp:BoundField DataField="CustID" HeaderText="Customer" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                         <asp:BoundField DataField="MarketingID" HeaderText="Marketing" ItemStyle-Wrap="false" SortExpression="MarketingID"></asp:BoundField>
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
        <div class="modal fade" id="modal-delete-header">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Request ID :&nbsp;</h6>
                        <label id="LblReqID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtReqIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYes();" onserverclick="CmdYes_ServerClick" id="CmdYes">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-header').modal('hide');">No</button>
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
                        <h6 class="modal-title">Are you sure to delete sequence :&nbsp;</h6>
                        <label id="LblSeq" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtSeqDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="buttonYesDetail();" onserverclick="CmdYesDetail_ServerClick" id="CmdYesDetail">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete-detail').modal('hide');">No</button>
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
                        <h4 class="modal-title">Add Detail Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="req_procurement_mutation_detail.aspx" style="width: 100%; border: none; height: 415px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function buttonYes() {
            $("#modal-delete-header").modal('hide');
        }
        function confirmDelete(sText,sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblReqID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtReqIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete-header").modal('show');
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
        function showDetails() {
            var objfr = document.getElementById("iframedetails").contentWindow;
            var cmdClear = objfr.document.getElementById("Button1");
            cmdClear.click();
            $('#modal-details').modal('show');
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
