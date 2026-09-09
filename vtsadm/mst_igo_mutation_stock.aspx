<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="mst_igo_mutation_stock.aspx.cs" Inherits="vtsadm.mst_igo_mutation_stock" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Stock           
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">IGO TRACKER</a></li>
            <li class="active">Mutation Stock</li>
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
                        <div class="box-body">
                            <div class="form-group form-group-sm">
                                <label>Device ID</label>
                                <div class="input-group input-group-sm">
                                    <input type="text" id="txtDeviceID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                    <span class="input-group-btn">
                                        <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-device"><i class="fa fa-search"></i></button>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>No SN</label>
                                <asp:TextBox ID="txtNoSN" runat="server" class="form-control" placeholder="No SN ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Vendor Name</label>
                                <asp:TextBox ID="txtVendorName" runat="server" class="form-control" placeholder="Vendor Name ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Device Type Desc</label>
                                <asp:TextBox ID="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Source Name</label>
                                <asp:TextBox ID="txtSourceName" runat="server" class="form-control" placeholder="Source Name ..." required="required"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm">
                                <label>Tdw ID</label>
                                <asp:TextBox ID="txtTdwID" runat="server" class="form-control" placeholder="Tdw ID ..." required="required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Gsm Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Gsm ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-gsm"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>MSIDN</label>
                            <asp:TextBox ID="txtMSIDN" runat="server" class="form-control" placeholder="MSIDN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Provider Name</label>
                            <asp:TextBox ID="txtProviderName" runat="server" class="form-control" placeholder="Provider Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Source Name</label>
                            <asp:TextBox ID="txtGsmSource" runat="server" class="form-control" placeholder="Source Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tgw ID</label>
                            <asp:TextBox ID="txtTgwID" runat="server" class="form-control" placeholder="Tgw ID ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm" style="display: none">
                            <label>Stock ID</label>
                            <asp:TextBox ID="txtStockID" runat="server" class="form-control" placeholder="No SN ..." required="required"></asp:TextBox>
                        </div>
                        <div class="box-footer">
                            <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                            <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
          <%--                  <button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload</button>--%>
                        </div>
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
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>
                                        <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-edit'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Changes"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="GsmID" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="GsmID"></asp:BoundField>
                                        
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
        <div class="modal fade bs-example-modal-lg" id="modal-gsm">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Gsm</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="mst_igo_gsm_search.aspx" style="width: 100%; border: none; height: 350px; overflow: hidden;" scrolling="no"></iframe>
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
                            <iframe src="mst_igo_stock_upload.aspx" style="width: 100%; border: none; height: 460px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-device">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Device</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="mst_igo_device_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <div class="modal fade bs-example-modal-lg" id="modal-technician">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframetechnician" src="gsm_mutation_technician_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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


        function postDeviceChild(sTdwID, sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sSourceName, sWarehouseID, sWarehouseName, sWarehouseAddress, sWarehouseBranchID, sWarehouseBranchName) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = sDeviceID;
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = sVendorName;
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = sDeviceTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtSourceName').value = sSourceName;
                document.getElementById('ContentPlaceHolder1_txtTdwID').value = sTdwID;

                $('#modal-device').modal('hide');

                var objfr2 = document.getElementById('iframetechnician').contentWindow;
                var objBranchID = objfr2.document.getElementById('txtBranchID');
                var cmdSearchTechnician = objfr2.document.getElementById('CmdSearchTechnician');
                objBranchID.value = sWarehouseBranchID;
                cmdSearchTechnician.click();
            }
        }

        function postGsmChild(sTgwID, sGsmID, sMSIDN, sProviderName, sSourceName, sWarehouseID, sWarehouseName, sWarehouseAddress, sWarehouseBranchID, sWarehouseBranchName) {
            if (sGsmID != '') {
                document.getElementById('ContentPlaceHolder1_txtGsmID').value = sGsmID;
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = sMSIDN;
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = sProviderName;
                document.getElementById('ContentPlaceHolder1_txtGsmSource').value = sSourceName;
                document.getElementById('ContentPlaceHolder1_txtTgwID').value = sTgwID;

                $('#modal-gsm').modal('hide');

                var objfr2 = document.getElementById('iframetechnician').contentWindow;
                var objBranchID = objfr2.document.getElementById('txtBranchID');
                var cmdSearchTechnician = objfr2.document.getElementById('CmdSearchTechnician');
                objBranchID.value = sWarehouseBranchID;
                cmdSearchTechnician.click();
            }
        }
        function postTechnicianChild(sTechnicianID, sEmpNo, sName, sBranchName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = sTechnicianID;
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = sEmpNo;
                document.getElementById('ContentPlaceHolder1_txtName').value = sName;
                document.getElementById('ContentPlaceHolder1_txtTechnicianBranchName').value = sBranchName;

                $('#modal-technician').modal('hide');
            }
        }
        function confirmDelete(sText, sStatus) {
            if (sText != '') {
                document.getElementById('ContentPlaceHolder1_LblStockID').innerHTML = sText;
                document.getElementById('ContentPlaceHolder1_txtStockIDDelete').value = sText;
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
