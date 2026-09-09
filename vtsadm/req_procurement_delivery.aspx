<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="req_procurement_delivery.aspx.cs" Inherits="vtsadm.req_procurement_delivery" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Delivery Order - Goods</h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Delivery Order - Goods </li>
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
                            <label>Delivery Order ID</label>
                            <asp:TextBox ID="txtDOID" runat="server" class="form-control" placeholder="Skip for new Delivery Order ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Purchase Order ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtOrderID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-order"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm" style="display: none">
                            <label>Delivery Order Number</label>
                            <asp:TextBox ID="txtDoNumber" runat="server" class="form-control" placeholder="DO Number ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Delivery Order</label>
                            <asp:TextBox ID="txtDoDate" TextMode="Date" runat="server" class="form-control" placeholder="DO Date ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..."></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <button id="CmdCreate" type="button" class="btn btn-primary" runat="server" onserverclick="CmdCreate_Click">Create</button>
                            <button id="CmdAddDetail" type="button" class="btn btn-primary" runat="server" onclick="return showDetails();">Add Details</button>
                           <%-- <button id="CmdUploadDevice" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-device">Device</button>
                            <button id="CmdUploadGSM" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload-gsm">GSM</button>--%>
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
                            <h3 class="box-title">List Detail Delivery Order</h3>
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
                        <h3 class="box-title">List Delivery Order</h3>
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
                                        <asp:BoundField DataField="DloID" HeaderText="Delivery Order ID" ItemStyle-Wrap="false" SortExpression="DloID"></asp:BoundField>
                                        <asp:BoundField DataField="DloDate" HeaderText="Delivery Date" ItemStyle-Wrap="false" SortExpression="DloDate"></asp:BoundField>
                                        <asp:BoundField DataField="PurID" HeaderText="Purchase Order ID" ItemStyle-Wrap="false" SortExpression="PurID"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false" SortExpression="Remark"></asp:BoundField>
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
                        <h4 class="modal-title">Purchase Order - Procurement</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="req_procurement_header_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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
                        <h6 class="modal-title">Are you sure to delete DO ID :&nbsp;</h6>
                        <label id="LblDOID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtDOIDDelete" runat="server" />
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
                            <iframe id="iframedetails" src="req_procurement_delivery_details.aspx" style="width: 100%; border: none; height: 415px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function postOrderChild(s1, s2, s3, s4, s5) {
            if (s1 != '') {
                document.getElementById('ContentPlaceHolder1_txtOrderID').value = s1;
                $('#modal-order').modal('hide');

                var objfr = document.getElementById('iframedetails').contentWindow;
                var objOrderID = objfr.document.getElementById('txtOrderID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');

                objOrderID.value = s1;
                cmdSearch.click();

            }
        }
        function buttonYes() {
            $("#modal-delete-header").modal('hide');
        }
        function confirmDelete(sText, sPoID, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblDOID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtDOIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtPOIDDelete').value = sPoID;
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
