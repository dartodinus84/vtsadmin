<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_stock.aspx.cs" Inherits="vtsadm.mst_igo_stock" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Stock           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">IGO TRACK</a></li>
            <li class="active">Master Stock</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Device Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Device ID</label>
                            <asp:TextBox ID="txtDeviceID" runat="server" class="form-control" placeholder="Skip for new device ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Group Description</label>
                            <asp:DropDownList ID="CmbDeviceGroupID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbDeviceGroupID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Device Type Description</label>
                            <asp:DropDownList ID="CmbDeviceTypeID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbDeviceTypeID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>No SN</label>
                            <asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Gmt</label>
                            <asp:TextBox ID="txtGMT" runat="server" class="form-control" placeholder="GMT ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date Arrival</label>
                            <asp:TextBox ID="txtDate" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source</label>
                            <asp:DropDownList ID="CmbSourceDevice" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Server Name</label>
                            <asp:DropDownList ID="CmbServer" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vendor Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vendor Name</label>
                            <asp:DropDownList ID="CmbVendorID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CmbVendorID_TextChanged"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <asp:TextBox ID="txtVendorAddress" runat="server" class="form-control" placeholder="Address ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <asp:TextBox ID="txtGSMID" runat="server" class="form-control" placeholder="Skip for new gsm ..." required="required" disabled=""></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>MSIDN</label>
                            <asp:TextBox ID="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date Arrival</label>
                            <asp:TextBox ID="txtDateArival" TextMode="Date" runat="server" class="form-control" placeholder="Date Arrival ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Provider Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Provider Name</label>
                            <asp:DropDownList ID="CmbProviderID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source</label>
                            <asp:DropDownList ID="CmbSourceGSM" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <!--<button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload</button>-->
             
                        </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Stock</h3>
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
                                        <asp:BoundField DataField="autoid" HeaderText="Stock ID" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="gps_model" HeaderText="Type GPS" ItemStyle-Wrap="false" SortExpression="GMT"></asp:BoundField>
                                        <asp:BoundField DataField="gps_sn" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="gsm_no" HeaderText="No GSM" ItemStyle-Wrap="false" SortExpression="GMT"></asp:BoundField>
                                        <asp:BoundField DataField="sGenDate" HeaderText="Generate Date" ItemStyle-Wrap="false" SortExpression="sDateArrival"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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

        <div class="modal fade" id="modal-delete">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <h6 class="modal-title">Are you sure to delete Stock ID :&nbsp;</h6>
                        <label id="LblStockID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtStockIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
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
                            <iframe src="mst_igo_stock_upload.aspx" style="width: 100%; border: none; height: 460px;" scrolling="no"></iframe>
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
    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblStockID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtStockIDDelete').value = sText;
                document.getElementById('ContentPlaceHolder1_txtStatusDelete').value = sStatus;
                $("#modal-delete").modal('show');
            }
        }

        function endRequest(sender, args) {
            //$('#ContentPlaceHolder1_CmbDeviceGroupID').select2();
            //$('#ContentPlaceHolder1_CmbDeviceTypeID').select2();
            //$('#ContentPlaceHolder1_CmbSource').select2();
            //$('#ContentPlaceHolder1_CmbServer').select2();
            //$('#ContentPlaceHolder1_CmbVendorID').select2();

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
