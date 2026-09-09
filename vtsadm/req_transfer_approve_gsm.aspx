<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_transfer_approve_gsm.aspx.cs" Inherits="vtsadm.req_transfer_approve_gsm" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Expenditures Of Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Expenditures Of Goods - Approve</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Expenditures Approve Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Expenditures ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtProcurementID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Expenditures Desc</label>
                            <input type="text" id="txtProcurementDesc" runat="server" class="form-control" placeholder="Procurement Desc Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Desc</label>
                            <input type="text" id="txtWarehouseDesc" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Device Type</label>
                                <input type="text" id="txtDeviceDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." readonly="readonly" />
                            </div>
                            <div class="form-group col-md-6">
                                <label>Quantity Device</label>
                                <input type="text" id="txtQtyDevice" runat="server" class="form-control" placeholder="Value ..." readonly="readonly" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>GSM Type</label>
                                <input type="text" id="txtGSMDesc" runat="server" class="form-control" placeholder="GSM Type Desc ..." readonly="readonly" />
                            </div>
                            <div class="form-group col-md-6">
                                <label>Quantity GSM</label>
                                <input type="text" id="txtQtyGSM" runat="server" class="form-control" placeholder="Value ..." readonly="readonly" />
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Relay 12 V</label>
                                <input type="text" id="txtQtyRelay12V" runat="server" class="form-control" placeholder="Value ..." readonly="readonly" />
                            </div>
                            <div class="form-group col-md-6">
                                <label>Relay 24 V</label>
                                <input type="text" id="txtQtyRelay24V" runat="server" class="form-control" placeholder="Value ..." readonly="readonly" />
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
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <label>Qty GSM Final</label>
                                <asp:TextBox ID="txtQtyFinalGsm" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Qty Relay 12 V Final</label>
                                <asp:TextBox ID="txtQtyFinal12V" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Qty Relay 12 V Final</label>
                                <asp:TextBox ID="txtQtyFinal24V" runat="server" class="form-control" placeholder="Value ..." required="required"></asp:TextBox>
                            </div>
                            <div class="box-footer">
                                <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                                <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                                <button id="CmdUploadGSMMutation" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Mutation Upload</button>
                                <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">List Expenditures Of Goods</h3>
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
                        <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView2_RowDataBound" OnSorting="GridView2_Sorting">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField DataField="ProcurementID" HeaderText="Procurement ID" ItemStyle-Wrap="false" SortExpression="ProcurementID"></asp:BoundField>
                                <asp:BoundField DataField="ParValue" HeaderText="Procurement Type" ItemStyle-Wrap="false" SortExpression="ProcurementType"></asp:BoundField>
                                <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                <asp:BoundField DataField="QtyDeviceFinal" HeaderText="Quantity Device" ItemStyle-Wrap="false" SortExpression="QtyDeviceFinal"></asp:BoundField>
                                <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="false" SortExpression="ProviderName"></asp:BoundField>
                                <asp:BoundField DataField="QtyGSMFinal" HeaderText="Quantity GSM" ItemStyle-Wrap="false" SortExpression="QtyGSMFinal"></asp:BoundField>
                                <asp:BoundField DataField="Qty12VFinal" HeaderText="Quantity Relay 12 V" ItemStyle-Wrap="false"></asp:BoundField>
                                <asp:BoundField DataField="Qty24VFinal" HeaderText="Quantity Relay 24 V" ItemStyle-Wrap="false"></asp:BoundField>
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
        <div class="modal fade bs-example-modal-lg" id="modal-order">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Procurement Of Goods</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="req_transfer_gsm_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-upload">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="gsm_mutation_technician_upload_procurement.aspx" style="width: 100%; border: none; height: 430px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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
        function postOrderChild(s1, s2, s3, s4, s5, s6, s7, s8, s9) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_txtProcurementID').value = s1;
                document.getElementById('ContentPlaceHolder1_txtProcurementDesc').value = s2;
                document.getElementById('ContentPlaceHolder1_txtWarehouseDesc').value = s3;
                document.getElementById('ContentPlaceHolder1_txtDeviceDesc').value = s4;
                document.getElementById('ContentPlaceHolder1_txtQtyDevice').value = s5;
                document.getElementById('ContentPlaceHolder1_txtGSMDesc').value = s6;
                document.getElementById('ContentPlaceHolder1_txtQtyGSM').value = s7;
                document.getElementById('ContentPlaceHolder1_txtQtyRelay12V').value = s8;
                document.getElementById('ContentPlaceHolder1_txtQtyRelay24V').value = s9;

                $('#modal-order').modal('hide');
                cmdSearch.click();

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
