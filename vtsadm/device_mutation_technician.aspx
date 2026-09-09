<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="device_mutation_technician.aspx.cs" Inherits="vtsadm.device_mutation_technician" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Device - Technician Mutation       
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Mutation</a></li>
            <li><a href="#">Device</a></li>
            <li class="active">Device Technician Mutation</li>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Warehouse Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Warehouse ID</label>
                            <asp:TextBox ID="txtWarehouseID" runat="server" class="form-control" placeholder="Warehouse ID ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Name</label>
                            <asp:TextBox ID="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Address</label>
                            <asp:TextBox ID="txtWarehouseAddress" runat="server" class="form-control" placeholder="Warehouse Address ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtWarehouseBranchName" runat="server" class="form-control" placeholder="Warehouse Branch ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Technician Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Technician ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtTechnicianID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button3" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-technician"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Employee No</label>
                            <asp:TextBox ID="txtEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Name</label>
                            <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Technician Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtTechnicianBranchName" runat="server" class="form-control" placeholder="Technician Branch ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="div_mutation" runat="server" class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Technician Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Technician ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtNewTechnicianID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-techniciannew"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Employee No</label>
                            <asp:TextBox ID="txtNewEmployeeNo" runat="server" class="form-control" placeholder="Employee No ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Name</label>
                            <asp:TextBox ID="txtNewName" runat="server" class="form-control" placeholder="Technician Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtNewTechnicianBranchName" runat="server" class="form-control" placeholder="Technician Branch ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Old Tdt ID</label>
                            <asp:TextBox ID="txtOldTdtID" runat="server" class="form-control" placeholder="Old Tdt ID ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Mutation Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" runat="server" class="form-control" placeholder="Remark ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <button id="CmdUploadPicture" type="button" class="btn btn-primary" onclick="$('#modal-picture').modal('show');"><i class="fa fa-upload"></i>&nbsp;Image</button>
                        <button id="CmdClear" type="button" class="btn btn-primary" runat="server" onserverclick="CmdClear_ServerClick">Clear</button>
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClientClick="$('#modal-submit').modal('show');return false;" Text="Submit" />
                        <button id="CmdUpload" type="button" class="btn btn-primary" runat="server" data-toggle="modal" data-target="#modal-upload">Upload</button>
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Device - Technician Mutation</h3>
                        <div class="box-tools" style="width: 150px;">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtSearch" runat="server" class="form-control pull-right" placeholder="Search by No SN ..."></asp:TextBox>
                                <span class="input-group-btn">
                                    <button id="CmdSearch" runat="server" type="button" class="btn btn-primary" data-widget="collapse" onserverclick="CmdSearch_ServerClick">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-Wrap="false" SortExpression="VendorName"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseName" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="Status"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-warning btn-xs" Text="<i class='fa fa-refresh'></i>" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="White" CommandName="Mutation"></asp:ButtonField>
                                        <%--<asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDelete" runat="server" Text="<i class='fa fa-close'></i>" ToolTip="Delete" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="TdtID" HeaderText="Tdt ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TdwID" HeaderText="Tdw ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseAddress" HeaderText="Warehouse Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseBranch" HeaderText="Warehouse Branch" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="EmployeeNo" HeaderText="Employee No" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianBranchID" HeaderText="Branch ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianBranch" HeaderText="Technician Branch" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SourceName" HeaderText="Source Name" ItemStyle-Wrap="false"></asp:BoundField>

                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="false" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;"><asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
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
        <div class="modal fade bs-example-modal-lg" id="modal-picture">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Picture</h4>
                    </div>
                    <div class="modal-body"
                        <div class="form-group form-group-sm" style="overflow: hidden;">
                            <iframe src="device_mutation_upload.aspx" style="width: 100%; border: none; height: 370px; overflow: hidden;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
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
                        <h6 class="modal-title">Are you sure to delete Tdt ID :&nbsp;</h6>
                        <label id="LblTdtID" runat="server"></label>
                        &nbsp;?
                        <input type="hidden" id="txtTdtIDDelete" runat="server" />
                        <input type="hidden" id="txtTdwIDDelete" runat="server" />
                        <input type="hidden" id="txtDeviceIDDelete" runat="server" />
                        <input type="hidden" id="txtWareIDDelete" runat="server" />
                        <input type="hidden" id="txtTechIDDelete" runat="server" />
                        <input type="hidden" id="txtStatusDelete" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" runat="server" onclick="$('#modal-delete').modal('hide');" onserverclick="CmdYesDelete_ServerClick" id="CmdYesDelete">Yes</button>
                        <button type="button" class="btn btn-primary" onclick="$('#modal-delete').modal('hide');">No</button>
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
                            <iframe src="device_mutation_technician_device_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                            <iframe id="iframetechnician" src="device_mutation_technician_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-techniciannew">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframetechniciannew" src="device_mutation_technician_new_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                        <h4 class="modal-title">Upload [<a href="Export/device_mutation_upload_final.csv">Download Template</a>]</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="device_mutation_technician_upload.aspx" style="width: 100%; border: none; height: 520px;" scrolling="auto"></iframe>
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

        function postDeviceChild(sTdwID, sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sSourceName, sWarehouseID, sWarehouseName, sWarehouseAddress, sWarehouseBranchID, sWarehouseBranchName) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = sDeviceID;
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = sNoSN;
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = sVendorName;
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = sDeviceTypeDesc;
                document.getElementById('ContentPlaceHolder1_txtSourceName').value = sSourceName;

                document.getElementById('ContentPlaceHolder1_txtTdwID').value = sTdwID;
                document.getElementById('ContentPlaceHolder1_txtWarehouseID').value = sWarehouseID;
                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = sWarehouseName;
                document.getElementById('ContentPlaceHolder1_txtWarehouseAddress').value = sWarehouseAddress;
                document.getElementById('ContentPlaceHolder1_txtWarehouseBranchName').value = sWarehouseBranchName;
                $('#modal-device').modal('hide');

                var objfr2 = document.getElementById('iframetechnician').contentWindow;
                var objBranchID = objfr2.document.getElementById('txtBranchID');
                var cmdSearchTechnician = objfr2.document.getElementById('CmdSearchTechnician');
                objBranchID.value = sWarehouseBranchID;
                cmdSearchTechnician.click();
                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postTechnicianChild(sTechnicianID, sEmpNo, sName, sBranchName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = sTechnicianID;
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = sEmpNo;
                document.getElementById('ContentPlaceHolder1_txtName').value = sName;
                document.getElementById('ContentPlaceHolder1_txtTechnicianBranchName').value = sBranchName;

                $('#modal-technician').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postNewTechnicianChild(sTechnicianID, sEmpNo, sName, sBranchName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtNewTechnicianID').value = sTechnicianID;
                document.getElementById('ContentPlaceHolder1_txtNewEmployeeNo').value = sEmpNo;
                document.getElementById('ContentPlaceHolder1_txtNewName').value = sName;
                document.getElementById('ContentPlaceHolder1_txtNewTechnicianBranchName').value = sBranchName;

                $('#modal-techniciannew').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }
        function confirmDelete(sTdtID, sTdwID, sDeviceID, sWarehouseID, sTechnicianID, sStatus) {
            if (sTdtID != '') {
                document.getElementById('ContentPlaceHolder1_LblTdtID').innerHTML = sTdtID;
                document.getElementById('ContentPlaceHolder1_txtTdtIDDelete').value = sTdtID;
                document.getElementById('ContentPlaceHolder1_txtTdwIDDelete').value = sTdwID;
                document.getElementById('ContentPlaceHolder1_txtDeviceIDDelete').value = sDeviceID;
                document.getElementById('ContentPlaceHolder1_txtWareIDDelete').value = sWarehouseID;
                document.getElementById('ContentPlaceHolder1_txtTechIDDelete').value = sTechnicianID;
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
