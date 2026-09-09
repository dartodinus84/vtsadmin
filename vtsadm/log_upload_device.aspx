<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="log_upload_device.aspx.cs" Inherits="vtsadm.log_upload_device" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Mutation Device           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Mutation</a></li>
            <li class="active">Device</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Search Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <label>Search By</label>
                            <asp:TextBox ID="txtSearch" runat="server" class="form-control" placeholder="Search by any fields ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date From</label>
                            <asp:TextBox ID="txtDateFrom" TextMode="Date" runat="server" class="form-control" placeholder="Input date from ..."></asp:TextBox>
                        </div>
                        <div class="form-group form-group-sm">
                            <label>Date To</label>
                            <asp:TextBox ID="txtDateTo" TextMode="Date" runat="server" class="form-control" placeholder="Input date to ..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdClear" CssClass="btn btn-primary" runat="server" OnClick="CmdClear_Click" Text="Clear" />
                        <asp:Button ID="CmdSearch" CssClass="btn btn-primary" runat="server" OnClick="CmdSearch_Click" Text="Search" />
                    </div>
                </div>
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">List Mutation Device</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="LogID" HeaderText="Log ID" ItemStyle-Wrap="false" SortExpression="LogID"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceID" HeaderText="Device ID" ItemStyle-Wrap="false" SortExpression="DeviceID"></asp:BoundField>
                                        <asp:BoundField DataField="NoSN" HeaderText="No SN" ItemStyle-Wrap="false" SortExpression="NoSN"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeDesc" HeaderText="Device Type Desc" ItemStyle-Wrap="false" SortExpression="DeviceTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupDesc" HeaderText="Device Group Desc" ItemStyle-Wrap="false" SortExpression="DeviceGroupDesc"></asp:BoundField>
                                        <asp:BoundField DataField="vendorname" HeaderText="Vendor Name" ItemStyle-Wrap="false" SortExpression="VendorName"></asp:BoundField>
                                        <asp:BoundField DataField="warehousename" HeaderText="Warehouse Name" ItemStyle-Wrap="false" SortExpression="WarehouseName"></asp:BoundField>
                                        <asp:BoundField DataField="technicianname" HeaderText="Technician Name" ItemStyle-Wrap="false" SortExpression="TechnicianName"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>
                                       
                                        <asp:BoundField DataField="VendorID" HeaderText="Vendor ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceTypeID" HeaderText="Device Type ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Gmt" HeaderText="Gmt" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="vendoraddress" HeaderText="Vendor Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="DeviceGroupID" HeaderText="Device Group ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseID" HeaderText="Warehouse ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseRemark" HeaderText="Warehouse Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseUsrUpd" HeaderText="Warehouse UsrUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="WarehouseDtmUpd" HeaderText="Warehouse DtmUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianID" HeaderText="Technician ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianRemark" HeaderText="Technician Remark" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianUsrUpd" HeaderText="Technician UsrUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="TechnicianDtmUpd" HeaderText="Technician DtmUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="SourceName" HeaderText="SourceName" ItemStyle-Wrap="false"></asp:BoundField>
                                    </Columns>
                                    <RowStyle ForeColor="#003481" BackColor="White" />
                                    <SelectedRowStyle BackColor="LightBlue" Font-Bold="True" ForeColor="#6298ff" />
                                    <PagerStyle Wrap="true" CssClass="pagination-ys" ForeColor="#003481" HorizontalAlign="Left" BorderColor="White" />
                                    <PagerSettings PageButtonCount="3" FirstPageText="<<" LastPageText=">>" Mode="NumericFirstLast" />
                                    <HeaderStyle Height="20px" CssClass="pagination-ys" Wrap="True" />
                                    <AlternatingRowStyle BackColor="#f9f9f9" BorderColor="White" />
                                </asp:GridView>
                                <div style="margin-top: -18px; margin-bottom: 12px; margin-left: 10px;">
                                    <asp:Label ID="LblPaging" runat="server" Style="color: #003481; font-style: italic; font-size: 13px;"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-logwarehouse">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Log Device Warehouse</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelogwarehouse" src="view_mutation_device_warehouse.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportWarehouse" CssClass="btn btn-primary" runat="server" OnClick="CmdExportWarehouse_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportWarehouseXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportWarehouseXls_Click" Text="Export XLS" />
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg" id="modal-logtechnician">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Log Device Technician</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelogtechnician" src="view_mutation_device_technician.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="CmdExportTechnician" CssClass="btn btn-primary" runat="server" OnClick="CmdExportTechnician_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportTechnicianXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportTechnicianXls_Click" Text="Export XLS" />
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
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
        <div class="modal fade bs-example-modal-lg" id="modal-details">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Mutation Device Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Device Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Device ID</label>
                                            <p id="LblDeviceID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>No SN</label>
                                            <p id="LblNoSN" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Vendor Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Vendor ID</label>
                                            <p id="LblVendorID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Vendor Name</label>
                                            <p id="LblVendorName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Vendor Address</label>
                                            <p id="LblVendorAddress" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Device Type Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Device Type ID</label>
                                            <p id="LblDeviceTypeID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Device Type Desc</label>
                                            <p id="LblDeviceTypeDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Device Group ID</label>
                                            <p id="LblDeviceGroupID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Device Group Desc</label>
                                            <p id="LblDeviceGroupDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Source Name</label>
                                            <p id="LblSourceName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Status</label>
                                            <p id="LblStatus" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Warehouse Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Warehouse ID</label>
                                            <p id="LblWarehouseID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Warehouse Name</label>
                                            <p id="LblWarehouseName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Remark</label>
                                            <p id="LblWarehouseRemark" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>User Update</label>
                                            <p id="LblWarehouseUsrUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Date Update</label>
                                            <p id="LblWarehouseDtmUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Technician Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Technician ID</label>
                                            <p id="LblTechnicianID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Technician Name</label>
                                            <p id="LblTechnicianName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Remark</label>
                                            <p id="LblTechnicianRemark" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>User Update</label>
                                            <p id="LblTechnicianUsrUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Date Update</label>
                                            <p id="LblTechnicianDtmUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>

                            </div>
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

        function CheckNbsp(sbuff) {
            var sOut;
            if (sbuff == "&nbsp;") {
                sOut = "";
            }
            else {
                sOut = sbuff;
            }
            return sOut;
        }

        function postLogWarehouse(sDeviceID) {
            if (sDeviceID != '') {
                $('#modal-logwarehouse').modal('show');
                var objfr = document.getElementById('iframelogwarehouse').contentWindow;
                var objDeviceID = objfr.document.getElementById('txtDeviceID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objDeviceID.value = sDeviceID;
                cmdSearchLog.click();
            }
        }

        function postLogTechnician(sDeviceID) {
            if (sDeviceID != '') {
                $('#modal-logtechnician').modal('show');
                var objfr = document.getElementById('iframelogtechnician').contentWindow;
                var objDeviceID = objfr.document.getElementById('txtDeviceID');
                var cmdSearchLog = objfr.document.getElementById('CmdSearchLog');
                objDeviceID.value = sDeviceID;
                cmdSearchLog.click();
            }
        }

        function postDetails(sDeviceID, sNoSN, sDeviceTypeDesc, sDeviceGroupDesc, sVendorName, sWarehouseName, sTechnicianName, sStatus, objCmd, objLogWarehouse, objLogTechnician, sVendorID, sDeviceTypeID,
            sGmt, sVendorAddress, sDeviceGroupID, sWarehouseID, sWarehouseRemark, sWarehouseUsrUpd, sWarehouseDtmUpd, sTechnicianID, sTechnicianRemark, sTechnicianUsrUpd,
            sTechnicianDtmUpd, sSourceName) {
            if (sDeviceID != '') {
                document.getElementById('ContentPlaceHolder1_LblDeviceID').innerText = sDeviceID;
                document.getElementById('ContentPlaceHolder1_LblNoSN').innerText = CheckNbsp(sNoSN);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);

                document.getElementById('ContentPlaceHolder1_LblWarehouseID').innerText = CheckNbsp(sWarehouseID);
                document.getElementById('ContentPlaceHolder1_LblWarehouseName').innerText = CheckNbsp(sWarehouseName);
                document.getElementById('ContentPlaceHolder1_LblWarehouseRemark').innerText = CheckNbsp(sWarehouseRemark);
                document.getElementById('ContentPlaceHolder1_LblWarehouseUsrUpd').innerText = CheckNbsp(sWarehouseUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblWarehouseDtmUpd').innerText = CheckNbsp(sWarehouseDtmUpd);

                document.getElementById('ContentPlaceHolder1_LblDeviceTypeID').innerText = sDeviceTypeID;
                document.getElementById('ContentPlaceHolder1_LblDeviceTypeDesc').innerText = sDeviceTypeDesc;
                document.getElementById('ContentPlaceHolder1_LblDeviceGroupID').innerText = sDeviceGroupID;
                document.getElementById('ContentPlaceHolder1_LblDeviceGroupDesc').innerText = sDeviceGroupDesc;

                document.getElementById('ContentPlaceHolder1_LblTechnicianID').innerText = CheckNbsp(sTechnicianID);
                document.getElementById('ContentPlaceHolder1_LblTechnicianName').innerText = CheckNbsp(sTechnicianName);
                document.getElementById('ContentPlaceHolder1_LblTechnicianRemark').innerText = CheckNbsp(sTechnicianRemark);
                document.getElementById('ContentPlaceHolder1_LblTechnicianUsrUpd').innerText = CheckNbsp(sTechnicianUsrUpd);
                document.getElementById('ContentPlaceHolder1_LblTechnicianDtmUpd').innerText = CheckNbsp(sTechnicianDtmUpd);

                document.getElementById('ContentPlaceHolder1_LblVendorID').textContent = sVendorID;
                document.getElementById('ContentPlaceHolder1_LblVendorName').textContent = CheckNbsp(sVendorName);
                document.getElementById('ContentPlaceHolder1_LblVendorAddress').textContent = CheckNbsp(sVendorAddress);

                document.getElementById('ContentPlaceHolder1_LblSourceName').textContent = CheckNbsp(sSourceName);
                $('#modal-details').modal('show');
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












