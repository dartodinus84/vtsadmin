<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="view_assign_vehicle.aspx.cs" Inherits="vtsadm.view_assign_vehicle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <h1>Assignment Vehicle           
                <small>View</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">View</a></li>
            <li><a href="#">Assignment</a></li>
            <li class="active">Vehicle</li>
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
                        <h3 class="box-title">List Assignment Vehicle</h3>
                    </div>
                    <div class="box-body">
                        <div class="form-group form-group-sm">
                            <asp:Panel runat="server" ScrollBars="Auto">
                                <asp:GridView ID="GridView2" runat="server" BackColor="WhiteSmoke" AllowSorting="true" Font-Size="Small" CssClass="table table-bordered" CellPadding="2" Width="100%" AutoGenerateColumns="False" Font-Bold="False" CellSpacing="1" EmptyDataText="No items to display" ForeColor="#003481" GridLines="None" BorderWidth="0px" AllowPaging="True" PageSize="5" OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging" OnSorting="GridView2_Sorting">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <Columns>
                                        <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" ItemStyle-Wrap="false" SortExpression="VehicleID"></asp:BoundField>
                                        <asp:BoundField DataField="VehicleDesc" HeaderText="Vehicle Desc" ItemStyle-Wrap="false" SortExpression="VehicleDesc"></asp:BoundField>
                                        <asp:BoundField DataField="PoliceNo" HeaderText="Police No" ItemStyle-Wrap="false" SortExpression="PoliceNo"></asp:BoundField>
                                        <asp:BoundField DataField="AssetNo" HeaderText="Asset No" ItemStyle-Wrap="false" SortExpression="AssetNo"></asp:BoundField>
                                        <asp:BoundField DataField="CustID" HeaderText="Customer ID" ItemStyle-Wrap="false" SortExpression="CustID"></asp:BoundField>
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Wrap="false" SortExpression="FullName"></asp:BoundField>
                                        <asp:BoundField DataField="CustTypeDesc" HeaderText="Customer Type" ItemStyle-Wrap="false" SortExpression="CustTypeDesc"></asp:BoundField>
                                        <asp:BoundField DataField="StatusDesc" HeaderText="Status" ItemStyle-Wrap="false" SortExpression="StatusDesc"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdDetails" runat="server" Text="<i class='fa fa-list-alt'></i>" ToolTip="Details" Enabled="true" CssClass="btn btn-success btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdLog" runat="server" Text="<i class='fa fa-history'></i>" ToolTip="Log Assignment" Enabled="true" CssClass="btn btn-danger btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdUpline" runat="server" Text="<i class='fa fa-angle-up'></i>" ToolTip="Upline" Enabled="true" CssClass="btn btn-primary btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="CmdMaster" runat="server" Text="<i class='fa fa-angle-double-up'></i>" ToolTip="Master" Enabled="true" CssClass="btn btn-warning btn-xs" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TvaID" HeaderText="Tva ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDType" HeaderText="ID Type" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDName" HeaderText="ID Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="IDNumber" HeaderText="ID Number" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingID" HeaderText="Marketing ID" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="MarketingName" HeaderText="Marketing Name" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AssignUsrUpd" HeaderText="Assign UsrUpd" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="AssignDtmUpd" HeaderText="Assign DtmUpd" ItemStyle-Wrap="false"></asp:BoundField>
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
                    <div class="box-footer">
                        <asp:Button ID="CmdExport" CssClass="btn btn-primary" runat="server" OnClick="CmdExport_Click" Text="Export CSV" />
                        <asp:Button ID="CmdExportXls" CssClass="btn btn-primary" runat="server" OnClick="CmdExportXls_Click" Text="Export XLS" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" id="modal-log">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Log Assignment</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframelog" src="view_assign_vehicle_log.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <div class="modal fade bs-example-modal-lg" id="modal-upline">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Upline</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframeupline" src="view_assign_vehicle_upline.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
        <div class="modal fade bs-example-modal-lg" id="modal-master">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">List Master</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group form-group-sm">
                            <iframe id="iframemaster" src="view_assign_vehicle_master.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
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
                        <h4 class="modal-title">Assignment Vehicle Details</h4>
                    </div>
                    <div class="modal-body" style="background-color: #ecf0f5">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Vehicle Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Vehicle ID</label>
                                            <p id="LblVehicleID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Vehicle Desc</label>
                                            <p id="LblVehicleDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Others Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Police No</label>
                                            <p id="LblPoliceNo" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Asset No</label>
                                            <p id="LblAssetNo" runat="server" class="form-control-static"></p>
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
                                        <h3 class="box-title">Customer Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Customer ID</label>
                                            <p id="LblCustID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Full Name</label>
                                            <p id="LblFullName" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Address</label>
                                            <textarea id="txtAddress" runat="server" class="form-control" style="background-color:white;" rows="2" disabled></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Assignment Info</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Assignment User</label>
                                            <p id="LblAssignUsrUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Assignment Date</label>
                                            <p id="LblAssignDtmUpd" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Identity Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Customer Type</label>
                                            <p id="LblCustTypeDesc" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>ID Type</label>
                                            <p id="LblIDType" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>ID Number</label>
                                            <p id="LblIDNumber" runat="server" class="form-control-static"></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="box box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Others Information</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group form-group-sm">
                                            <label>Marketing ID</label>
                                            <p id="LblMarketingID" runat="server" class="form-control-static"></p>
                                        </div>
                                        <div class="form-group form-group-sm">
                                            <label>Marketing Name</label>
                                            <p id="LblMarketingName" runat="server" class="form-control-static"></p>
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
        function postLog(sVehicleID) {
            if (sVehicleID != '') {
                $('#modal-log').modal('show');
                var objfr = document.getElementById('iframelog').contentWindow;
                var objVehicleID = objfr.document.getElementById('txtVehicleID');
                var cmdSearch = objfr.document.getElementById('CmdSearch');
                objVehicleID.value = sVehicleID;
                cmdSearch.click();
            }
        }
        function postUpline(sTvaID, sVehicleID) {
            if (sTvaID != '') {
                $('#modal-upline').modal('show');
                var objfr = document.getElementById('iframeupline').contentWindow;
                var objTvaID = objfr.document.getElementById('txtTvaID');
                var objVehicleID = objfr.document.getElementById('txtVehicleID');
                var cmdSearchUpline = objfr.document.getElementById('CmdSearchUpline');
                objTvaID.value = sTvaID;
                objVehicleID.value = sVehicleID;
                cmdSearchUpline.click();
            }
        }
        function postMaster(sTvaID, sVehicleID) {
            if (sTvaID != '') {
                $('#modal-master').modal('show');
                var objfr = document.getElementById('iframemaster').contentWindow;
                var objTvaID = objfr.document.getElementById('txtTvaID');
                var objVehicleID = objfr.document.getElementById('txtVehicleID');
                var cmdSearchMaster = objfr.document.getElementById('CmdSearchMaster');
                objTvaID.value = sTvaID;
                objVehicleID.value = sVehicleID;
                cmdSearchMaster.click();
            }
        }
        function postDetails(sVehicleID, sVehicleDesc, sPoliceNo, sAssetNo, sCustID, sFullName, sCustTypeDesc, sStatus, objCmdDetails, objCmdLog, objCmdUpline, objCmdMaster,
            sTvaID, sAddress, sIDType, sIDName, sIDNumber, sMarketingID, sMarketingName, sAssignUsrUp, sAssignDtmUpd) {
            if (sVehicleID != '') {
                document.getElementById('ContentPlaceHolder1_LblVehicleID').innerText = CheckNbsp(sVehicleID);
                document.getElementById('ContentPlaceHolder1_LblVehicleDesc').innerText = CheckNbsp(sVehicleDesc);
                document.getElementById('ContentPlaceHolder1_LblPoliceNo').innerText = CheckNbsp(sPoliceNo);
                document.getElementById('ContentPlaceHolder1_LblAssetNo').innerText = CheckNbsp(sAssetNo);
                document.getElementById('ContentPlaceHolder1_LblStatus').innerText = CheckNbsp(sStatus);

                document.getElementById('ContentPlaceHolder1_LblCustID').innerText = CheckNbsp(sCustID);
                document.getElementById('ContentPlaceHolder1_LblFullName').innerText = CheckNbsp(sFullName);
                document.getElementById('ContentPlaceHolder1_txtAddress').textContent = CheckNbsp(sAddress);

                document.getElementById('ContentPlaceHolder1_LblAssignUsrUpd').innerText = CheckNbsp(sAssignUsrUp);
                document.getElementById('ContentPlaceHolder1_LblAssignDtmUpd').innerText = CheckNbsp(sAssignDtmUpd);

                document.getElementById('ContentPlaceHolder1_LblCustTypeDesc').innerText = CheckNbsp(sCustTypeDesc);
                document.getElementById('ContentPlaceHolder1_LblIDType').innerText = CheckNbsp(sIDType);
                document.getElementById('ContentPlaceHolder1_LblIDNumber').innerText = CheckNbsp(sIDNumber);

                document.getElementById('ContentPlaceHolder1_LblMarketingID').innerText = CheckNbsp(sMarketingID);
                document.getElementById('ContentPlaceHolder1_LblMarketingName').innerText = CheckNbsp(sMarketingName);
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
