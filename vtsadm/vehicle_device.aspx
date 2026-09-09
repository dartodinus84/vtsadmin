<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="vehicle_device.aspx.cs" Inherits="vtsadm.vehicle_device" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>New Installation
                <small>Input</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Installation</a></li>
            <li class="active">New Installation</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Vehicle Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Vehicle ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtVehicleID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button2" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-vehicle"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Vehicle Description</label>
                            <asp:TextBox ID="txtVehicleDesc" runat="server" class="form-control" placeholder="Vehicle Description ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Police No</label>
                            <asp:TextBox ID="txtPoliceNo" runat="server" class="form-control" placeholder="Police No ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Asset No</label>
                            <asp:TextBox ID="txtAssetNo" runat="server" class="form-control" placeholder="Asset No ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tva ID</label>
                            <asp:TextBox ID="txtTvaID" runat="server" class="form-control" placeholder="Tva ID ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Branch Name</label>
                            <asp:TextBox ID="txtCustBranchName" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" class="form-control" placeholder="Customer Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Upline Name</label>
                            <asp:TextBox ID="txtUplineName" runat="server" class="form-control" placeholder="Upline Name ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Master Name</label>
                            <asp:TextBox ID="txtMasterName" runat="server" class="form-control" placeholder="Master Name ..."></asp:TextBox>
                        </div>
                    </div>
                </div>
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
                            <asp:TextBox ID="txtTechBranchName" runat="server" class="form-control" placeholder="Branch Name ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
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
                                    <button id="Button1" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-device"><i class="fa fa-search"></i></button>
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
                            <label>Device Type Description</label>
                            <asp:TextBox ID="txtDeviceTypeDesc" runat="server" class="form-control" placeholder="Device Type Desc ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Warehouse Name</label>
                            <asp:TextBox ID="txtWarehouseName" runat="server" class="form-control" placeholder="Warehouse Name ..." required="required"></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Tdm ID</label>
                            <asp:TextBox ID="txtTdmID" runat="server" class="form-control" placeholder="Tdm ID ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">GSM Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>GSM ID</label>
                            <div class="input-group input-group-sm">
                                <input type="text" id="txtGsmID" runat="server" class="form-control" placeholder="Please select ..." readonly="readonly" required="required" />
                                <span class="input-group-btn">
                                    <button id="Button4" runat="server" type="button" class="btn btn-block btn-primary btn-xs" data-toggle="modal" data-target="#modal-gsm"><i class="fa fa-search"></i></button>
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
                            <label>Tgm ID</label>
                            <asp:TextBox ID="txtTgmID" runat="server" class="form-control" placeholder="Tgm ID ..." required="required"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_ServerClick" Text="Clear" />
                        <asp:Button ID="CmdSubmit" CssClass="btn btn-primary" runat="server" OnClick="CmdSubmit_ServerClick" Text="Submit" />
                    </div>
                </div>

                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List New Installation</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView1" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowCommand="GridView2_RowCommand" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="TvdID" HeaderText="Tvd ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="CustFullName" HeaderText="Customer Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="UplineFullName" HeaderText="Upline Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="MasterFullName" HeaderText="Master Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianName" HeaderText="Technician Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TdmID" HeaderText="Tva ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="TgmID" HeaderText="Tds ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="GsmID" HeaderText="Gsm ID" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="MSIDN" HeaderText="MSIDN" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:ButtonField ControlStyle-CssClass="btn btn-block btn-primary btn-xs" Text="Delete" ButtonType="Image" CommandName="Delete"></asp:ButtonField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-vehicle">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Vehicle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="vehicle_device_vehicle_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <!-- /.modal -->

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
                            <iframe src="vehicle_device_technician_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <!-- /.modal -->

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
                            <iframe src="vehicle_device_device_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <!-- /.modal -->

        <div class="modal fade bs-example-modal-lg" id="modal-gsm">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">GSM</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe src="vehicle_device_gsm_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <!-- /.modal -->
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
        <!-- /.modal -->


    </section>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);

        function checkNbsp(sBuff) {
            var sOut
            sOut = sBuff;
            if (sBuff == '&nbsp;') {
                sOut = '';
            }
            return sOut;
        }

        function postVehicleChild(sTvaID, sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sCustBranchName, sCustName, sUplineName, sMasterName) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_txtTvaID').value = checkNbsp(sTvaID);
                document.getElementById('ContentPlaceHolder1_txtVehicleID').value = checkNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_txtVehicleDesc').value = checkNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_txtPoliceNo').value = checkNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_txtAssetNo').value = checkNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_txtCustBranchName').value = checkNbsp(sCustBranchName);
                document.getElementById('ContentPlaceHolder1_txtCustomerName').value = checkNbsp(sCustName);
                document.getElementById('ContentPlaceHolder1_txtUplineName').value = checkNbsp(sUplineName);
                document.getElementById('ContentPlaceHolder1_txtMasterName').value = checkNbsp(sMasterName);

                $('#modal-vehicle').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postDeviceChild(sTdmID, sDeviceID, sNoSN, sVendorName, sDeviceTypeDesc, sWarehouseName) {
            if (sTdmID != '') {
                document.getElementById('ContentPlaceHolder1_txtTdmID').value = checkNbsp(sTdmID);
                document.getElementById('ContentPlaceHolder1_txtDeviceID').value = checkNbsp(sDeviceID);
                document.getElementById('ContentPlaceHolder1_txtNoSN').value = checkNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_txtVendorName').value = checkNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_txtDeviceTypeDesc').value = checkNbsp(sDeviceTypeDesc);
                document.getElementById('ContentPlaceHolder1_txtWarehouseName').value = checkNbsp(sWarehouseName);

                $('#modal-device').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postTechnicianChild(sTechnicianID, sEmpNo, sName, sBranchName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtTechnicianID').value = checkNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_txtEmployeeNo').value = checkNbsp(sEmpNo);
                document.getElementById('ContentPlaceHolder1_txtName').value = checkNbsp(sName);
                document.getElementById('ContentPlaceHolder1_txtTechBranchName').value = checkNbsp(sBranchName);

                $('#modal-technician').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function postGsmChild(sTgmID, sGsmID, sMSIDN, sProviderName) {
            if (sTechnicianID != '') {
                document.getElementById('ContentPlaceHolder1_txtTgmID').value = checkNbsp(sTgmID);
                document.getElementById('ContentPlaceHolder1_txtGsmID').value = checkNbsp(sGsmID);
                document.getElementById('ContentPlaceHolder1_txtMSIDN').value = checkNbsp(sMSIDN);
                document.getElementById('ContentPlaceHolder1_txtProviderName').value = checkNbsp(sProviderName);

                $('#modal-gsm').modal('hide');

                //var objfr1 = document.getElementById('iframe1').contentWindow;
                //var objfr2 = document.getElementById('ContentPlaceHolder1_CmdLoadDevice');
                //objfr2.click();
            }
        }

        function endRequest(sender, args) {
            var isExists = document.getElementById('ContentPlaceHolder1_div_comment').innerHTML;
            if (isExists != '') {
                //window.setTimeout(function () { $('.alert').fadeTo(500, 0).slideUp(500, function () { $(this).remove(); }); }, 2000)
                $('#modal-messagebox').modal('show');
            }
        }
        endRequest();
    </script>
</asp:Content>
