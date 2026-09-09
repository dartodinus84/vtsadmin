<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_procurement_header.aspx.cs" Inherits="vtsadm.req_procurement_header" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Purchase Order - Procurement Of Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Purchase Order - Procurement Of Goods</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Purchase Order - Procurement Of Goods Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Purchase Order ID</label>
                            <asp:TextBox ID="txtPOID" runat="server" class="form-control" placeholder="Skip for new Procurement ..." required="required" disabled=""></asp:TextBox>
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
                            <label>Remarks</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                            <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                            <button id="CmdLoad" type="button" class="btn btn-primary" style="visibility: hidden;" runat="server" onserverclick="CmdLoad_Click">1</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Detail Purchase Order - Procurement Of Goods Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="PurID" HeaderText="Purchase Order ID" ItemStyle-Wrap="false" SortExpression="JobID"></asp:BoundField>
                                        <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                        <asp:BoundField DataField="GroupTools" HeaderText="Group Tools" ItemStyle-Wrap="false" SortExpression="GroupTools"></asp:BoundField>
                                        <asp:BoundField DataField="TypeTools" HeaderText="Type Tools" ItemStyle-Wrap="false" SortExpression="TypeTools"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Wrap="false" SortExpression="Quantity"></asp:BoundField>
                                        <asp:BoundField DataField="Price" HeaderText="Amount / Items" ItemStyle-Wrap="false" SortExpression="Price"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDeleteDetail" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupID" HeaderText="Group Tools" ItemStyle-Wrap="false" SortExpression="TvdID"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeID" HeaderText="Type Tools" ItemStyle-Wrap="false" SortExpression="DeviceTypeID"></asp:BoundField>
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
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">List Purchasing Order - Procurement Of Goods</h3>
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
                                <asp:BoundField DataField="PurID" HeaderText="Purchase Order ID" ItemStyle-Wrap="false" SortExpression="PurID"></asp:BoundField>
                                <asp:BoundField DataField="PurDate" HeaderText="Purchase Order Date" ItemStyle-Wrap="false" SortExpression="PurDate"></asp:BoundField>
                                <asp:BoundField DataField="PurNumber" HeaderText="Purchase Number" ItemStyle-Wrap="false" SortExpression="PurNumber"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
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
                        <h6 class="modal-title">Are you sure to delete Purchase Order ID :&nbsp;</h6>
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
        <div class="modal fade" id="modal-details" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Add Detail Information</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframedetails" src="req_procurement_detail.aspx" style="width: 100%; border: none; height: 470px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
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
        function showDetails() {
            var procurementID;
            procurementID = document.getElementById('ContentPlaceHolder1_txtPOID');

            if (procurementID.value != '') {
                $('#modal-details').modal('show');
            }
            else {
                document.getElementById('ContentPlaceHolder1_div_comment').innerHTML = '<div class="alert alert-danger" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Failed!</strong> Please create delivery order header first!</div>';
                $('#modal-messagebox').modal('show');
            }
            return false;
        }
        function confirmDeleteDetail(sText) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblSeq').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtSeqDelete').value = sText;
                $("#modal-delete-detail").modal('show');
            }
        }

        function updateDevice(objDD) {
            var objfr = document.getElementById("iframedetails").contentWindow;
            var cmdClear = objfr.document.getElementById("Button1");
            var objCustID = objfr.document.getElementById("txtCustID");
            if (objDD.value == "JTP0000002") {
                objCustID.value = document.getElementById("ContentPlaceHolder1_txtCustID").value;
            }
            else {
                objCustID.value = "";
            }
            cmdClear.click();
            return false;
        }
        function buttonYesSubmit() {
            $("#modal-submit").modal('hide');
        }
        function endRequest(sender, args) {
            $('#modal-details').on('hidden.bs.modal', function () {
                var objLoad = document.getElementById('ContentPlaceHolder1_CmdLoad');
                objLoad.click();
            });
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
