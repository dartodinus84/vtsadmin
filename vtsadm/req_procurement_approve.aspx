<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_procurement_approve.aspx.cs" Inherits="vtsadm.req_procurement_approve" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Delivery Order Of Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Delivery Order Of Goods - Approve</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Delivery Order - Approve Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Delivery Order ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtDloID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Purchase Order ID</label>
                            <input type="text" id="txtPurID" runat="server" class="form-control" placeholder="Procurement Desc Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Delivery Order Date</label>
                            <input type="text" id="txtDloDate" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark Desc</label>
                            <input type="text" id="txtRemark" runat="server" class="form-control" placeholder="Warehouse Name ..." readonly="readonly" />
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                            <button id="CmdUploadDeviceMutation" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-device">Device Upload</button>
                            <button id="CmdUploadGSMMutation" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-gsm">GSM Upload</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">List Detail Order</h3>
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
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">List Detail Device</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <asp:Panel runat="server" ScrollBars="Auto">
                                    <asp:GridView ID="GridView3" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView3_PageIndexChanging" OnRowDeleting="GridView3_RowDeleting" OnRowDataBound="GridView3_RowDataBound" OnSorting="GridView3_Sorting">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField DataField="ToolsID" HeaderText="Tools ID" ItemStyle-Wrap="false" SortExpression="ToolsID"></asp:BoundField>
                                            <asp:BoundField DataField="Seq" HeaderText="Seq" ItemStyle-Wrap="false" SortExpression="Seq"></asp:BoundField>
                                            <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Group Tools" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                            <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Type Tools" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                            <asp:BoundField DataField="ToolsDetail" HeaderText="Tools Detail" ItemStyle-Wrap="false" SortExpression="ToolsDetail"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        </Columns>
                                        <RowStyle ForeColor="#003481" BackColor="White" />
                                        <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                        <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                        <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                        <HeaderStyle Height="20px" Wrap="True" />
                                        <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                    </asp:GridView>
                                    <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                        <asp:Label ID="LblPagingDetail2" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">List Delivery Order Of Goods</h3>
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
                                <asp:BoundField DataField="DloID" HeaderText="Delivery Order ID" ItemStyle-Wrap="false" SortExpression="DloID"></asp:BoundField>
                                <asp:BoundField DataField="DloDate" HeaderText="Delivery Order Date" ItemStyle-Wrap="false" SortExpression="DloDate"></asp:BoundField>
                                <asp:BoundField DataField="PurID" HeaderText="Purchase Order ID" ItemStyle-Wrap="false" SortExpression="PurID"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
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
                            <iframe src="req_procurement_approve_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-upload-device">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="device_upload_do.aspx" style="width: 100%; border: none; height: 430px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-upload-gsm">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Upload GSM</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="gsm_upload_do.aspx" style="width: 100%; border: none; height: 430px;" scrolling="no"></iframe>
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
        function postOrderChild(s1, s2, s3, s4) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_txtDloID').value = s1;
                document.getElementById('ContentPlaceHolder1_txtPurID').value = s2;
                document.getElementById('ContentPlaceHolder1_txtDloDate').value = s3;
                document.getElementById('ContentPlaceHolder1_txtRemark').value = s4;

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
