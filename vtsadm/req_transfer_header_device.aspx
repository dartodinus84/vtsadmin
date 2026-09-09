<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_transfer_header_device.aspx.cs" Inherits="vtsadm.req_transfer_header_device" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Expenditures Of Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Expenditures Of Goods</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Expenditures Of Goods Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Expenditures ID</label>
                            <asp:TextBox ID="txtProcurementID" runat="server" class="form-control" placeholder="Skip for new Procurement ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Type Expenditures</label>
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

                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Device Type</label>
                                <asp:DropDownList ID="CmbTypeDevice" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQtyDevice">Quantity Device</label>
                                <asp:TextBox ID="txtQtyDevice" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="form-row">
                            <div class="form-group col-md-6">
                                <label>GSM Type</label>
                                <asp:DropDownList ID="CmbTypeGSM" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label for="txtQtyGSM">Quantity GSM</label>
                                <asp:TextBox ID="txtQtyGSM" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>--%>
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
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Others Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Schedule Date</label>
                            <asp:TextBox ID="txtScheduleDate" TextMode="Date" runat="server" class="form-control" placeholder="Schedule Date ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">List Procurement Of Goods</h3>
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
                                <asp:BoundField DataField="ProcurementID" HeaderText="Procurement ID" ItemStyle-Wrap="false" SortExpression="ProcurementID"></asp:BoundField>
                                <asp:BoundField DataField="ParValue" HeaderText="Procurement Type" ItemStyle-Wrap="false" SortExpression="ProcurementType"></asp:BoundField>
                                <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                <asp:BoundField DataField="QtyDevice" HeaderText="Quantity Device" ItemStyle-Wrap="false" SortExpression="QtyDevice"></asp:BoundField>
                                <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="false" SortExpression="ProviderName"></asp:BoundField>
                                <asp:BoundField DataField="QtyGSM" HeaderText="Quantity GSM" ItemStyle-Wrap="false" SortExpression="QtyGSM"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="GSMID" HeaderText="GSM ID" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="WarehouseID" HeaderText="WarehouseID" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="SchDate" HeaderText="SchDate" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="Relay12V" HeaderText="Relay12V" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="Relay24V" HeaderText="Relay24V" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="ProcurementType" HeaderText="ProcurementType" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="BranchID" HeaderText="BranchID" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="TechnicianID" HeaderText="TechnicianID" ItemStyle-Wrap="false"></asp:BoundField>
                                
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
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-submit').modal('hide');" onserverclick="CmdSumbit_Click" id="CmdSumbit">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-submit').modal('hide');">No</button>
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
                        <h6 class="modal-title">Are you sure to delete Procurement ID :&nbsp;</h6>
                        <label id="LblProcurementID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtProcurementIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
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

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblProcurementID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtProcurementIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }
        function endRequest(sender, args) {

            $('#modal-upload').on('hidden.bs.modal', function () {
                var objClear = document.getElementById('ContentPlaceHolder1_CmdClear');
                objClear.click();
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
